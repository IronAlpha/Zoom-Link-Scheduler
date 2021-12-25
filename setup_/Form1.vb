Imports Microsoft.VisualBasic.FileIO

Public Class Form1
    Public core As String = Environ("appdata") & "\Scheduler\core"
    Public root As String = Environ("appdata") & "\Scheduler\Json"
    Public SchedulejsonPath As String = root & "\ScheduleJson.json"
    Public zoomlistjsonPath As String = root & "\ZoomJson.json"
    Public linkjsonPath As String = root & "\LinkJson.json"
    Public configurejsonPath As String = root & "\ConfigureJson.json"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        My.Computer.FileSystem.CreateDirectory(core)
        My.Computer.FileSystem.WriteAllBytes(core & "\Scheduler.exe", My.Resources.Scheduler, False)
        My.Computer.FileSystem.WriteAllBytes(core & "\Newtonsoft.Json.dll", My.Resources.Newtonsoft_Json, False)
        MsgBox("실행된 창에서 이름 설정이 끝난 후 그 창을 닫으면 설치가 계속됩니다.")
        Dim rc As Long = Shell(core & "\Scheduler.exe", AppWinStyle.NormalFocus)
        While Process.GetProcessById(rc).HasExited = False
        End While
        My.Computer.FileSystem.WriteAllBytes(SchedulejsonPath, My.Resources.ScheduleJson, False)
        My.Computer.FileSystem.WriteAllBytes(zoomlistjsonPath, My.Resources.ZoomJson, False)
        My.Computer.FileSystem.WriteAllBytes(linkjsonPath, My.Resources.LinkJson, False)
        CreateShortCut(core & "\Scheduler.exe", SpecialDirectories.Desktop, "Scheduler")
        MsgBox("설치가 완료되었습니다. 바탕화면에 바로가기가 생성되었습니다.")
        End
    End Sub
    Private Function CreateShortCut(ByVal TargetName As String, ByVal ShortCutPath As String, ByVal ShortCutName As String) As Boolean
        Dim oShell As Object
        Dim oLink As Object
        Try
            oShell = CreateObject("WScript.Shell")
            oLink = oShell.CreateShortcut(ShortCutPath & "\" & ShortCutName & ".lnk")
            oLink.TargetPath = TargetName
            oLink.WindowStyle = 1
            oLink.Save()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
