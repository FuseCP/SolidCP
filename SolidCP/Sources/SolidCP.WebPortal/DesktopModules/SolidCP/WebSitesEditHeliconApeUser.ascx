<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesEditHeliconApeUser.ascx.cs" Inherits="SolidCP.Portal.WebSitesEditHeliconApeUser" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<script src="/JavaScript/jquery.min.js?v=1.4.4" type="text/javascript">

</script>
<div class="panel-body form-horizontal">
<script type="text/javascript">
function pageLoad() {
    $('.AuthType input').change(function () {
        authTypeChanged(this);
    });

    authTypeChanged($('.AuthType input')[0]);
}

function authTypeChanged(el) {
    if ('Basic' == el.value) {
        if (el.checked) {
            $('.EncType').show();
            $('.DigestRealm').hide();
        } else {
            $('.EncType').hide();
            $('.DigestRealm').show();
        }
    } else if ('Digest' == el.value) {
        if (el.checked) {
            $('.EncType').hide();
            $('.DigestRealm').show();
        } else {
            $('.EncType').show();
            $('.DigestRealm').hide();
        }
    }
}
</script>
<table cellSpacing="0" cellPadding="0" width="100%">
	<tr>
		<td>
            <table cellSpacing="0" cellPadding="5" width="100%">
	            <tr>
		            <td class="SubHead" style="width: 150px;">
						<asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="User Name:"></asp:Label>
					</td>
		            <td class="NormalBold">
                        <uc3:UsernameControl ID="usernameControl" runat="server" />
                    </td>
	            </tr>
	            <tr>
		            <td class="SubHead" style="width: 150px; vertical-align: top;">
						<asp:Label ID="lblAuthType" runat="server" meta:resourcekey="lblAuthType" Text="Auth Type"></asp:Label>
					</td>
		            <td>
                        <asp:RadioButtonList ID="rblAuthType" runat="server" CssClass="AuthType"></asp:RadioButtonList>
                    </td>
	            </tr>
	            <tr class="EncType">
		            <td class="SubHead" style="width: 150px; vertical-align: top;">
						<asp:Label ID="lblEncType" runat="server" meta:resourcekey="lblEncType" Text="Encryption Type"></asp:Label>
					</td>
		            <td>
                        <asp:RadioButtonList ID="rblEncType" runat="server"></asp:RadioButtonList>
                    </td>
	            </tr>
	            <tr class="DigestRealm" style="display: none;">
		            <td class="SubHead" style="width: 150px; vertical-align: top;">
						<asp:Label ID="lblDigestRealm" runat="server" meta:resourcekey="lblDigestRealm" Text="Digest Realm"></asp:Label>
					</td>
		            <td>
                        <asp:TextBox ID="tbDigestRealm" runat="server"></asp:TextBox>
                    </td>
	            </tr>
	            <tr>
		            <td class="SubHead" valign="top">
                        <asp:Label ID="lblUserPassword" runat="server" meta:resourcekey="lblUserPassword" Text="User Password:"></asp:Label></td>
		            <td class="Normal" valign="top">
                        <uc2:PasswordControl ID="passwordControl" runat="server" />
		            </td>
	            </tr>
            </table>
            
            <scp:CollapsiblePanel id="secGroups" runat="server"
                TargetControlID="GroupsPanel" meta:resourcekey="secGroups" Text="Member Of">
            </scp:CollapsiblePanel>
	        <asp:Panel ID="GroupsPanel" runat="server" Height="0" style="overflow:hidden;">
                <table id="tblGroups" runat="server" cellSpacing="0" cellPadding="3" width="100%">
	                <tr>
		                <td colspan="2">
			                <asp:checkboxlist id="dlGroups" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" DataTextField="Name"
				                DataValueField="Name" Runat="server"></asp:checkboxlist>
		                </td>
	                </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" > <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnSaveAndAddAnother" CssClass="btn btn-success" runat="server" OnClick="btnSaveAndAddAnother_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveAndAddAnother"/> </CPCC:StyleButton>
</div>
