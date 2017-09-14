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
            }
            catch { }
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

                        Process p = new Process();
                        p.StartInfo.FileName = "cmd.exe";
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardInput = true;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.Start();

                        //p.StandardInput.WriteLine("regsvr32 /u /s ""C:\\Program Files\\Windows Defender\\shellext.dll""");
                        p.StandardInput.Close();
                        p.WaitForExit();

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private void btnDefender_Click(object sender, EventArgs e)
        {
            TweakDefender();
        }
    }
}
