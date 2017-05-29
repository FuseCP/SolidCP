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
using System.Web;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for PanelRequest.
    /// </summary>
    public class PanelRequest
    {
        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        public static int GetInt(string key, int defaultValue)
        {
            int result = defaultValue;
            try { result = Int32.Parse(HttpContext.Current.Request[key]); }
            catch { /* do nothing */ }
            return result;
        }

        public static bool GetBool(string key)
        {
            return GetBool(key, false);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            bool result = defaultValue;
            try { result = bool.Parse(HttpContext.Current.Request[key]); }
            catch { /* do nothing */ }
            return result;
        }

        public static int UserID
        {
            get { return GetInt("UserID"); }
        }

        public static int AccountID
        {
            get { return GetInt("AccountID"); }
        }

        public static int PeerID
        {
            get { return GetInt("PeerID"); }
        }

        public static int ServerId
        {
            get { return GetInt("ServerID"); }
        }

        public static string PoolId
        {
            get { return HttpContext.Current.Request["PoolID"]; }
        }

        public static string DeviceId
        {
            get { return HttpContext.Current.Request["DeviceID"]; }
        }

        public static int ServiceId
        {
            get { return GetInt("ServiceID"); }
        }

        public static string TaskID
        {
            get { return HttpContext.Current.Request["TaskID"]; }
        }

        public static int GroupID
        {
            get { return GetInt("GroupID"); }
        }

        public static int AddressID
        {
            get { return GetInt("AddressID"); }
        }

        public static string Addresses
        {
            get { return HttpContext.Current.Request["Addresses"]; }
        }

        public static int ResourceID
        {
            get { return GetInt("ResourceID"); }
        }

        public static int PlanID
        {
            get { return GetInt("PlanID"); }
        }

        public static int AddonID
        {
            get { return GetInt("AddonID"); }
        }

        public static int PackageID
        {
            get { return GetInt("PackageID", -1); }
        }

        public static int PackageAddonID
        {
            get { return GetInt("PackageAddonID"); }
        }

        public static int ItemID
        {
            get { return GetInt("ItemID"); }
        }

        public static int RegistrationID
        {
            get { return GetInt("RegistrationID"); }
        }

        public static int DomainID
        {
            get { return GetInt("DomainID"); }
        }

        public static int InstallationID
        {
            get { return GetInt("InstallationID"); }
        }

        public static int ScheduleID
        {
            get { return GetInt("ScheduleID"); }
        }

        public static string RecordID
        {
            get { return HttpContext.Current.Request["RecordID"]; }
        }

        public static string InstanceID
        {
            get { return HttpContext.Current.Request["InstanceID"]; }
        }


        public static string VirtDir
        {
            get { return HttpContext.Current.Request["VirtDir"] != null ? HttpContext.Current.Request["VirtDir"].Trim().Replace("__DOT__", ".") : ""; }
        }

        public static string Path
        {
            get { return HttpContext.Current.Request["Path"] != null ? HttpContext.Current.Request["Path"] : ""; }
        }

        public static string ApplicationID
        {
            get { return HttpContext.Current.Request["ApplicationID"] != null ? HttpContext.Current.Request["ApplicationID"].Trim() : ""; }
        }

        public static string Name
        {
            get { return HttpContext.Current.Request["Name"] != null
                ? HttpContext.Current.Request["Name"].Trim() : ""; }
        }

        public static string Context
        {
            get { return HttpContext.Current.Request["Context"]; }
        }

        public static string FolderID
        {
            get { return HttpContext.Current.Request["FolderID"] ?? ""; }
        }

        public static string Ctl
        {
            get { return HttpContext.Current.Request["ctl"] ?? ""; }
        }

        public static int CollectionID
        {
            get { return GetInt("CollectionId"); }
        }

        public static int SsLevelId
        {
            get { return GetInt("SsLevelId"); }
        }

        public static int StorageSpaceId
        {
            get { return GetInt("StorageSpaceId"); }
        }
    }
}
