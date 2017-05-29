<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileManager.ascx.cs" Inherits="SolidCP.Portal.FileManager" %>
<%@ Register Src="UserControls/FileNameControl.ascx" TagName="FileNameControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="uc4" %>
<%@ Register Src="UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>
	
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<asp:UpdatePanel ID="MessageBoxUpdatePanel" runat="server" UpdateMode="Always">
<ContentTemplate>
<scp:SimpleMessageBox id="messageBox" runat="server"></scp:SimpleMessageBox>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
	var gvFilesID = '<asp:Literal id="gvFilesID" runat="server"/>';
	function checkAll(selectAllCheckbox) {
	    //get all checkbox and select it
	    $('td :checkbox').prop("checked", selectAllCheckbox.checked);
	}
	function unCheckSelectAll(selectCheckbox) {
	    //if any item is unchecked, uncheck header checkbox as also
	    if (!selectCheckbox.checked)
	        $('th :checkbox').prop("checked", false);
	}

    function HighlightRow(chkB)
    {
		var xState=chkB.checked;    
		var row = chkB.parentNode.parentNode;
		if(xState)
		{
			row.setAttribute("temp_class", row.className);
			row.className = "SelectedRow";
		}
        else
        {
			if(row.getAttribute("temp_class"))
				row.className = row.getAttribute("temp_class");
			else
				row.className = "";
		}
    }
    
    function pageLoad(sender, args) {
        if (!args.get_isPartialLoad()) {
            $addHandler(document, 'keydown', onKeyDown);
        }
    }
    
    function onKeyDown(e) {
        if (e && e.keyCode == Sys.UI.Key.esc) {
            // if the key pressed is the escape key, dismiss all modal dialogs
            var c = Sys.Application.getComponents();
            for (var i=0; i<c.length; i++) {
                var type = Object.getType(c[i]).getName();
                if (AjaxControlToolkit.ModalPopupBehavior.isInstanceOfType(c[i])) 
                    c[i].hide();
            }
        }
    }
    
    function ShowUnzipFilesDialog() {
		ShowProgressDialog(FM_UNZIP_FILES_MESSAGE);
    }
  
    Sys.Application.add_load(modalPopupFocus);
</script>

