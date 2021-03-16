/*
OnReady
*/
$(document).ready(function() {
    
    //Navigation
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#ClientDetail_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#ClientDetail_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });


    if ($('#ClientDetail_EnabledDate').val() == "") {
        $('#ClientDetail_EnabledDate').val("No Enabled Date");
    }
    if ($('#ClientDetail_ExpiryDate').val() == "") {
        $('#ClientDetail_ExpiryDate').val("No Expiry Date");
    }
});
    /*
    Submit Form Validation
    */
$('#form0').submit(function() {

    if (!$(this).valid()) {
        return false;
    }

    if ($('#ClientDetail_EnabledDate').val() == "No Enabled Date") {
        $('#ClientDetail_EnabledDate').val("");
        }
    if ($('#ClientDetail_ExpiryDate').val() == "No Expiry Date") {
        $('#ClientDetail_ExpiryDate').val("");
        }
});
