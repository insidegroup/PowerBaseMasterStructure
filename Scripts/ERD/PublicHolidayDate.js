

$(document).ready(function() {
    $('#menu_publicholidays').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#PublicHolidayDate1').datepicker({
        onSelect: function(date) {
        $("#PublicHolidayDate1_validationMessage").text("");
        },
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
});




