'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************

'Option Strict On
Option Explicit On 

Imports System.ComponentModel
Imports System.ComponentModel.Design

' DefaultEvent("Click"), This sets the default event when double-clicking
'                       the control in design-mode.
' DefaultProperty("Text"), This will set the default
'                       property for the property inspector
'                       in design-mode.
'
' This attribute makes this control a container for
'                       other controls.
' Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", GetType(IDesigner))
'
<Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", _
            GetType(IDesigner)), DefaultEvent("Click"), DefaultProperty("Text")> _
Public Class PictureHoverButton
    Inherits System.Windows.Forms.UserControl

    Private _Selectable As Boolean = False

    Private _HoverForeColor As Color = Color.Empty
    Private _Border As Boolean = True
    Private _ImageAlign As ContentAlignment = ContentAlignment.MiddleLeft
    Private _TextAlign As ContentAlignment = ContentAlignment.MiddleCenter
    Private _GroupKey As String
    Private _HoverImage As Image
    Private _Image As Image
    Private _UnselectOtherButtons As Boolean = True
    Private _Selected As Boolean
    Private _BorderColor As Color = Color.FromArgb(179, 176, 208)
    Private _HoverColor As Color = Color.Empty
    Private _SelectedColor As Color = Color.LightGray
    Private _HasFocus As Boolean
    Private _TempBackColor As Color = Color.LightGray
    Private _HoverRectangle As Rectangle
    Private WithEvents HoverTimer As System.Timers.Timer
    Public Event SelectedChanged(ByVal sender As PictureHoverButton)


#Region " Component Designer generated code "

    Public Sub New()
        MyBase.New()

        ' This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        ' This draws the control whenever it is resized
        setstyle(ControlStyles.ResizeRedraw, True)

        ' This supports mouse movement such as the mouse wheel
        setstyle(ControlStyles.UserMouse, True)

        ' This allows the control to be transparent
        setstyle(ControlStyles.SupportsTransparentBackColor, True)

        ' This helps with drawing the control so that it doesn't flicker
        Me.SetStyle(ControlStyles.DoubleBuffer _
          Or ControlStyles.UserPaint _
          Or ControlStyles.AllPaintingInWmPaint, _
          True)

        ' This updates the styles
        Me.UpdateStyles()

        ' Sets the hover time to 30 milliseconds. This is used
        ' to trigger the control to change the backcolor back to the
        ' original value due to the mouse no longer being over
        ' the control. 
        '
        ' The time is used because it's more reliable than using the
        ' MouseLeave or MouseEnter events, which depending on the speed
        ' of the mouse, may or may not get fired.
        HoverTimer = New System.Timers.Timer(30)
    End Sub

    'Control overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Control Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  Do not modify it
    ' using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'PictureHoverButton
        '
        Me.Name = "PictureHoverButton"
        Me.Size = New System.Drawing.Size(75, 23)

    End Sub

