Imports System.Net
Imports Windows.Storage
Imports HtmlAgilityPack
Imports Windows.ApplicationModel.Resources

Public Class Connector

    ' String resources
    Private res As ResourceLoader = ResourceLoader.GetForCurrentView()

    Public Enum ConnectProgress
        RedirectToAuthPage = 0
        GatheringAuthData = 1
        SendAuthData = 2
        ExtractSessionData = 3
        AuthOK = 4
        AuthFailed = 5
        AuthError = 6
        GetSessionData = 7
        SendDeauthData = 8
        DeauthOK = 9
        DeauthFailed = 10
        DeauthError = 11
    End Enum

    Public Event ConnectProgressChanged(ByVal Progress As ConnectProgress, ByVal AdditionalString As String)

    Public Sub Connect()
        Try
            ' Redirect to auth page
            Dim reqRdr As New WebClient
            Dim strRdrReturn As String = reqRdr.DownloadString("http://cdn.ralf.ren/res/portal.html")
            Dim strRdrUrl As String = strRdrReturn.Replace("<script>top.self.location.href='", "").Replace("'</script>", "").Trim

            ' Progress: Redirect to auth page
            Logging($"{res.GetString("L_GetAuthPage")}: {strRdrUrl}")
            RaiseEvent ConnectProgressChanged(ConnectProgress.RedirectToAuthPage, "")

            ' Get auth page html
            Dim reqAuthPage As HttpWebRequest = HttpWebRequest.Create(strRdrUrl)
            reqAuthPage.AllowAutoRedirect = True
            Dim resAuthPage As HttpWebResponse = reqAuthPage.GetResponse
            Dim htmAuthPage As New HtmlDocument
            htmAuthPage.LoadHtml(New IO.StreamReader(resAuthPage.GetResponseStream).ReadToEnd)

            ' Progress: Gathering auth data
            RaiseEvent ConnectProgressChanged(ConnectProgress.GatheringAuthData, "")
            strHost = strRdrUrl.Replace("http://", "").Split("/")(0)
            Logging($"{res.GetString("L_GetAuthHost")}: {strHost}")

            ' Extract information from html
            data_mac = htmAuthPage.DocumentNode.SelectSingleNode("//input[@id='mac']").Attributes("value").Value
            Logging($"{res.GetString("L_GetAuthInfo")} - mac: {data_mac}")
            data_wlancname = htmAuthPage.DocumentNode.SelectSingleNode("//input[@id='wlanacname']").Attributes("value").Value
            Logging($"{res.GetString("L_GetAuthInfo")} - wlancname: {data_wlancname}")
            data_url = htmAuthPage.DocumentNode.SelectSingleNode("//input[@id='url']").Attributes("value").Value
            Logging($"{res.GetString("L_GetAuthInfo")} - url: {data_url}")
            data_nasip = htmAuthPage.DocumentNode.SelectSingleNode("//input[@id='nasip']").Attributes("value").Value
            Logging($"{res.GetString("L_GetAuthInfo")} - nasip: {data_nasip}")
            data_wlanuserip = htmAuthPage.DocumentNode.SelectSingleNode("//input[@id='wlanuserip']").Attributes("value").Value
            Logging($"{res.GetString("L_GetAuthInfo")} - wlanuserip: {data_wlanuserip}")

            ' Username & password
            Logging($"{res.GetString("L_GetAuthInfo")} - username: {data_username}")
            Logging($"{res.GetString("L_GetAuthInfo")} - pwd: {data_pwd}")

            ' Build auth form data
            Dim strParam As String = $"qrCodeId=请输入编号&username={data_username}&pwd={data_pwd}&validCode=验证码&validCodeFlag=false&ssid=&mac={data_mac}&t=wireless-v2&wlanacname={data_wlancname}&url={data_url}&nasip={data_nasip}&wlanuserip={data_wlanuserip}"
            Dim bytParam() As Byte = Text.Encoding.UTF8.GetBytes(strParam)
            Dim reqAuth As HttpWebRequest = HttpWebRequest.Create($"http://{strHost}/zportal/login/do")
            reqAuth.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36"
            reqAuth.AllowAutoRedirect = True
            reqAuth.Method = "POST"
            reqAuth.ContentType = "application/x-www-form-urlencoded"
            Using swAuth As IO.Stream = reqAuth.GetRequestStream
                swAuth.Write(bytParam, 0, bytParam.Length)
            End Using

            ' Send auth form data & get response
            Logging($"{res.GetString("L_SendAuthData")}")
            RaiseEvent ConnectProgressChanged(ConnectProgress.SendAuthData, "")
            Dim resResult As HttpWebResponse = reqAuth.GetResponse
            Dim strResult As String = New IO.StreamReader(resResult.GetResponseStream).ReadToEnd
            Logging($"{res.GetString("L_GotResponse")}: {strResult}")

            If strResult.Contains("""result"":""success""") Then
                ' Auth success
                Logging($"{res.GetString("L_AuthSuccess")}")

                ' Get JSessionID
                data_jsessionid = resResult.Headers.Get("Set-Cookie").Split(";")(0).Split("=")(1)
                Logging($"{res.GetString("L_SessionID")}: {data_jsessionid}")

                ' Create success page cookie container
                Dim cooks As New CookieContainer()
                cooks.Add(New Cookie("JSESSIONID", data_jsessionid, "/zportal/", strHost.Split(":")(0)))
                Dim reqSuccess As HttpWebRequest = HttpWebRequest.Create($"http://{strHost}/zportal/goToAuthResult")
                reqSuccess.Method = "GET"
                reqSuccess.CookieContainer = cooks

                ' Get success page
                Dim resSuccess As HttpWebResponse = reqSuccess.GetResponse
                Dim htmSuccess As New HtmlDocument
                htmSuccess.LoadHtml(New IO.StreamReader(resSuccess.GetResponseStream).ReadToEnd)

                ' Extract session data
                RaiseEvent ConnectProgressChanged(ConnectProgress.ExtractSessionData, "")
                data_usermac = htmSuccess.DocumentNode.SelectSingleNode("//input[@id='userMac']").Attributes("value").Value
                Logging($"{res.GetString("L_ExtractSessionData")} - userMac: {data_usermac}")
                data_userip = htmSuccess.DocumentNode.SelectSingleNode("//input[@name='userIp']").Attributes("value").Value
                Logging($"{res.GetString("L_ExtractSessionData")} - userIp: {data_userip}")
                data_deviceip = htmSuccess.DocumentNode.SelectSingleNode("//input[@name='deviceIp']").Attributes("value").Value
                Logging($"{res.GetString("L_ExtractSessionData")} - deviceIp: {data_deviceip}")

                ' Done
                RaiseEvent ConnectProgressChanged(ConnectProgress.AuthOK, "")

            Else
                Logging($"{res.GetString("L_AuthFailed")}")
                RaiseEvent ConnectProgressChanged(ConnectProgress.AuthFailed, "")
            End If

        Catch ex As Exception
            Logging($"{res.GetString("L_ErrorWhenAuth")}: {ex.Message}")
            RaiseEvent ConnectProgressChanged(ConnectProgress.AuthError, ex.Message)
        End Try
    End Sub

    Public Sub Disconnect()
        Try

            ' Get session data
            RaiseEvent ConnectProgressChanged(ConnectProgress.GetSessionData, "")
            Logging($"{res.GetString("L_SessionInfo")} - userName: {data_username}, {res.GetString("L_Loaded")}")
            Logging($"{res.GetString("L_SessionInfo")} - userIp: {data_userip}, {res.GetString("L_Loaded")}")
            Logging($"{res.GetString("L_SessionInfo")} - deviceIp: {data_deviceip}, {res.GetString("L_Loaded")}")
            Logging($"{res.GetString("L_SessionInfo")} - userMac: {data_usermac}, {res.GetString("L_Loaded")}")
            Logging($"{res.GetString("L_SessionID")}: {data_jsessionid}, {res.GetString("L_Loaded")}")
            Logging($"{res.GetString("L_AuthHost")}: {strHost}, {res.GetString("L_Loaded")}")

            ' Build de-auth form data
            Dim strParam As String = $"userName={data_username}&service.id=&autoLoginFlag=false&userIp={data_userip}&deviceIp={data_deviceip}&userMac={data_usermac}&operationType=&isMacFastAuth=false"
            Dim bytParam() As Byte = Text.Encoding.UTF8.GetBytes(strParam)
            Dim cooks As New CookieContainer()
            cooks.Add(New Cookie("JSESSIONID", data_jsessionid, "/zportal/", strHost.Split(":")(0)))

            Dim reqDeauth As HttpWebRequest = HttpWebRequest.Create($"http://{strHost}/zportal/logout")
            reqDeauth.CookieContainer = cooks
            reqDeauth.Method = "POST"
            reqDeauth.ContentType = "application/x-www-form-urlencoded"
            Using swDeAuth As IO.Stream = reqDeauth.GetRequestStream
                swDeAuth.Write(bytParam, 0, bytParam.Length)
            End Using

            ' Send de-auth data
            Logging($"{res.GetString("L_SendDeauthData")}")
            RaiseEvent ConnectProgressChanged(ConnectProgress.SendDeauthData, "")
            Dim resDeauth As HttpWebResponse = reqDeauth.GetResponse
            Dim htmSuccess As String = New IO.StreamReader(resDeauth.GetResponseStream).ReadToEnd

            If htmSuccess.Contains("已下线") Then
                Logging($"{res.GetString("L_DeauthOK")}")
                RaiseEvent ConnectProgressChanged(ConnectProgress.DeauthOK, "")
            Else
                Logging($"{res.GetString("L_DeauthFailed")}")
                RaiseEvent ConnectProgressChanged(ConnectProgress.DeauthFailed, "")
            End If

        Catch ex As Exception
            Logging($"{res.GetString("L_ErrorWhenDeauth")}")
            RaiseEvent ConnectProgressChanged(ConnectProgress.DeauthError, ex.Message)
        End Try
    End Sub

End Class
