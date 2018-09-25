Imports Windows.Storage
Imports Windows.ApplicationModel.Resources

Module Globals

    ' String resources
    Private res As ResourceLoader = ResourceLoader.GetForCurrentView()

    ' Time Structure
    Public Structure RemainingTime
        Public Days As Integer
        Public Hours As Integer
        Public Minutes As Integer
        Public Seconds As Integer

        Public Sub New(day As Integer, hour As Integer, minute As Integer, second As Integer)
            Days = day
            Hours = hour
            Minutes = minute
            Seconds = second
        End Sub

        Public Overrides Function ToString() As String
            Dim day As String = res.GetString("C_Days")
            Dim hour As String = res.GetString("C_Hours")
            Dim minute As String = res.GetString("C_Minutes")
            Dim second As String = res.GetString("C_Seconds")
            Return $"{Days} {day} {Hours} {hour} {Minutes} {minute} {Seconds} {second}"
        End Function
    End Structure

    'Logging content
    Public log_content As String = ""

    ' Connection Status
    Public conn_by_jauth As Boolean = False
    Public inet_status As Boolean

    ' Authentication website
    Public strHost As String = ""

    ' Post Form data
    Public data_username As String = ""
    Public data_pwd As String = ""
    Public data_mac As String = ""
    Public data_wlancname As String = ""
    Public data_url As String = ""
    Public data_nasip As String = ""
    Public data_wlanuserip As String = ""
    Public data_jsessionid As String = ""
    Public data_usermac As String = ""
    Public data_userip As String = ""
    Public data_deviceip As String = ""

    ' Data Plan
    Public plan_name As String = ""
    Public plan_remaining As New RemainingTime(0, 0, 0, 0)

End Module
