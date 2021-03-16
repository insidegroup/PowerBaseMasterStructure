$(document).ready(function() {

	//Navigation
	$('#menu_teams').click();

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	//Show DatePickers
	$('#SystemUserGDSAccessRight_EnabledDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	if ($('#SystemUserGDSAccessRight_EnabledDate').val() == "" || $('#SystemUserGDSAccessRight_EnabledDate').val() == "1/1/0001 12:00:00 AM") {
		$('#SystemUserGDSAccessRight_EnabledDate').val("No Enabled Date")
	}

	//The Home PCC/Office ID is not active until a GDS has been selected, unless the value selected is “All GDS Systems”, in which case the Home PCC/Office ID drop list remains inactive.

	//Edit
	if ($('#SystemUserGDSAccessRight_PseudoCityOrOfficeId').val() == "") {
		updatePseudoCityOrOfficeId();
	}

	$('#SystemUserGDSAccessRight_GDSCode').change(function() {
		updatePseudoCityOrOfficeId();
	});

	function updatePseudoCityOrOfficeId() {
		var value = $('#SystemUserGDSAccessRight_GDSCode').val();
		if (value != "" && value != "ALL") {
			$('#SystemUserGDSAccessRight_PseudoCityOrOfficeId').attr('disabled', false);
			$('#SystemUserGDSAccessRight_PseudoCityOrOfficeId_Error').show();
			$('#SystemUserGDSAccessRight_PseudoCityOrOfficeId').empty().append('<option value="">Please Select...</option>');
			LoadPseudoCityOrOfficeIds();
		} else {
			$('#SystemUserGDSAccessRight_PseudoCityOrOfficeId').attr('disabled', true);
			$('#SystemUserGDSAccessRight_PseudoCityOrOfficeId').empty().append('<option value="">Please Select...</option>');
			$('#SystemUserGDSAccessRight_PseudoCityOrOfficeId_Error').hide();
		}
	}

	function LoadPseudoCityOrOfficeIds() {
		var gdsCode = $('#SystemUserGDSAccessRight_GDSCode').val();
		jQuery.ajax({
			type: "POST",
			url: "/SystemUserGDSAccessRight.mvc/GetSystemUserPseudoCityOrOfficeIdsByGDSCode",
			data: {
				gdsCode: gdsCode,
				systemUserGuid: $('#SystemUserGDSAccessRight_SystemUserGuid').val()
			},
			success: function (data) {
				$.each(data, function (index, item) {
					$("#SystemUserGDSAccessRight_PseudoCityOrOfficeId").append(
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
	$('#lblSystemUserGDSAccessRightPseudoCityOrOfficeIdMsg').hide();

	//Edit
	if ($('#SystemUserGDSAccessRight_GDSAccessTypeId').val() == "") {
		updateGDSAccessTypeId();
	}

	$('#SystemUserGDSAccessRight_GDSCode').change(function () {
		updateGDSAccessTypeId();
	});

	function updateGDSAccessTypeId() {
		var value = $('#SystemUserGDSAccessRight_GDSCode').val();
		if (value != "" && value != "ALL") {
			$('#SystemUserGDSAccessRight_GDSAccessTypeId').attr('disabled', false);
			$('#SystemUserGDSAccessRight_GDSAccessTypeId').empty().append('<option value="">Please Select...</option>');
			LoadGDSAccessTypeIds();
		} else {
			$('#SystemUserGDSAccessRight_GDSAccessTypeId').empty().append('<option value="">Please Select...</option>');
			$('#SystemUserGDSAccessRight_GDSAccessTypeId').attr('disabled', true);
		}
	}

	function LoadGDSAccessTypeIds() {
		var gdsCode = $('#SystemUserGDSAccessRight_GDSCode').val();
		jQuery.ajax({
			type: "POST",
			url: "/GDSAccessType.mvc/GetGDSAccessTypesByGDSCode",
			data: { gdsCode: gdsCode },
			success: function (data) {
				$.each(data, function (index, item) {
					$("#SystemUserGDSAccessRight_GDSAccessTypeId").append(
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

	//IsAvailableSystemUserGDSAccessRightGDSSignOnID
	jQuery.ajax({
		type: "POST",
        url: "/GroupNameBuilder.mvc/IsAvailableGDSAccessRightGDSSignOnID",
		data: {
			gdsSignOnID: $("#SystemUserGDSAccessRight_GDSSignOnID").val(),
			pseudoCityOrOfficeId: $("#SystemUserGDSAccessRight_PseudoCityOrOfficeId").val(),
            id: $("#SystemUserGDSAccessRight_SystemUserGDSAccessRightId").length > 0 ? $("#SystemUserGDSAccessRight_SystemUserGDSAccessRightId").val() : 0,
            groupName: "SystemUser"
		},
		success: function (data) {
			valid = data;
		},
		dataType: "json",
		async: false
	});

	if (!valid) {
		$("#lblSystemUserGDSAccessRightMsg").removeClass('field-validation-valid');
		$("#lblSystemUserGDSAccessRightMsg").addClass('field-validation-error');
		$("#lblSystemUserGDSAccessRightMsg").text("This GDS Sign On ID is already in use for the selected Home PCC/Office ID.");
			return false;
		} else {
		$("#lblSystemUserGDSAccessRightMsg").text("");
	}

	if (!$(this).valid()) {
		return false;
	}

	//PCC
	var selected_GDS = $('#SystemUserGDSAccessRight_GDSCode').val();
	if (selected_GDS != "" && selected_GDS != "ALL") {
		var selected_PCC = $('#SystemUserGDSAccessRight_PseudoCityOrOfficeId').val();
		if (selected_PCC == '') {
			$('#lblSystemUserGDSAccessRightPseudoCityOrOfficeIdMsg').show();
			return false;
		}
	}

	if (valid) {
		if ($('#SystemUserGDSAccessRight_EnabledDate').val() == "No Enabled Date") {
			$('#SystemUserGDSAccessRight_EnabledDate').val("");
		}
		return true;
	}
});