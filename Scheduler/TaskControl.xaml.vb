Public Class TaskControl
    Public myTaskProp
    Public Sub LoadTask(TaskProp As TaskProperty)

        HorizontalAlignment = HorizontalAlignment.Left
        VerticalAlignment = VerticalAlignment.Top
        Width = ScheduleWidth - 20
        Height = getTop(TaskProp.Duration)
        titlebox.Content = TaskProp.Title
        If TaskProp.ColorHex = Nothing Then
            BaseBorder.BorderBrush = New SolidColorBrush(Color.FromRgb(Rand.Next(0, 255), Rand.Next(0, 255), Rand.Next(0, 255)))
        Else
            BaseBorder.BorderBrush = New SolidColorBrush(ColorConverter.ConvertFromString(TaskProp.ColorHex))
        End If
        Margin = New Thickness(10 + ScheduleWidth * (TaskProp.DayOfWeek + 1), getTop(TaskProp.StartTime.TimeOfDay), 0, 0)
        titlebox.Effect = Nothing
        myTaskProp = TaskProp
    End Sub
    Public Sub KillSelf()
        CType(Parent, Grid).Children.Remove(Me)
    End Sub
End Class
