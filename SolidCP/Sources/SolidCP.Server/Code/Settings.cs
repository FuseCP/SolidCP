using System;
using SolidCP.Providers;
using SolidCP.Web.Services;

namespace SolidCP.Server
{
    public class Settings
    {

#if NETFRAMEWORK
	    public static string Password => ServerConfiguration.Security.Password;
#else
	    public static string Password => Configuration.Password;
#endif

        static string cryptoKey = null;
        public static string CryptoKey => cryptoKey ?? (cryptoKey = CryptoUtility.SHA256($"{Password}{DateTime.Now}"));
    }
}
