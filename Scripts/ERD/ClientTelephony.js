$(document).ready(function () {
	$('#menu_admin').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	$('#ClientTelephony_ExpiryDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	if ($('#ClientTelephony_ExpiryDate').val() == "") {
		$('#ClientTelephony_ExpiryDate').val("No Expiry Date");
	}

	$('#ClientTelephony_CountryCode').change(function () {
		GetClientTelephony_PhoneNumberwithInternationalPrefixCode();
	});

	$('#lblClientSnSButtonText').hide();

	LoadClientSnSButtonText();
});

//The Client S&S Button Text field will be mandatory only when the Telephone Type is set to Safety and Security
$('#ClientTelephony_TelephonyTypeId').change(function() {
	LoadClientSnSButtonText();
});

function LoadClientSnSButtonText() {

	var clientTelephony_TelephonyTypeId = $('#ClientTelephony_TelephonyTypeId option:selected').text();

	if (clientTelephony_TelephonyTypeId == "Safety and Security") {
		$('#ClientSnSButtonTextError').show();
	} else {
		$('#ClientSnSButtonTextError').hide();
	}
}

function GetClientTelephony_PhoneNumberwithInternationalPrefixCode() {

	var clientTelephony_PhoneNumber = $('#ClientTelephony_PhoneNumber').val();

	var phoneNumberwithInternationalPrefixCode = $('#ClientTelephony_CountryCode option:selected').text();

	var matches = phoneNumberwithInternationalPrefixCode.match(/\((.*)\)/);

	if (matches != null && matches[1] != null) {
		var prefix = matches[1];
		var shortened_prefix = prefix.replace('+', '');
		$('#ClientTelephony_PhoneNumberwithInternationalPrefixCode').val(shortened_prefix + "" + clientTelephony_PhoneNumber);
		$('#ClientTelephony_InternationalPrefixCode').val(prefix);
	} else {
		$('#ClientTelephony_PhoneNumberwithInternationalPrefixCode').val(clientTelephony_PhoneNumber);
		$('#ClientTelephony_InternationalPrefixCode').val("");
	}
}

$('#form0').submit(function () {

	var validItem = false;
	var validPhoneNumber = true;
	var hierarchyType = $('#ClientTelephony_HierarchyType').val();
	var hierarchyItem = $('#ClientTelephony_HierarchyItem').val();
	var hierarchyName = $('#ClientTelephony_HierarchyName').val();
	var clientTelephonyId = ($('#ClientTelephony_ClientTelephonyId').length > 0) ? $('#ClientTelephony_ClientTelephonyId').val() : 0;

	//Fields for Button Text validation
	var clientTelephony_TelephonyTypeId = $('#ClientTelephony_TelephonyTypeId option:selected').text();
	var clientTelephony_ClientSnSButtonText = $('#ClientTelephony_ClientSnSButtonText');
	var lblClientSnSButtonText = $('#lblClientSnSButtonText');

	//Reset Button Text message
	lblClientSnSButtonText.hide();

	//Capture the prefix and phone number to the hidden/readonly fields
	GetClientTelephony_PhoneNumberwithInternationalPrefixCode();

	//Only one phone number associated to a client topunit or client subunit can be marked as Main Number
	var isMainNumberChecked = $('#ClientTelephony_MainNumberFlagNullable').is(':checked');
	if (isMainNumberChecked) {
		
		jQuery.ajax({
			type: "POST",
			url: "/Hierarchy.mvc/IsValidClientTelephonyMainNumber",
			data: { searchText: hierarchyItem, hierarchyType: hierarchyType, clientTelephonyId: clientTelephonyId },
			success: function (data) {

				if (!jQuery.isEmptyObject(data)) {
					validPhoneNumber = false;
				}
			},
			dataType: "json",
			async: false
		});

		if (!validPhoneNumber) {
			$("#lblMainNumberFlagNullableMsg").removeClass('field-validation-valid');
			$("#lblMainNumberFlagNullableMsg").addClass('field-validation-error');
			$("#lblMainNumberFlagNullableMsg").text("A main number already exists for this client.");
			return false;
		} else {
			$("#lblMainNumberFlagNullableMsg").text("");
		}
	}

	//Is Valid Hierarchy Item
	jQuery.ajax({
		type: "POST",
		url: "/Hierarchy.mvc/IsValid" + hierarchyType,
		data: { searchText: hierarchyName },
		success: function (data) {

			if (!jQuery.isEmptyObject(data)) {
				validItem = true;
			}
		},
		dataType: "json",
		async: false
	});
	if (!validItem) {
		$("#lblHierarchyItemMsg").removeClass('field-validation-valid');
		$("#lblHierarchyItemMsg").addClass('field-validation-error');
		$("#lblHierarchyItemMsg").text("This is not a valid entry.");
	} else {
		$("#lblHierarchyItemMsg").text("");
	}

	//The Client S&S Button Text field will be mandatory only when the Telephone Type is set to Safety and Security
	if(clientTelephony_TelephonyTypeId == "Safety and Security" && clientTelephony_ClientSnSButtonText.val() == '') {
		lblClientSnSButtonText.show();
		return false;
	}

	if (validItem && validPhoneNumber) {
		if ($('#ClientTelephony_ExpiryDate').val() == "No Expiry Date") {
			$('#ClientTelephony_ExpiryDate').val("");
		}
		return true;
	} else {
		return false
	};

});

/////////////////////////////////////////////////////////
// BEGIN AUTOCOMPLETES
/////////////////////////////////////////////////////////
$(function () {

	$("#ClientTelephony_HierarchyName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/GetClientTelephonyHierarchyName", type: "POST", dataType: "json",
				data: { searchText: request.term, hierarchyType: $('#ClientTelephony_HierarchyType').val() },
				success: function (data) {
					response($.map(data, function (item) {
						var hierarchyType = $('#ClientTelephony_HierarchyType').val();
						if (hierarchyType == 'ClientSubUnit') {
							return {
								label: item.ClientSubUnitName + ' (' + item.ClientTopUnitName + ')',
								value: item.ClientSubUnitName,
								id: item.ClientSubUnitGuid
							}
						} else if (hierarchyType == 'ClientTopUnit') {
							return {
								value: item.ClientTopUnitName,
								label: item.ClientTopUnitName,
								id: item.ClientTopUnitGuid
							}
						}
					}));
				}
			});
		},
		mustMatch: true,
		select: function (event, ui) {
			$("#ClientTelephony_HierarchyName").val(ui.item.value);
			$("#ClientTelephony_HierarchyItem").val(ui.item.id);
		}
	});
	
});
/////////////////////////////////////////////////////////
// END AUTOCOMPLETES
/////////////////////////////////////////////////////////