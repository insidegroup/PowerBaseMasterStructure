
jQuery.validator.unobtrusive.adapters.add(
        'atleastonerequired', ['properties'], function (options) {
            options.rules['atleastonerequired'] = options.params;
            options.messages['atleastonerequired'] = options.message;
        }
    );

jQuery.validator.addMethod('atleastonerequired', function (value, element, params) {
    var properties = params.properties.split(',');
    var values = $.map(properties, function (property, index) {
        var val = $('#ClientSubUnitClientDefinedReference_' + property).val();
        return val != '' ? val : null;
    });
    return values.length > 0;
}, '');




$(document).ready(function () {
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    $("#ClientSubUnitClientDefinedReference_CreditCardId").change(function () {
        $("#CreditCardValidTo").val("");
        if ($("#ClientSubUnitClientDefinedReference_CreditCardId").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/AutoComplete.mvc/CreditCardValidTo",
                data: { creditcardid: $("#ClientSubUnitClientDefinedReference_CreditCardId").val() },
                success: function (data) {
                    if (!jQuery.isEmptyObject(data)) {
                        var date = new Date(parseInt(data.substr(6)));
                        $("#CreditCardValidTo").val($.datepicker.formatDate("M dd yy", date));
                    } else {
                        $("#CreditCardValidTo").val("");
                    }
                },
                dataType: "json",
                async: false
            });
        }

    });

    $('#form0').submit(function () {
        var validCDR = false;

        if ($("#ClientSubUnitClientDefinedReference_Value").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidClientSubUnitCDR",
                data: {
                    cdrId: $("#ClientSubUnitClientDefinedReference_ClientSubUnitClientDefinedReferenceId").val(),
                    csuGuid: $("#ClientSubUnit_ClientSubUnitGuid").val(),
                    cdrValue: $("#ClientSubUnitClientDefinedReference_Value").val(),
                    canssc: $("#ClientSubUnitClientDefinedReference_ClientAccountNumberSourceSystemCode").val(),
                    ccId: $("#ClientSubUnitClientDefinedReference_CreditCardId").val()
                },
                success: function (data) {


                    validCDR = data;

                },
                dataType: "json",
                async: false
            });

            if (!validCDR) {
                $("#AirlineClassCabin_SupplierCode").val("");
                $("#lblClientSubUnitClientDefinedReferenceMsg").removeClass('field-validation-valid');
                $("#lblClientSubUnitClientDefinedReferenceMsg").addClass('field-validation-error');
                $("#lblClientSubUnitClientDefinedReferenceMsg").text("This CDR Value Already Exists");
                return false;
            } else {
                $("#lblClientSubUnitClientDefinedReferenceMsg").text("");
                return true;
            }

        }
    });
})