<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSImportCollection.ascx.cs" Inherits="SolidCP.Portal.RDS.RDSImportCollection" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-default">
			    <div class="panel-heading">
					<asp:Image ID="imgAddRDSServer" SkinID="AddRDSServer48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Import RDS Collection"></asp:Localize>
				</div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />

					<table>
					    <tr>
						    <td class="FormLabel150" style="width: 100px;"><asp:Localize ID="locCollectionName" runat="server" meta:resourcekey="locCollectionName" Text="Collection Name"></asp:Localize></td>
						    <td>
                                <asp:TextBox ID="txtCollectionName" runat="server" CssClass="TextBox300" />
                                <asp:RequiredFieldValidator ID="valCollectionName" runat="server" ErrorMessage="*" ControlToValidate="txtCollectionName" ValidationGroup="SaveRDSCollection"></asp:RequiredFieldValidator>
						    </td>                            
					    </tr>                        
					</table>                                                                                                  
               </div>
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Importing collection...');" ValidationGroup="SaveRDSCollection"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
				    </div>
				</div>