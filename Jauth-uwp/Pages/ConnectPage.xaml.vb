Imports System.Net
Imports System.Threading
Imports Windows.ApplicationModel.Resources

Public NotInheritable Class ConnectPage : Inherits Page

    ' String resources
    Dim res As ResourceLoader = ResourceLoader.GetForCurrentView()

    ' Connect button role
    Private Enum ConnectButtonRoles
        Connect = 1
        Disconnect = 0
    End Enum
    Private ConnectButtonRole As ConnectButtonRoles

    ' Jauth core connector
    Private WithEvents conn As New Connector

    ' Apply locale strings here
    Private Sub Page_Loading(sender As FrameworkElement, args As Object)

        tbTitle.Text = res.GetString("UI_Checking")
        btnConnect.Content = res.GetString("UI_Wait")

        proConnect.IsIndeterminate = True
        proConnect.ShowError = False
        proConnect.ShowPaused = False
        proConnect.Visibility = Visibility.Visible

        btnConnect.IsEnabled = False

    End Sub

    ' Check the Internet connection
    Private Async Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        tbTitle.Text = res.GetString("UI_Checking")

        btnConnect.Content = res.GetString("UI_Wait")
        btnConnect.IsEnabled = False

        proConnect.Visibility = Visibility.Collapsed


        If Await Util.CheckInternetAvailibility() = True Then
            tbTitle.Text = res.GetString("UI_Connected")

            btnConnect.Content = res.GetString("UI_Disconnect")
            btnConnect.IsEnabled = True
            ConnectButtonRole = ConnectButtonRoles.Disconnect

            proConnect.Visibility = Visibility.Collapsed

            Globals.inet_status = True
        Else
            tbTitle.Text = res.GetString("UI_Disconnected")

            btnConnect.Content = res.GetString("UI_Connect")
            btnConnect.IsEnabled = True
            ConnectButtonRole = ConnectButtonRoles.Connect

            proConnect.Visibility = Visibility.Collapsed

            Globals.inet_status = False
        End If

    End Sub

    ' Connect button tapped
    Private Sub btnConnect_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Select Case ConnectButtonRole
            Case ConnectButtonRoles.Connect

                ' No username or password specified
                If data_username = "" Or data_pwd = "" Then
                    tbTitle.Text = res.GetString("UI_NoAccountSpecified")
                    Exit Sub
                End If

                ' Make progress bar visible
                proConnect.Visibility = Visibility.Visible

                ' Disable the button
                btnConnect.IsEnabled = False

                ' Start authentication
                Dim thConnect As New Thread(AddressOf conn.Connect)
                thConnect.Start()

            Case ConnectButtonRoles.Disconnect

                ' No username or password specified
                If data_username = "" Or data_pwd = "" Then
                    tbTitle.Text = res.GetString("UI_NoAccountSpecified")
                    Exit Sub
                End If

                ' Make progress bar visible
                proConnect.Visibility = Visibility.Visible

                ' Disable the button
                btnConnect.IsEnabled = False

                ' Start authentication
                Dim thDisconnect As New Thread(AddressOf conn.Disconnect)
                thDisconnect.Start()

        End Select
    End Sub

    ' UI updater
    Private Async Sub conn_ConnectionProgressChangedAsync(ByVal Progress As Connector.ConnectProgress, ByVal AdditionalString As String) Handles conn.ConnectProgressChanged
        Await Me.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
            Sub()
                Select Case Progress
                    Case Connector.ConnectProgress.RedirectToAuthPage
                        tbTitle.Text = res.GetString("C_RedirectToAuthPage")
                        proConnect.Value = 20
                    Case Connector.ConnectProgress.GatheringAuthData
                        tbTitle.Text = res.GetString("C_GatheringAuthData")
                        proConnect.Value = 40
                    Case Connector.ConnectProgress.SendAuthData
                        tbTitle.Text = res.GetString("C_SendAuthData")
                        proConnect.Value = 60
                    Case Connector.ConnectProgress.ExtractSessionData
                        tbTitle.Text = res.GetString("C_ExtractSessionData")
                        proConnect.Value = 80
                    Case Connector.ConnectProgress.AuthOK
                        tbTitle.Text = res.GetString("C_AuthOK")
                        proConnect.Value = 100
                        btnConnect.Content = res.GetString("UI_Disconnect")
                        btnConnect.IsEnabled = True

                        ConnectButtonRole = ConnectButtonRoles.Disconnect

                        Globals.inet_status = True
                        Globals.conn_by_jauth = True

                        ' Save all settings, NOW!
                        Util.SaveSettings()

                    Case Connector.ConnectProgress.AuthFailed
                        tbTitle.Text = res.GetString("C_AuthFailed")
                        proConnect.IsIndeterminate = True
                        proConnect.ShowPaused = False
                        proConnect.ShowError = True
                        btnConnect.IsEnabled = True
                    Case Connector.ConnectProgress.AuthError
                        tbTitle.Text = res.GetString("C_AuthError")
                        proConnect.IsIndeterminate = True
                        proConnect.ShowPaused = False
                        proConnect.ShowError = True
                        btnConnect.IsEnabled = True
                    Case Connector.ConnectProgress.GetSessionData
                        tbTitle.Text = res.GetString("C_GetSessionData")
                        proConnect.Value = 40
                    Case Connector.ConnectProgress.SendDeauthData
                        tbTitle.Text = res.GetString("C_SendDeauthData")
                        proConnect.Value = 80
                    Case Connector.ConnectProgress.DeauthOK
                        tbTitle.Text = res.GetString("C_DeauthOK")
                        proConnect.Value = 100
                        btnConnect.Content = res.GetString("UI_Connect")
                        btnConnect.IsEnabled = True

                        ConnectButtonRole = ConnectButtonRoles.Connect

                        Globals.inet_status = False
                        Globals.conn_by_jauth = False

                        ' Save all settings, NOW!
                        Util.SaveSettings()

                    Case Connector.ConnectProgress.DeauthFailed
                        tbTitle.Text = res.GetString("C_DeauthFailed")
                        proConnect.IsIndeterminate = True
                        proConnect.ShowPaused = False
                        proConnect.ShowError = True
                        btnConnect.IsEnabled = True
                    Case Connector.ConnectProgress.DeauthError
                        tbTitle.Text = res.GetString("C_DeauthError")
                        proConnect.IsIndeterminate = True
                        proConnect.ShowPaused = False
                        proConnect.ShowError = True
                        btnConnect.IsEnabled = True
                End Select
            End Sub)
    End Sub

End Class
