$(document).ready(function () {
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    /* State/Provinces */
    if ($("#Location_CountryCode").val() !== "") {
        $('#Address_StateProvinceCode').attr('disabled', false);
    } else {
        $('#Address_StateProvinceCode').attr('disabled', true);
    }

    if ($("#Address_StateProvinceCode option").length > 1) {
        $('.stateProvinceCodeError').show();
    } else {
        $('.stateProvinceCodeError').hide();
    }
});

function LoadStateProvincesByCountryCode() {

    var selected = $("#Address_StateProvinceCode option:selected").val();

    $.ajax({
        url: "/AutoComplete.mvc/GetStateProvincesByCountryCode", type: "POST", dataType: "json",
        data: { countryCode: $("#Location_CountryCode").val() },
        success: function (data) {

            // Clear the old options
            $("#Address_StateProvinceCode").find('option').remove();

            // Add a default
            $("<option value=''>Please Select...</option>").appendTo($("#Address_StateProvinceCode"));

            // Load the new options
            $(data).each(function () {
                $("<option value=" + this.StateProvinceCode + ">" + this.Name + "</option>").appendTo($("#Address_StateProvinceCode"));
            });

            // Show dropdown
            if ($("#Address_StateProvinceCode option").length > 1) {
                $('#Address_StateProvinceCode').attr('disabled', false);
                $('.stateProvinceCodeError').show();

                //Reapply Edit
                if (selected != null) {
                    $("#Address_StateProvinceCode").val(selected)
                }

            } else {
                $('#Address_StateProvinceCode').attr('disabled', true);
                $('.stateProvinceCodeError').hide();
            }

        }
    });
}

$(function() {
	$("#Location_CountryName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/LocationCountries", type: "POST", dataType: "json",
                data: { searchText: request.term},
                success: function(data) {
                    response($.map(data, function(item) {
                        return { 
                                label: item.CountryName, 
                                id: item.CountryCode
                               }
                    }))
                }
            })
        },
        select: function (event, ui) {

            $("#Location_CountryCode, #Address_CountryCode").val(ui.item.id);
			$("#Location_CountryRegionId").find('option').remove();

            //Load Regions
            $.ajax({
                url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
                data: { countryCode: $("#Location_CountryCode").val() },
                success: function (data) {
                    $(data).each(function () {
                        $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                    });
                }
            });

            //Load State/Provinces
            LoadStateProvincesByCountryCode();
        }
    });
});

$(function () {
	$("#Location_CountryName").change(function () {
        $("#Location_CountryRegionId").find('option').remove();

        $.ajax({
            url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
            data: { countryCode: $("#Location_CountryCode") },
            success: function (data) {
                $(data).each(function () {
                    $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                });
            }
        })

    });
});


$('#form0').submit(function () {

	var validLocation = false;
    var validCountry = false;
    var validStateProvince = false;

    if ($("#Location_LocationName").val() != "" && $("#Location_CountryRegionId").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Location.mvc/IsAvailableLocation",
            data: {
            	countryRegionId: $("#Location_CountryRegionId").val(),
            	locationName: $("#Location_LocationName").val(),
            	locationId: $("#Location_LocationId").val()
            },
            success: function (data) {
                validLocation = data;
            },
            dataType: "json",
            async: false
        });


        if (validLocation) {
            $("#lblLocationNameMsg").text("");
        } else {
            $("#lblLocationNameMsg").addClass('field-validation-error');
            $("#lblLocationNameMsg").text("This Location already exists in this Country Region.");
            return false;
        };
    }

    if ($("#Location_CountryName").val() != "") {
        $.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidAdminUserCountry",
            data: { countryName: $("#Location_CountryName").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validCountry = true;
                    $("#Location_CountryCode, #Address_CountryCode").val(data.CountryCode);
                    $("#lblCountryNameMsg").text("");
                }
            },
            dataType: "json",
            async: false
        });

        if (!validCountry) {
            $("#lblCountryNameMsg").addClass('field-validation-error');
            $("#lblCountryNameMsg").text("This Is Not a Valid Country");
        }

    }

    //The State/Province should be mandatory if of a country in state/province table
    $("#lblStateProvinceCodeMsg").hide();
    $("#lblStateProvinceMsg").hide();

    if ($("#Address_StateProvinceCode option").length > 1 && $("#Address_StateProvinceCode").val() == "") {
        $("#lblStateProvinceCodeMsg").removeClass('field-validation-valid');
        $("#lblStateProvinceCodeMsg").addClass('field-validation-error');
        $("#lblStateProvinceCodeMsg").text("State/Province is required.");
        $("#lblStateProvinceCodeMsg").show();
        return false;
    }

    if ($("#Address_StateProvinceCode option:selected").val() !== undefined) {

        jQuery.ajax({
            type: "POST",
            url: "/StateProvince.mvc/IsValidStateProvince",
            data: {
                searchText: $("#Address_StateProvinceCode option:selected").text(),
                countryCode: $("#Location_CountryCode").val()
            },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validStateProvince = true;
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
        validStateProvince = true;
    }

    if (validCountry && validLocation && validStateProvince) {
        return true;
    } else {
        return false;
    };


});