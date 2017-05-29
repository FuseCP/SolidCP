<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LyncAllocatePhoneNumbers.ascx.cs" Inherits="SolidCP.Portal.Lync.LyncAllocatePhoneNumbers" %>
<%@ Register Src="UserControls/AllocatePackagePhoneNumbers.ascx" TagName="AllocatePackagePhoneNumbers" TagPrefix="scp" %>

<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="UserControls/LyncUserPlanSelector.ascx" TagName="LyncUserPlanSelector" TagPrefix="scp" %>

<%@ Register Src="../UserControls/PackagePhoneNumbers.ascx" TagName="PackagePhoneNumbers" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
			<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="LyncLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </h3>
                        </div>
                <div class="panel-body form-horizontal">
                    <scp:AllocatePackagePhoneNumbers id="allocatePhoneNumbers" runat="server"
                            Pool="PhoneNumbers"
                            ResourceGroup="Lync"
                            ListAddressesControl="lync_phonenumbers" />                   
                </div>
            </div>
        </div>
    </div>
</div>
