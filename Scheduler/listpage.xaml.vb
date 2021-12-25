Class listpage
    Sub loadpage()
        wp.Children.Clear()

        For Each zp As ZoomProperty In ZoomPropertyList
            Dim zl As New zoomlist With {.Width = 272}
            zl.ZoomLoad(zp)
            wp.Children.Add(zl)
        Next
        For Each lp As LinkProperty In LinkPropertyList
            Dim zl As New zoomlist With {.Width = 272}
            zl.LinkLoad(lp)
            wp.Children.Add(zl)
        Next
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        ' loadpage()
    End Sub
End Class
