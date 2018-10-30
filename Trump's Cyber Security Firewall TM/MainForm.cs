using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WUApiLib;

namespace Trump_s_Cyber_Security_Firewall_TM
{
    public partial class MainForm : Form
    {
        bool stop = false;

        public MainForm()
        {
            InitializeComponent();
            MinimumSize = new System.Drawing.Size(948, 644);
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
        private int Log(string message, bool debug)
        {
            if (!ChkDebug.Checked && debug) return -1;
            TxtBoxInfo.AppendText(message + Environment.NewLine);

            TxtBoxInfo.Focus();
            TxtBoxInfo.SelectionStart = TxtBoxInfo.TextLength;
            TxtBoxInfo.ScrollToCaret();

            return TxtBoxInfo.Text.Split('\n').Length - 2;
        }

        private void editLog(int line, string message, bool debug)
        {
            if (!ChkDebug.Checked && debug) return;

            string[] lines = TxtBoxInfo.Text.Split('\n');

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == line)
                {
                    builder.Append(message + Environment.NewLine);
                }
                else
                {
                    builder.Append(lines[i]);
                    builder.Append('\n');
                }
            }
            TxtBoxInfo.Text = builder.ToString();

            TxtBoxInfo.Focus();
            TxtBoxInfo.SelectionStart = TxtBoxInfo.TextLength;
            TxtBoxInfo.ScrollToCaret();
        }

        static private string loadingBar(int percentage)
        {
            return $"{percentage}% [" +
                    $"{new string('=', Convert.ToInt32(percentage * 20 / 100))}" +
                    $"{new string(' ', Convert.ToInt32((100 - percentage) * 20 / 100))}]";
        }

