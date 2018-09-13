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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.BtnBasicSecurity = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnBasicSecurity
            // 
            this.BtnBasicSecurity.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBasicSecurity.ForeColor = System.Drawing.Color.Red;
            this.BtnBasicSecurity.Location = new System.Drawing.Point(237, 171);
            this.BtnBasicSecurity.Name = "BtnBasicSecurity";
            this.BtnBasicSecurity.Size = new System.Drawing.Size(341, 88);
            this.BtnBasicSecurity.TabIndex = 0;
            this.BtnBasicSecurity.Text = "Build the Wall!";
            this.BtnBasicSecurity.UseVisualStyleBackColor = true;
            this.BtnBasicSecurity.Click += new System.EventHandler(this.BtnBasicSecurity_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnBasicSecurity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Trump\'s Cyber Security Firewall TM";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnBasicSecurity;
    }
}

