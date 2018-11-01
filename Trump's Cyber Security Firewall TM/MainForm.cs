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
            MinimumSize = new System.Drawing.Size(690, 722);
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
                    catch (Exception ex)
                    {
                        Log(ex.Message, true);
                    }

                BtnSecure.Enabled = true;

            }
            else if (new WindowsPrincipal(WindowsIdentity.GetCurrent())
              .IsInRole(WindowsBuiltInRole.Administrator))
            {
                ElevateUser();
            }
            UpdateProgramList();
        }

        public delegate int mTask(PortableNumber p, params object[] args);
        public class PortableNumber
        {
            public PortableNumber(int n) { Number = n; }
            public int Number { get; set; }
        }
        /// <summary>
        /// Takes a function and runs it with automatic logging.
        /// </summary>
        public class LogTask
        {
            mTask func;
            MainForm form;
            string message = null;
            object[] args = { 0 };
            PortableNumber percent = new PortableNumber(-1);
            bool percentBased = false;
            int logLine = -1;

            /// <summary>
            /// Takes a function and runs it with automatic logging. 
            /// <para>Requires <see cref="string"/> message and <see cref="Task"/>, accepts any extra parameters.</para>
            /// </summary>
            /// <param name="message">The message that will be displayed while the function is running.</param>
            ///             /// <param name="func"></param>
            /// <param name="args">An array of any parameters that the specified function requires.</param>
            public LogTask(string message, mTask func, params object[] args)
            {
                this.func = func;
                form = (MainForm)(func.Target);
                this.message = message;
                this.args = args;
                logLine = form.Log(message + "...", false);

                IAsyncResult result = func.BeginInvoke(this.percent, args, null, null);

                int percent = -1;
                while (!result.IsCompleted)
                {
                    if (percent != this.percent.Number)
                    {
                        percentBased = true;
                        percent = this.percent.Number;
                        Progress(percent);
                    }
                }
                int code = func.EndInvoke(result);
                if (form.stop) Cancelled();
                else if (code == 0) Done();
                else Failed();
            }
            public static string LoadingBar(int percentage)
            {
                string line = new string('=', Convert.ToInt32(percentage * 20 / 100));
                return $"{percentage}% [" +
                        $"{line}" +
                        $"{new string(' ', 20 - line.Length)}]";
            }
            private void Progress(int percent)
            {
                form.EditLog(logLine, message + "... " + LoadingBar(percent), false);
            }
            private void Done()
            {
                if (percentBased)
                    form.EditLog(logLine, message + "... 100% [========DONE========]", false);
                else
                    form.EditLog(logLine, message + "... DONE", false);
            }
            private void Failed()
            {
                form.EditLog(logLine, message + "... FAILED", false);
            }
            private void Cancelled()
            {
                form.EditLog(logLine, message + "... CANCELLED", false);
            }
        }

        private void UpdateProgramList()
        {
            using (RegistryKey key = AccessRegistryKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                    true))
            {
                if (key != null)
                {
                    ChkLstBxPrograms.Items.Clear();
                    foreach (string program in key.GetSubKeyNames())
                    {
                        string potentialString = Convert.ToString(key.OpenSubKey(program).GetValue("UninstallString"));
                        if (potentialString != null && potentialString != "")
                        {
                            string programName = Convert.ToString(key.OpenSubKey(program).GetValue("DisplayName"));
                            ChkLstBxPrograms.Items.Add(programName, false);
                        }
                    }
                }
            }
            ChkLstBxPrograms.Update();
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

        /// <summary>
        /// Edits specified line in TxtBoxInfo.
        /// </summary>
        private void EditLog(int line, string message, bool debug)
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

        /// <summary>
        /// Runs the specified command in command prompt.
        /// </summary>
        /// <param name="command">The command to pass into command prompt.</param>
        /// <param name="waitForExit">Whether the thread will lock until process completes.</param>
        private int CMD(string command, bool waitForExit)
        {
            Log("Running: " + command, true);
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
                Log(ex.Message, true);
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

        private int EnableAuditing(PortableNumber p, params object[] args)
        {
            if (stop) return 1;

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
                        return 2;
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
                    return 0;
                }
            }
            return 3;
        }

        private int ConfigureUsers(PortableNumber p, params object[] args)
        {
            if (stop) return 1;

            if (TxtBoxPass.Text.Length < 8)
            {
                Log("Please provide a password before configuring users.", true);
                Log("Skipping configuring users...", true);
                return 0;
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
                    return 2;
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
                Log(ex.Message, true);
                Log("The provided list of names doesn't work.", false);
                return 3;
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
                if (stop) return 1;
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
                    CMD($"net user \"{user}\" \"{TxtBoxPass.Text}\"", false);
            }
            return 0;
        }



        private int ConfigurePolicy(PortableNumber p, params object[] args)
        {
            if (stop) return 1;
            try
            {
                CMD("wmic UserAccount set PasswordExpires=True", false);

                CMD("net accounts /minpwlen:10", false);
                CMD("net accounts /maxpwage:30", false);
                CMD("net accounts /minpwage:10", false);
                CMD("net accounts /uniquepw:8", false);

                using (RegistryKey key = AccessRegistryKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    true))
                {
                    if (key != null)
                        key.SetValue("EnableLUA", 1);
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message, true);
                return 2;
            }
            return 0;
        }

        class mWebClient : WebClient
        {
            private PortableNumber PortableNumber;
            public mWebClient(PortableNumber p)
            {
                PortableNumber = p;
            }
            public void SetNumber(int n)
            {
                PortableNumber.Number = n;
            }
        }

        private int DownloadProgram(PortableNumber p, params object[] args)
        {
            try
            {
                using (var client = new mWebClient(p))
                {
                    Task.Delay(3000);
                    client.DownloadFileAsync(new Uri((string)args[0]), $"C:\\Windows\\Temp\\{(string)args[1]}_setup.exe");
                    client.DownloadProgressChanged += (s, e) =>
                    {
                        client.SetNumber(e.ProgressPercentage);
                        if (stop)
                        {
                            client.CancelAsync();
                            return;
                        }
                        Task.Delay(500);
                    };
                    while (client.IsBusy)
                        if (stop) return 1;
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message, true);
                return 2;
            }
            return 0;
        }
        private int UninstallProgram(PortableNumber p, params object[] args)
        {
            try
            {
                string uninstallString = (string)args[0];
                int exitCode = -1;

                if (uninstallString != null || uninstallString != "")
                    exitCode = CMD(uninstallString, true);
                else return 2;
                if (exitCode < 0) return 3;
                return 0;
            }
            catch (Exception ex)
            {
                Log(ex.Message, true);
                return 4;
            }
        }
        private int InstallProgram(PortableNumber p, params object[] args)
        {
            if (File.Exists($"C:\\Windows\\Temp\\{args[0]}_setup.exe"))
                try
                {
                    string name = (string)args[0];
                    string arg = (string)args[1];
                    int exitCode = CMD($"C:\\Windows\\Temp\\{name}_setup.exe {arg}", true);

                    if (exitCode < 0) return 2;
                    File.Delete($"C:\\Windows\\Temp\\{name}_setup.exe");
                }
                catch (Exception ex)
                {
                    Log(ex.Message, true);
                    return 3;
                }
            else
            {
                return 4;
            }
            return 0;
        }

        private int UpdateMalwareBytes(PortableNumber p, params object[] arg)
        {
            if (stop) return 1;
            if (File.Exists(@"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe"))
            {
                Log("MalwareBytes already installed.", true);
            }
            else
            {
                new LogTask("Downloading MalwareBytes",
                    DownloadProgram,
                    "https://downloads.malwarebytes.com/file/mb3/",
                    "MalwareBytes");

                new LogTask("Installing MalwareBytes",
                    InstallProgram,
                    "MalwareBytes",
                    "/VERYSILENT /SUPPRESSMSGBOXES /NOCANCEL /NORESTART /SP- /LOG= %TEMP%\\mb3_install.log");
            }
            return 0;
        }
        private int UpdateFirefox(PortableNumber p, params object[] arg)
        {
            if (stop) return 1;
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
                new LogTask("Downloading Firefox",
                    DownloadProgram,
                    "https://download.mozilla.org/?product=firefox-stub&os=win&lang=en-US",
                    "Firefox");

                new LogTask("Installing Firefox",
                    InstallProgram,
                    "Firefox",
                    "-ms");
            }
            return 0;
        }

        private int UninstallPrograms(PortableNumber p, params object[] args)
        {
            if (ChkLstBxPrograms.CheckedItems.Count < 1)
            {
                Log("No programs were selected for uninstallation.", false);
                return 0;
            }

            using (RegistryKey key = AccessRegistryKey(
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
        true))
            {
                if (key != null)
                {

                    foreach (string program in key.GetSubKeyNames())
                    {
                        try
                        {
                            string programName = Convert.ToString(key.OpenSubKey(program).GetValue("DisplayName"));
                            if (ChkLstBxPrograms.CheckedItems.Contains(programName))
                            {
                                string programStringQuiet = Convert.ToString(key.OpenSubKey(program).GetValue("QuietUninstallString"));
                                string programString = Convert.ToString(key.OpenSubKey(program).GetValue("UninstallString"));
                                if (programStringQuiet != null && programStringQuiet != "")
                                    new LogTask($"Uninstalling {programName}", UninstallProgram, programStringQuiet);
                                else if ((programString != null && programString != ""))
                                    new LogTask($"Uninstalling {programName}", UninstallProgram, programString);
                                else Log($"Cannot uninstall {programName}.", false);
                                UpdateProgramList();
                            }
                        }
                        catch (Exception ex)
                        {
                            Log(ex.Message, true);
                            //return 2;
                        }
                    }
                    return 0;
                }
                else
                {
                    return 3;
                }
            }
        }

        public class Search_OnCompleted : ISearchCompletedCallback
        {
            MainForm form;
            public Search_OnCompleted(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(ISearchJob downloadJob, ISearchCompletedCallbackArgs e)
            {
                if (form.stop)
                {
                    form.EditLog(form.searchLine, $"Checking for Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.searched = true;
                    form.EditLog(form.searchLine, $"Checking for Windows Updates... DONE",
                        false);
                }
            }
        }

        public class Download_OnProgressChanged : IDownloadProgressChangedCallback
        {
            MainForm form;
            public Download_OnProgressChanged(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs e)
            {
                form.EditLog(form.downloadLine, $"Downloading Windows Updates... {LogTask.LoadingBar(e.Progress.PercentComplete)}", false);
            }
        }

        public class Download_OnCompleted : IDownloadCompletedCallback
        {
            MainForm form;
            public Download_OnCompleted(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(IDownloadJob downloadJob, IDownloadCompletedCallbackArgs e)
            {
                if (form.stop)
                {
                    form.EditLog(form.downloadLine, $"Downloading Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.downloaded = true;
                    form.EditLog(form.downloadLine, $"Downloading Windows Updates... 100% [========DONE========]",
                        false);
                }
            }
        }

        public class Install_OnProgressChanged : IInstallationProgressChangedCallback
        {
            MainForm form;
            public Install_OnProgressChanged(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(IInstallationJob downloadJob, IInstallationProgressChangedCallbackArgs e)
            {
                form.EditLog(form.installLine, $"Installing Windows Updates... {LogTask.LoadingBar(e.Progress.PercentComplete)}", false);
            }
        }

        public class Install_OnCompleted : IInstallationCompletedCallback
        {
            MainForm form;
            public Install_OnCompleted(MainForm mainForm)
            {
                this.form = mainForm;
            }
            public void Invoke(IInstallationJob downloadJob, IInstallationCompletedCallbackArgs e)
            {
                if (form.stop)
                {
                    form.EditLog(form.installLine, $"Installing Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.installed = true;
                    form.EditLog(form.installLine, $"Installing Windows Updates... 100% [========DONE========]",
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
            ISearchJob searchJob = updateSearcher.BeginSearch("IsInstalled=0 AND BrowseOnly=0 AND IsHidden=0", new Search_OnCompleted(this), null);
            while (!searched)
            {
                if (stop)
                {
                    searchJob.RequestAbort();
                    EditLog(searchLine, "Checking for Windows Updates... CANCELLED", false);
                    return;
                }
                Task.Delay(500);
            }
            ISearchResult searchResult = updateSearcher.EndSearch(searchJob);
            EditLog(searchLine, "Checking for Windows Updates... DONE", false);

            if (searchResult.Updates.Count < 1)
            {
                Log("No Windows Updates to download.", false);
            }
            else
            {
                downloadLine = Log("Downloading Windows Updates...", false);
                UpdateDownloader downloader = updateSession.CreateUpdateDownloader();
                downloader.Updates = searchResult.Updates;
                IDownloadJob downloadJob = downloader.BeginDownload(new Download_OnProgressChanged(this), new Download_OnCompleted(this), null);
                while (!downloaded)
                {
                    if (stop)
                    {
                        downloadJob.RequestAbort();
                        EditLog(downloadLine, "Downloading Windows Updates... CANCELLED", false);
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

                if (updatesToInstall.Count < 1)
                {
                    EditLog(downloadLine, "Downloading Windows Updates... FAILED", false);
                    return;
                }
                else
                {
                    EditLog(downloadLine, "Downloading Windows Updates... DONE", false);
                }

                installLine = Log("Installing Windows Updates...", false);
                IUpdateInstaller installer = updateSession.CreateUpdateInstaller();
                installer.Updates = updatesToInstall;
                IInstallationJob installationJob = installer.BeginInstall(new Install_OnProgressChanged(this), new Install_OnCompleted(this), null);
                while (!installed)
                {
                    if (stop)
                    {
                        installationJob.RequestAbort();
                        EditLog(installLine, "Installing Windows Updates... CANCLLED", false);
                        return;
                    }
                    Task.Delay(500);
                }
                installer.EndInstall(installationJob);
                EditLog(installLine, "Installing Windows Updates... DONE", false);
            }
        }

        private void ListOfTasks(object sender, DoWorkEventArgs e)
        {
            ProgressBar.Maximum = 7;
            ProgressBar.Step = 1;
            ProgressBar.Value = 0;

            new LogTask("Enabling auditing", EnableAuditing);
            ++ProgressBar.Value;
            new LogTask("Configuring users", ConfigureUsers);
            ++ProgressBar.Value;
            new LogTask("Configuring policy", ConfigurePolicy);
            ++ProgressBar.Value;
            new LogTask("Setting up MalwareBytes", UpdateMalwareBytes);
            ++ProgressBar.Value;
            new LogTask("Setting up Firefox", UpdateFirefox);
            ++ProgressBar.Value;
            new LogTask("Uninstalling programs", UninstallPrograms);
            ++ProgressBar.Value;
            UpdateWindows();
            ++ProgressBar.Value;
        }

        BackgroundWorker tasks;
        private async void BtnSecure_Click(object sender, EventArgs e)
        {
            BtnSecure.Enabled = false;
            tasks = new BackgroundWorker();
            tasks.WorkerSupportsCancellation = true;
            tasks.DoWork += new DoWorkEventHandler(ListOfTasks);
            tasks.RunWorkerAsync();

            BtnStop.Enabled = true;
            while (tasks.IsBusy)
            {
                await Task.Delay(500);
                if (stop) try { tasks.CancelAsync(); }
                    catch (Exception ex)
                    {
                        Log(ex.Message, true);
                    }
            }

            UpdateProgramList();

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
