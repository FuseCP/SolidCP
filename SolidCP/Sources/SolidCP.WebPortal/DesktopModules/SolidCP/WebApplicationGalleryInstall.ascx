<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebApplicationGalleryInstall.ascx.cs" Inherits="SolidCP.Portal.WebApplicationGalleryInstall" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register src="WebApplicationGalleryHeader.ascx" tagname="WebApplicationGalleryHeader" tagprefix="uc1" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="uc1" %>
    
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-body form-horizontal">

    <uc1:SimpleMessageBox ID="messageBox" runat="server" />
     		
    <uc1:WebApplicationGalleryHeader ID="appHeader" runat="server" />
    
    <asp:CheckBox ID="chIgnoreDependencies" runat="server"  
        Text="Ignore dependency fail and install selected product anyway."  Visible="false" AutoPostBack="True" 
        oncheckedchanged="chIgnoreDependencies_CheckedChanged" />
        

</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnInstall" CssClass="btn btn-success" runat="server" OnClick="btnInstall_Click" OnClientClick="ShowProgressDialog('Installing application...');"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnInstallText"/> </CPCC:StyleButton>
</div>