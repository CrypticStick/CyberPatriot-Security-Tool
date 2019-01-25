using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Management;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WUApiLib;

namespace Trump_s_Cyber_Security_Firewall_TM
{
    public partial class MainForm : Form
    {
        //------------------------------------//
        //              Start Up              //
        //------------------------------------//

        Applications apps;
        public static bool stop = false;
        public static MainForm form;

        public MainForm()
        {
            InitializeComponent();
            MinimumSize = new System.Drawing.Size(520, 587);
            TxtBoxInfo.AppendText(Environment.UserName);
            Init(this);
            apps = new Applications(this);
        }

        private static void Init(MainForm f)
        {
            form = f;
        }

        private void ElevateUser()
        {

            Log("Restarting as System user...", false);
            try
            {
                File.WriteAllBytes("C:\\Windows\\Temp\\PsExec64.exe",
                    Properties.Resources.PsExec
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

            UpdateUninstallList();
            UpdateInstallList();
            UpdateGroupList();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        //------------------------------------//
        //          Special Classes           //
        //------------------------------------//

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
                form = MainForm.form;
                if (stop) return;
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
                    if (stop) break;
                    Thread.Sleep(500);
                }
                int code = func.EndInvoke(result);
                if (stop) Cancelled();
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

        public class Search_OnCompleted : ISearchCompletedCallback
        {
            MainForm form;
            int searchLine;
            public Search_OnCompleted(MainForm mainForm, int searchLine)
            {
                form = mainForm;
                this.searchLine = searchLine;
            }
            public void Invoke(ISearchJob downloadJob, ISearchCompletedCallbackArgs e)
            {
                if (stop)
                {
                    form.EditLog(searchLine, $"Checking for Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.searched = true;
                    form.EditLog(searchLine, $"Checking for Windows Updates... DONE",
                        false);
                }
            }
        }

        public class Download_OnProgressChanged : IDownloadProgressChangedCallback
        {
            MainForm form;
            int downloadLine;
            public Download_OnProgressChanged(MainForm mainForm, int downloadLine)
            {
                form = mainForm;
                this.downloadLine = downloadLine;
            }
            public void Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs e)
            {
                form.EditLog(downloadLine, $"Downloading Windows Updates... {LogTask.LoadingBar(e.Progress.PercentComplete)}", false);
            }
        }

        public class Download_OnCompleted : IDownloadCompletedCallback
        {
            MainForm form;
            int downloadLine;
            public Download_OnCompleted(MainForm mainForm, int downloadLine)
            {
                form = mainForm;
                this.downloadLine = downloadLine;
            }
            public void Invoke(IDownloadJob downloadJob, IDownloadCompletedCallbackArgs e)
            {
                if (stop)
                {
                    form.EditLog(downloadLine, $"Downloading Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.downloaded = true;
                    form.EditLog(downloadLine, $"Downloading Windows Updates... 100% [========DONE========]",
                        false);
                }
            }
        }

        public class Install_OnProgressChanged : IInstallationProgressChangedCallback
        {
            MainForm form;
            int installLine;
            public Install_OnProgressChanged(MainForm mainForm, int installLine)
            {
                form = mainForm;
                this.installLine = installLine;
            }
            public void Invoke(IInstallationJob downloadJob, IInstallationProgressChangedCallbackArgs e)
            {
                form.EditLog(installLine, $"Installing Windows Updates... {LogTask.LoadingBar(e.Progress.PercentComplete)}", false);
            }
        }

        public class Install_OnCompleted : IInstallationCompletedCallback
        {
            MainForm form;
            int installLine;
            public Install_OnCompleted(MainForm mainForm, int installLine)
            {
                form = mainForm;
                this.installLine = installLine;
            }
            public void Invoke(IInstallationJob downloadJob, IInstallationCompletedCallbackArgs e)
            {
                if (stop)
                {
                    form.EditLog(installLine, $"Installing Windows Updates... CANCELLED", false);
                }
                else
                {
                    form.installed = true;
                    form.EditLog(installLine, $"Installing Windows Updates... 100% [========DONE========]",
                        false);
                }
            }
        }

        //------------------------------------//
        //             Functions              //
        //------------------------------------//

        private void UpdateUninstallList()
        {
            using (RegistryKey key = AccessRegistryKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                    true))
            {
                if (key != null)
                {
                    ChkLstBoxUninstall.Items.Clear();
                    foreach (string program in key.GetSubKeyNames())
                    {
                        string potentialString = Convert.ToString(key.OpenSubKey(program).GetValue("UninstallString"));
                        if (potentialString != null && potentialString != "")
                        {
                            string programName = Convert.ToString(key.OpenSubKey(program).GetValue("DisplayName"));
                            ChkLstBoxUninstall.Items.Add(programName, false);
                        }
                    }
                }
            }
            ChkLstBoxUninstall.Update();
        }

        private void UpdateInstallList()
        {
            ChkLstBoxInstall.Items.Clear();
            foreach (Applications.Program program in (Applications.Program[])Enum.GetValues(typeof(Applications.Program)))
                ChkLstBoxInstall.Items.Add(program.ToString());
        }

        private void UpdateGroupList()
        {
            try
            {
                DirectoryEntry machine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer");
                foreach (DirectoryEntry child in machine.Children)
                {
                    if (child.SchemaClassName == "Group")
                    {
                        CmboBoxGroups.Items.Add(child.Name);
                    }
                }
                CmboBoxGroups.Update();
            }
            catch (Exception ex) { Log(ex.Message, true); }
        }

        /// <summary>
        /// Logs the specified string to TxtBoxInfo on a new line.
        /// </summary>
        public int Log(string message, bool debug)
        {
            if (!ChkDebug.Checked && debug) return -1;
            TxtBoxInfo.AppendText(Environment.NewLine);
            TxtBoxInfo.AppendText(message);

            TxtBoxInfo.Focus();
            TxtBoxInfo.SelectionStart = TxtBoxInfo.TextLength;
            TxtBoxInfo.ScrollToCaret();
            TxtBoxInfo.Update();

            return TxtBoxInfo.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Length - 1;
        }

        /// <summary>
        /// Edits specified line in TxtBoxInfo.
        /// </summary>
        private void EditLog(int line, string message, bool debug)
        {
            if (!ChkDebug.Checked && debug) return;

            string[] lines = TxtBoxInfo.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == line)
                {
                    builder.Append(Environment.NewLine);
                    builder.Append(message);
                }
                else
                {
                    if (i != 0)
                        builder.Append(Environment.NewLine);
                    builder.Append(lines[i]);
                }
            }

            TxtBoxInfo.Text = builder.ToString();

            TxtBoxInfo.Focus();
            TxtBoxInfo.SelectionStart = TxtBoxInfo.TextLength;
            TxtBoxInfo.ScrollToCaret();
            TxtBoxInfo.Update();
        }

