<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RentLinkMyPropertiesControl
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
    Friend WithEvents addPropertyButton As System.Windows.Forms.Button
    Friend WithEvents propertyCardsFlow As System.Windows.Forms.FlowLayoutPanel

    Private Sub InitializeComponent()
        headingLabel = New Label()
        subtitleLabel = New Label()
        addPropertyButton = New Button()
        propertyCardsFlow = New FlowLayoutPanel()
        SuspendLayout()
        headingLabel.AutoSize = True
        headingLabel.Font = New Font("Segoe UI", 25.0F, FontStyle.Bold)
        headingLabel.ForeColor = Color.FromArgb(30, 41, 59)
        headingLabel.Location = New Point(30, 22)
        headingLabel.Name = "headingLabel"
        headingLabel.Size = New Size(260, 46)
        headingLabel.TabIndex = 0
        headingLabel.Text = "Your properties"
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 10.0F)
        subtitleLabel.ForeColor = Color.FromArgb(100, 116, 139)
        subtitleLabel.Location = New Point(31, 62)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(425, 19)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Track review status and engagement for each home you list."
        addPropertyButton.BackColor = Color.FromArgb(13, 148, 136)
        addPropertyButton.FlatAppearance.BorderSize = 0
        addPropertyButton.FlatStyle = FlatStyle.Flat
        addPropertyButton.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        addPropertyButton.ForeColor = Color.White
        addPropertyButton.Location = New Point(30, 92)
        addPropertyButton.Name = "addPropertyButton"
        addPropertyButton.Size = New Size(190, 40)
        addPropertyButton.TabIndex = 2
        addPropertyButton.Text = "+  Add another property"
        addPropertyButton.UseVisualStyleBackColor = False
        propertyCardsFlow.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        propertyCardsFlow.AutoScroll = True
        propertyCardsFlow.BackColor = Color.FromArgb(248, 250, 252)
        propertyCardsFlow.FlowDirection = FlowDirection.TopDown
        propertyCardsFlow.Location = New Point(30, 150)
        propertyCardsFlow.Name = "propertyCardsFlow"
        propertyCardsFlow.Size = New Size(950, 510)
        propertyCardsFlow.TabIndex = 3
        propertyCardsFlow.WrapContents = False
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(248, 250, 252)
        Controls.Add(propertyCardsFlow)
        Controls.Add(addPropertyButton)
        Controls.Add(subtitleLabel)
        Controls.Add(headingLabel)
        Name = "RentLinkMyPropertiesControl"
        Size = New Size(1020, 680)
        ResumeLayout(False)
        PerformLayout()
    End Sub
End Class
