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
using System.Data;
using System.Text;
using System.Globalization;
using System.Web;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for PanelFormatter.
    /// </summary>
    public class PanelFormatter
    {
		public static string GetYesNo(bool flag)
		{
			return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "YesNo." + flag.ToString());
		}

		public static string GetLocalizedResourceGroupName(string groupName)
		{
			return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, String.Format("ResourceGroup.{0}", groupName));
		}

        public static string GetDisplaySizeInBytes(long size)
        {
            if (size >= 0x400 && size < 0x100000)
                // kilobytes
                return Convert.ToString((int)Math.Round(((float)size / 1024))) + "K";
            else if (size >= 0x100000 && size < 0x40000000)
                // megabytes
                return Convert.ToString((int)Math.Round(((float)size / 1024 / 1024))) + "M";
            else if (size >= 0x40000000 && size < 0x10000000000)
                // gigabytes
                return Convert.ToString((int)Math.Round(((float)size / 1024 / 1024 / 1024))) + "G";
            else
                return size.ToString();
        }

        public static string GetUserRoleName(int roleId)
        {
            string roleKey = ((UserRole)roleId).ToString();
            return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "PanelRole." + roleKey);
        }

        public static string GetAccountStatusName(int statusId)
        {
            string statusKey = ((UserStatus)statusId).ToString();
			return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "AccountStatus." + statusKey);
        }

        public static string GetPackageStatusName(int statusId)
        {
            string statusKey = ((PackageStatus)statusId).ToString();
			return PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "PackageStatus." + statusKey);
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("d");
        }

        public static DateTime ParseDate(string strDate, DateTime defValue)
        {
            try
            {
                return DateTime.Parse(strDate);
            }
            catch
            {
                return defValue;
            }
        }

        public static string FormatMoney(decimal val)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencySymbol = "";

            return val.ToString("C", nfi);
        }

        public static decimal ParseMoney(string val)
        {
            return ParseMoney(val, 0);
        }

        public static decimal ParseMoney(string val, decimal defaultValue)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            decimal result = defaultValue;
            try { result = Decimal.Parse(val, nfi); }
            catch { /* do nothing */ }
            return result;
        }
    }
}
