$(document).ready(function() {
    //Navigation
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');


    //Show DatePickers
    $('#PolicyMessageGroupItemHotel_ExpiryDate').datepicker({
            constrainInput: true,
            buttonImageOnly: true,
            showOn: 'button',
            buttonImage: '/Images/Common/Calendar.png',
            dateFormat: 'M dd yy',
            duration: 0
    });
    $('#PolicyMessageGroupItemHotel_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyMessageGroupItemHotel_TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyMessageGroupItemHotel_TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#PolicyMessageGroupItemHotel_EnabledDate').val() == "") {
        $('#PolicyMessageGroupItemHotel_EnabledDate').val("No Enabled Date")
    }
    if ($('#PolicyMessageGroupItemHotel_ExpiryDate').val() == "") {
        $('#PolicyMessageGroupItemHotel_ExpiryDate').val("No Expiry Date")
    }
    if ($('#PolicyMessageGroupItemHotel_TravelDateValidFrom').val() == "") {
        $('#PolicyMessageGroupItemHotel_TravelDateValidFrom').val("No Travel Date Valid From")
    }
    if ($('#PolicyMessageGroupItemHotel_TravelDateValidTo').val() == "") {
        $('#PolicyMessageGroupItemHotel_TravelDateValidTo').val("No Travel Date Valid To")
    }

    $(function () {
        $("#PolicyMessageGroupItemHotel_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#PolicyMessageGroupItemHotel_ProductId").val() },
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
                $("#PolicyMessageGroupItemHotel_SupplierName").val(ui.item.value);
                $("#PolicyMessageGroupItemHotel_SupplierCode").val(ui.item.id);
                $("#lblSupplierNameMsg").text("");

            }
        });

    });
    //Submit Form Validation
    $('#form0').submit(function () {
        if ($('#PolicyMessageGroupItemHotel_EnabledDate').val() == "No Enabled Date") {
            $('#PolicyMessageGroupItemHotel_EnabledDate').val("");
        }
        if ($('#PolicyMessageGroupItemHotel_ExpiryDate').val() == "No Expiry Date") {
            $('#PolicyMessageGroupItemHotel_ExpiryDate').val("");
        }
        if ($('#PolicyMessageGroupItemHotel_TravelDateValidFrom').val() == "No Travel Date Valid From") {
            $('#PolicyMessageGroupItemHotel_TravelDateValidFrom').val("");
        }
        if ($('#PolicyMessageGroupItemHotel_TravelDateValidTo').val() == "No Travel Date Valid To") {
            $('#PolicyMessageGroupItemHotel_TravelDateValidTo').val("");
        }
    });


});
