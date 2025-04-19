<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsMailPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsMailPolicy" %>
<%@ Register Src="UserControls/UsernamePolicyEditor.ascx" TagName="UsernamePolicyEditor" TagPrefix="uc2" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secGeneral" runat="server"
    TargetControlID="GeneralPanel" meta:resourcekey="secGeneral" Text="General Settings"/>
<asp:Panel ID="GeneralPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtCatchAll" runat="server" CssClass="form-control" Width="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireCatchAll" runat="server" ControlToValidate="txtCatchAll" meta:resourcekey="valRequireCatchAll"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="SettingsEditor"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secAccountPolicy" runat="server"
    TargetControlID="AccountPolicyPanel" meta:resourcekey="secAccountPolicy" Text="Mail Accounts Policy"/>
<asp:Panel ID="AccountPolicyPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblAccount" runat="server" meta:resourcekey="lblAccount" Text="Account Policy:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="accountNamePolicy" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
        <tr>
            <td class="SubHead" valign="top">
                <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password Policy:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:PasswordPolicyEditor id="accountPasswordPolicy" runat="server">
                </uc1:PasswordPolicyEditor></td>
        </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secAccessPolicy" runat="server"
    TargetControlID="AccessPolicyPanel" meta:resourcekey="secAccessPolicy" Text="Mail Access Policy"/>
<asp:Panel ID="AccessPolicyPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <asp:Label ID="lblAccessPolicyNote" meta:resourcekey="lblAccessPolicyNote" Text="These settings only apply to the SmarterMail100 provider" runat="server" />
        </tr>
        <tr>
            <td width="200px"><asp:Label ID="lblAuthType" meta:resourcekey="lblAuthType" Text="Countries To Block:" runat="server" /></td>
            <td>
                <asp:DropDownList ID="ddlAuthType" runat="server">
                    <asp:ListItem Value="1" meta:resourcekey="ddlAuthType1">Specified Countries</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="ddlAuthType2">All But Specified Countries</asp:ListItem>
                </asp:DropDownList>
        </td>
        </tr>
            <tr>
        <td width="200px" valign="top">
            <asp:Localize ID="Localize1" runat="server" meta:resourcekey="lblCountry" Text="Add Country:" />
        </td>
        <td class="Normal">
            <asp:DropDownList ID="ddlAddCountry" runat="server"> </asp:DropDownList>
            <asp:Button ID="btnAddCountry" runat="server" Text="Add Country" OnClick="btnAddCountry_Click" meta:resourcekey="btnAddCountry" CssClass="Button2" />
        </td>
    </tr>
    <tr>
        <td width="200px" valign="top">
            <asp:Localize ID="Localize2" runat="server" meta:resourcekey="lblSelectedCountries" Text="Selected Countries:" />
        </td>
        <td class="Normal">
            <asp:GridView ID="gvSelectedCountries" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Name" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            &nbsp&nbsp<CPCC:StyleButton ID="btnRemove" runat="server" CssClass="btn btn-danger" CommandArgument='<%# Eval("Code") %>' OnClick="btnRemove_Click" >
                                &nbsp<i class="fa fa-trash-o"></i>&nbsp; 
                            </CPCC:StyleButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    </table>
</asp:Panel>