<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditOCSUser.ascx.cs" Inherits="SolidCP.Portal.OCS.EditOCSUser" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register src="../ExchangeServer/UserControls/MailboxSelector.ascx" tagname="MailboxSelector" tagprefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div id="ExchangeContainer">
    <div class="Module">
        <div class="Left">
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="OCSLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                    -
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                </div>
                <div class="panel-body form-horizontal">
                    
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <table>
                        <tr>
                            <td>
                                <asp:Localize runat="server" ID="locDisplayName" meta:resourcekey="locDisplayName" />
                            </td>
                            <td class="Huge">
                                <asp:Label runat="server" ID="lblDisplayName" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize runat="server" ID="locEmail" meta:resourcekey="locEmail"/>
                            </td>
                            <td class="Huge">
                                <asp:Label runat="server" ID="lblSIP" />
                            </td>
                        </tr>
                    </table>
                    
                    <scp:CollapsiblePanel id="secFedaration" runat="server"
                        TargetControlID="pnlFedaration" meta:resourcekey="secFedaration" Text="Company Information"/>
                    
                    <asp:Panel runat="server" ID="pnlFedaration" >
                        <asp:CheckBox runat="server" ID="cbEnableFederation" meta:resourcekey="cbEnableFederation"/><br/>
                        <asp:CheckBox runat="server" ID="cbEnablePublicConnectivity" meta:resourcekey="cbEnablePublicConnectivity"/>                        
                    
                    </asp:Panel>
                    <br />
                    
                    <scp:CollapsiblePanel id="secArchiving" runat="server"
                        TargetControlID="pnlArchiving" meta:resourcekey="secArchiving" Text="Company Information"/>
                    
                    <asp:Panel runat="server" ID="pnlArchiving" >
                        <asp:CheckBox runat="server" ID="cbArchiveInternal" meta:resourcekey="cbArchiveInternal"/><br/>
                        <asp:CheckBox runat="server" ID="cbArchiveFederation" meta:resourcekey="cbArchiveFederation"/>                                            
                    </asp:Panel>
                    <br />
                    
                    <scp:CollapsiblePanel id="secPresence" runat="server"
                        TargetControlID="pnlPresence" meta:resourcekey="secPresence" Text="Company Information"/>
                    
                    <asp:Panel runat="server" ID="pnlPresence" >
                        <asp:CheckBox runat="server" ID="cbEnablePresence" meta:resourcekey="cbEnablePresence"/><br/>
                        <asp:Localize runat="server" ID="locNote" meta:resourcekey="locNote"/>                        
                    </asp:Panel>
                    
                    
                        
					<div class="panel-footer text-right">
					 <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>				 					                                                
				    </div>			
                </div>
            </div>
        </div>
    </div>
</div>
