$(document).ready(function() {
	$('#menu_localoperatinghours').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	//Time Picker
    $('#OpeningTime, #ClosingTime').calendricalTimeRange();

    //$('#OpeningTime, #ClosingTime').timepicker({
	//	interval: 15,
	//	dynamic: true,
	//	dropdown: true,
	//	timeFormat: 'hh:mm:ss'
	//});
});