/*OnReady*/
$(document).ready(function() {
    $('#menu_reasoncodes').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$('#breadcrumb').css('width', '725px');
	$('#search').css('width', '5px');
})

$(function() {

    $("#sortable tbody.content").sortable({
	    update : function () {
	        $("#sortable tr").removeClass("row_odd");
            $("#sortable tr").removeClass("row_even");
	        $("#sortable tr:odd").addClass("row_odd");
            $("#sortable tr:even").addClass("row_even");
        }
    });
    $("#sortable tbody.content").disableSelection();
    $("#RCSequenceOriginal").val($("#sortable tbody.content").sortable('toArray'))
    $("#form0").submit(function() {
        $("#RCSequence").val($("#sortable tbody.content").sortable('toArray'))
    })
});

function checkSaved() {
    $("#RCSequence").val($("#sortable tbody.content").sortable('toArray'))
    if (document.forms["form0"].RCSequence.value == document.forms["form0"].RCSequenceOriginal.value) {
        return true;
    }else{
        return confirm("Click OK to continue to the next page without saving");
    }
}
