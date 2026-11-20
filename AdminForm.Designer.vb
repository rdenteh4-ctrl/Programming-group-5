<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class AdminForm
    Inherits System.Windows.Forms.Form

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer
    Friend WithEvents adminLayout As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents sidebarPanel As System.Windows.Forms.Panel
    Friend WithEvents brandLabel As System.Windows.Forms.Label
    Friend WithEvents adminTagLabel As System.Windows.Forms.Label
    Friend WithEvents overviewButton As System.Windows.Forms.Button
    Friend WithEvents propertyReviewButton As System.Windows.Forms.Button
    Friend WithEvents transactionsButton As System.Windows.Forms.Button
    Friend WithEvents logoutButton As System.Windows.Forms.Button
    Friend WithEvents content As System.Windows.Forms.Panel

    Private Sub InitializeComponent()
        adminLayout = New TableLayoutPanel()
        sidebarPanel = New Panel()
        brandLabel = New Label()
        adminTagLabel = New Label()
        overviewButton = New Button()
        propertyReviewButton = New Button()
        transactionsButton = New Button()
        logoutButton = New Button()
        content = New Panel()
        adminLayout.SuspendLayout()
        sidebarPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' adminLayout
        ' 
        adminLayout.ColumnCount = 2
        adminLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 245.0F))
        adminLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        adminLayout.Controls.Add(sidebarPanel, 0, 0)
        adminLayout.Controls.Add(content, 1, 0)
        adminLayout.Dock = DockStyle.Fill
        adminLayout.Location = New Point(0, 0)
        adminLayout.Margin = New Padding(0)
        adminLayout.Name = "adminLayout"
        adminLayout.RowCount = 1
        adminLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        adminLayout.Size = New Size(1250, 749)
        adminLayout.TabIndex = 0
        ' 
        ' sidebarPanel
        ' 
        sidebarPanel.BackColor = Color.White
        sidebarPanel.Controls.Add(brandLabel)
        sidebarPanel.Controls.Add(adminTagLabel)
        sidebarPanel.Controls.Add(overviewButton)
        sidebarPanel.Controls.Add(propertyReviewButton)
        sidebarPanel.Controls.Add(transactionsButton)
        sidebarPanel.Controls.Add(logoutButton)
        sidebarPanel.Dock = DockStyle.Fill
        sidebarPanel.Location = New Point(3, 3)
        sidebarPanel.Name = "sidebarPanel"
        sidebarPanel.Size = New Size(239, 743)
        sidebarPanel.TabIndex = 0
        ' 
        ' brandLabel
        ' 
        brandLabel.AutoSize = True
        brandLabel.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        brandLabel.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        brandLabel.Location = New Point(20, 25)
        brandLabel.Name = "brandLabel"
        brandLabel.Size = New Size(118, 30)
        brandLabel.TabIndex = 0
        brandLabel.Text = "RENTLINK"
        ' 
        ' adminTagLabel
        ' 
        adminTagLabel.AutoSize = True
        adminTagLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        adminTagLabel.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        adminTagLabel.Location = New Point(21, 55)
        adminTagLabel.Name = "adminTagLabel"
        adminTagLabel.Size = New Size(104, 15)
        adminTagLabel.TabIndex = 1
        adminTagLabel.Text = "ADMIN CONSOLE"
        ' 
        ' overviewButton
        ' 
        overviewButton.BackColor = Color.FromArgb(CByte(204), CByte(251), CByte(241))
        overviewButton.FlatAppearance.BorderSize = 0
        overviewButton.FlatStyle = FlatStyle.Flat
        overviewButton.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        overviewButton.ForeColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        overviewButton.Location = New Point(12, 115)
        overviewButton.Name = "overviewButton"
        overviewButton.Size = New Size(195, 40)
        overviewButton.TabIndex = 2
        overviewButton.Text = "Overview"
        overviewButton.TextAlign = ContentAlignment.MiddleLeft
        overviewButton.UseVisualStyleBackColor = False
        ' 
        ' propertyReviewButton
        ' 
        propertyReviewButton.BackColor = Color.White
        propertyReviewButton.FlatAppearance.BorderSize = 0
        propertyReviewButton.FlatStyle = FlatStyle.Flat
        propertyReviewButton.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        propertyReviewButton.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        propertyReviewButton.Location = New Point(12, 165)
        propertyReviewButton.Name = "propertyReviewButton"
        propertyReviewButton.Size = New Size(195, 40)
        propertyReviewButton.TabIndex = 3
        propertyReviewButton.Text = "Property review"
        propertyReviewButton.TextAlign = ContentAlignment.MiddleLeft
        propertyReviewButton.UseVisualStyleBackColor = False
        ' 
        ' transactionsButton
        ' 
        transactionsButton.BackColor = Color.White
        transactionsButton.FlatAppearance.BorderSize = 0
        transactionsButton.FlatStyle = FlatStyle.Flat
        transactionsButton.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        transactionsButton.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        transactionsButton.Location = New Point(12, 215)
        transactionsButton.Name = "transactionsButton"
        transactionsButton.Size = New Size(195, 40)
        transactionsButton.TabIndex = 4
        transactionsButton.Text = "Transactions"
        transactionsButton.TextAlign = ContentAlignment.MiddleLeft
        transactionsButton.UseVisualStyleBackColor = False
        ' 
        ' logoutButton
        ' 
        logoutButton.BackColor = Color.White
        logoutButton.FlatAppearance.BorderSize = 0
        logoutButton.FlatStyle = FlatStyle.Flat
        logoutButton.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        logoutButton.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        logoutButton.Location = New Point(12, 265)
        logoutButton.Name = "logoutButton"
        logoutButton.Size = New Size(195, 40)
        logoutButton.TabIndex = 5
        logoutButton.Text = "Log out"
        logoutButton.TextAlign = ContentAlignment.MiddleLeft
        logoutButton.UseVisualStyleBackColor = False
        ' 
        ' content
        ' 
        content.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        content.Dock = DockStyle.Fill
        content.Location = New Point(248, 3)
        content.Name = "content"
        content.Padding = New Padding(30)
        content.Size = New Size(999, 743)
        content.TabIndex = 1
        ' 
        ' AdminForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        ClientSize = New Size(1250, 749)
        Controls.Add(adminLayout)
        MinimumSize = New Size(1000, 680)
        Name = "AdminForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "RentLink | Administration"
        adminLayout.ResumeLayout(False)
        sidebarPanel.ResumeLayout(False)
        sidebarPanel.PerformLayout()
        ResumeLayout(False)
    End Sub
End Class
