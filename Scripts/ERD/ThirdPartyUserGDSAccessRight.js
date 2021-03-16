$(document).ready(function() {

	//Navigation
	$('#menu_gdsmanagement').click();
	$('#breadcrumb').css('width', 'auto');

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	//Show DatePickers
	$('#ThirdPartyUserGDSAccessRight_EnabledDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	if ($('#ThirdPartyUserGDSAccessRight_EnabledDate').val() == "" || $('#ThirdPartyUserGDSAccessRight_EnabledDate').val() == "1/1/0001 12:00:00 AM") {
		$('#ThirdPartyUserGDSAccessRight_EnabledDate').val("No Enabled Date")
	}

	//The Home PCC/Office ID is not active until a GDS has been selected, unless the value selected is “All GDS Systems”, in which case the Home PCC/Office ID drop list remains inactive.

	if ($('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').val() == "") {
		updatePseudoCityOrOfficeId();
	}

	$('#ThirdPartyUserGDSAccessRight_GDSCode').change(function() {
		updatePseudoCityOrOfficeId();
	});

	function updatePseudoCityOrOfficeId() {
		var value = $('#ThirdPartyUserGDSAccessRight_GDSCode').val();
		if (value != "" && value != "ALL") {
			$('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').attr('disabled', false);
			$('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId_Error').show();
			$('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').empty().append('<option value="">Please Select...</option>');
			LoadPseudoCityOrOfficeIds();
		} else {
			$('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').attr('disabled', true);
			$('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').empty().append('<option value="">Please Select...</option>');
			$('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId_Error').hide();
		}
	}

	function LoadPseudoCityOrOfficeIds() {
		
		var gdsCode = $('#ThirdPartyUserGDSAccessRight_GDSCode').val();
		var thirdPartyUserId = $('#ThirdPartyUserGDSAccessRight_ThirdPartyUserId').val();

		jQuery.ajax({
			type: "POST",
			url: "/ThirdPartyUser.mvc/GetThirdPartyUserPseudoCityOrOfficeIdsByGDSCode",
			data: {
				gdsCode: gdsCode,
				thirdPartyUserId: thirdPartyUserId
			},
			success: function (data) {
				$.each(data, function (index, item) {
					$("#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId").append(
						$("<option></option>")
							.text(item.PseudoCityOrOfficeId)
							.val(item.PseudoCityOrOfficeId)
					);
				});

			},
			dataType: "json",
			async: false
		});
	}

	//GDS Access Type is not active until a GDS has been selected, unless the value selected is "All GDS Systems",
	//in which case the GDS Access Type drop list remains inactive.
	$('#lblThirdPartyUserGDSAccessRightPseudoCityOrOfficeIdMsg').hide();

	//Edit
	if ($('#ThirdPartyUserGDSAccessRight_GDSAccessTypeId').val() == "") {
	updateGDSAccessTypeId();
	}

	$('#ThirdPartyUserGDSAccessRight_GDSCode').change(function () {
		updateGDSAccessTypeId();
	});

	function updateGDSAccessTypeId() {
		var value = $('#ThirdPartyUserGDSAccessRight_GDSCode').val();
		if (value != "" && value != "ALL") {
			$('#ThirdPartyUserGDSAccessRight_GDSAccessTypeId').attr('disabled', false);
				$('#ThirdPartyUserGDSAccessRight_GDSAccessTypeId').empty().append('<option value="">Please Select...</option>');
				LoadGDSAccessTypeIds();
		} else {
			$('#ThirdPartyUserGDSAccessRight_GDSAccessTypeId').empty().append('<option value="">Please Select...</option>');
			$('#ThirdPartyUserGDSAccessRight_GDSAccessTypeId').attr('disabled', true);
		}
	}

	function LoadGDSAccessTypeIds() {
		var gdsCode = $('#ThirdPartyUserGDSAccessRight_GDSCode').val();
		jQuery.ajax({
			type: "POST",
			url: "/GDSAccessType.mvc/GetGDSAccessTypesByGDSCode",
			data: { gdsCode: gdsCode },
			success: function (data) {
				$.each(data, function (index, item) {
					$("#ThirdPartyUserGDSAccessRight_GDSAccessTypeId").append(
						$("<option></option>")
							.text(item.GDSAccessTypeName)
							.val(item.GDSAccessTypeId)
					);
				});

			},
			dataType: "json",
			async: false
		});
	}


});

//Submit Form Validation
$('#form0').submit(function () {

	var valid = false;

	//IsAvailableThirdPartyUserGDSAccessRightGDSSignOnID
	jQuery.ajax({
		type: "POST",
        url: "/GroupNameBuilder.mvc/IsAvailableGDSAccessRightGDSSignOnID",
		data: {
			gdsSignOnID: $("#ThirdPartyUserGDSAccessRight_GDSSignOnID").val(),
			pseudoCityOrOfficeId: $("#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId").val(),
            id: $("#ThirdPartyUserGDSAccessRight_ThirdPartyUserGDSAccessRightId").length > 0 ? $("#ThirdPartyUserGDSAccessRight_ThirdPartyUserGDSAccessRightId").val() : 0,
            groupName: "ThirdPartyUser"
		},
		success: function (data) {
			valid = data;
		},
		dataType: "json",
		async: false
	});

	if (!valid) {
		$("#lblThirdPartyUserGDSAccessRightMsg").removeClass('field-validation-valid');
		$("#lblThirdPartyUserGDSAccessRightMsg").addClass('field-validation-error');
		$("#lblThirdPartyUserGDSAccessRightMsg").text("This GDS Sign On ID is already in use for the selected Home PCC/Office ID.");
		return false;
	} else {
		$("#lblThirdPartyUserGDSAccessRightMsg").text("");
	}

	if (!$(this).valid()) {
		return false;
	}

	//PCC
	var selected_GDS = $('#ThirdPartyUserGDSAccessRight_GDSCode').val();
	if (selected_GDS != "" && selected_GDS != "ALL") {
		var selected_PCC = $('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').val();
		if (selected_PCC == '') {
			$('#lblThirdPartyUserGDSAccessRightPseudoCityOrOfficeIdMsg').show();
			return false;
		}
	}

	if (valid) {
		if ($('#ThirdPartyUserGDSAccessRight_EnabledDate').val() == "No Enabled Date") {
			$('#ThirdPartyUserGDSAccessRight_EnabledDate').val("");
		}

		//If form is valid, send data to the Traveler Identity Store
		return traveller_identity_service();
	}
});