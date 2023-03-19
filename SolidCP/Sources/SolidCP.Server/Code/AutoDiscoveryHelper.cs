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
using System.IO;
//using System.Configuration;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Server.Utils;
using System.Reflection;

namespace SolidCP.Server.Code
{
    public class AutoDiscoveryHelper
    {
        public const string DisableAutoDiscovery = "DisableAutoDiscovery";
        public static BoolResult IsInstalled(string name)
        {
            
            Log.WriteStart("IsInstalled started. Name:{0}", name);
            BoolResult res = new BoolResult {IsSuccess = true};

            try
            {
                bool disableAutoDiscovery;

                if (!bool.TryParse(System.Configuration.ConfigurationManager.AppSettings[DisableAutoDiscovery], out disableAutoDiscovery))
                    disableAutoDiscovery = false;
                
                if (disableAutoDiscovery)
                {
                    res.Value = true;
                    
                }
                else
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        res.IsSuccess = false;
                        res.ErrorCodes.Add(ErrorCodes.PROVIDER_NANE_IS_NOT_SPECIFIED);
                        return res;
                    }

                    Type providerType = Type.GetType(name);
                    IHostingServiceProvider provider = (IHostingServiceProvider)Activator.CreateInstance(providerType);
                    res.Value = provider.IsInstalled();
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorCodes.Add(ErrorCodes.CANNOT_CREATE_PROVIDER_INSTANCE);
                Log.WriteError(ex);
            }
            finally
            {
                Log.WriteEnd("IsInstalled ended. Name:{0}", name);
            }

            return res;
        }

        public static string GetServerFilePath() {
#if NETFRAMEWORK
            return System.Web.HttpContext.Current.Server.MapPath("~/");
#else
            return new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..")).FullName;
#endif
        }

        public static string GetServerVersion()
        {
            object[] attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true);
            if (attrs.Length > 0)
                return ((AssemblyFileVersionAttribute)attrs[0]).Version;
            else
			    return typeof(AutoDiscoveryHelper).Assembly.GetName().Version.ToString(3);
        }

    }
}
