/*
OnReady
*/
$(document).ready(function() {

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$("#breadcrumb").css("width", "auto");

	$('#PolicyOtherGroupHeader_SubProductId').attr('disabled', true);
	$('#PolicyOtherGroupHeader_LabelLanguageCode').attr('disabled', true);
	$('#PolicyOtherGroupHeader_TableNameLanguageCode').attr('disabled', true);
	$('#PolicyOtherGroupHeader_TableName').attr('disabled', true);
	$('#PolicyOtherGroupHeader_SubProductIdRequired').hide();
	$('#PolicyOtherGroupHeader_TableNameRequired').hide();

	//Product Name

	var productName = $('#PolicyOtherGroupHeader_ProductId option:selected').text();
	if (productName == 'Other') {
		$('#PolicyOtherGroupHeader_SubProductId').attr('disabled', false);
		$('#PolicyOtherGroupHeader_SubProductIdRequired').show();
	}

	$('#PolicyOtherGroupHeader_ProductId').change(function () {
		var selectedProductName = $('#PolicyOtherGroupHeader_ProductId option:selected').text();
		if (selectedProductName == 'Other') {
			$('#PolicyOtherGroupHeader_SubProductId').attr('disabled', false);
			$('#PolicyOtherGroupHeader_SubProductIdRequired').show();
		} else {
			$('#PolicyOtherGroupHeader_SubProductId').find('option:selected').removeAttr("selected");
			$('#PolicyOtherGroupHeader_SubProductId').val('');
			$('#PolicyOtherGroupHeader_SubProductId').attr('disabled', true);
			$('#PolicyOtherGroupHeader_SubProductIdRequired').hide();
		}
	});

	//Table Name

	if ($('#PolicyOtherGroupHeader_TableDefinitionsAttachedFlag').is(':checked')) {
		$('#PolicyOtherGroupHeader_TableName').attr('disabled', false);
		$('#PolicyOtherGroupHeader_TableNameRequired').show();
	}

	$('#PolicyOtherGroupHeader_TableDefinitionsAttachedFlag').change(function () {
		if ($(this).is(':checked')) {
			$('#PolicyOtherGroupHeader_TableName').attr('disabled', false);
			$('#PolicyOtherGroupHeader_TableNameRequired').show();
		} else {
			$('#PolicyOtherGroupHeader_TableName').attr('disabled', true).val('');
			$('#PolicyOtherGroupHeader_TableNameRequired').hide();
		}
	});

    /* Display Top / Display Bottom */

    $('#PolicyOtherGroupHeader_DisplayTopFlag').change(function () {
        if ($(this).is(':checked')) {
            $('#PolicyOtherGroupHeader_DisplayBottomFlag').attr('checked', false);
        }
    });

    $('#PolicyOtherGroupHeader_DisplayBottomFlag').change(function () {
        if ($(this).is(':checked')) {
            $('#PolicyOtherGroupHeader_DisplayTopFlag').attr('checked', false);
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

        $("#PolicyOtherGroupHeader_SubProductIdError").text("");
        if ($('#PolicyOtherGroupHeader_ProductId option:selected').text() == 'Other' && $('#PolicyOtherGroupHeader_SubProductId').val() == "") {
        	$("#PolicyOtherGroupHeader_SubProductIdError").text("The Sub Product is required if Product is Other.");
        	return false;
        }

    	/*
		If Table Flag checked, then Table name required
        */

        $("#PolicyOtherGroupHeader_TableNameError").text("");
        if ($('#PolicyOtherGroupHeader_TableDefinitionsAttachedFlag').is(':checked') && $('#PolicyOtherGroupHeader_TableName').val() == "") {
        	$("#PolicyOtherGroupHeader_TableNameError").text("The Table Name is required if Table? is checked.");
        	return false;
        }

        return validItem;

    });
});
