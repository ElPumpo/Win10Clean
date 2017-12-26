using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Management.Automation;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Threading;
using Win10Clean.Common;

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
        public Version offlineVer = new Version(Application.ProductVersion);
        public Version onlineVer;
        string serverUrl = "https://ElPumpo.github.io/Win10Clean/version2.txt";
        string releasesUrl = "https://github.com/ElPumpo/Win10Clean/releases";
        bool amd64 = Environment.Is64BitOperatingSystem;
        bool defenderSwitch = false;

        /* Metro related */
        List<string> uninstallSuccessList = new List<string>();
        List<string> uninstallFailedList = new List<string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += " v" + offlineVer.ToString();
            CheckTweaks();
        }

        /* Buttons / Main stuff */

        private void OneDriveBtn_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                string processName = "OneDrive";
                int byteArray = BitConverter.ToInt32(BitConverter.GetBytes(0xb090010d), 0);
                string onePath;

                try {
                    Process.GetProcessesByName(processName)[0].Kill();
                } catch (Exception) { // Throws IndexOutOfRangeException
                    Log("Could not kill process: " + processName);
                    // ignore errors
                }

                if (amd64) {
                    onePath = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OneDriveSetup.exe";
                } else {
                    onePath = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\OneDriveSetup.exe";
                }

                try {
                    Process.Start(onePath, "/uninstall");
                    Log("Uninstalled OneDrive using the setup!");
                } catch (Exception ex) {
                    Log(ex.ToString());
                }

                // All the folders to be deleted
                string[] onePaths = {
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OneDrive",
                    Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "OneDriveTemp",
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\OneDrive",
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microsoft OneDrive"
                };

                foreach (string dir in onePaths) {
                    if (Directory.Exists(dir)) {
                        try {
                            Directory.Delete(dir, true);
                            Log("Folder deleted: " + dir);
                        } catch (Exception) {
                            Log("Could not delete folder: " + dir);
                            // ignore errors
                        }
                    }
                }

                // Remove OneDrive from Explorer
                string oneKey = @"CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}";
                Registry.ClassesRoot.CreateSubKey(oneKey);

                var baseReg = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                try {
                    // Remove from the Explorer file dialog
                    using (var key = Registry.ClassesRoot.OpenSubKey(oneKey, true)) {
                        key.SetValue("System.IsPinnedToNameSpaceTree", 0, RegistryValueKind.DWord);
                        Log("OneDrive removed from Explorer (FileDialog)!");
                    }

                    // amd64 system fix
                    if (amd64) {
                        using (var key = baseReg.OpenSubKey(oneKey, true)) {
                            key.SetValue("System.IsPinnedToNameSpaceTree", 0, RegistryValueKind.DWord);
                            Log("OneDrive removed from Explorer (FileDialog, amd64)!");
                        }
                    }

                    // Remove from the alternative file dialog (legacy)
                    using (var key = Registry.ClassesRoot.OpenSubKey(oneKey + "\\ShellFolder", true)) {
                        key.SetValue("Attributes", byteArray, RegistryValueKind.DWord);
                        Log("OneDrive removed from Explorer (Legacy FileDialog)!");
                    }

                    // amd64 system fix
                    if (amd64) {
                        using (var key = baseReg.OpenSubKey(oneKey + "\\ShellFolder", true)) {
                            key.SetValue("Attributes", byteArray, RegistryValueKind.DWord);
                            Log("OneDrive removed from Explorer (Legacy FileDialog, amd64)!");
                        }
                    }

                    baseReg = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                    // Remove the startup
                    using (var key = baseReg.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true)) {
                        key.DeleteValue("OneDriveSetup", false);
                        Log("Removed startup object!");
                    }
                } catch (Exception ex) {
                    Log(ex.ToString());
                    MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                baseReg.Dispose();

                // Delete scheduled leftovers
                CMDHelper.RunCommand(@"SCHTASKS /Delete /TN ""OneDrive Standalone Update Task"" /F");
                CMDHelper.RunCommand(@"SCHTASKS /Delete /TN ""OneDrive Standalone Update Task v2"" /F");
                Log("OneDrive scheduled tasks deleted!");

                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try {
                Log("Checking for updates...");

                WebRequest req = WebRequest.Create(serverUrl);
                req.Timeout = 10000;
                req.Headers.Set("Cache-Control", "no-cache, no-store, must-revalidate");

                WebResponse res = req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());

                onlineVer = new Version(reader.ReadToEnd().Trim());

                // Dispone when done
                reader.Dispose();
                res.Dispose();
            } catch (Exception ex) {
                Log(ex.ToString());
                MessageBox.Show("Could not check for updates!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var diff = onlineVer.CompareTo(offlineVer);
            if (diff == 0) {
                Log("Client up-to-date");
                MessageBox.Show("No new updates were found, you are running the latest version!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (diff < 0) {
                Log("Client > Remote!");
                MessageBox.Show("You are running a newer version than remote! This is not normal and there might be a new update available.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else {
                Log("Update available!");
                if (MessageBox.Show("There is a new update available for download, do you want to visit the GitHub releases website?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    Process.Start(releasesUrl);
                }
            }
            Log(string.Format("Offline: v{0} | Online: v{1} | Diff: {2}", offlineVer, onlineVer, diff));
            Enabled = true;
        }

        private void btnDefender_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                var baseReg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                if (!defenderSwitch) {
                    try {
                        // Disable engine
                        using (var key = baseReg.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true)) {
                            key.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
                            Log("Disabled main Defender functions!");
                        }

                        // Delete Defender from startup / tray icons
                        using (var key = baseReg.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true)) {
                            key.DeleteValue("WindowsDefender", false);
                            key.DeleteValue("SecurityHealth", false);
                            Log("Windows Defender removed from startup!");
                        }

                        // Unregister Defender shell extension
                        CMDHelper.RunCommand(@"regsvr32 /u /s """ + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Windows Defender\shellext.dll""");
                        Log("Windows Defender shell addons unregistered!");

                        MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch (Exception ex) {
                        Log(ex.ToString());
                        MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else { // re-enable Defender
                    try {
                        using (var key = baseReg.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true)) {
                            key.SetValue("DisableAntiSpyware", 0, RegistryValueKind.DWord);
                            Log("Main Windows Defender functions enabled!");
                        }

                        MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch (Exception ex) {
                        Log(ex.ToString());
                        MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                baseReg.Dispose();
            }
            Enabled = true;
        }

        private void HomeGroupBtn_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                CMDHelper.RunCommand(@"sc config ""HomeGroupProvider"" start= disabled"); // stop autorun
                CMDHelper.RunCommand(@"sc stop ""HomeGroupProvider"""); // stop process now
                Log("HomeGroup disabled!");

                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Enabled = true;
        }

        private void Revert7Btn_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                
                // Get ride of libary folders in My PC
                string libKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions\";
                string[] guidArray = {
                    "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
                    "{7d83ee9b-2244-4e70-b1f5-5393042af1e4}", // Downloads
                    "{f42ee2d3-909f-4907-8871-4c22fc0bf756}", // Documents
                    "{0ddd015d-b06c-45d5-8c4c-f59713854639}", // Pictures
                    "{a0c69a99-21c8-4671-8703-7934162fcf1d}", // Music
                    "{35286a68-3c57-41a1-bbb1-0eae73d76c95}", // Videos
                    "{31C0DD25-9439-4F12-BF41-7FF4EDA38722}"  // 3D builder
                };
                string finalKey;
                var baseReg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64); // we don't want Wow6432Node

                foreach (string guid in guidArray) {
                    try {
                        finalKey = libKey + guid + @"\PropertyBag";
                        using (var key = baseReg.CreateSubKey(finalKey, true)) {
                            key.SetValue("ThisPCPolicy", "Hide");
                            Log(string.Format("Value of {0} modified", guid));
                        }

                        // amd64 fix
                        if (amd64) {
                            using (var key = Registry.LocalMachine.CreateSubKey(finalKey)) {
                                key.SetValue("ThisPCPolicy", "Hide");
                                Log(string.Format("Value of {0} modified (amd64)", guid));
                            }
                        }
                    } catch (Exception ex) {
                        Log(ex.ToString());
                        MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                baseReg = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                byte[] bytes = { 2, 0, 0, 0, 64, 31, 210, 5, 170, 22, 211, 1, 0, 0, 0, 0, 67, 66, 1, 0, 194, 10, 1, 203, 50, 10, 2, 5, 134, 145, 204, 147, 5, 36, 170, 163, 1, 68, 195, 132, 1, 102, 159, 247, 157, 177, 135, 203, 209, 172, 212, 1, 0, 5, 188, 201, 168, 164, 1, 36, 140, 172, 3, 68, 137, 133, 1, 102, 160, 129, 186, 203, 189, 215, 168, 164, 130, 1, 0, 194, 60, 1, 0 };

                try {
                    // Pin libary folders
                    string pinLib = @"Software\Classes\CLSID\{031E4825-7B94-4dc3-B131-E946B44C8DD5}";
                    baseReg.CreateSubKey(pinLib); // doesn't exist as default, normal behaviour
                    using (var key = baseReg.OpenSubKey(pinLib, true)) {
                        key.SetValue("System.IsPinnedToNameSpaceTree", 1, RegistryValueKind.DWord);
                        Log("Pinned the libary folders in Explorer!");
                    }

                    // Stop quick access from filling up with folders and files
                    using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer", true)) {
                        key.SetValue("ShowFrequent", 0, RegistryValueKind.DWord); // folders
                        key.SetValue("ShowRecent", 0, RegistryValueKind.DWord);   // files
                        Log("Disabled quick access filling up!");
                    }

                    // Make explorer open 'My PC' by default
                    using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true)){
                        key.SetValue("LaunchTo", 1, RegistryValueKind.DWord);
                        Log("Open explorer to: This PC!");
                    }

                    // Add explorer on start menu
                    using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.unifiedtile.startglobalproperties\Current", true)) {
                        key.SetValue("Data", bytes, RegistryValueKind.Binary);
                        Log("File Explorer from Start Menu enabled!");
                    }
                    
                    // Hide OneDrive popup in Explorer
                    using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true)) {
                        key.SetValue("ShowSyncProviderNotifications", 0, RegistryValueKind.DWord);
                        Log("Hide OneDrive popup in Explorer!");
                    }

                    // Hide My People in taskbar
                    pinLib = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People";
                    Registry.CurrentUser.CreateSubKey(pinLib); // doesn't exist as default, normal behaviour
                    using (var key = Registry.CurrentUser.OpenSubKey(pinLib, true)) {
                        key.SetValue("PeopleBand", 0, RegistryValueKind.DWord);
                        Log("Hide My People from taskbar");
                    }
                } catch (Exception ex) {
                    Log(ex.ToString());
                    MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                baseReg.Dispose();
                RestartExplorer();
                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Enabled = true;
        }

        private void btnContext_Click(object sender, EventArgs e)
        {
            Enabled = false;
            // Extended = only when SHIFT is pressed
            // LegacyDisable = item disabled

            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var baseReg = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                // Provided by http://fragme.blogspot.se/2007/07/windows-tip-18-remove-unnecessary-right.html
                string[] extensions = {
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

                // Disable print
                foreach (string ext in extensions) {
                    try {
                        string finalKey = ext + @"\shell\print";
                        using (var key = baseReg.OpenSubKey(finalKey, true)) {
                            key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                            Log("Print disabled for: " + ext);
                        }
                    } catch (Exception ex) {
                        Log(ex.GetType().ToString() + " - couldn't disable print for: " + ext);
                        // Ignore errors
                    }
                }

                // Disable edit
                foreach (string ext in extensions) {
                    try {
                        string finalKey = ext + @"\shell\edit";
                        using (var key = baseReg.OpenSubKey(finalKey, true)) {
                            key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                            Log("Edit disabled for: " + ext);
                        }
                    } catch (Exception ex) {
                        Log(ex.GetType().ToString() + " - couldn't disable edit for: " + ext);
                        // Ignore errors
                    }
                }

                // Extra things
                try {
                    // Manual fix for txt
                    using (var key = baseReg.OpenSubKey(@"SystemFileAssociations\text\shell\edit", true)) {
                        key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                        Log("Edit disabled for: TXT files");
                    }

                    // WMP #1 - add to list
                    using (var key = baseReg.OpenSubKey(@"SystemFileAssociations\audio\shell\Enqueue", true)) {
                        key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                        Log("Disabled add to play list for: audio files!");
                    }

                    // WMP #2 - play
                    using (var key = baseReg.OpenSubKey(@"SystemFileAssociations\audio\shell\Play", true)) {
                        key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                        Log("Disabled play song for: audio files!");
                    }

                    // WMP #3 - add to list (audio folder)
                    using (var key = baseReg.OpenSubKey(@"SystemFileAssociations\Directory.Audio\shell\Enqueue", true)) {
                        key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                        Log("Disabled add to play list for: audio directories!");
                    }

                    // WMP #4 - play (audio folder)
                    using (var key = baseReg.OpenSubKey(@"SystemFileAssociations\Directory.Audio\shell\Play", true)) {
                        key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                        Log("Disabled play song for: audio directories!");
                    }

                    // WMP #5 - add to list (image folder?!)
                    using (var key = baseReg.OpenSubKey(@"SystemFileAssociations\Directory.Image\shell\Enqueue", true)) {
                        key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                        Log("Disabled add to play list for: image directories!");
                    }

                    // WMP #6 - play (image folder?!)
                    using (var key = baseReg.OpenSubKey(@"SystemFileAssociations\Directory.Image\shell\Play", true)) {
                        key.SetValue("LegacyDisable", string.Empty, RegistryValueKind.String);
                        Log("Disabled play song for: image directories!");
                    }

                    // Include in library context
                    using (var key = baseReg.OpenSubKey(@"Folder\shellex\ContextMenuHandlers\Library Location", true)) {
                        key.SetValue(string.Empty, "-{3dad6c5d-2167-4cae-9914-f99e41c12cfa}");
                        Log("Disabled include in library menu!");
                    }

                    // Buy music?
                    using (var key = baseReg.OpenSubKey(@"SystemFileAssociations\Directory.Audio\shellex\ContextMenuHandlers\WMPShopMusic", true)) {
                        key.SetValue(string.Empty, "-{8A734961-C4AA-4741-AC1E-791ACEBF5B39}");
                        Log("Disabled buying music online context menu!");
                    }

                    // Troubleshoot compability EXE
                    using (var key = baseReg.OpenSubKey(@"exefile\shellex\ContextMenuHandlers\Compatibility", true)) {
                        key.SetValue(string.Empty, "-{1d27f844-3a1f-4410-85ac-14651078412d}");
                        Log("Disabled troubleshooting compability (EXE)!");
                    }

                    // Troubleshoot compability MSI
                    using (var key = baseReg.OpenSubKey(@"Msi.Package\shellex\ContextMenuHandlers\Compatibility", true)) {
                        key.SetValue(string.Empty, "-{1d27f844-3a1f-4410-85ac-14651078412d}");
                        Log("Disabled troubleshooting compability (MSI)!");
                    }

                    // Disable printing .url files
                    var registryUtil = new RegistryUtilities();
                    registryUtil.TakeOwnership(@"InternetShortcut\shell\print", RegistryHive.ClassesRoot);
                    using (var key = baseReg.OpenSubKey(@"InternetShortcut\shell\print", true)) {
                        key.SetValue("LegacyDisable", string.Empty);
                        Log("Disabled print for: InternetShortcut!");
                    }

                    // Restore previous version (file)
                    baseReg.DeleteSubKey(@"AllFilesystemObjects\shellex\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}", false);
                    Log("Removed restoring previous version menu! (files)");

                    // Restore previous version (directory)
                    baseReg.DeleteSubKey(@"Directory\shellex\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}", false);
                    Log("Removed restoring previous version menu! (directories)");

                    // Manual fix for .java
                    baseReg.DeleteSubKeyTree(".java", false);
                    Log("Edit disabled for: JAVA files");

                    // https://superuser.com/a/808730
                    // Pin to Start on recycle bin
                    bool skip = false;
                    using (var key = baseReg.OpenSubKey(@"CLSID\{645FF040-5081-101B-9F08-00AA002F954E}\shell\empty")) {
                        if (key == null) skip = true;
                    }

                    // Do we really need to do this?
                    if (!skip) {
                        // Take ownership of the keys
                        registryUtil.TakeOwnership(@"CLSID\{645FF040-5081-101B-9F08-00AA002F954E}", RegistryHive.ClassesRoot);
                        registryUtil.TakeOwnership(@"CLSID\{645FF040-5081-101B-9F08-00AA002F954E}\shell", RegistryHive.ClassesRoot);
                        registryUtil.TakeOwnership(@"CLSID\{645FF040-5081-101B-9F08-00AA002F954E}\shell\empty", RegistryHive.ClassesRoot);
                        registryUtil.TakeOwnership(@"CLSID\{645FF040-5081-101B-9F08-00AA002F954E}\shell\empty\command", RegistryHive.ClassesRoot);

                        registryUtil.RenameSubKey(baseReg, @"CLSID\{645FF040-5081-101B-9F08-00AA002F954E}\shell\empty", @"CLSID\{645FF040-5081-101B-9F08-00AA002F954E}\shell\pintostartscreen");
                        Log("Disabled Pin to Start for: Recycle Bin!");
                    }

                    // Disable modern share
                    using (var key = baseReg.OpenSubKey(@"*\shellex\ContextMenuHandlers\ModernSharing", true)) {
                        key.SetValue(string.Empty, "-{1d27f844-3a1f-4410-85ac-14651078412d}");
                        Log("Disabled modern share!");
                    }

                    // Disable share menu
                    baseReg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    string regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked";
                    baseReg.CreateSubKey(regKey); // doesn't exist as default, normal behaviour
                    using (var key = baseReg.OpenSubKey(regKey, true)) {
                        key.SetValue("{f81e9010-6ea4-11ce-a7ff-00aa003ca9f6}", string.Empty);
                        Log("Disabled (old) share!");
                    }
                } catch (Exception ex) {
                    Log(ex.ToString());
                    // Ignore errors
                }

                baseReg.Dispose();

                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Enabled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (!string.IsNullOrEmpty(consoleBox.Text))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "Win10Clean - v" + offlineVer + " - " + DateTime.Now.ToString("yyyy/MM/dd HH-mm-ss");
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
            Enabled = true;
        }

        private void btnApps_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                try {
                    using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", true)) {
                        key.SetValue("SilentInstalledAppsEnabled", 0, RegistryValueKind.DWord);
                    }

                    Log("Silent Modern App install disabled");
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Enabled = true;
        }

        private void btnStartAds_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                try {
                    using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", true)) {
                        key.SetValue("SubscribedContent-338388Enabled", 0, RegistryValueKind.DWord);
                    }

                    Log("Start menu ads disabled!");
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /* Other stuff */
        private void RestartExplorer()
        {
            Process[] explorerProcess = Process.GetProcessesByName("explorer");
            foreach (var process in explorerProcess)
            {
                process.Kill();
            }
        }

        public void Log(string msg)
        {
            if (!string.IsNullOrEmpty(msg)) {
                try {
                    consoleBox.Text += msg + Environment.NewLine;
                } catch (Exception) {
                    try {
                        consoleBox.Text += msg + Environment.NewLine;
                    } catch { }
                }
                
            }
        }

        private void CheckTweaks()
        {
            // check defender state
            try {  
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", false)) {
                    if ((int)key.GetValue("DisableAntiSpyware", 0) == 1) {
                        defenderSwitch = true;
                        btnDefender.Text = "Enable Windows Defender";
                    }
                }
            } catch { }

            // is homegroup already disabled?
            var output = CMDHelper.RunCommandReturn("sc query HomeGroupProvider");
            if (output.Contains("1  STOPPED")) {
                HomeGroupBtn.Enabled = false; // TODO: enable reverse
            }

            // check internet connection
            if (!NetworkInterface.GetIsNetworkAvailable()) {
                btnUpdate.Enabled = false;
                Log("Checking for updates is disabled because no internet connection were found!");
            }


        }

        /* Metro related */
        private void UninstallBtn_Click(object sender, EventArgs e)
        {
            Enabled = false;
            string selectedApps = string.Empty;
            string successList = string.Empty;
            string failedList = string.Empty;

            uninstallSuccessList.Clear();
            uninstallFailedList.Clear();

            // Displays all the apps to be uninstalled
            if (appBox.CheckedItems.Count > 0) {
                foreach (string app in appBox.CheckedItems) {
                    selectedApps += app + Environment.NewLine;
                }

                if (MessageBox.Show("Are you sure you want to uninstall the following app(s)?" + Environment.NewLine + selectedApps, "Confirm uninstall", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                    foreach (string app in appBox.CheckedItems) {
                        Task.Run(() => UninstallApp(app));
                    }

                    Thread.Sleep(700); // workaround (kinda dirty) for MessageBox not appearing?!
                    RefreshAppList(true); // refresh list when we're done

                    foreach (var str in uninstallSuccessList) {
                        successList += str + Environment.NewLine;
                    }
                    foreach (var str in uninstallFailedList) {
                        failedList += str + Environment.NewLine;
                    }
                    
                    // Construct message
                    string message = string.Format("App uninstall finished! Of the {0} total app(s), {1} has been uninstalled.",
                        uninstallSuccessList.Count + uninstallFailedList.Count, uninstallSuccessList.Count) + Environment.NewLine + Environment.NewLine;

                    if (uninstallSuccessList.Count != 0) {
                        message += "Successfully uninstalled:" + Environment.NewLine + successList;
                    }

                    if (uninstallFailedList.Count != 0) {
                        message += "Failed uninstall:" + Environment.NewLine + failedList;
                    }

                    MessageBox.Show(message, "Win10Clean", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } else {
                MessageBox.Show("You haven't selected anything!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Enabled = true;
        }

        private void RetrieveApps()
        {
            using (PowerShell script = PowerShell.Create()) {
                if (chkAll.Checked) {
                    script.AddScript("Get-AppxPackage -AllUsers | Select Name | Out-String -Stream");
                } else {
                    script.AddScript("Get-AppxPackage | Select Name | Out-String -Stream");
                }

                string trimmed = string.Empty;
                foreach (PSObject x in script.Invoke()) {
                    trimmed = x.ToString().Trim();
                    if (!string.IsNullOrEmpty(trimmed) && !trimmed.Contains("---")) {
                        if (trimmed != "Name") appBox.Items.Add(trimmed);
                    }
                }
            }
        }

        private void RefreshAppList(bool minusOne)
        {
            appBox.Items.Clear();
            RetrieveApps();

            try {
                if (minusOne) {
                    appBox.SelectedIndex =- 1;
                }
            } catch { }
        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void UninstallApp(string app)
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
                uninstallFailedList.Add(app);
                Log("Could not uninstall app: " + app);
            }
            else
            {
                uninstallSuccessList.Add(app);
                Log("App uninstalled: " + app);
            }

            return;
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            RefreshAppList(false);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Enabled = false;
            RefreshAppList(false);
            Enabled = true;
        }

        private void tabMetro_Enter(object sender, EventArgs e)
        {
            this.Enabled = false;
            RefreshAppList(false);
            this.Enabled = true;
        }

    }
}
