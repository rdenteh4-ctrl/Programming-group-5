<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RentLinkActivityControl
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
    Friend WithEvents savedHomesHeadingLabel As System.Windows.Forms.Label
    Friend WithEvents savedHomesCountLabel As System.Windows.Forms.Label
    Friend WithEvents savedHomesFlow As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents recentActivityHeadingLabel As System.Windows.Forms.Label
    Friend WithEvents recentActivityFlow As System.Windows.Forms.FlowLayoutPanel

    Private Sub InitializeComponent()
        headingLabel = New Label()
        subtitleLabel = New Label()
        savedHomesHeadingLabel = New Label()
        savedHomesCountLabel = New Label()
        savedHomesFlow = New FlowLayoutPanel()
        recentActivityHeadingLabel = New Label()
        recentActivityFlow = New FlowLayoutPanel()
        SuspendLayout()
        '
        'headingLabel
        '
        headingLabel.AutoSize = True
        headingLabel.Font = New Font("Segoe UI", 25.0F, FontStyle.Bold)
        headingLabel.ForeColor = Color.FromArgb(30, 41, 59)
        headingLabel.Location = New Point(30, 22)
        headingLabel.Name = "headingLabel"
        headingLabel.Size = New Size(249, 46)
        headingLabel.TabIndex = 0
        headingLabel.Text = "Your activity"
        '
        'subtitleLabel
        '
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 10.0F)
        subtitleLabel.ForeColor = Color.FromArgb(100, 116, 139)
        subtitleLabel.Location = New Point(31, 62)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(370, 19)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Your saved homes and recent steps, all in one place."
        '
        'savedHomesHeadingLabel
        '
        savedHomesHeadingLabel.AutoSize = True
        savedHomesHeadingLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        savedHomesHeadingLabel.ForeColor = Color.FromArgb(13, 148, 136)
        savedHomesHeadingLabel.Location = New Point(30, 111)
        savedHomesHeadingLabel.Name = "savedHomesHeadingLabel"
        savedHomesHeadingLabel.Size = New Size(100, 19)
        savedHomesHeadingLabel.TabIndex = 2
        savedHomesHeadingLabel.Text = "SAVED HOMES"
        '
        'savedHomesCountLabel
        '
        savedHomesCountLabel.AutoSize = True
        savedHomesCountLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        savedHomesCountLabel.ForeColor = Color.FromArgb(13, 148, 136)
        savedHomesCountLabel.Location = New Point(145, 111)
        savedHomesCountLabel.Name = "savedHomesCountLabel"
        savedHomesCountLabel.Size = New Size(17, 19)
        savedHomesCountLabel.TabIndex = 3
        savedHomesCountLabel.Text = "0"
        '
        'savedHomesFlow
        '
        savedHomesFlow.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        savedHomesFlow.AutoScroll = True
        savedHomesFlow.BackColor = Color.FromArgb(248, 250, 252)
        savedHomesFlow.FlowDirection = FlowDirection.TopDown
        savedHomesFlow.Location = New Point(30, 140)
        savedHomesFlow.Name = "savedHomesFlow"
        savedHomesFlow.Size = New Size(1000, 180)
        savedHomesFlow.TabIndex = 4
        savedHomesFlow.WrapContents = False
        '
        'recentActivityHeadingLabel
        '
        recentActivityHeadingLabel.AutoSize = True
        recentActivityHeadingLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        recentActivityHeadingLabel.ForeColor = Color.FromArgb(13, 148, 136)
        recentActivityHeadingLabel.Location = New Point(30, 345)
        recentActivityHeadingLabel.Name = "recentActivityHeadingLabel"
        recentActivityHeadingLabel.Size = New Size(128, 19)
        recentActivityHeadingLabel.TabIndex = 5
        recentActivityHeadingLabel.Text = "RECENT ACTIVITY"
        '
        'recentActivityFlow
        '
        recentActivityFlow.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        recentActivityFlow.AutoScroll = True
        recentActivityFlow.BackColor = Color.FromArgb(248, 250, 252)
        recentActivityFlow.FlowDirection = FlowDirection.TopDown
        recentActivityFlow.Location = New Point(30, 374)
        recentActivityFlow.Name = "recentActivityFlow"
        recentActivityFlow.Size = New Size(1000, 310)
        recentActivityFlow.TabIndex = 6
        recentActivityFlow.WrapContents = False
        '
        'RentLinkActivityControl
        '
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(248, 250, 252)
        Controls.Add(recentActivityFlow)
        Controls.Add(recentActivityHeadingLabel)
        Controls.Add(savedHomesFlow)
        Controls.Add(savedHomesCountLabel)
        Controls.Add(savedHomesHeadingLabel)
        Controls.Add(subtitleLabel)
        Controls.Add(headingLabel)
        Name = "RentLinkActivityControl"
        Size = New Size(1060, 700)
        ResumeLayout(False)
        PerformLayout()
    End Sub
End Class
