
$(document).ready(function () {
    $('#menu_servicingoptions').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //for pages with long breadcrumb and no search box
	$('#breadcrumb').css('width', '725px');
	$('#search').css('width', '5px');


    //no item selected
    if ($("#ServicingOptionId").val() == "") {
    	$('#trGDSs, .parameter-fields').css('display', 'none');
        $("#selServicingOptionItemValue").find('option').remove();
        $('#txtServicingOptionItemValue').val("");
        $('#txtServicingOptionItemValue').attr('disabled', 'disabled');
        return;
    }

   

    LoadServicingOptionValues();

});


function LoadServicingOptionValues() {

    $('#selServicingOptionItemValidation').html("");
    $('#txtServicingOptionItemValidation').html("");
    $('#GDSCodeValidation').html(""); 

	//Check if ServicingOption requires a GDS adding
    $.ajax({
    	url: "/ServicingOptionItemValue.mvc/GetServicingOptionGDSRequired", type: "POST", dataType: "json",
    	data: { servicingOptionId: $("#ServicingOptionId").val() },
    	success: function (data) {
    		$('#ServicingOptionGDSRequired').val(data);
    		if (data == true) {
    			$('#trGDSs').css('display', '');
    		} else {
    			$('#trGDSs').css('display', 'none');
    		}
    	}
    });

	//Check if ServicingOption requires Parameters adding
    $.ajax({
    	url: "/ServicingOptionItemValue.mvc/GetServicingOptionParametersRequired", type: "POST", dataType: "json",
    	data: { servicingOptionId: $("#ServicingOptionId").val() },
    	success: function (data) {
    		if (data == true) {
    			$('.parameter-fields').css('display', '');
    		} else {
    			$('.parameter-fields').css('display', 'none');
    		}
    	}
    });

    //item selected
    $.ajax({
        url: "/ServicingOptionItemValue.mvc/GetServicingOptionItemValues", type: "POST", dataType: "json",
        data: { servicingOptionId: $("#ServicingOptionId").val(), id: $("#ServicingOptionGroupId").val() },
        success: function (data) {
            if (data.length == 0) { //show text box
                $('#txtServicingOptionItemValue').attr('disabled', '');
                $('#txtServicingOptionItemValue').val($('#ServicingOptionItemValue').val());
                $('#trTextboxServicingOptionItemValue').css('display', '');
                $('#trSelectListServicingOptionItemValue').css('display', 'none');
                $("#selServicingOptionItemValue").find('option').remove();
            } else { //show select listbox
                $('#txtServicingOptionItemValue').attr('disabled', 'disabled');
                $('#trTextboxServicingOptionItemValue').css('display', 'none');
                $('#trSelectListServicingOptionItemValue').css('display', '');

                $("#selServicingOptionItemValue").find('option').remove();
                $("<option value=''>Please Select...</option>").appendTo($("#selServicingOptionItemValue"));
                $(data).each(function () {
                    if (this.ServicingOptionItemValue1 == $('#ServicingOptionItemValue').val()) {
                        $("<option value=" + this.ServicingOptionId + " selected>" + this.ServicingOptionItemValue1 + "</option>").appendTo($("#selServicingOptionItemValue"));

                    } else {
                        if ($("#ServicingOptionId").val() == "180" && this.ServicingOptionItemValue1 == "Exact" && $('#ServicingOptionItemValue').val()=="") {
                            $("<option value=" + this.ServicingOptionId + " selected>" + this.ServicingOptionItemValue1 + "</option>").appendTo($("#selServicingOptionItemValue"));
                        } else {
                            $("<option value=" + this.ServicingOptionId + ">" + this.ServicingOptionItemValue1 + "</option>").appendTo($("#selServicingOptionItemValue"));
                        }
                    }

                });

               
            }
        }
    })
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
}


$('#form0').submit(function () {

    var valid = true;
    $('#ServicingOptionItemValue').val("");

    if ($('#trTextboxServicingOptionItemValue').is(":visible")) {
        if ($('#txtServicingOptionItemValue').val() == "") {
            $("#txtServicingOptionItemValidation").addClass('field-validation-error');
            $('#txtServicingOptionItemValidation').html("Value Required");
            valid = false;
        } else {
            $('#ServicingOptionItemValue').val($('#txtServicingOptionItemValue').val());
        }
    }
    if ($('#trSelectListServicingOptionItemValue').is(":visible")) {
        if ($("#selServicingOptionItemValue").val() == "") {
            $("#selServicingOptionItemValidation").addClass('field-validation-error');
            $('#selServicingOptionItemValidation').html("Value Required");
            valid = false;
        } else {
            $('#ServicingOptionItemValue').val($("#selServicingOptionItemValue :selected").text());
        }
    }

    if($('#ServicingOptionGDSRequired').val() == "true"){
        if ($("#GDSCode").val() == "") {
            $("#GDSCodeValidation").addClass('field-validation-error');
            $('#GDSCodeValidation').html("GDS Required");
            valid = false;
        }
    }

    if (!valid) {
        return false;
    }

    //ensure fields are clear if not used
    if (!$('#trGDSs').is(":visible")) {
    	$('#GDSCode').val("");
    }

	//ensure parameters are clear if not used
    if (!$('.parameter-fields').is(":visible")) {
    	$('#DepartureTimeWindow').val("");
    	$('#ArrivalTimeWindow').val("");
    	$('#MaximumStops').val("");
    	$('#MaximumConnectionTime').val("");
    	$('#UseAlternateAirportFlag').val("");
    	$('#NoPenaltyFlag').val("");
    	$('#NoRestrictionsFlag').val("");
	}
});
