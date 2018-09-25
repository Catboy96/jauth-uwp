Imports Windows.Storage

Module Util

    Public Sub Logging(ByVal Prompt As String)
        Globals.log_content = Globals.log_content & $"[{Date.Now}] {Prompt}" & vbCrLf
    End Sub

    Public Async Function CheckInternetAvailibility() As Task(Of Boolean)
        Dim req As New Net.WebClient
        Dim ret As String = ""
        Try
            ret = Await req.DownloadStringTaskAsync("http://cdn.ralf.ren/res/portal.html")
            If Not ret = "Success" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub SaveSettings()

        Dim settings As ApplicationDataContainer = ApplicationData.Current.LocalSettings()

        If settings.Values.Keys.Contains(NameOf(conn_by_jauth)) Then
            settings.Values(NameOf(conn_by_jauth)) = conn_by_jauth
        Else
            settings.Values.Add(NameOf(conn_by_jauth), conn_by_jauth)
        End If

        If settings.Values.Keys.Contains(NameOf(strHost)) Then
            settings.Values(NameOf(strHost)) = strHost
        Else
            settings.Values.Add(NameOf(strHost), strHost)
        End If

        If settings.Values.Keys.Contains(NameOf(data_username)) Then
            settings.Values(NameOf(data_username)) = data_username
        Else
            settings.Values.Add(NameOf(data_username), data_username)
        End If

        If settings.Values.Keys.Contains(NameOf(data_pwd)) Then
            settings.Values(NameOf(data_pwd)) = data_pwd
        Else
            settings.Values.Add(NameOf(data_pwd), data_pwd)
        End If

        If settings.Values.Keys.Contains(NameOf(data_mac)) Then
            settings.Values(NameOf(data_mac)) = data_mac
        Else
            settings.Values.Add(NameOf(data_mac), data_mac)
        End If

        If settings.Values.Keys.Contains(NameOf(data_wlancname)) Then
            settings.Values(NameOf(data_wlancname)) = data_wlancname
        Else
            settings.Values.Add(NameOf(data_wlancname), data_wlancname)
        End If

        If settings.Values.Keys.Contains(NameOf(data_url)) Then
            settings.Values(NameOf(data_url)) = data_url
        Else
            settings.Values.Add(NameOf(data_url), data_url)
        End If

        If settings.Values.Keys.Contains(NameOf(data_nasip)) Then
            settings.Values(NameOf(data_nasip)) = data_nasip
        Else
            settings.Values.Add(NameOf(data_nasip), data_nasip)
        End If

        If settings.Values.Keys.Contains(NameOf(data_wlanuserip)) Then
            settings.Values(NameOf(data_wlanuserip)) = data_wlanuserip
        Else
            settings.Values.Add(NameOf(data_wlanuserip), data_wlanuserip)
        End If

        If settings.Values.Keys.Contains(NameOf(data_jsessionid)) Then
            settings.Values(NameOf(data_jsessionid)) = data_jsessionid
        Else
            settings.Values.Add(NameOf(data_jsessionid), data_jsessionid)
        End If

        If settings.Values.Keys.Contains(NameOf(data_usermac)) Then
            settings.Values(NameOf(data_usermac)) = data_usermac
        Else
            settings.Values.Add(NameOf(data_usermac), data_usermac)
        End If

        If settings.Values.Keys.Contains(NameOf(data_userip)) Then
            settings.Values(NameOf(data_userip)) = data_userip
        Else
            settings.Values.Add(NameOf(data_userip), data_userip)
        End If

        If settings.Values.Keys.Contains(NameOf(data_deviceip)) Then
            settings.Values(NameOf(data_deviceip)) = data_deviceip
        Else
            settings.Values.Add(NameOf(data_deviceip), data_deviceip)
        End If

        If settings.Values.Keys.Contains(NameOf(plan_name)) Then
            settings.Values(NameOf(plan_name)) = plan_name
        Else
            settings.Values.Add(NameOf(plan_name), plan_name)
        End If

        If settings.Values.Keys.Contains("plan_remaining_days") Then
            settings.Values("plan_remaining_days") = plan_remaining.Days
        Else
            settings.Values.Add("plan_remaining_days", plan_remaining.Days)
        End If

        If settings.Values.Keys.Contains("plan_remaining_hours") Then
            settings.Values("plan_remaining_hours") = plan_remaining.Hours
        Else
            settings.Values.Add("plan_remaining_hours", plan_remaining.Hours)
        End If

        If settings.Values.Keys.Contains("plan_remaining_minutes") Then
            settings.Values("plan_remaining_minutes") = plan_remaining.Minutes
        Else
            settings.Values.Add("plan_remaining_minutes", plan_remaining.Minutes)
        End If

        If settings.Values.Keys.Contains("plan_remaining_seconds") Then
            settings.Values("plan_remaining_seconds") = plan_remaining.Seconds
        Else
            settings.Values.Add("plan_remaining_seconds", plan_remaining.Seconds)
        End If

    End Sub

    Public Sub LoadSettings()
        Dim settings As ApplicationDataContainer = ApplicationData.Current.LocalSettings()
        conn_by_jauth = settings.Values(NameOf(conn_by_jauth))
        strHost = settings.Values(NameOf(strHost))
        data_username = settings.Values(NameOf(data_username))
        data_pwd = settings.Values(NameOf(data_pwd))
        data_mac = settings.Values(NameOf(data_mac))
        data_wlancname = settings.Values(NameOf(data_wlancname))
        data_url = settings.Values(NameOf(data_url))
        data_nasip = settings.Values(NameOf(data_nasip))
        data_wlanuserip = settings.Values(NameOf(data_wlanuserip))
        data_jsessionid = settings.Values(NameOf(data_jsessionid))
        data_usermac = settings.Values(NameOf(data_usermac))
        data_userip = settings.Values(NameOf(data_userip))
        data_deviceip = settings.Values(NameOf(data_deviceip))

        plan_name = settings.Values(NameOf(plan_name))
        plan_remaining.Days = settings.Values("plan_remaining_days")
        plan_remaining.Hours = settings.Values("plan_remaining_hours")
        plan_remaining.Minutes = settings.Values("plan_remaining_minutes")
        plan_remaining.Seconds = settings.Values("plan_remaining_seconds")
    End Sub

End Module
