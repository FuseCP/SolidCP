<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SolidCP.EnterpriseServer.DefaultPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SolidCP Enterprise Server</title>
    <style>
		BODY
		{
			margin:0px;
			padding: 10px;
			font-family: Tahoma, Arial;
			font-size: 10pt;
		}
		
		.Content
		{
			width: 400px;
			margin-top: 30px;
			margin-left: auto;
			margin-right: auto;
            text-align: center;
		}
		
		H1
		{
			font-family: Arial, Tahoma;
			font-size: 18pt;
			margin-top: 40px;
			margin-bottom: 5px;
			font-weight: bold;
			padding: 4px;
		}
		
		TABLE
		{
			background-color: #e0e0e0;
			padding: 4px;
		}
		
		TD
		{
			padding: 8px;
			background-color: #ffffff;
		}
		
		.FieldName
		{
			width: 100px;
			font-weight: bold;
		}
    </style>
</head>
<body>
    <form id="AspForm" runat="server">
		<div class="Content">
			<div>
				<asp:Image ID="imgLogo" runat="server" />
			</div>
		
			<h1>Enterprise Server</h1>
			
			<table cellpadding="0" cellspacing="1" align="center">
				<tr>
					<td class="FieldName">Status:</td>
					<td>Running</td>
				</tr>
				<tr>
					<td class="FieldName">Version:</td>
					<td><asp:Literal id="litVersion" runat="server"></asp:Literal></td>
				</tr>
				<tr>
					<td class="FieldName">URL:</td>
					<td><asp:Literal id="litUrl" runat="server"/></td>
				</tr>
			</table>
            <br /><br />
            <a href="https://solidcp.com">SolidCP</a> &COPY; Copyright <%=DateTime.Now.Year%> All Rights Reserved.
		</div>
    </form>
</body>
</html>
