/*
OnReady
*/
$(document).ready(function() {

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$("#breadcrumb").css("width", "auto");

	//Time Pickers
	$('#ServiceFund_ServiceFundStartTimeString, #ServiceFund_ServiceFundEndTimeString').calendricalTimeRange();

	//Product
	$("#ServiceFund_ProductId").change(function () {

		$('#ServiceFund_SupplierName').val("").attr('disabled', true);
		$('#ServiceFund_SupplierCode').val("");


		var productId = $(this).val();
		if (productId != "") {
			$('#ServiceFund_SupplierName').attr('disabled', false);
		}
	});

	//Supplier
	if ($("#ServiceFund_ProductId").val() == "") {
		$('#ServiceFund_SupplierName').attr('disabled', true);
	}

	//Client Account
	if ($("#ServiceFund_ClientTopUnitName").val() == "") {
		$('#ServiceFund_ClientAccountName').val("").attr('disabled', true);
		$('#ServiceFund_ClientAccountNumber').val("");
		$('#ServiceFund_SourceSystemCode').val("");
	}

	$("#ServiceFund_ClientTopUnitName").live("change keyup", function () {
		var clientTopUnit = $(this).val();
		if (clientTopUnit != "") {
			$('#ServiceFund_ClientAccountName').attr('disabled', false);
		} else {
			$('#ServiceFund_ClientAccountName').val("").attr('disabled', true);
			$('#ServiceFund_ClientAccountNumber').val("");
			$('#ServiceFund_SourceSystemCode').val("");
		}
	}); 


	/*
    Submit Form Validation
    */
    $("form").submit(function() {
    
    	var validItem = true;

    	//IsValid ServiceFundPseudoCityOrOfficeId/GDS

        var serviceFundPcc = $("#ServiceFund_ServiceFundPseudoCityOrOfficeId").val();
        var gdsCode = $("#ServiceFund_GDSCode").val();

        $('#lblValidServiceFundPseudoCityOrOfficeIdMessage').text("");

        var validServiceFundPseudoCityOrOfficeId = false;

        if (serviceFundPcc != '' && gdsCode != '') {
        	jQuery.ajax({
        		type: "POST",
        		url: "/GroupNameBuilder.mvc/IsValidPccGDS",
        		data: { pcc: serviceFundPcc, gds: gdsCode },
        		success: function (data) {
        			validServiceFundPseudoCityOrOfficeId = data;
        		},
        		dataType: "json",
        		async: false
        	});

        	if (!validServiceFundPseudoCityOrOfficeId) {
        		$('#lblValidServiceFundPseudoCityOrOfficeIdMessage')
					.addClass('field-validation-error')
					.text('The PCC/Office ID you have selected is not valid for this GDS.');
        		return false;
        	}
        }

    	//IsValid Supplier/Product

        var validSupplier = false;
        var validSupplierProduct = false;

        if ($("#ServiceFund_SupplierName").val()) {
        	jQuery.ajax({
        		type: "POST",
        		url: "/Validation.mvc/IsValidSupplierName",
        		data: { supplierName: $("#ServiceFund_SupplierName").val() },
        		success: function (data) {

        			if (!jQuery.isEmptyObject(data)) {
        				validSupplier = true;

        				//user may not use auto, need to populate ID field
        				//$("#ServiceFund_SupplierCode").val(data[0].SupplierCode);
        			}
        		},
        		dataType: "json",
        		async: false
        	});

        	//issue where value of correct supplier code was overwritten by incorrect supplier code when 
        	//more than one supplier is returned when checking name (as product ID is different)
        	//force to use selection so supplier code is correctly set
        	if ($("#ServiceFund_SupplierCode").val() == "") {
        		$("#lblSupplierNameMsg").removeClass('field-validation-valid');
        		$("#lblSupplierNameMsg").addClass('field-validation-error');
        		$("#lblSupplierNameMsg").text("Please use typeahead box to select supplier");
        		return false;
        	}

        	if (!validSupplier) {
        		$("#ServiceFund_SupplierCode").val("");
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
        		data: { supplierCode: $("#ServiceFund_SupplierCode").val(), productId: $("#ServiceFund_ProductId").val() },
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
        	} else {
        		$("#lblSupplierNameMsg").text("");
        	}
        }

    	//IsValid ServiceFund

        jQuery.ajax({
        	type: "POST",
        	url: "/ServiceFund.mvc/IsAvailableServiceFund",
        	data: {
        		clientTopUnitGUID: $("#ServiceFund_ClientTopUnitGuid").val(),
        		clientAccountNumber: $("#ServiceFund_ClientAccountNumber").val(),
        		productId: $("#ServiceFund_ProductId").val(),
        		supplierCode: $("#ServiceFund_SupplierCode").val(),
        		serviceFundChannelTypeId: $("#ServiceFund_ServiceFundChannelTypeId").val(),
				serviceFundID: $("#ServiceFund_ServiceFundId").val() != "" ? $("#ServiceFund_ServiceFundId").val() : 0
        	},
        	success: function (data) {
        		valid = data;
        	},
        	dataType: "json",
        	async: false
        });

        if (validItem) {
        	$("#lblServiceFundMsg").text("");
        	return true;
        } else {
        	$("#lblServiceFundMsg").addClass('field-validation-error');
        	$("#lblServiceFundMsg").text("This combination of Client TopUnit Name, Client Account Number, Supplier Code and Channel Type has already been used, please choose a different combination or edit the existing item.");
        	return false;
        };

        return validItem;

    });
});

//Client Top Unit Autocomplete
$(function () {

	$("#ServiceFund_ClientTopUnitName").autocomplete({
		minLength: 2,
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
				data: { searchText: request.term, hierarchyItem: "ClientTopUnit", domainName: 'Service Funds Administrator', resultCount: 5000 },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
							value: item.HierarchyName,
							id: item.HierarchyCode,
							text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
						}
					}))
				}
			});
		},
		select: function (event, ui) {
			$("#ServiceFund_ClientTopUnitName").val(ui.item.value);
			$("#ServiceFund_ClientTopUnitGuid").val(ui.item.id);
		}
	});

	$('.ui-autocomplete').addClass('widget-overflow');
	
});

//Account Autocomplete
$(function () {
	
	$("#ServiceFund_ClientAccountName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/ClientTopUnitClientAccounts", type: "POST", dataType: "json",
				data: { searchText: request.term, clientTopUnitGuid: $("#ServiceFund_ClientTopUnitGuid").val() },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: "<span class=\"ca-name\">" + item.HierarchyName + "</span><span class=\"ca-number\">" + item.ClientAccountNumber + "</span><span class=\"ca-ssc\">" + item.SourceSystemCode + "</span>",
							value: item.HierarchyName,
							id: item.ClientAccountNumber,
							ssc: item.SourceSystemCode,
							text: ""
						}
					}))
				}
			});	},
		select: function (event, ui) {
			$("#ServiceFund_ClientAccountName").val(ui.item.value);
			$("#ServiceFund_ClientAccountNumber").val(ui.item.id);
			$("#ServiceFund_SourceSystemCode").val(ui.item.ssc);
		}
	});

	$('.ui-autocomplete').addClass('widget-overflow');
});

//Supplier Autocomplete
$(function () {
	$("#ServiceFund_SupplierName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
				data: { searchText: request.term, productId: $("#ServiceFund_ProductId").val() },
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
			$("#ServiceFund_SupplierName").val(ui.item.value);
			$("#ServiceFund_SupplierCode").val(ui.item.id);
			$("#lblSupplierNameMsg").text("");
		}
	});

	$('.ui-autocomplete').addClass('widget-overflow');
});