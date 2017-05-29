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

namespace SolidCP.Providers.Web.Iis.Extensions
{
    using Handlers;
    using Common;
    using Microsoft.Web.Administration;
    using System.Collections.Generic;

    internal sealed class ExtensionsModuleService : ConfigurationModuleService
    {
        public SettingPair[] GetExtensionsInstalled(ServerManager srvman)
        {
            var settings = new List<SettingPair>();
			var config = srvman.GetApplicationHostConfiguration();

            var handlersSection = (HandlersSection) config.GetSection(Constants.HandlersSection, typeof (HandlersSection));

            var executalbesToLookFor = new[]
            {
                // Perl
                new KeyValuePair<string, string>(Constants.PerlPathSetting, "\\perl.exe"),
                // Php
                new KeyValuePair<string, string>(Constants.Php4PathSetting, "\\php.exe"),
                new KeyValuePair<string, string>(Constants.PhpPathSetting, "\\php-cgi.exe"),
                // Classic ASP
                new KeyValuePair<string, string>(Constants.AspPathSetting, @"\inetsrv\asp.dll"),
                // ASP.NET
                new KeyValuePair<string, string>(Constants.AspNet11PathSetting, @"\Framework\v1.1.4322\aspnet_isapi.dll"),
                new KeyValuePair<string, string>(Constants.AspNet20PathSetting, @"\Framework\v2.0.50727\aspnet_isapi.dll"),
                new KeyValuePair<string, string>(Constants.AspNet40PathSetting, @"\Framework\v4.0.30319\aspnet_isapi.dll"),
                // ASP.NET x64
                new KeyValuePair<string, string>(Constants.AspNet20x64PathSetting, @"\Framework64\v2.0.50727\aspnet_isapi.dll"),
                new KeyValuePair<string, string>(Constants.AspNet40x64PathSetting, @"\Framework64\v4.0.30319\aspnet_isapi.dll"),
            };

            foreach (var handler in handlersSection.Handlers)
            {
                foreach (var valuePair in executalbesToLookFor)
                {
                    var key = valuePair.Key;
                    if (handler.ScriptProcessor.EndsWith(valuePair.Value) && !settings.Exists(s => s.Name == key))
                    {
						settings.Add(new SettingPair{Name = valuePair.Key, Value = handler.ScriptProcessor});
                    }
                }
            }

            return settings.ToArray();
        }
    }
}
