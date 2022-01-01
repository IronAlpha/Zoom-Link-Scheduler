Class infopage
    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        wp.Children.Clear()
        Dim txt As String = "줌 / 링크 자동 접속기
github.com/IronAlpha/Zoom-Link-Scheduler/
이메일:ironalph125@gmail.com

줌이나 링크를 시간에 맞춰서 자동으로 접속하게 해주는 프로그램입니다!
감사합니다!
"
        wp.Children.Add(New TextBlock With {.Text = txt, .MaxWidth = wp.ActualWidth, .Height = Double.NaN, .FontFamily = New FontFamily("나눔스퀘어_ac"), .FontSize = 12, .TextWrapping = TextWrapping.Wrap})
    End Sub
End Class
