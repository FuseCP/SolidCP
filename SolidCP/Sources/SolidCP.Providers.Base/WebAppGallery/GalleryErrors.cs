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

namespace SolidCP.Providers.WebAppGallery
{
	public static class GalleryErrors
	{
        // SCP Server
        public const string ProcessingFeedXMLError = "ProcessingFeedXMLError"; // + exception
        public const string PackageFileNotFound = "PackageFileNotFound"; // + exception
        public const string ProcessingPackageError = "ProcessingPackageError"; // + exception
        public const string PackageInstallationError = "PackageInstallationError"; // + exception

        // application requirements
        public const string PackageDoesNotMeetRequirements = "PackageDoesNotMeetRequirements";
        public const string AspNet20Required = "AspNet20Required";
        public const string AspNet40Required = "AspNet40Required";
        public const string PhpRequired = "PhpRequired";
        public const string DatabaseRequired = "DatabaseRequired";
        public const string SQLRequired = "SQLRequired";
        public const string MySQLRequired = "MySQLRequired";
        public const string MariaDBRequired = "MariaDBRequired";

        // Common
        public const string MsDeployIsNotInstalled = "MsDeployIsNotInstalled";
        public const string GeneralError = "GeneralError"; // + exception message

        // Languages
        public const string GetLanguagesError = "GetLanguagesError";
        
        // Categories
        public const string GetCategoriesError = "GetCategoriesError";

        // Applications
        public const string GetApplicationsError = "GetApplicationsError";
        public const string GetApplicationError = "GetApplicationError";
        public const string GetApplicationParametersError = "GetApplicationParametersError";

        // Install app
        public const string WebApplicationNotFound = "WebApplicationNotFound"; // + app id
        public const string WebSiteNotFound = "WebSiteNotFound"; // + web site name
        public const string AppPathParameterNotFound = "AppPathParameterNotFound";
        public const string DatabaseServiceIsNotAvailable = "DatabaseServiceIsNotAvailable";
        public const string DatabaseServerExternalAddressIsEmpty = "DatabaseServerExternalAddressIsEmpty";
        public const string DatabaseAdminUsernameNotSpecified = "DatabaseAdminUsernameNotSpecified";
        public const string DatabaseAdminPasswordNotSpecified = "DatabaseAdminPasswordNotSpecified";
        public const string DatabaseCreationError = "DatabaseCreationError";
        public const string DatabaseCreationException = "DatabaseCreationException"; // + exception message
        public const string DatabaseUserCreationError = "DatabaseUserCreationError";
        public const string DatabaseUserCreationException = "DatabaseUserCreationException"; // + exception message
        public const string DatabaseUserCannotAccessDatabase = "DatabaseUserCannotAccessDatabase"; // + username
        public const string ApplicationInstallationError = "ApplicationInstallationError";
	}

}
