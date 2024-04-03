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
	    public static string Password => StartupCore.Password;
#endif

        static string cryptoKey = null;
        public static string CryptoKey => cryptoKey ?? (cryptoKey = CryptoUtility.SHA1($"{Password}{DateTime.Now}"));
    }
}
