Imports System.Windows.Media.Animation
Imports System.Windows.Threading

Class MainWindow
    Dim WheelTimer As New DispatcherTimer With {.Interval = TimeSpan.FromMilliseconds(300)}
    Dim WheelZoomLabel As New Label With {.Background = New SolidColorBrush(Color.FromRgb(250, 250, 250)), .VerticalContentAlignment = VerticalAlignment.Center, .HorizontalContentAlignment = HorizontalAlignment.Center, .FontFamily = New FontFamily("나눔스퀘어_ac"), .FontSize = 16}
    Dim TaskPropTimer As New DispatcherTimer
    Dim menustate As Boolean = False

    Public sb As Storyboard
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        MainFrm = Me
        ScheduleWidth = (TaskGrid.ActualWidth / 8)
        TopDateGrid.Width = TaskGrid.ActualWidth
        Setup()
        bigPanelCompose()

        getZoomlist()
        getLinklist()
        getTasklist()
        getLinklist()
        setDatebox()
        LoadTaskTopDate()
        SideMenuCompose()

        loadScheduleControl()
        loadSideControl()

        InitializeTaskPropTimer()
        InitializeWheelZoom()

        notifysetup()
    End Sub
    Sub bigPanelCompose()
        bigPanel.Created()
        sb = bigPanel.FindResource("fadeout")
        bigPanel.HorizontalAlignment = HorizontalAlignment.Left
        bigPanel.VerticalAlignment = VerticalAlignment.Top
        Grid.SetZIndex(bigPanel, 100)
    End Sub
    Sub BigpanelRemove()
        AddHandler FrmScheduleGrid.MouseDown, Sub(sender As Object, e As MouseButtonEventArgs)
                                                  bigPanel.Hide()
                                              End Sub
    End Sub
    Sub SideMenuClose()
        Dim sb As Storyboard = FindResource("menuin")
        sb.Begin()
        menustate = False
        AddHandler sb.Completed, Sub()
                                     menuborder.Visibility = Visibility.Hidden
                                 End Sub
    End Sub
    Sub SideMenuOpen()
        menuborder.Visibility = Visibility.Visible
        Dim sb As Storyboard = FindResource("menuout")
        sb.Begin()
        menustate = True
    End Sub
    Sub SideMenuCompose()
        UserLabel.Content = uname
        menuborder.Visibility = Visibility.Hidden
        SideSearchPage = New searchpage
        SideListPage = New listpage
        SideSettingPage = New settingpage
        SideFuncPage = New funcpage
        SideInfoPage = New infopage
        sidemenufrm.Content = SideSearchPage
        SideListPage.loadpage()
    End Sub
    Sub InitializeTaskPropTimer()
        TaskPropTimer.Interval = TimeSpan.FromSeconds(10)
        AddHandler TaskPropTimer.Tick, AddressOf TaskPropTimer_Tick
        TaskPropTimer_Tick()
        TaskPropTimer.Start()
    End Sub
    Sub TaskPropTimer_Tick()
        For Each TaskProp As TaskProperty In TodayList
            With TaskProp
                Dim scr = ScheduleControlReturn(TaskProp)
                If .StartTime < Now And Now < .EndTime Then
                    scr.BaseBorder.Background = New SolidColorBrush(ColorConverter.ConvertFromString("#FFFFEA"))
                ElseIf .StartTime.AddMinutes(-AutoStartMin) <= Now And Now < .StartTime Then
                    scr.BaseBorder.Background = New SolidColorBrush(ColorConverter.ConvertFromString("#EAEAEA"))
                Else
                    scr.BaseBorder.Background = New SolidColorBrush(Color.FromRgb(255, 255, 255))
                End If
                Dim R As Integer = 0
                If .AutoStarted = False Then
                    If AutoStartZoom = True Then
                        If .StartTime.AddMinutes(-AutoStartMin) <= Now And Now < .StartTime Then
                            If TaskProp.ZoomProp IsNot Nothing Then
                                If AlertBeforeStart = True Then
                                    If BoolMsgbox(.ZoomProp.Title & " 줌을 실행할까요?") = True Then
                                        joinZoom(.ZoomProp)
                                    End If
                                Else
                                    joinZoom(.ZoomProp)
                                End If

                                R += 1
                                End If
                            End If
                    End If
                    If AutoStartLink = True Then
                        If .StartTime.AddMinutes(-AutoStartMin) <= Now And Now < .StartTime Then
                            If TaskProp.LinkProp IsNot Nothing Then
                                openLink(.LinkProp)
                                R += 1
                            End If
                        End If
                    End If
                    If R > 0 Then
                        .AutoStarted = True
                    End If
                End If
            End With
        Next
    End Sub
    Private Sub closebutton_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles closebutton.MouseDown
        Close()
    End Sub

    Private Sub baseborder_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles baseborder.MouseDown
        If e.LeftButton Then
            Me.DragMove()
        End If
    End Sub
    Sub setDatebox()
        If DisplayLanguage = EnumModule.Language.English Then
            monthbox.FontFamily = New FontFamily("Roboto Regular")
            monthbox.Content = MonthOfYear_EN(Now.Month)
        Else
            monthbox.FontFamily = New FontFamily("나눔스퀘어_ac Bold")
            monthbox.Content = MonthOfYear(Now.Month)
        End If
        yearbox.Content = Now.Year

        monthbox.Measure(New Size(Double.PositiveInfinity, Double.PositiveInfinity))

        yearbox.Margin = New Thickness(monthbox.DesiredSize.Width, 10, 0, 0)
    End Sub
    Sub LoadTaskTopDate()
        Dim week_startday As Date = Now.AddDays(-Now.DayOfWeek)
        Dim topLabelFontSize As Double = 12
        Dim scheduleHeight As Double = TopDateGrid.Height

        TopDateGrid.Children.Clear()

        Dim timelineTopLabel As New Label With {.Content = Int(Now.DayOfYear / 7), .BorderThickness = New Thickness(1, 0, 1, 1), .BorderBrush = New SolidColorBrush(Color.FromRgb(180, 180, 180)), .Width = ScheduleWidth, .Height = scheduleHeight, .FontFamily = New FontFamily("Roboto Light"), .FontSize = topLabelFontSize, .Background = New SolidColorBrush(Color.FromArgb(120, 5, 20, 180))}
        If DisplayLanguage = EnumModule.Language.English Then
            timelineTopLabel.Content = "Week " & timelineTopLabel.Content
        Else
            timelineTopLabel.Content = timelineTopLabel.Content & "번째 주"
        End If
        CenterAlignLabel(timelineTopLabel)
        TopDateGrid.Children.Add(timelineTopLabel)

        For i As Integer = 0 To 7
            Dim targetdate As Date = week_startday.AddDays(i)
            Dim targetLabel As New DateLabel With {.BorderBrush = New SolidColorBrush(Color.FromRgb(180, 180, 180)), .Width = ScheduleWidth, .Height = scheduleHeight, .FontSize = topLabelFontSize, .TargetDate = targetdate}
            With targetLabel
                If DisplayLanguage = EnumModule.Language.English Then
                    .FontFamily = New FontFamily("Roboto Light")
                    .Content = DayOfWeek_s_EN(targetdate.DayOfWeek) & " " & targetdate.Day
                Else
                    .FontFamily = New FontFamily("나눔스퀘어_ac ")
                    .Content = DayOfWeek_s(targetdate.DayOfWeek) & " " & targetdate.Day
                End If
                .BorderThickness = New Thickness(0, 0, 1, 1)
                If targetdate.Date = Now.Date Then
                    .Background = New SolidColorBrush(Color.FromArgb(120, 5, 180, 20))
                End If
                CenterAlignLabel(targetLabel)
                AddHandler .MouseDown, Sub()
                                           loadSideControl(.TargetDate)
                                       End Sub
                TopDateGrid.Children.Add(targetLabel)
            End With
        Next
    End Sub
    Sub loadSideControl(Optional LoadDate As Date = Nothing)
        SidePanel.Children.Clear()
        TopSideGrid.Children.Clear()

        Dim LoadList As New List(Of TaskProperty)
        If LoadDate <> Nothing Then
            For Each TaskProp As TaskProperty In TaskPropertyList
                If TaskProp.DayOfWeek = LoadDate.DayOfWeek Then
                    LoadList.Add(TaskProp)
                End If
            Next
            LoadList.Sort(Function(x As TaskProperty, y As TaskProperty) x.StartTime.CompareTo(y.StartTime))
        Else
            LoadDate = Now.Date
            LoadList = TodayList
        End If
        Dim targetLabel As New Label With {.BorderBrush = New SolidColorBrush(Color.FromRgb(180, 180, 180)), .Width = SidePanel.ActualWidth - 2, .Height = TopSideGrid.Height, .FontSize = 12, .Margin = New Thickness(2, 0, 0, 0)}
        With targetLabel
            If LoadDate.Date = Now.Date Then
                .Background = New SolidColorBrush(Color.FromArgb(120, 5, 180, 20))
            End If
            .Content = LoadDate.Date.ToShortDateString
            If DisplayLanguage = EnumModule.Language.English Then
                .FontFamily = New FontFamily("Roboto Light")
            Else
                .FontFamily = New FontFamily("나눔스퀘어_ac")
            End If
            .BorderThickness = New Thickness(1, 0, 1, 1)
            CenterAlignLabel(targetLabel)
            TopSideGrid.Children.Add(targetLabel)
        End With

        Dim prLB As Label = Nothing
        Dim prGr As Grid = Nothing
        Dim prEd As DateTime = Nothing
        Dim br As VisualBrush = Nothing
        '  If LoadDate.Date = Now.Date Then
        '       br = FindResource("hatchBackground")
        '    Else
        '        br = Nothing
        '   End If
        SidePanel.Background = br
        If LoadList.Count > 0 Then
            For Each TaskProp As TaskProperty In LoadList
                Dim il As New TaskBigControl
                il.Compose(TaskProp)
                il.SideCompose()
                il.Width = SidePanel.ActualWidth - 2
                SidePanel.Children.Add(il)
                If prLB IsNot Nothing Then
                    Dim sb As TimeSpan = TaskProp.StartTime.Subtract(prEd)
                    If sb.TotalMinutes >= 10 Then
                        prLB.Content = sb.ToString
                    End If
                    If sb.TotalMinutes > 0 Then
                        prLB.Height = Math.Log(TaskProp.StartTime.Subtract(prEd).TotalMinutes, 5) ^ 2 * 20
                    Else
                        prLB.Height = 10
                    End If
                End If
                Dim l As New Label With {.Width = SidePanel.ActualWidth, .HorizontalAlignment = HorizontalAlignment.Center, .VerticalAlignment = VerticalAlignment.Center, .Foreground = New SolidColorBrush(Color.FromRgb(160, 160, 160)), .FontFamily = New FontFamily("나눔스퀘어_ac Light"), .FontSize = 14, .VerticalContentAlignment = VerticalAlignment.Center, .HorizontalContentAlignment = HorizontalAlignment.Center}
                SidePanel.Children.Add(l)
                prLB = l
                prEd = TaskProp.EndTime
            Next
        Else
            Dim gr As New Grid With {.Background = br, .Height = SideSCRV.ActualHeight, .Width = SidePanel.ActualWidth}
            Dim l As New Label With {.HorizontalAlignment = HorizontalAlignment.Center, .VerticalAlignment = VerticalAlignment.Center, .Foreground = New SolidColorBrush(Color.FromRgb(160, 160, 160)), .FontFamily = New FontFamily("나눔스퀘어_ac Light"), .FontSize = 14, .Content = "일정이 없습니다."}
            gr.Children.Add(l)
            SidePanel.Children.Add(gr)
        End If
        GC.Collect()
    End Sub
    Sub loadScheduleControl()
        TaskGrid.Children.Clear()
        Dim Sepline As New List(Of Double)
        Dim SeplineTask As New List(Of Double)
        Dim ScheduleGrid As New ScheduleControl With {.Height = 60 * MinuteHeight * 24, .Width = ScheduleWidth * 8}
        For i As Integer = 1 To 23
            Dim ll As New Label
            With ll
                If HourFormat = HourFormatEnum.Use24 Then
                    .Content = Convert.ToDateTime(i & ":00").ToString("HH:mm")
                Else
                    .Content = Convert.ToDateTime(i & ":00").ToString("hh:mm")
                End If
                If DisplayLanguage = EnumModule.Language.English Then
                    If i >= 12 Then
                        .Content = .Content & " PM"
                    Else
                        .Content = .Content & " AM"
                    End If
                Else
                    If i >= 12 Then
                        .Content = "오후 " & .Content
                    Else
                        .Content = "오전 " & .Content
                    End If
                End If
                .Width = ScheduleWidth
                .Height = getTop(TimeSpan.FromHours(1))
                .FontFamily = New FontFamily("나눔스퀘어_ac")
                .FontSize = 12
                .VerticalAlignment = VerticalAlignment.Top
                .HorizontalAlignment = HorizontalAlignment.Left
                .HorizontalContentAlignment = HorizontalAlignment.Right
                .Margin = New Thickness(0, getTop(TimeSpan.FromHours(i)) - 6, 0, 0)
                If Not Sepline.Contains(getTop(TimeSpan.FromHours(i))) Then
                    Sepline.Add(getTop(TimeSpan.FromHours(i)))
                End If
                .Padding = New Thickness(0, 0, 30, 0)
            End With
            Grid.SetZIndex(ll, 1)
            ScheduleGrid.add(ll)
        Next

        Dim TodayGrid As New Grid With {.Background = FindResource("hatchBackground"), .Width = ScheduleWidth, .Height = ScheduleGrid.Height, .Margin = New Thickness(ScheduleWidth * (Now.DayOfWeek + 1), 0, 0, 0), .VerticalAlignment = VerticalAlignment.Top, .HorizontalAlignment = HorizontalAlignment.Left}
        ScheduleGrid.add(TodayGrid)

        For Each TaskProp As TaskProperty In TaskPropertyList
            Dim tc As New TaskControl
            With tc
                Grid.SetZIndex(tc, 1)
                .LoadTask(TaskProp)
                If Not Sepline.Contains(getTop(TaskProp.StartTime.TimeOfDay)) Then
                    If Not SeplineTask.Contains(getTop(TaskProp.StartTime.TimeOfDay)) Then
                        SeplineTask.Add(getTop(TaskProp.StartTime.TimeOfDay))
                    End If
                End If
                SetBigPanel(tc, TaskProp, sb)
            End With
            If UseShadow = False Then
                tc.BaseBorder.Effect = Nothing
            End If
            ScheduleGrid.add(tc)
        Next

        If LoadLine = TaskGridLineEnum.Both Or LoadLine = TaskGridLineEnum.HorizontalOnly Then
            For Each i As Double In Sepline
                Dim li As New Line
                With li
                    .X1 = ScheduleWidth - 30
                    .X2 = ScheduleWidth * 8
                    .Y1 = i
                    .Y2 = i
                    .VerticalAlignment = VerticalAlignment.Top
                    .Stroke = New SolidColorBrush(Color.FromRgb(180, 180, 180))
                    .StrokeThickness = 0.3
                End With
                ScheduleGrid.add(li)
            Next
        End If

        If LoadLineTask = TaskGridLineEnum.HorizontalOnly Then
            For Each i As Double In SeplineTask
                Dim li As New Line
                With li
                    .X1 = ScheduleWidth
                    .X2 = ScheduleWidth * 8
                    .Y1 = i
                    .Y2 = i
                    .VerticalAlignment = VerticalAlignment.Top
                    .Stroke = New SolidColorBrush(Color.FromRgb(180, 180, 180))
                    .StrokeThickness = 0.3
                End With
                ScheduleGrid.add(li)
            Next
        End If

        Dim TimeLab As New Label
        With TimeLab
            .Content = Now.ToString("HH:mm ")
            If Now.Hour >= 12 Then
                .Content = .Content & "PM"
            Else
                .Content = .Content & "AM"
            End If
            .FontFamily = New FontFamily("나눔스퀘어_ac")
            .FontSize = 14
            .HorizontalContentAlignment = HorizontalAlignment.Center
            .VerticalContentAlignment = VerticalAlignment.Center
            .Padding = New Thickness(5, 3, 5, 3)
            Grid.SetZIndex(TimeLab, 10)
        End With
        Dim br As New Border With {.CornerRadius = New CornerRadius(5), .Child = TimeLab, .Background = New SolidColorBrush(Color.FromArgb(120, 5, 20, 180)), .HorizontalAlignment = HorizontalAlignment.Left, .VerticalAlignment = VerticalAlignment.Top}
        br.Measure(New Size(Double.PositiveInfinity, Double.PositiveInfinity))
        br.Margin = New Thickness(ScheduleWidth - br.DesiredSize.Width - 30, getTop(Now.TimeOfDay) - 12, 0, 0)
        ScheduleGrid.add(br)

        Dim TimeLabLine As New Line
        With TimeLabLine
            .X1 = ScheduleWidth - 30
            .X2 = ScheduleWidth * 8
            .Y1 = getTop(Now.TimeOfDay)
            .Y2 = getTop(Now.TimeOfDay)
            .VerticalAlignment = VerticalAlignment.Top
            .Stroke = New SolidColorBrush(Color.FromArgb(120, 5, 20, 180))
            .StrokeThickness = 0.5
            Grid.SetZIndex(TimeLabLine, 10)
        End With
        ScheduleGrid.add(TimeLabLine)

        Dim timeSep As New DispatcherTimer
        With timeSep
            .Interval = TimeSpan.FromSeconds(10)
            AddHandler .Tick, Sub()
                                  With TimeLab
                                      br.Margin = New Thickness(ScheduleWidth - br.ActualWidth - 30, getTop(Now.TimeOfDay) - 12, 0, 0)
                                      .Content = Now.ToString("HH:mm ")
                                      If Now.Hour >= 12 Then
                                          .Content = .Content & "PM"
                                      Else
                                          .Content = .Content & "AM"
                                      End If

                                      TimeLabLine.Y1 = getTop(Now.TimeOfDay)
                                      TimeLabLine.Y2 = getTop(Now.TimeOfDay)
                                  End With
                              End Sub
            .Start()
        End With

        TaskGrid.Children.Add(ScheduleGrid)
        FrmScheduleGrid = ScheduleGrid.BaseGrid
        TaskSCRV.ScrollToVerticalOffset(getTop(Now.TimeOfDay) - TaskSCRV.Height / 2)

        BigpanelRemove()
        GC.Collect()
    End Sub

    Sub loadScheduleControl_old()
        Dim Sepline As New List(Of Double)
        Dim timeline, sun, mon, tue, wed, thu, fri, sat As New ScheduleControl
        Dim ScheduleGridArray As ScheduleControl() = {timeline, sun, mon, tue, wed, thu, fri, sat}
        Dim DateGridArray As ScheduleControl() = {sun, mon, tue, wed, thu, fri, sat}


        For i As Integer = 1 To 23
            Dim ll As New Label
            With ll
                If HourFormat = HourFormatEnum.Use24 Then
                    .Content = Convert.ToDateTime(i & ":00").ToString("HH:mm")
                Else
                    .Content = Convert.ToDateTime(i & ":00").ToString("hh:mm")
                End If
                If DisplayLanguage = EnumModule.Language.English Then
                    If i >= 12 Then
                        .Content = .Content & " PM"
                    Else
                        .Content = .Content & " AM"
                    End If
                Else
                    If i >= 12 Then
                        .Content = "오후 " & .Content
                    Else
                        .Content = "오전 " & .Content
                    End If
                End If
                .Width = 100
                .Height = getTop(TimeSpan.FromHours(1))
                .FontFamily = New FontFamily("나눔스퀘어_ac")
                .FontSize = 12
                .VerticalAlignment = VerticalAlignment.Top
                .HorizontalContentAlignment = HorizontalAlignment.Right
                .Margin = New Thickness(0, getTop(TimeSpan.FromHours(i)) - 6, 0, 0)
                If Not Sepline.Contains(getTop(TimeSpan.FromHours(i))) Then
                    Sepline.Add(getTop(TimeSpan.FromHours(i)))
                End If
                .Padding = New Thickness(0, 0, 15, 0)
            End With
            Grid.SetZIndex(ll, 1)
            timeline.add(ll)
        Next

        For Each TaskProp As TaskProperty In TaskPropertyList
            Dim TaskControl As New TaskControl
            With TaskControl
                .Margin = New Thickness(10, getTop(TaskProp.StartTime.TimeOfDay), 0, 0)
                .HorizontalAlignment = HorizontalAlignment.Left
                .VerticalAlignment = VerticalAlignment.Top
                .Width = ScheduleWidth - 20
                Grid.SetZIndex(TaskControl, 1)
                .LoadTask(TaskProp)
            End With
            ScheduleGridArray(TaskProp.DayOfWeek + 1).add(TaskControl)
        Next

        For Each i As Double In Sepline
            For Each sgrd As ScheduleControl In DateGridArray
                Dim li As New Line
                With li
                    .X1 = 0
                    .X2 = ScheduleWidth
                    .Y1 = i
                    .Y2 = i
                    .VerticalAlignment = VerticalAlignment.Top
                    .Stroke = New SolidColorBrush(Color.FromRgb(180, 180, 180))
                    .StrokeThickness = 0.4
                End With
                sgrd.add(li)
            Next
            timeline.add(New Line With {.X1 = ScheduleWidth / 10 * 8,
                    .X2 = ScheduleWidth,
            .Y1 = i,
            .Y2 = i,
            .VerticalAlignment = VerticalAlignment.Top,
            .Stroke = New SolidColorBrush(Color.FromRgb(180, 180, 180)),
            .StrokeThickness = 0.4})
        Next

        For Each sgrd As ScheduleControl In ScheduleGridArray
            sgrd.Width = ScheduleWidth
            If ScheduleGridArray.First Is sgrd Then
                sgrd.baseborder.BorderThickness = New Thickness(0, 0, 1, 0)
            Else
                sgrd.baseborder.BorderThickness = New Thickness(0, 0, 1, 0)
            End If
            TaskGrid.Children.Add(sgrd)
        Next
    End Sub


    Private Sub TaskSCRV_PreviewMouseWheel(sender As Object, e As MouseWheelEventArgs) Handles TaskSCRV.PreviewMouseWheel
        Dim ScrollAmount As Double = 0.1
        If Keyboard.Modifiers = ModifierKeys.Control Then
            TaskGrid.Children.Clear()
            If WheelZoomLabel.Parent Is Nothing Then
                TaskGrid.Children.Add(WheelZoomLabel)
                If e.Delta > 0 Then
                    MinuteHeight += ScrollAmount
                    WheelTimer.Stop()
                    WheelTimer.Start()
                Else
                    If MinuteHeight - ScrollAmount >= 0.1 Then
                        MinuteHeight -= ScrollAmount
                        WheelTimer.Stop()
                        WheelTimer.Start()
                    Else
                        MinuteHeight = 0.1
                        WheelTimer.Start()
                    End If
                End If
            End If
            WheelZoomLabel.Content = Math.Round(MinuteHeight / 3 * 100, 2) & "%"
        End If
    End Sub
    Private Sub InitializeWheelZoom()
        AddHandler WheelTimer.Tick, Sub()
                                        WheelTimer.Stop()
                                        If WheelZoomLabel.Parent IsNot Nothing Then
                                            TaskGrid.Children.Remove(WheelZoomLabel)
                                        End If
                                        loadScheduleControl()
                                    End Sub
        WheelZoomLabel.Width = TaskGrid.ActualWidth
        WheelZoomLabel.Height = TaskGrid.ActualHeight
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        If hideontaskbar = False Then
            notify.Dispose()
            End
        Else
            e.Cancel = True
            Me.Hide()
        End If
    End Sub

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        If menustate = False Then
            SideMenuOpen()
        Else
            SideMenuClose()
        End If
    End Sub

    Private Sub Border_MouseDown(sender As Object, e As MouseButtonEventArgs)

        ChangeSideMenuMode(SideMenuMode.List)
    End Sub

    Private Sub Border_MouseDown_1(sender As Object, e As MouseButtonEventArgs)

        ChangeSideMenuMode(SideMenuMode.Search)
    End Sub
    Sub ChangeSideMenuMode(menumode As SideMenuMode)
        Dim sb As Storyboard = FindResource("menuleave")
        Dim st As Storyboard = FindResource("menuclick")
        Storyboard.SetTarget(sb, searchbuttonborder)
        sb.Begin()
        Storyboard.SetTarget(sb, functionbuttonborder)
        sb.Begin()
        Storyboard.SetTarget(sb, settingbuttonborder)
        sb.Begin()
        Storyboard.SetTarget(sb, listbuttonborder)
        sb.Begin()
        If menumode = SideMenuMode.List Then
            Storyboard.SetTarget(st, listbuttonborder)
            sidemenufrm.Content = SideListPage
        ElseIf menumode = SideMenuMode.Search Then
            Storyboard.SetTarget(st, searchbuttonborder)
            sidemenufrm.Content = SideSearchPage
        ElseIf menumode = SideMenuMode.Setting Then
            Storyboard.SetTarget(st, settingbuttonborder)
            sidemenufrm.Content = SideSettingPage
        ElseIf menumode = SideMenuMode.Func Then
            Storyboard.SetTarget(st, functionbuttonborder)
            sidemenufrm.Content = SideFuncPage
        End If
        st.Begin()
    End Sub

    Enum SideMenuMode
        Search
        Func
        Setting
        List
    End Enum

    Private Sub userbuttonborder_MouseDown(sender As Object, e As MouseButtonEventArgs)

        ChangeSideMenuMode(SideMenuMode.Func)
    End Sub

    Private Sub settingbuttonborder_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles settingbuttonborder.MouseDown

        ChangeSideMenuMode(SideMenuMode.Setting)
    End Sub

    Private Sub functionbuttonborder_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles functionbuttonborder.MouseDown
        ChangeSideMenuMode(SideMenuMode.Func)
    End Sub

    Private Sub minbutton_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles minbutton.MouseDown
        WindowState = WindowState.Minimized
    End Sub
End Class
