Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq

Class editzoom
    Dim myzoom As ZoomProperty
    Dim mymode As PageMode = PageMode.Add
    Enum PageMode
        Edit
        Add
    End Enum
    Public Sub Loadzoom(zp As ZoomProperty)
        clear()
        titlebox.Text = zp.Title
        grcodebox.Text = zp.RoomCode
        passbox.Text = zp.Password
        linkbox.Text = zp.Link
        myzoom = zp

        makeLinkbox()
        mymode = PageMode.Edit
        deletlab.Visibility = Visibility.Visible
    End Sub
    Public Sub clear()
        titlebox.Text = ""
        grcodebox.Text = ""
        passbox.Text = ""
        linkbox.Text = ""
        deletlab.Visibility = Visibility.Hidden
    End Sub
    Sub makeLinkbox()
        Dim regex As New Regex("[0-9]*" & "?pwd=" & "[A-Za-z0-9]*$")
        If regex.Match(linkbox.Text).Success Then
            Try
                grcodebox.Text = Between(linkbox.Text, "/j/", "?pwd=").ToString.Trim
                passbox.Text = Split(linkbox.Text, "?pwd=", 2)(1).ToString.Trim
                grcodebox.IsEnabled = False
                passbox.IsEnabled = False
            Catch ex As Exception
                grcodebox.IsEnabled = True
                passbox.IsEnabled = True
            End Try
        Else
            grcodebox.IsEnabled = True
            passbox.IsEnabled = True
        End If
    End Sub
    Private Sub Label_MouseDown(sender As Object, e As MouseButtonEventArgs)
        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.List)
        GC.Collect()
    End Sub

    Private Sub Label_MouseDown_1(sender As Object, e As MouseButtonEventArgs)
        If titlebox.Text = "" Then
            BoolMsgbox("제목을 입력해 주세요.")
            Exit Sub
        End If

        If grcodebox.Text = "" Then
            BoolMsgbox("코드를 입력해 주세요.")
            Exit Sub
        End If

        If passbox.Text = "" Then
            BoolMsgbox("비밀번호를 입력해 주세요.")
            Exit Sub
        End If

        If mymode = PageMode.Edit Then
            With myzoom
                .Title = titlebox.Text
                .RoomCode = grcodebox.Text
                .Password = passbox.Text
                .Link = linkbox.Text
                .JObject("Title") = .Title
                .JObject("Link") = AES_Encrypt(.Link, MD5Hash(.ID & .Title))
                .JObject("Code") = AES_Encrypt(.RoomCode, MD5Hash(.ID & .Title))
                .JObject("Pass") = AES_Encrypt(.Password, MD5Hash(.ID & .Title))
            End With
        Else
            Dim zp As New ZoomProperty
            Dim jo As New JObject
            With zp
                .Title = titlebox.Text
                .RoomCode = grcodebox.Text
                .Password = passbox.Text
                .Link = linkbox.Text
                .SetID()
                .JObject = jo
                .JObject.Add("ID", .ID)
                .JObject.Add("Title", .Title)
                .JObject.Add("Link", AES_Encrypt(.Link, MD5Hash(.ID & .Title)))
                .JObject.Add("Code", AES_Encrypt(.RoomCode, MD5Hash(.ID & .Title)))
                .JObject.Add("Pass", AES_Encrypt(.Password, MD5Hash(.ID & .Title)))
            End With
            ZoomPropertyList.Add(zp)
            ZoomlistJson.Add(zp.ID, jo)
        End If
        SideListPage.loadpage()
        saveZoomJson()

        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.List)
        GC.Collect()
    End Sub

    Private Sub linkbox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles linkbox.TextChanged
        makeLinkbox()
    End Sub

    Private Sub Label_MouseDown_2(sender As Object, e As MouseButtonEventArgs)
        Dim zp = ZoomControlReturn(myzoom)
        CType(zp.Parent, WrapPanel).Children.Remove(zp)
        ZoomlistJson.Remove(myzoom.ID)
        ZoomPropertyList.Remove(myzoom)
        saveZoomJson()
        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.List)
    End Sub
End Class
