$(document).ready(function () {
	$('#menu_admin').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	/*
	The State/Province drop list will be empty unless a country with state/province codes in the StateProvince table has been selected as a Country.
	For example, if United Kingdom is selected as a country, the State/Province drop list will remain empty as there are no states. 
	If United States is selected as country, the State/Province drop list will contain a list of the name of States for the United States from the StateProvince table.
	*/
	if ($("#CountryName").val() != "") {
		$('#StateProvinceCode').attr('disabled', false);
	} else {
		$('#StateProvinceCode').attr('disabled', true);
	}

	if ($("#StateProvinceCode option").length > 1) {
		$('.stateProvinceCodeError').show();
	} else {
		$('.stateProvinceCodeError').hide();
	}

	LoadStateProvincesByCountryCode();

});

$('#form0').submit(function () {

	var validItem = false;
	var validCity = false;

	$("#lblCityCodeMsg").hide();
	$("#lblCountryNameMsg").hide();
	$("#lblStateProvinceCodeMsg").hide();
	$("#lblStateProvinceMsg").hide();

	//Only validate CityCode on Create - Edit is hidden field
	if ($('input[type="text"][name="CityCode"]').length > 0 && $('input[type="text"][name="CityCode"]').val() != "") {
		jQuery.ajax({
			type: "POST",
			url: "/Validation.mvc/IsValidCityCode",
			data: { cityCode: $("#CityCode").val() },
			success: function (data) {
				if (data == null) {
					validCity = true;
				} else if(data.city != null) {
					validCity = false;
				}				
			},
			dataType: "json",
			async: false
		});

		if (!validCity) {
			$("#lblCityCodeMsg").removeClass('field-validation-valid');
			$("#lblCityCodeMsg").addClass('field-validation-error');
			$("#lblCityCodeMsg").text("A City already exists for this code. Please amend the existing City.");
			$("#lblCityCodeMsg").show();
			return false;
		}
	} else {
		validCity = true;
	}

	if ($("#CountryName").val() != "") {
		jQuery.ajax({
			type: "POST",
			url: "/Hierarchy.mvc/IsValidCountry",
			data: { searchText: $("#CountryName").val() },
			success: function (data) {

				if (!jQuery.isEmptyObject(data)) {
					validItem = true;
				}
			},
			dataType: "json",
			async: false
		});

		if (!validItem) {
			$("#lblCountryNameMsg").removeClass('field-validation-valid');
			$("#lblCountryNameMsg").addClass('field-validation-error');
			$("#lblCountryNameMsg").text("This is not a valid country.");
			$("#lblCountryNameMsg").show();
			return false;
		}
	}

	//The State/Province should be mandatory if of a country in state/province table
	if ($("#StateProvinceCode option").length > 1 && $("#StateProvinceCode").val() == "") {
		$("#lblStateProvinceCodeMsg").removeClass('field-validation-valid');
		$("#lblStateProvinceCodeMsg").addClass('field-validation-error');
		$("#lblStateProvinceCodeMsg").text("State/Province is required.");
		$("#lblStateProvinceCodeMsg").show();
	}

	if ($("#StateProvince option:selected").val() != "") {

		jQuery.ajax({
			type: "POST",
			url: "/StateProvince.mvc/IsValidStateProvince",
			data: { searchText: $("#StateProvince").val(), countryCode: $("#CountryCode").val() },
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
	}

	if (validItem && validCity) {
		return true;
	} else {
		return false
	};
	
});

/////////////////////////////////////////////////////////
// BEGIN AUTOCOMPLETES
/////////////////////////////////////////////////////////
$(function () {

	$("#CountryName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/Countries", type: "POST", dataType: "json",
				data: { searchText: request.term },
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

			$("#CountryName").val(ui.item.label);
			$("#CountryCode").val(ui.item.id);
			$("#lblStateProvinceCodeMsg").hide();

			LoadStateProvincesByCountryCode();
			
		}
	});

	
});
/////////////////////////////////////////////////////////
// END AUTOCOMPLETES
/////////////////////////////////////////////////////////

function LoadStateProvincesByCountryCode() {

	var selected = $("#StateProvinceCode option:selected").val();

	$.ajax({
		url: "/AutoComplete.mvc/GetStateProvincesByCountryCode", type: "POST", dataType: "json",
		data: { countryCode: $("#CountryCode").val() },
		success: function (data) {

			// Clear the old options
			$("#StateProvinceCode").find('option').remove();

			// Add a default
			$("<option value=''>Please Select...</option>").appendTo($("#StateProvinceCode"));

			// Load the new options
			$(data).each(function () {
				$("<option value=" + this.StateProvinceCode + ">" + this.Name + "</option>").appendTo($("#StateProvinceCode"));
			});

			// Show dropdown
			if ($("#StateProvinceCode option").length > 1) {
				$('#StateProvinceCode').attr('disabled', false);
				$('.stateProvinceCodeError').show();

				//Reapply Edit
				if (selected != null) {
					$("#StateProvinceCode").val(selected)
				}

			} else {
				$('#StateProvinceCode').attr('disabled', true);
				$('.stateProvinceCodeError').hide();
			}

		}
	});
}