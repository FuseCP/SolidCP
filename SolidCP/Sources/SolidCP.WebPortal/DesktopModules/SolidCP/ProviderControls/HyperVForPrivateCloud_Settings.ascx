<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HyperVForPrivateCloud_Settings.ascx.cs"
    Inherits="SolidCP.Portal.ProviderControls.HyperVForPrivateCloud_Settings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="true"
    ShowSummary="false" />
<fieldset>
    <legend>
        <asp:Localize ID="locHyperVServer" runat="server" meta:resourcekey="locHyperVServer"
            Text="Host name"></asp:Localize>
    </legend>
    <scp:SimpleMessageBox id="messageBoxError" runat="server" />
    <table cellpadding="2" cellspacing="0" style="margin: 10px;">
            <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locConnectTypeName" runat="server" meta:resourcekey="locConnectTypeName"
                    Text="Select Server type:"></asp:Localize>
            </td>
            <td>
		        <asp:RadioButtonList ID="radioServer" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="radioServer_SelectedIndexChanged">
                    <asp:ListItem Value="cluster" meta:resourcekey="radioServerCloud" Selected="True">Cluster</asp:ListItem>
                    <asp:ListItem Value="host" meta:resourcekey="radioServerHost">Host</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="ServerNameRow" runat="server">
            <td class="FormLabel150">
                <asp:Localize ID="locHost" runat="server" meta:resourcekey="locHost" Text="Hosts:"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList ID="listHosts" runat="server" DataValueField="Path" DataTextField="Name" CssClass="form-control"
                    Width="500" AutoPostBack="true" OnSelectedIndexChanged="listHosts_OnSelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="HostsValidator" runat="server" Text="*" Display="Dynamic"
                    ControlToValidate="listHosts" meta:resourcekey="HostsValidator" SetFocusOnError="true"
                    ValidationGroup="ValidationSummary">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="LibraryPath" runat="server">
            <td class="FormLabel150">
                <asp:Localize ID="locLibraryPath" runat="server" meta:resourcekey="locLibraryPath" Text="Library path:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="300px" CssClass="form-control" runat="server" ID="txtLibraryPath"></asp:TextBox>
                <asp:RequiredFieldValidator ID="LibraryPathValidator" runat="server" ControlToValidate="txtLibraryPath"
                    Text="*" meta:resourcekey="LibraryPathValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
        <tr id="monitoringServerName" runat="server">
            <td class="FormLabel150">
                <asp:Localize ID="locMonitoringServerName" runat="server" meta:resourcekey="locMonitoringServerName" Text="Monitoring server:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="300px" CssClass="form-control" runat="server" ID="txtMonitoringServerName"></asp:TextBox>
                <asp:RequiredFieldValidator ID="MonitoringServerNameValidator" runat="server" ControlToValidate="txtMonitoringServerName"
                    Text="*" meta:resourcekey="MonitoringServerNameValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
    </table>
