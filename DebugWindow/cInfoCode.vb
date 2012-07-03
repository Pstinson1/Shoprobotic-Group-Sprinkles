'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Public Class cInfoCode

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum Level
        INF
        WRN
        ERR
    End Enum

    Private eventCode As Integer
    Private eventDescription As String
    Private eventLevel As Level

    Sub New(ByVal newEventLevel As Level, ByVal newEventCode As Integer, ByVal newEventDescription As String)

        eventCode = newEventCode
        eventDescription = newEventDescription
        eventLevel = eventLevel

    End Sub

    Public Sub GetEventDetails(ByRef eventLevelReturn As Level, ByRef eventCodeReturn As Integer, ByRef eventDescriptionReturn As String)

        eventDescriptionReturn = eventDescription
        eventCodeReturn = eventCode
        eventLevelReturn = eventLevel

    End Sub

End Class
