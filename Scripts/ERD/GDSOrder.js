//////////////////////////////////
//Required If
//////////////////////////////////
$.validator.addMethod('requiredif',
function (value, element, parameters) {

	var id = '#GDSOrder_' + parameters['dependentproperty'];

	// get the target value (as a string, 
	// as that's what actual value will be)
	var targetvalue = parameters['targetvalue'];
	targetvalue = (targetvalue == null ? '' : targetvalue).toString();

	// get the actual value of the target control
	var control = $(id);
	var controltype = control.attr('type');
	var actualvalue = null;
	if(controltype === 'checkbox') {
		actualvalue = control.attr('checked').toString();
	} else if(controltype === 'select') {
		actualvalue = control.find('option:selected').text();
	} else {
		actualvalue = control.val();
	}   

	// if the condition is true, reuse the existing 
	// required field validator functionality
	if (targetvalue === actualvalue)
		return $.validator.methods.required.call(this, value, element, parameters);

	return true;
});

$.validator.unobtrusive.adapters.add(
	'requiredif',
	['dependentproperty', 'targetvalue'],
	function (options) {
		options.rules['requiredif'] = {
			dependentproperty: options.params['dependentproperty'],
			targetvalue: options.params['targetvalue']
		};
		options.messages['requiredif'] = options.message;
	}
);

