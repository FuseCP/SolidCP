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

namespace SolidCP.Providers.Web.Iis.Common
{
    using Microsoft.Web.Administration;
    using System;
    using System.Reflection;

    internal sealed class IsapiCgiRestrictionCollection : ConfigurationElementCollectionBase<IsapiCgiRestrictionElement>
    {
        public IsapiCgiRestrictionElement Add(string path, bool allowed)
        {
            IsapiCgiRestrictionElement element = base.CreateElement();
            element.Path = path;
            element.Allowed = allowed;
            return base.Add(element);
        }

        protected override IsapiCgiRestrictionElement CreateNewElement(string elementTagName)
        {
            return new IsapiCgiRestrictionElement();
        }

        public void Remove(string path)
        {
            base.Remove(this[path]);
        }

        public new IsapiCgiRestrictionElement this[string path]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    IsapiCgiRestrictionElement element = base[i];
                    if (string.Equals(Environment.ExpandEnvironmentVariables(element.Path), Environment.ExpandEnvironmentVariables(path), StringComparison.OrdinalIgnoreCase))
                    {
                        return element;
                    }
                }
                return null;
            }
        }
    }
}