</fieldset>
<br />
<asp:Panel runat="server" ID="upHyperVCloud">
        <fieldset>
            <legend>
                <asp:Localize ID="locHyperVCloud" runat="server" meta:resourcekey="locHyperVCloud"
                    Text="Hyper-V Cloud"></asp:Localize>
            </legend>
            <fieldset>
                <legend>
                    <asp:Localize ID="locSCVMMServer" runat="server" meta:resourcekey="locSCVMMServer"
                        Text="SCVMM Server Connection options"></asp:Localize>
                </legend>
                <div class="pnlControl">
                    <asp:Label ID="lblSCVMMServer" runat="server" meta:resourcekey="lblSCVMMServer" Text="Server URL"
                        CssClass="SubHead"></asp:Label>
                    <asp:TextBox ID="txtSCVMMServer" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Button ID="btnSCVMMServer" runat="server" CssClass="disabled" Text="" ToolTip="Check button"
                        OnClick="btnSCVMMServer_Click" />
                </div>
                <div class="pnlControl">
                    <asp:Label ID="lblSCVMMPrincipalName" runat="server" meta:resourcekey="lblSCVMMPrincipalName"
                        Text="SCVMM pribcipal name" CssClass="SubHead"></asp:Label>
                    <asp:TextBox ID="txtSCVMMPrincipalName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="pnlControl">
                    <asp:CheckBox ID="chkUseSPNSCVMM" runat="server" Text="Use SPN Binding" CssClass="UseSPN" />
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    <asp:Localize ID="locSCOMServer" runat="server" meta:resourcekey="locSCOMServer"
                        Text="SCOM Server Connection options"></asp:Localize>
                </legend>
                <div class="pnlControl">
                    <asp:Label ID="lblSCOMServer" runat="server" meta:resourcekey="lblSCOMServer" Text="Server URL"
                        CssClass="SubHead"></asp:Label>
                    <asp:TextBox ID="txtSCOMServer" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Button ID="btnSCOMServer" runat="server" CssClass="disabled" ToolTip="Check button"
                        OnClick="btnSCOMServer_Click" />
                </div>
                <div class="pnlControl">
                    <asp:Label ID="lblSCOMPrincipalName" runat="server" meta:resourcekey="lblSCOMPrincipalName"
                        Text="SCOM pribcipal name" CssClass="SubHead"></asp:Label>
                    <asp:TextBox ID="txtSCOMPrincipalName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="pnlControl">
                    <asp:CheckBox ID="chkUseSPNSCOM" runat="server" Text="Use SPN Binding" CssClass="UseSPN" />
                </div>
            </fieldset>

<%--            <fieldset>
                <legend>
                    <asp:Localize ID="locVPServer" runat="server" meta:resourcekey="locVPServer"
                        Text="Virtualization Provisioning Server Connection options"></asp:Localize>
                </legend>
                <div class="pnlControl">
                    <asp:Label ID="lblVPServer" runat="server" meta:resourcekey="lblVPServer" Text="Server URL"
                        CssClass="SubHead"></asp:Label>
                    <asp:TextBox ID="txtVPServer" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Button ID="btnVPServer" runat="server" CssClass="disabled" ToolTip="Check button"
                        OnClick="btnVPServer_Click" />
                </div>
            </fieldset>
--%>

            <%--          <div class="pnlControl">
              <asp:Label ID="lblSCDPMServer" runat="server" meta:resourcekey="lblSCDPMServer" Text="SCDPM Server" CssClass="SubHead"></asp:Label>
              <asp:TextBox ID="txtSCDPMServer" runat="server" CssClass="form-control"></asp:TextBox>
              <asp:Button ID="btnSCDPMServer" runat="server" CssClass="disabled" 
                   ToolTip="Check button" OnClick="btnSCDPMServer_Click"/>
          </div>
          <div class="pnlControl">
              <asp:Label ID="lblSCCMServer" runat="server" meta:resourcekey="lblSCCMServer" Text="SCCM Server" CssClass="SubHead"></asp:Label>
              <asp:TextBox ID="txtSCCMServer" runat="server" CssClass="form-control"></asp:TextBox>
              <asp:Button ID="btnSCCMServer" runat="server" CssClass="disabled" 
                   ToolTip="Check button" OnClick="btnSCCMServer_Click"/>
          </div>
            --%>
<%--            <div class="pnlControl">
                <asp:Label ID="lblSCVMMEndPoint" runat="server" meta:resourcekey="lblSCVMMEndPoint"
                    Text="SCVMM Endpoint" CssClass="SubHead"></asp:Label>
                <asp:TextBox ID="txtSCVMMEndPoint" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:Button ID="btnSCVMMEndPoint" runat="server" CssClass="disabled" ToolTip="Check button"
                    OnClick="btnSCVMMEndPoint_Click" />
            </div>
            <div class="pnlControl">
                <asp:Label ID="lblSCOMEndPoint" runat="server" meta:resourcekey="lblSCOMEndPoint"
                    Text="SCOM Endpoint" CssClass="SubHead"></asp:Label>
                <asp:TextBox ID="txtSCOMEndPoint" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:Button ID="btnSCOMEndPoint" runat="server" CssClass="disabled" ToolTip="Check button"
                    OnClick="btnSCOMEndPoint_Click" />
            </div>
