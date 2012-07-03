'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports System.IO

Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal exceptionArguments As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException

            Dim fileStreamObject As FileStream
            Dim fileStreamWriterObject As StreamWriter
            Dim timeNow As String
            Dim logFileName As String
            Dim exeptionObject As System.Exception = exceptionArguments.Exception
            MsgBox(exceptionArguments.Exception.ToString)
            ' open and write the exception to the current unhandled exception log.
            logFileName = "UnhandledException_" & Format(Now.Month, "##00") & "_" & Format(Now.Day, "##00") & "_" & Format(Now.Year, "####0000") & ".txt"
            timeNow = Format(Now.Month, "##00") & "/" & Format(Now.Day, "##00") & "/" & Format(Now.Year, "####0000") & " " & Format(Now.Hour, "##00") & ":" & Format(Now.Minute, "##00") & ":" & Format(Now.Second, "##00") & ":" & Format(Now.Millisecond, "###000")

            fileStreamObject = New FileStream("C:\Control\Logs\" & logFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
            fileStreamWriterObject = New StreamWriter(fileStreamObject)
            fileStreamWriterObject.BaseStream.Seek(0, SeekOrigin.End)

            fileStreamWriterObject.WriteLine(timeNow & "Message" & exeptionObject.Message & ControlChars.CrLf)
            fileStreamWriterObject.WriteLine(timeNow & "InnerException.Message" & exeptionObject.InnerException.Message & ControlChars.CrLf)
            fileStreamWriterObject.WriteLine(timeNow & "StackTrace" & exeptionObject.StackTrace & ControlChars.CrLf)

            fileStreamWriterObject.Flush()

            fileStreamWriterObject.Close()
            fileStreamObject.Close()

        End Sub

    End Class

End Namespace

