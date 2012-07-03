'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports Microsoft.Win32
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports System.Xml
Imports System.Data.SqlClient

Imports DebugWindow

Partial Class fAttractManager

    ' structures
    Structure DmxCueStruct

        Dim number As Integer
        Dim name As String
        Dim fadeInDuration As Integer
        Dim fadeOutDuration As Integer
        Dim isFollow As Boolean
        Dim followTime As Integer
        Dim channel() As Byte

    End Structure

    Structure DmxScriptStruct

        Dim fileName As String
        Dim name As String
        Dim followOn As Boolean
        Dim followOnTo As String
        Dim cueTotal As Integer
        Dim lightingCues() As DmxCueStruct
        Dim channelblend() As Boolean

    End Structure

    Enum FadePhaseEnum
        FD_IDLE
        FD_SETUP
        FD_TRANSITION
        FD_WAIT
        FD_NEXT_CUE
        FD_NEXT_SCRIPT
    End Enum

    ' constants
    Private Const CHANNEL_COUNT = 128
    Private Const MSG_START = &H7E
    Private Const MSG_END = &HE7


    Private Const MSG_GET_WIDGET_PARAMETERS_REQUEST = 3
    Private Const MSG_SEND_DMX_PACKET_REQUEST = 6
    Private Const TIMER_UPDATE_DELAY = 25

    ' variables
    Private portListLoading As Boolean = False
    Private trackbarsLoading As Boolean = False
    Private channelLevel(CHANNEL_COUNT) As Byte
    Private lightingDelay As Integer = TIMER_UPDATE_DELAY
    Private channelLevelFrom(CHANNEL_COUNT) As Byte
    Private channelLevelTo(CHANNEL_COUNT) As Byte
    Private crossFadeFrameCount As Integer = 0
    Private crossFadeFrameTotal As Integer = 0
    Private incommingCue As DmxCueStruct
    Private scriptList() As DmxScriptStruct
    Private currentScriptIndex As Integer
    Private ignoreScriptSelectionChange As Boolean = False

    Private currentCueIndex As Integer
    Private transmissionBytes(CHANNEL_COUNT + 5) As Byte
    Private intermediateLevel(CHANNEL_COUNT) As Byte
    Private chanelDescriptor(CHANNEL_COUNT) As Char
    Private channelBlend(CHANNEL_COUNT) As Boolean
    Private chanelGroup(CHANNEL_COUNT) As Char
    Private lightingThread As Thread
    Private redScalar As Integer = 255
    Private greenScalar As Integer = 255
    Private blueScalar As Integer = 255
    Private baseSelectedChannel As Integer
    Private fadePhase As FadePhaseEnum = FadePhaseEnum.FD_IDLE
    Private channelIndexing(CHANNEL_COUNT) As Integer

    ' InitialiseLighting
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub InitialiseLighting()

        debugManager.Progress(fDebugWindow.Level.INF, 2230, "Starting Lighting", True)

        BuildListOfPossiblePorts()
        OpenLightingPort()
        StartLighting()

        Thread.Sleep(100)

        CreateChannelIndexing()
        LoadChannelDescriptorsAndGroups()
        LoadDMXScripts(Application.StartupPath.ToString & "\Lighting\" & settingsManager.GetValue("DmxFilename"))

        ' set up the transmission level array
        transmissionBytes(0) = MSG_START
        transmissionBytes(1) = MSG_SEND_DMX_PACKET_REQUEST
        transmissionBytes(2) = CHANNEL_COUNT Mod &HFF
        transmissionBytes(3) = CHANNEL_COUNT / &H100
        transmissionBytes(CHANNEL_COUNT + 4) = MSG_END

        ' start the management thread
        lightingThread = New Thread(AddressOf LightingThreadProcess)
        lightingThread.Priority = ThreadPriority.AboveNormal
        lightingThread.Name = "Lighting"
        lightingThread.Start()

    End Sub

    Private Sub CreateChannelIndexing()

        Dim channelIndex As Integer
        Dim resultSet As SqlDataReader
        Dim colourTable() As String = {"R", "G", "B"}

        If settingsManager.ConnectToDatabase() Then

            If Not settingsManager.TableExists("LightingChannelIndex") Then

                settingsManager.RunDatabaseNonQuery("CREATE TABLE [dbo].[LightingChannelIndex](" & _
                                                                                     "[Id] [int] IDENTITY(1,1) NOT NULL," & _
                                                                                     "[Channel] [int] NULL," & _
                                                                                     "[Colour] [nchar](1) NULL," & _
                                                                                     "[Cluster] [nchar](1) NULL" & _
                                                                                    ") ON [PRIMARY]")

                For channelIndex = 1 To CHANNEL_COUNT

                    settingsManager.RunDatabaseNonQuery("Insert into LightingChannelIndex (Channel,Colour,Cluster) values (" & channelIndex & ", '" & colourTable((channelIndex - 1) Mod 3) & "', ' ')")
                    channelIndexing(channelIndex) = channelIndex
                Next

            Else

                For channelIndex = 1 To CHANNEL_COUNT

                    resultSet = settingsManager.RunDatabaseQuery("select channel from LightingChannelIndex where id =" & channelIndex)

                    If Not resultSet Is Nothing AndAlso resultSet.HasRows Then

                        resultSet.Read()
                        channelIndexing(channelIndex) = resultSet("channel")
                        settingsManager.CloseQuery(resultSet)

                    End If

                Next

            End If

            settingsManager.DisconnectFromDatabase()

        End If

    End Sub


    ' LoadChannelDescriptorsAndGroups
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub LoadChannelDescriptorsAndGroups()

        Dim channelIndex As Integer
        Dim descriptorString As String = ""
        Dim groupString As String = ""

        ' by default all are null
        For channelIndex = 0 To CHANNEL_COUNT - 1
            chanelDescriptor(channelIndex) = " "
            chanelGroup(channelIndex) = " "
        Next

        ' get the channel info
        descriptorString = settingsManager.GetValue("LightChannelDescriptors")
        groupString = settingsManager.GetValue("LightChannelGroups")

        ' break it up for easy access
        If Not descriptorString Is Nothing Then
            For channelIndex = 0 To descriptorString.Length - 1
                chanelDescriptor(channelIndex + 1) = descriptorString.Substring(channelIndex, 1)
            Next
        End If

        If Not groupString Is Nothing Then
            For channelIndex = 0 To groupString.Length - 1
                chanelGroup(channelIndex + 1) = groupString.Substring(channelIndex, 1)
            Next
        End If


    End Sub

    Private masterScrollingManually As Boolean = False
    ' SetBaseColour
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetBaseColour(ByVal redComponent As Integer, ByVal greenComponent As Integer, ByVal blueComponent As Integer)

        redScalar = redComponent
        greenScalar = greenComponent
        blueScalar = blueComponent

        helperFunctions.StatusStripMessage(StatusStrip, 2, "RGB: " & redScalar.ToString & "," & greenScalar.ToString & "," & blueScalar.ToString)
        Progress("Base Colour Set to: " & redScalar.ToString("X2") & "-" & greenScalar.ToString("X2") & "-" & blueScalar.ToString("X2"))

        If Not masterScrollingManually Then
            helperFunctions.SetTrackbarLevel(MasterRedTrackbar, redScalar)
            helperFunctions.SetTrackbarLevel(MasterGreenTrackbar, greenScalar)
            helperFunctions.SetTrackbarLevel(MasterBlueTrackbar, blueScalar)

        End If

    End Sub


    Private Sub MasterTrackbar_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MasterRedTrackbar.Scroll, MasterGreenTrackbar.Scroll, MasterBlueTrackbar.Scroll

        masterScrollingManually = True
        SetBaseColour(MasterRedTrackbar.Value, MasterGreenTrackbar.Value, MasterBlueTrackbar.Value)
        masterScrollingManually = False

    End Sub

    ' LightingThreadProcess
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub LightingThreadProcess()

        Dim fromLevel As Integer
        Dim toLevel As Integer
        Dim levelDifference As Integer

        While continueProcessing

            If Not editMode And Not scriptList Is Nothing Then

                Select Case fadePhase

                    Case FadePhaseEnum.FD_IDLE

                    Case FadePhaseEnum.FD_SETUP

                        SetTimerDelay(TIMER_UPDATE_DELAY)
                        fadePhase = FadePhaseEnum.FD_TRANSITION
                        crossFadeFrameCount = 0

                    Case FadePhaseEnum.FD_TRANSITION

                        ' we are in the middle of a fade   
                        crossFadeFrameCount = crossFadeFrameCount + 1

                        For channelIndex = 1 To CHANNEL_COUNT
                            fromLevel = channelLevelFrom(channelIndex)
                            toLevel = channelLevelTo(channelIndex)

                            levelDifference = ((toLevel - fromLevel) * crossFadeFrameCount) / crossFadeFrameTotal
                            intermediateLevel(channelIndex) = fromLevel + levelDifference

                        Next

                        ' update the lights
                        Buffer.BlockCopy(intermediateLevel, 0, channelLevel, 0, CHANNEL_COUNT)
                        ApplyLightingLevels()

                        If crossFadeFrameCount = crossFadeFrameTotal Then
                            fadePhase = FadePhaseEnum.FD_WAIT
                        End If

                    Case FadePhaseEnum.FD_WAIT

                        If scriptList(currentScriptIndex).cueTotal <> 0 Then
                            SetTimerDelay(scriptList(currentScriptIndex).lightingCues(currentCueIndex).followTime)

                            If (scriptList(currentScriptIndex).lightingCues(currentCueIndex).isFollow) Then

                                ' move to the next cue, but wrap on the last cue in the script.
                                currentCueIndex = (currentCueIndex + 1) Mod scriptList(currentScriptIndex).cueTotal

                                FadeToCue(scriptList(currentScriptIndex).lightingCues(currentCueIndex))

                            ElseIf scriptList(currentScriptIndex).followOn Then
                                RunLightingScript(scriptList(currentScriptIndex).followOnTo, True)
                            End If

                        End If

                End Select

            End If


            If continueProcessing Then
                Thread.Sleep(lightingDelay)
            End If

        End While

    End Sub

    ' RunLightingScript
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub RunLightingScript(ByVal newScriptName As String, ByVal highlightListRequired As Boolean)

        Dim newLightingCue As ListViewItem
        Dim scriptIndex As Integer

        Progress("Switching to script: " & newScriptName)

        ' if there is no script list, abort
        If scriptList Is Nothing Then
            Progress("no scripts, aborting")
            Return
        End If

        ' search the scripts for one with this name
        For scriptIndex = 0 To scriptList.Length - 1

            If scriptList(scriptIndex).name = newScriptName Then

                Progress("Found.")

                If highlightListRequired Then
                    ignoreScriptSelectionChange = True
                    helperFunctions.SetListViewSelection(ScriptListView, scriptIndex)

                    Application.DoEvents()

                    ignoreScriptSelectionChange = False
                End If

                channelBlend = scriptList(scriptIndex).channelblend

                helperFunctions.SetCheckChecked(FollowOnCheck, scriptList(scriptIndex).followOn)
                helperFunctions.SetTextBoxText(FollowOnText, scriptList(scriptIndex).followOnTo)

                currentScriptIndex = scriptIndex
                currentCueIndex = 0

                ' display the script details
                helperFunctions.ClearListView(CueListView)

                If scriptList(currentScriptIndex).cueTotal > 0 Then

                    ' insert the products in the list view
                    For Each singleCue In scriptList(currentScriptIndex).lightingCues

                        newLightingCue = New ListViewItem

                        newLightingCue.Tag = singleCue
                        newLightingCue.Text = singleCue.number.ToString

                        newLightingCue.SubItems.Add(singleCue.name)
                        newLightingCue.SubItems.Add(singleCue.fadeInDuration.ToString)
                        newLightingCue.SubItems.Add(singleCue.fadeOutDuration.ToString)
                        newLightingCue.SubItems.Add(singleCue.isFollow.ToString)
                        newLightingCue.SubItems.Add(singleCue.followTime.ToString)

                        helperFunctions.AddToListView(CueListView, newLightingCue)

                    Next

                    SetTimerDelay(TIMER_UPDATE_DELAY)
                    FadeToCue(scriptList(currentScriptIndex).lightingCues(0))

                Else
                    Progress("Script activated, but no cues in current script.")
                End If

            End If

        Next

        DrawTrackbarControls(FaderScrollBar.Value)

    End Sub


    ' BuildListOfPossiblePorts
    ' show all identified comports and highlight the one selected from the settings manager and open the chosen one
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub BuildListOfPossiblePorts()

        Dim possiblePorts() As String = System.IO.Ports.SerialPort.GetPortNames()
        Dim portComboIndex As Integer

        PortCombo.Items.Clear()

        For Each portName As String In possiblePorts
            PortCombo.Items.Add(portName)
        Next

        portListLoading = True
        portComboIndex = PortCombo.FindStringExact(settingsManager.GetValue("LightingPort"))

        If portComboIndex <> -1 Then
            PortCombo.SelectedIndex = portComboIndex
        End If
        portListLoading = False

    End Sub

    Private Sub OpenLightingPort()

        ' set up for the comm port
        LightingPort.Encoding = System.Text.Encoding.UTF8
        LightingPort.PortName = settingsManager.GetValue("LightingPort")

        Try
            LightingPort.Open()
        Catch ex As Exception

            Progress("Unable to open port")
        End Try


        Thread.Sleep(200)
    End Sub

    ' LoadDMXScripts
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub LoadDMXScripts(ByVal xmlScriptFilename As String)

        Dim listDocument As New XmlDocument
        Dim scriptFilename As String
        Dim scriptNodeList As XmlNodeList
        Dim scriptCount As Integer = 0
        Dim newScriptItem As ListViewItem
        Dim blendIndex As Integer
        Dim blendList() As String

        helperFunctions.SetTextBoxText(DmxFilenameText, settingsManager.GetValue("DmxFilename"))
        Progress("Loading DMX scripts from '" & xmlScriptFilename & "'")

        ' Load data  
        Try
            listDocument.Load(xmlScriptFilename)

            helperFunctions.ClearListView(ScriptListView)
            scriptCount = 0

            scriptNodeList = listDocument.GetElementsByTagName("script")

            For Each script As XmlNode In scriptNodeList

                ' load the script
                ReDim Preserve scriptList(scriptCount)

                ' create the blend list for this script
                ReDim scriptList(scriptCount).channelblend(CHANNEL_COUNT)

                For blendIndex = 0 To CHANNEL_COUNT
                    scriptList(scriptCount).channelblend(CHANNEL_COUNT) = False
                Next

                blendList = script.SelectSingleNode("blendchannels").InnerText.Trim().Split(",")

                For Each blendChannel In blendList

                    If helperFunctions.StringToInteger(blendChannel, blendIndex) Then
                        scriptList(scriptCount).channelblend(blendIndex) = True
                    End If

                Next

                scriptFilename = script.SelectSingleNode("file").InnerText.Trim()
                scriptList(scriptCount).fileName = scriptFilename
                scriptList(scriptCount).name = script.SelectSingleNode("name").InnerText.Trim()
                scriptList(scriptCount).followOnTo = script.SelectSingleNode("followonto").InnerText.Trim()
                helperFunctions.StringToBoolean(script.SelectSingleNode("followon").InnerText.Trim(), scriptList(scriptCount).followOn)

                Progress("    " & scriptList(scriptCount).name & "-" & scriptFilename)
                LoadDmxScript(Application.StartupPath.ToString & "\Lighting\" & scriptFilename, scriptList(scriptCount))

                ' enter this script into the script list
                newScriptItem = New ListViewItem
                newScriptItem.Tag = scriptList(scriptCount)
                newScriptItem.Text = scriptList(scriptCount).name.ToString

                helperFunctions.AddToListView(ScriptListView, newScriptItem)

                ' count this script
                scriptCount = scriptCount + 1
            Next

            ' if available select the 1st item
            If ScriptListView.Items.Count > 0 Then
                RunLightingScript(ScriptListView.Items(0).Text, True)
            End If

        Catch ex As Exception
        End Try

    End Sub

    Private Sub LoadDmxScript(ByVal dmxScriptFilename As String, ByRef dmxScript As DmxScriptStruct)

        Try
            Dim streamReader As System.IO.StreamReader = New StreamReader(dmxScriptFilename)
            Dim cueIndex As Integer = 0
            Dim singleLevel As Integer = 0
            Dim valueList() As String

            ' kill any existing transition
            crossFadeFrameCount = 0
            crossFadeFrameTotal = 0

            valueList = streamReader.ReadLine().Split(",")         ' version number
            valueList = streamReader.ReadLine().Split(",")         ' patch list

            ' initial light levels, we ignore these
            valueList = streamReader.ReadLine().Split(",")

            ' get current CurrentCue/NextCue/Crossfader Values, we ignore these
            valueList = streamReader.ReadLine().Split(",")

            ' cue count, treat as an estimate, ie ignore
            valueList = streamReader.ReadLine().Split(",")

            ' load the cues
            While Not streamReader.EndOfStream

                valueList = streamReader.ReadLine().Split(",")

                If valueList.Length >= 6 Then

                    ' create space for the cue and the channels in that cue.
                    ReDim Preserve dmxScript.lightingCues(cueIndex)
                    ReDim dmxScript.lightingCues(cueIndex).channel(CHANNEL_COUNT)

                    helperFunctions.StringToInteger(valueList(0), dmxScript.lightingCues(cueIndex).number)
                    dmxScript.lightingCues(cueIndex).name = valueList(1)
                    helperFunctions.StringToInteger(valueList(2), dmxScript.lightingCues(cueIndex).fadeInDuration)
                    helperFunctions.StringToInteger(valueList(3), dmxScript.lightingCues(cueIndex).fadeOutDuration)
                    helperFunctions.StringToBoolean(valueList(4), dmxScript.lightingCues(cueIndex).isFollow)
                    helperFunctions.StringToInteger(valueList(5), dmxScript.lightingCues(cueIndex).followTime)

                    For levelIndex = 1 To CHANNEL_COUNT
                        If (levelIndex <= valueList.Length - 5) Then
                            helperFunctions.StringToInteger(valueList(levelIndex + 5), dmxScript.lightingCues(cueIndex).channel(levelIndex))
                        End If
                    Next

                    cueIndex = cueIndex + 1

                End If

            End While

            dmxScript.cueTotal = cueIndex

        Catch ex As Exception
        End Try

    End Sub


    Sub SetTrackBarLevels(ByVal channelLevel() As Byte)

        Dim levelIndex As Integer
        Dim channelTrackBar() As System.Windows.Forms.TrackBar = {TrackBar1, TrackBar2, TrackBar3, TrackBar4, TrackBar5, TrackBar6, TrackBar7, TrackBar8, TrackBar9, TrackBar10, TrackBar11, TrackBar12, TrackBar13, TrackBar14, TrackBar15, TrackBar16, _
             TrackBar17, TrackBar18, TrackBar19, TrackBar20, TrackBar21, TrackBar22, TrackBar23, TrackBar24}

        trackbarsLoading = True

        If continueProcessing Then

            For levelIndex = 0 To 23
                helperFunctions.SetTrackbarLevel(channelTrackBar(levelIndex), channelLevel(baseSelectedChannel + levelIndex + 1))
            Next
        End If

        trackbarsLoading = False

    End Sub

    'Private Sub ApplyLightingLevels()

    '    Dim displayedIndex As Integer = 0
    '    Dim lightIndex As Integer = 4
    '    Dim channelIndex As Integer = 0

    '    'If Not LightingPort.IsOpen Then _
    '    '    Return


    '    For channelIndex = 0 To CHANNEL_COUNT - 1
    '        transmissionBytes(channelIndex + 4) = channelLevel(channelIndex)

    '    Next

    '    Try
    '        LightingPort.Write(transmissionBytes, 0, transmissionBytes.Length)
    '    Catch ex As Exception
    '        Progress("ApplyLightingLevels, Error: " & ex.Message)
    '    End Try

    'End Sub

    Private Sub ApplyLightingLevels()

        Dim channelReference As Integer
        Dim displayedIndex As Integer = 0
        Dim lightIndex As Integer = 4
        Dim channelIndex As Integer = 0

        If Not LightingPort.IsOpen Then _
            Return

        ' clear all the channels
        For channelIndex = 0 To CHANNEL_COUNT - 1
            transmissionBytes(channelIndex + 4) = 0
        Next

        For channelIndex = 1 To CHANNEL_COUNT - 1

            channelReference = channelIndexing(channelIndex) - 1

            If channelReference >= 0 And channelReference < CHANNEL_COUNT Then
                transmissionBytes(channelReference + 5) = channelLevel(channelIndex)

            Else

            End If

        Next

        Try
            LightingPort.Write(transmissionBytes, 0, transmissionBytes.Length)
        Catch ex As Exception
            Progress("ApplyLightingLevels, Error: " & ex.Message)
        End Try

    End Sub

    Private Sub StartLighting()

        Dim message1() As Byte = {126, MSG_GET_WIDGET_PARAMETERS_REQUEST, 2, 0, 0, 0, 231}

        Try
            LightingPort.Write(message1, 0, message1.Length)
        Catch ex As Exception
            Progress("ApplyLightingLevels, Error: " & ex.Message)
        End Try

    End Sub


    Private Sub LightingDataRecieved(ByVal sender As Object, ByVal e As IO.Ports.SerialDataReceivedEventArgs) Handles LightingPort.DataReceived
        Progress("Lighting data recieved: " & e.EventType.ToString)


        Dim header As Byte() = New Byte(3) {}
        Dim message As Byte()
        Dim footer As Byte
        Dim length As Integer
        LightingPort.Read(header, 0, 4)
        If header(0) <> 126 Then
            Return
        End If
        length = header(2) Or (header(3) << 8)
        message = New Byte(length - 1) {}
        LightingPort.Read(message, 0, length)
        '            footer = new byte[1];
        footer = CByte(LightingPort.ReadByte())
        If footer <> 231 Then
            Return
        End If

    End Sub

    Private Sub LightingErrorReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialErrorReceivedEventArgs) Handles LightingPort.ErrorReceived
        Progress("Lighting error recieved: " & e.EventType.ToString)
    End Sub


    Private Sub PortCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PortCombo.SelectedIndexChanged

        If Not portListLoading Then
            settingsManager.SetValue("LightingPort", PortCombo.Text)
        End If

    End Sub

    Private Sub SelectFileButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectFileButton.Click

        DmxFileDialog.FileName = settingsManager.GetValue("DmxFilename")
        DmxFileDialog.InitialDirectory = Application.StartupPath & "\Lighting\"

        If DmxFileDialog.ShowDialog() Then

            Dim newFileName As String = Path.GetFileName(DmxFileDialog.FileName)

            helperFunctions.SetTextBoxText(DmxFilenameText, newFileName)
            settingsManager.SetValue("DmxFilename", newFileName)
            LoadDMXScripts(Path.GetFileName(newFileName))

        End If

    End Sub


    Private Sub CueListView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CueListView.SelectedIndexChanged

        If sender.SelectedItems.Count > 0 Then

            Dim selectedCue As DmxCueStruct = sender.SelectedItems(0).Tag()
            Dim selectedIndex = sender.SelectedItems(0).Index

            If editMode Then
                currentCueIndex = selectedIndex
                Buffer.BlockCopy(selectedCue.channel, 0, channelLevel, 0, CHANNEL_COUNT)
                SetTrackBarLevels(selectedCue.channel)
                ApplyLightingLevels()

            Else

                ' the tag is the cue structure
                SetTimerDelay(TIMER_UPDATE_DELAY)
                FadeToCue(selectedCue)

            End If
        End If

    End Sub

    Private Sub ScriptListView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScriptListView.SelectedIndexChanged

        Dim selectedScript As DmxScriptStruct

        If sender.SelectedItems.Count > 0 And Not ignoreScriptSelectionChange Then

            selectedScript = sender.SelectedItems(0).Tag()
            RunLightingScript(selectedScript.name, False)

        End If

    End Sub


    Private Sub SetTimerDelay(ByVal newTimerDelay As Integer)

        lightingDelay = newTimerDelay
    End Sub

    Private Sub FadeToCue(ByVal selectedCue As DmxCueStruct)

        Dim channelIndex As Integer
        Dim calculatedColour As Integer

        incommingCue = selectedCue

        For channelIndex = 0 To CHANNEL_COUNT


            If channelBlend(channelIndex) Then

                Select Case chanelDescriptor(channelIndex)

                    Case "R"
                        calculatedColour = (selectedCue.channel(channelIndex) * redScalar) / 255
                        channelLevelTo(channelIndex) = calculatedColour

                    Case "G"
                        calculatedColour = (selectedCue.channel(channelIndex) * greenScalar) / 255
                        channelLevelTo(channelIndex) = calculatedColour

                    Case "B"
                        calculatedColour = (selectedCue.channel(channelIndex) * blueScalar) / 255
                        channelLevelTo(channelIndex) = calculatedColour

                    Case Else
                        channelLevelTo(channelIndex) = selectedCue.channel(channelIndex)

                End Select

            Else
                channelLevelTo(channelIndex) = selectedCue.channel(channelIndex)
            End If



        Next

        Buffer.BlockCopy(channelLevel, 0, channelLevelFrom, 0, CHANNEL_COUNT)
        fadePhase = FadePhaseEnum.FD_SETUP
        crossFadeFrameTotal = selectedCue.fadeInDuration / TIMER_UPDATE_DELAY

        SetTrackBarLevels(selectedCue.channel)

    End Sub

    Private Sub TrackBar_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll, TrackBar2.Scroll, TrackBar3.Scroll, TrackBar4.Scroll, TrackBar5.Scroll, TrackBar6.Scroll, TrackBar7.Scroll, TrackBar8.Scroll, TrackBar9.Scroll, TrackBar10.Scroll, TrackBar11.Scroll, TrackBar12.Scroll, TrackBar13.Scroll, TrackBar14.Scroll, TrackBar15.Scroll, TrackBar16.Scroll, TrackBar17.Scroll, TrackBar18.Scroll, TrackBar19.Scroll, TrackBar20.Scroll, TrackBar21.Scroll, TrackBar22.Scroll, TrackBar23.Scroll, TrackBar24.Scroll

        Dim levelTrackbar() As TrackBar = {TrackBar1, TrackBar2, TrackBar3, TrackBar4, TrackBar5, TrackBar6, TrackBar7, TrackBar8, TrackBar9, TrackBar10, TrackBar11, TrackBar12, TrackBar13, TrackBar14, TrackBar15, TrackBar16, TrackBar17, TrackBar18, TrackBar19, TrackBar20, TrackBar21, TrackBar22, TrackBar23, TrackBar24}
        Dim channelSelected As Integer = sender.tag
        Dim channelIndex As Integer
        Dim currentGroup As Char
        Dim currentDescription As Char
        Dim trackbarScreenIndex As Integer

        Debug.WriteLine(chanelDescriptor(baseSelectedChannel + channelSelected) & "-" & chanelGroup(baseSelectedChannel + channelSelected))

        currentGroup = chanelGroup(baseSelectedChannel + channelSelected)
        currentDescription = chanelDescriptor(baseSelectedChannel + channelSelected)

        If currentDescription = "R" OrElse currentDescription = "G" OrElse currentDescription = "B" Then

            For channelIndex = 1 To CHANNEL_COUNT

                If channelIndex <> (baseSelectedChannel + channelSelected) AndAlso chanelGroup(channelIndex) = currentGroup Then

                    If (channelIndex > baseSelectedChannel And channelIndex <= baseSelectedChannel + 24) Then

                        trackbarScreenIndex = channelIndex - baseSelectedChannel - 1
                        helperFunctions.SetTrackbarLevel(levelTrackbar(trackbarScreenIndex), sender.Value)

                    End If

                    channelLevel(channelIndex) = sender.Value

                End If

            Next

        End If



        channelLevel(baseSelectedChannel + channelSelected) = sender.Value
        ApplyLightingLevels()

    End Sub

    Private Sub FaderScrollBar_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles FaderScrollBar.Scroll

        DrawTrackbarControls(FaderScrollBar.Value)

    End Sub

    Private Sub DrawTrackbarControls(ByVal baseBottomChanel)

        Dim blockIndex As Integer
        Dim chanelIndex As Integer
        Dim channelLabel() As Label = {ChannelLabel0, ChannelLabel1, ChannelLabel2, ChannelLabel3, ChannelLabel4, ChannelLabel5, ChannelLabel6, ChannelLabel7}
        Dim descriptorLabel() As Label = {DecriptorLabel0, DecriptorLabel1, DecriptorLabel2, DecriptorLabel3, DecriptorLabel4, DecriptorLabel5, DecriptorLabel6, DecriptorLabel7, DecriptorLabel8, DecriptorLabel9, DecriptorLabel10, DecriptorLabel11, DecriptorLabel12, DecriptorLabel13, DecriptorLabel14, DecriptorLabel15, DecriptorLabel16, DecriptorLabel17, DecriptorLabel18, DecriptorLabel19, DecriptorLabel20, DecriptorLabel21, DecriptorLabel22, DecriptorLabel23}
        Dim groupLabel() As Label = {GroupLabel0, GroupLabel1, GroupLabel2, GroupLabel3, GroupLabel4, GroupLabel5, GroupLabel6, GroupLabel7, GroupLabel8, GroupLabel9, GroupLabel10, GroupLabel11, GroupLabel12, GroupLabel13, GroupLabel14, GroupLabel15, GroupLabel16, GroupLabel17, GroupLabel18, GroupLabel19, GroupLabel20, GroupLabel21, GroupLabel22, GroupLabel23}
        Dim levelTrackbar() As TrackBar = {TrackBar1, TrackBar2, TrackBar3, TrackBar4, TrackBar5, TrackBar6, TrackBar7, TrackBar8, TrackBar9, TrackBar10, TrackBar11, TrackBar12, TrackBar13, TrackBar14, TrackBar15, TrackBar16, TrackBar17, TrackBar18, TrackBar19, TrackBar20, TrackBar21, TrackBar22, TrackBar23, TrackBar24}
        Dim channelIndex() As TextBox = {ChannelIndex1Text, ChannelIndex2Text, ChannelIndex3Text, ChannelIndex4Text, ChannelIndex5Text, ChannelIndex6Text, ChannelIndex7Text, ChannelIndex8Text, ChannelIndex9Text, ChannelIndex10Text, ChannelIndex11Text, ChannelIndex12Text, ChannelIndex13Text, ChannelIndex14Text, ChannelIndex15Text, ChannelIndex16Text, ChannelIndex17Text, ChannelIndex18Text, ChannelIndex19Text, ChannelIndex20Text, ChannelIndex21Text, ChannelIndex22Text, ChannelIndex23Text, ChannelIndex24Text}

        Dim singleDescriptor As String

        baseSelectedChannel = baseBottomChanel * 24

        For blockIndex = 0 To 7

            chanelIndex = blockIndex * 3

            helperFunctions.SetLabelText(channelLabel(blockIndex), baseSelectedChannel + chanelIndex + 1)

            helperFunctions.SetLabelText(groupLabel(chanelIndex), chanelGroup(baseSelectedChannel + chanelIndex + 1))
            helperFunctions.SetLabelText(groupLabel(chanelIndex + 1), chanelGroup(baseSelectedChannel + chanelIndex + 2))
            helperFunctions.SetLabelText(groupLabel(chanelIndex + 2), chanelGroup(baseSelectedChannel + chanelIndex + 3))

            singleDescriptor = IIf(channelBlend(baseSelectedChannel + chanelIndex + 1), "*", " ") & chanelDescriptor(baseSelectedChannel + chanelIndex + 1)
            helperFunctions.SetLabelText(descriptorLabel(chanelIndex), singleDescriptor)
            singleDescriptor = IIf(channelBlend(baseSelectedChannel + chanelIndex + 2), "*", " ") & chanelDescriptor(baseSelectedChannel + chanelIndex + 2)
            helperFunctions.SetLabelText(descriptorLabel(chanelIndex + 1), singleDescriptor)
            singleDescriptor = IIf(channelBlend(baseSelectedChannel + chanelIndex + 3), "*", " ") & chanelDescriptor(baseSelectedChannel + chanelIndex + 3)
            helperFunctions.SetLabelText(descriptorLabel(chanelIndex + 2), singleDescriptor)


            helperFunctions.SetTrackbarLevel(levelTrackbar(chanelIndex), channelLevel(baseSelectedChannel + chanelIndex + 1))
            helperFunctions.SetTrackbarLevel(levelTrackbar(chanelIndex + 1), channelLevel(baseSelectedChannel + chanelIndex + 2))
            helperFunctions.SetTrackbarLevel(levelTrackbar(chanelIndex + 2), channelLevel(baseSelectedChannel + chanelIndex + 3))

            helperFunctions.SetTextBoxText(channelIndex(chanelIndex), channelIndexing(baseSelectedChannel + chanelIndex + 1))
            helperFunctions.SetTextBoxText(channelIndex(chanelIndex + 1), channelIndexing(baseSelectedChannel + chanelIndex + 2))
            helperFunctions.SetTextBoxText(channelIndex(chanelIndex + 2), channelIndexing(baseSelectedChannel + chanelIndex + 3))

        Next

    End Sub

    Private Sub SetCueButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetCueButton.Click

        Dim channelIndex As Integer

        For channelIndex = 0 To CHANNEL_COUNT
            scriptList(currentScriptIndex).lightingCues(currentCueIndex).channel(channelIndex) = channelLevel(channelIndex)
        Next

    End Sub

    Private Sub SaveScriptButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveScriptButton.Click

        Dim fileStreamObject As FileStream
        Dim fileStreamWriterObject As StreamWriter
        Dim itemIndex As Integer
        Dim cueIndex As Integer
        Dim channelIndex As Integer

        Try

            fileStreamObject = New FileStream(Application.StartupPath.ToString & "\Lighting\" & scriptList(currentScriptIndex).fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
            fileStreamWriterObject = New StreamWriter(fileStreamObject)

            ' software version
            fileStreamWriterObject.WriteLine("200,")

            ' patch list
            fileStreamWriterObject.Write(itemIndex.ToString & "0,0,0,0,0,")
            For itemIndex = 0 To CHANNEL_COUNT
                fileStreamWriterObject.Write(itemIndex.ToString & ",")
            Next
            fileStreamWriterObject.WriteLine()

            ' initial light levels, we ignore these
            fileStreamWriterObject.WriteLine("0,0")

            ' get current CurrentCue/NextCue/Crossfader Values, we ignore these
            fileStreamWriterObject.WriteLine("0,0")

            ' cue count, treat as an estimate, ie ignore
            fileStreamWriterObject.WriteLine(scriptList(currentScriptIndex).cueTotal.ToString)

            For cueIndex = 0 To scriptList(currentScriptIndex).cueTotal - 1

                fileStreamWriterObject.Write(cueIndex.ToString & ",")
                fileStreamWriterObject.Write(scriptList(currentScriptIndex).lightingCues(cueIndex).name & ",")
                fileStreamWriterObject.Write(scriptList(currentScriptIndex).lightingCues(cueIndex).fadeInDuration.ToString & ",")
                fileStreamWriterObject.Write(scriptList(currentScriptIndex).lightingCues(cueIndex).fadeOutDuration.ToString & ",")
                fileStreamWriterObject.Write(scriptList(currentScriptIndex).lightingCues(cueIndex).isFollow.ToString & ",")
                fileStreamWriterObject.Write(scriptList(currentScriptIndex).lightingCues(cueIndex).followTime.ToString & ",")

                For channelIndex = 1 To CHANNEL_COUNT
                    fileStreamWriterObject.Write(scriptList(currentScriptIndex).lightingCues(cueIndex).channel(channelIndex).ToString & ",")
                Next

                fileStreamWriterObject.WriteLine()
            Next

            fileStreamWriterObject.Flush()
            fileStreamWriterObject.Close()
            fileStreamObject.Close()
        Catch ex As Exception
        End Try

    End Sub

End Class
