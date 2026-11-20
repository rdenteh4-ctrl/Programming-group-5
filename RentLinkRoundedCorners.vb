Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Public Module RentLinkRoundedCorners
    Public ReadOnly InkColor As Color = Color.FromArgb(30, 41, 59)
    Public ReadOnly MutedColor As Color = Color.FromArgb(100, 116, 139)
    Public ReadOnly AccentColor As Color = Color.FromArgb(124, 58, 237)
    Public ReadOnly AccentSoftColor As Color = Color.FromArgb(237, 233, 254)
    Public ReadOnly PageColor As Color = Color.FromArgb(248, 250, 252)
    Public ReadOnly SurfaceColor As Color = Color.White
    Public ReadOnly HeroColor As Color = Color.FromArgb(15, 23, 42)

    Private Const CornerRadius As Integer = 10
    Private ReadOnly attachedControls As New ConditionalWeakTable(Of Control, Object)()

    Public Sub ApplyTo(root As Control)
        If root Is Nothing Then Return
        Attach(root)
    End Sub

    Public Sub ApplyRoundedBorder(panel As Panel, borderColor As Color, radius As Integer)
        If panel Is Nothing Then Return
        panel.BorderStyle = BorderStyle.None
        AddHandler panel.Resize, Sub(sender, args) UpdateRoundedPanelRegion(panel, radius)
        AddHandler panel.Paint, Sub(sender, args) DrawRoundedPanelBorder(panel, args.Graphics, borderColor, radius)
        UpdateRoundedPanelRegion(panel, radius)
    End Sub

    Private Sub UpdateRoundedPanelRegion(panel As Panel, radius As Integer)
        If panel.Width < 2 OrElse panel.Height < 2 Then Return
        Dim previousRegion = panel.Region
        panel.Region = CreateRoundedRegion(panel.Width, panel.Height, radius)
        If previousRegion IsNot Nothing Then previousRegion.Dispose()
    End Sub

    Private Sub DrawRoundedPanelBorder(panel As Panel, graphics As Graphics, borderColor As Color, radius As Integer)
        If panel.Width < 2 OrElse panel.Height < 2 Then Return
        graphics.SmoothingMode = SmoothingMode.AntiAlias
        Dim diameter = Math.Min(radius * 2, Math.Min(panel.Width - 1, panel.Height - 1))
        Using path As New GraphicsPath(), pen As New Pen(borderColor, 1.0F)
            path.AddArc(0.5F, 0.5F, diameter, diameter, 180, 90)
            path.AddArc(panel.Width - diameter - 0.5F, 0.5F, diameter, diameter, 270, 90)
            path.AddArc(panel.Width - diameter - 0.5F, panel.Height - diameter - 0.5F, diameter, diameter, 0, 90)
            path.AddArc(0.5F, panel.Height - diameter - 0.5F, diameter, diameter, 90, 90)
            path.CloseFigure()
            graphics.DrawPath(pen, path)
        End Using
    End Sub

    Private Sub Attach(control As Control)
        If TypeOf control Is TextBoxBase Then
            DirectCast(control, TextBoxBase).BorderStyle = BorderStyle.FixedSingle
            Return
        End If

        Dim attachment As Object = Nothing
        If attachedControls.TryGetValue(control, attachment) Then Return
        attachedControls.Add(control, New Object())

        ApplyPalette(control)
        ApplyShape(control)
        AddHandler control.ControlAdded, AddressOf ChildAdded
        AddHandler control.Resize, AddressOf ControlResized
        For Each child As Control In control.Controls
            Attach(child)
        Next
    End Sub

    Private Sub ChildAdded(sender As Object, e As ControlEventArgs)
        Attach(e.Control)
    End Sub

    Private Sub ControlResized(sender As Object, e As EventArgs)
        ApplyShape(DirectCast(sender, Control))
    End Sub

    Private Sub ApplyPalette(control As Control)
        Dim previousAccent = Color.FromArgb(13, 148, 136).ToArgb()
        Dim previousAccentSoft = Color.FromArgb(204, 251, 241).ToArgb()
        If control.BackColor.ToArgb() = previousAccent Then control.BackColor = AccentColor
        If control.ForeColor.ToArgb() = previousAccent Then control.ForeColor = AccentColor
        If control.BackColor.ToArgb() = previousAccentSoft Then control.BackColor = AccentSoftColor
        If control.ForeColor.ToArgb() = previousAccentSoft Then control.ForeColor = AccentSoftColor
    End Sub

    Private Sub ApplyShape(control As Control)
        If TypeOf control Is Panel Then DirectCast(control, Panel).BorderStyle = BorderStyle.None
        If TypeOf control Is ComboBox Then DirectCast(control, ComboBox).FlatStyle = FlatStyle.Flat
        If TypeOf control Is Button Then DirectCast(control, Button).FlatAppearance.BorderSize = 0

        If Not TypeOf control Is Button OrElse control.Width < 2 OrElse control.Height < 2 Then Return

        Dim previousRegion = control.Region
        control.Region = CreateRoundedRegion(control.Width, control.Height, CornerRadius)
        If previousRegion IsNot Nothing Then previousRegion.Dispose()
    End Sub

    Private Function CreateRoundedRegion(width As Integer, height As Integer, radius As Integer) As Region
        If width < 2 OrElse height < 2 Then Return New Region()
        Dim diameter = Math.Min(radius * 2, Math.Min(width, height))
        Using path As New GraphicsPath()
            path.AddArc(0, 0, diameter, diameter, 180, 90)
            path.AddArc(width - diameter, 0, diameter, diameter, 270, 90)
            path.AddArc(width - diameter, height - diameter, diameter, diameter, 0, 90)
            path.AddArc(0, height - diameter, diameter, diameter, 90, 90)
            path.CloseFigure()
            Return New Region(path)
        End Using
    End Function
End Module
