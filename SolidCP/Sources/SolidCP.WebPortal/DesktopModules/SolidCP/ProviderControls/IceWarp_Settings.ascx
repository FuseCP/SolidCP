<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IceWarp_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.IceWarp_Settings" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<%@ Import Namespace="SolidCP.Portal" %>
<table width="100%">
	<tr>
		<td class="SubHead" width="150">
		    <asp:Label ID="lblPublicIP" runat="server" Text="Public IP Address:" meta:resourcekey="lblPublicIP"></asp:Label> 
		</td>
		<td>
            <uc1:SelectIPAddress ID="ipAddress" runat="server" ServerIdParam="ServerID" />
        </td>
	</tr>
    <tr>
        <td class="SubHead">
            <asp:Label runat="server" ID="lblMaxMessageSizeInMB" Text="Global max message size:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtMaxMessageSizeInMB" Text="0" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="txtMaxMessageSizeInMBRequired" runat="server" Text="*" ControlToValidate="txtMaxMessageSizeInMB"></asp:RequiredFieldValidator>
            <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
        </td>
    </tr>
    <tr>
        <td></td>
        <td><asp:Label ID="MaxMessageSizeInMB" runat="server" meta:resourcekey="MaxMessageSizeInMB" Text="Can be overridden on domain level if you check 'Override global limits'"></asp:Label></td>
    </tr>
	<tr>
		<td colspan="2">
		    <asp:CheckBox runat="server" ID="cbUseDomainDiskQuota" Text="Use domain disk quota" />
        </td>
	</tr>
	<tr>
		<td colspan="2">
		    <asp:CheckBox runat="server" ID="cbUseDomainLimits" Text="Use domain limits" />
        </td>
	</tr>
	<tr>
		<td colspan="2">
		    <asp:CheckBox runat="server" ID="cbUseUserLimits" Text="Use user limits" />
        </td>
	</tr>
	<tr>
		<td colspan="2">
		    <asp:CheckBox runat="server" ID="cbOverrideGlobal" Text="Override global limits" />
        </td>
	</tr>
    <tr>
        <td class="SubHead">
            <asp:Label runat="server" ID="lblWarnDomainSize" Text="Warn domain administrator when domain size exceeds quota (%)"></asp:Label>
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtWarnDomainSize" Text="0" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" Text="*" ControlToValidate="txtWarnDomainSize"></asp:RequiredFieldValidator>
            <asp:RangeValidator ErrorMessage="Must be a value between 0 and 99, 0 means disabled" MinimumValue="0" MaximumValue="99" Type="Integer" meta:resourcekey="txtWarnSizeValidator" ControlToValidate="txtWarnDomainSize" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Label runat="server" ID="lblWarnMailboxUsage" Text="Warn user when mailbox size exceeds quota (%)"></asp:Label>
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtWarnMailboxUsage" Text="0" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="txtWarnMailboxUsageRequired" runat="server" Text="*" ControlToValidate="txtWarnMailboxUsage"></asp:RequiredFieldValidator>
            <asp:RangeValidator ErrorMessage="Must be a value between 0 and 99, 0 means disabled" MinimumValue="0" MaximumValue="99" Type="Integer" meta:resourcekey="txtWarnSizeValidator" ControlToValidate="txtWarnMailboxUsage" runat="server" />
        </td>
    </tr>
</table>

<div id="SeDiv" runat="server">
<fieldset>
    <legend>
        <asp:Label ID="lblRouteFromSE" runat="server" meta:resourcekey="lblRouteFromSE"
            Text="Automatic Mail Filter" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
    <asp:CheckBox ID="chkSEEnable" runat="server" meta:resourcekey="chkEnableSE" Text="Enable Automatic Mail Filtering (SpamExperts) for domains managed by this provider" />
    <asp:UpdatePanel ID="GeneralUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:GridView id="gvSEDestinations" runat="server"  EnableViewState="true" AutoGenerateColumns="false"
	        Width="100%" EmptyDataText="" CssSelectorClass="NormalGridView" OnRowCommand="gvSEDestinations_RowCommand" >
	        <Columns>
		        <asp:TemplateField HeaderText="Destinations">
			        <ItemStyle Width="100%"></ItemStyle>
			        <ItemTemplate>
				        <asp:Label id="lblSEDestination" runat="server" EnableViewState="true" ><%# PortalAntiXSS.Encode((string)Container.DataItem)%></asp:Label>
                    </ItemTemplate>
		        </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Width="65px" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <CPCC:StyleButton id="imgDelRouteFromSE" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# (string)Container.DataItem %>' OnClientClick="return confirm('Are you sure you want to delete selected route?')"> 
                            &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                        </CPCC:StyleButton>
                    </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
	        </asp:GridView>
            <br />
            <div class="input-group col-sm-6">
             <asp:TextBox ID="tbSEDestinations" CssClass="form-control" style="vertical-align: middle;" runat="server"></asp:TextBox>
                <span class="input-group-btn">
                <CPCC:StyleButton ID="bntAddSEDestination" runat="server" CssClass="btn btn-primary" OnClick="bntAddSEDestination_Click" CausesValidation="False">
                <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="bntAddSEDestination" />
                </CPCC:StyleButton>
                </span>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
</fieldset>
</div>