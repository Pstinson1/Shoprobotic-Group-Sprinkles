'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

Imports Microsoft.Win32
Imports System.Windows.Forms
Imports System.IO
Imports System.Reflection
Imports System.Drawing
Imports System.Drawing.Drawing2D

Imports HelperFunctions
Imports SerialManager

Public Class uPowerMonitor

    ' enumerations
    Public Structure AdcSampleStruct

        Dim adc1 As Integer
        Dim adc2 As Integer
        Dim adc3 As Integer
        Dim adc4 As Integer

    End Structure

    ' managers
    Private serialManager As fSerialManager
    Private serialManagerFactory As cSerialManagerFactory = New cSerialManagerFactory
    Private helperFunctions As cHelperFunctions
    Private helperFunctionsFactory As cHelperFunctionsFactory = New cHelperFunctionsFactory

    ' variables
    Private samplesPerInterval As Integer = 0
    Private dueSampleRateDisplay As Date
    Private adcSample(100) As AdcSampleStruct
    Private adcBitmap As Bitmap
    Private pen(4) As Pen

    Private timeCursor As Integer = 0
    Private adcState(4) As Integer

    ' constants
    Private Const HORZ_MULT = 3
    Private Const VERT_MULT = 3



    ' Initialise
    ' connect to the database
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Public Sub Initialise()

        ' get the managers
        helperFunctions = helperFunctionsFactory.GetManager()
        serialManager = serialManagerFactory.GetManager

        ' start examining the serial data
        serialManager.AddCallback(AddressOf SerialPortEvent)

        Dim graphics = PictureBox1.CreateGraphics()

        graphics.FillRectangle(Drawing.Brushes.Black, 0, 0, PictureBox1.Width, PictureBox1.Height)
        graphics.Dispose()

        pen(0) = New System.Drawing.Pen(Color.Teal, 1)
        pen(1) = New System.Drawing.Pen(Color.AliceBlue, 1)
        pen(2) = New System.Drawing.Pen(Color.DarkSlateBlue, 1)
        pen(3) = New System.Drawing.Pen(Color.Red, 1)

        ' start outputting power data
        SampleIntervalTimer.Enabled = True

    End Sub

    ' SerialPortEvent
    ' event driven incomming serial messages.
    ' ----------------------------------------------------------------------------------------------------------------------------------------------------
    Private Sub SerialPortEvent(ByVal eventCode As Integer, ByVal messageContent As String)

        If eventCode = fSerialManager.Message.COM_RECV Then

            Dim parameterList As String() = Split(messageContent, "=")
            Dim adcList As String()
            Dim parameterCount As Integer = UBound(parameterList)

            Select Case parameterList(0)

                Case "ADC"

                    adcList = Split(parameterList(1), ",")

                    helperFunctions.SetLabelText(DataBlockLabel, parameterList(1))

                    helperFunctions.StringToInteger(adcList(0), adcSample(samplesPerInterval).adc1)
                    helperFunctions.StringToInteger(adcList(1), adcSample(samplesPerInterval).adc2)
                    helperFunctions.StringToInteger(adcList(2), adcSample(samplesPerInterval).adc3)
                    helperFunctions.StringToInteger(adcList(3), adcSample(samplesPerInterval).adc4)



                    'adcSample(samplesPerInterval).adc1 = Convert.ToInt32(adcList(0))
                    'adcSample(samplesPerInterval).adc2 = Convert.ToInt32(adcList(1))
                    'adcSample(samplesPerInterval).adc3 = Convert.ToInt32(adcList(2))
                    'adcSample(samplesPerInterval).adc4 = Convert.ToInt32(adcList(3))

                    samplesPerInterval += 1

                Case Else

            End Select

        End If

    End Sub

    Private Sub SampleIntervalTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SampleIntervalTimer.Tick

        '  Dim graphics = PictureBox1.CreateGraphics()
        '     Dim bottomScale = PictureBox1.Height - 5
        'For sampleIndex As Integer = 0 To samplesPerInterval - 1

        '    graphics.FillRectangle(Drawing.Brushes.Black, timeCursor, 1, HORZ_MULT, PictureBox1.Height)

        '    graphics.DrawLine(pen(0), timeCursor, bottomScale - adcState(0) * VERT_MULT, timeCursor + HORZ_MULT, bottomScale - adcSample(sampleIndex).adc1 * VERT_MULT)
        '    graphics.DrawLine(pen(1), timeCursor, bottomScale - adcState(1) * VERT_MULT, timeCursor + HORZ_MULT, bottomScale - adcSample(sampleIndex).adc2 * VERT_MULT)
        '    graphics.DrawLine(pen(2), timeCursor, bottomScale - adcState(2) * VERT_MULT, timeCursor + HORZ_MULT, bottomScale - adcSample(sampleIndex).adc3 * VERT_MULT)
        '    graphics.DrawLine(pen(3), timeCursor, bottomScale - adcState(3) * VERT_MULT, timeCursor + HORZ_MULT, bottomScale - adcSample(sampleIndex).adc4 * VERT_MULT)

        '    adcState(0) = adcSample(sampleIndex).adc1
        '    adcState(1) = adcSample(sampleIndex).adc2
        '    adcState(2) = adcSample(sampleIndex).adc3
        '    adcState(3) = adcSample(sampleIndex).adc4

        '    timeCursor = (timeCursor + HORZ_MULT) Mod 434

        '    graphics.DrawLine(pen(0), timeCursor, 0, timeCursor, bottomScale)

        'Next

        'graphics.Dispose()

        helperFunctions.SetLabelText(SampleRateLabel, samplesPerInterval.ToString)
        samplesPerInterval = 0

    End Sub

End Class
