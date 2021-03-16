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
	var externalNameId = $("#FareRedistribution_FareRedistributionId").length > 0 ? $("#FareRedistribution_FareRedistributionId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailableFareRedistributionName",
		data: {
			groupName: $("#FareRedistribution_FareRedistributionName").val(),
			id: externalNameId,
			gdsCode: $("#FareRedistribution_GDSCode").val()
		},
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblFareRedistributionMsg").removeClass('field-validation-valid');
		$("#lblFareRedistributionMsg").addClass('field-validation-error');
		if ($("#FareRedistribution_FareRedistributionId").val() == "0") {//Create
			$("#lblFareRedistributionMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#FareRedistribution_FareRedistributionName").val() != "") {
				$("#lblFareRedistributionMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblFareRedistributionMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});