using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils.LogParser;
using SolidCP.Providers.Virtualization;

namespace SolidCP.UniversalInstaller
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
		public string Name;
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int CursorX { get; set; }
		public int CursorY { get; set; }
		public int WindowX { get; set; } = 0;
		public int WindowY { get; set; } = 0;
		public virtual bool Centered => false;
		public bool HasFocus;
		public virtual bool CanFocus => false;
		public virtual bool Editable => false;
		public virtual string Text { get; set; } = "";
		public Action Click;
		public ConsoleColor BackgroundColor { get; set; }
		public ConsoleColor ForegroundColor { get; set; }
		public virtual ConsoleForm Parent { get; set; }

		public virtual bool Edit(ConsoleKeyInfo key) => false;

		public virtual void Show()
		{
			// add space fillers to lines so Console.Write will cover the whole window
			var displayText = Regex.Replace(Text, "^(.*)$", match =>
			{
				var text = match.Groups[1].Value;
				int len = text.Length;
				var addSpaceLen = Width - len - WindowX;
				int addLeft, addRight;
				if (!Centered)
				{
					addLeft = 0;
					addRight = Width - len - WindowX;
				}
				else
				{
					var w = Width - len;
					addLeft = w / 2;
					addRight = w - addLeft;
				}
				char[] spacesLeft = Enumerable.Repeat(' ', addLeft).ToArray();
				char[] spacesRight = Enumerable.Repeat(' ', addRight).ToArray();
				return $"{new string(spacesLeft)}{text}{new string(spacesRight)}";
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
		public override bool CanFocus => false;

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

		public int EstimatedMaxProgress { get; set; }

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
							Value = 1.0f - (float)Math.Exp(-(lines++ / (float)EstimatedMaxProgress));
						};
					}
				}
			}
		}

		public override void Show()
		{
			var num = $"{Value * 100 + 0.5} %";
			var pos = Math.Max(0, (Width - num.Length) / 2);
			var spaces = new string(Enumerable.Repeat(' ', pos).ToArray());
			Text = $"{spaces}{Value * 100} %";
			pos = Math.Min(Width, Math.Max(0, (int)(Value * (Width - 1) + 0.5)));
			var width = Width;
			var background = Console.BackgroundColor;
			var x = X;
			Width = pos;
			Console.BackgroundColor = ConsoleColor.Green;
			base.Show();
			Console.BackgroundColor = background;
			Width = width - pos;
			WindowX = pos;
			X = pos;
			base.Show();
			Width = width;
			WindowX = 0;
			X = x;
		}
	}
	public class TextField : ConsoleField
	{
		public override bool Editable => true;
		public override bool Edit(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
				case ConsoleKey.DownArrow:
					Parent.EditNavigate(key); break;
				case ConsoleKey.LeftArrow:
					CursorX = CursorX - 1;
					if (CursorX < 0)
					{
						CursorX = 0;
						if (WindowX > 0) WindowX--;
					}
					Show();
					break;
				case ConsoleKey.RightArrow:
					CursorX++;
					if (WindowX + CursorX > Text.Length) CursorX--; 
					if (CursorX >= Width)
					{
						CursorX = Width - 1;
						if (WindowX < Text.Length - Width) WindowX++;
					}
					CursorX = Math.Min(Width - 1, CursorX + 1);
					Show();
					break;
				case ConsoleKey.Enter:
					if (Parent.DefaultButton != null)
					{
						Parent.DefaultButton?.Click?.Invoke();
						return true;
					}
					break;
				default:
					var text = Text;
					var pos = CursorX + WindowX;
					Text = $"{text.Substring(0, pos)}{key.KeyChar}{text.Substring(pos)}";
					CursorX++;
					if (CursorX >= Width)
					{
						CursorX = Width - 1;
						WindowX++;
					}
					Show();
					break;
			}
			return false;
		}

		public override void Show()
		{
			var text = Text;
			Text = new string(Enumerable.Repeat('*', text.Length).ToArray());
			base.Show();
			Text = text;
		}
	}

	public class PasswordField: TextField
	{

		public override void Show()
		{
			var text = Text;
			Text = new string(Enumerable.Repeat('*', text.Length).ToArray());
			base.Show();
			Text = text;
		}
	}

	public class Button : ConsoleField
	{
		public bool Default { get; set; } = false;
	}

	public class FieldList: KeyedCollection<string, ConsoleField>
	{
		protected override string GetKeyForItem(ConsoleField item) => item.Name;
	}

	public class ConsoleForm
	{
		public FieldList Fields = new FieldList();
		public Installer Installer { get; set; }
		public Shell Shell => Installer.Shell;
		public string Template = "";
		public PercentField Progress => Fields
			.OfType<PercentField>()
			.FirstOrDefault();

		public ConsoleField Focus = null;
		
		public Button DefaultButton => Fields
			.OfType<Button>()
			.FirstOrDefault(f => f.Default);

		public ConsoleForm ShowProgress(Shell shell, int estimatedMaxProgress)
		{
			if (Progress != null)
			{
				Progress.EstimatedMaxProgress = estimatedMaxProgress;
				Progress.Shell = shell;
			}
			return this;
		}

		public ConsoleField this[string name] => Fields[name];
		public ConsoleForm Show()
		{
			Console.Clear();
			// count lines in Template
			int nlines = 0;
			var nl = Template.IndexOf('\n');
			while (nl >= 0)
			{
				nlines++;
				nl = Template.IndexOf("\n", nl + 1);
			}
			var y = (Console.WindowHeight - nlines) / 2;
			var con = ConsoleSettings.Save();
			Console.CursorVisible = false;
			Console.SetCursorPosition(0, y);
			Console.Write(Template);
			foreach (var field in Fields) field.Show();
			SetFocus(Fields.FirstOrDefault());
			return this;
		}
		public ConsoleForm ShowDialog()
		{
			Show();
			Edit();
			return this;
		}

		public ConsoleForm Apply(Action<ConsoleForm> action)
		{
			action?.Invoke(this);
			return this;
		}
		public void SetFocus(ConsoleField field)
		{
			Console.CursorVisible = false;
			if (Focus != null) Focus.HasFocus = false;
			if (field != null)
			{
				field.HasFocus = true;
				Focus = field;
				field.Show();
			}
		}
		public ConsoleForm Edit()
		{
			if (Focus == null) return this;

			do
			{
				var key = Console.ReadKey();
				if (Focus.Editable && Focus.Edit(key)) return this;
				if (EditNavigate(key)) return this;
			} while (true);
			//return this;
		}

		public bool EditNavigate(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
				case ConsoleKey.DownArrow:
				case ConsoleKey.LeftArrow:
				case ConsoleKey.RightArrow:
					EditChangeFocus(key); break;
				case ConsoleKey.Enter:
					if (Focus is Button)
					{
						Focus.Click?.Invoke();
						return true;
					}
					else
					{
						var defaultButton = DefaultButton;
						if (defaultButton != null) defaultButton.Click?.Invoke();
						return true;
					}
				default: break;
			}
			return false;


		}

		public void EditChangeFocus(ConsoleKeyInfo key)
		{
			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
					var fieldUpwards = Fields
						.Where(f => f.Y < Focus.Y)
						.OrderByDescending(f => f.Y)
						.FirstOrDefault();
					if (fieldUpwards != null) SetFocus(fieldUpwards);
					break;
				case ConsoleKey.DownArrow:
					var fieldDownwards = Fields
						.Where(f => f.Y > Focus.Y)
						.OrderBy(f => f.Y)
						.FirstOrDefault();
					if (fieldDownwards != null) SetFocus(fieldDownwards);
					break;
				case ConsoleKey.LeftArrow:
					var fieldLeftwards = Fields
						.Where(f => f.X < Focus.X)
						.OrderByDescending(f => f.X)
						.FirstOrDefault();
					if (fieldLeftwards != null) SetFocus(fieldLeftwards);
					break;
				case ConsoleKey.RightArrow:
					var fieldRightwards = Fields
						.Where(f => f.X > Focus.X)
						.OrderBy(f => f.X)
						.FirstOrDefault();
					if (fieldRightwards != null) SetFocus(fieldRightwards);
					break;
				default: break;
			}
		}

		public ConsoleForm Close()
		{
			Console.Clear();
			return this;
		}

		public ConsoleForm Save(object result)
		{
			var type = result.GetType();
			foreach (var p in type.GetProperties())
			{
				if (Fields.Contains(p.Name))
				{
					var text = Fields[p.Name].Text;
					if (p.PropertyType == typeof(string))
					{
						p.SetValue(result, text);
					}
					else if (p.PropertyType == typeof(int))
					{
						int i = 0;
						if (int.TryParse(text, out i)) p.SetValue(result, i);
					}
					else if (p.PropertyType == typeof(long))
					{
						long i = 0;
						if (long.TryParse(text, out i)) p.SetValue(result, i);
					}
					else if (p.PropertyType == typeof(float))
					{
						float x = 0;
						if (float.TryParse(text, out x)) p.SetValue(result, x);
					}
					else if (p.PropertyType == typeof(double))
					{
						double x = 0;
						if (double.TryParse(text, out x)) p.SetValue(result, x);
					}
				}
			}
			foreach (var f in type.GetFields())
			{
				if (Fields.Contains(f.Name))
				{
					var text = Fields[f.Name].Text;
					if (f.FieldType == typeof(string))
					{
						f.SetValue(result, text);
					}
					else if (f.FieldType == typeof(int))
					{
						int i = 0;
						if (int.TryParse(text, out i)) f.SetValue(result, i);
					}
					else if (f.FieldType == typeof(long))
					{
						long i = 0;
						if (long.TryParse(text, out i)) f.SetValue(result, i);
					}
					else if (f.FieldType == typeof(float))
					{
						float x = 0;
						if (float.TryParse(text, out x)) f.SetValue(result, x);
					}
					else if (f.FieldType == typeof(double))
					{
						double x = 0;
						if (double.TryParse(text, out x)) f.SetValue(result, x);
					}
				}
			}
			return this;
		}

		public ConsoleForm Load(object source)
		{
			var type = source.GetType();
			foreach (var p in type.GetProperties())
			{
				if (Fields.Contains(p.Name))
				{
					var field = Fields[p.Name];
					field.Text = p.GetValue(source).ToString();
				}
			}
			foreach (var f in type.GetFields())
			{
				if (Fields.Contains(f.Name))
				{
					var field = Fields[f.Name];
					field.Text = f.GetValue(source).ToString();
				}
			}
			return this;
		}

		public ConsoleForm() { }
		public ConsoleForm(string template): this()
		{
			Parse(template);
		}

		public ConsoleForm Parse(string template)
		{
			template = template.Trim();
			var fieldMatches = Regex.Matches(template, @"^(?<prefix>.*?)\[(<?option>\*|%|\?|\!|)\s*(?<name>[A-Za-z_][A-Za-z_0-9]*))(?<text>[^\]]*?)\]", RegexOptions.Singleline);
			var fields = fieldMatches
				.OfType<Match>()
				.Select<Match, ConsoleField>(m =>
				{
					int y = 0, x = 0;
					var prefix = m.Groups["prefix"].Value;
					int nl = prefix.IndexOf('\n');
					int lastnl = 0;
					while (nl > 0)
					{
						y++;
						lastnl = nl;
						nl = prefix.IndexOf('\n', lastnl + 1);
					}
					x = prefix.Length - lastnl - 1;

					var text = m.Groups["text"].Value;
					var option = m.Groups["option"].Value;
					switch (option)
					{
						case "*":
							return new Button() { Default = true, X = x, Y = y, Width = text.Length, Height = 1, Text = text.Trim() };
						case "":
						default:
							return new Button() { X = x, Y = y, Width = text.Length, Height = 1, Text = text.Trim() };
						case "?":
							return new TextField() { X = x, Y = y, Width = text.Length, Height = 1, Text = text.Trim() };
						case "!":
							return new PasswordField() { X = x, Y = y, Width = text.Length, Height = 1, Text = "" };
						case "%":
							return new PercentField() { X = x, Y = y, Width = text.Length, Height = 1 };
					}
				});

			Fields.Clear();
			foreach (var field in fields) {
				Fields.Add(field);
			}

			Template = Regex.Replace(template, @"(?:\[\?|(?<=\[)\*)|\[%)\s*[A-Za-z_][A-Za-z_0-9]*)|(?:(?<=\[(?:\?|%)\s*[A-Za-z_][A-Za-z_0-9]*[^\]]*?)\])", "", RegexOptions.Singleline)
				.Trim();
			return this;
		}
	}
}