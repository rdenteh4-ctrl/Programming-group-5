Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms

Partial Public Class RentLinkActivityControl
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
        BackColor = RentLinkRoundedCorners.PageColor
        AddHandler savedHomesFlow.Resize, AddressOf SavedHomesFlowResized
        AddHandler recentActivityFlow.Resize, AddressOf RecentActivityFlowResized
        RentLinkRoundedCorners.ApplyTo(Me)
    End Sub

    Public Sub ClearSavedHomes()
        savedHomesFlow.Controls.Clear()
        savedHomesCountLabel.Text = "0"
    End Sub

    Public Sub AddSavedHome(displayText As String, openAction As Action)
        Dim row = CreateRow(displayText, savedHomesFlow.ClientSize.Width)
        If openAction IsNot Nothing Then
            Dim openButton As New Button With {
                .Text = "Open home",
                .BackColor = Color.White,
                .ForeColor = RentLinkRoundedCorners.AccentColor,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .Size = New Size(104, 32),
                .Location = New Point(row.Width - 118, 10),
                .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
                .UseVisualStyleBackColor = False
            }
            openButton.FlatAppearance.BorderSize = 0
            row.Controls.Add(openButton)
            AddHandler openButton.Click, Sub() openAction()
        End If
        savedHomesFlow.Controls.Add(row)
        savedHomesCountLabel.Text = savedHomesFlow.Controls.Count.ToString()
    End Sub

    Public Sub ShowEmptySavedHomes()
        AddEmptyState(savedHomesFlow, "Homes you save will appear here.")
    End Sub

    Public Sub SetRecentActivities(items As IEnumerable(Of String))
        recentActivityFlow.Controls.Clear()
        If items Is Nothing Then
            AddEmptyState(recentActivityFlow, "Your home-search activity will show up here.")
            Return
        End If

        Dim count = 0
        For Each item In items.Take(20)
            AddRecentActivity(item)
            count += 1
        Next

        If count = 0 Then AddEmptyState(recentActivityFlow, "Your home-search activity will show up here.")
    End Sub

    Private Sub AddRecentActivity(text As String)
        Dim row As New Panel With {
            .BackColor = Color.White,
            .Size = New Size(Math.Max(520, recentActivityFlow.ClientSize.Width - 24), 60),
            .Margin = New Padding(0, 0, 0, 8)
        }
        Dim phoneMarker = "Phone: "
        Dim phoneMarkerIndex = text.LastIndexOf(phoneMarker, StringComparison.Ordinal)
        If phoneMarkerIndex >= 0 Then
            Dim phoneStart = phoneMarkerIndex + phoneMarker.Length
            Dim phone = text.Substring(phoneStart).Trim()
            Dim phoneLink As New LinkLabel With {
                .AutoEllipsis = False,
                .Font = New Font("Segoe UI", 9.5F),
                .ForeColor = RentLinkRoundedCorners.InkColor,
                .Location = New Point(14, 8),
                .Size = New Size(Math.Max(300, row.Width - 28), 44),
                .Text = text,
                .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            }
            phoneLink.Links.Add(phoneStart, phone.Length, phone)
            AddHandler phoneLink.LinkClicked, AddressOf EnquiryPhoneClicked
            row.Controls.Add(phoneLink)
        Else
            Dim label As New Label With {
                .AutoEllipsis = False,
                .Font = New Font("Segoe UI", 9.5F),
                .ForeColor = RentLinkRoundedCorners.InkColor,
                .Location = New Point(14, 8),
                .Size = New Size(Math.Max(300, row.Width - 28), 44),
                .Text = text,
                .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            }
            row.Controls.Add(label)
        End If
        recentActivityFlow.Controls.Add(row)
    End Sub

    Private Sub EnquiryPhoneClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Dim phone = CStr(e.Link.LinkData)
        Dim phoneNumber = New String(phone.Where(Function(character) Char.IsDigit(character) OrElse character = "+"c).ToArray())
        Dim phoneDigits = New String(phoneNumber.Where(Function(character) Char.IsDigit(character)).ToArray())
        If phoneDigits.Length < 7 OrElse phoneDigits.Length > 15 OrElse phoneNumber.Count(Function(character) character = "+"c) > 1 OrElse (phoneNumber.Contains("+"c) AndAlso Not phoneNumber.StartsWith("+", StringComparison.Ordinal)) Then
            MessageBox.Show("This renter's phone number is invalid.", "Contact details unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            Process.Start(New ProcessStartInfo("tel:" & phoneNumber) With {.UseShellExecute = True})
        Catch ex As Win32Exception
            MessageBox.Show("Windows could not open a phone app for this number. Check that a suitable calling app is installed and configured.", "Could not open phone app", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As InvalidOperationException
            MessageBox.Show("Windows could not open a phone app for this number. Check that a suitable calling app is installed and configured.", "Could not open phone app", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function CreateRow(text As String, availableWidth As Integer) As Panel
        Dim row As New Panel With {
            .BackColor = Color.White,
            .Size = New Size(Math.Max(520, availableWidth - 24), 52),
            .Margin = New Padding(0, 0, 0, 8)
        }
        Dim label As New Label With {
            .AutoEllipsis = True,
            .Font = New Font("Segoe UI", 10.0F),
            .ForeColor = RentLinkRoundedCorners.InkColor,
            .Location = New Point(14, 14),
            .Size = New Size(Math.Max(300, row.Width - 160), 24),
            .Text = text,
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        }
        row.Controls.Add(label)
        Return row
    End Function

    Private Sub AddEmptyState(target As FlowLayoutPanel, message As String)
        Dim label As New Label With {
            .AutoSize = False,
            .Font = New Font("Segoe UI", 10.0F),
            .ForeColor = RentLinkRoundedCorners.MutedColor,
            .Size = New Size(Math.Max(520, target.ClientSize.Width - 24), 42),
            .Text = message,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Margin = New Padding(0)
        }
        target.Controls.Add(label)
    End Sub

    Private Sub ResizeRows(target As FlowLayoutPanel)
        For Each row As Control In target.Controls
            row.Width = Math.Max(520, target.ClientSize.Width - 24)
            For Each child As Control In row.Controls
                If TypeOf child Is Label Then
                    child.Width = Math.Max(300, row.Width - If(row.Controls.Count > 1, 160, 28))
                ElseIf TypeOf child Is Button Then
                    child.Left = row.Width - child.Width - 14
                End If
            Next
        Next
    End Sub

    Private Sub SavedHomesFlowResized(sender As Object, e As EventArgs)
        ResizeRows(savedHomesFlow)
    End Sub

    Private Sub RecentActivityFlowResized(sender As Object, e As EventArgs)
        ResizeRows(recentActivityFlow)
    End Sub
End Class
