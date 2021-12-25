Imports System.Windows.Media.Animation

Public Class itemlist
    Dim isSearchMode As Boolean = False
    Dim myMode As ItemListMode
    Dim myTask As TaskProperty
    Dim myZoom As ZoomProperty
    Dim myLink As LinkProperty
    Public Sub ZoomCompose(ZoomProp As ZoomProperty)
        iconbox.Source = New BitmapImage(New Uri("pack://application:,,,/imagesource/zoomicon.png"))
        titlebox.Content = ZoomProp.Title
        myMode = ItemListMode.Zoom
        myZoom = ZoomProp
    End Sub
    Public Sub LinkCompose(Linkprop As LinkProperty)
        iconbox.Source = New BitmapImage(New Uri("pack://application:,,,/imagesource/linked.png"))
        titlebox.Content = Linkprop.Title
        myMode = ItemListMode.Link
        myLink = Linkprop
    End Sub
    Public Sub TaskCompose(TaskProp As TaskProperty)
        iconbox.Source = New BitmapImage(New Uri("pack://application:,,,/imagesource/checked.png"))
        titlebox.Content = TaskProp.Title
        myMode = ItemListMode.Schedule
        myTask = TaskProp
    End Sub
    Public Sub descriptionCompose(txt As String)
        titlebox.Measure(New Size(Double.PositiveInfinity, Double.PositiveInfinity))
        descriptionlabel.Content = txt
        descriptionlabel.Margin = New Thickness(titlebox.DesiredSize.Width + 5, 0, 0, 0)
    End Sub
    Public Sub SearchCompose()
        Width = SideSearchPage.searchlist.ActualWidth
        Height = 50
        titlebox.FontSize = 15
        isSearchMode = True
    End Sub

    Private Sub UserControl_MouseEnter(sender As Object, e As MouseEventArgs)
        Dim sb As Storyboard
        If isSearchMode = False Then
            sb = FindResource("enter")
        Else
            sb = FindResource("searchenter")
        End If
        Storyboard.SetTarget(sb, titlebox)
        sb.Begin()
    End Sub

    Private Sub UserControl_MouseLeave(sender As Object, e As MouseEventArgs)
        Dim sb As Storyboard
        If isSearchMode = False Then
            sb = FindResource("leave")
        Else
            sb = FindResource("searchleave")
        End If
        Storyboard.SetTarget(sb, titlebox)
        sb.Begin()
    End Sub

    Private Sub userControl_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles userControl.MouseDown
        If myMode = ItemListMode.Schedule Then
            GoTo 1
            Dim sb As Storyboard = MainFrm.FindResource("menuhover")
            Storyboard.SetTarget(sb, ScheduleControlReturn(myTask).BaseBorder)
            sb.Begin()
            AddHandler sb.Completed, Sub()
                                         Dim st As Storyboard = MainFrm.FindResource("menuleave")
                                         Storyboard.SetTarget(st, ScheduleControlReturn(myTask).BaseBorder)
                                         st.Begin()
                                     End Sub
            MainFrm.TaskSCRV.ScrollToVerticalOffset(ScheduleControlReturn(myTask).Margin.Top - 300)
1:
            Dim yn As New yesnodialog
            If myTask.ZoomProp IsNot Nothing Then
                Dim it As New itemlist
                it.ZoomCompose(myTask.ZoomProp)
                it.VerticalAlignment = VerticalAlignment.Top
                yn.listpanel.Children.Add(it)
            End If
            If myTask.LinkProp IsNot Nothing Then
                Dim it As New itemlist
                it.LinkCompose(myTask.LinkProp)
                it.VerticalAlignment = VerticalAlignment.Top
                yn.listpanel.Children.Add(it)
            End If
            yn.ShowDialog()
        ElseIf myMode = ItemListMode.Zoom Then
                MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.List)
                ZoomControlReturn(myZoom).BringIntoView()
                Dim sb As Storyboard = MainFrm.FindResource("menuhover")
                Storyboard.SetTarget(sb, ZoomControlReturn(myZoom).baseborder)
                sb.Begin()
                AddHandler sb.Completed, Sub()
                                             Dim st As Storyboard = MainFrm.FindResource("menuleave")
                                             Storyboard.SetTarget(st, ZoomControlReturn(myZoom).baseborder)
                                             st.Begin()
                                         End Sub
            ElseIf myMode = ItemListMode.Link Then
                MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.List)
            LinkControlReturn(myLink).BringIntoView()
            Dim sb As Storyboard = MainFrm.FindResource("menuhover")
            Storyboard.SetTarget(sb, LinkControlReturn(myLink).baseborder)
            sb.Begin()
            AddHandler sb.Completed, Sub()
                                         Dim st As Storyboard = MainFrm.FindResource("menuleave")
                                         Storyboard.SetTarget(st, LinkControlReturn(myLink).baseborder)
                                         st.Begin()
                                     End Sub
        End If
    End Sub
End Class
