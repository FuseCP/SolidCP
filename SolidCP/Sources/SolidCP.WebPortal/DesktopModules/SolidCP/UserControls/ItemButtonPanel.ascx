<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemButtonPanel.ascx.cs" Inherits="SolidCP.Portal.ItemButtonPanel" %>
<CPCC:StyleButton id="btnSaveExit" runat="server"  CssClass="btn btn-success" 
    OnClick="btnSaveExit_Click" OnClientClick="ShowProgressDialog('Updating ...');">
    <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" id="btnSaveExitText" meta:resourcekey="btnSaveExit"/>
</CPCC:StyleButton>
<span class="pull-right">&nbsp;</span>
<CPCC:StyleButton id="btnSave" runat="server"  CssClass="btn btn-success" 
    OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Updating ...');">
    <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server"  id="btnSaveText" meta:resourcekey="btnSave"/>
</CPCC:StyleButton>

