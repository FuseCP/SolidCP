<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsVPS.ascx.cs" Inherits="SolidCP.Portal.SpaceSettingsVPS" %>

<fieldset>
    <legend>
        <asp:Localize ID="locHostname" runat="server" meta:resourcekey="locHostname" Text="Host name"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
	    <tr>
		    <td class="SubHead" style="width:200px;">
		        <asp:Localize ID="locHostnamePattern" runat="server" meta:resourcekey="locHostnamePattern" Text="VPS host name pattern:"></asp:Localize>
		    </td>
		    <td>
                <asp:TextBox Width="200px" CssClass="form-control" Runat="server" ID="txtHostnamePattern"></asp:TextBox>
                <asp:RequiredFieldValidator ID="HostnamePatternValidator" runat="server" ControlToValidate="txtHostnamePattern"
                    Text="*" meta:resourcekey="HostnamePatternValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
	    </tr>
	</table>
	<p style="margin: 10px;">
	    <asp:Localize ID="locPatternText" runat="server" meta:resourcekey="locPatternText" Text="Help text goes here..."></asp:Localize>
	</p>
</fieldset>
<br />