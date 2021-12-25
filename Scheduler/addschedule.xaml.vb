Imports Newtonsoft.Json.Linq

Class addschedule
    Private Sub Label_MouseDown(sender As Object, e As MouseButtonEventArgs)
        MainFrm.SideMenuClose()
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
        Dim tp As New TaskProperty
        With tp
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
            Dim c = Color.FromRgb(Rand.Next(0, 255), Rand.Next(0, 255), Rand.Next(0, 255))
            .ColorHex = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2") ' Nothing
            .SetDuration()
            .SetID()
        End With
        Dim j As New JObject
        With j
            .Add("Title", tp.Title)
            .Add("ID", tp.ID)
            .Add("DOW", DOWCOMBO.SelectedIndex)
            .Add("StartTime", tp.StartTime.ToString("HH:mm"))
            .Add("EndTime", tp.EndTime.ToString("HH:mm"))
            .Add("ZoomID", tp.ZoomProp.ID)
            .Add("LinkID", tp.LinkProp.ID)
            .Add("Duration", tp.Duration)
            .Add("Color", tp.ColorHex)
        End With
        TaskPropertyList.Add(tp)
        ScheduleJson.Add(tp.ID, j)
        saveTaskJson()
        Dim tc As New TaskControl
        tc.LoadTask(tp)
        SetBigPanel(tc, tp, MainFrm.sb)
        FrmScheduleGrid.Children.Add(tc)
        MakeTodayList()
        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.Setting)
        MainFrm.SideMenuClose()
    End Sub
End Class
