using System;
using System.Text;
using System.Threading;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Menus
    {
        delegate void Menu();
        Menu CurrentMenu;
        public event EventHandler WindowResizedEvent;
        public event EventHandler<KeyReceivedArgs> KeyReceivedEvent;
        int LastWidth = 0;
        int LastHeight = 0;
        int TestVal = 0;

        public Menus()
        {
            CurrentMenu = Main;

            var t1 = new Thread(() => CheckWindow());
            t1.Start();

            var t2 = new Thread(() => CheckInput());
            t2.Start();
        }

        public enum MenuSelect
        {
            Main, Config
        }

        public void SetMenu(MenuSelect menu)
        {
            switch (menu)
            {
                case MenuSelect.Main:
                    Main();
                    break;
                case MenuSelect.Config:
                    Config();
                    break;
            }
        }

        public void Reload()
        {
            CurrentMenu.Invoke();
        }

        void Main()
        {
            CurrentMenu = Main;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Title = "Trump's Console Cyber Security Firewall TM - Main Menu";
            Console.WriteLine(CreateHeader("Main Menu"));
            Console.WriteLine();
            Console.WriteLine($"Bazinga x{++TestVal}");
            Console.WriteLine("Press C for Config");
            Console.WriteLine("Press Q to exit");
        }

        void Config()
        {
            CurrentMenu = Config;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Title = "Trump's Console Cyber Security Firewall TM - Config";
            Console.WriteLine(CreateHeader("Config"));
            Console.WriteLine();
            Console.WriteLine($"Kachow x{++TestVal}");
            Console.WriteLine("Press M for Main");
            Console.WriteLine("Press Q to exit");
        }

        string CreateHeader(string title)
        {
            int totalChar = Console.WindowWidth;
            StringBuilder st = new StringBuilder();
            st.Append('>', (totalChar - title.Length) / 2);
            st.Append(title);
            st.Append('<', (totalChar - title.Length) / 2);
            return st.ToString();
        }

        void CheckWindow()
        {
            while (true)
            {
                if (Console.WindowWidth != LastWidth ||
                    Console.WindowHeight != LastHeight)
                {
                    LastWidth = Console.WindowWidth;
                    LastHeight = Console.WindowHeight;
                    Reload();
                    WindowResizedEvent?.Invoke(null, EventArgs.Empty);
                }
                Thread.Sleep(100);
            }
        }

        void CheckInput()
        {
            while (true)
            {
                KeyReceivedEvent?.Invoke(null, new KeyReceivedArgs(Console.ReadKey()));
            }
        }
    }

    internal class KeyReceivedArgs : EventArgs
    {
        public KeyReceivedArgs(ConsoleKeyInfo key)
        {
            KeyInfo = key;
        }
        public ConsoleKeyInfo KeyInfo { get; set; }
    }
}
