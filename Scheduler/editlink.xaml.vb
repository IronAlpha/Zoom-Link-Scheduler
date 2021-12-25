Imports Newtonsoft.Json.Linq
Public Class editlink
    Dim mylink As LinkProperty
    Dim mymode As PageMode = PageMode.Add
    Enum PageMode
        Edit
        Add
    End Enum
    Private Sub Label_MouseDown_1(sender As Object, e As MouseButtonEventArgs)

        If titlebox.Text = "" Then
            BoolMsgbox("제목을 입력해 주세요.")
            Exit Sub
        End If

        If linkbox.Text = "" Then
            BoolMsgbox("링크를 입력해 주세요.")
            Exit Sub
        End If

        If mymode = PageMode.Edit Then
            With mylink
                .Title = titlebox.Text
                .Link = linkbox.Text
                .JObject("Title") = .Title
                .JObject("Link") = AES_Encrypt(.Link, MD5Hash(.ID & .Title))
            End With
        Else
            Dim lp As New LinkProperty
            Dim jo As New JObject
            With lp
                .Title = titlebox.Text
                .Link = linkbox.Text
                .SetID()
                .JObject = jo
                .JObject.Add("ID", .ID)
                .JObject.Add("Title", .Title)
                .JObject.Add("Link", AES_Encrypt(.Link, MD5Hash(.ID & .Title)))
            End With
            LinkPropertyList.Add(lp)
            LinkJson.Add(lp.ID, jo)
        End If
        SideListPage.loadpage()
        saveLinkJson()

        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.List)
        GC.Collect()
    End Sub
    Public Sub Loadzoom(lp As LinkProperty)
        clear()
        titlebox.Text = lp.Title
        linkbox.Text = lp.Link
        mylink = lp
        mymode = PageMode.Edit
        deletlab.Visibility = Visibility.Visible
    End Sub
    Public Sub clear()
        titlebox.Text = ""
        linkbox.Text = ""
        deletlab.Visibility = Visibility.Hidden
    End Sub

    Private Sub Label_MouseDown(sender As Object, e As MouseButtonEventArgs)
        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.List)
        GC.Collect()
    End Sub

    Private Sub Label_MouseDown_2(sender As Object, e As MouseButtonEventArgs)
        Dim lp = LinkControlReturn(mylink)
        CType(lp.Parent, WrapPanel).Children.Remove(lp)
        LinkJson.Remove(mylink.ID)
        LinkPropertyList.Remove(mylink)
        saveLinkJson()
        MainFrm.ChangeSideMenuMode(MainWindow.SideMenuMode.List)
    End Sub
End Class
