<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainsImportZone.ascx.cs" Inherits="SolidCP.Portal.DomainsImportZone" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<asp:Panel ID="pnlImport" runat="server">
    <div class="panel-body form-horizontal">
        <div class="Huge" style="padding: 10px;border: solid 1px #e5e5e5;background-color: #f5f5f5;">
            <asp:Literal ID="litDomainName" runat="server"></asp:Literal>
        </div>
    </div>
    <fieldset id="ImportPanel" runat="server" visible="True">
        <div class="FormRow" runat="server">
            <div class="col-md-12">
                <form enctype="multipart/form-data">
                    <label for="file">Zone file to upload</label>
                    <input id="file" type="file" name="zoneFile" runat="server" required/>
                    <br/>
                    <CPCC:StyleButton ID="UploadZoneFile" runat="server" CssClass="btn btn-primary"
                                      Text="Upload zone file"
                                      OnClientClick="ShowProgressDialog('Uploading zone file');"
                                      OnClick="UploadZoneFile_OnClick"></CPCC:StyleButton>
                    <br/>
                    <br/>
                </form>
            </div>
        </div>
    </fieldset>
</asp:Panel>