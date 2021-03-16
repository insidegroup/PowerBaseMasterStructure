$(document).ready(function() {
	//Navigation
	$('#menu_admin, #menu_admin_gdsmanagement').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
});

//Submit Form Validation
$('#form0').submit(function () {

	//GroupName Begin
	var validGroupName = false;
	var partnerId = $("#Partner_PartnerId").length > 0 ? $("#Partner_PartnerId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailablePartner",
		data: {
			groupName: $("#Partner_PartnerName").val(),
			countryCode: $("#Partner_CountryCode").val(),
			id: partnerId
		},
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		var message = "This name has already been used, please choose a different name.";

		$("#lblPartnerMsg").removeClass('field-validation-valid');
		$("#lblPartnerMsg").addClass('field-validation-error');

		//Create
		if ($("#Partner_PartnerId").val() == "0") {
			$("#lblPartnerMsg").text(message);

		//Edit
		} else {
			if ($("#Partner_PartnerName").val() != "") {
				$("#lblPartnerMsg").text(message);
			}
		}
		return false;
	} else {
		$("#lblPartnerMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});