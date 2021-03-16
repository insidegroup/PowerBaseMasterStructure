/*
OnReady
*/
$(document).ready(function () {

    if ($("#MerchantFeeClientFeeGroup_MerchantFeeId").val() != "") {
        jQuery.ajax({
            url: "/Validation.mvc/IsMerchantFee", type: "POST", dataType: "json",
            data: { merchantFeeId: $("#MerchantFeeClientFeeGroup_MerchantFeeId").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    $("#CountryName").text(data.CountryName);
                    $("#CreditCardVendor").text(data.CreditCardVendorName);
                    $("#MerchantFeePercent").text(data.MerchantFeePercent);
                    $("#ProductName").text(data.ProductName);
                    $("#SupplierName").text(data.SupplierName);
                }
            },
            async: false
        })
    }

    //Navigation
    $('#menu_clientfeegroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    //Hierarchy Disable/Enable OnChange
    $("#MerchantFeeClientFeeGroup_MerchantFeeId").change(function () {

        if ($("#MerchantFeeClientFeeGroup_MerchantFeeId").val() == "") {
            $("#CountryName").text("")
            $("#CreditCardVendor").text("")
            $("#MerchantFeePercent").text("")
            $("#ProductName").text("")
            $("#SupplierName").text("")
        } else {

            jQuery.ajax({
                url: "/Validation.mvc/IsMerchantFee", type: "POST", dataType: "json",
                data: { merchantFeeId: $("#MerchantFeeClientFeeGroup_MerchantFeeId").val() },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        $("#CountryName").text(data.CountryName);
                        $("#CreditCardVendor").text(data.CreditCardVendorName);
                        $("#MerchantFeePercent").text(data.MerchantFeePercent);
                        $("#ProductName").text(data.ProductName);
                        $("#SupplierName").text(data.SupplierName);
                    }
                },
                async: false
            })
        }

    });
});

       