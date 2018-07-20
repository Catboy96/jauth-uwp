Imports Windows.ApplicationModel.Core

NotInheritable Class App : Inherits Application

    ' Application start
    Protected Overrides Sub OnLaunched(e As LaunchActivatedEventArgs)

        ' Extend view into title bar
        Dim titleBar = CoreApplication.GetCurrentView().TitleBar
        titleBar.ExtendViewIntoTitleBar = True

        ' Root window frame
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
        If rootFrame Is Nothing Then

            rootFrame = New Frame()
            AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed

            ' Application was suspended and been terminated by system due to lack of RAM.
            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                Util.LoadSettings()
            End If

            ' Application was suspended and still stays in the RAM.
            If e.PreviousExecutionState = ApplicationExecutionState.Suspended Then
                Util.LoadSettings()
            End If

            ' Application was closed by user.
            If e.PreviousExecutionState = ApplicationExecutionState.ClosedByUser Then
                Util.LoadSettings()
            End If

            ' Application has not ever launched since last boot.
            If e.PreviousExecutionState = ApplicationExecutionState.NotRunning Then
                Util.LoadSettings()
            End If

            Window.Current.Content = rootFrame
        End If

        ' Start window
        If e.PrelaunchActivated = False Then

            ' Navigate to first page
            If rootFrame.Content Is Nothing Then
                rootFrame.Navigate(GetType(MainPage), e.Arguments)
            End If

            ' Activate window
            Window.Current.Activate()
        End If
    End Sub

    ' Navigation failed
    Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
        Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
    End Sub

    ' Application suspending
    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()

        ' Save settings when suspended
        Util.SaveSettings()

        deferral.Complete()
    End Sub

End Class
