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
            this.BtnAuditing = new System.Windows.Forms.Button();
            this.TxtBoxInfo = new System.Windows.Forms.TextBox();
            this.LblInfo = new System.Windows.Forms.Label();
            this.BtnUsers = new System.Windows.Forms.Button();
            this.TxtBoxUsers = new System.Windows.Forms.TextBox();
            this.BtnSecure = new System.Windows.Forms.Button();
            this.Tltp_Btns = new System.Windows.Forms.ToolTip(this.components);
            this.BtnGodMode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnAuditing
            // 
            this.BtnAuditing.Enabled = false;
            this.BtnAuditing.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAuditing.ForeColor = System.Drawing.Color.Red;
            this.BtnAuditing.Location = new System.Drawing.Point(12, 37);
            this.BtnAuditing.Name = "BtnAuditing";
            this.BtnAuditing.Size = new System.Drawing.Size(181, 190);
            this.BtnAuditing.TabIndex = 0;
            this.BtnAuditing.Text = "Retrieve the emails!";
            this.Tltp_Btns.SetToolTip(this.BtnAuditing, "Enables all auditing on the system.");
            this.BtnAuditing.UseVisualStyleBackColor = false;
            this.BtnAuditing.Click += new System.EventHandler(this.BtnAuditing_Click);
            // 
            // TxtBoxInfo
            // 
            this.TxtBoxInfo.Location = new System.Drawing.Point(95, 358);
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
            this.LblInfo.Location = new System.Drawing.Point(91, 335);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(45, 20);
            this.LblInfo.TabIndex = 2;
            this.LblInfo.Text = "Info:";
            // 
            // BtnUsers
            // 
            this.BtnUsers.Enabled = false;
            this.BtnUsers.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUsers.ForeColor = System.Drawing.Color.Red;
            this.BtnUsers.Location = new System.Drawing.Point(400, 37);
            this.BtnUsers.Name = "BtnUsers";
            this.BtnUsers.Size = new System.Drawing.Size(183, 190);
            this.BtnUsers.TabIndex = 3;
            this.BtnUsers.Text = "Deport the aliens!";
            this.Tltp_Btns.SetToolTip(this.BtnUsers, "Automatically configures system users based on text input.");
            this.BtnUsers.UseVisualStyleBackColor = false;
            this.BtnUsers.Click += new System.EventHandler(this.BtnUsers_Click);
            // 
            // TxtBoxUsers
            // 
            this.TxtBoxUsers.Location = new System.Drawing.Point(606, 37);
            this.TxtBoxUsers.Multiline = true;
            this.TxtBoxUsers.Name = "TxtBoxUsers";
            this.TxtBoxUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtBoxUsers.Size = new System.Drawing.Size(156, 190);
            this.TxtBoxUsers.TabIndex = 4;
            // 
            // BtnSecure
            // 
            this.BtnSecure.Enabled = false;
            this.BtnSecure.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSecure.ForeColor = System.Drawing.Color.Red;
            this.BtnSecure.Location = new System.Drawing.Point(206, 37);
            this.BtnSecure.Name = "BtnSecure";
            this.BtnSecure.Size = new System.Drawing.Size(181, 190);
            this.BtnSecure.TabIndex = 5;
            this.BtnSecure.Text = "Build the wall!";
            this.Tltp_Btns.SetToolTip(this.BtnSecure, "Configures system for optimal security.\r\nAlso installs anti-malaware and enables " +
        "security features.");
            this.BtnSecure.UseVisualStyleBackColor = false;
            this.BtnSecure.Click += new System.EventHandler(this.BtnSecure_Click);
            // 
            // BtnGodMode
            // 
            this.BtnGodMode.Enabled = false;
            this.BtnGodMode.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGodMode.ForeColor = System.Drawing.Color.Red;
            this.BtnGodMode.Location = new System.Drawing.Point(26, 275);
            this.BtnGodMode.Name = "BtnGodMode";
            this.BtnGodMode.Size = new System.Drawing.Size(158, 41);
            this.BtnGodMode.TabIndex = 6;
            this.BtnGodMode.Text = "Become God";
            this.Tltp_Btns.SetToolTip(this.BtnGodMode, "Runs the program as System\r\n(enables the rest of the program\'s features)");
            this.BtnGodMode.UseVisualStyleBackColor = false;
            this.BtnGodMode.Click += new System.EventHandler(this.BtnGodMode_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 494);
            this.Controls.Add(this.BtnGodMode);
            this.Controls.Add(this.BtnSecure);
            this.Controls.Add(this.TxtBoxUsers);
            this.Controls.Add(this.BtnUsers);
            this.Controls.Add(this.LblInfo);
            this.Controls.Add(this.TxtBoxInfo);
            this.Controls.Add(this.BtnAuditing);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Trump\'s Cyber Security Firewall TM";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnAuditing;
        private System.Windows.Forms.TextBox TxtBoxInfo;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.Button BtnUsers;
        private System.Windows.Forms.TextBox TxtBoxUsers;
        private System.Windows.Forms.Button BtnSecure;
        private System.Windows.Forms.ToolTip Tltp_Btns;
        private System.Windows.Forms.Button BtnGodMode;
    }
}

