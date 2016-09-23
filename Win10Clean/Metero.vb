Imports System.Management.Automation

Public Class Metero

    'Win10Clean - Cleanup your Windows 10 enviroment
    'Copyright (C) 2016 Hawaii_Beach

    'This program Is free software: you can redistribute it And/Or modify
    'it under the terms Of the GNU General Public License As published by
    'the Free Software Foundation, either version 3 Of the License, Or
    '(at your option) any later version.

    'This program Is distributed In the hope that it will be useful,
    'but WITHOUT ANY WARRANTY; without even the implied warranty Of
    'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
    'GNU General Public License For more details.

    'You should have received a copy Of the GNU General Public License
    'along with this program.  If Not, see <http://www.gnu.org/licenses/>.

    Dim AppList As New List(Of String)

    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Home.Show()
        Close()
    End Sub

    Private Sub Metero_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Enabled = False
        FindApps()
        Enabled = True
    End Sub

    Private Sub RefreshBtn_Click(sender As Object, e As EventArgs) Handles RefreshBtn.Click
        Enabled = False
        RefreshList()
        Enabled = True
    End Sub

    Private Sub UninstallBtn_Click(sender As Object, e As EventArgs) Handles UninstallBtn.Click
        Enabled = False
        If Not AppBox.SelectedItem = Nothing Then
            Select Case MsgBox("Are you sure you want to uninstall " + AppBox.SelectedItem + "?", MsgBoxStyle.YesNo)
                Case MsgBoxResult.Yes
                    UninstallApp(AppBox.SelectedItem)
            End Select
        End If
        Enabled = True
    End Sub

    Private Sub RefreshList()
        AppBox.Items.Clear()
        FindApps()
    End Sub

    Private Sub UninstallApp(AppName As String)
        Using PowerScript As PowerShell = PowerShell.Create()
            PowerScript.AddScript("Get-AppxPackage " + AppBox.SelectedItem.ToString + "| Remove-AppxPackage")
            PowerScript.Invoke()
        End Using
        RefreshList()
    End Sub

    Private Sub FindApps()
        Using PowerScript As PowerShell = PowerShell.Create()
            PowerScript.AddScript("Get-AppxPackage | Select Name | Out-String -stream")

            ' Cleanup output and do not include weird stuff
            Dim cleanStr As String = Nothing
            For Each line As PSObject In PowerScript.Invoke()
                cleanStr = line.ToString.Trim()
                If Not cleanStr Is String.Empty Then
                    If Not cleanStr.Contains("---") Then
                        If Not cleanStr = "Name" Then
                            AppBox.Items.Add(cleanStr)
                        End If

                    End If
                End If
            Next
        End Using
    End Sub
End Class