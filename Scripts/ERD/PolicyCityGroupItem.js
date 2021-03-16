

$(document).ready(function () {
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    $('#PolicyCityGroupItem_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyCityGroupItem_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyCityGroupItem_TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyCityGroupItem_TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#PolicyCityGroupItem_EnabledDate').val() == "") {
        $('#PolicyCityGroupItem_EnabledDate').val("No Enabled Date");
    }
    if ($('#PolicyCityGroupItem_ExpiryDate').val() == "") {
        $('#PolicyCityGroupItem_ExpiryDate').val("No Expiry Date");
    }
    if ($('#PolicyCityGroupItem_TravelDateValidFrom').val() == "") {
        $('#PolicyCityGroupItem_TravelDateValidFrom').val("No Travel Date Valid From");
    }
    if ($('#PolicyCityGroupItem_TravelDateValidTo').val() == "") {
        $('#PolicyCityGroupItem_TravelDateValidTo').val("No Travel Date Valid To");
    }
});


$(function () {
    $("#CityCode_validationMessage").text("");
    $("#CityName").autocomplete({

        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/Cities", type: "POST", dataType: "json",
                data: { searchText: request.term, maxResults: 10 },
                success: function (data) {

                    response($.map(data, function (item) {
                        return {
                            label: item.Name + " (" + item.CityCode + ")",
                            value: item.Name,
                            name: item.CityCode
                        }
                    }))
                }
            })
        },
        select: function (event, ui) {
            $("#CityName").val(ui.item.value); //label
            $("#PolicyCityGroupItem_CityCode").val(ui.item.name); //hidden
        }
    });





    //Submit Form Validation
    $('#form0').submit(function () {


        var validCity = true;


        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidCityCode",
            data: { cityCode: $("#PolicyCityGroupItem_CityCode").val() },
            success: function (data) {

                if (jQuery.isEmptyObject(data)) {
                    validCity = false;
                }
            },
            dataType: "json",
            async: false
        });

        
        if (!validCity) {
            $("#PolicyCityGroupItem_CityCode_validationMessage").removeClass('field-validation-valid');
            $("#PolicyCityGroupItem_CityCode_validationMessage").addClass('field-validation-error');
            $("#PolicyCityGroupItem_CityCode_validationMessage").text("This is not a valid City.");
            return false;
        } else {

            if ($('#PolicyCityGroupItem_EnabledDate').val() == "No Enabled Date") {
                $('#PolicyCityGroupItem_EnabledDate').val("");
            }
            if ($('#PolicyCityGroupItem_ExpiryDate').val() == "No Expiry Date") {
                $('#PolicyCityGroupItem_ExpiryDate').val("");
            }

            if ($('#PolicyCityGroupItem_TravelDateValidFrom').val() == "No Travel Date Valid From") {
                $('#PolicyCityGroupItem_TravelDateValidFrom').val("");
            }
            if ($('#PolicyCityGroupItem_TravelDateValidTo').val() == "No Travel Date Valid To") {
                $('#PolicyCityGroupItem_TravelDateValidTo').val("");
            }

            $("#PolicyCityGroupItem_CityCode").val($("#CityCode").val().toUpperCase())
            $("#PolicyCityGroupItem_CityCode_validationMessage").text("");
        }

       
    });
});