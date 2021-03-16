/*
OnReady
*/

$(document).ready(function() {
    //Navigation
    $('#menu_teams').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
});



$(function() {
$("#ClientSubUnitName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/TeamAvailableClientSubUnits", type: "POST", dataType: "json",
            data: { searchText: request.term, id: $("#TeamId").val() },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                        label: item.ClientSubUnitName,
                        value: item.ClientSubUnitName,
                        id: item.ClientSubUnitGuid
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
        $("#lblClientSubUnitNameMsg").text("");
            $("#ClientSubUnitGuid").val(ui.item.id);
        }
    });
});




/*
Submit Form Validation
*/
$('#form0').submit(function() {

    var validItem = false;
    if ($("#SystemUserName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/ClientSubUnitTeam.mvc/IsValidClientSubUnitForTeam",
            data: { id: $("#ClientSubUnitGuid").val(), teamid: $("#TeamId").val() },
            success: function(data) {
                validItem = data;
            },
            dataType: "json",
            async: false
        });
        if (!validItem) {
            $("#lblClientSubUnitNameMsg").removeClass('field-validation-valid');
            $("#lblClientSubUnitNameMsg").addClass('field-validation-error');
            $("#lblClientSubUnitNameMsg").text("This is not a valid entry.");
        } else {
        $("#lblClientSubUnitNameMsg").text("");
        }
    }

    if (validItem) {
        return true;
    } else {
        return false
    };
});
