/*
This script is not strictly sequencing
It is to alllow drag and drop between 2 lists when moving users between Teams
*/
/*OnReady*/
$(document).ready(function() {
    $('#menu_teams').click();
})

$(function() {

$("#sortable tbody.content").sortable({
connectWith: "#sortable2 tbody.content", dropOnEmpty: true,
items: "tr:not(.titlerow)",
    update: function() {
        $("#sortable tr").removeClass("row_odd2");
        $("#sortable tr").removeClass("row_even");
        $("#sortable tr:odd").addClass("row_odd2");
        $("#sortable tr:even").addClass("row_even");
    }
});
$("#sortable tbody.content").disableSelection();

$("#sortable2 tbody.content").sortable({
connectWith: "#sortable tbody.content", dropOnEmpty: true,
items: "tr:not(.titlerow)",
update: function() {
        $("#sortable2 tr").removeClass("row_odd2");
        $("#sortable2 tr").removeClass("row_even");
        $("#sortable2 tr:odd").addClass("row_odd2");
        $("#sortable2 tr:even").addClass("row_even");
    }
});
$("#sortable tbody.content, #sortable2 tbody.content").disableSelection();
    
    $("#form0").submit(function() {
        $("#SystemUsers").val($("#sortable2 tbody.content").sortable('toArray'))
    })
});
