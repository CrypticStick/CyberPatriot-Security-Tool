using Terminal.Gui;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Program
    {
        static Toplevel top = Application.Top;

        static Window winMain = new Window(
                new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1),
                "Trump's (Console) Cyber Security Firewall TM"
                );

        static void Main(string[] args)
        {
            Application.Init();
            // Creates the top-level window to show
            top.Add(winMain);
            winMain.Add(new Window[] {

                new Window(
                    new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), 
                    "Main"
                    )
                    {
                        new Label(3, 2, "MAIN: "),
                        new TextField(14, 2, 40, ""),
                        new Label(3, 4, "Password: "),
                        new TextField(14, 4, 40, "") { Secret = true },
                        new CheckBox(3, 6, "Remember me"),
                        new RadioGroup(3, 8, new[] { "MAIN", "_Company" }),
                        new Button(3, 14, "Ok"),
                        new Button(10, 14, "Cancel"),
                        new Label(3, 18, "Press ESC and 9 to activate the menubar")
                    },

                new Window(
                    new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), 
                    "Config"
                    )
                    {
                        new Label(3, 2, "CONFIG: "),
                        new Button(3, 14, "Ok"),
                        new Button(10, 14, "Cancel"),
                        new Label(3, 18, "Press ESC and 9 to activate the menubar")
                    },

                    new Window(
                    new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), 
                    "Tools"
                    )
                    {
                        new Label(3, 2, "TOOLS: "),
                        new TextField(14, 4, 40, "") { Secret = true },
                        new CheckBox(3, 6, "Remember me"),
                        new Button(3, 14, "Ok"),
                        new Label(3, 18, "Press ESC and 9 to activate the menubar")
                    }
                });

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("Tabs", new MenuItem[] {
                new MenuItem ("Main", "", MainTab),
                new MenuItem ("Config", "", ConfigTab),
                new MenuItem ("Tools", "", ToolsTab)
                })
            });
            top.Add(menu);

            winMain.SetFocus(winMain.Subviews[0]);

            try
            {
                Application.Run();
            } catch
            {
                Quit();
            }
        }

        private static void MainTab()
        {
            winMain.SetFocus(winMain.Subviews[0]);
        }

        private static void ConfigTab()
        {
            winMain.SetFocus(winMain.Subviews[1]);
        }

        private static void ToolsTab()
        {
            winMain.SetFocus(winMain.Subviews[2]);
        }

        private static void Quit()
        {
            Application.RequestStop();
        }
    }
}
