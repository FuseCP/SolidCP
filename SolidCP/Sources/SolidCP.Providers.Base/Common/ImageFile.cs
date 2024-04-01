using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers
{
	public class ImageFile
	{
		public string MimeType { get; set; }
		public string FileExtension { get; set; }
		public byte[] RawData { get; set; }
	}
}
