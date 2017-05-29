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
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;




namespace SolidCP.Providers.Statistics
{
    class SmarterStats5 : SmarterStats
    {
        public override bool IsInstalled()
        {
            string productName = null, productVersion = null;
            
            // Check x86 platform
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key != null)
            {
                var names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = key.OpenSubKey(s);
                    //
                    if (subkey == null)
                        continue;
                    //
                    productName = subkey.GetValue("DisplayName") as String;
                    //
                    if (String.IsNullOrEmpty(productName))
                        continue;

                    if (productName.Equals("SmarterStats")
                        || productName.Equals("SmarterStats Service"))
                    {
                        productVersion = subkey.GetValue("DisplayVersion") as String;
                        goto Version_Match;
                    }
                }
            }

            // Check x64 platform
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key != null)
            {
                var names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = key.OpenSubKey(s);
                    //
                    if (subkey == null)
                        continue;
                    //
                    productName = subkey.GetValue("DisplayName") as String;
                    //
                    if (String.IsNullOrEmpty(productName))
                        continue;

                    if (productName.Equals("SmarterStats") 
                        || productName.Equals("SmarterStats Service"))
                    {
                        productVersion = subkey.GetValue("DisplayVersion") as String;
                        goto Version_Match;
                    }
                }
            }
    
    Version_Match:
            //
            if (String.IsNullOrEmpty(productVersion))
                return false;
				
			// Match SmarterStats 5.x or newer versions
			int version = 0;
			string[] split = productVersion.Split(new[] { '.' });

			if (int.TryParse(split[0], out version))
			{
				if(version >= 5)
					return true;
			}
			//
			
            return false;
        }
    }
}