$(document).ready(function () {

	$('#menu_gdsmanagement').click();
	$("#form0 > table > tbody > tr:odd").addClass("row_odd");
	$("#form0 > table > tbody > tr:even").addClass("row_even");

	//Show DatePickers

	$('#GDSOrder_GDSOrderDateTime').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd, yy',
		duration: 0,
	});

	$('#GDSOrder_DeactivationDateTime').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd, yy',
		duration: 0
	});

	if ($('#GDSOrder_DeactivationDateTime').val() == "") {
		$('#GDSOrder_DeactivationDateTime').val("No Deactivation Date");
	}

	/* Order Types */

	if ($('#GDSOrder_GDSCode').val() == '') {
		$('#GDSOrder_GDSOrderTypeId').attr('disabled', 'disabled');
	}

	$('#GDSOrderTypeIdError, #GDSThirdPartyVendorIdError').hide();

	//Clear address box ready for Ajax
	if ($('#GDSOrder_PseudoCityOrOfficeAddress').val() == '') {
		$('#GDSOrder_PseudoCityOrOfficeAddress').val('');
	}
	
	$('#GDSOrder_GDSOrderTypeId').change(function () {
		LoadAdditionalFields();
	});

	function LoadAdditionalFields() {

		$('#GDSOrder_PseudoCityOrOfficeAddress').val('');
		$('.PseudoCityOrOfficeMaintenance_Row').hide();

		//Create / Edit Page
		var value = $('#GDSOrder_GDSOrderTypeId option:selected').val();
		
		//Check to Show All Data from Database Flag
		var show_data = false;

		if (value != '') {

			jQuery.ajax({
				type: "POST",
				url: "/GDSOrderType.mvc/GDSOrderTypeShowDataFlag",
				data: {
					gdsOrderTypeId: value,
				},
				success: function (data) {
					if (data == true) {
						show_data = true;
					}
				},
				dataType: "json",
				async: false
			});
		}

		if (show_data) {

			//A temporary unique ID is assigned to a PCC/OID like GDSCode-nnnnnn where the nnnnnn is a unique generated value (ex. 1S-12345)
			var timestamp = Date.now().toString(); //The Date.now() method returns the number of milliseconds elapsed since 1 January 1970 00:00:00 UTC.
			var selected_gds = $('#GDSOrder_GDSCode option:selected').val();
			var uid_length = 8 - selected_gds.length; //db field is 9 minus 
			var temporary_uid = selected_gds + '-' + timestamp.slice(timestamp.length - uid_length, timestamp.length);

			$('#GDSOrder_PseudoCityOrOfficeId').val(temporary_uid).attr('readonly', 'readonly');
			$('#GDSOrder_PseudoCityOrOfficeMaintenanceId').val('0');
			$('#GDSOrder_PseudoCityOrOfficeAddress').val('');
			$('.PseudoCityOrOfficeMaintenance_Row').show();
			$('#GDSOrder_ShowDataFlag').val(true);

		} else {
			$('#GDSOrder_PseudoCityOrOfficeMaintenanceId').val('');
			$('#GDSOrder_PseudoCityOrOfficeId').val('').attr('readonly', '');
			$('#GDSOrder_PseudoCityOrOfficeAddress').val('');
			$('.PseudoCityOrOfficeMaintenance_Row').hide();
			$('#GDSOrder_ShowDataFlag').val(false);
		}
	}

	/* Order Status */

	if ($('#GDSOrder_GDSOrderStatusId').val() == "") {
		$('#GDSOrder_GDSOrderStatusId').val("3");
	}

	/* Order Details */

	$('#GDSOrder_GDSCode').change(function () {
		LoadGDSOrderDetails();
	});

	//Edit
	var pageType = $('#PageType').val();
	if (pageType == 'Edit' && $('.gds-order-line-item').length == 1) {
		LoadGDSOrderDetails();
	}

	function LoadGDSOrderDetails() {

		var value = $('#GDSOrder_GDSCode').val();

		if (value != '') {

			$('#GDSOrder_GDSOrderTypeId').attr('disabled', ''); //.val('');
			$('#GDSOrder_PseudoCityOrOfficeId').val('');
			$('#GDSOrder_PseudoCityOrOfficeAddress').val('');

			jQuery.ajax({
				type: "POST",
				url: "/GDSOrderDetail.mvc/GetGDSOrderDetailsByGDSCode",
				data: {
					gdsCode: value,
				},
				success: function (data) {

					$('.GDSOrderDetailId').each(function () {

						var dropdown = $(this);

						var existing_value = $(this).val();

						$(dropdown).empty().append('<option value="">Please Select...</option>');

						$.each(data, function (index, item) {
							$(dropdown).append(
								$("<option></option>")
									.text(item.GDSOrderDetailName)
									.val(item.GDSOrderDetailId)
							);
						});

						//If item selected is in new list then keep it displayed
						if (existing_value != '') {
							$(dropdown).val(existing_value);
						}
					});

				},
				dataType: "json",
				async: false
			});

		} else {
			$('#GDSOrder_GDSOrderTypeId').attr('disabled', 'disabled').val('');
			$('#GDSOrder_PseudoCityOrOfficeId').val('');
			$('#GDSOrder_PseudoCityOrOfficeAddress').val('');
		}
	}

	//Add button
	$('.btn-add').live('click', function (e) {

		e.preventDefault();

		//Clone last row and add to end
		var lastItem = $('.gds-order-line-item').last().clone();
		$('.gds-order-line-item').last().after(lastItem);

		//Select the last item
		var newItem = $('.gds-order-line-item').last();

		//Increment Id
		var regExp = /\[([^\]]+)\]/;
		var first_field = newItem.find('.Quantity');
		var first_field_name = first_field.attr('name');
		var first_field_id = regExp.exec(first_field_name);
		var new_id = Number(first_field_id[1]) + 1;

		//Quantity
		var quantity = newItem.find('.Quantity');
		quantity.attr('name', 'GDSOrder.GDSOrderLineItem[' + new_id + '].Quantity').val('1');

		//GDSOrderLineItemActionId
		var gdsOrderLineItemActionId = newItem.find('.GDSOrderLineItemActionId');
		gdsOrderLineItemActionId.attr('name', 'GDSOrder.GDSOrderLineItem[' + new_id + '].GDSOrderLineItemActionId').val('');

		//AdditionalOrderDetail 
		var gdsOrderDetailId = newItem.find('.GDSOrderDetailId');
		gdsOrderDetailId.attr('name', 'GDSOrder.GDSOrderLineItem[' + new_id + '].GDSOrderDetailId').val('');

		//Comment
		var comment = newItem.find('.Comment');
		comment.attr('name', 'GDSOrder.GDSOrderLineItem[' + new_id + '].Comment').val('');

	});

	//Remove btn
	$('.btn-remove').live('click', function (e) {

		e.preventDefault();

		//Remove all items but clear last remaining ones
		var gdsOrderLineItemCount = $('.gds-order-line-item').length;
		if (gdsOrderLineItemCount > 1) {
			$(this).closest('.gds-order-line-item').remove();
		} else {
			$(this).closest('.gds-order-line-item').find('.Quantity').val('1');
			$(this).closest('.gds-order-line-item').find('.GDSOrderLineItemActionId').val('');
			$(this).closest('.gds-order-line-item').find('.GDSOrderDetailId').val('');
			$(this).closest('.gds-order-line-item').find('.Comment').val('');
		}

		//If removed a middle one, update all numbers
		for (var i = 0; i < $('.gds-order-line-item').length; i++) {

			var item = $('.gds-order-line-item:eq(' + i + ')');

			var new_id = i + 1;

			//Quantity
			var quantity = item.find('.Quantity');
			quantity.attr('name', 'GDSOrder.GDSOrderLineItem[' + new_id + '].Quantity');

			//GDSOrderLineItemActionId
			var gdsOrderLineItemActionId = item.find('.GDSOrderLineItemActionId');
			gdsOrderLineItemActionId.attr('name', 'GDSOrder.GDSOrderLineItem[' + new_id + '].GDSOrderLineItemActionId');

			//AdditionalOrderDetail 
			var gdsOrderDetailId = item.find('.GDSOrderDetailId');
			gdsOrderDetailId.attr('name', 'GDSOrder.GDSOrderLineItem[' + new_id + '].GDSOrderDetailId');

			//Comment
			var comment = item.find('.Comment');
			comment.attr('name', 'GDSOrder.GDSOrderLineItem[' + new_id + '].Comment');
		}

	});

	/* Defined Regions */

	//Disable PseudoCityOrOfficeDefinedRegionId until Country/GlobalRegion selected, or show for edit if has a value
	$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label, #GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Error').hide();

	if ($('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').val() == "") {
		$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').attr('disabled', true);
	}
	
	/* Fare Redistribution */

	//This is a drop list displaying values from the Fare Redistribution table based upon the GDS
	//If there are no fare redistribution values for the GDS chosen, this field will be greyed out
	if ($('.FareRedistributionId').val() == "") {
		$('.FareRedistributionId').attr('disabled', true);
	}

	$('#GDSOrder_PseudoCityOrOfficeMaintenance_FareRedistributionId_Label, #GDSOrder_PseudoCityOrOfficeMaintenance_FareRedistributionId_Error').hide();
	$('.PseudoCityOrOfficeMaintenanceGDSCode').change(function () {
		LoadGDSOrder_GDSCode();
	});

	//GetFareRedistributionsByGDSCode
	function LoadGDSOrder_GDSCode() {

		var selectedGDS = $('.PseudoCityOrOfficeMaintenanceGDSCode').val();

		if (selectedGDS != "") {

			$.ajax({
				url: "/FareRedistribution.mvc/GetFareRedistributionsByGDSCode", type: "POST", dataType: "json",
				data: { gdsCode: selectedGDS },
				success: function (data) {

					// Clear the old options
					$(".FareRedistributionId").find('option').remove();

					// Add a default
					$("<option value=''>Please Select...</option>").appendTo($(".FareRedistributionId"));

					// Load the new options
					$(data).each(function () {
						$("<option value=" + this.FareRedistributionId + ">" + this.FareRedistributionName + "</option>").appendTo($(".FareRedistributionId"));
					});

					//Create
					if ($('.FareRedistributionId option').length > 1) {
						$('.FareRedistributionId').attr('disabled', false);
						$('#GDSOrder_PseudoCityOrOfficeMaintenance_FareRedistributionId_Error').show();
					} else {
						$('.FareRedistributionId').val('').attr('disabled', true);
						$('#GDSOrder_PseudoCityOrOfficeMaintenance_FareRedistributionId_Error').hide();
					}
				}
			});

		} else {
			$('.FareRedistributionId').val('').attr('disabled', true);
		}
	}

	/* PseudoCityOrOfficeMaintenance */

	//The Country and Global Region fields will be filled in for the user based upon the Address selected and will be read only
	$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId').change(function () {
		var selectedAddress = $(this).val();
		if (selectedAddress != "") {
			jQuery.ajax({
				type: "POST",
				url: "/PseudoCityOrOfficeAddress.mvc/GetPseudoCityOrOfficeAddress",
				data: {
					addressId: $("#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId").val()
				},
				success: function (data) {

					if (data.length > 0 && data[0] != null) {

						//CountryName
						var countryName = data[0].CountryName;
						if (countryName != "") {
							$('#GDSOrder_PseudoCityOrOfficeMaintenance_CountryName').val(countryName);
						}

						//GlobalRegionName
						var globalRegionName = data[0].GlobalRegionName;
						var globalRegionCode = data[0].GlobalRegionCode;
						if (globalRegionName != "" && globalRegionCode != "") {
							$('#GDSOrder_PseudoCityOrOfficeMaintenance_GlobalRegionName').val(globalRegionName);
							$('#GDSOrder_PseudoCityOrOfficeMaintenance_GlobalRegionCode').val(globalRegionCode);
						}

						//Pseudo City/Office ID Defined Region is a drop list based upon Global Region selection
						$.ajax({
							url: "/PseudoCityOrOfficeMaintenance.mvc/GetPseudoCityOrOfficeDefinedRegionsByGlobalRegionCode", type: "POST", dataType: "json",
							data: { globalRegionCode: $("#GDSOrder_PseudoCityOrOfficeMaintenance_GlobalRegionCode").val() },
							success: function (data) {

								// Clear the old options
								$("#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId").find('option').remove();

								// Add a default
								$("<option value=''>Please Select...</option>").appendTo($("#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId"));

								// Load the new options
								$(data).each(function () {
									$("<option value=" + this.PseudoCityOrOfficeDefinedRegionId + ">" + this.PseudoCityOrOfficeDefinedRegionName + "</option>")
										.appendTo($("#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId"));
								});

								//The Pseudo City/Office ID Defined Region field will only be mandatory if any options
								if ($("#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId option").length > 1) {

									// Enable dropdown
									$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').attr('disabled', false);
									$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Error').show();

								} else {

									// Disable dropdown
									$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').attr('disabled', true);
									$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Error').hide();
								}
							}
						});

					} else {
						$('#GDSOrder_PseudoCityOrOfficeMaintenance_CountryName').val('');
						$('#GDSOrder_PseudoCityOrOfficeMaintenance_GlobalRegionName').val('');
					}
				},
				dataType: "json",
				async: false
			});
		} else {
			$('#GDSOrder_PseudoCityOrOfficeMaintenance_CountryName').val('');
			$('#GDSOrder_PseudoCityOrOfficeMaintenance_GlobalRegionName').val('');
		}
	});

	//If the 3rd Party Vendor checkbox is checked then the 3rd Party Vendor(s) list becomes mandatory
	if ($('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds').val() == "") {
		$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds').attr('disabled', true);
	}
	$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds_Error, #PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds_Label').hide();
	LoadGDSThirdPartyVendorIds();

	$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable').change(function () {
		LoadGDSThirdPartyVendorIds();
	});

	function LoadGDSThirdPartyVendorIds() {
		var checked = $('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable').is(':checked');
		if (checked) {
			$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds').attr('disabled', false);
			$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds_Error').show();
		} else {
			$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds').val('').attr('disabled', true);
			$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds_Error, #GDSThirdPartyVendorIds_Label').hide();
		}
	}
});

