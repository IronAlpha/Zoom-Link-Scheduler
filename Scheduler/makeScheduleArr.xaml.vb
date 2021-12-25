Imports System.Globalization
Imports Newtonsoft.Json.Linq

Public Class makeScheduleArr
    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Dim n As New classcontrol
        With n
            .numlab.Content = ""
        End With
        wp.Children.Add(n)
    End Sub

    Private Sub Border_MouseDown_1(sender As Object, e As MouseButtonEventArgs)
        For Each cc As classcontrol In wp.Children
            Dim StartValue, EndValue As DateTime
            Dim format() As String = {"H:m"}
            If DateTime.TryParseExact(cc.startt.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, StartValue) = False Then
                MsgBox("시간 형식으로 입력해야 합니다.")
                Exit Sub
            End If
            If DateTime.TryParseExact(cc.endt.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, EndValue) = False Then
                MsgBox("시간 형식으로 입력해야 합니다.")
                Exit Sub
            End If
        Next

        For Each cc As classcontrol In wp.Children
            Dim StartValue, EndValue As DateTime
            Dim format() As String = {"H:m"}
            If DateTime.TryParseExact(cc.startt.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, StartValue) Then
                If DateTime.TryParseExact(cc.endt.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, EndValue) Then
                    With cc
                        Dim togGroup() As Primitives.ToggleButton = { .suntog, .montog, .tuetog, .wedtog, .thutog, .fritog, .sattog}
                        Dim TbGroup() As TextBox = { .suntitle, .montitle, .tuetitle, .wedtitle, .thutitle, .frititle, .sattitle}
                        For i As Integer = 0 To 6
                            If togGroup(i).IsChecked = True Then
                                Dim TaskProp As New TaskProperty
                                With TaskProp
                                    .DayOfWeek = i
                                    .Title = TbGroup(i).Text
                                    .StartTime = StartValue
                                    .EndTime = EndValue
                                    Dim c = Color.FromRgb(Rand.Next(0, 255), Rand.Next(0, 255), Rand.Next(0, 255))
                                    .ColorHex = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")

                                    .SetDuration()
                                    .SetID()
                                End With

                                Dim TaskJObject As New JObject()
                                With TaskJObject
                                    .Add("ID", TaskProp.ID)
                                    .Add("Title", TaskProp.Title)
                                    .Add("DOW", TaskProp.DayOfWeek)
                                    .Add("StartTime", TaskProp.StartTime.ToString("HH:mm"))
                                    .Add("EndTime", TaskProp.EndTime.ToString("HH:mm"))
                                    .Add("Duration", TaskProp.Duration)
                                    .Add("Color", TaskProp.ColorHex)
                                End With
                                ScheduleJson.Add(TaskProp.ID, TaskJObject)
                            End If
                        Next
                    End With
                Else
                    Exit For
                End If
            End If
        Next
        saveTaskJson()
    End Sub
End Class