<div class="FormButtonsBar">
		<CPCC:StyleButton ID="cmdUpload" runat="server" CssClass="btn btn-default" CausesValidation="false">
			<asp:Image ID="Img1" runat="server" SkinID="FM_Upload" /><asp:Localize ID="locUpload" runat="server" meta:resourcekey="locUpload" Text="Upload"/>
		</CPCC:StyleButton>
		<CPCC:StyleButton ID="cmdCreateFile" runat="server" CssClass="btn btn-default" CausesValidation="false">
			<asp:Image ID="Img2" runat="server" SkinID="FM_CreateFile" /><asp:Localize ID="locCreateFile" runat="server" meta:resourcekey="locCreateFile" Text="Create File"/>
		</CPCC:StyleButton>
		<CPCC:StyleButton ID="cmdCreateFolder" runat="server" CssClass="btn btn-default" CausesValidation="false">
			<asp:Image ID="Img3" runat="server" SkinID="FM_CreateFolder" /><asp:Localize ID="locCreateFolder" runat="server" meta:resourcekey="locCreateFolder" Text="Create Folder"/>
		</CPCC:StyleButton>
	
	<asp:Image ID="Image8" runat="server" SkinID="FM_Separator" />
	
		<CPCC:StyleButton ID="cmdCreateAccessDB" runat="server" CssClass="btn btn-default" CausesValidation="false" Enabled="false" >
			<asp:Image ID="Image2" runat="server" SkinID="FM_CreateAccessDB" /><asp:Localize ID="locCreateAccessDB" runat="server" meta:resourcekey="locCreateAccessDB" Text="Create Access DB" />
		</CPCC:StyleButton>
	
	<asp:Image ID="Image9" runat="server" SkinID="FM_Separator" />
	
		<CPCC:StyleButton ID="cmdZipFiles" runat="server" CssClass="btn btn-default" CausesValidation="false">
			<asp:Image ID="Image3" runat="server" SkinID="FM_Zip" /><asp:Localize ID="locZip" runat="server" meta:resourcekey="locZip" Text="Zip"/>
		</CPCC:StyleButton>
		<CPCC:StyleButton ID="cmdUnzipFiles" runat="server" CssClass="btn btn-default" CausesValidation="false" OnClick="cmdUnzipFiles_Click" OnClientClick="ShowUnzipFilesDialog();">
			<asp:Image ID="Image4" runat="server" SkinID="FM_Unzip" /><asp:Localize ID="locUnzip" runat="server" meta:resourcekey="locUnzip" Text="Unzip"/>
		</CPCC:StyleButton>
	
	<asp:Image ID="Image10" runat="server" SkinID="FM_Separator" />
	
		<CPCC:StyleButton ID="cmdCopyFiles" runat="server" CssClass="btn btn-default" CausesValidation="false">
			<asp:Image ID="Image5" runat="server" SkinID="FM_Copy" /><asp:Localize ID="locCopy" runat="server" meta:resourcekey="locCopy" Text="Copy"/>
		</CPCC:StyleButton>
		<CPCC:StyleButton ID="cmdMoveFiles" runat="server" CssClass="btn btn-default" CausesValidation="false">
			<asp:Image ID="Image6" runat="server" SkinID="FM_Move" /><asp:Localize ID="locMove" runat="server" meta:resourcekey="locMove" Text="Move"/>
		</CPCC:StyleButton>
		<CPCC:StyleButton ID="cmdDeleteFiles" runat="server" CssClass="btn btn-default" CausesValidation="false">
			<asp:Image ID="Image7" runat="server" SkinID="FM_Delete" /><asp:Localize ID="locDelete" runat="server" meta:resourcekey="locDelete" Text="Delete"/>
		</CPCC:StyleButton>
</div>

<asp:Panel ID="UploadFilePanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="btnUpload">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-upload"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblUploadFile" Text="Upload File" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:FileUpload ID="fileUpload1" runat="server" Width="400px" />
				<asp:FileUpload ID="fileUpload2" runat="server" Width="400px" />
				<asp:FileUpload ID="fileUpload3" runat="server" Width="400px" />
				<asp:FileUpload ID="fileUpload4" runat="server" Width="400px" />
				<asp:FileUpload ID="fileUpload5" runat="server" Width="400px" />
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelUpload" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnUpload" CssClass="btn btn-success" runat="server" ValidationGroup="UploadFile" OnClick="btnUpload_Click"> <i class="fa fa-upload">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUploadText"/> </CPCC:StyleButton>
	</div>
  </div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="UploadModal" runat="server"
    TargetControlID="cmdUpload" PopupControlID="UploadFilePanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelUpload" />

<asp:Panel ID="CopyFilesPanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="btnCopy">
     <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-files-o"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblCopySelectedFiles" Text="Copy Selected Files" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblDestinationFolder1" runat="server" meta:resourcekey="lblDestinationFolder" Text="Destination Folder:"></asp:Label>
				<uc1:FileLookup ID="copyDestination" runat="server" Width="400px" DropShadow="False" ValidationGroup="CopyFiles" />
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelCopy" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnCopy" CssClass="btn btn-success" runat="server" OnClick="btnCopy_Click" ValidationGroup="CopyFiles"> <i class="fa fa-clone">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCopyText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="CopyFilesModal" runat="server"
    TargetControlID="cmdCopyFiles" PopupControlID="CopyFilesPanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelCopy" />

