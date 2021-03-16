$(document).ready(function () {

	$('#menu_passivesegmentbuilder').click();
	$('#menu_passivesegmentbuilder_optionalfields').click();

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	//for pages with long breadcrumb and no search box
	$('#breadcrumb').css('width', '725px');
	$('#search').css('width', '5px');

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#OptionalFieldItem_ProductId").val() == "") {
        $("#OptionalFieldItem_SupplierName").attr("disabled", true);
    } else {
        $("#OptionalFieldItem_SupplierName").removeAttr("disabled");
    }

    $("#OptionalFieldItem_ProductId").change(function () {
    	$("#OptionalFieldItem_SupplierName").val("");
    	$("#OptionalFieldItem_SupplierCode").val("");
        $("#lblAuto").text("");
     
        if ($("#OptionalFieldItem_ProductId").val() == "") {
            $("#OptionalFieldItem_SupplierName").attr("disabled", true);
        } else {
            $("#OptionalFieldItem_SupplierName").removeAttr("disabled");
        }
    });

    $("#OptionalFieldItem_SupplierName").change(function () {
        if ($("#OptionalFieldItem_SupplierName").val() == "") {
            $("#OptionalFieldItem_SupplierCode").val("");
            $("#SupplierCode").val("");
        }
    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////



    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////
    $(function () {
        $("#OptionalFieldItem_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#OptionalFieldItem_ProductId").val() },
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
                $("#OptionalFieldItem_SupplierName").val(ui.item.value);
                $("#OptionalFieldItem_SupplierCode").val(ui.item.id);
                $("#lblSupplierNameMsg").text("");
            }
        });

    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////

    $('#form0').submit(function () {

		/////////////////////////////////////////////////////////
        // BEGIN PRODUCT/SUPPLIER VALIDATION
		// Supplier not Mandatory
        // 1. Check Text for Supplier to see if valid
        // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
        /////////////////////////////////////////////////////////
    	var validChars = true;
    	var allowedChars = /^([\w\s-()*]+)$/;
    	var validSupplier = false;
        var validSupplierProduct = false;
        var supplierProvided = ($("#OptionalFieldItem_SupplierName").val() != '')
        var productProvided = ($("#OptionalFieldItem_ProductId").val() != '');

        if (supplierProvided) {

            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierName",
                data: { supplierName: $("#OptionalFieldItem_SupplierName").val() },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validSupplier = true;

                        //user may not use auto, need to populate ID field
                        if (data.length > 1) {
                            var productId = Number($("#OptionalFieldItem_ProductId").val());
                            
                            for (var i = 0; i < data.length; i++) {
                                if(data[i].ProductId == productId) {
                                    $("#OptionalFieldItem_SupplierCode").val(data[i].SupplierCode);
                                    break;
                                }
                            }
                        }
                        else {
                        $("#OptionalFieldItem_SupplierCode").val(data[0].SupplierCode);
                        }
                        
                    }
                },
                dataType: "json",
                async: false
            });

            if (!validSupplier) {
                $("#OptionalFieldItem_SupplierCode").val("");
                $("#lblSupplierNameMsg").removeClass('field-validation-valid');
                $("#lblSupplierNameMsg").addClass('field-validation-error');
                $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            } else {
                $("#lblSupplierNameMsg").text("");
            }

            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierProduct",
                data: { supplierCode: $("#OptionalFieldItem_SupplierCode").val(), productId: $("#OptionalFieldItem_ProductId").val() },
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
            } else {
                $("#lblSupplierNameMsg").text("");
            }
        }

        /////////////////////////////////////////////////////////
        // END PRODUCT/SUPPLIER VALIDATION
        /////////////////////////////////////////////////////////

        if ((supplierProvided && !validSupplier) || !validChars || !productProvided || (supplierProvided && !validSupplierProduct)) {
            return false;
        }else{
            return true;
        }
    });
});