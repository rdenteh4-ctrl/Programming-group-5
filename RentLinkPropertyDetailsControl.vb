Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Partial Public Class RentLinkPropertyDetailsControl
    Inherits UserControl

    Public Event BackRequested As EventHandler
    Public Event DirectionsRequested As EventHandler
    Public Event SaveRequested As EventHandler
    Public Event EnquireRequested As EventHandler
    Public Event ContactRequested As EventHandler
    Public Event MakePaymentRequested As EventHandler

    Private ReadOnly makePaymentButton As New Button()

    Public Sub New()
        InitializeComponent()
        makePaymentButton.BackColor = Color.FromArgb(13, 148, 136)
        makePaymentButton.FlatAppearance.BorderSize = 0
        makePaymentButton.FlatStyle = FlatStyle.Flat
        makePaymentButton.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        makePaymentButton.ForeColor = Color.White
        makePaymentButton.Location = New Point(20, 355)
        makePaymentButton.Name = "makePaymentButton"
        makePaymentButton.Size = New Size(340, 38)
        makePaymentButton.Text = "Make payment"
        makePaymentButton.UseVisualStyleBackColor = False
        makePaymentButton.Visible = False
        enquiryPanel.Controls.Add(makePaymentButton)
        AddHandler makePaymentButton.Click, AddressOf MakePaymentButtonClicked
        BackColor = RentLinkRoundedCorners.PageColor
        RentLinkRoundedCorners.ApplyTo(Me)
    End Sub

    <Browsable(False)>
    Public ReadOnly Property ImageGallery As FlowLayoutPanel
        Get
            Return galleryFlow
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ViewingDate As DateTimePicker
        Get
            Return viewingDatePicker
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property RenterPhone As TextBox
        Get
            Return renterPhoneInput
        End Get
    End Property

    Public Sub SetListingDetails(title As String, location As String, facts As String, description As String, price As String, owner As String, contact As String, communication As String, saveText As String)
        titleLabel.Text = title
        locationLabel.Text = location
        factsLabel.Text = facts
        descriptionLabel.Text = description
        descriptionLabel.Width = Math.Max(320, ClientSize.Width - 60)
        descriptionLabel.Height = Math.Max(60, TextRenderer.MeasureText(description, descriptionLabel.Font, New Size(descriptionLabel.Width, 100000), TextFormatFlags.WordBreak).Height + 8)
        directionsButton.Top = descriptionLabel.Bottom + 12
        enquiryPanel.Top = directionsButton.Bottom + 18
        priceLabel.Text = price
        ownerLabel.Text = "Listed by " & owner
        contactLabel.Text = "Contact owner via " & communication & ": " & contact
        contactLabel.LinkArea = New LinkArea(0, contactLabel.Text.Length)
        saveButton.Text = saveText
    End Sub

    Public Sub AddGalleryPlaceholder(caption As String, size As Size, color As Color)
        Dim image As New Panel With {.Size = size, .BackColor = color, .Margin = New Padding(0, 0, 10, 0)}
        Dim label As New Label With {.Text = caption, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleCenter, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(66, 89, 84)}
        image.Controls.Add(label)
        galleryFlow.Controls.Add(image)
    End Sub

    Public Sub SetSaveText(value As String)
        saveButton.Text = value
    End Sub

    Public Sub SetPaymentMode(enabled As Boolean, Optional alreadyPurchased As Boolean = False, Optional isPurchase As Boolean = False, Optional alreadyPaidRent As Boolean = False)
        makePaymentButton.Visible = enabled OrElse alreadyPurchased OrElse alreadyPaidRent
        makePaymentButton.Enabled = enabled
        If alreadyPurchased Then
            makePaymentButton.Text = "Bought this property"
        ElseIf alreadyPaidRent Then
            makePaymentButton.Text = "You paid your rent"
        Else
            makePaymentButton.Text = If(isPurchase, "Make payment", "Pay rent")
        End If
        paymentNoteLabel.Text = If(enabled, "Simulated payment to the property owner's account · no real funds transferred", If(alreadyPurchased, "This property has already been purchased", "Confirm payment arrangements with the owner"))
        enquiryPanel.Height = If(makePaymentButton.Visible, 405, 360)
    End Sub

    Private Sub BackButtonClicked(sender As Object, e As EventArgs) Handles backButton.Click
        RaiseEvent BackRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub DirectionsButtonClicked(sender As Object, e As EventArgs) Handles directionsButton.Click
        RaiseEvent DirectionsRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub SaveButtonClicked(sender As Object, e As EventArgs) Handles saveButton.Click
        RaiseEvent SaveRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub EnquireButtonClicked(sender As Object, e As EventArgs) Handles enquireButton.Click
        RaiseEvent EnquireRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub ContactLinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles contactLabel.LinkClicked
        RaiseEvent ContactRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub MakePaymentButtonClicked(sender As Object, e As EventArgs)
        RaiseEvent MakePaymentRequested(Me, EventArgs.Empty)
    End Sub
End Class
