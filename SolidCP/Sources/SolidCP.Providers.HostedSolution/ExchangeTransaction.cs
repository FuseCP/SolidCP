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

using System.Collections.Generic;

namespace SolidCP.Providers.HostedSolution
{
    public class ExchangeTransaction
    {
        List<TransactionAction> actions = null;

        public ExchangeTransaction()
        {
            actions = new List<TransactionAction>();
        }

        public List<TransactionAction> Actions
        {
            get { return actions; }
        }

        public void RegisterNewOrganizationUnit(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateOrganizationUnit;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewDistributionGroup(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateDistributionGroup;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterMailEnabledDistributionGroup(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.EnableDistributionGroup;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewGlobalAddressList(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateGlobalAddressList;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewAddressList(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateAddressList;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewAddressBookPolicy(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateAddressBookPolicy;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterNewRoomsAddressList(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateAddressList;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterNewOfflineAddressBook(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateOfflineAddressBook;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewActiveSyncPolicy(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateActiveSyncPolicy;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterNewAcceptedDomain(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateAcceptedDomain;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewUPNSuffix(string id, string suffix)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.AddUPNSuffix;
            action.Id = id;
            action.Suffix = suffix;
            Actions.Add(action);
        }

        public void RegisterNewMailbox(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateMailbox;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterEnableMailbox(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.EnableMailbox;
            action.Id = id;
            Actions.Add(action);
        }


        public void RegisterNewContact(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreateContact;
            action.Id = id;
            Actions.Add(action);
        }

        public void RegisterNewPublicFolder(string mailbox, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreatePublicFolder;
            action.Id = id;
            action.Account = mailbox;
            Actions.Add(action);
        }


        public void RegisterNewPublicFolderMailbox(string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.CreatePublicFolderMailbox;
            action.Id = id;
            Actions.Add(action);
        }

        public void ResetMailboxOnBehalfPermissions(string id, string[] accounts)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.ResetMailboxOnBehalfPermissions;
            action.Accounts = accounts;
            action.Id = id;
            Actions.Add(action);
        }

        public void AddMailBoxFullAccessPermission(string accountName, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.AddMailboxFullAccessPermission;
            action.Id = id;
            action.Account = accountName;
            Actions.Add(action);
        }

        public void AddSendAsPermission(string accountName, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.AddSendAsPermission;
            action.Id = id;
            action.Account = accountName;
            Actions.Add(action);
        }

        public void RemoveMailboxFullAccessPermission(string accountName, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.RemoveMailboxFullAccessPermission;
            action.Id = id;
            action.Account = accountName;
            Actions.Add(action);
        }

        public void RemoveSendAsPermission(string accountName, string id)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.RemoveSendAsPermission;
            action.Id = id;
            action.Account = accountName;
            Actions.Add(action);
        }

        public void RemoveMailboxFolderPermissions(string folderPath, ExchangeAccount account)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.RemoveMailboxFolderPermissions;
            action.Id = folderPath;
            action.ExchangeAccount = account;
            Actions.Add(action);
        }

        public void AddMailboxFolderPermission(string folderPath, ExchangeAccount account)
        {
            TransactionAction action = new TransactionAction();
            action.ActionType = TransactionAction.TransactionActionTypes.AddMailboxFolderPermission;
            action.Id = folderPath;
            action.ExchangeAccount = account;
            Actions.Add(action);
        }
    }
}
