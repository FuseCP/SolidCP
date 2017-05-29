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
using System.Management;

namespace SolidCP.Providers.Utils
{
    /// <summary>
    /// Summary description for WmiHelper.
    /// </summary>
    public class WmiHelper
    {
        // namespace
        string ns = null;
        ManagementScope scope = null;

        public WmiHelper(string ns)
        {
            this.ns = ns;
        }

        public ManagementObjectCollection ExecuteQuery(string query)
        {
            ObjectQuery objectQuery = new ObjectQuery(query);

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(WmiScope, objectQuery);
            return searcher.Get();
        }

        // execute wmi query with parameters (taken from internal class Wmi used in Hyper-V provider)
        public ManagementObjectCollection ExecuteWmiQuery(string query, params object[] args)
        {
            if (args != null && args.Length > 0)
                query = String.Format(query, args);

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(WmiScope, new ObjectQuery(query));
            return searcher.Get();
        }

        //(taken from internal class Wmi used in Hyper-V provider)
        public ManagementObject GetWmiObject(string className)
        {
            return GetWmiObject(className, null);
        }

        //(taken from internal class Wmi used in Hyper-V provider)
        public ManagementObjectCollection GetWmiObjects(string className, string filter, params object[] args)
        {
            string query = "select * from " + className;
            if (!String.IsNullOrEmpty(filter))
                query += " where " + filter;
            return ExecuteWmiQuery(query, args);
        }

        //(taken from internal class Wmi used in Hyper-V provider)
        public ManagementObject GetWmiObject(string className, string filter, params object[] args)
        {
            ManagementObjectCollection col = GetWmiObjects(className, filter, args);
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = col.GetEnumerator();
            return enumerator.MoveNext() ? (ManagementObject)enumerator.Current : null;
        }

        public ManagementClass GetClass(string path)
        {
            return new ManagementClass(WmiScope, new ManagementPath(path), null);
        }

        public ManagementObject GetObject(string path)
        {
            return new ManagementObject(WmiScope, new ManagementPath(path), null);
        }

        private ManagementScope WmiScope
        {
            get
            {
                if (scope == null)
                {
                    ManagementPath path = new ManagementPath(ns);
                    scope = new ManagementScope(path, new ConnectionOptions());
                    scope.Connect();
                }
                return scope;
            }
        }

		public ManagementObject CreateInstance(string path)
		{
			ManagementClass objClass = GetClass(path);
			return objClass.CreateInstance();
		}
    }
}
