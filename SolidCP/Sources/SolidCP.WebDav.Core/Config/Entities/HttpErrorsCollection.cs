using System.Globalization;

namespace SolidCP.WebDav.Core.Config.Entities
{
    public class HttpErrorsCollection
    {
        public string this[int statusCode]
        {
            get
            {
                var message = Resources.HttpErrors.ResourceManager.GetString("_" + statusCode.ToString(CultureInfo.InvariantCulture));
                return message ?? Default;
            }
        }

        public string Default
        {
            get { return Resources.HttpErrors.Default; }
        }
    }
}