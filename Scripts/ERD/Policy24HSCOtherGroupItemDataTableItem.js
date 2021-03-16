/*
OnReady
*/
$(document).ready(function() {

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$("#breadcrumb").css("width", "auto");

	/*
    Submit Form Validation
    */
    $("form").submit(function() {
    
        var validItem = false;

    	/*
		At least one item must be completed to add
		*/

        $("#Policy24HSCOtherGroupItemDataTableItemErrorMessage").text("");
        
        $('.Policy24HSCOtherGroupItemDataTableItem').each(function() {
        	if ($(this).val().length != 0) {
        		validItem = true;
        	}
        });
        
        if (!validItem) {
        	$("#Policy24HSCOtherGroupItemDataTableItemErrorMessage").text("At least one field must be completed.");
        	return false;
        }

        return validItem;

    });
});
