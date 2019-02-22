using System;
using System.Diagnostics;
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
            MainMenu = new Menu().SetTitle("Main").SetBackground(ConsoleColor.DarkRed);
            MainMenu.AddLabel(new Menu.Label("Bazinga"));

            ConfigMenu = new Menu().SetTitle("Config").SetBackground(ConsoleColor.DarkGreen);
            ConfigMenu.AddLabel(new Menu.Label("Kachow"));

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

        static void MainM()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Bazinga");
            Console.WriteLine("Press C for Config");
            Console.WriteLine("Press Q to exit");
        }

        static void ConfigM()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Kachow");
            Console.WriteLine("Press M for Main");
            Console.WriteLine("Press Q to exit");
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
