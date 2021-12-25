Imports System.Windows.Media.Animation
Imports Newtonsoft.Json.Linq

Public Class zoomlist
    Public link, code, pwd As String
    Public jProp As JProperty
    Public jsonProp As JToken

    Public MyZoomProp As ZoomProperty
    Public MyLinkProp As LinkProperty
    Public MyMode As ItemListMode

    Public Sub ZoomLoad(zp As ZoomProperty)
        MyZoomProp = zp
        title.Content = zp.Title
        If zp.Link <> "" Then
            link = zp.Link
        End If
        pwd = zp.Password
        code = zp.RoomCode
        jProp = zp.JProperty
        jsonProp = zp.JObject
        iconbox.Source = New BitmapImage(New Uri("pack://application:,,,/imagesource/zoomicon.png"))
        MyMode = ItemListMode.Zoom
    End Sub
    Public Sub LinkLoad(lp As LinkProperty)
        MyLinkProp = lp
        title.Content = lp.Title
        jProp = lp.JProperty
        jsonProp = lp.JObject

        joinButton.Content = "열기"
        iconbox.Source = New BitmapImage(New Uri("pack://application:,,,/imagesource/linked.png"))
        MyMode = ItemListMode.Link
    End Sub

    Private Sub UserControl_MouseEnter(sender As Object, e As MouseEventArgs)
        Dim sb As Storyboard = Me.FindResource("fadeinmenu")
        Storyboard.SetTarget(sb, menuborder)
        sb.Begin()
    End Sub


    Private Sub editButton_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles editButton.MouseDown
        If MyMode = ItemListMode.Zoom Then
            edz.Loadzoom(MyZoomProp)
            MainFrm.sidemenufrm.Content = edz
            MainFrm.SideMenuOpen()
        ElseIf MyMode = ItemListMode.Link Then
            edl.Loadzoom(MyLinkProp)
            MainFrm.sidemenufrm.Content = edl
            MainFrm.SideMenuOpen()
        End If
    End Sub

    Private Sub joinButton_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles joinButton.MouseDown
        If MyMode = ItemListMode.Zoom Then
            joinZoom(MyZoomProp)
        Else
            openLink(MyLinkProp)
        End If
    End Sub

    Private Sub UserControl_MouseLeave(sender As Object, e As MouseEventArgs)
        Dim sb As Storyboard = Me.FindResource("fadeoutmenu")
        Storyboard.SetTarget(sb, menuborder)
        sb.Begin()
    End Sub
End Class
