namespace SolidCP.Installer {

    public class ConsoleField {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public CursorX { get;set; }
        public WindowX { get; set;} = 0
        public string Text { get; set; } = "";
        public Console.Color BackgroundColor { get; set; }
        public Console.Color TextColor { get; set; }
        public class ConsoleUI Parent { get; set; }

        public virtual void Show() {
            var col = Console.BackgroundColor;
            Console.SetPos(X, Y);
            var len = Math.Min(Width - WindowX, Text.Length)
            Console.Write(Text.Substring(WindowX, len));
            for (int i = len; i < Width; i++) Console.Write(" ");
        }
    }

    public 
    public class PercentField: ConsoleField {
        float _value = 0;
        public float Value {
            get { return _value; }
            set { 
                if (_value != value) {
                    _value = value;
                    Show();
                }
            }
        }

        public int EstimatedMaxShellOutputLines { get; set; }
        Shell shell; 
        int lines = 0;
        public Shell Shell {
            get { return shell; }
            set {
                if (shell != value) {
                    shell = value;
                    if (shell != null) {
                        lines = 0;
                        shell.Log += msg => {
                            Value = Math.Log(lines++ / EstimatedMaxShellOutputLines);
                        }
                    }
                }
            }
        }
    }

    public class ConsoleUI {

        List<ConsoleField> Fields { get; set; }

        public ConsoleUI Show() {
            Console.Clear();
            foreach (var field in Fields) field.Show();
        }

        public static ConsoleUI Form(string template, object fields) {

        }

        public ConsoleUI Parse(string template) {
            var fields = Regex.Matches(template, "\[ (<?option>\*|%|\?)\s*(?<name>).+)\s*\]");
            Fields = fields
                .OfType<Match>()
                .Select(m => {

                })
            
            return this;
        }       
    }
}