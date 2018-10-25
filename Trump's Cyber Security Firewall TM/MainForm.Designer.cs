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
            this.SuspendLayout();
            // 
            // TxtBoxInfo
            // 
            this.TxtBoxInfo.Location = new System.Drawing.Point(95, 396);
            this.TxtBoxInfo.Multiline = true;
            this.TxtBoxInfo.Name = "TxtBoxInfo";
            this.TxtBoxInfo.ReadOnly = true;
            this.TxtBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxInfo.Size = new System.Drawing.Size(601, 108);
            this.TxtBoxInfo.TabIndex = 1;
            // 
            // LblInfo
            // 
            this.LblInfo.AutoSize = true;
            this.LblInfo.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblInfo.Location = new System.Drawing.Point(91, 373);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(45, 20);
            this.LblInfo.TabIndex = 2;
            this.LblInfo.Text = "Info:";
            // 
            // TxtBoxUsers
            // 
            this.TxtBoxUsers.Location = new System.Drawing.Point(585, 39);
            this.TxtBoxUsers.Multiline = true;
            this.TxtBoxUsers.Name = "TxtBoxUsers";
            this.TxtBoxUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxUsers.Size = new System.Drawing.Size(203, 284);
            this.TxtBoxUsers.TabIndex = 4;
            // 
            // BtnSecure
            // 
            this.BtnSecure.Enabled = false;
            this.BtnSecure.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSecure.ForeColor = System.Drawing.Color.Red;
            this.BtnSecure.Location = new System.Drawing.Point(12, 40);
            this.BtnSecure.Name = "BtnSecure";
            this.BtnSecure.Size = new System.Drawing.Size(567, 283);
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
            this.ChkDebug.Location = new System.Drawing.Point(12, 331);
            this.ChkDebug.Name = "ChkDebug";
            this.ChkDebug.Size = new System.Drawing.Size(114, 17);
            this.ChkDebug.TabIndex = 6;
            this.ChkDebug.Text = "Enable Debugging";
            this.ChkDebug.UseVisualStyleBackColor = true;
            // 
            // TxtBoxPass
            // 
            this.TxtBoxPass.Location = new System.Drawing.Point(585, 329);
            this.TxtBoxPass.Name = "TxtBoxPass";
            this.TxtBoxPass.Size = new System.Drawing.Size(203, 20);
            this.TxtBoxPass.TabIndex = 7;
            // 
            // LblNewPass
            // 
            this.LblNewPass.AutoSize = true;
            this.LblNewPass.Location = new System.Drawing.Point(425, 332);
            this.LblNewPass.Name = "LblNewPass";
            this.LblNewPass.Size = new System.Drawing.Size(154, 13);
            this.LblNewPass.TabIndex = 8;
            this.LblNewPass.Text = "New Password (for other users)";
            // 
            // LblAuthUser
            // 
            this.LblAuthUser.AutoSize = true;
            this.LblAuthUser.Location = new System.Drawing.Point(582, 23);
            this.LblAuthUser.Name = "LblAuthUser";
            this.LblAuthUser.Size = new System.Drawing.Size(180, 13);
            this.LblAuthUser.TabIndex = 9;
            this.LblAuthUser.Text = "Authorized Users (list from README)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 516);
            this.Controls.Add(this.LblAuthUser);
            this.Controls.Add(this.LblNewPass);
            this.Controls.Add(this.TxtBoxPass);
            this.Controls.Add(this.ChkDebug);
            this.Controls.Add(this.BtnSecure);
            this.Controls.Add(this.TxtBoxUsers);
            this.Controls.Add(this.LblInfo);
            this.Controls.Add(this.TxtBoxInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}

