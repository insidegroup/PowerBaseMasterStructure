//////////////////////////////////
//requiredif
//////////////////////////////////
$.validator.addMethod('requiredif',
function (value, element, parameters) {
    var id = '#TransactionFee_' + parameters['dependentproperty'];

    // get the target value (as a string, 
    // as that's what actual value will be)
    var targetvalue = parameters['targetvalue'];
    targetvalue =
      (targetvalue == null ? '' : targetvalue).toString();

    // get the actual value of the target control
    // note - this probably needs to cater for more 
    // control types, e.g. radios
    var control = $(id);
    var controltype = control.attr('type');
    var actualvalue =
        controltype === 'checkbox' ?
        control.attr('checked').toString() :
        control.val();

    // alert(actualvalue)
    // if the condition is true, reuse the existing 
    // required field validator functionality
    if (targetvalue === actualvalue)
        return $.validator.methods.required.call(
          this, value, element, parameters);

    return true;
}
 );

$.validator.unobtrusive.adapters.add(
'requiredif',
['dependentproperty', 'targetvalue'],
function (options) {
    options.rules['requiredif'] = {
        dependentproperty: options.params['dependentproperty'],
        targetvalue: options.params['targetvalue']
    };
    options.messages['requiredif'] = options.message;
});

//////////////////////////////////
//requiredifnot
//////////////////////////////////
$.validator.addMethod('requiredifnot',
function (value, element, parameters) {
    var id = '#TransactionFee_' + parameters['dependentproperty'];
    //alert(id);
    // get the target value (as a string, 
    // as that's what actual value will be)
    var targetvalue = parameters['targetvalue'];
    targetvalue =
      (targetvalue == null ? '' : targetvalue).toString();

    // get the actual value of the target control
    // note - this probably needs to cater for more 
    // control types, e.g. radios
    var control = $(id);
    var controltype = control.attr('type');
    var actualvalue =
        controltype === 'checkbox' ?
        control.attr('checked').toString() :
        control.val();

    // alert(actualvalue)
    // if the condition is true, reuse the existing 
    // required field validator functionality
    if (targetvalue != actualvalue)
        return $.validator.methods.required.call(
          this, value, element, parameters);

    return true;
}
 );

$.validator.unobtrusive.adapters.add(
'requiredifnot',
['dependentproperty', 'targetvalue'],
function (options) {
    options.rules['requiredifnot'] = {
        dependentproperty: options.params['dependentproperty'],
        targetvalue: options.params['targetvalue']
    };
    options.messages['requiredifnot'] = options.message;
});


//////////////////////////////////
//atleastonerequired
//////////////////////////////////
jQuery.validator.unobtrusive.adapters.add(
        'atleastonerequired', ['properties'], function (options) {
            options.rules['atleastonerequired'] = options.params;
            options.messages['atleastonerequired'] = options.message;
        }
    );

jQuery.validator.addMethod('atleastonerequired', function (value, element, params) {
    var properties = params.properties.split(',');
    var values = $.map(properties, function (property, index) {
        var val = $('#TransactionFee_' + property).val();
        return val != '' ? val : null;
    });
    return values.length > 0;
}, '');



//////////////////////////////////
//onSubmit
//////////////////////////////////
$.validator.setDefaults({
    submitHandler: function (form) {


        if ($("#PolicyRouting_Name").val() == "" && !$("#PolicyRouting_FromGlobalFlag").is(':checked') && !$("#PolicyRouting_FromGlobalFlag").is(':checked')
                && $("#PolicyRouting_FromCode").val() == "" && $("#PolicyRouting_ToCode").val() == "" && $("#TransactionFee_FeeCategory").val() != "Destination") {


            if ($('#TransactionFee_EnabledDate').val() == "No Enabled Date") {
                $('#TransactionFee_EnabledDate').val("");
            }
            if ($('#TransactionFee_ExpiryDate').val() == "No Expiry Date") {
                $('#TransactionFee_ExpiryDate').val("");
            }

            form.submit();
            return;
        }
        if ($("#TransactionFee_FeeCategory").val() == "Destination" && $("#lblAuto").text() == "") {
            $("#lblPolicyRoutingNameMsg").text("Policy Routing is required");
        }

        var validEnabledDate = false;
        var validExpiryDate = false;
        var validFrom = false;
        var validTo = false;
        
        /////////////////////////////////////////////////////////
        // DATE VALIDATION
        /////////////////////////////////////////////////////////
        if ($('#TransactionFee_EnabledDate').val() == "No Enabled Date" || isValidDate($('#TransactionFee_EnabledDate').val())) {
            validEnabledDate = true;
        } else {
            alert("EE");
            $("#TransactionFee_EnabledDate_validationMessage").removeClass('field-validation-valid');
            $("#TransactionFee_EnabledDate_validationMessage").addClass('field-validation-error');
            $("#TransactionFee_EnabledDate_validationMessage").text("Date should be in format MMM dd yyyy. eg Dec 01 2014");
            alert("EE2");

        }
        if ($('#TransactionFee_ExpiryDate').val() == "No Expiry Date" || isValidDate($('#TransactionFee_ExpiryDate').val())) {
            validExpiryDate = true;
        } else {
            $("#TransactionFee_ExpiryDate_validationMessage").removeClass('field-validation-valid');
            $("#TransactionFee_ExpiryDate_validationMessage").addClass('field-validation-error');
            $("#TransactionFee_ExpiryDate_validationMessage").text("Date should be in format MMM dd yyyy. eg Dec 01 2014");
        }

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

        if (validFrom && validTo && validEnabledDate && validExpiryDate) {

            if ($('#TransactionFee_EnabledDate').val() == "No Enabled Date") {
                $('#TransactionFee_EnabledDate').val("");
            }
            if ($('#TransactionFee_ExpiryDate').val() == "No Expiry Date") {
                $('#TransactionFee_ExpiryDate').val("");
            }

            form.submit();
            return;
        }

    }
});



$(document).ready(function () {

    //////////////////////////////////
    //setup
    //////////////////////////////////

    //Navigation
    $('#menu_clientfeegroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#TransactionFee_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#TransactionFee_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });


    if ($('#TransactionFee_ExpiryDate').val() == "") {
        $('#TransactionFee_ExpiryDate').val("No Expiry Date")
    }
    if ($('#TransactionFee_EnabledDate').val() == "") {
        $('#TransactionFee_EnabledDate').val("No Enabled Date")
    }

    $("#TransactionFee_SupplierName").change(function () {
        if ($("#TransactionFee_SupplierName").val() == "") {
            $("#TransactionFee_SupplierCode").val("");
        }
    });

    $("#TransactionFee_FeeCategory").change(function () {
        if ($("#TransactionFee_FeeCategory").val() != "Destination") {
            $("#lblPolicyRoutingNameMsg").text("");
        }
    });

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////
    $(function () {
        $("#TransactionFee_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#TransactionFee_ProductId").val() },
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
                $("#TransactionFee_SupplierName").val(ui.item.value);
                $("#TransactionFee_SupplierCode").val(ui.item.id);
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

    /////////////////////////////////////////////////////////
    // BEGIN POLICY ROUTING SETUP
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
    // END POLICY ROUTING SETUP
    /////////////////////////////////////////////////////////


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
    if ($("#TransactionFee_SupplierCode").val() == "" || from == "" || to == "") {
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");
        return;
    }
    var autoName = from + "_to_" + to + "_on_" + $("#TransactionFee_SupplierCode").val();
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


