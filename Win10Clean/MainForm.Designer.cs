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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnContext = new System.Windows.Forms.Button();
            this.btnOnedrive = new System.Windows.Forms.Button();
            this.btnStartAds = new System.Windows.Forms.Button();
            this.btnApps = new System.Windows.Forms.Button();
            this.btnHomegroup = new System.Windows.Forms.Button();
            this.btnExplorer = new System.Windows.Forms.Button();
            this.btnDefender = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.consoleBox = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.aboutBox = new System.Windows.Forms.RichTextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(652, 473);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnExit);
            this.tabPage1.Controls.Add(this.btnUpdate);
            this.tabPage1.Controls.Add(this.lblVersion);
            this.tabPage1.Controls.Add(this.btnContext);
            this.tabPage1.Controls.Add(this.btnOnedrive);
            this.tabPage1.Controls.Add(this.btnStartAds);
            this.tabPage1.Controls.Add(this.btnApps);
            this.tabPage1.Controls.Add(this.btnHomegroup);
            this.tabPage1.Controls.Add(this.btnExplorer);
            this.tabPage1.Controls.Add(this.btnDefender);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(644, 440);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Home";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnExit.Location = new System.Drawing.Point(557, 392);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 40);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.button9_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnUpdate.Location = new System.Drawing.Point(472, 392);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(79, 40);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(6, 412);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(68, 20);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Version: ";
            // 
            // btnContext
            // 
            this.btnContext.Location = new System.Drawing.Point(277, 108);
            this.btnContext.Name = "btnContext";
            this.btnContext.Size = new System.Drawing.Size(261, 40);
            this.btnContext.TabIndex = 6;
            this.btnContext.Text = "Cleanup Context Menu";
            this.btnContext.UseVisualStyleBackColor = true;
            this.btnContext.Click += new System.EventHandler(this.btnContext_Click);
            // 
            // btnOnedrive
            // 
            this.btnOnedrive.Location = new System.Drawing.Point(277, 62);
            this.btnOnedrive.Name = "btnOnedrive";
            this.btnOnedrive.Size = new System.Drawing.Size(261, 40);
            this.btnOnedrive.TabIndex = 5;
            this.btnOnedrive.Text = "Uninstall OneDrive";
            this.btnOnedrive.UseVisualStyleBackColor = true;
            this.btnOnedrive.Click += new System.EventHandler(this.btnOnedrive_Click);
            // 
            // btnStartAds
            // 
            this.btnStartAds.Location = new System.Drawing.Point(277, 16);
            this.btnStartAds.Name = "btnStartAds";
            this.btnStartAds.Size = new System.Drawing.Size(261, 40);
            this.btnStartAds.TabIndex = 4;
            this.btnStartAds.Text = "Disable Start Menu ads";
            this.btnStartAds.UseVisualStyleBackColor = true;
            this.btnStartAds.Click += new System.EventHandler(this.btnStartAds_Click);
            // 
            // btnApps
            // 
            this.btnApps.Location = new System.Drawing.Point(10, 154);
            this.btnApps.Name = "btnApps";
            this.btnApps.Size = new System.Drawing.Size(261, 40);
            this.btnApps.TabIndex = 3;
            this.btnApps.Text = "Don\'t reinstall Modern Apps";
            this.btnApps.UseVisualStyleBackColor = true;
            this.btnApps.Click += new System.EventHandler(this.btnApps_Click);
            // 
            // btnHomegroup
            // 
            this.btnHomegroup.Location = new System.Drawing.Point(10, 108);
            this.btnHomegroup.Name = "btnHomegroup";
            this.btnHomegroup.Size = new System.Drawing.Size(261, 40);
            this.btnHomegroup.TabIndex = 2;
            this.btnHomegroup.Text = "Disable HomeGroup";
            this.btnHomegroup.UseVisualStyleBackColor = true;
            this.btnHomegroup.Click += new System.EventHandler(this.btnHomegroup_Click);
            // 
            // btnExplorer
            // 
            this.btnExplorer.Location = new System.Drawing.Point(10, 62);
            this.btnExplorer.Name = "btnExplorer";
            this.btnExplorer.Size = new System.Drawing.Size(261, 40);
            this.btnExplorer.TabIndex = 1;
            this.btnExplorer.Text = "Revert Explorer to Windows 7 Style";
            this.btnExplorer.UseVisualStyleBackColor = true;
            this.btnExplorer.Click += new System.EventHandler(this.btnExplorer_Click);
            // 
            // btnDefender
            // 
            this.btnDefender.Location = new System.Drawing.Point(10, 16);
            this.btnDefender.Name = "btnDefender";
            this.btnDefender.Size = new System.Drawing.Size(261, 40);
            this.btnDefender.TabIndex = 0;
            this.btnDefender.Text = "Disable Windows Defender";
            this.btnDefender.UseVisualStyleBackColor = true;
            this.btnDefender.Click += new System.EventHandler(this.btnDefender_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(636, 420);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Modern Apps";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnExport);
            this.tabPage3.Controls.Add(this.consoleBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(644, 440);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Console";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.aboutBox);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(644, 440);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "About";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // aboutBox
            // 
            this.aboutBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.aboutBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutBox.Location = new System.Drawing.Point(3, 3);
            this.aboutBox.Name = "aboutBox";
            this.aboutBox.ReadOnly = true;
            this.aboutBox.Size = new System.Drawing.Size(638, 434);
            this.aboutBox.TabIndex = 1;
            this.aboutBox.Text = resources.GetString("aboutBox.Text");
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
            this.btnExport.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(652, 473);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Win10Clean";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnContext;
        private System.Windows.Forms.Button btnOnedrive;
        private System.Windows.Forms.Button btnStartAds;
        private System.Windows.Forms.Button btnApps;
        private System.Windows.Forms.Button btnHomegroup;
        private System.Windows.Forms.Button btnExplorer;
        private System.Windows.Forms.Button btnDefender;
        private System.Windows.Forms.RichTextBox consoleBox;
        private System.Windows.Forms.RichTextBox aboutBox;
        private System.Windows.Forms.Button btnExport;
    }
}

