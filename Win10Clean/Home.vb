Imports System.IO
Imports System.Management.Automation
Imports Microsoft.Win32

Public Class Home

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

    Dim OfflineVer As String = My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString + "." + My.Application.Info.Version.Build.ToString

    Private Sub CloseBtn_Click(sender As Object, e As EventArgs) Handles CloseBtn.Click
        Application.Exit()
    End Sub

    Private Sub MeteroBtn_Click(sender As Object, e As EventArgs) Handles MeteroBtn.Click
        Metero.Show()
        Close()
    End Sub

    Private Sub DelLibBtn_Click(sender As Object, e As EventArgs) Handles DelLibBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                HideLibs()
        End Select

        Enabled = True
    End Sub

    Private Sub OneDriveBtn_Click(sender As Object, e As EventArgs) Handles OneDriveBtn.Click

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                UninstallOneDrive()
                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
    End Sub

    Private Sub AboutBtn_Click(sender As Object, e As EventArgs) Handles AboutBtn.Click
        'MessageBox.Show("Made by Hawaii_Beach, version " + OfflineVer + ", the project is hosted on GitHub. Licensed under GPLv3", "About", MessageBoxButtons.OK)
        About.Show()
        Enabled = False
    End Sub

    Private Sub GameDVRBtn_Click(sender As Object, e As EventArgs) Handles GameDVRBtn.Click
        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                Enabled = False
                Dim RegKey As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"
                Try
                    Static Key As RegistryKey
                    Dim RegVal() As String = {"0x00000000", 0}
                    Console.WriteLine("key='" + RegKey + "',val='" + RegVal(0) + "',type='" + RegistryValueKind.DWord.ToString() + "'")
                    Key = Registry.CurrentUser.OpenSubKey(RegKey, True)

                    Key.SetValue("AppCaptureEnabled", RegVal(1), RegistryValueKind.DWord)
                    Key.Close()
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As NullReferenceException
                    Registry.CurrentUser.CreateSubKey(RegKey)
                    MessageBox.Show("Please try again, I may have solved a possible issue.")

                Catch ex As Exception
                    MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                Enabled = True
        End Select
    End Sub

    Private Sub DefenderBtn_Click(sender As Object, e As EventArgs) Handles DefenderBtn.Click
        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                Enabled = False
                Try
                    Static Key As RegistryKey
                    Dim RegKey As String = "SOFTWARE\Policies\Microsoft\Windows Defender"
                    Dim RegVal() As String = {"0x00000001", 1}
                    Key = Registry.LocalMachine.OpenSubKey(RegKey, True)

                    Key.SetValue("DisableAntiSpyware", RegVal(1), RegistryValueKind.DWord)
                    Key.Close()
                    Console.WriteLine("key='" + RegKey + "',val='" + RegVal(0) + "',type='" + RegistryValueKind.DWord.ToString() + "'")
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                Enabled = True
        End Select
    End Sub

    Private Sub HomeGroupBtn_Click(sender As Object, e As EventArgs) Handles HomeGroupBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes

                ' run cmd commands via powershell, smart ha?
                Using PowerScript As PowerShell = PowerShell.Create()
                    PowerScript.AddScript("cmd /c ""sc config ""HomeGroupProvider"" start= disabled""")
                    PowerScript.AddScript("cmd /c ""sc stop ""HomeGroupProvider""""")
                    PowerScript.Invoke()
                End Using
                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select

        Enabled = True
    End Sub

    Private Sub UninstallOneDrive()
        Dim ProcessName As String = "OneDrive"
        Try

            Process.GetProcessesByName(ProcessName)(0).Kill()
        Catch ex As Exception
            Console.WriteLine("Could not kill process:  " + ProcessName)
            ' ignore errors
        End Try

        Dim OnePath As String = Nothing
        If Environment.Is64BitOperatingSystem = True Then
            OnePath = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\OneDriveSetup.exe"
        Else
            OnePath = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\OneDriveSetup.exe"
        End If
        Process.Start(OnePath, "/uninstall")

        ' All the folders to be deleted
        Dim OnePaths() As String =
        {
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\OneDrive",
            Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "OneDriveTemp",
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\Microsoft\OneDrive",
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\Microsoft OneDrive"
        }

        For Each dir As String In OnePaths
            If (Directory.Exists(dir)) Then
                Try
                    Directory.Delete(dir, True)
                Catch ex As Exception
                    ' ignore errors
                End Try
            End If
        Next

        Dim OneKeys() As String =
        {
            "CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}",
            "Wow6432Node\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}"
        }
        For Each keys As String In OneKeys
            Try
                Registry.ClassesRoot.DeleteSubKeyTree(keys)
            Catch ex As Exception
                ' ignore errors
            End Try

        Next
    End Sub

    Private Sub HideLibs()
        Static LibReg As RegistryKey
        Dim LibVal As String = "Hide"
        Static LibKey As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions\"
        Dim LibGUID() As String = {"{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", "{7d83ee9b-2244-4e70-b1f5-5393042af1e4}", "{f42ee2d3-909f-4907-8871-4c22fc0bf756}", "{0ddd015d-b06c-45d5-8c4c-f59713854639}", "{a0c69a99-21c8-4671-8703-7934162fcf1d}", "{35286a68-3c57-41a1-bbb1-0eae73d76c95}"}
        For Each key As String In LibGUID
            Try
                Dim FinalKey = LibKey + key + "\PropertyBag"
                LibReg = Registry.LocalMachine.OpenSubKey(FinalKey, True)

                LibReg.SetValue("ThisPCPolicy", LibVal)
                LibReg.Close()
                Console.WriteLine("key='" + FinalKey + "',val='" + LibVal + "',type='" + RegistryValueKind.String.ToString + "'")
            Catch ex As Exception
                MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        Next

        MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub
End Class
