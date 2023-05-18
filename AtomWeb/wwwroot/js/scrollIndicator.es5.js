
//this will wait for the text assets to be loaded before calling this (the dom.. css.. js)
"use strict";

$(document).ready(function () {
    //set scroll indicator on page load
    SetScrollProgress();

    // recalculate scrollindicator for subNavivations since it can change the window height
    $(".subnav").on('click', function () {
        SetScrollProgress();
    });
});

// this wil trigger on page scroll
$(document).on("scroll", function () {
    // scroll indicator
    SetScrollProgress();
});

function SetScrollProgress() {
    var pixels = $(document).scrollTop() || 0;
    var docHeight = $(document).height() || 0;
    var windowHeight = $(window).height() || 0;
    var pageHeight = docHeight - windowHeight;
    var progress = 100 * pixels / pageHeight;
    if (isNaN(progress)) progress = 100;
    $("div.progress").css("width", progress + "%");
}

