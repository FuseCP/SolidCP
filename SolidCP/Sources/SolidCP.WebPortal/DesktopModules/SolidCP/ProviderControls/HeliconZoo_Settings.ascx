<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="HeliconZoo_Settings.ascx.cs" Inherits="HeliconZoo_Settings" %>
<%@ Register Src="../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<%@ Import Namespace="SolidCP.Portal.ProviderControls" %>
<%@ Import Namespace="SolidCP.Server" %>

<style>
p.description {
    margin: 4px 0 10px 0;
    color: #666;
}    
table.EnginesTable th {
    vertical-align: top;
    text-align: left;
    color: #000;
    font-weight: normal;
}
span.ValidationMessageBlock {
    display: block !important;
}
#EngineEnvs INPUT.NormalTextBox {
    font-family: Consolas, Courier New, monospace;
}
.AspNet-GridView td {
    padding: 3px 3px 5px 3px;
}
.AspNet-GridView-Alternate {
    background-color: #f2f2f2;
}
</style>

<fieldset>
    <legend>
        <span>Hosting Packages</span>
    </legend>
    
    <asp:GridView runat="server" ID="HostingPackagesGrid" 
    AutoGenerateColumns="False"
    EmptyDataText="Hosting Packages not found. Please check Web Platform Installer settings in System settings."
    EnableViewState="True"
    ShowHeader="False"
    OnRowCommand="HostingPackagesGrid_OnRowCommand"
    >
    <Columns>
        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="gvWPILogo" ItemStyle-Width="10%"  >
            <ItemTemplate>
                <asp:Image ID="icoLogo" runat="server" SkinID="Dvd48" src='<%# Eval("Logo") %>' Width="48" Height="48" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField ItemStyle-Wrap="true" HeaderText="gvServicesName" ItemStyle-Width="80%">
            <ItemTemplate>
                <h2 class="ProductTitle Huge"><%# Eval("Title")%></h2>
                <p class="ProductSummary"><%# Eval("Summary") %></p>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="gvInstall" ItemStyle-Width="10%">
            <ItemStyle HorizontalAlign="Left" />
            <ItemTemplate>
                <asp:Label ID="LabelInstalled" runat="server" Text="installed" Visible='<%#  Eval("IsInstalled") %>'></asp:Label>
                <asp:Button ID="btnAdd" runat="server"
                CssClass='Button1'
                Visible = '<%#  !(bool)Eval("IsInstalled") %>'
                Text = '<%# AddUpgradeRemoveText( (WPIProduct)Container.DataItem ) %>'
			    CommandArgument='<%# Container.DataItemIndex %>'
			    CommandName="WpiAdd" 
                />
        </ItemTemplate>
    </asp:TemplateField>

    </Columns>
</asp:GridView>

<asp:Panel runat="server" ID="HostingPackagesInstallButtonsPanel">
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnInstall" CssClass="btn btn-success" runat="server" OnClick="btnInstall_Click" Enabled="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnInstallText"/> </CPCC:StyleButton>
    </div>  
</asp:Panel>

<asp:Panel runat="server" ID="HostingPackagesErrorsPanel" Visible="False">
    <h3 class="Header ErrorText">Unable to load Hosting Package list with exception:</h3>
    <asp:Label runat="server" ID="HostingPackagesLoadingError"></asp:Label>
</asp:Panel>

</fieldset>


<asp:Panel runat="server" ID="EnginesPanel">

<fieldset>
  <legend>
      <span>Helicon Zoo settings</span>
  </legend>
  <div class="panel-body form-horizontal">
      <asp:CheckBox runat="server" ID="QuotasEnabled" Text="Enable hosting plan controls for web engines."/>
      <br />
      <asp:CheckBox runat="server" ID="WebCosoleEnabled" Text="Enable web console."/>
  </div>
</fieldset>


<fieldset>
    <legend>
        <span>Engines management</span>
    </legend>

<div class="FormButtonsBar">
    <asp:Button runat="server" ID="ButtonAddEngine" CssClass="Button2" Text="Add engine" OnClick="ButtonAddEngine_Click" />
</div>