<asp:Panel ID="MoveFilesPanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="btnMove">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-clipboard"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblMoveSelectedFiles" Text="Move Selected Files" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblDestinationFolder2" runat="server" meta:resourcekey="lblDestinationFolder" Text="Destination Folder:"></asp:Label>
				<uc1:FileLookup ID="moveDestination" runat="server" Width="400px" DropShadow="False" ValidationGroup="MoveFiles" />
			</div>
			</div>
					<div class="popup-buttons text-right">
			<CPCC:StyleButton id="btnCancelMove" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnMove" CssClass="btn btn-success" runat="server" OnClick="btnMove_Click" ValidationGroup="MoveFiles"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnMoveText"/> </CPCC:StyleButton>
		</div>
     </div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="MoveFilesModal" runat="server"
    TargetControlID="cmdMoveFiles" PopupControlID="MoveFilesPanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelMove" />


<asp:Panel ID="CreateFilePanel" runat="server" CssClass="PopupContainer mpeTarget" style="display:none" DefaultButton="btnCreateFile">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-file-code-o"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblCreateFile" Text="Create Text File" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblFileName" runat="server" meta:resourcekey="lblFileName" Text="File Name:"></asp:Label>
				<uc2:FileNameControl ID="txtFileName" runat="server" ValidationGroup="NewFileName" Width="400px" />
			</div>
			<div class="FormRow">
				<asp:Label ID="lblFileContentOptional" runat="server" meta:resourcekey="lblFileContentOptional" Text="File Content (Optional):"></asp:Label>
				<asp:TextBox ID="txtFileContent" runat="server" Rows="10" TextMode="MultiLine" Width="100%" Wrap="False"></asp:TextBox>
			</div>
            </div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelCreateFile" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnCreateFile" CssClass="btn btn-success" runat="server" OnClick="btnCreateFile_Click" ValidationGroup="NewFileName"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="CreateFileModal" runat="server"
    TargetControlID="cmdCreateFile" PopupControlID="CreateFilePanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelCreateFile" />
    
    
<asp:Panel ID="CreateFolderPanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="btnCreateFolder">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-folder"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblCreateFolder" Text="Create Folder" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblFolderName" runat="server" meta:resourcekey="lblFolderName" Text="Folder Name:"></asp:Label>
				<uc2:FileNameControl ID="txtFolderName" runat="server" ValidationGroup="NewFolderName" Width="400px" />
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelCreateFolder" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnCreateFolder" CssClass="btn btn-success" runat="server" OnClick="btnCreateFolder_Click" ValidationGroup="NewFolderName"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="CreateFolderModal" runat="server"
    TargetControlID="cmdCreateFolder" PopupControlID="CreateFolderPanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelCreateFolder" />

<asp:Panel ID="ZipFilesPanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="btnZip">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-file-archive-o"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblZipFiles" Text="Zip Selected Files" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblZIPArchiveName" runat="server" meta:resourcekey="lblZIPArchiveName" Text="ZIP Archive Name:"></asp:Label>
				<uc2:FileNameControl ID="txtZipName" runat="server" ValidationGroup="ZipName" Width="400px" />
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelZip" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnZip" CssClass="btn btn-success" runat="server" OnClick="btnZip_Click" ValidationGroup="ZipName"> <i class="fa fa-file-archive-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnZipText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="ZipFilesModal" runat="server"
    TargetControlID="cmdZipFiles" PopupControlID="ZipFilesPanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelZip" />

<asp:Panel ID="CreateDatabasePanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="btnCreateDatabase">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-database"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblCreateAccessDatabase" Text="Create Access Database" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblDatabaseName" runat="server" meta:resourcekey="lblDatabaseName" Text="Database Name:"></asp:Label>
				<uc2:FileNameControl ID="txtDatabaseName" runat="server" ValidationGroup="DatabaseName" Width="400px" />
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelCreateDatabase" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnCreateDatabase" CssClass="btn btn-success" runat="server" OnClick="btnCreateDatabase_Click" ValidationGroup="DatabaseName"> <i class="fa fa-database">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="CreateDatabaseModal" runat="server"
    TargetControlID="cmdCreateAccessDB" PopupControlID="CreateDatabasePanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelCreateDatabase" />


