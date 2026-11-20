<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RentLinkPropertyDetailsControl
    Inherits System.Windows.Forms.UserControl

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer
    Friend WithEvents backButton As System.Windows.Forms.Button
    Friend WithEvents titleLabel As System.Windows.Forms.Label
    Friend WithEvents locationLabel As System.Windows.Forms.Label
    Friend WithEvents galleryFlow As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents detailsHeadingLabel As System.Windows.Forms.Label
    Friend WithEvents factsLabel As System.Windows.Forms.Label
    Friend WithEvents descriptionLabel As System.Windows.Forms.Label
    Friend WithEvents directionsButton As System.Windows.Forms.Button
    Friend WithEvents enquiryPanel As System.Windows.Forms.Panel
    Friend WithEvents priceLabel As System.Windows.Forms.Label
    Friend WithEvents ownerLabel As System.Windows.Forms.Label
    Friend WithEvents paymentNoteLabel As System.Windows.Forms.Label
    Friend WithEvents viewingLabel As System.Windows.Forms.Label
    Friend WithEvents viewingDatePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents renterPhoneLabel As System.Windows.Forms.Label
    Friend WithEvents renterPhoneInput As System.Windows.Forms.TextBox
    Friend WithEvents saveButton As System.Windows.Forms.Button
    Friend WithEvents enquireButton As System.Windows.Forms.Button
    Friend WithEvents contactLabel As System.Windows.Forms.LinkLabel

    Private Sub InitializeComponent()
        backButton = New Button()
        titleLabel = New Label()
        locationLabel = New Label()
        galleryFlow = New FlowLayoutPanel()
        detailsHeadingLabel = New Label()
        factsLabel = New Label()
        descriptionLabel = New Label()
        directionsButton = New Button()
        enquiryPanel = New Panel()
        priceLabel = New Label()
        ownerLabel = New Label()
        paymentNoteLabel = New Label()
        viewingLabel = New Label()
        viewingDatePicker = New DateTimePicker()
        renterPhoneLabel = New Label()
        renterPhoneInput = New TextBox()
        saveButton = New Button()
        enquireButton = New Button()
        contactLabel = New LinkLabel()
        enquiryPanel.SuspendLayout()
        SuspendLayout()
        backButton.BackColor = Color.White
        backButton.FlatAppearance.BorderSize = 0
        backButton.FlatStyle = FlatStyle.Flat
        backButton.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        backButton.ForeColor = Color.FromArgb(13, 148, 136)
        backButton.Location = New Point(30, 20)
        backButton.Name = "backButton"
        backButton.Size = New Size(145, 35)
        backButton.TabIndex = 0
        backButton.Text = "←  Back to homes"
        backButton.UseVisualStyleBackColor = False
        titleLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        titleLabel.Font = New Font("Segoe UI", 25.0F, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(30, 41, 59)
        titleLabel.Location = New Point(30, 70)
        titleLabel.Name = "titleLabel"
        titleLabel.Size = New Size(950, 50)
        titleLabel.TabIndex = 1
        titleLabel.Text = "Property title"
        locationLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        locationLabel.Font = New Font("Segoe UI", 10.0F)
        locationLabel.ForeColor = Color.FromArgb(100, 116, 139)
        locationLabel.Location = New Point(32, 122)
        locationLabel.Name = "locationLabel"
        locationLabel.Size = New Size(950, 24)
        locationLabel.TabIndex = 2
        locationLabel.Text = "Property location"
        galleryFlow.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        galleryFlow.AutoScroll = True
        galleryFlow.BackColor = Color.FromArgb(248, 250, 252)
        galleryFlow.Location = New Point(30, 155)
        galleryFlow.Name = "galleryFlow"
        galleryFlow.Size = New Size(950, 220)
        galleryFlow.TabIndex = 3
        galleryFlow.WrapContents = False
        detailsHeadingLabel.AutoSize = True
        detailsHeadingLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        detailsHeadingLabel.ForeColor = Color.FromArgb(13, 148, 136)
        detailsHeadingLabel.Location = New Point(30, 390)
        detailsHeadingLabel.Name = "detailsHeadingLabel"
        detailsHeadingLabel.Size = New Size(126, 15)
        detailsHeadingLabel.TabIndex = 4
        detailsHeadingLabel.Text = "PROPERTY DETAILS"
        factsLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        factsLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        factsLabel.ForeColor = Color.FromArgb(30, 41, 59)
        factsLabel.Location = New Point(30, 416)
        factsLabel.Name = "factsLabel"
        factsLabel.Size = New Size(950, 30)
        factsLabel.TabIndex = 5
        factsLabel.Text = "Bedrooms · Property type · Listing mode"
        descriptionLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        descriptionLabel.Font = New Font("Segoe UI", 10.0F)
        descriptionLabel.ForeColor = Color.FromArgb(100, 116, 139)
        descriptionLabel.Location = New Point(30, 455)
        descriptionLabel.Name = "descriptionLabel"
        descriptionLabel.Size = New Size(950, 70)
        descriptionLabel.TabIndex = 6
        descriptionLabel.Text = "Property description"
        directionsButton.BackColor = Color.FromArgb(204, 251, 241)
        directionsButton.FlatAppearance.BorderSize = 0
        directionsButton.FlatStyle = FlatStyle.Flat
        directionsButton.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        directionsButton.ForeColor = Color.FromArgb(13, 148, 136)
        directionsButton.Location = New Point(30, 535)
        directionsButton.Name = "directionsButton"
        directionsButton.Size = New Size(195, 38)
        directionsButton.TabIndex = 7
        directionsButton.Text = "Open directions in Maps  ↗"
        directionsButton.UseVisualStyleBackColor = False
        enquiryPanel.BackColor = Color.White
        enquiryPanel.Controls.Add(priceLabel)
        enquiryPanel.Controls.Add(ownerLabel)
        enquiryPanel.Controls.Add(paymentNoteLabel)
        enquiryPanel.Controls.Add(viewingLabel)
        enquiryPanel.Controls.Add(viewingDatePicker)
        enquiryPanel.Controls.Add(renterPhoneLabel)
        enquiryPanel.Controls.Add(renterPhoneInput)
        enquiryPanel.Controls.Add(saveButton)
        enquiryPanel.Controls.Add(enquireButton)
        enquiryPanel.Controls.Add(contactLabel)
        enquiryPanel.Location = New Point(30, 590)
        enquiryPanel.Name = "enquiryPanel"
        enquiryPanel.Size = New Size(380, 360)
        enquiryPanel.TabIndex = 8
        priceLabel.AutoSize = True
        priceLabel.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)
        priceLabel.ForeColor = Color.FromArgb(30, 41, 59)
        priceLabel.Location = New Point(18, 18)
        priceLabel.Name = "priceLabel"
        priceLabel.Size = New Size(150, 32)
        priceLabel.TabIndex = 0
        priceLabel.Text = "GH₵ 0.00"
        ownerLabel.Location = New Point(20, 58)
        ownerLabel.Name = "ownerLabel"
        ownerLabel.Size = New Size(340, 22)
        ownerLabel.TabIndex = 1
        ownerLabel.Text = "Listed by owner"
        paymentNoteLabel.Location = New Point(20, 80)
        paymentNoteLabel.Name = "paymentNoteLabel"
        paymentNoteLabel.Size = New Size(340, 20)
        paymentNoteLabel.TabIndex = 2
        paymentNoteLabel.Text = "Payment arrangements confirmed with the owner"
        viewingLabel.AutoSize = True
        viewingLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        viewingLabel.Location = New Point(20, 105)
        viewingLabel.Name = "viewingLabel"
        viewingLabel.Size = New Size(145, 19)
        viewingLabel.TabIndex = 3
        viewingLabel.Text = "Choose a viewing day"
        viewingDatePicker.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        viewingDatePicker.Font = New Font("Segoe UI", 9.0F)
        viewingDatePicker.Format = DateTimePickerFormat.Long
        viewingDatePicker.Location = New Point(20, 130)
        viewingDatePicker.MinDate = Date.Today.AddDays(1)
        viewingDatePicker.Name = "viewingDatePicker"
        viewingDatePicker.Size = New Size(340, 23)
        viewingDatePicker.TabIndex = 4
        renterPhoneLabel.AutoSize = True
        renterPhoneLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        renterPhoneLabel.Location = New Point(20, 165)
        renterPhoneLabel.Name = "renterPhoneLabel"
        renterPhoneLabel.Size = New Size(150, 15)
        renterPhoneLabel.TabIndex = 8
        renterPhoneLabel.Text = "Your phone number"
        renterPhoneInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        renterPhoneInput.Font = New Font("Segoe UI", 9.0F)
        renterPhoneInput.Location = New Point(20, 185)
        renterPhoneInput.Name = "renterPhoneInput"
        renterPhoneInput.PlaceholderText = "+233..."
        renterPhoneInput.Size = New Size(340, 23)
        renterPhoneInput.TabIndex = 5
        saveButton.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        saveButton.BackColor = Color.FromArgb(204, 251, 241)
        saveButton.FlatAppearance.BorderSize = 0
        saveButton.FlatStyle = FlatStyle.Flat
        saveButton.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        saveButton.ForeColor = Color.FromArgb(13, 148, 136)
        saveButton.Location = New Point(20, 220)
        saveButton.Name = "saveButton"
        saveButton.Size = New Size(340, 38)
        saveButton.TabIndex = 6
        saveButton.Text = "Save this home"
        saveButton.UseVisualStyleBackColor = False
        enquireButton.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        enquireButton.BackColor = Color.FromArgb(13, 148, 136)
        enquireButton.FlatAppearance.BorderSize = 0
        enquireButton.FlatStyle = FlatStyle.Flat
        enquireButton.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        enquireButton.ForeColor = Color.White
        enquireButton.Location = New Point(20, 263)
        enquireButton.Name = "enquireButton"
        enquireButton.Size = New Size(340, 42)
        enquireButton.TabIndex = 7
        enquireButton.Text = "Enquire / request a viewing"
        enquireButton.UseVisualStyleBackColor = False
        contactLabel.LinkBehavior = LinkBehavior.HoverUnderline
        contactLabel.LinkColor = Color.FromArgb(13, 148, 136)
        contactLabel.Location = New Point(20, 315)
        contactLabel.Name = "contactLabel"
        contactLabel.Size = New Size(340, 30)
        contactLabel.TabIndex = 7
        contactLabel.Text = "Contact owner via preferred method"
        contactLabel.UseMnemonic = False
        contactLabel.VisitedLinkColor = Color.FromArgb(13, 148, 136)
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        AutoScroll = True
        BackColor = Color.FromArgb(248, 250, 252)
        Controls.Add(enquiryPanel)
        Controls.Add(directionsButton)
        Controls.Add(descriptionLabel)
        Controls.Add(factsLabel)
        Controls.Add(detailsHeadingLabel)
        Controls.Add(galleryFlow)
        Controls.Add(locationLabel)
        Controls.Add(titleLabel)
        Controls.Add(backButton)
        Name = "RentLinkPropertyDetailsControl"
        Size = New Size(1020, 700)
        enquiryPanel.ResumeLayout(False)
        enquiryPanel.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
End Class
