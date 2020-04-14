// Write your JavaScript code.


var $scroll_div = $('#scroll_top_wrap');
$(document).scroll(function () {
    $scroll_div.css({ display: $(this).scrollTop() > 100 ? "block" : "none" });
});


function HideShowFilesInContainer() {
    if ($(".FolderFiles").is(":visible")){
        $(".FolderFiles").hide();
    } else {
        $(".FolderFiles").show();
    }
}

