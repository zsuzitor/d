;;;

var time_for_page_up;// for up()

///--------------------------------------------------------------------------------------

//метод для скроло страницы вверх
function up() {
    var top = Math.max(document.body.scrollTop, document.documentElement.scrollTop);
    if (top > 0) {
        window.scrollBy(0, -100);
        time_for_page_up = setTimeout('up()', 20);
    } else clearTimeout(time_for_page_up);
    return false;
}

//метод для проверки находится ли элемент в зоне экрана
function isVisible(tag) {
    var t = $(tag);
    var w = $(window);
    var top_window = w.scrollTop();
    var bot_window = top_window + document.documentElement.clientHeight;
    var top_tag = t.offset().top;
    var bot_tag = top_tag + t.height();
    return ((bot_tag >= top_window && bot_tag <= bot_window) || (top_tag >= top_window && top_tag <= bot_window) || (bot_tag >= bot_window && top_tag <= top_window));
}

//событие при скроле
$(function () {
    $(window).scroll(function () {
        Change_main_header();
    });
});


//метод изменения отображения header
function Change_main_header() {
    var b = $("#Main_header_check_small_or_big_header");
    if (!b.prop("shown") && !isVisible(b)) {
        b.prop("shown", true);
        var o = document.getElementById("Main_header_small_id")
        var o1 = document.getElementById("Main_header_back_to_top_id")
        o1.style.display = 'block';

        o.style.display = 'block';

    }
    else {
        if (b.prop("shown") && isVisible(b)) {
            b.prop("shown", false);
            var o = document.getElementById("Main_header_small_id")
            var o1 = document.getElementById("Main_header_back_to_top_id")
            o1.style.display = 'none';
            o.style.display = 'none';
        }
    }

}

;;;