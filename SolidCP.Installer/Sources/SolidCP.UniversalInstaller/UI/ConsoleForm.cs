using System.Collections.ObjectModel;
using System.ComponentModel;
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
		public string? Name = null;
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
		string text = "";
		public virtual string Text
		{
			get => text;
			set
			{
				if (text != value)
				{
					text = value;
					Validate?.Invoke(this);
				}
				else text = value;
			}
		}

		public ConsoleField() { Text = ""; }
		public Action<ConsoleField>? Validate = null;
		public Action? Click = null;
		public bool Clicked { get; set; } = false;
		public bool Checked { get; set; } = false;
		public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
		public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
		public virtual ConsoleForm? Parent { get; set; } = null;

		public virtual bool Edit(ConsoleKeyInfo key) => false;
		public virtual void ReceiveFocus()
		{
			HasFocus = true;
		}

		public virtual void Show()
		{
			// add space fillers to lines so Console.Write will cover the whole window
			var displayText = Regex.Replace(Text, "(?<=^|\r?\n)([^\r\n$]*)(?=\r?\n|$)", match =>
			{
				var text = match.Groups[1].Value;
				int len = text.Length;
				int addLeft, addRight;
				if (!Centered)
				{
					addLeft = 0;
					addRight = Math.Max(0, Width - len + WindowX);
				}
				else
				{
					var w = Math.Max(0, Width - len);
					addLeft = w / 2;
					addRight = w - addLeft;
				}
				char[] spacesLeft = Enumerable.Repeat(' ', addLeft).ToArray();
				char[] spacesRight = Enumerable.Repeat(' ', addRight).ToArray();
				text = $"{new string(spacesLeft)}{text}{new string(spacesRight)}";
				if (WindowX > 0 || text.Length > Width - WindowX) text = text.Substring(WindowX, Width);
				return text;
			}, RegexOptions.Singleline);

			// save settings
			var con = ConsoleSettings.Set(X, Parent!.Y + Y, false, BackgroundColor, ForegroundColor);

			Console.Write(displayText);

			// restore settings
			con.Restore();
		}
	}

	public class PercentField : ConsoleField
	{

		public PercentField() : base()
		{
			BackgroundColor = ConsoleColor.DarkGray;
		}

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

		public override bool Centered => false;
		public int EstimatedMaxProgress { get; set; }

		Shell? shell;
		int lines = 0;
		public Shell? Shell
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
							Value = 1.0f - (float)Math.Exp(-((float)lines++ / (float)EstimatedMaxProgress));
						};
					}
				}
			}
		}

		public override void Show()
		{
			var num = $"{Value:P0}";
			var pos = Math.Max(0, (Width - num.Length) / 2);
			var leftSpaces = new string(Enumerable.Repeat(' ', pos).ToArray());
			var rightSpacesCount = Math.Max(0, Width - pos - num.Length);
			var rightSpaces = new string(Enumerable.Repeat(' ', rightSpacesCount).ToArray());
			Text = $"{leftSpaces}{num}{rightSpaces}";
			pos = Math.Min(Width, Math.Max(0, (int)(Value*Width + 0.5)));
			var width = Width;
			var background = BackgroundColor;
			var x = X;
			Width = pos;
			BackgroundColor = ConsoleColor.Green;
			base.Show();
			BackgroundColor = background;
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
		public TextField() : base()
		{
			BackgroundColor = ConsoleColor.DarkGray;
			ForegroundColor = ConsoleColor.Black;
		}
		public override bool CanFocus => true;
		public override bool Editable => true;
		public override void ReceiveFocus()
		{
			base.ReceiveFocus();
			CursorX = Text.Length;
		}
		public override bool Edit(ConsoleKeyInfo key)
		{
			int pos;
			string text;
			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
				case ConsoleKey.DownArrow:
				case ConsoleKey.Tab:
				case ConsoleKey.Enter:
					return Parent!.EditNavigate(key);
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
						if (WindowX <= Text.Length - Width) WindowX++;
					}
					Show();
					break;
				case ConsoleKey.Delete:
					pos = WindowX + CursorX;
					text = Text;
					if (pos < text.Length)
					{
						text = $"{text.Substring(0, pos)}{text.Substring(pos + 1)}";
						Text = text;
					}
					Show();
					break;
				case ConsoleKey.Backspace:
					pos = WindowX + CursorX;
					if (pos > 0)
					{
						text = Text;
						text = $"{text.Substring(0, pos - 1)}{text.Substring(pos)}";
						Text = text;
					}
					if (CursorX > 0) CursorX--;
					else if (WindowX > 0) WindowX--;
					Show();
					break;
				default:
					text = Text;
					pos = CursorX + WindowX;
					Text = $"{text.Substring(0, pos)}{key.KeyChar}{text.Substring(pos)}";
					CursorX++;
					if (CursorX >= Width)
					{
						CursorX = Width - 1;
						if (WindowX <= Text.Length - Width) WindowX++;
					}
					Show();
					break;
			}
			return false;
		}

		public override void Show()
		{
			base.Show();
			if (HasFocus)
			{
				Console.SetCursorPosition(X + CursorX, Parent!.Y + Y);
				Console.CursorVisible = true;
			}
		}
	}

	public class PasswordField : TextField
	{
		public PasswordField() : base() { }
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
		public override bool CanFocus => true;
		public override bool Centered => true;
		public Button() : base() { }
		public override void Show()
		{
			var text = Text;
			text = text.Trim(' ', '[', ']');
			int len = text.Length;
			int addLeft, addRight;
			var w = Math.Max(0, Width - len - 2);
			addLeft = w / 2;
			addRight = w - addLeft;
			char[] spacesLeft = Enumerable.Repeat(' ', addLeft).ToArray();
			char[] spacesRight = Enumerable.Repeat(' ', addRight).ToArray();
			Text = $"[{new string(spacesLeft)}{text}{new string(spacesRight)}]";
			var background = BackgroundColor;
			if (HasFocus) BackgroundColor = ConsoleColor.DarkGray;
			base.Show();
			BackgroundColor = background;
			Text = text;
		}
	}

	public class Choice : Button
	{

		public Choice() : base() { }
		public override bool Editable => true;

		public override void ReceiveFocus()
		{
			HasFocus = true;
		}

		public override bool Edit(ConsoleKeyInfo key)
		{
			if (key.Key == ConsoleKey.Spacebar)
			{
				Checked = !Checked;
				Text = Checked ? "x" : " ";
				Show();
				return false;
			}
			else return Parent!.EditNavigate(key);
		}
	}
	public class FieldList : KeyedCollection<string, ConsoleField>
	{
		protected override string GetKeyForItem(ConsoleField item) => item.Name?.Trim() ?? "";
	}

	public class ConsoleForm
	{
		public int Y = 0;
		public FieldList Fields = new FieldList();
		public Installer Installer { get; set; } = Installer.Current;
		public Shell Shell => Installer.Shell;
		public string Template = "";
		public PercentField Progress => Fields
			.OfType<PercentField>()
			.FirstOrDefault();

		public ConsoleField? Focus = null;

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
		public ConsoleField this[int index] => Fields[index];
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
			Y = (Console.WindowHeight - nlines) / 2;
			var con = ConsoleSettings.Save();
			Console.CursorVisible = false;
			Console.SetCursorPosition(0, Y);
			Console.Write(Template);
			SetFocus(Fields.FirstOrDefault(f => f.CanFocus));

			foreach (var field in Fields)
				field.Show();

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
			if (Focus != null)
			{
				Focus.HasFocus = false;
				Focus.Show();
			}
			if (field != null)
			{
				field.ReceiveFocus();
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
				if (Focus.Editable)
				{
					if (Focus.Edit(key)) return this;
				}
				else if (EditNavigate(key)) return this;
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
				case ConsoleKey.Tab:
					EditChangeFocus(key);
					break;
				case ConsoleKey.Enter:
					if (Focus is Button)
					{
						Focus.Clicked = true;
						Focus.Click?.Invoke();
						return true;
					}
					else
					{
						var defaultButton = DefaultButton;
						if (defaultButton != null)
						{
							defaultButton.Clicked = true;
							defaultButton.Click?.Invoke();
							return true;
						}
					}
					break;
				default: break;
			}
			return false;


		}

		public void EditChangeFocus(ConsoleKeyInfo key)
		{
			if (Focus == null) return;

			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
					var fieldUpwards = Fields
						.Where(f => f.CanFocus && f.Y + f.Height / 2 < Focus.Y + Focus.Height / 2)
						.OrderByDescending(f => f.Y + f.Height / 2)
						.ThenBy(f => Math.Abs(f.X + f.Width / 2 - Focus.X - Focus.Width / 2))
						.FirstOrDefault();
					if (fieldUpwards != null) SetFocus(fieldUpwards);
					break;
				case ConsoleKey.DownArrow:
					var fieldDownwards = Fields
						.Where(f => f.CanFocus && f.Y + f.Height / 2 > Focus.Y + Focus.Height / 2)
						.OrderBy(f => f.Y + f.Height / 2)
						.ThenBy(f => Math.Abs(f.X + f.Width / 2 - Focus.X - Focus.Width / 2))
						.FirstOrDefault();
					if (fieldDownwards != null) SetFocus(fieldDownwards);
					break;
				case ConsoleKey.LeftArrow:
					var fieldLeftwards = Fields
						.Where(f => f.CanFocus && f.X + f.Width / 2 < Focus.X + Focus.Width / 2)
						.OrderByDescending(f => f.X + f.Width / 2)
						.ThenBy(f => Math.Abs(f.Y + f.Height / 2 - Focus.Y - Focus.Height / 2))
						.FirstOrDefault();
					if (fieldLeftwards != null) SetFocus(fieldLeftwards);
					break;
				case ConsoleKey.RightArrow:
					var fieldRightwards = Fields
						.Where(f => f.CanFocus && f.X + f.Width / 2 > Focus.X + Focus.Width / 2)
						.OrderBy(f => f.X + f.Width / 2)
						.ThenBy(f => Math.Abs(f.Y + f.Height / 2 - Focus.Y - Focus.Height / 2))
						.FirstOrDefault();
					if (fieldRightwards != null) SetFocus(fieldRightwards);
					break;
				case ConsoleKey.Tab:
					var index = Fields.IndexOf(Focus);
					index = (index + 1) % Fields.Count;
					while (!Fields[index].CanFocus) index = (index + 1) % Fields.Count;
					SetFocus(Fields[index]);
					break;
				default: break;
			}
		}

		public ConsoleForm Close()
		{
			Console.Clear();
			return this;
		}

		public ConsoleForm Save(object? result)
		{
			if (result == null) return this;

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

		public ConsoleForm Load(object? source)
		{
			if (source == null) return this;

			var type = source.GetType();
			foreach (var p in type.GetProperties())
			{
				if (Fields.Contains(p.Name))
				{
					var field = Fields[p.Name];
					var text = p.GetValue(source)?.ToString();
					if (!string.IsNullOrEmpty(text)) field.Text = text!;
				}
			}
			foreach (var f in type.GetFields())
			{
				if (Fields.Contains(f.Name))
				{
					var field = Fields[f.Name];
					var text = f.GetValue(source)?.ToString();
					if (!string.IsNullOrEmpty(text)) field.Text = text!;
				}
			}
			return this;
		}

		public ConsoleForm() { }
		public ConsoleForm(string template) : this()
		{
			Parse(template);
		}

		public ConsoleForm Parse(string template)
		{
			template = template.Trim();
			int n = 0;
			var fieldMatches = Regex.Matches(template, @"(?<=^(?<prefix>.*?))\[(?<option>\*|%|\?|\!|x|)(?:(?<!\[|\[\*)\s*(?<name>[A-Za-z_][A-Za-z_0-9]*))?(?<text>[^\]]*?)\]", RegexOptions.Singleline);
			var fields = fieldMatches
				.OfType<Match>()
				.Select<Match, ConsoleField>(m =>
				{
					int y = 0, x = 0;
					string prefix = "";
					if (m.Groups["prefix"].Success) prefix = m.Groups["prefix"].Value;
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
					string name = "";
					if (m.Groups["name"].Success) name = m.Groups["name"].Value;
					else name = text.Trim();
					if (string.IsNullOrEmpty(name)) name = $"_noname{n++}";
					var option = m.Groups["option"].Value;
					switch (option)
					{
						case "*":
							return new Button() { Parent = this, Name = name, Default = true, X = x, Y = y, Width = text.Length + 2, Height = 1, Text = text.Trim() };
						case "":
						default:
							return new Button() { Parent = this, Name = name, X = x, Y = y, Width = text.Length + 2, Height = 1, Text = text.Trim() };
						case "?":
							return new TextField() { Parent = this, Name = name, X = x, Y = y, Width = text.Length, Height = 1, Text = text.Trim() };
						case "!":
							return new PasswordField() { Parent = this, Name = name, X = x, Y = y, Width = text.Length, Height = 1, Text = text.Trim() };
						case "%":
							return new PercentField() { Parent = this, Name = name, X = x, Y = y, Width = text.Length, Height = 1, Value = 0 };
						case "x":
							return new Choice() { Parent = this, Name = name, X = x, Y = y, Width = 3, Height = 1, Text = " " };
					}
				});

			Fields.Clear();
			foreach (var field in fields)
			{
				Fields.Add(field);
			}

			if (DefaultButton == null)
			{
				var last = Fields.OfType<Button>().LastOrDefault();
				if (last != null) last.Default = true;
			}

			Focus = null;
			SetFocus(Fields.FirstOrDefault(f => f.CanFocus));

			template = Regex.Replace(template.Trim(), @"(?:\[(?:\?|!|%)\s*[a-zA-Z_][a-zA-Z0-9_]*)|(?:(?<=\[)\*)(?=[^\n\]]*\])|(?<=\[(?:\?|!|%)[^\]]*)\]", "", RegexOptions.Singleline);
			Template = Regex.Replace(template, @"\[x\s*[A-Za-z_][A-Za-z_0-9]*\]", "[ ]");
			return this;
		}
	}
}