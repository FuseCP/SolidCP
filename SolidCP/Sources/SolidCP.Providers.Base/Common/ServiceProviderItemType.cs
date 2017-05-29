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

namespace SolidCP.Providers
{
    public class ServiceProviderItemType
    {
        private int itemTypeId;
        private int groupId;
        private string displayName;
        private string typeName;
        private int typeOrder;
        private bool calculateDiskspace;
        private bool calculateBandwidth;
        private bool suspendable;
        private bool disposable;
        private bool searchable;
        private bool importable;
        private bool backupable;

        public int ItemTypeId
        {
            get { return itemTypeId; }
            set { itemTypeId = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public int TypeOrder
        {
            get { return typeOrder; }
            set { typeOrder = value; }
        }

        public bool CalculateDiskspace
        {
            get { return calculateDiskspace; }
            set { calculateDiskspace = value; }
        }

        public bool CalculateBandwidth
        {
            get { return calculateBandwidth; }
            set { calculateBandwidth = value; }
        }

        public bool Suspendable
        {
            get { return suspendable; }
            set { suspendable = value; }
        }

        public bool Disposable
        {
            get { return disposable; }
            set { disposable = value; }
        }

        public bool Searchable
        {
            get { return searchable; }
            set { searchable = value; }
        }

        public bool Importable
        {
            get { return importable; }
            set { importable = value; }
        }

        public bool Backupable
        {
            get { return backupable; }
            set { backupable = value; }
        }
    }
}
