Imports Windows.ApplicationModel.Resources

Public NotInheritable Class MainPage : Inherits Page

    ' Pages
    Private WithEvents pageConnect As New ConnectPage
    Private WithEvents pageSettings As New SettingsPage
    Private WithEvents pageInfo As New InfoPage
    Private WithEvents pageLog As New LogPage

    ' String resources
    Private res As ResourceLoader = ResourceLoader.GetForCurrentView()

    Private Sub navConnect_Tapped(sender As Object, e As TappedRoutedEventArgs)
        frmMain.Navigate(pageConnect.GetType)
    End Sub

    Private Sub navInfo_Tapped(sender As Object, e As TappedRoutedEventArgs)
        frmMain.Navigate(pageInfo.GetType)
    End Sub

    Private Sub navLog_Tapped(sender As Object, e As TappedRoutedEventArgs)
        frmMain.Navigate(pageLog.GetType)
    End Sub

    Private Sub Page_Loading(sender As FrameworkElement, args As Object)
        nvMain.AlwaysShowHeader = False
        Logging($"{res.GetString("L_JauthStarted")}")
        frmMain.Navigate(pageConnect.GetType)
    End Sub

    Private Sub NavigationView_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)
        If (args.IsSettingsInvoked) Then
            frmMain.Navigate(pageSettings.GetType)
        End If
    End Sub

End Class
