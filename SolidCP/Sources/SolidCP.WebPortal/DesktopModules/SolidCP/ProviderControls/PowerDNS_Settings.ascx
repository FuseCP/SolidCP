<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PowerDNS_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.PowerDNS_Settings" %>
<%@ Register Src="Common_IPAddressesList.ascx" TagName="IPAddressesList" TagPrefix="common" %>
<%@ Register Src="Common_SecondaryDNSServers.ascx" TagName="SecondaryDNSServers" TagPrefix="common" %>
<%@ Register Src="../UserControls/ScheduleInterval.ascx" TagName="ScheduleInterval" TagPrefix="common" %>
<%@ Register Src="../UserControls/EditDomainsList.ascx" TagName="EditDomainsList" TagPrefix="common" %>

<fieldset>
    <legend>
        <asp:Label runat="server" CssClass="NormalBold" meta:resourcekey="lblFtuNote" />
    </legend>
    <div runat="server" id="lblFirsttimeUserNote" />
</fieldset>

<fieldset>
    <legend>
        <asp:Label runat="server" CssClass="NormalBold" meta:resourcekey="lblServiceConfig" />
    </legend>
	<table cellspacing="0" width="100%">

		<!-- Connection Strign parameters: server, port, database, user, password -->
		<tr>
			<td class="SubHead" width="200" nowrap>
				<asp:Label runat="server" ID="lblServerAddress" meta:resourcekey="lblServerAddress" /></td>
			<td class="Normal">
				<asp:TextBox runat="server" ID="txtServerAddress" MaxLength="1000" CssClass="form-control"/>
				<asp:RequiredFieldValidator ID="valRequireServerAddress" runat="server" Display="Dynamic" ControlToValidate="txtServerAddress"
						ErrorMessage="*" meta:resourcekey="requiredFieldValidator"></asp:RequiredFieldValidator>
			</td>
		</tr>
	    
		<tr>
			<td class="SubHead" width="200" nowrap>
				<asp:Label runat="server" ID="lblServerPort" meta:resourcekey="lblServerPort" /></td>
			<td class="Normal">
				<asp:TextBox runat="server" ID="txtServerPort" MaxLength="1000" CssClass="form-control"/>
				<asp:Label runat="server" ID="lblServerPortDefault" meta:resourcekey="lblServerPortDefault" />
			</td>
		</tr>
	    
		<tr>
			<td class="SubHead" width="200" nowrap>
				<asp:Label runat="server" ID="lblDatabase" meta:resourcekey="lblDatabase" /></td>
			<td class="Normal">
				<asp:TextBox runat="server" ID="txtDatabase" MaxLength="1000" CssClass="form-control"/>
				<asp:RequiredFieldValidator ID="valRequireDatabase" runat="server" Display="Dynamic" ControlToValidate="txtDatabase"
						ErrorMessage="*" meta:resourcekey="requiredFieldValidator"></asp:RequiredFieldValidator>
			</td>
		</tr>    
	    
		<tr>
			<td class="SubHead" width="200" nowrap>
				<asp:Label runat="server" ID="lblUsername" meta:resourcekey="lblUsername" /></td>
			<td class="Normal">
				<asp:TextBox runat="server" ID="txtUsername" MaxLength="1000" CssClass="form-control"/>
				<asp:RequiredFieldValidator ID="valRequireUsername" runat="server" Display="Dynamic" ControlToValidate="txtUsername"
						ErrorMessage="*" meta:resourcekey="requiredFieldValidator"></asp:RequiredFieldValidator>
			</td>
		</tr>        
	    
		<tr id="trCurrentPassword" runat="server">
			<td class="SubHead" width="200" nowrap>
				<asp:Label runat="server" ID="lblCurrentPassword" meta:resourcekey="lblCurrentPassword" />
			</td>
			<td class="Normal">
				<asp:Label runat="server" ID="lblCurrentPasswordText" meta:resourcekey="lblCurrentPasswordText" /><br />
			</td>
		</tr>
	    
		<tr id="trPassword" runat="server">
			<td class="SubHead" width="200" nowrap>
				<asp:Label runat="server" ID="lblPassword" meta:resourcekey="lblPassword" /></td>
			<td class="Normal">
				<asp:TextBox runat="server" ID="txtPassword" MaxLength="1000" TextMode="Password" CssClass="form-control"/>
				<asp:RequiredFieldValidator ID="varRequirePassword" runat="server" Display="Dynamic" ControlToValidate="txtPassword"
						ErrorMessage="*" meta:resourcekey="requiredFieldValidator"></asp:RequiredFieldValidator>
			</td>
		</tr>            
	    
		<tr id="trPasswordConfirm" runat="server">
			<td class="SubHead" width="200" nowrap>
				<asp:Label runat="server" ID="lblConfirmPassword" meta:resourcekey="lblConfirmPassword" /></td>
			<td class="Normal">
				<asp:TextBox runat="server" ID="txtConfirmPassword" MaxLength="1000" TextMode="Password" CssClass="form-control"/>                   
				<asp:CompareValidator id="passwordsIdentValidator" runat="server" meta:resourcekey="passwordsIdentValidator" ErrorMessage="Both passwords should be identical"
						ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Display="Dynamic"></asp:CompareValidator>

				<asp:CustomValidator ID="valRequireConfirmPasswordNotEmpty" runat="server" meta:resourcekey="requiredFieldValidator" ErrorMessage="*"
						ClientValidationFunction="pdnsComparePasswordFields" Display="Dynamic" ControlToValidate="txtPassword" Enabled="true"></asp:CustomValidator>

			</td>
		</tr>
		<tr>
			<td colspan="2">&nbsp;</td>
		</tr>
	    
	          

	    
		<!-- SOA Record Properties -->
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblResponsiblePerson" runat="server" meta:resourcekey="lblResponsiblePerson" Text="Responsible Person:"></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="txtResponsiblePerson" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblRefreshInterval" runat="server" meta:resourcekey="lblRefreshInterval" Text="Refresh Interval:"></asp:Label>
			</td>
			<td class="Normal">
				<common:ScheduleInterval ID="intRefresh" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblRetryInterval" runat="server" meta:resourcekey="lblRetryInterval" Text="Retry Interval:"></asp:Label>
			</td>
			<td class="Normal">
				<common:ScheduleInterval ID="intRetry" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblExpireLimit" runat="server" meta:resourcekey="lblExpireLimit" Text="Expire Limit:"></asp:Label>
			</td>
			<td class="Normal">
				<common:ScheduleInterval ID="intExpire" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="SubHead" noWrap>
				<asp:Label ID="lblMinimumTtl" runat="server" meta:resourcekey="lblMinimumTtl" Text="Minimum TTL:"></asp:Label>
			</td>
			<td class="Normal">
				<common:ScheduleInterval ID="intTtl" runat="server" />
			</td>
		</tr>
		<tr>
			<td colspan="2">&nbsp;</td>
		</tr>
		
	    
		<!-- IP Adresses -->
		<tr>
			<td class="SubHead" width="200" nowrap valign="top">
				<asp:Label ID="lblIPAddresses" runat="server" meta:resourcekey="lblIPAddresses" Text="Listening IP Addresses:"></asp:Label>
			</td>
			<td width="100%" valign="top">
				<common:IPAddressesList id="iPAddressesList" runat="server">
				</common:IPAddressesList>
			</td>
		</tr>
		<tr>
			<td colspan="2">&nbsp;</td>
		</tr>
	    
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblSecondaryDNS" runat="server" meta:resourcekey="lblSecondaryDNS" Text="Secondary DNS Services:"></asp:Label>
			</td>
			<td class="Normal" valign="top">
				<common:SecondaryDNSServers ID="secondaryDNSServers" runat="server" />
			</td>
		</tr>		        
		<tr>
			<td colspan="2">&nbsp;</td>
		</tr>
		
			
		<!-- Name Servers -->	
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblNameServers" runat="server" meta:resourcekey="lblNameServers" Text="Name Servers:"></asp:Label>
			</td>
			<td class="Normal" valign="top">
				<common:EditDomainsList id="nameServers" runat="server">
				</common:EditDomainsList>
			</td>
		</tr>
			
	</table>
</fieldset>