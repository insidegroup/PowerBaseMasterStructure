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
	var gdsRequestTypeId = $("#GDSRequestType_GDSRequestTypeId").length > 0 ? $("#GDSRequestType_GDSRequestTypeId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailableGDSRequestTypeName",
		data: { groupName: $("#GDSRequestType_GDSRequestTypeName").val(), id: gdsRequestTypeId },
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblGDSRequestTypeMsg").removeClass('field-validation-valid');
		$("#lblGDSRequestTypeMsg").addClass('field-validation-error');
		if ($("#GDSRequestType_GDSRequestTypeId").val() == "0") {//Create
			$("#lblGDSRequestTypeMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#GDSRequestType_GDSRequestTypeName").val() != "") {
				$("#lblGDSRequestTypeMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblGDSRequestTypeMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});