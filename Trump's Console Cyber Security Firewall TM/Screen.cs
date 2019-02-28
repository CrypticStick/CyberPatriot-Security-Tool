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
        List<Label> Labels = new List<Label>();

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

        public Menu AddLabel(Label label)
        {
            Labels.Add(label);
            return this;
        }

        public void WriteMenu()
        {
            Console.Clear();
            Console.BackgroundColor = Background;
            Console.Title = $"Trump's Console Cyber Security Firewall TM - {Title}";
            Console.WriteLine(CreateHeader(Title));
            Console.WriteLine();

            foreach (Label label in Labels)
                label.WriteLabel();

            Console.CursorLeft = 0;
            Console.CursorTop = Console.WindowHeight;
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
        AnchorSide Anchor = AnchorSide.Left | AnchorSide.Top;
        string Text = "New Label";
        int DistVert = 0;
        int DistHori = 0;

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

        public Label SetPosition(int distHori, int distVert)
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

        public Label SetAnchor(AnchorSide anchor)
        {
            Anchor = anchor;
            return this;
        }

        public Label EditLabel(string text)
        {
            Text = text;
            return this;
        }

        public void WriteLabel()
        {
            Console.CursorLeft =
                (Anchor.HasFlag(AnchorSide.Left)) ? DistHori :
                (Anchor.HasFlag(AnchorSide.Right)) ? Console.WindowWidth - DistHori : 0;
            Console.CursorTop = 
                (Anchor.HasFlag(AnchorSide.Top)) ? DistVert :
                (Anchor.HasFlag(AnchorSide.Bottom)) ? Console.WindowHeight - DistVert : 0;
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
