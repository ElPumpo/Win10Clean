namespace Win10Clean
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabHome = new System.Windows.Forms.TabPage();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnContext = new System.Windows.Forms.Button();
            this.OneDriveBtn = new System.Windows.Forms.Button();
            this.btnStartAds = new System.Windows.Forms.Button();
            this.btnApps = new System.Windows.Forms.Button();
            this.Revert7Btn = new System.Windows.Forms.Button();
            this.btnDefender = new System.Windows.Forms.Button();
            this.tabMetro = new System.Windows.Forms.TabPage();
            this.appPanel = new System.Windows.Forms.Panel();
            this.appBox = new System.Windows.Forms.CheckedListBox();
            this.UninstallBtn = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabConsole = new System.Windows.Forms.TabPage();
            this.btnExport = new System.Windows.Forms.Button();
            this.consoleBox = new System.Windows.Forms.RichTextBox();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.aboutBox = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabHome.SuspendLayout();
            this.tabMetro.SuspendLayout();
            this.appPanel.SuspendLayout();
            this.tabConsole.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabHome);
            this.tabControl1.Controls.Add(this.tabMetro);
            this.tabControl1.Controls.Add(this.tabConsole);
            this.tabControl1.Controls.Add(this.tabAbout);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(652, 473);
            this.tabControl1.TabIndex = 8;
            // 
            // tabHome
            // 
            this.tabHome.Controls.Add(this.btnExit);
            this.tabHome.Controls.Add(this.btnUpdate);
            this.tabHome.Controls.Add(this.btnContext);
            this.tabHome.Controls.Add(this.OneDriveBtn);
            this.tabHome.Controls.Add(this.btnStartAds);
            this.tabHome.Controls.Add(this.btnApps);
            this.tabHome.Controls.Add(this.Revert7Btn);
            this.tabHome.Controls.Add(this.btnDefender);
            this.tabHome.Location = new System.Drawing.Point(4, 25);
            this.tabHome.Name = "tabHome";
            this.tabHome.Padding = new System.Windows.Forms.Padding(3);
            this.tabHome.Size = new System.Drawing.Size(644, 444);
            this.tabHome.TabIndex = 0;
            this.tabHome.Text = "Home";
            this.tabHome.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnExit.Location = new System.Drawing.Point(557, 392);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 40);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnUpdate.Location = new System.Drawing.Point(472, 392);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(79, 40);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnContext
            // 
            this.btnContext.Location = new System.Drawing.Point(325, 225);
            this.btnContext.Name = "btnContext";
            this.btnContext.Size = new System.Drawing.Size(261, 40);
            this.btnContext.TabIndex = 5;
            this.btnContext.Text = "Cleanup Context Menus";
            this.toolTip1.SetToolTip(this.btnContext, resources.GetString("btnContext.ToolTip"));
            this.btnContext.UseVisualStyleBackColor = true;
            this.btnContext.Click += new System.EventHandler(this.btnContext_Click);
            // 
            // OneDriveBtn
            // 
            this.OneDriveBtn.Location = new System.Drawing.Point(325, 179);
            this.OneDriveBtn.Name = "OneDriveBtn";
            this.OneDriveBtn.Size = new System.Drawing.Size(261, 40);
            this.OneDriveBtn.TabIndex = 3;
            this.OneDriveBtn.Text = "Uninstall OneDrive";
            this.toolTip1.SetToolTip(this.OneDriveBtn, "- runs onedrive uninstall setup\r\n- remove all onedrive directories\r\n- remove oned" +
        "rive from explorer\r\n- remove onedrive startup object\r\n- remove tasks");
            this.OneDriveBtn.UseVisualStyleBackColor = true;
            this.OneDriveBtn.Click += new System.EventHandler(this.OneDriveBtn_Click);
            // 
            // btnStartAds
            // 
            this.btnStartAds.Location = new System.Drawing.Point(325, 133);
            this.btnStartAds.Name = "btnStartAds";
            this.btnStartAds.Size = new System.Drawing.Size(261, 40);
            this.btnStartAds.TabIndex = 1;
            this.btnStartAds.Text = "Disable start menu ads";
            this.toolTip1.SetToolTip(this.btnStartAds, "disable start menu ads, not sure if it\'s still relevant / needed");
            this.btnStartAds.UseVisualStyleBackColor = true;
            this.btnStartAds.Click += new System.EventHandler(this.btnStartAds_Click);
            // 
            // btnApps
            // 
            this.btnApps.Location = new System.Drawing.Point(58, 225);
            this.btnApps.Name = "btnApps";
            this.btnApps.Size = new System.Drawing.Size(261, 40);
            this.btnApps.TabIndex = 4;
            this.btnApps.Text = "Don\'t reinstall modern apps";
            this.toolTip1.SetToolTip(this.btnApps, "don\'t reinstall apps that come preinstalled with windows if you recently uninstal" +
        "led them");
            this.btnApps.UseVisualStyleBackColor = true;
            this.btnApps.Click += new System.EventHandler(this.btnApps_Click);
            // 
            // Revert7Btn
            // 
            this.Revert7Btn.Location = new System.Drawing.Point(58, 179);
            this.Revert7Btn.Name = "Revert7Btn";
            this.Revert7Btn.Size = new System.Drawing.Size(261, 40);
            this.Revert7Btn.TabIndex = 2;
            this.Revert7Btn.Text = "Revert Explorer to Windows 7 Style";
            this.toolTip1.SetToolTip(this.Revert7Btn, resources.GetString("Revert7Btn.ToolTip"));
            this.Revert7Btn.UseVisualStyleBackColor = true;
            this.Revert7Btn.Click += new System.EventHandler(this.Revert7Btn_Click);
            // 
            // btnDefender
            // 
            this.btnDefender.Location = new System.Drawing.Point(58, 133);
            this.btnDefender.Name = "btnDefender";
            this.btnDefender.Size = new System.Drawing.Size(261, 40);
            this.btnDefender.TabIndex = 0;
            this.btnDefender.Text = "Disable Windows Defender";
            this.toolTip1.SetToolTip(this.btnDefender, "- disable active anti malware protection\r\n- delete from startup list\r\n- unregiste" +
        "r shell extension");
            this.btnDefender.UseVisualStyleBackColor = true;
            this.btnDefender.Click += new System.EventHandler(this.btnDefender_Click);
            // 
            // tabMetro
            // 
            this.tabMetro.Controls.Add(this.appPanel);
            this.tabMetro.Controls.Add(this.UninstallBtn);
            this.tabMetro.Controls.Add(this.btnRefresh);
            this.tabMetro.Location = new System.Drawing.Point(4, 25);
            this.tabMetro.Name = "tabMetro";
            this.tabMetro.Padding = new System.Windows.Forms.Padding(3);
            this.tabMetro.Size = new System.Drawing.Size(644, 444);
            this.tabMetro.TabIndex = 1;
            this.tabMetro.Text = "Windows 10 Native Apps";
            this.tabMetro.UseVisualStyleBackColor = true;
            this.tabMetro.Enter += new System.EventHandler(this.tabMetro_Enter);
            // 
            // appPanel
            // 
            this.appPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.appPanel.Controls.Add(this.appBox);
            this.appPanel.Location = new System.Drawing.Point(6, 6);
            this.appPanel.Name = "appPanel";
            this.appPanel.Size = new System.Drawing.Size(468, 426);
            this.appPanel.TabIndex = 12;
            // 
            // appBox
            // 
            this.appBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.appBox.CheckOnClick = true;
            this.appBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appBox.FormattingEnabled = true;
            this.appBox.Location = new System.Drawing.Point(0, 0);
            this.appBox.Name = "appBox";
            this.appBox.Size = new System.Drawing.Size(466, 424);
            this.appBox.Sorted = true;
            this.appBox.TabIndex = 0;
            this.appBox.UseCompatibleTextRendering = true;
            // 
            // UninstallBtn
            // 
            this.UninstallBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UninstallBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.UninstallBtn.Location = new System.Drawing.Point(480, 135);
            this.UninstallBtn.Name = "UninstallBtn";
            this.UninstallBtn.Size = new System.Drawing.Size(79, 40);
            this.UninstallBtn.TabIndex = 11;
            this.UninstallBtn.Text = "Uninstall";
            this.UninstallBtn.UseVisualStyleBackColor = true;
            this.UninstallBtn.Click += new System.EventHandler(this.UninstallBtn_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnRefresh.Location = new System.Drawing.Point(480, 89);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(79, 40);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tabConsole
            // 
            this.tabConsole.Controls.Add(this.btnExport);
            this.tabConsole.Controls.Add(this.consoleBox);
            this.tabConsole.Location = new System.Drawing.Point(4, 25);
            this.tabConsole.Name = "tabConsole";
            this.tabConsole.Padding = new System.Windows.Forms.Padding(3);
            this.tabConsole.Size = new System.Drawing.Size(644, 444);
            this.tabConsole.TabIndex = 2;
            this.tabConsole.Text = "Console";
            this.tabConsole.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnExport.Location = new System.Drawing.Point(557, 392);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(79, 40);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // consoleBox
            // 
            this.consoleBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.consoleBox.Location = new System.Drawing.Point(8, 6);
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.ReadOnly = true;
            this.consoleBox.Size = new System.Drawing.Size(628, 380);
            this.consoleBox.TabIndex = 0;
            this.consoleBox.Text = "";
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.aboutBox);
            this.tabAbout.Location = new System.Drawing.Point(4, 25);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(644, 444);
            this.tabAbout.TabIndex = 3;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // aboutBox
            // 
            this.aboutBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.aboutBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutBox.Location = new System.Drawing.Point(3, 3);
            this.aboutBox.Name = "aboutBox";
            this.aboutBox.ReadOnly = true;
            this.aboutBox.Size = new System.Drawing.Size(638, 438);
            this.aboutBox.TabIndex = 1;
            this.aboutBox.Text = resources.GetString("aboutBox.Text");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(652, 473);
            this.Controls.Add(this.tabControl1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Win10Clean";
            this.toolTip1.SetToolTip(this, "- runs onedrive uninstall setup\r\n- remove all onedrive directories\r\n- remove oned" +
        "rive from explorer\r\n- remove onedrive startup object\r\n- remove tasks");
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabHome.ResumeLayout(false);
            this.tabMetro.ResumeLayout(false);
            this.appPanel.ResumeLayout(false);
            this.tabConsole.ResumeLayout(false);
            this.tabAbout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabHome;
        private System.Windows.Forms.TabPage tabMetro;
        private System.Windows.Forms.TabPage tabConsole;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnContext;
        private System.Windows.Forms.Button OneDriveBtn;
        private System.Windows.Forms.Button btnStartAds;
        private System.Windows.Forms.Button btnApps;
        private System.Windows.Forms.Button Revert7Btn;
        private System.Windows.Forms.Button btnDefender;
        private System.Windows.Forms.RichTextBox consoleBox;
        private System.Windows.Forms.RichTextBox aboutBox;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel appPanel;
        private System.Windows.Forms.Button UninstallBtn;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckedListBox appBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

