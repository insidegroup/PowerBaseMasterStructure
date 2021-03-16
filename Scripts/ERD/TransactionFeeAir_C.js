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
        if ($('#TransactionFee_EnabledDate').val() == "No Enabled Date") {
            $('#TransactionFee_EnabledDate').val("");
        }
        if ($('#TransactionFee_ExpiryDate').val() == "No Expiry Date") {
            $('#TransactionFee_ExpiryDate').val("");
        }
        form.submit();
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

            }
        });

    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////

});

