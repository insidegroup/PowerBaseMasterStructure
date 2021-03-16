$(document).ready(function() {
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    
    //Show DatePicker
    $('#StartDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    $("#OriginCountryCode").change(function() {
        $("#lblOriginCountryCode").text("");
        $("#lblDestinationCountryCode").text("");
    });
    $("#DestinationCountryCode").change(function() {
        $("#lblOriginCountryCode").text("");
        $("#lblDestinationCountryCode").text("");
    });
})


$('#form0').submit(function() {

    //if coutries have not changed, form is ok
    var valid = false;
    if (($("#OriginalOCC").val() == $("#OriginCountryCode").val()) && ($("#OriginalDCC").val() == $("#DestinationCountryCode").val())) {
        return true;
    }

    //if countries have changed, we need to prevent user from creating a  duplicate
    jQuery.ajax({
        type: "POST",
        url: "/Validation.mvc/IsValidAPISCountries",
        data: { occ: $("#OriginCountryCode").val(), dcc: $("#DestinationCountryCode").val() },
        success: function(data) {
            valid = data;
        },
        dataType: "json",
        async: false
    });

    if (valid) {
        $("#lblOriginCountryCode").text("");
        $("#lblDestinationCountryCode").text("");
        return true;
    } else {
        $("#lblOriginCountryCode").addClass('field-validation-error');
        $("#lblOriginCountryCode").text("This APISCountry pair already exists");
        $("#lblDestinationCountryCode").addClass('field-validation-error');
        $("#lblDestinationCountryCode").text("This APISCountry pair already exists");
        return false;
    };
});