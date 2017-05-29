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
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Services;
using System.Data;
using System.Xml.Linq;
using System.Diagnostics;

namespace SolidCP.WebSite.Services
{
    public class InstallerServiceBase
    {
        protected string RELEASES_FEED_PATH = "~/Data/ProductReleasesFeed.xml";

		#region WebMethods

		[WebMethod]
		public DataSet GetReleaseFileInfo(string componentCode, string version)
		{
			// get XML doc
			XDocument xml = GetReleasesFeed();

			// get current release
			var release = (from r in xml.Descendants("release")
						   where r.Parent.Parent.Attribute("code").Value == componentCode
						   && r.Attribute("version").Value == version
						   select r).FirstOrDefault();

			if (release == null)
				return null; // requested release was not found

			DataSet ds = new DataSet();
			DataTable dt = ds.Tables.Add();
			dt.Columns.Add("ReleaseFileID", typeof(int));
			dt.Columns.Add("FullFilePath", typeof(string));
			dt.Columns.Add("UpgradeFilePath", typeof(string));
			dt.Columns.Add("InstallerPath", typeof(string));
			dt.Columns.Add("InstallerType", typeof(string));

			dt.Rows.Add(
				Int32.Parse(release.Element("releaseFileID").Value),
				release.Element("fullFilePath").Value,
				release.Element("upgradeFilePath").Value,
				release.Element("installerPath").Value,
				release.Element("installerType").Value);

			ds.AcceptChanges(); // save
			return ds;
		}

		[WebMethod]
		public byte[] GetFileChunk(string fileName, int offset, int size)
		{
			string path = HttpContext.Current.Server.MapPath(fileName);
			return GetFileBinaryContent(path, offset, size);
		}

		[WebMethod]
		public long GetFileSize(string fileName)
		{
			string path = HttpContext.Current.Server.MapPath(fileName);
			long ret = 0;
			if (File.Exists(path))
			{
				FileInfo fi = new FileInfo(path);
				ret = fi.Length;
			}
			return ret;
		}

		[WebMethod]
		public DataSet GetAvailableComponents()
		{
			return GetAvailableComponents(false);
		}

		[WebMethod]
		public DataSet GetLatestComponentUpdate(string componentCode)
		{
			return GetLatestComponentUpdate(componentCode, false);
		}

		[WebMethod]
		public DataSet GetComponentUpdate(string componentCode, string release)
		{
			return GetComponentUpdate(componentCode, release, false);
		}

		#endregion

        public DataSet GetLatestComponentUpdate(string componentCode, bool includeBeta)
        {
            // get XML doc
            XDocument xml = GetReleasesFeed();

            // get all active component releases
            var releases = from release in xml.Descendants("release")
                           where release.Parent.Parent.Attribute("code").Value == componentCode
                           && release.Element("upgradeFilePath") != null
                           && release.Element("upgradeFilePath").Value != ""
						   // This line has been commented because the function is used only by SolidCP Installer
						   // itself. However, it may cause an incovenience if used inappropriately.
						   // The Installer's releases are hidden (not available) and should not be displayed in the list of available components.	
                           //&& Boolean.Parse(release.Attribute("available").Value)
                           && (includeBeta || !includeBeta && !Boolean.Parse(release.Attribute("beta").Value))
                           select release;

            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("ReleaseFileID", typeof(int));
            dt.Columns.Add("Version", typeof(string));
            dt.Columns.Add("Beta", typeof(bool));
            dt.Columns.Add("FullFilePath", typeof(string));
            dt.Columns.Add("UpgradeFilePath", typeof(string));
            dt.Columns.Add("InstallerPath", typeof(string));
            dt.Columns.Add("InstallerType", typeof(string));
			//
            var r = releases.FirstOrDefault();
			//
            if (r != null)
            {
                dt.Rows.Add(
                    Int32.Parse(r.Element("releaseFileID").Value),
                    r.Attribute("version").Value,
                    Boolean.Parse(r.Attribute("beta").Value),
                    r.Element("fullFilePath").Value,
                    r.Element("upgradeFilePath").Value,
                    r.Element("installerPath").Value,
                    r.Element("installerType").Value);
            }

            ds.AcceptChanges(); // save
            return ds;
        }

