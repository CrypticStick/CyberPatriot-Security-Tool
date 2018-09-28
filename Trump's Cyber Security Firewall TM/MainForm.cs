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
using System.Collections;

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
                array[index++] = value;
                array[index++] = 0x00;
            }
        }

        private void BtnAuditing_Click(object sender, EventArgs e)
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

                EditByteArray(ref keyValue, 12, 114, 0x03); //Enable all auditing on Windows 7

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

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            ArrayList userList = new ArrayList();
            userList.AddRange(TxtBoxUsers.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

            for (int i = 0; i < userList.Count; i++) //removes everything except users
            {
                string line = userList[i].ToString().Trim();

                if (line.Contains("password:") || line.Equals(""))
                {
                    userList.RemoveAt(i);
                    continue;
                }

                userList[i] = line;
            }

            ArrayList admins = new ArrayList(userList);
            try
            {
                admins.RemoveRange(0, 2);
                admins.RemoveRange(
                    admins.IndexOf("Authorized Users:"), 
                    admins.Count - admins.IndexOf("Authorized Users:")
                    );
            } catch (Exception ex) {             
                TxtBoxInfo.AppendText(Environment.NewLine + "This list of names doesn't work.");
                return;
            }

            RegistryKey key = Registry.LocalMachine;
            RegistryKey OGKey = key;
            try
            {
                key = Registry.LocalMachine.OpenSubKey(
                    @"SECURITY\SAM\Domains\Account\Users\Names",
                    false
                    );
            }
            catch (SecurityException ex)
            {
                TxtBoxInfo.AppendText("You do not have sufficient privilages to access this key!");
            }

            if (key != OGKey)
            {
                TxtBoxInfo.AppendText(Environment.NewLine + "RegistryKey successfully created!");
                String[] userNames = key.GetSubKeyNames();
                key.Close();

                String userAdminCmd;

                foreach (String user in userNames)
                {
                    if (admins.Contains(user))
                        userAdminCmd = String.Format("/C net localgroup \"Administrators\" \"%s\" /ADD", user);
                    else
                        userAdminCmd = String.Format("/C net localgroup \"Administrators\" \"%s\" /DELETE", user);

                    System.Diagnostics.Process.Start("CMD.exe", userAdminCmd);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

    }
}