        /// <summary>
        /// Runs the specified command in command prompt.
        /// </summary>
        /// <param name="command">The command to pass into command prompt.</param>
        /// <param name="waitForExit">Whether the thread will lock until process completes.</param>
        public int CMD(string command, bool waitForExit)
        {
            Process cmdTask = new Process();
            cmdTask.StartInfo.FileName = "CMD.exe";

            if (command.Contains(".exe"))
            {
                Log("Running: " + "start /wait \"\" " + command, true);
                cmdTask.StartInfo.Arguments = "/C " + "start /wait \"\" " + command;
            }
            else
            {
                Log("Running: " + command, true);
                cmdTask.StartInfo.Arguments = "/C " + command;
            }


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
        public RegistryKey AccessRegistryKey(string key, bool writable)
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

        private void GetAllFoldersUnder(string path, ref List<string> dirs)
        {
            try
            {
                if ((File.GetAttributes(path) & FileAttributes.ReparsePoint)
                    != FileAttributes.ReparsePoint)
                {
                    foreach (string folder in Directory.GetDirectories(path))
                    {
                        dirs.Add(folder);
                        GetAllFoldersUnder(folder, ref dirs);
                    }
                }
            }
            catch (UnauthorizedAccessException) { }
        }

        //------------------------------------//
        //            Main Tasks              //
        //------------------------------------//

        private void ListOfTasks(object sender, DoWorkEventArgs e)
        {
            ProgressBar.Maximum = 9;
            ProgressBar.Step = 1;
            ProgressBar.Value = 0;

            new LogTask("Enabling auditing", EnableAuditing); ++ProgressBar.Value;
            new LogTask("Configuring users", ConfigureUsers); ++ProgressBar.Value;
            new LogTask("Configuring policy", ConfigurePolicy); ++ProgressBar.Value;
            new LogTask("Removing unauthorized files", RemoveUnauthorizedFiles); ++ProgressBar.Value;
            new LogTask("Uninstalling programs", apps.UninstallPrograms, ChkLstBoxUninstall.CheckedIndices.GetEnumerator()); ++ProgressBar.Value;
            new LogTask("Installing programs", apps.InstallPrograms, ChkLstBoxInstall.CheckedIndices.GetEnumerator()); ++ProgressBar.Value;
            UpdateWindows(); ++ProgressBar.Value;
        }

        private int EnableAuditing(PortableNumber p, params object[] args)
        {
            if (stop) return 1;

            int endindex = 12;
            int Major = 0;
            int Minor = 0;

            KeyValuePair<string, string> kvpOSSpecs = new KeyValuePair<string, string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption, Version FROM Win32_OperatingSystem");
            try
            {

                foreach (var os in searcher.Get())
                {
                    var version = os["Version"].ToString();
                    var productName = os["Caption"].ToString();
                    kvpOSSpecs = new KeyValuePair<string, string>(productName, version);
                }

                Major = Convert.ToInt32(kvpOSSpecs.Value.Split('.')[0]);
                Minor = Convert.ToInt32(kvpOSSpecs.Value.Split('.')[1]);
            }
            catch { }

            switch (Major)
            {
                case 6:
                    if (Minor == 0)
                    {
                        endindex = 114; //Vista
                        Log("WinVista detected.", false);
                    }
                    else if (Minor == 1)
                    {
                        endindex = 116; //7
                        Log("Win7 detected.", false);
                    }
                    else if (Minor == 2)
                    {
                        endindex = 122; //8
                        Log("Win8 detected.", false);
                    }
                    else if (Minor == 3)
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

            List<string> usersOnSystem = new List<string>();
            using (var key = AccessRegistryKey(@"SECURITY\SAM\Domains\Account\Users\Names", false))
            {
                if (key != null)
                {
                    Log("Reading list of users...", true);
                    usersOnSystem.AddRange(key.GetSubKeyNames());
                }
            }

            String userAdminCmd;

            foreach (string user in userList)
            {
                if (!usersOnSystem.Contains(user))
                {
                    CMD($"net user \"{user}\" \"{TxtBoxPass.Text}\" /add", false);
                }
            }

            foreach (string user in usersOnSystem)
            {
                if (stop) return 1;
                if (user.Equals("Administrator") || user.Equals("Guest"))
                {
                    Log($"Ensuring {user} is disabled...", true);
                    userAdminCmd = $"net user \"{user}\" /active:no & wmic useraccount where name='{user}' rename '{user}-Renamed'";
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
                CMD("NetSh Advfirewall set allprofiles state on", false);

                CMD("net accounts /minpwlen:10", false);
                CMD("net accounts /maxpwage:30", false);
                CMD("net accounts /minpwage:10", false);
                CMD("net accounts /uniquepw:8", false);
                CMD("net accounts /lockoutthreshold:5", false);

                CMD(@"secedit.exe /export /cfg C:\Windows\Temp\secconfig.cfg", true);
                string text = File.ReadAllText(@"C:\Windows\Temp\secconfig.cfg");
                text = text.Replace("PasswordComplexity = 0", "PasswordComplexity = 1");
                File.WriteAllText(@"C:\Windows\Temp\secconfig.cfg", text);
                CMD(@"secedit.exe / configure / db % windir %\securitynew.sdb / cfg C:\Windows\Temp\secconfig.cfg / areas SECURITYPOLICY", false);

                using (RegistryKey key = AccessRegistryKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true))
                    if (key != null)
                        key.SetValue("EnableLUA", 1);

                using (RegistryKey key = AccessRegistryKey(
                @"SOFTWARE\Policies\Microsoft\Windows NT\Reliability", true))
                    if (key != null)
                    {
                        key.SetValue("ShutdownReason", 1);
                        key.SetValue("OnShutdownReasonUI", 1);
                    }

                using (RegistryKey key = AccessRegistryKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", true))
                    if (key != null)
                    {
                        key.SetValue("NoControlPanel", 1);
                        key.SetValue("NoRun", 1);
                    }

                using (RegistryKey key = AccessRegistryKey(
                @"SYSTEM\CurrentControlSet\Control\Lsa", true))
                    if (key != null)
                    {
                        key.SetValue("auditbasedirectories", 1);
                        key.SetValue("auditbaseobjects", 1);
                        //key.SetValue("crashonauditfail", 1);
                        key.SetValue("fullprivilegeauditing", 1);
                        key.SetValue("LimitBlankPasswordUse", 1);
                        key.SetValue("RestrictAnonymous", 1);
                        key.SetValue("RestrictAnonymousSAM", 1);
                    }

                using (RegistryKey key = AccessRegistryKey(
                @"SOFTWARE\Classes\Msi.Package\DefaultIcon", true))
                    if (key != null)
                        key.SetValue(null, @"C:\Windows\System32\msiexec.exe,1");

                using (RegistryKey key = AccessRegistryKey(
                @"SOFTWARE\Policies\Microsoft\Windows", true))
                    if (key != null)
                    {
                        key.CreateSubKey("WindowsUpdate").CreateSubKey("AU").
                            SetValue("NoAutoRebootWithLoggedOnUsers", 1);
                        key.CreateSubKey("RemovableStorageDevices").
                            SetValue("Deny_All", 1);
                    }

                using (RegistryKey key = AccessRegistryKey(
                @"SYSTEM\CurrentControlSet\Services\USBSTOR", true))
                    if (key != null)
                        key.SetValue("Start", 4);

                using (RegistryKey key = AccessRegistryKey(
                @"SYSTEM\CurrentControlSet\Control", true))
                    if (key != null)
                        key.CreateSubKey("StorageDevicePolicies").
                            SetValue("WriteProtect", 1);

                using (RegistryKey key = AccessRegistryKey(
                @"SOFTWARE\Policies\Microsoft\Windows\System", true))
                    if (key != null)
                        key.SetValue("DisableCMD", 1);

                using (RegistryKey key = AccessRegistryKey(
                @"SYSTEM\CurrentControlSet\Control\Terminal Server", true))
                    if (key != null)
                        key.SetValue("fDenyTSConnections", (ChkRDP.Checked) ? 0 : 1);

                using (RegistryKey key = AccessRegistryKey(
                @"SYSTEM\CurrentControlSet\Control\Remote Assistance", true))
                    if (key != null)
                    { 
                        key.SetValue("fAllowToGetHelp", (ChkRDP.Checked) ? 1 : 0);
                        CMD($"netsh advfirewall firewall set rule group = \"Remote Assistance\" new enable=" + ((ChkRDP.Checked) ? "yes" : "no"), false);
                    }

                foreach (string service in new string[] { "msftpsvc", "termservice" })
                {
                    CMD($"sc config \"{service}\" start= {((ChkRDP.Checked) ? "enabled" : "disabled")}", false);
                    CMD($"sc {((ChkRDP.Checked) ? "start" : "stop")} \"{service}\"", false);
                }

            }
            catch (Exception ex)
            {
                Log(ex.Message, true);
                return 2;
            }
            return 0;
        }

        private int RemoveUnauthorizedFiles(PortableNumber p, params object[] args)
        {
            try
            {
                string[] prohibitedFileTypes = new string[] { "mp3", "mp4", "wav" };
                List<string> dirs = new List<string>();
                GetAllFoldersUnder(@"C:\Users", ref dirs);
                List<string> badFiles = new List<string>();

                foreach (string dir in dirs)
                    foreach (string extention in prohibitedFileTypes)
                        try
                        {
                            foreach (string file in Directory.GetFiles(dir, $"*.{extention}", SearchOption.TopDirectoryOnly))
                                try { badFiles.Add(file); }
                                catch (UnauthorizedAccessException ex) { Log(ex.Message, true); }
                        }
                        catch (Exception ex) { Log(ex.Message, true); }

                foreach (string badFile in badFiles)
                {
                    if (!badFile.Contains("(Deleted)."))
                    {
                        File.Move(badFile,
                            badFile.Insert(badFile.LastIndexOf('.'), " (Deleted)"));
                        Log($"'Deleted' {badFile}", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message, true);
                return 2;
            }
            return 0;
        }

        bool searched;
        bool downloaded;
        bool installed;

        private void UpdateWindows()
        {
            if (stop) return;

            int searchLine;
            int downloadLine;
            int installLine;

            searched = false;
            downloaded = false;
            installed = false;

            searchLine = Log("Checking for Windows Updates...", false);
            UpdateSession updateSession = new UpdateSession();

            IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
            updateSearcher.Online = true;
            ISearchJob searchJob = updateSearcher.BeginSearch("IsInstalled=0 AND BrowseOnly=0 AND IsHidden=0", new Search_OnCompleted(this, searchLine), null);
            while (!searched)
            {
                if (stop)
                {
                    searchJob.RequestAbort();
                    EditLog(searchLine, "Checking for Windows Updates... CANCELLED", false);
                    return;
                }
                Thread.Sleep(500);
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
                IDownloadJob downloadJob = downloader.BeginDownload(new Download_OnProgressChanged(this, downloadLine), new Download_OnCompleted(this, downloadLine), null);
                while (!downloaded)
                {
                    if (stop)
                    {
                        downloadJob.RequestAbort();
                        EditLog(downloadLine, "Downloading Windows Updates... CANCELLED", false);
                        return;
                    }
                    Thread.Sleep(500);
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
                IInstallationJob installationJob = installer.BeginInstall(new Install_OnProgressChanged(this, installLine), new Install_OnCompleted(this, installLine), null);
                while (!installed)
                {
                    if (stop)
                    {
                        installationJob.RequestAbort();
                        EditLog(installLine, "Installing Windows Updates... CANCLLED", false);
                        return;
                    }
                    Thread.Sleep(500);
                }
                installer.EndInstall(installationJob);
                EditLog(installLine, "Installing Windows Updates... DONE", false);
            }
        }

        //------------------------------------//
        //            GUI Events              //
        //------------------------------------//

        private void BtnStop_Click(object sender, EventArgs e)
        {
            Log("Cancelling process...", false);
            stop = true;
            BtnStop.Enabled = false;
        }

        private void BtnBrowseFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(@"C:\Windows\system32\config\systemprofile\Desktop"))
                    Directory.CreateDirectory(@"C:\Windows\system32\config\systemprofile\Desktop");
            }
            catch (Exception ex) { Log(ex.Message, true); }

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtBoxBrowseFile.Text = openFileDialog.FileName;
            }
        }

        private void TxtBoxBrowseFile_TextChanged(object sender, EventArgs e)
        {
            string pathToFile = TxtBoxBrowseFile.Text;
            if (File.Exists(pathToFile))
            {
                string md5Hash = "N/A";
                try
                {
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(pathToFile))
                        {
                            var hash = md5.ComputeHash(stream);
                            md5Hash = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
                        }
                    }

                    FileInfo Fi = new FileInfo(pathToFile);

                    TxtBoxFileInfo.Text =
                    "File Name: " + Path.GetFileName(pathToFile) + Environment.NewLine +
                    "Owner: " + Fi.GetAccessControl().GetOwner(typeof(NTAccount)).ToString() + Environment.NewLine +
                    "Group: " + Fi.GetAccessControl().GetGroup(typeof(NTAccount)).ToString() + Environment.NewLine +
                    "MD5 Hash: " + md5Hash;

                }
                catch (Exception ex) { Log(ex.Message, true); }
            }
        }
        private void CmboBoxGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new PrincipalContext(ContextType.Machine))
            {
                using (var group = GroupPrincipal.FindByIdentity(context, CmboBoxGroups.SelectedItem.ToString()))
                {
                    if (group == null)
                    {
                        MessageBox.Show("Group does not exist!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                            );
                    }
                    else
                    {
                        var users = group.GetMembers(true);
                        TxtBoxGroupInfo.Text = "Users: ";
                        try
                        {
                            foreach (UserPrincipal user in users)
                            {
                                TxtBoxGroupInfo.AppendText(Environment.NewLine + user.Name);
                            }
                        }
                        catch (Exception ex) { Log(ex.Message, true); }
                    }
                }
            }
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

            UpdateUninstallList();

            if (stop)
                Log(@"The wall was somewhat finished ¯\_(ツ)_/¯", false);
            else
                Log("The wall has been built!", false);

            Log("Note: 9/10 doctors agree the computer must be restarted!", false);
            Log("----------------------------------------------------", false);

            ProgressBar.Value = 0;
            stop = false;
            BtnStop.Enabled = false;
            BtnSecure.Enabled = true;
        }

        //TODO:
        /*
         * Add new list of features to add after competition!!
         */
    }
}
