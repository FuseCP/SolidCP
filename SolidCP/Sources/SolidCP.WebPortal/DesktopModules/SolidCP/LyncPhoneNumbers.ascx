<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LyncPhoneNumbers.ascx.cs" Inherits="SolidCP.Portal.LyncPhoneNumbers" %>
<%@ Register Src="UserControls/PackagePhoneNumbers.ascx" TagName="PackagePhoneNumbers" TagPrefix="scp" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>

<div class="panel-body form-horizontal">
    <scp:PackagePhoneNumbers id="webAddresses" runat="server"
            Pool="PhoneNumbers"
            EditItemControl=""
            SpaceHomeControl=""
            ManageAllowed="true" />
    
    <br />
    <scp:CollapsiblePanel id="secQuotas" runat="server"
        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
    </scp:CollapsiblePanel>
    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
    
    <table cellspacing="6">
        <tr>
            <td><asp:Localize ID="locIPQuota" runat="server" meta:resourcekey="locIPQuota" Text="Number of Phone Numbes:"></asp:Localize></td>
            <td><scp:Quota ID="addressesQuota" runat="server" QuotaName="Lync.PhoneNumbers" /></td>
        </tr>
    </table>
    
    
    </asp:Panel>
</div>

