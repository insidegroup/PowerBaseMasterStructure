$(document).ready(function() {
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

});

$(function () {
    $('#ClientDetailESCInformation_ESCInformation').NobleCount('#countBO', { on_negative: function () { $('#countBO').css('color', 'red') }, on_positive: function () { $('#countBO').css('color', 'black') }, max_chars: 255 });
});