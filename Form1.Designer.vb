<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents RentLinkWorkspace1 As RentLinkWorkspace

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        RentLinkWorkspace1 = New RentLinkWorkspace()
        SuspendLayout()
        ' 
        ' RentLinkWorkspace1
        ' 
        RentLinkWorkspace1.BackColor = Color.FromArgb(CByte(248), CByte(250), CByte(252))
        RentLinkWorkspace1.Dock = DockStyle.Fill
        RentLinkWorkspace1.Location = New Point(0, 0)
        RentLinkWorkspace1.Name = "RentLinkWorkspace1"
        RentLinkWorkspace1.PreviewScreen = RentLinkPreviewScreen.SignIn
        RentLinkWorkspace1.Size = New Size(1370, 749)
        RentLinkWorkspace1.TabIndex = 0
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1370, 749)
        Controls.Add(RentLinkWorkspace1)
        Name = "Form1"
        Text = "RentLink | Find your next home"
        ResumeLayout(False)
    End Sub

End Class
