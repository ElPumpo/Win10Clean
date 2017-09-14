using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;
using System.IO;
using System.Diagnostics;

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
            catch (Exception ex)
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
                RegistryKey k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", false);
                if ((int)k.GetValue("SystemPaneSuggestionsEnabled", 1) == 0)
                {
                    _adsSwitch = 1;
                    btnStartAds.Text = "Enable Start Menu ads";
                    _adsMessage = "Start Menu ads enabled";
                }

                //  check defender state
                k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", false);
                if ((int)k.GetValue("DisableAntiSpyware", 0) == 1)
                {
                    _defenderSwitch = true;
                    btnDefender.Text = "Enable Windows Defender";
                }

                k.Close();
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

        }

        private void CleanupContextMenu()
        {

        }

        private void DisableStartMenuAds()
        {
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    RegistryKey k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", true);
                    k.SetValue("SystemPaneSuggestionsEnabled", _adsSwitch);

                    Log(_adsMessage);
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!_defenderSwitch)
                {
                    try
                    {
                        RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true);
                        k.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
                        Log("Main Windows Defender functions disabled");

                        k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                        k.DeleteValue("WindowsDefender", false);
                        k.DeleteValue("SecurityHealth", false);
                        Log("Windows Defender removed from startup");

                        RunCommand("regsvr32 /u /s \"C:\\Program Files\\Windows Defender\\shellext.dll\"");

                        Log("Windows Defender shell addons unregistered");
                        k.Close();

                        MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Log(ex.ToString());
                        MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true);
                        k.SetValue("DisableAntiSpyware", 0, RegistryValueKind.DWord);
                        Log("Main Windows Defender functions enabled");
                        k.Close();

                        MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Log(ex.ToString());
                        MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
         
        private void DisableSilentInstall()
        {
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    RegistryKey k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", true);
                    k.SetValue("SilentInstalledAppsEnabled", _adsSwitch, RegistryValueKind.DWord);
                    k.Close();

                    Log("Silent Modern App install disabled");
                    MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                RegistryKey k;

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
                }

                string pinLib = @"Software\Classes\CLSID\{031E4825-7B94-4dc3-B131-E946B44C8DD5}";

                try
                {
                    k = Registry.CurrentUser.OpenSubKey(pinLib, true);
                    k.SetValue("System.IsPinnedToNameSpaceTree", 1, RegistryValueKind.DWord);
                    Log("Library folders pinned");
                }
                catch (NullReferenceException)
                {
                    Registry.CurrentUser.CreateSubKey(pinLib);
                    //RevertExplorer();
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    MessageBox.Show(ex.ToString(), ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                RestartExplorer();

                MessageBox.Show("OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
