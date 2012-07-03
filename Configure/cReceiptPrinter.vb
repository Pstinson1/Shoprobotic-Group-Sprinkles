'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************
Imports System.IO
Imports System.Drawing.Printing
Imports System.Threading
Imports System
Imports System.Management

Imports DebugWindow
Imports SettingsManager


Public Class cReceiptPrinter

    ' Variables
    ' ******************************************************************************************************************************************
    Private receiptLine(1) As String
    Private receiptLineCount As Integer = 0
    Private isPrinting As Boolean = False
    Private receiptFont As Font
    Private recieptWidth
    Private debugInformation As fDebugWindow
    Private debugInformationFactory As cDebugWindowFactory = New cDebugWindowFactory
    Private settingsManager As fSettingsManager
    Private settingsManagerFactory As cSettingsManagerFactory = New cSettingsManagerFactory
    Private recieptLineIndex As Integer

    ' Initialise
    ' ******************************************************************************************************************************************
    Public Sub Initialise(ByVal fontName As String, ByVal fontSize As Integer, ByVal allowedWidth As Integer)

        debugInformation = debugInformationFactory.GetManager
        settingsManager = settingsManagerFactory.GetManager

        receiptFont = New Font(fontName, fontSize)
        recieptWidth = allowedWidth

    End Sub

    ' Reset - Clear a reciept ready for reconstruction
    ' ******************************************************************************************************************************************
    Public Sub Reset()
        ReDim receiptLine(1)
        receiptLineCount = 0
    End Sub

    ' AddRecieptHeader
    ' ******************************************************************************************************************************************
    Public Sub AddHeader()

        Dim lineIndex As Integer = 1
        Dim terminated As Boolean = False
        Dim valueRecovered As Boolean = False
        Dim recieptLine As String

        While lineIndex < 10 And Not terminated

            recieptLine = settingsManager.GetValue("RecieptHeader" & lineIndex, valueRecovered)
            terminated = True

            If valueRecovered Then

                If recieptLine <> "." Then

                    AddLine(recieptLine)
                    terminated = False

                End If

            End If

            lineIndex += 1

        End While

    End Sub

    ' AddRecieptFooter
    ' ******************************************************************************************************************************************
    Public Sub AddFooter()

        Dim lineIndex As Integer = 1
        Dim terminated As Boolean = False
        Dim valueRecovered As Boolean = False
        Dim recieptLine As String

        While lineIndex < 5 And Not terminated

            recieptLine = settingsManager.GetValue("RecieptFooter" & lineIndex, valueRecovered)
            terminated = True

            If valueRecovered Then

                If recieptLine <> "." Then

                    AddLine(recieptLine)
                    terminated = False

                End If

            End If

            lineIndex += 1

        End While

    End Sub

    ' AddLine - Add an new line to the receipt
    ' ******************************************************************************************************************************************
    Public Sub AddLine(ByVal NewLine As String)

        Dim FirstLine As Boolean = True

        While NewLine.Length() > 0 Or FirstLine

            If NewLine.Length() > recieptWidth Then

                receiptLine(receiptLineCount) = Mid(NewLine, 1, recieptWidth)
                NewLine = Mid(NewLine, recieptWidth + 1, NewLine.Length() - recieptWidth)
            Else
                receiptLine(receiptLineCount) = NewLine
                NewLine = ""
            End If

            FirstLine = False
            receiptLineCount = receiptLineCount + 1
            ReDim Preserve receiptLine(receiptLineCount)

        End While

    End Sub

    ' Print - Create the thread that will print the reciept.
    ' ******************************************************************************************************************************************
    Public Function Print() As Boolean

        Dim ReportThread As Thread

        ' You can only Print one reciept at a time
        If isPrinting Then
            Return False

        Else
            isPrinting = True
            ReportThread = New Thread(AddressOf PrintThread)
            ReportThread.Name = "Print Receipt"
            ReportThread.Start()
            Return True
        End If

    End Function

    ' PrintThread - Print the receipt
    ' ******************************************************************************************************************************************
    Private Sub PrintThread()

        Dim printerDocument As PrintDocument = New PrintDocument

        printerDocument.DocumentName = "Reciept from Configure Application"
        printerDocument.PrintController = New StandardPrintController

        recieptLineIndex = 0

        AddHandler printerDocument.PrintPage, AddressOf PrintReceiptPage

        Try
            printerDocument.Print()
        Catch ex As Exception
            debugInformation.Progress(fDebugWindow.Level.ERR, 1280, "Could not print vend failed receipt", True)
        End Try

        isPrinting = False

    End Sub

    ' PrintReceiptPage - event is raised for each page to be printed.
    ' ******************************************************************************************************************************************
    Private Sub PrintReceiptPage(ByVal sender As Object, ByVal eventArgs As PrintPageEventArgs)

        Dim yPosition As Integer = eventArgs.MarginBounds.Top
        Dim xPosition As Integer = 0
        Dim recieptBitmap As Bitmap
        Dim tabIndex As Integer
        Dim tabBlock() As String
        Dim singleLine As String
        Dim blockSize As New SizeF

        While yPosition < eventArgs.MarginBounds.Bottom AndAlso recieptLineIndex < receiptLine.Length - 1

            singleLine = receiptLine(recieptLineIndex)

            Select Case singleLine

                Case "[LogoHeader]"

                    ' do we want to print the header logo
                    Try
                        recieptBitmap = CType(Bitmap.FromFile(System.Windows.Forms.Application.StartupPath & "\Printer\header.bmp"), Bitmap)




                        '    eventArgs.Graphics.ScaleTransform(0.25, 0.25) ' = Drawing2D.InterpolationMode.NearestNeighbor
                        '     eventArgs.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                        '          eventArgs.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

                        eventArgs.Graphics.DrawImage(recieptBitmap, 0, yPosition)
                        yPosition = yPosition + ((recieptBitmap.Height() * 125) / 100)
                        recieptBitmap.Dispose()
                    Catch ex As Exception
                        Debug.WriteLine(ex.ToString)
                    Finally
                        recieptBitmap = Nothing
                    End Try

                Case "[LogoFooter]"

                    ' do we want to print the footer logo
                    Try
                        recieptBitmap = CType(Bitmap.FromFile(System.Windows.Forms.Application.StartupPath & "\Printer\footer.bmp"), Bitmap)
                        eventArgs.Graphics.DrawImage(recieptBitmap, 0, yPosition)
                        yPosition = yPosition + ((recieptBitmap.Height() * 125) / 100)
                        recieptBitmap.Dispose()
                    Catch ex As Exception
                        Debug.WriteLine(ex.ToString)
                    End Try

                Case Else

                    ' just print the text
                    xPosition = 0
                    tabBlock = Split(singleLine, ControlChars.Tab)

                    For tabIndex = 0 To UBound(tabBlock)
                        eventArgs.Graphics.DrawString(tabBlock(tabIndex), receiptFont, Brushes.Black, xPosition, yPosition, New StringFormat)
                        blockSize = eventArgs.Graphics.MeasureString(tabBlock(tabIndex), receiptFont)

                        xPosition = xPosition + (blockSize.Width * 94) / 100
                        xPosition = (Int(xPosition / 25) + 1) * 25

                    Next

                    yPosition = yPosition + receiptFont.GetHeight(eventArgs.Graphics)

            End Select

            recieptLineIndex = recieptLineIndex + 1

        End While

        eventArgs.HasMorePages = (recieptLineIndex < receiptLine.Length - 1)

    End Sub

