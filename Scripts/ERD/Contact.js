$(document).ready(function() {
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    var countryCodeVal = $("#Contact_CountryCode").val();
    LoadStateProvincesByCountryCode(countryCodeVal, true);

    $("#Contact_CountryCode").change(function () {
    	var countryCodeVal = $("#Contact_CountryCode").val();
    	LoadStateProvincesByCountryCode(countryCodeVal, false);
    });

    $('#form0').submit(function () {
    	var error = false;
    	$("#Contact_StateProvinceName")
				.removeClass('input-validation-error')
				.parent().parent().find("td:nth-child(3)").find("span")
					.html("")
					.removeClass("field-validation-error")
					.addClass("field-validation-valid");

    	if ($("#Contact_StateProvinceName option").length > 1 && $("#Contact_StateProvinceName").val() == "") {
    		error = true;
    	}

    	if (error) {
    		$("#Contact_StateProvinceName")
					.addClass('input-validation-error')
    				.parent().parent().find("td:nth-child(3)").find("span")
						.html('<span htmlfor="Contact_StateProvinceName" generated="true" class="">State/Province Required</span>')
						.addClass("field-validation-error")
						.removeClass("field-validation-valid");
    		return false;
    	}

    	return true;
    });

});

function LoadStateProvincesByCountryCode(countryCodeVal, firstLoad) {

    var selected = $("#Contact_StateProvinceName option:selected").val();

    $.ajax({
    	url: "/AutoComplete.mvc/GetStateProvincesByCountryCode", type: "POST", dataType: "json",
    	data: { countryCode: countryCodeVal },
    	success: function (data) {

    		// Clear the old options
    		$("#Contact_StateProvinceName").find('option').remove();

    		// Add a default
    		$("<option value=''>Please Select...</option>").appendTo($("#Contact_StateProvinceName"));

    		// Load the new options
    		$(data).each(function () {
    			$("<option value=" + this.StateProvinceCode + ">" + this.Name + "</option>").appendTo($("#Contact_StateProvinceName"));
    		});

    		// Show dropdown
    		if ($("#Contact_StateProvinceName option").length > 1) {
    			$('#Contact_StateProvinceName').attr('disabled', false);
    			$('.stateProvinceCodeError').show();

    			//Reapply Edit
    			if (selected != null) {
    				$("#Contact_StateProvinceName").val(selected);
    			}

    			$("#Contact_StateProvinceName").after('<span class="error"> *</span>');

    		} else {
    			$('#Contact_StateProvinceName').attr('disabled', true);
    			$('.stateProvinceCodeError').hide();
    			$('#Contact_StateProvinceName').next('.error').remove();
    		}

    		if (firstLoad == true) {
    			if (selected != null) {
    				$("#Contact_StateProvinceName").val(selected);
    			}
    		}

    	}
    });
}