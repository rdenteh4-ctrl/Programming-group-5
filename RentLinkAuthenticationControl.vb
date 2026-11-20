Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Partial Public Class RentLinkAuthenticationControl
    Inherits UserControl

    Private isRegisterValue As Boolean

    Public Event SubmitRequested As EventHandler
    Public Event ToggleRequested As EventHandler

    Public Sub New()
        InitializeComponent()
        BackColor = RentLinkRoundedCorners.PageColor
        RentLinkRoundedCorners.ApplyRoundedBorder(authCard, Color.FromArgb(220, 228, 237), 18)
        AddHandler formAreaPanel.Resize, AddressOf CenterAuthCard
        AddHandler Resize, AddressOf CenterAuthCard
        UpdateMode()
        CenterAuthCard(Me, EventArgs.Empty)
    End Sub

    Private Sub CenterAuthCard(sender As Object, e As EventArgs)
        authCard.Location = New Point(Math.Max(20, (formAreaPanel.ClientSize.Width - authCard.Width) \ 2), Math.Max(20, (formAreaPanel.ClientSize.Height - authCard.Height) \ 2))
    End Sub

    <Browsable(False)>
    Public ReadOnly Property NameInput As TextBox
        Get
            Return nameInputField
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property EmailInput As TextBox
        Get
            Return emailInputField
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property PasswordInput As TextBox
        Get
            Return passwordInputField
        End Get
    End Property

    <Category("RentLink"), Description("Show the registration layout instead of the sign-in layout."), DefaultValue(False)>
    Public Property IsRegister As Boolean
        Get
            Return isRegisterValue
        End Get
        Set(value As Boolean)
            isRegisterValue = value
            UpdateMode()
        End Set
    End Property

    Private Sub SubmitButtonClicked(sender As Object, e As EventArgs) Handles actionButton.Click
        RaiseEvent SubmitRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub ToggleButtonClicked(sender As Object, e As EventArgs) Handles toggleButton.Click
        RaiseEvent ToggleRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub UpdateMode()
        If nameLabel Is Nothing Then Return
        headingLabel.Text = If(isRegisterValue, "Create your account", "Welcome back")
        subheadingLabel.Text = If(isRegisterValue, "Join a better way to find your next home.", "Sign in to pick up where you left off.")
        nameLabel.Visible = isRegisterValue
        nameInputField.Visible = isRegisterValue
        emailLabel.Text = "Email address"
        emailInputField.PlaceholderText = "you@example.com"
        actionButton.Text = If(isRegisterValue, "Create account", "Sign in")
        toggleButton.Text = If(isRegisterValue, "Already have an account?  Sign in", "New to RentLink?  Create an account")

        If isRegisterValue Then
            nameLabel.Location = New Point(21, 165)
            nameInputField.Location = New Point(21, 190)
            emailLabel.Location = New Point(21, 251)
            emailInputField.Location = New Point(21, 276)
            passwordLabel.Location = New Point(21, 337)
            passwordInputField.Location = New Point(21, 362)
            actionButton.Location = New Point(21, 438)
            toggleButton.Location = New Point(21, 500)
        Else
            emailLabel.Location = New Point(21, 165)
            emailInputField.Location = New Point(21, 190)
            passwordLabel.Location = New Point(21, 251)
            passwordInputField.Location = New Point(21, 276)
            actionButton.Location = New Point(21, 352)
            toggleButton.Location = New Point(21, 414)
        End If
    End Sub
End Class
