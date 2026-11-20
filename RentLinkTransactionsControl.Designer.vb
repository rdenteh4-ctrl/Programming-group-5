<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RentLinkTransactionsControl
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
    Friend WithEvents cautionLabel As System.Windows.Forms.Label
    Friend WithEvents annualSummaryLabel As System.Windows.Forms.Label
    Friend WithEvents monthlyHeadingLabel As System.Windows.Forms.Label
    Friend WithEvents monthlySummaryFlow As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents recordsHeadingLabel As System.Windows.Forms.Label
    Friend WithEvents purchaseRecordsFlow As System.Windows.Forms.FlowLayoutPanel

    Private Sub InitializeComponent()
        headingLabel = New Label()
        subtitleLabel = New Label()
        cautionLabel = New Label()
        annualSummaryLabel = New Label()
        monthlyHeadingLabel = New Label()
        monthlySummaryFlow = New FlowLayoutPanel()
        recordsHeadingLabel = New Label()
        purchaseRecordsFlow = New FlowLayoutPanel()
        SuspendLayout()
        headingLabel.AutoSize = True
        headingLabel.Font = New Font("Segoe UI", 25.0F, FontStyle.Bold)
        headingLabel.ForeColor = Color.FromArgb(30, 41, 59)
        headingLabel.Location = New Point(30, 22)
        headingLabel.Name = "headingLabel"
        headingLabel.Size = New Size(390, 46)
        headingLabel.TabIndex = 0
        headingLabel.Text = "Transactions & commission"
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 10.0F)
        subtitleLabel.ForeColor = Color.FromArgb(100, 116, 139)
        subtitleLabel.Location = New Point(31, 62)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(780, 19)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Review RentLink payment activity and estimated 3% commission."
        cautionLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        cautionLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        cautionLabel.ForeColor = Color.FromArgb(139, 90, 20)
        cautionLabel.Location = New Point(30, 91)
        cautionLabel.Name = "cautionLabel"
        cautionLabel.Size = New Size(900, 36)
        cautionLabel.TabIndex = 2
        cautionLabel.Text = "Simulated checkout records activity only; no real funds are transferred."
        annualSummaryLabel.AutoEllipsis = True
        annualSummaryLabel.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
        annualSummaryLabel.ForeColor = Color.FromArgb(30, 41, 59)
        annualSummaryLabel.Location = New Point(30, 137)
        annualSummaryLabel.Name = "annualSummaryLabel"
        annualSummaryLabel.Size = New Size(900, 46)
        annualSummaryLabel.TabIndex = 3
        annualSummaryLabel.Text = "Year total: GH₵ 0.00 in simulated payments · GH₵ 0.00 estimated commission"
        monthlyHeadingLabel.AutoSize = True
        monthlyHeadingLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        monthlyHeadingLabel.ForeColor = Color.FromArgb(13, 148, 136)
        monthlyHeadingLabel.Location = New Point(30, 193)
        monthlyHeadingLabel.Name = "monthlyHeadingLabel"
        monthlyHeadingLabel.Size = New Size(153, 15)
        monthlyHeadingLabel.TabIndex = 4
        monthlyHeadingLabel.Text = "MONTHLY PAYMENT SUMMARY · YEAR"
        monthlySummaryFlow.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        monthlySummaryFlow.AutoScroll = True
        monthlySummaryFlow.BackColor = Color.FromArgb(248, 250, 252)
        monthlySummaryFlow.FlowDirection = FlowDirection.TopDown
        monthlySummaryFlow.Location = New Point(30, 217)
        monthlySummaryFlow.Name = "monthlySummaryFlow"
        monthlySummaryFlow.Size = New Size(900, 240)
        monthlySummaryFlow.TabIndex = 5
        monthlySummaryFlow.WrapContents = False
        recordsHeadingLabel.AutoSize = True
        recordsHeadingLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        recordsHeadingLabel.ForeColor = Color.FromArgb(13, 148, 136)
        recordsHeadingLabel.Location = New Point(30, 468)
        recordsHeadingLabel.Name = "recordsHeadingLabel"
        recordsHeadingLabel.Size = New Size(280, 15)
        recordsHeadingLabel.TabIndex = 6
        recordsHeadingLabel.Text = "RECENT RENT & PURCHASE PAYMENTS"
        purchaseRecordsFlow.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        purchaseRecordsFlow.AutoScroll = True
        purchaseRecordsFlow.BackColor = Color.FromArgb(248, 250, 252)
        purchaseRecordsFlow.FlowDirection = FlowDirection.TopDown
        purchaseRecordsFlow.Location = New Point(30, 493)
        purchaseRecordsFlow.Name = "purchaseRecordsFlow"
        purchaseRecordsFlow.Size = New Size(900, 182)
        purchaseRecordsFlow.TabIndex = 7
        purchaseRecordsFlow.WrapContents = False
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(248, 250, 252)
        Controls.Add(purchaseRecordsFlow)
        Controls.Add(recordsHeadingLabel)
        Controls.Add(monthlySummaryFlow)
        Controls.Add(monthlyHeadingLabel)
        Controls.Add(annualSummaryLabel)
        Controls.Add(cautionLabel)
        Controls.Add(subtitleLabel)
        Controls.Add(headingLabel)
        Name = "RentLinkTransactionsControl"
        Size = New Size(1000, 700)
        ResumeLayout(False)
        PerformLayout()
    End Sub
End Class
