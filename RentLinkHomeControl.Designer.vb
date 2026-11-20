<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RentLinkHomeControl
    Inherits System.Windows.Forms.UserControl

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer
    Friend WithEvents sidebarPanel As System.Windows.Forms.Panel
    Friend WithEvents brandLabel As System.Windows.Forms.Label
    Friend WithEvents taglineLabel As System.Windows.Forms.Label
    Friend WithEvents exploreButton As System.Windows.Forms.Button
    Friend WithEvents activityButton As System.Windows.Forms.Button
    Friend WithEvents propertiesButton As System.Windows.Forms.Button
    Friend WithEvents listPropertyButton As System.Windows.Forms.Button
    Friend WithEvents profilePanel As System.Windows.Forms.Panel
    Friend WithEvents accountInitialsLabel As System.Windows.Forms.Label
    Friend WithEvents accountNameLabel As System.Windows.Forms.Label
    Friend WithEvents signOutButton As System.Windows.Forms.Button
    Friend WithEvents contentPanel As System.Windows.Forms.Panel
    Friend WithEvents screenHostPanel As System.Windows.Forms.Panel
    Friend WithEvents headingLabel As System.Windows.Forms.Label
    Friend WithEvents subtitleLabel As System.Windows.Forms.Label
    Friend WithEvents rentButton As System.Windows.Forms.Button
    Friend WithEvents purchaseButton As System.Windows.Forms.Button
    Friend WithEvents searchInput As System.Windows.Forms.TextBox
    Friend WithEvents searchButton As System.Windows.Forms.Button
    Friend WithEvents resultsHeadingLabel As System.Windows.Forms.Label
    Friend WithEvents resultsFlow As System.Windows.Forms.FlowLayoutPanel

    Private Sub InitializeComponent()
        sidebarPanel = New Panel()
        brandLabel = New Label()
        taglineLabel = New Label()
        exploreButton = New Button()
        activityButton = New Button()
        propertiesButton = New Button()
        listPropertyButton = New Button()
        profilePanel = New Panel()
        accountInitialsLabel = New Label()
        accountNameLabel = New Label()
        signOutButton = New Button()
        contentPanel = New Panel()
        screenHostPanel = New Panel()
        resultsFlow = New FlowLayoutPanel()
        resultsHeadingLabel = New Label()
        searchButton = New Button()
        searchInput = New TextBox()
        purchaseButton = New Button()
        rentButton = New Button()
        subtitleLabel = New Label()
        headingLabel = New Label()
        sidebarPanel.SuspendLayout()
        profilePanel.SuspendLayout()
        contentPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' sidebarPanel
        ' 
        sidebarPanel.BackColor = Color.White
        sidebarPanel.Controls.Add(brandLabel)
        sidebarPanel.Controls.Add(taglineLabel)
        sidebarPanel.Controls.Add(exploreButton)
        sidebarPanel.Controls.Add(activityButton)
        sidebarPanel.Controls.Add(propertiesButton)
        sidebarPanel.Controls.Add(listPropertyButton)
        sidebarPanel.Controls.Add(profilePanel)
        sidebarPanel.Dock = DockStyle.Left
        sidebarPanel.Location = New Point(0, 0)
        sidebarPanel.Name = "sidebarPanel"
        sidebarPanel.Padding = New Padding(20, 26, 16, 18)
        sidebarPanel.Size = New Size(246, 749)
        sidebarPanel.TabIndex = 0
        ' 
        ' brandLabel
        ' 
        brandLabel.AutoSize = True
        brandLabel.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
        brandLabel.ForeColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        brandLabel.Location = New Point(22, 24)
        brandLabel.Name = "brandLabel"
        brandLabel.Size = New Size(118, 30)
        brandLabel.TabIndex = 0
        brandLabel.Text = "RENTLINK"
        ' 
        ' taglineLabel
        ' 
        taglineLabel.AutoSize = True
        taglineLabel.Font = New Font("Segoe UI", 9F)
        taglineLabel.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        taglineLabel.Location = New Point(23, 55)
        taglineLabel.Name = "taglineLabel"
        taglineLabel.Size = New Size(100, 15)
        taglineLabel.TabIndex = 1
        taglineLabel.Text = "A place to belong"
        ' 
        ' exploreButton
        ' 
        exploreButton.BackColor = Color.FromArgb(CByte(204), CByte(251), CByte(241))
        exploreButton.FlatAppearance.BorderSize = 0
        exploreButton.FlatStyle = FlatStyle.Flat
        exploreButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        exploreButton.ForeColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        exploreButton.Location = New Point(12, 125)
        exploreButton.Name = "exploreButton"
        exploreButton.Size = New Size(205, 42)
        exploreButton.TabIndex = 2
        exploreButton.Text = "   Explore homes"
        exploreButton.TextAlign = ContentAlignment.MiddleLeft
        exploreButton.UseVisualStyleBackColor = False
        ' 
        ' activityButton
        ' 
        activityButton.BackColor = Color.White
        activityButton.FlatAppearance.BorderSize = 0
        activityButton.FlatStyle = FlatStyle.Flat
        activityButton.Font = New Font("Segoe UI", 10F)
        activityButton.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        activityButton.Location = New Point(12, 178)
        activityButton.Name = "activityButton"
        activityButton.Size = New Size(205, 42)
        activityButton.TabIndex = 3
        activityButton.Text = "   My activity"
        activityButton.TextAlign = ContentAlignment.MiddleLeft
        activityButton.UseVisualStyleBackColor = False
        ' 
        ' propertiesButton
        ' 
        propertiesButton.BackColor = Color.White
        propertiesButton.FlatAppearance.BorderSize = 0
        propertiesButton.FlatStyle = FlatStyle.Flat
        propertiesButton.Font = New Font("Segoe UI", 10F)
        propertiesButton.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        propertiesButton.Location = New Point(12, 231)
        propertiesButton.Name = "propertiesButton"
        propertiesButton.Size = New Size(205, 42)
        propertiesButton.TabIndex = 4
        propertiesButton.Text = "   My properties"
        propertiesButton.TextAlign = ContentAlignment.MiddleLeft
        propertiesButton.UseVisualStyleBackColor = False
        ' 
        ' listPropertyButton
        ' 
        listPropertyButton.BackColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        listPropertyButton.FlatAppearance.BorderSize = 0
        listPropertyButton.FlatStyle = FlatStyle.Flat
        listPropertyButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        listPropertyButton.ForeColor = Color.White
        listPropertyButton.Location = New Point(18, 300)
        listPropertyButton.Name = "listPropertyButton"
        listPropertyButton.Size = New Size(193, 44)
        listPropertyButton.TabIndex = 5
        listPropertyButton.Text = "+   List a property"
        listPropertyButton.UseVisualStyleBackColor = False
        ' 
        ' profilePanel
        ' 
        profilePanel.BackColor = Color.White
        profilePanel.Controls.Add(accountInitialsLabel)
        profilePanel.Controls.Add(accountNameLabel)
        profilePanel.Controls.Add(signOutButton)
        profilePanel.Dock = DockStyle.Bottom
        profilePanel.Location = New Point(20, 655)
        profilePanel.Name = "profilePanel"
        profilePanel.Size = New Size(210, 76)
        profilePanel.TabIndex = 6
        ' 
        ' accountInitialsLabel
        ' 
        accountInitialsLabel.BackColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        accountInitialsLabel.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        accountInitialsLabel.ForeColor = Color.White
        accountInitialsLabel.Location = New Point(3, 15)
        accountInitialsLabel.Name = "accountInitialsLabel"
        accountInitialsLabel.Size = New Size(38, 38)
        accountInitialsLabel.TabIndex = 0
        accountInitialsLabel.Text = "R"
        accountInitialsLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' accountNameLabel
        ' 
        accountNameLabel.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        accountNameLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        accountNameLabel.Location = New Point(47, 15)
        accountNameLabel.Name = "accountNameLabel"
        accountNameLabel.Size = New Size(150, 20)
        accountNameLabel.TabIndex = 1
        accountNameLabel.Text = "Signed-in user"
        ' 
        ' signOutButton
        ' 
        signOutButton.BackColor = Color.White
        signOutButton.FlatAppearance.BorderSize = 0
        signOutButton.FlatStyle = FlatStyle.Flat
        signOutButton.Font = New Font("Segoe UI", 9F)
        signOutButton.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        signOutButton.Location = New Point(48, 36)
        signOutButton.Name = "signOutButton"
        signOutButton.Size = New Size(80, 25)
        signOutButton.TabIndex = 2
        signOutButton.Text = "Sign out"
        signOutButton.UseVisualStyleBackColor = False
        ' 
        ' contentPanel
        ' 
        contentPanel.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        contentPanel.Controls.Add(screenHostPanel)
        contentPanel.Controls.Add(resultsFlow)
        contentPanel.Controls.Add(resultsHeadingLabel)
        contentPanel.Controls.Add(searchButton)
        contentPanel.Controls.Add(searchInput)
        contentPanel.Controls.Add(purchaseButton)
        contentPanel.Controls.Add(rentButton)
        contentPanel.Controls.Add(subtitleLabel)
        contentPanel.Controls.Add(headingLabel)
        contentPanel.Dock = DockStyle.Fill
        contentPanel.Location = New Point(246, 0)
        contentPanel.Name = "contentPanel"
        contentPanel.Padding = New Padding(30, 22, 30, 22)
        contentPanel.Size = New Size(1124, 749)
        contentPanel.TabIndex = 1
        ' 
        ' screenHostPanel
        ' 
        screenHostPanel.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        screenHostPanel.Dock = DockStyle.Fill
        screenHostPanel.Location = New Point(30, 22)
        screenHostPanel.Name = "screenHostPanel"
        screenHostPanel.Size = New Size(1064, 705)
        screenHostPanel.TabIndex = 8
        screenHostPanel.Visible = False
        ' 
        ' resultsFlow
        ' 
        resultsFlow.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        resultsFlow.AutoScroll = True
        resultsFlow.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        resultsFlow.Location = New Point(30, 238)
        resultsFlow.Name = "resultsFlow"
        resultsFlow.Size = New Size(1064, 489)
        resultsFlow.TabIndex = 7
        ' 
        ' resultsHeadingLabel
        ' 
        resultsHeadingLabel.AutoSize = True
        resultsHeadingLabel.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        resultsHeadingLabel.ForeColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        resultsHeadingLabel.Location = New Point(30, 212)
        resultsHeadingLabel.Name = "resultsHeadingLabel"
        resultsHeadingLabel.Size = New Size(137, 15)
        resultsHeadingLabel.TabIndex = 6
        resultsHeadingLabel.Text = "HANDPICKED FOR YOU"
        ' 
        ' searchButton
        ' 
        searchButton.BackColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        searchButton.FlatAppearance.BorderSize = 0
        searchButton.FlatStyle = FlatStyle.Flat
        searchButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        searchButton.ForeColor = Color.White
        searchButton.Location = New Point(430, 144)
        searchButton.Name = "searchButton"
        searchButton.Size = New Size(130, 42)
        searchButton.TabIndex = 5
        searchButton.Text = "Search homes"
        searchButton.UseVisualStyleBackColor = False
        ' 
        ' searchInput
        ' 
        searchInput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        searchInput.Font = New Font("Segoe UI", 10F)
        searchInput.Location = New Point(30, 152)
        searchInput.Name = "searchInput"
        searchInput.PlaceholderText = "Search by area, home or property type..."
        searchInput.Size = New Size(390, 25)
        searchInput.TabIndex = 4
        ' 
        ' purchaseButton
        ' 
        purchaseButton.BackColor = Color.White
        purchaseButton.FlatAppearance.BorderSize = 0
        purchaseButton.FlatStyle = FlatStyle.Flat
        purchaseButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        purchaseButton.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        purchaseButton.Location = New Point(128, 98)
        purchaseButton.Name = "purchaseButton"
        purchaseButton.Size = New Size(108, 38)
        purchaseButton.TabIndex = 3
        purchaseButton.Text = "Purchase"
        purchaseButton.UseVisualStyleBackColor = False
        ' 
        ' rentButton
        ' 
        rentButton.BackColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        rentButton.FlatAppearance.BorderSize = 0
        rentButton.FlatStyle = FlatStyle.Flat
        rentButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        rentButton.ForeColor = Color.White
        rentButton.Location = New Point(30, 98)
        rentButton.Name = "rentButton"
        rentButton.Size = New Size(92, 38)
        rentButton.TabIndex = 2
        rentButton.Text = "Rent"
        rentButton.UseVisualStyleBackColor = False
        ' 
        ' subtitleLabel
        ' 
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 10F)
        subtitleLabel.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        subtitleLabel.Location = New Point(31, 62)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(353, 19)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Browse thoughtful places to rent or buy, all in one place."
        ' 
        ' headingLabel
        ' 
        headingLabel.AutoSize = True
        headingLabel.Font = New Font("Segoe UI", 25F, FontStyle.Bold)
        headingLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        headingLabel.Location = New Point(30, 22)
        headingLabel.Name = "headingLabel"
        headingLabel.Size = New Size(352, 46)
        headingLabel.TabIndex = 0
        headingLabel.Text = "Find your next home"
        ' 
        ' RentLinkHomeControl
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        Controls.Add(contentPanel)
        Controls.Add(sidebarPanel)
        Name = "RentLinkHomeControl"
        Size = New Size(1370, 749)
        sidebarPanel.ResumeLayout(False)
        sidebarPanel.PerformLayout()
        profilePanel.ResumeLayout(False)
        contentPanel.ResumeLayout(False)
        contentPanel.PerformLayout()
        ResumeLayout(False)
    End Sub
End Class
