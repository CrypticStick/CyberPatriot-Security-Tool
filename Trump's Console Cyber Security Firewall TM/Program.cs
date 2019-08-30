using System;
using System.Drawing;
using System.Diagnostics;
using static Trump_s_Console_Cyber_Security_Firewall_TM.Label;
using static Trump_s_Console_Cyber_Security_Firewall_TM.MenuItem;
using static Trump_s_Console_Cyber_Security_Firewall_TM.Screen;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Program
    {
        static private Screen MyScreen;
        static private Menu MainMenu, ConfigMenu;
        private static readonly ConsoleColor StartColor = Console.BackgroundColor;

        static void Main(string[] args)
        {
            MainMenu = new Menu("Main", Color.DarkRed);
            MainMenu.Add(new Label("Bazinga", AnchorSide.Left | AnchorSide.Top, 10, 4, Color.Black));
            MainMenu.Add(new Button("Button 1", AnchorSide.Right | AnchorSide.Top, 20, 10, Color.BlueViolet));
            MainMenu.Add(new Button("Button 2", AnchorSide.Right | AnchorSide.Bottom, 20, 5));

            ConfigMenu = new Menu("Config",Color.DarkGreen);
            ConfigMenu.Add(new Label("Kachow", AnchorSide.Right | AnchorSide.Bottom, 10, 4));

            MyScreen = new Screen(MainMenu);
            MyScreen.AddMenu(ConfigMenu);

            MyScreen.WindowResizedEvent += OnWindowResized;
            MyScreen.KeyReceivedEvent += OnKeyReceived;

            //Secure();
        }

        static void OnWindowResized(object sender, EventArgs e)
        {
        }

        static void OnKeyReceived(object sender, KeyReceivedArgs e)
        {
            switch (e.KeyInfo.Key)
            {
                case (ConsoleKey.Q):
                    Quit();
                    break;
            }
        }

        static string ReplaceStringInFile(string filepath, string toReplace, string replacement)
        {
            return $"sed -i '.bak' -e 's/{toReplace}/{replacement}/g' {filepath}".Bash();
        }

        static void Secure()
        {
            string file = "/etc/ssh/sshd_config";
            ReplaceStringInFile(file, "Port 22", "Port 2020");
            ReplaceStringInFile(file, "Protocol 1", "Protocol 2");
            ReplaceStringInFile(file, "PermitRootLogin yes", "PermitRootLogin no");
            ReplaceStringInFile(file, "ChallengeResponseAuthentication yes", "ChallengeResponseAuthentication no");
            ReplaceStringInFile(file, "PasswordAuthentication yes", "PasswordAuthentication no");
            ReplaceStringInFile(file, "UsePAM yes", "UsePAM no");
            ReplaceStringInFile(file, "PubkeyAuthentication yes", "PubkeyAuthentication no");
            ReplaceStringInFile(file, "PermitEmptyPasswords yes", "PermitEmptyPasswords no");

            "echo \"install usb-storage /bin/true\" > /etc/modprobe.d/no-usb".Bash();
            "sudo ufw deny 22".Bash();
            "sudo ufw deny 2020/tcp".Bash();
            "sudo apt-get update && sudo apt-get upgrade".Bash();

            foreach (Label mi in MyScreen.GetMenu().GetMenuItems())
            {
                mi.EditText("WE DID IT");
            }
        }

        static void Quit()
        {
            Console.BackgroundColor = StartColor;
            Console.Clear();
            Environment.Exit(0);
        }
    }

    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
