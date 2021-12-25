Imports Newtonsoft.Json.Linq

Public Class Transfer
    Public saveScheduleJson As New JObject
    Public saveZoomlistJson As New JObject
    Public saveLinkJson As New JObject
    Public saveConfigureJson As New JObject
    Public savejsonlist() As String = {saveSchedulejsonPath, savezoomlistjsonPath, saveconfigurejsonPath, savelinkjsonPath}

    Public core As String = Environ("appdata") & "\Schedule\core"
    Public root As String = Environ("appdata") & "\Schedule\Json"
    Public SchedulejsonPath As String = root & "\ScheduleJson.json"
    Public zoomlistjsonPath As String = root & "\ZoomJson.json"
    Public linkjsonPath As String = root & "\LinkJson.json"
    Public configurejsonPath As String = root & "\ConfigureJson.json"

    Public savecore As String = Environ("appdata") & "\Scheduler\core"
    Public saveroot As String = Environ("appdata") & "\Scheduler\Json"
    Public saveSchedulejsonPath As String = saveroot & "\ScheduleJson.json"
    Public savezoomlistjsonPath As String = saveroot & "\ZoomJson.json"
    Public savelinkjsonPath As String = saveroot & "\LinkJson.json"
    Public saveconfigurejsonPath As String = saveroot & "\ConfigureJson.json"
    Public Function ZoomContain(title As String) As Boolean
        If title = "" Then
            Return False
            Exit Function
        End If
        For Each zp As ZoomProperty In ZoomPropertyList
            If zp.Title = title Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
    Public Function ZoomReturn(title As String) As ZoomProperty
        If title = "" Then
            Return Nothing
            Exit Function
        End If
        For Each zp As ZoomProperty In ZoomPropertyList
            If zp.Title = title Then
                Return zp
                Exit Function
            End If
        Next
        Return Nothing
    End Function
    Public Function LinkContain(title As String) As Boolean
        If title = "" Then
            Return False
            Exit Function
        End If
        For Each zp As LinkProperty In LinkPropertyList
            If zp.Title = title Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
    Public Function LinkReturn(title As String) As LinkProperty
        If title = "" Then
            Return Nothing
            Exit Function
        End If
        For Each zp As LinkProperty In LinkPropertyList
            If zp.Title = title Then
                Return zp
                Exit Function
            End If
        Next
        Return Nothing
    End Function
    Public Sub getlist()
        ScheduleJson = JObject.Parse(My.Computer.FileSystem.ReadAllText(SchedulejsonPath, Text.Encoding.UTF8))
        ZoomlistJson = JObject.Parse(My.Computer.FileSystem.ReadAllText(zoomlistjsonPath, Text.Encoding.UTF8))
        LinkJson = JObject.Parse(My.Computer.FileSystem.ReadAllText(linkjsonPath, Text.Encoding.UTF8))
        ConfigureJson = JObject.Parse(My.Computer.FileSystem.ReadAllText(configurejsonPath, Text.Encoding.UTF8))

        TaskPropertyList.Clear()
        ZoomPropertyList.Clear()
        LinkPropertyList.Clear()

        For Each Zprop As JProperty In ZoomlistJson.Properties
            Dim ZoomObject As JObject = Zprop.First
            Dim ZoomProp As New ZoomProperty
            Dim getTaskValue As New JsonValue With {.Jobject = ZoomObject}
            With ZoomProp
                .JObject = ZoomObject
                .JProperty = Zprop

                .Title = getTaskValue.getValue("title")
                .Link = getTaskValue.getValue("link")
                .RoomCode = getTaskValue.getValue("code")
                .Password = getTaskValue.getValue("pwd")
                .SetID()
            End With
            ZoomPropertyList.Add(ZoomProp)
        Next

        For Each Lprop As JProperty In LinkJson.Properties
            Dim LinkObject As JObject = Lprop.First
            Dim LinkProp As New LinkProperty
            Dim getTaskValue As New JsonValue With {.Jobject = LinkObject}
            With LinkProp
                .JObject = LinkObject
                .JProperty = Lprop

                .Title = getTaskValue.getValue("title")
                .Link = getTaskValue.getValue("link")
                .SetID()
            End With
            LinkPropertyList.Add(LinkProp)
        Next

        For Each Tprop As JProperty In ScheduleJson.Properties
            Dim TaskObject As JObject = Tprop.First
            Dim TaskProp As New TaskProperty
            Dim getTaskValue As New JsonValue With {.Jobject = TaskObject}
            With TaskProp
                .JObject = TaskObject
                .JProperty = Tprop

                .Title = getTaskValue.getValue("title")
                .StartTime = Convert.ToDateTime(getTaskValue.getValue("starttime"))
                .EndTime = Convert.ToDateTime(getTaskValue.getValue("endtime"))
                .DayOfWeek = DayOfWeek_s_s_EN.ToList.IndexOf(getTaskValue.getValue("dayofweek"))
                If getTaskValue.getValue("zoomtitle") IsNot Nothing Then
                    If ZoomContain(getTaskValue.getValue("zoomtitle").ToString) = True Then
                        .ZoomProp = ZoomReturn(getTaskValue.getValue("zoomtitle").ToString)
                    Else
                        .ZoomProp = Nothing
                    End If
                End If
                If getTaskValue.getValue("linktitle") IsNot Nothing Then
                    If LinkContain(getTaskValue.getValue("linktitle").ToString) = True Then
                        .LinkProp = LinkReturn(getTaskValue.getValue("linktitle").ToString)
                    Else
                        .LinkProp = Nothing
                    End If
                End If
                If isColorHex(getTaskValue.getValue("colorhash")) And getTaskValue.getValue("colorhash") <> Nothing Then
                    .ColorHex = getTaskValue.getValue("colorhash")
                Else
                    Dim c = Color.FromRgb(Rand.Next(0, 255), Rand.Next(0, 255), Rand.Next(0, 255))
                    .ColorHex = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2") ' Nothing
                End If
                .SetDuration()
                .SetID()
            End With
            TaskPropertyList.Add(TaskProp)
        Next
    End Sub

    Public Sub MakeNewJObject()
        For Each TaskProp As TaskProperty In TaskPropertyList
            Dim TaskJObject As New JObject()
            With TaskJObject
                .Add("ID", TaskProp.ID)
                .Add("Title", TaskProp.Title)
                .Add("DOW", TaskProp.DayOfWeek)
                .Add("StartTime", TaskProp.StartTime.ToString("HH:mm"))
                .Add("EndTime", TaskProp.EndTime.ToString("HH:mm"))
                .Add("Duration", TaskProp.Duration)
                .Add("Color", TaskProp.ColorHex)
                If TaskProp.ZoomProp IsNot Nothing Then
                    .Add("ZoomID", TaskProp.ZoomProp.ID)
                Else
                    .Add("ZoomID", Nothing)
                End If
                If TaskProp.LinkProp IsNot Nothing Then
                    .Add("LinkID", TaskProp.LinkProp.ID)
                Else
                    .Add("LinkID", Nothing)
                End If
            End With
            saveScheduleJson.Add(TaskProp.ID, TaskJObject)
        Next

        For Each ZoomProp As ZoomProperty In ZoomPropertyList
            Dim ZoomJObject As New JObject()
            With ZoomJObject
                .Add("ID", ZoomProp.ID)
                .Add("Title", ZoomProp.Title)
                .Add("Code", AES_Encrypt(ZoomProp.RoomCode, MD5Hash(ZoomProp.ID & ZoomProp.Title)))
                .Add("Pass", AES_Encrypt(ZoomProp.Password, MD5Hash(ZoomProp.ID & ZoomProp.Title)))
                .Add("Link", AES_Encrypt(ZoomProp.Link, MD5Hash(ZoomProp.ID & ZoomProp.Title)))
            End With
            saveZoomlistJson.Add(ZoomProp.ID, ZoomJObject)
        Next

        For Each LinkProp As LinkProperty In LinkPropertyList
            Dim LinkJObject As New JObject()
            With LinkJObject
                .Add("ID", LinkProp.ID)
                .Add("Title", LinkProp.Title)
                .Add("Link", AES_Encrypt(LinkProp.Link, MD5Hash(LinkProp.ID & LinkProp.Title)))
            End With
            saveLinkJson.Add(LinkProp.ID, LinkJObject)
        Next

        Dim ConfigObject As JObject = ConfigureJson
        Dim getValue As New JsonValue With {.Jobject = ConfigObject}
        With saveConfigureJson
            .Add("UserName", getValue.getValue("uname"))
            .Add("ViewScale", getValue.getValue("viewscale"))
        End With
    End Sub
    Public Sub SaveNewJObject()
        My.Computer.FileSystem.WriteAllText(saveSchedulejsonPath, saveScheduleJson.ToString, False)
        My.Computer.FileSystem.WriteAllText(savezoomlistjsonPath, saveZoomlistJson.ToString, False)
        My.Computer.FileSystem.WriteAllText(savelinkjsonPath, saveLinkJson.ToString, False)
        My.Computer.FileSystem.WriteAllText(saveconfigurejsonPath, saveConfigureJson.ToString, False)
    End Sub
End Class
