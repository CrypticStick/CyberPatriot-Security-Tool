using Microsoft.Win32;
using System;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Security.Principal;

namespace Trump_s_Cyber_Security_Firewall_TM
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            if (WindowsIdentity.GetCurrent().IsSystem)
            {
                foreach (Process Proc in Process.GetProcesses())
                    if (Proc.ProcessName.Equals("PSEXESVC"))
                        Proc.Kill();

                BtnAuditing.Enabled = true;
                BtnSecure.Enabled = true;
                BtnUsers.Enabled = true;

            }
            else if (new WindowsPrincipal(WindowsIdentity.GetCurrent())
              .IsInRole(WindowsBuiltInRole.Administrator))
            {
                BtnSecure.Enabled = true;
                BtnGodMode.Enabled = true;
            }

            TxtBoxInfo.AppendText(Environment.UserName);
        }

        /// <summary>
        /// Logs the specified string to TxtBoxInfo on a new line.
        /// </summary>
        private void Log(string message)
        {
            TxtBoxInfo.AppendText(Environment.NewLine + message);

        }

        /// <summary>
        /// Converts an array of bytes into a hexideciamal string.
        /// </summary>
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }

        /// <summary>
        /// Edits a byte array at a specified index.
        /// <para>Requires an unsigned 8-bit number, but sets the following byte to zero.</para>
        /// <para>This method is designed specifically for editing the Windows registry for auditing.</para>
        /// </summary>
        /// <param name="array">A reference to the byte array being edited.</param>
        /// <param name="index">The byte being edited.</param>
        /// <param name="value">The new value for the specified byte.</param>
        public static void EditByteArray(ref byte[] array, int index, byte value)
        {
            array[index] = value;
            array[++index] = 0x00;
        }

        /// <summary>
        /// Edits a byte array at a specified index.
        /// <para>Requires an unsigned 8-bit number, but sets the following byte to zero.</para>
        /// <para>This method is designed specifically for editing the Windows registry for auditing.</para>
        /// </summary>
        /// <param name="array">A reference to the byte array being edited.</param>
        /// <param name="start">The index of the first byte to be edited.</param>
        /// <param name="end">The index of the final byte to be edited.</param>
        /// <param name="value">The new value for the specified bytes.</param>
        public static void EditByteArray(ref byte[] array, int start, int end, byte value)
        {
            int index = start;
            while (index <= end) {
                array[index++] = value;
                array[index++] = 0x00;
            }
        }

        /// <summary>
        /// Provides a <see cref="Microsoft.Win32.RegistryKey"/> object for the specified registry directory.
        /// </summary>
        /// <param name="key">The registry key being accessed.</param>
        /// <param name="writable">Whether the key can be written to.</param>
        /// <returns>Returns a <see cref="Microsoft.Win32.RegistryKey"/> object, or null if an error occurs.</returns>
        private RegistryKey AccessRegistryKey(string key, bool writable)
        {
            RegistryKey regKey = Registry.LocalMachine;
            RegistryKey OGKey = regKey;
            try
            {
                regKey = Registry.LocalMachine.OpenSubKey(key, writable);
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

        /// <summary>
        /// Removes the specified user from the system.
        /// </summary>
        /// <param name="username">The user to be deleted.</param>
        /// <param name="retainFiles">Whether the user's files should be saved.</param>
        private void RemoveUser(string username, bool retainFiles)
        {
            Log($"Trying to delete {username}...");

            string theoreticalDirectory = $@"C:\Users\{username}";

            RegistryKey userIDs = AccessRegistryKey(
            @"SECURITY\SAM\Domains\Account\Users",
            true
            );

            if (userIDs != null)
            {
                if (!userIDs.OpenSubKey("Names").GetSubKeyNames().Contains(username))
                {
                    Log("User does not exist. Continuing...");
                    return;
                }

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
                    bool found = false;
                    foreach (string chaoticString in profiles)
                    {
                        subKey = key.OpenSubKey(chaoticString);
                        currentPath = subKey.GetValue("ProfileImagePath").ToString();
                        if (subKey.GetValue("ProfileImagePath").ToString()
                            .Equals(theoreticalDirectory))
                        {
                            found = true;
                            Log("Found user! Attempting to remove user files...");
                            try
                            {
                                key.DeleteSubKey(chaoticString);
                                Log($"Removed {username}'s profile.");
                                if (retainFiles)
                                {
                                    System.IO.Directory.Move(theoreticalDirectory, theoreticalDirectory + " (Deleted)");
                                    Log($"{username}'s file directory successfully renamed.");
                                }
                                else
                                {
                                    System.IO.Directory.Delete(theoreticalDirectory);
                                    Log($"{username}'s files successfully deleted.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Log("Warning: Could not remove user files!!");
                            }
                        }
                    }
                    if (!found) Log("Didn't find user in ProfileList. Continuing...");
                    key.Close();
                }
                else
                {
                    Log("ERROR: ProfileList key could not be opened. Do you have permission?");
                    key.Close();
                    return;
                }

                Log($"Attempting to export profile info of {username}...");
                String exportKeyCmd = $"/C regedit /E \"C:\\Windows\\Temp\\{username}_type.reg\" \"HKEY_LOCAL_MACHINE\\SECURITY\\SAM\\Domains\\Account\\Users\\Names\\{username}\"";

                Process exportUser = Process.Start("CMD.exe", exportKeyCmd);
                if (!exportUser.WaitForExit(5000))
                {
                    Log("User export timed out. Reading will likely fail.");
                }
                string regContents;
                try
                {
                    regContents = System.IO.File.ReadAllText($@"C:\Windows\Temp\{username}_type.reg");
                    System.IO.File.Delete($@"C:\Windows\Temp\{username}_type.reg");
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    Log("ERROR: Cannot delete user - failed to read user info.");
                    return;
                }
                int startIndex = regContents.IndexOf('(') + 1;
                int substringLength = regContents.IndexOf(')') - startIndex;
                string regType = regContents.Substring(startIndex, substringLength);

                Log("Successfully read user info! Attempting to fully wipe user...");

                foreach (string id in userIDs.GetSubKeyNames())
                {
                    if (id.Contains(regType.ToUpper()))
                    {
                        try
                        {
                            userIDs.DeleteSubKey(id);
                            userIDs.OpenSubKey("Names", true).DeleteSubKey(username);
                            Log($"User {username} has been fully removed!");
                            userIDs.Close();
                            return;
                        } catch (Exception ex)
                        {
                            Log(ex.Message);
                        }
                    }
                }
                Log($"User {username} not found.");
            }
            else
            {
                Log("Failed to access users.");
            }
            Log($"ERROR: {username} failed to delete.");
            userIDs.Close();
        }

        private void BtnAuditing_Click(object sender, EventArgs e)
        {
            RegistryKey auditKey = AccessRegistryKey(@"SECURITY\Policy\PolAdtEv", true);

            int endindex = 12;

            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;

            if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 6:
                        if (vs.Minor == 0)
                        {
                            endindex = 114; //Vista
                            Log("WinVista detected.");
                        }
                        else if (vs.Minor == 1)
                        {
                            endindex = 116; //7
                            Log("Win7 detected.");
                        }
                        else if (vs.Minor == 2)
                        {
                            endindex = 122; //8
                            Log("Win8 detected.");
                        }
                        else
                        {
                            endindex = 122;   //8.1
                            Log("Win8.1 detected.");
                        }
                        break;
                    case 10:
                        endindex = 126;    //10
                        Log("Win10 detected.");
                        break;
                    default:
                        Log("This version of Windows is not supported!");
                        return;
                }
            }

            if (auditKey != null)
            {
                byte[] keyValue = (byte[])auditKey.GetValue(null);
                EditByteArray(ref keyValue, 12, endindex, 0x03); //Enable all auditing
                auditKey.SetValue(null, keyValue);
                auditKey.Close();

                Log("All auditing successfully enabled!");
            }

        }

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            ArrayList userList;
            ArrayList admins;
            try
            {
                userList = new ArrayList();
                userList.AddRange(TxtBoxUsers.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
                if (userList.Count < 2 || !userList.Contains("Authorized Administrators:") || !userList.Contains("Authorized Users:"))
                {
                    Log("Please paste \"Authorized Users\" and \"Authorized Administrators\" text in textbox.");
                    return;
                }

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
                userList[0] = userList[0].ToString().
                    Substring(0, userList[0].ToString().LastIndexOf(' '));

                admins = new ArrayList(userList);

                    admins.RemoveRange(
                        admins.IndexOf("Authorized Users:"),
                        admins.Count - admins.IndexOf("Authorized Users:")
                        );
            }
            catch (Exception ex)
            {
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
                    {
                        Log($"Ensuring {user} is disabled...");
                        userAdminCmd = $"/C net user \"{user}\" /active:no";
                    }
                    else
                    {

                        if (!userList.Contains(user))
                        {
                            RemoveUser(user, true);
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
                    }

                    Process.Start("CMD.exe", userAdminCmd);
                }

            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnSecure_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe")) {
                Log("MalawareBytes already installed.");
            } else {
                Log("Installing MalawareBytes...");
                try
                {
                    File.WriteAllBytes("mb3_setup.exe",
                        Properties.Resources.mb3_setup_consumer_3_6_1_2711_1_0_463_1_0_7197
                        );

                    Process.Start("CMD.exe",
                        "/C mb3_setup.exe /VERYSILENT /SUPPRESSMSGBOXES /NOCANCEL /NORESTART /SP- /LOG= %TEMP%\\mb3_install.log"
                        ).WaitForExit();

                    File.Delete("mb3_setup.exe");

                } catch (Exception ex)
                {
                    Log(ex.Message);
                }
                Log("Done installing MalawareBytes!");
            }
        }

        private void BtnGodMode_Click(object sender, EventArgs e)
        {
            Log("Restarting as System user...");
            try
            {
                File.WriteAllBytes("PsExec.exe",
                    Properties.Resources.PsExec
                    );

                Process.Start("PsExec.exe",
                     $"-h -s -i \"{Process.GetCurrentProcess().MainModule.FileName}\""
                     );

                File.Delete("PsExec.exe");
                Close();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Log("Operation falied!");
            }
        }

        //TODO: Add new button - Build the Wall! (Sets up security)
        /*
         * 'Build the Wall!' should be able to configure windows security settings 
         * (UAC, Firewall, Password Requirements, etc.)
         * and install antimalaware (Malawarebytes) automatically.
         */
    }
}
