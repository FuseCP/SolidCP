<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBPhoneNumbers.ascx.cs" Inherits="SolidCP.Portal.SfB.SfBPhoneNumbers" %>
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
                    <scp:PackagePhoneNumbers id="phoneNumbers" runat="server"
                            Pool="PhoneNumbers"
                            EditItemControl=""
                            SpaceHomeControl=""
                            AllocateAddressesControl="sfb_allocate_phonenumbers"
                            ManageAllowed="true" />
    
                    <br />
                    <scp:CollapsiblePanel id="secQuotas" runat="server"
                        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
    
                    <table cellspacing="6">
                        <tr>
                            <td><asp:Localize ID="locIPQuota" runat="server" meta:resourcekey="locIPQuota" Text="Number of Phone Numbes:"></asp:Localize></td>
                            <td><scp:Quota ID="phoneQuota" runat="server" QuotaName="SfB.PhoneNumbers" /></td>
                        </tr>
                    </table>
    
    
                    </asp:Panel>
                </div>


