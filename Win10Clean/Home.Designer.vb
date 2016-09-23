<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Home
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DelLibBtn = New System.Windows.Forms.Button()
        Me.OneDriveBtn = New System.Windows.Forms.Button()
        Me.DefenderBtn = New System.Windows.Forms.Button()
        Me.MeteroBtn = New System.Windows.Forms.Button()
        Me.HomeGroupBtn = New System.Windows.Forms.Button()
        Me.CloseBtn = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'DelLibBtn
        '
        Me.DelLibBtn.Location = New System.Drawing.Point(12, 144)
        Me.DelLibBtn.Name = "DelLibBtn"
        Me.DelLibBtn.Size = New System.Drawing.Size(221, 60)
        Me.DelLibBtn.TabIndex = 0
        Me.DelLibBtn.Text = "Remove libary folders in My PC"
        Me.DelLibBtn.UseVisualStyleBackColor = True
        '
        'OneDriveBtn
        '
        Me.OneDriveBtn.Location = New System.Drawing.Point(12, 210)
        Me.OneDriveBtn.Name = "OneDriveBtn"
        Me.OneDriveBtn.Size = New System.Drawing.Size(221, 32)
        Me.OneDriveBtn.TabIndex = 1
        Me.OneDriveBtn.Text = "Uninstall OneDrive"
        Me.OneDriveBtn.UseVisualStyleBackColor = True
        '
        'DefenderBtn
        '
        Me.DefenderBtn.Location = New System.Drawing.Point(12, 12)
        Me.DefenderBtn.Name = "DefenderBtn"
        Me.DefenderBtn.Size = New System.Drawing.Size(221, 60)
        Me.DefenderBtn.TabIndex = 2
        Me.DefenderBtn.Text = "Disable Windows Defender"
        Me.DefenderBtn.UseVisualStyleBackColor = True
        '
        'MeteroBtn
        '
        Me.MeteroBtn.Location = New System.Drawing.Point(12, 78)
        Me.MeteroBtn.Name = "MeteroBtn"
        Me.MeteroBtn.Size = New System.Drawing.Size(221, 60)
        Me.MeteroBtn.TabIndex = 3
        Me.MeteroBtn.Text = "Uninstall Metero apps"
        Me.MeteroBtn.UseVisualStyleBackColor = True
        '
        'HomeGroupBtn
        '
        Me.HomeGroupBtn.Location = New System.Drawing.Point(12, 248)
        Me.HomeGroupBtn.Name = "HomeGroupBtn"
        Me.HomeGroupBtn.Size = New System.Drawing.Size(221, 32)
        Me.HomeGroupBtn.TabIndex = 4
        Me.HomeGroupBtn.Text = "Disable HomeGroup"
        Me.HomeGroupBtn.UseVisualStyleBackColor = True
        '
        'CloseBtn
        '
        Me.CloseBtn.Location = New System.Drawing.Point(12, 286)
        Me.CloseBtn.Name = "CloseBtn"
        Me.CloseBtn.Size = New System.Drawing.Size(105, 31)
        Me.CloseBtn.TabIndex = 5
        Me.CloseBtn.Text = "Close"
        Me.CloseBtn.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(123, 286)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(110, 31)
        Me.Button4.TabIndex = 6
        Me.Button4.Text = "Undo"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Home
        '
        Me.AcceptButton = Me.CloseBtn
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(245, 329)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.CloseBtn)
        Me.Controls.Add(Me.HomeGroupBtn)
        Me.Controls.Add(Me.MeteroBtn)
        Me.Controls.Add(Me.DefenderBtn)
        Me.Controls.Add(Me.OneDriveBtn)
        Me.Controls.Add(Me.DelLibBtn)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MinimumSize = New System.Drawing.Size(262, 196)
        Me.Name = "Home"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Win10Clean"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DelLibBtn As Button
    Friend WithEvents OneDriveBtn As Button
    Friend WithEvents DefenderBtn As Button
    Friend WithEvents MeteroBtn As Button
    Friend WithEvents HomeGroupBtn As Button
    Friend WithEvents CloseBtn As Button
    Friend WithEvents Button4 As Button
End Class
