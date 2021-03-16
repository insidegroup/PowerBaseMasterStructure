/*OnReady*/
$(document).ready(function() {
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$('#breadcrumb').css('width', 'auto');
	$('#search').hide();
	$('#search_wrapper').css('height', '23px');
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
    $("#SequenceOriginal").val($("#sortable tbody.content").sortable('toArray'))
    $("#form0").submit(function() {
        $("#Sequence").val($("#sortable tbody.content").sortable('toArray'))
    })
});

function checkSaved() {
    $("#Sequence").val($("#sortable tbody.content").sortable('toArray'))
    if (document.forms["form0"].Sequence.value == document.forms["form0"].SequenceOriginal.value) {
        return true;
    }else{
        return confirm("Click OK to continue to the next page without saving");
    }
}
