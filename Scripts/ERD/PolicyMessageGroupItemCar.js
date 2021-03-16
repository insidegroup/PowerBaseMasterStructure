$(document).ready(function() {
    //Navigation
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');


    //Show DatePickers
    $('#PolicyMessageGroupItemCar_ExpiryDate').datepicker({
            constrainInput: true,
            buttonImageOnly: true,
            showOn: 'button',
            buttonImage: '/Images/Common/Calendar.png',
            dateFormat: 'M dd yy',
            duration: 0
    });
    $('#PolicyMessageGroupItemCar_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyMessageGroupItemCar_TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyMessageGroupItemCar_TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#PolicyMessageGroupItemCar_EnabledDate').val() == "") {
        $('#PolicyMessageGroupItemCar_EnabledDate').val("No Enabled Date")
    }
    if ($('#PolicyMessageGroupItemCar_ExpiryDate').val() == "") {
        $('#PolicyMessageGroupItemCar_ExpiryDate').val("No Expiry Date")
    }
    if ($('#PolicyMessageGroupItemCar_TravelDateValidFrom').val() == "") {
        $('#PolicyMessageGroupItemCar_TravelDateValidFrom').val("No Travel Date Valid From")
    }
    if ($('#PolicyMessageGroupItemCar_TravelDateValidTo').val() == "") {
        $('#PolicyMessageGroupItemCar_TravelDateValidTo').val("No Travel Date Valid To")
    }
    $(function () {
        $("#PolicyMessageGroupItemCar_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#PolicyMessageGroupItemCar_ProductId").val() },
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
                $("#PolicyMessageGroupItemCar_SupplierName").val(ui.item.value);
                $("#PolicyMessageGroupItemCar_SupplierCode").val(ui.item.id);
                $("#lblSupplierNameMsg").text("");

            }
        });

    });
    //Submit Form Validation
    $('#form0').submit(function () {
        if ($('#PolicyMessageGroupItemCar_EnabledDate').val() == "No Enabled Date") {
            $('#PolicyMessageGroupItemCar_EnabledDate').val("");
        }
        if ($('#PolicyMessageGroupItemCar_ExpiryDate').val() == "No Expiry Date") {
            $('#PolicyMessageGroupItemCar_ExpiryDate').val("");
        }
        if ($('#PolicyMessageGroupItemCar_TravelDateValidFrom').val() == "No Travel Date Valid From") {
            $('#PolicyMessageGroupItemCar_TravelDateValidFrom').val("");
        }
        if ($('#PolicyMessageGroupItemCar_TravelDateValidTo').val() == "No Travel Date Valid To") {
            $('#PolicyMessageGroupItemCar_TravelDateValidTo').val("");
        }
    });


});
