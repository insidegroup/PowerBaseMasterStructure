/*
OnReady
*/
$(document).ready(function() {

	$('#menu_admin').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$("#breadcrumb").css("width", "auto");

	$('#SubProductId').attr('disabled', true);
	$('#SubProductIdRequired').hide();

	//Product
	$('#ProductId').change(function () {
		var selectedProductName = $('#ProductId option:selected').text();
		if (selectedProductName == 'Other') {
			$('#SubProductId').attr('disabled', false);
			$('#SubProductIdRequired').show();
		} else {
			$('#SubProductId').find('option:selected').removeAttr("selected");
			$('#SubProductId').val('');
			$('#SubProductId').attr('disabled', true);
			$('#SubProductIdRequired').hide();
		}
	});

	/*
    Submit Form Validation
    */
    $("form").submit(function() {
    
        var validItem = true;

    	/*
		If Product is Other, Sub Product Required
		*/

        $("#SubProductIdError").text("");
        if ($('#ProductId option:selected').text() == 'Other' && $('#SubProductId').val() == "") {
        	$("#SubProductIdError").text("The Sub Product is required if Product is Other.");
        	return false;
        }

        return validItem;

    });
});