$(function () {

	//GDS Order
	$("#GDSOrder_PseudoCityOrOfficeId").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/GDSOrder.mvc/GetPseudoCityOrOfficeMaintenance", type: "POST", dataType: "json",
				data: {
					searchText: request.term,
					gdsCode: $('#GDSOrder_GDSCode option:selected').val()
				},
				success: function (data) {
					response($.map(data, function (item) {
						return {
							id: item.PseudoCityOrOfficeMaintenanceId,
							value: item.PseudoCityOrOfficeId,
							pseudoCityOrOfficeAddressId: item.PseudoCityOrOfficeAddressId,
                            firstAddressLine: item.FirstAddressLine,
                            gdsThirdPartyVendorFlag: item.GDSThirdPartyVendorFlag
						}
					}))
				}
			})
		},
		select: function (event, ui) {
			$("#GDSOrder_PseudoCityOrOfficeId").val(ui.item.value);
			$("#GDSOrder_PseudoCityOrOfficeMaintenanceId").val(ui.item.id);

			//Add Address Data
            $('#GDSOrder_PseudoCityOrOfficeAddress').val(ui.item.firstAddressLine);

            //3rd Party Vendor(s) list is only mandatory when the PCC/OID has been flagged as 3rd Party Vendor mandatory
            $('#IsGDSThirdPartyVendorIdRequired').val('');
            $('#GDSThirdPartyVendorIdError, #GDSThirdPartyVendorIdRequired').hide();

            if (ui.item.gdsThirdPartyVendorFlag == true) {
                $('#GDSThirdPartyVendorIdRequired').show();
                $('#IsGDSThirdPartyVendorIdRequired').val('1');
            }
		}
	});

});

