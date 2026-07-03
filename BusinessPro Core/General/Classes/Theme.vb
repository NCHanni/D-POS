Public Class Theme

    Public Function [Get]() As IO.Stream
        Try
            Return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("BusinessPro.Core.DefaultTheme.isl")
        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
