using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Screen
    { 
        Menu CurrentMenu;
        public event EventHandler WindowResizedEvent;
        public event EventHandler<KeyReceivedArgs> KeyReceivedEvent;
        int LastWidth = 0;
        int LastHeight = 0;

        Thread t1;
        Thread t2;

        public Screen(Menu defaultMenu)
        {
            SetMenu(defaultMenu);

            WindowResizedEvent += OnWindowResized;

            t1 = new Thread(() => CheckWindow());
            t1.Start();

            t2 = new Thread(() => CheckInput());
            t2.Start();
        }

        public void SetMenu(Menu newMenu)
        {
            CurrentMenu = newMenu;
            Reload();
        }

        public void Reload()
        {
            CurrentMenu.WriteMenu();
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
                    WindowResizedEvent?.Invoke(null, EventArgs.Empty);
                }
                Thread.Sleep(10);
            }
        }

        void CheckInput()
        {
            while (true)
            {
                KeyReceivedEvent?.Invoke(null, new KeyReceivedArgs(Console.ReadKey()));
            }
        }

        void OnWindowResized(object sender, EventArgs e)
        {
            Reload();
        }
    }

    internal class Menu
    {
        List<MenuItem> Items = new List<MenuItem>();

        string Title = "New Menu";
        ConsoleColor Background = ConsoleColor.DarkBlue;

        public Menu()
        {
        }

        public Menu(string title)
        {
            Title = title;
        }

        public Menu(string title, ConsoleColor background)
        {
            Title = title;
            Background = background;
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

        public Menu Add(MenuItem item)
        {
            Items.Add(item);
            return this;
        }

        public void WriteMenu()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = Background;
            Console.Clear();
            Console.Title = $"Trump's Console Cyber Security Firewall TM - {Title}";
            Console.WriteLine(CreateHeader(Title));
            Console.WriteLine();

            foreach (MenuItem item in Items)
                item.WriteItem();

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.CursorVisible = true;
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
    }

    internal abstract class MenuItem
    {
        protected AnchorSide Anchor = AnchorSide.Left | AnchorSide.Top;
        protected int DistVert = 0;
        protected int DistHori = 0;

        public MenuItem()
        {
        }

        public MenuItem SetPosition(int distHori, int distVert)
        {
            if (distHori < 0 || distVert < 0) throw new InvalidOperationException();
            DistHori = (Console.WindowWidth < distHori) ?
                Console.WindowWidth : distHori;
            DistVert = (Console.WindowHeight < distVert) ?
                Console.WindowHeight : distVert;
            return this;
        }

        public enum AnchorSide
        {
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8
        }

        public MenuItem SetAnchor(AnchorSide anchor)
        {
            Anchor = anchor;
            return this;
        }

        public void MoveCursorToWrite()
        {
            Console.SetCursorPosition(

                (Anchor.HasFlag(AnchorSide.Left)) ? DistHori :
                (Anchor.HasFlag(AnchorSide.Right)) ? Console.WindowWidth - DistHori : 0,

                (Anchor.HasFlag(AnchorSide.Top)) ? DistVert :
                (Anchor.HasFlag(AnchorSide.Bottom)) ? Console.WindowHeight - DistVert : 0

                );
        }

        public abstract void WriteItem();
    }

    internal class Button : MenuItem
    {
        public event EventHandler ButtonClickedEvent;

        string Text = "New Label";

        public Button()
        {
        }

        public Button(string text)
        {
            Text = text;
        }

        public Button(string text, AnchorSide anchor)
        {
            Text = text;
            Anchor = anchor;
        }

        public Button(string text, AnchorSide anchor, int distHori, int distVert)
        {
            Text = text;
            Anchor = anchor;
            DistVert = distVert;
            DistHori = distHori;
        }

        public Button EditText(string text)
        {
            Text = text;
            return this;
        }

        public override void WriteItem()
        {
            MoveCursorToWrite();
            Console.Write(Text);
        }
    }

    internal class Label : MenuItem
    {
        string Text = "New Label";

        public Label()
        {
        }

        public Label(string text)
        {
            Text = text;
        }

        public Label(string text, AnchorSide anchor)
        {
            Text = text;
            Anchor = anchor;
        }

        public Label(string text, AnchorSide anchor, int distHori, int distVert)
        {
            Text = text;
            Anchor = anchor;
            DistVert = distVert;
            DistHori = distHori;

        }

        public Label EditText(string text)
        {
            Text = text;
            return this;
        }

        public override void WriteItem()
        {
            MoveCursorToWrite();
            Console.Write(Text);
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
