Friend Class CashUp
    Inherits DataSource

#Region "Properties"

    Private m_Data As DataSet
    Public ReadOnly Property Data() As DataSet
        Get
            Return m_Data
        End Get
    End Property

    Property CashierSessionId As Long
    Property DaySessionId As Long
    Property Operation As String

    Private m_ServerDate As Date = Now
    Public ReadOnly Property ServerDate() As Date
        Get
            Return m_ServerDate
        End Get
    End Property

#End Region

#Region "SQL"

    Private Function FillSQL(ByVal terminalCode As String) As String
        Dim sql As String =
            "EXEC dbo.GetTerminalSession " & terminalCode.Quote(False)
        Return sql
    End Function

    Private Function LogInSessionSQL(cashierCode As String, terminalCode As String) As String
        Dim sql As String =
            String.Concat("EXEC dbo.LogInTerminalSession ",
                terminalCode.Quote,
                cashierCode.Quote(False))
        Return sql
    End Function

    Private Function LogOutSessionSQL(
            cashierCode As String,
            terminalCode As String) As String
        Dim sql As String =
            String.Concat("EXEC dbo.LogOutTerminalSession ",
                terminalCode.Quote,
                cashierCode.Quote(False))
        Return sql
    End Function

    Private Function GetCashUpSQL(endingBalance As Double) As String
        Dim sql As String = String.Format(
            "EXEC dbo.GenerateXReading {0} {1}",
                CashierSessionId.Quote,
                endingBalance.Quote(False))
        Return sql
    End Function

    Private Function FinalizeCashUpSQL(
            cashierCode As String,
            endingBalance As Decimal,
            terminalCode As String,
            denominations As DataTable) As String

        Dim sql As String =
            "EXEC dbo.FinalizeXReading " & vbCrLf &
            "   " & CashierSessionId.Quote &
            "   " & terminalCode.Quote &
            "   " & cashierCode.Quote &
            "   " & endingBalance.Quote(False)

        For Each row As DataRow In denominations.Rows
            Select Case row.RowState
                Case DataRowState.Added : Operation = "INSERT"
                Case DataRowState.Modified : Operation = "UPDATE"
                Case Else : Continue For
            End Select

            sql &= vbCrLf &
                "EXEC dbo.SaveCashUpDenomination " & vbCrLf &
                "   " & CashierSessionId.Quote &
                "   " & row("denomination").ToString().Quote &
                "   " & row("pieces").ToString().Quote &
                "   " & row("amount").ToString().Quote &
                "   " & Operation.Quote(False) & vbCrLf
        Next

        Return CreateTryCatchBlockSQL(sql)
    End Function

    Private Function GetCashEndSQL(cashierSessionId As Long) As String
        Dim sql As String =
            String.Format("SELECT locked, amount FROM dbo.GetCashEnd({0})", cashierSessionId.Quote(False))
        Return sql
    End Function

    Private Function LockCashEndSQL(
            ByVal cashierSessionId As Long,
            ByVal cashEndAmount As Double,
            ByVal unlock As Boolean)
        Dim unlockParam = IIf(unlock, "1;", "0;")
        Dim sql As String =
            String.Concat("EXEC dbo.LockCashEndCashUp ", cashierSessionId.Quote, cashEndAmount.Quote, unlockParam)
        Return sql
    End Function

    Private Function GetLastZReadingDateSQL() As String
        Dim sql As String =
            "SELECT TOP 1 date_finalized AS [date] " &
            "FROM [z_reading] " &
            "WHERE is_finalized = 1 " &
            "ORDER BY [date] DESC"
        Return sql
    End Function

    Private Function SaveSQL(
            ByVal terminalCode As String,
            ByVal cashierCode As String,
            ByVal cashIn As Double) As String
        Dim sql As String =
            "EXEC dbo.SaveCashUp " &
            "  " & terminalCode.Quote &
            "  " & cashierCode.Quote &
            "  " & "DEFAULT," & 'date in - use server data
            "  " & cashIn.Quote(False)
        Return sql
    End Function

    'Private Function GetCashUpValue(
    '        ByVal cashUpTable As DataTable,
    '        ByVal description As String,
    '        Optional ByVal getCount As Boolean = False) As Double
    '    Dim row() As DataRow = cashUpTable.Select("description = " & description.Quote(False))
    '    If row.Length > 0 Then
    '        Return CDbl(row(0)(If(getCount, "data1", "data2")))
    '    End If
    'End Function

#End Region

#Region "Methods"

    Sub Fill(ByVal terminalCode As String)
        Try
            m_Data = ExecuteQuery(FillSQL(terminalCode))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub LogInSession(cashierCode As String, terminalCode As String)
        Try
            m_Data = ExecuteQuery(LogInSessionSQL(cashierCode, terminalCode))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub LogOutSession(cashierCode As String, terminalCode As String)
        Try
            ExecuteQuery(LogOutSessionSQL(cashierCode, terminalCode))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Function GetCashUp(endingBalance As Double, denominations As DataTable) As DataSet
        Try
            Dim ds As DataSet =
                ExecuteQuery(GetCashUpSQL(endingBalance))

            ds.Tables.Add(denominations.Copy)

            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Sub FinalizeCashUp(cashierCode As String, endingBalance As Decimal, terminalCode As String, denominations As DataTable)
        Try
            m_Data = ExecuteQuery(FinalizeCashUpSQL(cashierCode, endingBalance, terminalCode, denominations))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Function GetCashEnd(ByVal cashierSessionId As Long) As DataTable
        Try
            Return ExecuteQuery(GetCashEndSQL(cashierSessionId)).Tables(0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Sub LockCashEnd(ByVal cashierSessionId As Long, ByVal cashEndAmount As Double, Optional unlock As Boolean = False)
        Try
            ExecuteNonQuery(LockCashEndSQL(cashierSessionId, cashEndAmount, unlock))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub Save(ByVal terminalCode As String, ByVal cashierCode As String, ByVal cashIn As Double)
        Try
            Dim row As DataRow =
                ExecuteQuery(SaveSQL(terminalCode, cashierCode, cashIn)).Tables(0)(0)

            CashierSessionId = row("session_id")
            m_ServerDate = row("server_date")
            DaySessionId = row("day_session_id")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Function GetLastZReadingDate() As Nullable(Of DateTime)
        Try
            Dim dt = ExecuteQuery(GetLastZReadingDateSQL()).Tables(0)

            If dt.Rows.Count <= 0 Then
                Return Nothing
            End If

            Return DirectCast(dt.Rows(0)("date"), DateTime)
        Catch
            Throw
        End Try
    End Function

    Function HasActiveSession(terminalCode As String) As Boolean
        Try
            Dim sql As String =
                "SELECT dbo.HasActiveSession(" & terminalCode.Quote(False) & ")"
            Return CBool(ExecuteQuery(sql).Tables(0)(0)(0))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Function IsCurrentlyLoggedIn(userCode As String, terminalCode As String) As Boolean
        Try
            Dim sql As String =
                "SELECT TOP 1 1 " &
                "FROM [cash_up] " &
                "WHERE terminal_code <> " & terminalCode.Quote(False) &
                " AND terminal_code IS NOT NULL" &
                " AND cashier_code = " & userCode.Quote(False) &
                " AND date_out IS NULL"
            Return Exists(sql)
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

End Class
