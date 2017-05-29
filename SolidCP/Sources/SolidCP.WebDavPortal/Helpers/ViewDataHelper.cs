using System;

namespace SolidCP.WebDavPortal.Helpers
{
    public class ViewDataHelper
    {
        public static string BytesToSize(long bytes)
        {
            if (bytes == 0)
            {
                return string.Format("0 {0}", Resources.UI.Byte);
            }

            var k = 1024;

            var sizes = new[]
            {
                Resources.UI.Bytes,
                Resources.UI.KilobyteShort,
                Resources.UI.MegabyteShort,
                Resources.UI.GigabyteShort,
                Resources.UI.TerabyteShort,
                Resources.UI.PetabyteShort,
                Resources.UI.ExabyteShort
            };

            var i = (int) Math.Floor(Math.Log(bytes)/Math.Log(k));
            return string.Format("{0} {1}", Math.Round(bytes/Math.Pow(k, i), 3), sizes[i]);
        }
    }
}