namespace Trump_s_Cyber_Security_Firewall_TM
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
            this.TxtBoxInfo = new System.Windows.Forms.TextBox();
            this.LblInfo = new System.Windows.Forms.Label();
            this.TxtBoxUsers = new System.Windows.Forms.TextBox();
            this.BtnSecure = new System.Windows.Forms.Button();
            this.Tltp_Btns = new System.Windows.Forms.ToolTip(this.components);
            this.BtnStop = new System.Windows.Forms.Button();
            this.TxtBoxPass = new System.Windows.Forms.TextBox();
            this.LblNewPass = new System.Windows.Forms.Label();
            this.LblAuthUser = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabPgMain = new System.Windows.Forms.TabPage();
            this.TabPgConfig = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.ChkLstBoxInstall = new System.Windows.Forms.CheckedListBox();
            this.ChkForceReinstall = new System.Windows.Forms.CheckBox();
            this.ChkRDP = new System.Windows.Forms.CheckBox();
            this.LblProgramList = new System.Windows.Forms.Label();
            this.ChkLstBoxUninstall = new System.Windows.Forms.CheckedListBox();
            this.ChkDebug = new System.Windows.Forms.CheckBox();
            this.TabPgTools = new System.Windows.Forms.TabPage();
            this.CmboBoxGroups = new System.Windows.Forms.ComboBox();
            this.TxtBoxGroupInfo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtBoxFileInfo = new System.Windows.Forms.TextBox();
            this.LblBrowseFile = new System.Windows.Forms.Label();
            this.TxtBoxBrowseFile = new System.Windows.Forms.TextBox();
            this.BtnBrowseFile = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.TabPgMain.SuspendLayout();
            this.TabPgConfig.SuspendLayout();
            this.TabPgTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtBoxInfo
            // 
            this.TxtBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBoxInfo.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtBoxInfo.Location = new System.Drawing.Point(6, 335);
            this.TxtBoxInfo.Multiline = true;
            this.TxtBoxInfo.Name = "TxtBoxInfo";
            this.TxtBoxInfo.ReadOnly = true;
            this.TxtBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxInfo.Size = new System.Drawing.Size(465, 162);
            this.TxtBoxInfo.TabIndex = 1;
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LblInfo.AutoSize = true;
            this.LblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblInfo.Location = new System.Drawing.Point(6, 310);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(30, 15);
            this.LblInfo.TabIndex = 2;
            this.LblInfo.Text = "Info:";
            // 
            // TxtBoxUsers
            // 
            this.TxtBoxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBoxUsers.Location = new System.Drawing.Point(248, 23);
            this.TxtBoxUsers.Multiline = true;
            this.TxtBoxUsers.Name = "TxtBoxUsers";
            this.TxtBoxUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxUsers.Size = new System.Drawing.Size(218, 222);
            this.TxtBoxUsers.TabIndex = 4;
            // 
            // BtnSecure
            // 
            this.BtnSecure.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSecure.Enabled = false;
            this.BtnSecure.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSecure.ForeColor = System.Drawing.Color.Red;
            this.BtnSecure.Location = new System.Drawing.Point(6, 3);
            this.BtnSecure.Name = "BtnSecure";
            this.BtnSecure.Size = new System.Drawing.Size(300, 300);
            this.BtnSecure.TabIndex = 5;
            this.BtnSecure.Text = "Build the wall!";
            this.Tltp_Btns.SetToolTip(this.BtnSecure, "Configures system for optimal security.\r\nAlso installs anti-malaware and enables " +
        "security features.");
            this.BtnSecure.UseVisualStyleBackColor = false;
            this.BtnSecure.Click += new System.EventHandler(this.BtnSecure_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStop.Enabled = false;
            this.BtnStop.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStop.ForeColor = System.Drawing.Color.Red;
            this.BtnStop.Location = new System.Drawing.Point(312, 3);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(158, 301);
            this.BtnStop.TabIndex = 10;
            this.BtnStop.Text = "Abandon office";
            this.Tltp_Btns.SetToolTip(this.BtnStop, "Stops building the wall.");
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // TxtBoxPass
            // 
            this.TxtBoxPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBoxPass.Location = new System.Drawing.Point(248, 285);
            this.TxtBoxPass.Name = "TxtBoxPass";
            this.TxtBoxPass.Size = new System.Drawing.Size(218, 20);
            this.TxtBoxPass.TabIndex = 7;
            // 
            // LblNewPass
            // 
            this.LblNewPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblNewPass.AutoSize = true;
            this.LblNewPass.Location = new System.Drawing.Point(246, 268);
            this.LblNewPass.Name = "LblNewPass";
            this.LblNewPass.Size = new System.Drawing.Size(154, 13);
            this.LblNewPass.TabIndex = 8;
            this.LblNewPass.Text = "New Password (for other users)";
            // 
            // LblAuthUser
            // 
            this.LblAuthUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblAuthUser.AutoSize = true;
            this.LblAuthUser.Location = new System.Drawing.Point(246, 6);
            this.LblAuthUser.Name = "LblAuthUser";
            this.LblAuthUser.Size = new System.Drawing.Size(180, 13);
            this.LblAuthUser.TabIndex = 9;
            this.LblAuthUser.Text = "Authorized Users (list from README)";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(41, 310);
            this.ProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(429, 19);
            this.ProgressBar.TabIndex = 11;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.TabPgMain);
            this.tabControl1.Controls.Add(this.TabPgConfig);
            this.tabControl1.Controls.Add(this.TabPgTools);
            this.tabControl1.Location = new System.Drawing.Point(10, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(482, 526);
            this.tabControl1.TabIndex = 12;
            // 
            // TabPgMain
            // 
            this.TabPgMain.Controls.Add(this.ProgressBar);
            this.TabPgMain.Controls.Add(this.BtnSecure);
            this.TabPgMain.Controls.Add(this.BtnStop);
            this.TabPgMain.Controls.Add(this.TxtBoxInfo);
            this.TabPgMain.Controls.Add(this.LblInfo);
            this.TabPgMain.Location = new System.Drawing.Point(4, 22);
            this.TabPgMain.Name = "TabPgMain";
            this.TabPgMain.Padding = new System.Windows.Forms.Padding(3);
            this.TabPgMain.Size = new System.Drawing.Size(474, 500);
            this.TabPgMain.TabIndex = 0;
            this.TabPgMain.Text = "Main";
            this.TabPgMain.UseVisualStyleBackColor = true;
            // 
            // TabPgConfig
            // 
            this.TabPgConfig.Controls.Add(this.label2);
            this.TabPgConfig.Controls.Add(this.ChkLstBoxInstall);
            this.TabPgConfig.Controls.Add(this.ChkForceReinstall);
            this.TabPgConfig.Controls.Add(this.ChkRDP);
            this.TabPgConfig.Controls.Add(this.LblProgramList);
            this.TabPgConfig.Controls.Add(this.ChkLstBoxUninstall);
            this.TabPgConfig.Controls.Add(this.ChkDebug);
            this.TabPgConfig.Controls.Add(this.LblAuthUser);
            this.TabPgConfig.Controls.Add(this.TxtBoxUsers);
            this.TabPgConfig.Controls.Add(this.LblNewPass);
            this.TabPgConfig.Controls.Add(this.TxtBoxPass);
            this.TabPgConfig.Location = new System.Drawing.Point(4, 22);
            this.TabPgConfig.Name = "TabPgConfig";
            this.TabPgConfig.Padding = new System.Windows.Forms.Padding(3);
            this.TabPgConfig.Size = new System.Drawing.Size(474, 500);
            this.TabPgConfig.TabIndex = 1;
            this.TabPgConfig.Text = "Config";
            this.TabPgConfig.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 323);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Check programs to install";
            // 
            // ChkLstBoxInstall
            // 
            this.ChkLstBoxInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChkLstBoxInstall.FormattingEnabled = true;
            this.ChkLstBoxInstall.HorizontalScrollbar = true;
            this.ChkLstBoxInstall.Location = new System.Drawing.Point(22, 340);
            this.ChkLstBoxInstall.Name = "ChkLstBoxInstall";
            this.ChkLstBoxInstall.Size = new System.Drawing.Size(218, 154);
            this.ChkLstBoxInstall.TabIndex = 15;
            // 
            // ChkForceReinstall
            // 
            this.ChkForceReinstall.AutoSize = true;
            this.ChkForceReinstall.Location = new System.Drawing.Point(6, 47);
            this.ChkForceReinstall.Margin = new System.Windows.Forms.Padding(2);
            this.ChkForceReinstall.Name = "ChkForceReinstall";
            this.ChkForceReinstall.Size = new System.Drawing.Size(138, 17);
            this.ChkForceReinstall.TabIndex = 14;
            this.ChkForceReinstall.Text = "Force Program Reinstall";
            this.ChkForceReinstall.UseVisualStyleBackColor = true;
            // 
            // ChkRDP
            // 
            this.ChkRDP.AutoSize = true;
            this.ChkRDP.Location = new System.Drawing.Point(6, 26);
            this.ChkRDP.Margin = new System.Windows.Forms.Padding(2);
            this.ChkRDP.Name = "ChkRDP";
            this.ChkRDP.Size = new System.Drawing.Size(142, 17);
            this.ChkRDP.TabIndex = 13;
            this.ChkRDP.Text = "Enable Remote Desktop";
            this.ChkRDP.UseVisualStyleBackColor = true;
            // 
            // LblProgramList
            // 
            this.LblProgramList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblProgramList.AutoSize = true;
            this.LblProgramList.Location = new System.Drawing.Point(246, 323);
            this.LblProgramList.Name = "LblProgramList";
            this.LblProgramList.Size = new System.Drawing.Size(137, 13);
            this.LblProgramList.TabIndex = 12;
            this.LblProgramList.Text = "Check programs to uninstall";
            // 
            // ChkLstBoxUninstall
            // 
            this.ChkLstBoxUninstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkLstBoxUninstall.FormattingEnabled = true;
            this.ChkLstBoxUninstall.HorizontalScrollbar = true;
            this.ChkLstBoxUninstall.Location = new System.Drawing.Point(248, 340);
            this.ChkLstBoxUninstall.Name = "ChkLstBoxUninstall";
            this.ChkLstBoxUninstall.Size = new System.Drawing.Size(218, 154);
            this.ChkLstBoxUninstall.TabIndex = 11;
            // 
            // ChkDebug
            // 
            this.ChkDebug.AutoSize = true;
            this.ChkDebug.Location = new System.Drawing.Point(6, 6);
            this.ChkDebug.Name = "ChkDebug";
            this.ChkDebug.Size = new System.Drawing.Size(114, 17);
            this.ChkDebug.TabIndex = 10;
            this.ChkDebug.Text = "Enable Debugging";
            this.ChkDebug.UseVisualStyleBackColor = true;
            // 
            // TabPgTools
            // 
            this.TabPgTools.Controls.Add(this.CmboBoxGroups);
            this.TabPgTools.Controls.Add(this.TxtBoxGroupInfo);
            this.TabPgTools.Controls.Add(this.label1);
            this.TabPgTools.Controls.Add(this.TxtBoxFileInfo);
            this.TabPgTools.Controls.Add(this.LblBrowseFile);
            this.TabPgTools.Controls.Add(this.TxtBoxBrowseFile);
            this.TabPgTools.Controls.Add(this.BtnBrowseFile);
            this.TabPgTools.Location = new System.Drawing.Point(4, 22);
            this.TabPgTools.Name = "TabPgTools";
            this.TabPgTools.Size = new System.Drawing.Size(474, 500);
            this.TabPgTools.TabIndex = 2;
            this.TabPgTools.Text = "Tools";
            this.TabPgTools.UseVisualStyleBackColor = true;
            // 
            // CmboBoxGroups
            // 
            this.CmboBoxGroups.FormattingEnabled = true;
            this.CmboBoxGroups.Location = new System.Drawing.Point(245, 16);
            this.CmboBoxGroups.Name = "CmboBoxGroups";
            this.CmboBoxGroups.Size = new System.Drawing.Size(226, 21);
            this.CmboBoxGroups.TabIndex = 25;
            this.CmboBoxGroups.SelectedIndexChanged += new System.EventHandler(this.CmboBoxGroups_SelectedIndexChanged);
            // 
            // TxtBoxGroupInfo
            // 
            this.TxtBoxGroupInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TxtBoxGroupInfo.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtBoxGroupInfo.Location = new System.Drawing.Point(245, 42);
            this.TxtBoxGroupInfo.Multiline = true;
            this.TxtBoxGroupInfo.Name = "TxtBoxGroupInfo";
            this.TxtBoxGroupInfo.ReadOnly = true;
            this.TxtBoxGroupInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtBoxGroupInfo.Size = new System.Drawing.Size(226, 247);
            this.TxtBoxGroupInfo.TabIndex = 24;
            this.TxtBoxGroupInfo.WordWrap = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(242, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "CIA Identity Detector";
            // 
            // TxtBoxFileInfo
            // 
            this.TxtBoxFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TxtBoxFileInfo.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtBoxFileInfo.Location = new System.Drawing.Point(3, 42);
            this.TxtBoxFileInfo.Multiline = true;
            this.TxtBoxFileInfo.Name = "TxtBoxFileInfo";
            this.TxtBoxFileInfo.ReadOnly = true;
            this.TxtBoxFileInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtBoxFileInfo.Size = new System.Drawing.Size(226, 247);
            this.TxtBoxFileInfo.TabIndex = 21;
            this.TxtBoxFileInfo.WordWrap = false;
            // 
            // LblBrowseFile
            // 
            this.LblBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBrowseFile.AutoSize = true;
            this.LblBrowseFile.Location = new System.Drawing.Point(1, 3);
            this.LblBrowseFile.Name = "LblBrowseFile";
            this.LblBrowseFile.Size = new System.Drawing.Size(177, 13);
            this.LblBrowseFile.TabIndex = 20;
            this.LblBrowseFile.Text = "Trump\'s Top Secret Email Decryptor";
            // 
            // TxtBoxBrowseFile
            // 
            this.TxtBoxBrowseFile.AllowDrop = true;
            this.TxtBoxBrowseFile.Location = new System.Drawing.Point(61, 19);
            this.TxtBoxBrowseFile.Margin = new System.Windows.Forms.Padding(2);
            this.TxtBoxBrowseFile.Name = "TxtBoxBrowseFile";
            this.TxtBoxBrowseFile.Size = new System.Drawing.Size(168, 20);
            this.TxtBoxBrowseFile.TabIndex = 19;
            this.TxtBoxBrowseFile.TextChanged += new System.EventHandler(this.TxtBoxBrowseFile_TextChanged);
            // 
            // BtnBrowseFile
            // 
            this.BtnBrowseFile.Location = new System.Drawing.Point(3, 19);
            this.BtnBrowseFile.Margin = new System.Windows.Forms.Padding(2);
            this.BtnBrowseFile.Name = "BtnBrowseFile";
            this.BtnBrowseFile.Size = new System.Drawing.Size(56, 19);
            this.BtnBrowseFile.TabIndex = 18;
            this.BtnBrowseFile.Text = "Browse...";
            this.BtnBrowseFile.UseVisualStyleBackColor = true;
            this.BtnBrowseFile.Click += new System.EventHandler(this.BtnBrowseFile_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.InitialDirectory = "C:\\Users";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 548);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Trump\'s Cyber Security Firewall TM";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this.TabPgMain.ResumeLayout(false);
            this.TabPgMain.PerformLayout();
            this.TabPgConfig.ResumeLayout(false);
            this.TabPgConfig.PerformLayout();
            this.TabPgTools.ResumeLayout(false);
            this.TabPgTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox TxtBoxInfo;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.TextBox TxtBoxUsers;
        private System.Windows.Forms.Button BtnSecure;
        private System.Windows.Forms.ToolTip Tltp_Btns;
        private System.Windows.Forms.TextBox TxtBoxPass;
        private System.Windows.Forms.Label LblNewPass;
        private System.Windows.Forms.Label LblAuthUser;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabPgMain;
        private System.Windows.Forms.TabPage TabPgConfig;
        private System.Windows.Forms.Label LblProgramList;
        private System.Windows.Forms.CheckBox ChkDebug;
        private System.Windows.Forms.CheckBox ChkRDP;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage TabPgTools;
        private System.Windows.Forms.TextBox TxtBoxFileInfo;
        private System.Windows.Forms.Label LblBrowseFile;
        private System.Windows.Forms.TextBox TxtBoxBrowseFile;
        private System.Windows.Forms.Button BtnBrowseFile;
        private System.Windows.Forms.TextBox TxtBoxGroupInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CmboBoxGroups;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox ChkLstBoxUninstall;
        private System.Windows.Forms.CheckedListBox ChkLstBoxInstall;
        public System.Windows.Forms.CheckBox ChkForceReinstall;
    }
}

