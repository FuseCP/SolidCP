using System.IO;

namespace SolidCP.LinuxVmConfig
{
    public enum OsVersionEnum
    {
        NA,
        Linux_NA,
        Ubuntu,
        CentOS,
        FreeBSD
    }

    public static class OsVersion
    {
        private static OsVersionEnum osVersion = OsVersionEnum.NA;
        public static OsVersionEnum GetOsVersion()
        {
            if (osVersion != OsVersionEnum.NA) return osVersion;

            if (File.Exists("/bin/freebsd-version"))
            {
                osVersion = OsVersionEnum.FreeBSD;
            }
            else
            {
                osVersion = OsVersionEnum.Linux_NA;
            }

            if (osVersion == OsVersionEnum.Linux_NA)
            {
                ExecutionResult res = ShellHelper.RunCmd("cat /etc/os-release | grep \"ID=\"");
                if (res.ResultCode != 1) osVersion = FindOsVersion(res.Value);
            }
            return osVersion;
        }

        private static OsVersionEnum FindOsVersion(string result)
        {
            if (result.ToLower().Contains("freebsd"))
            {
                return OsVersionEnum.FreeBSD;
            }
            else if (result.ToLower().Contains("ubuntu"))
            {
                return OsVersionEnum.Ubuntu;
            }
            else if (result.ToLower().Contains("centos"))
            {
                return OsVersionEnum.CentOS;
            }
            else if (result.ToLower().Contains("linux"))
            {
                return OsVersionEnum.Linux_NA;
            }
            return osVersion;
        }
    }
}
