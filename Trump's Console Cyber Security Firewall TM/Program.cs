using System;
using static Trump_s_Console_Cyber_Security_Firewall_TM.Menus;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Program
    {
        static private Menus MyMenu;
        static void Main(string[] args)
        {
            MyMenu = new Menus();
            MyMenu.WindowResizedEvent += OnWindowResized;
            MyMenu.KeyReceivedEvent += OnKeyReceived;
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
                    MyMenu.SetMenu(MenuSelect.Main);
                    break;
                case (ConsoleKey.C):
                    MyMenu.SetMenu(MenuSelect.Config);
                    break;
                default:
                    MyMenu.Reload();
                    break;
            }
        }

        static void Quit()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
