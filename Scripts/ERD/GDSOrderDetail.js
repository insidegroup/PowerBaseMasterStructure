$(document).ready(function() {
	//Navigation
	$('#menu_admin, #menu_admin_gdsmanagement').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	$('#lblGDSOrderDetailLabelGDSMsg').hide();
});

//Submit Form Validation
$('#form0').submit(function () {

	//At least one GDS checkbox must be checked in order to save a GDS Order Detail
	if ($('.checkbox:checked').length == 0) {
		$('#lblGDSOrderDetailLabelGDSMsg').show();
		return false;
	}

	//GroupName Begin
	var validGroupName = false;
	var GDSOrderDetailId = $("#GDSOrderDetail_GDSOrderDetailId").length > 0 ? $("#GDSOrderDetail_GDSOrderDetailId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailableGDSOrderDetailName",
		data: {
			groupName: $("#GDSOrderDetail_GDSOrderDetailName").val(),
			id: GDSOrderDetailId
		},
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblGDSOrderDetailMsg").removeClass('field-validation-valid');
		$("#lblGDSOrderDetailMsg").addClass('field-validation-error');
		if ($("#GDSOrderDetail_GDSOrderDetailId").val() == "0") {//Create
			$("#lblGDSOrderDetailMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#GDSOrderDetail_GDSOrderDetailName").val() != "") {
				$("#lblGDSOrderDetailMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblGDSOrderDetailMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validGroupName) {
		return true;
	}
});