$(document).ready(function () {
    $(window).load(function () {
        GetResources();
    });
    $(window).scroll(function () {
        if (($(window).scrollTop() + 1) >= ($(document).height() - $(window).height())) {
            GetResources();
        };
    });
});

var oldResourcesDivHeight = $('#resourcesDiv').height();

function GetResources() {
    $.ajax({
        type: 'POST',
        url: '/storage/show-additional-content',
        data: { path: window.location.pathname, resourseRenderCount: $(".element-container").length },
        dataType: "html",
        success: function (result) {
            var domElement = $(result);
            $('#resourcesDiv').append(domElement);
            if ($(document).height() == $(window).height() && oldResourcesDivHeight != $('#resourcesDiv').height()) {
                GetResources();
                oldResourcesDivHeight = $('#resourcesDiv').height();
            };

            recalculateResourseHeight();
        }
    });
};
