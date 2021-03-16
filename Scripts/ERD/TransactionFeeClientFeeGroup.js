/*
OnReady
*/
$(document).ready(function () {

    if ($("#TransactionFee_ProductId").val() == "1") {
        $('#PolicyLocationNameDiv').css('display', 'none');
        $('#PolicyRouting_Header_Div').css('display', '');
        $('#PolicyRouting_Name_Div').css('display', '');
        $('#PolicyRouting_FromGlobalFlag_Div').css('display', '');
        $('#PolicyRouting_FromCode_Div').css('display', '');
        $('#PolicyRouting_ToGlobalFlag_Div').css('display', '');
        $('#PolicyRouting_ToCode_Div').css('display', '');
        $('#PolicyRouting_RoutingViceVersaFlag_Div').css('display', '');
    } else {
        $('#PolicyLocationNameDiv').css('display', '');
        $('#PolicyRouting_Header_Div').css('display', 'none');
        $('#PolicyRouting_Name_Div').css('display', 'none');
        $('#PolicyRouting_FromGlobalFlag_Div').css('display', 'none');
        $('#PolicyRouting_FromCode_Div').css('display', 'none');
        $('#PolicyRouting_ToGlobalFlag_Div').css('display', 'none');
        $('#PolicyRouting_ToCode_Div').css('display', 'none');
        $('#PolicyRouting_RoutingViceVersaFlag_Div').css('display', 'none');
        
       
    }
    var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    
    if ($("#TransactionFeeClientFeeGroup_ProductId").val() == "1") {
        jQuery.ajax({
            url: "/Validation.mvc/IsTransactionFeeAir", type: "POST", dataType: "json",
            data: { transactionFeeId: $("#TransactionFeeClientFeeGroup_TransactionFeeId").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    $("#ProductName").text(data.TransactionFee.ProductName);
                    $("#TransactionFeeDescription").text(data.TransactionFee.TransactionFeeDescription);
                    $("#TravelIndicatorDescription").text(data.TransactionFee.TravelIndicatorDescription);
                    $("#BookingSourceDescription").text(data.TransactionFee.BookingSourceDescription);
                    $("#BookingOriginationDescription").text(data.TransactionFee.BookingOriginationDescription);
                    $("#ChargeTypeDescription").text(data.TransactionFee.ChargeTypeDescription);
                    $("#TransactionTypeCode").text(data.TransactionFee.TransactionTypeCode);
                    $("#FeeCategory").text(data.TransactionFee.FeeCategory);
                    $("#TravelerClassCode").text(data.TransactionFee.TravelerClassCode);
                    $("#SupplierName").text(data.TransactionFee.SupplierName);
                    $("#MinimumFeeCategoryQuantity").text(data.TransactionFee.MinimumFeeCategoryQuantity);
                    $("#MaximumFeeCategoryQuantity").text(data.TransactionFee.MaximumFeeCategoryQuantity);
                    $("#MinimumTicketPrice").text(data.TransactionFee.MinimumTicketPrice);
                    $("#MaximumTicketPrice").text(data.TransactionFee.MaximumTicketPrice);
                    $("#TicketPriceCurrencyCode").text(data.TransactionFee.TicketPriceCurrencyCode);
                    $("#IncursGSTFlag").text(data.TransactionFee.IncursGSTFlag);
                    $("#TripTypeClassificationDescription").text(data.TransactionFee.TripTypeClassificationDescription);

                    if (data.TransactionFee.EnabledDate != null) {
                        var parsedDate = new Date(parseInt(data.TransactionFee.EnabledDate.substr(6)));
                        var jsDate = new Date(parsedDate); //Date object
                        $("#EnabledDate").text(jsDate.getDay() + " " + months[jsDate.getMonth()] + " " + jsDate.getFullYear());
                    }
                    if (data.TransactionFee.ExpiryDate != null) {
                        var parsedDate = new Date(parseInt(data.TransactionFee.ExpiryDate.substr(6)));
                        var jsDate = new Date(parsedDate); //Date object
                        $("#ExpiryDate").text(jsDate.getDay() + " " + months[jsDate.getMonth()] + " " + jsDate.getFullYear());
                    }

                    $("#FeeAmount").text(data.TransactionFee.FeeAmount);
                    $("#FeePercent").text(data.TransactionFee.FeePercent);
                    $("#FeeCurrencyCode").text(data.TransactionFee.FeeCurrencyCode);
                    $("#Name").text(data.Name);
                    $("#FromGlobalFlag").text(data.FromGlobalFlag);
                    $("#FromCode").text(data.FromCode);
                    $("#ToGlobalFlag").text(data.ToGlobalFlag);
                    $("#ToCode").text(data.ToCode);
                    $("#RoutingViceVersaFlag").text(data.RoutingViceVersaFlag);
                }
            },
            async: false
        })
    }


    if ($("#TransactionFeeClientFeeGroup_ProductId").val() == "2" || $("#TransactionFeeClientFeeGroup_ProductId").val() == "3") {
        jQuery.ajax({
            url: "/Validation.mvc/IsTransactionFeeCarHotel", type: "POST", dataType: "json",
            data: { transactionFeeId: $("#TransactionFeeClientFeeGroup_TransactionFeeId").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    $("#ProductName").text(data.ProductName);
                    $("#TransactionFeeDescription").text(data.TransactionFeeDescription);
                    $("#PolicyLocationName").text(data.PolicyLocationName);
                    $("#TravelIndicatorDescription").text(data.TravelIndicatorDescription);
                    $("#BookingSourceDescription").text(data.BookingSourceDescription);
                    $("#BookingOriginationDescription").text(data.BookingOriginationDescription);
                    $("#ChargeTypeDescription").text(data.ChargeTypeDescription);
                    $("#TransactionTypeCode").text(data.TransactionTypeCode);
                    $("#FeeCategory").text(data.FeeCategory);
                    $("#TravelerClassCode").text(data.TravelerClassCode);
                    $("#SupplierName").text(data.SupplierName);
                    $("#MinimumFeeCategoryQuantity").text(data.MinimumFeeCategoryQuantity);
                    $("#MaximumFeeCategoryQuantity").text(data.MaximumFeeCategoryQuantity);
                    $("#MinimumTicketPrice").text(data.MinimumTicketPrice);
                    $("#MaximumTicketPrice").text(data.MaximumTicketPrice);
                    $("#TicketPriceCurrencyCode").text(data.TicketPriceCurrencyCode);
                    $("#IncursGSTFlag").text(data.IncursGSTFlag);
                    $("#TripTypeClassificationDescription").text(data.TripTypeClassificationDescription);
                    $("#EnabledDate").text(data.EnabledDate);
                    $("#ExpiryDate").text(data.ExpiryDate);
                    $("#FeeAmount").text(data.FeeAmount);
                    $("#FeePercent").text(data.FeePercent);
                    $("#FeeCurrencyCode").text(data.FeeCurrencyCode);
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

    //when Product is changed
    $("#TransactionFee_ProductId").change(function () {

        $("#TransactionFeeClientFeeGroup_ProductId").val($("#TransactionFee_ProductId").val())
        $("#TransactionFeeClientFeeGroup_TransactionFeeId").find('option').remove();
        $("<option value=''>Please Select...</option>").appendTo($("#TransactionFeeClientFeeGroup_TransactionFeeId"));

        $("#ProductName").text("");
        $("#TransactionFeeDescription").text("");
        $("#PolicyLocationName").text("");
        $("#TravelIndicatorDescription").text("");
        $("#BookingSourceDescription").text("");
        $("#BookingOriginationDescription").text("");
        $("#ChargeTypeDescription").text("");
        $("#TransactionTypeCode").text("");
        $("#FeeCategory").text("");
        $("#TravelerClassCode").text("");
        $("#SupplierName").text("");
        $("#MinimumFeeCategoryQuantity").text("");
        $("#MaximumFeeCategoryQuantity").text("");
        $("#MinimumTicketPrice").text("");
        $("#MaximumTicketPrice").text("");
        $("#TicketPriceCurrencyCode").text("");
        $("#IncursGSTFlag").text("");
        $("#TripTypeClassificationDescription").text("");
        $("#EnabledDate").text("");
        $("#ExpiryDate").text("");
        $("#FeeAmount").text("");
        $("#FeePercent").text("");
        $("#FeeCurrencyCode").text("");
        $("#Name").text("");
        $("#FromGlobalFlag").text("");
        $("#FromCode").text("");
        $("#ToGlobalFlag").text("");
        $("#ToCode").text("");
        $("#RoutingViceVersaFlag").text("");


        if ($("#TransactionFee_ProductId").val() != "") {

            jQuery.ajax({
                url: "/TransactionFeeClientFeeGroup.mvc/GetUnUsedTransactionFees", type: "POST", dataType: "json",
                data: {
                    clientFeeGroupId: $("#TransactionFeeClientFeeGroup_ClientFeeGroupId").val(),
                    productid: $("#TransactionFeeClientFeeGroup_ProductId").val(),
                    transactionFeeId: $("#TransactionFeeClientFeeGroup_TransactionFeeId").val()
                },
                success: function (data) {
                    $("#TransactionFeeClientFeeGroup_TransactionFeeId").find('option').remove();
                    $("<option value=''>Please Select...</option>").appendTo($("#TransactionFeeClientFeeGroup_TransactionFeeId"));
                    $(data).each(function () {
                        $("<option value=" + this.TransactionFeeId + ">" + this.TransactionFeeDescription + "</option>").appendTo($("#TransactionFeeClientFeeGroup_TransactionFeeId"));

                    })
                },
                async: false
            })
        }

    });

    //when feedescriptin is changed
    $("#TransactionFeeClientFeeGroup_TransactionFeeId").change(function () {

        if ($("#TransactionFeeClientFeeGroup_TransactionFeeId").val() == "") {
            $("#ProductName").text("");
            $("#TransactionFeeDescription").text("");
            $("#PolicyLocationName").text("");
            $("#TravelIndicatorDescription").text("");
            $("#BookingSourceDescription").text("");
            $("#BookingOriginationDescription").text("");
            $("#ChargeTypeDescription").text("");
            $("#TransactionTypeCode").text("");
            $("#FeeCategory").text("");
            $("#TravelerClassCode").text("");
            $("#SupplierName").text("");
            $("#MinimumFeeCategoryQuantity").text("");
            $("#MaximumFeeCategoryQuantity").text("");
            $("#MinimumTicketPrice").text("");
            $("#MaximumTicketPrice").text("");
            $("#TicketPriceCurrencyCode").text("");
            $("#IncursGSTFlag").text("");
            $("#TripTypeClassificationDescription").text("");
            $("#EnabledDate").text("");
            $("#ExpiryDate").text("");
            $("#FeeAmount").text("");
            $("#FeePercent").text("");
            $("#FeeCurrencyCode").text("");
            $("#Name").text("");
            $("#FromGlobalFlag").text("");
            $("#FromCode").text("");
            $("#ToGlobalFlag").text("");
            $("#ToCode").text("");
            $("#RoutingViceVersaFlag").text("");
        } else {

            if ($("#TransactionFee_ProductId").val() == "1") {

                $('#PolicyLocationNameDiv').css('display', 'none');
                $('#PolicyRouting_Header_Div').css('display', '');
                $('#PolicyRouting_Name_Div').css('display', '');
                $('#PolicyRouting_FromGlobalFlag_Div').css('display', '');
                $('#PolicyRouting_FromCode_Div').css('display', '');
                $('#PolicyRouting_ToGlobalFlag_Div').css('display', '');
                $('#PolicyRouting_ToCode_Div').css('display', '');
                $('#PolicyRouting_RoutingViceVersaFlag_Div').css('display', '');


                jQuery.ajax({
                    url: "/Validation.mvc/IsTransactionFeeAir", type: "POST", dataType: "json",
                    data: { transactionFeeId: $("#TransactionFeeClientFeeGroup_TransactionFeeId").val() },
                    success: function (data) {
                        if (!jQuery.isEmptyObject(data)) {
                            $("#ProductName").text(data.TransactionFee.ProductName);
                            $("#TransactionFeeDescription").text(data.TransactionFee.TransactionFeeDescription);
                            $("#TravelIndicatorDescription").text(data.TransactionFee.TravelIndicatorDescription);
                            $("#BookingSourceDescription").text(data.TransactionFee.BookingSourceDescription);
                            $("#BookingOriginationDescription").text(data.TransactionFee.BookingOriginationDescription);
                            $("#ChargeTypeDescription").text(data.TransactionFee.ChargeTypeDescription);
                            $("#TransactionTypeCode").text(data.TransactionFee.TransactionTypeCode);
                            $("#FeeCategory").text(data.TransactionFee.FeeCategory);
                            $("#TravelerClassCode").text(data.TransactionFee.TravelerClassCode);
                            $("#SupplierName").text(data.TransactionFee.SupplierName);
                            $("#MinimumFeeCategoryQuantity").text(data.TransactionFee.MinimumFeeCategoryQuantity);
                            $("#MaximumFeeCategoryQuantity").text(data.TransactionFee.MaximumFeeCategoryQuantity);
                            $("#MinimumTicketPrice").text(data.TransactionFee.MinimumTicketPrice);
                            $("#MaximumTicketPrice").text(data.TransactionFee.MaximumTicketPrice);
                            $("#TicketPriceCurrencyCode").text(data.TransactionFee.TicketPriceCurrencyCode);
                            $("#IncursGSTFlag").text(data.TransactionFee.IncursGSTFlag);
                            $("#TripTypeClassificationDescription").text(data.TransactionFee.TripTypeClassificationDescription);

                            if (data.TransactionFee.EnabledDate != null) {
                                var parsedDate = new Date(parseInt(data.TransactionFee.EnabledDate.substr(6)));
                                var jsDate = new Date(parsedDate); //Date object
                                $("#EnabledDate").text(jsDate.getDay() + " " + months[jsDate.getMonth()] + " " + jsDate.getFullYear());
                            }
                            if (data.TransactionFee.ExpiryDate != null) {
                                var parsedDate = new Date(parseInt(data.TransactionFee.ExpiryDate.substr(6)));
                                var jsDate = new Date(parsedDate); //Date object
                                $("#ExpiryDate").text(jsDate.getDay() + " " + months[jsDate.getMonth()] + " " + jsDate.getFullYear());
                            }

                            $("#FeeAmount").text(data.TransactionFee.FeeAmount);
                            $("#FeePercent").text(data.TransactionFee.FeePercent);
                            $("#FeeCurrencyCode").text(data.TransactionFee.FeeCurrencyCode);
                            $("#Name").text(data.Name);
                            $("#FromGlobalFlag").text(data.FromGlobalFlag);
                            $("#FromCode").text(data.FromCode);
                            $("#ToGlobalFlag").text(data.ToGlobalFlag);
                            $("#ToCode").text(data.ToCode);
                            $("#RoutingViceVersaFlag").text(data.RoutingViceVersaFlag);
                        }

                    },
                    async: false
                })
            } else {

                $('#PolicyLocationNameDiv').css('display', '');
                $('#PolicyRouting_Header_Div').css('display', 'none');
                $('#PolicyRouting_Name_Div').css('display', 'none');
                $('#PolicyRouting_FromGlobalFlag_Div').css('display', 'none');
                $('#PolicyRouting_FromCode_Div').css('display', 'none');
                $('#PolicyRouting_ToGlobalFlag_Div').css('display', 'none');
                $('#PolicyRouting_ToCode_Div').css('display', 'none');
                $('#PolicyRouting_RoutingViceVersaFlag_Div').css('display', 'none');

                jQuery.ajax({
                    url: "/Validation.mvc/IsTransactionFeeCarHotel", type: "POST", dataType: "json",
                    data: { transactionFeeId: $("#TransactionFeeClientFeeGroup_TransactionFeeId").val() },
                    success: function (data) {
                        if (!jQuery.isEmptyObject(data)) {
                            $("#ProductName").text(data.ProductName);
                            $("#TransactionFeeDescription").text(data.TransactionFeeDescription);
                            $("#PolicyLocationName").text(data.PolicyLocationName);
                            $("#TravelIndicatorDescription").text(data.TravelIndicatorDescription);
                            $("#BookingSourceDescription").text(data.BookingSourceDescription);
                            $("#BookingOriginationDescription").text(data.BookingOriginationDescription);
                            $("#ChargeTypeDescription").text(data.ChargeTypeDescription);
                            $("#TransactionTypeCode").text(data.TransactionTypeCode);
                            $("#FeeCategory").text(data.FeeCategory);
                            $("#TravelerClassCode").text(data.TravelerClassCode);
                            $("#SupplierName").text(data.SupplierName);
                            $("#MinimumFeeCategoryQuantity").text(data.MinimumFeeCategoryQuantity);
                            $("#MaximumFeeCategoryQuantity").text(data.MaximumFeeCategoryQuantity);
                            $("#MinimumTicketPrice").text(data.MinimumTicketPrice);
                            $("#MaximumTicketPrice").text(data.MaximumTicketPrice);
                            $("#TicketPriceCurrencyCode").text(data.TicketPriceCurrencyCode);
                            $("#IncursGSTFlag").text(data.IncursGSTFlag);
                            $("#TripTypeClassificationDescription").text(data.TripTypeClassificationDescription);

                            if (data.EnabledDate != null) {
                                var parsedDate = new Date(parseInt(data.EnabledDate.substr(6)));
                                var jsDate = new Date(parsedDate); //Date object
                                $("#EnabledDate").text(jsDate.getDay() + " " + months[jsDate.getMonth()] + " " + jsDate.getFullYear());
                            }
                            if (data.ExpiryDate != null) {
                                var parsedDate = new Date(parseInt(data.ExpiryDate.substr(6)));
                                var jsDate = new Date(parsedDate); //Date object
                                $("#ExpiryDate").text(jsDate.getDay() + " " + months[jsDate.getMonth()] + " " + jsDate.getFullYear());
                            }

                            $("#FeeAmount").text(data.FeeAmount);
                            $("#FeePercent").text(data.FeePercent);
                            $("#FeeCurrencyCode").text(data.FeeCurrencyCode);
                        }

                    },
                    async: false
                })
            }
        }
    });
});

function formatJSONDate(jsonDate) {
    var newDate = dateFormat(jsonDate, "mm/dd/yyyy");
    return newDate;
}