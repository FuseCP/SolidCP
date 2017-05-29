<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualDirectoryHomeFolderControl.ascx.cs" Inherits="SolidCP.Portal.VirtualDirectoryHomeFolderControl" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<div style="padding: 20px;">
<table>

	<tr>
	    <td width="30" nowrap></td>
	    <td>
	        <table id="tblFolder" runat="server">
	            <tr>
	                <td colspan="3">
			            <table width="100%">
				            <tr>
					            <td class="Normal" width="10%" nowrap valign="top" style="padding-top:7px;">
					                <asp:Label ID="lblSitePath" runat="server" meta:resourcekey="lblSitePath" Text="Content Path: "></asp:Label>&nbsp;
					            </td>
					            <td width="90%" valign="top">
                                    <uc1:FileLookup ID="fileLookup" runat="server" Width="300" />
                                </td>
				            </tr>
			            </table>     
	                </td>
	            </tr>
	            <tr>
	                <td colspan="3" class="Normal">&nbsp;</td>
	            </tr>
	            <tr>
	                <td valign="top" class="Normal">
			            <asp:PlaceHolder runat="server" id="pnlCustomAuth">
			            <br />
			            <table class="Normal" cellSpacing="0" cellPadding="3">
			                <tr>
			                    <td class="NormalBold">
			                        <asp:Label ID="lblAuthentication" runat="server" meta:resourcekey="lblAuthentication" Text="Authentication:"></asp:Label>
			                    </td>
			                </tr>
                            <tr>
	                            <td nowrap><asp:checkbox id="chkAuthAnonymous" meta:resourcekey="chkAuthAnonymous" Text="Enable anonymous access" Runat="server"></asp:checkbox></td>
                            </tr>
                            <tr>
	                            <td nowrap><asp:checkbox id="chkAuthWindows" meta:resourcekey="chkAuthWindows" Text="Integrated Windows authentication" Runat="server"></asp:checkbox></td>
                            </tr>
                            <tr>
	                            <td nowrap><asp:checkbox id="chkAuthBasic" meta:resourcekey="chkAuthBasic" Text="Basic authentication" Runat="server"></asp:checkbox></td>
                            </tr>
                        </table>
			            </asp:PlaceHolder>
	                </td>
	            </tr>
	        </table>
	    </td>
	</tr>
</table>
</div>