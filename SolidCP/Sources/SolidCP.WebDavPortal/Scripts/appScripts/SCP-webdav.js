//Toggle file select + Ctrl multiselect
$(document).on('click', '.element-container', function (e) {
    if (e.ctrlKey) {
        $(this).toggleClass("selected-file");
    } else {

        scp.fileBrowser.clearAllSelectedItems();

        scp.fileBrowser.selectItem(this);
    }

    scp.fileBrowser.refreshDeletionBlock();
});

$(document).on('touchstart', '.element-container', function (e) {
    var now = new Date().getTime();
    var lastTouch = $(this).data('lastTouch') || now + 1;
    var delta = now - lastTouch;

    if (delta < 300 && delta > 0) {
        scp.fileBrowser.openItem(this);
        $(this).data('lastTouch', 0);
    }

    $(this).data('lastTouch', now);
});

//Double click file open
$(document).on('dblclick', '.element-container', function (e) {
    scp.fileBrowser.openItem(this);

    var links = $(this).find('.file-link');

    if (links.length != 0 && $(links[0]).hasClass('processing-dialog')) {
        scp.dialogs.showProcessDialog();
    }
});


//Delete button click
$(document).on('click', '.file-deletion #delete-button', function (e) {
    var dialogId = $(this).data('target');
    var buttonText = $(this).data('target-positive-button-text');
    var content = $(this).data('target-content');
    var title = $(this).data('target-title-text');

    content = jQuery.validator.format(content, scp.fileBrowser.getSelectedItemsCount());

    scp.dialogs.showConfirmDialog(title, content, buttonText, scp.fileBrowser.deleteSelectedItems, dialogId);
});


$(document).click(function (event) {
    if (!$(event.target).closest('.element-container, .prevent-deselect').length) {
        scp.fileBrowser.clearAllSelectedItems();
        scp.fileBrowser.refreshDeletionBlock();
    }
});

$('#drag-and-drop-area').click(function (e) {
    $('#file-input').click();
});

$('#drag-and-drop-area #file-input').click(function (e) {
    e.stopPropagation();
});



$("#create-button").click(function (e) {

    if ($('#filenameForm').valid()) {

        var fileName = $('#createNewItemDialog #filename').val() + $(this).data('extension');

        $(this).attr('href', $(this).data('href') + '/' + fileName);

        $(this).attr('target', $(this).data('target'));

        scp.fileBrowser.hideCreateNewItemDialog();
        //;
    } else {
        e.preventDefault();
    }
});

$(document).ready(function () {

    $('#filenameForm').validate({
        onkeyup: false,
        onclick: false,
        async: false,
        rules: {
            filename: {
                required: true,
                synchronousRemote: scp.fileBrowser.uniqueFileNameFieldRule("#filename")
            }
        },
        messages: {
            filename: {
                synchronousRemote: scp.fileBrowser.settings.textItemExist
            }
        }
    });

});

$('#filename').keydown(function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        return false;
    }
    return true;
});


$(".create-new-item li a").click(function () {

    $("#filenameForm").clearValidation();

    scp.fileBrowser.showCreateNewItemDialog($(this).data('extension'), $(this).data('target'),  $(this).text());

    $("#filename").focus();
});
