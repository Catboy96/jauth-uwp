Public NotInheritable Class InfoPage : Inherits Page

    Private Sub Page_Loading(sender As FrameworkElement, args As Object)
        txtSessionId.Text = If(Globals.data_jsessionid = "", "", Globals.data_jsessionid)
        txtUserMac.Text = If(Globals.data_usermac = "", "", Globals.data_usermac)
        txtUserIP.Text = If(Globals.data_userip = "", "", Globals.data_userip)
        txtDeviceIP.Text = If(Globals.data_deviceip = "", "", Globals.data_deviceip)
        txtMac.Text = If(Globals.data_mac = "", "", Globals.data_mac)
        txtWlancname.Text = If(Globals.data_wlancname = "", "", Globals.data_wlancname)
        txtUrl.Text = If(Globals.data_url = "", "", Globals.data_url)
        txtNasip.Text = If(Globals.data_nasip = "", "", Globals.data_nasip)
        txtWlanuserip.Text = If(Globals.data_wlanuserip = "", "", Globals.data_wlanuserip)
    End Sub

End Class
