<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalculatePackagesBandwidth.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.CalculatePackagesBandwidth" %>
	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblSuspendOverusedSpaces" runat="server" meta:resourcekey="lblSuspendOverusedSpaces" Text="Suspend overused spaces?"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:DropDownList ID="ddlSuspendOverusedSpaces" runat="server"  CssClass="NormalDropDown"></asp:DropDownList>
        </tr>
	</table>