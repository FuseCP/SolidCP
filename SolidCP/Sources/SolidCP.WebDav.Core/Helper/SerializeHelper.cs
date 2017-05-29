using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SolidCP.WebDav.Core.Helper
{
    public class SerializeHelper
    {
        public static TResult Deserialize<TResult>(string inputString)
        {
            TResult result;

            var serializer = new XmlSerializer(typeof(TResult));

            using (TextReader reader = new StringReader(inputString))
            {
                result = (TResult)serializer.Deserialize(reader);
            }

            return result;
        }

        public static string Serialize<TEntity>(TEntity entity)
        {
            string result = string.Empty;

            var xmlSerializer = new XmlSerializer(typeof(TEntity));

            using (var stringWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(writer, entity);
                    result = stringWriter.ToString();
                }
            }

            return result;
        } 
    }
}