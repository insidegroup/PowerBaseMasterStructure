$(document).ready(function () {
	$('#menu_teams').click();
	$('#breadcrumb').css('width', 'auto');
	$('.full-width #search_wrapper').css('height', 'auto');

	//Search
	$('#search').hide();
	$('#SearchButton').button();
	$('#SearchButton').click(function () {
		$('#form0').submit();
	});

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	$('#lastSearch').hide().find('.error').html("");
});

//Submit Form Validation
$('#form0').submit(function () {

	$('#lastSearch').hide().find('.error').html("");

	var filterField_1Value, filterField_2Value, filterField_3Value;

	//The user must select at least one filter and provide the corresponding search input value in order to search.
	//If neither filter nor input value are provided there is an error message "Please provide at least one filter".
	if ($('#filterField_1').val() == "" && $('#filterValue_1').val() == "" &&
		$('#filterField_2').val() == "" && $('#filterValue_2').val() == "" &&
		$('#filterField_3').val() == "" && $('#filterValue_3').val() == "") {
		$('#lastSearch').show().find('.error').html("Please provide at least one filter");
		return false;
	}

	//If the user enters a search input value but does not select the corresponding filter, there is an error message "Please provide at least one filter" as appropriate.
	if (($('#filterField_1').val() == "" && $('#filterValue_1').val() != "") ||
		($('#filterField_2').val() == "" && $('#filterValue_2').val() != "") ||
		($('#filterField_3').val() == "" && $('#filterValue_3').val() != "")) {
		$('#lastSearch').show().find('.error').html("Please provide at least one filter");
		return false;
	}

	//If the user selects a filter but does not provide a corresponding search input value, there is an error message "Please provide at least one filter".
	if ($('#filterField_1').val() != "" && $('#filterValue_1').val() == "") {
		$('#lastSearch').show().find('.error').html("Please select a filter field for your first filter");
		return false;
	} else {
		filterField_1Value = $('#filterField_1').val();
	}

	if ($('#filterField_2').val() != "" && $('#filterValue_2').val() == "") {
		$('#lastSearch').show().find('.error').html("Please select a filter field for your second filter");
		return false;
	} else {
		filterField_2Value = $('#filterField_2').val();
	}

	if ($('#filterField_3').val() != "" && $('#filterValue_3').val() == "") {
		$('#lastSearch').show().find('.error').html("Please select a filter field for your third filter");
		return false;
	} else {
		filterField_3Value = $('#filterField_3').val();
	}

	//PCI = Enforce minimum length to reduce timeouts
	if ((filterField_1Value != "" && filterField_1Value.length < 2) ||
		(filterField_2Value != "" && filterField_2Value.length < 2) ||
		(filterField_3Value != "" && filterField_3Value.length < 2)) {
		$('#lastSearch').show().find('.error').html("Please ensure a minimum of 2 characters per filter");
		return false;
	}

	return true;
});