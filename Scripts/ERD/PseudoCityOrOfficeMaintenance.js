$(document).ready(function() {

	//Navigation
	$('#menu_gdsmanagement').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	//Show DatePickers
	$('#PseudoCityOrOfficeMaintenance_VendorAssignedDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	if ($('#PseudoCityOrOfficeMaintenance_VendorAssignedDate').val() == "") {
		$('#PseudoCityOrOfficeMaintenance_VendorAssignedDate').val("No Enabled Date")
	}

	//Fare Redistribution is a drop list displaying values from the Fare Redistribution table based upon the GDS
	//If there are no fare redistribution values for the GDS chosen, this field will be greyed out
	if ($('#PseudoCityOrOfficeMaintenance_FareRedistributionId').val() == "") {
		$('#PseudoCityOrOfficeMaintenance_FareRedistributionId').attr('disabled', true);
	}
	$('#PseudoCityOrOfficeMaintenance_FareRedistributionId_Label, #PseudoCityOrOfficeMaintenance_FareRedistributionId_Error').hide();
	$('#PseudoCityOrOfficeMaintenance_GDSCode').change(function () {
		LoadPseudoCityOrOfficeMaintenance_GDSCode();
	});

	function LoadPseudoCityOrOfficeMaintenance_GDSCode() {
		var selectedGDS = $('#PseudoCityOrOfficeMaintenance_GDSCode').val();
		if (selectedGDS != "") {

			//Pseudo City/Office ID Defined Region is a drop list based upon Global Region selection
			$.ajax({
				url: "/FareRedistribution.mvc/GetFareRedistributionsByGDSCode", type: "POST", dataType: "json",
				data: { gdsCode: selectedGDS },
				success: function (data) {

					// Clear the old options
					$("#PseudoCityOrOfficeMaintenance_FareRedistributionId").find('option').remove();

					// Add a default
					$("<option value=''>Please Select...</option>").appendTo($("#PseudoCityOrOfficeMaintenance_FareRedistributionId"));

					// Load the new options
					$(data).each(function () {
						$("<option value=" + this.FareRedistributionId + ">" + this.FareRedistributionName + "</option>")
							.appendTo($("#PseudoCityOrOfficeMaintenance_FareRedistributionId"));
					});

					if ($('#PseudoCityOrOfficeMaintenance_FareRedistributionId option').length > 1) {
						$('#PseudoCityOrOfficeMaintenance_FareRedistributionId').attr('disabled', false);
						$('#PseudoCityOrOfficeMaintenance_FareRedistributionId_Error').show();
					} else {
						$('#PseudoCityOrOfficeMaintenance_FareRedistributionId').val('').attr('disabled', true);
						$('#PseudoCityOrOfficeMaintenance_FareRedistributionId_Error').hide();
					}
				}
			});

		} else {
			$('#PseudoCityOrOfficeMaintenance_FareRedistributionId').val('').attr('disabled', true);
		}
	}
	

	//Disable PseudoCityOrOfficeDefinedRegionId until Country/GlobalRegion selected, or show for edit if has a value
	if ($('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').val() == "") {
		$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').attr('disabled', true);
	}
	$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label, #PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Error').hide();
	LoadPseudoCityOrOfficeMaintenance_AmadeusId();

	//Amadeus ID is mandatory only when the GDS is Amadeus
	$('#PseudoCityOrOfficeMaintenance_AmadeusId_Label, #PseudoCityOrOfficeMaintenance_AmadeusId_Error').hide();
	$('#PseudoCityOrOfficeMaintenance_GDSCode').change(function () {
		LoadPseudoCityOrOfficeMaintenance_AmadeusId();
	});
	
	function LoadPseudoCityOrOfficeMaintenance_AmadeusId() {
		var selectedGDS = $('#PseudoCityOrOfficeMaintenance_GDSCode').val();
		if (selectedGDS == '1A') {
			$('#PseudoCityOrOfficeMaintenance_AmadeusId').attr('disabled', false);
			$('#PseudoCityOrOfficeMaintenance_AmadeusId_Error').show();
		} else {
			$('#PseudoCityOrOfficeMaintenance_AmadeusId').val('').attr('disabled', true);
			$('#PseudoCityOrOfficeMaintenance_AmadeusId_Error, #PseudoCityOrOfficeMaintenance_AmadeusId_Label').hide();
		}
	}

	//The Country and Global Region fields will be filled in for the user based upon the Address selected and will be read only
	$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId').change(function () {
		var selectedAddress = $(this).val();
		if (selectedAddress != "") {
			jQuery.ajax({
				type: "POST",
				url: "/PseudoCityOrOfficeAddress.mvc/GetPseudoCityOrOfficeAddress",
				data: {
					addressId: $("#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeAddressId").val()
				},
				success: function (data) {

					if (data.length > 0 && data[0] != null) {

						//CountryName
						var countryName = data[0].CountryName;
						if (countryName != "") {
							$('#PseudoCityOrOfficeMaintenance_CountryName').val(countryName);
						}

						//GlobalRegionName
						var globalRegionName = data[0].GlobalRegionName;
						var globalRegionCode = data[0].GlobalRegionCode;
						if (globalRegionName != "" && globalRegionCode != "") {
							$('#PseudoCityOrOfficeMaintenance_GlobalRegionName').val(globalRegionName);
							$('#PseudoCityOrOfficeMaintenance_GlobalRegionCode').val(globalRegionCode);
						}

						//Pseudo City/Office ID Defined Region is a drop list based upon Global Region selection
						$.ajax({
							url: "/PseudoCityOrOfficeMaintenance.mvc/GetPseudoCityOrOfficeDefinedRegionsByGlobalRegionCode", type: "POST", dataType: "json",
							data: { globalRegionCode: $("#PseudoCityOrOfficeMaintenance_GlobalRegionCode").val() },
							success: function (data) {

								// Clear the old options
								$("#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId").find('option').remove();

								// Add a default
								$("<option value=''>Please Select...</option>").appendTo($("#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId"));

								// Load the new options
								$(data).each(function () {
									$("<option value=" + this.PseudoCityOrOfficeDefinedRegionId + ">" + this.PseudoCityOrOfficeDefinedRegionName + "</option>")
										.appendTo($("#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId"));
								});

								//The Pseudo City/Office ID Defined Region field will only be mandatory if any options
								if ($("#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId option").length > 1) {

									// Enable dropdown
									$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').attr('disabled', false);
									$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Error').show();

								} else {

									// Disable dropdown
									$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').attr('disabled', true);
									$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Error').hide();
								}
							}
						});

					} else {
						$('#PseudoCityOrOfficeMaintenance_CountryName').val('');
						$('#PseudoCityOrOfficeMaintenance_GlobalRegionName').val('');
					}
				},
				dataType: "json",
				async: false
			});
		} else {
			$('#PseudoCityOrOfficeMaintenance_CountryName').val('');
			$('#PseudoCityOrOfficeMaintenance_GlobalRegionName').val('');
			$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId').val('').attr('disabled', true);
		}
	});

	//If the 3rd Party Vendor checkbox is checked then the 3rd Party Vendor(s) list becomes mandatory
	if($('#GDSThirdPartyVendorIds').val() == "") {
		$('#GDSThirdPartyVendorIds').attr('disabled', true);
	}
	$('#GDSThirdPartyVendorIds_Error, #GDSThirdPartyVendorIds_Label').hide();
	LoadGDSThirdPartyVendorIds();

	$('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable').change(function () {
		LoadGDSThirdPartyVendorIds();
	});

	function LoadGDSThirdPartyVendorIds() {
		var checked = $('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable').is(':checked');
		if (checked) {
			$('#GDSThirdPartyVendorIds').attr('disabled', false);
			$('#GDSThirdPartyVendorIds_Error').show();
		} else {
			$('#GDSThirdPartyVendorIds').val('').attr('disabled', true);
			$('#GDSThirdPartyVendorIds_Error, #GDSThirdPartyVendorIds_Label').hide();
		}
	}

	//Autcomplete ClientSubUnit
	var autocomplete_options = {
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
				data: { searchText: request.term, hierarchyItem: "ClientSubUnit", domainName: 'GDS Administrator', resultCount: 5000 },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
							value: item.HierarchyName,
							id: item.HierarchyCode,
							text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
						}
					}));
				}
			});
		},
		select: function (event, ui) {
			$(this).val(ui.item.value);
			$(this).next(".ClientSubUnitGuid").val(ui.item.id);
		}
	};

	//Hierarchy
	$(".ClientSubUnitName").autocomplete(autocomplete_options);

	//Add button
	$('.btn-add').live('click', function (e) {

		e.preventDefault();

		//Clone last row and add to end
		var lastItem = $('.ClientSubUnitGuidRow').last().clone();
		$('.ClientSubUnitGuidRow').last().after(lastItem);

		var newItem = $('.ClientSubUnitGuidRow').last();
		newItem.find('.ClientSubUnitName').val('').autocomplete(autocomplete_options);
		newItem.find('.ClientSubUnitGuid').val('');
	});

	//Remove btn
	$('.btn-remove').live('click', function (e) {

		e.preventDefault();

		//Remove all items but clear last remaining one
		var row_count = $('.ClientSubUnitGuidRow').length;
		if (row_count > 1) {
			$(this).parent('.ClientSubUnitGuidRow').remove();
		} else {
			$(this).parent('.ClientSubUnitGuidRow').find('.ClientSubUnitName').val('').next('ClientSubUnitGuid').val('');
		}
	});
});

