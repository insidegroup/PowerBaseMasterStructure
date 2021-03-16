$(document).ready(function() {

     $('#menu_policies').click();
     $("tr:odd").addClass("row_odd");
     $("tr:even").addClass("row_even");

     //for pages with long breadcrumb and no search box
     $('#breadcrumb').css('width', '725px');
     $('#search').css('width', '5px');

     /////////////////////////////////////////////////////////
     // BEGIN PRODUCT/SUPPLIER SETUP
     /////////////////////////////////////////////////////////
     if ($("#ProductId").val() == "") {
         $("#SupplierName").attr("disabled", true);
     } else {
         $("#SupplierName").removeAttr("disabled");
     }

     $("#ProductId").change(function () {
         $("#SupplierName").val("");
         $("#SupplierCode").val("");
         if ($("#ProductId").val() == "") {
             $("#SupplierName").attr("disabled", true);
         } else {
             $("#SupplierName").removeAttr("disabled");
         }
     });

     $("#SupplierName").change(function () {
         if ($("#SupplierName").val() == "") {
             $("#SupplierCode").val("");
         }
     });

     /////////////////////////////////////////////////////////
     // END PRODUCT/SUPPLIER SETUP
     /////////////////////////////////////////////////////////
     
    $('#ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
});


/////////////////////////////////////////////////////////
// BEGIN PRODUCT/SUPPLIER AUTOCOMPLETE
/////////////////////////////////////////////////////////
$(function() {
    $("#SupplierName").autocomplete({

        source: function(request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                data: { searchText: request.term, productId: $("#ProductId").val() },
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
        mustMatch: true,
        select: function(event, ui) {
            $("#SupplierName").val(ui.item.value);
            $("#SupplierCode").val(ui.item.id);
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

    if ($("#SupplierName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierName",
            data: { supplierName: $("#SupplierName").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validSupplier = true;

                    //user may not use auto, need to populate ID field
                    $("#SupplierCode").val(data[0].SupplierCode);
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
            url: "/Validation.mvc/IsValidSupplierProduct",
            data: { supplierCode: $("#SupplierCode").val(), productId: $("#ProductId").val() },
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
