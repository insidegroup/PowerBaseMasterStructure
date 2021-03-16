$(document).ready(function() {

    //Navigation
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#SupplierProduct_ProductId").val() == "") {
        $("#SupplierProduct_SupplierName").attr("disabled", true);
    } else {
        $("#SupplierProduct_SupplierName").removeAttr("disabled");
    }

    $("#SupplierProduct_ProductId").change(function () {
        $("#SupplierProduct_SupplierName").val("");
        $("#SupplierProduct_SupplierCode").val("");
        if ($("#SupplierProduct_ProductId").val() == "") {
            $("#SupplierProduct_SupplierName").attr("disabled", true);
        } else {
            $("#SupplierProduct_SupplierName").removeAttr("disabled");
        }
    });

    $("#SupplierName").change(function () {
        if ($("#SupplierProduct_SupplierName").val() == "") {
            $("#SupplierProduct_SupplierCode").val("");
        }
    });

    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////

});
/////////////////////////////////////////////////////////
// BEGIN PRODUCT/SUPPLIER AUTOCOMPLETE
/////////////////////////////////////////////////////////
$(function () {
    $("#SupplierProduct_SupplierName").autocomplete({
        search: function (event, ui) {
            $("#lblSupplierNameMsg").val("");
            $("#SupplierProduct_SupplierCode").val("");
        },
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/ClientDetailProductSuppliers", type: "POST", dataType: "json",
                data: { searchText: request.term, clientDetailId: $("#ClientDetail_ClientDetailId").val(), productId: $("#SupplierProduct_ProductId").val() },
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
            $("#SupplierProduct_SupplierName").val(ui.item.value);
            $("#SupplierProduct_SupplierCode").val(ui.item.id);
            $("#lblSupplierNameMsg").text("");
        }

    });
});
/////////////////////////////////////////////////////////
// END PRODUCT/SUPPLIER AUTOCOMPLETE
/////////////////////////////////////////////////////////

$('#form0').submit(function() {

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER VALIDATION
    // 1. Check Text for Supplier to see if valid
    // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
    /////////////////////////////////////////////////////////
    var validSupplier = false;
    var validSupplierProduct = false;

    if ($("#SupplierProduct_SupplierName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierName",
            data: { supplierName: $("#SupplierProduct_SupplierName").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validSupplier = true;

                    //user may not use auto, need to populate ID field
                    $("#SupplierProduct_SupplierCode").val(data[0].SupplierCode);
                }
            },
            dataType: "json",
            async: false
        });

        if (!validSupplier) {
            $("#SupplierProduct_SupplierCode").val("");
            $("#lblSupplierNameMsg").removeClass('field-validation-valid');
            $("#lblSupplierNameMsg").addClass('field-validation-error');
            $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            return false;
        } else {
            $("#lblSupplierNameMsg").text("");
        }

        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidClientDetailSupplierProduct",
            data: { clientDetailId: $("#ClientDetail_ClientDetailId").val(), productId: $("#SupplierProduct_ProductId").val(), supplierCode: $("#SupplierProduct_SupplierCode").val() },
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
            return false;
        } else {
            $("#lblSupplierNameMsg").text("");
            return true;
        }
    }

    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER VALIDATION
    /////////////////////////////////////////////////////////
});