using System.Text.RegularExpressions;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller.UI
{

    public class ConsoleSettings
    {
        public int CursorX;
        public int CursorY;
        public bool CursorVisible;
        public ConsoleColor ForegroundColor;
        public ConsoleColor BackgroundColor;

        public static ConsoleSettings Save()
        {
            ConsoleSettings settings = new ConsoleSettings();
            settings.CursorVisible = Console.CursorVisible;
            settings.CursorX = Console.CursorLeft;
            settings.CursorY = Console.CursorTop;
            settings.BackgroundColor = Console.BackgroundColor;
            settings.ForegroundColor = Console.ForegroundColor;
            return settings;
        }

        public void Restore()
        {
            Set(CursorX, CursorY, CursorVisible, BackgroundColor, ForegroundColor);
        }

        public static ConsoleSettings Set(int X, int Y, bool visible, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            var con = Save();
            Console.CursorLeft = X;
            Console.CursorTop = Y;
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.CursorVisible = visible;
            return con;
        }
    }

    public class ConsoleField
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CursorX { get; set; }
        public int CursorY { get; set; }
        public int WindowX { get; set; } = 0;
        public int WindowY { get; set; } = 0;
        public string Text { get; set; } = "";
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public virtual ConsoleUI Parent { get; set; }

        public virtual void Show()
        {
            // add space fillers to lines so Console.Write will cover the whole window
            var displayText = Regex.Replace(Text, "^(.*)$", match =>
            {
                var text = match.Groups[1].Value;
                var len = text.Length;
                var addSpaceLen = Width - len - WindowX;
                char[] spaces = Enumerable.Repeat(' ', addSpaceLen).ToArray();
                var addSpacesString = new string(spaces);
                return $"{text}{spaces}";
            }, RegexOptions.Multiline);

            // save settings
            var con = ConsoleSettings.Set(X, Y, false, BackgroundColor, ForegroundColor);

            Console.Write(displayText);

            // restore settings
            con.Restore();
        }
    }

    public class PercentField : ConsoleField
    {

        float _value = 0;
        public float Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    Show();
                }
            }
        }

        public int EstimatedMaxShellOutputLines { get; set; }

        Shell shell;
        int lines = 0;
        public Shell Shell
        {
            get { return shell; }
            set
            {
                if (shell != value)
                {
                    shell = value;
                    if (shell != null)
                    {
                        lines = 0;
                        shell.Log += msg =>
                        {
                            Value = 1.0f - (float)Math.Exp(-(lines++ / EstimatedMaxShellOutputLines));
                        };
                    }
                }
            }
        }

        public class TextField : ConsoleField
        {

        }

        public class Button : ConsoleField
        {
            public bool Default { get; set; } = false;
        }

        public override ConsoleUI Parent
        {
            get => base.Parent;
            set
            {
                base.Parent = value;
                Shell = Parent.Shell;
            }
        }

        public class ConsoleForm
        {

            public List<ConsoleField> Fields { get; set; }

            public Installer Installer { get; set; }

            public Shell Shell => Installer.Shell;

            public string Template { get; set; }
            public ConsoleUI Show()
            {
                Console.Clear();
                foreach (var field in Fields) field.Show();
            }

            public static ConsoleForm Form(string template, object fields)
            {
                return new ConsoleForm()
                    .Parse(template);
            }

            public ConsoleForm Parse(string template)
            {
                var fields = Regex.Matches(template, @"\[(<?option>\*|%|\?|)\s*(?<name>[A-Za-z_][A-Za-z_0-9]*)).*?\]");
                Fields = fields
                    .OfType<Match>()
                    .Select(m =>
                    {
                        switch (m.Groups["option"].Value)
                        {
                            case "*":
                                return new Button() { Default = true };
                            case "":
                                return new Button();
                            case "?":
                                return new TextField();
                            case "%":
                                return new PercentField();
                        }
                    })
                    .ToList();
                return this;
            }
        }
    }
}