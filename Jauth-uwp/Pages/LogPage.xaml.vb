Imports Windows.ApplicationModel.Resources
Imports Windows.Storage.Provider
Imports Windows.Storage.Pickers
Imports Windows.Storage

Public NotInheritable Class LogPage : Inherits Page

    ' String resources
    Private res As ResourceLoader = ResourceLoader.GetForCurrentView()

    ' Show log text when page loading
    Private Sub Page_Loading(sender As FrameworkElement, args As Object)
        txtLogText.Text = Globals.log_content
    End Sub

    ' Clear the log text
    Private Sub btnClearLog_Tapped(sender As Object, e As TappedRoutedEventArgs)
        txtLogText.Text = ""
        Globals.log_content = ""
        flyClearLog.Hide()
    End Sub

    ' Reload the log text
    Private Sub barRefresh_Tapped(sender As Object, e As TappedRoutedEventArgs)
        txtLogText.Text = Globals.log_content
    End Sub

    ' Export log text to file
    Private Async Sub barSave_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Dim savePicker As New FileSavePicker
        savePicker.SuggestedStartLocation = PickerLocationId.Desktop
        savePicker.FileTypeChoices.Add("Plain Text", New List(Of String) From {".txt"})
        savePicker.SuggestedFileName = "Jauth_Log"

        Dim file As StorageFile = Await savePicker.PickSaveFileAsync()
        If file IsNot Nothing Then

            ' Prevent updates to the remote version of the file
            CachedFileManager.DeferUpdates(file)

            ' Write to file
            Await FileIO.WriteTextAsync(file, Globals.log_content)

            ' Let Windows know we're finished changing the file
            Dim status As FileUpdateStatus = Await CachedFileManager.CompleteUpdatesAsync(file)
            If status = FileUpdateStatus.Complete Then

                ' Write successful
                Logging($"{res.GetString("L_SaveFile")}: {file.Name}, {res.GetString("L_Success")}")

            Else

                ' Write failed
                Logging($"{res.GetString("L_SaveFile")}: {file.Name}, {res.GetString("L_Failed")}")

            End If

        Else
            ' Operation canceled
            Logging($"{res.GetString("FileSavingCanceled")}")
        End If
    End Sub


End Class
