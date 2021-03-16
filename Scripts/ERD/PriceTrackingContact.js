$(document).ready(function () {
    $('#menu_pricetracking').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    $('#PriceTrackingContact_PriceTrackingDashboardAccessFlagSelectedValue').change(function () {

        var selectedValue = $(this).find('option:selected').val();

        if (selectedValue == "false" || selectedValue == "true") {
            $('#PriceTrackingContact_PriceTrackingDashboardAccessFlag').val(selectedValue);
        } else {
            $('#PriceTrackingContact_PriceTrackingDashboardAccessFlag').val("");
        }

    });
});


$('#form0').submit(function () {

	
});