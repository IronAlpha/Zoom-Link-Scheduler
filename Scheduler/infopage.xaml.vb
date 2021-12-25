Class infopage
    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        wp.Children.Clear()
        wp.Children.Add(New TextBlock With {.Text = My.Resources.nanumlicense, .MaxWidth = wp.ActualWidth, .Height = Double.NaN, .FontFamily = New FontFamily("나눔스퀘어_ac"), .FontSize = 12, .TextWrapping = TextWrapping.Wrap})
    End Sub
End Class
