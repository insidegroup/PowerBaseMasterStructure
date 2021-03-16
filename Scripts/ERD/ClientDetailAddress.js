

$(document).ready(function() {
$('#menu_clientdetails').click();
$("tr:odd").addClass("row_odd");
$("tr:even").addClass("row_even");

    $("#CountryCode").change(function() {

        $("#StateProvince").val("");
        if ($("#CountryCode").val() == "") {
            $("#StateProvince").attr("disabled", true);
        } else {
            $("#StateProvince").removeAttr("disabled");
        }
    });

    $("#StateProvince").change(function() {

        if ($("#StateProvince").val() == "") {
            $("#StateProvinceCode").val("");
        }
    });
});




$(function() {
    $("#StateProvince").autocomplete({
        source: function(request, response) {
            $.ajax({
            url: "/ClientDetailAddress.mvc/AutoCompleteStateProvince", type: "POST", dataType: "json",
                data: { searchText: request.term, countryCode:  $("#CountryCode").val() },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                            id: item.StateProvinceCode,
                            value: item.Name
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#StateProvince").val(ui.item.value);
            $("#StateProvinceCode").val(ui.item.id);
        }
    });
});


$('#form0').submit(function() {
    if ($("#StateProvince").val() != "") {

        var validItem = false;
        jQuery.ajax({
            type: "POST",
            url: "/StateProvince.mvc/IsValidStateProvince",
            data: { searchText: $("#StateProvince").val(), countryCode: $("#CountryCode").val() },
            success: function(data) {

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
        } else {
            $("#lblStateProvinceMsg").text("");
        }
    }


});