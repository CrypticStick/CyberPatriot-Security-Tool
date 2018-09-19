using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trump_s_Cyber_Security_Firewall_TM
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            TxtBoxInfo.AppendText(Environment.UserName);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }

        private void BtnBasicSecurity_Click(object sender, EventArgs e)
        {
            RegistryKey key = Registry.LocalMachine;
            RegistryKey OGKey = key;
            try
            {
                key = Registry.LocalMachine.OpenSubKey(
                    @"SECURITY\Policy\PolAdtEv"
                    );
            } catch (SecurityException ex)
            {
                TxtBoxInfo.AppendText(Environment.NewLine + ex.ToString());
            }

            if(key != OGKey) {
                TxtBoxInfo.AppendText(Environment.NewLine + "RegistryKey successfully created!");
                byte[] value = (byte[])key.GetValue(null);
                TxtBoxInfo.AppendText(Environment.NewLine + ByteArrayToString(value));
                key.Close();
            } else {
                TxtBoxInfo.AppendText(Environment.NewLine + "Failed to read registry.");
            }
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
