

$(document).ready(function () {
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    $('#PolicyAirParameterGroupItem_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirParameterGroupItem_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirParameterGroupItem_TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirParameterGroupItem_TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#PolicyAirParameterGroupItem_EnabledDate').val() == "") {
        $('#PolicyAirParameterGroupItem_EnabledDate').val("No Enabled Date")
    }
    if ($('#PolicyAirParameterGroupItem_ExpiryDate').val() == "") {
        $('#PolicyAirParameterGroupItem_ExpiryDate').val("No Expiry Date")
    }
    if ($('#PolicyAirParameterGroupItem_TravelDateValidFrom').val() == "") {
        $('#PolicyAirParameterGroupItem_TravelDateValidFrom').val("No Travel Date Valid From")
    }
    if ($('#PolicyAirParameterGroupItem_TravelDateValidTo').val() == "") {
        $('#PolicyAirParameterGroupItem_TravelDateValidTo').val("No Travel Date Valid To")
    }

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

    	var validForm = true;

    	//Reset Errors if filled in
    	var PolicyAirParameterGroupItem_PolicyAirParameterValue = $("#PolicyAirParameterGroupItem_PolicyAirParameterValue").val();

    	if (PolicyAirParameterGroupItem_PolicyAirParameterValue != "") {
    		$('#PolicyAirParameterGroupItem_PolicyAirParameterValue_validationMessage').text("").hide();
    	}

    	//Must be numerical only up to 4 numbers
    	var reg = /^\d{0,4}$/;
    	if (!reg.test(PolicyAirParameterGroupItem_PolicyAirParameterValue)) {
    		$('#PolicyAirParameterGroupItem_PolicyAirParameterValue_validationMessage').addClass("error").text("Please provide a numerical value - maximum of 4 numbers.").show();
    		$("#PolicyAirParameterGroupItem_PolicyAirParameterValue").focus();
    		return false;
    	}
    	
		if ($('#PolicyAirParameterGroupItem_EnabledDate').val() == "No Enabled Date") {
            $('#PolicyAirParameterGroupItem_EnabledDate').val("");
        }
        if ($('#PolicyAirParameterGroupItem_ExpiryDate').val() == "No Expiry Date") {
            $('#PolicyAirParameterGroupItem_ExpiryDate').val("");
        }
        if ($('#PolicyAirParameterGroupItem_TravelDateValidFrom').val() == "No Travel Date Valid From") {
            $('#PolicyAirParameterGroupItem_TravelDateValidFrom').val("");
        }
        if ($('#PolicyAirParameterGroupItem_TravelDateValidTo').val() == "No Travel Date Valid To") {
            $('#PolicyAirParameterGroupItem_TravelDateValidTo').val("");
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

            if (validFrom && validTo && validForm) {
                return true;
            } else {
                return false
            };
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
    var routingNameExtension = $('#RoutingNameExtension').val();
    var autoName = from + "_to_" + to + "_" + routingNameExtension;
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
