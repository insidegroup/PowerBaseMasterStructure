

$(document).ready(function() {
    $('#menu_clientdetails').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    
    if ($("#SupplierName").val() == "") {
        $("#ProductName").attr("disabled", true);
    } else {
        $("#ProductName").removeAttr("disabled");
    }

    $("#SupplierName").change(function() {

        $("#ProductName").val("");
        if ($("#SupplierName").val() == "") {
            $("#ProductName").attr("disabled", true);
        } else {
            $("#ProductName").removeAttr("disabled");
        }
    });

    $("#ProductName").change(function() {

    if ($("#ProductName").val() == "") {
        $("#ProductId").val("");
        }
    });
});



$(function() {
    $("#SupplierName").autocomplete({

        source: function(request, response) {
            $.ajax({
            url: "/Supplier.mvc/AutoCompleteSuppliers", type: "POST", dataType: "json",
                data: { searchText: request.term, maxResults: "10" },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                            id: item.SupplierCode,
                            value: item.SupplierName
                        }
                    }))
                }
            })
        },
        minLength: 2,
        mustMatch: true,
        select: function(event, ui) {
            $("#SupplierName").val(ui.item.value);
            $("#SupplierCode").val(ui.item.id);
            $("#lblSupplierNameMsg").text("");
        }

    });
    $("#ProductName").autocomplete({
        source: function(request, response) {
            $.ajax({
            url: "/Supplier.mvc/AutoCompleteSupplierProducts", type: "POST", dataType: "json",
                data: { searchText: request.term, maxResults: "10", supplierCode: $("#SupplierCode").val() },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                            id: item.ProductID,
                            value: item.ProductName
                        }
                    }))
                }
            })
        },
        mustMatch: true,
        select: function(event, ui) {
            $("#ProductName").val(ui.item.value);
            $("#ProductId").val(ui.item.id);
            $("#lblProductNameMsg").text("");
        }
    });
});


$('#form0').submit(function() {

    var validSupplier = false;
    var validSupplierProduct = false;

    jQuery.ajax({
        type: "POST",
        url: "/Supplier.mvc/IsValidSupplier",
        data: { supplierName: $("#SupplierName").val() },
        success: function(data) {

            if (!jQuery.isEmptyObject(data)) {
                validSupplier = true;
            }
        },
        dataType: "json",
        async: false
    });

    if (!validSupplier) {
        $("#SupplierCode").val("");
        $("#lblSupplierNameMsg").removeClass('field-validation-valid');
        $("#lblSupplierNameMsg").addClass('field-validation-error');
        $("#lblSupplierNameMsg").text("This is not a valid Supplier");
        return false;
    } else {
        $("#lblSupplierNameMsg").text("");
    }


    jQuery.ajax({
        type: "POST",
        url: "/ClientDetailSupplierProduct.mvc/IsValidSupplierProduct",
        data: { supplierCode: $("#SupplierCode").val(), productName: $("#ProductName").val() },
        success: function(data) {

            if (!jQuery.isEmptyObject(data)) {
                validSupplierProduct = true;
            }
        },
        dataType: "json",
        async: false
    });
    if (!validSupplierProduct) {
        $("#ProductId").val("");
        $("#lblProductNameMsg").removeClass('field-validation-valid');
        $("#lblProductNameMsg").addClass('field-validation-error');
        $("#lblProductNameMsg").text("This is not a valid product.");
    } else {
        $("#lblProductNameMsg").text("");
    }

    if (validSupplierProduct && validSupplier) {
        return true;
    } else {
        return false
    };
});