$(document).ready(function () {

    //Navigation
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    //Show DatePickers
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

    if ($('#ExpiryDate').val() == "") {
        $('#ExpiryDate').val("No Expiry Date");
    }
    if ($('#EnabledDate').val() == "") {
        $('#EnabledDate').val("No Enabled Date");
    }
    if ($('#TravelDateValidFrom').val() == "") {
        $('#TravelDateValidFrom').val("No From Travel Date");
    }
    if ($('#TravelDateValidTo').val() == "") {
        $('#TravelDateValidTo').val("No To Travel Date");
    }

});


/*
Submit Form Validation
*/
$('#form0').submit(function () {

    if ($('#EnabledDate').val() == "No Enabled Date") {
        $('#EnabledDate').val("");
    }
    if ($('#ExpiryDate').val() == "No Expiry Date") {
        $('#ExpiryDate').val("");
    }
    if ($('#TravelDateValidFrom').val() == "No From Travel Date") {
        $('#TravelDateValidFrom').val("");
    }
    if ($('#TravelDateValidTo').val() == "No To Travel Date") {
        $('#TravelDateValidTo').val("");
    }

});