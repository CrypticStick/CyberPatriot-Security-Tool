using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }

        private void BtnBasicSecurity_Click(object sender, EventArgs e)
        {
            /*
            RegistryKey key = Registry.LocalMachine.OpenSubKey("HKEY_LOCAL_MACHINE\\SECURITY\\Policy\\PolAdtEv", true);
            if(key != null)
            {

                key.SetValue("item", "value");
                key.Close();
            }
            */
        }
    }
}
