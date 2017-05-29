<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesHeliconZooControl.ascx.cs" Inherits="SolidCP.Portal.WebSitesHeliconZooControl" %>
<%@ Import Namespace="SolidCP.Portal" %>

<%--
<p class="NormalBold">Web engines allowed for this site:</p>
<asp:Panel ID="AllowedEnginesPanel" runat="server">
    <asp:DataList ID="AllowedEnginesList" runat="server" RepeatColumns="1">
        <ItemTemplate>
            <div class="Quota">
                <%# Eval("DisplayName") %>
            </div>
        </ItemTemplate>
    </asp:DataList>   
</asp:Panel>
<br/>

<p class="NormalBold">Web engines enabled for this site:</p>
<asp:Panel ID="EnabledEnginesPanel" runat="server">
    <asp:DataList ID="EnabledEnginesList" runat="server" RepeatColumns="1">
        <ItemTemplate>
            <div class="Quota">
                <%# Container.DataItem.ToString() %>
            </div>
        </ItemTemplate>
    </asp:DataList>   
</asp:Panel>
<br
--%>

<p>
<asp:Label  ID="lblConsole" runat="server" meta:resourcekey="EnableWebConsole" CssClass="NormalBold"></asp:Label>
</p>
<br />

<%--<asp:GridView id="gvInstalledApplications" runat="server" AutoGenerateColumns="True" AllowPaging="true" 
	ShowHeader="false" CssSelectorClass="LightGridView" EmptyDataText="gvInstalledApplications.Empty"
	>
</asp:GridView>--%>

  <asp:GridView ID="gvInstalledApplications" runat="server" 
    EnableViewState="True" AutoGenerateColumns="false"
        ShowHeader="true" CssSelectorClass="NormalGridView" 
    EmptyDataText="gvAppVirtualDirectories" 
    onrowcommand="gvInstalledApplications_RowCommand">
        <Columns>
              <asp:BoundField DataField="Name" HeaderText="Name">
                <ItemStyle Width="60%" />
            </asp:BoundField>
           
           <asp:TemplateField HeaderText="gvInstalledApplicationsEnableConsole">
		    <ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
			    <CPCC:StyleButton id="btnDisable" CssClass="btn btn-danger" runat="server" CommandArgument='<%# Eval("Name") %>' CommandName="DisableConsole" Visible= '<%# !IsNullOrEmpty( (string)Eval("ConsoleUrl")) %>'> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDisableText"/> </CPCC:StyleButton>&nbsp;
                <CPCC:StyleButton id="btnEnable" CssClass="btn btn-success" runat="server" CommandArgument='<%# Eval("Name") %>' CommandName="EnableConsole" Visible= '<%# IsNullOrEmpty( (string)Eval("ConsoleUrl")) %>'> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnEnableText"/> </CPCC:StyleButton>
                    &nbsp;&nbsp;
                        
			    <asp:hyperlink 
                    CssClass="MediumBold" 
                    NavigateUrl='<%# GetConsoleFullUrl((string)Eval("ConsoleUrl")) %>' 
                    runat="server" 
                    ID="lnkAppDetails" 
                    ToolTip='<%# Eval("ConsoleUrl") %>' 
                    Target="_blank" 
                    Visible= '<%# !IsNullOrEmpty( (string)Eval("ConsoleUrl")) %>'>
			            Open console
                </asp:hyperlink>
              
           
			</ItemTemplate>
		</asp:TemplateField>
        
    
        </Columns>
    </asp:GridView>


<p>
<asp:Label runat="server" meta:resourcekey="SelectWebEngine" CssClass="NormalBold"></asp:Label>
</p>
<br />


<asp:GridView id="gvApplications" runat="server" AutoGenerateColumns="False" AllowPaging="true" 
	ShowHeader="false" CssSelectorClass="LightGridView" EmptyDataText="There are no applications" OnRowCommand="gvApplications_RowCommand" 
	OnPageIndexChanging="gvApplications_PageIndexChanging">
	<Columns>
		<asp:TemplateField HeaderText="gvApplicationsApplication">
		    <ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
				<div style="text-align:center;">
					<%-- <asp:hyperlink NavigateUrl='<%# GetWebAppInstallUrl(Eval("Id").ToString()) %>'
							runat="server" ID="Hyperlink3" ToolTip='<%# Eval("Title") %>'>
                    --%>
						<asp:Image runat="server" ID="Image1" Width="120" Height="120"
                            ImageUrl='<%# GetIconUrlOrDefault((string)Eval("IconUrl"))  %>'
							AlternateText='<%# Eval("Title") %>'>
						</asp:Image>
					<%-- </asp:hyperlink> --%>
				</div>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<div class="MediumBold" style="padding:4px;">
					<%-- <asp:hyperlink CssClass="MediumBold" NavigateUrl='<%# GetWebAppInstallUrl(Eval("Id").ToString()) %>'
					    runat="server" ID="lnkAppDetails" ToolTip='<%# Eval("Title") %>'>
                     --%>
						<span class="MediumBold"><%# Eval("Title")%></span>
					<%-- </asp:hyperlink> --%>
				</div>
				<div class="Normal" style="padding:4px;">
					<%# Eval("Summary") %>
				</div>				
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="gvApplicationsApplication">
		    <ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
			    <CPCC:StyleButton id="btnInstall" CssClass="btn btn-success" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Install"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnInstallText"/> </CPCC:StyleButton>
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>

