Public Class InputboxFrm
    Private Sub closebutton_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles closebutton.MouseDown
        Me.DialogResult = False
        Close()
    End Sub

    Private Sub inputtextbox_KeyDown(sender As Object, e As KeyEventArgs) Handles inputtextbox.KeyDown
        If e.Key = Key.Return Then
            Me.DialogResult = True
            Close()
        End If
    End Sub
End Class
