<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class RentLinkAuthenticationControl
    Inherits System.Windows.Forms.UserControl

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer
    Friend WithEvents heroPanel As System.Windows.Forms.Panel
    Friend WithEvents heroBrandLabel As System.Windows.Forms.Label
    Friend WithEvents heroHeadlineLabel As System.Windows.Forms.Label
    Friend WithEvents heroIntroLabel As System.Windows.Forms.Label
    Friend WithEvents heroNoteLabel As System.Windows.Forms.Label
    Friend WithEvents formAreaPanel As System.Windows.Forms.Panel
    Friend WithEvents authCard As System.Windows.Forms.Panel
    Friend WithEvents overlineLabel As System.Windows.Forms.Label
    Friend WithEvents headingLabel As System.Windows.Forms.Label
    Friend WithEvents subheadingLabel As System.Windows.Forms.Label
    Friend WithEvents nameLabel As System.Windows.Forms.Label
    Friend WithEvents nameInputField As System.Windows.Forms.TextBox
    Friend WithEvents emailLabel As System.Windows.Forms.Label
    Friend WithEvents emailInputField As System.Windows.Forms.TextBox
    Friend WithEvents passwordLabel As System.Windows.Forms.Label
    Friend WithEvents passwordInputField As System.Windows.Forms.TextBox
    Friend WithEvents actionButton As System.Windows.Forms.Button
    Friend WithEvents toggleButton As System.Windows.Forms.Button

    Private Sub InitializeComponent()
        heroPanel = New Panel()
        heroBrandLabel = New Label()
        heroHeadlineLabel = New Label()
        heroIntroLabel = New Label()
        heroNoteLabel = New Label()
        formAreaPanel = New Panel()
        authCard = New Panel()
        overlineLabel = New Label()
        headingLabel = New Label()
        subheadingLabel = New Label()
        nameLabel = New Label()
        nameInputField = New TextBox()
        emailLabel = New Label()
        emailInputField = New TextBox()
        passwordLabel = New Label()
        passwordInputField = New TextBox()
        actionButton = New Button()
        toggleButton = New Button()
        heroPanel.SuspendLayout()
        formAreaPanel.SuspendLayout()
        authCard.SuspendLayout()
        SuspendLayout()
        ' 
        ' heroPanel
        ' 
        heroPanel.BackColor = Color.FromArgb(CByte(15), CByte(23), CByte(42))
        heroPanel.Controls.Add(heroBrandLabel)
        heroPanel.Controls.Add(heroHeadlineLabel)
        heroPanel.Controls.Add(heroIntroLabel)
        heroPanel.Controls.Add(heroNoteLabel)
        heroPanel.Dock = DockStyle.Left
        heroPanel.Location = New Point(0, 0)
        heroPanel.Name = "heroPanel"
        heroPanel.Padding = New Padding(54)
        heroPanel.Size = New Size(430, 749)
        heroPanel.TabIndex = 0
        ' 
        ' heroBrandLabel
        ' 
        heroBrandLabel.AutoSize = True
        heroBrandLabel.Font = New Font("Segoe UI", 15.0F, FontStyle.Bold)
        heroBrandLabel.ForeColor = Color.White
        heroBrandLabel.Location = New Point(54, 58)
        heroBrandLabel.Name = "heroBrandLabel"
        heroBrandLabel.Size = New Size(109, 28)
        heroBrandLabel.TabIndex = 0
        heroBrandLabel.Text = "RENTLINK"
        ' 
        ' heroHeadlineLabel
        ' 
        heroHeadlineLabel.Font = New Font("Segoe UI", 30.0F, FontStyle.Bold)
        heroHeadlineLabel.ForeColor = Color.White
        heroHeadlineLabel.Location = New Point(54, 184)
        heroHeadlineLabel.Name = "heroHeadlineLabel"
        heroHeadlineLabel.Size = New Size(320, 100)
        heroHeadlineLabel.TabIndex = 1
        heroHeadlineLabel.Text = "Find a place" & vbCrLf & "to call yours."
        ' 
        ' heroIntroLabel
        ' 
        heroIntroLabel.Font = New Font("Segoe UI", 13.0F)
        heroIntroLabel.ForeColor = Color.FromArgb(CByte(203), CByte(213), CByte(225))
        heroIntroLabel.Location = New Point(54, 312)
        heroIntroLabel.Name = "heroIntroLabel"
        heroIntroLabel.Size = New Size(350, 60)
        heroIntroLabel.TabIndex = 2
        heroIntroLabel.Text = "Thoughtful homes. Trusted connections. A simpler way to rent or buy."
        ' 
        ' heroNoteLabel
        ' 
        heroNoteLabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        heroNoteLabel.AutoSize = True
        heroNoteLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        heroNoteLabel.ForeColor = Color.FromArgb(CByte(94), CByte(234), CByte(212))
        heroNoteLabel.Location = New Point(54, 657)
        heroNoteLabel.Name = "heroNoteLabel"
        heroNoteLabel.Size = New Size(206, 15)
        heroNoteLabel.TabIndex = 3
        heroNoteLabel.Text = "YOUR NEXT CHAPTER STARTS HERE"
        ' 
        ' formAreaPanel
        ' 
        formAreaPanel.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        formAreaPanel.Controls.Add(authCard)
        formAreaPanel.Dock = DockStyle.Fill
        formAreaPanel.Location = New Point(430, 0)
        formAreaPanel.Name = "formAreaPanel"
        formAreaPanel.Size = New Size(940, 749)
        formAreaPanel.TabIndex = 1
        ' 
        ' authCard
        ' 
        authCard.BackColor = Color.White
        authCard.Controls.Add(overlineLabel)
        authCard.Controls.Add(headingLabel)
        authCard.Controls.Add(subheadingLabel)
        authCard.Controls.Add(nameLabel)
        authCard.Controls.Add(nameInputField)
        authCard.Controls.Add(emailLabel)
        authCard.Controls.Add(emailInputField)
        authCard.Controls.Add(passwordLabel)
        authCard.Controls.Add(passwordInputField)
        authCard.Controls.Add(actionButton)
        authCard.Controls.Add(toggleButton)
        authCard.Location = New Point(238, 57)
        authCard.Name = "authCard"
        authCard.Size = New Size(464, 634)
        authCard.TabIndex = 0
        ' 
        ' overlineLabel
        ' 
        overlineLabel.AutoSize = True
        overlineLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        overlineLabel.ForeColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        overlineLabel.Location = New Point(21, 51)
        overlineLabel.Name = "overlineLabel"
        overlineLabel.Size = New Size(169, 19)
        overlineLabel.TabIndex = 0
        overlineLabel.Text = "WELCOME TO RENTLINK"
        ' 
        ' headingLabel
        ' 
        headingLabel.AutoSize = True
        headingLabel.Font = New Font("Segoe UI", 29.0F, FontStyle.Bold)
        headingLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        headingLabel.Location = New Point(21, 79)
        headingLabel.Name = "headingLabel"
        headingLabel.Size = New Size(289, 52)
        headingLabel.TabIndex = 1
        headingLabel.Text = "Welcome back"
        ' 
        ' subheadingLabel
        ' 
        subheadingLabel.AutoSize = True
        subheadingLabel.Font = New Font("Segoe UI", 11.0F)
        subheadingLabel.ForeColor = Color.FromArgb(CByte(100), CByte(116), CByte(139))
        subheadingLabel.Location = New Point(21, 126)
        subheadingLabel.Name = "subheadingLabel"
        subheadingLabel.Size = New Size(248, 20)
        subheadingLabel.TabIndex = 2
        subheadingLabel.Text = "Sign in to pick up where you left off."
        ' 
        ' nameLabel
        ' 
        nameLabel.AutoSize = True
        nameLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        nameLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        nameLabel.Location = New Point(21, 186)
        nameLabel.Name = "nameLabel"
        nameLabel.Size = New Size(81, 19)
        nameLabel.TabIndex = 3
        nameLabel.Text = "Your name"
        ' 
        ' nameInputField
        ' 
        nameInputField.Font = New Font("Segoe UI", 11.0F)
        nameInputField.Location = New Point(21, 211)
        nameInputField.Name = "nameInputField"
        nameInputField.PlaceholderText = "e.g. Alex Morgan"
        nameInputField.Size = New Size(420, 27)
        nameInputField.TabIndex = 4
        ' 
        ' emailLabel
        ' 
        emailLabel.AutoSize = True
        emailLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        emailLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        emailLabel.Location = New Point(21, 272)
        emailLabel.Name = "emailLabel"
        emailLabel.Size = New Size(192, 19)
        emailLabel.TabIndex = 5
        emailLabel.Text = "Username or email address"
        ' 
        ' emailInputField
        ' 
        emailInputField.Font = New Font("Segoe UI", 11.0F)
        emailInputField.Location = New Point(21, 297)
        emailInputField.Name = "emailInputField"
        emailInputField.PlaceholderText = "Username or email"
        emailInputField.Size = New Size(420, 27)
        emailInputField.TabIndex = 6
        ' 
        ' passwordLabel
        ' 
        passwordLabel.AutoSize = True
        passwordLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        passwordLabel.ForeColor = Color.FromArgb(CByte(30), CByte(41), CByte(59))
        passwordLabel.Location = New Point(21, 358)
        passwordLabel.Name = "passwordLabel"
        passwordLabel.Size = New Size(73, 19)
        passwordLabel.TabIndex = 7
        passwordLabel.Text = "Password"
        ' 
        ' passwordInputField
        ' 
        passwordInputField.Font = New Font("Segoe UI", 11.0F)
        passwordInputField.Location = New Point(21, 383)
        passwordInputField.Name = "passwordInputField"
        passwordInputField.PlaceholderText = "Enter your password"
        passwordInputField.Size = New Size(420, 27)
        passwordInputField.TabIndex = 8
        passwordInputField.UseSystemPasswordChar = True
        ' 
        ' actionButton
        ' 
        actionButton.BackColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        actionButton.FlatAppearance.BorderSize = 0
        actionButton.FlatStyle = FlatStyle.Flat
        actionButton.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        actionButton.ForeColor = Color.White
        actionButton.Location = New Point(21, 459)
        actionButton.Name = "actionButton"
        actionButton.Size = New Size(420, 50)
        actionButton.TabIndex = 9
        actionButton.Text = "Sign in"
        actionButton.UseVisualStyleBackColor = False
        ' 
        ' toggleButton
        ' 
        toggleButton.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        toggleButton.FlatAppearance.BorderSize = 0
        toggleButton.FlatStyle = FlatStyle.Flat
        toggleButton.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        toggleButton.ForeColor = Color.FromArgb(CByte(13), CByte(148), CByte(136))
        toggleButton.Location = New Point(21, 521)
        toggleButton.Name = "toggleButton"
        toggleButton.Size = New Size(420, 42)
        toggleButton.TabIndex = 10
        toggleButton.Text = "New to RentLink?  Create an account"
        toggleButton.UseVisualStyleBackColor = False
        ' 
        ' RentLinkAuthenticationControl
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        Controls.Add(formAreaPanel)
        Controls.Add(heroPanel)
        Name = "RentLinkAuthenticationControl"
        Size = New Size(1370, 749)
        heroPanel.ResumeLayout(False)
        heroPanel.PerformLayout()
        formAreaPanel.ResumeLayout(False)
        authCard.ResumeLayout(False)
        authCard.PerformLayout()
        ResumeLayout(False)
    End Sub
End Class
