$(document).ready(function () {
    var countryCodeVal = $("#ClientTopUnitClientLocation_CountryCode").val();
    LoadStateProvincesByCountryCode(countryCodeVal, true);

    $("#ClientTopUnitClientLocation_CountryCode").change(function () {

        var countryCodeVal = $("#ClientTopUnitClientLocation_CountryCode").val();
        LoadStateProvincesByCountryCode(countryCodeVal, false);

    });

    $('#mainForm').submit(function () {
        var error = false;
        $("#ClientTopUnitClientLocation_StateProvinceName").removeClass('input-validation-error');
        $("#ClientTopUnitClientLocation_StateProvinceName").parent().parent().find("td:nth-child(3)").find("span").html("");
        $("#ClientTopUnitClientLocation_StateProvinceName").parent().parent().find("td:nth-child(3)").find("span").removeClass("field-validation-error");
        $("#ClientTopUnitClientLocation_StateProvinceName").parent().parent().find("td:nth-child(3)").find("span").addClass("field-validation-valid");

        if ($("#ClientTopUnitClientLocation_StateProvinceName option").length > 1 && $("#ClientTopUnitClientLocation_StateProvinceName").val() == "") {
            error = true;
        }

        if (error) {
            $("#ClientTopUnitClientLocation_StateProvinceName").addClass('input-validation-error');
            $("#ClientTopUnitClientLocation_StateProvinceName").parent().parent().find("td:nth-child(3)").find("span").html('<span htmlfor="ClientTopUnitClientLocation_StateProvinceName" generated="true" class="">StateProvinceName Required</span>');
            $("#ClientTopUnitClientLocation_StateProvinceName").parent().parent().find("td:nth-child(3)").find("span").addClass("field-validation-error");
            $("#ClientTopUnitClientLocation_StateProvinceName").parent().parent().find("td:nth-child(3)").find("span").removeClass("field-validation-valid");
            return false;
        }


        return true;
    });

});

function LoadStateProvincesByCountryCode(countryCodeVal, firstLoad) {

    var selected = $("#ClientTopUnitClientLocation_StateProvinceName option:selected").val();

    $.ajax({
        url: "/AutoComplete.mvc/GetStateProvincesByCountryCode", type: "POST", dataType: "json",
        data: { countryCode: countryCodeVal },
        success: function (data) {

            // Clear the old options
            $("#ClientTopUnitClientLocation_StateProvinceName").find('option').remove();

            // Add a default
            $("<option value=''>Please Select...</option>").appendTo($("#ClientTopUnitClientLocation_StateProvinceName"));

            // Load the new options
            $(data).each(function () {
                $("<option value=" + this.StateProvinceCode + ">" + this.Name + "</option>").appendTo($("#ClientTopUnitClientLocation_StateProvinceName"));
            });

            // Show dropdown
            if ($("#ClientTopUnitClientLocation_StateProvinceName option").length > 1) {
                $('#ClientTopUnitClientLocation_StateProvinceName').attr('disabled', false);
                $('.stateProvinceCodeError').show();

                //Reapply Edit
                if (selected != null) {
                    $("#ClientTopUnitClientLocation_StateProvinceName").val(selected);
                }

            } else {
                $('#ClientTopUnitClientLocation_StateProvinceName').attr('disabled', true);
                $('.stateProvinceCodeError').hide();
            }

            if (firstLoad == true) {
                if (selectedState != null) {

                    $("#ClientTopUnitClientLocation_StateProvinceName").val(selectedState);
                }
            }

        }
    });
}



