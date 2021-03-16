
$(document).ready(function () {

    //Navigation
    $('#menu_clientfeegroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#MerchantFee_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    $('#MerchantFee_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });


    if ($('#MerchantFee_ExpiryDate').val() == "") {
        $('#MerchantFee_ExpiryDate').val("No Expiry Date")
    }
    if ($('#MerchantFee_EnabledDate').val() == "") {
        $('#MerchantFee_EnabledDate').val("No Enabled Date")
    }

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#MerchantFee_ProductId").val() == "" || $("#MerchantFee_ProductId").val() == "8") {
        $("#MerchantFee_SupplierName").attr("disabled", true);
    } else {
        $("#MerchantFee_SupplierName").removeAttr("disabled");
    }

    $("#MerchantFee_ProductId").change(function () {
        $("#MerchantFee_SupplierName").val("");
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");

        if ($("#MerchantFee_ProductId").val() == "") {
            $("#MerchantFee_SupplierName").attr("disabled", true);
        } else {
            $("#MerchantFee_SupplierName").removeAttr("disabled");
        }
    });

    $("#MerchantFee_SupplierName").change(function () {
        if ($("#MerchantFee_SupplierName").val() == "") {
            $("#MerchantFee_SupplierCode").val("");
            $("#SupplierCode").val("");
        }
    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////

    $(function () {
        $("#MerchantFee_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#MerchantFee_ProductId").val() },
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
                $("#MerchantFee_SupplierName").val(ui.item.value);
                $("#MerchantFee_SupplierCode").val(ui.item.id);
                $("#lblSupplierNameMsg").text("");
            }
        });

    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////


    $('#form0').submit(function () {

        var validEnabledDate = false;
        var validExpiryDate = false;
        var validSupplier = false;
        var validSupplierProduct = false;
        var validMerchantFeePercent = true;


        /////////////////////////////////////////////////////////
        // DATE VALIDATION
        /////////////////////////////////////////////////////////
        if ($('#MerchantFee_EnabledDate').val() == "No Enabled Date" || isValidDate($('#MerchantFee_EnabledDate').val())) {
            validEnabledDate = true;
        } else {
            $("#MerchantFee_EnabledDate_validationMessage").removeClass('field-validation-valid');
            $("#MerchantFee_EnabledDate_validationMessage").addClass('field-validation-error');
            $("#MerchantFee_EnabledDate_validationMessage").text("Date should be in format MMM dd yyyy. eg Dec 01 2014");

        }
        if ($('#MerchantFee_ExpiryDate').val() == "No Expiry Date" || isValidDate($('#MerchantFee_ExpiryDate').val())) {
            validExpiryDate = true;
        } else {
            $("#MerchantFee_ExpiryDate_validationMessage").removeClass('field-validation-valid');
            $("#MerchantFee_ExpiryDate_validationMessage").addClass('field-validation-error');
            $("#MerchantFee_ExpiryDate_validationMessage").text("Date should be in format MMM dd yyyy. eg Dec 01 2014");
        }

        /////////////////////////////////////////////////////////
        // BEGIN PRODUCT/SUPPLIER VALIDATION
        // 1. Check Text for Supplier to see if valid
        // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
        /////////////////////////////////////////////////////////


        if ($("#MerchantFee_ProductId").val()) {

        	jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidSupplierName",
                data: { supplierName: $("#MerchantFee_SupplierName").val() },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validSupplier = true;

                    	//user may not use auto, need to populate ID field
						//$("#MerchantFee_SupplierCode").val(data[0].SupplierCode);
                    }
                },
                dataType: "json",
                async: false
        	});

        	//issue where value of correct supplier code was overwritten by incorrect supplier code when 
        	//more than one supplier is returned when checking name (as product ID is different)
        	//force to use selection so supplier code is correctly set
        	if ($("#MerchantFee_SupplierCode").val() == "") {
        		$("#lblSupplierNameMsg").removeClass('field-validation-valid');
        		$("#lblSupplierNameMsg").addClass('field-validation-error');
        		$("#lblSupplierNameMsg").text("Please use typeahead box to select supplier");
        		return false;
        	}

            if (!validSupplier) {
                $("#MerchantFee_SupplierCode").val("");
                $("#lblSupplierNameMsg").removeClass('field-validation-valid');
                $("#lblSupplierNameMsg").addClass('field-validation-error');
                $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            } else {
                $("#lblSupplierNameMsg").text("");
            }

            if (validSupplier) {
                jQuery.ajax({
                    type: "POST",
                    url: "/Validation.mvc/IsValidSupplierProduct",
                    data: { supplierCode: $("#MerchantFee_SupplierCode").val(), productId: $("#MerchantFee_ProductId").val() },
                    success: function (data) {

                        if (!jQuery.isEmptyObject(data)) {
                            validSupplierProduct = true;
                        }
                    },
                    dataType: "json",
                    async: false
                });
            }
            if (!validSupplierProduct) {
                $("#lblSupplierNameMsg").removeClass('field-validation-valid');
                $("#lblSupplierNameMsg").addClass('field-validation-error');
                $("#lblSupplierNameMsg").text("This is not a valid Supplier");
                $("#MerchantFee_SupplierCode").val("");
            } else {
                $("#lblSupplierNameMsg").text("");
            }
        } else {
            validSupplier = true;
            validSupplierProduct = true;
        }


        var mfPercent = $("#MerchantFee_MerchantFeePercent").val();
        var re = /^\d{1,2}(\.(\d){0,3}){0,1}$/;
        var isMatch = !!mfPercent.match(re);
        $("#MerchantFee_MerchantFeePercent_validationMessage").text("");

        if (isNaN(parseFloat(mfPercent)) || mfPercent >= 100 || mfPercent <= 0 || !isMatch) {
            $("#MerchantFee_MerchantFeePercent_validationMessage").removeClass('field-validation-valid');
            $("#MerchantFee_MerchantFeePercent_validationMessage").addClass('field-validation-error');
            $("#MerchantFee_MerchantFeePercent_validationMessage").text("Merchant Fee Percent must be greater than zero, maximum value 99.999");
            validMerchantFeePercent = false;
        };

        if (!validSupplierProduct || !validMerchantFeePercent || !validEnabledDate || !validExpiryDate) {
            return false;
        }


        /////////////////////////////////////////////////////////
        // FORM IS VALID, CLEAR DATES AND SUBMIT
        /////////////////////////////////////////////////////////
        if ($('#MerchantFee_EnabledDate').val() == "No Enabled Date") {
            $('#MerchantFee_EnabledDate').val("");
        }
        if ($('#MerchantFee_ExpiryDate').val() == "No Expiry Date") {
            $('#MerchantFee_ExpiryDate').val("");
        }
    });

});

