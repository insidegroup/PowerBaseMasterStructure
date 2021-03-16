/*
FOR non-CWT Credit Cards
ClientSubUnitTravlerType
TravlerType
ClientTopUnit

*/

$(document).ready(function() {

    //Navigation
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#CreditCard_CreditCardValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    //Show DatePickers
    $('#CreditCard_CreditCardValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#CreditCard_CreditCardValidFrom').val() == "") {
        $('#CreditCard_CreditCardValidFrom').val("No Valid From Date");
    }


});

/////////////////////////////////////////////////////////
// BEGIN PRODUCT/SUPPLIER SETUP
/////////////////////////////////////////////////////////
if ($("#CreditCard_ProductId").val() == "") {
	$("#CreditCard_SupplierName").attr("disabled", true);
} else {
	$("#CreditCard_SupplierName").removeAttr("disabled");
}

$("#CreditCard_ProductId").change(function () {
	$("#CreditCard_SupplierName").val("");
	$("#lblAuto").text("");
	$("#PolicyRouting_Name").val("");
	$("#lblPolicyRoutingNameMsg").text("");

	if ($("#CreditCard_ProductId").val() == "") {
		$("#CreditCard_SupplierName").attr("disabled", true);
	} else {
		$("#CreditCard_SupplierName").removeAttr("disabled");
	}
});

$("#CreditCard_SupplierName").change(function () {
	if ($("#CreditCard_SupplierName").val() == "") {
		$("#CreditCard_SupplierCode").val("");
		$("#CreditCard_SupplierCode").val("");
	}
});
/////////////////////////////////////////////////////////
// END PRODUCT/SUPPLIER SETUP
/////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////
// BEGIN PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
/////////////////////////////////////////////////////////
$(function () {
	$("#CreditCard_SupplierName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
				data: { searchText: request.term, productId: $("#CreditCard_ProductId").val() },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							id: item.SupplierCode,
							value: item.SupplierName
						}
					}))
				}
			})
		},
		mustMatch: true,
		select: function (event, ui) {
			$("#CreditCard_SupplierName").val(ui.item.value);
			$("#CreditCard_SupplierCode").val(ui.item.id);
			$("#lblSupplierNameMsg").text("");
		}
	});

});
/////////////////////////////////////////////////////////
// END PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
/////////////////////////////////////////////////////////

$('#form0').submit(function () {

	/////////////////////////////////////////////////////////
	// BEGIN PRODUCT/SUPPLIER VALIDATION
	// 1. Check Text for Supplier to see if valid
	// 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
	/////////////////////////////////////////////////////////
	var validSupplier = false;
	var validSupplierProduct = false;

	if ($("#CreditCard_SupplierName").val() || $("#CreditCard_ProductId").val() != "") {
		if ($("#CreditCard_SupplierName").val() != "") {
			jQuery.ajax({
				type: "POST",
				url: "/Validation.mvc/IsValidSupplierName",
				data: { supplierName: $("#CreditCard_SupplierName").val() },
				success: function (data) {

					if (!jQuery.isEmptyObject(data)) {
						validSupplier = true;

						//user may not use auto, need to populate ID field
						$("#CreditCard_SupplierCode").val(data[0].SupplierCode);
					}
				},
				dataType: "json",
				async: false
			});

			if (!validSupplier) {
				$("#SupplierCode").val("");
				$("#lblSupplierNameMsg").removeClass('field-validation-valid');
				$("#lblSupplierNameMsg").addClass('field-validation-error');
				$("#lblSupplierNameMsg").text("This is not a valid Supplier");
				return false;
			} else {
				$("#lblSupplierNameMsg").text("");
			}

			jQuery.ajax({
				type: "POST",
				url: "/Validation.mvc/IsValidSupplierProduct",
				data: { supplierCode: $("#CreditCard_SupplierCode").val(), productId: $("#CreditCard_ProductId").val() },
				success: function (data) {

					if (!jQuery.isEmptyObject(data)) {
						validSupplierProduct = true;
					}
				},
				dataType: "json",
				async: false
			});
			if (!validSupplierProduct) {
				$("#lblSupplierNameMsg").removeClass('field-validation-valid');
				$("#lblSupplierNameMsg").addClass('field-validation-error');
				$("#lblSupplierNameMsg").text("This is not a valid Supplier");
				return false;
			} else {
				$("#lblSupplierNameMsg").text("");
			}
		}
	}

	/////////////////////////////////////////////////////////
	// END PRODUCT/SUPPLIER VALIDATION
	/////////////////////////////////////////////////////////

    if ($('#CreditCard_CreditCardValidFrom').val() == "No Valid From Date") {
        $('#CreditCard_CreditCardValidFrom').val("");
    }

});

