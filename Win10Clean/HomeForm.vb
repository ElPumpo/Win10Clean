Imports System.IO
Imports System.Management.Automation
Imports System.Net
Imports Microsoft.Win32

Public Class HomeForm

    'Win10Clean - Cleanup your Windows 10 environment
    'Copyright (C) 2016-2017 Hawaii_Beach

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

    Public OfflineVer As String = My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString + "." + My.Application.Info.Version.Build.ToString
    Dim OnlineVer As String = "Unknown"
    Dim ServerURL As String = "http://raw.githubusercontent.com/ElPumpo/Win10Clean/master/Win10Clean/Resources/version"
    Dim Is64 As Boolean = Environment.Is64BitOperatingSystem
    Dim GoBack As Integer
    Dim TheApps As String = Nothing

    ' States
    Dim AdsSwitch As Integer = 0
    Dim AdsMessage As String = "Disabled ads on start menu!"

    Private Sub HomeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        VerLabel.Text = VerLabel.Text + OfflineVer

        Try
            ' Check ads
            Using Key As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", True)
                Select Case Key.GetValue("SystemPaneSuggestionsEnabled", 1)
                    Case 0
                        AdsSwitch = 1
                        AdsBtn.Text = "Enable start menu ads"
                        ToolTip1.SetToolTip(AdsBtn, "Re-enable the ads")
                        AdsMessage = "Enabled ads on start menu!"
                End Select
            End Using

        Catch ex As Exception
            ' nothing
        End Try


    End Sub

    ' Home related
    Private Sub CloseBtn_Click(sender As Object, e As EventArgs) Handles CloseBtn.Click
        Application.Exit()
    End Sub

    Private Sub Revert7Btn_Click(sender As Object, e As EventArgs) Handles Revert7Btn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                Revert7()
                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select

        Enabled = True
    End Sub

    Private Sub OneDriveBtn_Click(sender As Object, e As EventArgs) Handles OneDriveBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                UninstallOneDrive()
                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select

        Enabled = True
    End Sub

    Private Sub GameDVRBtn_Click(sender As Object, e As EventArgs) Handles GameDVRBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                Dim RegKey As String = "Software\Microsoft\Windows\CurrentVersion\GameDVR"
                Try
                    Using Key As RegistryKey = Registry.CurrentUser.OpenSubKey(RegKey, True)
                        Key.SetValue("AppCaptureEnabled", 0, RegistryValueKind.DWord)
                    End Using
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As NullReferenceException
                    Registry.CurrentUser.CreateSubKey(RegKey)
                    MessageBox.Show("Please try again, I may have solved a possible issue.")

                Catch ex As Exception
                    MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
        End Select

        Enabled = True
    End Sub

    Private Sub DefenderBtn_Click(sender As Object, e As EventArgs) Handles DefenderBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                Try
                    ' Disable GUI and engine
                    Using Key As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Policies\Microsoft\Windows Defender", True)
                        Key.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord)
                        AddToConsole("Disabled main Defender functions!")
                    End Using

                    ' Delete Defender from startup
                    Using Key As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                        Key.DeleteValue("WindowsDefender", False) ' Don't error out if key doesn't exist
                        AddToConsole("Removed Defender from startup!")
                    End Using

                    ' Unregister Defender shell extension
                    Using process As Process = New Process()
                        ' unused: process.StandardOutput.ReadToEnd())
                        process.StartInfo.FileName = "cmd.exe"
                        process.StartInfo.CreateNoWindow = True
                        process.StartInfo.UseShellExecute = False
                        process.StartInfo.RedirectStandardInput = True
                        process.StartInfo.RedirectStandardOutput = True
                        process.Start()

                        ' Silent unregister of dll file
                        process.StandardInput.WriteLine("regsvr32 /u /s ""C:\Program Files\Windows Defender\shellext.dll""")
                        'process.StandardInput.Flush()
                        process.StandardInput.Close()
                        process.WaitForExit()

                        AddToConsole("Unregistered Defender shell addon!")
                    End Using

                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    AddToConsole(ex.ToString)
                    MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
        End Select

        Enabled = True
    End Sub

    Private Sub HomeGroupBtn_Click(sender As Object, e As EventArgs) Handles HomeGroupBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes

                Try
                    Using process As Process = New Process()
                        process.StartInfo.FileName = "cmd.exe"
                        process.StartInfo.CreateNoWindow = True
                        process.StartInfo.UseShellExecute = False
                        process.StartInfo.RedirectStandardInput = True
                        process.StartInfo.RedirectStandardOutput = True
                        process.Start()

                        process.StandardInput.WriteLine("sc config ""HomeGroupProvider"" start= disabled")
                        process.StandardInput.WriteLine("sc stop ""HomeGroupProvider""")
                        'process.StandardInput.Flush()
                        process.StandardInput.Close()
                        process.WaitForExit()

                        AddToConsole("Disabled HomeGroup!")
                    End Using

                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    AddToConsole(ex.ToString)
                    MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

        End Select

        Enabled = True
    End Sub

    Private Sub CheckUpdatesBtn_Click(sender As Object, e As EventArgs) Handles CheckUpdatesBtn.Click
        Enabled = False

        Try
            AddToConsole("Searching for updates . . .")

            'Start request
            Dim theRequest As HttpWebRequest = WebRequest.Create(ServerURL)
            theRequest.Timeout = 10000 '10sec timeout

            Using responce As HttpWebResponse = theRequest.GetResponse()
                Using reader As StreamReader = New StreamReader(responce.GetResponseStream())
                    OnlineVer = reader.ReadToEnd.Trim()
                End Using
            End Using

        Catch ex As Exception
            'Letting itself know that it cannot reach to the server
            OnlineVer = "0.0.0"
            AddToConsole("Could not search for updates!")
            MessageBox.Show("Could not search for updates!")

        End Try

        Dim OfflineVerI As Integer = Convert.ToInt32(OfflineVer.Replace(".", String.Empty))
        Dim OnlineVerI As Integer = Convert.ToInt32(OnlineVer.Replace(".", String.Empty))

        If OnlineVerI = OfflineVerI Then
            MessageBox.Show("Client is up to date")
        Else

            If OfflineVerI > OnlineVerI Then
                MessageBox.Show("OfflineVer is greater than OnlineVer!")
            End If

            If OnlineVerI < OfflineVerI Then
                MessageBox.Show("Client is up to date")
            Else

                Select Case MsgBox("Your client is outdated and a new update can be downloaded from the offical webpage, do you want me to open a webpage of the download page?", MsgBoxStyle.YesNo)
                    Case MsgBoxResult.Yes
                        Process.Start("https://github.com/ElPumpo/Win10Clean/releases")
                End Select
            End If
        End If

        AddToConsole("OfflineVer: " + OfflineVer)
        AddToConsole("OnlineVer: " + OnlineVer)

        Enabled = True
    End Sub

    Private Sub AdsBtn_Click(sender As Object, e As EventArgs) Handles AdsBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                Try
                    Using Key As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", True)
                        Key.SetValue("SystemPaneSuggestionsEnabled", AdsSwitch, RegistryValueKind.DWord)
                        AddToConsole(AdsMessage)
                    End Using

                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    AddToConsole(ex.ToString)
                    MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
        End Select

        Enabled = True
    End Sub

    Private Sub AppKeepBtn_Click(sender As Object, e As EventArgs) Handles AppKeepBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes

                Try
                    Using Key As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", True)
                        Key.SetValue("SilentInstalledAppsEnabled", AdsSwitch, RegistryValueKind.DWord)
                        AddToConsole("Stopped automatic app install!")
                    End Using

                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    AddToConsole(ex.ToString)
                    MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
        End Select

        Enabled = True
    End Sub

    Private Sub UninstallOneDrive()

        Dim ProcessName As String = "OneDrive"
        Try
            Process.GetProcessesByName(ProcessName)(0).Kill()
        Catch ex As Exception
            AddToConsole("Could not kill process: " + ProcessName)
            ' ignore errors
        End Try

        Dim OnePath As String = Nothing
        If Is64 Then
            OnePath = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\OneDriveSetup.exe"
        Else
            OnePath = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\OneDriveSetup.exe"
        End If
        Process.Start(OnePath, "/uninstall")
        AddToConsole("Uninstalled OneDrive using the setup!")

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
                    AddToConsole("Deleted dir: " + dir)
                Catch ex As Exception
                    AddToConsole("Could not delete dir: " + dir)
                    ' ignore errors
                End Try
            End If
        Next

        ' Remove OneDrive from Explorer

        ' Default Attribute: 0xf080004d
        Dim OneKeyExplorer As String = "CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}"

        Try

            ' Remove from the Explorer file dialog
            Using Key As RegistryKey = Registry.ClassesRoot.OpenSubKey(OneKeyExplorer, True)
                Key.SetValue("System.IsPinnedToNameSpaceTree", 0, RegistryValueKind.DWord)
                AddToConsole("Deleted OneDrive from Explorer (FileDialog)!")
            End Using

            ' amd64 system fix 
            If Is64 Then
                Using Key As RegistryKey = Registry.ClassesRoot.OpenSubKey("WOW6432Node\" + OneKeyExplorer, True)
                    Key.SetValue("System.IsPinnedToNameSpaceTree", 0, RegistryValueKind.DWord)
                    AddToConsole("Deleted OneDrive from Explorer (FileDialog, amd64)!")
                End Using
            End If

            ' Remove from the alternative file dialog (legacy)
            Using Key As RegistryKey = Registry.ClassesRoot.OpenSubKey(OneKeyExplorer + "\ShellFolder", True)
                Key.SetValue("Attributes", &HB090010D, RegistryValueKind.DWord)
                AddToConsole("Deleted OneDrive from Explorer (Legacy FileDialog)!")
            End Using

            ' amd64 system fix
            If Is64 Then
                Using Key As RegistryKey = Registry.ClassesRoot.OpenSubKey("WOW6432Node\" + OneKeyExplorer + "\ShellFolder", True)
                    Key.SetValue("Attributes", &HB090010D, RegistryValueKind.DWord)
                    AddToConsole("Deleted OneDrive from Explorer (Legacy FileDialog, amd64)!")
                End Using
            End If

        Catch ex As NullReferenceException
            Registry.ClassesRoot.CreateSubKey(OneKeyExplorer)
            MessageBox.Show("Please run me again!")

        Catch ex As Exception
            AddToConsole(ex.ToString)
            MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Delete scheduled leftovers
        Try
            ' unused: process.StandardOutput.ReadToEnd())
            Using process As Process = New Process()
                process.StartInfo.FileName = "cmd.exe"
                process.StartInfo.CreateNoWindow = True
                process.StartInfo.UseShellExecute = False
                process.StartInfo.RedirectStandardInput = True
                process.StartInfo.RedirectStandardOutput = True
                process.Start()

                process.StandardInput.WriteLine("SCHTASKS /Delete /TN ""OneDrive Standalone Update Task"" /F")
                'process.StandardInput.Flush()
                process.StandardInput.Close()
                process.WaitForExit()

                AddToConsole("Removed OneDrive scheduled tasks!")
            End Using

        Catch ex As Exception
            AddToConsole(ex.ToString)
            MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub Revert7()

        ' Get ride of libary folders in My PC
        Static LibKey As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions\"
        Dim LibGUID() As String = {"{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", "{7d83ee9b-2244-4e70-b1f5-5393042af1e4}", "{f42ee2d3-909f-4907-8871-4c22fc0bf756}", "{0ddd015d-b06c-45d5-8c4c-f59713854639}", "{a0c69a99-21c8-4671-8703-7934162fcf1d}", "{35286a68-3c57-41a1-bbb1-0eae73d76c95}"}

        For Each key As String In LibGUID
            Try
                Dim FinalKey = LibKey + key + "\PropertyBag"

                Using RegKey As RegistryKey = Registry.LocalMachine.OpenSubKey(FinalKey, True)
                    RegKey.SetValue("ThisPCPolicy", "Hide")
                    AddToConsole("Modified value of: " + key)
                End Using

            Catch ex As Exception
                AddToConsole(ex.GetType().Name + " - Couldn't modify the value of: " + key)
                MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        Next

        ' Pin libary folders
        Static PinLib As String = "Software\Classes\CLSID\{031E4825-7B94-4dc3-B131-E946B44C8DD5}"

        Try
            Using Key As RegistryKey = Registry.CurrentUser.OpenSubKey(PinLib, True)
                Key.SetValue("System.IsPinnedToNameSpaceTree", 1, RegistryValueKind.DWord)
                AddToConsole("Pinned the libary folders in Explorer!")
            End Using

        Catch ex As NullReferenceException
            Registry.CurrentUser.CreateSubKey(PinLib) ' doesn't exist as default, normal behaviour
            MessageBox.Show("Please run me again!")

        Catch ex As Exception
            AddToConsole(ex.ToString)
            MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Try
            ' Stop quick access from filling up with folders and files
            Using Key As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer", True)
                Key.SetValue("ShowFrequent", 0, RegistryValueKind.DWord) ' Folders
                Key.SetValue("ShowRecent", 0, RegistryValueKind.DWord) ' Files
                AddToConsole("Disabled quick access filling up!")
            End Using

            ' Make explorer open 'My PC' by default
            Using Key As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", True)
                Key.SetValue("LaunchTo", 1, RegistryValueKind.DWord)
                AddToConsole("Made 'My PC' the default dir when launching Explorer!")
            End Using

        Catch ex As Exception
            AddToConsole(ex.ToString)
            MessageBox.Show(ex.ToString, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ContextBtn_Click(sender As Object, e As EventArgs) Handles ContextBtn.Click
        Enabled = False

        Select Case MsgBox("Are you sure?", MsgBoxStyle.YesNo)
            Case MsgBoxResult.Yes
                Enabled = False

                ' provided by http://fragme.blogspot.se/2007/07/windows-tip-18-remove-unnecessary-right.html
                Dim Extentions() As String =
                {
                "batfile",
                "cmdfile",
                "docfile",
                "fonfile",
                "htmlfile",
                "inffile",
                "inifile",
                "JSEFile",
                "JSFile",
                "MSInfo.Document",
                "otffile",
                "pfmfile",
                "regfile",
                "rtffile",
                "ttcfile",
                "ttffile",
                "txtfile",
                "VBEFile",
                "VBSFile",
                "Wordpad.Document.1",
                "WSFFile"
                }

                ' Disable print
                For Each ext As String In Extentions
                    Try
                        Dim FinalKey As String = ext + "\shell\print"

                        Using Key As RegistryKey = Registry.ClassesRoot.OpenSubKey(FinalKey, True)
                            Key.SetValue("LegacyDisable", String.Empty)
                            AddToConsole("Disabled print for: " + ext + "!")
                        End Using
                    Catch ex As Exception
                        AddToConsole(ex.GetType().ToString() + " - couldn't disable print for: " + ext + "!")
                        ' ignore errors
                    End Try
                Next

                ' Disable edit
                For Each ext As String In Extentions
                    Try
                        Dim FinalKey As String = ext + "\shell\edit"

                        Using Key As RegistryKey = Registry.ClassesRoot.OpenSubKey(FinalKey, True)
                            Key.SetValue("LegacyDisable", String.Empty, RegistryValueKind.String)
                            AddToConsole("Disabled edit for: " + ext + "!")
                        End Using
                    Catch ex As Exception
                        AddToConsole(ex.GetType().ToString() + " - couldn't disable edit for: " + ext + "!")
                        ' ignore errors
                    End Try
                Next

                Try
                    Static Key As RegistryKey

                    ' txt file
                    Key = Registry.ClassesRoot.OpenSubKey("SystemFileAssociations\text\shell\edit", True)
                    Key.SetValue("LegacyDisable", String.Empty, RegistryValueKind.String)
                    AddToConsole("Disabled edit for: txt file!")

                    ' WMP #1 - add to list
                    Key = Registry.ClassesRoot.OpenSubKey("SystemFileAssociations\audio\shell\Enqueue", True)
                    Key.SetValue("LegacyDisable", String.Empty, RegistryValueKind.String)
                    AddToConsole("Disabled add to play list for: audio files!")

                    ' WMP #2 - play
                    Key = Registry.ClassesRoot.OpenSubKey("SystemFileAssociations\audio\shell\Play", True)
                    Key.SetValue("LegacyDisable", String.Empty, RegistryValueKind.String)
                    AddToConsole("Disabled play song for: audio files!")

                    Key.Dispose()

                Catch ex As Exception
                    AddToConsole(ex.GetType().ToString() + " - couldn't disable edit for")
                    ' ignore errors
                End Try

                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Enabled = True
        End Select

        Enabled = True
    End Sub

    ' Metero related
    Private Sub RefreshBtn_Click(sender As Object, e As EventArgs) Handles RefreshBtn.Click
        Enabled = False
        RefreshList(False)
        Enabled = True
    End Sub

    Private Async Sub UninstallBtn_Click(sender As Object, e As EventArgs) Handles UninstallBtn.Click
        Enabled = False
        TheApps = Nothing ' Reset list

        ' Displays all the apps to be uninstalled
        If Not AppBox.SelectedItem = Nothing Then
            For Each app In AppBox.SelectedItems
                If TheApps Is Nothing Then
                    TheApps = app ' First app
                Else
                    TheApps = TheApps + ", " + app ' If user selected multiple apps, seperate with a comma
                End If

            Next
            Select Case MsgBox("Are you sure you want to uninstall " + TheApps + "?", MsgBoxStyle.YesNo)
                Case MsgBoxResult.Yes
                    For Each app In AppBox.SelectedItems
                        Await UninstallApp(app)
                    Next
                    RefreshList(True) ' refresh list when we're done
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
        Try
            If (MinusOne) Then
                AppBox.SelectedIndex = GoBack - 1
            Else
                AppBox.SelectedIndex = GoBack
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Async Function UninstallApp(AppName As String) As Task
        Dim WasError As Boolean = False
        Using PowerScript As PowerShell = PowerShell.Create()

            If (AllUserBox.Checked) Then
                PowerScript.AddScript("Get-AppxPackage -AllUsers " + AppName + " | Remove-AppxPackage")
            Else
                PowerScript.AddScript("Get-AppxPackage " + AppName + " | Remove-AppxPackage")
            End If

            PowerScript.Invoke()

            WasError = PowerScript.HadErrors ' Doesn't work in some cases
        End Using

        ' Is app really uninstalled?-
        If WasError = True Then
            AddToConsole("Couldn't uninstall app: " + AppName)
        Else
            AddToConsole("Uninstalled app: " + AppName)
        End If
        Return
    End Function

    Private Sub FindApps()
        Using PowerScript As PowerShell = PowerShell.Create()
            If (AllUserBox.Checked) Then
                PowerScript.AddScript("Get-AppxPackage -AllUsers | Select Name | Out-String -Stream")
            Else
                PowerScript.AddScript("Get-AppxPackage | Select Name | Out-String -Stream")
            End If


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

    Private Sub MeteroTab_Enter(sender As Object, e As EventArgs) Handles MeteroTab.Enter
        ' When the user selects the tab
        Enabled = False
        RefreshList(False)
        Enabled = True
    End Sub

    Private Sub AddToConsole(Information As String)
        If Not Information = Nothing Then
            DebugBox.Text = DebugBox.Text + Information + Environment.NewLine
            Console.WriteLine(Information)
        End If
    End Sub

    Private Sub PhotoRegBtn_Click(sender As Object, e As EventArgs) Handles PhotoRegBtn.Click

    End Sub
End Class