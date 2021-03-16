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
	var externalNameId = $("#ExternalName_ExternalNameId").length > 0 ? $("#ExternalName_ExternalNameId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailableExternalName",
		data: { groupName: $("#ExternalName_ExternalName1").val(), id: externalNameId },
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblExternalNameMsg").removeClass('field-validation-valid');
		$("#lblExternalNameMsg").addClass('field-validation-error');
		if ($("#ExternalName_ExternalNameId").val() == "0") {//Create
			$("#lblExternalNameMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#ExternalName_ExternalName1").val() != "") {
				$("#lblExternalNameMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblExternalNameMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});