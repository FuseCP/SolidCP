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

using System.Text.RegularExpressions;
using SolidCP.Providers.OS;

namespace SolidCP.Portal
{
    public class EnterpriseStorageHelper
    {
        #region Folders

        public static bool ValidateFolderName(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z0-9-_. ]+$");
        }

        SystemFilesPaged folders;

        public int GetEnterpriseFoldersPagedCount(int itemId, string filterValue)
        {
            return folders.RecordsCount;
        }

        public SystemFile[] GetEnterpriseFoldersPaged(int itemId, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            filterValue = filterValue ?? string.Empty;

            folders = ES.Services.EnterpriseStorage.GetEnterpriseFoldersPaged(itemId, false, false, false, filterValue, sortColumn, startRowIndex, maximumRows);

            return folders.PageItems;
        }

        #endregion

        #region Drive Maps

        MappedDrivesPaged mappedDrives;

        public int GetEnterpriseDriveMapsPagedCount(int itemId, string filterValue)
        {
            return mappedDrives.RecordsCount;
        }

        public MappedDrive[] GetEnterpriseDriveMapsPaged(int itemId, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            filterValue = filterValue ?? string.Empty;

            mappedDrives = ES.Services.EnterpriseStorage.GetDriveMapsPaged(itemId,
                filterValue, sortColumn, startRowIndex, maximumRows);

            return mappedDrives.PageItems;
        }

        #endregion
    }
}
