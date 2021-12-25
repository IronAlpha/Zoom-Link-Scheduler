Imports System.Text
Imports System.Windows.Media.Animation
Imports Newtonsoft.Json.Linq

Public Module sys
    Public MainFrm As MainWindow
    Public SideSearchPage As searchpage
    Public SideListPage As listpage
    Public SideSettingPage As settingpage
    Public SideFuncPage As funcpage
    Public SideInfoPage As infopage

    Public Rand As New Random
    Public FrmScheduleGrid As Grid

    Public DisplayLanguage As Language = Language.English
    Public HourFormat As HourFormatEnum = HourFormatEnum.Use24

    Public ScheduleJson As JObject
    Public ZoomlistJson As JObject
    Public LinkJson As JObject
    Public ConfigureJson As JObject
    Public jsonlist() As String = {SchedulejsonPath, zoomlistjsonPath, configurejsonPath, linkjsonPath}

    Public core As String = Environ("appdata") & "\Scheduler\core"
    Public root As String = Environ("appdata") & "\Scheduler\Json"
    Public SchedulejsonPath As String = root & "\ScheduleJson.json"
    Public zoomlistjsonPath As String = root & "\ZoomJson.json"
    Public linkjsonPath As String = root & "\LinkJson.json"
    Public configurejsonPath As String = root & "\ConfigureJson.json"

    Public TaskPropertyList As New List(Of TaskProperty)
    Public ZoomPropertyList As New List(Of ZoomProperty)
    Public LinkPropertyList As New List(Of LinkProperty)
    Public TodayList As New List(Of TaskProperty)

    Public Property ScheduleWidth As Double = 120
    Public Property MinuteHeight As Double = 3
    Public Property uname As String
    Public Property LoadLine As TaskGridLineEnum = TaskGridLineEnum.HorizontalOnly
    Public Property LoadLineTask As TaskGridLineEnum = TaskGridLineEnum.HorizontalOnly
    Public Property AutoStartMin As Double = 3
    Public Property AutoStartZoom As Boolean = True
    Public Property AutoStartLink As Boolean = False
    Public Property AlertBeforeStart As Boolean = False
    Public Property hideontaskbar As Boolean = False
    Public Property UseShadow As Boolean = True
    Public Property bigPanel As New TaskBigControl
    Public Property edz As New editzoom
    Public Property eds As New editschedule
    Public Property ads As New addschedule
    Public Property edl As New editlink
    Public Function BoolMsgbox(message As String) As Boolean
        Dim msg As New yesnodialog
        msg.listpanel.Children.Add(New Label With {.Content = message, .HorizontalContentAlignment = HorizontalAlignment.Center, .VerticalContentAlignment = VerticalAlignment.Center, .FontFamily = New FontFamily("나눔스퀘어_ac"), .FontSize = 16, .HorizontalAlignment = HorizontalAlignment.Stretch, .VerticalAlignment = VerticalAlignment.Stretch})
        Return msg.ShowDialog
    End Function
    Public Function Between(str As String, str1 As String, str2 As String) As String
        Try
            Return Split(Split(str, str1)(1), str2)(0)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function MD5Hash(ByVal str As String) As String
        Dim md5 As Security.Cryptography.MD5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create
        Dim hashed As Byte() = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(str))
        Dim sb As New System.Text.StringBuilder
        For i As Integer = 0 To hashed.Length - 1
            sb.AppendFormat("{0:x2}", hashed(i))
        Next
        Return sb.ToString
    End Function
    Public Function encode64(t As String)
        Dim nByte As Byte()
        nByte = Encoding.UTF8.GetBytes(t)
        Return Convert.ToBase64String(nByte)
        nByte = Nothing
    End Function
    Public Function decode64(t As String)
        Dim nByte As Byte()
        Try
            nByte = Convert.FromBase64String(t)
            Return Encoding.UTF8.GetString(nByte)
        Catch ex As Exception
            Return t
        End Try
        nByte = Nothing
    End Function
    Public Function AES_Encrypt(ByVal input As String, ByVal pass As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = Security.Cryptography.CipherMode.ECB
            Dim DESEncrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateEncryptor
            Dim Buffer As Byte() = System.Text.UTF8Encoding.UTF8.GetBytes(input)
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return encrypted
        Catch ex As Exception
        End Try
    End Function
    Public Function AES_Decrypt(ByVal input As String, ByVal pass As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim decrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = Security.Cryptography.CipherMode.ECB
            Dim DESDecrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateDecryptor
            Dim Buffer As Byte() = Convert.FromBase64String(input)
            decrypted = System.Text.UTF8Encoding.UTF8.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return decrypted
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    Public Sub CenterAlignLabel(lab As Label)
        lab.HorizontalContentAlignment = HorizontalAlignment.Center
        lab.VerticalContentAlignment = VerticalAlignment.Center
    End Sub
    Public Sub SetBigPanel(tc As TaskControl, tp As TaskProperty, sb As Storyboard)
        AddHandler tc.MouseDown, Sub(sender As Object, e As MouseButtonEventArgs)
                                     If bigPanel.Parent Is Nothing Then
                                         FrmScheduleGrid.Children.Add(bigPanel)
                                     End If
                                     Dim p As Point = Mouse.GetPosition(FrmScheduleGrid)
                                     If p.X + bigPanel.Width > MainFrm.TaskGrid.ActualWidth + MainFrm.TaskSCRV.Margin.Left Then
                                         bigPanel.Margin = New Thickness(MainFrm.TaskGrid.ActualWidth + MainFrm.TaskSCRV.Margin.Left - bigPanel.Width - 10, p.Y, 0, 0)
                                     Else
                                         bigPanel.Margin = New Thickness(p.X, p.Y, 0, 0)
                                     End If
                                     bigPanel.Show()
                                     bigPanel.Compose(tp)
                                     e.Handled = True
                                 End Sub
    End Sub
    Public Function InputBox(title As String, Optional desc As String = "", Optional inp As String = "")
        Dim f As New InputboxFrm
        f.titlelab.Content = title
        f.deslab.Content = desc
        f.inputtextbox.Text = inp
        If f.ShowDialog() = True Then
            Return f.inputtextbox.Text
        Else
            Return Nothing
        End If
    End Function

    Public notify As New Forms.NotifyIcon
    Public Sub notifysetup()
        With notify
            AddHandler .MouseDoubleClick, Sub() MainFrm.Show()
            .Icon = My.Resources.in_time
            .BalloonTipTitle = "Scheduler"
            .Text = "Scheduler"
            .Visible = True
            Dim ctm As New Forms.ContextMenu
            Dim it1 As New Forms.MenuItem With {.Index = 0, .Text = "열기"}
            Dim it2 As New Forms.MenuItem With {.Index = 1, .Text = "종료"}
            Dim itcollection = {it1, it2}
            AddHandler it1.Click, Sub() MainFrm.Show()
            AddHandler it2.Click, Sub()
                                      notify.Dispose()
                                      End
                                  End Sub
            ctm.MenuItems.AddRange(itcollection.ToArray)
            .ContextMenu = ctm
        End With
    End Sub
    Public Sub joinZoom(zp As ZoomProperty)
        If Process.GetProcessesByName("Zoom").Count < 1 Then
            Process.Start("zoommtg://zoom.us/join?confno=" & zp.RoomCode & "&pwd=" & zp.Password & "&" & "uname=" & Uri.EscapeDataString(uname))
            Exit Sub
        Else
            MsgBox("줌이 이미 실행중입니다.")
        End If
    End Sub
    Public Sub openLink(lp As LinkProperty)
        Try
            Process.Start(lp.Link)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub Setup(Optional ForceSetup As Boolean = False)
        Dim Transfer As New Transfer
        With My.Computer.FileSystem
            If .DirectoryExists(root) = False Or ForceSetup = True Then
                If .DirectoryExists(Transfer.root) Then
                    If MsgBox("이전 버전의 데이터를 가져오시겠습니까?", vbYesNo) = MsgBoxResult.Yes Then
                        .CreateDirectory(root)
                        Transfer.getlist()
                        Transfer.MakeNewJObject()
                        Transfer.SaveNewJObject()
                        GoTo 2
                    End If
                End If
1:

                ConfigureJson = New JObject
                With ConfigureJson
                    Dim inp As String = InputBox("이름", "줌 입장시 사용할 이름을 입력해 주세요.")
                    If Not inp = "" Then
                        .Add("UserName", inp)
                    Else
                        MsgBox("사용할 이름을 입력해 주세요.")
                        GoTo 1
                    End If
                End With
                .CreateDirectory(root)
                .WriteAllText(SchedulejsonPath, "{" & vbCrLf & "}", False)
                .WriteAllText(zoomlistjsonPath, "{" & vbCrLf & "}", False)
                .WriteAllText(linkjsonPath, "{" & vbCrLf & "}", False)
                .WriteAllText(configurejsonPath, ConfigureJson.ToString, False)
            End If
2:
            ScheduleJson = JObject.Parse(My.Computer.FileSystem.ReadAllText(SchedulejsonPath, Text.Encoding.UTF8))
            ZoomlistJson = JObject.Parse(My.Computer.FileSystem.ReadAllText(zoomlistjsonPath, Text.Encoding.UTF8))
            LinkJson = JObject.Parse(My.Computer.FileSystem.ReadAllText(linkjsonPath, Text.Encoding.UTF8))
            ConfigureJson = JObject.Parse(My.Computer.FileSystem.ReadAllText(configurejsonPath, Text.Encoding.UTF8))
            Dim getValue As New JsonValue With {.Jobject = ConfigureJson}
            With getValue
                uname = .getValue("UserName")
                AutoStartZoom = .getValue("AutoStartZoom")
                AutoStartLink = .getValue("AutoStartLink")
                HourFormat = .getValue("HourFormat")
                DisplayLanguage = .getValue("DisplayLanguage")
                UseShadow = .getValue("UseShadow")
                LoadLine = .getValue("LoadLine")
                LoadLineTask = .getValue("LoadLineTask")
                AlertBeforeStart = .getValue("AlertBeforeStart")
                hideontaskbar = .getValue("HideOnTaskbar")
            End With
        End With
    End Sub
    Public Sub ModifySetting(PropertyName As String, Value As String, Optional SaveJsonFile As Boolean = True)
        If ConfigureJson.ContainsKey(PropertyName) Then
            ConfigureJson(PropertyName) = Value
        Else
            ConfigureJson.Add(PropertyName, Value)
        End If
        If SaveJsonFile = True Then
            saveConfigJson()
        End If
    End Sub
    Public Function ZoomTitleReturn(id As String) As ZoomProperty
        If id = "" Then
            Return Nothing
            Exit Function
        End If
        For Each zp As ZoomProperty In ZoomPropertyList
            If zp.Title = id Then
                Return zp
                Exit Function
            End If
        Next
        Return Nothing
    End Function
    Public Function ZoomContain(id As String) As Boolean
        If id = "" Then
            Return False
            Exit Function
        End If
        For Each zp As ZoomProperty In ZoomPropertyList
            If zp.ID = id Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
    Public Function ZoomReturn(id As String) As ZoomProperty
        If id = "" Then
            Return Nothing
            Exit Function
        End If
        For Each zp As ZoomProperty In ZoomPropertyList
            If zp.ID = id Then
                Return zp
                Exit Function
            End If
        Next
        Return Nothing
    End Function
    Public Function LinkTitleReturn(id As String) As LinkProperty
        If id = "" Then
            Return Nothing
            Exit Function
        End If
        For Each zp As LinkProperty In LinkPropertyList
            If zp.Title = id Then
                Return zp
                Exit Function
            End If
        Next
        Return Nothing
    End Function
    Public Function LinkContain(id As String) As Boolean
        If id = "" Then
            Return False
            Exit Function
        End If
        For Each zp As LinkProperty In LinkPropertyList
            If zp.ID = id Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
    Public Function LinkReturn(id As String) As LinkProperty
        If id = "" Then
            Return Nothing
            Exit Function
        End If
        For Each zp As LinkProperty In LinkPropertyList
            If zp.ID = id Then
                Return zp
                Exit Function
            End If
        Next
        Return Nothing
    End Function
    Public Function ScheduleControlReturn(TaskProp As TaskProperty) As TaskControl
        For Each tp As DependencyObject In FrmScheduleGrid.Children
            If tp.GetType.ToString = "Scheduler.TaskControl" Then
                If CType(tp, TaskControl).myTaskProp Is TaskProp Then
                    Return tp
                    Exit Function
                End If
            End If
        Next
        Return Nothing
    End Function
    Public Function ZoomControlReturn(ZoomProp As ZoomProperty) As zoomlist
        Dim FindArea As WrapPanel = SideListPage.wp
        For Each tp As DependencyObject In FindArea.Children
            If tp.GetType.ToString = "Scheduler.zoomlist" Then
                If CType(tp, zoomlist).MyZoomProp Is ZoomProp Then
                    Return tp
                    Exit Function
                End If
            End If
        Next
        Return Nothing
    End Function
    Public Function LinkControlReturn(linkProp As LinkProperty) As zoomlist
        Dim FindArea As WrapPanel = SideListPage.wp
        For Each tp As DependencyObject In FindArea.Children
            If tp.GetType.ToString = "Scheduler.zoomlist" Then
                If CType(tp, zoomlist).MyLinkProp Is linkProp Then
                    Return tp
                    Exit Function
                End If
            End If
        Next
        Return Nothing
    End Function
    Public Sub getTasklist()
        TaskPropertyList.Clear()

        For Each Tprop As JProperty In ScheduleJson.Properties
            Dim TaskObject As JObject = Tprop.First
            Dim TaskProp As New TaskProperty
            Dim getTaskValue As New JsonValue With {.Jobject = TaskObject}
            With TaskProp
                .JObject = TaskObject
                .JProperty = Tprop
                .ID = getTaskValue.getValue("ID")
                .Title = getTaskValue.getValue("Title")
                .StartTime = Convert.ToDateTime(getTaskValue.getValue("StartTime"))
                .EndTime = Convert.ToDateTime(getTaskValue.getValue("EndTime"))
                .DayOfWeek = getTaskValue.getValue("DOW")
                If getTaskValue.getValue("ZoomID") IsNot Nothing Then
                    If ZoomContain(getTaskValue.getValue("ZoomID").ToString) = True Then
                        .ZoomProp = ZoomReturn(getTaskValue.getValue("ZoomID").ToString)
                    Else
                        .ZoomProp = Nothing
                    End If
                End If
                If getTaskValue.getValue("LinkID") IsNot Nothing Then
                    If LinkContain(getTaskValue.getValue("LinkID").ToString) = True Then
                        .LinkProp = LinkReturn(getTaskValue.getValue("LinkID").ToString)
                    Else
                        .LinkProp = Nothing
                    End If
                End If
                If isColorHex(getTaskValue.getValue("Color")) And getTaskValue.getValue("Color") <> Nothing Then
                    .ColorHex = getTaskValue.getValue("Color")
                Else
                    Dim c = Color.FromRgb(Rand.Next(0, 255), Rand.Next(0, 255), Rand.Next(0, 255))
                    .ColorHex = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2") ' Nothing
                End If
                .SetDuration()
            End With
            TaskPropertyList.Add(TaskProp)
        Next
        MakeTodayList()
    End Sub
    Public Sub MakeTodayList()

        TodayList.Clear()
        For Each Taskprop In TaskPropertyList
            If Taskprop.DayOfWeek = Now.DayOfWeek Then
                TodayList.Add(Taskprop)
            End If
        Next
        TodayList.Sort(Function(x As TaskProperty, y As TaskProperty) x.StartTime.CompareTo(y.StartTime))
    End Sub
    Public Sub getZoomlist()
        ZoomPropertyList.Clear()

        For Each Zprop As JProperty In ZoomlistJson.Properties
            Dim ZoomObject As JObject = Zprop.First
            Dim ZoomProp As New ZoomProperty
            Dim getTaskValue As New JsonValue With {.Jobject = ZoomObject}
            With ZoomProp
                .JObject = ZoomObject
                .JProperty = Zprop
                .ID = getTaskValue.getValue("ID")
                .Title = getTaskValue.getValue("Title")
                If getTaskValue.getValue("Link") = Nothing Then
                    .Link = Nothing
                Else
                    .Link = AES_Decrypt(getTaskValue.getValue("Link"), MD5Hash(ZoomProp.ID & ZoomProp.Title))
                End If
                .RoomCode = AES_Decrypt(getTaskValue.getValue("Code"), MD5Hash(ZoomProp.ID & ZoomProp.Title))
                .Password = AES_Decrypt(getTaskValue.getValue("Pass"), MD5Hash(ZoomProp.ID & ZoomProp.Title))
            End With
            ZoomPropertyList.Add(ZoomProp)
        Next
    End Sub
    Public Sub getLinklist()
        LinkPropertyList.Clear()

        For Each Lprop As JProperty In LinkJson.Properties
            Dim LinkObject As JObject = Lprop.First
            Dim LinkProp As New LinkProperty
            Dim getTaskValue As New JsonValue With {.Jobject = LinkObject}
            With LinkProp
                .JObject = LinkObject
                .JProperty = Lprop
                .ID = getTaskValue.getValue("ID")
                .Title = getTaskValue.getValue("Title")
                .Link = AES_Decrypt(getTaskValue.getValue("Link"), MD5Hash(LinkProp.ID & LinkProp.Title))
            End With
            LinkPropertyList.Add(LinkProp)
        Next
    End Sub
    Public Function isColorHex(hex As String) As Boolean
        Try
            ColorConverter.ConvertFromString(hex)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function getTop(t As TimeSpan) As Integer
        Dim i As Integer = t.TotalMinutes
        Return (i * MinuteHeight)
    End Function
    Public Sub saveTaskJson()
        My.Computer.FileSystem.WriteAllText(SchedulejsonPath, ScheduleJson.ToString, False)
    End Sub
    Public Sub saveZoomJson()
        My.Computer.FileSystem.WriteAllText(zoomlistjsonPath, ZoomlistJson.ToString, False)
    End Sub
    Public Sub saveLinkJson()
        My.Computer.FileSystem.WriteAllText(linkjsonPath, LinkJson.ToString, False)
    End Sub
    Public Sub saveConfigJson()
        My.Computer.FileSystem.WriteAllText(configurejsonPath, ConfigureJson.ToString, False)
    End Sub
End Module
Public Module EnumModule
    Public Enum DayOfWeek
        Sunday = 0
        Monday
        Tuesday
        Wednesday
        Thursday
        Friday
        Saturday
    End Enum
    Public Enum Month
        January = 1
        February
        March
        April
        May
        June
        July
        August
        September
        October
        November
        December
    End Enum
    Public Enum Language
        Korean
        English
    End Enum
    Public Enum HourFormatEnum
        Use12
        Use24
    End Enum
    Public Enum TaskGridLineEnum
        None
        HorizontalOnly
        VerticalOnly
        Both
    End Enum
    Public Enum ItemListMode
        Schedule
        Zoom
        Link
    End Enum
End Module
Public Module ArrayModule
    Public MonthOfYear As String() = {"", "1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월"}
    Public MonthOfYear_EN As String() = {"", "January", " February", " March", " April", " May", " June", " July", " August", " September", " October", " November", " December"}

    Public DayOfWeek As String() = {"일요일", "월요일", "화요일", "수요일", "목요일", "금요일", "토요일", "일요일"}
    Public DayOfWeek_EN As String() = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
    Public DayOfWeek_s As String() = {"일", "월", "화", "수", "목", "금", "토", "일"}
    Public DayOfWeek_s_EN As String() = {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"}

    Public DayOfWeek_s_s_EN As String() = {"sun", "mon", "tue", "wed", "thr", "fri", "sat"}

End Module
Public Class TaskProperty
    Public Property JObject As JObject
    Public Property JProperty As JProperty
    Public Property Title As String
    Public Property StartTime As DateTime
    Public Property EndTime As DateTime
    Public Property DayOfWeek As DayOfWeek
    Public Property ZoomProp As ZoomProperty
    Public Property LinkProp As LinkProperty
    Public Property TaskDate As Date
    Public Property Duration As TimeSpan
    Public Property ColorHex As String
    Public Property ID As String
    Public Property AutoStarted As Boolean = False
    Public Sub SetID()
        ID = "SCH" + MD5Hash(Title & StartTime.ToLongTimeString & EndTime.ToLongTimeString & TaskDate.ToLongDateString & DayOfWeek & Rand.NextDouble) & Rand.Next
    End Sub
    Public Sub SetDuration()
        Duration = EndTime.Subtract(StartTime)
    End Sub
    Public Function getDayofweek(language As Language)
        If language = Language.English Then
            Return Me.DayOfWeek.ToString
        Else
            Return ArrayModule.DayOfWeek(Me.DayOfWeek)
        End If
    End Function
    Public Function getTime(format As HourFormatEnum, Optional isEnd As Boolean = False)
        If isEnd = False Then
            If format = HourFormatEnum.Use12 Then
                Return StartTime.ToString("hh:mm")
            Else
                Return StartTime.ToString("HH:mm")
            End If
        Else
            If format = HourFormatEnum.Use12 Then
                Return EndTime.ToString("hh:mm")
            Else
                Return EndTime.ToString("HH:mm")
            End If
        End If
    End Function
End Class
Public Class ZoomProperty
    Public Property JObject As JObject
    Public Property JProperty As JProperty
    Public Property Title As String
    Public Property Link As String
    Public Property RoomCode As String
    Public Property Password As String
    Public Property ID As String
    Public Sub SetID()
        ID = "ZCM" + MD5Hash(Title & RoomCode & Password & Rand.NextDouble) & Rand.Next
    End Sub
End Class
Public Class LinkProperty
    Public Property JObject As JObject
    Public Property JProperty As JProperty
    Public Property Title As String
    Public Property Link As String
    Public Property ID As String
    Public Sub SetID()
        ID = "LNK" + MD5Hash(Title & Link & Rand.NextDouble) & Rand.Next
    End Sub
End Class
Public Class JsonValue
    Public Property Jobject As JObject
    Public Function getValue(Key As String)
        If Jobject.ContainsKey(Key) Then
            Return Jobject(Key)
        Else
            Return Nothing
        End If
    End Function
    Public Sub setValue(ByVal Key As String, ByVal Value As Object)
        If Jobject.ContainsKey(Key) Then
            Jobject(Key) = Value
        Else
            Jobject.Add(Key, Value)
        End If
    End Sub
End Class
Public Class DateLabel : Inherits Label
    Public Property TargetDate As Date
End Class