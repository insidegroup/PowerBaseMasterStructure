$(document).ready(function() {
	//Navigation
	$('#menu_admin, #menu_admin_gdsmanagement').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	$('#lblGDSOrderTypeLabelGDSMsg').hide();
});

//Submit Form Validation
$('#form0').submit(function () {

	//At least one GDS checkbox must be checked in order to save a GDS Order Type
	if ($('.checkbox:checked').length == 0) {
		$('#lblGDSOrderTypeLabelGDSMsg').show();
		return false;
	}

	//GroupName Begin
	var validGroupName = false;
	var GDSOrderTypeId = $("#GDSOrderType_GDSOrderTypeId").length > 0 ? $("#GDSOrderType_GDSOrderTypeId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailableGDSOrderTypeName",
		data: {
			groupName: $("#GDSOrderType_GDSOrderTypeName").val(),
			id: GDSOrderTypeId
		},
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblGDSOrderTypeMsg").removeClass('field-validation-valid');
		$("#lblGDSOrderTypeMsg").addClass('field-validation-error');
		if ($("#GDSOrderType_GDSOrderTypeId").val() == "0") {//Create
			$("#lblGDSOrderTypeMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#GDSOrderType_GDSOrderTypeName").val() != "") {
				$("#lblGDSOrderTypeMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblGDSOrderTypeMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});