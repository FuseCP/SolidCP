<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestVirtualMachineTemplate.aspx.cs" Inherits="SolidCP.Portal.VPS2012.TestVirtualMachineTemplate" %>

<%@ Register src="../UserControls/MessageBox.ascx" tagname="MessageBox" TagPrefix="scp" %>

<%@ Register src="../UserControls/SimpleMessageBox.ascx" tagname="SimpleMessageBox" tagprefix="uc1" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Test VPS2012 Summary Template</title>
    <style type="text/css">
        BODY
        {
            background-color: #ffffff!important;
            margin: 5px!important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Test VPS2012 Summary Template
        </h1>
        
        <uc1:SimpleMessageBox ID="messageBox"  runat="server" />
        
        <p>
            Item ID: <br />
            <asp:TextBox ID="txtItemId" runat="server" Width="50"></asp:TextBox>
        </p>
        <p>
            Template:<br />
            <asp:TextBox ID="txtTemplate" runat="server" Width="100%" Rows="20" TextMode="MultiLine"></asp:TextBox>
            <br />
            <asp:Button ID="btnEvaluate" runat="server" onclick="btnEvaluate_Click" 
                Text="Evaluate" />
        </p>
        <p>
            Results:
            <br />
            <pre><asp:Literal ID="litResults" runat="server"></asp:Literal></pre>            
        </p>
    </div>
    </form>
</body>
</html>
