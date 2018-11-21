using System;
using System.Threading.Tasks;
using Terminal.Gui;

namespace Trump_s_Console_Cyber_Security_Firewall_TM
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;
            // Creates the top-level window to show
            var win = new Window(
                new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), 
                "Trump's (Console) Cyber Security Firewall TM"
                );

            top.Add(win);

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Just an example", null),
                new MenuItem ("_Quit", "", () => Quit())
            }),
            new MenuBarItem ("_Edit", new MenuItem [] {
                new MenuItem ("_Copy", "", null),
                new MenuItem ("C_ut", "", null),
                new MenuItem ("_Paste", "", null)
            })
            });
            top.Add(menu);

            // Add some controls
            win.Add(
                    new Label(3, 2, "Login: "),
                    new TextField(14, 2, 40, ""),
                    new Label(3, 4, "Password: "),
                    new TextField(14, 4, 40, "") { Secret = true },
                    new CheckBox(3, 6, "Remember me"),
                    new RadioGroup(3, 8, new[] { "_Personal", "_Company" }),
                    new Button(3, 14, "Ok"),
                    new Button(10, 14, "Cancel"),
                    new Label(3, 18, "Press ESC and 9 to activate the menubar"));

            Application.Run();
        }

        private static void Quit()
        {
            Application.RequestStop();
        }
    }
}
