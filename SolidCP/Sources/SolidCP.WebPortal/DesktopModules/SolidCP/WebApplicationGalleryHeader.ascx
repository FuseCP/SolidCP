<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebApplicationGalleryHeader.ascx.cs" Inherits="SolidCP.Portal.WebApplicationGalleryHeader" %>

<table>
	<tr>
	    <td style="padding: 0px 20px 20px 0px;"><asp:Image runat="server" ID="imgLogo" Width="200" Height="200" /></td>
	    <td style="vertical-align: top;">
	        <table>
	            <tr>
	                <td colspan="2" class="Huge" style="padding-bottom: 15px;">
	                    <asp:Label runat="server" ID="lblTitle" />
	                </td>	        
	            </tr>
	            <tr>
	                <td colspan="2" style="padding-bottom: 15px;">
	                    <asp:Label runat="server" ID="lblDescription" />
	                </td>	        
	            </tr>
	            <tr>
	                <td class="NormalBold" style="width:20%;"><asp:Literal runat="server" ID="litVersion" meta:resourcekey="litVersion" /></td>
	                <td style="width:80%;"><asp:Label runat="server" ID="lblVersion" /></td>
	            </tr>
	            <tr>
	                <td class="NormalBold"><asp:Literal runat="server" ID="litAuthor" meta:resourcekey="litAuthor"/></td>
	                <td><asp:HyperLink runat="server" id="hlAuthor" Target="_blank" /></td>
	            </tr>
	            <tr>
	                <td class="NormalBold"><asp:Literal runat="server" ID="litSize" meta:resourcekey="litSize"/></td>
	                <td><asp:Label runat="server" ID="lblSize" /></td>
	            </tr>
	        </table>
	    </td>
	</tr>

</table>
    
