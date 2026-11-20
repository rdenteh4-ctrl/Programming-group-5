Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Partial Public Class RentLinkPropertyListingControl
    Inherits UserControl

    Public Event ChoosePhotosRequested As EventHandler
    Public Event SubmitRequested As EventHandler

    Private rentPriceInput As TextBox
    Private purchasePriceInput As TextBox
    Private rentPeriodInput As ComboBox
    Private ReadOnly paymentOptionInput As New ComboBox()
    Private ReadOnly paymentFieldsPanel As New Panel()
    Private ReadOnly mobileMoneyPanel As New Panel()
    Private ReadOnly bankAccountPanel As New Panel()
    Private ReadOnly mobileMoneyNetworkInput As New ComboBox()
    Private ReadOnly mobileMoneyNameInput As New TextBox()
    Private ReadOnly mobileMoneyNumberInput As New TextBox()
    Private ReadOnly bankNameInput As New TextBox()
    Private ReadOnly bankAccountNameInput As New TextBox()
    Private ReadOnly bankAccountNumberInput As New TextBox()
    Private paymentOptionLabel As Label

    Public Sub New()
        InitializeComponent()
        BackColor = RentLinkRoundedCorners.PageColor
        InitializePaymentInputs()
        propertyTypeInput.Items.AddRange(New Object() {"House", "Apartment", "Townhouse", "Cottage", "Land"})
        propertyTypeInput.SelectedIndex = 0
        listingModeInput.Items.AddRange(New Object() {"Rent", "Purchase", "Both"})
        listingModeInput.SelectedIndex = 0
        communicationInput.Items.AddRange(New Object() {"WhatsApp", "Phone", "Email"})
        communicationInput.SelectedIndex = 0
        UpdatePricingFields()
        UpdatePaymentFields()
        RentLinkRoundedCorners.ApplyTo(Me)
    End Sub

    <Browsable(False)>
    Public ReadOnly Property PropertyHeadlineInput As TextBox
        Get
            Return headlineInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property PropertyLocationInput As TextBox
        Get
            Return locationInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property PropertyTypeChoice As ComboBox
        Get
            Return propertyTypeInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ListingModeChoice As ComboBox
        Get
            Return listingModeInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property BedroomCountField As TextBox
        Get
            Return bedroomsInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ContactDetailsInput As TextBox
        Get
            Return contactInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property CommunicationChoice As ComboBox
        Get
            Return communicationInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property PropertyDescriptionInput As TextBox
        Get
            Return descriptionInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property RentAmountInput As TextBox
        Get
            Return rentPriceInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property PurchaseAmountInput As TextBox
        Get
            Return purchasePriceInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property RentPeriodChoice As ComboBox
        Get
            Return rentPeriodInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property PaymentOptionChoice As ComboBox
        Get
            Return paymentOptionInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property MobileMoneyNetworkChoice As ComboBox
        Get
            Return mobileMoneyNetworkInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property MobileMoneyAccountNameInput As TextBox
        Get
            Return mobileMoneyNameInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property MobileMoneyNumberField As TextBox
        Get
            Return mobileMoneyNumberInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property BankNameField As TextBox
        Get
            Return bankNameInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property BankAccountNameField As TextBox
        Get
            Return bankAccountNameInput
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property BankAccountNumberField As TextBox
        Get
            Return bankAccountNumberInput
        End Get
    End Property

    Public Sub SetPhotoCount(count As Integer)
        photoCountLabel.Text = count & If(count = 1, " photo selected", " photos selected")
    End Sub

    Private Sub InitializePaymentInputs()
        paymentOptionLabel = CreateLabel("Payment details for rent or purchase")
        paymentOptionLabel.Location = New Point(22, 610)
        paymentOptionInput.DropDownStyle = ComboBoxStyle.DropDownList
        paymentOptionInput.Font = New Font("Segoe UI", 10)
        paymentOptionInput.Items.AddRange(New Object() {"Mobile Money", "Bank account", "Both"})
        paymentOptionInput.SelectedIndex = 0
        paymentOptionInput.Location = New Point(22, 632)
        paymentOptionInput.Size = New Size(797, 25)
        paymentFieldsPanel.Location = New Point(22, 665)
        paymentFieldsPanel.Size = New Size(797, 112)
        paymentFieldsPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        mobileMoneyNetworkInput.DropDownStyle = ComboBoxStyle.DropDownList
        mobileMoneyNetworkInput.Font = New Font("Segoe UI", 10)
        mobileMoneyNetworkInput.Items.AddRange(New Object() {"MTN MoMo", "Telecel Cash", "AT Money"})
        mobileMoneyNetworkInput.SelectedIndex = 0
        mobileMoneyNameInput.Font = New Font("Segoe UI", 10)
        mobileMoneyNumberInput.Font = New Font("Segoe UI", 10)
        bankNameInput.Font = New Font("Segoe UI", 10)
        bankAccountNameInput.Font = New Font("Segoe UI", 10)
        bankAccountNumberInput.Font = New Font("Segoe UI", 10)
        AddPaymentField(mobileMoneyPanel, "MoMo network", mobileMoneyNetworkInput, 0)
        AddPaymentField(mobileMoneyPanel, "Account name", mobileMoneyNameInput, 265)
        AddPaymentField(mobileMoneyPanel, "MoMo number", mobileMoneyNumberInput, 530)
        AddPaymentField(bankAccountPanel, "Bank name", bankNameInput, 0)
        AddPaymentField(bankAccountPanel, "Account name", bankAccountNameInput, 265)
        AddPaymentField(bankAccountPanel, "Account number", bankAccountNumberInput, 530)
        mobileMoneyPanel.Location = New Point(0, 0)
        mobileMoneyPanel.Size = New Size(797, 52)
        mobileMoneyPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        bankAccountPanel.Location = New Point(0, 58)
        bankAccountPanel.Size = New Size(797, 52)
        bankAccountPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        paymentFieldsPanel.Controls.AddRange({mobileMoneyPanel, bankAccountPanel})
        formPanel.Controls.AddRange({paymentOptionLabel, paymentOptionInput, paymentFieldsPanel})
        AddHandler paymentOptionInput.SelectedIndexChanged, AddressOf PaymentOptionChanged
    End Sub

    Private Sub AddPaymentField(parent As Control, caption As String, input As Control, x As Integer)
        Dim label = CreateLabel(caption)
        label.Location = New Point(x, 0)
        parent.Controls.Add(label)
        input.Location = New Point(x, 22)
        input.Size = New Size(250, 25)
        parent.Controls.Add(input)
    End Sub

    Private Sub UpdatePaymentFields()
        paymentOptionLabel.Visible = True
        paymentOptionInput.Visible = True
        paymentFieldsPanel.Visible = True
        Dim paymentChoice = If(paymentOptionInput.SelectedItem Is Nothing, "Mobile Money", CStr(paymentOptionInput.SelectedItem))
        mobileMoneyPanel.Visible = paymentChoice = "Mobile Money" OrElse paymentChoice = "Both"
        bankAccountPanel.Visible = paymentChoice = "Bank account" OrElse paymentChoice = "Both"
        bankAccountPanel.Top = If(mobileMoneyPanel.Visible, 58, 0)
        paymentFieldsPanel.Height = If(paymentChoice = "Both", 112, 52)
        Dim descriptionTop = paymentFieldsPanel.Bottom + 20
        descriptionLabel.Top = descriptionTop
        descriptionInput.Top = descriptionTop + 22
        photosLabel.Top = descriptionInput.Bottom + 20
        choosePhotosButton.Top = photosLabel.Bottom + 10
        photoCountLabel.Top = choosePhotosButton.Top + 13
        submitButton.Top = choosePhotosButton.Bottom + 23
        formPanel.AutoScrollMinSize = New Size(0, submitButton.Bottom + 20)
    End Sub

    Private Sub UpdatePricingFields()
        If listingModeInput.SelectedItem Is Nothing Then Return
        pricingPanel.Controls.Clear()
        rentPriceInput = Nothing
        purchasePriceInput = Nothing
        rentPeriodInput = Nothing
        Dim mode = CStr(listingModeInput.SelectedItem)
        Dim width = Math.Max(360, pricingPanel.ClientSize.Width)

        If mode = "Rent" OrElse mode = "Both" Then
            Dim rentLabel = CreateLabel("Rent price (GH₵)")
            rentLabel.Location = New Point(0, 0)
            pricingPanel.Controls.Add(rentLabel)
            rentPriceInput = CreateTextInput("Rent amount")
            rentPriceInput.Location = New Point(0, 25)
            rentPriceInput.Size = New Size(Math.Max(180, width - 190), 40)
            rentPriceInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            pricingPanel.Controls.Add(rentPriceInput)

            Dim periodLabel = CreateLabel("Rent charged")
            periodLabel.Location = New Point(width - 175, 0)
            periodLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            pricingPanel.Controls.Add(periodLabel)
            rentPeriodInput = New ComboBox With {.Location = New Point(width - 175, 25), .Size = New Size(175, 40), .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 10), .Anchor = AnchorStyles.Top Or AnchorStyles.Right}
            rentPeriodInput.Items.AddRange(New Object() {"Per month", "Per year"})
            rentPeriodInput.SelectedIndex = 0
            pricingPanel.Controls.Add(rentPeriodInput)
        End If

        If mode = "Purchase" OrElse mode = "Both" Then
            Dim purchaseY = If(mode = "Both", 78, 0)
            Dim purchaseLabel = CreateLabel("Full purchase price (GH₵)")
            purchaseLabel.Location = New Point(0, purchaseY)
            pricingPanel.Controls.Add(purchaseLabel)
            purchasePriceInput = CreateTextInput("Full purchase amount")
            purchasePriceInput.Location = New Point(0, purchaseY + 25)
            purchasePriceInput.Size = New Size(width, 40)
            purchasePriceInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            pricingPanel.Controls.Add(purchasePriceInput)
        End If
        UpdatePaymentFields()
    End Sub

    Private Function CreateLabel(caption As String) As Label
        Return New Label With {.Text = caption, .AutoSize = True, .Font = New Font("Segoe UI", 9, FontStyle.Bold), .ForeColor = RentLinkRoundedCorners.InkColor}
    End Function

    Private Function CreateTextInput(hint As String) As TextBox
        Return New TextBox With {.Font = New Font("Segoe UI", 10), .PlaceholderText = hint, .BorderStyle = BorderStyle.FixedSingle}
    End Function

    Private Sub ListingModeChanged(sender As Object, e As EventArgs) Handles listingModeInput.SelectedIndexChanged
        UpdatePricingFields()
    End Sub

    Private Sub PaymentOptionChanged(sender As Object, e As EventArgs)
        UpdatePaymentFields()
    End Sub

    Private Sub ChoosePhotosClicked(sender As Object, e As EventArgs) Handles choosePhotosButton.Click
        RaiseEvent ChoosePhotosRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub SubmitClicked(sender As Object, e As EventArgs) Handles submitButton.Click
        RaiseEvent SubmitRequested(Me, EventArgs.Empty)
    End Sub
End Class