        /// <summary>
        /// Runs the specified command in command prompt.
        /// </summary>
        /// <param name="command">The command to pass into command prompt.</param>
        /// <param name="waitForExit">Whether the thread will lock until process completes.</param>
        private int CMD(string command, bool waitForExit)
        {
            Process cmdTask = new Process();
            cmdTask.StartInfo.FileName = "CMD.exe";
            cmdTask.StartInfo.Arguments = "/C " + command;
            cmdTask.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmdTask.Start();
            if (waitForExit)
            {
                cmdTask.WaitForExit();
                return cmdTask.ExitCode;
            }
            return 0;
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
            if (stop) return;
            int auditLine = Log("Enabling Auditing...", false);

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
                        editLog(auditLine, "Enabling Auditing... FAILED", false);
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

                    editLog(auditLine, "Enabling Auditing... DONE", false);
                    return;
                }
            }
            editLog(auditLine, "Enabling Auditing... FAILED", false);
        }

        private void ConfigureUsers(string password)
        {
            if (stop) return;

            if (TxtBoxPass.Text.Length < 8)
            {
                Log("Please provide a password before configuring users.", true);
                Log("Skipping configuring users...", false);
                return;
            }

            int userLine = Log("Configuring users...", false);
            ArrayList userList;
            ArrayList admins;
            try
            {
                userList = new ArrayList();
                userList.AddRange(TxtBoxUsers.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
                if (userList.Count < 2 || !userList.Contains("Authorized Administrators:") || !userList.Contains("Authorized Users:"))
                {
                    Log("Please paste \"Authorized Users\" and \"Authorized Administrators\" text in textbox.", false);
                    editLog(userLine, "Configuring users... FAILED", false);
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
                editLog(userLine, "Configuring users... FAILED", false);
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
                if (stop)
                {
                    editLog(userLine, "Configuring users... CANCELLED", false);
                    return;
                }
                if (user.Equals("Administrator") || user.Equals("Guest"))
                {
                    Log($"Ensuring {user} is disabled...", true);
                    userAdminCmd = $"net user \"{user}\" /active:no";
                }
                else
                {

                    if (!userList.Contains(user))
                    {
                        Log($"Deleting \"{user}\"...", true);
                        CMD($"net user \"{user}\" /delete", false);
                        continue;
                    }

                    if (admins.Contains(user))
                    {
                        Log($"Setting {user} as admin...", true);
                        userAdminCmd = $"net localgroup \"Administrators\" \"{user}\" /ADD";
                    }
                    else
                    {
                        Log($"Setting {user} as normal user...", true);
                        userAdminCmd = $"net localgroup \"Administrators\" \"{user}\" /DELETE";
                    }
                }
                CMD(userAdminCmd, false);

                if (!admins[0].Equals(user))
                    CMD($"net user \"{user}\" \"{password}\"", false);
            }
            editLog(userLine, "Configuring users... DONE", false);
        }

        private void InstallProgram(string url, string name, string args)
        {
            int downloadLine = Log($"Downloading {name}...", false);
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFileAsync(new Uri(url), $"C:\\Windows\\Temp\\{name}_setup.exe");
                    while (client.IsBusy)
                    {
                        client.DownloadProgressChanged += (s, e) =>
                        {
                            editLog(downloadLine, $"Downloading {name}... {loadingBar(e.ProgressPercentage)}", false);
                        };
                        if (stop)
                        {
                            client.CancelAsync();
                            editLog(downloadLine, $"Downloading {name}... CANCELLED", false);
                            return;
                        }
                        Task.Delay(500);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message, true);
                editLog(downloadLine, $"Downloading {name}... FAILED", false);
                return;
            }

            try
            { 
            int installLine = Log($"Installing {name}...", false);

                int exitCode = CMD($"C:\\Windows\\Temp\\{name}_setup.exe {args}" , true);

                if(exitCode == 0)
                    editLog(installLine, $"Installing {name}... DONE", false);
                else
                {
                    editLog(installLine, $"Installing {name}... FAILED", false);
                }
                File.Delete($"C:\\Windows\\Temp\\{name}_setup.exe");
            }
            catch (Exception ex)
            {
                Log(ex.Message, true);
                editLog(installLine, $"Installing {name}... FAILED", false);
            }
        }

        private void InstallMalawareBytes()
        {
            if (stop) return;
            if (File.Exists(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe"))
            {
                Log("MalwareBytes already installed.", true);
                Log("Skipping installing MalwareBytes...", false);
            }
            else
            {
                InstallProgram("https://downloads.malwarebytes.com/file/mb3/",
                    "MalwareBytes",
                    "/VERYSILENT /SUPPRESSMSGBOXES /NOCANCEL /NORESTART /SP- /LOG= %TEMP%\\mb3_install.log");
            }
        }

        private void UpdateFirefox()
        {
            if (stop) return;
            if (File.Exists(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe"))
            {
                Log("Ensuring Firefox is up to date...", false);
                Process.Start(@"C:\Program Files (x86)\Mozilla Firefox\updater.exe");
            }
            else if (File.Exists(@"C:\Program Files\Mozilla Firefox\firefox.exe"))
            {
                Log("Ensuring Firefox is up to date...", false);
                Process.Start(@"C:\Program Files\Mozilla Firefox\updater.exe");
            }
            else
            {
                InstallProgram("https://download.mozilla.org/?product=firefox-stub&os=win&lang=en-US",
                    "Firefox",
                    "-ms");
            }
        }

        public class search_OnCompleted : ISearchCompletedCallback
        {
            MainForm form;
            public search_OnCompleted(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(ISearchJob downloadJob, ISearchCompletedCallbackArgs e)
            {
                if (form.stop)
                {
                    form.editLog(form.searchLine, $"Checking for Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.searched = true;
                    form.editLog(form.searchLine, $"Checking for Windows Updates... DONE",
                        false);
                }
            }
        }

        public class download_OnProgressChanged : IDownloadProgressChangedCallback
        {
            MainForm form;
            public download_OnProgressChanged(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs e)
            {
                form.editLog(form.downloadLine, $"Downloading Windows Updates... {loadingBar(e.Progress.PercentComplete)}", false);
            }
        }

        public class download_OnCompleted : IDownloadCompletedCallback
        {
            MainForm form;
            public download_OnCompleted(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(IDownloadJob downloadJob, IDownloadCompletedCallbackArgs e)
            {
                if (form.stop)
                {
                    form.editLog(form.downloadLine, $"Downloading Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.downloaded = true;
                    form.editLog(form.downloadLine, $"Downloading Windows Updates... 100% [========DONE========]",
                        false);
                }
            }
        }

        public class install_OnProgressChanged : IInstallationProgressChangedCallback
        {
            MainForm form;
            public install_OnProgressChanged(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(IInstallationJob downloadJob, IInstallationProgressChangedCallbackArgs e)
            {
                form.editLog(form.installLine, $"Installing Windows Updates... {loadingBar(e.Progress.PercentComplete)}", false);
            }
        }

        public class install_OnCompleted : IInstallationCompletedCallback
        {
            MainForm form;
            public install_OnCompleted(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(IInstallationJob downloadJob, IInstallationCompletedCallbackArgs e)
            {
                if (form.stop)
                {
                    form.editLog(form.installLine, $"Installing Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.installed = true;
                    form.editLog(form.installLine, $"Installing Windows Updates... 100% [========DONE========]",
                        false);
                }
            }
        }

        private bool searched;
        private bool downloaded;
        private bool installed;
        private int searchLine;
        private int downloadLine;
        private int installLine;
        private void UpdateWindows()
        {
            if (stop) return;

            searched = false;
            downloaded = false;
            installed = false;
            searchLine = Log("Checking for Windows Updates...", false);
            UpdateSession updateSession = new UpdateSession();

            IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
            updateSearcher.Online = true;
            ISearchJob searchJob = updateSearcher.BeginSearch("IsInstalled=0 AND BrowseOnly=0 AND IsHidden=0", new search_OnCompleted(this), null);
            while (!searched)
            {
                if (stop)
                {
                    searchJob.RequestAbort();
                    editLog(searchLine, "Checking for Windows Updates... CANCELLED", false);
                    return;
                }
                Task.Delay(500);
            }
            ISearchResult searchResult = updateSearcher.EndSearch(searchJob);
            editLog(searchLine, "Checking for Windows Updates... DONE", false);

            if (searchResult.Updates.Count < 1)
            {
                Log("No Windows Updates to download.", false);
            }
            else
            {
                downloadLine = Log("Downloading Windows Updates...", false);
                UpdateDownloader downloader = updateSession.CreateUpdateDownloader();
                downloader.Updates = searchResult.Updates;
                IDownloadJob downloadJob = downloader.BeginDownload(new download_OnProgressChanged(this), new download_OnCompleted(this), null);
                while (!downloaded)
                {
                    if (stop)
                    {
                        downloadJob.RequestAbort();
                        editLog(downloadLine, "Downloading Windows Updates... CANCELLED", false);
                        return;
                    }
                    Task.Delay(500);
                }
                downloader.EndDownload(downloadJob);
                

                UpdateCollection updatesToInstall = new UpdateCollection();
                foreach (IUpdate update in searchResult.Updates)
                {
                    if (update.IsDownloaded)
                    {
                        updatesToInstall.Add(update);
                    }
                }

                if(updatesToInstall.Count < 1)
                {
                    editLog(downloadLine, "Downloading Windows Updates... FAILED", false);
                    return;
                } else
                {
                    editLog(downloadLine, "Downloading Windows Updates... DONE", false);
                }

                installLine = Log("Installing Windows Updates...", false);
                IUpdateInstaller installer = updateSession.CreateUpdateInstaller();
                installer.Updates = updatesToInstall;
                IInstallationJob installationJob = installer.BeginInstall(new install_OnProgressChanged(this), new install_OnCompleted(this), null);
                while (!installed)
                {
                    if (stop)
                    {
                        installationJob.RequestAbort();
                        editLog(installLine, "Installing Windows Updates... CANCLLED", false);
                        return;
                    }
                    Task.Delay(500);
                }
                installer.EndInstall(installationJob);
                editLog(installLine, "Installing Windows Updates... DONE", false);
            }
        }


        private void ConfigurePolicy()
        {
            if (stop) return;

            int policyLine = Log("Configuring password policy...", false);

            try
            {
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
            } catch (Exception ex)
            {
                Log(ex.Message, true);
                editLog(policyLine, "Configuring password policy... FAILED", false);
                return;
            }
            editLog(policyLine, "Configuring password policy... DONE", false);
        }

        private void listOfTasks(object sender, DoWorkEventArgs e)
        {
            ProgressBar.Maximum = 6;
            ProgressBar.Step = 1;
            ProgressBar.Value = 0;

            EnableAuditing();
            ++ProgressBar.Value;
            ConfigureUsers(TxtBoxPass.Text);
            ++ProgressBar.Value;
            ConfigurePolicy();
            ++ProgressBar.Value;
            InstallMalawareBytes();
            ++ProgressBar.Value;
            UpdateFirefox();
            ++ProgressBar.Value;
            UpdateWindows();
            ++ProgressBar.Value;
        }

        BackgroundWorker tasks;
        private async void BtnSecure_Click(object sender, EventArgs e)
        {
            BtnSecure.Enabled = false;
            tasks = new BackgroundWorker();
            tasks.DoWork += new DoWorkEventHandler(listOfTasks);
            tasks.RunWorkerAsync();

            BtnStop.Enabled = true;
            while (tasks.IsBusy)
            {
                await Task.Delay(500);
                if (stop) try { tasks.CancelAsync(); } catch (Exception ex) { };
            }

            if (stop)
            {
                Log("The wall was not built ;(", false);
                Log("----------------------------------------------------", false);
            }
            else
            {
                Log("The wall has been built!", false);
                Log("----------------------------------------------------", false);
            }

            ProgressBar.Value = 0;
            stop = false;
            BtnStop.Enabled = false;
            BtnSecure.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            Log("Cancelling process...", false);
            stop = true;
            BtnStop.Enabled = false;
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
