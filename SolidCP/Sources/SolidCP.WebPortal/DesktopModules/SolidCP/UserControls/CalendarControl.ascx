<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarControl.ascx.cs" Inherits="SolidCP.Portal.CalendarControl" %>

<asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Width="100px">22/22/2006</asp:TextBox>
    <asp:RequiredFieldValidator id="dateValidator" CssClass="NormalBold" runat="server" ControlToValidate="txtDate"
	Display="Dynamic" ErrorMessage="*" Enabled="false"></asp:RequiredFieldValidator>
    <ajaxToolkit:CalendarExtender runat="server" ID ="Calendar" TargetControlID="txtDate"  />
    <asp:CompareValidator ID="CompareValidator" runat = "server" ControlToValidate = "txtDate" EnableClientScript = "true" Type = "Date" Operator = "DataTypeCheck" Display = "Dynamic" ErrorMessage = "Invalid date">
</asp:CompareValidator>

  

	

                   

