using Microsoft.Win32;
using WUApiLib;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;

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
            if (!ChkDebug.Checked && debug) return;
            TxtBoxInfo.AppendText(Environment.NewLine + message);
        }

        /// <summary>
        /// Runs the specified command in command prompt, waiting until the process finishes or times out.
        /// </summary>
        /// <param name="command">The command to pass into command prompt.</param>
        /// <param name="millisWait">Milliseconds until the process times out.</param>
        private bool CMD(string command, int millisWait)
        {
            Process cmdTask = new Process();
            cmdTask.StartInfo.FileName = "CMD.exe";
            cmdTask.StartInfo.Arguments = "/C " + command;
            cmdTask.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmdTask.Start();
            return cmdTask.WaitForExit(millisWait);
        }

        /// <summary>
        /// Runs the specified command in command prompt.
        /// </summary>
        /// <param name="command">The command to pass into command prompt.</param>
        /// <param name="waitForExit">Whether the thread will lock until process completes.</param>
        private void CMD(string command, bool waitForExit)
        {
            Process cmdTask = new Process();
            cmdTask.StartInfo.FileName = "CMD.exe";
            cmdTask.StartInfo.Arguments = "/C " + command;
            cmdTask.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmdTask.Start();
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
        /// Provides a <see cref="Microsoft.Win32.RegistryKey"/> object for the specified local machine registry directory.
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

        private void EnableAuditing()
        {
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

            using (var key = AccessRegistryKey(@"SECURITY\Policy\PolAdtEv", true))
            {
                if (key != null)
                {
                    byte[] keyValue = (byte[])key.GetValue(null);
                    EditByteArray(ref keyValue, 12, endindex, 0x03); //Enable all auditing
                    key.SetValue(null, keyValue);
                    key.Close();

                    Log("All auditing successfully enabled!", false);
                }
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

            String[] userNames = null;
            using (var key = AccessRegistryKey(@"SECURITY\SAM\Domains\Account\Users\Names", false))
            {
                if (key != null)
                {
                    Log("Reading list of users...", true);
                    userNames = key.GetSubKeyNames();
                }
            }

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
                        Log($"Deleting \"{user}\"...", false);
                        CMD($"net user \"{user}\" /delete", false);
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

        static bool malwareBytesDownloaded = false;
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
                        while (client.IsBusy) { Task.Delay(500); }
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
                    Log("Failed to install MalwareBytes.", false);
                }
            }
        }

        private void UpdateFirefox()
        {
            if (File.Exists(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe"))
            {
                Log("Ensuring Firefox is up to date...", false);
                Process.Start(@"C:\Program Files (x86)\Mozilla Firefox\updater.exe");
            }
            else
            {
                Log("Downloading Firefox...", false);
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile("https://download.mozilla.org/?product=firefox-stub&os=win&lang=en-US", @"C:\Windows\Temp\firefox_setup.exe");
                        while (client.IsBusy) { Task.Delay(500); }
                    }

                    Log("Installing Firefox...", false);
                    CMD("C:\\Windows\\Temp\\firefox_setup.exe -ms", true);

                    Log("Successfully installed Firefox!", false);
                    File.Delete(@"C:\Windows\Temp\firefox_setup.exe");
                }
                catch (Exception ex)
                {
                    Log(ex.Message, true);
                    Log("Failed to install Firefox.", false);
                }
            }
        }

        private UpdateSession updateSession;
        private ISearchResult searchResult;
        private void UpdateWindows()
        {
            Log("Checking for Windows Updates...", false);
            updateSession = new UpdateSession();
            searchResult = updateSession.CreateUpdateSearcher().Search("IsInstalled=0 AND BrowseOnly=0 AND IsHidden=0");

            if (searchResult.Updates.Count < 1)
            {
                Log("No Windows Updates to download.", false);
            }
            else
            {
                Log("Downloading Windows Updates...", false);
                UpdateDownloader downloader = updateSession.CreateUpdateDownloader();
                downloader.Updates = searchResult.Updates;
                downloader.Download();

                Log("Installing Windows Updates...", false);
                UpdateCollection updatesToInstall = new UpdateCollection();
                foreach (IUpdate update in searchResult.Updates)
                {
                    if (update.IsDownloaded)
                    {
                        updatesToInstall.Add(update);
                    }
                }

                //install downloaded updates
                IUpdateInstaller installer = updateSession.CreateUpdateInstaller();
                installer.Updates = updatesToInstall;
                IInstallationResult installationRes = installer.Install();

                Log("Windows Update Complete!", false);
            }
        }

        private void ConfigurePolicy()
        {
            Log("Configuring password policy...", true);
            CMD("wmic UserAccount set PasswordExpires=True", false);

            CMD("net accounts /minpwlen:10", false);
            CMD("net accounts /maxpwage:30", false);
            CMD("net accounts /minpwage:10", false);
            CMD("net accounts /uniquepw:8", false);

            using (var key = AccessRegistryKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                true))
            {
                if (key != null)
                    key.SetValue("EnableLUA", 1);
            }
        }

        private void listOfTasks(object sender, DoWorkEventArgs e)
        {
            EnableAuditing();
            ConfigureUsers(TxtBoxPass.Text);
            ConfigurePolicy();
            InstallMalawareBytes();
            UpdateFirefox();
            //UpdateWindows();
        }

        private async void BtnSecure_Click(object sender, EventArgs e)
        {
            BtnSecure.Enabled = false;

            var tasks = new BackgroundWorker();
            tasks.DoWork += new DoWorkEventHandler(listOfTasks);
            tasks.RunWorkerAsync();
            while (tasks.IsBusy) await Task.Delay(500);

            Log("The wall has been built!", false);
            Log("----------------------------------------------------", false);
            BtnSecure.Enabled = true;
        }

        //TODO:
        /*
         * 'Build the Wall!' should be able to configure windows security settings 
         * (UAC, Firewall (Account Policies), etc.)
         * Add easy way to turn on and off all FTP / 
         * Remote conection services / windows feature)
         *  Update firefox and Windows (DEFER FEATURE UPDATES)
         * Tool to get info about file (Owner, MD5, Group, EVERYTHING)
         * Automatically give every user a password
         * Remove (AKA rename prohibited files (.mp3, any personal files from users lol)
         */
    }
}
