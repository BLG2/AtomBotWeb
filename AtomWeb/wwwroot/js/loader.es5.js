"use strict";

$(document).ready(function () {
    console.log('ready');
    $("#loading").fadeOut(500);
    //freezeScreen();
});

$(".showLoadingScreen, .btn").not($(".noLoadingScreenButton")).click(function () {
    $("#loading").fadeIn(500);
    //freezeScreen();
});

function FadeOutAfther1Min() {
    setTimeout(function () {
        if ($('#loading').is(":visible")) $("#loading").fadeOut(500);
        //freezeScreen();
    }, 60000);
}

