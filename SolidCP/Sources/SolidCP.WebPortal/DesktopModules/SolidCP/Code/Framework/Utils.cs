// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

using SolidCP.EnterpriseServer;
using SolidCP.Portal.SkinControls;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for Utils.
    /// </summary
    public static class Utils
    {
        public const string ModuleName = "SolidCP";

        public const Int32 MAX_DIR_LENGTH = 248;
        public const Int32 MAX_FILE_LENGTH = 260;

        public const int CHANGE_PASSWORD_REDIRECT_TIMEOUT = 7000;

        public static DateTime ParseDate(string val)
        {
            // Perf: allow only non-empty strings to go through
            if (!String.IsNullOrEmpty(val))
            {
                try
                {
                    return DateTime.Parse(val);
                }
                catch
                {
                    /* do nothing */
                }
            }
            return DateTime.MinValue;
        }

        public static int ParseInt(string val)
        {
            return ParseInt(val, 0);
        }

        public static int ParseInt(object val, int defaultValue)
        {
            int result = defaultValue;
            if (val != null && !String.IsNullOrEmpty(val.ToString()))
            {
                try
                {
                    result = Int32.Parse(val.ToString());
                }
                catch
                {
                    /* do nothing */
                }
            }
            return result;
        }

        public static bool ParseBool(string val, bool defaultValue)
        {
            bool result = defaultValue;
            // Perf: allow only non-empty strings to go through
            if (!String.IsNullOrEmpty(val))
            {
                try
                {
                    result = Boolean.Parse(val);
                }
                catch
                {
                    /* do nothing */
                }
            }

            return result;
        }

        public static decimal ParseDecimal(string val, decimal defaultValue)
        {
            decimal result = defaultValue;
            // Perf: allow only non-empty strings to go through
            if (!String.IsNullOrEmpty(val))
            {
                try
                {
                    result = Decimal.Parse(val);
                }
                catch
                {
                    /* do nothing */
                }
            }
            return result;
        }

        public static string FormatDateTime(DateTime dt)
        {
            return dt == DateTime.MinValue ? "" : dt.ToString();
        }

        public static string FixRelativePath(string path)
        {
            string reversed = path.Replace("/", "\\");
            return Regex.Replace(reversed, @"\.\\|\.\.|\\\\|\?|\:|\""|\<|\>|\||%|\$", "");
        }

        public static String NormalizeString(String inString, Boolean pathSymbolsForbidden, Char substitute, Int32 maxLength)
        {
            inString = Regex.Replace(inString, "[^\\w\\s.-]+", substitute.ToString());

            if ((inString).Length > maxLength)
                inString = inString.Remove(maxLength, inString.Length - maxLength);

            return inString;
        }

        public static string[] ParseDelimitedString(string str, params char[] delimiter)
        {
            string[] parts = str.Split(delimiter);
            ArrayList list = new ArrayList();
            foreach (string part in parts)
                if (part.Trim() != "" && !list.Contains(part.Trim()))
                    list.Add(part);
            return (string[]) list.ToArray(typeof (string));
        }

        public static string ReplaceStringVariable(string str, string variable, string value)
        {
            Regex re = new Regex("\\[" + variable + "\\]+", RegexOptions.IgnoreCase);
            return re.Replace(str, value);
        }

        public static byte[] ConvertStreamToBytes(Stream stream)
        {
            long length = stream.Length;
            byte[] content = new byte[length];
            stream.Read(content, 0, (int) length);
            stream.Close();
            return content;
        }


        /// <summary>
        /// Builds list of items from supplied group string.
        /// </summary>
        /// <param name="group">Group string.</param>
        /// <returns>List of items.</returns>
        public static List<KeyValuePair<string, string>> ParseGroup(string group)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
            string[] vals = group.Split(';');
            foreach (string v in vals)
            {
                string itemValue = v;
                string itemText = v;

                int eqIdx = v.IndexOf("=");
                if (eqIdx != -1)
                {
                    itemValue = v.Substring(0, eqIdx);
                    itemText = v.Substring(eqIdx + 1);
                }

                items.Add(new KeyValuePair<string, string>(itemText, itemValue));
            }
            return items;
        }


        public static void SelectListItem(ListControl ctrl, object value)
        {
            string val = (value != null) ? value.ToString() : "";
            ListItem item = ctrl.Items.FindByValue(val);
            if (item != null)
            {
                // unselect currently selected item
                if (ctrl.SelectedIndex != -1)
                    ctrl.SelectedItem.Selected = false;

                item.Selected = true;
            }
        }

        public static void SelectListItem(BootstrapDropDownList ctrl, object value)
        {
            string val = (value != null) ? value.ToString() : "";
            ListItem item = ctrl.Items.FindByValue(val);
            if (item != null)
            {
                // unselect currently selected item
                if (ctrl.SelectedIndex != -1)
                    ctrl.SelectedItem.Selected = false;

                item.Selected = true;
            }
        }

        public static void SaveListControlState(ListControl ctrl)
        {
            HttpResponse response = HttpContext.Current.Response;

            // build cookie value
            ArrayList selValues = new ArrayList();
            foreach (ListItem item in ctrl.Items)
            {
                if (item.Selected)
                    selValues.Add(item.Value);
            }

            string cookieVal = String.Join(",", (string[]) selValues.ToArray(typeof (string)));

            // create cookie
            HttpCookie cookie = new HttpCookie(ctrl.UniqueID, cookieVal);
            response.Cookies.Add(cookie);
        }

        public static void LoadListControlState(ListControl ctrl)
        {
            HttpRequest request = HttpContext.Current.Request;

            // get cookie
            HttpCookie cookie = request.Cookies[ctrl.UniqueID];
            if (cookie == null)
                return;

            // reset all items
            foreach (ListItem item in ctrl.Items)
                item.Selected = false;

            string[] vals = cookie.Value.Split(new char[] {','});
            foreach (string val in vals)
            {
                ListItem item = ctrl.Items.FindByValue(val);
                if (item != null) item.Selected = true;
            }
        }

        public static string EllipsisString(string str, int maxLen)
        {
            if (String.IsNullOrEmpty(str) || str.Length <= maxLen)
                return str;

            return str.Substring(0, maxLen) + "...";
        }

        public static string GetRandomString(int length)
        {
            string ptrn = "abcdefghjklmnpqrstwxyz0123456789";
            StringBuilder sb = new StringBuilder();

            byte[] randomBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                       randomBytes[1] << 16 |
                       randomBytes[2] << 8 |
                       randomBytes[3];


            Random rnd = new Random(seed);

            for (int i = 0; i < length; i++)
                sb.Append(ptrn[rnd.Next(ptrn.Length - 1)]);

            return sb.ToString();
        }

        public static bool CheckQouta(string key, PackageContext cntx)
        {
            return cntx.Quotas.ContainsKey(key) &&
                   ((cntx.Quotas[key].QuotaAllocatedValue == 1 && cntx.Quotas[key].QuotaTypeId == 1) ||
                    (cntx.Quotas[key].QuotaTypeId != 1 && (cntx.Quotas[key].QuotaAllocatedValue > 0 || cntx.Quotas[key].QuotaAllocatedValue == -1)));
        }


        public static bool CheckQouta(string key, HostingPlanContext cntx)
        {
            return cntx.Quotas.ContainsKey(key) &&
                   ((cntx.Quotas[key].QuotaAllocatedValue == 1 && cntx.Quotas[key].QuotaTypeId == 1) ||
                    (cntx.Quotas[key].QuotaTypeId != 1 && (cntx.Quotas[key].QuotaAllocatedValue > 0 || cntx.Quotas[key].QuotaAllocatedValue == -1)));
        }

        public static bool IsIdnDomain(string domainName)
        {
            if (string.IsNullOrEmpty(domainName))
            {
                return false;
            }

            var idn = new IdnMapping();
            return idn.GetAscii(domainName) != domainName;
        }

        public static string UnicodeToAscii(string domainName)
        {
            if (string.IsNullOrEmpty(domainName))
            {
                return string.Empty;
            }

            var idn = new IdnMapping();
            return idn.GetAscii(domainName);
        }

        public static List<T> GetCheckboxValuesFromGrid<T>(GridView gridView, string checkboxName)
        {
            // Get checked users
            var userIds = new List<T>();

            foreach (GridViewRow gvr in gridView.Rows)
            {
                if (((CheckBox)gvr.FindControl(checkboxName)).Checked)
                {
                    string userId = gridView.DataKeys[gvr.DataItemIndex % gridView.PageSize].Value.ToString();
                    userIds.Add((T)Convert.ChangeType(userId, typeof(T)));
                }
            }

            return userIds;
        }
    }
}
