Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms

Partial Public Class AdminForm
    Inherits Form

    Public Event LogoutRequested As EventHandler

    Private ReadOnly ink As Color = RentLinkRoundedCorners.InkColor
    Private ReadOnly muted As Color = RentLinkRoundedCorners.MutedColor
    Private ReadOnly accent As Color = RentLinkRoundedCorners.AccentColor
    Private ReadOnly pageBack As Color = RentLinkRoundedCorners.PageColor
    Private events As List(Of RentLinkActivityEvent)
    Private listings As List(Of RentLinkWorkspace.PropertyListing)

    Public Sub New()
        InitializeComponent()
        AddHandler overviewButton.Click, AddressOf ShowOverview
        AddHandler propertyReviewButton.Click, AddressOf ShowListings
        AddHandler transactionsButton.Click, AddressOf ShowTransactions
        AddHandler logoutButton.Click, AddressOf LogOut
        ShowOverview()
        RentLinkRoundedCorners.ApplyTo(Me)
    End Sub

    Private Sub LogOut(sender As Object, e As EventArgs)
        RaiseEvent LogoutRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub ShowOverview(sender As Object, e As EventArgs)
        ShowOverview()
    End Sub

    Private Sub ShowOverview()
        If Not LoadData() Then Return
        Dim page = NewPage()
        AddHeading(page, "Platform overview", "Account activity, listing decisions, and recorded purchase commissions.")

        Dim period = New ComboBox With {.Location = New Point(0, 86), .Size = New Size(180, 38), .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 10)}
        period.Items.AddRange(New Object() {"Today", "This week", "This month", "This year"})
        period.SelectedIndex = 0
        page.Controls.Add(period)
        Dim dashboard As New Panel With {.Location = New Point(0, 137), .Size = New Size(Math.Max(650, page.ClientSize.Width - 55), 670), .BackColor = pageBack}
        page.Controls.Add(dashboard)
        AddHandler period.SelectedIndexChanged, Sub() RenderOverview(dashboard, CStr(period.SelectedItem))
        RenderOverview(dashboard, CStr(period.SelectedItem))
    End Sub

    Private Sub RenderOverview(parent As Control, selectedPeriod As String)
        parent.Controls.Clear()
        Dim current = DateTime.Now
        Dim currentStart As DateTime
        Dim previousStart As DateTime
        If selectedPeriod = "Today" Then
            currentStart = current.Date
            previousStart = currentStart.AddDays(-1)
        ElseIf selectedPeriod = "This week" Then
            currentStart = current.Date.AddDays(-CInt((CInt(current.DayOfWeek) + 6) Mod 7))
            previousStart = currentStart.AddDays(-7)
        ElseIf selectedPeriod = "This month" Then
            currentStart = New DateTime(current.Year, current.Month, 1)
            previousStart = currentStart.AddMonths(-1)
        Else
            currentStart = New DateTime(current.Year, 1, 1)
            previousStart = currentStart.AddYears(-1)
        End If

        Dim previousEnd = previousStart.Add(current - currentStart)
        Dim currentEvents = events.Where(Function(item) item.Timestamp >= currentStart AndAlso item.Timestamp <= current).ToList()
        Dim previousEvents = events.Where(Function(item) item.Timestamp >= previousStart AndAlso item.Timestamp < previousEnd).ToList()
        Dim loginEvents = currentEvents.Where(Function(item) item.EventType = "Login" AndAlso Not IsAdministratorLogin(item)).ToList()
        Dim previousLoginEvents = previousEvents.Where(Function(item) item.EventType = "Login" AndAlso Not IsAdministratorLogin(item)).ToList()
        Dim y = 0
        AddMetric(parent, "Login events", loginEvents.Count, Difference(loginEvents.Count, previousLoginEvents.Count), 0, y) : y += 118
        AddMetric(parent, "Unique users logged in", UniqueUsers(loginEvents), Difference(UniqueUsers(loginEvents), UniqueUsers(previousLoginEvents)), 0, y) : y += 118
        AddMetric(parent, "Properties submitted", CountEvents(currentEvents, "ListingSubmitted"), Difference(CountEvents(currentEvents, "ListingSubmitted"), CountEvents(previousEvents, "ListingSubmitted")), 0, y) : y += 118
        Dim listers = UniqueUsers(currentEvents.Where(Function(item) item.EventType = "ListingSubmitted"))
        Dim previousListers = UniqueUsers(previousEvents.Where(Function(item) item.EventType = "ListingSubmitted"))
        AddMetric(parent, "Unique property listers", listers, Difference(listers, previousListers), 0, y) : y += 118
        AddMetric(parent, "Approved / rejected", CountEvents(currentEvents, "ListingApproved") & " / " & CountEvents(currentEvents, "ListingRejected"), Difference(CountEvents(currentEvents, "ListingApproved"), CountEvents(previousEvents, "ListingApproved")) & " approved; " & Difference(CountEvents(currentEvents, "ListingRejected"), CountEvents(previousEvents, "ListingRejected")) & " rejected vs prior period", 0, y)

        Dim paymentEvents = events.Where(Function(item) (item.EventType = "SimulatedPurchaseCompleted" OrElse item.EventType = "SimulatedRentPaymentCompleted") AndAlso item.Timestamp.Year = current.Year).ToList()
        Dim commission = paymentEvents.Sum(Function(item) Decimal.Round(item.Amount * 0.03D, 2, MidpointRounding.AwayFromZero))
        Dim yearlyLabel = LabelText("YEAR-TO-DATE PAYMENTS", 9, accent, True)
        yearlyLabel.Location = New Point(380, 0)
        yearlyLabel.AutoSize = True
        parent.Controls.Add(yearlyLabel)
        Dim yearlyValue = LabelText(paymentEvents.Count & " payments   ·   " & FormatCedis(commission) & " estimated 3% commission", 13, ink, True)
        yearlyValue.Location = New Point(380, 28)
        yearlyValue.Size = New Size(520, 52)
        parent.Controls.Add(yearlyValue)
        Dim distinction = LabelText("Payment activity is simulated for ledger and reporting purposes; no funds are transferred.", 9, muted, False)
        distinction.Location = New Point(380, 88)
        distinction.Size = New Size(500, 80)
        parent.Controls.Add(distinction)
    End Sub

    Private Sub AddMetric(parent As Control, title As String, amount As String, comparison As String, x As Integer, y As Integer)
        Dim card As New Panel With {.Location = New Point(x, y), .Size = New Size(335, 100), .BackColor = Color.White, .BorderStyle = BorderStyle.None}
        parent.Controls.Add(card)
        Dim name = LabelText(title, 9, muted, True)
        name.Location = New Point(15, 12)
        name.AutoSize = True
        card.Controls.Add(name)
        Dim value = LabelText(amount, 21, ink, True)
        value.Location = New Point(15, 33)
        value.AutoSize = True
        card.Controls.Add(value)
        Dim change = LabelText(comparison, 8, accent, False)
        change.Location = New Point(15, 72)
        change.Size = New Size(305, 20)
        card.Controls.Add(change)
    End Sub

    Private Sub ShowListings(sender As Object, e As EventArgs)
        ShowListings()
    End Sub

    Private Sub ShowListings()
        If Not LoadData() Then Return
        Dim page = NewPage()
        Dim reviewControl As New RentLinkPropertyReviewControl With {.Dock = DockStyle.Fill}
        page.Controls.Add(reviewControl)
        Dim pending = listings.Where(Function(item) item.Status = "Pending review").OrderBy(Function(item) item.Id).ToList()
        reviewControl.SetPendingCount(pending.Count)
        reviewControl.ClearListings()
        AddHandler reviewControl.ReviewRequested, Sub(sender, args) ReviewListing(args.Listing, args.Status)
        If pending.Count = 0 Then
            reviewControl.ShowEmptyState()
            Return
        End If
        For Each item In pending
            Dim paymentSummary = "Payment details: not provided"
            Try
                Dim paymentDetails = RentLinkDataStore.LoadListingPaymentDetails(item.RemoteId)
                If paymentDetails IsNot Nothing Then
                    paymentSummary = If(paymentDetails.PaymentOption = "Mobile Money" OrElse paymentDetails.PaymentOption = "Both", "MoMo: " & paymentDetails.MobileMoneyNetwork & " · " & paymentDetails.MobileMoneyAccountName & " · " & paymentDetails.MobileMoneyNumber & Environment.NewLine, "") &
                        If(paymentDetails.PaymentOption = "Bank account" OrElse paymentDetails.PaymentOption = "Both", "Bank: " & paymentDetails.BankName & " · " & paymentDetails.BankAccountName & " · " & paymentDetails.BankAccountNumber, "")
                End If
            Catch ex As IOException
                ShowStorageError(ex)
                Return
            Catch ex As UnauthorizedAccessException
                ShowStorageError(ex)
                Return
            Catch ex As System.Text.Json.JsonException
                ShowStorageError(ex)
                Return
            End Try
            reviewControl.AddListing(item, ListingPriceText(item), paymentSummary)
        Next
    End Sub

    Private Sub ReviewListing(item As RentLinkWorkspace.PropertyListing, status As String)
        Dim oldStatus = item.Status
        item.Status = status
        Try
            RentLinkDataStore.SaveListings(listings)
        Catch ex As IOException
            item.Status = oldStatus
            ShowStorageError(ex)
            Return
        Catch ex As UnauthorizedAccessException
            item.Status = oldStatus
            ShowStorageError(ex)
            Return
        Catch ex As System.Text.Json.JsonException
            item.Status = oldStatus
            ShowStorageError(ex)
            Return
        End Try

        Try
            RentLinkDataStore.RecordActivity(If(status = "Approved", "ListingApproved", "ListingRejected"), item.OwnerEmail, item.Owner, item.Id, item.Title, listingRemoteId:=item.RemoteId, listingOwnerUid:=item.OwnerUid)
        Catch ex As IOException
            ShowStorageError(ex)
        Catch ex As UnauthorizedAccessException
            ShowStorageError(ex)
        Catch ex As System.Text.Json.JsonException
            ShowStorageError(ex)
        End Try
        ShowListings()
    End Sub

    Private Sub ShowTransactions(sender As Object, e As EventArgs)
        ShowTransactions()
    End Sub

    Private Sub ShowTransactions()
        If Not LoadData() Then Return
        Dim page = NewPage()
        Dim transactionsControl As New RentLinkTransactionsControl With {.Dock = DockStyle.Fill}
        page.Controls.Add(transactionsControl)

        Dim year = DateTime.Now.Year
        Dim paymentEvents = events.Where(Function(item) item.EventType = "SimulatedPurchaseCompleted" OrElse item.EventType = "SimulatedRentPaymentCompleted").OrderByDescending(Function(item) item.Timestamp).ToList()
        Dim currentYearPayments = paymentEvents.Where(Function(item) item.Timestamp.Year = year).ToList()
        Dim annualTotal = currentYearPayments.Sum(Function(item) item.Amount)
        Dim annualCommission = currentYearPayments.Sum(Function(item) Decimal.Round(item.Amount * 0.03D, 2, MidpointRounding.AwayFromZero))
        Dim annualOwnerBalance = currentYearPayments.Sum(Function(item) item.Amount - Decimal.Round(item.Amount * 0.03D, 2, MidpointRounding.AwayFromZero))
        transactionsControl.SetAnnualSummary(year & " payments: " & currentYearPayments.Count & " · total: " & FormatCedis(annualTotal) & " · 3% commission: " & FormatCedis(annualCommission) & " · owner balances: " & FormatCedis(annualOwnerBalance))
        transactionsControl.ClearMonthlySummary()
        For month As Integer = 1 To 12
            Dim monthStart = New DateTime(year, month, 1)
            Dim monthPayments = paymentEvents.Where(Function(item) item.Timestamp >= monthStart AndAlso item.Timestamp < monthStart.AddMonths(1)).ToList()
            Dim monthTotal = monthPayments.Sum(Function(item) item.Amount)
            Dim monthCommission = monthPayments.Sum(Function(item) Decimal.Round(item.Amount * 0.03D, 2, MidpointRounding.AwayFromZero))
            transactionsControl.AddMonthlySummary(monthStart.ToString("MMMM"), monthPayments.Count, FormatCedis(monthTotal), FormatCedis(monthCommission), month)
        Next

        transactionsControl.ClearPurchaseRecords()
        If paymentEvents.Count = 0 Then
            transactionsControl.ShowEmptyRecords()
        Else
            For Each payment In paymentEvents
                Dim details = If(payment.Notes, "").Split("|"c)
                Dim paymentAccount = If(details.Length > 2, details(2), "Account details unavailable")
                Dim commission = Decimal.Round(payment.Amount * 0.03D, 2, MidpointRounding.AwayFromZero)
                Dim ownerBalance = payment.Amount - commission
                Dim paymentType = If(payment.EventType = "SimulatedRentPaymentCompleted", "Rent", "Purchase")
                transactionsControl.AddPaymentRecord(payment.Timestamp.ToString("yyyy-MM-dd"), paymentType, payment.ListingTitle, payment.UserName, payment.UserEmail, FormatCedis(payment.Amount), FormatCedis(commission), FormatCedis(ownerBalance), paymentAccount)
            Next
        End If
    End Sub

    Private Function LoadData() As Boolean
        Try
            listings = RentLinkDataStore.LoadListings()
            events = RentLinkDataStore.LoadActivity()
            Return True
        Catch ex As IOException
            ShowStorageError(ex)
        Catch ex As UnauthorizedAccessException
            ShowStorageError(ex)
        Catch ex As System.Text.Json.JsonException
            ShowStorageError(ex)
        End Try
        Return False
    End Function

    Private Sub ShowStorageError(ex As Exception)
        MessageBox.Show("Local admin data could not be loaded or saved: " & ex.Message, "Local data error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Function NewPage() As Panel
        content.Controls.Clear()
        Dim page As New Panel With {.Dock = DockStyle.Fill, .AutoScroll = True, .BackColor = pageBack}
        content.Controls.Add(page)
        Return page
    End Function

    Private Sub AddHeading(page As Control, title As String, subtitle As String)
        Dim heading = LabelText(title, 25, ink, True)
        heading.Location = New Point(0, 0)
        heading.AutoSize = True
        page.Controls.Add(heading)
        Dim subheading = LabelText(subtitle, 10, muted, False)
        subheading.Location = New Point(1, 40)
        subheading.Size = New Size(900, 28)
        page.Controls.Add(subheading)
    End Sub

    Private Function LabelText(text As String, size As Single, color As Color, bold As Boolean) As Label
        Return New Label With {.Text = text, .Font = New Font("Segoe UI", size, If(bold, FontStyle.Bold, FontStyle.Regular)), .ForeColor = color, .BackColor = Color.Transparent, .AutoEllipsis = True}
    End Function

    Private Function MakeButton(text As String, back As Color, fore As Color, size As Single, bold As Boolean) As Button
        Dim button As New Button With {.Text = text, .Font = New Font("Segoe UI", size, If(bold, FontStyle.Bold, FontStyle.Regular)), .BackColor = back, .ForeColor = fore, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .UseVisualStyleBackColor = False}
        button.FlatAppearance.BorderSize = 0
        Return button
    End Function

    Private Function FormatCedis(amount As Decimal) As String
        Return "GH₵ " & amount.ToString("N2", CultureInfo.GetCultureInfo("en-GH"))
    End Function

    Private Function ListingPriceText(item As RentLinkWorkspace.PropertyListing) As String
        If item.Mode = "Purchase" Then Return FormatCedis(GetPurchasePrice(item)) & " full purchase price"
        If item.Mode = "Both" Then Return FormatCedis(GetRentPrice(item)) & RentPeriodSuffix(item.RentPeriod) & " rent  ·  " & FormatCedis(GetPurchasePrice(item)) & " full purchase price"
        Return FormatCedis(GetRentPrice(item)) & RentPeriodSuffix(item.RentPeriod)
    End Function

    Private Function GetRentPrice(item As RentLinkWorkspace.PropertyListing) As Decimal
        If item.RentPrice > 0D Then Return item.RentPrice
        Return If(item.Mode = "Rent" OrElse item.Mode = "Both", item.Price, 0D)
    End Function

    Private Function GetPurchasePrice(item As RentLinkWorkspace.PropertyListing) As Decimal
        If item.PurchasePrice > 0D Then Return item.PurchasePrice
        Return If(item.Mode = "Purchase" OrElse item.Mode = "Both", item.Price, 0D)
    End Function

    Private Function RentPeriodSuffix(period As String) As String
        Return If(String.Equals(period, "Per year", StringComparison.OrdinalIgnoreCase), " / year", " / month")
    End Function

    Private Function CountEvents(source As IEnumerable(Of RentLinkActivityEvent), eventType As String) As Integer
        Return source.Count(Function(item) item.EventType = eventType)
    End Function

    Private Function IsAdministratorLogin(item As RentLinkActivityEvent) As Boolean
        Return String.Equals(item.UserUid, RentLinkLocalStore.AdministratorUid, StringComparison.Ordinal)
    End Function

    Private Function UniqueUsers(source As IEnumerable(Of RentLinkActivityEvent)) As Integer
        Return source.Where(Function(item) Not String.IsNullOrWhiteSpace(item.UserEmail)).Select(Function(item) item.UserEmail.ToLowerInvariant()).Distinct().Count()
    End Function

    Private Function Difference(current As Integer, previous As Integer) As String
        Dim delta = current - previous
        Return If(delta > 0, "+", "") & delta.ToString(CultureInfo.InvariantCulture)
    End Function

End Class