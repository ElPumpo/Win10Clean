using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Management.Automation;
using Microsoft.Win32;

/*
 * Win10Clean - Cleanup your Windows 10 environment
 * Copyright (C) 2017 Hawaii_Beach & deadmoon
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the license, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/

namespace Win10Clean
{
    public partial class MainForm : Form
    {
        string _currentVersion = Application.ProductVersion;
        string _onlineVersion = "Unknown";
        int _offline = 9;
        string _serverUrl = "https://ElPumpo.github.io/Win10Clean/version.txt";
        string _releasesUrl = "https://github.com/ElPumpo/Win10Clean/releases";

        bool _is64 = Environment.Is64BitOperatingSystem;
        int _goBack;
        string _apps = string.Empty;
        string _logInfo = string.Empty;

        int _adsSwitch = 0;
        bool _defenderSwitch = false;
        string _adsMessage = "Start Menu ads disabled";

        public MainForm()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += string.Format(" [{0}]", _currentVersion);
            lblVersion.Text += _currentVersion;
            CheckTweaks();
        }

        private void Log(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                consoleBox.Text += msg + Environment.NewLine;
            }
        }

        private void ExportLog()
        {
            if (!string.IsNullOrEmpty(consoleBox.Text))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "Win10Clean - v" + _currentVersion + " - " + DateTime.Now.ToString("yyyy/MM/dd HH-mm-ss");
                dialog.Filter = "Text files | *.txt";
                dialog.DefaultExt = "txt";
                dialog.Title = "Exporting log...";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dialog.FileName, consoleBox.Text);
                }
            }
            else
            {
                MessageBox.Show("There is nothing to export!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CheckForUpdates()
        {
            try
            {
                Log("Checking for updates...");

                WebRequest req = WebRequest.Create(_serverUrl);
                req.Timeout = 10000;
                req.Headers.Set("Cache-Control", "no-cache, no-store, must-revalidate");

                WebResponse res = req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());

                _onlineVersion = reader.ReadToEnd().Trim();
                reader.Close();
                res.Close();
            }
            catch (Exception)
            {
                _onlineVersion = "0";
                Log("Could not check for updates");
                MessageBox.Show("Could not check for updates!");
            }

            int tmp = Convert.ToInt32(_onlineVersion);

            if (tmp == _offline)
            {
                Log("You have the latest version");
                MessageBox.Show("You have the latest version!");
            }
            else
            {
                if (MessageBox.Show("A new update is available, download it now?", "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start(_releasesUrl);
                }
            }

            if (_offline > tmp)
            {
                Log("You have a beta version");
                MessageBox.Show("You have a beta version!");
            }
        }

        private void RestartExplorer()
        {
            const string explorer = "explorer.exe";
            string explorerPath = string.Format("{0}\\{1}", Environment.GetEnvironmentVariable("WINDIR"), explorer);
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    if (string.Compare(process.MainModule.FileName, explorerPath, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        process.Kill();
                    }
                }
                catch { }
            }
            Process.Start(explorer);
        }

        private void CheckTweaks()
        {
            try
            {
                // check start menu ads
                using (RegistryKey k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", false))
                {
                    if ((int)k.GetValue("SystemPaneSuggestionsEnabled", 1) == 0)
                    {
                        _adsSwitch = 1;
                        btnStartAds.Text = "Enable Start Menu ads";
                        _adsMessage = "Start Menu ads enabled";
                    }
                }

                //  check defender state
                using (RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", false))
                {
                    if ((int)k.GetValue("DisableAntiSpyware", 0) == 1)
                    {
                        _defenderSwitch = true;
                        btnDefender.Text = "Enable Windows Defender";
                    }
                }
            }
            catch { }
        }

        private void RunCommand(string cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            p.StandardInput.WriteLine(cmd);
            p.StandardInput.Close();
            p.WaitForExit();
        }

        private void UninstallOneDrive()
        {
            RegistryKey k = null;
            string processName = "OneDrive";

            byte[] byteArray = BitConverter.GetBytes(0xb090010d);
            int oneDriveSwitch = BitConverter.ToInt32(byteArray, 0);

            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Process.GetProcessesByName(processName)[0].Kill();
                }
                catch (Exception)
                {
                    Log("Could not kill process: " + processName);
                }

                string path = string.Empty;

                if (_is64)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OneDriveSetup.exe";
                }
                else
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\OneDriveSetup.exe";
                }

                Process.Start(path, "/uninstall");
                Log("OneDrive uninstalled");

                string[] paths =
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OneDrive",
                    Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "OneDriveTemp",
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\OneDrive",
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microsoft OneDrive"
                };

                foreach (string x in paths)
                {
                    if (Directory.Exists(x))
                    {
                        try
                        {
                            Directory.Delete(x, true);
                            Log("Folder deleted: " + x);
                        }
                        catch (Exception)
                        {
                            Log("Could not delete folder: " + x);
                        }
                    }
                }

                string oneKey = @"CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}";
                Registry.ClassesRoot.CreateSubKey(oneKey);

                try
                {
                    k = Registry.ClassesRoot.OpenSubKey(oneKey, true);
                    k.SetValue("System.IsPinnedToNameSpaceTree", 0, RegistryValueKind.DWord);
                    Log("OneDrive removed from File Explorer");

                    if (_is64)
                    {
                        k = Registry.ClassesRoot.OpenSubKey("WOW6432Node\\" + oneKey, true);
                        k.SetValue("System.IsPinnedToNameSpaceTree", 0, RegistryValueKind.DWord);
                        Log("OneDrive removed from File Explorer");
                    }

                    k = Registry.ClassesRoot.OpenSubKey(oneKey + "\\ShellFolder", true);
                    k.SetValue("Attributes", oneDriveSwitch, RegistryValueKind.DWord); // value needs testing
                    Log("OneDrive removed from legacy File Dialog");

                    if (_is64)
                    {
                        k = Registry.ClassesRoot.OpenSubKey("WOW6432Node\\" + oneKey + "\\ShellFolder", true);
                        k.SetValue("Attributes", oneDriveSwitch, RegistryValueKind.DWord); // value needs testing
                        Log("OneDrive removed from legacy File Dialog");
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    k.Close();
                }

                try
                {
                    RunCommand("SCHTASKS /Delete /TN \"OneDrive Standalone Update Task\" /F");
                    RunCommand("SCHTASKS /Delete /TN \"OneDrive Standalone Update Task v2\" /F");

                    Log("OneDrive scheduled tasks removed");
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CleanupContextMenu()
        {
            // Extended = only when SHIFT is pressed
            // LegacyDisable = item disabled

            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string[] extensions =
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
                };

                foreach (string x in extensions)
                {
                    try
                    {
                        string finalKey = x + @"\shell\print";

                        using (RegistryKey k = Registry.ClassesRoot.OpenSubKey(finalKey, true))
                        {
                            k.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                            Log("Print disabled for: " + x);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(ex.GetType().ToString() + " - couldn't disable print for: " + x);
                    }
                }

                foreach (string x in extensions)
                {
                    try
                    {
                        string finalKey = x + @"\shell\edit";

                        using (RegistryKey k = Registry.ClassesRoot.OpenSubKey(finalKey, true))
                        {
                            k.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                            Log("Edit disabled for: " + x);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(ex.GetType().ToString() + " - couldn't disable edit for: " + x);
                    }
                }

                RegistryKey key = null;

                try
                {
                    key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\text\shell\edit", true);
                    key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                    Log("Edit disabled for: TXT files");

                    key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\audio\shell\Enqueue", true);
                    key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                    Log("Disabled add to play list for: audio files!");

                    key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\audio\shell\Play", true);
                    key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                    Log("Disabled play song for: audio files!");

                    key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\Directory.Audio\shell\Enqueue", true);
                    key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                    Log("Disabled add to play list for: audio directories!");

                    key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\Directory.Audio\shell\Play", true);
                    key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                    Log("Disabled play song for: audio directories!");

                    key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\Directory.Image\shell\Enqueue", true);
                    key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                    Log("Disabled add to play list for: image directories!");

                    key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\Directory.Image\shell\Play", true);
                    key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                    Log("Disabled play song for: image directories!");

                    key = Registry.ClassesRoot.OpenSubKey(@"Folder\shellex\ContextMenuHandlers\Library Location", true);
                    key.SetValue(string.Empty, "-{3dad6c5d-2167-4cae-9914-f99e41c12cfa}");
                    Log("Disabled include in library menu!");

                    key = Registry.ClassesRoot.OpenSubKey(@"SystemFileAssociations\Directory.Audio\shellex\ContextMenuHandlers\WMPShopMusic", true);
                    key.SetValue(string.Empty, "-{8A734961-C4AA-4741-AC1E-791ACEBF5B39}");
                    Log("Disabled buying music online context menu!");

                    key = Registry.ClassesRoot.OpenSubKey(@"exefile\shellex\ContextMenuHandlers\Compatibility", true);
                    key.SetValue(string.Empty, "-{1d27f844-3a1f-4410-85ac-14651078412d}");
                    Log("Disabled troubleshooting compability (EXE)!");

                    key = Registry.ClassesRoot.OpenSubKey(@"Msi.Package\shellex\ContextMenuHandlers\Compatibility", true);
                    key.SetValue(string.Empty, "-{1d27f844-3a1f-4410-85ac-14651078412d}");
                    Log("Disabled troubleshooting compability (MSI)!");

                    Registry.ClassesRoot.DeleteSubKey(@"AllFilesystemObjects\shellex\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}", false);
                    Log("Removed restoring previous version menu! (files)");

                    Registry.ClassesRoot.DeleteSubKey(@"Directory\shellex\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}", false);
                    Log("Removed restoring previous version menu! (directory)");
                }
                catch (Exception ex)
                {
                    Log(ex.GetType().ToString() + " - something went wrong!");
                }
                finally
                {
                    key.Close();
                }

                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DisableStartMenuAds()
        {
            RegistryKey k = null;

            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", true);
                    k.SetValue("SystemPaneSuggestionsEnabled", _adsSwitch);

                    Log(_adsMessage);
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    k.Close();
                }
            }
        }

        private void DisableHomeGroup()
        {
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    RunCommand("sc config \"HomeGroupProvider\" start=disabled");
                    RunCommand("sc stop \"HomeGroupProvider\"");

                    Log("HomeGroup disabled");
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TweakDefender()
        {
            RegistryKey k = null;

            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!_defenderSwitch)
                {
                    try
                    {
                        k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true);
                        k.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
                        Log("Main Windows Defender functions disabled");

                        k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                        k.DeleteValue("WindowsDefender", false);
                        k.DeleteValue("SecurityHealth", false);
                        Log("Windows Defender removed from startup");

                        RunCommand("regsvr32 /u /s \"C:\\Program Files\\Windows Defender\\shellext.dll\"");

                        Log("Windows Defender shell addons unregistered");

                        MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Log(ex.ToString());
                        MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        k.Close();
                    }
                }
                else
                {
                    try
                    {
                        k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true);
                        k.SetValue("DisableAntiSpyware", 0, RegistryValueKind.DWord);

                        Log("Main Windows Defender functions enabled");
                        MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Log(ex.ToString());
                        MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        k.Close();   
                    }
                }
            }
        }
         
        private void DisableSilentInstall()
        {
            RegistryKey k = null;

            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", true);
                    k.SetValue("SilentInstalledAppsEnabled", _adsSwitch, RegistryValueKind.DWord);

                    Log("Silent Modern App install disabled");
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    k.Close();
                }
            }
        }

        private void RevertExplorer()
        {
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string libKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions\";
                string[] libGuid = { "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", "{7d83ee9b-2244-4e70-b1f5-5393042af1e4}", "{f42ee2d3-909f-4907-8871-4c22fc0bf756}", "{0ddd015d-b06c-45d5-8c4c-f59713854639}", "{a0c69a99-21c8-4671-8703-7934162fcf1d}", "{35286a68-3c57-41a1-bbb1-0eae73d76c95}" };
                string finalKey = string.Empty;
                RegistryKey k = null;

                foreach (string x in libGuid)
                {
                    try
                    {
                        finalKey = libKey + x + "\\PropertyBag";
                        k = Registry.LocalMachine.OpenSubKey(finalKey, true);
                        k.SetValue("ThisPCPolicy", "Hide");
                        Log(string.Format("Value of {0} modified", x));
                    }
                    catch (Exception ex)
                    {
                        Log(ex.GetType().Name + " - Couldn't modify the value of: " + x);
                        MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        k.Close();
                    }
                }

                string pinLib = @"Software\Classes\CLSID\{031E4825-7B94-4dc3-B131-E946B44C8DD5}";
                Registry.CurrentUser.CreateSubKey(pinLib);

                try
                {
                    k = Registry.CurrentUser.OpenSubKey(pinLib, true);
                    k.SetValue("System.IsPinnedToNameSpaceTree", 1, RegistryValueKind.DWord);
                    Log("Library folders pinned");
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    k.Close();
                }

                try
                {
                    k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer", true);
                    k.SetValue("ShowFrequent", 0, RegistryValueKind.DWord); // folders
                    k.SetValue("ShowRecent", 0, RegistryValueKind.DWord); //files
                    Log("Quick Access disabled");

                    k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true);
                    k.SetValue("LaunchTo", 1, RegistryValueKind.DWord);
                    Log("Open explorer to: This PC");
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    k.Close();
                }

                byte[] bytes = { 2, 0, 0, 0, 64, 31, 210, 5, 170, 22, 211, 1, 0, 0, 0, 0, 67, 66, 1, 0, 194, 10, 1, 203, 50, 10, 2, 5, 134, 145, 204, 147, 5, 36, 170, 163, 1, 68, 195, 132, 1, 102, 159, 247, 157, 177, 135, 203, 209, 172, 212, 1, 0, 5, 188, 201, 168, 164, 1, 36, 140, 172, 3, 68, 137, 133, 1, 102, 160, 129, 186, 203, 189, 215, 168, 164, 130, 1, 0, 194, 60, 1, 0 };

                try
                {
                    k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.unifiedtile.startglobalproperties\Current", true);
                    k.SetValue("Data", bytes, RegistryValueKind.Binary);
                    Log("File Explorer from Start Menu enabled");
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    k.Close();
                }

                RestartExplorer();

                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RetrieveApps()
        {
            using (PowerShell script = PowerShell.Create())
            {
                if (chkAll.Checked)
                {
                    script.AddScript("Get-AppxPackage -AllUsers | Select Name | Out-String -Stream");
                }
                else
                {
                    script.AddScript("Get-AppxPackage | Select Name | Out-String -Stream");
                }

                string trimmed = string.Empty;
                foreach (PSObject x in script.Invoke())
                {
                    trimmed = x.ToString().Trim();
                    if (!string.IsNullOrEmpty(trimmed) && !trimmed.Contains("---"))
                    {
                        if (trimmed != "Name") appBox.Items.Add(trimmed);
                    }
                }
            }
        }

        private void RefreshAppList(bool minusOne)
        {
            _goBack = appBox.SelectedIndex;
            appBox.Items.Clear();
            RetrieveApps();

            try
            {
                if (minusOne)
                {
                    appBox.SelectedIndex = _goBack - 1;
                }
                else
                {
                    appBox.SelectedIndex = _goBack;
                }
            }
            catch { }
        }

        private void UninstallApps()
        {
            _apps = string.Empty;

            if (appBox.CheckedItems.Count > 0)
            {
                foreach (var x in appBox.CheckedItems)
                {
                    if (string.IsNullOrEmpty(_apps))
                    {
                        _apps = x.ToString();
                    }
                    else
                    {
                        _apps = _apps + "\n" + x.ToString();
                    }
                }

                if (MessageBox.Show("Are you sure you want to uninstall these apps?\n\n" + _apps, "Confirm uninstall", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (var x in appBox.CheckedItems)
                    {
                        Task.Run(() => UninstallApp(x.ToString()));
                    }

                    RefreshAppList(true);
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void UninstallApp(string app)
        {
            bool error = false;

            using (PowerShell script = PowerShell.Create())
            {
                if (chkAll.Checked)
                {
                    script.AddScript("Get-AppxPackage -AllUsers " + app + " | Remove-AppxPackage");
                }
                else
                {
                    script.AddScript("Get-AppxPackage " + app + " | Remove-AppxPackage");
                }

                script.Invoke();
                error = script.HadErrors;
            }

            if (error)
            {
                Log("Could not uninstall app: " + app);
            }
            else
            {
                Log("App uninstalled: " + app);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private void btnDefender_Click(object sender, EventArgs e)
        {
            TweakDefender();
        }

        private void btnHomegroup_Click(object sender, EventArgs e)
        {
            DisableHomeGroup();
        }

        private void btnApps_Click(object sender, EventArgs e)
        {
            DisableSilentInstall();
        }

        private void btnStartAds_Click(object sender, EventArgs e)
        {
            DisableStartMenuAds();
        }

        private void btnExplorer_Click(object sender, EventArgs e)
        {
            RevertExplorer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportLog();
        }

        private void btnOnedrive_Click(object sender, EventArgs e)
        {
            UninstallOneDrive();
        }

        private void btnContext_Click(object sender, EventArgs e)
        {
            CleanupContextMenu();   
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshAppList(false);
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            RefreshAppList(false);
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            UninstallApps();
        }
    }
}
