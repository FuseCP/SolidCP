<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangePublicFolders.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangePublicFolders" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangePublicFolder48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Public Folders"></asp:Localize>
                    </h3>
				</div>
			     <div class="FormButtonsBar right">
                        <CPCC:StyleButton id="btnCreatePublicFolder" CssClass="btn btn-primary" runat="server" OnClick="btnCreatePublicFolder_Click"> <i class="fa fa-folder-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreatePublicFolderText"/> </CPCC:StyleButton>
                    </div>	
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    <asp:TreeView ID="FoldersTree" runat="server">
				    </asp:TreeView>			    
				</div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-md-6">
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Public Folders Created:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;
				    <scp:QuotaViewer ID="foldersQuota" runat="server" QuotaTypeId="2" />
                            </div>
                        <div class="col-md-6 text-right">
                                <CPCC:StyleButton id="btnDeleteFolders" CssClass="btn btn-danger" runat="server" OnClick="btnDeleteFolders_Click"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteFoldersText"/> </CPCC:StyleButton>
                            </div>
                        </div>
				</div>