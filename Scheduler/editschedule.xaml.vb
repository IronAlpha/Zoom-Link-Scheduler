Class editschedule
    Dim myschedule As TaskProperty
    Public Sub loadtask(tp As TaskProperty)
        clear()
        myschedule = tp
        titlebox.Text = tp.Title
        sttimebox.Text = tp.StartTime.ToString("HH:mm")
        endtimebox.Text = tp.EndTime.ToString("HH:mm")
        DOWCOMBO.SelectedValue = DOWCOMBO.Items(tp.DayOfWeek)
        If tp.ZoomProp IsNot Nothing Then
            zoombox.Text = tp.ZoomProp.Title
        End If
        If tp.LinkProp IsNot Nothing Then
            linkbox.Text = tp.LinkProp.Title
        End If
    End Sub
    Public Sub clear()
        titlebox.Text = ""
        sttimebox.Text = ""
        endtimebox.Text = ""
        zoombox.Text = ""
        linkbox.Text = ""
    End Sub
    Private Sub Label_MouseDown_1(sender As Object, e As MouseButtonEventArgs)
        Dim StartTime, EndTime As DateTime
        If DateTime.TryParse(sttimebox.Text, StartTime) = False Then
            BoolMsgbox("시간을 정확히 입력해 주세요.")
            Exit Sub
        End If
        If DateTime.TryParse(endtimebox.Text, EndTime) = False Then
            BoolMsgbox("시간을 정확히 입력해 주세요.")
            Exit Sub
        End If
        If titlebox.Text = "" Then
            BoolMsgbox("제목을 입력해 주세요.")
            Exit Sub
        End If
        If EndTime < StartTime Then
            BoolMsgbox("시간을 정확히 입력해 주세요.")
            Exit Sub
        End If
        If Not zoombox.Text = "" Then
            If ZoomTitleReturn(zoombox.Text) Is Nothing Then
                BoolMsgbox("입력된 제목의 줌이 없습니다.")
                Exit Sub
            End If
        End If
        If Not linkbox.Text = "" Then
            If LinkTitleReturn(linkbox.Text) Is Nothing Then
                BoolMsgbox("입력된 제목의 링크가 없습니다.")
                Exit Sub
            End If
        End If
        If DOWCOMBO.SelectedItem Is Nothing Then
            BoolMsgbox("요일을 선택하세요.")
            Exit Sub
        End If
        With myschedule
            .Title = titlebox.Text
            .StartTime = StartTime
            .EndTime = EndTime
            .DayOfWeek = DOWCOMBO.SelectedIndex

            .ZoomProp = New ZoomProperty With {.ID = ""}
            .LinkProp = New LinkProperty With {.ID = ""}

            If Not zoombox.Text = "" Then
                .ZoomProp = ZoomTitleReturn(zoombox.Text)
            End If
            If Not linkbox.Text = "" Then
                .LinkProp = LinkTitleReturn(linkbox.Text)
            End If
            .SetDuration()

            .JObject("Title") = myschedule.Title
            .JObject("DOW") = DOWCOMBO.SelectedIndex
            .JObject("StartTime") = StartTime.ToString("HH:mm")
            .JObject("EndTime") = EndTime.ToString("HH:mm")
            .JObject("ZoomID") = myschedule.ZoomProp.ID
            .JObject("LinkID") = myschedule.LinkProp.ID
            .JObject("Duration") = myschedule.Duration
        End With
        saveTaskJson()
        ScheduleControlReturn(myschedule).LoadTask(myschedule)
        MakeTodayList()
        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.Setting)
        MainFrm.SideMenuClose()
    End Sub

    Private Sub Label_MouseDown(sender As Object, e As MouseButtonEventArgs)
        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.Setting)
        MainFrm.SideMenuClose()
    End Sub

    Private Sub Label_MouseDown_2(sender As Object, e As MouseButtonEventArgs)
        TaskPropertyList.Remove(myschedule)
        ScheduleJson.Remove(myschedule.ID)
        ScheduleControlReturn(myschedule).KillSelf()
        MakeTodayList()
        saveTaskJson()
        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.Setting)
        MainFrm.SideMenuClose()
    End Sub
End Class
