Imports System.Drawing
Imports System.Windows.Forms

Partial Public Class RentLinkTransactionsControl
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
        BackColor = RentLinkRoundedCorners.PageColor
        RentLinkRoundedCorners.ApplyTo(Me)
    End Sub

    Public Sub SetAnnualSummary(text As String)
        annualSummaryLabel.Text = text
    End Sub

    Public Sub ClearMonthlySummary()
        monthlySummaryFlow.Controls.Clear()
    End Sub

    Public Sub AddMonthlySummary(monthName As String, paymentCount As Integer, total As String, commission As String, monthIndex As Integer)
        Dim row As New Panel With {.BackColor = If(monthIndex Mod 2 = 0, Color.White, RentLinkRoundedCorners.PageColor), .Size = New Size(Math.Max(680, monthlySummaryFlow.ClientSize.Width - 24), 32), .Margin = New Padding(0)}
        row.Controls.Add(New Label With {.Text = monthName, .Location = New Point(10, 6), .Size = New Size(180, 20), .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .ForeColor = RentLinkRoundedCorners.InkColor})
        row.Controls.Add(New Label With {.Text = paymentCount & " payments · " & total & " total", .Location = New Point(200, 6), .Size = New Size(300, 20), .Font = New Font("Segoe UI", 9.0F), .ForeColor = RentLinkRoundedCorners.InkColor})
        row.Controls.Add(New Label With {.Text = "3% commission: " & commission, .Location = New Point(515, 6), .Size = New Size(270, 20), .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .ForeColor = RentLinkRoundedCorners.AccentColor})
        monthlySummaryFlow.Controls.Add(row)
    End Sub

    Public Sub ClearPurchaseRecords()
        purchaseRecordsFlow.Controls.Clear()
    End Sub

    Public Sub AddPaymentRecord(dateText As String, paymentType As String, listingTitle As String, customerName As String, customerEmail As String, amountText As String, commissionText As String, ownerBalanceText As String, paymentAccount As String)
        Dim row As New Panel With {.BackColor = Color.FromArgb(255, 251, 245), .Size = New Size(Math.Max(680, purchaseRecordsFlow.ClientSize.Width - 24), 72), .Margin = New Padding(0, 0, 0, 6)}
        row.Controls.Add(New Label With {.Text = "SIMULATED " & paymentType.ToUpperInvariant() & " PAYMENT · " & dateText & " · " & listingTitle & " · Customer: " & customerName & " (" & customerEmail & ")", .Location = New Point(12, 7), .Size = New Size(row.Width - 24, 22), .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .ForeColor = RentLinkRoundedCorners.InkColor, .AutoEllipsis = True})
        row.Controls.Add(New Label With {.Text = amountText & " · 3% commission " & commissionText & " · Owner balance " & ownerBalanceText & " · " & paymentAccount & " · No actual funds transferred", .Location = New Point(12, 36), .Size = New Size(row.Width - 24, 24), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .ForeColor = Color.FromArgb(139, 90, 20), .AutoEllipsis = True})
        purchaseRecordsFlow.Controls.Add(row)
    End Sub

    Public Sub ShowEmptyRecords()
        purchaseRecordsFlow.Controls.Add(New Label With {.Text = "No simulated payments have been recorded.", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F), .ForeColor = RentLinkRoundedCorners.MutedColor, .Margin = New Padding(0, 8, 0, 0)})
    End Sub
End Class