<asp:Panel ID="DeleteFilesPanel" runat="server" CssClass="PopupContainer" style="display:none">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-trash"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblDeleteFiles" Text="Delete Selected Files" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblDeleteConfirmation" runat="server" meta:resourcekey="lblDeleteConfirmation" Text="Do you really want to delete selected files and folders?"></asp:Label>
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnDeleteFiles" CssClass="btn btn-danger" runat="server" OnClick="btnDeleteFiles_Click" CausesValidation="false"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteFilesText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnCancelDeleteFiles" CssClass="btn btn-warning" runat="server"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelDeleteFilesText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="DeleteFilesModal" runat="server"
    TargetControlID="cmdDeleteFiles" PopupControlID="DeleteFilesPanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelDeleteFiles" />
    

<asp:UpdatePanel ID="FilesUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
 <div class="FormButtonsBar">
	<table cellpadding="2" cellspacing="0" width="100%">
		<tr>
			<td class="Medium" style="width:100%;">
				<asp:Repeater ID="path" Runat="server" OnItemCommand="path_ItemCommand">
					<ItemTemplate><asp:LinkButton ID="fileName" Runat="server" CssClass=CommandButton CommandName="browse" CommandArgument='<%# Eval("FullName")%>' Text='<%# Eval("Name")%>' CausesValidation="false">
					</asp:LinkButton></ItemTemplate>
					<SeparatorTemplate>
						&nbsp;/&nbsp;
					</SeparatorTemplate>
				</asp:Repeater>
			</td>
			<td>
			    <asp:UpdateProgress ID="filesProgress" runat="server"
			        AssociatedUpdatePanelID="FilesUpdatePanel" DynamicLayout="false">
			        <ProgressTemplate>
			            <asp:Image ID="imgSep" runat="server" SkinID="MediumAjaxIndicator" />
			        </ProgressTemplate>
			    </asp:UpdateProgress>
			</td>
		</tr>
	</table>
</div><div class="NormalGridView">
	<div class="AspNet-GridView">
		<table cellpadding="0" cellspacing="0">
			<thead>
				<tr>
					<th style="width:25px;"><asp:CheckBox ID="selectAll" Runat="server" onclick="checkAll(this);"></asp:CheckBox></th>
					<th><asp:Localize ID="locFileName" runat="server" meta:resourcekey="locFileName" /></th>
					<th style="width:65px;"><asp:Localize ID="locSize" runat="server" meta:resourcekey="locSize" /></th>
					<th style="width:135px;"><asp:Localize ID="locModified" runat="server" meta:resourcekey="locModified" /></th>
				</tr>
			</thead>
		</table>
	</div>
