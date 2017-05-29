<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsToolsDeleteServer.ascx.cs" Inherits="SolidCP.Portal.VPS.VpsToolsDeleteServer" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
	     <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="DeleteServer48" runat="server" />
				    <scp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Delete Server" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
             <scp:ServerTabs id="tabs" runat="server" />	
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="Tools" ShowMessageBox="True" ShowSummary="False" />
                    
		            <p class="SubTitle">
		                <asp:Localize ID="locSubTitle" runat="server"
		                    meta:resourcekey="locSubTitle" Text="Delete VPS Server" />
		            </p>
                    <p>
                        <asp:Localize ID="locDescription" runat="server"
		                    meta:resourcekey="locDescription" Text="This wizard will delete VPS and all its contents from the virtualization server." />
                    </p>
                    <p>
                        <asp:CheckBox ID="chkConfirmDelete" runat="server" CssClass="NormalBold"
				                    meta:resourcekey="chkConfirmDelete" Text="Yes, I confirm deletion of this VPS" />
                    </p>
				    
                    <fieldset id="AdminOptionsPanel" runat="server">
                        <legend>
                            <asp:Localize ID="locAdminOptions" runat="server" meta:resourcekey="locAdminOptions" Text="Administrator options"></asp:Localize>
                        </legend>

                            <table cellspacing="5">
				                <tr>
				                    <td>
				                        <asp:CheckBox ID="chkSaveFiles" runat="server"
				                            meta:resourcekey="chkSaveFiles" Text="Do not delete VPS files (virtual hard disk, snapshots)" />
				                    </td>
				                </tr>
				                <tr>
				                    <td>
				                        <asp:CheckBox ID="chkExport" runat="server" AutoPostBack="true"
				                            meta:resourcekey="chkExport" Text="Export VPS before deletion to the following folder:" />
				                    </td>
				                </tr>
				                <tr>
				                    <td style="padding-left:20px;">
				                        <asp:TextBox ID="txtExportPath" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
        				                
				                        <asp:RequiredFieldValidator ID="ExportPathValidator" runat="server" Text="*" Display="Dynamic"
                                                ControlToValidate="txtExportPath" meta:resourcekey="ExportPathValidator" SetFocusOnError="true"
                                                ValidationGroup="Tools">*</asp:RequiredFieldValidator>
				                    </td>
				                </tr>
				            </table>
				     </fieldset>
				    
                    <p>
                        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click" ValidationGroup="Tools"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>
                    </p>
			    </div>
                </div>
                </div>
                </div>