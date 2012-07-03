'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

' uPosition
' deal with the positioning of the head and the storage in the database
' ----------------------------------------------------------------------------------------------------------------------------------------------------
Imports System.Data.SqlClient
Imports System.IO
Imports SettingsManager
Imports DebugWindow
Imports HelperFunctions
Imports SerialManager
Imports FridgeManager
Imports MechInterface

Public Class uPosition

    ' external API calls
    Declare Auto Function SetFocus Lib "USER32.DLL" (ByVal lpClassName As IntPtr) As IntPtr

    ' manager components
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private fridgeManager As fFridgeManager
    Private fridgeManagerFactory As cFridgeManagerFactory = New cFridgeManagerFactory
    Private mechManager As fMechInterface
    Private mechManagerFactory As cMechInterfaceFactory = New cMechInterfaceFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' events
    Public Event NameIdentifierChanges(ByVal changedName As String)
    Public Event EnableExplorer(ByVal requiredState As Boolean)
    Public Event ActiveStatusChanges(ByVal activeState As String)

    ' enumerations
    Enum KeypadTargetEnum
        xPositionTargetText
        yPositionTargetText
        vdoText
        pickAttemptsText
        noneSelected
    End Enum

    ' structures
    Structure PositionStore

        Dim productId As Integer
        Dim identifier As Integer
        Dim position As Point
        Dim vdo As Integer
        Dim fridge As Integer
        Dim isActive As Boolean
        Dim stockAvailable As Integer
        Dim restockLevel As Integer
        Dim pickAttempts As Integer
        Dim itemsInStock As Integer

        ' new bin development.
        Dim containerNumber As String
        Dim xVisualPosition As Integer
        Dim yVisualPosition As Integer

    End Structure

    ' variables
    Private currentHeadPosition As Point
    Private currentPositionStore As PositionStore
    Private dataLoading As Boolean
    Private testProductVend As New fTestVend
    Private focusedTextBox As TextBox
    Private keypadTarget As KeypadTargetEnum = KeypadTargetEnum.noneSelected
    Private fridgeDoorIsSafe As Boolean = False
    Private currentSaleId As Integer = 0
    Private usesContainers As Boolean = False

    ' constants
    Private Const NO_FRIDGE = -1
    Private Const HEAD_LOST = -9999

    ' Load
    ' fire up the control
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub uPosition_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    ' Initialise
    ' tell the settings manager the connectuion string
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()


        settingsManager = settingsManagerFactory.GetManager()
        debugInformation = debugInformationFactory.GetManager()
        serialManager = serialManagerFactory.GetManager()
        mechManager = mechManagerFactory.GetManager()
        helperFunctions = helperFunctionsFactory.GetManager()

        ' fire up the fridge management
        fridgeManager = fridgeManagerFactory.GetManager
        fridgeManager.AddCallback(AddressOf FridgeEvent)

        ' query the current position
        mechManager.SendMessageToVMC("GETRACKPOS")
        mechManager.AddCallback(AddressOf MechanismEvent)


        If settingsManager.ConnectToDatabase() Then
            usesContainers = settingsManager.TableColumnExists("productposition", "containernumber")
            settingsManager.DisconnectFromDatabase()
        End If

        If Not usesContainers Then

            ContainerInfoLabel.Text = "This database does not hold container or bin information"
            ContainerNumberText.Enabled = False
            LayoutXText.Enabled = False
            LayoutYText.Enabled = False
            ContainerLabel1.Enabled = False
            ContainerLabel2.Enabled = False
            ContainerLabel3.Enabled = False

        End If

    End Sub

    Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String) As Boolean

        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
        End Try
        Return bAns
    End Function

    ' MechanismEvent
    ' things are happenning on the vend process
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Sub MechanismEvent(ByVal eventCode As fMechInterface.Message, ByVal integerValue1 As Integer, ByVal integerValue2 As Integer)

        Dim eventDescription As String = ""
        Dim eventLevel As fDebugWindow.Level

        Select Case eventCode

            Case fMechInterface.Message.MECH_DELIVERY_STARTS

            Case fMechInterface.Message.MECH_FRIDGE_OPEN
                SetFridgeDoorProgress(integerValue1 + 1, IIf(integerValue2, "Door opened okay", "Door failed to open"))

            Case fMechInterface.Message.MECH_FRIDGE_CLOSE
                SetFridgeDoorProgress(integerValue1 + 1, IIf(integerValue2, "Door closed okay", "Door failed to close"))

            Case fMechInterface.Message.MECH_VEND_ACTIVITY

            Case fMechInterface.Message.MECH_PRODUCT_OFFERED
                testProductVend.Complete(True)

            Case fMechInterface.Message.MECH_DELIVERY_COMPLETES

            Case fMechInterface.Message.MECH_DELIVERY_FAILS
                testProductVend.Complete(False)

            Case fMechInterface.Message.MECH_PC_SERVICE_REQUEST

            Case fMechInterface.Message.MECH_POSITION

                If (integerValue1 = -1 Or integerValue2 = -1) Then

                    currentHeadPosition.X = HEAD_LOST
                    currentHeadPosition.Y = HEAD_LOST

                Else
                    currentHeadPosition.X = integerValue1
                    currentHeadPosition.Y = integerValue2

                End If

                UpdateActualPositon(currentHeadPosition)


            Case fMechInterface.Message.MECH_VEND_INFO

                testProductVend.DisplayInfoCode(integerValue1)

                If debugInformation.VMCEventDetails(eventLevel, integerValue1, eventDescription) Then

                    debugInformation.Progress(eventLevel, integerValue1, "VMC: " & eventDescription, True)
                Else

                    debugInformation.Progress(eventLevel, integerValue1, "VMC: unrecognised code=" & integerValue1.ToString, True)
                End If

        End Select

    End Sub

    ' FridgeEvent
    ' things are happenning on the fridge
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Sub FridgeEvent(ByVal eventCode As fFridgeManager.Message, ByVal doorParameter As Integer)

        Select Case eventCode

            Case fFridgeManager.Message.FRD_COMMS_OK

            Case fFridgeManager.Message.FRD_COMMS_FAIL

            Case fFridgeManager.Message.FRD_CLOSE_FAIL
                SetFridgeDoorProgress(doorParameter + 1, "Door fails to close in time")

            Case fFridgeManager.Message.FRD_CLOSE_OK
                SetFridgeDoorProgress(doorParameter + 1, "Door closes ok")

            Case fFridgeManager.Message.FRD_OPEN_FAIL
                SetFridgeDoorProgress(doorParameter + 1, "Door fails to open in time")

            Case fFridgeManager.Message.FRD_OPEN_OK
                SetFridgeDoorProgress(doorParameter + 1, "Door opens ok")

            Case fFridgeManager.Message.FRD_DOORS_SAFE
                helperFunctions.SetButtonEnable(MoveButton1, True)
                helperFunctions.SetButtonEnable(MoveButton2, True)
                helperFunctions.SetButtonEnable(MoveButton8, True)
                helperFunctions.SetButtonEnable(MoveButton9, True)

                helperFunctions.SetButtonEnable(TestPositionButton, True)
                helperFunctions.SetButtonEnable(TestVendButton, True)

                fridgeDoorIsSafe = True

            Case fFridgeManager.Message.FRD_DOORS_NOT_SAFE
                helperFunctions.SetButtonEnable(MoveButton1, False)
                helperFunctions.SetButtonEnable(MoveButton2, False)
                helperFunctions.SetButtonEnable(MoveButton8, False)
                helperFunctions.SetButtonEnable(MoveButton9, False)

                If ActualX.Text = "????" Then
                    helperFunctions.SetButtonEnable(TestPositionButton, False)
                    helperFunctions.SetButtonEnable(TestVendButton, False)
                End If

                fridgeDoorIsSafe = False

        End Select

    End Sub

    ' SetFridgeDoorProgress
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SetFridgeDoorProgress(ByVal doorIndex As Integer, ByVal progressMessage As String)

        If doorIndex = 1 Then helperFunctions.SetLabelText(Fridge1ProgressLabel, progressMessage)
        If doorIndex = 2 Then helperFunctions.SetLabelText(Fridge2ProgressLabel, progressMessage)
        If doorIndex = 3 Then helperFunctions.SetLabelText(Fridge3ProgressLabel, progressMessage)
        If doorIndex = 4 Then helperFunctions.SetLabelText(Fridge4ProgressLabel, progressMessage)
        If doorIndex = 5 Then helperFunctions.SetLabelText(Fridge5ProgressLabel, progressMessage)
        If doorIndex = 6 Then helperFunctions.SetLabelText(Fridge6ProgressLabel, progressMessage)
        If doorIndex = 7 Then helperFunctions.SetLabelText(Fridge7ProgressLabel, progressMessage)

    End Sub

    Public Sub RecoverPositionContainerData(ByVal positionId As Integer)

        Dim resultSet As SqlDataReader

        If usesContainers Then

            If settingsManager.ConnectToDatabase() Then

                ' general positioning data
                resultSet = settingsManager.RunDatabaseQuery("select isnull(containernumber,'') as container, isnull(xvisualposition, -1) as xpos, isnull(yvisualposition, -1)  as ypos from productposition where posid=" & positionId)

                If Not resultSet Is Nothing Then

                    If resultSet.Read() Then
                        Try
                            currentPositionStore.containerNumber = resultSet("container")
                            currentPositionStore.xVisualPosition = resultSet("xpos")
                            currentPositionStore.yVisualPosition = resultSet("ypos")
                        Catch ex As Exception

                        End Try


                    End If

                    settingsManager.CloseQuery(resultSet)


                End If
                settingsManager.DisconnectFromDatabase()
            End If

        End If

    End Sub

    ' RecoverPositionData
    ' update the display with a new position
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub RecoverPositionData(ByVal positionId As Integer)

        Dim resultSet As SqlDataReader

        If (settingsManager.ConnectToDatabase()) Then

            ' general positioning data
            resultSet = settingsManager.RunDatabaseQuery("select prodid, itemsinstock, xpos, ypos, vdo, fridgeid, pickattempts, restocklevel, isactive From productposition where posid=" & positionId)

            If Not resultSet Is Nothing Then

                If resultSet.Read() Then
                    Try
                        currentPositionStore.productId = resultSet("prodid")
                        currentPositionStore.position.X = resultSet("xpos")
                        currentPositionStore.position.Y = resultSet("ypos")
                        currentPositionStore.vdo = resultSet("vdo")
                        currentPositionStore.fridge = resultSet("fridgeid")
                        currentPositionStore.isActive = resultSet("isactive")
                        currentPositionStore.restockLevel = resultSet("restocklevel")
                        currentPositionStore.pickAttempts = resultSet("pickattempts")
                        currentPositionStore.itemsInStock = resultSet("itemsinstock")
                    Catch ex As Exception
                    End Try

                End If

                settingsManager.CloseQuery(resultSet)

            End If

            ' get the stock level
            resultSet = settingsManager.RunDatabaseQuery("select ISNULL (itemsinstock, 0) as positionstock from productposition where posid=" & positionId)
            '     resultSet = settingsManager.RunDatabaseQuery("select ISNULL (SUM (adjustment), 0) as positionstock from stockadjustments where posid=" & positionId)

            If Not resultSet Is Nothing Then

                If resultSet.Read() Then
                    currentPositionStore.stockAvailable = resultSet("positionstock")
                End If

                settingsManager.CloseQuery(resultSet)
            End If


            settingsManager.DisconnectFromDatabase()

        End If

    End Sub




    ' LoadItem
    ' recover the position property
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub LoadItem(ByVal itemIndex As Integer)

        dataLoading = True

        currentPositionStore.identifier = itemIndex

        RecoverPositionData(itemIndex)
        RecoverPositionContainerData(itemIndex)

        UpdateTargetPositon(currentPositionStore.position)
        UpdateFridgeSelection(currentPositionStore.fridge)
        UpdateActiveStatus(currentPositionStore.isActive)
        UpdateVdo(currentPositionStore.vdo)
        UpdatePickAttempts(currentPositionStore.pickAttempts)
        UpdateStockInformation()
        UpdateContainer()

        dataLoading = False

        ApplyButton.Enabled = False
        CancelButton.Enabled = False

    End Sub

    ' StorePositionData
    ' store the position
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function StorePositionData() As Boolean

        Dim returnResult As Boolean = False

        If (settingsManager.ConnectToDatabase()) Then

            returnResult = settingsManager.RunDatabaseNonQuery("update productposition set isactive='" & ActiveCheck.Checked & "', xpos = " & currentPositionStore.position.X & ", ypos = " & currentPositionStore.position.Y & ", pickattempts = " & PickAttemptsText.Text & ", vdo = " & VdoText.Text & ", fridgeid = " & currentPositionStore.fridge & " where posid = " & currentPositionStore.identifier)
            settingsManager.DisconnectFromDatabase()

        End If

        Return returnResult

    End Function

    ' MoveButton_Clicked
    ' move the pick head
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub MoveButton_Clicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveButton1.Click, MoveButton2.Click, MoveButton3.Click, MoveButton4.Click, MoveButton5.Click, MoveButton6.Click, MoveButton7.Click, MoveButton8.Click, MoveButton9.Click, MoveButton10.Click, MoveButton11.Click, MoveButton12.Click

        Dim directionParameter = sender.Tag.Split(",")

        If directionParameter.Length = 2 Then

            currentPositionStore.position.X += Convert.ToInt32(directionParameter(0))
            currentPositionStore.position.Y += Convert.ToInt32(directionParameter(1))

            mechManager.AllowASmallVerticalMove()

            TrimPositionToLimits(currentPositionStore.position)
            UpdateTargetPositon(currentPositionStore.position)
            mechManager.MoveHead(currentPositionStore.position.X, currentPositionStore.position.Y)



            '       serialManager.SendMessage("MOVEHEAD " & Format(currentPositionStore.position.X, "####0000") & "," & Format(currentPositionStore.position.Y, "####0000"))

        End If

    End Sub

    ' TargetCurrentHeadPosition_Click
    ' copy the actual position to the tatrget position
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub TargetCurrentHeadPosition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TargetCurrentHeadPositionButton.Click

        If helperFunctions.StringToInteger(ActualX.Text, currentPositionStore.position.X) AndAlso helperFunctions.StringToInteger(ActualY.Text, currentPositionStore.position.Y) Then
            TargetXText.Text = currentPositionStore.position.X.ToString
            TargetYText.Text = currentPositionStore.position.Y.ToString
        End If

    End Sub

    ' TestPosition_Click
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub TestPosition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestPositionButton.Click

        currentPositionStore.position.X = Convert.ToInt32(TargetXText.Text)
        currentPositionStore.position.Y = Convert.ToInt32(TargetYText.Text)
        mechManager.TestPosition(currentPositionStore.position.X, currentPositionStore.position.Y, 1, currentPositionStore.fridge)

    End Sub

    ' TestPosition_Click
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub TestVend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestVendButton.Click

        Dim vdoToSend As Integer
        Dim pickAttempts As Integer
        Dim fridgeDoor As Integer = currentPositionStore.fridge

        helperFunctions.StringToInteger(VdoText.Text, vdoToSend)
        helperFunctions.StringToInteger(PickAttemptsText.Text, pickAttempts)
        helperFunctions.StringToInteger(TargetXText.Text, currentPositionStore.position.X)
        helperFunctions.StringToInteger(TargetYText.Text, currentPositionStore.position.Y)

        If testProductVend.Available() Then

            testProductVend.Go()
            mechManager.VendProduct(currentPositionStore.position.X, currentPositionStore.position.Y, vdoToSend, pickAttempts, fridgeDoor)

        End If


    End Sub

    Private Sub MoveHeadSafeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveHeadSafeButton.Click
        mechManager.MoveHeadSafe()
    End Sub

    ' TrimPositionToLimits
    ' keep a given position within the rack limits
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub TrimPositionToLimits(ByRef positionToTrim As Point)

        Dim maximumExtent As Point = settingsManager.GetValue("RackMaximum")
        Dim minimumExtent As Point = settingsManager.GetValue("RackMinimum")

        ' test and apply limits.
        If positionToTrim.X > maximumExtent.X Then positionToTrim.X = maximumExtent.X
        If positionToTrim.Y > maximumExtent.Y Then positionToTrim.Y = maximumExtent.Y
        If positionToTrim.X < minimumExtent.X Then positionToTrim.X = minimumExtent.X
        If positionToTrim.Y < minimumExtent.Y Then positionToTrim.Y = minimumExtent.Y

    End Sub

    ' UpdateTargetPositon
    ' update the on screen position
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub UpdateTargetPositon(ByVal newPosition As Point)

        TargetXText.Text = Format(newPosition.X, "####0000")
        TargetYText.Text = Format(newPosition.Y, "####0000")

    End Sub

    ' UpdateFridgeSelection
    ' update the on selected fridge
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub UpdateFridgeSelection(ByVal fridgeId As Integer)
        FridgeCombo.SelectedIndex = IIf(fridgeId = NO_FRIDGE, 0, fridgeId)
    End Sub

    Private Sub UpdateActiveStatus(ByVal isActive As Boolean)
        ActiveCheck.Checked = isActive
    End Sub

    Private Sub UpdateVdo(ByVal vdoSettting As Integer)
        VdoText.Text = vdoSettting.ToString
    End Sub

    Private Sub UpdatePickAttempts(ByVal pickAttemptsSettting As Integer)
        PickAttemptsText.Text = pickAttemptsSettting.ToString
    End Sub

    ' UpdateActualPositon
    ' update the on screen position
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub UpdateActualPositon(ByVal newPosition As Point)

        If newPosition.X = HEAD_LOST Or newPosition.Y = HEAD_LOST Then

            helperFunctions.SetLabelText(ActualX, "????")
            helperFunctions.SetLabelText(ActualY, "????")

            helperFunctions.SetButtonEnable(TargetCurrentHeadPositionButton, False)

            If fridgeDoorIsSafe = False Then
                helperFunctions.SetButtonEnable(TestPositionButton, False)
                helperFunctions.SetButtonEnable(TestVendButton, False)
            End If

        Else
            helperFunctions.SetLabelText(ActualX, Format(newPosition.X, "####0000"))
            helperFunctions.SetLabelText(ActualY, Format(newPosition.Y, "####0000"))

            helperFunctions.SetButtonEnable(TargetCurrentHeadPositionButton, True)

        End If

    End Sub

    ' UpdateStockInformation
    ' how many do we have, when do we need more
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub UpdateStockInformation()

        QuantityinHand.Text = currentPositionStore.stockAvailable
        CurrentRestockLevel.Text = currentPositionStore.restockLevel
        QuantityInStock.Text = currentPositionStore.itemsInStock

    End Sub

    ' UpdateContainer
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub UpdateContainer()

        If usesContainers Then

            ContainerNumberText.Text = currentPositionStore.containerNumber
            LayoutXText.Text = currentPositionStore.xVisualPosition
            LayoutYText.Text = currentPositionStore.yVisualPosition
        End If

    End Sub

    ' ApplyPositionChanges
    ' store the new position..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ApplyPositionChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click

        Dim newExplorerIdent As String
        Dim doorIdent As String

        EnsureValidDefaultValue(VdoText, 0)
        EnsureValidDefaultValue(PickAttemptsText, 0)
        EnsureValidDefaultValue(TargetXText, settingsManager.GetValue("HORZ_HP"))
        EnsureValidDefaultValue(TargetXText, settingsManager.GetValue("VERT_HP"))

        helperFunctions.StringToInteger(TargetXText.Text, currentPositionStore.position.X)
        helperFunctions.StringToInteger(TargetYText.Text, currentPositionStore.position.Y)
        helperFunctions.StringToInteger(VdoText.Text, currentPositionStore.vdo)

        doorIdent = IIf(FridgeCombo.SelectedIndex = 0, "no door", "door " & FridgeCombo.SelectedIndex.ToString)
        newExplorerIdent = "Position: " & currentPositionStore.position.X & ", " & currentPositionStore.position.Y & " (" & doorIdent & ")"

        RaiseEvent NameIdentifierChanges(newExplorerIdent)
        RaiseEvent ActiveStatusChanges(ActiveCheck.Checked)

        If Not StorePositionData() Then
            MsgBox("A problem was encountered while performing the update." & ControlChars.CrLf & ControlChars.CrLf & "Record was not saved.  Please Try Again.", MsgBoxStyle.Exclamation, "Database Update Failed!")
        End If

        ' enable explorer, disable the apply and cancel buttons
        RaiseEvent EnableExplorer(True)
        ApplyButton.Enabled = False
        CancelButton.Enabled = False

    End Sub

    ' EnsureValidDefaultValue
    ' look at a given text box, if it isn't a valid value, enter the default provided
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub EnsureValidDefaultValue(ByVal textBoxToCheck As TextBox, ByVal defaultValue As Integer)

        If Not IsNumeric(textBoxToCheck.Text) Then
            textBoxToCheck.Text = defaultValue.ToString
        End If

    End Sub


    ' CancelPositionChanges_Click
    ' copy the exiting values back..
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub CancelChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click

        LoadItem(currentPositionStore.identifier)

        ' enable explorer, disable the apply and cancel buttons
        RaiseEvent EnableExplorer(True)
        ApplyButton.Enabled = False
        CancelButton.Enabled = False

    End Sub

    ' SettingHasChanged
    ' the user has made a change so disable selection of another position, product, or category
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SettingHasChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PickAttemptsText.TextChanged, VdoText.TextChanged, TargetYText.TextChanged, TargetXText.TextChanged, FridgeCombo.SelectedIndexChanged, ActiveCheck.CheckedChanged

        If Not dataLoading Then

            currentPositionStore.fridge = IIf(FridgeCombo.SelectedIndex = 0, NO_FRIDGE, FridgeCombo.SelectedIndex)

            ApplyButton.Enabled = True
            CancelButton.Enabled = True

            RaiseEvent EnableExplorer(False)
        End If

    End Sub

    ' AdjustRestockText_Enter & AdjustmentQuantityText_Enter
    ' record the text box just entered
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub AdjustRestockText_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdjustmentQuantity.Enter
        focusedTextBox = AdjustmentQuantity
    End Sub

    Private Sub AdjustmentQuantityText_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestockLevel.Enter
        focusedTextBox = RestockLevel
    End Sub

    ' NumericButton_Click
    ' update a text box
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub NumericButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericButton1.Click, NumericButton2.Click, NumericButton3.Click, NumericButton4.Click, NumericButton5.Click, NumericButton6.Click, NumericButton7.Click, NumericButton8.Click, NumericButton9.Click, NumericButton0.Click, NumericButtonD.Click

        If Not focusedTextBox Is Nothing Then

            Select Case sender.Tag

                Case "d"
                    SetFocus(focusedTextBox.Handle)
                    SendKeys.Send("{BACKSPACE}")

                Case Else
                    SetFocus(focusedTextBox.Handle)
                    SendKeys.Send(sender.Tag)
            End Select

        End If

    End Sub

    ' NumericMasking_KeyPress
    ' ensure that only numerics and delete can be pressed
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub NumericMasking_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles LayoutXText.KeyPress, LayoutYText.KeyPress, RestockLevel.KeyPress, AdjustmentQuantity.KeyPress, VdoText.KeyPress

        If (e.KeyChar < "0" Or e.KeyChar > "9") And e.KeyChar <> Chr(8) Then
            e.Handled = True
        End If

    End Sub

    ' RemoveStock_Click & AddStock_Click
    ' adjust the position inventory
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub RemoveStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveStockButton.Click

        Dim adjustmentValue As Integer = -Convert.ToInt32(AdjustmentQuantity.Text)

        If (settingsManager.ConnectToDatabase()) Then

            settingsManager.RunDatabaseNonQuery("update productposition set itemsinstock=itemsinstock+" & adjustmentValue & " where posid=" & currentPositionStore.identifier)
            settingsManager.RunDatabaseNonQuery("insert stockadjustments (prodid, posid, adjustment, adjustmentdate) values (" & currentPositionStore.productId & ", " & currentPositionStore.identifier & ", " & adjustmentValue & ", getdate())")
            settingsManager.DisconnectFromDatabase()

            currentPositionStore.stockAvailable += adjustmentValue
            currentPositionStore.itemsInStock += adjustmentValue

            UpdateStockInformation()

            AdjustmentQuantity.Text = "0"
        End If

    End Sub

    Private Sub AddStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStockButton.Click

        Dim adjustmentValue As Integer = Convert.ToInt32(AdjustmentQuantity.Text)

        If (settingsManager.ConnectToDatabase()) Then

            settingsManager.RunDatabaseNonQuery("update productposition set itemsinstock=itemsinstock+" & adjustmentValue & " where posid=" & currentPositionStore.identifier)

            settingsManager.RunDatabaseNonQuery("insert stockadjustments (prodid, posid, adjustment, adjustmentdate) values (" & currentPositionStore.productId & ", " & currentPositionStore.identifier & ", " & adjustmentValue & ", getdate())")
            settingsManager.DisconnectFromDatabase()

            currentPositionStore.stockAvailable += adjustmentValue
            currentPositionStore.itemsInStock += adjustmentValue

            UpdateStockInformation()

            AdjustmentQuantity.Text = "0"
        End If

    End Sub

    ' EditRestockButton_Click
    ' change the restock level
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub EditRestockButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditRestockButton.Click

        Dim adjustmentValue As Integer = Convert.ToInt32(RestockLevel.Text)

        If (settingsManager.ConnectToDatabase()) Then

            settingsManager.RunDatabaseNonQuery("update productposition set restocklevel=" & adjustmentValue & ", moddate=getdate() where posid=" & currentPositionStore.identifier)
            settingsManager.DisconnectFromDatabase()

            currentPositionStore.restockLevel = adjustmentValue
            UpdateStockInformation()
        End If

    End Sub


    ' AdjustmentQuantity_Changed & RestockLevelText_Changed
    ' are we going to allo edits
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub AdjustmentQuantity_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdjustmentQuantity.TextChanged

        Dim allowAddAdjustments As Boolean = False
        Dim allowSubtractAdjustments As Boolean = False
        Dim stockAdjustment As Integer
        Dim existingStockLevel As Integer = Convert.ToInt32(QuantityInStock.Text)

        If IsNumeric(AdjustmentQuantity.Text) Then

            stockAdjustment = Convert.ToInt32(AdjustmentQuantity.Text)

            If stockAdjustment > 0 Then
                allowAddAdjustments = True
            End If

            If stockAdjustment > 0 And stockAdjustment <= existingStockLevel Then
                allowSubtractAdjustments = True
            End If

        End If

        RemoveStockButton.Enabled = allowSubtractAdjustments
        AddStockButton.Enabled = allowAddAdjustments

    End Sub

    Private Sub RestockLevelText_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestockLevel.TextChanged

        Dim allowRestockEdit As Boolean = False

        If IsNumeric(RestockLevel.Text) Then
            If (Convert.ToInt32(RestockLevel.Text) > 0) Then
                allowRestockEdit = True
            End If
        End If

        EditRestockButton.Enabled = allowRestockEdit

    End Sub

    ' MoveButtonClicked_Click
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub MoveButtonClicked_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click, Button2.Click, Button3.Click, Button4.Click, Button5.Click, Button6.Click, Button7.Click, Button8.Click, Button9.Click, Button10.Click, Button11.Click, Button12.Click, Button13.Click, Button14.Click

        Dim parameterList() As String = sender.Tag.Split(",")
        Dim doorIndex As Integer = Convert.ToInt32(parameterList(1))

        Select Case parameterList(0)

            Case "O"
                If fridgeManager.OpenDoor(doorIndex) Then

                    SetFridgeDoorProgress(doorIndex, "Requested open door")
                Else

                    SetFridgeDoorProgress(doorIndex, "Unable to request open door")
                End If

            Case "C"
                If fridgeManager.CloseDoor(doorIndex) Then

                    SetFridgeDoorProgress(doorIndex, "Requested close door")
                Else

                    SetFridgeDoorProgress(doorIndex, "Unable to request close door")
                End If

        End Select

    End Sub

    ' PositionNumpadButton_Click
    ' edit the VDO number
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub PositionNumpadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PositionNumpadButton0.Click, PositionNumpadButton1.Click, PositionNumpadButton2.Click, PositionNumpadButton3.Click, PositionNumpadButton4.Click, PositionNumpadButton5.Click, PositionNumpadButton6.Click, PositionNumpadButton7.Click, PositionNumpadButton8.Click, PositionNumpadButton9.Click, PositionNumpadButtonDelete.Click

        Dim existingContent As String
        Dim targetText As TextBox = Nothing

        Select Case keypadTarget

            Case KeypadTargetEnum.vdoText
                targetText = VdoText
            Case KeypadTargetEnum.xPositionTargetText
                targetText = TargetXText
            Case KeypadTargetEnum.yPositionTargetText
                targetText = TargetYText
            Case KeypadTargetEnum.pickAttemptsText
                targetText = PickAttemptsText
            Case Else

        End Select

        If Not targetText Is Nothing Then

            existingContent = targetText.Text

            Select Case sender.Tag

                Case "d"
                    If existingContent.Length > 0 Then
                        targetText.Text = existingContent.Substring(0, existingContent.Length - 1)
                    End If

                Case Else
                    targetText.Text = existingContent & sender.Tag

            End Select
        End If



    End Sub

    ' TargetXText_Enter, TargetYText_Enter & VdoText_Enter
    ' assign the numeric pad target
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub TargetXText_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TargetXText.Enter
        keypadTarget = KeypadTargetEnum.xPositionTargetText
    End Sub
    Private Sub TargetYText_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TargetYText.Enter
        keypadTarget = KeypadTargetEnum.yPositionTargetText
    End Sub
    Private Sub VdoText_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles VdoText.Enter
        keypadTarget = KeypadTargetEnum.vdoText
    End Sub
    Private Sub PickAttemptsText_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles PickAttemptsText.Enter
        keypadTarget = KeypadTargetEnum.pickAttemptsText
    End Sub

    ' ActuaY_TextChanged
    ' onkly allow targeting the current head position if known
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ActualY_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ActualY.TextChanged
        If IsNumeric(ActualY.Text) AndAlso IsNumeric(ActualX.Text) Then
            TargetCurrentHeadPositionButton.Enabled = True
        Else
            TargetCurrentHeadPositionButton.Enabled = False
        End If

    End Sub


    Private Sub RecordSaleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordSaleButton.Click

        Dim success As Boolean

        success = helperFunctions.RecordaSale(currentPositionStore.identifier, currentSaleId, My.Settings.DatabaseConnection)

    End Sub


    Private Sub AddTxnIdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTxnIdButton.Click

        Dim success As Boolean

        success = helperFunctions.AddTransactionId(currentSaleId, "TXN 4512", My.Settings.DatabaseConnection)
    End Sub

    Private Function SetStockLevel(ByVal positionId As Integer, ByVal newStockLevel As Integer)

        Dim updateResult As Boolean = False
        Dim databaseConnection As New SqlConnection(My.Settings.DatabaseConnection)
        Dim sqlCommand As New SqlCommand("spUpdateStock", databaseConnection)

        sqlCommand.CommandType = CommandType.StoredProcedure

        sqlCommand.Parameters.Add("@PosID", SqlDbType.Int, 1)
        sqlCommand.Parameters("@PosID").Value = positionId
        sqlCommand.Parameters("@PosID").Direction = ParameterDirection.Input

        sqlCommand.Parameters.Add("@Qty", SqlDbType.Int, 1)
        sqlCommand.Parameters("@Qty").Value = newStockLevel
        sqlCommand.Parameters("@Qty").Direction = ParameterDirection.Input

        sqlCommand.Parameters.Add("@Result", SqlDbType.Int, 1)
        sqlCommand.Parameters("@Result").Direction = ParameterDirection.Output

        Try
            databaseConnection.Open()
            sqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        End Try

        If sqlCommand.Parameters("@Result").Value = "1" Then
            updateResult = True
        End If

        databaseConnection.Close()

        Return updateResult

    End Function

    ' ContainerText_TextChanged
    ' the user changes the container details.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ContainerText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LayoutXText.TextChanged, LayoutYText.TextChanged, ContainerNumberText.TextChanged

        If Not dataLoading Then

            CancelContainerButton.Enabled = True
            ApplyContainerButton.Enabled = True
        End If

    End Sub

    ' CancelContainerButton_Click
    ' the user cancels and changes made
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub CancelContainerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelContainerButton.Click
        dataLoading = True
        UpdateContainer()
        dataLoading = False

        CancelContainerButton.Enabled = False
        ApplyContainerButton.Enabled = False
    End Sub

    ' ApplyContainerButton_Click
    ' the user applies the changes made
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub ApplyContainerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyContainerButton.Click

        Dim newContainerNumber As String
        Dim newXPosition As Integer
        Dim newYPosition As Integer

        If helperFunctions.StringToInteger(LayoutXText.Text, newXPosition) AndAlso helperFunctions.StringToInteger(LayoutYText.Text, newYPosition) Then

            newContainerNumber = MakeSqlSafe(ContainerNumberText.Text, 8)

            If (settingsManager.ConnectToDatabase()) Then

                If settingsManager.RunDatabaseNonQuery("update productposition set containernumber='" & newContainerNumber & "', xvisualposition=" & newXPosition & ", yvisualposition=" & newYPosition & " where posid=" & currentPositionStore.identifier) Then

                    currentPositionStore.containerNumber = newContainerNumber
                    currentPositionStore.xVisualPosition = newXPosition
                    currentPositionStore.yVisualPosition = newYPosition


                End If

                settingsManager.DisconnectFromDatabase()

            End If

        End If





        CancelContainerButton.Enabled = False
        ApplyContainerButton.Enabled = False


    End Sub

    ' MakeSqlSafe
    ' get rid of bad charactors
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function MakeSqlSafe(ByVal sqlString As String, ByVal maximumLength As Integer) As String

        Dim returnString As String = sqlString

        returnString = returnString.Replace("'", "''")
        returnString = returnString.Replace("|", "")
        returnString = returnString.Replace("-", "")

        If returnString.Length >= maximumLength Then
            returnString = returnString.Substring(0, maximumLength)
        End If

        Return returnString

    End Function
End Class
