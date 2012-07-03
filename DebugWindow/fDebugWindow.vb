'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

' Imports
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Imports System.IO
Imports System.Text
Imports System.Net.Sockets
Imports System.Net
Public Class fDebugWindow

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Enum Level
        INF
        WRN
        ERR
    End Enum

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private eventCodeList() As cInfoCode
    Private broadcastSockets As cSocketListener = Nothing
    Private broadcastRunning As Boolean = False

    ' Delegates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Delegate Sub SetProgressCallback(ByVal messageLevel As Level, ByVal messageCode As Integer, ByVal messageText As String, ByVal writeToFile As Boolean)

    Sub New()

        '  this call is required by the Windows Form Designer.
        InitializeComponent()

        ' ensure that the logging folder exists
        If Not Directory.Exists("C:\Control\Logs\") Then _
            Directory.CreateDirectory("C:\Control\Logs\")

        broadcastSockets = New cSocketListener()

    End Sub


    Public Sub AllowDebugConnections()

        If broadcastSockets.Start(15269, AddressOf ListenerEventCallback) Then
            broadcastRunning = True
        End If

    End Sub



    Private Sub ListenerEventCallback(ByVal eventCode As cSocketListener.Message, ByVal socketReporting As StateObject, ByVal messageSent As String)

        Select Case eventCode

            Case cSocketListener.Message.SKT_CONNECT
                Console.WriteLine("SKT_CONNECT " & socketReporting.remoteAddress.ToString & ", id " & socketReporting.identifier)

            Case cSocketListener.Message.SKT_REFUSED
                Console.WriteLine("SKT_REFUSED, no vacant connection objects, connection declined")

            Case cSocketListener.Message.SKT_DISCONNECT
                Console.WriteLine("SKT_DISCONNECT " & socketReporting.remoteAddress.ToString & ", id " & socketReporting.identifier)

            Case cSocketListener.Message.SKT_READ
                Console.WriteLine(socketReporting.identifier & "! " & messageSent)
        End Select

    End Sub

    Private Sub ConnectionEventCallback(ByVal eventCode As cSocketConnection.Message, ByVal messageReceived As String)


    End Sub

    Public Sub Shutdown()

        broadcastSockets.Shutdown()

    End Sub

    ' Initialise
    ' set up debug information, build the event list
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        ' this line is suprizing important, it forced .net to assign a widow handle enabling the gui invoke to work..
        Dim temporaryHandle As System.IntPtr = Me.Handle

        ' Software Report Codes
        RegisterEvent(Level.INF, 0, "VMC (Re-)started")

        ' operational codes
        RegisterEvent(Level.INF, 18, "Sensible exit Z position read from NVR - OK")
        RegisterEvent(Level.ERR, 19, "No sensible exit Z position read from NVR 0- FAILED")
        RegisterEvent(Level.INF, 22, "Product detected on vacuum switch")
        RegisterEvent(Level.INF, 23, "No product detected on extension")

        RegisterEvent(Level.INF, 28, "Sucker turns on OK")
        RegisterEvent(Level.ERR, 29, "Sucker won't turn on")
        RegisterEvent(Level.INF, 30, "Sucker turns off OK")
        RegisterEvent(Level.ERR, 31, "Sucker won't turn off")
        RegisterEvent(Level.INF, 32, "Picker returns to tray OK")
        RegisterEvent(Level.ERR, 33, "Picker couldn't return to tray - FAILED")
        RegisterEvent(Level.INF, 34, "Product detected on vaccuum after return")

        ' mech, arm and door movement status codes
        RegisterEvent(Level.WRN, 100, "Moving : Vertical overcurrent - FAILED")
        RegisterEvent(Level.WRN, 101, "Moving : Horizontal overcurrent - FAILED")
        RegisterEvent(Level.WRN, 103, "Moving : Arm is not home - FAILED")
        RegisterEvent(Level.INF, 106, "Product-hold state is not correct at end of move - FAIL")
        RegisterEvent(Level.INF, 109, "Moving : Rack operation ends")
        RegisterEvent(Level.ERR, 112, "Homing : chips went over temperature - FAILED")
        RegisterEvent(Level.INF, 113, "Homing : Completes OK")
        RegisterEvent(Level.INF, 114, "Moving : Rack operation ends")

        RegisterEvent(Level.WRN, 115, "Homing FAILED")
        RegisterEvent(Level.WRN, 116, "Rack movement timeout : Timeout")

        RegisterEvent(Level.WRN, 118, "Moving : Unexpected X limit reached")
        RegisterEvent(Level.WRN, 119, "Moving : Unexpected Y limit reached")



        RegisterEvent(Level.WRN, 120, "Picking arm movement timed out")
        RegisterEvent(Level.WRN, 121, "Picking arm movement overcurrent")
        RegisterEvent(Level.INF, 123, "Picking arm moves OK")
        RegisterEvent(Level.INF, 124, "Picking arm already home")
        RegisterEvent(Level.INF, 125, "Picking arm failed to reach target")
        RegisterEvent(Level.WRN, 127, "Product lost during arm movement - FAIL")
        RegisterEvent(Level.INF, 129, "Pick Retry Initiated")
        RegisterEvent(Level.INF, 130, "Secure Door Move started")
        RegisterEvent(Level.WRN, 131, "Secure Door overcurrent - ABORTED")
        RegisterEvent(Level.WRN, 132, "Secure Door Move timed out - FAIL")
        RegisterEvent(Level.WRN, 133, "Secure Door Move prevented by Arm not home - ABORTED")
        RegisterEvent(Level.INF, 134, "Secure Door reached desired position - OK")
        RegisterEvent(Level.INF, 135, "Product detected during Door sweep")
        RegisterEvent(Level.WRN, 136, "Product NOT detected during door sweep.")
        RegisterEvent(Level.WRN, 138, "Secure Door Move prevented by S.R.B. - ABORTED")
        RegisterEvent(Level.INF, 139, "Secure Door Move completed - OK")

        RegisterEvent(Level.INF, 156, "Rack homing, rack position is known")
        RegisterEvent(Level.INF, 157, "Rack homing, fridge door open")

        ' commands
        RegisterEvent(Level.INF, 183, "Unrecognised command")

        RegisterEvent(Level.INF, 195, "Rack move command completes")

        ' delivery cycle status codes
        RegisterEvent(Level.INF, 210, "Delivery cycle completes - OK")

        ' bill validator status codes
        RegisterEvent(Level.INF, 300, "Product delivery - starts")
        RegisterEvent(Level.INF, 301, "Product delivery - door closed and clear of product")
        RegisterEvent(Level.INF, 302, "Product delivery - initial homing completed")
        RegisterEvent(Level.INF, 303, "Product delivery - waypoint required on way to product")
        RegisterEvent(Level.INF, 304, "Product delivery - waypoint reached on way to product")
        RegisterEvent(Level.INF, 305, "Product delivery - waypoint to product, sequence complete")
        RegisterEvent(Level.INF, 306, "Product delivery - rack reached product position")
        RegisterEvent(Level.INF, 307, "Product delivery - vacuum turned on okay")
        RegisterEvent(Level.INF, 308, "Product delivery - unable to get the product from the shelf")

        RegisterEvent(Level.WRN, 312, "Product delivery - unable collected onto pick head")
        RegisterEvent(Level.INF, 313, "Product delivery - waypoint required on return to door")
        RegisterEvent(Level.WRN, 314, "Product delivery - unable to reach the waypoint with product on return")
        RegisterEvent(Level.INF, 315, "Product delivery - rack at waypoint on return")
        RegisterEvent(Level.INF, 316, "Product delivery - waypoint reached with product on return")
        RegisterEvent(Level.INF, 317, "Product delivery - no VDO for this product")
        RegisterEvent(Level.INF, 318, "Product delivery - VDO used for this product")
        RegisterEvent(Level.INF, 319, "Product delivery - arm moved out to drum delivery position")

        RegisterEvent(Level.INF, 320, "Product delivery - product held on arm at the delivery position in drum")
        RegisterEvent(Level.INF, 321, "Product delivery - product not held at arm delivery extent")
        RegisterEvent(Level.INF, 323, "Product delivery - vacuum turned off for shove")
        RegisterEvent(Level.INF, 324, "Product delivery - arm backed off for shove")
        RegisterEvent(Level.INF, 325, "Product delivery - initial delivery attempt complete - return the arm home")
        RegisterEvent(Level.INF, 326, "Product delivery - arm makes it home")
        RegisterEvent(Level.WRN, 327, "Product delivery - product not detected by optical sensor/camera in the drum")
        RegisterEvent(Level.INF, 329, "Product delivery - full spectrum shove complete")

        RegisterEvent(Level.INF, 330, "Product delivery - ready to try to open the door")
        RegisterEvent(Level.INF, 331, "Product delivery - door did not open to give product to the customer")
        RegisterEvent(Level.INF, 332, "Product delivery - door open attempt complete")
        RegisterEvent(Level.INF, 333, "Product delivery - door opened okay")

        RegisterEvent(Level.WRN, 349, "Product delivery - unable to get door empty and closed")

        RegisterEvent(Level.WRN, 350, "Product delivery - Failed initial homing")
        RegisterEvent(Level.WRN, 351, "Product delivery - rack move failed travelling to waypoint to product")
        RegisterEvent(Level.WRN, 352, "Product delivery - no satisfatory waypoint response from PC when moving to product")
        RegisterEvent(Level.WRN, 353, "Product delivery - rack did not reach the product")
        RegisterEvent(Level.WRN, 354, "Product delivery - unable to turn on the vacuum")
        RegisterEvent(Level.WRN, 355, "Product delivery - unable to reach waypoint on return - likley product lost")
        RegisterEvent(Level.WRN, 356, "Product delivery - no satisfatory waypoint response from PC when returning")
        RegisterEvent(Level.WRN, 357, "Product delivery - unable to home when returning")
        RegisterEvent(Level.WRN, 358, "Product delivery - unable to perform VDO")
        RegisterEvent(Level.WRN, 359, "Product delivery - unable to move arm out to the delivery position")

        RegisterEvent(Level.ERR, 360, "Product delivery - unable to turn off the vacuum at delivery position")
        RegisterEvent(Level.ERR, 361, "Product delivery - unable to turn off the vacuum at delivery position for shove sequence")
        RegisterEvent(Level.ERR, 362, "Product delivery - unable to back arm off away from the door to try shove")
        RegisterEvent(Level.ERR, 363, "Product delivery - unable to re-reach the delivery position during shove")
        RegisterEvent(Level.ERR, 364, "Product delivery - arm unable to return home afte 1st attempt at placing the product in the drum")
        RegisterEvent(Level.ERR, 367, "Product delivery - product not detected after full spectrum shove")
        RegisterEvent(Level.ERR, 369, "Product delivery - unable to open the door after sever attempts")

        ' vacuum system status codes
        RegisterEvent(Level.WRN, 630, "Sucker timed-out awaiting product release - FAIL")

        ' product sensor
        RegisterEvent(Level.WRN, 720, "Product detected on SVS2")
        RegisterEvent(Level.WRN, 721, "No product detected on SVS2")

        ' I²C Status Codes
        RegisterEvent(Level.INF, 850, "I²C start blocked")
        RegisterEvent(Level.INF, 851, "I²C re-trying failed instruction")
        RegisterEvent(Level.INF, 852, "I²C start blocked, data direction = input")
        RegisterEvent(Level.INF, 853, "I²C start blocked, clock=high")

        ' waypoints
        RegisterEvent(Level.INF, 870, "Waypoint - CONTINUE recieved")
        RegisterEvent(Level.WRN, 871, "Waypoint - No recognised response recieved in time / FAIL")

        ' service Required codes
        RegisterEvent(Level.INF, 990, "Service required set")
        RegisterEvent(Level.INF, 991, "Service required cancelled")

    End Sub

    Private Sub RegisterEvent(ByVal eventLevel As Level, ByVal infoCode As Integer, ByVal description As String)

        Dim itemIndex As Integer = 0

        If Not IsNothing(eventCodeList) Then _
            itemIndex = eventCodeList.Length

        Array.Resize(eventCodeList, itemIndex + 1)

        eventCodeList(itemIndex) = New cInfoCode(eventLevel, infoCode, description)

    End Sub

    ' SetTitle
    ' Change the title At the top of the form
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetTitle(ByVal newTitle As String)

        Text = newTitle

    End Sub


    ' DebugWindowFormClosing
    ' hide, reather than close the window
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub DebugWindowFormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Hide()
    End Sub

    ' Progress
    ' enter text in to the textbox in a thread safe manner
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Progress(ByVal messageLevel As Level, ByVal messageCode As Integer, ByVal messageText As String, ByVal writeToFile As Boolean)

        Dim itemIndex As Integer
        Dim timeNow As String
        Dim logFileName As String
        Dim fileStreamObject As FileStream
        Dim fileStreamWriterObject As StreamWriter

        If Me.ProgressListBox.InvokeRequired Then

            ' invoke this message in the correct thread
            Dim d As New SetProgressCallback(AddressOf Progress)
            Me.Invoke(d, New Object() {messageLevel, messageCode, messageText, writeToFile})

        Else

            ' only allow 100 items in a listbox
            If ProgressListBox.Items.Count = 100 Then _
                ProgressListBox.Items.RemoveAt(0)

            ' add the message to the list box and scroll it.
            itemIndex = ProgressListBox.Items.Add(messageText)

            ' scroll to the latest item
            ProgressListBox.TopIndex = itemIndex - 1

            ' do we need to add this message to the log ?
            If writeToFile Then

                Try
                    ' open and close when writing logs kaseya works a lot better if the files arn't held open
                    logFileName = "ActionLogVend_" & Format(Now.Month, "##00") & "_" & Format(Now.Day, "##00") & "_" & Format(Now.Year, "####0000") & ".txt"

                    timeNow = Format(Now.Month, "##00") & "/" & Format(Now.Day, "##00") & "/" & Format(Now.Year, "####0000") & " " & _
                                      Format(Now.Hour, "##00") & ":" & Format(Now.Minute, "##00") & ":" & Format(Now.Second, "##00") & ":" & Format(Now.Millisecond, "###000")

                    fileStreamObject = New FileStream("C:\Control\Logs\" & logFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)
                    fileStreamWriterObject = New StreamWriter(fileStreamObject)

                    fileStreamWriterObject.BaseStream.Seek(0, SeekOrigin.End)

                    fileStreamWriterObject.WriteLine(timeNow & ControlChars.Tab & messageLevel.ToString & ControlChars.Tab & messageCode.ToString & ControlChars.Tab & messageText)
                    fileStreamWriterObject.Flush()

                    fileStreamWriterObject.Close()
                    fileStreamObject.Close()


                    If Not broadcastSockets Is Nothing AndAlso broadcastRunning Then

                        broadcastSockets.Broadcast(timeNow & ControlChars.Tab & messageLevel.ToString & ControlChars.Tab & messageText & ControlChars.CrLf)

                    End If

                Catch ex As Exception
                End Try

            End If

        End If

    End Sub

    ' VMCEventDetails
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function VMCEventDetails(ByRef eventLevelReturn As Level, ByRef eventCode As Integer, ByRef eventDescriptionReturn As String) As Boolean

        Dim eventDescriptionSearch As String = ""
        Dim eventLevelSearch As Level
        Dim eventCodeSearch As Integer
        Dim codeFound As Boolean = False

        If IsNothing(eventCodeList) Then

            Return False

        End If

        For Each eventItem As cInfoCode In eventCodeList

            eventItem.GetEventDetails(eventLevelSearch, eventCodeSearch, eventDescriptionSearch)

            If eventCodeSearch = eventCode Then

                eventLevelReturn = eventLevelSearch
                eventDescriptionReturn = eventDescriptionSearch
                codeFound = True
            End If

        Next

        Return codeFound

    End Function

    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

End Class

' Class cHotDrinkManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cDebugWindowFactory

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared debugWindow As fDebugWindow = Nothing

    Public Function GetManager() As fDebugWindow

        If IsNothing(debugWindow) Then

            debugWindow = New fDebugWindow
            debugWindow.Initialise()

        End If

        Return debugWindow

    End Function

End Class
