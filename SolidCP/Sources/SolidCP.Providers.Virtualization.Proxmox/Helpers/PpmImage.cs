using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using SkiaSharp;

namespace SolidCP.Providers.Virtualization
{
	public class PpmImage
	{
		public SKImage SkiaImage { get; set; } = null;

		public static implicit operator SKImage(PpmImage img) => img.SkiaImage; 

		public enum ImageFormat { P3, P6 };

		public class Reader: IDisposable
		{
			public ImageFormat Format;
			public Stream BaseStream;
			public bool EOF = false;
			public int MaxColor;

			char[] token = new char[64];

			public Reader() { }
			public Reader(Stream stream): this()
			{
				BaseStream = stream;
			}

			public int ReadColorFragment()
			{
				int x, y;
				x = BaseStream.ReadByte();
				if (x == -1 || MaxColor <= 255) return x;
				else x = x * 256;
				y = BaseStream.ReadByte();
				if (y == -1) new Exception("Bad file format.");
				return (x + y) * 255 / MaxColor;
			}

			public string? ReadString()
			{
				if (EOF) return null;

				int n = 0, x;
				char ch;
				while ((x = BaseStream.ReadByte()) != -1 && char.IsWhiteSpace((char)x)) ;
				if (x != -1)
				{
					token[n++] = (char)x;
					while ((x = BaseStream.ReadByte()) != -1 && !char.IsWhiteSpace(ch = (char)x)) token[n++] = ch;
					EOF = x == -1;
					return new string(token, 0, n);
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
					color = new SKColor((byte)(int.Parse(ReadString() ?? "0") * 255 / MaxColor),
						(byte)(int.Parse(ReadString() ?? "0") * 255 / MaxColor),
						(byte)(int.Parse(ReadString() ?? "0") * 255 / MaxColor));
					return true;
				} else
				{
					int red, green, blue;
					red = ReadColorFragment();
					if (red == -1)
					{
						EOF = true;
						color = default(SKColor);
						return false;
					}
					green = ReadColorFragment();
					if (green == -1)
					{
						EOF = true;
						throw new Exception("Bad file format.");
					}
					blue = ReadColorFragment();
					if (blue == -1)
					{
						EOF = true;
						throw new Exception("Bad file format.");
					}
					color = new SKColor((byte)(red * 255 / MaxColor), (byte)(green * 255 / MaxColor), (byte)(blue * 255 / MaxColor));

					return true;
				}
			}

			public void Dispose() => BaseStream?.Dispose();
		}

		public static SKBitmap BitmapFromStream(Stream stream)
		{

			using (var r = new Reader(stream))
			{
				string format = r.ReadString();
				if (format != "P3" && format != "P6") throw new Exception("This file format is not supported.");
				if (format == "P3") r.Format = ImageFormat.P3;
				else r.Format = ImageFormat.P6;

				int width = int.Parse(r.ReadString() ?? "0");
				int height = int.Parse(r.ReadString() ?? "0");
				r.MaxColor = int.Parse(r.ReadString() ?? "255");

				// prepare the bitmap
				var bitmap = new SKBitmap(width, height, SKColorType.Rgb888x, SKAlphaType.Opaque);
				var nofpixels = width * height;

				/*
				int n = 0, x = 0, y = 0;
				SKColor color;
				while (r.ReadColor(out color))
				{
					if (x >= width)
					{
						y++; x = 0;
						if (y >= height) throw new Exception("Bad image foramt.");
					}
					bitmap.SetPixel(x++, y, color);
				}

				if (y * width + x != nofpixels) throw new Exception("Bad image format.");
				*/
				var pixels = new SKColor[nofpixels];

				int i = 0;
                SKColor color;
                while (r.ReadColor(out color) && i < nofpixels) pixels[i++] = color;
				
                if (i != nofpixels) throw new Exception("Bad image format.");

				bitmap.Pixels = pixels;

                return bitmap;
			}
		}
		public static SKBitmap BitmapFromFile(string filename)
		{
			using (var file = File.OpenRead(filename)) return BitmapFromStream(file);
		}

		public static SKImage SkiaImageFromStream(Stream stream) => SKImage.FromBitmap(BitmapFromStream(stream));
		public static SKImage SkiaImageFromFile(string filename) => SKImage.FromBitmap(BitmapFromFile(filename));

		public PpmImage() { }
		public PpmImage(Stream stream)
		{
			SkiaImage = SkiaImageFromStream(stream);
		}
		public PpmImage(string filename)
		{
			SkiaImage = SkiaImageFromFile(filename);
		}

		public static PpmImage FromStream(Stream stream) => new PpmImage(stream);
		public static PpmImage FromFile(string filename) => new PpmImage(filename);
	}
}
