﻿// Copyright (c) 2016, SolidCP
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
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.Win32;
//using MySql.Data.MySqlClient;
using System.IO;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using SolidCP.Providers;
using System.Reflection;

namespace SolidCP.Providers.Database {
    public class MySqlServer57 : MySqlServer {

        public MySqlServer57() {

        }

        public override bool IsInstalled() {
            if (Server.Utils.OS.IsWindows)
            {

                string versionNumber = null;

                RegistryKey HKLM = Registry.LocalMachine;

                RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\MySQL AB\MySQL Server 5.7");

                if (key != null)
                {
                    versionNumber = (string)key.GetValue("Version");
                }
                else
                {
                    key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\MySQL AB\MySQL Server 5.7");
                    if (key != null)
                    {
                        versionNumber = (string)key.GetValue("Version");
                    }
                    else
                    {
                        return false;
                    }
                }

                string[] split = versionNumber.Split(new char[] { '.' });

                return split[0].Equals("5") & split[1].Equals("7");
            }
            else if (Server.Utils.OS.IsUnix)
            {
                if (Shell.Default.Find("mysql") == null) return false;

                var version = Shell.Default.ExecAsync("mysql -version").Output().Result;

                return version.Contains("Ver 5.7.");
            }
            else return false;
		}
	}
}
