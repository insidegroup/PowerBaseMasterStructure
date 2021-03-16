$(document).ready(function() {

	//Navigation
	$('#menu_fopadvicemessages').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$('#breadcrumb').css('width', 'auto');
	
	/////////////////////////////////////////////////////////
	// BEGIN PRODUCT/SUPPLIER SETUP
	/////////////////////////////////////////////////////////
	if ($("#FormOfPaymentAdviceMessageGroupItem_ProductId").val() == "" || $("#FormOfPaymentAdviceMessageGroupItem_ProductId").val() == "8") {
		$("#FormOfPaymentAdviceMessageGroupItem_SupplierName").attr("disabled", true);
	} else {
		$("#FormOfPaymentAdviceMessageGroupItem_SupplierName").removeAttr("disabled");
	}

	$("#FormOfPaymentAdviceMessageGroupItem_ProductId").change(function () {
		$("#FormOfPaymentAdviceMessageGroupItem_SupplierName").val("");
		$("#lblAuto").text("");
		$("#PolicyRouting_Name").val("");
		$("#lblPolicyRoutingNameMsg").text("");

		if ($("#FormOfPaymentAdviceMessageGroupItem_ProductId").val() == "") {
    		$("#FormOfPaymentAdviceMessageGroupItem_SupplierName").attr("disabled", true);
		} else {
    		$("#FormOfPaymentAdviceMessageGroupItem_SupplierName").removeAttr("disabled");
		}
	});

	$("#FormOfPaymentAdviceMessageGroupItem_SupplierName").change(function () {
		if ($("#FormOfPaymentAdviceMessageGroupItem_SupplierName").val() == "") {
    		$("#FormOfPaymentAdviceMessageGroupItem_SupplierCode").val("");
    		$("#SupplierCode").val("");
		}
	});
	/////////////////////////////////////////////////////////
	// END PRODUCT/SUPPLIER SETUP
	/////////////////////////////////////////////////////////

	$(function () {
		$("#FormOfPaymentAdviceMessageGroupItem_SupplierName").autocomplete({
    		source: function (request, response) {
    			$.ajax({
    				url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
    				data: { searchText: request.term, productId: $("#FormOfPaymentAdviceMessageGroupItem_ProductId").val() },
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
    			$("#FormOfPaymentAdviceMessageGroupItem_SupplierName").val(ui.item.value);
    			$("#FormOfPaymentAdviceMessageGroupItem_SupplierCode").val(ui.item.id);
    			$("#lblSupplierNameMsg").text("");
    		}
		});

	});
	/////////////////////////////////////////////////////////
	// END PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
	/////////////////////////////////////////////////////////


	$('#form0').submit(function () {

		var validEnabledDate = false;
		var validExpiryDate = false;
		var validSupplier = false;
		var validSupplierProduct = false;
		var validFormOfPaymentAdviceMessageGroupItemPercent = true;


		/////////////////////////////////////////////////////////
		// DATE VALIDATION
		/////////////////////////////////////////////////////////
		if ($('#FormOfPaymentAdviceMessageGroupItem_EnabledDate').val() == "No Enabled Date" || isValidDate($('#FormOfPaymentAdviceMessageGroupItem_EnabledDate').val())) {
    		validEnabledDate = true;
		} else {
    		$("#FormOfPaymentAdviceMessageGroupItem_EnabledDate_validationMessage").removeClass('field-validation-valid');
    		$("#FormOfPaymentAdviceMessageGroupItem_EnabledDate_validationMessage").addClass('field-validation-error');
    		$("#FormOfPaymentAdviceMessageGroupItem_EnabledDate_validationMessage").text("Date should be in format MMM dd yyyy. eg Dec 01 2014");

		}
		if ($('#FormOfPaymentAdviceMessageGroupItem_ExpiryDate').val() == "No Expiry Date" || isValidDate($('#FormOfPaymentAdviceMessageGroupItem_ExpiryDate').val())) {
    		validExpiryDate = true;
		} else {
    		$("#FormOfPaymentAdviceMessageGroupItem_ExpiryDate_validationMessage").removeClass('field-validation-valid');
    		$("#FormOfPaymentAdviceMessageGroupItem_ExpiryDate_validationMessage").addClass('field-validation-error');
    		$("#FormOfPaymentAdviceMessageGroupItem_ExpiryDate_validationMessage").text("Date should be in format MMM dd yyyy. eg Dec 01 2014");
		}

		/////////////////////////////////////////////////////////
		// BEGIN PRODUCT/SUPPLIER VALIDATION
		// 1. Check Text for Supplier to see if valid
		// 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
		/////////////////////////////////////////////////////////


		if ($("#FormOfPaymentAdviceMessageGroupItem_ProductId").val()) {

    		jQuery.ajax({
    			type: "POST",
    			url: "/Validation.mvc/IsValidSupplierName",
    			data: { supplierName: $("#FormOfPaymentAdviceMessageGroupItem_SupplierName").val() },
    			success: function (data) {

    				if (!jQuery.isEmptyObject(data)) {
    					validSupplier = true;

    					//user may not use auto, need to populate ID field
    					//$("#FormOfPaymentAdviceMessageGroupItem_SupplierCode").val(data[0].SupplierCode);
    				}
    			},
    			dataType: "json",
    			async: false
    		});

    		//issue where value of correct supplier code was overwritten by incorrect supplier code when 
    		//more than one supplier is returned when checking name (as product ID is different)
    		//force to use selection so supplier code is correctly set
    		if ($("#FormOfPaymentAdviceMessageGroupItem_SupplierCode").val() == "") {
    			$("#lblSupplierNameMsg").removeClass('field-validation-valid');
    			$("#lblSupplierNameMsg").addClass('field-validation-error');
    			$("#lblSupplierNameMsg").text("Please use typeahead box to select supplier");
    			return false;
    		}

    		if (!validSupplier) {
    			$("#FormOfPaymentAdviceMessageGroupItem_SupplierCode").val("");
    			$("#lblSupplierNameMsg").removeClass('field-validation-valid');
    			$("#lblSupplierNameMsg").addClass('field-validation-error');
    			$("#lblSupplierNameMsg").text("This is not a valid Supplier");
    		} else {
    			$("#lblSupplierNameMsg").text("");
    		}

    		if (validSupplier) {
    			jQuery.ajax({
    				type: "POST",
    				url: "/Validation.mvc/IsValidSupplierProduct",
    				data: { supplierCode: $("#FormOfPaymentAdviceMessageGroupItem_SupplierCode").val(), productId: $("#FormOfPaymentAdviceMessageGroupItem_ProductId").val() },
    				success: function (data) {

    					if (!jQuery.isEmptyObject(data)) {
    						validSupplierProduct = true;
    					}
    				},
    				dataType: "json",
    				async: false
    			});
    		}
    		if (!validSupplierProduct) {
    			$("#lblSupplierNameMsg").removeClass('field-validation-valid');
    			$("#lblSupplierNameMsg").addClass('field-validation-error');
    			$("#lblSupplierNameMsg").text("This is not a valid Supplier");
    			$("#FormOfPaymentAdviceMessageGroupItem_SupplierCode").val("");
    		} else {
    			$("#lblSupplierNameMsg").text("");
    		}
		} else {
    		validSupplier = true;
    		validSupplierProduct = true;
		}


		var mfPercent = $("#FormOfPaymentAdviceMessageGroupItem_FormOfPaymentAdviceMessageGroupItemPercent").val();
		var re = /^\d{1,2}(\.(\d){0,3}){0,1}$/;
		var isMatch = !!mfPercent.match(re);
		$("#FormOfPaymentAdviceMessageGroupItem_FormOfPaymentAdviceMessageGroupItemPercent_validationMessage").text("");

		if (isNaN(parseFloat(mfPercent)) || mfPercent >= 100 || mfPercent <= 0 || !isMatch) {
    		$("#FormOfPaymentAdviceMessageGroupItem_FormOfPaymentAdviceMessageGroupItemPercent_validationMessage").removeClass('field-validation-valid');
    		$("#FormOfPaymentAdviceMessageGroupItem_FormOfPaymentAdviceMessageGroupItemPercent_validationMessage").addClass('field-validation-error');
    		$("#FormOfPaymentAdviceMessageGroupItem_FormOfPaymentAdviceMessageGroupItemPercent_validationMessage").text("Merchant Fee Percent must be greater than zero, maximum value 99.999");
    		validFormOfPaymentAdviceMessageGroupItemPercent = false;
		};

		if (!validSupplierProduct || !validFormOfPaymentAdviceMessageGroupItemPercent || !validEnabledDate || !validExpiryDate) {
    		return false;
		}


		/////////////////////////////////////////////////////////
		// FORM IS VALID, CLEAR DATES AND SUBMIT
		/////////////////////////////////////////////////////////
		if ($('#FormOfPaymentAdviceMessageGroupItem_EnabledDate').val() == "No Enabled Date") {
    		$('#FormOfPaymentAdviceMessageGroupItem_EnabledDate').val("");
		}
		if ($('#FormOfPaymentAdviceMessageGroupItem_ExpiryDate').val() == "No Expiry Date") {
    		$('#FormOfPaymentAdviceMessageGroupItem_ExpiryDate').val("");
		}
	});

});