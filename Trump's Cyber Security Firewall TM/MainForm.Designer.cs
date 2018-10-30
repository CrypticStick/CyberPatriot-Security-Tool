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
            this.ChkDebug = new System.Windows.Forms.CheckBox();
            this.TxtBoxPass = new System.Windows.Forms.TextBox();
            this.LblNewPass = new System.Windows.Forms.Label();
            this.LblAuthUser = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // TxtBoxInfo
            // 
            this.TxtBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBoxInfo.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtBoxInfo.Location = new System.Drawing.Point(13, 455);
            this.TxtBoxInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtBoxInfo.Multiline = true;
            this.TxtBoxInfo.Name = "TxtBoxInfo";
            this.TxtBoxInfo.ReadOnly = true;
            this.TxtBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxInfo.Size = new System.Drawing.Size(904, 132);
            this.TxtBoxInfo.TabIndex = 1;
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LblInfo.AutoSize = true;
            this.LblInfo.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblInfo.Location = new System.Drawing.Point(8, 425);
            this.LblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(58, 26);
            this.LblInfo.TabIndex = 2;
            this.LblInfo.Text = "Info:";
            // 
            // TxtBoxUsers
            // 
            this.TxtBoxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBoxUsers.Location = new System.Drawing.Point(648, 34);
            this.TxtBoxUsers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtBoxUsers.Multiline = true;
            this.TxtBoxUsers.Name = "TxtBoxUsers";
            this.TxtBoxUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxUsers.Size = new System.Drawing.Size(269, 349);
            this.TxtBoxUsers.TabIndex = 4;
            // 
            // BtnSecure
            // 
            this.BtnSecure.Enabled = false;
            this.BtnSecure.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSecure.ForeColor = System.Drawing.Color.Red;
            this.BtnSecure.Location = new System.Drawing.Point(13, 13);
            this.BtnSecure.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnSecure.Name = "BtnSecure";
            this.BtnSecure.Size = new System.Drawing.Size(409, 370);
            this.BtnSecure.TabIndex = 5;
            this.BtnSecure.Text = "Build the wall!";
            this.Tltp_Btns.SetToolTip(this.BtnSecure, "Configures system for optimal security.\r\nAlso installs anti-malaware and enables " +
        "security features.");
            this.BtnSecure.UseVisualStyleBackColor = false;
            this.BtnSecure.Click += new System.EventHandler(this.BtnSecure_Click);
            // 
            // ChkDebug
            // 
            this.ChkDebug.AutoSize = true;
            this.ChkDebug.Location = new System.Drawing.Point(13, 399);
            this.ChkDebug.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChkDebug.Name = "ChkDebug";
            this.ChkDebug.Size = new System.Drawing.Size(147, 21);
            this.ChkDebug.TabIndex = 6;
            this.ChkDebug.Text = "Enable Debugging";
            this.ChkDebug.UseVisualStyleBackColor = true;
            // 
            // TxtBoxPass
            // 
            this.TxtBoxPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBoxPass.Location = new System.Drawing.Point(648, 400);
            this.TxtBoxPass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtBoxPass.Name = "TxtBoxPass";
            this.TxtBoxPass.Size = new System.Drawing.Size(269, 22);
            this.TxtBoxPass.TabIndex = 7;
            // 
            // LblNewPass
            // 
            this.LblNewPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblNewPass.AutoSize = true;
            this.LblNewPass.Location = new System.Drawing.Point(433, 400);
            this.LblNewPass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblNewPass.Name = "LblNewPass";
            this.LblNewPass.Size = new System.Drawing.Size(207, 17);
            this.LblNewPass.TabIndex = 8;
            this.LblNewPass.Text = "New Password (for other users)";
            // 
            // LblAuthUser
            // 
            this.LblAuthUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblAuthUser.AutoSize = true;
            this.LblAuthUser.Location = new System.Drawing.Point(645, 13);
            this.LblAuthUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblAuthUser.Name = "LblAuthUser";
            this.LblAuthUser.Size = new System.Drawing.Size(242, 17);
            this.LblAuthUser.TabIndex = 9;
            this.LblAuthUser.Text = "Authorized Users (list from README)";
            // 
            // BtnStop
            // 
            this.BtnStop.Enabled = false;
            this.BtnStop.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStop.ForeColor = System.Drawing.Color.Red;
            this.BtnStop.Location = new System.Drawing.Point(430, 13);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(4);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(210, 370);
            this.BtnStop.TabIndex = 10;
            this.BtnStop.Text = "Abandon office";
            this.Tltp_Btns.SetToolTip(this.BtnStop, "Stops building the wall.");
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(65, 425);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(357, 23);
            this.ProgressBar.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 597);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.LblAuthUser);
            this.Controls.Add(this.LblNewPass);
            this.Controls.Add(this.TxtBoxPass);
            this.Controls.Add(this.ChkDebug);
            this.Controls.Add(this.BtnSecure);
            this.Controls.Add(this.TxtBoxUsers);
            this.Controls.Add(this.LblInfo);
            this.Controls.Add(this.TxtBoxInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Trump\'s Cyber Security Firewall TM";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TxtBoxInfo;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.TextBox TxtBoxUsers;
        private System.Windows.Forms.Button BtnSecure;
        private System.Windows.Forms.ToolTip Tltp_Btns;
        private System.Windows.Forms.CheckBox ChkDebug;
        private System.Windows.Forms.TextBox TxtBoxPass;
        private System.Windows.Forms.Label LblNewPass;
        private System.Windows.Forms.Label LblAuthUser;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.ProgressBar ProgressBar;
    }
}

