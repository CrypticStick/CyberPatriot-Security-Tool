using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using Console = Colorful.Console;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Screen
    {
        List<Menu> Menus = new List<Menu>();
        Menu CurrentMenu;
        public event EventHandler WindowResizedEvent;
        public event EventHandler<KeyReceivedArgs> KeyReceivedEvent;
        int LastWidth = 0, LastHeight = 0;
        Thread t1, t2, t3;

        public Screen(Menu defaultMenu)
        {
            SetMenu(defaultMenu);

            WindowResizedEvent += OnWindowResized;
            KeyReceivedEvent += OnKeyReceived;

            t1 = new Thread(() => CheckWindow());
            t1.Start();

            t2 = new Thread(() => CheckInput());
            t2.Start();

            t3 = new Thread(() => CheckAnimation());
            t3.Start();
        }

        public void SetMenu(Menu newMenu)
        {
            if (!Menus.Contains(newMenu))
                Menus.Add(newMenu);
            CurrentMenu = newMenu;
            Reload();
        }

        public void AddMenu(Menu newMenu)
        {
            if (!Menus.Contains(newMenu))
            {
                Menus.Add(newMenu);
                Reload();
            }
        }

        public Menu GetMenu()
        {
            return CurrentMenu;
        }

        public List<Menu> GetMenus()
        {
            return Menus;
        }

        public void Reload()
        {
            Console.BackgroundColor = CurrentMenu.GetBackground();
            Console.Clear();
            DrawTabs();
            CurrentMenu.WriteMenu();
        }

        void DrawTabs()
        {
            int totalChar = Console.WindowWidth;
            StringBuilder st = new StringBuilder();
            int totalchar = 0;
            foreach (Menu menu in Menus)
            {
                st = new StringBuilder();
                Console.BackgroundColor = menu.GetBackground();
                st.Append(' ', (totalChar/Menus.Count - menu.GetTitle().Length) / 2);
                st.Append("{0}"+menu.GetTitle().Substring(1));
                st.Append(' ', (totalChar/Menus.Count - menu.GetTitle().Length) / 2);
                Console.WriteFormatted(st.ToString(), Color.Black, Color.White, menu.GetTitle().ToCharArray()[0]);
                totalchar += st.ToString().Length-2;
            }
            st = new StringBuilder();
            st.Append(' ',Console.WindowWidth - totalchar);
            Console.WriteLine(st.ToString());
            Console.BackgroundColor = CurrentMenu.GetBackground();
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

        void CheckAnimation()
        {
            MenuItem item;
            bool state = false;
            while (true)
            {
                Thread.Sleep(500);
                Console.CursorVisible = false;
                item = CurrentMenu.GetHighlightedItem();
                if (item != null)
                {
                    if (state) {
                        Console.BackgroundColor = Color.White;
                        item.WriteItem();
                        Console.BackgroundColor = CurrentMenu.GetBackground();
                    } else {
                        Console.BackgroundColor = CurrentMenu.GetBackground();
                        item.WriteItem();
                    }
                    state = !state;
                }
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.CursorVisible = true;
            }
        }

        void OnWindowResized(object sender, EventArgs e)
        {
            Reload();
        }

        void OnKeyReceived(object sender, KeyReceivedArgs e)
        {
            MenuItem closestButton = null;
            switch (e.KeyInfo.Key)
            {
                case (ConsoleKey.UpArrow):
                    if (CurrentMenu.GetHighlightedItem() == null) return;
                    foreach (MenuItem item in CurrentMenu.GetMenuItems())
                        if (item.GetType().Equals(typeof(Button)))
                        {
                            if (CurrentMenu.GetHighlightedItem().Equals(item)) break;
                            if (item.GetVertPos() < CurrentMenu.GetHighlightedItem().GetVertPos())
                            {
                                if (closestButton == null)
                                    closestButton = item;
                                else if (CurrentMenu.GetHighlightedItem().GetVertPos() - closestButton.GetVertPos() >
                                    CurrentMenu.GetHighlightedItem().GetVertPos() - item.GetVertPos())
                                    closestButton = item;
                            }
                        }
                    break;
                case (ConsoleKey.LeftArrow):
                    if (CurrentMenu.GetHighlightedItem() == null) return;
                    foreach (MenuItem item in CurrentMenu.GetMenuItems())
                        if (item.GetType().Equals(typeof(Button)))
                        {
                            if (CurrentMenu.GetHighlightedItem().Equals(item)) break;
                            if (item.GetHoriPos() < CurrentMenu.GetHighlightedItem().GetHoriPos())
                            {
                                if (closestButton == null)
                                    closestButton = item;
                                else if (CurrentMenu.GetHighlightedItem().GetHoriPos() - closestButton.GetHoriPos() >
                                    CurrentMenu.GetHighlightedItem().GetHoriPos() - item.GetHoriPos())
                                    closestButton = item;
                            }
                        }
                    break;
                case (ConsoleKey.DownArrow):
                    if (CurrentMenu.GetHighlightedItem() == null) return;
                    foreach (MenuItem item in CurrentMenu.GetMenuItems())
                        if (item.GetType().Equals(typeof(Button)))
                        {
                            if (CurrentMenu.GetHighlightedItem().Equals(item)) break;
                            if (item.GetVertPos() > CurrentMenu.GetHighlightedItem().GetVertPos())
                            {
                                if (closestButton == null)
                                    closestButton = item;
                                else if (CurrentMenu.GetHighlightedItem().GetVertPos() - closestButton.GetVertPos() <
                                    CurrentMenu.GetHighlightedItem().GetVertPos() - item.GetVertPos())
                                    closestButton = item;
                            }
                        }
                    break;
                case (ConsoleKey.RightArrow):
                    if (CurrentMenu.GetHighlightedItem() == null) return;
                    foreach (MenuItem item in CurrentMenu.GetMenuItems())
                        if (item.GetType().Equals(typeof(Button)))
                        {
                            if (CurrentMenu.GetHighlightedItem().Equals(item)) break;
                            if (item.GetHoriPos() > CurrentMenu.GetHighlightedItem().GetHoriPos())
                            {
                                if (closestButton == null)
                                    closestButton = item;
                                else if (CurrentMenu.GetHighlightedItem().GetHoriPos() - closestButton.GetHoriPos() <
                                    CurrentMenu.GetHighlightedItem().GetHoriPos() - item.GetHoriPos())
                                    closestButton = item;
                            }
                        }
                    break;
                default:
                    foreach (Menu menu in Menus)
                    {
                        if (e.KeyInfo.Key.ToString().ToLower().ToCharArray()[0].Equals(
                            menu.GetTitle().ToLower().ToCharArray()[0]))
                        {
                            SetMenu(menu);
                            break;
                        }
                    }
                    Console.SetCursorPosition(0, Console.WindowHeight - 1);
                    break;
            }
            if (closestButton != null)
                CurrentMenu.SetHighlightedItem(closestButton);
        }
    }

    internal class Menu
    {
        List<MenuItem> Items = new List<MenuItem>();
        MenuItem HighlightedItem = null;

        string Title = "New Menu";
        Color Background = Color.DarkBlue;

        public Menu()
        {
        }

        public Menu(string title)
        {
            Title = title;
        }

        public Menu(string title, Color background)
        {
            Title = title;
            Background = background;
        }

        public Menu SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public string GetTitle()
        {
            return Title;
        }

        public Menu SetBackground(Color background)
        {
            Background = background;
            return this;
        }

        public Color GetBackground()
        {
            return Background;
        }

        public Menu Add(MenuItem item)
        {
            Items.Add(item);
            return this;
        }

        public List<MenuItem> GetMenuItems()
        {
            return Items;
        }

        public MenuItem GetHighlightedItem()
        {
            return HighlightedItem;
        }

        public Menu SetHighlightedItem(MenuItem item)
        {
            HighlightedItem = item;
            return this;
        }

        public void WriteMenu()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = Background;
            Console.Title = $"Trump's Console Cyber Security Firewall TM - {Title}";
            Console.WriteLine(CreateHeader(Title));
            Console.WriteLine();

            if (HighlightedItem == null)
                foreach (MenuItem item in Items)
                    if (item.GetType().Equals(typeof(Button))) {
                        HighlightedItem = item;
                        break;
                    }

            foreach (MenuItem item in Items)
                item.WriteItem();

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.CursorVisible = true;
        }

        string CreateHeader(string title)
        {
            int totalChar = Console.WindowWidth;
            StringBuilder st = new StringBuilder();
            st.Append('>', (totalChar - title.Length) / 2 - 1);
            st.Append(" " + title + " ");
            st.Append('<', (totalChar - title.Length) / 2 - 1);
            return st.ToString();
        }
    }

    internal abstract class MenuItem
    {
        protected AnchorSide Anchor = AnchorSide.Left | AnchorSide.Top;
        protected Color color = Color.White;
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

        public int GetHoriPos()
        {
            return DistHori;
        }

        public int GetVertPos()
        {
            return DistVert;
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

        public MenuItem SetColor(Color color)
        {
            this.color = color;
            return this;
        }

        public Color GetColor()
        {
            return color;
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

        string Text = "New Button";

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

        public Button(string text, AnchorSide anchor, int distHori, int distVert, Color color)
        {
            Text = text;
            Anchor = anchor;
            DistVert = distVert;
            DistHori = distHori;
            this.color = color;
        }

        public Button EditText(string text)
        {
            Text = text;
            return this;
        }

        public override void WriteItem()
        {
            MoveCursorToWrite();
            Console.Write(Text,color);
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

        public Label(string text, AnchorSide anchor, int distHori, int distVert, Color color)
        {
            Text = text;
            Anchor = anchor;
            DistVert = distVert;
            DistHori = distHori;
            this.color = color;
        }

        public Label EditText(string text)
        {
            Text = text;
            return this;
        }

        public override void WriteItem()
        {
            MoveCursorToWrite();
            Console.Write(Text, color);
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
