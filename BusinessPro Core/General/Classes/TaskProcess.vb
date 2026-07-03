Imports System.Threading.Tasks

''' <summary>
''' Usage: used as alternative to the wait form
''' <para>Properties:</para>
'''<para>Form, Control, ShowLoader</para>
''' <para>Methods:</para>
'''  <para>Start (5 overloads)</para>
''' </summary>
''' <remarks>Created By: Patric Ryan C. Lee 01.24.2011 | Modified by: Noel A. Dacara More And More Improvements + Enhancements</remarks>
Public Class TaskProcess
    Implements IDisposable

#Region "Constructor"

    ''' <summary>
    ''' constructor
    ''' </summary>
    ''' <param name="f">the form where this class is declared, usually the value 'Me'</param>
    Public Sub New(ByVal f As Form)
        Me.Form = f
    End Sub

    ''' <summary>
    ''' constructor
    ''' </summary>
    ''' <param name="f">the form where this class is declared, usually the value 'Me'</param>
    ''' <param name="c">the control where the loader will be shown, usually a grid</param>
    Public Sub New(ByVal f As Form, ByVal c As Control)
        Me.Form = f
        Me.Control = c
    End Sub

#End Region

#Region "Declarations"

    Private m_PictureBox As PictureBox = Nothing

    Public Delegate Sub GenericSubProcedure()
    Public Delegate Sub GenericSubProcedureWithOneParam(Of param)(ByVal p1 As param)
    Public Delegate Sub GenericSubProcedureWithParam(Of InputType)(ByVal input() As InputType)

#End Region

#Region "Properties"

    Property Form As Form
    Property Control As Control = Nothing
    Property ShowLoader As Boolean = True ' will be ignored if Control is not set

#End Region

#Region "Methods"

    ''' <summary>Starts the task process</summary>
    ''' <param name="procedure">sub procedure that will be called</param>
    Public Sub Start(ByVal procedure As GenericSubProcedure)
        Try
            InitializeLoader()

            'Task.Factory.StartNew(
            '    Sub()
            '        procedure()
            '        InvokeEx()

            '    End Sub)
            Task.Factory.StartNew(
                Sub()
                    procedure()
                    InvokeEx(
                        Sub()
                            If Control IsNot Nothing AndAlso m_PictureBox IsNot Nothing Then
                                Control.Parent.Controls.Remove(m_PictureBox)
                            End If
                        End Sub, Form)
                End Sub)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Start(Of param)(ByVal procedure As GenericSubProcedureWithOneParam(Of param), ByVal p1 As param)
        Try
            InitializeLoader()

            Task.Factory.StartNew(Of param)(
                Function(param_1 As param) As Object
                    procedure(param_1)
                    InvokeEx(
                        Sub()
                            If Control IsNot Nothing AndAlso m_PictureBox IsNot Nothing Then
                                Control.Parent.Controls.Remove(m_PictureBox)
                            End If
                        End Sub, Form)
                    Return Nothing
                End Function, p1)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Start(Of InputType)(ByVal procedure As GenericSubProcedureWithParam(Of InputType), ByVal ParamArray input() As InputType)
        Try
            InitializeLoader()

            Task.Factory.StartNew(Of InputType)(
                Function(i() As InputType) As Object
                    procedure(i)
                    InvokeEx(
                        Sub()
                            If Not Form.IsDisposed Then
                                If Control IsNot Nothing AndAlso m_PictureBox IsNot Nothing Then
                                    Control.Parent.Controls.Remove(m_PictureBox)
                                End If
                            End If
                        End Sub, Form)
                    Return Nothing
                End Function, input)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>Starts the task process</summary>
    ''' <param name="procedure">sub procedure that will be called</param>
    ''' <param name="procedureAfter">sub procedure that will be called after 'procedure' is finished executing</param>
    Public Sub Start(ByVal procedure As GenericSubProcedure, ByVal procedureAfter As GenericSubProcedure)
        Try
            InitializeLoader()

            Task.Factory.StartNew(
                Sub()
                    procedure()
                End Sub).ContinueWith(
                Sub()
                    InvokeEx(
                        Sub()
                            If Not Form.IsDisposed Then
                                If Control IsNot Nothing AndAlso m_PictureBox IsNot Nothing Then
                                    Control.Parent.Controls.Remove(m_PictureBox)
                                End If
                                procedureAfter()
                            End If
                        End Sub, Form)
                End Sub)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Start(Of param)(ByVal procedure As GenericSubProcedureWithOneParam(Of param), ByVal p1 As param, ByVal procedureAfter As GenericSubProcedureWithOneParam(Of param), ByVal p2 As param)
        Try
            InitializeLoader()

            Task.Factory.StartNew(Of param)(
                Function(param_1 As param)
                    procedure(param_1)
                    Return Nothing
                End Function, p1).ContinueWith(
                Function(t)
                    InvokeEx(
                        Sub()
                            If Not Form.IsDisposed Then
                                If Control IsNot Nothing AndAlso m_PictureBox IsNot Nothing Then
                                    Control.Parent.Controls.Remove(m_PictureBox)
                                End If
                                procedureAfter(p2)
                            End If
                        End Sub, Form)
                    Return Nothing
                End Function)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InitializeLoader()
        Try
            If ShowLoader AndAlso Me.Control IsNot Nothing Then
                m_PictureBox = New PictureBox

                With m_PictureBox
                    .Name = "picLoader"
                    .Image = My.Resources.wait_loader
                    .Size = New System.Drawing.Size(32, 32)
                    '.Size = New System.Drawing.Size(24, 24)
                    '.Location = New Point(Control.Size.Width / 2 - 12, Control.Size.Height / 2 - 12)
                    .Location = New Point(Control.Size.Width / 2 - 16, Control.Size.Height / 2 - 16)
                    .UseWaitCursor = True
                    .Visible = True
                End With

                Control.Parent.Controls.Add(m_PictureBox)

                m_PictureBox.BringToFront()
                m_PictureBox.BackColor = Color.Transparent
                m_PictureBox.Refresh()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub


    Public Function GetCoreLoadingImage() As Image
        Try
            Return My.Resources.wait_loader
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' dispose managed state (managed objects).
            End If

            ' free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' set large fields to null.
            If m_PictureBox IsNot Nothing Then
                m_PictureBox.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
