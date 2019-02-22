using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Screen
    { 
        Menu CurrentMenu;
        public event EventHandler WindowResizedEvent;
        public event EventHandler<KeyReceivedArgs> KeyReceivedEvent;
        int LastWidth = 0;
        int LastHeight = 0;

        public Screen(Menu defaultMenu)
        {
            SetMenu(defaultMenu);

            var t1 = new Thread(() => CheckWindow());
            t1.Start();

            var t2 = new Thread(() => CheckInput());
            t2.Start();
        }

        public void SetMenu(Menu newMenu)
        {
            CurrentMenu = newMenu;
            Reload();
        }

        public void Reload()
        {
            CurrentMenu.RunMenu().Invoke();
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

    internal class Menu
    {
        public delegate void MenuProc();
        MenuProc ThisMenu;

        string Title = "New Menu";
        ConsoleColor Background = ConsoleColor.DarkBlue;

        public Menu()
        {
            ThisMenu = () => {
                Console.Clear();
                Console.BackgroundColor = Background;
                Console.Title = $"Trump's Console Cyber Security Firewall TM - {Title}";
                Console.WriteLine(CreateHeader(Title));
                Console.WriteLine();
            };
        }

        public Menu SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public Menu SetBackground(ConsoleColor background)
        {
            Background = background;
            return this;
        }

        public Menu AddLabel(Label label)
        {
            ThisMenu += label.WriteLabel();
            return this;
        }

        internal MenuProc RunMenu()
        {
            return ThisMenu;
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

        internal class Button
        {
            public event EventHandler ButtonClickedEvent;

            string FirstLine = "";
            string SecondLine = "";
            string ThirdLine = "";
            public Button(string text)
            {
                FirstLine = new string('-', text.Length);
                SecondLine = $"|{text}|";
                ThirdLine = new string('-', text.Length);
            }
        }

        internal class Label
        {
            string Text = "New Label";
            int PosRow = 0;
            int PosCol = 0;

            public Label(string text)
            {
                Text = text;
            }

            public Label SetPosition(int x, int y)
            {
                PosCol = (Console.BufferWidth < x) ?
                    Console.BufferWidth : x;
                PosRow = (Console.BufferHeight < y) ?
                    Console.BufferHeight : y;
                return this;
            }

            public MenuProc WriteLabel()
            {
                return () =>
                {
                    Console.CursorLeft = PosCol;
                    Console.CursorTop = PosRow;
                    Console.Write(Text);
                };
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
