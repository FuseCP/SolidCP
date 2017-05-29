<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterStats_EditSite.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterStats_EditSite" %>
<table width="100%" cellpadding="3" border="0">
    <tr>
        <td class="SubHead" width="200" nowrap height="25px">
            <asp:Label ID="lblSiteID" runat="server" meta:resourcekey="lblSiteID" Text="Site ID:"></asp:Label>
        </td>
        <td class="NormalBold" width="100%">
            <asp:TextBox id="txtSiteId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Label ID="lblSiteStatus" runat="server" meta:resourcekey="lblSiteStatus" Text="Site Status:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:Literal id="litSiteStatus" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td class="Normal">&nbsp;</td>
    </tr>
    <tr>
        <td class="SubHead" valign="top" colspan="2">
            <asp:Label ID="lblSiteUsers" runat="server" meta:resourcekey="lblSiteUsers" Text="Site Users:"></asp:Label>
        </td>
    </tr>
</table>

<div class="FormButtonsBar">
    <asp:Button id="btnAdd" runat="server" meta:resourcekey="btnAdd" Text="Add User" CssClass="Button2" CausesValidation="false" OnClick="btnAdd_Click"/>
</div>

<asp:GridView id="gvUsers" Runat="server" AutoGenerateColumns="False"
    CssSelectorClass="NormalGridView"
    OnRowCommand="gvUsers_RowCommand" OnRowDataBound="gvUsers_RowDataBound"
    EmptyDataText="gvUsers">
    <columns>
        <asp:TemplateField HeaderText="gvUsersOwner">
            <itemtemplate>
	            <asp:CheckBox ID="chkOwner" runat="server" Checked='<%# Eval("IsOwner") %>' Enabled="false" />
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvUsersAdmin">
            <itemtemplate>
	            <asp:CheckBox ID="chkAdmin" runat="server" Checked='<%# Eval("IsAdmin") %>' />
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvUsersUsername">
            <itemtemplate>
	            <asp:TextBox id="txtUsername" Runat=server Width="90px" CssClass=NormalTextBox Text='<%# Eval("Username") %>'>
	            </asp:TextBox>
	            <asp:RequiredFieldValidator ID="valRequireUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="*"></asp:RequiredFieldValidator>
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvUsersPassword">
            <itemtemplate>
	            <asp:TextBox id="txtPassword" Runat=server Width="90px" CssClass=NormalTextBox Text='<%# Eval("Password") %>'>
	            </asp:TextBox>
	            <asp:RequiredFieldValidator ID="valRequirePassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="*"></asp:RequiredFieldValidator>
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvUsersFirstName">
            <itemtemplate>
	            <asp:TextBox id="txtFirstName" Runat=server Width="90px" CssClass=NormalTextBox Text='<%# Eval("FirstName") %>'>
	            </asp:TextBox>
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvUsersLastName">
            <itemtemplate>
	            <asp:TextBox id="txtLastName" Runat=server Width="90px" CssClass=NormalTextBox Text='<%# Eval("LastName") %>'>
	            </asp:TextBox>
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
            <itemtemplate>
                <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName='delete_item' CausesValidation="false"> 
                    &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                </CPCC:StyleButton>
            </itemtemplate>
        </asp:TemplateField>
    </columns>
</asp:GridView>