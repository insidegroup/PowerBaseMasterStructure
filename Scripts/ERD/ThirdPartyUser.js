$(document).ready(function() {
    //Navigation
	$('#menu_gdsmanagement').click();
	$('#breadcrumb').css('width', 'auto');

	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
	
    $('#ClientTopUnitGuidError').hide();
    $('#ClientSubUnitGuidError').hide();
    $('#StateProvinceError').hide();
    $('#lblTISUserIdMsg').hide();
    $('#lblThirdPartyUser_PartnerName').hide();
    $('#lblThirdPartyUser_VendorName').hide();

    LoadThirdPartyUserTypes();

    $('#ThirdPartyUser_ThirdPartyUserTypeId').change(function () {
    	LoadThirdPartyUserTypes();
    });

	/*
	If the user selects Client from User Type drop list values, the field Client TopUnit and Client SubUnit are enabled.
	Client TopUnit and Client SubUnit are required fields if the user selects Client as a User Type.
	The Client TopUnit field is a type ahead field proposing client topunit names from the ClientTopUnit table.
	The Client SubUnit field is a type ahead field proposing client subunit names from the ClientSubUnit table associated to the selected Client TopUnit.
	Only Client SubUnits falling under the country or global region of the user’s GDS Third Party Administrator roles will be displayed.
	
	If the user selects Partner from User Type drop list values, the field Partner Name is enabled.
	Partner Name is a required field if the user selects Partner as a User Type.
	The Partner Name field is a type ahead list proposing Partner Names, and their country from the Partner table, for example “Orient Travel, Bahrein”
	Only Partners falling under the country or global region of the user’s GDS Third Party Administrator role will be displayed.
	
	If the user selects Vendor from User Type drop list values, the field Vendor Name is enabled and required.
	The Vendor Name field is a drop list containing values from the Third Party Vendor table. 
	*/

    function LoadThirdPartyUserTypes() {

    	$('#ClientTopUnitGuidError, #ClientSubUnitGuidError, #PartnerIdError, #GDSThirdPartyVendorIdError').hide();

    	var selection = $('#ThirdPartyUser_ThirdPartyUserTypeId option:selected').text();

    	if (selection == "Client") {
    		$('#ThirdPartyUser_PartnerName, #ThirdPartyUser_GDSThirdPartyVendorId').val('').attr('disabled', true);
    		$('#ThirdPartyUser_ClientTopUnitName, #ThirdPartyUser_ClientTopUnitGuid, #ThirdPartyUser_ClientSubUnitGuid').attr('disabled', false);
    		if ($('#ThirdPartyUser_ClientTopUnitName').val() != '') {
				$('#ThirdPartyUser_ClientSubUnitName').attr('disabled', false);
    		}
    		$('#ClientTopUnitGuidError, #ClientSubUnitGuidError').show();

    	} else if (selection == "Partner") {
    		$('#ThirdPartyUser_ClientTopUnitName, #ThirdPartyUser_ClientSubUnitName, #ThirdPartyUser_ClientTopUnitGuid, #ThirdPartyUser_ClientSubUnitGuid, #ThirdPartyUser_GDSThirdPartyVendorId').val('').attr('disabled', true);
    		$('#ThirdPartyUser_PartnerName').attr('disabled', false);
    		$('#PartnerIdError').show();

    	} else if (selection == "Vendor") {
    		$('#ThirdPartyUser_ClientTopUnitName, #ThirdPartyUser_ClientSubUnitName, #ThirdPartyUser_ClientTopUnitGuid, #ThirdPartyUser_ClientSubUnitGuid, #ThirdPartyUser_PartnerName').val('').attr('disabled', true);
    		$('#ThirdPartyUser_GDSThirdPartyVendorId').attr('disabled', false);
    		$('#GDSThirdPartyVendorIdError').show();

    	} else {
    		$('#ThirdPartyUser_ClientTopUnitName, #ThirdPartyUser_ClientSubUnitName, #ThirdPartyUser_ClientTopUnitGuid, #ThirdPartyUser_ClientSubUnitGuid, #ThirdPartyUser_PartnerName, #ThirdPartyUser_GDSThirdPartyVendorId').val('').attr('disabled', true);
    	}
    }

	/*
	The State/Province drop list will be empty unless a country with state/province codes in the StateProvince table has been selected as a Country.
	For example, if United Kingdom is selected as a country, the State/Province drop list will remain empty as there are no states. 
	If United States is selected as country, the State/Province drop list will contain a list of the name of States for the United States from the StateProvince table.
	*/
    if ($("#ThirdPartyUser_CountryName").val() != "") {
    	$('#ThirdPartyUser_StateProvinceCode').attr('disabled', false);
    } else {
    	$('#ThirdPartyUser_StateProvinceCode').attr('disabled', true);
    }

    if ($("#ThirdPartyUser_StateProvinceCode option").length > 1) {
    	$('#StateProvinceError').show();
    } else {
    	$('#StateProvinceError').hide();
    }

    LoadStateProvincesByCountryCode();

});

