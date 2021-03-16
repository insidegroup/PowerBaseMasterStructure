$(document).ready(function () {
	$('#menu_commissionableroutes').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    $('#breadcrumb').css('width', 'auto');

    var classOfTravel = $('#CommissionableRouteItem_ClassOfTravel');
	classOfTravel.val($.trim(classOfTravel.val()));

	/////////////////////////////////////////////////////////
	// Policy Routing
	/////////////////////////////////////////////////////////

    if ($("#CommissionableRouteItem_ProductId option:selected").text() != "Air") {
    	$('#PolicyRouting_FromGlobalFlag').attr("disabled", true);
    	$('#PolicyRouting_FromCode').attr("disabled", true);
    	$('#PolicyRouting_ToGlobalFlag').attr("disabled", true);
    	$('#PolicyRouting_ToCode').attr("disabled", true);
    	$('#PolicyRouting_RoutingViceVersaFlag').attr("disabled", true);
    }

	$('#CommissionableRouteItem_ProductId').change(function () {
		if ($("#CommissionableRouteItem_ProductId option:selected").text() == "Air") {
			$('#PolicyRouting_FromGlobalFlag').removeAttr("disabled");
			$('#PolicyRouting_FromCode').removeAttr("disabled");
			$('#PolicyRouting_ToGlobalFlag').removeAttr("disabled");
			$('#PolicyRouting_ToCode').removeAttr("disabled");
			$('#PolicyRouting_RoutingViceVersaFlag').removeAttr("disabled");
		} else {
			$("#lblAuto").text("");
			$("#PolicyRouting_Name").val("");
			$("#lblPolicyRoutingNameMsg").text("");

			$("#PolicyRouting_FromCode").val("");
			$("#PolicyRouting_ToCode").val("");
			$('#PolicyRouting_FromGlobalFlag').attr('checked', false);
			$('#PolicyRouting_ToGlobalFlag').attr('checked', false);
			$('#PolicyRouting_RoutingViceVersaFlag').attr('checked', false);

			$('#PolicyRouting_FromGlobalFlag').attr("disabled", true);
			$('#PolicyRouting_FromCode').attr("disabled", true);
			$('#PolicyRouting_ToGlobalFlag').attr("disabled", true);
			$('#PolicyRouting_ToCode').attr("disabled", true);
			$('#PolicyRouting_RoutingViceVersaFlag').attr("disabled", true);
		}
	});

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#CommissionableRouteItem_ProductId").val() == "") {
        $("#CommissionableRouteItem_SupplierName").attr("disabled", true);
    } else {
        $("#CommissionableRouteItem_SupplierName").removeAttr("disabled");
    }

    $("#CommissionableRouteItem_ProductId").change(function () {
        $("#CommissionableRouteItem_SupplierName").val("");
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");

        if ($("#CommissionableRouteItem_ProductId").val() == "") {
            $("#CommissionableRouteItem_SupplierName").attr("disabled", true);
        } else {
            $("#CommissionableRouteItem_SupplierName").removeAttr("disabled");

            if ($("#PolicyRouting_FromCodeType").val() != "" && $("#PolicyRouting_ToCodeType").val() != "") {

                var to = $("#PolicyRouting_ToCode").val();
                if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                    to = "Global";
                }
                var from = $("#PolicyRouting_FromCode").val();
                if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                    from = "Global";
                }
                BuildRoutingName(from, to);
            }
        }
    });

    $("#CommissionableRouteItem_SupplierName").change(function () {
        if ($("#CommissionableRouteItem_SupplierName").val() == "") {
            $("#CommissionableRouteItem_SupplierCode").val("");
            $("#SupplierCode").val("");
        }
    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////


    if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
        $("#PolicyRouting_FromCode").attr('disabled', 'disabled');
    }
    if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
        $("#PolicyRouting_ToCode").attr('disabled', 'disabled');
    }

    $("#PolicyRouting_FromGlobalFlag").click(function () {
        if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
            $("#PolicyRouting_FromCode").val("");
            $("#PolicyRouting_FromCodeType").val("Global");
            $("#PolicyRouting_FromCode").attr('disabled', 'disabled');
            $("#PolicyRouting_FromCode_validationMessage").text("");
            $("#lblFrom").text("");

            var to = $("#PolicyRouting_ToCode").val();
            if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                to = "Global";
            }
            BuildRoutingName("Global", to);
        } else {
            $("#PolicyRouting_FromCode").removeAttr('disabled');
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
        }
    });
    $("#PolicyRouting_ToGlobalFlag").click(function () {
        if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
            $("#PolicyRouting_ToCode").val("");
            $("#PolicyRouting_ToCodeType").val("Global");
            $("#PolicyRouting_ToCode").attr('disabled', 'disabled');
            $("#PolicyRouting_ToCode_validationMessage").text("");
            $("#lblTo").text("");

            var from = $("#PolicyRouting_FromCode").val();
            if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                from = "Global";
            }
            BuildRoutingName(from, "Global");
        } else {
            $("#PolicyRouting_ToCode").removeAttr('disabled');
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
        }
    });


    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////
    $(function () {
        $("#CommissionableRouteItem_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#CommissionableRouteItem_ProductId").val() },
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
            select: function (event, ui) {
                $("#CommissionableRouteItem_SupplierName").val(ui.item.value);
                $("#CommissionableRouteItem_SupplierCode").val(ui.item.id);
                $("#lblSupplierNameMsg").text("");

                var from = $("#PolicyRouting_FromCode").val();
                if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                    from = "Global";
                }
                var to = $("#PolicyRouting_ToCode").val();
                if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                    to = "Global";
                }
                BuildRoutingName(from, to);

            }
        });

    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////


	//Validate required fields
    $("input, select").blur(function () {

    	//Commission % on Tax will be mandatory if the Commission Tax code has a value in it.
    	var CommissionableTaxCodes = $("#CommissionableRouteItem_CommissionableTaxCodes").val();
    	var CommissionOnTaxes = $("#CommissionableRouteItem_CommissionOnTaxes").val();

    	if (CommissionOnTaxes != "" && CommissionableTaxCodes == "") {
    		$("#CommissionableTaxCodesError").text("Field is required if Commission % on Tax is completed");
    	} else {
    		$("#CommissionableTaxCodesError").text("");
    	}

    	//Commission Tax Code will be mandatory if the Commission % on Tax field has a value in it.
    	if (CommissionableTaxCodes != "" && CommissionOnTaxes == "") {
    		$("#CommissionOnTaxesError").text("Field is required if Commission Tax Code is completed");
    	} else {
    		$("#CommissionOnTaxesError").text("");
    	}

    	//The Commission Currency is required if Commission Amount field contains a value
    	var CommissionAmount = $('#CommissionableRouteItem_CommissionAmount').val();
    	var CurrencyCode = $('#CommissionableRouteItem_CommissionAmountCurrencyCode').val();

    	if (CommissionAmount != '' && CurrencyCode == '') {
    		$("#CommissionAmountCurrencyCodeError").text("Field is required if Commission Amount is completed");
    	} else {
    		$("#CommissionAmountCurrencyCodeError").text("");
    	}

    });
    

    $('form').submit(function () {

    	//Commission % on Tax will be mandatory if the Commission Tax code has a value in it.
    	var CommissionableTaxCodes = $("#CommissionableRouteItem_CommissionableTaxCodes").val();
    	var CommissionOnTaxes = $("#CommissionableRouteItem_CommissionOnTaxes").val();

    	if (CommissionOnTaxes != "" && CommissionableTaxCodes == "") {
    		$("#CommissionableTaxCodesError").text("Field is required if Commission % on Tax is completed");
    		return false;
    	} else {
    		$("#CommissionableTaxCodesError").text("");
    	}

    	//Commission Tax Code will be mandatory if the Commission % on Tax field has a value in it.
    	if (CommissionableTaxCodes != "" && CommissionOnTaxes == "") {
    		$("#CommissionOnTaxesError").text("Field is required if Commission Tax Code is completed");
    		return false;
    	} else {
    		$("#CommissionOnTaxesError").text("");
    	}

    	//The Commission Currency is required if Commission Amount field contains a value
    	var CommissionAmount = $('#CommissionableRouteItem_CommissionAmount').val();
    	var CurrencyCode = $('#CommissionableRouteItem_CommissionAmountCurrencyCode').val();

    	if (CommissionAmount != '' && CurrencyCode == '') {
    		$("#CommissionAmountCurrencyCodeError").text("Field is required if Commission Amount is completed");
    		return false;
    	} else {
    		$("#CommissionAmountCurrencyCodeError").text("");
    	}

    	//Every Commissionable Route Item must have either a Commission Amount (and Commission Currency) or Commission %
    	var BSPCommission = $('#CommissionableRouteItem_BSPCommission').val();
    	
    	if (CommissionAmount == '' && BSPCommission == '') {
    		$("#CommissionAmountCurrencyCodeError").text("Please either enter a Commission Amount (and Commission Currency) or Commission %");
    		return false;
    	}

        /////////////////////////////////////////////////////////
        // BEGIN PRODUCT/SUPPLIER VALIDATION
        // 1. Check Text for Supplier to see if valid
        // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
        /////////////////////////////////////////////////////////
        var validSupplier = false;
        var validSupplierProduct = false;

        if ($("#CommissionableRouteItem_SupplierName").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierName",
                data: { supplierName: $("#CommissionableRouteItem_SupplierName").val() },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validSupplier = true;

                        //user may not use auto, need to populate ID field
                        //$("#CommissionableRouteItem_SupplierCode").val(data[0].SupplierCode);
                    }
                },
                dataType: "json",
                async: false
            });

        	//issue where value of correct supplier code was overwritten by incorrect supplier code when 
            //more than one supplier is returned when checking name (as product ID is different)
        	//force to use selection so supplier code is correctly set
            if ($("#CommissionableRouteItem_SupplierCode").val() == "") {
            	$("#lblSupplierNameMsg").removeClass('field-validation-valid');
            	$("#lblSupplierNameMsg").addClass('field-validation-error');
            	$("#lblSupplierNameMsg").text("Please use typeahead box to select supplier");
            	return false;
            }

            if (!validSupplier) {
                $("#CommissionableRouteItem_SupplierCode").val("");
                $("#lblSupplierNameMsg").removeClass('field-validation-valid');
                $("#lblSupplierNameMsg").addClass('field-validation-error');
                $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            } else {
                $("#lblSupplierNameMsg").text("");
            }

            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierProduct",
                data: { supplierCode: $("#CommissionableRouteItem_SupplierCode").val(), productId: $("#CommissionableRouteItem_ProductId").val() },
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
        } else {
            validSupplier = true;
            validSupplierProduct = true;
        }

        /////////////////////////////////////////////////////////
        // END PRODUCT/SUPPLIER VALIDATION
        /////////////////////////////////////////////////////////
        if (!validSupplier || !validSupplierProduct) {
            return false;
        } else {
            return true;
        }
    });



    $(function () {
        $("#PolicyRouting_ToCode").autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: "/PolicyRouting.mvc/AutoCompletePolicyRoutingFromTo", type: "POST", dataType: "json",
                    data: { searchText: request.term, maxResults: 10 },
                    success: function (data) {

                        response($.map(data, function (item) {
                            return {
                                label: (item.CodeType == "TravelPortType") ? item.Name : item.Name + "," + item.Parent,
                                value: item.Code,
                                id: item.CodeType
                            }
                        }))
                    }
                })
            },
            select: function (event, ui) {
                if (ui.item.id == "TravelPortType") {
                    $("#lblTo").text("TravelPort Type");
                } else {
                    $("#lblTo").text(ui.item.label + ' (' + ui.item.value + ')');
                }
                $("#PolicyRouting_ToCodeType").val(ui.item.id);
                $("#PolicyRouting_ToGlobalFlag").attr('checked', false);


                var from = $("#PolicyRouting_FromCode").val();
                if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                    from = "Global";
                }
                BuildRoutingName(from, ui.item.value);
            },
            search: function (event, ui) {
                $("#PolicyRouting_ToCodeType").val("");
            }
        });

    });

    $(function () {
        $("#PolicyRouting_FromCode").autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: "/PolicyRouting.mvc/AutoCompletePolicyRoutingFromTo", type: "POST", dataType: "json",
                    data: { searchText: request.term, maxResults: 10 },
                    success: function (data) {

                        response($.map(data, function (item) {
                            return {
                                label: (item.CodeType == "TravelPortType") ? item.Name : item.Name + "," + item.Parent,
                                value: item.Code,
                                id: item.CodeType
                            }
                        }))
                    }
                })
            },
            select: function (event, ui) {
                if (ui.item.id == "TravelPortType") {
                    $("#lblFrom").text("TravelPort Type");
                } else {
                    $("#lblFrom").text(ui.item.label + ' (' + ui.item.value + ')');
                }
                $("#PolicyRouting_FromCodeType").val(ui.item.id);
                $("#PolicyRouting_FromGlobalFlag").attr('checked', false);

                var to = $("#PolicyRouting_ToCode").val();
                if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                    to = "Global";
                }
                BuildRoutingName(ui.item.value, to);
            },
            search: function (event, ui) {
                $("#PolicyRouting_FromCodeType").val("");
            }
        });

    });


    $('#form0').submit(function () {
        if ($("#PolicyRouting_Name").val() == "" && !$("#PolicyRouting_FromGlobalFlag").is(':checked') && !$("#PolicyRouting_FromGlobalFlag").is(':checked')
                && $("#PolicyRouting_FromCode").val() == "" && $("#PolicyRouting_ToCode").val() == "") {
            return true;
        }

        var validFrom = false;
        var validTo = false;
        $("#lblFrom").text("");
        $("#lblTo").text("");

        if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
            validFrom = true;
        }
        if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
            validTo = true;
        }
        if (!$("#PolicyRouting_FromGlobalFlag").is(':checked') && $("#PolicyRouting_FromCode").val() == "") {
            if ($("#PolicyRouting_ToGlobalFlag").is(':checked') || $("#PolicyRouting_ToCode").val() != "") {
                $("#lblFrom").removeClass('field-validation-valid');
                $("#lblFrom").addClass('field-validation-error');
                $("#lblFrom").text("Please enter a value or choose Global");
            }
        }
        if ($("#PolicyRouting_FromCode").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
                data: { fromto: $("#PolicyRouting_FromCode").val(), codetype: $("#PolicyRouting_FromCodeType").val() },
                success: function (data) {
                    if (!jQuery.isEmptyObject(data)) {
                        validFrom = true;
                    }
                },
                dataType: "json",
                async: false
            });

            if (!validFrom) {
                $("#lblFrom").removeClass('field-validation-valid');
                $("#lblFrom").addClass('field-validation-error');
                $("#lblFrom").text("This is not a valid entry");
            }
        };

        if (!$("#PolicyRouting_ToGlobalFlag").is(':checked') && $("#PolicyRouting_ToCode").val() == "") {
            if ($("#PolicyRouting_FromGlobalFlag").is(':checked') || $("#PolicyRouting_FromCode").val() != "") {
                $("#lblTo").removeClass('field-validation-valid');
                $("#lblTo").addClass('field-validation-error');
                $("#lblTo").text("Please enter a value or choose Global");
            }
        }



        if ($("#PolicyRouting_ToCode").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
                data: { fromto: $("#PolicyRouting_ToCode").val(), codetype: $("#PolicyRouting_ToCodeType").val() },
                success: function (data) {
                    if (!jQuery.isEmptyObject(data)) {
                        validTo = true;
                    }

                },
                dataType: "json",
                async: false
            });

            if (!validTo) {
                $("#lblTo").removeClass('field-validation-valid');
                $("#lblTo").addClass('field-validation-error');
                $("#lblTo").text("This is not a valid entry");
            }
        };
        if (validFrom && validTo) {
            return true;
        } else {
            return false
        };


    });
});
function BuildRoutingName(from, to) {

    if (from == "" || to == "" || $("#CommissionableRouteItem_ProductId").val() == "") {
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");
        return;
    }
    
    var autoName = from + "_to_" + to + "_on_" ;
    if ($("#CommissionableRouteItem_SupplierCode").val() != "") {
        autoName = autoName + $("#CommissionableRouteItem_SupplierCode").val();
    } else {
        autoName = autoName + $("#CommissionableRouteItem_ProductId option:selected").text();
    }
    autoName = autoName.replace(/ /g, "_");
    autoName = autoName.substring(0, 95);

    $.ajax({
        url: "/PolicyRouting.mvc/BuildRoutingName", type: "POST", dataType: "json",
        data: { routingName: autoName },
        success: function (data) {
            $("#lblAuto").text(data);
            $("#PolicyRouting_Name").val(data);
            $("#lblPolicyRoutingNameMsg").text("");
        }
    })
}
