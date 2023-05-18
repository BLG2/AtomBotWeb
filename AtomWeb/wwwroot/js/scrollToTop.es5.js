'use strict';

$(document).ready(function () {

    // display back to top button if needed
    $(window).scroll(function () {
        CalculateScrollToTopButton();
        //checl when scrolling stopped
        clearTimeout($.data(this, 'scrollTimer'));
        $.data(this, 'scrollTimer', setTimeout(function () {
            CalculateScrollToTopButton();
        }, 250));
    });

    // back to top button click funtionality
    $("#scrollToTopButton").on('click', function () {
        $(window).scrollTop(0);
    });
});

function CalculateScrollToTopButton() {
    var height = $(window).scrollTop();
    if (height > 100 && $(".scrollToTopBox").css('display') == 'none') {
        if ($(".scrollToTopBox").css('display') == 'none') $(".scrollToTopBox").fadeIn();
    } else if (height < 100 && $(".scrollToTopBox").css('display') != 'none') {
        $(".scrollToTopBox").fadeOut();
    }
}

