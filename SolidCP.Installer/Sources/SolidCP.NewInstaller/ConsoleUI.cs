namespace SolidCP.Installer {

    public class ConsoleField {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public string Text { get; set; } = "";
        public Console.Color BackgroundColor { get; set; }
        public class ConsoleUI Parent { get; set; }

        public virtual void Show() {
            var col = Console.BackgroundColor;
            Console.SetPos(X, Y);
            Console.Write(Text);
            for (int i = Text.Length; i < Width) {
                Console.Write(" ");
            }
        }
    }

    public class ProgressField: ConsoleField {
        double Progress { get; set; }
        public override void Show() {

        }
    }
    public class ConsoleUI {

        List<ConsoleField> Fields { get; set; }

        public static ConsoleUI Form(string template, object fields) {

        }

        void Parse(string template) {
            
        }       
    }
}