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
using SolidCP.Providers.Web;
namespace SolidCP.Providers.OS
{
    /// <summary>
    /// Summary description for FileSystemItem.
    /// </summary>
    [Serializable]
    public class SystemFile : ServiceProviderItem
    {
        private string fullName;
        private DateTime created;
        private DateTime changed;
        private bool isDirectory;
        private long size;
        private long quota;
        private bool isEmpty;
        private bool isPublished;
        private WebDavFolderRule[] rules;
        private string url;
        private int fsrmQuotaMB;
        private int frsmQuotaGB;
        private QuotaType fsrmQuotaType = QuotaType.Soft;
        private string driveLetter;

        public SystemFile()
        {
        }

        public SystemFile(string name, string fullName, bool isDirectory, long size,
            DateTime created, DateTime changed)
        {
            this.Name = name;
            this.fullName = fullName;
            this.isDirectory = isDirectory;
            this.size = size;
            this.created = created;
            this.changed = changed;
        }

        public int FRSMQuotaMB
        {
            get { return fsrmQuotaMB; }
            set { fsrmQuotaMB = value; }
        }

        public int FRSMQuotaGB
        {
            get { return frsmQuotaGB; }
            set { frsmQuotaGB = value; }
        }

        public QuotaType FsrmQuotaType
        {
            get { return fsrmQuotaType; }
            set { fsrmQuotaType = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public DateTime Changed
        {
            get { return changed; }
            set { changed = value; }
        }

        public bool IsDirectory
        {
            get { return isDirectory; }
            set { isDirectory = value; }
        }

        public long Size
        {
            get { return size; }
            set { size = value; }
        }

        public long Quota
        {
            get { return quota; }
            set { quota = value; }
        }

        public bool IsEmpty
        {
            get { return this.isEmpty; }
            set { this.isEmpty = value; }
        }

        public bool IsPublished
        {
            get { return this.isPublished; }
            set { this.isPublished = value; }
        }

        public WebDavFolderRule[] Rules
        {
            get { return this.rules; }
            set { this.rules = value; }
        }

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        public string RelativeUrl { get; set; }
        public string Summary { get; set; }
        public int? StorageSpaceFolderId { get; set; }
        public string UncPath { get; set; }

        public string DriveLetter
        {
            get { return this.driveLetter; }
            set { this.driveLetter = value; }
        }
    }
}
