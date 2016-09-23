<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Metero
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
        Me.AppBox = New System.Windows.Forms.ListBox()
        Me.RefreshBtn = New System.Windows.Forms.Button()
        Me.BackBtn = New System.Windows.Forms.Button()
        Me.UninstallBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'AppBox
        '
        Me.AppBox.FormattingEnabled = True
        Me.AppBox.ItemHeight = 16
        Me.AppBox.Location = New System.Drawing.Point(12, 12)
        Me.AppBox.Name = "AppBox"
        Me.AppBox.Size = New System.Drawing.Size(405, 196)
        Me.AppBox.TabIndex = 1
        '
        'RefreshBtn
        '
        Me.RefreshBtn.Location = New System.Drawing.Point(342, 214)
        Me.RefreshBtn.Name = "RefreshBtn"
        Me.RefreshBtn.Size = New System.Drawing.Size(75, 27)
        Me.RefreshBtn.TabIndex = 2
        Me.RefreshBtn.Text = "Refresh"
        Me.RefreshBtn.UseVisualStyleBackColor = True
        '
        'BackBtn
        '
        Me.BackBtn.Location = New System.Drawing.Point(261, 214)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(75, 27)
        Me.BackBtn.TabIndex = 3
        Me.BackBtn.Text = "Go back"
        Me.BackBtn.UseVisualStyleBackColor = True
        '
        'UninstallBtn
        '
        Me.UninstallBtn.Location = New System.Drawing.Point(12, 214)
        Me.UninstallBtn.Name = "UninstallBtn"
        Me.UninstallBtn.Size = New System.Drawing.Size(75, 27)
        Me.UninstallBtn.TabIndex = 4
        Me.UninstallBtn.Text = "Uninstall"
        Me.UninstallBtn.UseVisualStyleBackColor = True
        '
        'Metero
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(429, 253)
        Me.Controls.Add(Me.UninstallBtn)
        Me.Controls.Add(Me.BackBtn)
        Me.Controls.Add(Me.RefreshBtn)
        Me.Controls.Add(Me.AppBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Metero"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Handle Win10 / metero apps"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AppBox As ListBox
    Friend WithEvents RefreshBtn As Button
    Friend WithEvents BackBtn As Button
    Friend WithEvents UninstallBtn As Button
End Class
