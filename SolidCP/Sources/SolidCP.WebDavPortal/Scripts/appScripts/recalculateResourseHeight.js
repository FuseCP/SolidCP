function recalculateResourseHeight() {
    $(document).ready(function () {
        var heights = $(".element-container").map(function () {
            return $(this).height();
        }).get(),

            maxHeight = Math.max.apply(null, heights);

        if (maxHeight < 135) {
            maxHeight = 135;
        }

        $(".element-container").height(maxHeight);
    });
}
