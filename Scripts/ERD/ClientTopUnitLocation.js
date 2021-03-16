$(document).ready(function() {
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //var locationName = $("#Location_LocationName").val();
    //if (locationName != '') {
    //	$('#Address_AddressLocationName').val(locationName);
    //}

})

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
            $.ajax({
                url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
                data: { countryCode: $("#Location_CountryCode").val() },
                success: function(data) {
                    $(data).each(function() {
                        $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                    });
                }
            })
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

	//var locationName = $("#Location_LocationName").val();
	//if (locationName != '') {
	//	$('#Address_AddressLocationName').val(locationName);
	//}
	
	var validLocation = false;
    var validCountry = false;

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
    

    if (validCountry && validLocation) {
        return true;
    } else {
        return false;
    };


});