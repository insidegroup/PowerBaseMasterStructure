$(document).ready(function () {
	$('#menu_admin').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
});

//Validate SupplierCode and SupplierName
$('#form0').submit(function () {

	var validSupplierCode = false;
	var validSupplierProduct = false;

	$("#lblSupplierCodeMsg").hide();
	$("#lblSupplierNameMsg").hide();

	//Validate Supplier Code
    //Only validate on Create - Edit is hidden field
	if ($('input[type="text"][name="SupplierCode"]').length > 0 && $('input[type="text"][name="SupplierCode"]').val() != "") {
		jQuery.ajax({
			type: "POST",
            url: "/Validation.mvc/IsValidSupplierProduct",
            data: { supplierCode: $("#SupplierCode").val(), productId: $("#ProductId").val() },
			success: function (data) {
				if (jQuery.isEmptyObject(data)) {
					validSupplierCode = true;
				}
			},
			dataType: "json",
			async: false
		});

		if (!validSupplierCode) {
			$("#lblSupplierCodeMsg").removeClass('field-validation-valid');
			$("#lblSupplierCodeMsg").addClass('field-validation-error');
			$("#lblSupplierCodeMsg").text("A Supplier already exists for this code. Please choose a different Supplier Code.");
			$("#lblSupplierCodeMsg").show();
			return false;
		}
	} else {
		validSupplierCode = true;
	}

	//Validate Supplier Product / Name
    //Only validate on Create - Edit is hidden field
    if ($('input[type="text"][name="SupplierCode"]').length > 0 && $('input[type="text"][name="SupplierCode"]').val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierProductName",
            data: {
                supplierName: $("#SupplierName").val(), productId: $("#ProductId").val()
            },
            success: function (data) {
                console.log(data);
                if (jQuery.isEmptyObject(data)) {
                    validSupplierProduct = true;
                }
            },
            dataType: "json",
            async: false
        });
    } else {
        validSupplierProduct = true;
    }

	if (!validSupplierProduct) {
		$("#lblSupplierNameMsg").removeClass('field-validation-valid');
		$("#lblSupplierNameMsg").addClass('field-validation-error');
		$("#lblSupplierNameMsg").text("The Supplier Name already exists. Please choose a different Supplier Name.");
		$("#lblSupplierNameMsg").show();
		return false;
	}

	if (validSupplierCode && validSupplierProduct) {
		return true;
	} else {
		return false
	};
	
});