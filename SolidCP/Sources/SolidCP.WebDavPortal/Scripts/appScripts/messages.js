function ScpMessager(messageDivId) {
    this.settings = {
        messageDivId: messageDivId,
        successClass: "alert-success",
        infoClass: "alert-info",
        warningClass: "alert-warning",
        dangerClass: "alert-danger",
        messageDivtemplate: '<div class="alert {0} alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>{1}</div>'
    };
}

ScpMessager.prototype = {
    addMessage: function(cssClass, message) {
        var messageDiv = jQuery.validator.format(this.settings.messageDivtemplate, cssClass, message);

        $(messageDiv).appendTo(this.settings.messageDivId);
    },

    addSuccessMessage: function(message) {
        this.addMessage(this.settings.successClass, message);
    },

    addInfoMessage: function(message) {
        this.addMessage(this.settings.infoClass, message);
    },

    addWarningMessage : function (message) {
        this.addMessage(this.settings.warningClass, message);
    },

    addErrorMessage: function (message) {
        this.addMessage(this.settings.dangerClass, message);
    },

    showMessages: function (messages) {
        var objthis = this;

        $.each(messages, function(i, message) {
                
                if ((message.Type == 0)) {
                    objthis.addSuccessMessage(message.Value);
                }
                else if (message.Type == 1) {
                    objthis.addInfoMessage(message.Value);
                }
                else if (message.Type == 2) {
                    objthis.addWarningMessage(message.Value);
                }
                else if (message.Type == 3) {
                    objthis.addErrorMessage(message.Value);
                }
            }
        );
    }
};
