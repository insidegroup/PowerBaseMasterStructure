$(document).ready(function() {
$('#menu_admin').click();
$("tr:odd").addClass("row_odd");
$("tr:even").addClass("row_even");
})


$('#form0').submit(function() {

    var valid = false;
    jQuery.ajax({
        type: "POST",
        url: "/CountryRegion.mvc/IsAvailableCountryRegion",
        data: { countryCode: $("#CountryCode").val(), countryRegionName: $("#CountryRegionName").val(), countryRegionId: $("#CountryRegionId").val() },
        success: function(data) {
            valid = data;
        },
        dataType: "json",
        async: false
    });

    if (valid) {
        $("#lblCountryRegionNameMsg").text("");
        return true;
    } else {
        $("#lblCountryRegionNameMsg").addClass('field-validation-error');
        $("#lblCountryRegionNameMsg").text("This Region already exists for this country.");
        return false;
    };
});