<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RentLinkPropertyListingControl
    Inherits System.Windows.Forms.UserControl

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer
    Friend WithEvents headingLabel As System.Windows.Forms.Label
    Friend WithEvents subtitleLabel As System.Windows.Forms.Label
    Friend WithEvents formPanel As System.Windows.Forms.Panel
    Friend WithEvents headlineLabel As System.Windows.Forms.Label
    Friend WithEvents headlineInput As System.Windows.Forms.TextBox
    Friend WithEvents locationLabel As System.Windows.Forms.Label
    Friend WithEvents locationInput As System.Windows.Forms.TextBox
    Friend WithEvents typeLabel As System.Windows.Forms.Label
    Friend WithEvents propertyTypeInput As System.Windows.Forms.ComboBox
    Friend WithEvents modeLabel As System.Windows.Forms.Label
    Friend WithEvents listingModeInput As System.Windows.Forms.ComboBox
    Friend WithEvents pricingPanel As System.Windows.Forms.Panel
    Friend WithEvents bedroomsLabel As System.Windows.Forms.Label
    Friend WithEvents bedroomsInput As System.Windows.Forms.TextBox
    Friend WithEvents contactLabel As System.Windows.Forms.Label
    Friend WithEvents contactInput As System.Windows.Forms.TextBox
    Friend WithEvents communicationLabel As System.Windows.Forms.Label
    Friend WithEvents communicationInput As System.Windows.Forms.ComboBox
    Friend WithEvents descriptionLabel As System.Windows.Forms.Label
    Friend WithEvents descriptionInput As System.Windows.Forms.TextBox
    Friend WithEvents photosLabel As System.Windows.Forms.Label
    Friend WithEvents choosePhotosButton As System.Windows.Forms.Button
    Friend WithEvents photoCountLabel As System.Windows.Forms.Label
    Friend WithEvents submitButton As System.Windows.Forms.Button

    Private Sub InitializeComponent()
        headingLabel = New Label()
        subtitleLabel = New Label()
        formPanel = New Panel()
        headlineLabel = New Label()
        headlineInput = New TextBox()
        locationLabel = New Label()
        locationInput = New TextBox()
        typeLabel = New Label()
        propertyTypeInput = New ComboBox()
        modeLabel = New Label()
        listingModeInput = New ComboBox()
        pricingPanel = New Panel()
        bedroomsLabel = New Label()
        bedroomsInput = New TextBox()
        contactLabel = New Label()
        contactInput = New TextBox()
        communicationLabel = New Label()
        communicationInput = New ComboBox()
        descriptionLabel = New Label()
        descriptionInput = New TextBox()
        photosLabel = New Label()
        choosePhotosButton = New Button()
        photoCountLabel = New Label()
        submitButton = New Button()
        formPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' headingLabel
        ' 
        headingLabel.AutoSize = True
        headingLabel.Font = New Font("Segoe UI", 25.0F, FontStyle.Bold)
        headingLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        headingLabel.Location = New Point(30, 22)
        headingLabel.Name = "headingLabel"
        headingLabel.Size = New Size(308, 46)
        headingLabel.TabIndex = 0
        headingLabel.Text = "List your property"
        ' 
        ' subtitleLabel
        ' 
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 10.0F)
        subtitleLabel.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        subtitleLabel.Location = New Point(31, 62)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(417, 19)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Share the details. Our team reviews every listing before it goes live."
        ' 
        ' formPanel
        ' 
        formPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        formPanel.AutoScroll = True
        formPanel.BackColor = Color.White
        formPanel.Controls.Add(headlineLabel)
        formPanel.Controls.Add(headlineInput)
        formPanel.Controls.Add(locationLabel)
        formPanel.Controls.Add(locationInput)
        formPanel.Controls.Add(typeLabel)
        formPanel.Controls.Add(propertyTypeInput)
        formPanel.Controls.Add(modeLabel)
        formPanel.Controls.Add(listingModeInput)
        formPanel.Controls.Add(pricingPanel)
        formPanel.Controls.Add(bedroomsLabel)
        formPanel.Controls.Add(bedroomsInput)
        formPanel.Controls.Add(contactLabel)
        formPanel.Controls.Add(contactInput)
        formPanel.Controls.Add(communicationLabel)
        formPanel.Controls.Add(communicationInput)
        formPanel.Controls.Add(descriptionLabel)
        formPanel.Controls.Add(descriptionInput)
        formPanel.Controls.Add(photosLabel)
        formPanel.Controls.Add(choosePhotosButton)
        formPanel.Controls.Add(photoCountLabel)
        formPanel.Controls.Add(submitButton)
        formPanel.Location = New Point(30, 98)
        formPanel.Name = "formPanel"
        formPanel.Padding = New Padding(22)
        formPanel.Size = New Size(850, 1036)
        formPanel.TabIndex = 2
        ' 
        ' headlineLabel
        ' 
        headlineLabel.AutoSize = True
        headlineLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        headlineLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        headlineLabel.Location = New Point(22, 18)
        headlineLabel.Name = "headlineLabel"
        headlineLabel.Size = New Size(106, 15)
        headlineLabel.TabIndex = 0
        headlineLabel.Text = "Property headline"
        ' 
        ' headlineInput
        ' 
        headlineInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        headlineInput.Font = New Font("Segoe UI", 10.0F)
        headlineInput.Location = New Point(22, 40)
        headlineInput.Name = "headlineInput"
        headlineInput.PlaceholderText = "e.g. Bright family home with garden"
        headlineInput.Size = New Size(797, 25)
        headlineInput.TabIndex = 1
        ' 
        ' locationLabel
        ' 
        locationLabel.AutoSize = True
        locationLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        locationLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        locationLabel.Location = New Point(22, 82)
        locationLabel.Name = "locationLabel"
        locationLabel.Size = New Size(54, 15)
        locationLabel.TabIndex = 2
        locationLabel.Text = "Location"
        ' 
        ' locationInput
        ' 
        locationInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        locationInput.Font = New Font("Segoe UI", 10.0F)
        locationInput.Location = New Point(22, 104)
        locationInput.Name = "locationInput"
        locationInput.PlaceholderText = "Suburb, city"
        locationInput.Size = New Size(797, 25)
        locationInput.TabIndex = 3
        ' 
        ' typeLabel
        ' 
        typeLabel.AutoSize = True
        typeLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        typeLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        typeLabel.Location = New Point(22, 146)
        typeLabel.Name = "typeLabel"
        typeLabel.Size = New Size(84, 15)
        typeLabel.TabIndex = 4
        typeLabel.Text = "Property type"
        ' 
        ' propertyTypeInput
        ' 
        propertyTypeInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        propertyTypeInput.DropDownStyle = ComboBoxStyle.DropDownList
        propertyTypeInput.Font = New Font("Segoe UI", 10.0F)
        propertyTypeInput.Location = New Point(22, 168)
        propertyTypeInput.Name = "propertyTypeInput"
        propertyTypeInput.Size = New Size(797, 25)
        propertyTypeInput.TabIndex = 5
        ' 
        ' modeLabel
        ' 
        modeLabel.AutoSize = True
        modeLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        modeLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        modeLabel.Location = New Point(22, 210)
        modeLabel.Name = "modeLabel"
        modeLabel.Size = New Size(121, 15)
        modeLabel.TabIndex = 6
        modeLabel.Text = "List this property for"
        ' 
        ' listingModeInput
        ' 
        listingModeInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        listingModeInput.DropDownStyle = ComboBoxStyle.DropDownList
        listingModeInput.Font = New Font("Segoe UI", 10.0F)
        listingModeInput.Location = New Point(22, 232)
        listingModeInput.Name = "listingModeInput"
        listingModeInput.Size = New Size(797, 25)
        listingModeInput.TabIndex = 7
        ' 
        ' pricingPanel
        ' 
        pricingPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        pricingPanel.Location = New Point(22, 274)
        pricingPanel.Name = "pricingPanel"
        pricingPanel.Size = New Size(797, 132)
        pricingPanel.TabIndex = 8
        ' 
        ' bedroomsLabel
        ' 
        bedroomsLabel.AutoSize = True
        bedroomsLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        bedroomsLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        bedroomsLabel.Location = New Point(22, 418)
        bedroomsLabel.Name = "bedroomsLabel"
        bedroomsLabel.Size = New Size(64, 15)
        bedroomsLabel.TabIndex = 9
        bedroomsLabel.Text = "Bedrooms"
        ' 
        ' bedroomsInput
        ' 
        bedroomsInput.Font = New Font("Segoe UI", 10.0F)
        bedroomsInput.Location = New Point(22, 440)
        bedroomsInput.Name = "bedroomsInput"
        bedroomsInput.PlaceholderText = "Number of bedrooms"
        bedroomsInput.Size = New Size(780, 25)
        bedroomsInput.TabIndex = 10
        ' 
        ' contactLabel
        ' 
        contactLabel.AutoSize = True
        contactLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        contactLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        contactLabel.Location = New Point(22, 482)
        contactLabel.Name = "contactLabel"
        contactLabel.Size = New Size(146, 15)
        contactLabel.TabIndex = 11
        contactLabel.Text = "Preferred contact details"
        ' 
        ' contactInput
        ' 
        contactInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        contactInput.Font = New Font("Segoe UI", 10.0F)
        contactInput.Location = New Point(22, 504)
        contactInput.Name = "contactInput"
        contactInput.PlaceholderText = "Phone number or email"
        contactInput.Size = New Size(797, 25)
        contactInput.TabIndex = 12
        ' 
        ' communicationLabel
        ' 
        communicationLabel.AutoSize = True
        communicationLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        communicationLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        communicationLabel.Location = New Point(22, 546)
        communicationLabel.Name = "communicationLabel"
        communicationLabel.Size = New Size(151, 15)
        communicationLabel.TabIndex = 13
        communicationLabel.Text = "Preferred communication"
        ' 
        ' communicationInput
        ' 
        communicationInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        communicationInput.DropDownStyle = ComboBoxStyle.DropDownList
        communicationInput.Font = New Font("Segoe UI", 10.0F)
        communicationInput.Location = New Point(22, 568)
        communicationInput.Name = "communicationInput"
        communicationInput.Size = New Size(797, 25)
        communicationInput.TabIndex = 14
        ' 
        ' descriptionLabel
        ' 
        descriptionLabel.AutoSize = True
        descriptionLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        descriptionLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        descriptionLabel.Location = New Point(22, 610)
        descriptionLabel.Name = "descriptionLabel"
        descriptionLabel.Size = New Size(99, 15)
        descriptionLabel.TabIndex = 15
        descriptionLabel.Text = "About this home"
        ' 
        ' descriptionInput
        ' 
        descriptionInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        descriptionInput.Font = New Font("Segoe UI", 10.0F)
        descriptionInput.Location = New Point(22, 632)
        descriptionInput.Multiline = True
        descriptionInput.Name = "descriptionInput"
        descriptionInput.PlaceholderText = "Describe the home, its features and what makes it special..."
        descriptionInput.Size = New Size(797, 110)
        descriptionInput.TabIndex = 16
        ' 
        ' photosLabel
        ' 
        photosLabel.AutoSize = True
        photosLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        photosLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        photosLabel.Location = New Point(22, 762)
        photosLabel.Name = "photosLabel"
        photosLabel.Size = New Size(97, 15)
        photosLabel.TabIndex = 17
        photosLabel.Text = "Property photos"
        ' 
        ' choosePhotosButton
        ' 
        choosePhotosButton.BackColor = Color.FromArgb(CByte(204), CByte(251), CByte(241))
        choosePhotosButton.FlatAppearance.BorderSize = 0
        choosePhotosButton.FlatStyle = FlatStyle.Flat
        choosePhotosButton.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        choosePhotosButton.ForeColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        choosePhotosButton.Location = New Point(22, 787)
        choosePhotosButton.Name = "choosePhotosButton"
        choosePhotosButton.Size = New Size(280, 40)
        choosePhotosButton.TabIndex = 18
        choosePhotosButton.Text = "Add property photos (first is the cover)"
        choosePhotosButton.UseVisualStyleBackColor = False
        ' 
        ' photoCountLabel
        ' 
        photoCountLabel.AutoSize = True
        photoCountLabel.Font = New Font("Segoe UI", 9.0F)
        photoCountLabel.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        photoCountLabel.Location = New Point(316, 800)
        photoCountLabel.Name = "photoCountLabel"
        photoCountLabel.Size = New Size(99, 15)
        photoCountLabel.TabIndex = 19
        photoCountLabel.Text = "0 photos selected"
        ' 
        ' submitButton
        ' 
        submitButton.BackColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        submitButton.FlatAppearance.BorderSize = 0
        submitButton.FlatStyle = FlatStyle.Flat
        submitButton.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        submitButton.ForeColor = Color.White
        submitButton.Location = New Point(22, 850)
        submitButton.Name = "submitButton"
        submitButton.Size = New Size(230, 44)
        submitButton.TabIndex = 20
        submitButton.Text = "Submit for admin review"
        submitButton.UseVisualStyleBackColor = False
        ' 
        ' RentLinkPropertyListingControl
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        Controls.Add(formPanel)
        Controls.Add(subtitleLabel)
        Controls.Add(headingLabel)
        Name = "RentLinkPropertyListingControl"
        Size = New Size(940, 1176)
        formPanel.ResumeLayout(False)
        formPanel.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
End Class
