

$(document).ready(function () {
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    $('#PolicyAirVendorGroupItem_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirVendorGroupItem_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirVendorGroupItem_TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirVendorGroupItem_TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#PolicyAirVendorGroupItem_EnabledDate').val() == "") {
        $('#PolicyAirVendorGroupItem_EnabledDate').val("No Enabled Date")
    }
    if ($('#PolicyAirVendorGroupItem_ExpiryDate').val() == "") {
        $('#PolicyAirVendorGroupItem_ExpiryDate').val("No Expiry Date")
    }
    if ($('#PolicyAirVendorGroupItem_TravelDateValidFrom').val() == "") {
        $('#PolicyAirVendorGroupItem_TravelDateValidFrom').val("No Travel Date Valid From")
    }
    if ($('#PolicyAirVendorGroupItem_TravelDateValidTo').val() == "") {
        $('#PolicyAirVendorGroupItem_TravelDateValidTo').val("No Travel Date Valid To")
    }

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#PolicyAirVendorGroupItem_ProductId").val() == "") {
        $("#PolicyAirVendorGroupItem_SupplierName").attr("disabled", true);
    } else {
        $("#PolicyAirVendorGroupItem_SupplierName").removeAttr("disabled");
    }

    $("#PolicyAirVendorGroupItem_ProductId").change(function () {
        $("#PolicyAirVendorGroupItem_SupplierName").val("");
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");

        if ($("#PolicyAirVendorGroupItem_ProductId").val() == "") {
            $("#PolicyAirVendorGroupItem_SupplierName").attr("disabled", true);
        } else {
            $("#PolicyAirVendorGroupItem_SupplierName").removeAttr("disabled");
        }
    });

    $("#PolicyAirVendorGroupItem_SupplierName").change(function () {
        if ($("#PolicyAirVendorGroupItem_SupplierName").val() == "") {
            $("#PolicyAirVendorGroupItem_SupplierCode").val("");
            $("#SupplierCode").val("");
        }
    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////
    // BEGIN AIRSTATUS SETUP
    /////////////////////////////////////////////////////////
    if ($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val() != "1") {
        $("#PolicyAirVendorGroupItem_AirVendorRanking").attr("disabled", true);
    }
    $("#PolicyAirVendorGroupItem_PolicyAirStatusId").change(function () {
        if ($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val() != "1") {
            $("#PolicyAirVendorGroupItem_AirVendorRanking").val("");
            $("#PolicyAirVendorGroupItem_AirVendorRanking").attr("disabled", true);
        } else {
            $("#PolicyAirVendorGroupItem_AirVendorRanking").removeAttr("disabled");
        }
    });
    /////////////////////////////////////////////////////////
    // END AIRSTATUS SETUP
    /////////////////////////////////////////////////////////


    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////
    $(function () {
        $("#PolicyAirVendorGroupItem_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#PolicyAirVendorGroupItem_ProductId").val() },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: "<span class=\"supplier-name\">" + item.SupplierName + "</span><span class=\"supplier-code\">" + item.SupplierCode + "</span>",
                                id: item.SupplierCode,
                                value: item.SupplierName
                            }
                        }))
                    }
                })
            },
            mustMatch: true,
            select: function (event, ui) {
                $("#PolicyAirVendorGroupItem_SupplierName").val(ui.item.value);
                $("#PolicyAirVendorGroupItem_SupplierCode").val(ui.item.id);
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

    $('#form0').submit(function () {

        /////////////////////////////////////////////////////////
        // BEGIN PRODUCT/SUPPLIER VALIDATION
        // 1. Check Text for Supplier to see if valid
        // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
        /////////////////////////////////////////////////////////
        var validSupplier = false;
        var validSupplierProduct = false;

        if ($("#PolicyAirVendorGroupItem_SupplierName").val()) {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierName",
                data: { supplierName: $("#PolicyAirVendorGroupItem_SupplierName").val() },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validSupplier = true;

                        //user may not use auto, need to populate ID field
                        //$("#PolicyAirVendorGroupItem_SupplierCode").val(data[0].SupplierCode);
                    }
                },
                dataType: "json",
                async: false
            });

        	//issue where value of correct supplier code was overwritten by incorrect supplier code when 
        	//more than one supplier is returned when checking name (as product ID is different)
        	//force to use selection so supplier code is correctly set
            if ($("#PolicyAirVendorGroupItem_SupplierCode").val() == "") {
            	$("#lblSupplierNameMsg").removeClass('field-validation-valid');
            	$("#lblSupplierNameMsg").addClass('field-validation-error');
            	$("#lblSupplierNameMsg").text("Please use typeahead box to select supplier");
            	return false;
            }

            if (!validSupplier) {
                $("#PolicyAirVendorGroupItem_SupplierCode").val("");
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
                data: { supplierCode: $("#PolicyAirVendorGroupItem_SupplierCode").val(), productId: $("#PolicyAirVendorGroupItem_ProductId").val() },
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

        /////////////////////////////////////////////////////////
        // END PRODUCT/SUPPLIER VALIDATION
        /////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////
        // BEGIN AIRSTATUS/RANKING VALIDATION
        /////////////////////////////////////////////////////////
        var validAirVendorRanking = false;
        if ($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val() != "1") {
            $("#PolicyAirVendorGroupItem_AirVendorRanking").val("");
            $("#PolicyAirVendorGroupItem_AirVendorRanking").attr("disabled", true);
            validAirVendorRanking = true;
        } else {
            if (!$("#PolicyAirVendorGroupItem_AirVendorRanking").val() == "") {
                validAirVendorRanking = true;
            }
        }
        if (!validAirVendorRanking) {
            $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").removeClass('field-validation-valid');
            $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").addClass('field-validation-error');
            $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").text("Ranking Required for Preferred Items");
        } else {
            $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").text("");
        }
        if (!validSupplierProduct || !validAirVendorRanking) {
            return false;
        }
        /////////////////////////////////////////////////////////
        // END AIRSTATUS VALIDATION/RANKING
        /////////////////////////////////////////////////////////

       
       
    });


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

        if ($('#PolicyAirVendorGroupItem_EnabledDate').val() == "No Enabled Date") {
            $('#PolicyAirVendorGroupItem_EnabledDate').val("");
        }
        if ($('#PolicyAirVendorGroupItem_ExpiryDate').val() == "No Expiry Date") {
            $('#PolicyAirVendorGroupItem_ExpiryDate').val("");
        }
        if ($('#PolicyAirVendorGroupItem_TravelDateValidFrom').val() == "No Travel Date Valid From") {
            $('#PolicyAirVendorGroupItem_TravelDateValidFrom').val("");
        }
        if ($('#PolicyAirVendorGroupItem_TravelDateValidTo').val() == "No Travel Date Valid To") {
            $('#PolicyAirVendorGroupItem_TravelDateValidTo').val("");
        }


        if ($("#PolicyRouting_Name").val() != "") {
            return true;
        } else {

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
                $("#lblFrom").removeClass('field-validation-valid');
                $("#lblFrom").addClass('field-validation-error');
                $("#lblFrom").text("Please enter a value or choose Global");
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
                $("#lblTo").removeClass('field-validation-valid');
                $("#lblTo").addClass('field-validation-error');
                $("#lblTo").text("Please enter a value or choose Global");
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
        };

    });
});
    function BuildRoutingName(from, to) {
        if ($("#PolicyAirVendorGroupItem_SupplierCode").val() == "" || from == "" || to == "") {
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
            return;
        }
        var autoName = from + "_to_" + to + "_on_" + $("#PolicyAirVendorGroupItem_SupplierCode").val();
        autoName = autoName.replace(/ /g, "_");
        autoName = autoName.substring(0, 95);

        $.ajax({
            url: "/PolicyRouting.mvc/BuildRoutingName", type: "POST", dataType: "json",
            data: { routingName: autoName },
            success: function(data) {
                $("#lblAuto").text(data);
                $("#PolicyRouting_Name").val(data);
                $("#lblPolicyRoutingNameMsg").text("");
            }
        })
    }
