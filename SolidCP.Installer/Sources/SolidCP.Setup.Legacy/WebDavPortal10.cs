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
using System.Windows.Forms;
using SolidCP.Setup.Actions;

namespace SolidCP.Setup
{
    /// <summary>
    /// Release 1.5.0
    /// </summary>
    public class WebDavPortal150 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.4.9,1.4.8,1.4.7,1.4.6,1.4.5",
                updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.5
    /// </summary>
    public class WebDavPortal145 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.4.8,1.4.7,1.4.6,1.4.5,1.4.4",
                updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.4
    /// </summary>
    public class WebDavPortal144 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.4.3,1.4.2",
                updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.3
    /// </summary>
    public class WebDavPortal143 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.4.2,1.4.1",
                updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.2
    /// </summary>
    public class WebDavPortal142 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.4.1,1.4.0",
                updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.1
    /// </summary>
    public class WebDavPortal141 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.4.0,1.3.0",
                updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.4.0
    /// </summary>
    public class WebDavPortal140 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.3.0,1.2.1",
                updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.3.0
    /// </summary>
    public class WebDavPortal130 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.2.1,1.2.0",
                updateSql: false);
        }
    }
    /// <summary>
    /// Release 1.2.1
    /// </summary>
    public class WebDavPortal121 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.2.0,1.1.1",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.2.0
    /// </summary>
    public class WebDavPortal120 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            //
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.4,1.1.2",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.4
    /// </summary>
    public class WebDavPortal114 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.3,1.1.2",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.3
    /// </summary>
    public class WebDavPortal113 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.2,1.1.1",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.2
    /// </summary>
    public class WebDavPortal112 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.1,1.1.0",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.1
    /// </summary>
    public class WebDavPortal111 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.1.0,1.0.4",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.1.0
    /// </summary>
    public class WebDavPortal110 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.4,1.0.3",
                updateSql: false);
        }
    }

    /// Release 1.0.4
    /// </summary>
    public class WebDavPortal104 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.3,1.0.0",
                updateSql: false);
        }
    }

    /// Release 1.0.3
    /// </summary>
    public class WebDavPortal103 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.2,1.0.0",
                updateSql: false);
        }
    }

    /// Release 1.0.2
    /// </summary>
    public class WebDavPortal102 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.1,1.0.0",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.0.1
    /// </summary>
    public class WebDavPortal101 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, minimalInstallerVersion: "1.0.1");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }

        public static new object Update(object obj)
        {
            return WebDavPortal.UpdateBase(obj,
                minimalInstallerVersion: "1.0.1",
                versionToUpgrade: "1.0.0",
                updateSql: false);
        }
    }

    /// <summary>
    /// Release 1.0
    /// </summary>
    public class WebDavPortal10 : WebDavPortal
    {
        public static new object Install(object obj)
        {
            return WebDavPortal.InstallBase(obj, "1.0.0");
        }

        public static new object Uninstall(object obj)
        {
            return WebDavPortal.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return WebDavPortal.Setup(obj);
        }
    }
}
