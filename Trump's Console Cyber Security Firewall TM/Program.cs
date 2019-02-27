using System;
using System.Diagnostics;
using static Trump_s_Console_Cyber_Security_Firewall_TM.Label;
using static Trump_s_Console_Cyber_Security_Firewall_TM.Screen;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Program
    {
        static private Screen MyScreen;
        static private Menu MainMenu;
        static private Menu ConfigMenu;

        static void Main(string[] args)
        {
            MainMenu = new Menu("Main", ConsoleColor.DarkRed);
            MainMenu.AddLabel(new Label("Bazinga", AnchorSide.Left | AnchorSide.Top, 10, 4));

            ConfigMenu = new Menu("Config",ConsoleColor.DarkGreen);
            ConfigMenu.AddLabel(new Label("Kachow", AnchorSide.Right | AnchorSide.Bottom, 10, 4));

            MyScreen = new Screen(MainMenu);

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
                case (ConsoleKey.M):
                    MyScreen.SetMenu(MainMenu);
                    break;
                case (ConsoleKey.C):
                    MyScreen.SetMenu(ConfigMenu);
                    break;
                default:
                    MyScreen.Reload();
                    break;
            }
        }

        static void Secure()
        {
            var output = "echo \"this is a test\" > testFileha.txt".Bash();
        }

        static void Quit()
        {
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
