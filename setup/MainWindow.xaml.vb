Class MainWindow
    Public core As String = Environ("appdata") & "\Scheduler\core"
    Public root As String = Environ("appdata") & "\Scheduler\Json"
    Public SchedulejsonPath As String = root & "\ScheduleJson.json"
    Public zoomlistjsonPath As String = root & "\ZoomJson.json"
    Public linkjsonPath As String = root & "\LinkJson.json"
    Public configurejsonPath As String = root & "\ConfigureJson.json"
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub
    Private Sub ShellEnd(ProcessID As Long)
        Dim hProcess As Long
        Dim EndCode As Long
        Dim EndRet As Long
        '핸들을 가져옴
        hProcess = OpenProcess(PROCESS_QUERY_INFORMATION, 1, ProcessID)
        '종료할 때까지 대기
        Do
            EndRet = GetExitCodeProcess(hProcess, EndCode)
        Loop While (EndCode = STILL_ACTIVE)
        '핸들을 닫음
        EndRet = CloseHandle(hProcess)
    End Sub
    Private Declare Function OpenProcess Lib "kernel32" _
    (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long,
     ByVal dwProcessId As Long) As Long
    '지정한 프로세스 오브젝트 핸들링
    Private Declare Function GetExitCodeProcess Lib "kernel32" _
        (ByVal hProcess As Long, lpExitCode As Long) As Long
    '열러있는 프로세스 오브젝트 핸들링 해제
    Private Declare Function CloseHandle Lib "kernel32" _
        (ByVal hObject As Long) As Long
    Private Const PROCESS_QUERY_INFORMATION = &H400&
    Private Const STILL_ACTIVE = &H103&
End Class