/* Partner Name Autocomplete */
$(function () {
	$("#ThirdPartyUser_PartnerName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/Partners", type: "POST", dataType: "json",
				data: { searchText: request.term, domainName: "GDS Third Party Administrator" },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.PartnerName + ", " + item.CountryName,
							value: item.PartnerId,
							name: item.PartnerName
						}
					}))
				}
			})
        },
        change: function (event, ui) {
            if (!ui.item) {
                // no item selected
                $("#ThirdPartyUser_PartnerId").val("");
            }
        },
		select: function (event, ui) {
			event.preventDefault();
			$("#ThirdPartyUser_PartnerName").val(ui.item.name);
			$("#ThirdPartyUser_PartnerId").val(ui.item.value);
		}
	});
});

/* Client Top Unit Name Autocomplete */
$(function () {
	$("#ThirdPartyUser_ClientTopUnitName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/ClientTopUnitName", type: "POST", dataType: "json",
				data: { searchText: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.ClientTopUnitName,
							value: item.ClientTopUnitGuid,
							name: item.ClientTopUnitName
						}
					}))
				}
			})
		},
		select: function (event, ui) {
			event.preventDefault();
			$("#ThirdPartyUser_ClientTopUnitName").val(ui.item.name);
			$("#ThirdPartyUser_ClientTopUnitGuid").val(ui.item.value);
			$('#ThirdPartyUser_ClientSubUnitName').attr('disabled', false);
		}
	});
});

/* Client Sub Unit Name Autocomplete */
$(function () {
	$("#ThirdPartyUser_ClientSubUnitName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/ThirdPartyUser.mvc/AutoCompleteClientTopUnitClientSubUnits", type: "POST", dataType: "json",
				data: { searchText: request.term, clientTopUnitGuid: $("#ThirdPartyUser_ClientTopUnitGuid").val() },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.HierarchyName,
							value: item.HierarchyCode,
							name: item.HierarchyName
						}
					}))
				}
			})
		},
		select: function (event, ui) {
			event.preventDefault();
			$("#ThirdPartyUser_ClientSubUnitName").val(ui.item.name);
			$("#ThirdPartyUser_ClientSubUnitGuid").val(ui.item.value);
		}
	});
});