<asp:UpdatePanel runat="server" ID="EngineForm" Visible="False">
    <ContentTemplate>
    <fieldset>
        <legend>
            <asp:Label runat="server" CssClass="NormalBold" Text="Engine settings"></asp:Label>
        </legend>
        
        <table width="100%" cellpadding="4" class="EnginesTable">

		<tr>
		    <th width="160px">
		        <span>Name</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineName" runat="server" CssClass="NormalTextBox" Width="666px"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server"
                    ControlToValidate="EngineName" 
                    ErrorMessage="Engine name can not be empty" 
                    CssClass="ValidationMessageBlock" 
                    EnableViewState="True"
                    ></asp:RequiredFieldValidator>
                <p class="description">Id for the engine</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Friendly name</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineFriendlyName" runat="server" CssClass="NormalTextBox" Width="666px"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="EngineFriendlyName" 
                    ErrorMessage="Engine friendly name can not be empty" 
                    CssClass="ValidationMessageBlock" 
                    ></asp:RequiredFieldValidator>
                <p class="description">Frendly engine name</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Full path to executable</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineFullPath" runat="server" CssClass="NormalTextBox" Width="666px"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="EngineFullPath" 
                    ErrorMessage="Path to executable can not be empty" 
                    CssClass="ValidationMessageBlock" 
                    ></asp:RequiredFieldValidator>
                <p class="description">Path to a worker executable file</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Executable`s arguments</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineArguments" runat="server" CssClass="NormalTextBox" Width="666px"></asp:TextBox>
                <p class="description">Set of arguments to be passed to a worker on a call, can include special environment variables</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Transport</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:DropDownList runat="server" ID="EngineTransport"></asp:DropDownList>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Protocol</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:DropDownList runat="server" ID="EngineProtocol"></asp:DropDownList>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Lower port</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EnginePortLower" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
                <asp:RangeValidator runat="server" 
                    ControlToValidate="EnginePortLower" 
                    MinimumValue="1" MaximumValue="65535"
                    ErrorMessage="Out of range"
                    CssClass="ValidationMessage"
                    ></asp:RangeValidator>
                <p class="description">Specifies lower automatic port range bound for TCP transport, default = 49152</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Upper port</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EnginePortUpper" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
                <asp:RangeValidator runat="server" 
                    ControlToValidate="EnginePortUpper" 
                    MinimumValue="1" MaximumValue="65535"
                    ErrorMessage="Out of range"
                    CssClass="ValidationMessage"
                    ></asp:RangeValidator>
                <p class="description">Specifies upper automatic port range bound for TCP transport, default = 65535</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Minimum instances</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineMinInstances" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" 
                    ControlToValidate="EngineMinInstances"
                    ValidationExpression="\d*"
                    ErrorMessage="Out of range"
                    CssClass="ValidationMessage"
                    ></asp:RegularExpressionValidator>
                <p class="description">Minimum number of worker instances</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Maximum instances</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineMaxInstances" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" 
                    ControlToValidate="EngineMaxInstances"
                    ValidationExpression="\d*"
                    ErrorMessage="Out of range"
                    CssClass="ValidationMessage"
                    ></asp:RegularExpressionValidator>
                <p class="description">Maximum number of instances of worker processes to start under high load. Default value of 0 means start as many instances as there are cores.</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Time limit</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineTimeLimit" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" 
                    ControlToValidate="EngineTimeLimit"
                    ValidationExpression="\d*"
                    ErrorMessage="Out of range"
                    CssClass="ValidationMessage"
                    ></asp:RegularExpressionValidator>
                <p class="description">Restart the worker after it has worked for specific amount of time (minutes), default = 0 (infinite)</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Graceful Shutdown Timeout</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineGracefulShutdownTimeout" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" 
                    ControlToValidate="EngineGracefulShutdownTimeout"
                    ValidationExpression="\d*"
                    ErrorMessage="Out of range"
                    CssClass="ValidationMessage"
                    ></asp:RegularExpressionValidator>
                <p class="description">XXXXXXXXXXXXXXXXXXX</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Memory limit</span>
		    </th>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="EngineMemoryLimit" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" 
                    ControlToValidate="EngineMemoryLimit"
                    ValidationExpression="\d*"
                    ErrorMessage="Out of range"
                    CssClass="ValidationMessage"
                    ></asp:RegularExpressionValidator>
                <p class="description">Restart the worker if it has consumed specific amount of memory (megabytes), default = 0 (unlimited)</p>
            </td>
		</tr>

		<tr>
		    <th>
		        <span>Environment variables</span>
		    </th>
		    <td class="Normal" valign="top">
		        <table id="EngineEnvs">
		            <tr>
		                <th>Key</th>
                        <th>Value</th>
		            </tr>
                    <tr>
                        <td><asp:TextBox ID="EnvKey1" runat="server" CssClass="NormalTextBox" Width="240px"></asp:TextBox></td>
                        <td><asp:TextBox ID="EnvValue1" runat="server" CssClass="NormalTextBox" Width="440px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="EnvKey2" runat="server" CssClass="NormalTextBox" Width="240px"></asp:TextBox></td>
                        <td><asp:TextBox ID="EnvValue2" runat="server" CssClass="NormalTextBox" Width="440px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="EnvKey3" runat="server" CssClass="NormalTextBox" Width="240px"></asp:TextBox></td>
                        <td><asp:TextBox ID="EnvValue3" runat="server" CssClass="NormalTextBox" Width="440px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="EnvKey4" runat="server" CssClass="NormalTextBox" Width="240px"></asp:TextBox></td>
                        <td><asp:TextBox ID="EnvValue4" runat="server" CssClass="NormalTextBox" Width="440px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="EnvKey5" runat="server" CssClass="NormalTextBox" Width="240px"></asp:TextBox></td>
                        <td><asp:TextBox ID="EnvValue5" runat="server" CssClass="NormalTextBox" Width="440px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="EnvKey6" runat="server" CssClass="NormalTextBox" Width="240px"></asp:TextBox></td>
                        <td><asp:TextBox ID="EnvValue6" runat="server" CssClass="NormalTextBox" Width="440px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="EnvKey7" runat="server" CssClass="NormalTextBox" Width="240px"></asp:TextBox></td>
                        <td><asp:TextBox ID="EnvValue7" runat="server" CssClass="NormalTextBox" Width="440px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="EnvKey8" runat="server" CssClass="NormalTextBox" Width="240px"></asp:TextBox></td>
                        <td><asp:TextBox ID="EnvValue8" runat="server" CssClass="NormalTextBox" Width="440px"></asp:TextBox></td>
                    </tr>
		        </table>
            </td>
		</tr>

        </table>
    </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>

<br/>
<asp:Panel runat="server" ID="EngineFormButtons" Visible="False">
    <div class="FormButtonsBar">
        <asp:Button runat="server" ID="ButtonSaveEngine" Text="Save" CssClass="Button2" OnClick="ButtonSaveEngine_Click"></asp:Button>
        &nbsp;
        <asp:Button runat="server" ID="ButtonCancelEngineForm" Text="Cancel" CssClass="Button2" OnClick="ButtonCancelEngineForm_Click"></asp:Button>
    </div>
</asp:Panel>

<asp:GridView runat="server" ID="EngineGrid" 
    AutoGenerateColumns="False"
    EmptyDataText="Engines not found. Please reinstall Helicon Zoo."
    EnableViewState="True"
    ShowHeader="False"
    OnRowCommand="EngineGrid_OnRowCommand"
    >
    <Columns>
        <asp:TemplateField runat="server" ItemStyle-Width="75%">
            <ItemTemplate>
                <span runat="server" class="NormalBold"><%# Eval("displayName")%></span>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField runat="server" ItemStyle-Width="25%">
            <ItemTemplate>
                <asp:Button ID="ButtonEditEngine" runat="server"
                    CssClass='Button1'
                    Text = 'Edit'
			        CommandArgument='<%# Eval("name") %>'
			        CommandName="EngineEdit" 
                     />
                <asp:Button ID="ButtonDeleteEngine" runat="server"
                    CssClass='Button1'
                    Text="Delete"
			        CommandArgument='<%# Eval("name") %>'
			        CommandName="EngineDelete" 
                    Visible='<%# (bool)Eval("isUserEngine") %>'
                     />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>

</fieldset>

</asp:Panel>