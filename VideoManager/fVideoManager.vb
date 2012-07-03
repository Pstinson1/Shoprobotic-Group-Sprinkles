'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

' Imports
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Imports System
Imports System.IO
Imports DebugWindow
Imports DirectShowLib
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Drawing


Public Class fVideoManager

    ' Enumerations
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Enum CameraListItem

        PICK_HEAD_VIEW
        CUSTOMER_VIEW
        MECH_VIEW

    End Enum

    ' Structure
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Structure CaremaItemStruct

        Dim theDevice As IBaseFilter
        Dim theCompressor As IBaseFilter
        Dim mediaControl As IMediaControl
        Dim graphBuilder As IGraphBuilder
        Dim videoWindow As IVideoWindow
        Dim m_rot As DsROTEntry
        Dim captureGraphBuilder As ICaptureGraphBuilder2
        Dim captureIsActive As Boolean
        Dim displayWindow As fVideoView
        Dim devicePath As String
        Dim deviceInstance As Integer
        Dim compressorPath As String
        Dim frameSize As Size
        Dim frameRate As Integer
        Dim descriptionLabel As Label

    End Structure

    ' Delagates
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Delegate Sub StartRecordingCallback(ByVal cameraIndex As CameraListItem)
    Private Delegate Sub StopRecordingCallback(ByVal cameraIndex As CameraListItem)
    Private Delegate Sub ShowViewCallback(ByVal cameraIndex As CameraListItem, ByVal newState As Boolean)
    Private Delegate Sub SetLabelTextCallback(ByVal labelToChange As Label, ByVal messageText As String)

    ' Variables
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory

    Private cameraItem(3) As CaremaItemStruct
    Private cameraDeviceList() As DsDevice
    Private compressorDeviceList() As DsDevice

    ' Initialise
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        ' this line is suprizing important, it forced .net to assign a widow handle enabling the reader invoke to work..
        Dim temporaryHandle As Integer = Me.Handle

        cameraItem(0).descriptionLabel = PickHeadLabel
        cameraItem(1).descriptionLabel = CustomerLabel
        cameraItem(2).descriptionLabel = MechanismLabel

        debugInformation = debugInformationFactory.GetManager()

        ' ensure that the video path exists
        If Not Directory.Exists("C:\Control\Video") Then _
            Directory.CreateDirectory("C:\Control\Video")

        ' get the lists of compressor and camera devices
        cameraDeviceList = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice)
        compressorDeviceList = DsDevice.GetDevicesOfCat(FilterCategory.VideoCompressorCategory)

        ShowAvailableDevicesAndCompressors()

    End Sub

    ' ConfigureVideoStream
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ConfigureVideoStream(ByVal cameraIndex As CameraListItem, ByVal frameWidth As Integer, ByVal frameHeight As Integer, ByVal frameRate As Integer)

        Dim o As Object = Nothing
        Dim videoStreamConfig As IAMStreamConfig
        Dim videoHeaderInfo As VideoInfoHeader
        Dim cat As Guid = PinCategory.Capture
        Dim med As Guid = MediaType.Interleaved
        Dim iid As Guid = GetType(IAMStreamConfig).GUID
        Dim hr As Integer

        hr = cameraItem(cameraIndex).captureGraphBuilder.FindInterface(cat, med, cameraItem(cameraIndex).theDevice, iid, o)

        If hr <> 0 Then
            ' If not found, try looking for a video media type
            med = MediaType.Video
            hr = cameraItem(cameraIndex).captureGraphBuilder.FindInterface(cat, med, cameraItem(cameraIndex).theDevice, iid, o)

            If hr <> 0 Then
                o = Nothing
            End If
        End If

        Try
            videoStreamConfig = TryCast(o, IAMStreamConfig)
            videoHeaderInfo = GetStreamConfigSetting(videoStreamConfig)

            videoHeaderInfo.BmiHeader.Width = frameWidth
            videoHeaderInfo.BmiHeader.Height = frameHeight
            videoHeaderInfo.AvgTimePerFrame = (10000000 / frameRate)

            SetStreamConfigSetting(videoStreamConfig, videoHeaderInfo)

        Catch ex As Exception

        End Try

    End Sub

    ' GetStreamConfigSetting
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function GetStreamConfigSetting(ByVal streamConfig As IAMStreamConfig) As VideoInfoHeader

        Dim formatStruct As VideoInfoHeader = Nothing
        Dim formatStructPointer As DirectShowLib.AMMediaType = Nothing
        Dim hr As Integer

        If streamConfig Is Nothing Then
            Throw New NotSupportedException()
        End If

        Try
            ' Get the current format info
            hr = streamConfig.GetFormat(formatStructPointer)

            If hr <> 0 Then
                Marshal.ThrowExceptionForHR(hr)
            End If

            If formatStructPointer.formatType = FormatType.VideoInfo Then

                formatStruct = New VideoInfoHeader()

                ' Retrieve the nested structure
                Marshal.PtrToStructure(formatStructPointer.formatPtr, formatStruct)

            Else
                Throw New NotSupportedException("This device does not support a recognized format block.")
            End If

        Finally
            DsUtils.FreeAMMediaType(formatStructPointer)

        End Try

        Return (formatStruct)

    End Function

    ' GetStreamConfigSetting
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function SetStreamConfigSetting(ByVal streamConfig As IAMStreamConfig, ByVal formatStruct As VideoInfoHeader)

        Dim hr As Integer
        Dim returnResult As Boolean
        Dim formatStructPointer As DirectShowLib.AMMediaType = Nothing

        If streamConfig Is Nothing Then
            Throw New NotSupportedException()
        End If

        ' Get the current format info
        hr = streamConfig.GetFormat(formatStructPointer)

        If (hr <> 0) Then
            Marshal.ThrowExceptionForHR(hr)
        End If

        ' PtrToStructure copies the data so we need to copy it back
        Marshal.StructureToPtr(formatStruct, formatStructPointer.formatPtr, False)

        'Save the changes
        hr = streamConfig.SetFormat(formatStructPointer)
        If (hr <> 0) Then
            Marshal.ThrowExceptionForHR(hr)
        End If

        DsUtils.FreeAMMediaType(formatStructPointer)

        Return returnResult

    End Function

    ' ShowAvailableDevicesAndCompressors
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ShowAvailableDevicesAndCompressors()

        compressorDeviceList = DsDevice.GetDevicesOfCat(FilterCategory.VideoCompressorCategory)
        For Each device As DsDevice In cameraDeviceList
            DeviceListBox.Items.Add(device.Name)
        Next

        For Each device As DsDevice In compressorDeviceList
            CompressorListBox.Items.Add(device.Name)
        Next

    End Sub

    ' SetView
    ' when showing the recording on screen, where do we want the windows? 
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub SetView(ByVal cameraIndex As CameraListItem, ByVal videoLocation As Point, ByVal videoSize As Size, ByVal frameSize As Size, ByVal frameRate As Integer)

        cameraItem(cameraIndex).displayWindow.Location = videoLocation
        cameraItem(cameraIndex).displayWindow.Size = videoSize
        cameraItem(cameraIndex).frameSize = frameSize
        cameraItem(cameraIndex).frameRate = frameRate

    End Sub

    ' ShowView
    ' show or hide the video.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub ShowView(ByVal cameraIndex As CameraListItem, ByVal newState As Boolean)

        If Me.InvokeRequired Then

            Dim d As New ShowViewCallback(AddressOf ShowView)
            Me.Invoke(d, New Object() {cameraIndex, newState})

        Else

            If (newState) Then
                cameraItem(cameraIndex).displayWindow.Show()

            Else
                cameraItem(cameraIndex).displayWindow.Hide()
                Application.DoEvents()

            End If

        End If

    End Sub

    ' AddCamera
    ' create the filter for the selected video input device, then create the filter for the selected video compressor
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub AddCamera(ByVal cameraIndex As CameraListItem, ByVal devicePath As String, ByVal deviceInstance As Integer, ByVal compressorPath As String, ByVal viewDescription As String)

        cameraItem(cameraIndex).displayWindow = New fVideoView()
        cameraItem(cameraIndex).displayWindow.Initialise(viewDescription)

        cameraItem(cameraIndex).devicePath = devicePath
        cameraItem(cameraIndex).deviceInstance = deviceInstance
        cameraItem(cameraIndex).compressorPath = compressorPath

        SetLabelText(cameraItem(cameraIndex).descriptionLabel, devicePath & " - " & deviceInstance.ToString & ControlChars.NewLine & compressorPath)

    End Sub

    ' RealeaseCameraComObjects
    ' Release com objects for a given camera
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Sub RealeaseCameraComObjects(ByVal cameraIndex As CameraListItem)

        ' release COM objects
        If Not cameraItem(cameraIndex).theDevice Is Nothing Then
            Try
                Marshal.ReleaseComObject(cameraItem(cameraIndex).theDevice)
                cameraItem(cameraIndex).theDevice = Nothing
            Catch ex As Exception
            End Try
        End If

        If Not cameraItem(cameraIndex).theCompressor Is Nothing Then
            Try
                Marshal.ReleaseComObject(cameraItem(cameraIndex).theCompressor)
                cameraItem(cameraIndex).theCompressor = Nothing
            Catch ex As Exception
            End Try
        End If

    End Sub

    ' StartRecording
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub StartRecording(ByVal cameraIndex As CameraListItem)

        If Me.InvokeRequired Then

            Try
                Dim d As New StartRecordingCallback(AddressOf StartRecording)
                Me.Invoke(d, New Object() {cameraIndex})
            Catch ex As Exception
            End Try

        Else
            debugInformation.Progress(fDebugWindow.Level.INF, 1800, "Start video recording", True)

            cameraItem(cameraIndex).theDevice = CreateCameraFilter(cameraItem(cameraIndex).devicePath, cameraItem(cameraIndex).deviceInstance)
            cameraItem(cameraIndex).theCompressor = CreateCompressorFilter(cameraItem(cameraIndex).compressorPath)

            If cameraItem(cameraIndex).theDevice Is Nothing Or cameraItem(cameraIndex).theCompressor Is Nothing Then
                debugInformation.Progress(fDebugWindow.Level.INF, 1801, "either no codec or no camera", True)

            ElseIf cameraItem(cameraIndex).captureIsActive = True Then
                debugInformation.Progress(fDebugWindow.Level.INF, 1802, "Capture is already running", True)

            Else

                ' create and run the graph      
                Try
                    cameraItem(cameraIndex).captureIsActive = True
                    InitialiseGraph(cameraIndex)

                    If Not cameraItem(cameraIndex).mediaControl Is Nothing Then

                        cameraItem(cameraIndex).mediaControl.Run()

                    End If


                Catch ex As Exception
                    debugInformation.Progress(fDebugWindow.Level.INF, 1803, "Exception: Start Recording " & cameraIndex.ToString & " " & ex.Message, True)
                End Try

            End If

        End If

    End Sub

    ' StopRecording
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub StopRecording(ByVal cameraIndex As CameraListItem)

        If InvokeRequired Then

            Try
                Dim d As New StopRecordingCallback(AddressOf StopRecording)
                Me.Invoke(d, New Object() {cameraIndex})
            Catch ex As Exception
            End Try

        Else

            debugInformation.Progress(fDebugWindow.Level.INF, 1804, "Stop video recording", True)

            If cameraItem(cameraIndex).theDevice Is Nothing Or cameraItem(cameraIndex).theCompressor Is Nothing Then
                debugInformation.Progress(fDebugWindow.Level.INF, 1805, "either no codec or no camera", True)

            ElseIf cameraItem(cameraIndex).captureIsActive = False Then
                debugInformation.Progress(fDebugWindow.Level.INF, 1806, "Capture is not running", True)

            Else

                Try

                    ' stop the Graph
                    cameraItem(cameraIndex).mediaControl.Stop()

                    ShutdownGraph(cameraIndex)
                    RealeaseCameraComObjects(cameraIndex)

                    cameraItem(cameraIndex).captureIsActive = False

                Catch ex As Exception
                    debugInformation.Progress(fDebugWindow.Level.INF, 1807, "Exception: Stop Recording " & cameraIndex.ToString & " " & ex.Message, True)
                End Try

            End If
        End If

    End Sub

    ' fVideoCapture_FormClosing
    ' stop the form from closing, just hide it
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub fVideoCapture_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        e.Cancel = True
        Hide()

    End Sub


    ' InitialiseGraph
    ' create a graph for this recording..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub InitialiseGraph(ByVal cameraIndex As CameraListItem)

        Dim aviFilename As String
        Dim mux As IBaseFilter = Nothing
        Dim sink As IFileSinkFilter = Nothing
        Dim currentPanel = cameraItem(cameraIndex).displayWindow.Panel()
        Dim hr As Integer

        If cameraItem(cameraIndex).theDevice Is Nothing Then
            Return
        End If

        Try

            'Create the Graph
            cameraItem(cameraIndex).graphBuilder = DirectCast(New FilterGraph(), IGraphBuilder)

            'Create the Capture Graph Builder
            cameraItem(cameraIndex).captureGraphBuilder = DirectCast(New CaptureGraphBuilder2(), ICaptureGraphBuilder2)

            'Create the media control for controlling the graph
            cameraItem(cameraIndex).mediaControl = DirectCast(cameraItem(cameraIndex).graphBuilder, IMediaControl)

            ' Attach the filter graph to the capture graph
            hr = cameraItem(cameraIndex).captureGraphBuilder.SetFiltergraph(cameraItem(cameraIndex).graphBuilder)
            DsError.ThrowExceptionForHR(hr)

            'Add the Video input device to the graph
            hr = cameraItem(cameraIndex).graphBuilder.AddFilter(cameraItem(cameraIndex).theDevice, "source filter")
            DsError.ThrowExceptionForHR(hr)

            'Add the Video compressor filter to the graph
            hr = cameraItem(cameraIndex).graphBuilder.AddFilter(cameraItem(cameraIndex).theCompressor, "compressor filter")
            DsError.ThrowExceptionForHR(hr)

            ' set the size and the frame rate
            ConfigureVideoStream(cameraIndex, cameraItem(cameraIndex).frameSize.Width, cameraItem(cameraIndex).frameSize.Height, cameraItem(cameraIndex).frameRate)

            ' create the file writer part of the graph. SetOutputFileName does this for us, and returns the mux and sink
            aviFilename = "C:\Control\Video\Video_" & cameraIndex.ToString & "_" & Format(Now.Month, "##00") & Format(Now.Day, "##00") & Format(Now.Year, "####0000") & "_" & Format(Now.Hour, "##00") & Format(Now.Minute, "##00") & Format(Now.Second, "##00") & ".avi"

            hr = cameraItem(cameraIndex).captureGraphBuilder.SetOutputFileName(MediaSubType.Avi, aviFilename, mux, sink)
            DsError.ThrowExceptionForHR(hr)

            ' render any preview pin of the device
            hr = cameraItem(cameraIndex).captureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, cameraItem(cameraIndex).theDevice, Nothing, Nothing)
            DsError.ThrowExceptionForHR(hr)

            ' connect the device and compressor to the mux to render the capture part of the graph
            hr = cameraItem(cameraIndex).captureGraphBuilder.RenderStream(PinCategory.Capture, MediaType.Video, cameraItem(cameraIndex).theDevice, cameraItem(cameraIndex).theCompressor, mux)
            DsError.ThrowExceptionForHR(hr)

            cameraItem(cameraIndex).m_rot = New DsROTEntry(cameraItem(cameraIndex).graphBuilder)

            ' get the video window from the graph
            cameraItem(cameraIndex).videoWindow = DirectCast(cameraItem(cameraIndex).graphBuilder, IVideoWindow)

            'Set the owener of the videoWindow to an IntPtr of some sort (the Handle of any control - could be a form / button etc.)
            hr = cameraItem(cameraIndex).videoWindow.put_Owner(currentPanel.Handle)
            DsError.ThrowExceptionForHR(hr)

            'Set the style of the video window
            hr = cameraItem(cameraIndex).videoWindow.put_WindowStyle(WindowStyle.Child Or WindowStyle.ClipChildren)
            DsError.ThrowExceptionForHR(hr)

            ' Position video window in client rect of main application window
            hr = cameraItem(cameraIndex).videoWindow.SetWindowPosition(0, 0, currentPanel.Width, currentPanel.Height)
            DsError.ThrowExceptionForHR(hr)

            ' Make the video window visible
            hr = cameraItem(cameraIndex).videoWindow.put_Visible(OABool.[True])
            DsError.ThrowExceptionForHR(hr)

            Marshal.ReleaseComObject(mux)
            Marshal.ReleaseComObject(sink)
            Marshal.ReleaseComObject(cameraItem(cameraIndex).captureGraphBuilder)

        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.INF, 1808, "Exception: Initialise Graph " & cameraIndex.ToString & " " & ex.Message, True)
        End Try
    End Sub

    ' ShutdownGraph
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ShutdownGraph(ByVal cameraIndex As CameraListItem)

        Try
            ' release COM objects
            Marshal.ReleaseComObject(cameraItem(cameraIndex).mediaControl)
            Marshal.ReleaseComObject(cameraItem(cameraIndex).graphBuilder)

            If Not cameraItem(cameraIndex).m_rot Is Nothing Then

                cameraItem(cameraIndex).m_rot.Dispose()
                cameraItem(cameraIndex).m_rot = Nothing
            End If
        Catch ex As Exception
        End Try

    End Sub


    ' CreateFilter
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function CreateCameraFilter(ByVal friendlyname As String, ByVal requiredInstance As Integer) As IBaseFilter

        Dim source As Object = Nothing
        Dim iid As Guid = GetType(IBaseFilter).GUID
        Dim instanceCount As Integer = 0

        debugInformation.Progress(fDebugWindow.Level.INF, 1809, "Looking for Camera " & friendlyname & ", Instance # " & requiredInstance, False)

        For Each device As DsDevice In cameraDeviceList

            If device.Name.CompareTo(friendlyname) = 0 Then

                If instanceCount = requiredInstance Then

                    debugInformation.Progress(fDebugWindow.Level.INF, 1810, "Found " & device.DevicePath.ToString, False)
                    device.Mon.BindToObject(Nothing, Nothing, iid, source)
                End If

                instanceCount += 1
            End If

        Next

        Return (DirectCast(source, IBaseFilter))

    End Function

    ' CreateFilter
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Function CreateCompressorFilter(ByVal friendlyname As String) As IBaseFilter

        Dim source As Object = Nothing
        Dim iid As Guid = GetType(IBaseFilter).GUID

        For Each device As DsDevice In compressorDeviceList

            If device.Name.CompareTo(friendlyname) = 0 Then

                device.Mon.BindToObject(Nothing, Nothing, iid, source)

            End If

        Next

        Return (DirectCast(source, IBaseFilter))

    End Function

    ' SetLabelText
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SetLabelText(ByVal labelToChange As Label, ByVal messageText As String)

        If labelToChange.InvokeRequired Then

            Dim d As New SetLabelTextCallback(AddressOf SetLabelText)            ' invoke this message in the correct thread
            Me.Invoke(d, New Object() {labelToChange, messageText})
        Else

            labelToChange.Text = messageText
        End If
    End Sub

    ' the show buttons
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ShowCustomerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCustomerButton.Click
        ShowView(CameraListItem.CUSTOMER_VIEW, True)
    End Sub

    Private Sub ShowPickHeadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowPickHeadButton.Click
        ShowView(CameraListItem.PICK_HEAD_VIEW, True)
    End Sub

    Private Sub ShowMechanismButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowMechanismButton.Click
        ShowView(CameraListItem.MECH_VIEW, True)
    End Sub

    ' the hide buttons
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub HidePickHeadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HidePickHeadButton.Click
        ShowView(CameraListItem.PICK_HEAD_VIEW, False)
    End Sub

    Private Sub HideCustomerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideCustomerButton.Click
        ShowView(CameraListItem.CUSTOMER_VIEW, False)
    End Sub

    Private Sub HideMechanismButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideMechanismButton.Click
        ShowView(CameraListItem.MECH_VIEW, False)
    End Sub

    ' the record buttons
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub RecordPickHeadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordPickHeadButton.Click
        StartRecording(CameraListItem.PICK_HEAD_VIEW)
    End Sub

    Private Sub RecordCustomerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordCustomerButton.Click
        StartRecording(CameraListItem.CUSTOMER_VIEW)
    End Sub

    Private Sub RecordMechanismButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordMechanismButton.Click
        StartRecording(CameraListItem.MECH_VIEW)
    End Sub

    ' the stop buttons
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub StopPickHeadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopPickHeadButton.Click
        StopRecording(CameraListItem.PICK_HEAD_VIEW)
    End Sub

    Private Sub StopCustomerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopCustomerButton.Click
        StopRecording(CameraListItem.CUSTOMER_VIEW)
    End Sub

    Private Sub StopMechanismButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopMechanismButton.Click
        StopRecording(CameraListItem.MECH_VIEW)
    End Sub

    Private Sub HideButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideButton.Click
        Hide()
    End Sub

End Class

' class cVideoManagerFactory
' ensure only one manager is created
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Public Class cVideoManagerFactory

    ' Varaibles
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Shared videoManager As fVideoManager = Nothing

    Public Function GetManager() As fVideoManager

        If IsNothing(videoManager) Then

            videoManager = New fVideoManager

        End If

        Return videoManager

    End Function

End Class

