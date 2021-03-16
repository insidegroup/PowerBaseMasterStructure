
$(document).ready(function() {
	//Navigation
	$('#menu_clients').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$('#breadcrumb').css('width', 'auto');


	/*
	Load Channel Product based on Booking Channel
	*/

	var channelProduct = $('#BookingChannel_ProductChannelTypeId');
	var bookingChannel = $('#BookingChannel_BookingChannelTypeId');
	var desktopUsed = $('#BookingChannel_DesktopUsedTypeId');
	var desktopUsedError = $('#BookingChannelDesktopUsedIdError');

	desktopUsedError.hide();

	if (channelProduct.val() == '') {
		channelProduct.attr("disabled", true);
	}

	if (desktopUsed.val() == '') {
		desktopUsed.attr("disabled", true);
	}

	bookingChannel.change(function () {

		var bookingChannelTypeId = bookingChannel.val();
		if (bookingChannelTypeId != '') {
			jQuery.ajax({
				type: "POST",
				url: "/BookingChannel.mvc/GetProductChannelTypes",
				data: { BookingChannelTypeId: bookingChannel.val() },
				success: function (data) {
					channelProduct.get(0).options.length = 0;
					channelProduct.get(0).options[0] = new Option("Please Select...", "");

					$.each(data, function (index, item) {
						channelProduct.get(0).options[channelProduct.get(0).options.length] = new Option(item.ProductChannelTypeDescription, item.ProductChannelTypeId);
					});

					channelProduct.attr("disabled", false);
				},
				dataType: "json",
				async: false
			});
		} else {
			channelProduct.attr("disabled", true);
		}

		var bookingChannelTypeDescription = $("option:selected", bookingChannel).text();

		if (bookingChannelTypeDescription == 'Offline' || bookingChannelTypeDescription == '24HSC') {
			desktopUsed.attr("disabled", false);
			desktopUsedError.show();
		} else {
			desktopUsed.val('').attr("disabled", true);
			desktopUsedError.hide();
		}

	});

});

/*
Submit Form Validation
*/
$('#form0').submit(function () {

	var channelProduct = $('#BookingChannel_ProductChannelTypeId');
	var bookingChannel = $('#BookingChannel_BookingChannelTypeId');
	var desktopUsed = $('#BookingChannel_DesktopUsedTypeId');
	var desktopUsedError = $('#BookingChannelDesktopUsedIdError');
	var ticketingPseudoCityOrOfficeId = $("#BookingChannel_TicketingPseudoCityOrOfficeId").val();
	var bookingPseudoCityOrOfficeId = $("#BookingChannel_BookingPseudoCityOrOfficeId").val();
	var gds = $("#BookingChannel_GDSCode").val();

	//Reset Errors
	$('#lblValidDesktopUsedIdMessage').text('');
	desktopUsed.removeClass('input-validation-error');

	//Desktop Used is required if enabled
	var bookingChannelTypeDescription = $("option:selected", bookingChannel).text();
	if (bookingChannelTypeDescription == 'Offline' || bookingChannelTypeDescription == '24HSC') {
		if (desktopUsed.val() == '') {
			desktopUsed.addClass('input-validation-error');
			$('#lblValidDesktopUsedIdMessage').addClass('field-validation-error').text('Desktop Used Required');
			return false;
		}
	}

	//IsValid Booking PCC/GDS
	$('#lblValidBookingPseudoCityOrOfficeIdMessage').text("");

	var validBookingPCCGDS = false;

	if(bookingPseudoCityOrOfficeId != '' && gds != ''){
		jQuery.ajax({
			type: "POST",
			url: "/GroupNameBuilder.mvc/IsValidPccGDS",
			data: { pcc: bookingPseudoCityOrOfficeId, gds: gds },
			success: function (data) {
				validBookingPCCGDS = data;
			},
			dataType: "json",
			async: false
		});

		if (!validBookingPCCGDS) {
			$('#lblValidBookingPseudoCityOrOfficeIdMessage')
				.addClass('field-validation-error')
				.text('The PCC/Office ID you have selected is not valid for this GDS.');
			return false;
		}
	}

	//IsValid Ticketing PCC/GDS
	//Validates once Booking PCC is valid
	$('#lblValidTicketingPseudoCityOrOfficeIdMessage').text("");

	var validTicketingPCCGDS = false;
	
	if (ticketingPseudoCityOrOfficeId != '' && gds != '') {

		jQuery.ajax({
			type: "POST",
			url: "/GroupNameBuilder.mvc/IsValidPccGDS",
			data: { pcc: ticketingPseudoCityOrOfficeId, gds: gds },
			success: function (data) {
				validTicketingPCCGDS = data;
			},
			dataType: "json",
			async: false
		});

		if (!validTicketingPCCGDS) {
			$('#lblValidTicketingPseudoCityOrOfficeIdMessage')
				.addClass('field-validation-error')
				.text('The PCC/Office ID you have selected is not valid for this GDS.');
			return false;
		}
	}

	return;
});