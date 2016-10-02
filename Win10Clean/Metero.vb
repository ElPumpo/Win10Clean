Imports System.Management.Automation

Public Class Metero

    Dim GoBack As Integer

    'Win10Clean - Cleanup your Windows 10 environment
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

    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Home.Show()
        Close()
    End Sub

    Private Sub Metero_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GoBack = Nothing
        Enabled = False
        FindApps()
        Enabled = True
    End Sub

    Private Sub RefreshBtn_Click(sender As Object, e As EventArgs) Handles RefreshBtn.Click
        Enabled = False
        RefreshList(False)
        Enabled = True
    End Sub

    Private Sub UninstallBtn_Click(sender As Object, e As EventArgs) Handles UninstallBtn.Click
        Enabled = False
        If Not AppBox.SelectedItem = Nothing Then
            Select Case MsgBox("Are you sure you want to uninstall " + AppBox.SelectedItem + "?", MsgBoxStyle.YesNo)
                Case MsgBoxResult.Yes
                    UninstallApp(AppBox.SelectedItem)
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Select
        Else
            MsgBox("Please select a app!", MsgBoxStyle.Exclamation)
        End If
        Enabled = True
    End Sub

    Private Sub RefreshList(MinusOne As Boolean)
        ' Leads to higher memory usage over time
        GoBack = AppBox.SelectedIndex ' Store where the user was
        AppBox.Items.Clear()
        FindApps()

        ' Go back to where the user was before refresh,
        ' If the app is uninstalled, we want to get the last item or else the application will flip
        If (MinusOne) Then
            AppBox.SelectedIndex = GoBack - 1
        Else
            AppBox.SelectedIndex = GoBack
        End If

    End Sub

    Private Sub UninstallApp(AppName As String)
        Using PowerScript As PowerShell = PowerShell.Create()
            PowerScript.AddScript("Get-AppxPackage " + AppName + " | Remove-AppxPackage")
            PowerScript.Invoke()
        End Using
        RefreshList(True)
    End Sub

    Private Sub FindApps()
        Using PowerScript As PowerShell = PowerShell.Create()
            PowerScript.AddScript("Get-AppxPackage | Select Name | Out-String -Stream")

            ' Cleanup output and do not include weird stuff
            Dim TrimmedString As String = Nothing
            For Each line As PSObject In PowerScript.Invoke()
                TrimmedString = line.ToString.Trim()
                If Not TrimmedString Is String.Empty AndAlso Not TrimmedString.Contains("---") Then
                    If Not TrimmedString = "Name" Then
                        AppBox.Items.Add(TrimmedString)
                    End If

                End If
            Next
        End Using
    End Sub
End Class