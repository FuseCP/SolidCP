using System;

namespace SolidCP.WIXInstaller.Common
{
    internal struct YesNo
    {
        public static string Yes { get { return "1"; } }

        public static string No { get { return "0"; } }

        public static string Get(string Value)
        {
            return Value.TrimStart('#');
        }
    }
}
