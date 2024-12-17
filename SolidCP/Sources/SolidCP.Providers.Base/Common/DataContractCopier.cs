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
using System.Runtime.Serialization;

namespace SolidCP.Providers
{
    public class DataContractCopier
    {

        public static readonly IByteBufferPool BufferPool = new ByteBufferPool(1000, 1024);
        public static object Clone(object src, IEnumerable<Type> knownTypes = null)
        {
            if (src == null) return null;

            if (knownTypes == null) knownTypes = new Type[0];

            knownTypes = knownTypes
                .Distinct()
                .Where(t => t != null);

            var type = src.GetType();
            if (type.IsPrimitive || type == typeof(string) ||
                type == typeof(DateTime) || type == typeof(TimeSpan) ||
                type == typeof(DateTimeOffset) || type == typeof(Guid) ||
                type == typeof(Uri) || type == typeof(XmlQualifiedName)) return src;

            if (type.IsArray && type.GetArrayRank() == 1) // treat array of primitive types special
            {
                var array = (Array)src;
                bool isPrimitive = false;

                var elementType = type.GetElementType();
                if (elementType == typeof(object))
                {
                    isPrimitive = array.OfType<object>().All(item =>
                    {
                        var it = item?.GetType();
                        return it == null || it.IsPrimitive || it == typeof(string) ||
                            it == typeof(DateTime) || it == typeof(TimeSpan) ||
                            it == typeof(DateTimeOffset) || it == typeof(Guid) ||
                            it == typeof(Uri) || it == typeof(XmlQualifiedName);
                    });
                    if (!isPrimitive)
                    {
                        var types = array.OfType<object>()
                            .Select(obj => obj?.GetType())
                            .Where(t => t != null);
                        knownTypes = knownTypes.Concat(types)
                            .Distinct();
                    }
                }
                else
                {
                    isPrimitive = elementType.IsPrimitive || elementType == typeof(string) ||
                        elementType == typeof(DateTime) || elementType == typeof(TimeSpan) ||
                        elementType == typeof(DateTimeOffset) || elementType == typeof(Guid) ||
                        elementType == typeof(Uri) || elementType == typeof(XmlQualifiedName);
                }

                if (isPrimitive)
                {
                    var arrayCopy = Array.CreateInstance(type.GetElementType(), array.Length);
                    Array.Copy(array, arrayCopy, array.Length);
                    return arrayCopy;
                }
            }
            var mem = new ChunkedMemoryStream(BufferPool);
            var writer = XmlDictionaryWriter.CreateBinaryWriter(mem);
            var serializer = new DataContractSerializer(type, knownTypes);
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
