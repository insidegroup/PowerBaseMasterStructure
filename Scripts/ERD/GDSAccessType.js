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
	var externalNameId = $("#GDSAccessType_GDSAccessTypeId").length > 0 ? $("#GDSAccessType_GDSAccessTypeId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailableGDSAccessTypeName",
		data: {
			groupName: $("#GDSAccessType_GDSAccessTypeName").val(),
			id: externalNameId,
			gdsCode: $("#GDSAccessType_GDSCode").val()
		},
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblGDSAccessTypeMsg").removeClass('field-validation-valid');
		$("#lblGDSAccessTypeMsg").addClass('field-validation-error');
		if ($("#GDSAccessType_GDSAccessTypeId").val() == "0") {//Create
			$("#lblGDSAccessTypeMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#GDSAccessType_GDSAccessTypeName").val() != "") {
				$("#lblGDSAccessTypeMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblGDSAccessTypeMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});