</div><div style="height:350px;overflow:auto;">
	<asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False"
		AllowSorting="True" CssSelectorClass="NormalGridView" ShowHeader="false"
		EmptyDataText="gvFiles" DataKeyNames="Name" OnRowCommand="gvFiles_RowCommand"
		DataSourceID="odsFilesPaged" PageSize="20" EnableViewState="true">
		<Columns>
			<asp:TemplateField>
				<ItemStyle Width="25px"></ItemStyle>
				<ItemTemplate>
					<asp:CheckBox ID="selected" Runat="server" onclick="javascript:HighlightRow(this);"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="gvFilesFileName">
				<ItemTemplate>
				    <img src="<%# GetFileIcon(Container.DataItem) %>" align=absmiddle border="0" hspace="2">
					<asp:HyperLink ID="lnkDownload" runat="server" Visible='<%# !(bool)Eval("IsDirectory") %>'
						NavigateUrl='<%# GetDownloadLink((string)Eval("Name")) %>'>
						<%# Eval("Name")%>
					</asp:HyperLink>
					<asp:LinkButton ID="fileName" Runat="server" CssClass=CommandButton CausesValidation="false"
							CommandName='<%# (bool)Eval("IsDirectory") ? "browse" : "download" %>' Visible='<%# (bool)Eval("IsDirectory") %>'
							CommandArgument='<%# Eval("Name") %>'>
						<%# Eval("Name")%>
					</asp:LinkButton>
					
					<asp:ImageButton ID="cmdRenameFile" runat="server" SkinID="RenameFile" AlternateText="Rename file/folder"
							CommandName='rename' meta:resourcekey="cmdRenameFile" CausesValidation="false"
							CommandArgument='<%# Eval("Name") %>'></asp:ImageButton>
					<asp:ImageButton ID="cmdEditFile" runat="server" SkinID="EditFile" AlternateText="Edit file"
						visible='<%# IsEditable(Container.DataItem) %>'
							CommandName='edit_file' meta:resourcekey="cmdEditFile" CausesValidation="false"
							CommandArgument='<%# Eval("Name") %>'></asp:ImageButton>
					<asp:ImageButton ID="cmdEditPermissions" runat="server" SkinID="EditPermissions" AlternateText="Edit Permissions"
							CommandName='edit_permissions' meta:resourcekey="cmdEditPermissions" CausesValidation="false"
							CommandArgument='<%# Eval("Name") %>'></asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="gvFilesSize">
				<ItemStyle Wrap="False" Width="65px"></ItemStyle>
				<ItemTemplate>
					<span id="Span1" runat=server visible='<%# !IsFolder(Container.DataItem) %>' title='<%# Eval("Size") %>'>
						<%# GetFileSize((long)Eval("Size")) %>
					</span>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="Changed" HeaderText="gvFilesModified">
				<ItemStyle Wrap="False" Width="135px" ></ItemStyle>
			</asp:BoundField>
		</Columns>
	</asp:GridView>
	<asp:Literal ID="litPath" runat="server" Visible="false" Text="\"></asp:Literal>
	<asp:ObjectDataSource ID="odsFilesPaged" runat="server"
		SelectMethod="GetFiles" TypeName="SolidCP.Portal.FilesHelper" MaximumRowsParameterName="" StartRowIndexParameterName="" OnSelected="odsFilesPaged_Selected" 
		OnSelecting="odsFilesPaged_Selecting">
		<SelectParameters>
			<asp:ControlParameter ControlID="litPath" Name="path" PropertyName="Text" />
		</SelectParameters>
	</asp:ObjectDataSource>
</div>

<asp:Panel ID="EditFilePanel" runat="server" CssClass="PopupContainer fileeditor" style="display:none" DefaultButton="btnSaveEditFile">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-pencil"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblEditFile" Text="Edit File" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblFileEncoding" runat="server" meta:resourcekey="lblFileEncoding" Text="Encoding:"></asp:Label><br />
				<asp:DropDownList ID="ddlFileEncodings" runat="server" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlFileEncodings_SelectedIndexChanged"></asp:DropDownList>
			</div>
			<div class="FormRow">
				<asp:Label ID="lblFileContent" runat="server" meta:resourcekey="lblFileContent" Text="File Content:"></asp:Label>
				<asp:TextBox ID="txtEditFileContent" runat="server" Rows="10" TextMode="MultiLine" Width="100%" Wrap="False"></asp:TextBox>
			</div>
            </div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelEditFile" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancelEditFile_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnSaveEditFile" CssClass="btn btn-success" runat="server" OnClick="btnSaveEditFile_Click" CausesValidation="false"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveEditFileText"/> </CPCC:StyleButton> 
		</div>
	</div>
</asp:Panel>
<asp:Button ID="btnEditFile" runat="server" style="display:none;" />
<ajaxToolkit:ModalPopupExtender ID="EditFileModal" runat="server"
    TargetControlID="btnEditFile" PopupControlID="EditFilePanel"
    BackgroundCssClass="modalBackground" DropShadow="false" />
    
    