--%>
            <%--          <div class="pnlControl">
              <asp:Label ID="lblSCDPMEndPoint" runat="server" meta:resourcekey="lblSCDPMEndPoint" Text="SCDPM Endpoint" CssClass="SubHead"></asp:Label>
              <asp:TextBox ID="txtSCDPMEndPoint" runat="server" CssClass="form-control"></asp:TextBox>
              <asp:Button ID="btnSCDPMEndPoint" runat="server" CssClass="disabled" 
                    ToolTip="Check button" OnClick="btnSCDPMEndPoint_Click"/>
          </div>
          <div class="pnlControl">
              <asp:Label ID="lblSCCMEndPoint" runat="server" meta:resourcekey="lblSCCMEndPoint" Text="SCCM Endpoint" CssClass="SubHead"></asp:Label>
              <asp:TextBox ID="txtSCCMEndPoint" runat="server" CssClass="form-control"></asp:TextBox>
              <asp:Button ID="btnSCCMEndPoint" runat="server" CssClass="disabled" 
                    ToolTip="Check button" OnClick="btnSCCMEndPoint_Click"/>
          </div>
          <div class="pnlControl">
              <asp:Label ID="lblStorageEndPoint" runat="server" meta:resourcekey="lblStorageEndPoint" Text="Storage Endpoint" CssClass="SubHead"></asp:Label>
              <asp:TextBox ID="txtStorageEndPoint" runat="server" CssClass="form-control"></asp:TextBox>
              <asp:Button ID="btnStorageEndPoint" runat="server" CssClass="disabled" 
                    ToolTip="Check button" OnClick="btnStorageEndPoint_Click"/>
          </div>
          <div class="pnlControl">
              <asp:Label ID="lblCoreSvcEndpoint" runat="server" meta:resourcekey="lblCoreSvcEndpoint" Text="Core Svc Endpoint" CssClass="SubHead"></asp:Label>
              <asp:TextBox ID="txtCoreSvcEndpoint" runat="server" CssClass="form-control"></asp:TextBox>
              <asp:Button ID="btnCoreSvcEndpoint" runat="server" CssClass="disabled" 
                   ToolTip="Check button" OnClick="btnCoreSvcEndpoint_Click"/>
          </div>
            --%>
        </fieldset>
</asp:Panel>
<%--<fieldset>
    <legend>
        <asp:Localize ID="locGeneralSettings" runat="server" meta:resourcekey="locGeneralSettings"
            Text="General Settings"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locVpsRootFolder" runat="server" meta:resourcekey="locVpsRootFolder"
                    Text="VPS root folder:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="400px" CssClass="form-control" runat="server" ID="txtVpsRootFolder"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RootFolderValidator" runat="server" ControlToValidate="txtVpsRootFolder"
                    Text="*" meta:resourcekey="RootFolderValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Localize ID="locFolderVariables" runat="server" meta:resourcekey="locFolderVariables"
                    Text="The following variables..."></asp:Localize>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locOSTemplatesPath" runat="server" meta:resourcekey="locOSTemplatesPath"
                    Text="OS Templates path:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="300px" CssClass="form-control" runat="server" ID="txtOSTemplatesPath"></asp:TextBox>
                <asp:RequiredFieldValidator ID="TemplatesPathValidator" runat="server" ControlToValidate="txtOSTemplatesPath"
                    Text="*" meta:resourcekey="TemplatesPathValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locExportedVpsPath" runat="server" meta:resourcekey="locExportedVpsPath"
                    Text="Exported VPS path:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="300px" CssClass="form-control" runat="server" ID="txtExportedVpsPath"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ExportedVpsPathValidator" runat="server" ControlToValidate="txtExportedVpsPath"
                    Text="*" meta:resourcekey="ExportedVpsPathValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
    </table>

