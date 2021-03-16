$(document).ready(function() {

	$('#policyLocationError').hide();
    
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    
    if ($("#GlobalFlag").is(':checked')) {
        $("#LocationName").attr('disabled', 'disabled');
    }
    
    if ($("#TravelPortTypeId").val() == "") {
        $("#TravelPortName").attr("disabled", true);
    } else {
        $("#TravelPortName").removeAttr("disabled");
    }
})

$("#GlobalFlag").click(function() {
    $("#LocationName").val("");
    $("#LocationCode").val("");

    $("#LocationName_validationMessage").text("");
    if ($("#GlobalFlag").is(':checked')) {
        $("#LocationName").attr('disabled', 'disabled');
    } else {
        $("#LocationName").removeAttr("disabled");
    }
});

$("#TravelPortTypeId").change(function() {

    $("#TravelPortName").val("");
    if ($("#TravelPortTypeId").val() == "") {
        $("#TravelPortName").attr("disabled", true);
    } else {
        $("#TravelPortName").removeAttr("disabled");
    }
});

$(function() {

	$('#policyLocationError').hide();

	$("#Location_validationMessage").text("");
    $("#LocationName").autocomplete({

        source: function(request, response) {
            $.ajax({
                url: "/PolicyLocation.mvc/AutoCompleteLocation", type: "POST", dataType: "json",
                data: { searchText: request.term, maxResults: 10 },
                success: function(data) {

                    response($.map(data, function(item) {
                        return {
                        	label: item.Name + " (" + item.Code  + "), " + item.Parent,
                            value: item.Name,
                            code: item.Code,
                            codetype: item.CodeType
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#lblLocation").text(ui.item.label); //label
            $("#LocationType").val(ui.item.codetype); //hidden
            $("#LocationCode").val(ui.item.code); //hidden
            $("#GlobalFlag").attr('checked', false);

			//If a policy already exists with chosen location code - prevent creating
            var policyLocationID = $("#PolicyLocationId").length ? $("#PolicyLocationId").val() : 0;
            jQuery.ajax({
            	type: "POST",
            	url: "/PolicyLocation.mvc/IsAvailablePolicyLocationCode",
            	data: { locationCode: $("#LocationCode").val(), locationType: $('#LocationType').val(), policyLocationID: policyLocationID },
            	success: function (data) {
            		if (data == "true") {
            			$('#policyLocationError').hide();
            		} else {
            			$('#policyLocationError').show();
            		}
            	},
            	dataType: "json",
            	async: false
            }); 
        }
    });

});


$(function() {
$("#TravelPortCode").val("");
    $("#TravelPortName").autocomplete({

        source: function(request, response) {
            $.ajax({
            url: "/PolicyLocation.mvc/AutoCompleteTravelPortName", type: "POST", dataType: "json",
            data: { searchText: request.term, typeId: $("#TravelPortTypeId").val(), maxResults: 10 },
                success: function(data) {

                    response($.map(data, function(item) {
                        return {
                        label: item.TravelPortName + ",(" + item.TravelPortCode +")",
                        value: item.TravelPortName,
                        id: item.TravelPortCode
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#TravelPortCode").val(ui.item.id); //hidden
        }
    });

});

$('#form0').submit(function () {
	var validLocation = true;
    if (!$("#GlobalFlag").is(':checked')) {
        validLocation = false;

        if ($("#LocationName").val() == "") {
            validLocation = false;
        } else {
            jQuery.ajax({
                type: "POST",
                url: "/PolicyLocation.mvc/IsValidLocation",
                data: { locationName: $("#LocationName").val()},
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validLocation = true;
                    }
                },
                dataType: "json",
                async: false
            });
        }
    };
    if (!validLocation) {
        $("#LocationCode").val("");
        $("#lblLocation").removeClass('field-validation-valid');
        $("#lblLocation").addClass('field-validation-error');
        $("#lblLocation").text("This is not a valid Location.");
        return false;
    } else {
        $("#lblLocation").text("");
    }

	//Check TravelPortType
    var validTripType = true;
    if ($("#TravelPortTypeId option:selected").val() != "") {
    	if ($('#TravelPortName').val() == "") {
    		$('#lblTravelPortNameMsg').addClass('error').text('Travel Port Name Required').show();
    		validTripType = false;
    	}
    }

	//If a policy already exists with chosen location code - prevent creating
    var validPolicyLocaton = true;
    var policyLocationID = $("#PolicyLocationId").length ? $("#PolicyLocationId").val() : 0;
    if (validLocation) {
    	jQuery.ajax({
    		type: "POST",
    		url: "/PolicyLocation.mvc/IsAvailablePolicyLocationCode",
    		data: { locationCode: $("#LocationCode").val(), locationType: $('#LocationType').val(), policyLocationID: policyLocationID },
    		success: function (data) {
    			if (data == "true") {
    				$('#policyLocationError').hide();
    				validPolicyLocaton = true;
    			} else {
    				$('#policyLocationError').show();
    				validPolicyLocaton = false;
    			}
    		},
    		dataType: "json",
    		async: false
    	});
    }

    if (validLocation && validTripType && validPolicyLocaton) {
        return true;
    } else {
        return false
    };

});