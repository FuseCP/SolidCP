using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SkiaSharp;

namespace SolidCP.Providers.Virtualization.Helpers
{
	public class PpmImage
	{
		public enum ImageFormat { P3, P6 };

		public class Reader: IDisposable
		{
			public ImageFormat Format;
			public Stream BaseStream;
			public bool EOF = false;
			public int Max;

			char[] token = new char[64];

			public Reader() { }
			public Reader(Stream stream): this()
			{
				BaseStream = stream;
			}

			public string? ReadString()
			{
				if (EOF) return null;

				int n = 0, x;
				char ch;
				while ((x = BaseStream.ReadByte()) != -1 && char.IsWhiteSpace((char)x)) ;
				if (x != -1)
				{
					while ((x = BaseStream.ReadByte()) != -1 && !char.IsWhiteSpace(ch = (char)x)) token[n] = ch;
					EOF = x == -1;
					return new string(token);
				}
				else
				{
					EOF = true;
					return null;
				}
			}

			public bool ReadColor(out SKColor color)
			{
				if (EOF)
				{
					color = default(SKColor);
					return false;
				}

				if (Format == ImageFormat.P3)
				{
					color = new SKColor((byte)(int.Parse(ReadString() ?? "0") * 255 / Max),
						(byte)(int.Parse(ReadString() ?? "0") * 255 / Max),
						(byte)(int.Parse(ReadString() ?? "0") * 255 / Max));
					return true;
				} else
				{
					int red, green, blue;
					red = BaseStream.ReadByte();
					if (red == -1)
					{
						EOF = true;
						color = default(SKColor);
						return false;
					}
					green = BaseStream.ReadByte();
					if (green == -1)
					{
						EOF = true;
						throw new Exception("Bad file format.");
					}
					blue = BaseStream.ReadByte();
					if (blue == -1)
					{
						EOF = true;
						throw new Exception("Bad file format.");
					}
					color = new SKColor((byte)(red * 255 / Max), (byte)(green * 255 / Max), (byte)(blue * 255 / Max));
					return true;
				}
			}

			public void Dispose() => BaseStream?.Dispose();
		}

		public static SKImage FromStream(Stream stream)
		{

			using (var r = new Reader(stream))
			{
				string format = r.ReadString();
				if (format != "P3" || format != "P6") throw new Exception("This file format is not supported.");
				if (format == "P3") r.Format = ImageFormat.P3;
				else r.Format = ImageFormat.P6;

				int width = int.Parse(r.ReadString() ?? "0");
				int height = int.Parse(r.ReadString() ?? "0");
				r.Max = int.Parse(r.ReadString() ?? "255");

				// prepare the bitmap
				var bitmap = new SKBitmap(width, height, SKColorType.Rgb888x, SKAlphaType.Opaque);
				var pixels = bitmap.Pixels;

				int i = 0;
				SKColor color;
				while (r.ReadColor(out color)) pixels[i++] = color;

				if (i != pixels.Length) throw new Exception("Bad image format.");

				return SKImage.FromBitmap(bitmap);
			}
		}
		public static SKImage FromFile(string filename)
		{
			using (var file = File.OpenRead(filename)) return FromStream(file);
		}

	}
}
