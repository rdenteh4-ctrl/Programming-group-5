<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RentLinkPropertyReviewControl
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
    Friend WithEvents pendingCountLabel As System.Windows.Forms.Label
    Friend WithEvents listingsFlow As System.Windows.Forms.FlowLayoutPanel

    Private Sub InitializeComponent()
        headingLabel = New Label()
        subtitleLabel = New Label()
        pendingCountLabel = New Label()
        listingsFlow = New FlowLayoutPanel()
        SuspendLayout()
        headingLabel.AutoSize = True
        headingLabel.Font = New Font("Segoe UI", 25.0F, FontStyle.Bold)
        headingLabel.ForeColor = Color.FromArgb(30, 41, 59)
        headingLabel.Location = New Point(30, 22)
        headingLabel.Name = "headingLabel"
        headingLabel.Size = New Size(250, 46)
        headingLabel.TabIndex = 0
        headingLabel.Text = "Property review"
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 10.0F)
        subtitleLabel.ForeColor = Color.FromArgb(100, 116, 139)
        subtitleLabel.Location = New Point(31, 62)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(695, 19)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Approve or reject submitted properties. Decisions are shared with the user-facing app on this device."
        pendingCountLabel.AutoSize = True
        pendingCountLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        pendingCountLabel.ForeColor = Color.FromArgb(13, 148, 136)
        pendingCountLabel.Location = New Point(30, 100)
        pendingCountLabel.Name = "pendingCountLabel"
        pendingCountLabel.Size = New Size(160, 19)
        pendingCountLabel.TabIndex = 2
        pendingCountLabel.Text = "0 properties pending review"
        listingsFlow.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        listingsFlow.AutoScroll = True
        listingsFlow.BackColor = Color.FromArgb(248, 250, 252)
        listingsFlow.FlowDirection = FlowDirection.TopDown
        listingsFlow.Location = New Point(30, 130)
        listingsFlow.Name = "listingsFlow"
        listingsFlow.Size = New Size(950, 520)
        listingsFlow.TabIndex = 3
        listingsFlow.WrapContents = False
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(248, 250, 252)
        Controls.Add(listingsFlow)
        Controls.Add(pendingCountLabel)
        Controls.Add(subtitleLabel)
        Controls.Add(headingLabel)
        Name = "RentLinkPropertyReviewControl"
        Size = New Size(1020, 680)
        ResumeLayout(False)
        PerformLayout()
    End Sub
End Class
