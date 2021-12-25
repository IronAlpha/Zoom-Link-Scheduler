Class settingpage
    Private Sub ToggleButton_Click(sender As Object, e As RoutedEventArgs)
        If CType(sender, Primitives.ToggleButton).IsChecked = True Then
            HourFormat = HourFormatEnum.Use24
        Else
            HourFormat = HourFormatEnum.Use12
        End If
        ModifySetting("HourFormat", HourFormat)
        With MainFrm
            .loadScheduleControl()
            .loadSideControl()
        End With
    End Sub

    Private Sub ToggleButton_Click_1(sender As Object, e As RoutedEventArgs)
        If CType(sender, Primitives.ToggleButton).IsChecked = True Then
            DisplayLanguage = EnumModule.Language.English
        Else
            DisplayLanguage = EnumModule.Language.Korean
        End If
        ModifySetting("DisplayLanguage", DisplayLanguage)
        With MainFrm
            .setDatebox()
            .LoadTaskTopDate()
            .loadScheduleControl()
            .loadSideControl()
        End With
    End Sub

    Private Sub ToggleButton_Click_2(sender As Object, e As RoutedEventArgs)
        AutoStartZoom = CType(sender, Primitives.ToggleButton).IsChecked
        ModifySetting("AutoStartZoom", AutoStartZoom)
    End Sub

    Private Sub ToggleButton_Click_3(sender As Object, e As RoutedEventArgs)
        AutoStartLink = CType(sender, Primitives.ToggleButton).IsChecked
        ModifySetting("AutoStartLink", AutoStartLink)
    End Sub

    Private Sub ToggleButton_Click_4(sender As Object, e As RoutedEventArgs)
        If CType(sender, Primitives.ToggleButton).IsChecked = True Then
            LoadLine = TaskGridLineEnum.HorizontalOnly
        Else
            LoadLine = TaskGridLineEnum.None
        End If
        With MainFrm
            .loadScheduleControl()
        End With
        ModifySetting("LoadLine", LoadLine)
    End Sub

    Private Sub ToggleButton_Click_5(sender As Object, e As RoutedEventArgs)
        hideontaskbar = CType(sender, Primitives.ToggleButton).IsChecked
        ModifySetting("HideOnTaskbar", hideontaskbar)
    End Sub

    Private Sub ToggleButton_Click_6(sender As Object, e As RoutedEventArgs)
        If CType(sender, Primitives.ToggleButton).IsChecked = True Then
            LoadLineTask = TaskGridLineEnum.HorizontalOnly
        Else
            LoadLineTask = TaskGridLineEnum.None
        End If
        With MainFrm
            .loadScheduleControl()
        End With
        ModifySetting("LoadLineTask", LoadLineTask)
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        autozoomtog.IsChecked = AutoStartZoom
        autolinktog.IsChecked = AutoStartLink
        If HourFormat = HourFormatEnum.Use24 Then use24tog.IsChecked = True
        If DisplayLanguage = EnumModule.Language.English Then disEngtog.IsChecked = True
        If LoadLine = TaskGridLineEnum.HorizontalOnly Then usehortimeseptog.IsChecked = True
        If LoadLineTask = TaskGridLineEnum.HorizontalOnly Then taskseptog.IsChecked = True
        hidetskbtog.IsChecked = hideontaskbar
        useshadowtog.IsChecked = UseShadow
        autostartdialogtog.IsChecked = AlertBeforeStart


        namelab.Content = uname
        autominlab.Content = AutoStartMin & "분 전부터"
        updatelab.Content = Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
    End Sub

    Private Sub Border_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Dim i = InputBox("자동 시작 시간 바꾸기", "줌/링크가 몇분 전에 자동으로 시작될지 설정합니다.", AutoStartMin)
        If Not i = Nothing Then AutoStartMin = Int(i)
        ModifySetting("AutoStartMin", AutoStartMin)
        Page_Loaded(sender, Nothing)
    End Sub

    Private Sub Border_MouseDown_1(sender As Object, e As MouseButtonEventArgs)
        Dim t = InputBox("이름 바꾸기", "줌 입장시 사용할 이름을 변경합니다.", uname)
        If Not t = Nothing Then uname = t
        ModifySetting("UserName", uname)
        Page_Loaded(sender, Nothing)
        MainFrm.UserLabel.Content = uname
    End Sub

    Private Sub ToggleButton_Click_7(sender As Object, e As RoutedEventArgs)
        UseShadow = CType(sender, Primitives.ToggleButton).IsChecked
        ModifySetting("UseShadow", UseShadow)
        With MainFrm
            .loadScheduleControl()
        End With
    End Sub

    Private Sub autostartdialogtog_Click(sender As Object, e As RoutedEventArgs) Handles autostartdialogtog.Click
        AlertBeforeStart = CType(sender, Primitives.ToggleButton).IsChecked
        ModifySetting("AlertBeforeStart", AlertBeforeStart)
    End Sub

    Private Sub Border_MouseDown_2(sender As Object, e As MouseButtonEventArgs)
        MainFrm.sidemenufrm.Content = SideInfoPage
    End Sub
End Class
