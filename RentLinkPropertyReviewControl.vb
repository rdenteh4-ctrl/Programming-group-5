Imports System.Drawing
Imports System.Windows.Forms

Public Class AdminListingReviewEventArgs
    Inherits EventArgs

    Public Sub New(listing As RentLinkWorkspace.PropertyListing, status As String)
        Me.Listing = listing
        Me.Status = status
    End Sub

    Public ReadOnly Property Listing As RentLinkWorkspace.PropertyListing
    Public ReadOnly Property Status As String
End Class

Partial Public Class RentLinkPropertyReviewControl
    Inherits UserControl

    Public Event ReviewRequested As EventHandler(Of AdminListingReviewEventArgs)

    Public Sub New()
        InitializeComponent()
        BackColor = RentLinkRoundedCorners.PageColor
        RentLinkRoundedCorners.ApplyTo(Me)
    End Sub

    Public Sub SetPendingCount(count As Integer)
        pendingCountLabel.Text = count & If(count = 1, " property pending review", " properties pending review")
    End Sub

    Public Sub ClearListings()
        listingsFlow.Controls.Clear()
    End Sub

    Public Sub ShowEmptyState()
        Dim empty As New Label With {.Text = "No properties are waiting for review.", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F), .ForeColor = RentLinkRoundedCorners.MutedColor, .Margin = New Padding(0, 8, 0, 0)}
        listingsFlow.Controls.Add(empty)
    End Sub

    Public Sub AddListing(listing As RentLinkWorkspace.PropertyListing, priceText As String, paymentDetailsText As String)
        Dim card As New Panel With {.BackColor = Color.White, .Size = New Size(Math.Max(650, listingsFlow.ClientSize.Width - 24), 280), .Margin = New Padding(0, 0, 0, 14), .Tag = listing}
        Dim title = CreateLabel(listing.Title, 13, RentLinkRoundedCorners.InkColor, True, New Point(15, 14), New Size(card.Width - 165, 24))
        Dim place = CreateLabel(listing.Location & " · " & listing.Kind & " · " & listing.Mode, 9, RentLinkRoundedCorners.MutedColor, False, New Point(16, 43), New Size(card.Width - 165, 20))
        Dim price = CreateLabel(priceText, 10, RentLinkRoundedCorners.AccentColor, True, New Point(16, 70), New Size(card.Width - 165, 22))
        Dim owner = CreateLabel("Owner: " & listing.Owner & " · " & listing.OwnerEmail & " · " & listing.Contact & " (" & listing.Communication & ")", 9, RentLinkRoundedCorners.MutedColor, False, New Point(16, 98), New Size(card.Width - 165, 20))
        Dim description = CreateLabel(listing.Description, 9, RentLinkRoundedCorners.InkColor, False, New Point(16, 125), New Size(card.Width - 165, 55))
        description.AutoEllipsis = False
        Dim paymentDetails = CreateLabel(paymentDetailsText, 8.5F, RentLinkRoundedCorners.MutedColor, False, New Point(16, 190), New Size(card.Width - 165, 76))
        paymentDetails.AutoEllipsis = False
        card.Controls.AddRange({title, place, price, owner, description, paymentDetails})

        Dim approve As New Button With {.Text = "Approve", .Location = New Point(card.Width - 125, 43), .Size = New Size(100, 36), .BackColor = RentLinkRoundedCorners.AccentColor, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .UseVisualStyleBackColor = False, .Anchor = AnchorStyles.Top Or AnchorStyles.Right}
        approve.FlatAppearance.BorderSize = 0
        card.Controls.Add(approve)
        AddHandler approve.Click, Sub() RaiseEvent ReviewRequested(Me, New AdminListingReviewEventArgs(listing, "Approved"))

        Dim reject As New Button With {.Text = "Reject", .Location = New Point(card.Width - 125, 92), .Size = New Size(100, 36), .BackColor = Color.FromArgb(255, 239, 239), .ForeColor = Color.FromArgb(175, 55, 55), .FlatStyle = FlatStyle.Flat, .UseVisualStyleBackColor = False, .Anchor = AnchorStyles.Top Or AnchorStyles.Right}
        reject.FlatAppearance.BorderSize = 0
        card.Controls.Add(reject)
        AddHandler reject.Click, Sub() RaiseEvent ReviewRequested(Me, New AdminListingReviewEventArgs(listing, "Rejected"))
        listingsFlow.Controls.Add(card)
    End Sub

    Private Function CreateLabel(text As String, size As Single, color As Color, bold As Boolean, location As Point, bounds As Size) As Label
        Return New Label With {.Text = text, .Location = location, .Size = bounds, .Font = New Font("Segoe UI", size, If(bold, FontStyle.Bold, FontStyle.Regular)), .ForeColor = color, .AutoEllipsis = True}
    End Function
End Class
