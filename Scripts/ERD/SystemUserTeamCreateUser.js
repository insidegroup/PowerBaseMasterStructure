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
    $("#SystemUserName").autocomplete({
        source: function(request, response) {
            $.ajax({
            url: "/SystemUserTeam.mvc/AutoCompleteNonTeamSystemUsers", type: "POST", dataType: "json",
            data: { searchText: request.term, id: $("#TeamId").val() },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                        label: item.LastName + ", " + item.FirstName + (item.MiddleName == "" || item.MiddleName == null ? "" : " " + item.MiddleName),
                        value: item.LastName + ", " + item.FirstName + (item.MiddleName == "" || item.MiddleName == null ? "" : " " + item.MiddleName),
                        id: item.SystemUserGuid
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#lblTravelerTypeMsg").text("");
            $("#SystemUserGuid").val(ui.item.id);
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
            url: "/SystemUserTeam.mvc/IsValidSystemUserForTeam",
            data: { id: $("#SystemUserGuid").val(), teamid: $("#TeamId").val() },
            success: function(data) {
                validItem = data;
            },
            dataType: "json",
            async: false
        });
        if (!validItem) {
            $("#lblSystemUserNameMsg").removeClass('field-validation-valid');
            $("#lblSystemUserNameMsg").addClass('field-validation-error');
            $("#lblSystemUserNameMsg").text("This is not a valid entry.");
        } else {
            $("#lblSystemUserNameMsg").text("");
        }
    }

    if (validItem) {
        return true;
    } else {
        return false
    };
});
