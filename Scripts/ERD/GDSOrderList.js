$(document).ready(function () {

	$('#breadcrumb').css('width', 'auto');
	$('.full-width #search_wrapper').hide();

	//Search
	$('#search').hide();
	$('#SearchButton').button();
	$('#SearchButton').click(function () {
		$('#form0').submit();
	});

	$("#mainTable tr:odd").addClass("row_odd");
	$("#mainTable tr:even").addClass("row_even");

	//Show DatePickers

	$('#GDSOrderDateTimeStart').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd, yy',
		duration: 0,
	});

	if ($('#GDSOrderDateTimeStart').val() == "") {
		$('#GDSOrderDateTimeStart').attr("placeholder", "No Begin Date");
	}

	$('#GDSOrderDateTimeEnd').blur*
	$('#GDSOrderDateTimeEnd').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd, yy',
		duration: 0
	});

	if ($('#GDSOrderDateTimeEnd').val() == "") {
		$('#GDSOrderDateTimeEnd').attr("placeholder", "No End Date");
	}

	$('#GDSOrderDateTimeStart, #GDSOrderDateTimeEnd').removeClass('input-validation-error');

	$('#lastSearch').hide().find('.error').html("");

});

$(function () {

	//GDS Order
	$("#PseudoCityOrOfficeId").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/GDSOrder.mvc/GetPseudoCityOrOfficeMaintenance", type: "POST", dataType: "json",
				data: {
					searchText: request.term
				},
				success: function (data) {
					response($.map(data, function (item) {
						return {
							id: item.PseudoCityOrOfficeMaintenanceId,
							value: item.PseudoCityOrOfficeId,
							pseudoCityOrOfficeAddressId: item.PseudoCityOrOfficeAddressId,
							firstAddressLine: item.FirstAddressLine
						}
					}))
				}
			})
		},
		select: function (event, ui) {
			$("#GDSOrder_PseudoCityOrOfficeId").val(ui.item.value);
		}
	});

});

//Submit Form Validation
$('#form0').submit(function () {

	$('#lastSearch').hide().find('.error').html("");

	var GDSOrderId = $('#GDSOrderId').val();
	var PseudoCityOrOfficeId = $('#PseudoCityOrOfficeId').val();
	var Analyst = $('#Analyst').val();
	var GDSCode = $('#GDSCode option:selected').val();
	var GDSOrderStatusId = $('#GDSOrderStatusId option:selected').val();
	var GDSOrderTypeId = $('#GDSOrderTypeId option:selected').val();
	var InternalSiteName = $('#InternalSiteName').val();
	var TicketNumber = $('#TicketNumber').val();
	var GDSOrderDateTimeStart = $('#GDSOrderDateTimeStart').val();
	var GDSOrderDateTimeEnd = $('#GDSOrderDateTimeEnd').val();

	//The user must select at least one filter
	if (
		GDSOrderId == "" &&
		PseudoCityOrOfficeId == "" &&
		Analyst == "" && 
		GDSCode == "" &&
		GDSOrderStatusId == "" && 
		GDSOrderTypeId == "" && 
		InternalSiteName == "" && 
		TicketNumber == "" && 
		GDSOrderDateTimeStart == "No Begin Date" && 
		GDSOrderDateTimeEnd == "No End Date"
	) 
	{
		$('#lastSearch').show().find('.error').html("Please provide at least one filter");
		return false;
	}

	//Special characters = asterisk (*), dash (-), underscore (_), space, period (.) and right and left parenthesis (())
	var allowedNumberRegex = /^\s*[0-9]+\s*$/;
	if (!allowedNumberRegex.test(GDSOrderId) && GDSOrderId != '') {
		$('#lastSearch').show().find('.error').html("Only numbers are valid for Order Number");
		return false;
	}

	var allowedCharRegex = /^\s*[À-ÿ\w\s*\-_.()]+\s*$/;

	if (!allowedCharRegex.test(PseudoCityOrOfficeId) && PseudoCityOrOfficeId != '') {
		$('#lastSearch').show().find('.error').html("Special character entered is not allowed for PCC/OID");
		return false;
	}

	//Internal Site Name
	if (InternalSiteName != '') {
		if(InternalSiteName.length < 2) {
			$('#lastSearch').show().find('.error').html("Minimum 2 characters for Internal Site Name");
			return false;
		} else if (!allowedCharRegex.test(InternalSiteName)) {
			$('#lastSearch').show().find('.error').html("Special character entered is not allowed for Internal Site Name");
			return false;
		}
	}

	//Ticket Number – user needs to enter a minimum of 2 characters and can enter up to 50 alphanumeric, accented and allowable special characters
	if (TicketNumber != '') {
		if(TicketNumber.length < 2) {
			$('#lastSearch').show().find('.error').html("Minimum 2 characters for Ticket Number");
			return false;
		} else if (!allowedCharRegex.test(TicketNumber) && TicketNumber != '') {
			$('#lastSearch').show().find('.error').html("Special character entered is not allowed for Ticket Number");
			return false;
		}
	}

    return true;
});