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

        $("#PolicyPriceTrackingOtherGroupItemDataTableItemErrorMessage").text("");
        
        $('.PolicyPriceTrackingOtherGroupItemDataTableItem').each(function() {
        	if ($(this).val().length != 0) {
        		validItem = true;
        	}
        });
        
        if (!validItem) {
        	$("#PolicyPriceTrackingOtherGroupItemDataTableItemErrorMessage").text("At least one field must be completed.");
        	return false;
        }

        return validItem;

    });
});
