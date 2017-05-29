<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditBlackBerryUser.ascx.cs" Inherits="SolidCP.Portal.BlackBerry.EditBlackBerryUser" %>
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
                    <asp:Image ID="Image1" SkinID="BlackBerryUsersLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </div>
                <div class="panel-body form-horizontal">
                    
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <scp:CollapsiblePanel id="secPassword" runat="server"
                        TargetControlID="pnlSetPassword" meta:resourcekey="secPassowrd">
                    </scp:CollapsiblePanel>
                    
                    <asp:Panel runat="server" ID="pnlSetPassword">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td><asp:RadioButton runat="server" ID="rbSpecifyPassword" OnCheckedChanged="rbSpecifyPassword_OnCheckedChanged" Checked="true" AutoPostBack="true" meta:resourcekey="rbSpecifyPassword" GroupName="Password"/></td>
                        </tr>
                        <tr>
                            <td><asp:RadioButton OnCheckedChanged="rbGeneratePassword_OnCheckedChanged"  AutoPostBack="true" runat="server" ID="rbGeneratePassword" meta:resourcekey="rbGeneratePassword" GroupName="Password" /></td>
                        </tr>
                    </table>
                    
                    <table runat="server" id="tblPassword" visible="true">
                        <tr>
                            <td class="FormLabel150"><asp:Localize runat="server" ID="locPassword" meta:resourcekey="locPassword"></asp:Localize></td>
                            <td><asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator> 
                            </td>
                        </tr>
                        <tr>
                            <td  class="FormLabel150"><asp:Localize runat="server" ID="locTime" meta:resourcekey="locTime"/></td>
                            <td><asp:TextBox runat="server" ID="txtTime" CssClass="form-control" Text="48" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTime" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RangeValidator runa="server" ControlToValidate="txtTime"  ErrorMessage="*" Type="Integer" MinimumValue="1" MaximumValue="720" />
                                
                            </td>
                        </tr>
                    </table>                        
                        </ContentTemplate>
                    </asp:UpdatePanel>                    
                    <br />
                    <CPCC:StyleButton id="btnSetPassword" CssClass="btn btn-primary" runat="server" OnClick="btnSetPassword_Click"> <i class="fa fa-lock">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetPassword"/> </CPCC:StyleButton>              
                    </asp:Panel>
                    <br />                    
                   
                    
                    
                    <asp:GridView runat="server" ID="dvStats" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%"  CssSelectorClass="NormalGridView" ShowHeader="true" ShowFooter="false">
                        <Columns>
                            <asp:BoundField DataField="Name" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Value" />
                        </Columns>
                    </asp:GridView>
                        
					<div class="panel-footer text-right">
					<CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click" CausesValidation="false"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp; 
                    <CPCC:StyleButton id="btnDeleteData" CssClass="btn btn-danger" runat="server" OnClick="btnDeleteData_Click" CausesValidation="false"> <i class="fa fa-database">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteDataText"/> </CPCC:StyleButton>&nbsp; 
                    <CPCC:StyleButton id="btnSaveExit" CssClass="btn btn-success" runat="server" OnClick="btnSaveExit_Click"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveExitText"/> </CPCC:StyleButton>
				    </div>	
                </div>
            </div>
        </div>
    </div>
</div>