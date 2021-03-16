

$(document).ready(function () {
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    $('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val() == "") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val("No Enabled Date");
    }
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val() == "") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val("No Expiry Date");
    }
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val() == "") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val("No Travel Date Valid From");
    }
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val() == "") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val("No Travel Date Valid To");
    }
});


$(function () {
    //Submit Form Validation
    $('#form0').submit(function () {

        if ($('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val() == "No Enabled Date") {
            $('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val("");
        }
        if ($('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val() == "No Expiry Date") {
            $('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val("");
        }

        if ($('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val() == "No Travel Date Valid From") {
            $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val("");
        }
        if ($('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val() == "No Travel Date Valid To") {
            $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val("");
        }    
    });
});