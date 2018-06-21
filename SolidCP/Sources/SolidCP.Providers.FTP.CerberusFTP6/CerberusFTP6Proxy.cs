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

namespace SolidCP.Providers.FTP.CerberusFTP6Proxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "CerberusFTPServiceSoapBinding", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AuthenticatedRequest))]
    public partial class CerberusFTPService : Microsoft.Web.Services3.WebServicesClientProtocol
    {

        private System.Threading.SendOrPostCallback GetBackupServersOperationCompleted;

        private System.Threading.SendOrPostCallback SaveBackupServersOperationCompleted;

        private System.Threading.SendOrPostCallback SharePublicFileOperationCompleted;

        private System.Threading.SendOrPostCallback AddIpOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteIpOperationCompleted;

        private System.Threading.SendOrPostCallback TestAndVerifyDatabaseOperationCompleted;

        private System.Threading.SendOrPostCallback CreateStatisticsDatabaseOperationCompleted;

        private System.Threading.SendOrPostCallback DropStatisticsDatabaseOperationCompleted;

        private System.Threading.SendOrPostCallback GetMimeMappingsOperationCompleted;

        private System.Threading.SendOrPostCallback SaveMimeMappingsOperationCompleted;

        private System.Threading.SendOrPostCallback ServerSummaryStatusOperationCompleted;

        private System.Threading.SendOrPostCallback ServerInformationOperationCompleted;

        private System.Threading.SendOrPostCallback CurrentStatusOperationCompleted;

        private System.Threading.SendOrPostCallback StartServerOperationCompleted;

        private System.Threading.SendOrPostCallback StopServerOperationCompleted;

        private System.Threading.SendOrPostCallback ServerStartedOperationCompleted;

        private System.Threading.SendOrPostCallback InitializeServerOperationCompleted;

        private System.Threading.SendOrPostCallback ShutdownServerOperationCompleted;

        private System.Threading.SendOrPostCallback GetEventRulesOperationCompleted;

        private System.Threading.SendOrPostCallback SetEventRulesOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteRequestedAccountsOperationCompleted;

        private System.Threading.SendOrPostCallback GetRequestedAccountsOperationCompleted;

        private System.Threading.SendOrPostCallback SetRequestedAccountsOperationCompleted;

        private System.Threading.SendOrPostCallback GetAuthenticationListOperationCompleted;

        private System.Threading.SendOrPostCallback SetAuthenticationListOperationCompleted;

        private System.Threading.SendOrPostCallback GetHostnameOperationCompleted;

        private System.Threading.SendOrPostCallback SetWANIPOperationCompleted;

        private System.Threading.SendOrPostCallback AddUserOperationCompleted;

        private System.Threading.SendOrPostCallback AddGroupOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteUserOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteGroupOperationCompleted;

        private System.Threading.SendOrPostCallback AddRootOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteRootOperationCompleted;

        private System.Threading.SendOrPostCallback GetUserListOperationCompleted;

        private System.Threading.SendOrPostCallback GetGroupListOperationCompleted;

        private System.Threading.SendOrPostCallback GetUserInformationOperationCompleted;

        private System.Threading.SendOrPostCallback GetConnectedUserListOperationCompleted;

        private System.Threading.SendOrPostCallback ChangePasswordOperationCompleted;

        private System.Threading.SendOrPostCallback RenameUserOperationCompleted;

        private System.Threading.SendOrPostCallback TerminateConnectionOperationCompleted;

        private System.Threading.SendOrPostCallback GetProfilesOperationCompleted;

        private System.Threading.SendOrPostCallback GetGroupsOperationCompleted;

        private System.Threading.SendOrPostCallback GetConfigurationOperationCompleted;

        private System.Threading.SendOrPostCallback GetInterfacesOperationCompleted;

        private System.Threading.SendOrPostCallback GetIPBlockListOperationCompleted;

        private System.Threading.SendOrPostCallback GetAutoBlockListOperationCompleted;

        private System.Threading.SendOrPostCallback GetAppPathsOperationCompleted;

        private System.Threading.SendOrPostCallback GetLicenseInfoOperationCompleted;

        private System.Threading.SendOrPostCallback VerifyLicenseOperationCompleted;

        private System.Threading.SendOrPostCallback GetCurrentConnectionCountOperationCompleted;

        private System.Threading.SendOrPostCallback GetAllCurrentConnectionCountOperationCompleted;

        private System.Threading.SendOrPostCallback GetInterfaceByIDOperationCompleted;

        private System.Threading.SendOrPostCallback GetInterfaceListOperationCompleted;

        private System.Threading.SendOrPostCallback InitializeInterfaceOperationCompleted;

        private System.Threading.SendOrPostCallback ShutdownInterfaceOperationCompleted;

        private System.Threading.SendOrPostCallback GetStatisticsOperationCompleted;

        private System.Threading.SendOrPostCallback GetCurrentBandwidthOperationCompleted;

        private System.Threading.SendOrPostCallback GetFeaturesOperationCompleted;

        private System.Threading.SendOrPostCallback SaveProfilesOperationCompleted;

        private System.Threading.SendOrPostCallback SaveConfigurationOperationCompleted;

        private System.Threading.SendOrPostCallback CommitSettingsOperationCompleted;

        private System.Threading.SendOrPostCallback SaveBlockListOperationCompleted;

        private System.Threading.SendOrPostCallback ModifyInterfaceOperationCompleted;

        private System.Threading.SendOrPostCallback ShutdownConnectionsOnInterfaceOperationCompleted;

        private System.Threading.SendOrPostCallback GetFileTransfersOperationCompleted;

        private System.Threading.SendOrPostCallback GetLogMessagesOperationCompleted;

        private System.Threading.SendOrPostCallback BlockAddressOperationCompleted;

        private System.Threading.SendOrPostCallback GenerateStatisticsOperationCompleted;

        private System.Threading.SendOrPostCallback BackupServerConfigurationOperationCompleted;

        private System.Threading.SendOrPostCallback RestoreServerConfigurationOperationCompleted;

        /// <remarks/>
        public CerberusFTPService()
        {
            this.SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12;
            this.Url = "http://localhost:10001/service/cerberusftpservice";
        }

        /// <remarks/>
        public event GetBackupServersCompletedEventHandler GetBackupServersCompleted;

        /// <remarks/>
        public event SaveBackupServersCompletedEventHandler SaveBackupServersCompleted;

        /// <remarks/>
        public event SharePublicFileCompletedEventHandler SharePublicFileCompleted;

        /// <remarks/>
        public event AddIpCompletedEventHandler AddIpCompleted;

        /// <remarks/>
        public event DeleteIpCompletedEventHandler DeleteIpCompleted;

        /// <remarks/>
        public event TestAndVerifyDatabaseCompletedEventHandler TestAndVerifyDatabaseCompleted;

        /// <remarks/>
        public event CreateStatisticsDatabaseCompletedEventHandler CreateStatisticsDatabaseCompleted;

        /// <remarks/>
        public event DropStatisticsDatabaseCompletedEventHandler DropStatisticsDatabaseCompleted;

        /// <remarks/>
        public event GetMimeMappingsCompletedEventHandler GetMimeMappingsCompleted;

        /// <remarks/>
        public event SaveMimeMappingsCompletedEventHandler SaveMimeMappingsCompleted;

        /// <remarks/>
        public event ServerSummaryStatusCompletedEventHandler ServerSummaryStatusCompleted;

        /// <remarks/>
        public event ServerInformationCompletedEventHandler ServerInformationCompleted;

        /// <remarks/>
        public event CurrentStatusCompletedEventHandler CurrentStatusCompleted;

        /// <remarks/>
        public event StartServerCompletedEventHandler StartServerCompleted;

        /// <remarks/>
        public event StopServerCompletedEventHandler StopServerCompleted;

        /// <remarks/>
        public event ServerStartedCompletedEventHandler ServerStartedCompleted;

        /// <remarks/>
        public event InitializeServerCompletedEventHandler InitializeServerCompleted;

        /// <remarks/>
        public event ShutdownServerCompletedEventHandler ShutdownServerCompleted;

        /// <remarks/>
        public event GetEventRulesCompletedEventHandler GetEventRulesCompleted;

        /// <remarks/>
        public event SetEventRulesCompletedEventHandler SetEventRulesCompleted;

        /// <remarks/>
        public event DeleteRequestedAccountsCompletedEventHandler DeleteRequestedAccountsCompleted;

        /// <remarks/>
        public event GetRequestedAccountsCompletedEventHandler GetRequestedAccountsCompleted;

        /// <remarks/>
        public event SetRequestedAccountsCompletedEventHandler SetRequestedAccountsCompleted;

        /// <remarks/>
        public event GetAuthenticationListCompletedEventHandler GetAuthenticationListCompleted;

        /// <remarks/>
        public event SetAuthenticationListCompletedEventHandler SetAuthenticationListCompleted;

        /// <remarks/>
        public event GetHostnameCompletedEventHandler GetHostnameCompleted;

        /// <remarks/>
        public event SetWANIPCompletedEventHandler SetWANIPCompleted;

        /// <remarks/>
        public event AddUserCompletedEventHandler AddUserCompleted;

        /// <remarks/>
        public event AddGroupCompletedEventHandler AddGroupCompleted;

        /// <remarks/>
        public event DeleteUserCompletedEventHandler DeleteUserCompleted;

        /// <remarks/>
        public event DeleteGroupCompletedEventHandler DeleteGroupCompleted;

        /// <remarks/>
        public event AddRootCompletedEventHandler AddRootCompleted;

        /// <remarks/>
        public event DeleteRootCompletedEventHandler DeleteRootCompleted;

        /// <remarks/>
        public event GetUserListCompletedEventHandler GetUserListCompleted;

        /// <remarks/>
        public event GetGroupListCompletedEventHandler GetGroupListCompleted;

        /// <remarks/>
        public event GetUserInformationCompletedEventHandler GetUserInformationCompleted;

        /// <remarks/>
        public event GetConnectedUserListCompletedEventHandler GetConnectedUserListCompleted;

        /// <remarks/>
        public event ChangePasswordCompletedEventHandler ChangePasswordCompleted;

        /// <remarks/>
        public event RenameUserCompletedEventHandler RenameUserCompleted;

        /// <remarks/>
        public event TerminateConnectionCompletedEventHandler TerminateConnectionCompleted;

        /// <remarks/>
        public event GetProfilesCompletedEventHandler GetProfilesCompleted;

        /// <remarks/>
        public event GetGroupsCompletedEventHandler GetGroupsCompleted;

        /// <remarks/>
        public event GetConfigurationCompletedEventHandler GetConfigurationCompleted;

        /// <remarks/>
        public event GetInterfacesCompletedEventHandler GetInterfacesCompleted;

        /// <remarks/>
        public event GetIPBlockListCompletedEventHandler GetIPBlockListCompleted;

        /// <remarks/>
        public event GetAutoBlockListCompletedEventHandler GetAutoBlockListCompleted;

        /// <remarks/>
        public event GetAppPathsCompletedEventHandler GetAppPathsCompleted;

        /// <remarks/>
        public event GetLicenseInfoCompletedEventHandler GetLicenseInfoCompleted;

        /// <remarks/>
        public event VerifyLicenseCompletedEventHandler VerifyLicenseCompleted;

        /// <remarks/>
        public event GetCurrentConnectionCountCompletedEventHandler GetCurrentConnectionCountCompleted;

        /// <remarks/>
        public event GetAllCurrentConnectionCountCompletedEventHandler GetAllCurrentConnectionCountCompleted;

        /// <remarks/>
        public event GetInterfaceByIDCompletedEventHandler GetInterfaceByIDCompleted;

        /// <remarks/>
        public event GetInterfaceListCompletedEventHandler GetInterfaceListCompleted;

        /// <remarks/>
        public event InitializeInterfaceCompletedEventHandler InitializeInterfaceCompleted;

        /// <remarks/>
        public event ShutdownInterfaceCompletedEventHandler ShutdownInterfaceCompleted;

        /// <remarks/>
        public event GetStatisticsCompletedEventHandler GetStatisticsCompleted;

        /// <remarks/>
        public event GetCurrentBandwidthCompletedEventHandler GetCurrentBandwidthCompleted;

        /// <remarks/>
        public event GetFeaturesCompletedEventHandler GetFeaturesCompleted;

        /// <remarks/>
        public event SaveProfilesCompletedEventHandler SaveProfilesCompleted;

        /// <remarks/>
        public event SaveConfigurationCompletedEventHandler SaveConfigurationCompleted;

        /// <remarks/>
        public event CommitSettingsCompletedEventHandler CommitSettingsCompleted;

        /// <remarks/>
        public event SaveBlockListCompletedEventHandler SaveBlockListCompleted;

        /// <remarks/>
        public event ModifyInterfaceCompletedEventHandler ModifyInterfaceCompleted;

        /// <remarks/>
        public event ShutdownConnectionsOnInterfaceCompletedEventHandler ShutdownConnectionsOnInterfaceCompleted;

        /// <remarks/>
        public event GetFileTransfersCompletedEventHandler GetFileTransfersCompleted;

        /// <remarks/>
        public event GetLogMessagesCompletedEventHandler GetLogMessagesCompleted;

        /// <remarks/>
        public event BlockAddressCompletedEventHandler BlockAddressCompleted;

        /// <remarks/>
        public event GenerateStatisticsCompletedEventHandler GenerateStatisticsCompleted;

        /// <remarks/>
        public event BackupServerConfigurationCompletedEventHandler BackupServerConfigurationCompleted;

        /// <remarks/>
        public event RestoreServerConfigurationCompletedEventHandler RestoreServerConfigurationCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetBackupServers", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetBackupServersResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetBackupServersResponse GetBackupServers([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetBackupServersRequest GetBackupServersRequest)
        {
            object[] results = this.Invoke("GetBackupServers", new object[] {
                        GetBackupServersRequest});
            return ((GetBackupServersResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetBackupServers(GetBackupServersRequest GetBackupServersRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetBackupServers", new object[] {
                        GetBackupServersRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetBackupServersResponse EndGetBackupServers(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetBackupServersResponse)(results[0]));
        }

        /// <remarks/>
        public void GetBackupServersAsync(GetBackupServersRequest GetBackupServersRequest)
        {
            this.GetBackupServersAsync(GetBackupServersRequest, null);
        }

        /// <remarks/>
        public void GetBackupServersAsync(GetBackupServersRequest GetBackupServersRequest, object userState)
        {
            if ((this.GetBackupServersOperationCompleted == null))
            {
                this.GetBackupServersOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetBackupServersOperationCompleted);
            }
            this.InvokeAsync("GetBackupServers", new object[] {
                        GetBackupServersRequest}, this.GetBackupServersOperationCompleted, userState);
        }

        private void OnGetBackupServersOperationCompleted(object arg)
        {
            if ((this.GetBackupServersCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetBackupServersCompleted(this, new GetBackupServersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SaveBackupServers", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SaveBackupServersResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SaveBackupServersResponse SaveBackupServers([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SaveBackupServersRequest SaveBackupServersRequest)
        {
            object[] results = this.Invoke("SaveBackupServers", new object[] {
                        SaveBackupServersRequest});
            return ((SaveBackupServersResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSaveBackupServers(SaveBackupServersRequest SaveBackupServersRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SaveBackupServers", new object[] {
                        SaveBackupServersRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SaveBackupServersResponse EndSaveBackupServers(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SaveBackupServersResponse)(results[0]));
        }

        /// <remarks/>
        public void SaveBackupServersAsync(SaveBackupServersRequest SaveBackupServersRequest)
        {
            this.SaveBackupServersAsync(SaveBackupServersRequest, null);
        }

        /// <remarks/>
        public void SaveBackupServersAsync(SaveBackupServersRequest SaveBackupServersRequest, object userState)
        {
            if ((this.SaveBackupServersOperationCompleted == null))
            {
                this.SaveBackupServersOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveBackupServersOperationCompleted);
            }
            this.InvokeAsync("SaveBackupServers", new object[] {
                        SaveBackupServersRequest}, this.SaveBackupServersOperationCompleted, userState);
        }

        private void OnSaveBackupServersOperationCompleted(object arg)
        {
            if ((this.SaveBackupServersCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveBackupServersCompleted(this, new SaveBackupServersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SharePublicFile", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SharePublicFileResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SharePublicFileResponse SharePublicFile([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SharePublicFileRequest SharePublicFileRequest)
        {
            object[] results = this.Invoke("SharePublicFile", new object[] {
                        SharePublicFileRequest});
            return ((SharePublicFileResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSharePublicFile(SharePublicFileRequest SharePublicFileRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SharePublicFile", new object[] {
                        SharePublicFileRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SharePublicFileResponse EndSharePublicFile(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SharePublicFileResponse)(results[0]));
        }

        /// <remarks/>
        public void SharePublicFileAsync(SharePublicFileRequest SharePublicFileRequest)
        {
            this.SharePublicFileAsync(SharePublicFileRequest, null);
        }

        /// <remarks/>
        public void SharePublicFileAsync(SharePublicFileRequest SharePublicFileRequest, object userState)
        {
            if ((this.SharePublicFileOperationCompleted == null))
            {
                this.SharePublicFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSharePublicFileOperationCompleted);
            }
            this.InvokeAsync("SharePublicFile", new object[] {
                        SharePublicFileRequest}, this.SharePublicFileOperationCompleted, userState);
        }

        private void OnSharePublicFileOperationCompleted(object arg)
        {
            if ((this.SharePublicFileCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SharePublicFileCompleted(this, new SharePublicFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/AddIp", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("AddIpResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public AddIpResponse AddIp([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] AddIpRequest AddIpRequest)
        {
            object[] results = this.Invoke("AddIp", new object[] {
                        AddIpRequest});
            return ((AddIpResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddIp(AddIpRequest AddIpRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddIp", new object[] {
                        AddIpRequest}, callback, asyncState);
        }

        /// <remarks/>
        public AddIpResponse EndAddIp(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((AddIpResponse)(results[0]));
        }

        /// <remarks/>
        public void AddIpAsync(AddIpRequest AddIpRequest)
        {
            this.AddIpAsync(AddIpRequest, null);
        }

        /// <remarks/>
        public void AddIpAsync(AddIpRequest AddIpRequest, object userState)
        {
            if ((this.AddIpOperationCompleted == null))
            {
                this.AddIpOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddIpOperationCompleted);
            }
            this.InvokeAsync("AddIp", new object[] {
                        AddIpRequest}, this.AddIpOperationCompleted, userState);
        }

        private void OnAddIpOperationCompleted(object arg)
        {
            if ((this.AddIpCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddIpCompleted(this, new AddIpCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/DeleteIp", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DeleteIpResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public DeleteIpResponse DeleteIp([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] DeleteIpRequest DeleteIpRequest)
        {
            object[] results = this.Invoke("DeleteIp", new object[] {
                        DeleteIpRequest});
            return ((DeleteIpResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteIp(DeleteIpRequest DeleteIpRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteIp", new object[] {
                        DeleteIpRequest}, callback, asyncState);
        }

        /// <remarks/>
        public DeleteIpResponse EndDeleteIp(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DeleteIpResponse)(results[0]));
        }

        /// <remarks/>
        public void DeleteIpAsync(DeleteIpRequest DeleteIpRequest)
        {
            this.DeleteIpAsync(DeleteIpRequest, null);
        }

        /// <remarks/>
        public void DeleteIpAsync(DeleteIpRequest DeleteIpRequest, object userState)
        {
            if ((this.DeleteIpOperationCompleted == null))
            {
                this.DeleteIpOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteIpOperationCompleted);
            }
            this.InvokeAsync("DeleteIp", new object[] {
                        DeleteIpRequest}, this.DeleteIpOperationCompleted, userState);
        }

        private void OnDeleteIpOperationCompleted(object arg)
        {
            if ((this.DeleteIpCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteIpCompleted(this, new DeleteIpCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/TestAndVerifyDatabase", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("TestAndVerifyDatabaseResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public TestAndVerifyDatabaseResponse TestAndVerifyDatabase([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] TestAndVerifyDatabaseRequest TestAndVerifyDatabaseRequest)
        {
            object[] results = this.Invoke("TestAndVerifyDatabase", new object[] {
                        TestAndVerifyDatabaseRequest});
            return ((TestAndVerifyDatabaseResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginTestAndVerifyDatabase(TestAndVerifyDatabaseRequest TestAndVerifyDatabaseRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("TestAndVerifyDatabase", new object[] {
                        TestAndVerifyDatabaseRequest}, callback, asyncState);
        }

        /// <remarks/>
        public TestAndVerifyDatabaseResponse EndTestAndVerifyDatabase(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((TestAndVerifyDatabaseResponse)(results[0]));
        }

        /// <remarks/>
        public void TestAndVerifyDatabaseAsync(TestAndVerifyDatabaseRequest TestAndVerifyDatabaseRequest)
        {
            this.TestAndVerifyDatabaseAsync(TestAndVerifyDatabaseRequest, null);
        }

        /// <remarks/>
        public void TestAndVerifyDatabaseAsync(TestAndVerifyDatabaseRequest TestAndVerifyDatabaseRequest, object userState)
        {
            if ((this.TestAndVerifyDatabaseOperationCompleted == null))
            {
                this.TestAndVerifyDatabaseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTestAndVerifyDatabaseOperationCompleted);
            }
            this.InvokeAsync("TestAndVerifyDatabase", new object[] {
                        TestAndVerifyDatabaseRequest}, this.TestAndVerifyDatabaseOperationCompleted, userState);
        }

        private void OnTestAndVerifyDatabaseOperationCompleted(object arg)
        {
            if ((this.TestAndVerifyDatabaseCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TestAndVerifyDatabaseCompleted(this, new TestAndVerifyDatabaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/CreateStatisticsDatabase", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CreateStatisticsDatabaseResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public CreateStatisticsDatabaseResponse CreateStatisticsDatabase([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] CreateStatisticsDatabaseRequest CreateStatisticsDatabaseRequest)
        {
            object[] results = this.Invoke("CreateStatisticsDatabase", new object[] {
                        CreateStatisticsDatabaseRequest});
            return ((CreateStatisticsDatabaseResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCreateStatisticsDatabase(CreateStatisticsDatabaseRequest CreateStatisticsDatabaseRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CreateStatisticsDatabase", new object[] {
                        CreateStatisticsDatabaseRequest}, callback, asyncState);
        }

        /// <remarks/>
        public CreateStatisticsDatabaseResponse EndCreateStatisticsDatabase(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CreateStatisticsDatabaseResponse)(results[0]));
        }

        /// <remarks/>
        public void CreateStatisticsDatabaseAsync(CreateStatisticsDatabaseRequest CreateStatisticsDatabaseRequest)
        {
            this.CreateStatisticsDatabaseAsync(CreateStatisticsDatabaseRequest, null);
        }

        /// <remarks/>
        public void CreateStatisticsDatabaseAsync(CreateStatisticsDatabaseRequest CreateStatisticsDatabaseRequest, object userState)
        {
            if ((this.CreateStatisticsDatabaseOperationCompleted == null))
            {
                this.CreateStatisticsDatabaseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateStatisticsDatabaseOperationCompleted);
            }
            this.InvokeAsync("CreateStatisticsDatabase", new object[] {
                        CreateStatisticsDatabaseRequest}, this.CreateStatisticsDatabaseOperationCompleted, userState);
        }

        private void OnCreateStatisticsDatabaseOperationCompleted(object arg)
        {
            if ((this.CreateStatisticsDatabaseCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateStatisticsDatabaseCompleted(this, new CreateStatisticsDatabaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/DropStatisticsDatabase", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DropStatisticsDatabaseResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public DropStatisticsDatabaseResponse DropStatisticsDatabase([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] DropStatisticsDatabaseRequest DropStatisticsDatabaseRequest)
        {
            object[] results = this.Invoke("DropStatisticsDatabase", new object[] {
                        DropStatisticsDatabaseRequest});
            return ((DropStatisticsDatabaseResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDropStatisticsDatabase(DropStatisticsDatabaseRequest DropStatisticsDatabaseRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DropStatisticsDatabase", new object[] {
                        DropStatisticsDatabaseRequest}, callback, asyncState);
        }

        /// <remarks/>
        public DropStatisticsDatabaseResponse EndDropStatisticsDatabase(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DropStatisticsDatabaseResponse)(results[0]));
        }

        /// <remarks/>
        public void DropStatisticsDatabaseAsync(DropStatisticsDatabaseRequest DropStatisticsDatabaseRequest)
        {
            this.DropStatisticsDatabaseAsync(DropStatisticsDatabaseRequest, null);
        }

        /// <remarks/>
        public void DropStatisticsDatabaseAsync(DropStatisticsDatabaseRequest DropStatisticsDatabaseRequest, object userState)
        {
            if ((this.DropStatisticsDatabaseOperationCompleted == null))
            {
                this.DropStatisticsDatabaseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDropStatisticsDatabaseOperationCompleted);
            }
            this.InvokeAsync("DropStatisticsDatabase", new object[] {
                        DropStatisticsDatabaseRequest}, this.DropStatisticsDatabaseOperationCompleted, userState);
        }

        private void OnDropStatisticsDatabaseOperationCompleted(object arg)
        {
            if ((this.DropStatisticsDatabaseCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DropStatisticsDatabaseCompleted(this, new DropStatisticsDatabaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetMimeMappings", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetMimeMappingsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetMimeMappingsResponse GetMimeMappings([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetMimeMappingsRequest GetMimeMappingsRequest)
        {
            object[] results = this.Invoke("GetMimeMappings", new object[] {
                        GetMimeMappingsRequest});
            return ((GetMimeMappingsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetMimeMappings(GetMimeMappingsRequest GetMimeMappingsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetMimeMappings", new object[] {
                        GetMimeMappingsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetMimeMappingsResponse EndGetMimeMappings(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetMimeMappingsResponse)(results[0]));
        }

        /// <remarks/>
        public void GetMimeMappingsAsync(GetMimeMappingsRequest GetMimeMappingsRequest)
        {
            this.GetMimeMappingsAsync(GetMimeMappingsRequest, null);
        }

        /// <remarks/>
        public void GetMimeMappingsAsync(GetMimeMappingsRequest GetMimeMappingsRequest, object userState)
        {
            if ((this.GetMimeMappingsOperationCompleted == null))
            {
                this.GetMimeMappingsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetMimeMappingsOperationCompleted);
            }
            this.InvokeAsync("GetMimeMappings", new object[] {
                        GetMimeMappingsRequest}, this.GetMimeMappingsOperationCompleted, userState);
        }

        private void OnGetMimeMappingsOperationCompleted(object arg)
        {
            if ((this.GetMimeMappingsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetMimeMappingsCompleted(this, new GetMimeMappingsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SaveMimeMappings", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SaveMimeMappingsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SaveMimeMappingsResponse SaveMimeMappings([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SaveMimeMappingsRequest SaveMimeMappingsRequest)
        {
            object[] results = this.Invoke("SaveMimeMappings", new object[] {
                        SaveMimeMappingsRequest});
            return ((SaveMimeMappingsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSaveMimeMappings(SaveMimeMappingsRequest SaveMimeMappingsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SaveMimeMappings", new object[] {
                        SaveMimeMappingsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SaveMimeMappingsResponse EndSaveMimeMappings(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SaveMimeMappingsResponse)(results[0]));
        }

        /// <remarks/>
        public void SaveMimeMappingsAsync(SaveMimeMappingsRequest SaveMimeMappingsRequest)
        {
            this.SaveMimeMappingsAsync(SaveMimeMappingsRequest, null);
        }

        /// <remarks/>
        public void SaveMimeMappingsAsync(SaveMimeMappingsRequest SaveMimeMappingsRequest, object userState)
        {
            if ((this.SaveMimeMappingsOperationCompleted == null))
            {
                this.SaveMimeMappingsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveMimeMappingsOperationCompleted);
            }
            this.InvokeAsync("SaveMimeMappings", new object[] {
                        SaveMimeMappingsRequest}, this.SaveMimeMappingsOperationCompleted, userState);
        }

        private void OnSaveMimeMappingsOperationCompleted(object arg)
        {
            if ((this.SaveMimeMappingsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveMimeMappingsCompleted(this, new SaveMimeMappingsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/ServerSummaryStatus", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ServerSummaryStatusResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public ServerSummaryStatusResponse ServerSummaryStatus([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] ServerSummaryStatusRequest ServerSummaryStatusRequest)
        {
            object[] results = this.Invoke("ServerSummaryStatus", new object[] {
                        ServerSummaryStatusRequest});
            return ((ServerSummaryStatusResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginServerSummaryStatus(ServerSummaryStatusRequest ServerSummaryStatusRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ServerSummaryStatus", new object[] {
                        ServerSummaryStatusRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ServerSummaryStatusResponse EndServerSummaryStatus(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ServerSummaryStatusResponse)(results[0]));
        }

        /// <remarks/>
        public void ServerSummaryStatusAsync(ServerSummaryStatusRequest ServerSummaryStatusRequest)
        {
            this.ServerSummaryStatusAsync(ServerSummaryStatusRequest, null);
        }

        /// <remarks/>
        public void ServerSummaryStatusAsync(ServerSummaryStatusRequest ServerSummaryStatusRequest, object userState)
        {
            if ((this.ServerSummaryStatusOperationCompleted == null))
            {
                this.ServerSummaryStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnServerSummaryStatusOperationCompleted);
            }
            this.InvokeAsync("ServerSummaryStatus", new object[] {
                        ServerSummaryStatusRequest}, this.ServerSummaryStatusOperationCompleted, userState);
        }

        private void OnServerSummaryStatusOperationCompleted(object arg)
        {
            if ((this.ServerSummaryStatusCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServerSummaryStatusCompleted(this, new ServerSummaryStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/ServerInformation", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ServerInformationResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public ServerInformationResponse ServerInformation([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] ServerInformationRequest ServerInformationRequest)
        {
            object[] results = this.Invoke("ServerInformation", new object[] {
                        ServerInformationRequest});
            return ((ServerInformationResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginServerInformation(ServerInformationRequest ServerInformationRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ServerInformation", new object[] {
                        ServerInformationRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ServerInformationResponse EndServerInformation(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ServerInformationResponse)(results[0]));
        }

        /// <remarks/>
        public void ServerInformationAsync(ServerInformationRequest ServerInformationRequest)
        {
            this.ServerInformationAsync(ServerInformationRequest, null);
        }

        /// <remarks/>
        public void ServerInformationAsync(ServerInformationRequest ServerInformationRequest, object userState)
        {
            if ((this.ServerInformationOperationCompleted == null))
            {
                this.ServerInformationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnServerInformationOperationCompleted);
            }
            this.InvokeAsync("ServerInformation", new object[] {
                        ServerInformationRequest}, this.ServerInformationOperationCompleted, userState);
        }

        private void OnServerInformationOperationCompleted(object arg)
        {
            if ((this.ServerInformationCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServerInformationCompleted(this, new ServerInformationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/CurrentStatus", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CurrentStatusResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public CurrentStatusResponse CurrentStatus([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] CurrentStatusRequest CurrentStatusRequest)
        {
            object[] results = this.Invoke("CurrentStatus", new object[] {
                        CurrentStatusRequest});
            return ((CurrentStatusResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCurrentStatus(CurrentStatusRequest CurrentStatusRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CurrentStatus", new object[] {
                        CurrentStatusRequest}, callback, asyncState);
        }

        /// <remarks/>
        public CurrentStatusResponse EndCurrentStatus(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CurrentStatusResponse)(results[0]));
        }

        /// <remarks/>
        public void CurrentStatusAsync(CurrentStatusRequest CurrentStatusRequest)
        {
            this.CurrentStatusAsync(CurrentStatusRequest, null);
        }

        /// <remarks/>
        public void CurrentStatusAsync(CurrentStatusRequest CurrentStatusRequest, object userState)
        {
            if ((this.CurrentStatusOperationCompleted == null))
            {
                this.CurrentStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCurrentStatusOperationCompleted);
            }
            this.InvokeAsync("CurrentStatus", new object[] {
                        CurrentStatusRequest}, this.CurrentStatusOperationCompleted, userState);
        }

        private void OnCurrentStatusOperationCompleted(object arg)
        {
            if ((this.CurrentStatusCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CurrentStatusCompleted(this, new CurrentStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/StartServer", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("StartServerResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public StartServerResponse StartServer([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] StartServerRequest StartServerRequest)
        {
            object[] results = this.Invoke("StartServer", new object[] {
                        StartServerRequest});
            return ((StartServerResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginStartServer(StartServerRequest StartServerRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("StartServer", new object[] {
                        StartServerRequest}, callback, asyncState);
        }

        /// <remarks/>
        public StartServerResponse EndStartServer(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((StartServerResponse)(results[0]));
        }

        /// <remarks/>
        public void StartServerAsync(StartServerRequest StartServerRequest)
        {
            this.StartServerAsync(StartServerRequest, null);
        }

        /// <remarks/>
        public void StartServerAsync(StartServerRequest StartServerRequest, object userState)
        {
            if ((this.StartServerOperationCompleted == null))
            {
                this.StartServerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnStartServerOperationCompleted);
            }
            this.InvokeAsync("StartServer", new object[] {
                        StartServerRequest}, this.StartServerOperationCompleted, userState);
        }

        private void OnStartServerOperationCompleted(object arg)
        {
            if ((this.StartServerCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.StartServerCompleted(this, new StartServerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/StopServer", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("StopServerResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public StopServerResponse StopServer([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] StopServerRequest StopServerRequest)
        {
            object[] results = this.Invoke("StopServer", new object[] {
                        StopServerRequest});
            return ((StopServerResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginStopServer(StopServerRequest StopServerRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("StopServer", new object[] {
                        StopServerRequest}, callback, asyncState);
        }

        /// <remarks/>
        public StopServerResponse EndStopServer(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((StopServerResponse)(results[0]));
        }

        /// <remarks/>
        public void StopServerAsync(StopServerRequest StopServerRequest)
        {
            this.StopServerAsync(StopServerRequest, null);
        }

        /// <remarks/>
        public void StopServerAsync(StopServerRequest StopServerRequest, object userState)
        {
            if ((this.StopServerOperationCompleted == null))
            {
                this.StopServerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnStopServerOperationCompleted);
            }
            this.InvokeAsync("StopServer", new object[] {
                        StopServerRequest}, this.StopServerOperationCompleted, userState);
        }

        private void OnStopServerOperationCompleted(object arg)
        {
            if ((this.StopServerCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.StopServerCompleted(this, new StopServerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/ServerStarted", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ServerStartedResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public ServerStartedResponse ServerStarted([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] ServerStartedRequest ServerStartedRequest)
        {
            object[] results = this.Invoke("ServerStarted", new object[] {
                        ServerStartedRequest});
            return ((ServerStartedResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginServerStarted(ServerStartedRequest ServerStartedRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ServerStarted", new object[] {
                        ServerStartedRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ServerStartedResponse EndServerStarted(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ServerStartedResponse)(results[0]));
        }

        /// <remarks/>
        public void ServerStartedAsync(ServerStartedRequest ServerStartedRequest)
        {
            this.ServerStartedAsync(ServerStartedRequest, null);
        }

        /// <remarks/>
        public void ServerStartedAsync(ServerStartedRequest ServerStartedRequest, object userState)
        {
            if ((this.ServerStartedOperationCompleted == null))
            {
                this.ServerStartedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnServerStartedOperationCompleted);
            }
            this.InvokeAsync("ServerStarted", new object[] {
                        ServerStartedRequest}, this.ServerStartedOperationCompleted, userState);
        }

        private void OnServerStartedOperationCompleted(object arg)
        {
            if ((this.ServerStartedCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServerStartedCompleted(this, new ServerStartedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/InitializeServer", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("InitializeServerResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public InitializeServerResponse InitializeServer([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] InitializeServerRequest InitializeServerRequest)
        {
            object[] results = this.Invoke("InitializeServer", new object[] {
                        InitializeServerRequest});
            return ((InitializeServerResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginInitializeServer(InitializeServerRequest InitializeServerRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InitializeServer", new object[] {
                        InitializeServerRequest}, callback, asyncState);
        }

        /// <remarks/>
        public InitializeServerResponse EndInitializeServer(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((InitializeServerResponse)(results[0]));
        }

        /// <remarks/>
        public void InitializeServerAsync(InitializeServerRequest InitializeServerRequest)
        {
            this.InitializeServerAsync(InitializeServerRequest, null);
        }

        /// <remarks/>
        public void InitializeServerAsync(InitializeServerRequest InitializeServerRequest, object userState)
        {
            if ((this.InitializeServerOperationCompleted == null))
            {
                this.InitializeServerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInitializeServerOperationCompleted);
            }
            this.InvokeAsync("InitializeServer", new object[] {
                        InitializeServerRequest}, this.InitializeServerOperationCompleted, userState);
        }

        private void OnInitializeServerOperationCompleted(object arg)
        {
            if ((this.InitializeServerCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.InitializeServerCompleted(this, new InitializeServerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/ShutdownServer", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ShutdownServerResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public ShutdownServerResponse ShutdownServer([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] ShutdownServerRequest ShutdownServerRequest)
        {
            object[] results = this.Invoke("ShutdownServer", new object[] {
                        ShutdownServerRequest});
            return ((ShutdownServerResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginShutdownServer(ShutdownServerRequest ShutdownServerRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ShutdownServer", new object[] {
                        ShutdownServerRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ShutdownServerResponse EndShutdownServer(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ShutdownServerResponse)(results[0]));
        }

        /// <remarks/>
        public void ShutdownServerAsync(ShutdownServerRequest ShutdownServerRequest)
        {
            this.ShutdownServerAsync(ShutdownServerRequest, null);
        }

        /// <remarks/>
        public void ShutdownServerAsync(ShutdownServerRequest ShutdownServerRequest, object userState)
        {
            if ((this.ShutdownServerOperationCompleted == null))
            {
                this.ShutdownServerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnShutdownServerOperationCompleted);
            }
            this.InvokeAsync("ShutdownServer", new object[] {
                        ShutdownServerRequest}, this.ShutdownServerOperationCompleted, userState);
        }

        private void OnShutdownServerOperationCompleted(object arg)
        {
            if ((this.ShutdownServerCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ShutdownServerCompleted(this, new ShutdownServerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetEventRules", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetEventRulesResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetEventRulesResponse GetEventRules([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetEventRulesRequest GetEventRulesRequest)
        {
            object[] results = this.Invoke("GetEventRules", new object[] {
                        GetEventRulesRequest});
            return ((GetEventRulesResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetEventRules(GetEventRulesRequest GetEventRulesRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetEventRules", new object[] {
                        GetEventRulesRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetEventRulesResponse EndGetEventRules(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetEventRulesResponse)(results[0]));
        }

        /// <remarks/>
        public void GetEventRulesAsync(GetEventRulesRequest GetEventRulesRequest)
        {
            this.GetEventRulesAsync(GetEventRulesRequest, null);
        }

        /// <remarks/>
        public void GetEventRulesAsync(GetEventRulesRequest GetEventRulesRequest, object userState)
        {
            if ((this.GetEventRulesOperationCompleted == null))
            {
                this.GetEventRulesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetEventRulesOperationCompleted);
            }
            this.InvokeAsync("GetEventRules", new object[] {
                        GetEventRulesRequest}, this.GetEventRulesOperationCompleted, userState);
        }

        private void OnGetEventRulesOperationCompleted(object arg)
        {
            if ((this.GetEventRulesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetEventRulesCompleted(this, new GetEventRulesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SetEventRules", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SetEventRulesResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SetEventRulesResponse SetEventRules([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SetEventRulesRequest SetEventRulesRequest)
        {
            object[] results = this.Invoke("SetEventRules", new object[] {
                        SetEventRulesRequest});
            return ((SetEventRulesResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetEventRules(SetEventRulesRequest SetEventRulesRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetEventRules", new object[] {
                        SetEventRulesRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SetEventRulesResponse EndSetEventRules(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SetEventRulesResponse)(results[0]));
        }

        /// <remarks/>
        public void SetEventRulesAsync(SetEventRulesRequest SetEventRulesRequest)
        {
            this.SetEventRulesAsync(SetEventRulesRequest, null);
        }

        /// <remarks/>
        public void SetEventRulesAsync(SetEventRulesRequest SetEventRulesRequest, object userState)
        {
            if ((this.SetEventRulesOperationCompleted == null))
            {
                this.SetEventRulesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetEventRulesOperationCompleted);
            }
            this.InvokeAsync("SetEventRules", new object[] {
                        SetEventRulesRequest}, this.SetEventRulesOperationCompleted, userState);
        }

        private void OnSetEventRulesOperationCompleted(object arg)
        {
            if ((this.SetEventRulesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetEventRulesCompleted(this, new SetEventRulesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/DeleteRequestedAccounts", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DeleteRequestedAccountsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public DeleteRequestedAccountsResponse DeleteRequestedAccounts([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] DeleteRequestedAccountsRequest DeleteRequestedAccountsRequest)
        {
            object[] results = this.Invoke("DeleteRequestedAccounts", new object[] {
                        DeleteRequestedAccountsRequest});
            return ((DeleteRequestedAccountsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteRequestedAccounts(DeleteRequestedAccountsRequest DeleteRequestedAccountsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteRequestedAccounts", new object[] {
                        DeleteRequestedAccountsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public DeleteRequestedAccountsResponse EndDeleteRequestedAccounts(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DeleteRequestedAccountsResponse)(results[0]));
        }

        /// <remarks/>
        public void DeleteRequestedAccountsAsync(DeleteRequestedAccountsRequest DeleteRequestedAccountsRequest)
        {
            this.DeleteRequestedAccountsAsync(DeleteRequestedAccountsRequest, null);
        }

        /// <remarks/>
        public void DeleteRequestedAccountsAsync(DeleteRequestedAccountsRequest DeleteRequestedAccountsRequest, object userState)
        {
            if ((this.DeleteRequestedAccountsOperationCompleted == null))
            {
                this.DeleteRequestedAccountsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteRequestedAccountsOperationCompleted);
            }
            this.InvokeAsync("DeleteRequestedAccounts", new object[] {
                        DeleteRequestedAccountsRequest}, this.DeleteRequestedAccountsOperationCompleted, userState);
        }

        private void OnDeleteRequestedAccountsOperationCompleted(object arg)
        {
            if ((this.DeleteRequestedAccountsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteRequestedAccountsCompleted(this, new DeleteRequestedAccountsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetRequestedAccounts", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetRequestedAccountsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetRequestedAccountsResponse GetRequestedAccounts([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetRequestedAccountsRequest GetRequestedAccountsRequest)
        {
            object[] results = this.Invoke("GetRequestedAccounts", new object[] {
                        GetRequestedAccountsRequest});
            return ((GetRequestedAccountsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetRequestedAccounts(GetRequestedAccountsRequest GetRequestedAccountsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetRequestedAccounts", new object[] {
                        GetRequestedAccountsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetRequestedAccountsResponse EndGetRequestedAccounts(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetRequestedAccountsResponse)(results[0]));
        }

        /// <remarks/>
        public void GetRequestedAccountsAsync(GetRequestedAccountsRequest GetRequestedAccountsRequest)
        {
            this.GetRequestedAccountsAsync(GetRequestedAccountsRequest, null);
        }

        /// <remarks/>
        public void GetRequestedAccountsAsync(GetRequestedAccountsRequest GetRequestedAccountsRequest, object userState)
        {
            if ((this.GetRequestedAccountsOperationCompleted == null))
            {
                this.GetRequestedAccountsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRequestedAccountsOperationCompleted);
            }
            this.InvokeAsync("GetRequestedAccounts", new object[] {
                        GetRequestedAccountsRequest}, this.GetRequestedAccountsOperationCompleted, userState);
        }

        private void OnGetRequestedAccountsOperationCompleted(object arg)
        {
            if ((this.GetRequestedAccountsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRequestedAccountsCompleted(this, new GetRequestedAccountsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SetRequestedAccounts", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SetRequestedAccountsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SetRequestedAccountsResponse SetRequestedAccounts([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SetRequestedAccountsRequest SetRequestedAccountsRequest)
        {
            object[] results = this.Invoke("SetRequestedAccounts", new object[] {
                        SetRequestedAccountsRequest});
            return ((SetRequestedAccountsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetRequestedAccounts(SetRequestedAccountsRequest SetRequestedAccountsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetRequestedAccounts", new object[] {
                        SetRequestedAccountsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SetRequestedAccountsResponse EndSetRequestedAccounts(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SetRequestedAccountsResponse)(results[0]));
        }

        /// <remarks/>
        public void SetRequestedAccountsAsync(SetRequestedAccountsRequest SetRequestedAccountsRequest)
        {
            this.SetRequestedAccountsAsync(SetRequestedAccountsRequest, null);
        }

        /// <remarks/>
        public void SetRequestedAccountsAsync(SetRequestedAccountsRequest SetRequestedAccountsRequest, object userState)
        {
            if ((this.SetRequestedAccountsOperationCompleted == null))
            {
                this.SetRequestedAccountsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetRequestedAccountsOperationCompleted);
            }
            this.InvokeAsync("SetRequestedAccounts", new object[] {
                        SetRequestedAccountsRequest}, this.SetRequestedAccountsOperationCompleted, userState);
        }

        private void OnSetRequestedAccountsOperationCompleted(object arg)
        {
            if ((this.SetRequestedAccountsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetRequestedAccountsCompleted(this, new SetRequestedAccountsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetAuthenticationList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetAuthenticationListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetAuthenticationListResponse GetAuthenticationList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetAuthenticationListRequest GetAuthenticationListRequest)
        {
            object[] results = this.Invoke("GetAuthenticationList", new object[] {
                        GetAuthenticationListRequest});
            return ((GetAuthenticationListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetAuthenticationList(GetAuthenticationListRequest GetAuthenticationListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetAuthenticationList", new object[] {
                        GetAuthenticationListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetAuthenticationListResponse EndGetAuthenticationList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetAuthenticationListResponse)(results[0]));
        }

        /// <remarks/>
        public void GetAuthenticationListAsync(GetAuthenticationListRequest GetAuthenticationListRequest)
        {
            this.GetAuthenticationListAsync(GetAuthenticationListRequest, null);
        }

        /// <remarks/>
        public void GetAuthenticationListAsync(GetAuthenticationListRequest GetAuthenticationListRequest, object userState)
        {
            if ((this.GetAuthenticationListOperationCompleted == null))
            {
                this.GetAuthenticationListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAuthenticationListOperationCompleted);
            }
            this.InvokeAsync("GetAuthenticationList", new object[] {
                        GetAuthenticationListRequest}, this.GetAuthenticationListOperationCompleted, userState);
        }

        private void OnGetAuthenticationListOperationCompleted(object arg)
        {
            if ((this.GetAuthenticationListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAuthenticationListCompleted(this, new GetAuthenticationListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SetAuthenticationList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SetAuthenticationListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SetAuthenticationListResponse SetAuthenticationList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SetAuthenticationListRequest SetAuthenticationListRequest)
        {
            object[] results = this.Invoke("SetAuthenticationList", new object[] {
                        SetAuthenticationListRequest});
            return ((SetAuthenticationListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetAuthenticationList(SetAuthenticationListRequest SetAuthenticationListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetAuthenticationList", new object[] {
                        SetAuthenticationListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SetAuthenticationListResponse EndSetAuthenticationList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SetAuthenticationListResponse)(results[0]));
        }

        /// <remarks/>
        public void SetAuthenticationListAsync(SetAuthenticationListRequest SetAuthenticationListRequest)
        {
            this.SetAuthenticationListAsync(SetAuthenticationListRequest, null);
        }

        /// <remarks/>
        public void SetAuthenticationListAsync(SetAuthenticationListRequest SetAuthenticationListRequest, object userState)
        {
            if ((this.SetAuthenticationListOperationCompleted == null))
            {
                this.SetAuthenticationListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetAuthenticationListOperationCompleted);
            }
            this.InvokeAsync("SetAuthenticationList", new object[] {
                        SetAuthenticationListRequest}, this.SetAuthenticationListOperationCompleted, userState);
        }

        private void OnSetAuthenticationListOperationCompleted(object arg)
        {
            if ((this.SetAuthenticationListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetAuthenticationListCompleted(this, new SetAuthenticationListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetHostname", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetHostnameResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetHostnameResponse GetHostname([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetHostnameRequest GetHostnameRequest)
        {
            object[] results = this.Invoke("GetHostname", new object[] {
                        GetHostnameRequest});
            return ((GetHostnameResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetHostname(GetHostnameRequest GetHostnameRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetHostname", new object[] {
                        GetHostnameRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetHostnameResponse EndGetHostname(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetHostnameResponse)(results[0]));
        }

        /// <remarks/>
        public void GetHostnameAsync(GetHostnameRequest GetHostnameRequest)
        {
            this.GetHostnameAsync(GetHostnameRequest, null);
        }

        /// <remarks/>
        public void GetHostnameAsync(GetHostnameRequest GetHostnameRequest, object userState)
        {
            if ((this.GetHostnameOperationCompleted == null))
            {
                this.GetHostnameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetHostnameOperationCompleted);
            }
            this.InvokeAsync("GetHostname", new object[] {
                        GetHostnameRequest}, this.GetHostnameOperationCompleted, userState);
        }

        private void OnGetHostnameOperationCompleted(object arg)
        {
            if ((this.GetHostnameCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetHostnameCompleted(this, new GetHostnameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SetWANIP", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SetWANIPResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SetWANIPResponse SetWANIP([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SetWANIPRequest SetWANIPRequest)
        {
            object[] results = this.Invoke("SetWANIP", new object[] {
                        SetWANIPRequest});
            return ((SetWANIPResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetWANIP(SetWANIPRequest SetWANIPRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetWANIP", new object[] {
                        SetWANIPRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SetWANIPResponse EndSetWANIP(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SetWANIPResponse)(results[0]));
        }

        /// <remarks/>
        public void SetWANIPAsync(SetWANIPRequest SetWANIPRequest)
        {
            this.SetWANIPAsync(SetWANIPRequest, null);
        }

        /// <remarks/>
        public void SetWANIPAsync(SetWANIPRequest SetWANIPRequest, object userState)
        {
            if ((this.SetWANIPOperationCompleted == null))
            {
                this.SetWANIPOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetWANIPOperationCompleted);
            }
            this.InvokeAsync("SetWANIP", new object[] {
                        SetWANIPRequest}, this.SetWANIPOperationCompleted, userState);
        }

        private void OnSetWANIPOperationCompleted(object arg)
        {
            if ((this.SetWANIPCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetWANIPCompleted(this, new SetWANIPCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/AddUser", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("AddUserResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public AddUserResponse AddUser([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] AddUserRequest AddUserRequest)
        {
            object[] results = this.Invoke("AddUser", new object[] {
                        AddUserRequest});
            return ((AddUserResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddUser(AddUserRequest AddUserRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddUser", new object[] {
                        AddUserRequest}, callback, asyncState);
        }

        /// <remarks/>
        public AddUserResponse EndAddUser(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((AddUserResponse)(results[0]));
        }

        /// <remarks/>
        public void AddUserAsync(AddUserRequest AddUserRequest)
        {
            this.AddUserAsync(AddUserRequest, null);
        }

        /// <remarks/>
        public void AddUserAsync(AddUserRequest AddUserRequest, object userState)
        {
            if ((this.AddUserOperationCompleted == null))
            {
                this.AddUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddUserOperationCompleted);
            }
            this.InvokeAsync("AddUser", new object[] {
                        AddUserRequest}, this.AddUserOperationCompleted, userState);
        }

        private void OnAddUserOperationCompleted(object arg)
        {
            if ((this.AddUserCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddUserCompleted(this, new AddUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/AddGroup", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("AddGroupResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public AddGroupResponse AddGroup([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] AddGroupRequest AddGroupRequest)
        {
            object[] results = this.Invoke("AddGroup", new object[] {
                        AddGroupRequest});
            return ((AddGroupResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddGroup(AddGroupRequest AddGroupRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddGroup", new object[] {
                        AddGroupRequest}, callback, asyncState);
        }

        /// <remarks/>
        public AddGroupResponse EndAddGroup(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((AddGroupResponse)(results[0]));
        }

        /// <remarks/>
        public void AddGroupAsync(AddGroupRequest AddGroupRequest)
        {
            this.AddGroupAsync(AddGroupRequest, null);
        }

        /// <remarks/>
        public void AddGroupAsync(AddGroupRequest AddGroupRequest, object userState)
        {
            if ((this.AddGroupOperationCompleted == null))
            {
                this.AddGroupOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddGroupOperationCompleted);
            }
            this.InvokeAsync("AddGroup", new object[] {
                        AddGroupRequest}, this.AddGroupOperationCompleted, userState);
        }

        private void OnAddGroupOperationCompleted(object arg)
        {
            if ((this.AddGroupCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddGroupCompleted(this, new AddGroupCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/DeleteUser", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DeleteUserResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public DeleteUserResponse DeleteUser([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] DeleteUserRequest DeleteUserRequest)
        {
            object[] results = this.Invoke("DeleteUser", new object[] {
                        DeleteUserRequest});
            return ((DeleteUserResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteUser(DeleteUserRequest DeleteUserRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteUser", new object[] {
                        DeleteUserRequest}, callback, asyncState);
        }

        /// <remarks/>
        public DeleteUserResponse EndDeleteUser(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DeleteUserResponse)(results[0]));
        }

        /// <remarks/>
        public void DeleteUserAsync(DeleteUserRequest DeleteUserRequest)
        {
            this.DeleteUserAsync(DeleteUserRequest, null);
        }

        /// <remarks/>
        public void DeleteUserAsync(DeleteUserRequest DeleteUserRequest, object userState)
        {
            if ((this.DeleteUserOperationCompleted == null))
            {
                this.DeleteUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteUserOperationCompleted);
            }
            this.InvokeAsync("DeleteUser", new object[] {
                        DeleteUserRequest}, this.DeleteUserOperationCompleted, userState);
        }

        private void OnDeleteUserOperationCompleted(object arg)
        {
            if ((this.DeleteUserCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteUserCompleted(this, new DeleteUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/DeleteGroup", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DeleteGroupResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public DeleteGroupResponse DeleteGroup([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] DeleteGroupRequest DeleteGroupRequest)
        {
            object[] results = this.Invoke("DeleteGroup", new object[] {
                        DeleteGroupRequest});
            return ((DeleteGroupResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteGroup(DeleteGroupRequest DeleteGroupRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteGroup", new object[] {
                        DeleteGroupRequest}, callback, asyncState);
        }

        /// <remarks/>
        public DeleteGroupResponse EndDeleteGroup(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DeleteGroupResponse)(results[0]));
        }

        /// <remarks/>
        public void DeleteGroupAsync(DeleteGroupRequest DeleteGroupRequest)
        {
            this.DeleteGroupAsync(DeleteGroupRequest, null);
        }

        /// <remarks/>
        public void DeleteGroupAsync(DeleteGroupRequest DeleteGroupRequest, object userState)
        {
            if ((this.DeleteGroupOperationCompleted == null))
            {
                this.DeleteGroupOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteGroupOperationCompleted);
            }
            this.InvokeAsync("DeleteGroup", new object[] {
                        DeleteGroupRequest}, this.DeleteGroupOperationCompleted, userState);
        }

        private void OnDeleteGroupOperationCompleted(object arg)
        {
            if ((this.DeleteGroupCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteGroupCompleted(this, new DeleteGroupCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/AddRoot", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("AddRootResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public AddRootResponse AddRoot([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] AddRootRequest AddRootRequest)
        {
            object[] results = this.Invoke("AddRoot", new object[] {
                        AddRootRequest});
            return ((AddRootResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddRoot(AddRootRequest AddRootRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddRoot", new object[] {
                        AddRootRequest}, callback, asyncState);
        }

        /// <remarks/>
        public AddRootResponse EndAddRoot(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((AddRootResponse)(results[0]));
        }

        /// <remarks/>
        public void AddRootAsync(AddRootRequest AddRootRequest)
        {
            this.AddRootAsync(AddRootRequest, null);
        }

        /// <remarks/>
        public void AddRootAsync(AddRootRequest AddRootRequest, object userState)
        {
            if ((this.AddRootOperationCompleted == null))
            {
                this.AddRootOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddRootOperationCompleted);
            }
            this.InvokeAsync("AddRoot", new object[] {
                        AddRootRequest}, this.AddRootOperationCompleted, userState);
        }

        private void OnAddRootOperationCompleted(object arg)
        {
            if ((this.AddRootCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddRootCompleted(this, new AddRootCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/DeleteRoot", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DeleteRootResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public DeleteRootResponse DeleteRoot([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] DeleteRootRequest DeleteRootRequest)
        {
            object[] results = this.Invoke("DeleteRoot", new object[] {
                        DeleteRootRequest});
            return ((DeleteRootResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteRoot(DeleteRootRequest DeleteRootRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteRoot", new object[] {
                        DeleteRootRequest}, callback, asyncState);
        }

        /// <remarks/>
        public DeleteRootResponse EndDeleteRoot(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DeleteRootResponse)(results[0]));
        }

        /// <remarks/>
        public void DeleteRootAsync(DeleteRootRequest DeleteRootRequest)
        {
            this.DeleteRootAsync(DeleteRootRequest, null);
        }

        /// <remarks/>
        public void DeleteRootAsync(DeleteRootRequest DeleteRootRequest, object userState)
        {
            if ((this.DeleteRootOperationCompleted == null))
            {
                this.DeleteRootOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteRootOperationCompleted);
            }
            this.InvokeAsync("DeleteRoot", new object[] {
                        DeleteRootRequest}, this.DeleteRootOperationCompleted, userState);
        }

        private void OnDeleteRootOperationCompleted(object arg)
        {
            if ((this.DeleteRootCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteRootCompleted(this, new DeleteRootCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetUserList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetUserListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetUserListResponse GetUserList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetUserListRequest GetUserListRequest)
        {
            object[] results = this.Invoke("GetUserList", new object[] {
                        GetUserListRequest});
            return ((GetUserListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetUserList(GetUserListRequest GetUserListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetUserList", new object[] {
                        GetUserListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetUserListResponse EndGetUserList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetUserListResponse)(results[0]));
        }

        /// <remarks/>
        public void GetUserListAsync(GetUserListRequest GetUserListRequest)
        {
            this.GetUserListAsync(GetUserListRequest, null);
        }

        /// <remarks/>
        public void GetUserListAsync(GetUserListRequest GetUserListRequest, object userState)
        {
            if ((this.GetUserListOperationCompleted == null))
            {
                this.GetUserListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetUserListOperationCompleted);
            }
            this.InvokeAsync("GetUserList", new object[] {
                        GetUserListRequest}, this.GetUserListOperationCompleted, userState);
        }

        private void OnGetUserListOperationCompleted(object arg)
        {
            if ((this.GetUserListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetUserListCompleted(this, new GetUserListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetGroupList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetGroupListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetGroupListResponse GetGroupList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetGroupListRequest GetGroupListRequest)
        {
            object[] results = this.Invoke("GetGroupList", new object[] {
                        GetGroupListRequest});
            return ((GetGroupListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetGroupList(GetGroupListRequest GetGroupListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetGroupList", new object[] {
                        GetGroupListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetGroupListResponse EndGetGroupList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetGroupListResponse)(results[0]));
        }

        /// <remarks/>
        public void GetGroupListAsync(GetGroupListRequest GetGroupListRequest)
        {
            this.GetGroupListAsync(GetGroupListRequest, null);
        }

        /// <remarks/>
        public void GetGroupListAsync(GetGroupListRequest GetGroupListRequest, object userState)
        {
            if ((this.GetGroupListOperationCompleted == null))
            {
                this.GetGroupListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetGroupListOperationCompleted);
            }
            this.InvokeAsync("GetGroupList", new object[] {
                        GetGroupListRequest}, this.GetGroupListOperationCompleted, userState);
        }

        private void OnGetGroupListOperationCompleted(object arg)
        {
            if ((this.GetGroupListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetGroupListCompleted(this, new GetGroupListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetUserInformation", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetUserInformationResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetUserInformationResponse GetUserInformation([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetUserInformationRequest GetUserInformationRequest)
        {
            object[] results = this.Invoke("GetUserInformation", new object[] {
                        GetUserInformationRequest});
            return ((GetUserInformationResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetUserInformation(GetUserInformationRequest GetUserInformationRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetUserInformation", new object[] {
                        GetUserInformationRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetUserInformationResponse EndGetUserInformation(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetUserInformationResponse)(results[0]));
        }

        /// <remarks/>
        public void GetUserInformationAsync(GetUserInformationRequest GetUserInformationRequest)
        {
            this.GetUserInformationAsync(GetUserInformationRequest, null);
        }

        /// <remarks/>
        public void GetUserInformationAsync(GetUserInformationRequest GetUserInformationRequest, object userState)
        {
            if ((this.GetUserInformationOperationCompleted == null))
            {
                this.GetUserInformationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetUserInformationOperationCompleted);
            }
            this.InvokeAsync("GetUserInformation", new object[] {
                        GetUserInformationRequest}, this.GetUserInformationOperationCompleted, userState);
        }

        private void OnGetUserInformationOperationCompleted(object arg)
        {
            if ((this.GetUserInformationCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetUserInformationCompleted(this, new GetUserInformationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetConnectedUserList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetConnectedUserListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetConnectedUserListResponse GetConnectedUserList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetConnectedUserListRequest GetConnectedUserListRequest)
        {
            object[] results = this.Invoke("GetConnectedUserList", new object[] {
                        GetConnectedUserListRequest});
            return ((GetConnectedUserListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetConnectedUserList(GetConnectedUserListRequest GetConnectedUserListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetConnectedUserList", new object[] {
                        GetConnectedUserListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetConnectedUserListResponse EndGetConnectedUserList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetConnectedUserListResponse)(results[0]));
        }

        /// <remarks/>
        public void GetConnectedUserListAsync(GetConnectedUserListRequest GetConnectedUserListRequest)
        {
            this.GetConnectedUserListAsync(GetConnectedUserListRequest, null);
        }

        /// <remarks/>
        public void GetConnectedUserListAsync(GetConnectedUserListRequest GetConnectedUserListRequest, object userState)
        {
            if ((this.GetConnectedUserListOperationCompleted == null))
            {
                this.GetConnectedUserListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetConnectedUserListOperationCompleted);
            }
            this.InvokeAsync("GetConnectedUserList", new object[] {
                        GetConnectedUserListRequest}, this.GetConnectedUserListOperationCompleted, userState);
        }

        private void OnGetConnectedUserListOperationCompleted(object arg)
        {
            if ((this.GetConnectedUserListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetConnectedUserListCompleted(this, new GetConnectedUserListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/ChangePassword", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ChangePasswordResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public ChangePasswordResponse ChangePassword([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] ChangePasswordRequest ChangePasswordRequest)
        {
            object[] results = this.Invoke("ChangePassword", new object[] {
                        ChangePasswordRequest});
            return ((ChangePasswordResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginChangePassword(ChangePasswordRequest ChangePasswordRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ChangePassword", new object[] {
                        ChangePasswordRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ChangePasswordResponse EndChangePassword(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ChangePasswordResponse)(results[0]));
        }

        /// <remarks/>
        public void ChangePasswordAsync(ChangePasswordRequest ChangePasswordRequest)
        {
            this.ChangePasswordAsync(ChangePasswordRequest, null);
        }

        /// <remarks/>
        public void ChangePasswordAsync(ChangePasswordRequest ChangePasswordRequest, object userState)
        {
            if ((this.ChangePasswordOperationCompleted == null))
            {
                this.ChangePasswordOperationCompleted = new System.Threading.SendOrPostCallback(this.OnChangePasswordOperationCompleted);
            }
            this.InvokeAsync("ChangePassword", new object[] {
                        ChangePasswordRequest}, this.ChangePasswordOperationCompleted, userState);
        }

        private void OnChangePasswordOperationCompleted(object arg)
        {
            if ((this.ChangePasswordCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ChangePasswordCompleted(this, new ChangePasswordCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/RenameUser", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RenameUserResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public RenameUserResponse RenameUser([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] RenameUserRequest RenameUserRequest)
        {
            object[] results = this.Invoke("RenameUser", new object[] {
                        RenameUserRequest});
            return ((RenameUserResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRenameUser(RenameUserRequest RenameUserRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("RenameUser", new object[] {
                        RenameUserRequest}, callback, asyncState);
        }

        /// <remarks/>
        public RenameUserResponse EndRenameUser(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((RenameUserResponse)(results[0]));
        }

        /// <remarks/>
        public void RenameUserAsync(RenameUserRequest RenameUserRequest)
        {
            this.RenameUserAsync(RenameUserRequest, null);
        }

        /// <remarks/>
        public void RenameUserAsync(RenameUserRequest RenameUserRequest, object userState)
        {
            if ((this.RenameUserOperationCompleted == null))
            {
                this.RenameUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRenameUserOperationCompleted);
            }
            this.InvokeAsync("RenameUser", new object[] {
                        RenameUserRequest}, this.RenameUserOperationCompleted, userState);
        }

        private void OnRenameUserOperationCompleted(object arg)
        {
            if ((this.RenameUserCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RenameUserCompleted(this, new RenameUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/TerminateConnection", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("TerminateConnectionResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public TerminateConnectionResponse TerminateConnection([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] TerminateConnectionRequest TerminateConnectionRequest)
        {
            object[] results = this.Invoke("TerminateConnection", new object[] {
                        TerminateConnectionRequest});
            return ((TerminateConnectionResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginTerminateConnection(TerminateConnectionRequest TerminateConnectionRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("TerminateConnection", new object[] {
                        TerminateConnectionRequest}, callback, asyncState);
        }

        /// <remarks/>
        public TerminateConnectionResponse EndTerminateConnection(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((TerminateConnectionResponse)(results[0]));
        }

        /// <remarks/>
        public void TerminateConnectionAsync(TerminateConnectionRequest TerminateConnectionRequest)
        {
            this.TerminateConnectionAsync(TerminateConnectionRequest, null);
        }

        /// <remarks/>
        public void TerminateConnectionAsync(TerminateConnectionRequest TerminateConnectionRequest, object userState)
        {
            if ((this.TerminateConnectionOperationCompleted == null))
            {
                this.TerminateConnectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTerminateConnectionOperationCompleted);
            }
            this.InvokeAsync("TerminateConnection", new object[] {
                        TerminateConnectionRequest}, this.TerminateConnectionOperationCompleted, userState);
        }

        private void OnTerminateConnectionOperationCompleted(object arg)
        {
            if ((this.TerminateConnectionCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TerminateConnectionCompleted(this, new TerminateConnectionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetProfiles", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetProfilesResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetProfilesResponse GetProfiles([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetProfilesRequest GetProfilesRequest)
        {
            object[] results = this.Invoke("GetProfiles", new object[] {
                        GetProfilesRequest});
            return ((GetProfilesResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetProfiles(GetProfilesRequest GetProfilesRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetProfiles", new object[] {
                        GetProfilesRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetProfilesResponse EndGetProfiles(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetProfilesResponse)(results[0]));
        }

        /// <remarks/>
        public void GetProfilesAsync(GetProfilesRequest GetProfilesRequest)
        {
            this.GetProfilesAsync(GetProfilesRequest, null);
        }

        /// <remarks/>
        public void GetProfilesAsync(GetProfilesRequest GetProfilesRequest, object userState)
        {
            if ((this.GetProfilesOperationCompleted == null))
            {
                this.GetProfilesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetProfilesOperationCompleted);
            }
            this.InvokeAsync("GetProfiles", new object[] {
                        GetProfilesRequest}, this.GetProfilesOperationCompleted, userState);
        }

        private void OnGetProfilesOperationCompleted(object arg)
        {
            if ((this.GetProfilesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetProfilesCompleted(this, new GetProfilesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetGroups", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetGroupsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetGroupsResponse GetGroups([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetGroupsRequest GetGroupsRequest)
        {
            object[] results = this.Invoke("GetGroups", new object[] {
                        GetGroupsRequest});
            return ((GetGroupsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetGroups(GetGroupsRequest GetGroupsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetGroups", new object[] {
                        GetGroupsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetGroupsResponse EndGetGroups(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetGroupsResponse)(results[0]));
        }

        /// <remarks/>
        public void GetGroupsAsync(GetGroupsRequest GetGroupsRequest)
        {
            this.GetGroupsAsync(GetGroupsRequest, null);
        }

        /// <remarks/>
        public void GetGroupsAsync(GetGroupsRequest GetGroupsRequest, object userState)
        {
            if ((this.GetGroupsOperationCompleted == null))
            {
                this.GetGroupsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetGroupsOperationCompleted);
            }
            this.InvokeAsync("GetGroups", new object[] {
                        GetGroupsRequest}, this.GetGroupsOperationCompleted, userState);
        }

        private void OnGetGroupsOperationCompleted(object arg)
        {
            if ((this.GetGroupsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetGroupsCompleted(this, new GetGroupsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetConfiguration", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetConfigurationResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetConfigurationResponse GetConfiguration([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetConfigurationRequest GetConfigurationRequest)
        {
            object[] results = this.Invoke("GetConfiguration", new object[] {
                        GetConfigurationRequest});
            return ((GetConfigurationResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetConfiguration(GetConfigurationRequest GetConfigurationRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetConfiguration", new object[] {
                        GetConfigurationRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetConfigurationResponse EndGetConfiguration(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetConfigurationResponse)(results[0]));
        }

        /// <remarks/>
        public void GetConfigurationAsync(GetConfigurationRequest GetConfigurationRequest)
        {
            this.GetConfigurationAsync(GetConfigurationRequest, null);
        }

        /// <remarks/>
        public void GetConfigurationAsync(GetConfigurationRequest GetConfigurationRequest, object userState)
        {
            if ((this.GetConfigurationOperationCompleted == null))
            {
                this.GetConfigurationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetConfigurationOperationCompleted);
            }
            this.InvokeAsync("GetConfiguration", new object[] {
                        GetConfigurationRequest}, this.GetConfigurationOperationCompleted, userState);
        }

        private void OnGetConfigurationOperationCompleted(object arg)
        {
            if ((this.GetConfigurationCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetConfigurationCompleted(this, new GetConfigurationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetInterfaces", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetInterfacesResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetInterfacesResponse GetInterfaces([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetInterfacesRequest GetInterfacesRequest)
        {
            object[] results = this.Invoke("GetInterfaces", new object[] {
                        GetInterfacesRequest});
            return ((GetInterfacesResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetInterfaces(GetInterfacesRequest GetInterfacesRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetInterfaces", new object[] {
                        GetInterfacesRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetInterfacesResponse EndGetInterfaces(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetInterfacesResponse)(results[0]));
        }

        /// <remarks/>
        public void GetInterfacesAsync(GetInterfacesRequest GetInterfacesRequest)
        {
            this.GetInterfacesAsync(GetInterfacesRequest, null);
        }

        /// <remarks/>
        public void GetInterfacesAsync(GetInterfacesRequest GetInterfacesRequest, object userState)
        {
            if ((this.GetInterfacesOperationCompleted == null))
            {
                this.GetInterfacesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetInterfacesOperationCompleted);
            }
            this.InvokeAsync("GetInterfaces", new object[] {
                        GetInterfacesRequest}, this.GetInterfacesOperationCompleted, userState);
        }

        private void OnGetInterfacesOperationCompleted(object arg)
        {
            if ((this.GetInterfacesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetInterfacesCompleted(this, new GetInterfacesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetIPBlockList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetIPBlockListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetIPBlockListResponse GetIPBlockList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetIPBlockListRequest GetIPBlockListRequest)
        {
            object[] results = this.Invoke("GetIPBlockList", new object[] {
                        GetIPBlockListRequest});
            return ((GetIPBlockListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetIPBlockList(GetIPBlockListRequest GetIPBlockListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetIPBlockList", new object[] {
                        GetIPBlockListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetIPBlockListResponse EndGetIPBlockList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetIPBlockListResponse)(results[0]));
        }

        /// <remarks/>
        public void GetIPBlockListAsync(GetIPBlockListRequest GetIPBlockListRequest)
        {
            this.GetIPBlockListAsync(GetIPBlockListRequest, null);
        }

        /// <remarks/>
        public void GetIPBlockListAsync(GetIPBlockListRequest GetIPBlockListRequest, object userState)
        {
            if ((this.GetIPBlockListOperationCompleted == null))
            {
                this.GetIPBlockListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetIPBlockListOperationCompleted);
            }
            this.InvokeAsync("GetIPBlockList", new object[] {
                        GetIPBlockListRequest}, this.GetIPBlockListOperationCompleted, userState);
        }

        private void OnGetIPBlockListOperationCompleted(object arg)
        {
            if ((this.GetIPBlockListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetIPBlockListCompleted(this, new GetIPBlockListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetAutoBlockList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetAutoBlockListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetAutoBlockListResponse GetAutoBlockList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetAutoBlockListRequest GetAutoBlockListRequest)
        {
            object[] results = this.Invoke("GetAutoBlockList", new object[] {
                        GetAutoBlockListRequest});
            return ((GetAutoBlockListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetAutoBlockList(GetAutoBlockListRequest GetAutoBlockListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetAutoBlockList", new object[] {
                        GetAutoBlockListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetAutoBlockListResponse EndGetAutoBlockList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetAutoBlockListResponse)(results[0]));
        }

        /// <remarks/>
        public void GetAutoBlockListAsync(GetAutoBlockListRequest GetAutoBlockListRequest)
        {
            this.GetAutoBlockListAsync(GetAutoBlockListRequest, null);
        }

        /// <remarks/>
        public void GetAutoBlockListAsync(GetAutoBlockListRequest GetAutoBlockListRequest, object userState)
        {
            if ((this.GetAutoBlockListOperationCompleted == null))
            {
                this.GetAutoBlockListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAutoBlockListOperationCompleted);
            }
            this.InvokeAsync("GetAutoBlockList", new object[] {
                        GetAutoBlockListRequest}, this.GetAutoBlockListOperationCompleted, userState);
        }

        private void OnGetAutoBlockListOperationCompleted(object arg)
        {
            if ((this.GetAutoBlockListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAutoBlockListCompleted(this, new GetAutoBlockListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetAppPaths", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetAppPathsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetAppPathsResponse GetAppPaths([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetAppPathsRequest GetAppPathsRequest)
        {
            object[] results = this.Invoke("GetAppPaths", new object[] {
                        GetAppPathsRequest});
            return ((GetAppPathsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetAppPaths(GetAppPathsRequest GetAppPathsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetAppPaths", new object[] {
                        GetAppPathsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetAppPathsResponse EndGetAppPaths(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetAppPathsResponse)(results[0]));
        }

        /// <remarks/>
        public void GetAppPathsAsync(GetAppPathsRequest GetAppPathsRequest)
        {
            this.GetAppPathsAsync(GetAppPathsRequest, null);
        }

        /// <remarks/>
        public void GetAppPathsAsync(GetAppPathsRequest GetAppPathsRequest, object userState)
        {
            if ((this.GetAppPathsOperationCompleted == null))
            {
                this.GetAppPathsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAppPathsOperationCompleted);
            }
            this.InvokeAsync("GetAppPaths", new object[] {
                        GetAppPathsRequest}, this.GetAppPathsOperationCompleted, userState);
        }

        private void OnGetAppPathsOperationCompleted(object arg)
        {
            if ((this.GetAppPathsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAppPathsCompleted(this, new GetAppPathsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetLicenseInfo", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetLicenseInfoResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetLicenseInfoResponse GetLicenseInfo([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetLicenseInfoRequest GetLicenseInfoRequest)
        {
            object[] results = this.Invoke("GetLicenseInfo", new object[] {
                        GetLicenseInfoRequest});
            return ((GetLicenseInfoResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetLicenseInfo(GetLicenseInfoRequest GetLicenseInfoRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetLicenseInfo", new object[] {
                        GetLicenseInfoRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetLicenseInfoResponse EndGetLicenseInfo(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetLicenseInfoResponse)(results[0]));
        }

        /// <remarks/>
        public void GetLicenseInfoAsync(GetLicenseInfoRequest GetLicenseInfoRequest)
        {
            this.GetLicenseInfoAsync(GetLicenseInfoRequest, null);
        }

        /// <remarks/>
        public void GetLicenseInfoAsync(GetLicenseInfoRequest GetLicenseInfoRequest, object userState)
        {
            if ((this.GetLicenseInfoOperationCompleted == null))
            {
                this.GetLicenseInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLicenseInfoOperationCompleted);
            }
            this.InvokeAsync("GetLicenseInfo", new object[] {
                        GetLicenseInfoRequest}, this.GetLicenseInfoOperationCompleted, userState);
        }

        private void OnGetLicenseInfoOperationCompleted(object arg)
        {
            if ((this.GetLicenseInfoCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLicenseInfoCompleted(this, new GetLicenseInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/VerifyLicense", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("VerifyLicenseResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public VerifyLicenseResponse VerifyLicense([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] VerifyLicenseRequest VerifyLicenseRequest)
        {
            object[] results = this.Invoke("VerifyLicense", new object[] {
                        VerifyLicenseRequest});
            return ((VerifyLicenseResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginVerifyLicense(VerifyLicenseRequest VerifyLicenseRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("VerifyLicense", new object[] {
                        VerifyLicenseRequest}, callback, asyncState);
        }

        /// <remarks/>
        public VerifyLicenseResponse EndVerifyLicense(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((VerifyLicenseResponse)(results[0]));
        }

        /// <remarks/>
        public void VerifyLicenseAsync(VerifyLicenseRequest VerifyLicenseRequest)
        {
            this.VerifyLicenseAsync(VerifyLicenseRequest, null);
        }

        /// <remarks/>
        public void VerifyLicenseAsync(VerifyLicenseRequest VerifyLicenseRequest, object userState)
        {
            if ((this.VerifyLicenseOperationCompleted == null))
            {
                this.VerifyLicenseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnVerifyLicenseOperationCompleted);
            }
            this.InvokeAsync("VerifyLicense", new object[] {
                        VerifyLicenseRequest}, this.VerifyLicenseOperationCompleted, userState);
        }

        private void OnVerifyLicenseOperationCompleted(object arg)
        {
            if ((this.VerifyLicenseCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.VerifyLicenseCompleted(this, new VerifyLicenseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetCurrentConnectionCount", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetCurrentConnectionCountResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetCurrentConnectionCountResponse GetCurrentConnectionCount([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetCurrentConnectionCountRequest GetCurrentConnectionCountRequest)
        {
            object[] results = this.Invoke("GetCurrentConnectionCount", new object[] {
                        GetCurrentConnectionCountRequest});
            return ((GetCurrentConnectionCountResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetCurrentConnectionCount(GetCurrentConnectionCountRequest GetCurrentConnectionCountRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetCurrentConnectionCount", new object[] {
                        GetCurrentConnectionCountRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetCurrentConnectionCountResponse EndGetCurrentConnectionCount(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetCurrentConnectionCountResponse)(results[0]));
        }

        /// <remarks/>
        public void GetCurrentConnectionCountAsync(GetCurrentConnectionCountRequest GetCurrentConnectionCountRequest)
        {
            this.GetCurrentConnectionCountAsync(GetCurrentConnectionCountRequest, null);
        }

        /// <remarks/>
        public void GetCurrentConnectionCountAsync(GetCurrentConnectionCountRequest GetCurrentConnectionCountRequest, object userState)
        {
            if ((this.GetCurrentConnectionCountOperationCompleted == null))
            {
                this.GetCurrentConnectionCountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCurrentConnectionCountOperationCompleted);
            }
            this.InvokeAsync("GetCurrentConnectionCount", new object[] {
                        GetCurrentConnectionCountRequest}, this.GetCurrentConnectionCountOperationCompleted, userState);
        }

        private void OnGetCurrentConnectionCountOperationCompleted(object arg)
        {
            if ((this.GetCurrentConnectionCountCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCurrentConnectionCountCompleted(this, new GetCurrentConnectionCountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetAllCurrentConnectionCount", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetAllCurrentConnectionCountResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetAllCurrentConnectionCountResponse GetAllCurrentConnectionCount([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetAllCurrentConnectionCountRequest GetAllCurrentConnectionCountRequest)
        {
            object[] results = this.Invoke("GetAllCurrentConnectionCount", new object[] {
                        GetAllCurrentConnectionCountRequest});
            return ((GetAllCurrentConnectionCountResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetAllCurrentConnectionCount(GetAllCurrentConnectionCountRequest GetAllCurrentConnectionCountRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetAllCurrentConnectionCount", new object[] {
                        GetAllCurrentConnectionCountRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetAllCurrentConnectionCountResponse EndGetAllCurrentConnectionCount(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetAllCurrentConnectionCountResponse)(results[0]));
        }

        /// <remarks/>
        public void GetAllCurrentConnectionCountAsync(GetAllCurrentConnectionCountRequest GetAllCurrentConnectionCountRequest)
        {
            this.GetAllCurrentConnectionCountAsync(GetAllCurrentConnectionCountRequest, null);
        }

        /// <remarks/>
        public void GetAllCurrentConnectionCountAsync(GetAllCurrentConnectionCountRequest GetAllCurrentConnectionCountRequest, object userState)
        {
            if ((this.GetAllCurrentConnectionCountOperationCompleted == null))
            {
                this.GetAllCurrentConnectionCountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllCurrentConnectionCountOperationCompleted);
            }
            this.InvokeAsync("GetAllCurrentConnectionCount", new object[] {
                        GetAllCurrentConnectionCountRequest}, this.GetAllCurrentConnectionCountOperationCompleted, userState);
        }

        private void OnGetAllCurrentConnectionCountOperationCompleted(object arg)
        {
            if ((this.GetAllCurrentConnectionCountCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllCurrentConnectionCountCompleted(this, new GetAllCurrentConnectionCountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetInterfaceByID", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetInterfaceResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetInterfaceResponse GetInterfaceByID([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetInterfaceByIDRequest GetInterfaceByIDRequest)
        {
            object[] results = this.Invoke("GetInterfaceByID", new object[] {
                        GetInterfaceByIDRequest});
            return ((GetInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetInterfaceByID(GetInterfaceByIDRequest GetInterfaceByIDRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetInterfaceByID", new object[] {
                        GetInterfaceByIDRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetInterfaceResponse EndGetInterfaceByID(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public void GetInterfaceByIDAsync(GetInterfaceByIDRequest GetInterfaceByIDRequest)
        {
            this.GetInterfaceByIDAsync(GetInterfaceByIDRequest, null);
        }

        /// <remarks/>
        public void GetInterfaceByIDAsync(GetInterfaceByIDRequest GetInterfaceByIDRequest, object userState)
        {
            if ((this.GetInterfaceByIDOperationCompleted == null))
            {
                this.GetInterfaceByIDOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetInterfaceByIDOperationCompleted);
            }
            this.InvokeAsync("GetInterfaceByID", new object[] {
                        GetInterfaceByIDRequest}, this.GetInterfaceByIDOperationCompleted, userState);
        }

        private void OnGetInterfaceByIDOperationCompleted(object arg)
        {
            if ((this.GetInterfaceByIDCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetInterfaceByIDCompleted(this, new GetInterfaceByIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetInterfaceList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetInterfaceListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetInterfaceListResponse GetInterfaceList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetInterfaceListRequest GetInterfaceListRequest)
        {
            object[] results = this.Invoke("GetInterfaceList", new object[] {
                        GetInterfaceListRequest});
            return ((GetInterfaceListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetInterfaceList(GetInterfaceListRequest GetInterfaceListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetInterfaceList", new object[] {
                        GetInterfaceListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetInterfaceListResponse EndGetInterfaceList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetInterfaceListResponse)(results[0]));
        }

        /// <remarks/>
        public void GetInterfaceListAsync(GetInterfaceListRequest GetInterfaceListRequest)
        {
            this.GetInterfaceListAsync(GetInterfaceListRequest, null);
        }

        /// <remarks/>
        public void GetInterfaceListAsync(GetInterfaceListRequest GetInterfaceListRequest, object userState)
        {
            if ((this.GetInterfaceListOperationCompleted == null))
            {
                this.GetInterfaceListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetInterfaceListOperationCompleted);
            }
            this.InvokeAsync("GetInterfaceList", new object[] {
                        GetInterfaceListRequest}, this.GetInterfaceListOperationCompleted, userState);
        }

        private void OnGetInterfaceListOperationCompleted(object arg)
        {
            if ((this.GetInterfaceListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetInterfaceListCompleted(this, new GetInterfaceListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/InitializeInterface", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("InitializeInterfaceResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public InitializeInterfaceResponse InitializeInterface([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] InitializeInterfaceRequest InitializeInterfaceRequest)
        {
            object[] results = this.Invoke("InitializeInterface", new object[] {
                        InitializeInterfaceRequest});
            return ((InitializeInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginInitializeInterface(InitializeInterfaceRequest InitializeInterfaceRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InitializeInterface", new object[] {
                        InitializeInterfaceRequest}, callback, asyncState);
        }

        /// <remarks/>
        public InitializeInterfaceResponse EndInitializeInterface(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((InitializeInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public void InitializeInterfaceAsync(InitializeInterfaceRequest InitializeInterfaceRequest)
        {
            this.InitializeInterfaceAsync(InitializeInterfaceRequest, null);
        }

        /// <remarks/>
        public void InitializeInterfaceAsync(InitializeInterfaceRequest InitializeInterfaceRequest, object userState)
        {
            if ((this.InitializeInterfaceOperationCompleted == null))
            {
                this.InitializeInterfaceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInitializeInterfaceOperationCompleted);
            }
            this.InvokeAsync("InitializeInterface", new object[] {
                        InitializeInterfaceRequest}, this.InitializeInterfaceOperationCompleted, userState);
        }

        private void OnInitializeInterfaceOperationCompleted(object arg)
        {
            if ((this.InitializeInterfaceCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.InitializeInterfaceCompleted(this, new InitializeInterfaceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/ShutdownInterface", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ShutdownInterfaceResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public ShutdownInterfaceResponse ShutdownInterface([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] ShutdownInterfaceRequest ShutdownInterfaceRequest)
        {
            object[] results = this.Invoke("ShutdownInterface", new object[] {
                        ShutdownInterfaceRequest});
            return ((ShutdownInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginShutdownInterface(ShutdownInterfaceRequest ShutdownInterfaceRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ShutdownInterface", new object[] {
                        ShutdownInterfaceRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ShutdownInterfaceResponse EndShutdownInterface(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ShutdownInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public void ShutdownInterfaceAsync(ShutdownInterfaceRequest ShutdownInterfaceRequest)
        {
            this.ShutdownInterfaceAsync(ShutdownInterfaceRequest, null);
        }

        /// <remarks/>
        public void ShutdownInterfaceAsync(ShutdownInterfaceRequest ShutdownInterfaceRequest, object userState)
        {
            if ((this.ShutdownInterfaceOperationCompleted == null))
            {
                this.ShutdownInterfaceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnShutdownInterfaceOperationCompleted);
            }
            this.InvokeAsync("ShutdownInterface", new object[] {
                        ShutdownInterfaceRequest}, this.ShutdownInterfaceOperationCompleted, userState);
        }

        private void OnShutdownInterfaceOperationCompleted(object arg)
        {
            if ((this.ShutdownInterfaceCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ShutdownInterfaceCompleted(this, new ShutdownInterfaceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetStatistics", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetStatisticsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetStatisticsResponse GetStatistics([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetStatisticsRequest GetStatisticsRequest)
        {
            object[] results = this.Invoke("GetStatistics", new object[] {
                        GetStatisticsRequest});
            return ((GetStatisticsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetStatistics(GetStatisticsRequest GetStatisticsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetStatistics", new object[] {
                        GetStatisticsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetStatisticsResponse EndGetStatistics(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetStatisticsResponse)(results[0]));
        }

        /// <remarks/>
        public void GetStatisticsAsync(GetStatisticsRequest GetStatisticsRequest)
        {
            this.GetStatisticsAsync(GetStatisticsRequest, null);
        }

        /// <remarks/>
        public void GetStatisticsAsync(GetStatisticsRequest GetStatisticsRequest, object userState)
        {
            if ((this.GetStatisticsOperationCompleted == null))
            {
                this.GetStatisticsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetStatisticsOperationCompleted);
            }
            this.InvokeAsync("GetStatistics", new object[] {
                        GetStatisticsRequest}, this.GetStatisticsOperationCompleted, userState);
        }

        private void OnGetStatisticsOperationCompleted(object arg)
        {
            if ((this.GetStatisticsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetStatisticsCompleted(this, new GetStatisticsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetCurrentBandwidth", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetCurrentBandwidthResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetCurrentBandwidthResponse GetCurrentBandwidth([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetCurrentBandwidthRequest GetCurrentBandwidthRequest)
        {
            object[] results = this.Invoke("GetCurrentBandwidth", new object[] {
                        GetCurrentBandwidthRequest});
            return ((GetCurrentBandwidthResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetCurrentBandwidth(GetCurrentBandwidthRequest GetCurrentBandwidthRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetCurrentBandwidth", new object[] {
                        GetCurrentBandwidthRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetCurrentBandwidthResponse EndGetCurrentBandwidth(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetCurrentBandwidthResponse)(results[0]));
        }

        /// <remarks/>
        public void GetCurrentBandwidthAsync(GetCurrentBandwidthRequest GetCurrentBandwidthRequest)
        {
            this.GetCurrentBandwidthAsync(GetCurrentBandwidthRequest, null);
        }

        /// <remarks/>
        public void GetCurrentBandwidthAsync(GetCurrentBandwidthRequest GetCurrentBandwidthRequest, object userState)
        {
            if ((this.GetCurrentBandwidthOperationCompleted == null))
            {
                this.GetCurrentBandwidthOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCurrentBandwidthOperationCompleted);
            }
            this.InvokeAsync("GetCurrentBandwidth", new object[] {
                        GetCurrentBandwidthRequest}, this.GetCurrentBandwidthOperationCompleted, userState);
        }

        private void OnGetCurrentBandwidthOperationCompleted(object arg)
        {
            if ((this.GetCurrentBandwidthCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCurrentBandwidthCompleted(this, new GetCurrentBandwidthCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetFeatures", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetFeaturesResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetFeaturesResponse GetFeatures([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetFeaturesRequest GetFeaturesRequest)
        {
            object[] results = this.Invoke("GetFeatures", new object[] {
                        GetFeaturesRequest});
            return ((GetFeaturesResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetFeatures(GetFeaturesRequest GetFeaturesRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetFeatures", new object[] {
                        GetFeaturesRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetFeaturesResponse EndGetFeatures(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetFeaturesResponse)(results[0]));
        }

        /// <remarks/>
        public void GetFeaturesAsync(GetFeaturesRequest GetFeaturesRequest)
        {
            this.GetFeaturesAsync(GetFeaturesRequest, null);
        }

        /// <remarks/>
        public void GetFeaturesAsync(GetFeaturesRequest GetFeaturesRequest, object userState)
        {
            if ((this.GetFeaturesOperationCompleted == null))
            {
                this.GetFeaturesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetFeaturesOperationCompleted);
            }
            this.InvokeAsync("GetFeatures", new object[] {
                        GetFeaturesRequest}, this.GetFeaturesOperationCompleted, userState);
        }

        private void OnGetFeaturesOperationCompleted(object arg)
        {
            if ((this.GetFeaturesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetFeaturesCompleted(this, new GetFeaturesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SaveProfiles", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SaveProfilesResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SaveProfilesResponse SaveProfiles([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SaveProfilesRequest SaveProfilesRequest)
        {
            object[] results = this.Invoke("SaveProfiles", new object[] {
                        SaveProfilesRequest});
            return ((SaveProfilesResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSaveProfiles(SaveProfilesRequest SaveProfilesRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SaveProfiles", new object[] {
                        SaveProfilesRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SaveProfilesResponse EndSaveProfiles(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SaveProfilesResponse)(results[0]));
        }

        /// <remarks/>
        public void SaveProfilesAsync(SaveProfilesRequest SaveProfilesRequest)
        {
            this.SaveProfilesAsync(SaveProfilesRequest, null);
        }

        /// <remarks/>
        public void SaveProfilesAsync(SaveProfilesRequest SaveProfilesRequest, object userState)
        {
            if ((this.SaveProfilesOperationCompleted == null))
            {
                this.SaveProfilesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveProfilesOperationCompleted);
            }
            this.InvokeAsync("SaveProfiles", new object[] {
                        SaveProfilesRequest}, this.SaveProfilesOperationCompleted, userState);
        }

        private void OnSaveProfilesOperationCompleted(object arg)
        {
            if ((this.SaveProfilesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveProfilesCompleted(this, new SaveProfilesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SaveConfiguration", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SaveConfigurationResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SaveConfigurationResponse SaveConfiguration([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SaveConfigurationRequest SaveConfigurationRequest)
        {
            object[] results = this.Invoke("SaveConfiguration", new object[] {
                        SaveConfigurationRequest});
            return ((SaveConfigurationResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSaveConfiguration(SaveConfigurationRequest SaveConfigurationRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SaveConfiguration", new object[] {
                        SaveConfigurationRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SaveConfigurationResponse EndSaveConfiguration(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SaveConfigurationResponse)(results[0]));
        }

        /// <remarks/>
        public void SaveConfigurationAsync(SaveConfigurationRequest SaveConfigurationRequest)
        {
            this.SaveConfigurationAsync(SaveConfigurationRequest, null);
        }

        /// <remarks/>
        public void SaveConfigurationAsync(SaveConfigurationRequest SaveConfigurationRequest, object userState)
        {
            if ((this.SaveConfigurationOperationCompleted == null))
            {
                this.SaveConfigurationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveConfigurationOperationCompleted);
            }
            this.InvokeAsync("SaveConfiguration", new object[] {
                        SaveConfigurationRequest}, this.SaveConfigurationOperationCompleted, userState);
        }

        private void OnSaveConfigurationOperationCompleted(object arg)
        {
            if ((this.SaveConfigurationCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveConfigurationCompleted(this, new SaveConfigurationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/CommitSettings", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CommitSettingsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public CommitSettingsResponse CommitSettings([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] CommitSettingsRequest CommitSettingsRequest)
        {
            object[] results = this.Invoke("CommitSettings", new object[] {
                        CommitSettingsRequest});
            return ((CommitSettingsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCommitSettings(CommitSettingsRequest CommitSettingsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CommitSettings", new object[] {
                        CommitSettingsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public CommitSettingsResponse EndCommitSettings(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CommitSettingsResponse)(results[0]));
        }

        /// <remarks/>
        public void CommitSettingsAsync(CommitSettingsRequest CommitSettingsRequest)
        {
            this.CommitSettingsAsync(CommitSettingsRequest, null);
        }

        /// <remarks/>
        public void CommitSettingsAsync(CommitSettingsRequest CommitSettingsRequest, object userState)
        {
            if ((this.CommitSettingsOperationCompleted == null))
            {
                this.CommitSettingsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCommitSettingsOperationCompleted);
            }
            this.InvokeAsync("CommitSettings", new object[] {
                        CommitSettingsRequest}, this.CommitSettingsOperationCompleted, userState);
        }

        private void OnCommitSettingsOperationCompleted(object arg)
        {
            if ((this.CommitSettingsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CommitSettingsCompleted(this, new CommitSettingsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/SaveBlockList", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SaveBlockListResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public SaveBlockListResponse SaveBlockList([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] SaveBlockListRequest SaveBlockListRequest)
        {
            object[] results = this.Invoke("SaveBlockList", new object[] {
                        SaveBlockListRequest});
            return ((SaveBlockListResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSaveBlockList(SaveBlockListRequest SaveBlockListRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SaveBlockList", new object[] {
                        SaveBlockListRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SaveBlockListResponse EndSaveBlockList(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SaveBlockListResponse)(results[0]));
        }

        /// <remarks/>
        public void SaveBlockListAsync(SaveBlockListRequest SaveBlockListRequest)
        {
            this.SaveBlockListAsync(SaveBlockListRequest, null);
        }

        /// <remarks/>
        public void SaveBlockListAsync(SaveBlockListRequest SaveBlockListRequest, object userState)
        {
            if ((this.SaveBlockListOperationCompleted == null))
            {
                this.SaveBlockListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveBlockListOperationCompleted);
            }
            this.InvokeAsync("SaveBlockList", new object[] {
                        SaveBlockListRequest}, this.SaveBlockListOperationCompleted, userState);
        }

        private void OnSaveBlockListOperationCompleted(object arg)
        {
            if ((this.SaveBlockListCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveBlockListCompleted(this, new SaveBlockListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/ModifyInterface", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ModifyInterfaceResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public ModifyInterfaceResponse ModifyInterface([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] ModifyInterfaceRequest ModifyInterfaceRequest)
        {
            object[] results = this.Invoke("ModifyInterface", new object[] {
                        ModifyInterfaceRequest});
            return ((ModifyInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginModifyInterface(ModifyInterfaceRequest ModifyInterfaceRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ModifyInterface", new object[] {
                        ModifyInterfaceRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ModifyInterfaceResponse EndModifyInterface(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ModifyInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public void ModifyInterfaceAsync(ModifyInterfaceRequest ModifyInterfaceRequest)
        {
            this.ModifyInterfaceAsync(ModifyInterfaceRequest, null);
        }

        /// <remarks/>
        public void ModifyInterfaceAsync(ModifyInterfaceRequest ModifyInterfaceRequest, object userState)
        {
            if ((this.ModifyInterfaceOperationCompleted == null))
            {
                this.ModifyInterfaceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnModifyInterfaceOperationCompleted);
            }
            this.InvokeAsync("ModifyInterface", new object[] {
                        ModifyInterfaceRequest}, this.ModifyInterfaceOperationCompleted, userState);
        }

        private void OnModifyInterfaceOperationCompleted(object arg)
        {
            if ((this.ModifyInterfaceCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ModifyInterfaceCompleted(this, new ModifyInterfaceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/ShutdownConnectionsOnInterface", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ShutdownConnectionsOnInterfaceResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public ShutdownConnectionsOnInterfaceResponse ShutdownConnectionsOnInterface([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] ShutdownConnectionsOnInterfaceRequest ShutdownConnectionsOnInterfaceRequest)
        {
            object[] results = this.Invoke("ShutdownConnectionsOnInterface", new object[] {
                        ShutdownConnectionsOnInterfaceRequest});
            return ((ShutdownConnectionsOnInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginShutdownConnectionsOnInterface(ShutdownConnectionsOnInterfaceRequest ShutdownConnectionsOnInterfaceRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ShutdownConnectionsOnInterface", new object[] {
                        ShutdownConnectionsOnInterfaceRequest}, callback, asyncState);
        }

        /// <remarks/>
        public ShutdownConnectionsOnInterfaceResponse EndShutdownConnectionsOnInterface(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ShutdownConnectionsOnInterfaceResponse)(results[0]));
        }

        /// <remarks/>
        public void ShutdownConnectionsOnInterfaceAsync(ShutdownConnectionsOnInterfaceRequest ShutdownConnectionsOnInterfaceRequest)
        {
            this.ShutdownConnectionsOnInterfaceAsync(ShutdownConnectionsOnInterfaceRequest, null);
        }

        /// <remarks/>
        public void ShutdownConnectionsOnInterfaceAsync(ShutdownConnectionsOnInterfaceRequest ShutdownConnectionsOnInterfaceRequest, object userState)
        {
            if ((this.ShutdownConnectionsOnInterfaceOperationCompleted == null))
            {
                this.ShutdownConnectionsOnInterfaceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnShutdownConnectionsOnInterfaceOperationCompleted);
            }
            this.InvokeAsync("ShutdownConnectionsOnInterface", new object[] {
                        ShutdownConnectionsOnInterfaceRequest}, this.ShutdownConnectionsOnInterfaceOperationCompleted, userState);
        }

        private void OnShutdownConnectionsOnInterfaceOperationCompleted(object arg)
        {
            if ((this.ShutdownConnectionsOnInterfaceCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ShutdownConnectionsOnInterfaceCompleted(this, new ShutdownConnectionsOnInterfaceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetFileTransfers", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetFileTransfersResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetFileTransfersResponse GetFileTransfers([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetFileTransfersRequest GetFileTransfersRequest)
        {
            object[] results = this.Invoke("GetFileTransfers", new object[] {
                        GetFileTransfersRequest});
            return ((GetFileTransfersResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetFileTransfers(GetFileTransfersRequest GetFileTransfersRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetFileTransfers", new object[] {
                        GetFileTransfersRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetFileTransfersResponse EndGetFileTransfers(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetFileTransfersResponse)(results[0]));
        }

        /// <remarks/>
        public void GetFileTransfersAsync(GetFileTransfersRequest GetFileTransfersRequest)
        {
            this.GetFileTransfersAsync(GetFileTransfersRequest, null);
        }

        /// <remarks/>
        public void GetFileTransfersAsync(GetFileTransfersRequest GetFileTransfersRequest, object userState)
        {
            if ((this.GetFileTransfersOperationCompleted == null))
            {
                this.GetFileTransfersOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetFileTransfersOperationCompleted);
            }
            this.InvokeAsync("GetFileTransfers", new object[] {
                        GetFileTransfersRequest}, this.GetFileTransfersOperationCompleted, userState);
        }

        private void OnGetFileTransfersOperationCompleted(object arg)
        {
            if ((this.GetFileTransfersCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetFileTransfersCompleted(this, new GetFileTransfersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GetLogMessages", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetLogMessagesResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GetLogMessagesResponse GetLogMessages([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GetLogMessagesRequest GetLogMessagesRequest)
        {
            object[] results = this.Invoke("GetLogMessages", new object[] {
                        GetLogMessagesRequest});
            return ((GetLogMessagesResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetLogMessages(GetLogMessagesRequest GetLogMessagesRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetLogMessages", new object[] {
                        GetLogMessagesRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetLogMessagesResponse EndGetLogMessages(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetLogMessagesResponse)(results[0]));
        }

        /// <remarks/>
        public void GetLogMessagesAsync(GetLogMessagesRequest GetLogMessagesRequest)
        {
            this.GetLogMessagesAsync(GetLogMessagesRequest, null);
        }

        /// <remarks/>
        public void GetLogMessagesAsync(GetLogMessagesRequest GetLogMessagesRequest, object userState)
        {
            if ((this.GetLogMessagesOperationCompleted == null))
            {
                this.GetLogMessagesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLogMessagesOperationCompleted);
            }
            this.InvokeAsync("GetLogMessages", new object[] {
                        GetLogMessagesRequest}, this.GetLogMessagesOperationCompleted, userState);
        }

        private void OnGetLogMessagesOperationCompleted(object arg)
        {
            if ((this.GetLogMessagesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLogMessagesCompleted(this, new GetLogMessagesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/BlockAddress", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("BlockAddressResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public BlockAddressResponse BlockAddress([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] BlockAddressRequest BlockAddressRequest)
        {
            object[] results = this.Invoke("BlockAddress", new object[] {
                        BlockAddressRequest});
            return ((BlockAddressResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginBlockAddress(BlockAddressRequest BlockAddressRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("BlockAddress", new object[] {
                        BlockAddressRequest}, callback, asyncState);
        }

        /// <remarks/>
        public BlockAddressResponse EndBlockAddress(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((BlockAddressResponse)(results[0]));
        }

        /// <remarks/>
        public void BlockAddressAsync(BlockAddressRequest BlockAddressRequest)
        {
            this.BlockAddressAsync(BlockAddressRequest, null);
        }

        /// <remarks/>
        public void BlockAddressAsync(BlockAddressRequest BlockAddressRequest, object userState)
        {
            if ((this.BlockAddressOperationCompleted == null))
            {
                this.BlockAddressOperationCompleted = new System.Threading.SendOrPostCallback(this.OnBlockAddressOperationCompleted);
            }
            this.InvokeAsync("BlockAddress", new object[] {
                        BlockAddressRequest}, this.BlockAddressOperationCompleted, userState);
        }

        private void OnBlockAddressOperationCompleted(object arg)
        {
            if ((this.BlockAddressCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.BlockAddressCompleted(this, new BlockAddressCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/GenerateStatistics", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GenerateStatisticsResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public GenerateStatisticsResponse GenerateStatistics([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] GenerateStatisticsRequest GenerateStatisticsRequest)
        {
            object[] results = this.Invoke("GenerateStatistics", new object[] {
                        GenerateStatisticsRequest});
            return ((GenerateStatisticsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGenerateStatistics(GenerateStatisticsRequest GenerateStatisticsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GenerateStatistics", new object[] {
                        GenerateStatisticsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GenerateStatisticsResponse EndGenerateStatistics(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GenerateStatisticsResponse)(results[0]));
        }

        /// <remarks/>
        public void GenerateStatisticsAsync(GenerateStatisticsRequest GenerateStatisticsRequest)
        {
            this.GenerateStatisticsAsync(GenerateStatisticsRequest, null);
        }

        /// <remarks/>
        public void GenerateStatisticsAsync(GenerateStatisticsRequest GenerateStatisticsRequest, object userState)
        {
            if ((this.GenerateStatisticsOperationCompleted == null))
            {
                this.GenerateStatisticsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGenerateStatisticsOperationCompleted);
            }
            this.InvokeAsync("GenerateStatistics", new object[] {
                        GenerateStatisticsRequest}, this.GenerateStatisticsOperationCompleted, userState);
        }

        private void OnGenerateStatisticsOperationCompleted(object arg)
        {
            if ((this.GenerateStatisticsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GenerateStatisticsCompleted(this, new GenerateStatisticsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/BackupServerConfiguration", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("BackupServerConfigurationResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public BackupServerConfigurationResponse BackupServerConfiguration([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] BackupServerConfigurationRequest BackupServerConfigurationRequest)
        {
            object[] results = this.Invoke("BackupServerConfiguration", new object[] {
                        BackupServerConfigurationRequest});
            return ((BackupServerConfigurationResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginBackupServerConfiguration(BackupServerConfigurationRequest BackupServerConfigurationRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("BackupServerConfiguration", new object[] {
                        BackupServerConfigurationRequest}, callback, asyncState);
        }

        /// <remarks/>
        public BackupServerConfigurationResponse EndBackupServerConfiguration(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((BackupServerConfigurationResponse)(results[0]));
        }

        /// <remarks/>
        public void BackupServerConfigurationAsync(BackupServerConfigurationRequest BackupServerConfigurationRequest)
        {
            this.BackupServerConfigurationAsync(BackupServerConfigurationRequest, null);
        }

        /// <remarks/>
        public void BackupServerConfigurationAsync(BackupServerConfigurationRequest BackupServerConfigurationRequest, object userState)
        {
            if ((this.BackupServerConfigurationOperationCompleted == null))
            {
                this.BackupServerConfigurationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnBackupServerConfigurationOperationCompleted);
            }
            this.InvokeAsync("BackupServerConfiguration", new object[] {
                        BackupServerConfigurationRequest}, this.BackupServerConfigurationOperationCompleted, userState);
        }

        private void OnBackupServerConfigurationOperationCompleted(object arg)
        {
            if ((this.BackupServerConfigurationCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.BackupServerConfigurationCompleted(this, new BackupServerConfigurationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cerberusllc.com/service/cerberusftpservice/RestoreServerConfiguration", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RestoreServerConfigurationResponse", Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
        public RestoreServerConfigurationResponse RestoreServerConfiguration([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://cerberusllc.com/service/cerberusftpservice")] RestoreServerConfigurationRequest RestoreServerConfigurationRequest)
        {
            object[] results = this.Invoke("RestoreServerConfiguration", new object[] {
                        RestoreServerConfigurationRequest});
            return ((RestoreServerConfigurationResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRestoreServerConfiguration(RestoreServerConfigurationRequest RestoreServerConfigurationRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("RestoreServerConfiguration", new object[] {
                        RestoreServerConfigurationRequest}, callback, asyncState);
        }

        /// <remarks/>
        public RestoreServerConfigurationResponse EndRestoreServerConfiguration(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((RestoreServerConfigurationResponse)(results[0]));
        }

        /// <remarks/>
        public void RestoreServerConfigurationAsync(RestoreServerConfigurationRequest RestoreServerConfigurationRequest)
        {
            this.RestoreServerConfigurationAsync(RestoreServerConfigurationRequest, null);
        }

        /// <remarks/>
        public void RestoreServerConfigurationAsync(RestoreServerConfigurationRequest RestoreServerConfigurationRequest, object userState)
        {
            if ((this.RestoreServerConfigurationOperationCompleted == null))
            {
                this.RestoreServerConfigurationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRestoreServerConfigurationOperationCompleted);
            }
            this.InvokeAsync("RestoreServerConfiguration", new object[] {
                        RestoreServerConfigurationRequest}, this.RestoreServerConfigurationOperationCompleted, userState);
        }

        private void OnRestoreServerConfigurationOperationCompleted(object arg)
        {
            if ((this.RestoreServerConfigurationCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RestoreServerConfigurationCompleted(this, new RestoreServerConfigurationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetBackupServersRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class AuthenticatedRequest
    {

        /// <uwagi/>
        public Credentials credentials;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Credentials
    {

        /// <uwagi/>
        public string user;

        /// <uwagi/>
        public string password;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class ImportFileResult
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string file;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string message;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool success;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class LogMessage
    {

        /// <uwagi/>
        public string msg;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ulong id;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public int type;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public System.DateTime time;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class FileTransfer
    {

        /// <uwagi/>
        public string localFilename;

        /// <uwagi/>
        public string remoteFilename;

        /// <uwagi/>
        public string user;

        /// <uwagi/>
        public ulong percentElapsed;

        /// <uwagi/>
        public ulong currentPosition;

        /// <uwagi/>
        public ulong totalSize;

        /// <uwagi/>
        public double transferRate;

        /// <uwagi/>
        public string timeLeft;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ulong ID;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public TransferType type;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool isSecure;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum TransferType
    {

        /// <uwagi/>
        Download,

        /// <uwagi/>
        Upload,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Features
    {

        /// <uwagi/>
        public int maxConnectionLimit;

        /// <uwagi/>
        public bool enableActiveDirectory;

        /// <uwagi/>
        public bool enableLDAPAuthentication;

        /// <uwagi/>
        public bool enableFIPS1402;

        /// <uwagi/>
        public bool enableRemoteAdmin;

        /// <uwagi/>
        public bool enableClientCertificateVerify;

        /// <uwagi/>
        public bool enableSshFtp;

        /// <uwagi/>
        public bool enableHttp;

        /// <uwagi/>
        public bool enableEvents;

        /// <uwagi/>
        public bool enableReports;

        /// <uwagi/>
        public bool enableServerSync;

        /// <uwagi/>
        public int productEditionCode;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class FileHitInfo
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string user;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public TransferType type;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class FileHit
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("list")]
        public FileHitInfo[] list;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string file;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class PassiveOpts
    {

        /// <uwagi/>
        public string ipAddress;

        /// <uwagi/>
        public string dnsAddress;

        /// <uwagi/>
        public bool dontUseExternalIPForLocal;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public PassiveMode mode;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum PassiveMode
    {

        /// <uwagi/>
        Auto,

        /// <uwagi/>
        DirectIP,

        /// <uwagi/>
        DNS,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class InterfaceOpts
    {

        /// <uwagi/>
        public bool isActive;

        /// <uwagi/>
        public bool allowLogin;

        /// <uwagi/>
        public uint listenPort;

        /// <uwagi/>
        public int connectionLimit;

        /// <uwagi/>
        public PassiveOpts passiveSettings;

        /// <uwagi/>
        public bool requiresSecureControl;

        /// <uwagi/>
        public bool requiresSecureData;

        /// <uwagi/>
        public string logoPath;

        /// <uwagi/>
        public string loginIconPath;

        /// <uwagi/>
        public string companyName;

        /// <uwagi/>
        public bool allowWebAccountRequest;

        /// <uwagi/>
        public bool showWelcomeMsg;

        /// <uwagi/>
        public bool redirectToHttps;

        /// <uwagi/>
        public int defaultWebDirList;

        /// <uwagi/>
        public bool showTimezone;

        /// <uwagi/>
        public bool showLocalTime;

        /// <uwagi/>
        public bool allowUpdate;

        /// <uwagi/>
        public bool useHSTS;

        /// <uwagi/>
        public string captchaPrivKey;

        /// <uwagi/>
        public string captchaPubKey;

        /// <uwagi/>
        public bool captchaShowLogin;

        /// <uwagi/>
        public bool captchaShowReq;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Interface
    {

        /// <uwagi/>
        public long id;

        /// <uwagi/>
        public InterfaceOpts options;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ipAddress;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public uint type;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class LicenseInfo
    {

        /// <uwagi/>
        public string name;

        /// <uwagi/>
        public string companyName;

        /// <uwagi/>
        public ulong issuedDate;

        /// <uwagi/>
        public int upgradeTimeRemaining;

        /// <uwagi/>
        public int clientCount;

        /// <uwagi/>
        public int daysValid;

        /// <uwagi/>
        public ulong installedDate;

        /// <uwagi/>
        public bool isValid;

        /// <uwagi/>
        public bool isForCompanyUse;

        /// <uwagi/>
        public string productEdition;

        /// <uwagi/>
        public string errorMessage;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Connection
    {

        /// <uwagi/>
        public ulong id;

        /// <uwagi/>
        public long interfaceID;

        /// <uwagi/>
        public string userName;

        /// <uwagi/>
        public string lastCommand;

        /// <uwagi/>
        public bool isSecure;

        /// <uwagi/>
        public string ipAddr;

        /// <uwagi/>
        public System.DateTime loginTime;

        /// <uwagi/>
        public string commandProgress;

        /// <uwagi/>
        public string client;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Group
    {

        /// <uwagi/>
        public UserPropertyAuthentication authMethod;

        /// <uwagi/>
        public ProtocolsAllowed protocols;

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("root", IsNullable = false)]
        public VirtualDirectory[] rootList;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool isAllowPasswordChange;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isAllowPasswordChangeSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool isAnonymous;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isAnonymousSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool isSimpleDirectoryMode;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isSimpleDirectoryModeSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool isDisabled;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isDisabledSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public int maxLoginsAllowed;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxLoginsAllowedSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool requireSecureControl;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool requireSecureControlSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool requireSecureData;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool requireSecureDataSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public System.DateTime disableAfterTime;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool disableAfterTimeSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ulong maxUploadFilesize;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxUploadFilesizeSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string ipAllowedList;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string desc;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string name;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class UserPropertyAuthentication
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public AuthenticationMethod method;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool methodSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public UserPropertyPriority priority;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum AuthenticationMethod
    {

        /// <uwagi/>
        password,

        /// <uwagi/>
        public_key,

        /// <uwagi/>
        password_and_public_key,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum UserPropertyPriority
    {

        /// <uwagi/>
        user,

        /// <uwagi/>
        group,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class ProtocolsAllowed
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public UserPropertyPriority priority;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool ftp;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool ftps;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool sftp;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool http;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool https;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class VirtualDirectory
    {

        /// <uwagi/>
        public string name;

        /// <uwagi/>
        public string path;

        /// <uwagi/>
        public DirectoryPermissions permissions;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class DirectoryPermissions
    {

        /// <uwagi/>
        public bool allowListFile;

        /// <uwagi/>
        public bool allowListDir;

        /// <uwagi/>
        public bool allowDownload;

        /// <uwagi/>
        public bool allowUpload;

        /// <uwagi/>
        public bool allowRename;

        /// <uwagi/>
        public bool allowDelete;

        /// <uwagi/>
        public bool allowDirectoryCreation;

        /// <uwagi/>
        public bool allowDisplayHidden;

        /// <uwagi/>
        public bool allowZip;

        /// <uwagi/>
        public bool allowUnzip;

        /// <uwagi/>
        public bool allowShare;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class groupMember
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string name;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class UserPropertyString
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string value;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public UserPropertyPriority priority;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class UserPropertyULong
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ulong value;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public UserPropertyPriority priority;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class UserPropertyDateTime
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public System.DateTime value;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public UserPropertyPriority priority;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class UserPropertyInt
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public int value;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public UserPropertyPriority priority;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class UserPropertyBool
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool value;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public UserPropertyPriority priority;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Password
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string value;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public PasswordType type;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool noExpire;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public System.DateTime lastChange;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum PasswordType
    {

        /// <uwagi/>
        plain,

        /// <uwagi/>
        sha1,

        /// <uwagi/>
        sha256,

        /// <uwagi/>
        sha512,

        /// <uwagi/>
        md5,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class User
    {

        /// <uwagi/>
        public Password password;

        /// <uwagi/>
        public UserPropertyBool isAllowPasswordChange;

        /// <uwagi/>
        public UserPropertyBool isAnonymous;

        /// <uwagi/>
        public UserPropertyBool isSimpleDirectoryMode;

        /// <uwagi/>
        public UserPropertyBool isDisabled;

        /// <uwagi/>
        public UserPropertyInt maxLoginsAllowed;

        /// <uwagi/>
        public UserPropertyBool requireSecureControl;

        /// <uwagi/>
        public UserPropertyBool requireSecureData;

        /// <uwagi/>
        public UserPropertyDateTime disableAfterTime;

        /// <uwagi/>
        public UserPropertyAuthentication authMethod;

        /// <uwagi/>
        public ProtocolsAllowed protocols;

        /// <uwagi/>
        public UserPropertyULong maxUploadFilesize;

        /// <uwagi/>
        public UserPropertyString ipAllowedList;

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("group", IsNullable = false)]
        public groupMember[] groupList;

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("root", IsNullable = false)]
        public VirtualDirectory[] rootList;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string fname;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string sname;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string email;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string tel;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string desc;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string name;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class NewWANIP
    {

        /// <uwagi/>
        public string ip;

        /// <uwagi/>
        public bool overridePassiveMode;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class AuthenticationType
    {

        /// <uwagi/>
        public bool enabled;

        /// <uwagi/>
        public string name;

        /// <uwagi/>
        public string type;

        /// <uwagi/>
        public string description;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Statistics
    {

        /// <uwagi/>
        public ulong downloads;

        /// <uwagi/>
        public ulong uploads;

        /// <uwagi/>
        public ulong failedDownloads;

        /// <uwagi/>
        public ulong failedUploads;

        /// <uwagi/>
        public ulong totalConnections;

        /// <uwagi/>
        public ulong currentConnections;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Status
    {

        /// <uwagi/>
        public bool isStarted;

        /// <uwagi/>
        public Statistics stats;

        /// <uwagi/>
        public double downBandwidth;

        /// <uwagi/>
        public double upBandwidth;

        /// <uwagi/>
        public ulong totalConnections;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ulong activeListenerConnections;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool activeListenerConnectionsSpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class ServerInformation
    {

        /// <uwagi/>
        public Version version;

        /// <uwagi/>
        public string hostname;

        /// <uwagi/>
        public bool isStarted;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool isSuccess;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isSuccessSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class Version
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public int maj;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public int min;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public int maint;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public int build;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class ChangeDescription
    {

        /// <uwagi/>
        public Version ver;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("item")]
        public string[] item;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public uint s;
    }

    /// <uwagi/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionUpdateInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class VersionInfo
    {

        /// <uwagi/>
        public Version ver;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public System.DateTime buildDate;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ProductStatus status;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public ProcessArch arch;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public double minOS;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum ProductStatus
    {

        /// <uwagi/>
        production,

        /// <uwagi/>
        beta,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum ProcessArch
    {

        /// <uwagi/>
        Unknown,

        /// <uwagi/>
        I386,

        /// <uwagi/>
        Ia64,

        /// <uwagi/>
        Amd64,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class VersionUpdateInfo : VersionInfo
    {

        /// <uwagi/>
        public string downloadURL;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("changeDescList")]
        public ChangeDescription[] changeDescList;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class UpdateInformation
    {

        /// <uwagi/>
        public VersionInfo currentVer;

        /// <uwagi/>
        public VersionUpdateInfo latestVer;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public System.DateTime lastChecked;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class ServerSummaryInfo
    {

        /// <uwagi/>
        public bool isSslEnabled;

        /// <uwagi/>
        public string sslKeyType;

        /// <uwagi/>
        public uint sslKeyBits;

        /// <uwagi/>
        public uint sslCipherMinSymmetricBits;

        /// <uwagi/>
        public bool isClientVerifyEnabled;

        /// <uwagi/>
        public bool isFipsEnabled;

        /// <uwagi/>
        public bool isSoapWebEnabled;

        /// <uwagi/>
        public bool isSoapSecure;

        /// <uwagi/>
        public uint soapPort;

        /// <uwagi/>
        public string ipPublic;

        /// <uwagi/>
        public InterfaceStatus ftpStatus;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("ftpStatusMsgs")]
        public string[] ftpStatusMsgs;

        /// <uwagi/>
        public InterfaceStatus sftpStatus;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("sftpStatusMsgs")]
        public string[] sftpStatusMsgs;

        /// <uwagi/>
        public InterfaceStatus httpStatus;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("httpStatusMsgs")]
        public string[] httpStatusMsgs;

        /// <uwagi/>
        public bool hipaaCompliant;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("hipaaCompliantMsgs")]
        public string[] hipaaCompliantMsgs;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("generalMsgs")]
        public string[] generalMsgs;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("securityMsgs")]
        public string[] securityMsgs;

        /// <uwagi/>
        public VulnerabilityAssessmentStatus vulnerabilityStatus;

        /// <uwagi/>
        public UpdateInformation updateInfo;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum InterfaceStatus
    {

        /// <uwagi/>
        Secure,

        /// <uwagi/>
        NotSecure,

        /// <uwagi/>
        Disabled,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum VulnerabilityAssessmentStatus
    {

        /// <uwagi/>
        None,

        /// <uwagi/>
        Detected,

        /// <uwagi/>
        Disabled,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class MimeMapping
    {

        /// <uwagi/>
        public string ext;

        /// <uwagi/>
        public string type;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class DbOperationResult
    {

        /// <uwagi/>
        public bool success;

        /// <uwagi/>
        public string label;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class DbDriverDescription
    {

        /// <uwagi/>
        public string name;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class DbConfig
    {

        /// <uwagi/>
        public DbDriverDescription driver;

        /// <uwagi/>
        public string server;

        /// <uwagi/>
        public uint port;

        /// <uwagi/>
        public string username;

        /// <uwagi/>
        public string password;

        /// <uwagi/>
        public string databaseName;

        /// <uwagi/>
        public string databasePath;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class IpAccessRange
    {

        /// <uwagi/>
        public string addressFrom;

        /// <uwagi/>
        public string addressTo;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public IpAccessRangeType type;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum IpAccessRangeType
    {

        /// <uwagi/>
        single,

        /// <uwagi/>
        range,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class IpAccessEntry
    {

        /// <uwagi/>
        public IpAccessRange entry;

        /// <uwagi/>
        public string note;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public System.DateTime blockedSince;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool blockedSinceSpecified;

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public int blockForMinutes;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool blockForMinutesSpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class SyncServer
    {

        /// <uwagi/>
        public string host;

        /// <uwagi/>
        public ushort port;

        /// <uwagi/>
        public bool useSSL;

        /// <uwagi/>
        public string username;

        /// <uwagi/>
        public string password;

        /// <uwagi/>
        public bool enableSync;

        /// <uwagi/>
        public SyncStatus lastSyncStatus;

        /// <uwagi/>
        public System.DateTime lastSyncTime;

        /// <uwagi/>
        public string lastSyncStatusMessage;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public enum SyncStatus
    {

        /// <uwagi/>
        unknown,

        /// <uwagi/>
        success,

        /// <uwagi/>
        fail,
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class SyncServerConfig
    {

        /// <uwagi/>
        public bool enable;

        /// <uwagi/>
        public uint syncIntervalMinutes;

        /// <uwagi/>
        public bool syncOnServerChange;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cerberusllc.com/common")]
    public partial class SyncServerType
    {

        /// <uwagi/>
        public SyncServerConfig config;

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("server", IsNullable = false)]
        public SyncServer[] servers;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetBackupServersResponse
    {

        /// <uwagi/>
        public SyncServerType config;

        /// <uwagi/>
        public string message;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveBackupServersRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public SyncServerType config;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveBackupServersResponse
    {

        /// <uwagi/>
        public string message;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SharePublicFileRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string username;

        /// <uwagi/>
        public string password;

        /// <uwagi/>
        public string shareRemoteFilePath;

        /// <uwagi/>
        public int shareDurationHours;

        /// <uwagi/>
        public string sharePassword;

        /// <uwagi/>
        public bool shareDeleteOnExpire;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool shareDeleteOnExpireSpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SharePublicFileResponse
    {

        /// <uwagi/>
        public bool success;

        /// <uwagi/>
        public string link;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class AddIpRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("ipEntry")]
        public IpAccessEntry[] ipEntry;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class AddIpResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string errorMsg;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteIpRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("ipEntry")]
        public IpAccessRange[] ipEntry;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteIpResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string errorMsg;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class TestAndVerifyDatabaseRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public DbConfig config;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class TestAndVerifyDatabaseResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("result", Namespace = "http://cerberusllc.com/common")]
        public DbOperationResult[] info;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class CreateStatisticsDatabaseRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public DbConfig config;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class CreateStatisticsDatabaseResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("result", Namespace = "http://cerberusllc.com/common")]
        public DbOperationResult[] info;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DropStatisticsDatabaseRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public DbConfig config;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DropStatisticsDatabaseResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("result", Namespace = "http://cerberusllc.com/common")]
        public DbOperationResult[] info;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetMimeMappingsRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetMimeMappingsResponse
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("map", Namespace = "http://cerberusllc.com/common")]
        public MimeMapping[] mime;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveMimeMappingsRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("map", Namespace = "http://cerberusllc.com/common")]
        public MimeMapping[] mime;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveMimeMappingsResponse
    {

        /// <uwagi/>
        public string errorMsg;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ServerSummaryStatusRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ServerSummaryStatusResponse
    {

        /// <uwagi/>
        public ServerSummaryInfo result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ServerInformationRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ServerInformationResponse
    {

        /// <uwagi/>
        public ServerInformation result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class CurrentStatusRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public long activeInterfaceId;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool activeInterfaceIdSpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class CurrentStatusResponse
    {

        /// <uwagi/>
        public Status result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class StartServerRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class StartServerResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class StopServerRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class StopServerResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ServerStartedRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ServerStartedResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class InitializeServerRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class InitializeServerResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ShutdownServerRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ShutdownServerResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetEventRulesRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetEventRulesResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string dataXml;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SetEventRulesRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string dataXml;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SetEventRulesResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteRequestedAccountsRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("guids")]
        public string[] guids;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteRequestedAccountsResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("errors")]
        public string[] errors;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetRequestedAccountsRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetRequestedAccountsResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string dataXml;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SetRequestedAccountsRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string dataXml;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SetRequestedAccountsResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetAuthenticationListRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetAuthenticationListResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string dataXml;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("authenticationList")]
        public AuthenticationType[] authenticationList;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SetAuthenticationListRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string dataXml;

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("authenticationList")]
        public AuthenticationType[] authenticationList;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SetAuthenticationListResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetHostnameRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetHostnameResponse
    {

        /// <uwagi/>
        public string result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SetWANIPRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public NewWANIP newWANInfo;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SetWANIPResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class AddUserRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public User User;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class AddUserResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class AddGroupRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public Group Group;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class AddGroupResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteUserRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string name;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteUserResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteGroupRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string name;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteGroupResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class AddRootRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string userName;

        /// <uwagi/>
        public VirtualDirectory Root;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class AddRootResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteRootRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string userName;

        /// <uwagi/>
        public string dirName;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class DeleteRootResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetUserListRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetUserListResponse
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("UserList")]
        public string[] UserList;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetGroupListRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetGroupListResponse
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("GroupList")]
        public string[] GroupList;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetUserInformationRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string userName;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetUserInformationResponse
    {

        /// <uwagi/>
        public User UserInformation;

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetConnectedUserListRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetConnectedUserListResponse
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("ConnectionList")]
        public Connection[] ConnectionList;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ChangePasswordRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string userName;

        /// <uwagi/>
        public string oldPassword;

        /// <uwagi/>
        public string newPassword;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ChangePasswordResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class RenameUserRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string userName;

        /// <uwagi/>
        public string newUserName;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class RenameUserResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public string message;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class TerminateConnectionRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public long ConnectionID;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class TerminateConnectionResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetProfilesRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetProfilesResponse
    {

        /// <uwagi/>
        public string data;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetGroupsRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetGroupsResponse
    {

        /// <uwagi/>
        public string data;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetConfigurationRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetConfigurationResponse
    {

        /// <uwagi/>
        public string data;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetInterfacesRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetInterfacesResponse
    {

        /// <uwagi/>
        public string data;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetIPBlockListRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetIPBlockListResponse
    {

        /// <uwagi/>
        public string data;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetAutoBlockListRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetAutoBlockListResponse
    {

        /// <uwagi/>
        public string data;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetAppPathsRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetAppPathsResponse
    {

        /// <uwagi/>
        public string AppDataPath;

        /// <uwagi/>
        public string AppInstallPath;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetLicenseInfoRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetLicenseInfoResponse
    {

        /// <uwagi/>
        public LicenseInfo LicenseInformation;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class VerifyLicenseRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string LicenseString;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class VerifyLicenseResponse
    {

        /// <uwagi/>
        public LicenseInfo LicenseInformation;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetCurrentConnectionCountRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public long InterfaceID;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetCurrentConnectionCountResponse
    {

        /// <uwagi/>
        public long result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetAllCurrentConnectionCountRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetAllCurrentConnectionCountResponse
    {

        /// <uwagi/>
        public long result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetInterfaceByIDRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public long InterfaceID;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetInterfaceResponse
    {

        /// <uwagi/>
        public Interface Interface;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetInterfaceListRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetInterfaceListResponse
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("InterfaceList")]
        public Interface[] InterfaceList;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class InitializeInterfaceRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public long InterfaceID;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class InitializeInterfaceResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ShutdownInterfaceRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public long InterfaceID;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ShutdownInterfaceResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetStatisticsRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public bool includeFileStats;

        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool includeFileStatsSpecified;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetStatisticsResponse
    {

        /// <uwagi/>
        public bool result;

        /// <uwagi/>
        public Statistics stats;

        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("list", Namespace = "http://cerberusllc.com/common", IsNullable = false)]
        public FileHit[] fileStats;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetCurrentBandwidthRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetCurrentBandwidthResponse
    {

        /// <uwagi/>
        public double up;

        /// <uwagi/>
        public double down;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetFeaturesRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetFeaturesResponse
    {

        /// <uwagi/>
        public Features Features;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveProfilesRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string ProfilesXML;

        /// <uwagi/>
        public string GroupsXML;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveProfilesResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveConfigurationRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string ConfigXML;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveConfigurationResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class CommitSettingsRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("InterfaceList")]
        public Interface[] InterfaceList;

        /// <uwagi/>
        public string ConfigXML;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class CommitSettingsResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveBlockListRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string IPBlockList;

        /// <uwagi/>
        public string AutoBlockListXML;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class SaveBlockListResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ModifyInterfaceRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public long InterfaceID;

        /// <uwagi/>
        public InterfaceOpts Opts;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ModifyInterfaceResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ShutdownConnectionsOnInterfaceRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public long InterfaceID;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class ShutdownConnectionsOnInterfaceResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetFileTransfersRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetFileTransfersResponse
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("transferList")]
        public FileTransfer[] transferList;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetLogMessagesRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GetLogMessagesResponse
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("logList")]
        public LogMessage[] logList;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class BlockAddressRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string ipaddress;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class BlockAddressResponse
    {

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GenerateStatisticsRequest : AuthenticatedRequest
    {
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class GenerateStatisticsResponse
    {

        /// <uwagi/>
        public string filePath;

        /// <uwagi/>
        public string errorMsg;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class BackupServerConfigurationRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string filePath;

        /// <uwagi/>
        public string password;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class BackupServerConfigurationResponse
    {

        /// <uwagi/>
        public string filePath;

        /// <uwagi/>
        public string errorMsg;

        /// <uwagi/>
        public bool result;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class RestoreServerConfigurationRequest : AuthenticatedRequest
    {

        /// <uwagi/>
        public string filePath;

        /// <uwagi/>
        public string password;
    }

    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cerberusllc.com/service/cerberusftpservice")]
    public partial class RestoreServerConfigurationResponse
    {

        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("importResult")]
        public ImportFileResult[] importResult;

        /// <uwagi/>
        public string errorMsg;

        /// <uwagi/>
        public bool result;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetBackupServersCompletedEventHandler(object sender, GetBackupServersCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetBackupServersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetBackupServersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetBackupServersResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetBackupServersResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SaveBackupServersCompletedEventHandler(object sender, SaveBackupServersCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveBackupServersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SaveBackupServersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SaveBackupServersResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SaveBackupServersResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SharePublicFileCompletedEventHandler(object sender, SharePublicFileCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SharePublicFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SharePublicFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SharePublicFileResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SharePublicFileResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void AddIpCompletedEventHandler(object sender, AddIpCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddIpCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AddIpCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public AddIpResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((AddIpResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DeleteIpCompletedEventHandler(object sender, DeleteIpCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteIpCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteIpCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DeleteIpResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DeleteIpResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void TestAndVerifyDatabaseCompletedEventHandler(object sender, TestAndVerifyDatabaseCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TestAndVerifyDatabaseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal TestAndVerifyDatabaseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public TestAndVerifyDatabaseResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((TestAndVerifyDatabaseResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void CreateStatisticsDatabaseCompletedEventHandler(object sender, CreateStatisticsDatabaseCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateStatisticsDatabaseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CreateStatisticsDatabaseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CreateStatisticsDatabaseResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CreateStatisticsDatabaseResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DropStatisticsDatabaseCompletedEventHandler(object sender, DropStatisticsDatabaseCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DropStatisticsDatabaseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DropStatisticsDatabaseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DropStatisticsDatabaseResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DropStatisticsDatabaseResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetMimeMappingsCompletedEventHandler(object sender, GetMimeMappingsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetMimeMappingsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetMimeMappingsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetMimeMappingsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetMimeMappingsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SaveMimeMappingsCompletedEventHandler(object sender, SaveMimeMappingsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveMimeMappingsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SaveMimeMappingsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SaveMimeMappingsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SaveMimeMappingsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ServerSummaryStatusCompletedEventHandler(object sender, ServerSummaryStatusCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServerSummaryStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ServerSummaryStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ServerSummaryStatusResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ServerSummaryStatusResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ServerInformationCompletedEventHandler(object sender, ServerInformationCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServerInformationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ServerInformationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ServerInformationResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ServerInformationResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void CurrentStatusCompletedEventHandler(object sender, CurrentStatusCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CurrentStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CurrentStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CurrentStatusResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CurrentStatusResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void StartServerCompletedEventHandler(object sender, StartServerCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class StartServerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal StartServerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public StartServerResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((StartServerResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void StopServerCompletedEventHandler(object sender, StopServerCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class StopServerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal StopServerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public StopServerResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((StopServerResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ServerStartedCompletedEventHandler(object sender, ServerStartedCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServerStartedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ServerStartedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ServerStartedResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ServerStartedResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void InitializeServerCompletedEventHandler(object sender, InitializeServerCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InitializeServerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal InitializeServerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public InitializeServerResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((InitializeServerResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ShutdownServerCompletedEventHandler(object sender, ShutdownServerCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ShutdownServerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ShutdownServerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ShutdownServerResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ShutdownServerResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetEventRulesCompletedEventHandler(object sender, GetEventRulesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetEventRulesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetEventRulesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetEventRulesResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetEventRulesResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SetEventRulesCompletedEventHandler(object sender, SetEventRulesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetEventRulesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetEventRulesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SetEventRulesResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SetEventRulesResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DeleteRequestedAccountsCompletedEventHandler(object sender, DeleteRequestedAccountsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteRequestedAccountsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteRequestedAccountsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DeleteRequestedAccountsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DeleteRequestedAccountsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetRequestedAccountsCompletedEventHandler(object sender, GetRequestedAccountsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRequestedAccountsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetRequestedAccountsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetRequestedAccountsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetRequestedAccountsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SetRequestedAccountsCompletedEventHandler(object sender, SetRequestedAccountsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetRequestedAccountsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetRequestedAccountsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SetRequestedAccountsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SetRequestedAccountsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetAuthenticationListCompletedEventHandler(object sender, GetAuthenticationListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAuthenticationListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetAuthenticationListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetAuthenticationListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetAuthenticationListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SetAuthenticationListCompletedEventHandler(object sender, SetAuthenticationListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetAuthenticationListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetAuthenticationListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SetAuthenticationListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SetAuthenticationListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetHostnameCompletedEventHandler(object sender, GetHostnameCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetHostnameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetHostnameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetHostnameResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetHostnameResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SetWANIPCompletedEventHandler(object sender, SetWANIPCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetWANIPCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetWANIPCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SetWANIPResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SetWANIPResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void AddUserCompletedEventHandler(object sender, AddUserCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AddUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public AddUserResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((AddUserResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void AddGroupCompletedEventHandler(object sender, AddGroupCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddGroupCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AddGroupCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public AddGroupResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((AddGroupResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DeleteUserCompletedEventHandler(object sender, DeleteUserCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DeleteUserResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DeleteUserResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DeleteGroupCompletedEventHandler(object sender, DeleteGroupCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteGroupCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteGroupCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DeleteGroupResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DeleteGroupResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void AddRootCompletedEventHandler(object sender, AddRootCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddRootCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AddRootCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public AddRootResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((AddRootResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void DeleteRootCompletedEventHandler(object sender, DeleteRootCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteRootCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteRootCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DeleteRootResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DeleteRootResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetUserListCompletedEventHandler(object sender, GetUserListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetUserListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetUserListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetUserListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetUserListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetGroupListCompletedEventHandler(object sender, GetGroupListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetGroupListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetGroupListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetGroupListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetGroupListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetUserInformationCompletedEventHandler(object sender, GetUserInformationCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetUserInformationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetUserInformationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetUserInformationResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetUserInformationResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetConnectedUserListCompletedEventHandler(object sender, GetConnectedUserListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetConnectedUserListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetConnectedUserListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetConnectedUserListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetConnectedUserListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ChangePasswordCompletedEventHandler(object sender, ChangePasswordCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ChangePasswordCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ChangePasswordCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ChangePasswordResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ChangePasswordResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void RenameUserCompletedEventHandler(object sender, RenameUserCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RenameUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal RenameUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public RenameUserResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((RenameUserResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void TerminateConnectionCompletedEventHandler(object sender, TerminateConnectionCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TerminateConnectionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal TerminateConnectionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public TerminateConnectionResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((TerminateConnectionResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetProfilesCompletedEventHandler(object sender, GetProfilesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetProfilesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetProfilesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetProfilesResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetProfilesResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetGroupsCompletedEventHandler(object sender, GetGroupsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetGroupsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetGroupsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetGroupsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetGroupsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetConfigurationCompletedEventHandler(object sender, GetConfigurationCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetConfigurationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetConfigurationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetConfigurationResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetConfigurationResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetInterfacesCompletedEventHandler(object sender, GetInterfacesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetInterfacesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetInterfacesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetInterfacesResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetInterfacesResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetIPBlockListCompletedEventHandler(object sender, GetIPBlockListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetIPBlockListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetIPBlockListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetIPBlockListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetIPBlockListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetAutoBlockListCompletedEventHandler(object sender, GetAutoBlockListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAutoBlockListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetAutoBlockListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetAutoBlockListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetAutoBlockListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetAppPathsCompletedEventHandler(object sender, GetAppPathsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAppPathsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetAppPathsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetAppPathsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetAppPathsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetLicenseInfoCompletedEventHandler(object sender, GetLicenseInfoCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLicenseInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetLicenseInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetLicenseInfoResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetLicenseInfoResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void VerifyLicenseCompletedEventHandler(object sender, VerifyLicenseCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VerifyLicenseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal VerifyLicenseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public VerifyLicenseResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((VerifyLicenseResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetCurrentConnectionCountCompletedEventHandler(object sender, GetCurrentConnectionCountCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCurrentConnectionCountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetCurrentConnectionCountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetCurrentConnectionCountResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetCurrentConnectionCountResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetAllCurrentConnectionCountCompletedEventHandler(object sender, GetAllCurrentConnectionCountCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllCurrentConnectionCountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetAllCurrentConnectionCountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetAllCurrentConnectionCountResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetAllCurrentConnectionCountResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetInterfaceByIDCompletedEventHandler(object sender, GetInterfaceByIDCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetInterfaceByIDCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetInterfaceByIDCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetInterfaceResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetInterfaceResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetInterfaceListCompletedEventHandler(object sender, GetInterfaceListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetInterfaceListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetInterfaceListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetInterfaceListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetInterfaceListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void InitializeInterfaceCompletedEventHandler(object sender, InitializeInterfaceCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InitializeInterfaceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal InitializeInterfaceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public InitializeInterfaceResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((InitializeInterfaceResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ShutdownInterfaceCompletedEventHandler(object sender, ShutdownInterfaceCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ShutdownInterfaceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ShutdownInterfaceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ShutdownInterfaceResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ShutdownInterfaceResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetStatisticsCompletedEventHandler(object sender, GetStatisticsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetStatisticsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetStatisticsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetStatisticsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetStatisticsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetCurrentBandwidthCompletedEventHandler(object sender, GetCurrentBandwidthCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCurrentBandwidthCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetCurrentBandwidthCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetCurrentBandwidthResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetCurrentBandwidthResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetFeaturesCompletedEventHandler(object sender, GetFeaturesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetFeaturesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetFeaturesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetFeaturesResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetFeaturesResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SaveProfilesCompletedEventHandler(object sender, SaveProfilesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveProfilesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SaveProfilesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SaveProfilesResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SaveProfilesResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SaveConfigurationCompletedEventHandler(object sender, SaveConfigurationCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveConfigurationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SaveConfigurationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SaveConfigurationResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SaveConfigurationResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void CommitSettingsCompletedEventHandler(object sender, CommitSettingsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CommitSettingsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CommitSettingsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CommitSettingsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CommitSettingsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SaveBlockListCompletedEventHandler(object sender, SaveBlockListCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveBlockListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SaveBlockListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SaveBlockListResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SaveBlockListResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ModifyInterfaceCompletedEventHandler(object sender, ModifyInterfaceCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ModifyInterfaceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ModifyInterfaceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ModifyInterfaceResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ModifyInterfaceResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ShutdownConnectionsOnInterfaceCompletedEventHandler(object sender, ShutdownConnectionsOnInterfaceCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ShutdownConnectionsOnInterfaceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ShutdownConnectionsOnInterfaceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ShutdownConnectionsOnInterfaceResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ShutdownConnectionsOnInterfaceResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetFileTransfersCompletedEventHandler(object sender, GetFileTransfersCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetFileTransfersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetFileTransfersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetFileTransfersResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetFileTransfersResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetLogMessagesCompletedEventHandler(object sender, GetLogMessagesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLogMessagesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetLogMessagesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetLogMessagesResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetLogMessagesResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void BlockAddressCompletedEventHandler(object sender, BlockAddressCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BlockAddressCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal BlockAddressCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public BlockAddressResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((BlockAddressResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GenerateStatisticsCompletedEventHandler(object sender, GenerateStatisticsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GenerateStatisticsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GenerateStatisticsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GenerateStatisticsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GenerateStatisticsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void BackupServerConfigurationCompletedEventHandler(object sender, BackupServerConfigurationCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BackupServerConfigurationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal BackupServerConfigurationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public BackupServerConfigurationResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((BackupServerConfigurationResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void RestoreServerConfigurationCompletedEventHandler(object sender, RestoreServerConfigurationCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RestoreServerConfigurationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal RestoreServerConfigurationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public RestoreServerConfigurationResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((RestoreServerConfigurationResponse)(this.results[0]));
            }
        }
    }
}

