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
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Web;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;

namespace SolidCP.Providers.HeliconZoo
{
    [Serializable]
    public class HeliconZooEngine //: ServiceProviderItem
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string fullPath { get; set; }
        public string arguments { get; set; }
        public string transport { get; set; }
        public string protocol { get; set; }
        public string host { get; set; }
        public long portLower { get; set; }
        public long portUpper { get; set; }
        public long maxInstances { get; set; }
        public long minInstances { get; set; }
        public long timeLimit { get; set; }
        public long gracefulShutdownTimeout { get; set; }
        public long memoryLimit { get; set; }

        public HeliconZooEnv[] environmentVariables;

        public bool isUserEngine { get; set; }
        public bool disabled { get; set; }
    }

    [Serializable]
    public class HeliconZooEnv
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class ShortHeliconZooEngine
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }

        public string KeywordedName
        {
            get
            {
                // python.2.7.wsgi -> python27wsgi
                return Name.Replace(".", "");
            }
        }
    }

    public interface IHeliconZooServer
    {
        HeliconZooEngine[] GetEngines();
        void SetEngines(HeliconZooEngine[] userEngines);
        bool IsEnginesEnabled();
        void SwithEnginesEnabled(bool enabled);
        string[] GetEnabledEnginesForSite(string siteId);
        void SetEnabledEnginesForSite(string siteId, string[] engineNames);
        bool IsWebCosoleEnabled();
        void SetWebCosoleEnabled(bool enabled);
    }
}
