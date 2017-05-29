<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMOrganizationDetails.ascx.cs" Inherits="SolidCP.Portal.CRM.CRMOrganizationDetails" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="CRMLogo" runat="server" />
					<asp:Localize ID="locTitle" runat="server"  Text="CRM Organization"></asp:Localize>
				</div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
				    <div >
				       <table>
				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblName" meta:resourcekey="lblName"/></td>
				            <td><asp:Label runat="server" ID="lblCrmOrgName" />&nbsp;&nbsp; <span class="Huge"><asp:HyperLink runat="server" id="hlOrganizationPage" Visible="false"  Target="_blank" /></span></td>
				          </tr>
				          
				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblOrgID" meta:resourcekey="lblOrgID"/></td>
				            <td><asp:Label runat="server" ID="lblCrmOrgId" /></td>
				          </tr>
				          
				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblAdministrator" meta:resourcekey="lblAdministrator"/></td>
				            <td>
                                <scp:UserSelector  id="administrator" runat="server" IncludeMailboxes="true">
                                </scp:UserSelector>
                                <asp:Label runat="server" ID="lblAdminValid" Text="*" ForeColor="red" Visible="false" />
                                <asp:Label runat="server" ID="lblAdmin" />
                                </td>
				          </tr>
				          
				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblCurrency" meta:resourcekey="lblCurrency"/></td>
				            <td><asp:DropDownList runat="server" ID="ddlCurrency" /></td>
				          </tr>
				          				          				        
                          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblCollation" meta:resourcekey="lblCollation"/></td>
				            <td><asp:DropDownList runat="server" ID="ddlCollation" /></td>
				          </tr>                         

				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblBaseLanguage" meta:resourcekey="lblBaseLanguage" Text="Base Language"/></td>
				            <td><asp:DropDownList runat="server" ID="ddlBaseLanguage" /></td>
				          </tr>

				       </table>			            
			     
			            
				    </div>
				</div>
       <div class="panel-footer text-right">
					    <asp:Button runat="server" meta:resourcekey="btnCreate" ID="btnCreate" CssClass="Button2" OnClick="btnCreate_Click"  />		
					    <asp:Button runat="server" meta:resourcekey="btnDelete" ID="btnDelete" CssClass="Button2" Visible="false" OnClick="btnDelete_Click" />			    					    
				    </div>