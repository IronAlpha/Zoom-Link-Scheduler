Public Class classcontrol
    Private Sub suntog_Click(sender As Object, e As RoutedEventArgs) Handles suntog.Click
        suntitle.Visibility = getvisb(CType(sender, Primitives.ToggleButton).IsChecked)
    End Sub

    Private Sub montog_Click(sender As Object, e As RoutedEventArgs) Handles montog.Click
        montitle.Visibility = getvisb(CType(sender, Primitives.ToggleButton).IsChecked)
    End Sub

    Private Sub tuetog_Click(sender As Object, e As RoutedEventArgs) Handles tuetog.Click
        tuetitle.Visibility = getvisb(CType(sender, Primitives.ToggleButton).IsChecked)
    End Sub

    Private Sub wedtog_Click(sender As Object, e As RoutedEventArgs) Handles wedtog.Click
        wedtitle.Visibility = getvisb(CType(sender, Primitives.ToggleButton).IsChecked)
    End Sub

    Private Sub thutog_Click(sender As Object, e As RoutedEventArgs) Handles thutog.Click
        thutitle.Visibility = getvisb(CType(sender, Primitives.ToggleButton).IsChecked)
    End Sub

    Private Sub fritog_Click(sender As Object, e As RoutedEventArgs) Handles fritog.Click
        frititle.Visibility = getvisb(CType(sender, Primitives.ToggleButton).IsChecked)
    End Sub

    Private Sub sattog_Click(sender As Object, e As RoutedEventArgs) Handles sattog.Click
        sattitle.Visibility = getvisb(CType(sender, Primitives.ToggleButton).IsChecked)
    End Sub
    Function getvisb(bool As Boolean) As Visibility
        If bool = True Then
            Return Visibility.Visible
        Else
            Return Visibility.Hidden
        End If
    End Function

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        CType(Parent, WrapPanel).Children.Remove(Me)
    End Sub

End Class
