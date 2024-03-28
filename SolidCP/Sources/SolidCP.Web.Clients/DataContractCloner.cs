using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Serialization;
using System.Runtime.Remoting.Channels;
using System.Xml;
using System.IO;
using Compat.Runtime.Serialization;

namespace SolidCP.Web.Clients
{
	public class DataContractCopier
	{

		public static readonly IByteBufferPool BufferPool = new ByteBufferPool(1000, 1024);
		public static object Clone(object src)
		{
			if (src == null) return null;

			var type = src.GetType();
			if (type.IsPrimitive || type == typeof(string) ||
				type == typeof(DateTime) || type == typeof(TimeSpan) ||
				type == typeof(DateTimeOffset) || type == typeof(Guid) ||
				type == typeof(Uri) || type == typeof(XmlQualifiedName)) return src;

			var mem = new ChunkedMemoryStream(BufferPool);
			var writer = XmlDictionaryWriter.CreateBinaryWriter(mem);
			var serializer = new NetDataContractSerializer();
			serializer.WriteObject(writer, src);
			writer.Flush();
			mem.Seek(0, SeekOrigin.Begin);

			var reader = XmlDictionaryReader.CreateBinaryReader(mem, new XmlDictionaryReaderQuotas()
			{
				MaxArrayLength = int.MaxValue,
				MaxBytesPerRead = int.MaxValue,
				MaxDepth = int.MaxValue,
				MaxNameTableCharCount = int.MaxValue,
				MaxStringContentLength = int.MaxValue
			});
			var copy = serializer.ReadObject(reader);
			mem.Dispose();
			return copy;
		}
	}
}
