function ScpFileBrowser() {
    this.settings = {
        deletionBlockSelector: ".file-actions-menu .file-deletion",
        deletionUrl: "storage/files-group-action/delete",
        fileExistUrl: "storage/fileExist",
        textDateModified: "Date modified",
        textSize: "Size",
        textItemExist: "File already exists",
        textItemExistFunc: function() {
            return textItemExist;
        } ,
        createNewItemDialogId: "#createNewItemDialog",
        createNewItemButtonId: "#create-button",
        createNewItemTitleId: '#create-dalog-label',
        processingDialogDom: '<div><img src="/Content/Images/indicator_medium.gif"><h4 class="dialog-text">Please wait...</h4></div>'
    };
    this.itemsTable = null;
    this.searchTable = null;
}

ScpFileBrowser.prototype = {
    setSettings: function(options) {
        this.settings = $.extend(this.settings, options);
    },

    clearAllSelectedItems: function() {
        $('.element-container').removeClass("selected-file");
    },

    selectItem: function(item) {
        $(item).addClass("selected-file");
    },

    openItem: function(item) {
        var links = $(item).find('.file-link');

        if (links.length != 0) {
            links[0].click();
        }
    },

    getSelectedItemsCount: function() {
        return $('.element-container.selected-file').length;
    },

    getSelectedItemsPaths: function() {
        return $('.element-container.selected-file a').map(function() {
            return $(this).attr('href');
        }).get();
    },

    deleteSelectedItems: function (e) {

        $.ajax({
            type: 'POST',
            url: scp.fileBrowser.settings.deletionUrl,
            data: { filePathes: scp.fileBrowser.getSelectedItemsPaths() },
            dataType: "json",
            success: function(model) {
                scp.messages.showMessages(model.Messages);

                scp.fileBrowser.clearDeletedItems(model.DeletedFiles);
                scp.fileBrowser.refreshDeletionBlock();
                scp.fileBrowser.refreshDataTable(scp.fileBrowser.itemsTable);

                scp.dialogs.hideProcessDialog();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                scp.messages.addErrorMessage(errorThrown);

                scp.fileBrowser.refreshDeletionBlock();
                scp.fileBrowser.refreshDataTable(scp.fileBrowser.itemsTable);

                scp.dialogs.hideProcessDialog();
            }
        });

        scp.dialogs.showProcessDialog();
    },

    clearDeletedItems: function(items) {
        $.each(items, function(i, item) {
            $('.element-container').has('a[href="' + item + '"]').remove();
        });
    },

    refreshDeletionBlock: function() {
        if (this.getSelectedItemsCount() > 0) {

            $(this.settings.deletionBlockSelector).css('display', 'inline-block');

        } else {
            $(this.settings.deletionBlockSelector).hide();
        }
    },

    initDataTable: function (tableId, ajaxUrl) {
        this.itemsTable = $(tableId).dataTable({
            "ajax": ajaxUrl,
            "processing": true,
            "serverSide": true,
            "dom": 'rtlp',
            "columnDefs": [
                {
                    "render": function(data, type, row) {
                        return '<div class="column-name"><img class="table-icon" src="' + row.IconHref + '"/>' +
                            '<a href="' + row.Url + '" ' + (row.IsTargetBlank ? 'target="_blank"' : '') + ' class="file-link" title="' + row.DisplayName + '">' +
                                    row.DisplayName +
                                '</a>' + (row.IsRoot ? '<span id="quota">' + scp.fileBrowser.bytesToSize(row.Size) + ' / ' + scp.fileBrowser.bytesToSize(row.Quota) + '</span>' : '')
                        +'</div>';
                    },
                    "targets": 0
                },
                {
                    "render": function (data, type, row) {
                        return row.Type;
                    },
                    "orderable": false,
                    "className": "center",
                    "width":"10%",
                    "targets": 1
                },
                {
                    "render": function (data, type, row) {
                        return row.LastModifiedFormated;
                    },
                    "width": "20%",
                    "className": "center",
                    "targets": 2
                }
            ],
            "createdRow": function(row, data, index) {
                $(row).addClass('element-container');
            },
            "oLanguage": {
                "sProcessing": this.settings.processingDialogDom
            }
        });

        $(tableId).removeClass('dataTable');

        //var oTable = this.table;

        //$(searchInputId).bind('keyup', function (e) {
        //    if (e.keyCode == 13) {
        //        oTable.fnFilter(this.value);
        //    }
        //});

        //$(searchInputId).keydown(function (event) {
        //    if (event.keyCode == 13) {
        //        event.preventDefault();
        //        return false;
        //    }

        //    return true;
        //});

    },

    initSearchDataTable: function (tableId, ajaxUrl, initSearch) {

        var settings = this.settings;
        var classThis = this;

        this.searchTable = $(tableId).dataTable({
            "ajax": ajaxUrl,
            "processing": true,
            "serverSide": true,
            "oSearch": { "sSearch": initSearch },
            "dom": 'rtlp',
            "columnDefs": [
                {
                    "render": function (data, type, row) {
                        return '<div class="column-name">' +
                                    '<img class="table-icon search" src="' + row.IconHref + '"/>' +
                                    '<div class="file-info">' +
                                        '<a href="' + row.Url + '" ' + (row.IsTargetBlank ? 'target="_blank"' : '') + ' class="file-link" title="' + row.DisplayName + '">' +
                                            row.DisplayName +
                                        '</a>' +
                                        '<div id="summary" class="summary">' + (row.Summary ? (row.Summary + '').substring(0, 500) + '...' : '') + '</div>' +
                                        '<div>' +
                                            '<a href="' + row.FolderUrlLocalString + '" ' + 'target="_blank" class="file-link" >' +
                                                  row.FolderUrlAbsoluteString +
                                            '</a>' +
                                        '</div>' +
                                    '</div>' +
                                '</div>';
                    },
                    "targets": 0
                },
                {
                    "render": function (data, type, row) {
                        return '<div>' +settings.textDateModified+': '+ row.LastModifiedFormated+ '</div>' +
                                '<div>' + settings.textSize + ': ' + classThis.bytesToSize(row.Size) + '</div>';
                    },
                    "orderable": false,
                    "width": "25%",
                    "targets": 1
                }
            ],
            "createdRow": function (row, data, index) {
                $(row).addClass('element-container');
            },
            "oLanguage": {
                "sProcessing": this.settings.processingDialogDom
            }
        });

        $(tableId).removeClass('dataTable');

    },

    refreshDataTable: function (table) {
        if (table != null) {
            table.fnDraw(false);
        }
    },

    initFileUpload: function (elementId, url) {
        $(document).ready(function () {

            $(elementId).fileupload({ url: url, autoUpload: true });

            $(elementId).fileupload('option', {
                disableImagePreview: true,
                sequentialUploads: true
            });
        });
    },

    initBigIcons: function (elementId, url) {
        $(document).ready(function () {
            $(window).load(function () {
                getResources();
            });
            $(window).scroll(function () {
                if (($(window).scrollTop() + 1) >= ($(document).height() - $(window).height())) {
                    getResources();
                };
            });
        });

        var oldResourcesDivHeight = $(elementId).height();

        function getResources() {
            $.ajax({
                type: 'POST',
                url: url,//'/storage/show-additional-content',
                data: { path: window.location.pathname, resourseRenderCount: $(".element-container").length },
                dataType: "html",
                success: function (result) {
                    var domElement = $(result);
                    $(elementId).append(domElement);
                    if ($(document).height() == $(window).height() && oldResourcesDivHeight != $('#resourcesDiv').height()) {
                        getResources();
                        oldResourcesDivHeight = $(elementId).height();
                    };

                    recalculateResourseHeight();
                }
            });
        };
    },

    bytesToSize: function(bytes) {
        if (bytes == 0) return '0 Byte';
        var k = 1024;
        var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
        var i = Math.floor(Math.log(bytes) / Math.log(k));
        return (bytes / Math.pow(k, i)).toPrecision(3) + ' ' + sizes[i];
    },

    showCreateNewItemDialog: function (extension, target, title) {
        $(this.settings.createNewItemButtonId).data('extension', extension);
        $(this.settings.createNewItemButtonId).data('target', target);

        $(this.settings.createNewItemDialogId + " input").val("");

        $(this.settings.createNewItemTitleId).text($(this.settings.createNewItemTitleId).data('title') + " " + title);

        $(this.settings.createNewItemDialogId).modal();
    },

    hideCreateNewItemDialog: function () {
        $(this.settings.createNewItemDialogId).modal('hide');
    },

    uniqueFileNameFieldRule: function(fieldId) {

        return {
            url: this.settings.fileExistUrl,
            type: "post",
            data: {
                newItemName: function() {
                    return $(fieldId).val() + $(scp.fileBrowser.settings.createNewItemButtonId).data('extension');
                } 
            },
            beforeSend: function(response) {
                scp.dialogs.showInlineProcessing(fieldId);
            },
            complete: function() {
                scp.dialogs.hideInlineProcessing(fieldId);
            }
        };
    }
};














