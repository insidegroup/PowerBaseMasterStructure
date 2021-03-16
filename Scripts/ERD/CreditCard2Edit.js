/*
FOR non-CWT Credit Cards:
CreditCardClientSubUnit
CreditCardClientAccount
*/


$(document).ready(function() {

    //Navigation
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#CreditCardValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    //Show DatePickers
    $('#CreditCardValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#CreditCardValidFrom').val() == "") {
        $('#CreditCardValidFrom').val("No Valid From Date");
    }


});

//uses jquery.validate
$(document).ready(function () {
	$.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            if ($('#CreditCardNumber').val() != $('#OriginalCreditCardNumber').val()) {
                var re = new RegExp(regexp);
                return this.optional(element) || re.test(value.replace(/(^\s*)|(\s*$)/g, ""));
            } else {
                return true
            }
        },
        "Please enter a valid CreditCardNumber."
    );

	$("#form0").validate({
		errorPlacement: function (error, element) {
			$("#CreditCardNumber_validationMessage").addClass('field-validation-error');
			error.insertAfter($('#CreditCardNumber_validationMessage'));
		}
	});

	if ($("#CreditCardNumber").length > 0) {
		$("#CreditCardNumber").rules("add", { regex: "^(\\d{13,16})$" });
	}
});

/////////////////////////////////////////////////////////
// BEGIN PRODUCT/SUPPLIER SETUP
/////////////////////////////////////////////////////////
if ($("#ProductId").val() == "") {
	$("#SupplierName").attr("disabled", true);
} else {
	$("#SupplierName").removeAttr("disabled");
}

$("#ProductId").change(function () {
	$("#SupplierName").val("");
	$("#lblAuto").text("");
	$("#PolicyRouting_Name").val("");
	$("#lblPolicyRoutingNameMsg").text("");

	if ($("#ProductId").val() == "") {
		$("#SupplierName").attr("disabled", true);
	} else {
		$("#SupplierName").removeAttr("disabled");
	}
});

$("#SupplierName").change(function () {
	if ($("#SupplierName").val() == "") {
		$("#SupplierCode").val("");
		$("#SupplierCode").val("");
	}
});
/////////////////////////////////////////////////////////
// END PRODUCT/SUPPLIER SETUP
/////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////
// BEGIN PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
/////////////////////////////////////////////////////////
$(function () {
	$("#SupplierName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
				data: { searchText: request.term, productId: $("#ProductId").val() },
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
			$("#SupplierName").val(ui.item.value);
			$("#SupplierCode").val(ui.item.id);
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

	if ($("#SupplierName").val() || $("#ProductId").val() != "") {
		if ($("#SupplierName").val() != "") {
			jQuery.ajax({
				type: "POST",
				url: "/Validation.mvc/IsValidSupplierName",
				data: { supplierName: $("#SupplierName").val() },
				success: function (data) {

					if (!jQuery.isEmptyObject(data)) {
						validSupplier = true;

						//user may not use auto, need to populate ID field
						$("#SupplierCode").val(data[0].SupplierCode);
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
				data: { supplierCode: $("#SupplierCode").val(), productId: $("#ProductId").val() },
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

    if ($('#CreditCardValidFrom').val() == "No Valid From Date") {
        $('#CreditCardValidFrom').val("");
    }
});
