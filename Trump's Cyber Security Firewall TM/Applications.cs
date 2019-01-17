using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trump_s_Cyber_Security_Firewall_TM
{
    class Applications
    {
        MainForm form;

        public ProgramInfo MalawareBytes = new ProgramInfo(
            "MalawareBytes",
            "https://downloads.malwarebytes.com/file/mb3/",
            @"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe",
            "/VERYSILENT /SUPPRESSMSGBOXES /NOCANCEL /NORESTART /SP- /LOG= %TEMP%\\mb3_install.log"
            );

        public ProgramInfo Firefox = new ProgramInfo(
            "Firefox",
            "https://download.mozilla.org/?product=firefox-stub&os=win&lang=en-US",
            @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe",
            "-ms"
            );

        public ProgramInfo NotePadpp = new ProgramInfo(
            "Notepad++",
            "https://notepad-plus-plus.org/repository/7.x/7.6/npp.7.6.Installer.exe",
            @"C:\Program Files (x86)\Notepad++\notepad++.exe",
            "/S"
            );

        public Applications(MainForm form)
        {
            this.form = form;
        }

        public class ProgramInfo
        {
            public string Name { get; }
            public string Url { get; }
            public string InstallFile { get; }
            public string Arg { get; }
            public ProgramInfo(string name, string url, string installFile, string arg)
            {
                Name = name;
                Url = url;
                InstallFile = installFile;
                Arg = arg;
            }
        }

        class mWebClient : WebClient
        {
            private MainForm.PortableNumber PortableNumber;
            public mWebClient(MainForm.PortableNumber p)
            {
                PortableNumber = p;
            }
            public void SetNumber(int n)
            {
                PortableNumber.Number = n;
            }
        }

        public int UninstallProgram(MainForm.PortableNumber p, params object[] args)
        {
            try
            {
                string uninstallString = (string)args[0];
                int exitCode = -1;

                if (uninstallString != null || uninstallString != "")
                    exitCode = form.CMD(uninstallString, true);
                else return 2;
                if (exitCode < 0) return 3;
                return 0;
            }
            catch (Exception ex)
            {
                form.Log(ex.Message, true);
                return 4;
            }
        }

        /// <summary>
        /// Installs specified program to system.
        /// </summary>
        /// <returns></returns>
        public int InstallProgram(ProgramInfo info, bool force)
        {
            return InstallProgram(info.Name, info.Url, info.InstallFile, force, info.Arg);
        }

        /// <summary>
        /// Installs specified program to system.
        /// </summary>
        /// <returns></returns>
        public int InstallProgram(string name, string url, string installFile, bool force, string arg)
        {
            if (MainForm.stop) return 1;
            if (File.Exists(installFile) && !force)
            {
                form.Log($"{name} installed.", true);
            }
            else
            {
                new MainForm.LogTask($"Downloading {name}",
                        (_p, _args) =>
                        {

                            try
                            {
                                using (var client = new mWebClient(_p))
                                {
                                    Task.Delay(3000);
                                    client.DownloadFileAsync(new Uri(url), $"C:\\Windows\\Temp\\{name}_setup.exe");
                                    client.DownloadProgressChanged += (s, e) =>
                                    {
                                        client.SetNumber(e.ProgressPercentage);
                                        if (MainForm.stop)
                                        {
                                            client.CancelAsync();
                                            return;
                                        }
                                        Task.Delay(500);
                                    };
                                    while (client.IsBusy)
                                        if (MainForm.stop) return 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                form.Log(ex.Message, true);
                                return 2;
                            }
                            return 0;

                        });

                new MainForm.LogTask($"Installing {name}",
                    (_p, _args) =>
                    {

                        if (File.Exists($"C:\\Windows\\Temp\\{name}_setup.exe"))
                            try
                            {
                                int exitCode = form.CMD($"C:\\Windows\\Temp\\{name}_setup.exe {arg}", true);
                                File.Delete($"C:\\Windows\\Temp\\{name}_setup.exe");
                                if (exitCode != 0) return 2;
                            }
                            catch (Exception ex)
                            {
                                form.Log(ex.Message, true);
                                return 3;
                            }
                        else
                        {
                            form.Log($"File \"C:\\Windows\\Temp\\{ name}_setup.exe\" does not exist!", true);
                            return 4;
                        }
                        return 0;

                    });
            }
            return 0;
        }

        public int UninstallPrograms(MainForm.PortableNumber p, params object[] args)
        {
            if (args.Length < 1)
            {
                form.Log("No programs were selected for uninstallation.", false);
                return 0;
            }

            using (RegistryKey key = form.AccessRegistryKey(
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
        true))
            {
                if (key != null)
                {
                    IEnumerator enumToUninstall = args.GetEnumerator();
                    List<string> listToUninstall = new List<string>();
                    while (enumToUninstall.MoveNext())
                        listToUninstall.Add((string)enumToUninstall.Current);

                    foreach (string program in key.GetSubKeyNames())
                    {
                        try
                        {
                            string programName = Convert.ToString(key.OpenSubKey(program).GetValue("DisplayName"));
                            if (listToUninstall.Contains(programName))
                            {
                                string programStringQuiet = Convert.ToString(key.OpenSubKey(program).GetValue("QuietUninstallString"));
                                string programString = Convert.ToString(key.OpenSubKey(program).GetValue("UninstallString"));
                                if (programStringQuiet != null && programStringQuiet != "")
                                    new MainForm.LogTask($"Uninstalling {programName}", UninstallProgram, programStringQuiet);
                                else if ((programString != null && programString != ""))
                                    new MainForm.LogTask($"Uninstalling {programName}", UninstallProgram, programString);
                                else form.Log($"Cannot uninstall {programName}.", false);
                            }
                        }
                        catch (Exception ex)
                        {
                            form.Log(ex.Message, true);
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
    }
}
