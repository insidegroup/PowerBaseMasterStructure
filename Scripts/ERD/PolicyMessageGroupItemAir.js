$(document).ready(function () {
    //Navigation
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');


    //Show DatePickers
    $('#PolicyMessageGroupItemAir_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyMessageGroupItemAir_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyMessageGroupItemAir_TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyMessageGroupItemAir_TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#PolicyMessageGroupItemAir_EnabledDate').val() == "") {
        $('#PolicyMessageGroupItemAir_EnabledDate').val("No Enabled Date")
    }
    if ($('#PolicyMessageGroupItemAir_ExpiryDate').val() == "") {
        $('#PolicyMessageGroupItemAir_ExpiryDate').val("No Expiry Date")
    }
    if ($('#PolicyMessageGroupItemAir_TravelDateValidFrom').val() == "") {
        $('#PolicyMessageGroupItemAir_TravelDateValidFrom').val("No Travel Date Valid From")
    }
    if ($('#PolicyMessageGroupItemAir_TravelDateValidTo').val() == "") {
        $('#PolicyMessageGroupItemAir_TravelDateValidTo').val("No Travel Date Valid To")
    }
    $(function () {
        $("#PolicyMessageGroupItemAir_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#PolicyMessageGroupItemAir_ProductId").val() },
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
                $("#PolicyMessageGroupItemAir_SupplierName").val(ui.item.value);
                $("#PolicyMessageGroupItemAir_SupplierCode").val(ui.item.id);
                $("#lblSupplierNameMsg").text("");

            }
        });

    });

    //Submit Form Validation
    $('#form0').submit(function () {

        /////////////////////////////////////////////////////////
        // BEGIN PRODUCT/SUPPLIER VALIDATION
        // 1. Check Text for Supplier to see if valid
        // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
        /////////////////////////////////////////////////////////
        var validSupplier = false;

        if ($("#PolicyMessageGroupItemAir_SupplierName").val()) {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierName",
                data: { supplierName: $("#PolicyMessageGroupItemAir_SupplierName").val() },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validSupplier = true;

                        //user may not use auto, need to populate ID field
                        $("#PolicyMessageGroupItemAir_SupplierCode").val(data[0].SupplierCode);
                    }
                },
                dataType: "json",
                async: false
            });

            if (!validSupplier) {
                $("#PolicyMessageGroupItemAir_SupplierCode").val("");
                $("#lblSupplierNameMsg").removeClass('field-validation-valid');
                $("#lblSupplierNameMsg").addClass('field-validation-error');
                $("#lblSupplierNameMsg").text("This is not a valid Supplier");
                return false;
            } else {
                $("#lblSupplierNameMsg").text("");
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
        }



        if ($('#PolicyMessageGroupItemAir_EnabledDate').val() == "No Enabled Date") {
            $('#PolicyMessageGroupItemAir_EnabledDate').val("");
        }
        if ($('#PolicyMessageGroupItemAir_ExpiryDate').val() == "No Expiry Date") {
            $('#PolicyMessageGroupItemAir_ExpiryDate').val("");
        }
        if ($('#PolicyMessageGroupItemAir_TravelDateValidFrom').val() == "No Travel Date Valid From") {
            $('#PolicyMessageGroupItemAir_TravelDateValidFrom').val("");
        }
        if ($('#PolicyMessageGroupItemAir_TravelDateValidTo').val() == "No Travel Date Valid To") {
            $('#PolicyMessageGroupItemAir_TravelDateValidTo').val("");
        }
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
});

function BuildRoutingName(from, to) {
    if ($("#PolicyMessageGroupItemAir_SupplierCode").val() == "" || from == "" || to == "") {
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");
        return;
    }
    var autoName = from + "_to_" + to + "_on_" + $("#PolicyMessageGroupItemAir_SupplierCode").val();
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