$('#form0').submit(function () {

	var validItem = true;
	
	//Amadeus ID is mandatory only when the GDS is Amadeus
	$('#PseudoCityOrOfficeMaintenance_AmadeusId_Label').hide();
	if ($('#PseudoCityOrOfficeMaintenance_GDSCode').val() == "1A" && $('#PseudoCityOrOfficeMaintenance_AmadeusId').val() == "") {
		$('#PseudoCityOrOfficeMaintenance_AmadeusId_Label').show();
		return false;
	} else {
		$('#PseudoCityOrOfficeMaintenance_AmadeusId_Label').hide();
	}

	//The Pseudo City/Office ID Defined Region field will only be mandatory if any options
	$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label').hide();
	if ($("#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId option").length > 1 && $("#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId").val() == "") {
		$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label').show();
		return false;;
	} else {
		$('#PseudoCityOrOfficeMaintenance_PseudoCityOrOfficeDefinedRegionId_Label').hide();
	}

	//The FareRedistributionId field will only be mandatory if any options
	$('#PseudoCityOrOfficeMaintenance_FareRedistributionId_Label').hide();
	if ($("#PseudoCityOrOfficeMaintenance_FareRedistributionId").is(':enabled') &&
		$("#PseudoCityOrOfficeMaintenance_FareRedistributionId option").length > 1 &&
		$("#PseudoCityOrOfficeMaintenance_FareRedistributionId").val() == "") {
		$('#PseudoCityOrOfficeMaintenance_FareRedistributionId_Label').show();
		return false;
	} else {
		$('#PseudoCityOrOfficeMaintenance_FareRedistributionId_Label').hide();
	}

	//If the 3rd Party Vendor checkbox is checked then the 3rd Party Vendor(s) list becomes mandatory
	$('#GDSThirdPartyVendorIds_Label').hide();
	if ($('#PseudoCityOrOfficeMaintenance_GDSThirdPartyVendorFlagNonNullable').is(':checked') && $("#GDSThirdPartyVendorIds").val() === null) {
		$('#GDSThirdPartyVendorIds_Label').show();
		return false;
	} else {
		$('#GDSThirdPartyVendorIds_Label').hide();
	}

	if (validItem) {
		if ($('#ExpiryDate').val() == "No Expiry Date") {
			$('#ExpiryDate').val("");
		}
		return true;
	} else {
		return false
	};

});