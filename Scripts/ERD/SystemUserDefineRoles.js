

$(document).ready(function() {
    //Navigation
    $('#menu_teams').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
});

$(function() {
	$("#NewUserProfileIdentifier").autocomplete({

        source: function(request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/SystemUsers", type: "POST", dataType: "json",
                data: { searchText: request.term},
                success: function(data) {
                	response($.map(data, function (item) {
                        return {
                        	label: item.UserProfileIdentifier,
                        	value: item.UserProfileIdentifier,
                            id: item.UserProfileIdentifier,
                            text: item.UserProfileIdentifier
                        }
                    }))
                }
            })
        },
        select: function (event, ui) {
        	$("#lblNewUserId").text(ui.item.text);
        	$("#NewUserProfileIdentifier").val(ui.item.value);
        }
    });

});


//Submit Form Validation
$('#form0').submit(function () {

    var validItem = false;

    if ($("#NewUserProfileIdentifier").val() != "") {
    	jQuery.ajax({
    		type: "POST",
    		url: "/Validation.mvc/IsValidSystemUser",
    		data: { id: $("#NewUserProfileIdentifier").val() },
    		success: function (data) {

    			if (!jQuery.isEmptyObject(data)) {
    				validItem = true;
    				$("#NewUserProfileIdentifier").val(data.UserProfileIdentifier);
    			}
    		},
    		dataType: "json",
    		async: false
    	});
    	if (!validItem) {
    		$("#lblNewUserProfileIdentifier").removeClass('field-validation-valid');
    		$("#lblNewUserProfileIdentifier").addClass('field-validation-error');
    		$("#lblNewUserProfileIdentifier").text("This is not a valid entry.");
    		$("#NewUserProfileIdentifier").val("");
    		validItem = false
    	} else {
    		$("#lblNewUserProfileIdentifier").text("");
    		validItem = true;
    	}
    } else {
    	$("#lblNewUserProfileIdentifier").removeClass('field-validation-valid');
    	$("#lblNewUserProfileIdentifier").addClass('field-validation-error');
    	$("#lblNewUserProfileIdentifier").text("Profile ID required.");
    	validItem = false
    }
    return validItem;
});