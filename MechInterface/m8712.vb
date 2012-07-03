'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports SettingsManager
Imports DebugWindow

Module m8712

    ' Dll declarations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Declare Sub WdtEnable Lib "wdtdll.dll" Alias "WdtEnable" (ByVal type As Integer, ByVal value As Integer)
    '  Public Declare Sub WdtEnable Lib "wdtdll.dll" Alias "WdtEnable" (ByVal type As Short, ByVal value As Short)
    '  Public Declare Sub WdtEnable Lib "wdtdll.dll" Alias "WdtEnable" (ByVal type As Byte, ByVal value As Byte)
    Public Declare Function IoReadByte Lib "wdtdll.dll" Alias "IoReadByte" (ByVal portAddress As Short) As Byte
    Public Declare Sub IoWriteByte Lib "wdtdll.dll" Alias "IoWriteByte" (ByVal portAddress As Short, ByVal portValue As Byte)

    ' Constants
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Const MINUTES = 0
    Const SECONDS = 1

    Const GPIO_PORT = &H801

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    '  Private moduleRunning As Boolean = True
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory

    ' Initialise
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        settingsManager = settingsManagerFactory.GetManager()
        debugInformation = debugInformationFactory.GetManager()

    End Sub

    ' GetGpioBit & SetGpioBit
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function GetGpioBit(ByVal bitIndex As Integer) As Boolean

        Dim portValue As Byte = 0

        If settingsManager.GetValue("WatchdogPcEnable") Then

            If settingsManager.GetValue("Has8712") Then
                portValue = IoReadByte(GPIO_PORT)
            End If

            Return (portValue And (2 ^ bitIndex)) <> 0

        End If

    End Function

    Public Sub SetGpioBit(ByVal bitIndex As Integer, ByVal newState As Boolean)

        Dim portValue As Byte
        Dim newValue

        If settingsManager.GetValue("WatchdogPcEnable") Then

            If settingsManager.GetValue("Has8712") Then

                portValue = IoReadByte(GPIO_PORT)

                If newState Then

                    newValue = portValue Or (2 ^ bitIndex)
                    IoWriteByte(GPIO_PORT, newValue)

                Else

                    newValue = portValue And (&HFF - (2 ^ bitIndex))
                    IoWriteByte(GPIO_PORT, newValue)

                End If

            End If

        End If

    End Sub

End Module
