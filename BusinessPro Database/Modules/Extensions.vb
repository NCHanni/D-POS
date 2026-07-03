Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Drawing

<Extension()> _
Module Extensions

#Region "String Extensions"

    <Extension()> _
    Public Function ToProperCase(ByVal Text As String) As String
        Dim newText As String = ""
        Dim toUpper As Boolean = True
        Text = Trim(Text).Replace("_", " ")
        Try
            For Each character As Char In Text
                Select Case character
                    Case " ", ".", "-"
                        toUpper = True
                End Select
                If toUpper Then
                    newText &= character.ToString.ToUpper
                    toUpper = False
                Else
                    newText &= character.ToString.ToLower
                End If
            Next
        Catch ex As Exception
            newText = Text
        End Try
        Return newText.Replace(" Of ", " of ")
    End Function

    <Extension()> _
    Public Function ToWords(ByVal Value As Double) As String
        Static Ones(9) As String
        Static Teens(9) As String
        Static Tens(9) As String
        Static Thousands(4) As String
        Static Init As Boolean

        Dim i As Integer
        Dim AllZeros, ShowThousands As Boolean
        Dim strBuff, strVal, strTemp As String
        Dim nCol, nChar As Integer

        If Value < 0 Then
            Throw New ArgumentException("Only positive values are allowed")
        End If

        If Init = False Then
            Init = True
            Ones(0) = "zero"
            Ones(1) = "one"
            Ones(2) = "two"
            Ones(3) = "three"
            Ones(4) = "four"
            Ones(5) = "five"
            Ones(6) = "six"
            Ones(7) = "seven"
            Ones(8) = "eight"
            Ones(9) = "nine"
            Teens(0) = "ten"
            Teens(1) = "eleven"
            Teens(2) = "twelve"
            Teens(3) = "thirteen"
            Teens(4) = "fourteen"
            Teens(5) = "fifteen"
            Teens(6) = "sixteen"
            Teens(7) = "seventeen"
            Teens(8) = "eighteen"
            Teens(9) = "nineteen"
            Tens(0) = ""
            Tens(1) = "ten"
            Tens(2) = "twenty"
            Tens(3) = "thirty"
            Tens(4) = "forty"
            Tens(5) = "fifty"
            Tens(6) = "sixty"
            Tens(7) = "seventy"
            Tens(8) = "eighty"
            Tens(9) = "ninety"
            Thousands(0) = ""
            Thousands(1) = "thousand"
            Thousands(2) = "million"
            Thousands(3) = "billion"
            Thousands(4) = "trillion"
        End If

        Try
            ' Get fractional part
            strBuff = "and " & Format((Value - Int(Value)) * 100, "00") & "/100"
            ' Convert rest to string and process each digit
            strVal = CStr(Int(Value))
            ' Non-zero digit not yet encountered
            AllZeros = True
            ' Iterate through string
            For i = strVal.Length To 1 Step -1
                ' Get value of this digit
                nChar = CInt(Val(Mid(strVal, i, 1)))
                ' Get column position
                nCol = (Len(strVal) - i) + 1
                ' Action depends on 1's, 10's or 100's column
                Select Case (nCol Mod 3)
                    Case 1 ' 1's position
                        ShowThousands = True
                        If i = 1 Then
                            ' First digit in number (last in loop)
                            strTemp = Ones(nChar) & " "
                        ElseIf Mid(strVal, i - 1, 1) = "1" Then
                            ' This digit is part of "teen" number
                            strTemp = Teens(nChar) & " "
                            i = i - 1 'Skip tens position
                        ElseIf nChar > 0 Then
                            ' Any non-zero digit
                            strTemp = Ones(nChar) & " "
                        Else
                            ' This digit is zero. If digit in tens and hundreds column
                            ' are also zero, don't show "thousands"
                            ShowThousands = False
                            ' Test for non-zero digit in this grouping
                            If Mid(strVal, i - 1, 1) <> "0" Then
                                ShowThousands = True
                            ElseIf i > 2 Then
                                If Mid(strVal, i - 2, 1) <> "0" Then
                                    ShowThousands = True
                                End If
                            End If
                            strTemp = ""
                        End If
                        ' Show "thousands" if non-zero in grouping
                        If ShowThousands Then
                            If nCol > 1 Then
                                strTemp = strTemp & Thousands(nCol \ 3)
                                If AllZeros Then
                                    strTemp = strTemp & " "
                                Else
                                    strTemp = strTemp & " "
                                End If
                            End If
                            ' Indicate non-zero digit encountered
                            AllZeros = False
                        End If
                        strBuff = strTemp & strBuff
                    Case 2 ' 10's position
                        If nChar > 0 Then
                            If Mid(strVal, i + 1, 1) <> "0" Then
                                strBuff = Tens(nChar) & "-" & strBuff
                            Else
                                strBuff = Tens(nChar) & " " & strBuff
                            End If
                        End If
                    Case 0 ' 100's position
                        If nChar > 0 Then
                            strBuff = Ones(nChar) & " hundred " & strBuff
                        End If
                End Select
            Next i
            Return strBuff.Substring(0, 1).ToUpper() & strBuff.Substring(1) & " "
        Catch ex As Exception
            Return "#Error#"
        End Try
    End Function

    <Extension()> _
    Public Function Filter(ByVal Text As String) As String
        Text = Text.Replace("[", "")
        Text = Text.Replace("]", "")
        Text = Text.Replace("{", "")
        Text = Text.Replace("}", "")
        Text = Text.Replace("'", "''")
        Return Text
    End Function

    <Extension()> _
    Public Function ToText(ByVal Image As Image) As String
        Try
            Dim ms As New System.IO.MemoryStream
            Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            Dim st As String = System.Convert.ToBase64String(ms.ToArray)
            ms.Close()
            Return st
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function

    <Extension()> _
    Public Function ToImage(ByVal Text As String) As Image
        Try
            If Text.Length > 0 Then
                Dim arrImage() As Byte = Convert.FromBase64String(Text)
                Dim ms As New System.IO.MemoryStream(arrImage)
                Dim img As Image = Image.FromStream(ms)
                Return img
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function

    <Extension()> _
    Public Function Decrypt(ByVal _Value As String) As String
        Try
            Return ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(_Value.ToString))
        Catch ex As Exception
            Throw
        End Try
    End Function

    <Extension()> _
    Public Function Encrypt(ByVal _Value As String) As String
        Try
            Return Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_Value.ToString))
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Data Validation Extensions"
    <Extension()> _
    Public Function CheckNull(ByVal _Data As Object, ByVal _DefaultValue As Object) As Object
        If IsDBNull(_Data) Then
            Return _DefaultValue
        Else
            Return _Data
        End If
    End Function

    <Extension()> _
    Public Function CheckNothing(ByVal _Data As Object, ByVal _DefaultValue As Object) As Object
        If IsNothing(_Data) Then
            Return _DefaultValue
        Else
            Return _Data
        End If
    End Function

    <Extension()> _
    Public Function HasChanges(ByVal _Data As DataTable) As Boolean
        Dim i As Integer
        For i = 0 To _Data.Rows.Count - 1
            If _Data.Rows(i).RowState <> DataRowState.Detached And _Data.Rows(i).RowState <> DataRowState.Unchanged Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region

End Module