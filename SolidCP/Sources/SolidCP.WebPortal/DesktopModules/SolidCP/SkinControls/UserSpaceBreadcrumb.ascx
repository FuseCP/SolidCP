<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserSpaceBreadcrumb.ascx.cs" Inherits="SolidCP.Portal.SkinControls.UserSpaceBreadcrumb" %>
<%@ Register TagPrefix="scp" TagName="SpaceOrgsSelector" Src="SpaceOrganizationsSelector.ascx" %>
<div id="Breadcrumb">
    <div class="Path">
        <div class="col-xs-8">
        <asp:Repeater ID="repUsersPath" runat="server" OnItemDataBound="repUsersPath_ItemDataBound"
            EnableViewState="false">
            <ItemTemplate>
                <asp:HyperLink ID="lnkUser" runat="server"></asp:HyperLink>
            </ItemTemplate>
            <SeparatorTemplate>
                <asp:Image ID="imgSep" runat="server" SkinID="PathSeparatorWhite" />
            </SeparatorTemplate>
        </asp:Repeater>

        <span id="spanSpace" runat="server">
            <asp:Image ID="imgSep" runat="server" SkinID="PathSeparatorWhite" />
            <asp:Image ID="Image1" runat="server" SkinID="Space16" />
            <asp:HyperLink ID="lnkSpace" runat="server" Text="SpaceName" NavigateUrl="#"></asp:HyperLink>
        </span>

        <asp:Image ID="imgSep2" runat="server" SkinID="PathSeparatorWhite" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server"></asp:HyperLink>

        <span id="spanOrgn" class="OrgSpan" runat="server">
            <asp:Image ID="imgSep3" runat="server" SkinID="PathSeparatorWhite" />
            <asp:HyperLink ID="lnkOrgn" runat="server">Organization</asp:HyperLink>
            <asp:Image ID="imgSep4" runat="server" SkinID="PathSeparatorWhite" />
            <asp:HyperLink ID="lnkOrgCurPage" runat="server">Home</asp:HyperLink>
        </span>
        </div>
        <scp:SpaceOrgsSelector ID="SpaceOrgsSelector" runat="server" />
    </div>


    <div class="CurrentNode" runat="server" id="CurrentNode">
        <asp:UpdatePanel runat="server" ID="updatePanelUsers" UpdateMode="Conditional" ChildrenAsTriggers="true">
			<ContentTemplate>
	            <asp:Panel ID="pnlViewSpace" runat="server">
                    <asp:Label ID="lblUserAccountName" runat="server" Text="Account-" CssClass="Huge" style="margin-right:2px;"/>
			        <asp:LinkButton ID="cmdSpaceName" runat="server" Text="Change Name" OnClick="cmdChangeName_Click" CssClass="Huge" CausesValidation="false" />
			        <asp:Label ID="lblSpaceDescription" runat="server" Visible="false"></asp:Label>
		        </asp:Panel>
		        <asp:Panel ID="pnlEditSpace" runat="server" DefaultButton="cmdSave" Visible="false">
			        <table cellpadding="0" cellspacing="0">
				        <tr>
					        <td>
						        <asp:TextBox ID="txtName" runat="server" CssClass="Huge" Width="300px"></asp:TextBox>
					        </td>
					        <td rowspan="2" valign="top">
						        &nbsp;&nbsp;<asp:LinkButton ID="cmdSave" runat="server" Text="Save" CssClass="Button" OnClick="cmdSave_Click" ValidationGroup="SpaceName" />
						        <asp:LinkButton ID="cmdCancel" runat="server" Text="Cancel" CssClass="Button" OnClick="cmdCancel_Click" CausesValidation="false" />
					        </td>
				        </tr>
			        </table>
		        </asp:Panel><asp:RequiredFieldValidator ID="valRequireName" runat="server" ControlToValidate="txtName"
			        ErrorMessage="*" Display="Dynamic" ValidationGroup="SpaceName"></asp:RequiredFieldValidator></ContentTemplate>
		</asp:UpdatePanel>
    </div>
</div>
