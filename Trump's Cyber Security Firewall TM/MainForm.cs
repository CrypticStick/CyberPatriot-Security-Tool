using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
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

        private void ElevateUser()
        {

            Log("Restarting as System user...", false);
            try
            {
                File.WriteAllBytes("C:\\Windows\\Temp\\PsExec64.exe",
                    Properties.Resources.PsExec64
                    );

                Process.Start("C:\\Windows\\Temp\\PsExec64.exe",
                     $"-h -s -i \"{Process.GetCurrentProcess().MainModule.FileName}\""
                     );

                Close();
                Environment.Exit(5);
            }
            catch (Exception ex)
            {
                Log(ex.Message, true);
                Log("Operation falied!", false);
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (WindowsIdentity.GetCurrent().IsSystem)
            {
                foreach (Process Proc in Process.GetProcesses())
                    try
                    {
                        if (Proc.MainModule.FileName.Contains("PsExec64.exe"))
                            Proc.Kill();
                    }
                    catch (Exception ex) { }

                BtnSecure.Enabled = true;

            }
            else if (new WindowsPrincipal(WindowsIdentity.GetCurrent())
              .IsInRole(WindowsBuiltInRole.Administrator))
            {
                ElevateUser();
            }
        }

        /// <summary>
        /// Logs the specified string to TxtBoxInfo on a new line.
        /// </summary>
        private void Log(string message, bool debug)
        {
            if (ChkDebug.Checked && debug) return;
            TxtBoxInfo.AppendText(Environment.NewLine + message);
        }

        /// <summary>
        /// Runs the specified command in command prompt, waiting until the process finishes or times out.
        /// </summary>
        /// <param name="command">The command to pass into command prompt.</param>
        /// <param name="millisWait">Milliseconds until the process times out.</param>
        private bool CMD(string command, int millisWait)
        {
            Process cmdTask = Process.Start("CMD.exe", "/C " + command);
            return cmdTask.WaitForExit(millisWait);
        }

        /// <summary>
        /// Runs the specified command in command prompt.
        /// </summary>
        /// <param name="command">The command to pass into command prompt.</param>
        /// <param name="waitForExit">Whether the thread will lock until process completes.</param>
        private void CMD(string command, bool waitForExit)
        {
            Process cmdTask = Process.Start("CMD.exe", "/C " + command);
            if (waitForExit) cmdTask.WaitForExit();
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
            while (index <= end)
            {
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
                Log("You do not have sufficient privilages to access this key!", true);
            }

            if (regKey != OGKey)
            {
                //Log("RegistryKey successfully created!");
                return regKey;
            }
            else
            {
                Log("RegistryKey creation failed.", true);
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
            Log($"Trying to delete {username}...", true);

            string theoreticalDirectory = $@"C:\Users\{username}";

            RegistryKey userIDs = AccessRegistryKey(
            @"SECURITY\SAM\Domains\Account\Users",
            true
            );

            if (userIDs != null)
            {
                if (!userIDs.OpenSubKey("Names").GetSubKeyNames().Contains(username))
                {
                    Log("User does not exist. Continuing...", true);
                    return;
                }

                RegistryKey key = AccessRegistryKey(
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList",
                true
                );

                RegistryKey subKey;
                if (key != null)
                {
                    Log("Successfully accessed ProfileList.", true);
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
                            Log("Found user! Attempting to remove user files...", true);
                            try
                            {
                                key.DeleteSubKey(chaoticString);
                                Log($"Removed {username}'s desktop.", false);
                                if (retainFiles)
                                {
                                    System.IO.Directory.Move(theoreticalDirectory, theoreticalDirectory + " (Deleted)");
                                    Log($"{username}'s file directory successfully renamed.", false);
                                }
                                else
                                {
                                    System.IO.Directory.Delete(theoreticalDirectory);
                                    Log($"{username}'s files successfully deleted.", false);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log($"Warning: Could not remove {username}'s files!!", false);
                            }
                        }
                    }
                    if (!found) Log("Didn't find user in ProfileList. Continuing...", true);
                    key.Close();
                }
                else
                {
                    Log("ERROR: ProfileList key could not be opened. Do you have permission?", true);
                    key.Close();
                    return;
                }

                Log($"Attempting to export profile info of {username}...", true);
                String exportKeyCmd = $"regedit /E \"C:\\Windows\\Temp\\{username}_type.reg\" \"HKEY_LOCAL_MACHINE\\SECURITY\\SAM\\Domains\\Account\\Users\\Names\\{username}\"";

                if (!CMD(exportKeyCmd, 5000))
                {
                    Log("User export timed out. Reading will likely fail.", true);
                }
                string regContents;
                try
                {
                    regContents = System.IO.File.ReadAllText($@"C:\Windows\Temp\{username}_type.reg");
                    System.IO.File.Delete($@"C:\Windows\Temp\{username}_type.reg");
                }
                catch (Exception ex)
                {
                    Log(ex.Message, true);
                    Log("ERROR: Cannot delete user - failed to read user info.", false);
                    return;
                }
                int startIndex = regContents.IndexOf('(') + 1;
                int substringLength = regContents.IndexOf(')') - startIndex;
                string regType = regContents.Substring(startIndex, substringLength);

                Log("Successfully read user info! Attempting to fully wipe user...", true);

                foreach (string id in userIDs.GetSubKeyNames())
                {
                    if (id.Contains(regType.ToUpper()))
                    {
                        try
                        {
                            userIDs.DeleteSubKey(id);
                            userIDs.OpenSubKey("Names", true).DeleteSubKey(username);
                            Log($"User {username} has been fully removed!", false);
                            userIDs.Close();
                            return;
                        }
                        catch (Exception ex)
                        {
                            Log(ex.Message, true);
                        }
                    }
                }
                Log($"User {username} not found.", true);
            }
            else
            {
                Log("Failed to access users.", true);
            }
            Log($"ERROR: {username} failed to delete.", false);
            userIDs.Close();
        }

        private void EnableAuditing()
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
                            Log("WinVista detected.", false);
                        }
                        else if (vs.Minor == 1)
                        {
                            endindex = 116; //7
                            Log("Win7 detected.", false);
                        }
                        else if (vs.Minor == 2)
                        {
                            endindex = 122; //8
                            Log("Win8 detected.", false);
                        }
                        else
                        {
                            endindex = 122;   //8.1
                            Log("Win8.1 detected.", false);
                        }
                        break;
                    case 10:
                        endindex = 126;    //10
                        Log("Win10 detected.", false);
                        break;
                    default:
                        Log("This version of Windows is not supported!", false);
                        return;
                }
            }

            if (auditKey != null)
            {
                byte[] keyValue = (byte[])auditKey.GetValue(null);
                EditByteArray(ref keyValue, 12, endindex, 0x03); //Enable all auditing
                auditKey.SetValue(null, keyValue);
                auditKey.Close();

                Log("All auditing successfully enabled!", false);
            }

        }

        private void ConfigureUsers(string password)
        {
            if (TxtBoxPass.Text.Length < 8)
            {
                Log("Please provide a password before configuring users.", false);
                return;
            }

            ArrayList userList;
            ArrayList admins;
            try
            {
                userList = new ArrayList();
                userList.AddRange(TxtBoxUsers.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
                if (userList.Count < 2 || !userList.Contains("Authorized Administrators:") || !userList.Contains("Authorized Users:"))
                {
                    Log("Please paste \"Authorized Users\" and \"Authorized Administrators\" text in textbox.", false);
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
                Log("This list of names doesn't work.", false);
                return;
            }
            RegistryKey usersKey = AccessRegistryKey(@"SECURITY\SAM\Domains\Account\Users\Names", false);

            if (usersKey != null)
            {
                Log("Reading list of users...", true);
                String[] userNames = usersKey.GetSubKeyNames();

                usersKey.Close();

                String userAdminCmd;

                foreach (String user in userNames)
                {
                    if (user.Equals("Administrator") || user.Equals("Guest"))
                    {
                        Log($"Ensuring {user} is disabled...", false);
                        userAdminCmd = $"net user \"{user}\" /active:no";
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
                            Log($"Setting {user} as admin...", false);
                            userAdminCmd = $"net localgroup \"Administrators\" \"{user}\" /ADD";
                        }
                        else
                        {
                            Log($"Setting {user} as normal user...", false);
                            userAdminCmd = $"net localgroup \"Administrators\" \"{user}\" /DELETE";
                        }
                    }
                    CMD(userAdminCmd, false);

                    if (!admins[0].Equals(user))
                        CMD($"net user \"{user}\" \"{password}\"", false);
                }

            }
        }

        private void InstallMalawareBytes()
        {
            if (File.Exists(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe"))
            {
                Log("MalawareBytes already installed.", false);
            }
            else
            {
                Log("Downloading MalwareBytes...", false);
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile("https://downloads.malwarebytes.com/file/mb3/", "C:\\Windows\\Temp\\mb3_setup.exe");
                        while (client.IsBusy) { Thread.Sleep(500); }
                    }

                    Log("Installing MalwareBytes...", false);
                    CMD("C:\\Windows\\Temp\\mb3_setup.exe /VERYSILENT /SUPPRESSMSGBOXES /NOCANCEL /NORESTART /SP- /LOG= %TEMP%\\mb3_install.log"
                        , true);

                    Log("Successfully installed MalwareBytes!", false);
                    File.Delete("C:\\Windows\\Temp\\mb3_setup.exe");
                }
                catch (Exception ex)
                {
                    Log(ex.Message, true);
                    Log("Failed to install MalwareBytes", false);
                }
            }
        }

        private void ConfigurePolicy()
        {
            Log("Configuring password policy...", true);
            CMD("net accounts /minpwlen:8", false);
            CMD("net accounts /maxpwage:30", false);
            CMD("net accounts /minpwage:10", false);
            CMD("net accounts /uniquepw:8", false);
        }

        private void BtnSecure_Click(object sender, EventArgs e)
        {
            EnableAuditing();
            ConfigureUsers(TxtBoxPass.Text);
            ConfigurePolicy();
            InstallMalawareBytes();
            Log("The wall has been built!", false);
            Log("----------------------------------------------------", false);
        }

        //TODO:
        /*
         * 'Build the Wall!' should be able to configure windows security settings 
         * (UAC, Firewall, Password Requirements (ex. at least 10 characters long, password history, age policy) 
         * (Account Policies), etc.)
         * Add easy way to turn on and off all FTP / 
         * Remote conection services / windows feature)
         *  Update firefox and Windows (DEFER FEATURE UPDATES)
         * Tool to get info about file (Owner, MD5, Group, EVERYTHING)
         * Automatically give every user a password
         * Remove (AKA rename prohibited files (.mp3, any personal files from users lol)
         */
    }
}
