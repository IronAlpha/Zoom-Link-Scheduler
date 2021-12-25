Class funcpage
    Private Sub Border_MouseDown(sender As Object, e As MouseButtonEventArgs)
        If BoolMsgbox("정말 모든 시간표를 삭제하시겠습니까?") Then
            ScheduleJson.RemoveAll()
            TaskPropertyList.Clear()
            saveTaskJson()
        End If
    End Sub

    Private Sub Border_MouseDown_1(sender As Object, e As MouseButtonEventArgs)
        Dim msa As New makeScheduleArr
        msa.ShowDialog()
    End Sub

    Private Sub Border_MouseDown_2(sender As Object, e As MouseButtonEventArgs)
        For Each tp As TaskProperty In TaskPropertyList
            For Each zp As ZoomProperty In ZoomPropertyList
                If tp.ZoomProp Is Nothing Then
                    If tp.Title = zp.Title Then
                        tp.ZoomProp = zp
                        tp.JObject("ZoomID") = zp.ID
                    End If
                End If
            Next
        Next
        saveTaskJson()
        MainFrm.loadScheduleControl()
    End Sub

    Private Sub Border_MouseDown_3(sender As Object, e As MouseButtonEventArgs)
        ads.clear()
        MainFrm.sidemenufrm.Content = ads
        MainFrm.SideMenuOpen()
    End Sub

    Private Sub Border_MouseDown_4(sender As Object, e As MouseButtonEventArgs)
        edz.clear()
        MainFrm.sidemenufrm.Content = edz
        MainFrm.SideMenuOpen()
    End Sub

    Private Sub Border_MouseDown_5(sender As Object, e As MouseButtonEventArgs)
        edl.clear()
        MainFrm.sidemenufrm.Content = edl
        MainFrm.SideMenuOpen()
    End Sub
End Class
