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
	var pseudoCityOrOfficeLocationTypeId = $("#PseudoCityOrOfficeLocationType_PseudoCityOrOfficeLocationTypeId").length > 0 ? $("#PseudoCityOrOfficeLocationType_PseudoCityOrOfficeLocationTypeId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailablePseudoCityOrOfficeLocationTypeName",
		data: { groupName: $("#PseudoCityOrOfficeLocationType_PseudoCityOrOfficeLocationTypeName").val(), id: pseudoCityOrOfficeLocationTypeId },
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblPseudoCityOrOfficeLocationTypeMsg").removeClass('field-validation-valid');
		$("#lblPseudoCityOrOfficeLocationTypeMsg").addClass('field-validation-error');
		if ($("#PseudoCityOrOfficeLocationType_PseudoCityOrOfficeLocationTypeId").val() == "0") {//Create
			$("#lblPseudoCityOrOfficeLocationTypeMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#PseudoCityOrOfficeLocationType_PseudoCityOrOfficeLocationTypeName").val() != "") {
				$("#lblPseudoCityOrOfficeLocationTypeMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblPseudoCityOrOfficeLocationTypeMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});