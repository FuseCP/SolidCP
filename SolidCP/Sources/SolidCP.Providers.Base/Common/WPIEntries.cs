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
using System.Text;

namespace SolidCP.Server
{
    [Serializable]
    public class WPIProduct
    {
        private string productId;
        private string logo;
        private string summary;
        private string title;
        private string link;
        private string version;
        private string eulaUrl;
        private string downloadedLocation;
        private string longDescription;
        private bool isInstalled;
        private bool isUpgrade;
        private int fileSize;
        private DateTime published;
        private string author;
        private string authorUri;


        public WPIProduct()
        {
        }

        public string Logo
        {
            get { return this.logo; }
            set { this.logo = value; }
        }

        public string ProductId
        {
            get { return this.productId; }
            set { this.productId = value; }
        }


        public string Summary
        {
            get { return this.summary; }
            set { this.summary = value; }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Link
        {
            get { return this.link; }
            set { this.link = value; }
        }

        public bool IsInstalled
        {
            get { return this.isInstalled; }
            set { this.isInstalled = value; }
        }

        public bool IsUpgrade
        {
            get { return this.isUpgrade; }
            set { this.isUpgrade = value; }
        }

        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public string EulaUrl
        {
            get { return this.eulaUrl; }
            set { this.eulaUrl = value; }
        }

        public string DownloadedLocation
        {
            get { return this.downloadedLocation; }
            set { this.downloadedLocation = value; }
        }

        public int FileSize
        {
            get { return this.fileSize; }
            set { this.fileSize = value; }
        }

        public string LongDescription
        {
            get { return this.longDescription; }
            set { this.longDescription = value; }
        }

        public DateTime Published
        {
            get { return this.published; }
            set { this.published = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public string AuthorUri
        {
            get { return authorUri; }
            set { authorUri = value; }
        }

        public new string ToString()
        {
            return productId;
        }
    }

    public class WPITab
    {
        private string description;
        private bool fromCustomFeed;
        private string id;
        private string name;

        public WPITab()
        {

        }

        public WPITab(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public bool FromCustomFeed
        {
            get { return this.fromCustomFeed; }
            set { this.fromCustomFeed = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }

    public class WPIKeyword
    {
        public const string HOSTING_PACKAGE_KEYWORD = "ZooPackage";

        private string id;
        private string text;

        public WPIKeyword()
        {

        }

        public WPIKeyword(string id, string text)
        {
            this.id = id;
            this.text = text;
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
    }
}
