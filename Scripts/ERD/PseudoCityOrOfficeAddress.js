$(document).ready(function() {

	//Navigation
	$('#menu_gdsmanagement').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	/*
	The State/Province drop list will be empty unless a country with state/province codes in the StateProvince table has been selected as a Country.
	For example, if United Kingdom is selected as a country, the State/Province drop list will remain empty as there are no states. 
	If United States is selected as country, the State/Province drop list will contain a list of the name of States for the United States from the StateProvince table.
	*/
	if ($("#PseudoCityOrOfficeAddress_CountryCode option:selected").val() != "") {
		$('#PseudoCityOrOfficeAddress_StateProvinceCode').attr('disabled', false);
	} else {
		$('#PseudoCityOrOfficeAddress_StateProvinceCode').attr('disabled', true);
	}

	if ($("#PseudoCityOrOfficeAddress_StateProvinceCode option").length > 1) {
		$('.stateProvinceCodeError').show();
	} else {
		$('.stateProvinceCodeError').hide();
	}

	LoadStateProvincesByCountryCode();

	$("#PseudoCityOrOfficeAddress_CountryCode").change(function() {
		LoadStateProvincesByCountryCode();
	});
});

$('#form0').submit(function () {

	var validItem = false;
	var validGroupName = false;

	//The first address line and city combination must be unique or it will give a message to the user

	$("#lblPseudoCityOrOfficeAddressMsg").hide();

	var pseudoCityOrOfficeAddress_PseudoCityOrOfficeAddressId = $("#PseudoCityOrOfficeAddress_PseudoCityOrOfficeAddressId").length > 0 ? $("#PseudoCityOrOfficeAddress_PseudoCityOrOfficeAddressId").val() : 0;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailablePseudoCityOrOfficeAddressName",
		data: {
			id: pseudoCityOrOfficeAddress_PseudoCityOrOfficeAddressId,
			cityName: $("#PseudoCityOrOfficeAddress_CityName").val(),
			firstLineAddress: $('#PseudoCityOrOfficeAddress_FirstAddressLine').val()
		},
		success: function (data) {
			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {
		$("#lblPseudoCityOrOfficeAddressMsg").removeClass('field-validation-valid');
		$("#lblPseudoCityOrOfficeAddressMsg").addClass('field-validation-error');
		$("#lblPseudoCityOrOfficeAddressMsg").text("First Line Address and City combination must be unique.").show();
		return false;
	} else {
		$("#lblPseudoCityOrOfficeAddressMsg").text("");
	}

	//The State/Province should be mandatory if of a country in state/province table
	$("#lblStateProvinceCodeMsg").hide();
	$("#lblStateProvinceMsg").hide();

	if ($("#PseudoCityOrOfficeAddress_StateProvinceCode option").length > 1 && $("#PseudoCityOrOfficeAddress_StateProvinceCode").val() == "") {
		$("#lblStateProvinceCodeMsg").removeClass('field-validation-valid');
		$("#lblStateProvinceCodeMsg").addClass('field-validation-error');
		$("#lblStateProvinceCodeMsg").text("State/Province is required.");
		$("#lblStateProvinceCodeMsg").show();
	}

	if ($("#PseudoCityOrOfficeAddress_StateProvince option:selected").val() != undefined) {

		jQuery.ajax({
			type: "POST",
			url: "/StateProvince.mvc/IsValidStateProvince",
			data: {
				searchText: $("#PseudoCityOrOfficeAddress_StateProvinceCode option:selected").text(),
				countryCode: $("#PseudoCityOrOfficeAddress_CountryCode").val()
			},
			success: function (data) {

				if (!jQuery.isEmptyObject(data)) {
					validItem = true;
				}
			},
			dataType: "json",
			async: false
		});
		if (!validItem) {
			$("#lblStateProvinceMsg").removeClass('field-validation-valid');
			$("#lblStateProvinceMsg").addClass('field-validation-error');
			$("#lblStateProvinceMsg").text("This is not a valid entry.");
			$("#lblStateProvinceMsg").show();
			return false;
		}
	} else {
		validItem = true;
	}

	if (validItem && validGroupName) {
		return true;
	} else {
		return false
	};

});

function LoadStateProvincesByCountryCode() {

	var selected = $("#PseudoCityOrOfficeAddress_StateProvinceCode option:selected").val();

	$.ajax({
		url: "/AutoComplete.mvc/GetStateProvincesByCountryCode", type: "POST", dataType: "json",
		data: { countryCode: $("#PseudoCityOrOfficeAddress_CountryCode").val() },
		success: function (data) {

			// Clear the old options
			$("#PseudoCityOrOfficeAddress_StateProvinceCode").find('option').remove();

			// Add a default
			$("<option value=''>Please Select...</option>").appendTo($("#PseudoCityOrOfficeAddress_StateProvinceCode"));

			// Load the new options
			$(data).each(function () {
				$("<option value=" + this.StateProvinceCode + ">" + this.Name + "</option>").appendTo($("#PseudoCityOrOfficeAddress_StateProvinceCode"));
			});

			// Show dropdown
			if ($("#PseudoCityOrOfficeAddress_StateProvinceCode option").length > 1) {
				$('#PseudoCityOrOfficeAddress_StateProvinceCode').attr('disabled', false);
				$('.stateProvinceCodeError').show();

				//Reapply Edit
				if (selected != null) {
					$("#PseudoCityOrOfficeAddress_StateProvinceCode").val(selected)
				}

			} else {
				$('#PseudoCityOrOfficeAddress_StateProvinceCode').attr('disabled', true);
				$('.stateProvinceCodeError').hide();
			}

		}
	});
}