/* LoadStateProvincesByCountryCode */
function LoadStateProvincesByCountryCode() {

	var selected = $("#ThirdPartyUser_StateProvinceCode option:selected").val();

	$.ajax({
		url: "/AutoComplete.mvc/GetStateProvincesByCountryCode", type: "POST", dataType: "json",
		data: { countryCode: $("#ThirdPartyUser_CountryCode").val() },
		success: function (data) {

			// Clear the old options
			$("#ThirdPartyUser_StateProvinceCode").find('option').remove();

			// Add a default
			$("<option value=''>Please Select...</option>").appendTo($("#ThirdPartyUser_StateProvinceCode"));

			// Load the new options
			$(data).each(function () {
				$("<option value=" + this.StateProvinceCode + ">" + this.Name + "</option>").appendTo($("#ThirdPartyUser_StateProvinceCode"));
			});

			// Show dropdown
			if ($("#ThirdPartyUser_StateProvinceCode option").length > 1) {
				$('#ThirdPartyUser_StateProvinceCode').attr('disabled', false);
				$('#StateProvinceError').show();

				//Reapply Edit
				if (selected != null) {
					$("#ThirdPartyUser_StateProvinceCode").val(selected)
				}

			} else {
				$('#ThirdPartyUser_StateProvinceCode').attr('disabled', true);
				$('#StateProvinceError').hide();
			}

		}
	});
}

/* Country Name Autocomplete */
$(function () {
	$("#ThirdPartyUser_CountryName").autocomplete({
		source: function (request, response) {
			$.ajax({
			    url: "/AutoComplete.mvc/LocationCountriesByRole", type: "POST", dataType: "json",
			    data: { searchText: request.term, roleName: "GDS Third Party Administrator" },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.CountryName,
							id: item.CountryCode
						}
					}))
				}
			})
		},
		mustMatch: true,
		select: function (event, ui) {
			$("#ThirdPartyUser_CountryCode").val(ui.item.id);
			LoadStateProvincesByCountryCode();
		}
	});
});