#End Region
    Delegate Function GetPointToClientCallback(ByVal control As PictureHoverButton, ByVal mousePoz As Point) As Point

    Private Function GetPointToClient(ByVal control As PictureHoverButton, ByVal mousePoz As Point) As Point
        If control.InvokeRequired Then
            Dim d As New GetPointToClientCallback(AddressOf GetPointToClient)
            Try
                Return Me.Invoke(d, New Object() {control, mousePoz})
            Catch ex As Exception
                Return New Point(0, 0)
            End Try


        Else
            ' stop bombing out on closure, disposed items.
            Try
                Return control.PointToClient(mousePoz)
            Catch ex As Exception
                Return New Point(0, 0)
            End Try
        End If
    End Function

    Protected Overrides Sub OnPaint(ByVal pe As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(pe)
        '        Dim TopMargin As Integer
        Dim g As Graphics = pe.Graphics
        Dim DrawHover As Boolean

        _HoverRectangle = Nothing
        _HoverRectangle = New Rectangle(0, 0, Me.Width, Me.Height)


        ' The mouse is hovering over the control
        If _HasFocus Then
            If Me.Selected Then
                Dim rec As New Rectangle(0, 0, Me.Width, Me.Height)
                Dim b As New SolidBrush(Me.SelectedColor)
                g.FillRectangle(b, rec)
            Else
                ' Make sure the color is not Nothing, or Empty
                ' which is the equivalent to nothing for a color.
                If Not HoverColor.Equals(Color.Empty) Then
                    Dim rec As New Rectangle(0, 0, Me.Width, Me.Height)
                    Dim b As New SolidBrush(Me.HoverColor)
                    g.FillRectangle(b, rec)
                End If
            End If

            ' Draw the Hover image 
            If Not Me.Selected Then
                DrawHover = True
            End If
        Else
            If Me.Selected Then
                Dim rec As New Rectangle(0, 0, Me.Width, Me.Height)
                Dim b As New SolidBrush(Me.SelectedColor)
                g.FillRectangle(b, rec)
            End If
        End If

        Dim ptImage As Point
        ' Used to hold the dimensions of the current image
        Dim iWidth As Integer
        Dim iHeight As Integer
        '     Dim iCenter As Integer
        If DrawHover Then
            If Not Me.HoverImage Is Nothing Then
                iWidth = Me.HoverImage.Width
                iHeight = Me.HoverImage.Height
            Else
                If Not Me.Image Is Nothing Then
                    iWidth = Me.Image.Width
                    iHeight = Me.Image.Height
                End If
            End If
        Else
            If Not Me.Image Is Nothing Then
                iWidth = Me.Image.Width
                iHeight = Me.Image.Height
            End If
        End If

        ' Make sure we even need to draw an image to begin
        ' with. There may not be any images in this control.
        If Not iHeight = 0 Then
            ' Ge the position of the image
            Select Case ImageAlign
                Case ContentAlignment.BottomCenter
                    ptImage = New Point(Convert.ToInt32((Me.Width / 2) - (iWidth / 2)), (Me.Height - iHeight) - 3)
                Case ContentAlignment.BottomLeft
                    ptImage = New Point(5, (Me.Height - iHeight) - 3)
                Case ContentAlignment.BottomRight
                    ptImage = New Point((Me.Width - iWidth) - 5, (Me.Height - iHeight) - 3)

                Case ContentAlignment.MiddleCenter
                    ptImage = New Point(Convert.ToInt32((Me.Width / 2) - (iWidth / 2)), Convert.ToInt32((Me.Height / 2) - (iHeight / 2)))
                Case ContentAlignment.MiddleLeft
                    ptImage = New Point(5, Convert.ToInt32((Me.Height / 2) - (iHeight / 2)))
                Case ContentAlignment.MiddleRight
                    ptImage = New Point((Me.Width - iWidth) - 5, Convert.ToInt32((Me.Height / 2) - (iHeight / 2)))

                Case ContentAlignment.TopCenter
                    ptImage = New Point(Convert.ToInt32((Me.Width / 2) - (iWidth / 2)), 5)
                Case ContentAlignment.TopLeft
                    ptImage = New Point(5, 5)
                Case ContentAlignment.TopRight
                    ptImage = New Point((Me.Width - iWidth) - 5, 5)
            End Select

            If DrawHover Then
                If Not Me.HoverImage Is Nothing Then
                    g.DrawImage(Me.HoverImage, ptImage.X, ptImage.Y, Me.HoverImage.Width, Me.HoverImage.Height)
                Else
                    If Not Me.Image Is Nothing Then
                        g.DrawImage(Me.Image, ptImage.X, ptImage.Y, Me.Image.Width, Me.Image.Height)
                    End If
                End If
            Else
                If Not Me.Image Is Nothing Then
                    g.DrawImage(Me.Image, ptImage.X, ptImage.Y, Me.Image.Width, Me.Image.Height)
                End If
            End If
        End If

        ' Get the dimensions of the text.
        ' This will return a SizeF object that has height, width, etc.
        Dim ptSize As SizeF = g.MeasureString(Text, Font)

        Dim ptF As New PointF
        ' Get the location to set the Text.
        Select Case Me.TextAlign
            Case ContentAlignment.BottomCenter
                ptF.Y = Me.Height
                ptF.Y -= ptSize.Height
                ptF.X = Convert.ToInt32(Me.Width / 2)
                ptF.X -= ptSize.Width / 2

            Case ContentAlignment.BottomLeft
                ptF.Y = Me.Height
                ptF.Y -= ptSize.Height
                ptF.X = 2

            Case ContentAlignment.BottomRight
                ptF.Y = Me.Height
                ptF.Y -= ptSize.Height
                ptF.X = Me.Width
                ptF.X -= ptSize.Width

            Case ContentAlignment.MiddleCenter
                ptF.Y = Convert.ToInt32(Me.Height / 2)
                ptF.Y -= ptSize.Height / 2
                ptF.X = Convert.ToInt32(Me.Width / 2)
                ptF.X -= ptSize.Width / 2

            Case ContentAlignment.MiddleLeft
                ptF.Y = Convert.ToInt32(Me.Height / 2)
                ptF.Y -= ptSize.Height / 2
                ptF.X = 0

            Case ContentAlignment.MiddleRight
                ptF.Y = Convert.ToInt32(Me.Height / 2)
                ptF.Y -= ptSize.Height / 2
                ptF.X = Me.Width
                ptF.X -= ptSize.Width

            Case ContentAlignment.TopCenter
                ptF.Y = 0
                ptF.X = Convert.ToInt32(Me.Width / 2)
                ptF.X -= ptSize.Width / 2

            Case ContentAlignment.TopLeft
                ptF.Y = 0
                ptF.X = 0

            Case ContentAlignment.TopRight
                ptF.Y = 0
                ptF.X = Me.Width
                ptF.X -= ptSize.Width
        End Select

        If _HasFocus AndAlso Not Me.Selected Then
            ' Draw the Text for the control using the HoverForeColor
            If Not Me.HoverForeColor.Equals(Color.Empty) Then
                g.DrawString(Me.Text, Me.Font, New SolidBrush(Me.HoverForeColor), ptF)
            Else
                g.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), ptF)
            End If
        Else
            ' Draw the Text for the control
            g.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), ptF)
        End If

        ' Draw the border
        If Border Then
            If Not BorderColor.Equals(Color.Empty) Then
                Dim linePen As New Pen(New SolidBrush(_BorderColor), 1)
                g.DrawRectangle(linePen, New Rectangle(0, 0, Me.Width - 1, Me.Height - 1))
            End If
        End If
    End Sub

    <DefaultValue(GetType(ContentAlignment), "ContentAlignment.MiddleCenter")> _
    Public Property TextAlign() As ContentAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal Value As ContentAlignment)
            _TextAlign = Value
            Invalidate()
        End Set
    End Property

    <DefaultValue(GetType(ContentAlignment), "ContentAlignment.MiddleLeft")> _
    Public Property ImageAlign() As ContentAlignment
        Get
            Return _ImageAlign
        End Get
        Set(ByVal Value As ContentAlignment)
            _ImageAlign = Value
            Invalidate()
        End Set
    End Property

    <DefaultValue(GetType(Color), "Color.Empty")> _
    Public Property BorderColor() As Color
        Get
            Return _BorderColor
        End Get
        Set(ByVal Value As Color)
            _BorderColor = Value
            Invalidate()
        End Set
    End Property
    <DefaultValue(GetType(Color), "Color.Empty")> _
    Public Property HoverColor() As Color
        Get
            Return _HoverColor
        End Get
        Set(ByVal Value As Color)
            _HoverColor = Value
            Invalidate()
        End Set
    End Property

    <DefaultValue(GetType(Color), "Color.LightGray")> _
    Public Property SelectedColor() As Color
        Get
            Return _SelectedColor
        End Get
        Set(ByVal Value As Color)
            If Value.Equals(Color.Empty) Then
                _SelectedColor = Color.LightGray
            Else
                _SelectedColor = Value
            End If
            Invalidate()
        End Set
    End Property

    Private Sub ButtonHoverTimer_Elapsed(ByVal sender As Object, ByVal ex As System.Timers.ElapsedEventArgs) Handles HoverTimer.Elapsed
        Dim e As Point
        e = GetPointToClient(Me, Cursor.Position)
        If Selected Then
            HoverTimer.Stop()
        End If

        If Not Me._HoverRectangle.IntersectsWith(New Rectangle(e, New Size(1, 1))) Then
            HoverTimer.Stop()
            _HasFocus = False
            Invalidate()
        End If
    End Sub

    Private Sub PictureHoverButton_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        Dim pt As New Point(e.X, e.Y)
        If Selected Then Exit Sub
        If Me._HoverRectangle.IntersectsWith(New Rectangle(pt, New Size(1, 1))) Then
            If Not _HasFocus Then
                _HasFocus = True
                Invalidate()
            End If
            HoverTimer.Start()
        End If
    End Sub


    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal Value As Boolean)
            ' Do not let this get selected
            If Value And Not Selectable Then Exit Property
            _Selected = Value
            If Not Value Then
                _HasFocus = False
            End If
            Invalidate()
            If Value Then
                PrivateUnselectOtherButtons()
                RaiseEvent SelectedChanged(Me)
            End If
        End Set
    End Property


    Private Sub PrivateUnselectOtherButtons()
        For Each itm As Control In Me.Parent.Controls
            If TypeName(itm) = TypeName(Me) Then
                If Not itm Is Me Then
                    If Not Me.GroupKey Is Nothing Then
                        If DirectCast(itm, PictureHoverButton).GroupKey = Me.GroupKey Then
                            DirectCast(itm, PictureHoverButton).Selected = False
                        End If
                    Else
                        If DirectCast(itm, PictureHoverButton).GroupKey Is Nothing Then
                            DirectCast(itm, PictureHoverButton).Selected = False
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    <DefaultValue(GetType(Boolean), "True")> _
    Public Property UnselectOtherButtons() As Boolean
        Get
            Return _UnselectOtherButtons
        End Get
        Set(ByVal Value As Boolean)
            _UnselectOtherButtons = Value
            If Me.Selected Then
                If Value Then
                    PrivateUnselectOtherButtons()
                End If
            End If
        End Set
    End Property

    <Browsable(True), Description("Text that appears in the Button."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)> _
    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal Value As String)
            MyBase.Text = Value
            Invalidate()
        End Set
    End Property


    <DefaultValue(GetType(Image), "Nothing"), Description("Image that is shown when not mouse over.")> _
    Public Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal Value As Image)
            _Image = Value
            Invalidate()
        End Set
    End Property

    <DefaultValue(GetType(Image), "Nothing"), Description("Hover image that is shown when the mouse is over the control.")> _
    Public Property HoverImage() As Image
        Get
            Return _HoverImage
        End Get
        Set(ByVal Value As Image)
            _HoverImage = Value
            Invalidate()
        End Set
    End Property

    <DefaultValue(GetType(String), "Nothing"), Description("Enter a value to group like-buttons together, this will affect the unselect behavior.")> _
    Public Property GroupKey() As String
        Get
            Return _GroupKey
        End Get
        Set(ByVal Value As String)
            _GroupKey = Value
        End Set
    End Property


    <DefaultValue(GetType(Boolean), "True"), Description("Draw the border around the control.")> _
    Public Property Border() As Boolean
        Get
            Return _Border
        End Get
        Set(ByVal Value As Boolean)
            _Border = Value
            Invalidate()
        End Set
    End Property

    <DefaultValue(GetType(Color), "Color.Empty"), Description("When mouse hovers, this will be the ForeColor for the Font if set.")> _
    Public Property HoverForeColor() As Color
        Get
            Return _HoverForeColor
        End Get
        Set(ByVal Value As Color)
            _HoverForeColor = Value
        End Set
    End Property

    <DefaultValue(GetType(Boolean), "False"), Description("Can this button be selected.")> _
    Public Property Selectable() As Boolean
        Get
            Return _Selectable
        End Get
        Set(ByVal Value As Boolean)
            _Selectable = Value
        End Set
    End Property


    Private Sub PictureHoverButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Click
        If Not Me.Selected Then
            If Selectable Then
                _Selected = True
            End If
            Invalidate()
            If Selectable Then
                PrivateUnselectOtherButtons()
            End If

            ' This will raise the event
            ' that tells the host that the
            ' current selected item in the group,
            ' if there is one, is different.
            RaiseEvent SelectedChanged(Me)
        End If

    End Sub
End Class