</fieldset>--%>
<br />
<fieldset>
    <legend>
        <asp:Localize ID="locProcessorSettings" runat="server" meta:resourcekey="locProcessorSettings"
            Text="Processor Resource Settings"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locCpuReserve" runat="server" meta:resourcekey="locCpuReserve"
                    Text="Virtual machine reserve:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="50px" CssClass="form-control" runat="server" ID="txtCpuReserve"></asp:TextBox>
                %
                <asp:RequiredFieldValidator ID="CpuReserveValidator" runat="server" ControlToValidate="txtCpuReserve"
                    Text="*" meta:resourcekey="CpuReserveValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locCpuLimit" runat="server" meta:resourcekey="locCpuLimit" Text="Virtual machine limit:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="50px" CssClass="form-control" runat="server" ID="txtCpuLimit"></asp:TextBox>
                %
                <asp:RequiredFieldValidator ID="CpuLimitValidator" runat="server" ControlToValidate="txtCpuLimit"
                    Text="*" meta:resourcekey="CpuLimitValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locCpuWeight" runat="server" meta:resourcekey="locCpuWeight" Text="Relative weight:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="50px" CssClass="form-control" runat="server" ID="txtCpuWeight"></asp:TextBox>
                <asp:RequiredFieldValidator ID="CpuWeightValidator" runat="server" ControlToValidate="txtCpuWeight"
                    Text="*" meta:resourcekey="CpuWeightValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
    </table>
</fieldset>
<br />
<%--<fieldset>
    <legend>
        <asp:Localize ID="locMediaLibrary" runat="server" meta:resourcekey="locMediaLibrary"
            Text="DVD Media Library"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locDvdIsoPath" runat="server" meta:resourcekey="locDvdIsoPath"
                    Text="Path to DVD ISO files:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="300px" CssClass="form-control" runat="server" ID="txtDvdLibraryPath"></asp:TextBox>
                <asp:RequiredFieldValidator ID="DvdLibraryPathValidator" runat="server" ControlToValidate="txtDvdLibraryPath"
                    Text="*" meta:resourcekey="DvdLibraryPathValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
    </table>
</fieldset>
<br />
--%><fieldset>
    <legend>
        <asp:Localize ID="locVhd" runat="server" meta:resourcekey="locVhd" Text="Virtual Hard Drive"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" style="width: 200px;" valign="top">
                <asp:Localize ID="locDiskType" runat="server" meta:resourcekey="locDiskType" Text="Disk Type:"></asp:Localize>
            </td>
            <td>
                <asp:RadioButtonList ID="radioVirtualDiskType" runat="server">
                    <asp:ListItem Value="dynamic" meta:resourcekey="radioVirtualDiskTypeDynamic" Selected="True">Dynamic</asp:ListItem>
                    <asp:ListItem Value="fixed" meta:resourcekey="radioVirtualDiskTypeFixed">Fixed</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</fieldset>
<br />
<fieldset>
    <legend>
        <asp:Localize ID="locExternalNetwork" runat="server" meta:resourcekey="locExternalNetwork"
            Text="External Network"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locExternalNetworkName" runat="server" meta:resourcekey="locExternalNetworkName"
                    Text="Connect to Network:"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList ID="ddlExternalNetworks" runat="server" CssClass="form-control"
                    Width="450" DataValueField="Name" DataTextField="Name" />
                <asp:RequiredFieldValidator ID="ExternalNetworkValidator" runat="server" Text="*" Display="Dynamic"
                    ControlToValidate="ddlExternalNetworks" meta:resourcekey="ExternalNetworkValidator" 
                    SetFocusOnError="true" ValidationGroup="ValidationSummary">*</asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