//Submit Form Validation
$('#form0').submit(function () {

	$('#lblTISUserIdMsg').hide();

    $('#lblThirdPartyUser_PartnerName').text("Partner Name required").hide();
    $('#ThirdPartyUser_PartnerName').addClass('input-validation-valid');
    $('#ThirdPartyUser_PartnerName').removeClass('input-validation-error');

    $('#lblThirdPartyUser_VendorName').hide();
    $('#ThirdPartyUser_GDSThirdPartyVendorId').addClass('input-validation-valid');
    $('#ThirdPartyUser_GDSThirdPartyVendorId').removeClass('input-validation-error');

    $("#lblStateProvinceCodeMsg").addClass('field-validation-valid');
    $("#lblStateProvinceCodeMsg").removeClass('field-validation-error');
    $("#lblStateProvinceCodeMsg").hide()

	var valid = false;

	//ThirdPartyUserName
	var thirdPartyUser_ThirdPartyUserId = $("#ThirdPartyUser_ThirdPartyUserId").val() != null && $("#ThirdPartyUser_ThirdPartyUserId").val() != "" ? $("#ThirdPartyUser_ThirdPartyUserId").val() : 0;
	var thirdPartyUser_ThirdPartyUserName = $("#ThirdPartyUser_ThirdPartyName").val();

	if (thirdPartyUser_ThirdPartyUserName != "") {
		jQuery.ajax({
			type: "POST",
			url: "/GroupNameBuilder.mvc/IsAvailableThirdPartyUserName",
			data: {
				groupName: thirdPartyUser_ThirdPartyUserName,
				id: thirdPartyUser_ThirdPartyUserId
			},
			success: function (data) {
				valid = data;
			},
			dataType: "json",
			async: false
		});
		if (!valid) {
			$("#lblThirdPartyUserMsg").removeClass('field-validation-valid');
			$("#lblThirdPartyUserMsg").addClass('field-validation-error');
			if ($("#ThirdPartyUser_ThirdPartyUserId").val() == "0") {//Create
				$("#lblThirdPartyUserMsg").text("This name has already been used, please choose a different name.");
			} else {
				if ($("#ThirdPartyUser_ThirdPartyUserName").val() != "") {
					$("#lblThirdPartyUserMsg").text("This name has already been used, please choose a different name.");
				}
			}
			return false;
		} else {
			$("#lblThirdPartyUserMsg").text("");
		}
	}


	if ($("#ThirdPartyUser_CountryName").val() != "") {
		jQuery.ajax({
			type: "POST",
			url: "/Hierarchy.mvc/IsValidCountry",
			data: { searchText: $("#ThirdPartyUser_CountryName").val() },
			success: function (data) {

				if (!jQuery.isEmptyObject(data)) {
					valid = true;
				}
			},
			dataType: "json",
			async: false
		});

		if (!valid) {
			$("#lblCountryNameMsg").removeClass('field-validation-valid');
			$("#lblCountryNameMsg").addClass('field-validation-error');
			$("#lblCountryNameMsg").text("This is not a valid country.");
			$("#lblCountryNameMsg").show();
			return false;
		}
	}

	//The State/Province should be mandatory if of a country in state/province table
	if ($("#ThirdPartyUser_StateProvinceCode option").length > 1 && $("#ThirdPartyUser_StateProvinceCode").val() == "") {
		$("#lblStateProvinceCodeMsg").removeClass('field-validation-valid');
		$("#lblStateProvinceCodeMsg").addClass('field-validation-error');
		$("#lblStateProvinceCodeMsg").text("State/Province is required.");
		$("#lblStateProvinceCodeMsg").show();
		return false;
	}

	if ($("#ThirdPartyUser_StateProvinceCode option:selected").val() != "") {

		jQuery.ajax({
			type: "POST",
			url: "/StateProvince.mvc/IsValidStateProvince",
			data: { searchText: $("#ThirdPartyUser_StateProvinceCode").val(), countryCode: $("#ThirdPartyUser_CountryCode").val() },
			success: function (data) {

				if (!jQuery.isEmptyObject(data)) {
					valid = true;
				}
			},
			dataType: "json",
			async: false
		});
		if (!valid) {
			$("#lblStateProvinceCodeMsg").removeClass('field-validation-valid');
			$("#lblStateProvinceCodeMsg").addClass('field-validation-error');
			$("#lblStateProvinceCodeMsg").text("This is not a valid entry.");
			$("#lblStateProvinceCodeMsg").show();
			return false;
		}
	}

	var userTypeSelection = $('#ThirdPartyUser_ThirdPartyUserTypeId option:selected').text();

    if (userTypeSelection == "Partner") {

        if ($('#ThirdPartyUser_PartnerName').val() == '') {
            $('#ThirdPartyUser_PartnerName').removeClass('input-validation-valid');
            $('#ThirdPartyUser_PartnerName').addClass('input-validation-error');
            $("#lblThirdPartyUser_PartnerName").show();
            return false;

        } else {

            if ($("#ThirdPartyUser_PartnerId").val() != "") {
                jQuery.ajax({
                    type: "POST",
                    url: "/Validation.mvc/IsValidPartner",
                    data: {
                        id: $("#ThirdPartyUser_PartnerId").val(),
                        partnerName: $("#ThirdPartyUser_PartnerName").val()
                    },
                    success: function (data) {
                        valid = data;
                    },
                    dataType: "json",
                    async: false
                });
            } else {
                $("#lblThirdPartyUser_PartnerName").text("Valid Partner required. Please reselect Partner");
                valid = false;
            }

            if (!valid) {
                $('#ThirdPartyUser_PartnerName').removeClass('input-validation-valid');
                $('#ThirdPartyUser_PartnerName').addClass('input-validation-error');
                $("#lblThirdPartyUser_PartnerName").show();
                return false;
            }
        }

	} else if (userTypeSelection == "Vendor") {
	    if ($('#ThirdPartyUser_GDSThirdPartyVendorId').val() == '') {

	        $('#ThirdPartyUser_GDSThirdPartyVendorId').removeClass('input-validation-valid');
            $('#ThirdPartyUser_GDSThirdPartyVendorId').addClass('input-validation-error');
            $("#lblThirdPartyUser_VendorName").show();
	        return false;
	    }
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	//If form is valid, send data to the Traveler Identity Store
	if (valid) {
		return traveller_identity_service();
	}

});