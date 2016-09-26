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
        Me.components = New System.ComponentModel.Container()
        Me.AppBox = New System.Windows.Forms.ListBox()
        Me.BackBtn = New System.Windows.Forms.Button()
        Me.UninstallBtn = New System.Windows.Forms.Button()
        Me.RefreshBtn = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
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
        'BackBtn
        '
        Me.BackBtn.Image = Global.Win10Clean.My.Resources.Resources.arrow_left
        Me.BackBtn.Location = New System.Drawing.Point(12, 214)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(34, 34)
        Me.BackBtn.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.BackBtn, "Go back")
        Me.BackBtn.UseVisualStyleBackColor = True
        '
        'UninstallBtn
        '
        Me.UninstallBtn.Image = Global.Win10Clean.My.Resources.Resources.bin
        Me.UninstallBtn.Location = New System.Drawing.Point(383, 214)
        Me.UninstallBtn.Name = "UninstallBtn"
        Me.UninstallBtn.Size = New System.Drawing.Size(34, 34)
        Me.UninstallBtn.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.UninstallBtn, "Uninstall selected app")
        Me.UninstallBtn.UseVisualStyleBackColor = True
        '
        'RefreshBtn
        '
        Me.RefreshBtn.Image = Global.Win10Clean.My.Resources.Resources.arrow_refresh
        Me.RefreshBtn.Location = New System.Drawing.Point(343, 214)
        Me.RefreshBtn.Name = "RefreshBtn"
        Me.RefreshBtn.Size = New System.Drawing.Size(34, 34)
        Me.RefreshBtn.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.RefreshBtn, "Refresh")
        Me.RefreshBtn.UseVisualStyleBackColor = True
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
    Friend WithEvents ToolTip1 As ToolTip
End Class
