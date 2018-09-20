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

        public static void EditByteArray(ref byte[] array, int index, byte value)
        {
            array[index] = value;
            array[++index] = 0x00;
        }

        public static void EditByteArray(ref byte[] array, int start, int end, byte value)
        {
            int index = start;
            while (index <= end) {
                array[index] = value;
                array[++index] = 0x00;
                ++index;
            }
        }

        private void BtnBasicSecurity_Click(object sender, EventArgs e)
        {
            RegistryKey key = Registry.LocalMachine;
            RegistryKey OGKey = key;
            try
            {
                key = Registry.LocalMachine.OpenSubKey(
                    @"SECURITY\Policy\PolAdtEv", 
                    true
                    );
            }
            catch (SecurityException ex)
            {
                TxtBoxInfo.AppendText("You do not have sufficient privilages to access this key!");
            }

            if (key != OGKey)
            {
                TxtBoxInfo.AppendText(Environment.NewLine + "RegistryKey successfully created!");
                byte[] keyValue = (byte[])key.GetValue(null);

                TxtBoxInfo.AppendText(
                    Environment.NewLine +
                    "Old: " +
                    ByteArrayToString(keyValue)
                );

                EditByteArray(ref keyValue, 64, 68, 0x03);

                TxtBoxInfo.AppendText(
                    Environment.NewLine +
                    "New: " +
                    ByteArrayToString(keyValue)
                );

                key.SetValue(null, keyValue);

                key.Close();
            }
            else
            {
                TxtBoxInfo.AppendText(Environment.NewLine + "Failed to read registry.");
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
