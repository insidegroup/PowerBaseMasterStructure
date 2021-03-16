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
	var pseudoCityOrOfficeDefinedRegionId = $("#PseudoCityOrOfficeDefinedRegion_PseudoCityOrOfficeDefinedRegionId").length > 0 ? $("#PseudoCityOrOfficeDefinedRegion_PseudoCityOrOfficeDefinedRegionId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailablePseudoCityOrOfficeDefinedRegionName",
		data: {
			groupName: $("#PseudoCityOrOfficeDefinedRegion_PseudoCityOrOfficeDefinedRegionName").val(),
			id: pseudoCityOrOfficeDefinedRegionId,
			globalRegionCode: $('#PseudoCityOrOfficeDefinedRegion_GlobalRegionCode').val()
		},
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblPseudoCityOrOfficeDefinedRegionMsg").removeClass('field-validation-valid');
		$("#lblPseudoCityOrOfficeDefinedRegionMsg").addClass('field-validation-error');
		if ($("#PseudoCityOrOfficeDefinedRegion_PseudoCityOrOfficeDefinedRegionId").val() == "0") {//Create
			$("#lblPseudoCityOrOfficeDefinedRegionMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#PseudoCityOrOfficeDefinedRegion_PseudoCityOrOfficeDefinedRegionName").val() != "") {
				$("#lblPseudoCityOrOfficeDefinedRegionMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblPseudoCityOrOfficeDefinedRegionMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});