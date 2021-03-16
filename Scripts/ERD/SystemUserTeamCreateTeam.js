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
    $("#TeamName").autocomplete({
        source: function(request, response) {
            $.ajax({
            url: "/SystemUserTeam.mvc/AutoCompleteNonSystemUserTeams", type: "POST", dataType: "json",
            data: { searchText: request.term, id: $("#SystemUserGuid").val() },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                        label: item.TeamName,
                        value: item.TeamName,
                        id: item.TeamId
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#lblTeamNameMsg").text("");
            $("#TeamId").val(ui.item.id);
        }
    });
});




/*
Submit Form Validation
*/
$('#form0').submit(function() {

    var validItem = false;
    if ($("#TeamName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/SystemUserTeam.mvc/IsValidTeamForSystemUser",
            data: { id: $("#SystemUserGuid").val(), teamName: $("#TeamName").val() },
            success: function(data) {
                validItem = data;
            },
            dataType: "json",
            async: false
        });
        if (!validItem) {
            $("#TeamId").val("");
            $("#lblTeamNameMsg").removeClass('field-validation-valid');
            $("#lblTeamNameMsg").addClass('field-validation-error');
            $("#lblTeamNameMsg").text("This is not a valid entry.");
        } else {
            $("#lblTeamNameMsg").text("");
        }
    }

    if (validItem) {
        return true;
    } else {
        return false
    };
});
