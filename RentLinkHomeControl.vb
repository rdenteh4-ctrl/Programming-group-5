Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Partial Public Class RentLinkHomeControl
    Inherits UserControl

    Private modeValue As String = "Rent"

    Public Event ExploreRequested As EventHandler
    Public Event ActivityRequested As EventHandler
    Public Event PropertiesRequested As EventHandler
    Public Event ListPropertyRequested As EventHandler
    Public Event SignOutRequested As EventHandler
    Public Event SearchRequested As EventHandler
    Public Event ModeChanged As EventHandler

    Public Sub New()
        InitializeComponent()
        BackColor = RentLinkRoundedCorners.PageColor
        RentLinkRoundedCorners.ApplyTo(Me)
        UpdateModeButtons()
    End Sub

    <Browsable(False)>
    Public ReadOnly Property SearchText As String
        Get
            Return searchInput.Text
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ListingSearchInput As TextBox
        Get
            Return searchInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ScreenHost As Panel
        Get
            Return screenHostPanel
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ResultsPanel As FlowLayoutPanel
        Get
            Return resultsFlow
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ActiveMode As String
        Get
            Return modeValue
        End Get
    End Property

    Public Sub SetMode(mode As String)
        If mode <> "Rent" AndAlso mode <> "Purchase" Then Return
        modeValue = mode
        UpdateModeButtons()
    End Sub

    Public Sub SetAccountName(value As String)
        accountNameLabel.Text = value
        accountInitialsLabel.Text = GetInitials(value)
    End Sub

    Public Sub SetResultsHeading(value As String)
        resultsHeadingLabel.Text = value
    End Sub

    Private Sub UpdateModeButtons()
        rentButton.BackColor = If(modeValue = "Rent", RentLinkRoundedCorners.AccentColor, Color.White)
        rentButton.ForeColor = If(modeValue = "Rent", Color.White, RentLinkRoundedCorners.MutedColor)
        purchaseButton.BackColor = If(modeValue = "Purchase", RentLinkRoundedCorners.AccentColor, Color.White)
        purchaseButton.ForeColor = If(modeValue = "Purchase", Color.White, RentLinkRoundedCorners.MutedColor)
    End Sub

    Private Function GetInitials(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then Return "R"
        Dim parts = value.Split(" "c, StringSplitOptions.RemoveEmptyEntries)
        If parts.Length > 1 Then Return (parts(0)(0) & parts(parts.Length - 1)(0)).ToUpperInvariant()
        Return value.Substring(0, 1).ToUpperInvariant()
    End Function

    Private Sub ExploreButtonClicked(sender As Object, e As EventArgs) Handles exploreButton.Click
        RaiseEvent ExploreRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub RentButtonClicked(sender As Object, e As EventArgs) Handles rentButton.Click
        If modeValue = "Rent" Then Return
        modeValue = "Rent"
        UpdateModeButtons()
        RaiseEvent ModeChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub PurchaseButtonClicked(sender As Object, e As EventArgs) Handles purchaseButton.Click
        If modeValue = "Purchase" Then Return
        modeValue = "Purchase"
        UpdateModeButtons()
        RaiseEvent ModeChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub SearchButtonClicked(sender As Object, e As EventArgs) Handles searchButton.Click
        RaiseEvent SearchRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub SearchInputKeyDown(sender As Object, e As KeyEventArgs) Handles searchInput.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            RaiseEvent SearchRequested(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub ActivityButtonClicked(sender As Object, e As EventArgs) Handles activityButton.Click
        RaiseEvent ActivityRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub PropertiesButtonClicked(sender As Object, e As EventArgs) Handles propertiesButton.Click
        RaiseEvent PropertiesRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub ListPropertyButtonClicked(sender As Object, e As EventArgs) Handles listPropertyButton.Click
        RaiseEvent ListPropertyRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub SignOutButtonClicked(sender As Object, e As EventArgs) Handles signOutButton.Click
        RaiseEvent SignOutRequested(Me, EventArgs.Empty)
    End Sub
End Class
