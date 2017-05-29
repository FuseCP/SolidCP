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
using System.Text;

namespace SolidCP.EnterpriseServer
{
    public class ApplicationInfo
    {
        private string categoryName;

        private string id;
        private string folder;
        private string codebase;
        private string settingsControl;

        private string name;
        private string shortDescription;
        private string fullDescription;
        private string version;
        private string logo;
        private int size;
        private string homeSite;
        private string supportSite;
        private string docsSite;
        private string manufacturer;
        private string license;
        private ApplicationRequirement[] requirements;
        private ApplicationWebSetting[] webSettings;

        private int installationsNumber;

        public ApplicationInfo()
        {
        }

        public string HomeSite
        {
            get { return this.homeSite; }
            set { this.homeSite = value; }
        }

        public string SupportSite
        {
            get { return this.supportSite; }
            set { this.supportSite = value; }
        }

        public string ShortDescription
        {
            get { return this.shortDescription; }
            set { this.shortDescription = value; }
        }

        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        public string CategoryName
        {
            get { return this.categoryName; }
            set { this.categoryName = value; }
        }

        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public string DocsSite
        {
            get { return this.docsSite; }
            set { this.docsSite = value; }
        }

        public string FullDescription
        {
            get { return this.fullDescription; }
            set { this.fullDescription = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Codebase
        {
            get { return this.codebase; }
            set { this.codebase = value; }
        }

        public string Logo
        {
            get { return this.logo; }
            set { this.logo = value; }
        }

        public int InstallationsNumber
        {
            get { return this.installationsNumber; }
            set { this.installationsNumber = value; }
        }

        public string SettingsControl
        {
            get { return this.settingsControl; }
            set { this.settingsControl = value; }
        }

        public string Folder
        {
            get { return this.folder; }
            set { this.folder = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Manufacturer
        {
            get { return this.manufacturer; }
            set { this.manufacturer = value; }
        }

        public string License
        {
            get { return this.license; }
            set { this.license = value; }
        }

        public ApplicationWebSetting[] WebSettings
        {
            get { return this.webSettings; }
            set { this.webSettings = value; }
        }

        public ApplicationRequirement[] Requirements
        {
            get { return this.requirements; }
            set { this.requirements = value; }
        }
    }

    public class ApplicationRequirement
    {
        private string[] groups;
        private string[] quotas;
        private bool display;

        public string[] Groups
        {
            get { return this.groups; }
            set { this.groups = value; }
        }

        public string[] Quotas
        {
            get { return this.quotas; }
            set { this.quotas = value; }
        }

        public bool Display
        {
            get { return this.display; }
            set { this.display = value; }
        }
    }

    public class ApplicationWebSetting
    {
        private string name;
        private string value;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }

    public class ApplicationCategory
    {
        private string id;
        private string name;
        private string[] applications;

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

        public string[] Applications
        {
            get { return this.applications; }
            set { this.applications = value; }
        }
    }
}