<asp:Panel ID="RenameFilePanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="btnRename">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-pencil"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblRenameFileFolder" Text="Rename File" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="lblNewName" runat="server" meta:resourcekey="lblNewName" Text="New Name:"></asp:Label>
				<uc2:FileNameControl ID="txtRenameFile" runat="server" ValidationGroup="RenameFile" Width="400px" />
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelRename" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancelRename_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnRename" CssClass="btn btn-success" runat="server" OnClick="btnRename_Click" ValidationGroup="RenameFile"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRenameText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>
<asp:Button ID="btnRenameFile" runat="server" style="display:none;" />
<ajaxToolkit:ModalPopupExtender ID="RenameFileModal" runat="server"
    TargetControlID="btnRenameFile" PopupControlID="RenameFilePanel"
    BackgroundCssClass="modalBackground" DropShadow="false" />
    
    
    
<asp:Panel ID="PermissionsFilePanel" runat="server" CssClass="PopupContainer" style="display:none">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-users"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblPermissions" Text="File/Folder Permissions" /></h3>
        </div>
        <div class="widget-content Popup">
            <div style="border-top: solid 1px #e0e0e0;width:380px; height: 175px; overflow: auto; white-space: nowrap;">
                <asp:GridView id="gvFilePermissions" runat="server" AutoGenerateColumns="False"
                        CssSelectorClass="NormalGridView" ShowHeader="false">
                    <Columns>
			            <asp:TemplateField>
				            <ItemTemplate>
					            <b><asp:Literal id="litDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Literal></b>
					            <asp:Literal id="litAccountName" runat="server" Text='<%# Eval("AccountName") %>' visible="false"></asp:Literal>
				            </ItemTemplate>
			            </asp:TemplateField>
			            <asp:TemplateField>
				            <ItemTemplate>
					            <asp:CheckBox ID="chkRead" Runat="server" Checked='<%# Eval("Read") %>' Text="Read" meta:resourcekey="chkRead"></asp:CheckBox>
				            </ItemTemplate>
			            </asp:TemplateField>
			            <asp:TemplateField>
				            <ItemTemplate>
					            <asp:CheckBox ID="chkWrite" Runat="server" Checked='<%# Eval("Write") %>' Text="Write" meta:resourcekey="chkWrite"></asp:CheckBox>
				            </ItemTemplate>
			            </asp:TemplateField>
			         </Columns>
                </asp:GridView>
            </div>
			<div class="FormRow">
				<asp:CheckBox ID="chkReplaceChildPermissions" Runat="server" Text="Replace permissions on all child objects" meta:resourcekey="chkReplaceChildPermissions"></asp:CheckBox>
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelPermissions" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancelPermissions_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelPermissionsText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnSetPermissions" CssClass="btn btn-success" runat="server" OnClick="btnSetPermissions_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSetPermissionsText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>
<asp:LinkButton ID="btnSetPermissionsFile" runat="server" style="display:none;" />
<ajaxToolkit:ModalPopupExtender ID="PermissionsFileModal" runat="server"
    TargetControlID="btnSetPermissionsFile" PopupControlID="PermissionsFilePanel"
    BackgroundCssClass="modalBackground" DropShadow="false" />

	</ContentTemplate>
</asp:UpdatePanel>

<div class="GridFooter">
	<div class="Left">
		<asp:Label ID="lblDiskSpace" runat="server" meta:resourcekey="lblDiskSpace" Text="Disk Space, MB:"></asp:Label>
		<uc4:Quota ID="Quota1" runat="server" QuotaName="OS.Diskspace" />
	</div>
	<div class="Right">
		<asp:LinkButton ID="btnRecalcDiskspace" runat="server" meta:resourcekey="btnRecalcDiskspace"
						CssClass="Button3"
						Text="Calculate Diskspace" OnClientClick="return confirm('Proceed?');" OnClick="btnRecalcDiskspace_Click" />
	</div>
</div>