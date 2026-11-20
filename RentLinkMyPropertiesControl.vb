Imports System.Drawing
Imports System.Windows.Forms

Public Class PropertyAvailabilityEventArgs
    Inherits EventArgs

    Public Sub New(listing As RentLinkWorkspace.PropertyListing, available As Boolean)
        Me.Listing = listing
        Me.Available = available
    End Sub

    Public ReadOnly Property Listing As RentLinkWorkspace.PropertyListing
    Public ReadOnly Property Available As Boolean
End Class

Partial Public Class RentLinkMyPropertiesControl
    Inherits UserControl

    Public Event AddPropertyRequested As EventHandler
    Public Event AvailabilityChanged As EventHandler(Of PropertyAvailabilityEventArgs)

    Private updatingAvailability As Boolean

    Public Sub New()
        InitializeComponent()
        BackColor = RentLinkRoundedCorners.PageColor
        RentLinkRoundedCorners.ApplyTo(Me)
    End Sub

    Public Sub ClearListings()
        propertyCardsFlow.Controls.Clear()
    End Sub

    Public Sub ShowEmptyState()
        propertyCardsFlow.Controls.Add(New Label With {.Text = "You haven't listed a property yet. Start by adding your first home.", .AutoSize = True, .Font = New Font("Segoe UI", 11.0F), .ForeColor = RentLinkRoundedCorners.MutedColor, .Margin = New Padding(0, 8, 0, 0)})
    End Sub

    Public Sub AddPropertyCard(listing As RentLinkWorkspace.PropertyListing, metricsText As String, visibilityText As String)
        Dim card As New Panel With {.BackColor = Color.White, .Size = New Size(Math.Max(650, propertyCardsFlow.ClientSize.Width - 24), 205), .Margin = New Padding(0, 0, 0, 14), .Tag = listing}
        card.Controls.Add(New Label With {.Text = listing.Title, .Location = New Point(18, 16), .Size = New Size(card.Width - 36, 25), .Font = New Font("Segoe UI", 15.0F, FontStyle.Bold), .ForeColor = RentLinkRoundedCorners.InkColor, .AutoEllipsis = True, .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right})
        card.Controls.Add(New Label With {.Text = listing.Location & " · " & listing.Kind, .Location = New Point(19, 47), .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = RentLinkRoundedCorners.MutedColor})
        Dim stateColor = If(listing.Status = "Approved", RentLinkRoundedCorners.AccentColor, If(listing.Status = "Rejected", Color.FromArgb(175, 55, 55), Color.FromArgb(180, 119, 31)))
        card.Controls.Add(New Label With {.Text = listing.Status.ToUpperInvariant(), .Location = New Point(19, 78), .AutoSize = True, .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold), .ForeColor = stateColor})
        card.Controls.Add(New Label With {.Text = metricsText, .Location = New Point(19, 112), .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = RentLinkRoundedCorners.InkColor})
        Dim availability As New CheckBox With {.Text = If(listing.Available, "Available", "Unavailable"), .Checked = listing.Available, .AutoCheck = True, .AutoSize = True, .Location = New Point(19, 155), .Font = New Font("Segoe UI", 9.0F), .ForeColor = RentLinkRoundedCorners.InkColor, .Tag = listing}
        card.Controls.Add(availability)
        AddHandler availability.CheckedChanged, Sub() AvailabilityToggleChanged(listing, availability)
        card.Controls.Add(New Label With {.Text = If(listing.Available, visibilityText, "Hidden from the home page until you mark it available."), .Location = New Point(250, 158), .Size = New Size(Math.Max(300, card.Width - 270), 25), .Font = New Font("Segoe UI", 9.0F), .ForeColor = RentLinkRoundedCorners.MutedColor, .AutoEllipsis = True, .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right, .Tag = "availabilityVisibility"})
        propertyCardsFlow.Controls.Add(card)
    End Sub

    Public Sub SetAvailability(listing As RentLinkWorkspace.PropertyListing, available As Boolean)
        For Each card As Control In propertyCardsFlow.Controls
            If Object.ReferenceEquals(card.Tag, listing) Then
                For Each child As Control In card.Controls
                    If TypeOf child Is CheckBox Then
                        Dim toggle = DirectCast(child, CheckBox)
                        Try
                            updatingAvailability = True
                            toggle.Checked = available
                            toggle.Text = If(available, "Available", "Unavailable")
                        Finally
                            updatingAvailability = False
                        End Try
                        For Each detail As Control In card.Controls
                            If String.Equals(TryCast(detail.Tag, String), "availabilityVisibility", StringComparison.Ordinal) Then
                                detail.Text = If(available, "Visible in the " & If(listing.Mode = "Both", "Rent and Purchase", listing.Mode) & " section", "Hidden from the home page until you mark it available.")
                                Exit For
                            End If
                        Next
                        Return
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub AvailabilityToggleChanged(listing As RentLinkWorkspace.PropertyListing, toggle As CheckBox)
        If updatingAvailability Then Return
        toggle.Text = If(toggle.Checked, "Available", "Unavailable")
        RaiseEvent AvailabilityChanged(Me, New PropertyAvailabilityEventArgs(listing, toggle.Checked))
    End Sub

    Private Sub AddPropertyClicked(sender As Object, e As EventArgs) Handles addPropertyButton.Click
        RaiseEvent AddPropertyRequested(Me, EventArgs.Empty)
    End Sub
End Class
