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

﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Runtime.Serialization;

namespace SolidCP.Providers.WebAppGallery
{
    [Flags]
    public enum GalleryApplicationWellKnownDependency
    {
        None = 0,
        PHP = 1,
        AspNet20 = 2,
        AspNet40 = 4,
        SQL = 8,
        MySQL = 16
    }

	public class Author
	{
		[XmlElement(ElementName="name", Namespace="http://www.w3.org/2005/Atom")]
		public string Name { get; set; }
		[XmlElement(ElementName = "uri", Namespace = "http://www.w3.org/2005/Atom")]
		public string Uri { get; set; }
	}

	public class MsDeploy
	{
		[XmlElement(ElementName = "startPage", Namespace = "http://www.w3.org/2005/Atom")]
		public string StartPage { get; set; }
	}

	public class InstallerFile
	{
		[XmlElement(ElementName = "fileSize", Namespace = "http://www.w3.org/2005/Atom")]
		public string FileSize { get; set; }
		[XmlElement(ElementName = "installerURL", Namespace = "http://www.w3.org/2005/Atom")]
		public string InstallerUrl { get; set; }
		[XmlElement(ElementName = "displayURL", Namespace = "http://www.w3.org/2005/Atom")]
		public string DisplayUrl { get; set; }
		[XmlElement(ElementName = "md5", Namespace = "http://www.w3.org/2005/Atom")]
		public string MD5 { get; set; }
		[XmlElement(ElementName = "sha1", Namespace = "http://www.w3.org/2005/Atom")]
		public string SHA1 { get; set; }
	}

	public class Installer
	{
		[XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
		public string Id { get; set; }
		[XmlElement(ElementName = "languageId", Namespace = "http://www.w3.org/2005/Atom")]
		public string LanguageId { get; set; }
		[XmlElement(ElementName = "installerFile", Namespace = "http://www.w3.org/2005/Atom")]
		public InstallerFile InstallerFile { get; set; }
		[XmlElement(ElementName = "msDeploy", Namespace = "http://www.w3.org/2005/Atom")]
		public MsDeploy MsDeploy { get; set; }
	}

	public class InstallerItem
	{
		[XmlElement(ElementName = "installerFile", Namespace = "http://www.w3.org/2005/Atom")]
		public InstallerFile InstallerFile { get; set; }
		[XmlElement(ElementName = "installerItemId", Namespace = "http://www.w3.org/2005/Atom")]
		public string InstallerItemId { get; set; }
		[XmlElement(ElementName = "languageId", Namespace = "http://www.w3.org/2005/Atom")]
		public string LanguageId { get; set; }
		[XmlElement(ElementName = "helpLink", Namespace = "http://www.w3.org/2005/Atom")]
		public string HelpLink { get; set; }
		[XmlElement(ElementName = "msDeploy", Namespace = "http://www.w3.org/2005/Atom")]
		public MsDeploy MsDeploy { get; set; }
	}

	public class Dependency
	{
		[XmlElement(ElementName = "productId", Namespace = "http://www.w3.org/2005/Atom")]
		public string ProductId { get; set; }

        [XmlAttribute(AttributeName = "idref", Namespace = "http://www.w3.org/2005/Atom")]
        public string IdRef { get; set; }

		#region Version 0.2
		[XmlArray(ElementName = "logicalAnd", Namespace = "http://www.w3.org/2005/Atom"),
		XmlArrayItem(ElementName = "dependency")]
		public List<Dependency> LogicalAnd { get; set; }

		[XmlArray(ElementName = "logicalOr", Namespace = "http://www.w3.org/2005/Atom"),
		XmlArrayItem(ElementName = "dependency")]
		public List<Dependency> LogicalOr { get; set; }
		#endregion

		#region Version 2.0.1.0
		[XmlArray(ElementName = "and", Namespace = "http://www.w3.org/2005/Atom"),
		XmlArrayItem(ElementName = "dependency")]
		public List<Dependency> And { get; set; }

		[XmlArray(ElementName = "or", Namespace = "http://www.w3.org/2005/Atom"),
		XmlArrayItem(ElementName = "dependency")]
		public List<Dependency> Or { get; set; }

		#endregion
	}

	[XmlRoot("entry", Namespace = "http://www.w3.org/2005/Atom")]
    public class GalleryApplication
    {
		[XmlElement(ElementName = "productId", Namespace = "http://www.w3.org/2005/Atom")]
		public string Id { get; set; }

		[XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
        public string Title { get; set; }

		[XmlElement(ElementName = "version", Namespace = "http://www.w3.org/2005/Atom")]
        public string Version { get; set; }

		[XmlElement(ElementName = "summary", Namespace = "http://www.w3.org/2005/Atom")]
        public string Summary { get; set; }

		[XmlElement(ElementName = "longSummary", Namespace = "http://www.w3.org/2005/Atom")]
        public string Description { get; set; }

		public string Link { get; set; }

		[XmlElement(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
		public Author Author { get; set; }

		[XmlElement(ElementName = "productFamily", Namespace = "http://www.w3.org/2005/Atom")]
		public string ProductFamily { get; set; }

		[XmlArray(ElementName="keywords"), XmlArrayItem(ElementName="item")]
		public List<string> Keywords { get; set; }

		[XmlArray(ElementName = "installerItems"), XmlArrayItem(ElementName = "installerItem")]
		public List<InstallerItem> InstallerItems { get; set; }

		[XmlArray(ElementName = "installers"), XmlArrayItem(ElementName = "installer")]
		public List<Installer> Installers { get; set; }

		[XmlElement(ElementName = "dependency", Namespace = "http://www.w3.org/2005/Atom")]
		public Dependency Dependency { get; set; }

        public GalleryApplicationWellKnownDependency WellKnownDependencies { get; set; }

		public DateTime LastUpdated { get; set; }

		public DateTime Published { get; set; }

		[XmlIgnore]
        public string DownloadUrl
		{
			get
			{
				if (InstallerItems.Count > 0)
					return InstallerItems[0].InstallerFile.InstallerUrl;
				else if (Installers.Count > 0)
					return Installers[0].InstallerFile.InstallerUrl;
				else
					return "N/A";
			}
		}

        public string IconUrl { get; set; }

		[XmlIgnore]
		public string StartPage
		{
			get
			{
				if (InstallerItems.Count > 0)
					return InstallerItems[0].MsDeploy.StartPage;
				else if (Installers.Count > 0)
					return Installers[0].MsDeploy.StartPage;
				else
					return String.Empty;
			}
		}

        [XmlIgnore]
        public string Size
		{
			get
			{
				if (InstallerItems.Count > 0)
					return InstallerItems[0].InstallerFile.FileSize;
				else if (Installers.Count > 0)
					return Installers[0].InstallerFile.FileSize;
				else
					return "0";
			}
		}

        [XmlElement(ElementName = "installerFileSize", Namespace = "http://www.w3.org/2005/Atom")]
        public int InstallerFileSize { get; set; }


		public string AuthorName {
			get { return Author.Name; }
		}

		public string AuthorUrl {
			get { return Author.Uri; }
		}

		[XmlElement("pageName", Namespace = "http://www.w3.org/2005/Atom")]
		public string PageName { get; set; }
    }

	public enum GalleryWebAppStatus
	{
		NotDownloaded,
		Downloaded,
		Downloading,
        UnauthorizedAccessException,
		Failed
	}

	public enum UseDatabase
	{
		None,
		Sql,
		MySql
	}
}
