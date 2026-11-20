Imports System.ComponentModel

Public Class Form1
    Private previewScreenValue As RentLinkPreviewScreen = RentLinkPreviewScreen.SignIn

    <Category("RentLink"), Description("Choose which RentLink screen to show in the designer preview."), DefaultValue(GetType(RentLinkPreviewScreen), "SignIn")>
    Public Property PreviewScreen As RentLinkPreviewScreen
        Get
            Return previewScreenValue
        End Get
        Set(value As RentLinkPreviewScreen)
            previewScreenValue = value
            If RentLinkWorkspace1 IsNot Nothing Then RentLinkWorkspace1.PreviewScreen = value
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
        Text = "RentLink | Find your next home"
        StartPosition = FormStartPosition.CenterScreen
        WindowState = FormWindowState.Maximized
        MinimumSize = New Size(1100, 720)
    End Sub

    Private Sub RentLinkWorkspace1_Load(sender As Object, e As EventArgs) Handles RentLinkWorkspace1.Load

    End Sub
End Class
