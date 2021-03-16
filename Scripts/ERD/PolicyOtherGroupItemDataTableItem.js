/*
OnReady
*/
$(document).ready(function() {
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$("#breadcrumb").css("width", "auto");
	$('#search_wrapper').css('height', '24px');

	/*
    Submit Form Validation
    */
	$("form").submit(function () {

		var validItem = false;

		//At least one item must be filled in
		$("#PolicyOtherGroupItemDataTableItemErrorMessage").text("");
		$('.PolicyOtherGroupItemDataTableItem').each(function () {
			if($(this).val() != '') {
				validItem = true;
			}
		});
		
		if (!validItem) {
			$("#PolicyOtherGroupItemDataTableItemErrorMessage").text("At least one field is required.");
			return false;
		}

		return validItem;

	});

});
