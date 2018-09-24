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
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Filters;

namespace SolidCP.EnterpriseServer
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "SpamExpertsSoap", Namespace = "http://smbsaas/solidcp/server/")]
    public partial class esSpamExperts : WebServicesClientProtocol
    {

        public ServiceProviderSettingsSoapHeader ServiceProviderSettingsSoapHeaderValue;

        private System.Threading.SendOrPostCallback AddDomainFilterOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteDomainFilterOperationCompleted;

        private System.Threading.SendOrPostCallback AddEmailFilterOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteEmailFilterOperationCompleted;

        private System.Threading.SendOrPostCallback SetEmailFilterUserPasswordOperationCompleted;

        private System.Threading.SendOrPostCallback SetDomainFilterUserPasswordOperationCompleted;

        private System.Threading.SendOrPostCallback SetDomainFilterDestinationsOperationCompleted;

        private System.Threading.SendOrPostCallback SetDomainFilterUserOperationCompleted;

        private System.Threading.SendOrPostCallback AddDomainFilterAliasOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteDomainFilterAliasOperationCompleted;

        private System.Threading.SendOrPostCallback IsSpamExpertsEnabledOperationCompleted;

        /// <remarks/>
        public esSpamExperts()
        {
            this.Url = "http://localhost/EnterpriseServer/SpamExperts.asmx";
        }

        /// <remarks/>
        public event AddDomainFilterCompletedEventHandler AddDomainFilterCompleted;

        /// <remarks/>
        public event DeleteDomainFilterCompletedEventHandler DeleteDomainFilterCompleted;

        /// <remarks/>
        public event AddEmailFilterCompletedEventHandler AddEmailFilterCompleted;

        /// <remarks/>
        public event DeleteEmailFilterCompletedEventHandler DeleteEmailFilterCompleted;

        /// <remarks/>
        public event SetEmailFilterUserPasswordCompletedEventHandler SetEmailFilterUserPasswordCompleted;

        /// <remarks/>
        public event SetDomainFilterUserPasswordCompletedEventHandler SetDomainFilterUserPasswordCompleted;

        /// <remarks/>
        public event SetDomainFilterDestinationsCompletedEventHandler SetDomainFilterDestinationsCompleted;

        /// <remarks/>
        public event SetDomainFilterUserCompletedEventHandler SetDomainFilterUserCompleted;

        /// <remarks/>
        public event AddDomainFilterAliasCompletedEventHandler AddDomainFilterAliasCompleted;

        /// <remarks/>
        public event DeleteDomainFilterAliasCompletedEventHandler DeleteDomainFilterAliasCompleted;

        /// <remarks/>
        public event IsSpamExpertsEnabledCompletedEventHandler IsSpamExpertsEnabledCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/AddDomainFilter", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            object[] results = this.Invoke("AddDomainFilter", new object[] {
                        domain,
                        password,
                        email,
                        destinations});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddDomainFilter(string domain, string password, string email, string[] destinations, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddDomainFilter", new object[] {
                        domain,
                        password,
                        email,
                        destinations}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndAddDomainFilter(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void AddDomainFilterAsync(string domain, string password, string email, string[] destinations)
        {
            this.AddDomainFilterAsync(domain, password, email, destinations, null);
        }

        /// <remarks/>
        public void AddDomainFilterAsync(string domain, string password, string email, string[] destinations, object userState)
        {
            if ((this.AddDomainFilterOperationCompleted == null))
            {
                this.AddDomainFilterOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddDomainFilterOperationCompleted);
            }
            this.InvokeAsync("AddDomainFilter", new object[] {
                        domain,
                        password,
                        email,
                        destinations}, this.AddDomainFilterOperationCompleted, userState);
        }

        private void OnAddDomainFilterOperationCompleted(object arg)
        {
            if ((this.AddDomainFilterCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddDomainFilterCompleted(this, new AddDomainFilterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/DeleteDomainFilter", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult DeleteDomainFilter(string domain)
        {
            object[] results = this.Invoke("DeleteDomainFilter", new object[] {
                        domain});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteDomainFilter(string domain, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteDomainFilter", new object[] {
                        domain}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndDeleteDomainFilter(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void DeleteDomainFilterAsync(string domain)
        {
            this.DeleteDomainFilterAsync(domain, null);
        }

        /// <remarks/>
        public void DeleteDomainFilterAsync(string domain, object userState)
        {
            if ((this.DeleteDomainFilterOperationCompleted == null))
            {
                this.DeleteDomainFilterOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteDomainFilterOperationCompleted);
            }
            this.InvokeAsync("DeleteDomainFilter", new object[] {
                        domain}, this.DeleteDomainFilterOperationCompleted, userState);
        }

        private void OnDeleteDomainFilterOperationCompleted(object arg)
        {
            if ((this.DeleteDomainFilterCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteDomainFilterCompleted(this, new DeleteDomainFilterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/AddEmailFilter", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            object[] results = this.Invoke("AddEmailFilter", new object[] {
                        name,
                        domain,
                        password});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddEmailFilter(string name, string domain, string password, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddEmailFilter", new object[] {
                        name,
                        domain,
                        password}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndAddEmailFilter(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void AddEmailFilterAsync(string name, string domain, string password)
        {
            this.AddEmailFilterAsync(name, domain, password, null);
        }

        /// <remarks/>
        public void AddEmailFilterAsync(string name, string domain, string password, object userState)
        {
            if ((this.AddEmailFilterOperationCompleted == null))
            {
                this.AddEmailFilterOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddEmailFilterOperationCompleted);
            }
            this.InvokeAsync("AddEmailFilter", new object[] {
                        name,
                        domain,
                        password}, this.AddEmailFilterOperationCompleted, userState);
        }

        private void OnAddEmailFilterOperationCompleted(object arg)
        {
            if ((this.AddEmailFilterCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddEmailFilterCompleted(this, new AddEmailFilterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/DeleteEmailFilter", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult DeleteEmailFilter(string email)
        {
            object[] results = this.Invoke("DeleteEmailFilter", new object[] {
                        email});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteEmailFilter(string email, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteEmailFilter", new object[] {
                        email}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndDeleteEmailFilter(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void DeleteEmailFilterAsync(string email)
        {
            this.DeleteEmailFilterAsync(email, null);
        }

        /// <remarks/>
        public void DeleteEmailFilterAsync(string email, object userState)
        {
            if ((this.DeleteEmailFilterOperationCompleted == null))
            {
                this.DeleteEmailFilterOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteEmailFilterOperationCompleted);
            }
            this.InvokeAsync("DeleteEmailFilter", new object[] {
                        email}, this.DeleteEmailFilterOperationCompleted, userState);
        }

        private void OnDeleteEmailFilterOperationCompleted(object arg)
        {
            if ((this.DeleteEmailFilterCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteEmailFilterCompleted(this, new DeleteEmailFilterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/SetEmailFilterUserPassword", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            object[] results = this.Invoke("SetEmailFilterUserPassword", new object[] {
                        email,
                        password});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetEmailFilterUserPassword(string email, string password, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetEmailFilterUserPassword", new object[] {
                        email,
                        password}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndSetEmailFilterUserPassword(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void SetEmailFilterUserPasswordAsync(string email, string password)
        {
            this.SetEmailFilterUserPasswordAsync(email, password, null);
        }

        /// <remarks/>
        public void SetEmailFilterUserPasswordAsync(string email, string password, object userState)
        {
            if ((this.SetEmailFilterUserPasswordOperationCompleted == null))
            {
                this.SetEmailFilterUserPasswordOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetEmailFilterUserPasswordOperationCompleted);
            }
            this.InvokeAsync("SetEmailFilterUserPassword", new object[] {
                        email,
                        password}, this.SetEmailFilterUserPasswordOperationCompleted, userState);
        }

        private void OnSetEmailFilterUserPasswordOperationCompleted(object arg)
        {
            if ((this.SetEmailFilterUserPasswordCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetEmailFilterUserPasswordCompleted(this, new SetEmailFilterUserPasswordCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/SetDomainFilterUserPassword", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            object[] results = this.Invoke("SetDomainFilterUserPassword", new object[] {
                        name,
                        password});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetDomainFilterUserPassword(string name, string password, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetDomainFilterUserPassword", new object[] {
                        name,
                        password}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndSetDomainFilterUserPassword(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void SetDomainFilterUserPasswordAsync(string name, string password)
        {
            this.SetDomainFilterUserPasswordAsync(name, password, null);
        }

        /// <remarks/>
        public void SetDomainFilterUserPasswordAsync(string name, string password, object userState)
        {
            if ((this.SetDomainFilterUserPasswordOperationCompleted == null))
            {
                this.SetDomainFilterUserPasswordOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetDomainFilterUserPasswordOperationCompleted);
            }
            this.InvokeAsync("SetDomainFilterUserPassword", new object[] {
                        name,
                        password}, this.SetDomainFilterUserPasswordOperationCompleted, userState);
        }

        private void OnSetDomainFilterUserPasswordOperationCompleted(object arg)
        {
            if ((this.SetDomainFilterUserPasswordCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetDomainFilterUserPasswordCompleted(this, new SetDomainFilterUserPasswordCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/SetDomainFilterDestinations", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations)
        {
            object[] results = this.Invoke("SetDomainFilterDestinations", new object[] {
                        name,
                        destinations});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetDomainFilterDestinations(string name, string[] destinations, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetDomainFilterDestinations", new object[] {
                        name,
                        destinations}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndSetDomainFilterDestinations(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void SetDomainFilterDestinationsAsync(string name, string[] destinations)
        {
            this.SetDomainFilterDestinationsAsync(name, destinations, null);
        }

        /// <remarks/>
        public void SetDomainFilterDestinationsAsync(string name, string[] destinations, object userState)
        {
            if ((this.SetDomainFilterDestinationsOperationCompleted == null))
            {
                this.SetDomainFilterDestinationsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetDomainFilterDestinationsOperationCompleted);
            }
            this.InvokeAsync("SetDomainFilterDestinations", new object[] {
                        name,
                        destinations}, this.SetDomainFilterDestinationsOperationCompleted, userState);
        }

        private void OnSetDomainFilterDestinationsOperationCompleted(object arg)
        {
            if ((this.SetDomainFilterDestinationsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetDomainFilterDestinationsCompleted(this, new SetDomainFilterDestinationsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/SetDomainFilterUser", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            object[] results = this.Invoke("SetDomainFilterUser", new object[] {
                        domain,
                        password,
                        email});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetDomainFilterUser(string domain, string password, string email, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetDomainFilterUser", new object[] {
                        domain,
                        password,
                        email}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndSetDomainFilterUser(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void SetDomainFilterUserAsync(string domain, string password, string email)
        {
            this.SetDomainFilterUserAsync(domain, password, email, null);
        }

        /// <remarks/>
        public void SetDomainFilterUserAsync(string domain, string password, string email, object userState)
        {
            if ((this.SetDomainFilterUserOperationCompleted == null))
            {
                this.SetDomainFilterUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetDomainFilterUserOperationCompleted);
            }
            this.InvokeAsync("SetDomainFilterUser", new object[] {
                        domain,
                        password,
                        email}, this.SetDomainFilterUserOperationCompleted, userState);
        }

        private void OnSetDomainFilterUserOperationCompleted(object arg)
        {
            if ((this.SetDomainFilterUserCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetDomainFilterUserCompleted(this, new SetDomainFilterUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/AddDomainFilterAlias", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult AddDomainFilterAlias(string domain, string alias)
        {
            object[] results = this.Invoke("AddDomainFilterAlias", new object[] {
                        domain,
                        alias});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddDomainFilterAlias(string domain, string alias, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddDomainFilterAlias", new object[] {
                        domain,
                        alias}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndAddDomainFilterAlias(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void AddDomainFilterAliasAsync(string domain, string alias)
        {
            this.AddDomainFilterAliasAsync(domain, alias, null);
        }

        /// <remarks/>
        public void AddDomainFilterAliasAsync(string domain, string alias, object userState)
        {
            if ((this.AddDomainFilterAliasOperationCompleted == null))
            {
                this.AddDomainFilterAliasOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddDomainFilterAliasOperationCompleted);
            }
            this.InvokeAsync("AddDomainFilterAlias", new object[] {
                        domain,
                        alias}, this.AddDomainFilterAliasOperationCompleted, userState);
        }

        private void OnAddDomainFilterAliasOperationCompleted(object arg)
        {
            if ((this.AddDomainFilterAliasCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddDomainFilterAliasCompleted(this, new AddDomainFilterAliasCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/server/DeleteDomainFilterAlias", RequestNamespace = "http://smbsaas/solidcp/server/", ResponseNamespace = "http://smbsaas/solidcp/server/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias)
        {
            object[] results = this.Invoke("DeleteDomainFilterAlias", new object[] {
                        domain,
                        alias});
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteDomainFilterAlias(string domain, string alias, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteDomainFilterAlias", new object[] {
                        domain,
                        alias}, callback, asyncState);
        }

        /// <remarks/>
        public SpamExpertsResult EndDeleteDomainFilterAlias(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SpamExpertsResult)(results[0]));
        }

        /// <remarks/>
        public void DeleteDomainFilterAliasAsync(string domain, string alias)
        {
            this.DeleteDomainFilterAliasAsync(domain, alias, null);
        }

        /// <remarks/>
        public void DeleteDomainFilterAliasAsync(string domain, string alias, object userState)
        {
            if ((this.DeleteDomainFilterAliasOperationCompleted == null))
            {
                this.DeleteDomainFilterAliasOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteDomainFilterAliasOperationCompleted);
            }
            this.InvokeAsync("DeleteDomainFilterAlias", new object[] {
                        domain,
                        alias}, this.DeleteDomainFilterAliasOperationCompleted, userState);
        }

        private void OnDeleteDomainFilterAliasOperationCompleted(object arg)
        {
            if ((this.DeleteDomainFilterAliasCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteDomainFilterAliasCompleted(this, new DeleteDomainFilterAliasCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceProviderSettingsSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smbsaas/solidcp/enterpriseserver/IsSpamExpertsEnabled", RequestNamespace = "http://smbsaas/solidcp/enterpriseserver", ResponseNamespace = "http://smbsaas/solidcp/enterpriseserver", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool IsSpamExpertsEnabled(int packageId)
        {
            object[] results = this.Invoke("IsSpamExpertsEnabled", new object[] {
                        packageId});
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginIsSpamExpertsEnabled(int packageId, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("IsSpamExpertsEnabled", new object[] {
                        packageId}, callback, asyncState);
        }

        /// <remarks/>
        public bool EndIsSpamExpertsEnabled(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public void IsSpamExpertsEnabledAsync(int packageId)
        {
            this.IsSpamExpertsEnabledAsync(packageId, null);
        }

        /// <remarks/>
        public void IsSpamExpertsEnabledAsync(int packageId, object userState)
        {
            if ((this.IsSpamExpertsEnabledOperationCompleted == null))
            {
                this.IsSpamExpertsEnabledOperationCompleted = new System.Threading.SendOrPostCallback(this.OnIsSpamExpertsEnabledOperationCompleted);
            }
            this.InvokeAsync("IsSpamExpertsEnabled", new object[] {
                        packageId}, this.IsSpamExpertsEnabledOperationCompleted, userState);
        }

        private void OnIsSpamExpertsEnabledOperationCompleted(object arg)
        {
            if ((this.IsSpamExpertsEnabledCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.IsSpamExpertsEnabledCompleted(this, new IsSpamExpertsEnabledCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void AddDomainFilterCompletedEventHandler(object sender, AddDomainFilterCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddDomainFilterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AddDomainFilterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DeleteDomainFilterCompletedEventHandler(object sender, DeleteDomainFilterCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteDomainFilterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteDomainFilterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void AddEmailFilterCompletedEventHandler(object sender, AddEmailFilterCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddEmailFilterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AddEmailFilterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DeleteEmailFilterCompletedEventHandler(object sender, DeleteEmailFilterCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteEmailFilterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteEmailFilterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SetEmailFilterUserPasswordCompletedEventHandler(object sender, SetEmailFilterUserPasswordCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetEmailFilterUserPasswordCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetEmailFilterUserPasswordCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SetDomainFilterUserPasswordCompletedEventHandler(object sender, SetDomainFilterUserPasswordCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetDomainFilterUserPasswordCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetDomainFilterUserPasswordCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SetDomainFilterDestinationsCompletedEventHandler(object sender, SetDomainFilterDestinationsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetDomainFilterDestinationsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetDomainFilterDestinationsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SetDomainFilterUserCompletedEventHandler(object sender, SetDomainFilterUserCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetDomainFilterUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetDomainFilterUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void AddDomainFilterAliasCompletedEventHandler(object sender, AddDomainFilterAliasCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddDomainFilterAliasCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AddDomainFilterAliasCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DeleteDomainFilterAliasCompletedEventHandler(object sender, DeleteDomainFilterAliasCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteDomainFilterAliasCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteDomainFilterAliasCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SpamExpertsResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SpamExpertsResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void IsSpamExpertsEnabledCompletedEventHandler(object sender, IsSpamExpertsEnabledCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IsSpamExpertsEnabledCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal IsSpamExpertsEnabledCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public bool Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}
