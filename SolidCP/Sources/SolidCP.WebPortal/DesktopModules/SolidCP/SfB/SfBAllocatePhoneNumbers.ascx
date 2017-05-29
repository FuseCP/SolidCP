<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBAllocatePhoneNumbers.ascx.cs" Inherits="SolidCP.Portal.SfB.SfBAllocatePhoneNumbers" %>
<%@ Register Src="UserControls/SfBAllocatePackagePhoneNumbers.ascx" TagName="SfBAllocatePackagePhoneNumbers" TagPrefix="scp" %>

<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="UserControls/SfBUserPlanSelector.ascx" TagName="SfBUserPlanSelector" TagPrefix="scp" %>

<%@ Register Src="../UserControls/PackagePhoneNumbers.ascx" TagName="PackagePhoneNumbers" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

                <div class="panel-heading">
                    <asp:Image ID="Image1" SkinID="SfBLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </div>
                <div class="panel-body form-horizontal">
                    <scp:SfBAllocatePackagePhoneNumbers id="allocatePhoneNumbers" runat="server"
                            Pool="PhoneNumbers"
                            ResourceGroup="SfB"
                            ListAddressesControl="sfb_phonenumbers" />                   
                </div>