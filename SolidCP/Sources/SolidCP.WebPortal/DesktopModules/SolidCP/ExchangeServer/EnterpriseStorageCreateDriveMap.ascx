<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseStorageCreateDriveMap.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.EnterpriseStorageCreateDriveMap" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="imgESDM" SkinID="EnterpriseStorageDriveMaps48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create New Drive Map"></asp:Localize>
    </h3>
</div>
<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="row">
        <div class="col form-inline">
            <label class="col-sm-2 control-label">
                <asp:Localize ID="locDriveLetter" runat="server" meta:resourcekey="locDriveLetter" Text="Select Drive Letter:"></asp:Localize>
            </label>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlLetters" runat="server" CssClass="form-control" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col form-inline">
            <label class="col-sm-2 control-label">
                <asp:Localize ID="locFolder" runat="server" meta:resourcekey="locFolder" Text="Storage Folder:"></asp:Localize>
            </label>
            <div class="form-group">
                <div class="input-group">
                    <div class="Folders" style="display:inline;">
                        <asp:DropDownList ID="ddlFolders" runat="server" CssClass="form-control" />  
                        <asp:HiddenField id="txtFolderName" runat="server"/>
                    </div>
                    <div class="Url" style="display:inline;">
                        &nbsp;&nbsp;
                        <span class="input-group-addon">
                            <i class="fa fa-hdd-o" aria-hidden="true"></i>
                        </span>
                        <span class="input-group-addon" style="background-color:#ffffff;">
                            <asp:Literal ID="lbFolderUrl" runat="server"></asp:Literal>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col form-inline">
            <label class="col-sm-2 control-label">
                <asp:Localize ID="locDriveLabel" runat="server" meta:resourcekey="locDriveLabel" Text="Label As:"></asp:Localize>
            </label>
            <div class="form-group">
                <div class="input-group">
                    <asp:TextBox ID="txtLabelAs" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click" ValidationGroup="CreateDriveMap">
        <i class="fa fa-check">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCreateText"/>
    </CPCC:StyleButton>
    <asp:ValidationSummary ID="valSummary" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateDriveMap" />
</div>	
<script type="text/javascript" >
    $('document').ready(function () {
        $('.LabelAs input').bind('click', function () { $('.LabelAs input').val(""); });

        $('.LabelAs input').bind('focusout', function () {
            if ($('.LabelAs input').val() == "") {
                $('.LabelAs input').val($('.Folders select option:selected').text());
            }
        });

        $('.Folders select').bind('change', function () {
            $('.LabelAs input').val($('.Folders select option:selected').text());
            $('.Url').text($('.Folders select option:selected').val());
            $('.Folders input').val($('.Folders select option:selected').text());
        });
    });
</script>