<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsDvd.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsDvd" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_dvd" />
	
                        <scp:SimpleMessageBox id="messageBox" runat="server" />
                        
			            <table style="margin: 50px 0px 50px 50px">
			                <tr>
			                    <td><asp:Localize ID="locDvdDrive" runat="server" meta:resourcekey="locDvdDrive" Text="DVD Drive:"></asp:Localize></td>
			                </tr>
			                <tr>
			                    <td>
			                        <asp:TextBox ID="txtInsertedDisk" runat="server" Width="400px"
			                            CssClass="NormalTextBox" ReadOnly="true"></asp:TextBox>
			                    </td>
			                </tr>
			                <tr>
			                    <td>
			                        <br />
			                        <br />
			                        <CPCC:StyleButton id="btnEjectDisk" CssClass="btn btn-warning" runat="server" OnClick="btnEjectDisk_Click" CausesValidation="false"> <i class="fa fa-stop-circle">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnEjectDiskText"/> </CPCC:StyleButton>&nbsp;
                                    <CPCC:StyleButton id="btnInsertDisk" CssClass="btn btn-success" runat="server" OnClick="btnInsertDisk_Click" CausesValidation="false"> <i class="fa fa-play-circle">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnInsertDiskText"/> </CPCC:StyleButton> 
			                    </td>
			                </tr>
			            </table>
     
			    </div>
		    </div>
	    </div>
