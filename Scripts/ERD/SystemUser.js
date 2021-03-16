

$(document).ready(function() {
    //Navigation
    $('#menu_teams').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
});

$(function () {
	$("#LocationName").autocomplete({

		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/SystemUserLocations", type: "POST", dataType: "json",
				data: { searchText: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
							value: item.HierarchyName,
							id: item.HierarchyCode,
							text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName)
						}
					}))
				}
			})
		},
		select: function (event, ui) {
			$("#lblLocationNameMsg").text(ui.item.text);
			$("#LocationName").val(ui.item.value);
			$("#LocationId").val(ui.item.id);
		}
	});

});

//Default Profile
$(function () {
	$("#DefaultProfileIdentifier").change(function() {
		if(this.checked) {
			if (!$('#lblDefaultProfileIdentifierMsg').is(":visible")) {
				checkDefaultProfileIdentifier();
			}
		} else {
			$('#lblDefaultProfileIdentifierMsg').text('').hide();
		}
	});

});

function checkDefaultProfileIdentifier() {
	$.ajax({
		url: "/SystemUserWizard.mvc/DefaultProfileIdentifierExist",
		type: "POST",
		dataType: "json",
		data: { systemUserGuid: $('#SystemUserGuid').val() },
		success: function (data) {

			//Returns the profile with the default profile flag checked (if set)
			if (data == '') {
				$('#lblDefaultProfileIdentifierMsg').text('').hide();
			} else {
				$('#lblDefaultProfileIdentifierMsg')
					.text('A default profile is already set for ' + data + '. Check this box to change the default to this profile.')
					.show();
				$("#DefaultProfileIdentifier").attr('checked', false);
				return false;
			}
		}
	});
}

//Submit Form Validation
$('#form0').submit(function () {

    var validItem = false;

    if ($("#DefaultProfileIdentifier").is(':checked')) {
    	if (!$('#lblDefaultProfileIdentifierMsg').is(":visible")) {
    		checkDefaultProfileIdentifier();
    	}
    }

    if ($("#LocationName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidLocation",
            data: { locationName: $("#LocationName").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validItem = true;
                    $("#LocationId").val(data.LocationId);
                }
            },
            dataType: "json",
            async: false
        });
        if (!validItem) {
            $("#lblLocationNameMsg").removeClass('field-validation-valid');
            $("#lblLocationNameMsg").addClass('field-validation-error');
            $("#lblLocationNameMsg").text("This is not a valid entry.");
            $("#LocationId").val("");
            return false
        } else {
            $("#lblLocationNameMsg").text("");
            return true;
        }
    }
});