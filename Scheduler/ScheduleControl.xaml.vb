Public Class ScheduleControl
    Public Sub add(l As Object)
        BaseGrid.Children.Add(l)
    End Sub

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        Height = 1440 * MinuteHeight
    End Sub
End Class
