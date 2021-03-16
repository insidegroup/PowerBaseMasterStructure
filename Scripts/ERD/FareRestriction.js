$(document).ready(function () {
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");


    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#FareRestriction_ProductId").val() == "") {
        $("#FareRestriction_SupplierName").attr("disabled", true);
    } else {
        $("#FareRestriction_SupplierName").removeAttr("disabled");
    }

    $("#FareRestriction_ProductId").change(function () {
        $("#FareRestriction_SupplierName").val("");
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");

        if ($("#FareRestriction_ProductId").val() == "") {
            $("#FareRestriction_SupplierName").attr("disabled", true);
        } else {
            $("#FareRestriction_SupplierName").removeAttr("disabled");

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

    $("#FareRestriction_SupplierName").change(function () {
        if ($("#FareRestriction_SupplierName").val() == "") {
            $("#FareRestriction_SupplierCode").val("");
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
        $("#FareRestriction_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#FareRestriction_ProductId").val() },
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
                $("#FareRestriction_SupplierName").val(ui.item.value);
                $("#FareRestriction_SupplierCode").val(ui.item.id);
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

        var validChars = true;
        var allowedChars = /^([\w\s-()*]+)$/;

        if ($("#FareRestriction_FareBasis").val() != "" && !allowedChars.test($("#FareRestriction_FareBasis").val())) {
            $("#FareRestriction_FareBasis_validationMessage").removeClass('field-validation-valid');
            $("#FareRestriction_FareBasis_validationMessage").addClass('field-validation-error');
            $("#FareRestriction_FareBasis_validationMessage").text("Special character entered is not allowed.");
            validChars = false;
        } else {
            $("#FareRestriction_FareBasis_validationMessage").text("");
        }
        if ($("#FareRestriction_Changes").val() != "" && !allowedChars.test($("#FareRestriction_Changes").val())) {
            $("#FareRestriction_Changes_validationMessage").removeClass('field-validation-valid');
            $("#FareRestriction_Changes_validationMessage").addClass('field-validation-error');
            $("#FareRestriction_Changes_validationMessage").text("Special character entered is not allowed.");
            validChars = false;
        } else {
            $("#FareRestriction_Changes_validationMessage").text("");
        }
        if ($("#FareRestriction_Cancellations").val() != "" && !allowedChars.test($("#FareRestriction_Cancellations").val())) {
            $("#FareRestriction_Cancellations_validationMessage").removeClass('field-validation-valid');
            $("#FareRestriction_Cancellations_validationMessage").addClass('field-validation-error');
            $("#FareRestriction_Cancellations_validationMessage").text("Special character entered is not allowed.");
            validChars = false;
        } else {
            $("#FareRestriction_Cancellations_validationMessage").text("");
        }
        if ($("#FareRestriction_ReRoute").val() != "" && !allowedChars.test($("#FareRestriction_ReRoute").val())) {
            $("#FareRestriction_ReRoute_validationMessage").removeClass('field-validation-valid');
            $("#FareRestriction_ReRoute_validationMessage").addClass('field-validation-error');
            $("#FareRestriction_ReRoute_validationMessage").text("Special character entered is not allowed.");
            validChars = false;
        } else {
            $("#FareRestriction_ReRoute_validationMessage").text("");
        }
        if ($("#FareRestriction_ValidOn").val() != "" && !allowedChars.test($("#FareRestriction_ValidOn").val())) {
            $("#FareRestriction_ValidOn_validationMessage").removeClass('field-validation-valid');
            $("#FareRestriction_ValidOn_validationMessage").addClass('field-validation-error');
            $("#FareRestriction_ValidOn_validationMessage").text("Special character entered is not allowed.");
            validChars = false;
        } else {
            $("#FareRestriction_ValidOn_validationMessage").text("");
        }
        if ($("#FareRestriction_MinimumStay").val() != "" && !allowedChars.test($("#FareRestriction_MinimumStay").val())) {
            $("#FareRestriction_MinimumStay_validationMessage").removeClass('field-validation-valid');
            $("#FareRestriction_MinimumStay_validationMessage").addClass('field-validation-error');
            $("#FareRestriction_MinimumStay_validationMessage").text("Special character entered is not allowed.");
            validChars = false;
        } else {
            $("#FareRestriction_MinimumStay_validationMessage").text("");
        }
        if ($("#FareRestriction_MaximumStay").val() != "" && !allowedChars.test($("#FareRestriction_MaximumStay").val())) {
            $("#FareRestriction_MaximumStay_validationMessage").removeClass('field-validation-valid');
            $("#FareRestriction_MaximumStay_validationMessage").addClass('field-validation-error');
            $("#FareRestriction_MaximumStay_validationMessage").text("Special character entered is not allowed.");
            validChars = false;
        } else {
            $("#FareRestriction_MaximumStay_validationMessage").text("");
        }
        /////////////////////////////////////////////////////////
        // BEGIN PRODUCT/SUPPLIER VALIDATION
        // 1. Check Text for Supplier to see if valid
        // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
        /////////////////////////////////////////////////////////
        var validSupplier = false;
        var validSupplierProduct = false;

        if ($("#FareRestriction_SupplierName").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierName",
                data: { supplierName: $("#FareRestriction_SupplierName").val() },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validSupplier = true;

                        //user may not use auto, need to populate ID field
                        $("#FareRestriction_SupplierCode").val(data[0].SupplierCode);
                    }
                },
                dataType: "json",
                async: false
            });

            if (!validSupplier) {
                $("#FareRestriction_SupplierCode").val("");
                $("#lblSupplierNameMsg").removeClass('field-validation-valid');
                $("#lblSupplierNameMsg").addClass('field-validation-error');
                $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            } else {
                $("#lblSupplierNameMsg").text("");
            }

            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierProduct",
                data: { supplierCode: $("#FareRestriction_SupplierCode").val(), productId: $("#FareRestriction_ProductId").val() },
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
        if (!validSupplier || !validChars || !validSupplierProduct) {
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

    if (from == "" || to == "" || $("#FareRestriction_ProductId").val() == "") {
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");
        return;
    }
    
    var autoName = from + "_to_" + to + "_on_" ;
    if ($("#FareRestriction_SupplierCode").val() != "") {
        autoName = autoName + $("#FareRestriction_SupplierCode").val();
    } else {
        autoName = autoName + $("#FareRestriction_ProductId option:selected").text();
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
