<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsDvd.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsDetailsDvd" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_dvd" />
	
                        <wsp:SimpleMessageBox id="messageBox" runat="server" />
                        
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
			                        <asp:Button ID="btnInsertDisk" runat="server" CausesValidation="false"
			                            Text="Insert Disk..." meta:resourcekey="btnInsertDisk" CssClass="Button1" 
                                        onclick="btnInsertDisk_Click" />
                                    <asp:Button ID="btnEjectDisk" runat="server" CausesValidation="false"
			                            Text="Eject" meta:resourcekey="btnEjectDisk" CssClass="Button1" 
                                        onclick="btnEjectDisk_Click" />
			                    </td>
			                </tr>
			            </table>
     
			    </div>
		    </div>
	    </div>
