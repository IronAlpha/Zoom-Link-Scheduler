Class searchpage

    Private Sub SearchTextbox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles SearchTextbox.TextChanged

    End Sub

    Private Sub SearchTextbox_KeyDown(sender As Object, e As KeyEventArgs) Handles SearchTextbox.KeyDown
        If e.Key = Key.Enter Then
            If SearchTextbox.Text = "" Then
                SearchResultGrd.Visibility = Visibility.Hidden
            Else
                SearchResultGrd.Visibility = Visibility.Visible
                searchlist.ItemsSource = Nothing
                searchlist.Items.Clear()
                Dim itll As New List(Of DependencyObject)
                For Each tp In TaskPropertyList
                    If InStr(tp.Title, SearchTextbox.Text) Then
                        Dim il As New itemlist
                        With il
                            .TaskCompose(tp)
                            .SearchCompose()
                            .descriptionCompose(tp.getDayofweek(DisplayLanguage) & " " & tp.getTime(HourFormat) & "~")
                        End With
                        itll.Add(il)
                    End If
                Next
                For Each tp In ZoomPropertyList
                    If InStr(tp.Title, SearchTextbox.Text) Then
                        Dim il As New itemlist
                        With il
                            .ZoomCompose(tp)
                            .SearchCompose()
                        End With
                        itll.Add(il)
                    ElseIf InStr(tp.RoomCode, SearchTextbox.Text) Then
                        Dim il As New itemlist
                        With il
                            .ZoomCompose(tp)
                            .SearchCompose()
                            .descriptionCompose(tp.RoomCode)
                        End With
                        itll.Add(il)
                    ElseIf InStr(tp.Link, SearchTextbox.Text) Then
                        Dim il As New itemlist
                        With il
                            .ZoomCompose(tp)
                            .SearchCompose()
                            .descriptionCompose(tp.Link)
                        End With
                        itll.Add(il)
                    End If
                Next
                For Each tp In LinkPropertyList
                    If InStr(tp.Title, SearchTextbox.Text) Then
                        Dim il As New itemlist
                        With il
                            .LinkCompose(tp)
                            .SearchCompose()
                        End With
                        itll.Add(il)
                    ElseIf InStr(tp.Link, SearchTextbox.Text) Then
                        Dim il As New itemlist
                        With il
                            .LinkCompose(tp)
                            .SearchCompose()
                            .descriptionCompose(tp.Link)
                        End With
                        itll.Add(il)
                    End If
                Next
                If itll.Count = 0 Then
                    itll.Add(New Label With {.Content = "검색 결과가 없습니다.", .Width = SearchResultGrd.ActualWidth, .Height = SearchResultGrd.ActualHeight - 100, .FontFamily = New FontFamily("나눔스퀘어_ac"), .FontSize = 12, .VerticalContentAlignment = VerticalAlignment.Center, .HorizontalContentAlignment = HorizontalAlignment.Center})
                End If
                searchlist.ItemsSource = itll
            End If
        End If
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        If searchlist.ItemsSource Is Nothing Then
            Dim itll As New List(Of DependencyObject)
            itll.Add(New Label With {.Content = "검색할 내용을 입력해주세요.", .Width = SearchResultGrd.ActualWidth, .Height = SearchResultGrd.ActualHeight - 100, .FontFamily = New FontFamily("나눔스퀘어_ac"), .FontSize = 12, .VerticalContentAlignment = VerticalAlignment.Center, .HorizontalContentAlignment = HorizontalAlignment.Center})
            searchlist.ItemsSource = itll
        End If
    End Sub
End Class
