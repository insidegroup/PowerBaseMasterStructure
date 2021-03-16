


$(document).ready(function () {

    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    $('#PolicyAirCabinGroupItem_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0

    });

    $('#PolicyAirCabinGroupItem_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirCabinGroupItem_TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirCabinGroupItem_TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
        $("#PolicyRouting_FromCode").attr('disabled', 'disabled');
    }
    if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
        $("#PolicyRouting_ToCode").attr('disabled', 'disabled');
    }



    /* 
    Change Global Routing 
    */
    $("#PolicyRoutingAirCabin_FromGlobalFlag").click(function () {
        if ($("#PolicyRoutingAirCabin_FromGlobalFlag").is(':checked')) {
            $("#PolicyRoutingAirCabin_FromCode").val("");
            $("#PolicyRoutingAirCabin_FromCode").attr('disabled', 'disabled');
            $("#PolicyRoutingAirCabin_FromCode_validationMessage").text("");

        } else {
            $("#PolicyRoutingAirCabin_FromCode").removeAttr('disabled');
        }
    });
    $("#PolicyRoutingAirCabin_ToGlobalFlag").click(function () {
        if ($("#PolicyRoutingAirCabin_ToGlobalFlag").is(':checked')) {
            $("#PolicyRoutingAirCabin_ToCode").val("");
            $("#PolicyRoutingAirCabin_ToCode").attr('disabled', 'disabled');
            $("#PolicyRoutingAirCabin_ToCode_validationMessage").text("");
        } else {
            $("#PolicyRoutingAirCabin_ToCode").removeAttr('disabled');
        }
    });



    /*OnClick of Global Routing*/
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

    /*OnClick of Global Routing*/
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

    /*AutoComplete*/
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
            }
        });

    });

    /*AutoComplete*/
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
            }
        });

    });


    $('#form0').submit(function () {

        var validMileage = true;
        var minMileage = $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin").val();
        var maxMileage = $("#PolicyAirCabinGroupItem_FlightMileageAllowedMax").val();
        $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin_validationMessage").text("");

        if (isNan(minMileage)) {
            $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin_validationMessage").removeClass('field-validation-valid');
            $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin_validationMessage").addClass('field-validation-error');
            $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin_validationMessage").text("Minimum Mileage must be a number");
            validMileage = false;
        }
        if (isNan(maxMileage)) {
            $("#PolicyAirCabinGroupItem_FlightMileageAllowedMax_validationMessage").removeClass('field-validation-valid');
            $("#PolicyAirCabinGroupItem_FlightMileageAllowedMax_validationMessage").addClass('field-validation-error');
            $("#PolicyAirCabinGroupItem_FlightMileageAllowedMax_validationMessage").text("Maximum Mileage must be a number");
            validMileage = false;
        }
        if (minMileage != "" && maxMileage != "") {
            if (parseFloat(minMileage) > parseFloat(maxMileage)) {
                $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin_validationMessage").removeClass('field-validation-valid');
                $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin_validationMessage").addClass('field-validation-error');
                $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin_validationMessage").text("Minimum Mileage must be less than Maximum Mileage");
                validMileage = false;
            }

        }

        var validDuration = true;
        var minDuration = $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin").val();
        var maxDuration = $("#PolicyAirCabinGroupItem_FlightDurationAllowedMax").val();
        $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin_validationMessage").text("");

        if (isNan(minDuration)) {
            $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin_validationMessage").removeClass('field-validation-valid');
            $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin_validationMessage").addClass('field-validation-error');
            $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin_validationMessage").text("Minimum Mileage must be a number");
            validMileage = false;
        }
        if (isNan(maxDuration)) {
            $("#PolicyAirCabinGroupItem_FlightDurationAllowedMax_validationMessage").removeClass('field-validation-valid');
            $("#PolicyAirCabinGroupItem_FlightDurationAllowedMax_validationMessage").addClass('field-validation-error');
            $("#PolicyAirCabinGroupItem_FlightDurationAllowedMax_validationMessage").text("Maximum Mileage must be a number");
            validMileage = false;
        }
        if (minDuration != "" && maxDuration != "") {
            if (parseFloat(minDuration) > parseFloat(maxDuration)) {
                $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin_validationMessage").removeClass('field-validation-valid');
                $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin_validationMessage").addClass('field-validation-error');
                $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin_validationMessage").text("Minimum Duration must be less than Maximum Duration");
                validMileage = false;
            }

        }

        //No Routing Information is OK
        if (validMileage == true && validDuration == true) {

            if ($("#PolicyRouting_Name").val() == ""
                && !$("#PolicyRouting_FromGlobalFlag").is(':checked')
                && !$("#PolicyRouting_RoutingViceVersaFlag").is(':checked')
                && !$("#PolicyRouting_ToGlobalFlag").is(':checked')
                && $("#PolicyRouting_FromCode").val() == ""
                && $("#PolicyRouting_ToCode").val() == "") {

                return true;
            }
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

        //if no "FROM" is set, should it be?
        if (!$("#PolicyRouting_FromGlobalFlag").is(':checked') && $("#PolicyRouting_FromCode").val() == "") {
            if ($("#PolicyRouting_ToGlobalFlag").is(':checked') || $("#PolicyRouting_ToCode").val() != "" || $("#PolicyRouting_RoutingViceVersaFlag").is(':checked')) {
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

        //if no "TO" is set, should it be?
        if (!$("#PolicyRouting_ToGlobalFlag").is(':checked') && $("#PolicyRouting_ToCode").val() == "") {
            if ($("#PolicyRouting_FromGlobalFlag").is(':checked') || $("#PolicyRouting_FromCode").val() != "" || $("#PolicyRouting_RoutingViceVersaFlag").is(':checked')) {
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

        if (validFrom && validTo && validMileage && validDuration) {
            return true;
        } else {
            return false
        };

    });
});

function BuildRoutingName(from, to) {
    if (from == "" || to == "") {
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");
        return;
    }
    var autoName = from + "_to_" + to + "_AirCabin";
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