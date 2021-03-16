

$(document).ready(function() {
    $('#menu_clientdetails').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

});


/*
    $("#SubProductName").change(function() {
        if ($("#SubProductName").val() == "") {
            $("#SubProductId").val("");
        }
    });
    $("#FormOfPaymentTypeDescription").change(function() {
    if ($("#FormOfPaymentTypeDescription").val() == "") {
        $("#FormOfPaymentTypeId").val("");
        }
    });

$(function() {
$("#FormOfPaymentTypeDescription").autocomplete({

        source: function(request, response) {
            $.ajax({
            url: "/FormOfPaymentType.mvc/AutoCompleteFormOfPaymentTypes", type: "POST", dataType: "json",
                data: { searchText: request.term, maxResults: "10" },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                        id: item.FormOfPaymentTypeId,
                        value: item.FormOfPaymentTypeDescription
                        }
                    }))
                }
            })
        },
        minLength: 2,
        mustMatch: true,
        select: function(event, ui) {
        $("#FormOfPaymentTypeDescription").val(ui.item.value);
        $("#FormOfPaymentTypeId").val(ui.item.id);
        $("#lblFormOfPaymentTypeDescriptionMsg").text("");
        }

    });
    $("#SubProductName").autocomplete({
        source: function(request, response) {
            $.ajax({
            url: "/SubProduct.mvc/AutoCompleteSubProducts", type: "POST", dataType: "json",
                data: { searchText: request.term, maxResults: "10"},
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                        id: item.SubProductId,
                        value: item.SubProductName
                        }
                    }))
                }
            })
        },
        mustMatch: true,
        select: function(event, ui) {
        $("#SubProductName").val(ui.item.value);
        $("#SubProductId").val(ui.item.id);
        $("#lblSubProductNameMsg").text("");
        }
    });
});


$('#form0').submit(function() {

    var validSubProduct = false;
    var validFormOfPaymentType = false;

    jQuery.ajax({
        type: "POST",
        url: "/SubProduct.mvc/IsValidSubProduct",
        data: { searchText: $("#SubProductName").val() },
        success: function(data) {

            if (!jQuery.isEmptyObject(data)) {
                validSubProduct = true;
            }
        },
        dataType: "json",
        async: false
    });

    if (!validSubProduct) {
        $("#ubProductId").val("");
        $("#lblSubProductNameMsg").removeClass('field-validation-valid');
        $("#lblSubProductNameMsg").addClass('field-validation-error');
        $("#lblSubProductNameMsg").text("This is not a valid SubProduct.");
        return false;
    } else {
        $("#lblSupplierNameMsg").text("");
    }


    jQuery.ajax({
        type: "POST",
        url: "/FormOfPaymentType.mvc/IsValidFormOfPaymentType",
        data: { searchText: $("#FormOfPaymentTypeDescription").val() },
        success: function(data) {

            if (!jQuery.isEmptyObject(data)) {
                validFormOfPaymentType = true;
            }
        },
        dataType: "json",
        async: false
    });
    if (!validFormOfPaymentType) {
        $("#ValidFormOfPaymentTypeId").val("");
        $("#lblFormOfPaymentTypeDescriptionMsg").removeClass('field-validation-valid');
        $("#lblFormOfPaymentTypeDescriptionMsg").addClass('field-validation-error');
        $("#lblFormOfPaymentTypeDescriptionMsg").text("This is not a valid FormOfPaymentType.");
    } else {
    $("#lblFormOfPaymentTypeDescriptionMsg").text("");
    }

    if (validFormOfPaymentType && validSubProduct) {
        return true;
    } else {
        return false
    };
});
*/