</fieldset>
<br />
<fieldset>
    <legend>
        <asp:Localize ID="locPrivateNetwork" runat="server" meta:resourcekey="locPrivateNetwork"
            Text="Private Network"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locPrivateNetworkName" runat="server" meta:resourcekey="locPrivateNetworkName"
                    Text="Connect to Network:"></asp:Localize>
            </td>
            <td>
                <asp:DropDownList ID="ddlPrivateNetworks" runat="server" CssClass="form-control"
                    Width="450" DataValueField="Name" DataTextField="Name" />
                <asp:RequiredFieldValidator ID="PrivateNetworkValidator" runat="server" Text="*" Display="Dynamic"
                    ControlToValidate="ddlPrivateNetworks" meta:resourcekey="PrivateNetworkValidator" 
                    SetFocusOnError="true" ValidationGroup="ValidationSummary">*</asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
</fieldset>
<br />
<fieldset>
    <legend>
        <asp:Localize ID="locHostname" runat="server" meta:resourcekey="locHostname" Text="Host name"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" style="width: 200px;">
                <asp:Localize ID="locHostnamePattern" runat="server" meta:resourcekey="locHostnamePattern"
                    Text="VPS host name pattern:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox Width="200px" CssClass="form-control" runat="server" ID="txtHostnamePattern"></asp:TextBox>
                <asp:RequiredFieldValidator ID="HostnamePatternValidator" runat="server" ControlToValidate="txtHostnamePattern"
                    Text="*" meta:resourcekey="HostnamePatternValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
    </table>
    <p style="margin: 10px;">
        <asp:Localize ID="locPatternText" runat="server" meta:resourcekey="locPatternText"
            Text="Help text goes here..."></asp:Localize>
    </p>
</fieldset>
<br />
<fieldset>
    <legend>
        <asp:Localize ID="locStartAction" runat="server" meta:resourcekey="locStartAction"
            Text="Automatic Start Action"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td>
                <asp:Localize ID="locStartOptionsText" runat="server" meta:resourcekey="locStartOptionsText"
                    Text="What do you want VPS to do when the physical computer starts?"></asp:Localize>
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButtonList ID="radioStartAction" runat="server">
                    <asp:ListItem Value="0" meta:resourcekey="radioStartActionNothing">Nothing</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="radioStartActionStart" Selected="True">Start</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="radioStartActionAlwaysStart">AlwaysStart</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Localize ID="locStartupDelayText" runat="server" meta:resourcekey="locStartupDelayText"
                    Text="Specify a startup delay to reduce resource contention between virtual machines."></asp:Localize>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Localize ID="locStartupDelay" runat="server" meta:resourcekey="locStartupDelay"
                    Text="Startup delay:"></asp:Localize>
                <asp:TextBox ID="txtStartupDelay" runat="server" Width="30px"></asp:TextBox>
                <asp:Localize ID="locSeconds" runat="server" meta:resourcekey="locSeconds" Text="seconds"></asp:Localize>
                <asp:RequiredFieldValidator ID="StartupDelayValidator" runat="server" ControlToValidate="txtStartupDelay"
                    Text="*" meta:resourcekey="StartupDelayValidator" Display="Dynamic" SetFocusOnError="true" />
            </td>
        </tr>
    </table>
</fieldset>
<br />
<fieldset>
    <legend>
        <asp:Localize ID="locStopAction" runat="server" meta:resourcekey="locStopAction"
            Text="Automatic Stop Action"></asp:Localize>
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td>
                <asp:Localize ID="locStopActionText" runat="server" meta:resourcekey="locStopActionText"
                    Text="What do you want VPS to do when the physical shuts down?"></asp:Localize>
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButtonList ID="radioStopAction" runat="server">
                    <asp:ListItem Value="1" meta:resourcekey="radioStopActionSave">Save</asp:ListItem>
                    <asp:ListItem Value="0" meta:resourcekey="radioStopActionTurnOff">TurnOff</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="radioStopActionShutDown" Selected="True">ShutDown</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</fieldset>
<br />
