$(document).ready(function () {
	$('#menu_admin').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	/*
	The State/Province drop list will be empty unless a country with state/province codes in the StateProvince table has been selected as a Country.
	For example, if United Kingdom is selected as a country, the State/Province drop list will remain empty as there are no states. 
	If United States is selected as country, the State/Province drop list will contain a list of the name of States for the United States from the StateProvince table.
	*/

	LoadStateProvincesByCountryCode();

    $('#CountryCode').change(function () {
        LoadStateProvincesByCountryCode();
    });

});

$('#form0').submit(function () {

	var validItem = false;
    var validClientTopUnitGuid = true;

    if ($("#ClientTopUnitName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Hierarchy.mvc/IsValidClientTopUnit",
            data: { searchText: $("#ClientTopUnitName").val() },
            success: function (data) {

                if (jQuery.isEmptyObject(data)) {
                    validClientTopUnitGuid = false;
                }
            },
            dataType: "json",
            async: false
        });
        if (!validClientTopUnitGuid) {
            $("#lblClientTopUnitGuid_Msg").removeClass('field-validation-valid');
            $("#lblClientTopUnitGuid_Msg").addClass('field-validation-error');
            $("#lblClientTopUnitGuid_Msg").text("This is not a valid entry");
        } else {
            $("#lblClientTopUnitGuid_Msg").text("");
        }
    }

    if ($("#CountryCode").val() != "") {
		jQuery.ajax({
			type: "POST",
			url: "/Hierarchy.mvc/IsValidCountry",
            data: { searchText: $("#CountryCode option:selected").text() },
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
			$("#lblCountryNameMsg").text("This is not a valid country");
			$("#lblCountryNameMsg").show();
			return false;
		}
	}

	//The State/Province should be mandatory if of a country in state/province table
    if ($("#StateProvinceCode option").length > 1 && $("#StateProvinceCode").val() == "") {
        $("#lblStateProvinceCodeMsg").removeClass('field-validation-valid');
        $("#lblStateProvinceCodeMsg").addClass('field-validation-error');
        $("#lblStateProvinceCodeMsg").text("State/Province is required");
        $("#lblStateProvinceCodeMsg").show();
        return false;
    } else {
        $("#lblStateProvinceCodeMsg").text("");
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
            $("#lblStateProvinceMsg").text("This is not a valid entry");
            $("#lblStateProvinceMsg").show();
            return false;
        } else {
            $("#lblStateProvinceMsg").text("");
        }
	}

    if (validItem && validClientTopUnitGuid) {
		return true;
	} else {
		return false
	};
	
});

/////////////////////////////////////////////////////////
// BEGIN AUTOCOMPLETES
/////////////////////////////////////////////////////////
$(function () {

    $("#ClientTopUnitName").autocomplete({
        minLength: 2,
        source: function (request, response) {
            $.ajax({
            	url: "/AutoComplete.mvc/ClientTopUnitName", type: "POST", dataType: "json",
                data: { searchText: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                        	label: item.ClientTopUnitName,
                        	value: item.ClientTopUnitName,
                            id: item.ClientTopUnitGuid,
                            text: item.ClientTopUnitName
                        }
                    }))
                }
            });
        },
        select: function (event, ui) {
            $("#ClientTopUnitGuid").val(ui.item.id);
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