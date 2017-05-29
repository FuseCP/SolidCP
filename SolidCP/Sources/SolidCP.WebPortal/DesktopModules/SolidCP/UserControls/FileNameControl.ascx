<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileNameControl.ascx.cs" Inherits="SolidCP.Portal.FileNameControl" %>
<asp:TextBox ID="txtFileName" runat="server" Width="200"></asp:TextBox>
<asp:RequiredFieldValidator ID="valRequireNewName" runat="server" meta:resourcekey="valRequireNewName" ControlToValidate="txtFileName"
    CssClass="NormalBold" Display="Dynamic" ErrorMessage="*" ValidationGroup="NewFileName"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="valCorrectNewName" runat="server" meta:resourcekey="valCorrectNewName" ControlToValidate="txtFileName"
    CssClass="NormalBold" Display="Dynamic" EnableClientScript="False" ErrorMessage="Invalid file name."
    ValidationExpression='(?i)^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:\";|/]+$'
    ValidationGroup="NewFileName"></asp:RegularExpressionValidator>