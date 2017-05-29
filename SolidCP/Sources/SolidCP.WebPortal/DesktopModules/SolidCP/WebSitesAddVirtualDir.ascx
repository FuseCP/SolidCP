<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesAddVirtualDir.ascx.cs" Inherits="SolidCP.Portal.WebSitesAddVirtualDir" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />

<div class="panel-body form-horizontal">
    <fieldset>
        <div class="row">
            <div class="col-sm-12">
                <div class="form-group">
                    <CPCC:H5Label ID="lblDirectoryName" runat="server" for="txtSmtpServer" class="col-sm-2 control-label">
                                                <asp:Localize ID="locDirectoryName" runat="server" meta:resourcekey="lblAppDirectoryName" Text="Directory name:" />
                    </CPCC:H5Label>
                    <div class="col-sm-4">
                        <uc2:UsernameControl ID="virtDirName" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <CPCC:H5Label ID="lblFolder" runat="server" for="txtSmtpPort" class="col-sm-2 control-label">
                                                <asp:Localize ID="locFolder" runat="server" meta:resourcekey="lblFolder" Text="Folder:" />
                    </CPCC:H5Label>
                    <div class="col-sm-4">
                        <uc1:FileLookup ID="fileLookup" runat="server" ValidationEnabled="true" />
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton ID="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"><i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel" />
    </CPCC:StyleButton>
    <CPCC:StyleButton ID="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"><i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText" />
    </CPCC:StyleButton>
    &nbsp;
</div>
