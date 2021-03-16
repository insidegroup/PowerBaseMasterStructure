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
	var pseudoCityOrOfficeLocationTypeId = $("#PseudoCityOrOfficeType_PseudoCityOrOfficeTypeId").length > 0 ? $("#PseudoCityOrOfficeType_PseudoCityOrOfficeTypeId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailablePseudoCityOrOfficeTypeName",
		data: { groupName: $("#PseudoCityOrOfficeType_PseudoCityOrOfficeTypeName").val(), id: pseudoCityOrOfficeLocationTypeId },
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblPseudoCityOrOfficeTypeMsg").removeClass('field-validation-valid');
		$("#lblPseudoCityOrOfficeTypeMsg").addClass('field-validation-error');
		if ($("#PseudoCityOrOfficeType_PseudoCityOrOfficeTypeId").val() == "0") {//Create
			$("#lblPseudoCityOrOfficeTypeMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#PseudoCityOrOfficeType_PseudoCityOrOfficeTypeName").val() != "") {
				$("#lblPseudoCityOrOfficeTypeMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblPseudoCityOrOfficeTypeMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});