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
    public class SfBTransaction
    {
        #region Fields

        private readonly List<TransactionAction> actions;

        #endregion

        #region Properties

        public List<TransactionAction> Actions
        {
            get { return actions; }
        }

        #endregion

        #region Constructor

        public SfBTransaction()
        {
            actions = new List<TransactionAction>();
        }

        #endregion

        #region Methods

        public void RegisterNewSipDomain(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewSipDomain, Id = id });
        }

        public void RegisterNewSimpleUrl(string sipDomain, string tenantID)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewSimpleUrl, Id = sipDomain, Account = tenantID });
        }

        public void RegisterNewConferencingPolicy(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewConferencingPolicy, Id = id });
        }

        public void RegisterNewCsExternalAccessPolicy(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewExternalAccessPolicy, Id = id });
        }

        public void RegisterNewCsMobilityPolicy(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewMobilityPolicy, Id = id });
        }

        public void RegisterNewCsUser(string id)
        {
            Actions.Add(new TransactionAction { ActionType = TransactionAction.TransactionActionTypes.SfBNewUser, Id = id });
        }

        #endregion
    }
}
