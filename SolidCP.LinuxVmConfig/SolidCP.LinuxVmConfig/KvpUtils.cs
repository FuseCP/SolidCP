using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.LinuxVmConfig
{
    public class KvpUtils
    {
        internal const int KVP_MAX_KEY_SIZE = 512;
        internal const int KVP_MAX_VALUE_SIZE = 2048;

        public static string[] GetKvpKeys(string pool)
        {
            try
            {
                List<string> lKeys = new List<string>();
                byte[] bKey = new byte[KVP_MAX_KEY_SIZE];
                byte[] bValue = new byte[KVP_MAX_VALUE_SIZE];
                byte[] res;
                using (FileStream fs = File.Open(pool, FileMode.Open))
                {
                    long remaining = fs.Length;
                    while (remaining > 0)
                    {
                        for (int i = 0; i < KVP_MAX_KEY_SIZE; i++) bKey[i] = 0;
                        int read = fs.Read(bKey, 0, KVP_MAX_KEY_SIZE);
                        remaining -= read;
                        if (remaining <= 0) break;
                        read = fs.Read(bValue, 0, KVP_MAX_VALUE_SIZE);
                        remaining -= read;
                        int idx = -1;
                        res = bKey;
                        for (int i = 0; i < bKey.Length; i++)
                        {
                            if (bKey[i] == 0)
                            {
                                idx = i;
                                break;
                            }
                        }
                        if (idx != -1)
                        {
                            res = new byte[idx];
                            Array.Copy(bKey, 0, res, 0, idx);
                        }
                        string sKey = System.Text.Encoding.UTF8.GetString(res);
                        sKey = sKey.Replace("\0", "", StringComparison.Ordinal);
                        lKeys.Add(sKey);
                    }
                }
                return lKeys.ToArray();
            }
            catch (Exception ex)
            {
                ServiceLog.WriteError("GetKvpKeys error: ", ex);
                return null;
            }
        }

        public static string GetKvpStringValue(string pool, string key)
        {
            try
            {
                string sValue = null;
                byte[] bKey = new byte[KVP_MAX_KEY_SIZE];
                byte[] bValue = new byte[KVP_MAX_VALUE_SIZE];
                byte[] res;
                int idx;
                using (FileStream fs = File.Open(pool, FileMode.Open))
                {
                    long remaining = fs.Length;
                    while (remaining > 0)
                    {
                        for (int i = 0; i < KVP_MAX_KEY_SIZE; i++) bKey[i] = 0;
                        for (int i = 0; i < KVP_MAX_VALUE_SIZE; i++) bValue[i] = 0;
                        int read = fs.Read(bKey, 0, KVP_MAX_KEY_SIZE);
                        remaining -= read;
                        if (remaining <= 0) break;
                        read = fs.Read(bValue, 0, KVP_MAX_VALUE_SIZE);
                        remaining -= read;
                        res = bKey;
                        idx = -1;
                        for (int i = 0; i < bKey.Length; i++)
                        {
                            if (bKey[i] == 0)
                            {
                                idx = i;
                                break;
                            }
                        }
                        if (idx != -1)
                        {
                            res = new byte[idx];
                            Array.Copy(bKey, 0, res, 0, idx);
                        }
                        string sKey = System.Text.Encoding.UTF8.GetString(res);
                        sKey = sKey.Replace("\0", "", StringComparison.Ordinal);
                        if (sKey.Equals(key))
                        {
                            res = bValue;
                            idx = -1;
                            for (int i=0; i<bValue.Length; i++)
                            {
                                if (bValue[i]==0)
                                {
                                    idx = i;
                                    break;
                                }
                            }
                            if (idx != -1)
                            {
                                res = new byte[idx];
                                Array.Copy(bValue, 0, res, 0, idx);
                            }
                            sValue = System.Text.Encoding.UTF8.GetString(res);
                            sValue = sValue.Replace("\0", "", StringComparison.Ordinal);
                            break;
                        }
                    }
                }
                return sValue;
            }
            catch (Exception ex)
            {
                ServiceLog.WriteError("GetKvpStringValue error: ", ex);
                return null;
            }
        }

        private static void EditKvpValue(string pool, int offset, byte[] data)
        {
            try
            {
                byte[] fBytes = File.ReadAllBytes(pool);
                Array.Copy(data, 0, fBytes, offset, data.Length);
                File.WriteAllBytes(pool, fBytes);
            }
            catch (Exception ex)
            {
                ServiceLog.WriteError("EditKvpValue error: ", ex);
            }
        }

        private static void AddKvp(string pool, string key, string value)
        {
            try
            {
                int dataLength = KVP_MAX_KEY_SIZE + KVP_MAX_VALUE_SIZE;
                byte[] data = new byte[dataLength];
                byte[] temp = Encoding.UTF8.GetBytes(key);
                Array.Copy(temp, 0, data, 0, temp.Length);
                temp = Encoding.UTF8.GetBytes(value);
                Array.Copy(temp, 0, data, KVP_MAX_KEY_SIZE, temp.Length);
                using (FileStream fs = File.Open(pool, FileMode.Append, FileAccess.Write))
                {
                    fs.Write(data, 0, dataLength);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("AddKvp error:", ex);
            }
        }

        private static void DeleteKvp(string pool, int offset)
        {
            try
            {
                int delLength = KVP_MAX_KEY_SIZE + KVP_MAX_VALUE_SIZE;
                byte[] fBytes = File.ReadAllBytes(pool);
                byte[] resArray = new byte[fBytes.Length - delLength];
                if (offset > 0) Array.Copy(fBytes, 0, resArray, 0, offset);
                if (resArray.Length - offset > 0) Array.Copy(fBytes, offset + delLength, resArray, offset, resArray.Length - offset);
                File.WriteAllBytes(pool, resArray);
            }
            catch (Exception ex)
            {
                ServiceLog.WriteError("DeleteKvp error: ", ex);
            }
        }

        public static void SetKvpStringValue(String pool, String key, String value)
        {
            try
            {
                byte[] bKey = new byte[KVP_MAX_KEY_SIZE];
                byte[] bValue = new byte[KVP_MAX_VALUE_SIZE];

                byte[] data = new byte[KVP_MAX_VALUE_SIZE];
                byte[] temp = Encoding.UTF8.GetBytes(value);
                byte[] res;
                if (temp.Length > KVP_MAX_VALUE_SIZE) return;
                Array.Copy(temp, 0, data, 0, temp.Length);

                bool edit = false;
                long offset = 0;
                using (FileStream fs = File.Open(pool, FileMode.Open))
                {
                    long remaining = fs.Length;

                    while (remaining > 0)
                    {
                        for (int i = 0; i < KVP_MAX_KEY_SIZE; i++) bKey[i] = 0;
                        int read = fs.Read(bKey, 0, KVP_MAX_KEY_SIZE);
                        remaining -= read;
                        if (remaining <= 0) break;
                        int idx = -1;
                        res = bKey;
                        for (int i = 0; i < bKey.Length; i++)
                        {
                            if (bKey[i] == 0)
                            {
                                idx = i;
                                break;
                            }
                        }
                        if (idx != -1)
                        {
                            res = new byte[idx];
                            Array.Copy(bKey, 0, res, 0, idx);
                        }
                        string sKey = System.Text.Encoding.UTF8.GetString(res);
                        sKey = sKey.Replace("\0", "", StringComparison.Ordinal);
                        if (sKey.Equals(key))
                        {
                            offset = fs.Length - remaining;
                            edit = true;
                            break;
                        }
                        read = fs.Read(bValue, 0, KVP_MAX_VALUE_SIZE);
                        remaining -= read;
                    }
                }
                if (edit)
                {
                    EditKvpValue(pool, (int)offset, data);
                }
                else
                {
                    AddKvp(pool, key, value);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("SetKvpStringValue error:", ex);
            }
        }

        public static void DeleteKvpKey(string pool, string key)
        {
            try
            {
                byte[] bKey = new byte[KVP_MAX_KEY_SIZE];
                byte[] bValue = new byte[KVP_MAX_VALUE_SIZE];
                byte[] res;
                long offset = -1;
                using (FileStream fs = File.Open(pool, FileMode.Open))
                {
                    long remaining = fs.Length;
                    while (remaining > 0)
                    {
                        for (int i = 0; i < KVP_MAX_KEY_SIZE; i++) bKey[i] = 0;
                        int read = fs.Read(bKey, 0, KVP_MAX_KEY_SIZE);
                        remaining -= read;
                        if (remaining <= 0) break;
                        int idx = -1;
                        res = bKey;
                        for (int i = 0; i < bKey.Length; i++)
                        {
                            if (bKey[i] == 0)
                            {
                                idx = i;
                                break;
                            }
                        }
                        if (idx != -1)
                        {
                            res = new byte[idx];
                            Array.Copy(bKey, 0, res, 0, idx);
                        }
                        string sKey = System.Text.Encoding.UTF8.GetString(res);
                        sKey = sKey.Replace("\0", "", StringComparison.Ordinal);
                        if (sKey.Equals(key))
                        {
                            offset = fs.Length - remaining - KVP_MAX_KEY_SIZE;
                            break;
                        }
                        read = fs.Read(bValue, 0, KVP_MAX_VALUE_SIZE);
                        remaining -= read;
                    }
                }
                if (offset != -1) DeleteKvp(pool, (int)offset);
            }
            catch (Exception ex)
            {
                Log.WriteError("DeleteKvpKey error:", ex);
            }
        }
    }
}
