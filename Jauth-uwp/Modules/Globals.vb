Imports Windows.Storage

Module Globals

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

End Module
