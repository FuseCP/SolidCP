/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />


$.validator.unobtrusive.adapters.addSingleVal("minimumlength", "count");

$.validator.addMethod("minimumlength", function (value, element, count) {
    if (value.length < count) {
        return false;
    }

    return true;
});

$.validator.unobtrusive.adapters.addSingleVal("maximumlength", "count");

$.validator.addMethod("maximumlength", function (value, element, count) {
    if (value.length > count) {
        return false;
    }

    return true;
});

$.validator.unobtrusive.adapters.addSingleVal("uppercasecount", "count");

$.validator.addMethod("uppercasecount", function (value, element, count) {
    if (value.replace(/[^A-Z]/g, "").length < count) {
        return false;
    }

    return true;
});

$.validator.unobtrusive.adapters.addSingleVal("numberscount", "count");

$.validator.addMethod("numberscount", function (value, element, count) {
    if (value.replace(/[^0-9]/g, "").length < count) {
        return false;
    }

    return true;
});

$.validator.unobtrusive.adapters.addSingleVal("symbolscount", "count");

$.validator.addMethod("symbolscount", function (value, element, count) {
    if (value.replace(/[^~!@#$%^&*_\-+'\|\\(){}\[\]:;\"'<>,.?/]/g, "").length < count) {
        
        return false;
    }

    return true;
});