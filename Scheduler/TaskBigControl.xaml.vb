Imports System.Windows.Media.Animation

Public Class TaskBigControl
    Dim Prop As TaskProperty
    Public clicked As Boolean = False
    Public entered As Boolean = False
    Dim ShowStory As Storyboard
    Dim HideStory As Storyboard
    Dim CloseStory As Storyboard

    Public Sub Created()
        ShowStory = FindResource("fadein")
        HideStory = FindResource("fadeout")
        CloseStory = FindResource("fadeout")

        Storyboard.SetTarget(ShowStory, Me)
        Storyboard.SetTarget(HideStory, Me)
        Storyboard.SetTarget(CloseStory, Me)

        AddHandler CloseStory.Completed, AddressOf KillSelf
    End Sub
    Public Sub KillSelf()
        If FrmScheduleGrid.Children.Contains(Me) Then FrmScheduleGrid.Children.Remove(Me)
    End Sub

    Public Sub Compose(TaskProp As TaskProperty)
        Dim c As Integer = 0
        Prop = TaskProp
        titlebox.Content = TaskProp.Title
        timelab.Content = TaskProp.getDayofweek(DisplayLanguage) & " " & TaskProp.getTime(HourFormat) & " - " & TaskProp.getTime(HourFormat, True)
        baseborder.BorderBrush = New SolidColorBrush(ColorConverter.ConvertFromString(TaskProp.ColorHex))

        If TaskProp.ZoomProp IsNot Nothing Then
            If Not TaskProp.ZoomProp.ID = "" Then
                zoomborder.Visibility = Visibility.Visible
                zoomlab.Content = TaskProp.ZoomProp.Title
            Else
                zoomborder.Visibility = Visibility.Collapsed
                c += 1
            End If
        Else
            zoomborder.Visibility = Visibility.Collapsed
            c += 1
        End If
        If TaskProp.LinkProp IsNot Nothing Then
            If Not TaskProp.LinkProp.ID = "" Then
                linkborder.Visibility = Visibility.Visible
                linklab.Content = TaskProp.LinkProp.Title
            Else
                linkborder.Visibility = Visibility.Collapsed
                c += 1
            End If
        Else
                linkborder.Visibility = Visibility.Collapsed
            c += 1
        End If
        Height = 200 - c * 36
    End Sub
    Public Sub SideCompose()
        baseborder.CornerRadius = New CornerRadius(5)
        baseborder.BorderThickness = New Thickness(1, 1, 1, 1)
        Margin = New Thickness(2, 0, 0, 0)
        baseborder.Effect = Nothing
        baseborder.BorderBrush = New SolidColorBrush(Color.FromRgb(230, 230, 230))
    End Sub

    Private Sub linkborder_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles linkborder.MouseDown
        openLink(Prop.LinkProp)
    End Sub

    Private Sub zoomborder_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles zoomborder.MouseDown
        joinZoom(Prop.ZoomProp)
    End Sub

    Public Sub Show()
        ShowStory.Begin()
    End Sub
    Public Sub Hide()
        HideStory.Begin()
    End Sub
    Public Sub Close()
        CloseStory.Begin()
    End Sub
    Private Sub Border_MouseDown(sender As Object, e As MouseButtonEventArgs)
        eds.loadtask(Prop)
        MainFrm.sidemenufrm.Content = eds
        MainFrm.SideMenuOpen()
    End Sub
End Class
