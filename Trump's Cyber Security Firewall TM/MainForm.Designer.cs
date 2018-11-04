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
            this.TabPgConfig = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TxtBoxFileInfo = new System.Windows.Forms.TextBox();
            this.LblBrowseFile = new System.Windows.Forms.Label();
            this.TxtBoxBrowseFile = new System.Windows.Forms.TextBox();
            this.BtnBrowseFile = new System.Windows.Forms.Button();
            this.ChkRDP = new System.Windows.Forms.CheckBox();
            this.LblProgramList = new System.Windows.Forms.Label();
            this.ChkLstBxPrograms = new System.Windows.Forms.CheckedListBox();
            this.ChkDebug = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.TabPgConfig.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtBoxInfo
            // 
            this.TxtBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBoxInfo.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtBoxInfo.Location = new System.Drawing.Point(8, 412);
            this.TxtBoxInfo.Margin = new System.Windows.Forms.Padding(4);
            this.TxtBoxInfo.Multiline = true;
            this.TxtBoxInfo.Name = "TxtBoxInfo";
            this.TxtBoxInfo.ReadOnly = true;
            this.TxtBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxInfo.Size = new System.Drawing.Size(619, 198);
            this.TxtBoxInfo.TabIndex = 1;
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LblInfo.AutoSize = true;
            this.LblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblInfo.Location = new System.Drawing.Point(8, 382);
            this.LblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(36, 18);
            this.LblInfo.TabIndex = 2;
            this.LblInfo.Text = "Info:";
            // 
            // TxtBoxUsers
            // 
            this.TxtBoxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBoxUsers.Location = new System.Drawing.Point(331, 28);
            this.TxtBoxUsers.Margin = new System.Windows.Forms.Padding(4);
            this.TxtBoxUsers.Multiline = true;
            this.TxtBoxUsers.Name = "TxtBoxUsers";
            this.TxtBoxUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxUsers.Size = new System.Drawing.Size(290, 272);
            this.TxtBoxUsers.TabIndex = 4;
            // 
            // BtnSecure
            // 
            this.BtnSecure.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSecure.Enabled = false;
            this.BtnSecure.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSecure.ForeColor = System.Drawing.Color.Red;
            this.BtnSecure.Location = new System.Drawing.Point(8, 4);
            this.BtnSecure.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSecure.Name = "BtnSecure";
            this.BtnSecure.Size = new System.Drawing.Size(400, 369);
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
            this.BtnStop.Location = new System.Drawing.Point(416, 4);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(4);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(211, 370);
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
            this.TxtBoxPass.Location = new System.Drawing.Point(331, 351);
            this.TxtBoxPass.Margin = new System.Windows.Forms.Padding(4);
            this.TxtBoxPass.Name = "TxtBoxPass";
            this.TxtBoxPass.Size = new System.Drawing.Size(290, 22);
            this.TxtBoxPass.TabIndex = 7;
            // 
            // LblNewPass
            // 
            this.LblNewPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblNewPass.AutoSize = true;
            this.LblNewPass.Location = new System.Drawing.Point(328, 330);
            this.LblNewPass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblNewPass.Name = "LblNewPass";
            this.LblNewPass.Size = new System.Drawing.Size(207, 17);
            this.LblNewPass.TabIndex = 8;
            this.LblNewPass.Text = "New Password (for other users)";
            // 
            // LblAuthUser
            // 
            this.LblAuthUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblAuthUser.AutoSize = true;
            this.LblAuthUser.Location = new System.Drawing.Point(328, 7);
            this.LblAuthUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblAuthUser.Name = "LblAuthUser";
            this.LblAuthUser.Size = new System.Drawing.Size(242, 17);
            this.LblAuthUser.TabIndex = 9;
            this.LblAuthUser.Text = "Authorized Users (list from README)";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(55, 382);
            this.ProgressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(572, 23);
            this.ProgressBar.TabIndex = 11;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.TabPgConfig);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 14);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(643, 647);
            this.tabControl1.TabIndex = 12;
            // 
            // TabPgConfig
            // 
            this.TabPgConfig.Controls.Add(this.ProgressBar);
            this.TabPgConfig.Controls.Add(this.BtnSecure);
            this.TabPgConfig.Controls.Add(this.BtnStop);
            this.TabPgConfig.Controls.Add(this.TxtBoxInfo);
            this.TabPgConfig.Controls.Add(this.LblInfo);
            this.TabPgConfig.Location = new System.Drawing.Point(4, 25);
            this.TabPgConfig.Margin = new System.Windows.Forms.Padding(4);
            this.TabPgConfig.Name = "TabPgConfig";
            this.TabPgConfig.Padding = new System.Windows.Forms.Padding(4);
            this.TabPgConfig.Size = new System.Drawing.Size(635, 618);
            this.TabPgConfig.TabIndex = 0;
            this.TabPgConfig.Text = "Main";
            this.TabPgConfig.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TxtBoxFileInfo);
            this.tabPage2.Controls.Add(this.LblBrowseFile);
            this.tabPage2.Controls.Add(this.TxtBoxBrowseFile);
            this.tabPage2.Controls.Add(this.BtnBrowseFile);
            this.tabPage2.Controls.Add(this.ChkRDP);
            this.tabPage2.Controls.Add(this.LblProgramList);
            this.tabPage2.Controls.Add(this.ChkLstBxPrograms);
            this.tabPage2.Controls.Add(this.ChkDebug);
            this.tabPage2.Controls.Add(this.LblAuthUser);
            this.tabPage2.Controls.Add(this.TxtBoxUsers);
            this.tabPage2.Controls.Add(this.LblNewPass);
            this.tabPage2.Controls.Add(this.TxtBoxPass);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(635, 618);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TxtBoxFileInfo
            // 
            this.TxtBoxFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TxtBoxFileInfo.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtBoxFileInfo.Location = new System.Drawing.Point(8, 307);
            this.TxtBoxFileInfo.Margin = new System.Windows.Forms.Padding(4);
            this.TxtBoxFileInfo.Multiline = true;
            this.TxtBoxFileInfo.Name = "TxtBoxFileInfo";
            this.TxtBoxFileInfo.ReadOnly = true;
            this.TxtBoxFileInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtBoxFileInfo.Size = new System.Drawing.Size(300, 303);
            this.TxtBoxFileInfo.TabIndex = 17;
            this.TxtBoxFileInfo.WordWrap = false;
            // 
            // LblBrowseFile
            // 
            this.LblBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBrowseFile.AutoSize = true;
            this.LblBrowseFile.Location = new System.Drawing.Point(5, 258);
            this.LblBrowseFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblBrowseFile.Name = "LblBrowseFile";
            this.LblBrowseFile.Size = new System.Drawing.Size(237, 17);
            this.LblBrowseFile.TabIndex = 16;
            this.LblBrowseFile.Text = "Trump\'s Top Secret Email Decryptor";
            // 
            // TxtBoxBrowseFile
            // 
            this.TxtBoxBrowseFile.AllowDrop = true;
            this.TxtBoxBrowseFile.Location = new System.Drawing.Point(86, 278);
            this.TxtBoxBrowseFile.Name = "TxtBoxBrowseFile";
            this.TxtBoxBrowseFile.Size = new System.Drawing.Size(222, 22);
            this.TxtBoxBrowseFile.TabIndex = 15;
            this.TxtBoxBrowseFile.TextChanged += new System.EventHandler(this.TxtBoxBrowseFile_TextChanged);
            // 
            // BtnBrowseFile
            // 
            this.BtnBrowseFile.Location = new System.Drawing.Point(5, 277);
            this.BtnBrowseFile.Name = "BtnBrowseFile";
            this.BtnBrowseFile.Size = new System.Drawing.Size(75, 23);
            this.BtnBrowseFile.TabIndex = 14;
            this.BtnBrowseFile.Text = "Browse...";
            this.BtnBrowseFile.UseVisualStyleBackColor = true;
            this.BtnBrowseFile.Click += new System.EventHandler(this.BtnBrowseFile_Click);
            // 
            // ChkRDP
            // 
            this.ChkRDP.AutoSize = true;
            this.ChkRDP.Location = new System.Drawing.Point(8, 32);
            this.ChkRDP.Name = "ChkRDP";
            this.ChkRDP.Size = new System.Drawing.Size(183, 21);
            this.ChkRDP.TabIndex = 13;
            this.ChkRDP.Text = "Enable Remote Desktop";
            this.ChkRDP.UseVisualStyleBackColor = true;
            // 
            // LblProgramList
            // 
            this.LblProgramList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblProgramList.AutoSize = true;
            this.LblProgramList.Location = new System.Drawing.Point(328, 398);
            this.LblProgramList.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblProgramList.Name = "LblProgramList";
            this.LblProgramList.Size = new System.Drawing.Size(183, 17);
            this.LblProgramList.TabIndex = 12;
            this.LblProgramList.Text = "Check programs to uninstall";
            // 
            // ChkLstBxPrograms
            // 
            this.ChkLstBxPrograms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkLstBxPrograms.FormattingEnabled = true;
            this.ChkLstBxPrograms.HorizontalScrollbar = true;
            this.ChkLstBxPrograms.Location = new System.Drawing.Point(331, 419);
            this.ChkLstBxPrograms.Margin = new System.Windows.Forms.Padding(4);
            this.ChkLstBxPrograms.Name = "ChkLstBxPrograms";
            this.ChkLstBxPrograms.Size = new System.Drawing.Size(290, 191);
            this.ChkLstBxPrograms.TabIndex = 11;
            // 
            // ChkDebug
            // 
            this.ChkDebug.AutoSize = true;
            this.ChkDebug.Location = new System.Drawing.Point(8, 7);
            this.ChkDebug.Margin = new System.Windows.Forms.Padding(4);
            this.ChkDebug.Name = "ChkDebug";
            this.ChkDebug.Size = new System.Drawing.Size(147, 21);
            this.ChkDebug.TabIndex = 10;
            this.ChkDebug.Text = "Enable Debugging";
            this.ChkDebug.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.InitialDirectory = "C:\\Users";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 675);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Trump\'s Cyber Security Firewall TM";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.TabPgConfig.ResumeLayout(false);
            this.TabPgConfig.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TabPage TabPgConfig;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label LblProgramList;
        private System.Windows.Forms.CheckedListBox ChkLstBxPrograms;
        private System.Windows.Forms.CheckBox ChkDebug;
        private System.Windows.Forms.CheckBox ChkRDP;
        private System.Windows.Forms.TextBox TxtBoxBrowseFile;
        private System.Windows.Forms.Button BtnBrowseFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox TxtBoxFileInfo;
        private System.Windows.Forms.Label LblBrowseFile;
    }
}

