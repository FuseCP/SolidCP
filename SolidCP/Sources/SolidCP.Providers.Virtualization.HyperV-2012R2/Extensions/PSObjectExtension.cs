using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    static class PSObjectExtension
    {
        #region Properties

        public static object GetProperty(this PSObject obj, string name)
        {
            return obj.Members[name].Value;
        }
        public static T GetProperty<T>(this PSObject obj, string name)
        {
            return (T)obj.Members[name].Value;
        }
        public static T GetEnum<T>(this PSObject obj, string name, T? defaultValue = null) where T : struct
        {
            try
            {
                return (T) Enum.Parse(typeof (T), GetProperty(obj, name).ToString());
            }
            catch
            {
                if (defaultValue.HasValue) return defaultValue.Value;
                throw;
            }
        }
        public static int GetInt(this PSObject obj, string name)
        {
            return Convert.ToInt32(obj.Members[name].Value);
        }
        public static long GetLong(this PSObject obj, string name)
        {
            return Convert.ToInt64(obj.Members[name].Value);
        }
        public static string GetString(this PSObject obj, string name)
        {
            return obj.Members[name].Value == null ? "" : obj.Members[name].Value.ToString();
        }
        public static bool GetBool(this PSObject obj, string name)
        {
            return Convert.ToBoolean(obj.Members[name].Value);
        }

        public static string GetMb(this PSObject obj, string name)
        {
            var bytes = GetLong(obj, name);

            if (bytes == 0)
                return "0";

            if (bytes > Constants.Size1G)
                return string.Format("{0:0.0} GB", bytes / Constants.Size1G);

            if (bytes > Constants.Size1M)
                return string.Format("{0:0.0} MB", bytes / Constants.Size1M);

            if (bytes > Constants.Size1K)
                return string.Format("{0:0.0} KB", bytes / Constants.Size1K);

            return string.Format("{0} b", bytes);
        }
        
        #endregion


        #region Methods

        public static ManagementObject Invoke(this PSObject obj, string name, object argument)
        {
            return obj.Invoke(name, new[] {argument});
        }
        public static ManagementObject Invoke(this PSObject obj, string name, params object[] arguments)
        {
            var results = (ManagementObjectCollection)obj.Methods[name].Invoke(arguments);

            foreach (var result in results)
            {
                return (ManagementObject) result;
            }
            return null;
        }

        #endregion
    }
}
