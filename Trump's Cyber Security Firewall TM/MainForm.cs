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

        private void Log(string message)
        {
            TxtBoxInfo.AppendText(Environment.NewLine + message);

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

        private RegistryKey AccessRegistryKey(string key, bool editable)
        {
            RegistryKey regKey = Registry.LocalMachine;
            RegistryKey OGKey = regKey;
            try
            {
                regKey = Registry.LocalMachine.OpenSubKey(key, editable);
            }
            catch (SecurityException ex)
            {
                Log("You do not have sufficient privilages to access this key!");
            }

            if (regKey != OGKey)
            {
                //Log("RegistryKey successfully created!");
                return regKey;
            }
            else
            {
                Log("RegistryKey creation failed.");
                regKey.Close();
                return null;
            }
        }

        private void BtnAuditing_Click(object sender, EventArgs e)
        {
            RegistryKey auditKey = AccessRegistryKey(@"SECURITY\Policy\PolAdtEv", true);

            if (auditKey != null)
            {
                byte[] keyValue = (byte[])auditKey.GetValue(null);

                Log("Old: " + ByteArrayToString(keyValue)
                );

                EditByteArray(ref keyValue, 12, 114, 0x03); //Enable all auditing on Windows 7

                Log(ByteArrayToString(keyValue));

                auditKey.SetValue(null, keyValue);

                auditKey.Close();
            }
        }

        private void RemoveUser(string username)
        {
            Log($"Trying to delete {username}...");

            string theoreticalDirectory = $@"C:\Users\{username}";

            RegistryKey key = AccessRegistryKey(
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList",
                true
                );

            RegistryKey subKey;
            if (key != null)
            {
                Log("Successfully accessed ProfileList.");
                String[] profiles = key.GetSubKeyNames();
                String currentPath;
                foreach (string chaoticString in profiles)
                {
                    subKey = key.OpenSubKey(chaoticString);
                    currentPath = subKey.GetValue("ProfileImagePath").ToString();
                    Log($"Found '{currentPath}'");
                    if (subKey.GetValue("ProfileImagePath").ToString()
                        .Equals(theoreticalDirectory))
                    {
                        Log("Found user! Deleting...");
                        try
                        {
                            key.DeleteSubKey(chaoticString);
                            System.IO.Directory.Move(theoreticalDirectory, theoreticalDirectory + " (Deleted)");
                        }
                        catch (Exception ex)
                        {
                            Log("Account deletion failed!");
                            return;
                        }
                        key.Close();
                        return;
                    }
                }

                RegistryKey usersKey = AccessRegistryKey(@"SECURITY\SAM\Domains\Account\Users\Names", true);

                if (usersKey != null)
                {
                    RegistryKey toBeDeletKey = 
                        usersKey.OpenSubKey(username,true);

                    if (toBeDeletKey != null)
                    {
                        //.GetType().Name;
                        //Log($"{toBeDeletValue}");
                        //byte[] maybe = BitConverter.GetBytes(
                        //    toBeDeletKey.GetValueKind(null));
                        //Log(ByteArrayToString(maybe));
                    }
                }

                key.Close();
            }
            Log("Profile for specified user not found.");
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

            userList.RemoveAt(0);
            userList[0] = userList[0].ToString().Split(' ')[0];

            ArrayList admins = new ArrayList(userList);
            try
            { 
                admins.RemoveRange(
                    admins.IndexOf("Authorized Users:"), 
                    admins.Count - admins.IndexOf("Authorized Users:")
                    );
            } catch (Exception ex) {             
                Log("This list of names doesn't work.");
                return;
            }

            RegistryKey usersKey = AccessRegistryKey(@"SECURITY\SAM\Domains\Account\Users\Names", false);

            if (usersKey != null)
            {
                Log("Reading list of users...");
                String[] userNames = usersKey.GetSubKeyNames();

                usersKey.Close();

                String userAdminCmd;

                foreach (String user in userNames)
                {
                    if (user.Equals("Administrator") || user.Equals("Guest"))
                        continue;

                    if (!userList.Contains(user))
                    {
                        RemoveUser(user);
                        continue;
                    }

                    if (admins.Contains(user))
                    {
                        Log($"Setting {user} as admin...");
                        userAdminCmd = $"/C net localgroup \"Administrators\" \"{user}\" /ADD";
                    }
                    else
                    {
                        Log($"Setting {user} as normal user...");
                        userAdminCmd = $"/C net localgroup \"Administrators\" \"{user}\" /DELETE";
                    }
                    System.Diagnostics.Process.Start("CMD.exe", userAdminCmd);
                }

            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

    }
}