//////////////////////////////////
//onSubmit
//////////////////////////////////
$.validator.setDefaults({
	submitHandler: function (form) {

		//if (!$(this).valid()) {
		//	return false;
		//}

		//Check GDS Order Type
		$('#GDSOrderTypeIdError').hide();
		if ($('#GDSOrderTypeId').val == '') {
			$('#GDSOrderTypeIdError').show();
			return false;
		}

		//GDS Order 3rd Party Vendor(s) list is only mandatory when the PCC/OID has been flagged as 3rd Party Vendor mandatory
		if ($('#IsGDSThirdPartyVendorIdRequired').val() == '1' && $('#GDSThirdPartyVendorIds').val() == null) {
			$('#GDSThirdPartyVendorIdError').show();
			return false;
		}

		//The FareRedistributionId field will only be mandatory if any options
		$('#GDSOrder_PseudoCityOrOfficeMaintenance_FareRedistributionId_Label').hide();
		if ($(".FareRedistributionId").is(':enabled') && $(".FareRedistributionId option").length > 1 && $(".FareRedistributionId").val() == "") {
			$('#GDSOrder_PseudoCityOrOfficeMaintenance_FareRedistributionId_Label').show();
			return false;
		} else {
			$('#GDSOrder_PseudoCityOrOfficeMaintenance_FareRedistributionId_Label').hide();
		}

		//The Pseudo City/Office ID Defined Region field will only be mandatory if any options
		$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label').hide();
		if ($("#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId option").length > 1 && $("#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId").val() == "") {
			$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label').show();
			return false;
		} else {
			$('#GDSOrder_PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label').hide();
		}

		//#PseudoCityOrOfficeMaintenance 3rd Party Vendor checkbox is checked then the 3rd Party Vendor(s) list becomes mandatory
		$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds_Label').hide();
		if ($('#GDSOrder_PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable').is(':checked') && $("#PseudoCityOrOfficeMaintenanceGDSThirdPartyVendorIds").val() === null) {
			$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds_Label').show();
			return false;
		} else {
			$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorIds_Label').hide();
		}

		//Format Dates
		if ($('#GDSOrder_DeactivationDateTime').val() == "No Deactivation Date") {
			$('#GDSOrder_DeactivationDateTime').val("");
		}

		form.submit();
	}
});
