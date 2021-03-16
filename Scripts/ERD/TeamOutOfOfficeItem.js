

$(document).ready(function () {

    //Navigation
    $('#menu_chatmessages').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    $('#breadcrumb').css('width', 'auto');

    //Disable empty team fields
    if ($('#PrimaryBackupTeam_TeamName').val() != "") {
        $('#SecondaryBackupTeam_TeamName').removeAttr("disabled");
    } else {
        $('#SecondaryBackupTeam_TeamName').attr("disabled", true);
    }
    
    if ($('#SecondaryBackupTeam_TeamName').val() != "") {
        $('#SecondaryBackupTeam_TeamName').removeAttr("disabled");
    }

    $('#TertiaryBackupTeam_TeamName').attr("disabled", true);
    if ($('#TertiaryBackupTeam_TeamName').val() != "" || $('#SecondaryBackupTeam_TeamName').val() != "") {
        $('#TertiaryBackupTeam_TeamName').removeAttr("disabled");
    }

    //Update on change
    $('#PrimaryBackupTeam_TeamName').change(function () {
        if ($(this).val() == "") {

            //Secondary
            $('#SecondaryBackupTeam_TeamId').val("");
            $('#SecondaryBackupTeam_TeamName').val("");
            $('#SecondaryBackupTeam_TeamName').attr("disabled", true);

            //Tertiary
            $('#TertiaryBackupTeam_TeamId').val("");
            $('#TertiaryBackupTeam_TeamName').val("");
            $('#TertiaryBackupTeam_TeamName').attr("disabled", true);
        }
    });

    $('#SecondaryBackupTeam_TeamName').change(function () {
        if ($(this).val() == "") {
            $('#TertiaryBackupTeam_TeamId').val("");
            $('#TertiaryBackupTeam_TeamName').val("");
            $('#TertiaryBackupTeam_TeamName').attr("disabled", true);
        }
    });

	//Submit Form Validation
    $('#form0').submit(function () {

        $("#lblErrorMsg").text("");

        //Check for removed items

        if ($("#PrimaryBackupTeam_TeamName").val() == "") {
            $("#PrimaryBackupTeam_TeamId").val("");
        }

        if ($("#SecondaryBackupTeam_TeamName").val() == "") {
            $("#SecondaryBackupTeam_TeamId").val("");
        }
        if ($("#TertiaryBackupTeam_TeamName").val() == "") {
            $("#TertiaryBackupTeam_TeamId").val("");
        }

        //Team 1
        var validTeam1 = false;
        if ($("#PrimaryBackupTeam_TeamName").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValidTeam",
                data: { searchText: encodeURIComponent($("#PrimaryBackupTeam_TeamName").val()) },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validTeam1 = true;
                    }
                },
                dataType: "json",
                async: false
            });
            if (!validTeam1) {
                $("#lblBackupTeam1ItemMsg").removeClass('field-validation-valid');
                $("#lblBackupTeam1ItemMsg").addClass('field-validation-error');
                $("#lblBackupTeam1ItemMsg").text("This is not a valid entry.");
            } else {
                $("#lblBackupTeam1ItemMsg").text("");
            }
        } else {
            validTeam1 = true;
        }

        //Team 2
        var validTeam2 = false;
        if ($("#SecondaryBackupTeam_TeamName").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValidTeam",
                data: { searchText: encodeURIComponent($("#SecondaryBackupTeam_TeamName").val()) },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validTeam2 = true;
                    }
                },
                dataType: "json",
                async: false
            });
            if (!validTeam2) {
                $("#lblBackupTeam2ItemMsg").removeClass('field-validation-valid');
                $("#lblBackupTeam2ItemMsg").addClass('field-validation-error');
                $("#lblBackupTeam2ItemMsg").text("This is not a valid entry.");
            } else {
                $("#lblBackupTeam2ItemMsg").text("");
            }
        } else {
            validTeam2 = true;
        }

        //Team 3
        var validTeam3 = false;
        if ($("#TertiaryBackupTeam_TeamName").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValidTeam",
                data: { searchText: encodeURIComponent($("#TertiaryBackupTeam_TeamName").val()) },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validTeam3 = true;
                    }
                },
                dataType: "json",
                async: false
            });
            if (!validTeam3) {
                $("#lblBackupTeam3ItemMsg").removeClass('field-validation-valid');
                $("#lblBackupTeam3ItemMsg").addClass('field-validation-error');
                $("#lblBackupTeam3ItemMsg").text("This is not a valid entry.");
            } else {
                $("#lblBackupTeam3ItemMsg").text("");
            }
        } else {
            validTeam3 = true;
        }

		if (!$(this).valid()) {
			return false;
		}

        if (validTeam1 && validTeam2 && validTeam3) {
			return true;
		} else {
            return false;
		}
    });

    //Autocompletes
    $(function () {
        $("#PrimaryBackupTeam_TeamName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/TeamOutOfOfficeItemBackupTeams", type: "POST", dataType: "json",
                    data: { id: $("#TeamOutOfOfficeItemId").val(), searchText: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.TeamName,
                                value: item.TeamName,
                                id: item.TeamId,
                                text: item.TeamName
                            }
                        }))
                    }
                });
            },
            select: function (event, ui) {
                $("#lblBackupTeam1ItemMsg").text("");
                $("#PrimaryBackupTeam_TeamId").val(ui.item.id);
                $('#SecondaryBackupTeam_TeamName').removeAttr("disabled");
            }
        });

        $("#SecondaryBackupTeam_TeamName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/TeamOutOfOfficeItemBackupTeams", type: "POST", dataType: "json",
                    data: { id: $("#TeamOutOfOfficeItemId").val(), searchText: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.TeamName,
                                value: item.TeamName,
                                id: item.TeamId,
                                text: item.TeamName
                            }
                        }))
                    }
                });
            },
            select: function (event, ui) {
                $("#lblBackupTeam1ItemMsg").text("");
                $("#SecondaryBackupTeam_TeamId").val(ui.item.id);
                $('#TertiaryBackupTeam_TeamName').removeAttr("disabled");
            }
        });

        $("#TertiaryBackupTeam_TeamName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/TeamOutOfOfficeItemBackupTeams", type: "POST", dataType: "json",
                    data: { id: $("#TeamOutOfOfficeItemId").val(), searchText: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.TeamName,
                                value: item.TeamName,
                                id: item.TeamId,
                                text: item.TeamName
                            }
                        }))
                    }
                });
            },
            select: function (event, ui) {
                $("#lblBackupTeam1ItemMsg").text("");
                $("#TertiaryBackupTeam_TeamId").val(ui.item.id);
            }
        });
    });

});