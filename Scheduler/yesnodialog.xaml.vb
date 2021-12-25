Public Class yesnodialog
    Private Sub Border_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.DialogResult = False
        Close()
    End Sub

    Private Sub Border_MouseDown_1(sender As Object, e As MouseButtonEventArgs)
        Me.DialogResult = True
        Close()
    End Sub
End Class
