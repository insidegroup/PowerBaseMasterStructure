$(document).ready(function() {

    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    $('#EnabledDate').datepicker({
         constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#ExpiryDate').datepicker({
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
    if ($('#EnabledDate').val() == "") {
        $('#EnabledDate').val("No Enabled Date");
    }
    if ($('#ExpiryDate').val() == "") {
        $('#ExpiryDate').val("No Expiry Date");
    }

    $("#form0").submit(function () {
        if ($('#EnabledDate').val() == "No Enabled Date") {
            $('#EnabledDate').val("");
        }
        if ($('#ExpiryDate').val() == "No Expiry Date") {
            $('#ExpiryDate').val("");
        }
    })
});


$('#form0').submit(function() {
     var validItem = false;

    if ($("#CountryName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Hierarchy.mvc/IsValidCountry",
            data: { searchText: $("#CountryName").val() },
            success: function(data) {

                if (!jQuery.isEmptyObject(data)) {
                    validItem = true;
                }
            },
            dataType: "json",
            async: false
        });
        if (!validItem) {
                $("#lblCountryNameMsg").removeClass('field-validation-valid');
                $("#lblCountryNameMsg").addClass('field-validation-error');
                $("#lblCountryNameMsg").text("This is not a valid country.");
                return false;
            } else {
                $("#lblCountryNameMsg").text("");
                
                if ($('#ExpiryDate').val() == "No Expiry Date") {
                    $('#ExpiryDate').val("")
                }
                if ($('#EnabledDate').val() == "No Enabled Date") {
                    $('#EnabledDate').val("")
                }
                return true;
            }
        }
    

});


/////////////////////////////////////////////////////////
// BEGIN COUNTRY AUTOCOMPLETE
/////////////////////////////////////////////////////////
$(function () {
    $("#CountryName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/Countries", type: "POST", dataType: "json",
                data: { searchText: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.CountryName,
                            id: item.CountryCode
                        }
                    }))
                }
            })
        },
        mustMatch: true,
        select: function (event, ui) {
            $("#CountryCode").val(ui.item.id);
        }
    });
});
/////////////////////////////////////////////////////////
// END COUNTRY AUTOCOMPLETE
/////////////////////////////////////////////////////////
