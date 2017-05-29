<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FtpAccountEditAccount.ascx.cs" Inherits="SolidCP.Portal.FtpAccountEditAccount" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc4" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />

<script type="text/javascript">

    function confirmation() {
        if (!confirm("Are you sure you want to delete this FTP Account?")) return false; else ShowProgressDialog('Deleting FTP Account...');
    }
</script>


<div class="panel-body form-horizontal">
    <div class="form-group">
        <asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="User name:" CssClass="control-label col-sm-2" AssociatedControlID="usernameControl"></asp:Label>
        <div class="col-sm-8">
            <uc4:UsernameControl ID="usernameControl" runat="server" />
        </div>
    </div>
    <uc3:PasswordControl ID="passwordControl" runat="server" />
    <br>
    <div class="form-group inline-form">
        <asp:Label ID="lblHomeFolder" runat="server" meta:resourcekey="lblHomeFolder" Text="Home folder:" CssClass="control-label col-sm-2">
            <asp:Localize ID="locMailboxSizeLimit" runat="server" meta:resourcekey="lblMailboxSizeLimit" />
        </asp:Label>
        <div class="col-sm-8">
            <uc2:FileLookup ID="fileLookup" runat="server" Width="300" />
        </div>
    </div>
    <br />
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton ID="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirmation();">
        <i class="fa fa-trash-o">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnDeleteText" />
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton ID="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel" />
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton ID="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Updating FTP Account...');">
        <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnSaveText" />
    </CPCC:StyleButton>
</div>
