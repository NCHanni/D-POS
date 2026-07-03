Module Methods

#Region "LaunchForm"

    Private m_NewFormActivated As Boolean = False
    Private m_NewFormCaption As String = ""

    Public Function LaunchForm(ByRef _Form As Form, Optional ByVal _IsModal As Boolean = True, Optional ByVal _Owner As Form = Nothing) As DialogResult
        Try
            Core.Current.FormatUI.Apply(_Form)

            If _IsModal Then
                With _Form
                    .StartPosition = FormStartPosition.CenterScreen
                    .ShowInTaskbar = False

                    If _Owner Is Nothing Then
                        Return .ShowDialog()
                    Else
                        Return .ShowDialog(_Owner)
                    End If

                    If m_NewFormActivated Then
                        m_NewFormActivated = False
                        m_NewFormCaption = ""
                    End If
                End With
            Else
                With _Form
                    .WindowState = FormWindowState.Maximized
                    .MdiParent = Current.FormMain
                    .Padding = New Padding(3)
                    .Show()

                    m_NewFormCaption = .Text
                    m_NewFormActivated = True

                    Return DialogResult.None
                End With
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Others"

    Function CDateEx(value As Object) As Date
        If value Is Nothing OrElse IsDBNull(value) Then
            Return Nothing
        Else
            Dim parsedValue As Date
            If Date.TryParse(value.ToString, parsedValue) Then
                Return parsedValue
            Else
                Return Nothing
            End If
        End If
    End Function

    Function CDblEx(value As Object, Optional defaultIfNull As Double = 0.0) As Double
        If value Is Nothing OrElse IsDBNull(value) Then
            Return defaultIfNull
        Else
            Dim parsedValue As Double = 0.0
            If Double.TryParse(value.ToString, parsedValue) Then
                Return parsedValue
            Else
                Return defaultIfNull
            End If
        End If
    End Function

    Function CStrEx(value As Object, Optional defaultIfNull As String = "") As String
        If value Is Nothing OrElse IsDBNull(value) Then
            Return defaultIfNull
        Else
            Return value.ToString.Trim
        End If
    End Function

    Function GetAge(dateOfBirth As Date, Optional dateToCheck As Date = Nothing) As Integer
        Try
            Dim today As Date = If(dateToCheck = Nothing, Date.Today, dateToCheck)
            Dim age = today.Year - dateOfBirth.Year

            If dateOfBirth > today.AddYears(-age) Then
                age = age - 1
            End If

            Return age
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Module