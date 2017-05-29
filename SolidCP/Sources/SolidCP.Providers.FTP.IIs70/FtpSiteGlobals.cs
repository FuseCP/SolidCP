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

namespace SolidCP.Providers.FTP.IIs70
{
	internal class FtpSiteGlobals
	{
		//
		public const int BindingProtocol =						0;
		//
		public const int SslCertificate_FriendlyName =			0;
		//
		public const int Authorization_Users =					1;
		//
		public const int BindingInformation =					1;
		//
		public const int SslCertificate_Hash =					1;
		//
		public const int Authorization_Roles =					2;
		//
		public const int SslCertificate_IssuedTo =				2;
		//
		public const int BindingIndex =							2;
		//
		public const int Authorization_Permission =				3;
		//
		public const int Site_Name =							100;
		//
		public const int Site_ID =								103;
		//
		public const int Site_SingleBinding =					104;
		//
		public const int Site_Bindings =						105;
		//
		public const int AppVirtualDirectory_PhysicalPath =		300;
		//
		public const int AppVirtualDirectory_UserName =			301;
		//
		public const int AppVirtualDirectory_Password =			302;
		//
		public const int AppVirtualDirectory_Password_Set =		303;
		//
		public const int FtpSite_AutoStart =					350;
		//
		public const int FtpSite_Status =						351;
		//
		public const int Connections_UnauthenticatedTimeout =	400;
		//
		public const int Connections_ControlChannelTimeout =	401;
		//
		public const int Connections_DisableSocketPooling =		402;
		//
		public const int Connections_ServerListenBacklog =		403;
		//
		public const int Connections_DataChannelTimeout =		404;
		//
		public const int Connections_MinBytesPerSecond =		405;
		//
		public const int Connections_MaxConnections =			406;
		//
		public const int Connections_ResetOnMaxConnection =		407;
		//
		public const int Ssl_ServerCertHash =					410;
		//
		public const int Ssl_ControlChannelPolicy =				411;
		//
		public const int Ssl_DataChannelPolicy =				412;
		//
		public const int Ssl_Ssl128 =							413;
		//
		public const int Authentication_AnonymousEnabled =		420;
		//
		public const int Authentication_BasicEnabled =			421;
		//
		public const int Authorization_Rule =					422;
		//
		public const string FtpServerElementName =	"ftpServer";
		//
		public const string SearchHostHeader =		"SearchHostHeader";
		//
		public const string SearchIPAddress =		"SearchIPAddress";
		//
		public const string SearchPhysicalPath =	"SearchPhysicalPath";
		//
		public const string SearchPort =			"SearchPort";
		//
		public const string SearchSiteName =		"SearchSiteName";
	}
}
