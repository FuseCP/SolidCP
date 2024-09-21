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
using System.Data;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal
{
    public class OrganizationsHelper
    {
        #region Organizations
        DataSet orgs;

        public int GetOrganizationsPagedCount(int packageId,
            bool recursive, string filterColumn, string filterValue)
        {
            return (int)orgs.Tables[0].Rows[0][0];
        }

        public DataTable GetOrganizationsPaged(int packageId,
            bool recursive, string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";

            orgs = ES.Services.Organizations.GetRawOrganizationsPaged(packageId,
                recursive, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);
            
            return orgs.Tables[1];
        }

        //public Organization[] GetOrganizations(int packageId, bool recursive)
        //{
        //    return ES.Services.Organizations.GetOrganizations(packageId, recursive);
        //}

        public DataTable GetOrganizations(int packageId, bool recursive)
        {
            orgs = ES.Services.Organizations.GetRawOrganizationsPaged(packageId,
                recursive, "ItemName", "%", "ItemName", 0, int.MaxValue);

            return orgs.Tables[1];
        }
        #endregion

        #region Accounts
        OrganizationUsersPaged users;

        public int GetOrganizationUsersPagedCount(int itemId, 
            string filterColumn, string filterValue)
        {
            return users.RecordsCount;            
        }

        public OrganizationUser[] GetOrganizationUsersPaged(int itemId, 
            string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";
			if (maximumRows == 0)
			{
				maximumRows = Int32.MaxValue;
			}

            users = ES.Services.Organizations.GetOrganizationUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return users.PageUsers;            
        }

        OrganizationDeletedUsersPaged deletedUsers;

        public int GetOrganizationDeletedUsersPagedCount(int itemId,
            string filterColumn, string filterValue)
        {
            return deletedUsers.RecordsCount;
        }

        public OrganizationDeletedUser[] GetOrganizationDeletedUsersPaged(int itemId,
            string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";
            if (maximumRows == 0)
            {
                maximumRows = Int32.MaxValue;
            }

            deletedUsers = ES.Services.Organizations.GetOrganizationDeletedUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return deletedUsers.PageDeletedUsers;
        }

        #endregion

        #region Security Groups

        ExchangeAccountsPaged accounts;

        public int GetOrganizationSecurityGroupsPagedCount(int itemId, string accountTypes,
            string filterColumn, string filterValue)
        {
            return accounts.RecordsCount;
        }

        public ExchangeAccount[] GetOrganizationSecurityGroupsPaged(int itemId, string accountTypes,
            string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";

            accounts = ES.Services.Organizations.GetOrganizationSecurityGroupsPaged(itemId,
                filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return accounts.PageItems;
        }

        #endregion
    }
}
