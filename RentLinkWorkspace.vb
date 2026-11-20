Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Globalization
Imports System.Net.Mail
Imports System.Security.Cryptography
Imports System.Text.Json
Imports System.Threading.Tasks

Public Enum RentLinkPreviewScreen
    SignIn
    Register
    RentHome
    PurchaseHome
    PropertyDetails
    ListProperty
    MyProperties
    Activity
End Enum

Public Class RentLinkWorkspace
    Inherits UserControl

    Private ReadOnly ink As Color = RentLinkRoundedCorners.InkColor
    Private ReadOnly muted As Color = RentLinkRoundedCorners.MutedColor
    Private ReadOnly accent As Color = RentLinkRoundedCorners.AccentColor
    Private ReadOnly pageBack As Color = RentLinkRoundedCorners.PageColor
    Private ReadOnly cardBack As Color = RentLinkRoundedCorners.SurfaceColor
    Private ReadOnly ownerListings As New List(Of PropertyListing)
    Private ReadOnly savedListings As New List(Of PropertyListing)
    Private ReadOnly completedRentPayments As New HashSet(Of String)(StringComparer.Ordinal)
    Private ReadOnly activityItems As New List(Of String)
    Private allListings As New List(Of PropertyListing)
    Private contentHost As Panel
    Private homeControl As RentLinkHomeControl
    Private loggedInName As String = ""
    Private loggedInEmail As String = ""
    Private activeMode As String = "Rent"
    Private selectedListing As PropertyListing
    Private isRegister As Boolean
    Private authName As TextBox
    Private authEmail As TextBox
    Private authPassword As TextBox
    Private searchBox As TextBox
    Private titleInput As TextBox
    Private locationInput As TextBox
    Private rentPriceInput As TextBox
    Private purchasePriceInput As TextBox
    Private rentPeriodInput As ComboBox
    Private pricingPanel As Panel
    Private bedsInput As TextBox
    Private typeInput As ComboBox
    Private listingModeInput As ComboBox
    Private contactInput As TextBox
    Private descriptionInput As TextBox
    Private communicationInput As ComboBox
    Private listingFormControl As RentLinkPropertyListingControl
    Private imagePaths As New List(Of String)
    Private imageCountLabel As Label
    Private previewScreenValue As RentLinkPreviewScreen = RentLinkPreviewScreen.SignIn
    <Category("RentLink Preview"), Description("Choose which RentLink screen to preview in the Visual Studio designer."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property PreviewScreen As RentLinkPreviewScreen
        Get
            Return previewScreenValue
        End Get
        Set(value As RentLinkPreviewScreen)
            previewScreenValue = value
            If DesignMode OrElse LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                ShowDesignPreview()
            End If
        End Set
    End Property

    Public Class PropertyListing
        Public Property Id As Integer
        Public Property Title As String
        Public Property Location As String
        Public Property Price As Decimal
        Public Property RentPrice As Decimal
        Public Property PurchasePrice As Decimal
        Public Property RentPeriod As String
        Public Property Bedrooms As Integer
        Public Property Bathrooms As Integer
        Public Property Kind As String
        Public Property Mode As String
        Public Property Contact As String
        Public Property Communication As String
        Public Property Description As String
        Public Property Owner As String
        Public Property OwnerEmail As String
        Public Property OwnerUid As String
        Public Property RemoteId As String
        Public Property Status As String
        Public Property Available As Boolean = True
        Public Property Views As Integer
        Public Property Enquiries As Integer
        Public Property Images As New List(Of String)
    End Class

    Public Sub New()
        DoubleBuffered = True
        Dock = DockStyle.Fill
        BackColor = pageBack
        SeedListings()
        BuildAuth()
        RentLinkRoundedCorners.ApplyTo(Me)
    End Sub

    Private Sub SeedListings()
        allListings = New List(Of PropertyListing)()
    End Sub

    Private Function RefreshSavedListings() As Boolean
        Try
            allListings = RentLinkDataStore.LoadListings()
            ownerListings.Clear()
            If Not String.IsNullOrWhiteSpace(RentLinkLocalStore.CurrentUserId) Then
                ownerListings.AddRange(allListings.Where(Function(item) String.Equals(item.OwnerUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal)))
            End If
            Return True
        Catch ex As IOException
            MessageBox.Show("RentLink could not read saved listings: " & ex.Message, "Listing storage error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As UnauthorizedAccessException
            MessageBox.Show("RentLink does not have permission to read saved listings.", "Listing storage error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As JsonException
            MessageBox.Show("RentLink's local listing data could not be read. No listing changes were made.", "Listing data error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return False
    End Function

    Private Function SaveListings(Optional additionalListing As PropertyListing = Nothing) As Boolean
        Try
            If additionalListing IsNot Nothing Then
                RentLinkDataStore.SaveListings(New PropertyListing() {additionalListing})
            Else
                RentLinkDataStore.SaveListings(allListings.Where(Function(item) Not String.IsNullOrWhiteSpace(item.OwnerEmail)))
            End If
            Return True
        Catch ex As IOException
            MessageBox.Show("RentLink could not save the listing: " & ex.Message, "Listing storage error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As UnauthorizedAccessException
            MessageBox.Show("RentLink does not have permission to save listings.", "Listing storage error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return False
    End Function

    Private Function SaveListing(listing As PropertyListing) As Boolean
        Try
            RentLinkDataStore.UpdateListing(listing)
            Dim savedListing = RentLinkDataStore.LoadListings().FirstOrDefault(Function(candidate) String.Equals(candidate.RemoteId, listing.RemoteId, StringComparison.Ordinal))
            If savedListing Is Nothing Then Throw New IOException("RentLink could not find the saved listing while verifying the update. Please refresh and try again.")
            If savedListing.Available <> listing.Available Then Throw New IOException("RentLink could not confirm the availability change. Please refresh and try again.")
            Return True
        Catch ex As IOException
            MessageBox.Show("RentLink could not confirm the listing update: " & ex.Message, "Listing storage error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As UnauthorizedAccessException
            MessageBox.Show("RentLink does not have permission to update listings.", "Listing storage error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As JsonException
            MessageBox.Show("RentLink's local listing data could not be verified. Please refresh and try again.", "Listing data error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return False
    End Function

    Private Sub BuildAuth()
        Controls.Clear()
        BackColor = pageBack
        Dim authentication As New RentLinkAuthenticationControl With {.Dock = DockStyle.Fill, .IsRegister = isRegister}
        authName = authentication.NameInput
        authEmail = authentication.EmailInput
        authPassword = authentication.PasswordInput
        Controls.Add(authentication)
        AddHandler authentication.SubmitRequested, AddressOf CompleteAuth
        AddHandler authentication.ToggleRequested, Sub()
                                                       isRegister = Not isRegister
                                                       BuildAuth()
                                                   End Sub
    End Sub

    Private Sub CompleteAuth(sender As Object, e As EventArgs)
        Dim loginIdentifier = If(authEmail Is Nothing, "", authEmail.Text.Trim())
        Dim normalizedEmail = NormalizeEmail(loginIdentifier)
        Dim password = If(authPassword Is Nothing, "", authPassword.Text)
        Dim accountName = If(authName Is Nothing, "", authName.Text.Trim())
        Dim isLocalAdminLogin = Not isRegister AndAlso String.Equals(loginIdentifier, "Admin", StringComparison.OrdinalIgnoreCase)

        If isRegister AndAlso accountName.Length = 0 Then
            ShowAuthError("Please enter your name.", authName)
            Return
        End If
        If Not isLocalAdminLogin AndAlso Not IsValidEmail(normalizedEmail) Then
            ShowAuthError("Enter a valid email address. Sign in uses your registered email.", authEmail)
            Return
        End If
        If password.Length = 0 Then
            ShowAuthError("Please enter your password.", authPassword)
            Return
        End If
        If isRegister AndAlso password.Length < 8 Then
            ShowAuthError("Choose a password with at least 8 characters.", authPassword)
            Return
        End If

        Dim authenticatedName As String = ""
        Dim authError As String = ""
        If Not RentLinkLocalStore.Authenticate(If(isLocalAdminLogin, "Admin", normalizedEmail), password, isRegister, accountName, authenticatedName, authError) Then
            ShowAuthError(authError, authPassword)
            Return
        End If

        loggedInName = authenticatedName
        loggedInEmail = RentLinkLocalStore.CurrentUserEmail
        If Not RefreshBookmarkedListings() Then Return
        TrackActivity(If(isRegister, "Registration", "Login"))
        If RentLinkLocalStore.IsAdministrator Then
            OpenAdminSide()
            Return
        End If
        BuildWorkspace()
    End Sub

    Private Sub TrackActivity(eventType As String, Optional listing As PropertyListing = Nothing)
        Try
            RentLinkDataStore.RecordActivity(eventType, loggedInEmail, loggedInName, If(listing Is Nothing, 0, listing.Id), If(listing Is Nothing, "", listing.Title), listingRemoteId:=If(listing Is Nothing, "", listing.RemoteId), listingOwnerUid:=If(listing Is Nothing, "", listing.OwnerUid))
        Catch ex As IOException
            ShowActivityStorageError(ex)
        Catch ex As UnauthorizedAccessException
            ShowActivityStorageError(ex)
        Catch ex As JsonException
            ShowActivityStorageError(ex)
        End Try
    End Sub

    Private Sub ShowActivityStorageError(ex As Exception)
        MessageBox.Show("RentLink could not save activity for the admin reports: " & ex.Message, "Activity storage error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub OpenAdminSide()
        Dim host = FindForm()
        If host Is Nothing Then
            ShowAuthError("Could not open the admin window. Please restart the application and try again.", authEmail)
            Return
        End If

        host.Hide()
        Dim adminWindow As New AdminForm()
        AddHandler adminWindow.LogoutRequested, Sub()
                                                    RentLinkLocalStore.SignOut()
                                                    loggedInName = ""
                                                    loggedInEmail = ""
                                                    isRegister = False
                                                    BuildAuth()
                                                    host.Show()
                                                    adminWindow.Close()
                                                End Sub
        AddHandler adminWindow.FormClosed, Sub()
                                               If Not host.Visible Then host.Close()
                                           End Sub
        adminWindow.Show()
    End Sub

    Private Sub ShowAuthError(message As String, field As Control)
        MessageBox.Show(message, "Check your details", MessageBoxButtons.OK, MessageBoxIcon.Information)
        If field IsNot Nothing Then
            field.Focus()
            If TypeOf field Is TextBox Then DirectCast(field, TextBox).SelectAll()
        End If
    End Sub

    Private Function NormalizeEmail(email As String) As String
        Return If(email, "").Trim().ToLowerInvariant()
    End Function

    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim address As New MailAddress(email)
            Return String.Equals(address.Address, email, StringComparison.OrdinalIgnoreCase)
        Catch ex As FormatException
            Return False
        End Try
    End Function

    Private Function ToDisplayName(email As String) As String
        Dim name = email.Split("@"c)(0).Replace(".", " ").Replace("_", " ")
        If name.Length = 0 Then Return "Home seeker"
        Return Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)
    End Function

    Private Sub BuildWorkspace()
        Controls.Clear()
        BackColor = pageBack
        homeControl = New RentLinkHomeControl With {.Dock = DockStyle.Fill}
        homeControl.SetAccountName(loggedInName)
        contentHost = homeControl.ScreenHost
        Controls.Add(homeControl)
        AddHandler homeControl.ExploreRequested, AddressOf ShowExplore
        AddHandler homeControl.ActivityRequested, AddressOf ShowActivity
        AddHandler homeControl.PropertiesRequested, AddressOf ShowMyProperties
        AddHandler homeControl.ListPropertyRequested, AddressOf ShowListingForm
        AddHandler homeControl.SignOutRequested, Sub()
                                                     RentLinkLocalStore.SignOut()
                                                     isRegister = False
                                                     loggedInName = ""
                                                     loggedInEmail = ""
                                                     BuildAuth()
                                                 End Sub
        AddHandler homeControl.ModeChanged, Sub()
                                                activeMode = homeControl.ActiveMode
                                                homeControl.SetResultsHeading(If(activeMode = "Rent", "RENTAL HOMES", "HOMES FOR PURCHASE"))
                                                RenderListings(homeControl.ResultsPanel)
                                            End Sub
        AddHandler homeControl.SearchRequested, Sub() RenderListings(homeControl.ResultsPanel)
        ShowExplore()
    End Sub

    Private Sub ShowDesignPreview()
        If previewScreenValue = RentLinkPreviewScreen.SignIn OrElse previewScreenValue = RentLinkPreviewScreen.Register Then
            isRegister = previewScreenValue = RentLinkPreviewScreen.Register
            BuildAuth()
            Return
        End If

        loggedInName = "Alex Morgan"
        loggedInEmail = "alex@example.com"
        If allListings.Count = 0 Then
            allListings.Add(New PropertyListing With {.Id = 900, .Title = "Sample home for designer preview", .Location = "Accra", .Price = 2500D, .Bedrooms = 3, .Bathrooms = 2, .Kind = "House", .Mode = "Both", .Contact = "demo@example.com", .Communication = "Email", .Description = "Design preview only; this property is not added to the live listing store.", .Owner = "Design preview", .Status = "Approved"})
        End If
        activeMode = If(previewScreenValue = RentLinkPreviewScreen.PurchaseHome, "Purchase", "Rent")
        selectedListing = If(activeMode = "Purchase", allListings.First(Function(item) item.Mode = "Purchase" OrElse item.Mode = "Both"), allListings.First(Function(item) item.Mode = "Rent" OrElse item.Mode = "Both"))

        If previewScreenValue = RentLinkPreviewScreen.MyProperties Then
            ownerListings.Clear()
            ownerListings.Add(New PropertyListing With {.Id = 900, .Title = "Your garden-side townhouse", .Location = "Parkhurst, Johannesburg", .Price = 22000D, .Bedrooms = 3, .Bathrooms = 2, .Kind = "Townhouse", .Mode = "Both", .Contact = "+27 82 555 0184", .Communication = "WhatsApp", .Description = "A bright home with a private garden.", .Owner = loggedInName, .Status = "Approved", .Views = 128, .Enquiries = 12})
        End If
        If previewScreenValue = RentLinkPreviewScreen.Activity Then
            savedListings.Clear()
            savedListings.Add(allListings(0))
            activityItems.Clear()
            activityItems.AddRange({"You enquired about Sunlit garden residence", "You viewed The quiet city apartment", "You saved Sunlit garden residence"})
        End If

        BuildWorkspace()
        Select Case previewScreenValue
            Case RentLinkPreviewScreen.PropertyDetails
                BuildListingDetails()
            Case RentLinkPreviewScreen.ListProperty
                ShowListingForm()
            Case RentLinkPreviewScreen.MyProperties
                ShowMyProperties()
            Case RentLinkPreviewScreen.Activity
                ShowActivity()
        End Select
    End Sub

    Private Function Initials(value As String) As String
        Dim parts = value.Split(" "c, StringSplitOptions.RemoveEmptyEntries)
        If parts.Length > 1 Then Return (parts(0)(0) & parts(parts.Length - 1)(0)).ToUpperInvariant()
        If value.Length > 0 Then Return value.Substring(0, 1).ToUpperInvariant()
        Return "R"
    End Function

    Private Sub AddNav(parent As Control, caption As String, y As Integer, action As EventHandler, Optional selected As Boolean = False)
        Dim nav = MakeButton("   " & caption, If(selected, RentLinkRoundedCorners.AccentSoftColor, Color.White), If(selected, accent, muted), 10, selected)
        nav.TextAlign = ContentAlignment.MiddleLeft
        nav.Location = New Point(12, y)
        nav.Size = New Size(205, 42)
        parent.Controls.Add(nav)
        AddHandler nav.Click, action
    End Sub

    Private Function NewPage() As Panel
        contentHost.Controls.Clear()
        contentHost.Visible = True
        contentHost.BringToFront()
        Dim page As New Panel With {.Dock = DockStyle.Fill, .AutoScroll = True, .BackColor = pageBack}
        contentHost.Controls.Add(page)
        Return page
    End Function

    Private Sub AddHeading(page As Control, title As String, subtitle As String)
        Dim heading = MakeLabel(title, 25, ink, True)
        heading.Location = New Point(0, 0)
        heading.AutoSize = True
        page.Controls.Add(heading)
        Dim subtitleLabel = MakeLabel(subtitle, 10, muted, False)
        subtitleLabel.Location = New Point(1, 39)
        subtitleLabel.AutoSize = True
        page.Controls.Add(subtitleLabel)
    End Sub

    Private Sub ShowExplore(sender As Object, e As EventArgs)
        ShowExplore()
    End Sub

    Private Sub ShowExplore()
        If homeControl Is Nothing Then Return
        If Not RefreshSavedListings() Then Return
        contentHost.Visible = False
        homeControl.BringToFront()
        homeControl.SetMode(activeMode)
        homeControl.SetAccountName(loggedInName)
        homeControl.SetResultsHeading(If(activeMode = "Rent", "RENTAL HOMES", "HOMES FOR PURCHASE"))
        searchBox = homeControl.ListingSearchInput
        RenderListings(homeControl.ResultsPanel)
    End Sub

    Private Sub RenderListings(flow As FlowLayoutPanel)
        If flow Is Nothing Then Return
        flow.Controls.Clear()
        Dim query = If(searchBox Is Nothing, "", searchBox.Text.Trim())
        Dim publishedListings = allListings.Where(Function(item) String.Equals(item.Status, "Approved", StringComparison.OrdinalIgnoreCase) AndAlso item.Available AndAlso (String.Equals(item.Mode, activeMode, StringComparison.OrdinalIgnoreCase) OrElse String.Equals(item.Mode, "Both", StringComparison.OrdinalIgnoreCase))).ToList()
        Dim matches = publishedListings.Where(Function(item) query = "" OrElse (item.Title & " " & item.Location & " " & item.Kind).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToList()
        If matches.Count = 0 Then
            Dim message = If(publishedListings.Count = 0, "No property listed yet.", "No homes match that search yet. Try another location or search term.")
            Dim empty = MakeLabel(message, 11, muted, False)
            empty.Size = New Size(580, 55)
            flow.Controls.Add(empty)
            Return
        End If
        For Each item In matches
            flow.Controls.Add(CreatePropertyCard(item))
        Next
    End Sub

    Private Function CreatePropertyCard(item As PropertyListing) As Panel
        Dim isPaymentListing = activeMode = "Purchase" OrElse activeMode = "Rent"
        Dim paymentMode = activeMode
        Dim card As New Panel With {.Size = New Size(315, If(isPaymentListing, 380, 335)), .BackColor = cardBack, .Margin = New Padding(0, 0, 18, 18), .BorderStyle = BorderStyle.None, .Cursor = Cursors.Hand}
        Dim imagePanel As New Panel With {.Location = New Point(0, 0), .Size = New Size(313, 165), .BackColor = Color.FromArgb(218, 234, 229), .Cursor = Cursors.Hand}
        card.Controls.Add(imagePanel)
        imagePanel.Controls.Add(CreateImagePlaceholder(item))
        If item.Images.Count > 0 Then
            LoadPropertyCardImageAsync(item.Images(0), imagePanel, Sub() OpenListing(item))
        End If
        Dim badge = MakeLabel(If(item.Mode = "Both", "RENT  ·  BUY", item.Mode.ToUpperInvariant()), 8, Color.White, True)
        badge.BackColor = accent
        badge.TextAlign = ContentAlignment.MiddleCenter
        badge.Location = New Point(12, 12)
        badge.Size = New Size(95, 25)
        imagePanel.Controls.Add(badge)
        Dim priceText = If(activeMode = "Purchase", FormatCedis(GetPurchasePrice(item)) & "  ·  FOR SALE", FormatCedis(GetRentPrice(item)) & RentPeriodSuffix(item.RentPeriod))
        Dim price = MakeLabel(priceText, 13, accent, True)
        price.Location = New Point(16, 181)
        price.AutoSize = True
        card.Controls.Add(price)
        Dim title = MakeLabel(item.Title, 14, ink, True)
        title.Location = New Point(16, 207)
        title.Size = New Size(280, 25)
        card.Controls.Add(title)
        Dim location = MakeLabel(item.Location, 9, muted, False)
        location.Location = New Point(16, 236)
        location.Size = New Size(280, 20)
        card.Controls.Add(location)
        Dim facts = MakeLabel(item.Kind & "     " & item.Bedrooms & " bedrooms", 9, muted, False)
        facts.Location = New Point(16, 270)
        facts.Size = New Size(280, 21)
        card.Controls.Add(facts)
        Dim details = MakeLabel("View home  →", 9, accent, True)
        details.Location = New Point(16, 304)
        details.AutoSize = True
        card.Controls.Add(details)
        If isPaymentListing Then
            Dim paymentButton As New Button With {.Text = If(paymentMode = "Rent", "Pay rent", "Make payment"), .Location = New Point(16, 334), .Size = New Size(280, 34), .BackColor = accent, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .UseVisualStyleBackColor = False, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
            paymentButton.FlatAppearance.BorderSize = 0
            card.Controls.Add(paymentButton)
            AddHandler paymentButton.Click, Sub(sender, args) ShowSimulatedPayment(item, paymentButton:=paymentButton)
        End If
        MakeClickable(card, Sub() OpenListing(item))
        Return card
    End Function

    Private Async Sub LoadPropertyCardImageAsync(imageReference As String, imagePanel As Panel, openAction As Action)
        Dim photoImage As Bitmap = Nothing
        Try
            photoImage = Await Task.Run(Function()
                                            Dim imagePath = RentLinkLocalStore.ResolveImagePath(imageReference)
                                            If String.IsNullOrWhiteSpace(imagePath) OrElse Not File.Exists(imagePath) Then Return Nothing
                                            Using source As Image = Image.FromFile(imagePath)
                                                Return New Bitmap(source)
                                            End Using
                                        End Function)
        Catch ex As IOException
            Return
        Catch ex As UnauthorizedAccessException
            Return
        Catch ex As JsonException
            Return
        Catch ex As FormatException
            Return
        End Try

        If photoImage Is Nothing Then Return
        If imagePanel.IsDisposed OrElse Not imagePanel.IsHandleCreated Then
            photoImage.Dispose()
            Return
        End If

        Dim photo As New PictureBox With {.Image = photoImage, .Dock = DockStyle.Fill, .SizeMode = PictureBoxSizeMode.Zoom, .Cursor = Cursors.Hand}
        AddHandler photo.Click, Sub() openAction()
        For Each control As Control In imagePanel.Controls.Cast(Of Control)().ToList()
            If Equals(control.Tag, "photo-placeholder") Then
                imagePanel.Controls.Remove(control)
                control.Dispose()
            End If
        Next
        imagePanel.Controls.Add(photo)
        photo.SendToBack()
    End Sub

    Private Function CreateImagePlaceholder(item As PropertyListing) As Control
        Dim canvas As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.FromArgb(222, 234, 229), .Tag = "photo-placeholder"}
        Dim accentBlock As New Panel With {.BackColor = If(item.Kind = "Apartment", Color.FromArgb(207, 220, 239), Color.FromArgb(232, 220, 197)), .Size = New Size(150, 88), .Location = New Point(85, 51)}
        canvas.Controls.Add(accentBlock)
        Dim building = MakeLabel(If(item.Kind = "Apartment", "CITY LIVING", "A PLACE TO CALL HOME"), 10, Color.FromArgb(45, 70, 65), True)
        building.TextAlign = ContentAlignment.MiddleCenter
        building.Dock = DockStyle.Fill
        accentBlock.Controls.Add(building)
        Return canvas
    End Function

    Private Sub MakeClickable(control As Control, action As Action)
        If TypeOf control Is Button Then Return
        AddHandler control.Click, Sub() action()
        For Each child As Control In control.Controls
            MakeClickable(child, action)
        Next
    End Sub

    Private Sub OpenListing(item As PropertyListing)
        selectedListing = item
        item.Views += 1
        TrackActivity("PropertyViewed", item)
        activityItems.Insert(0, "You viewed " & item.Title)
        BuildListingDetails()
    End Sub

    Private Sub BuildListingDetails()
        Dim page = NewPage()
        Dim detailsControl As New RentLinkPropertyDetailsControl With {.Dock = DockStyle.Fill}
        page.Controls.Add(detailsControl)
        Dim item = selectedListing
        Dim isPurchaseMode = activeMode = "Purchase"
        Dim priceText = If(isPurchaseMode, FormatCedis(GetPurchasePrice(item)) & "  ·  Full price", FormatCedis(GetRentPrice(item)) & RentPeriodSuffix(item.RentPeriod))
        detailsControl.SetListingDetails(item.Title, item.Location & "   ·   " & item.Kind, item.Bedrooms & " bedrooms     " & item.Kind & "     " & If(item.Mode = "Both", "For rent or purchase", item.Mode), item.Description, priceText, item.Owner, item.Contact, item.Communication, If(savedListings.Contains(item), "Saved this home", "Save this home"))
        Dim paymentEligible = If(isPurchaseMode, item.Mode = "Purchase" OrElse item.Mode = "Both", item.Mode = "Rent" OrElse item.Mode = "Both")
        Dim alreadyPurchased As Boolean
        Dim alreadyPaidRent As Boolean
        If isPurchaseMode AndAlso paymentEligible AndAlso Not TryHasCompletedPurchase(item, alreadyPurchased) Then paymentEligible = False
        If Not isPurchaseMode AndAlso paymentEligible AndAlso Not TryHasCompletedRentPayment(item, alreadyPaidRent) Then paymentEligible = False
        detailsControl.SetPaymentMode(paymentEligible AndAlso Not alreadyPurchased AndAlso Not alreadyPaidRent, alreadyPurchased, isPurchaseMode, alreadyPaidRent)

        If item.Images.Count = 0 Then
            For i As Integer = 0 To 3
                detailsControl.AddGalleryPlaceholder(If(i = 0, "PROPERTY OVERVIEW", "GALLERY 0" & (i + 1)), New Size(If(i = 0, 440, 145), 205), If(i Mod 2 = 0, Color.FromArgb(218, 234, 229), Color.FromArgb(231, 224, 211)))
            Next
        Else
            For Each path In item.Images
                Try
                    Dim imagePath = RentLinkLocalStore.ResolveImagePath(path)
                    If String.IsNullOrWhiteSpace(imagePath) OrElse Not File.Exists(imagePath) Then Continue For
                    Using source As Image = Image.FromFile(imagePath)
                        detailsControl.ImageGallery.Controls.Add(New PictureBox With {.Image = New Bitmap(source), .Size = New Size(250, 205), .SizeMode = PictureBoxSizeMode.Zoom, .Margin = New Padding(0, 0, 10, 0), .BackColor = Color.White})
                    End Using
                Catch ex As IOException
                    Continue For
                Catch ex As UnauthorizedAccessException
                    Continue For
                End Try
            Next
        End If

        AddHandler detailsControl.BackRequested, AddressOf ShowExplore
        AddHandler detailsControl.DirectionsRequested, Sub() Process.Start(New ProcessStartInfo("https://www.google.com/maps/search/?api=1&query=" & Uri.EscapeDataString(item.Location)) With {.UseShellExecute = True})
        AddHandler detailsControl.SaveRequested, Sub() ToggleSave(item, detailsControl)
        AddHandler detailsControl.ContactRequested, Sub() OpenPreferredContact(item)
        AddHandler detailsControl.MakePaymentRequested, Sub() ShowSimulatedPayment(item, detailsControl)
        AddHandler detailsControl.EnquireRequested, Sub()
                                                        Dim renterPhone = detailsControl.RenterPhone.Text.Trim()
                                                        Dim phoneDigits = New String(renterPhone.Where(Function(character) Char.IsDigit(character)).ToArray())
                                                        If phoneDigits.Length < 7 OrElse phoneDigits.Length > 15 Then
                                                            MessageBox.Show("Enter a valid phone number so the property lister can contact you about the viewing.", "Phone number needed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                            detailsControl.RenterPhone.Focus()
                                                            Return
                                                        End If

                                                        Dim viewingDate = detailsControl.ViewingDate.Value.Date
                                                        Try
                                                            RentLinkDataStore.RecordActivity("Enquiry", loggedInEmail, loggedInName, item.Id, item.Title, listingRemoteId:=item.RemoteId, listingOwnerUid:=item.OwnerUid, renterPhone:=renterPhone, viewingDate:=viewingDate)
                                                        Catch ex As IOException
                                                            ShowActivityStorageError(ex)
                                                            Return
                                                        End Try
                                                        item.Enquiries += 1
                                                        activityItems.Insert(0, "You requested a viewing of " & item.Title)
                                                        MessageBox.Show("Your viewing request for " & viewingDate.ToString("dddd, d MMMM") & " has been sent to the property lister. They can contact you at " & renterPhone & ".", "Request a viewing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                    End Sub
    End Sub

    Private Sub ShowSimulatedPayment(item As PropertyListing, Optional detailsControl As RentLinkPropertyDetailsControl = Nothing, Optional paymentButton As Button = Nothing)
        Dim isRentPayment = activeMode = "Rent"
        Dim isEligible = If(isRentPayment, item IsNot Nothing AndAlso (item.Mode = "Rent" OrElse item.Mode = "Both"), item IsNot Nothing AndAlso (item.Mode = "Purchase" OrElse item.Mode = "Both"))
        If item Is Nothing OrElse Not String.Equals(item.Status, "Approved", StringComparison.OrdinalIgnoreCase) OrElse Not item.Available OrElse Not isEligible Then
            MessageBox.Show("This property is not currently available for this payment.", "Payment unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If isRentPayment Then
            Dim alreadyPaidRent As Boolean
            If Not TryHasCompletedRentPayment(item, alreadyPaidRent) Then Return
            If alreadyPaidRent Then
                If detailsControl IsNot Nothing Then detailsControl.SetPaymentMode(False, isPurchase:=False, alreadyPaidRent:=True)
                If paymentButton IsNot Nothing Then
                    paymentButton.Text = "You paid your rent"
                    paymentButton.Enabled = False
                End If
                Return
            End If
        Else
            Dim alreadyPurchased As Boolean
            If Not TryHasCompletedPurchase(item, alreadyPurchased) Then Return
            If alreadyPurchased Then
                If detailsControl IsNot Nothing Then detailsControl.SetPaymentMode(False, alreadyPurchased:=True, isPurchase:=True)
                If paymentButton IsNot Nothing Then
                    paymentButton.Text = "Bought this property"
                    paymentButton.Enabled = False
                End If
                Return
            End If
        End If

        Dim paymentAmount = If(isRentPayment, GetRentPrice(item), GetPurchasePrice(item))
        If paymentAmount <= 0D Then
            MessageBox.Show("This listing does not have a valid payment amount.", "Payment unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim paymentDetails As RentLinkListingPaymentDetails
        Try
            paymentDetails = RentLinkDataStore.LoadListingPaymentDetails(item.RemoteId)
        Catch ex As IOException
            MessageBox.Show("RentLink could not load the property's payment details: " & ex.Message, "Payment details error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As UnauthorizedAccessException
            MessageBox.Show("RentLink does not have permission to read the approved payment details.", "Payment details error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As JsonException
            MessageBox.Show("The lister's payment details could not be read. No transaction was recorded.", "Payment details error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        If paymentDetails Is Nothing Then
            MessageBox.Show("The property owner has not provided approved payment details for this listing yet.", "Payment details unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim commission = Decimal.Round(paymentAmount * 0.03D, 2, MidpointRounding.AwayFromZero)
        Dim ownerBalance = paymentAmount - commission
        Dim paymentType = If(isRentPayment, "Rent", "Purchase")
        Dim amountLabel = If(isRentPayment, "Rent amount " & RentPeriodSuffix(item.RentPeriod), "Purchase amount")
        Using dialog As New Form With {.Text = "RentLink checkout", .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .ClientSize = New Size(480, 420), .MinimizeBox = False, .MaximizeBox = False}
            Dim warning = New Label With {.Text = "SIMULATED CHECKOUT · No real payment or transfer will occur.", .Location = New Point(20, 18), .Size = New Size(440, 32), .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(139, 90, 20)}
            Dim propertyName = New Label With {.Text = "Property: " & item.Title, .Location = New Point(20, 58), .Size = New Size(440, 24), .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .AutoEllipsis = True}
            Dim methodLabel = New Label With {.Text = "Payment method", .Location = New Point(20, 96), .AutoSize = True}
            Dim paymentMethod As New ComboBox With {.Location = New Point(20, 119), .Size = New Size(440, 30), .DropDownStyle = ComboBoxStyle.DropDownList}
            If paymentDetails.PaymentOption = "Mobile Money" OrElse paymentDetails.PaymentOption = "Both" Then paymentMethod.Items.Add("Mobile Money")
            If paymentDetails.PaymentOption = "Bank account" OrElse paymentDetails.PaymentOption = "Both" Then paymentMethod.Items.Add("Bank account")
            If paymentMethod.Items.Count = 0 Then
                MessageBox.Show("The property owner has not provided a valid payment method.", "Payment details unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            paymentMethod.SelectedIndex = 0
            Dim accountDetails = New Label With {.Location = New Point(20, 160), .Size = New Size(440, 70), .Font = New Font("Segoe UI", 9.5F), .ForeColor = ink}
            AddHandler paymentMethod.SelectedIndexChanged, Sub()
                                                               accountDetails.Text = If(CStr(paymentMethod.SelectedItem) = "Mobile Money",
                                                                   "Mobile Money · " & paymentDetails.MobileMoneyNetwork & Environment.NewLine & "Account name: " & paymentDetails.MobileMoneyAccountName & " · Number: " & paymentDetails.MobileMoneyNumber,
                                                                   "Bank account · " & paymentDetails.BankName & Environment.NewLine & "Account name: " & paymentDetails.BankAccountName & " · Number: " & paymentDetails.BankAccountNumber)
                                                           End Sub
            accountDetails.Text = If(CStr(paymentMethod.SelectedItem) = "Mobile Money",
                "Mobile Money · " & paymentDetails.MobileMoneyNetwork & Environment.NewLine & "Account name: " & paymentDetails.MobileMoneyAccountName & " · Number: " & paymentDetails.MobileMoneyNumber,
                "Bank account · " & paymentDetails.BankName & Environment.NewLine & "Account name: " & paymentDetails.BankAccountName & " · Number: " & paymentDetails.BankAccountNumber)
            Dim amountDetails = New Label With {.Text = amountLabel & ": " & FormatCedis(paymentAmount) & Environment.NewLine & "Platform commission (3%): " & FormatCedis(commission) & Environment.NewLine & "Owner balance: " & FormatCedis(ownerBalance), .Location = New Point(20, 240), .Size = New Size(440, 90), .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)}
            Dim note = New Label With {.Text = "Proceed records a simulated transaction only. It does not debit a wallet or transfer funds.", .Location = New Point(20, 334), .Size = New Size(440, 42), .Font = New Font("Segoe UI", 9.0F), .ForeColor = muted}
            Dim proceed As New Button With {.Text = "Proceed", .DialogResult = DialogResult.OK, .Location = New Point(350, 380), .Size = New Size(110, 32), .BackColor = accent, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .UseVisualStyleBackColor = False}
            proceed.FlatAppearance.BorderSize = 0
            dialog.Controls.AddRange({warning, propertyName, methodLabel, paymentMethod, accountDetails, amountDetails, note, proceed})
            dialog.AcceptButton = proceed
            RentLinkRoundedCorners.ApplyTo(dialog)
            If dialog.ShowDialog(Me) <> DialogResult.OK Then Return

            Dim transactionId = Guid.NewGuid().ToString("N")
            Dim paymentAccount = CStr(paymentMethod.SelectedItem)
            Dim transactionNotes = "SIMULATED|" & transactionId & "|" & paymentAccount & "|Commission: " & FormatCedis(commission) & "|Owner balance: " & FormatCedis(ownerBalance) & "|No real funds transferred"
            Try
                If isRentPayment Then
                    RentLinkDataStore.RecordActivity("SimulatedRentPaymentCompleted", loggedInEmail, loggedInName, item.Id, item.Title, paymentAmount, transactionNotes, item.RemoteId, item.OwnerUid)
                Else
                    RentLinkDataStore.RecordPurchase(loggedInEmail, loggedInName, item.Id, item.Title, paymentAmount, transactionNotes, item.RemoteId, item.OwnerUid)
                End If
            Catch ex As InvalidOperationException
                MessageBox.Show(ex.Message, "Property already purchased", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            Catch ex As IOException
                MessageBox.Show("RentLink could not record this simulated payment: " & ex.Message, "Payment error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            Catch ex As UnauthorizedAccessException
                MessageBox.Show("RentLink does not have permission to record this simulated transaction.", "Payment error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            Catch ex As JsonException
                MessageBox.Show("RentLink's local activity data could not be read. No transaction was recorded.", "Payment error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            If isRentPayment Then completedRentPayments.Add(item.RemoteId)
            If detailsControl IsNot Nothing Then detailsControl.SetPaymentMode(isRentPayment, Not isRentPayment, Not isRentPayment, isRentPayment)
            If paymentButton IsNot Nothing Then
                paymentButton.Text = If(isRentPayment, "You paid your rent", "Bought this property")
                paymentButton.Enabled = isRentPayment
            End If
            MessageBox.Show("Simulated " & paymentType.ToLowerInvariant() & " payment recorded for " & item.Title & "." & Environment.NewLine &
                            amountLabel & ": " & FormatCedis(paymentAmount) & Environment.NewLine &
                            "3% commission: " & FormatCedis(commission) & Environment.NewLine &
                            "Owner balance: " & FormatCedis(ownerBalance) & Environment.NewLine & Environment.NewLine &
                            "The payment details belong to the property owner. RentLink records this as a simulation only; no actual money moved.", "Simulated payment complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Using
    End Sub

    Private Function TryHasCompletedPurchase(item As PropertyListing, ByRef alreadyPurchased As Boolean) As Boolean
        alreadyPurchased = False
        Try
            alreadyPurchased = RentLinkDataStore.HasCompletedPurchase(item.RemoteId)
            Return True
        Catch ex As IOException
            ShowActivityStorageError(ex)
        Catch ex As UnauthorizedAccessException
            ShowActivityStorageError(ex)
        Catch ex As JsonException
            ShowActivityStorageError(ex)
        End Try
        Return False
    End Function

    Private Function TryHasCompletedRentPayment(item As PropertyListing, ByRef alreadyPaid As Boolean) As Boolean
        alreadyPaid = completedRentPayments.Contains(item.RemoteId)
        Return True
    End Function

    Private Sub OpenPreferredContact(item As PropertyListing)
        Dim target As String
        Select Case If(item.Communication, "").Trim().ToUpperInvariant()
            Case "WHATSAPP"
                Dim contact = If(item.Contact, "")
                Dim phoneDigits = New String(contact.Where(Function(character) Char.IsDigit(character)).ToArray())
                If phoneDigits.Length < 7 OrElse phoneDigits.Length > 15 OrElse Not contact.TrimStart().StartsWith("+", StringComparison.Ordinal) Then
                    MessageBox.Show("This lister's WhatsApp number is invalid. Ask them to update it using the full international format, such as +233... .", "Contact details unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
                target = "https://wa.me/" & phoneDigits
            Case "PHONE"
                Dim contact = If(item.Contact, "")
                Dim phoneNumber = New String(contact.Where(Function(character) Char.IsDigit(character) OrElse character = "+"c).ToArray())
                Dim phoneDigits = New String(phoneNumber.Where(Function(character) Char.IsDigit(character)).ToArray())
                If phoneDigits.Length < 7 OrElse phoneDigits.Length > 15 OrElse phoneNumber.Count(Function(character) character = "+"c) > 1 OrElse (phoneNumber.Contains("+"c) AndAlso Not phoneNumber.StartsWith("+", StringComparison.Ordinal)) Then
                    MessageBox.Show("This lister's phone number is invalid. Please contact RentLink support.", "Contact details unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
                target = "tel:" & phoneNumber
            Case "EMAIL"
                Try
                    Dim address As New MailAddress(If(item.Contact, "").Trim())
                    target = "mailto:" & Uri.EscapeDataString(address.Address)
                Catch ex As FormatException
                    MessageBox.Show("This lister's email address is invalid. Please contact RentLink support.", "Contact details unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End Try
            Case Else
                MessageBox.Show("This lister has not selected a supported contact method.", "Contact details unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
        End Select

        Try
            Process.Start(New ProcessStartInfo(target) With {.UseShellExecute = True})
        Catch ex As System.ComponentModel.Win32Exception
            MessageBox.Show("Windows could not open the lister's preferred contact app. Check that a suitable phone, WhatsApp, or email app is installed and configured.", "Could not open contact app", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As InvalidOperationException
            MessageBox.Show("Windows could not open the lister's preferred contact app. Check that a suitable phone, WhatsApp, or email app is installed and configured.", "Could not open contact app", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ToggleSave(item As PropertyListing, detailsControl As RentLinkPropertyDetailsControl)
        If savedListings.Contains(item) Then
            savedListings.Remove(item)
            TrackActivity("BookmarkRemoved", item)
            activityItems.Insert(0, "You removed " & item.Title & " from saved homes")
            detailsControl.SetSaveText("Save this home")
        Else
            savedListings.Add(item)
            TrackActivity("BookmarkAdded", item)
            activityItems.Insert(0, "You saved " & item.Title)
            detailsControl.SetSaveText("Saved this home")
        End If
    End Sub

    Private Sub ShowActivity(sender As Object, e As EventArgs)
        ShowActivity()
    End Sub

    Private Function RefreshBookmarkedListings(Optional activityEvents As List(Of RentLinkActivityEvent) = Nothing) As Boolean
        Try
            If activityEvents Is Nothing Then activityEvents = RentLinkDataStore.LoadActivity()
            Dim userEvents = activityEvents.Where(Function(item) String.Equals(item.UserUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal) OrElse String.Equals(item.UserEmail, loggedInEmail, StringComparison.OrdinalIgnoreCase)).OrderBy(Function(item) item.Timestamp).ToList()
            savedListings.Clear()
            completedRentPayments.Clear()
            For Each item In userEvents
                If item.EventType = "SimulatedRentPaymentCompleted" AndAlso Not String.IsNullOrWhiteSpace(item.ListingRemoteId) Then
                    completedRentPayments.Add(item.ListingRemoteId)
                    Continue For
                End If
                If item.EventType <> "BookmarkAdded" AndAlso item.EventType <> "BookmarkRemoved" Then Continue For
                Dim listing = allListings.FirstOrDefault(Function(candidate) Not String.IsNullOrWhiteSpace(item.ListingRemoteId) AndAlso String.Equals(candidate.RemoteId, item.ListingRemoteId, StringComparison.Ordinal))
                If listing Is Nothing Then listing = allListings.FirstOrDefault(Function(candidate) candidate.Id = item.ListingId AndAlso String.Equals(candidate.Title, item.ListingTitle, StringComparison.OrdinalIgnoreCase))
                If listing Is Nothing Then Continue For
                If item.EventType = "BookmarkAdded" Then
                    If Not savedListings.Contains(listing) Then savedListings.Add(listing)
                Else
                    savedListings.Remove(listing)
                End If
            Next
            Return True
        Catch ex As IOException
            ShowActivityStorageError(ex)
        Catch ex As UnauthorizedAccessException
            ShowActivityStorageError(ex)
        Catch ex As JsonException
            ShowActivityStorageError(ex)
        End Try
        Return False
    End Function

    Private Sub ShowActivity()
        Dim allEvents As List(Of RentLinkActivityEvent)
        Try
            allEvents = RentLinkDataStore.LoadActivity()
            Dim userEvents = allEvents.Where(Function(item) String.Equals(item.UserUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal) OrElse String.Equals(item.UserEmail, loggedInEmail, StringComparison.OrdinalIgnoreCase)).OrderBy(Function(item) item.Timestamp).ToList()
            Dim ownerEvents = allEvents.Where(Function(item) String.Equals(item.ListingOwnerUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal) AndAlso Not String.Equals(item.UserUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal)).ToList()
            If Not RefreshBookmarkedListings(allEvents) Then Return
            activityItems.Clear()
            Dim visibleEvents = userEvents.Concat(ownerEvents).
                Where(Function(activity) Not String.Equals(activity.EventType, "Login", StringComparison.OrdinalIgnoreCase) AndAlso Not String.Equals(activity.EventType, "Registration", StringComparison.OrdinalIgnoreCase)).
                GroupBy(Function(activity) String.Join("|", activity.UserUid, activity.EventType, activity.ListingRemoteId, activity.Timestamp.Ticks.ToString(Globalization.CultureInfo.InvariantCulture))).
                Select(Function(group) group.First())
            For Each item In visibleEvents.OrderByDescending(Function(activity) activity.Timestamp).Take(100)
                activityItems.Add(DescribeActivity(item))
            Next
        Catch ex As IOException
            ShowActivityStorageError(ex)
        Catch ex As UnauthorizedAccessException
            ShowActivityStorageError(ex)
        Catch ex As JsonException
            ShowActivityStorageError(ex)
        End Try

        Dim page = NewPage()
        Dim activityControl As New RentLinkActivityControl With {.Dock = DockStyle.Fill}
        page.Controls.Add(activityControl)
        activityControl.ClearSavedHomes()
        If savedListings.Count = 0 Then
            activityControl.ShowEmptySavedHomes()
        Else
            For Each item In savedListings.ToList()
                activityControl.AddSavedHome(item.Title & "  ·  " & item.Location, Sub() OpenListing(item))
            Next
        End If
        activityControl.SetRecentActivities(activityItems)
    End Sub

    Private Function DescribeActivity(item As RentLinkActivityEvent) As String
        Dim action = item.EventType
        Select Case item.EventType
            Case "Login"
                action = "Signed in"
            Case "Registration"
                action = "Created an account"
            Case "PropertyViewed"
                action = "Viewed a property"
            Case "BookmarkAdded"
                action = "Saved a property"
            Case "BookmarkRemoved"
                action = "Removed a saved property"
            Case "Enquiry"
                action = If(String.Equals(item.ListingOwnerUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal), "New viewing request from " & item.UserName, "You requested a viewing")
            Case "ListingSubmitted"
                action = "Submitted a property for review"
            Case "ListingApproved"
                action = "Property approved"
            Case "ListingRejected"
                action = "Property not approved"
            Case "SimulatedPurchaseCompleted"
                action = If(String.Equals(item.ListingOwnerUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal) AndAlso Not String.Equals(item.UserUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal), "Simulated purchase payment received from " & item.UserName, "Completed a simulated purchase payment")
            Case "SimulatedRentPaymentCompleted"
                action = If(String.Equals(item.ListingOwnerUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal) AndAlso Not String.Equals(item.UserUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal), "Simulated rent payment received from " & item.UserName, "Completed a simulated rent payment")
        End Select

        Dim listingDetail = If(String.IsNullOrWhiteSpace(item.ListingTitle), "", " · " & item.ListingTitle)
        Dim enquiryDetails = ""
        If String.Equals(item.EventType, "Enquiry", StringComparison.OrdinalIgnoreCase) Then
            If item.ViewingDate.HasValue Then enquiryDetails &= " · Viewing: " & item.ViewingDate.Value.ToString("dddd, d MMM yyyy")
            If String.Equals(item.ListingOwnerUid, RentLinkLocalStore.CurrentUserId, StringComparison.Ordinal) AndAlso Not String.IsNullOrWhiteSpace(item.RenterPhone) Then
                enquiryDetails &= " · Phone: " & item.RenterPhone.Trim()
            End If
        ElseIf String.Equals(item.EventType, "SimulatedPurchaseCompleted", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(item.EventType, "SimulatedRentPaymentCompleted", StringComparison.OrdinalIgnoreCase) Then
            Dim details = If(item.Notes, "").Split("|"c)
            Dim paymentAccount = If(details.Length > 2, details(2), "Payment account")
            Dim commission = Decimal.Round(item.Amount * 0.03D, 2, MidpointRounding.AwayFromZero)
            Dim paymentType = If(item.EventType = "SimulatedRentPaymentCompleted", "rent", "sale")
            enquiryDetails = " · SIMULATED: " & FormatCedis(item.Amount) & " " & paymentType & " · 3% commission " & FormatCedis(commission) & " · owner balance " & FormatCedis(item.Amount - commission) & " · " & paymentAccount & " · no actual funds transferred"
        End If
        Return item.Timestamp.ToString("dd MMM yyyy, h:mm tt") & " · " & action & listingDetail & enquiryDetails
    End Function

    Private Sub ShowMyProperties(sender As Object, e As EventArgs)
        ShowMyProperties()
    End Sub

    Private Sub ShowMyProperties()
        If Not RefreshSavedListings() Then Return
        Dim activityEvents As List(Of RentLinkActivityEvent)
        Try
            activityEvents = RentLinkDataStore.LoadActivity()
        Catch ex As IOException
            MessageBox.Show("RentLink could not read local activity: " & ex.Message, "Local data error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As UnauthorizedAccessException
            MessageBox.Show("RentLink does not have permission to read local activity.", "Local data error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Catch ex As JsonException
            MessageBox.Show("RentLink's local activity data could not be read. No activity changes were made.", "Local data error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim page = NewPage()
        Dim propertiesControl As New RentLinkMyPropertiesControl With {.Dock = DockStyle.Fill}
        page.Controls.Add(propertiesControl)
        AddHandler propertiesControl.AddPropertyRequested, AddressOf ShowListingForm
        AddHandler propertiesControl.AvailabilityChanged, Sub(sender, args)
                                                              Dim previousValue = args.Listing.Available
                                                              If previousValue = args.Available Then Return

                                                              args.Listing.Available = args.Available
                                                              If SaveListing(args.Listing) Then
                                                                  propertiesControl.SetAvailability(args.Listing, args.Available)
                                                              Else
                                                                  args.Listing.Available = previousValue
                                                                  propertiesControl.SetAvailability(args.Listing, previousValue)
                                                              End If
                                                          End Sub
        propertiesControl.ClearListings()
        If ownerListings.Count = 0 Then
            propertiesControl.ShowEmptyState()
            Return
        End If

        For Each item In ownerListings.ToList()
            Dim listingEvents = activityEvents.Where(Function(activity) (Not String.IsNullOrWhiteSpace(activity.ListingRemoteId) AndAlso String.Equals(activity.ListingRemoteId, item.RemoteId, StringComparison.Ordinal)) OrElse (activity.ListingId = item.Id AndAlso String.Equals(activity.ListingTitle, item.Title, StringComparison.OrdinalIgnoreCase))).ToList()
            Dim bookmarkCount = listingEvents.Where(Function(activity) activity.EventType = "BookmarkAdded" OrElse activity.EventType = "BookmarkRemoved").GroupBy(Function(activity) If(activity.UserEmail, "").Trim(), StringComparer.OrdinalIgnoreCase).Count(Function(userEvents) userEvents.OrderByDescending(Function(activity) activity.Timestamp).First().EventType = "BookmarkAdded")
            Dim viewCount = listingEvents.Where(Function(activity) activity.EventType = "PropertyViewed").Count()
            Dim enquiryCount = listingEvents.Where(Function(activity) activity.EventType = "Enquiry").Count()
            Dim metrics = viewCount & " views     " & enquiryCount & " enquiries     " & bookmarkCount & If(bookmarkCount = 1, " bookmark", " bookmarks")
            Dim visibilityNote = If(item.Status = "Approved", "Visible in the " & If(item.Mode = "Both", "Rent and Purchase", item.Mode) & " section", If(item.Status = "Rejected", "Rejected by admin. Contact support before resubmitting.", "Your listing will appear publicly after admin approval."))
            propertiesControl.AddPropertyCard(item, metrics, visibilityNote)
        Next
    End Sub

    Private Sub ShowListingForm(sender As Object, e As EventArgs)
        ShowListingForm()
    End Sub

    Private Sub ShowListingForm()
        imagePaths = New List(Of String)
        Dim page = NewPage()
        listingFormControl = New RentLinkPropertyListingControl With {.Dock = DockStyle.Fill}
        page.Controls.Add(listingFormControl)
        titleInput = listingFormControl.PropertyHeadlineInput
        locationInput = listingFormControl.PropertyLocationInput
        typeInput = listingFormControl.PropertyTypeChoice
        listingModeInput = listingFormControl.ListingModeChoice
        bedsInput = listingFormControl.BedroomCountField
        contactInput = listingFormControl.ContactDetailsInput
        communicationInput = listingFormControl.CommunicationChoice
        descriptionInput = listingFormControl.PropertyDescriptionInput
        rentPriceInput = listingFormControl.RentAmountInput
        purchasePriceInput = listingFormControl.PurchaseAmountInput
        rentPeriodInput = listingFormControl.RentPeriodChoice
        AddHandler listingFormControl.ChoosePhotosRequested, AddressOf ChoosePhotos
        AddHandler listingFormControl.SubmitRequested, AddressOf SubmitListing
    End Sub

    Private Sub ShowListingError(message As String, field As Control)
        MessageBox.Show(message, "Check this field", MessageBoxButtons.OK, MessageBoxIcon.Information)
        field.Focus()
        If TypeOf field Is TextBox Then DirectCast(field, TextBox).SelectAll()
    End Sub

    Private Function TryParseCedis(value As String, ByRef amount As Decimal) As Boolean
        Dim normalized = value.Trim().Replace("GH₵", "", StringComparison.OrdinalIgnoreCase).Replace("GHS", "", StringComparison.OrdinalIgnoreCase).Trim()
        Return Decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.GetCultureInfo("en-GH"), amount)
    End Function



    Private Sub ChoosePhotos(sender As Object, e As EventArgs)
        Using dialog As New OpenFileDialog With {.Title = "Choose property photos", .Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.gif", .Multiselect = True}
            If dialog.ShowDialog() = DialogResult.OK Then
                For Each selectedPath In dialog.FileNames
                    If Not imagePaths.Contains(selectedPath, StringComparer.OrdinalIgnoreCase) Then
                        imagePaths.Add(selectedPath)
                    End If
                Next
                listingFormControl.SetPhotoCount(imagePaths.Count)
            End If
        End Using
    End Sub

    Private Sub SubmitListing(sender As Object, e As EventArgs)
        Dim rentAmount As Decimal
        Dim purchaseAmount As Decimal
        Dim beds As Integer
        rentPriceInput = listingFormControl.RentAmountInput
        purchasePriceInput = listingFormControl.PurchaseAmountInput
        rentPeriodInput = listingFormControl.RentPeriodChoice
        Dim mode = CStr(listingModeInput.SelectedItem)
        If String.IsNullOrWhiteSpace(titleInput.Text) Then
            ShowListingError("Please enter a property headline.", titleInput)
            Return
        End If
        If String.IsNullOrWhiteSpace(locationInput.Text) Then
            ShowListingError("Please enter the property location.", locationInput)
            Return
        End If
        If (mode = "Rent" OrElse mode = "Both") AndAlso (Not TryParseCedis(rentPriceInput.Text, rentAmount) OrElse rentAmount <= 0D) Then
            ShowListingError("Enter a valid rent price in Ghana cedis, using numbers only (for example, 2500 or 2,500.50).", rentPriceInput)
            Return
        End If
        If (mode = "Purchase" OrElse mode = "Both") AndAlso (Not TryParseCedis(purchasePriceInput.Text, purchaseAmount) OrElse purchaseAmount <= 0D) Then
            ShowListingError("Enter a valid full purchase price in Ghana cedis, using numbers only (for example, 2500 or 2,500.50).", purchasePriceInput)
            Return
        End If
        If Not Integer.TryParse(bedsInput.Text.Trim(), beds) OrElse beds < 0 Then
            ShowListingError("Enter a valid bedroom count (zero or more).", bedsInput)
            Return
        End If
        If String.IsNullOrWhiteSpace(contactInput.Text) Then
            ShowListingError("Please enter the contact details people should use to reach you.", contactInput)
            Return
        End If
        If String.IsNullOrWhiteSpace(descriptionInput.Text) Then
            ShowListingError("Please add a description of the property.", descriptionInput)
            Return
        End If

        Dim paymentDetails As RentLinkListingPaymentDetails = Nothing
        Dim paymentOption = CStr(listingFormControl.PaymentOptionChoice.SelectedItem)
        If paymentOption = "Mobile Money" OrElse paymentOption = "Both" Then
            Dim mobileNumber = listingFormControl.MobileMoneyNumberField.Text.Trim()
            Dim mobileDigits = New String(mobileNumber.Where(Function(character) Char.IsDigit(character)).ToArray())
            If listingFormControl.MobileMoneyNetworkChoice.SelectedItem Is Nothing OrElse String.IsNullOrWhiteSpace(listingFormControl.MobileMoneyAccountNameInput.Text) OrElse mobileDigits.Length < 7 OrElse mobileDigits.Length > 15 Then
                ShowListingError("Enter the mobile money network, account name, and a valid phone number.", listingFormControl.MobileMoneyNumberField)
                Return
            End If
        End If
        If paymentOption = "Bank account" OrElse paymentOption = "Both" Then
            Dim accountNumber = listingFormControl.BankAccountNumberField.Text.Trim()
            Dim accountDigits = New String(accountNumber.Where(Function(character) Char.IsDigit(character)).ToArray())
            If String.IsNullOrWhiteSpace(listingFormControl.BankNameField.Text) OrElse String.IsNullOrWhiteSpace(listingFormControl.BankAccountNameField.Text) OrElse accountDigits.Length < 5 OrElse accountDigits.Length > 24 Then
                ShowListingError("Enter the bank name, account name, and a valid account number.", listingFormControl.BankAccountNumberField)
                Return
            End If
        End If
        paymentDetails = New RentLinkListingPaymentDetails With {
            .PaymentOption = paymentOption,
            .MobileMoneyNetwork = If(listingFormControl.MobileMoneyNetworkChoice.SelectedItem Is Nothing, "", CStr(listingFormControl.MobileMoneyNetworkChoice.SelectedItem)),
            .MobileMoneyAccountName = listingFormControl.MobileMoneyAccountNameInput.Text.Trim(),
            .MobileMoneyNumber = listingFormControl.MobileMoneyNumberField.Text.Trim(),
            .BankName = listingFormControl.BankNameField.Text.Trim(),
            .BankAccountName = listingFormControl.BankAccountNameField.Text.Trim(),
            .BankAccountNumber = listingFormControl.BankAccountNumberField.Text.Trim()
        }
        If imagePaths.Count > 5 Then
            MessageBox.Show("Choose no more than 5 photos for one property.", "Too many photos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim legacyPrice = If(mode = "Purchase", purchaseAmount, rentAmount)
        Dim item As New PropertyListing With {.Id = If(allListings.Count = 0, 1, allListings.Max(Function(existing) existing.Id) + 1), .Title = titleInput.Text.Trim(), .Location = locationInput.Text.Trim(), .Price = legacyPrice, .RentPrice = rentAmount, .PurchasePrice = purchaseAmount, .RentPeriod = If(rentPeriodInput Is Nothing, "Per month", CStr(rentPeriodInput.SelectedItem)), .Bedrooms = beds, .Kind = CStr(typeInput.SelectedItem), .Mode = mode, .Contact = contactInput.Text.Trim(), .Communication = CStr(communicationInput.SelectedItem), .Description = descriptionInput.Text.Trim(), .Owner = loggedInName, .OwnerEmail = loggedInEmail, .OwnerUid = RentLinkLocalStore.CurrentUserId, .RemoteId = Guid.NewGuid().ToString("N"), .Status = "Pending review", .Images = imagePaths.ToList()}
        If paymentDetails IsNot Nothing Then
            paymentDetails.ListingRemoteId = item.RemoteId
            paymentDetails.OwnerUid = item.OwnerUid
        End If
        If Not SaveListings(item) Then Return
        If paymentDetails IsNot Nothing Then
            Try
                paymentDetails.PaymentOption = CStr(listingFormControl.PaymentOptionChoice.SelectedItem)
                paymentDetails.MobileMoneyNetwork = CStr(listingFormControl.MobileMoneyNetworkChoice.SelectedItem)
                paymentDetails.MobileMoneyAccountName = listingFormControl.MobileMoneyAccountNameInput.Text.Trim()
                paymentDetails.MobileMoneyNumber = listingFormControl.MobileMoneyNumberField.Text.Trim()
                paymentDetails.BankName = listingFormControl.BankNameField.Text.Trim()
                paymentDetails.BankAccountName = listingFormControl.BankAccountNameField.Text.Trim()
                paymentDetails.BankAccountNumber = listingFormControl.BankAccountNumberField.Text.Trim()
                RentLinkDataStore.SaveListingPaymentDetails(paymentDetails)
            Catch ex As IOException
                MessageBox.Show("The property was saved, but its payment details could not be saved for admin review. " & ex.Message, "Payment details error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            Catch ex As UnauthorizedAccessException
                MessageBox.Show("The property was saved, but RentLink does not have permission to save its payment details. Publish the updated Firestore rules and try again.", "Payment details error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If
        ownerListings.Add(item)
        allListings.Add(item)
        TrackActivity("ListingSubmitted", item)
        activityItems.Insert(0, "You submitted " & item.Title & " for admin review")
        MessageBox.Show("Your property has been submitted. It will appear on the appropriate home tab after administrator approval.", "Listing submitted", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ShowMyProperties()
    End Sub

    Private Function MakeLabel(text As String, size As Single, color As Color, bold As Boolean) As Label
        Return New Label With {.Text = text, .Font = New Font("Segoe UI", size, If(bold, FontStyle.Bold, FontStyle.Regular)), .ForeColor = color, .BackColor = Color.Transparent, .AutoEllipsis = True}
    End Function

    Private Function FormatCedis(amount As Decimal) As String
        Return "GH₵ " & amount.ToString("N2", CultureInfo.GetCultureInfo("en-GH"))
    End Function

    Private Function GetRentPrice(item As PropertyListing) As Decimal
        If item.RentPrice > 0D Then Return item.RentPrice
        Return If(item.Mode = "Rent" OrElse item.Mode = "Both", item.Price, 0D)
    End Function

    Private Function GetPurchasePrice(item As PropertyListing) As Decimal
        If item.PurchasePrice > 0D Then Return item.PurchasePrice
        Return If(item.Mode = "Purchase" OrElse item.Mode = "Both", item.Price, 0D)
    End Function

    Private Function RentPeriodSuffix(period As String) As String
        Return If(String.Equals(period, "Per year", StringComparison.OrdinalIgnoreCase), " / year", " / month")
    End Function

    Private Sub InitializeComponent()

    End Sub

    Private Function MakeButton(text As String, back As Color, fore As Color, size As Single, bold As Boolean) As Button
        Dim button As New Button With {.Text = text, .Font = New Font("Segoe UI", size, If(bold, FontStyle.Bold, FontStyle.Regular)), .BackColor = back, .ForeColor = fore, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .UseVisualStyleBackColor = False}
        button.FlatAppearance.BorderSize = 0
        Return button
    End Function
End Class
