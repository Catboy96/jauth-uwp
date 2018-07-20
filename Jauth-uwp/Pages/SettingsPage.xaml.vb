Imports Windows.ApplicationModel.Resources
Imports Windows.System

Public NotInheritable Class SettingsPage : Inherits Page

    ' String resources
    Private res As ResourceLoader = ResourceLoader.GetForCurrentView()

    Private Sub tbLicenseIcon_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Dim success = Launcher.LaunchUriAsync(New Uri("https://github.com/google/material-design-icons/blob/master/LICENSE"))
    End Sub

    Private Sub tbLicenseHtml_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Dim success = Launcher.LaunchUriAsync(New Uri("https://github.com/zzzprojects/html-agility-pack/blob/master/LICENSE"))
    End Sub

    Private Sub tbWebsite_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Dim success = Launcher.LaunchUriAsync(New Uri("https://make.ralf.ren/jauth"))
    End Sub

    Private Sub tbPrivacy_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Dim success = Launcher.LaunchUriAsync(New Uri("https://make.ralf.ren/jauth/privacy.html"))
    End Sub

    Private Sub btnSave_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Globals.data_username = txtAccount.Text
        Globals.data_pwd = txtPassword.Password

        ' Save all settings, NOW!
        Util.SaveSettings()

        btnSave.Content = $"{res.GetString("UI_Saved")}"
        btnSave.IsEnabled = False
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        txtAccount.Text = If(Globals.data_username = "", "", Globals.data_username)
        txtPassword.Password = If(Globals.data_pwd = "", "", Globals.data_pwd)
    End Sub
End Class