        public DataSet GetAvailableComponents(bool includeBeta)
        {
            XDocument xml = GetReleasesFeed();

            // select all available components
            var components = from component in xml.Descendants("component")
                             select component;

            // build dataset structure
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("ReleaseFileID", typeof(int));
            dt.Columns.Add("ApplicationName", typeof(string));
            dt.Columns.Add("Component", typeof(string));
            dt.Columns.Add("Version", typeof(string));
            dt.Columns.Add("Beta", typeof(bool));
            dt.Columns.Add("ComponentDescription", typeof(string));
            dt.Columns.Add("ComponentCode", typeof(string));
            dt.Columns.Add("ComponentName", typeof(string));
            dt.Columns.Add("FullFilePath", typeof(string));
            dt.Columns.Add("InstallerPath", typeof(string));
            dt.Columns.Add("InstallerType", typeof(string));

            // check each component for the latest available release
            foreach (var component in components)
            {
                var releases = from r in component.Descendants("release")
                               where Boolean.Parse(r.Attribute("available").Value)
                               && (includeBeta || !includeBeta && !Boolean.Parse(r.Attribute("beta").Value))
                               select r;

                var release = releases.FirstOrDefault();
                if (release == null)
                    continue; // component does not have active releases

                // add line to data set
                dt.Rows.Add(
                    Int32.Parse(release.Element("releaseFileID").Value),
                    component.Attribute("application").Value,
                    component.Attribute("application").Value + " " + component.Attribute("name").Value,
                    release.Attribute("version").Value,
                    Boolean.Parse(release.Attribute("beta").Value),
                    component.Element("description").Value,
                    component.Attribute("code").Value,
                    component.Attribute("name").Value,
                    release.Element("fullFilePath").Value,
                    release.Element("installerPath").Value,
                    release.Element("installerType").Value);
            }

            ds.AcceptChanges(); // save
            return ds;
        }

        public DataSet GetComponentUpdate(string componentCode, string release, bool includeBeta)
        {
            // get XML doc
            XDocument xml = GetReleasesFeed();

            // get current release
            var currentRelease = (from r in xml.Descendants("release")
                           where r.Parent.Parent.Attribute("code").Value == componentCode
                           && r.Attribute("version").Value == release
                           select r).FirstOrDefault();

            if(currentRelease == null)
                return null; // requested release was not found

            // get next available update
            var update = (from r in currentRelease.Parent.Descendants("release")
                          where r.IsBefore(currentRelease)
                          && r.Element("upgradeFilePath") != null
                          && r.Element("upgradeFilePath").Value != ""
                          && Boolean.Parse(r.Attribute("available").Value)
                          && (includeBeta || !includeBeta && !Boolean.Parse(r.Attribute("beta").Value))
                          select r).LastOrDefault();

            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("ReleaseFileID", typeof(int));
            dt.Columns.Add("Version", typeof(string));
            dt.Columns.Add("Beta", typeof(bool));
            dt.Columns.Add("FullFilePath", typeof(string));
            dt.Columns.Add("UpgradeFilePath", typeof(string));
            dt.Columns.Add("InstallerPath", typeof(string));
            dt.Columns.Add("InstallerType", typeof(string));

            if (update != null)
            {
                dt.Rows.Add(
                    Int32.Parse(update.Element("releaseFileID").Value),
                    update.Attribute("version").Value,
                    Boolean.Parse(update.Attribute("beta").Value),
                    update.Element("fullFilePath").Value,
                    update.Element("upgradeFilePath").Value,
                    update.Element("installerPath").Value,
                    update.Element("installerType").Value);
            }

            ds.AcceptChanges(); // save
            return ds;
        }

        private byte[] GetFileBinaryContent(string path)
        {
            if (!File.Exists(path))
                return null;

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (stream == null)
                return null;

            long length = stream.Length;
            byte[] content = new byte[length];
            stream.Read(content, 0, (int)length);
            stream.Close();
            return content;
        }

        private byte[] GetFileBinaryContent(string path, int offset, int size)
        {
            if (!File.Exists(path))
                return null;

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (stream == null)
                return null;

            long length = stream.Length;
            int count = size;
            if (offset + size - length > 0)
            {
                count = (int)(length - offset);
            }
            byte[] content = new byte[count];
            if (count > 0)
            {
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Read(content, 0, count);
                stream.Close();
            }
            return content;
        }

        private XDocument GetReleasesFeed()
        {
            return XDocument.Load(HttpContext.Current.Server.MapPath(RELEASES_FEED_PATH));
        }
    }
}