End Class

Class cPrinterStatus

    Public Enum STATUS

        PRT_NOTDETERMINED
        PRT_OFFLINE
        PRT_READY
        PRT_LID
        PRT_PAPER
        PRT_ERROR

    End Enum

    Private scope As ManagementScope
    Private printerStatus As STATUS
    Private managementThread As Thread
    Private continueProcessing As Boolean
    Private printerName As String
    Private extendedDetectedErrorState As Integer
    Private extendedPrinterStatus As Integer
    Private searcher As ManagementObjectSearcher

    Public Sub New()

        Dim printerSettings As New System.Drawing.Printing.PrinterSettings

        printerStatus = STATUS.PRT_NOTDETERMINED
        continueProcessing = True

        scope = New ManagementScope("\root\cimv2")
        scope.Connect()

        ' determine the name of the default printer
        Try
            printerName = printerSettings.PrinterName
        Catch ex As System.Exception
            printerName = ""
        Finally
            printerSettings = Nothing
        End Try

        ' fire up the management thread
        managementThread = New Thread(AddressOf PrinterStatusThread)
        managementThread.Name = "Printer Status"
        managementThread.Priority = ThreadPriority.BelowNormal
        managementThread.Start()

    End Sub

    Private Sub PrinterStatusThread()

        Dim statusResult As Boolean = False
        Dim printerToCheck As String = DefaultPrinterName()

        While (continueProcessing)

            searcher = New ManagementObjectSearcher("SELECT * FROM Win32_Printer where Name='" & printerToCheck & "'")
            Try
                For Each printer As ManagementObject In searcher.[Get]()

                    extendedDetectedErrorState = printer("ExtendedDetectedErrorState")
                    extendedPrinterStatus = printer("ExtendedPrinterStatus")

                    If printerName = printerToCheck Then

                        If extendedDetectedErrorState = 4 And extendedPrinterStatus = 1 Then
                            printerStatus = STATUS.PRT_PAPER

                        ElseIf extendedDetectedErrorState = 4 And extendedPrinterStatus = 1 Then
                            printerStatus = STATUS.PRT_PAPER

                        ElseIf printer("WorkOffline").ToString().ToLower().Equals("true") Then
                            printerStatus = STATUS.PRT_OFFLINE                   ' printer is offline by user

                        Else
                            printerStatus = STATUS.PRT_READY                    ' printer is not offline
                        End If

                    End If
                    '      End If


                Next

            Catch ex As Exception
            End Try

            If continueProcessing Then
                Thread.Sleep(500)
            End If

        End While

    End Sub

    Public Sub CheckPrinterStatus(ByRef printerStatusReturn As STATUS, ByRef extendedDetectedErrorStateReturn As Integer, ByRef extendedPrinterStatusReturn As Integer)

        printerStatusReturn = printerStatus
        extendedDetectedErrorStateReturn = extendedDetectedErrorState
        extendedPrinterStatusReturn = extendedPrinterStatus

    End Sub

    Public Function DefaultPrinterName() As String

        Return printerName

    End Function


    Public Sub Shutdown()

        '     managementThread.Abort()
        '       searcher.Dispose()
        continueProcessing = False

    End Sub

End Class

