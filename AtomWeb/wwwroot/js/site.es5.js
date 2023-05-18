
//this will wait for the text assets to be loaded before calling this (the dom.. css.. js)
'use strict';

$(document).ready(function () {
    // range output number above range input element
    $('.custom-range').on('change', function () {
        var val = $(this).val();
        $(this).attr('title', val);
        $(this).prev().html(val);
    });

    //popup
    var opoupVal = $("#customInput").data("value");
    if (opoupVal) {
        $(".popup").fadeOut().fadeIn().fadeOut(8000);
    }
    $(".popup").on('click', function () {
        $(".popup").fadeOut();
    });

    // subNavivations
    $(".subnav").on('click', function () {
        $(this).addClass("subNavOpen");
        $("#" + $(this).attr("data-category")).addClass("subContentShow");
        $(".subnav").not(this).removeClass("subNavOpen");
        $(".subContent").not("#" + $(this).attr("data-category")).removeClass("subContentShow");
    });

    // filter tru tables
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // filter tru lists
    $("#myListFilterInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myList li").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // command
    $(".show-more-btn").on('click', function () {
        $(this).next().slideToggle();
        $(".to-show").not($(this).next()).not($(this).parents()).slideUp();
        var id = $(this).data("id");
        $(this).toggleClass("open");
        $('.show-more-btn[data-id="' + id + '"]').not(this).removeClass("open");
    });

    //BreadCrumbs
    var bread = $(".breadcrumb");
    var showMoreread = $(".showMoreBreadCrumbs");
    if ($(".breadcrumb-item").length > 4) {
        showMoreread.attr("style", "display: flex !important");
    };
    showMoreread.click(function () {
        if (bread.hasClass('collapsed')) {
            bread.removeClass('collapsed');
        } else {
            bread.addClass('collapsed');
        }
    });

    /*Theme Swap*/
    $(".lightTheme").prop("checked", true);
    $("#themeSwap").on("change", function () {
        var theme = this.checked ? "light" : "dark";
        if (theme == "light") {
            $("#themeSwap").removeClass("darkTheme");
            $("#themeSwap").addClass("lightTheme");
        } else {
            $("#themeSwap").removeClass("lightTheme");
            $("#themeSwap").addClass("darkTheme");
        }
        $.ajax({
            type: 'POST',
            url: "/Home/ChangeTheme",
            dataType: "json",
            data: { "theme": theme },
            success: function success(response) {
                if (response.success == true) {
                    console.log("succes");
                    location.reload(true);
                }
            },
            error: function error(a, b, c) {}
        });
    });
});

$.fn.isInViewport = function () {
    var elementTop = $(this).offset().top;
    var elementBottom = elementTop + $(this).outerHeight();
    var viewportTop = $(window).scrollTop();
    var viewportBottom = viewportTop + $(window).height();
    return elementBottom > viewportTop && elementTop < viewportBottom;
};
$(window).on('ready resize scroll', function () {
    $('.box, .card ').each(function () {
        if ($(this).isInViewport()) {
            $(this).addClass('visible').removeClass('invisible');
        } else {
            $(this).removeClass('visible').addClass('invisible');
        }
    });
});

