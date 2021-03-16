
$(document).ready(function() {
    //Navigation
$('#menu_reasoncodes').click();
$("tr:odd").addClass("row_odd");
$("tr:even").addClass("row_even");

    if ($("#ReasonCode").val() == "") {
        $("#ProductId").val("");
        $("#ProductId").attr("disabled", true);
        $("#ReasonCodeTypeId").val("");
        $("#ReasonCodeTypeId").attr("disabled", true);
    }
});

$(function() {
$("#ReasonCode").change(function() {

    if ($("#ReasonCode").val() != "") {
        $("#ProductId").val("");
        $("#ProductId").removeAttr("disabled");
    }


        jQuery.ajax({
            type: "POST",
            url: "/ReasonCodeItem.mvc/GetReasonCodeProducts",
            data: { reasonCode: $("#ReasonCode").val() },
            success: function(data) {
                $("#ProductId").get(0).options.length = 0;
                $("#ProductId").get(0).options[0] = new Option("Please Select...", "");

                $.each(data, function(index, item) {
                    $("#ProductId").get(0).options[$("#ProductId").get(0).options.length] = new Option(item.ProductName, item.ProductId);
                });
            },
            dataType: "json",
            async: false
        });
    });
});

$(function() {
$("#ProductId").change(function() {

    if ($("#ReasonCode").val() != "") {
        $("#ReasonCodeTypeId").val("");
        $("#ReasonCodeTypeId").removeAttr("disabled");
    }
        jQuery.ajax({
            type: "POST",
            url: "/ReasonCodeItem.mvc/GetReasonCodeProductReasonCodeTypes",
            data: { groupId: $("#ReasonCodeGroupId").val(), reasonCode: $("#ReasonCode").val(), productId: $("#ProductId").val() },
            success: function(data) {
                $("#ReasonCodeTypeId").get(0).options.length = 0;
                $("#ReasonCodeTypeId").get(0).options[0] = new Option("Please Select...", "");

                $.each(data, function(index, item) {
                $("#ReasonCodeTypeId").get(0).options[$("#ReasonCodeTypeId").get(0).options.length] = new Option(item.ReasonCodeTypeDescription, item.ReasonCodeTypeId);
                });
            },
            dataType: "json",
            async: false
        });
    });
});


//Submit Form Validation
$('#form0').submit(function() {

    var valid = false;
    jQuery.ajax({
        type: "POST",
        url: "/ReasonCodeItem.mvc/IsValidReasonCodeProductType",
        data: { reasonCode: $("#ReasonCode").val(), productId: $("#ProductId").val(), reasonCodeTypeId: $("#ReasonCodeTypeId").val() },
        success: function(data) {

            if (!jQuery.isEmptyObject(data)) {
                return false;
            } else {
                return false;
            }
        },
        dataType: "json",
        async: false
    });

    if (validItem) {
        return true;
    } else {
        $("#ReasonCodeTypeId_validationMessage").removeClass('field-validation-valid');
        $("#ReasonCodeTypeId_validationMessage").addClass('field-validation-error');
        $("#ReasonCodeTypeId_validationMessage").text("This is not a valid entry.");
        return false
    };
});
