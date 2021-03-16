$(document).ready(function() {
    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#AirlineClassCabin_ProductId").val() == "") {
        $("#AirlineClassCabin_SupplierName").attr("disabled", true);
    } else {
        $("#AirlineClassCabin_SupplierName").removeAttr("disabled");
    }

    $("#AirlineClassCabin_ProductId").change(function () {
        $("#AirlineClassCabin_SupplierName").val("");
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");

        if ($("#AirlineClassCabin_ProductId").val() == "") {
            $("#AirlineClassCabin_SupplierName").attr("disabled", true);
        } else {
            $("#AirlineClassCabin_SupplierName").removeAttr("disabled");
        }
    });

    $("#AirlineClassCabin_SupplierName").change(function () {
        if ($("#AirlineClassCabin_SupplierName").val() == "") {
            $("#AirlineClassCabin_SupplierCode").val("");
            $("#SupplierCode").val("");
        }
    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////

})


/////////////////////////////////////////////////////////
// BEGIN PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
/////////////////////////////////////////////////////////
$(function () {
    $("#AirlineClassCabin_SupplierName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                data: { searchText: request.term, productId: $("#AirlineClassCabin_ProductId").val() },
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
            $("#AirlineClassCabin_SupplierName").val(ui.item.value);
            $("#AirlineClassCabin_SupplierCode").val(ui.item.id);
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



$('#form0').submit(function() {

    //edit item, no need to check product+supplier
    if ($("#AirlineClassCabin_VersionNumber").val() != "") {
        return true;
    }
    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER VALIDATION
    // 1. Check Text for Supplier to see if valid
    // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
    /////////////////////////////////////////////////////////
    var validSupplier = false;
    var validSupplierProduct = false;

    if ($("#AirlineClassCabin_SupplierName").val()) {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierName",
            data: { supplierName: $("#AirlineClassCabin_SupplierName").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validSupplier = true;

                    //user may not use auto, need to populate ID field
                    $("#AirlineClassCabin_SupplierCode").val(data[0].SupplierCode);
                }
            },
            dataType: "json",
            async: false
        });

        if (!validSupplier) {
            $("#AirlineClassCabin_SupplierCode").val("");
            $("#lblSupplierNameMsg").removeClass('field-validation-valid');
            $("#lblSupplierNameMsg").addClass('field-validation-error');
            $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            return false;
        } else {
            $("#lblSupplierNameMsg").text("");
        }

        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierProduct",
            data: { supplierCode: $("#AirlineClassCabin_SupplierCode").val(), productId: $("#AirlineClassCabin_ProductId").val() },
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