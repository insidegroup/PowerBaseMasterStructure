

$(document).ready(function () {

    //Navigation
    $('#menu_chatmessages').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	//Show DatePickers
	$('#ExpiryDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
    });

	$('#EnabledDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
    });

	if ($('#EnabledDate').val() == "") {
		$('#EnabledDate').val("No Enabled Date");
	}
	if ($('#ExpiryDate').val() == "") {
		$('#ExpiryDate').val("No Expiry Date");
	}

	//Hierarchy Disable/Enable OnLoad
	if ($("#HierarchyType").val() == "") {
		$("#HierarchyItem").val("");
		$("#HierarchyItem").attr("disabled", true);
	} else {
		$("#HierarchyItem").removeAttr("disabled");
	}

	//Hierarchy Disable/Enable OnChange
	$("#HierarchyType").change(function () {
		$("#lblHierarchyItemMsg").text("");
		$("#HierarchyItem").val("");
		if ($("#TeamOutOfOfficeGroupId").val() == "0") {
			$("#lblAuto").text("");
			$("#TeamOutOfOfficeGroupName").val("");
		}
		if ($("#HierarchyType").val() == "") {
			$("#HierarchyItem").attr("disabled", true);
			$('#TravelerType').css('display', 'none');
		} else {
			$("#HierarchyItem").removeAttr("disabled");
			$("#lblHierarchyItem").text($("#HierarchyType").val());
			$("#HierarchyCode").val("");
		}

		//Set Autocomplete minimum length based on Hierarchy Type
		if ($("#HierarchyType").val() == 'ClientSubUnitGUID') {
			$("#HierarchyItem").autocomplete('option', 'minLength', 5);
		} else {
			$("#HierarchyItem").autocomplete('option', 'minLength', 0);
		}
	});

	//Filter Hierarchy Item
	$('#HierarchyItem').keyup(function () {
		if ($("#HierarchyType").val() == 'ClientSubUnitGUID') {
			$(this).val($(this).val().replace(/[^a-z0-9\:]/gi, ''));
		}
	});

	//Submit Form Validation
	$('#form0').submit(function () {

		var validItem = false;

        if ($("#HierarchyItem").val() != "") {
        	var searchText = ($("#HierarchyType option:selected").text() == "ClientSubUnitGUID") ? $("#ClientSubUnitName").val() : $("#HierarchyItem").val();
        	jQuery.ajax({
				type: "POST",
				url: "/Hierarchy.mvc/IsValid" + UpdateHierarchyType($("#HierarchyType").val()),
				data: { searchText: encodeURIComponent(searchText) },
				success: function (data) {

					if (!jQuery.isEmptyObject(data)) {
						validItem = true;
					}
				},
				dataType: "json",
				async: false
			});
			if (!validItem) {
				$("#lblHierarchyItemMsg").removeClass('field-validation-valid');
				$("#lblHierarchyItemMsg").addClass('field-validation-error');
				$("#lblHierarchyItemMsg").text("This is not a valid entry.");
				if ($("#lblAuto").length) { $("#lblAuto").text("") };
			} else {
				$("#lblHierarchyItemMsg").text("");
			}
		}

		/*
        //Check Duplicate Active Group on Create/Edit
        There can only be one active group created for each SubUnit (whether in UnDeleted or Deleted).
        When saving an active group; the Enabled flag, Enabled Date and Expiry date of the existing groups should be evaluated
        */
        var validTeamOutOfOfficeGroupClientSubUnit = false;

        jQuery.ajax({
            type: "POST",
            url: "/TeamOutOfOfficeGroup.mvc/CanTeamOutOfOfficeGroupBeActive",
            data: { id: $("#TeamOutOfOfficeGroupId").val(), clientSubUnitGuid: $("#HierarchyCode").val() },
            success: function (data) {
                validTeamOutOfOfficeGroupClientSubUnit = data;
            },
            dataType: "json",
            async: false
        });

        if (!validTeamOutOfOfficeGroupClientSubUnit) {

            $("#lblTeamOutOfOfficeGroupActiveMsg").removeClass('field-validation-valid');
            $("#lblTeamOutOfOfficeGroupActiveMsg").addClass('field-validation-error');
            if ($("#TeamOutOfOfficeGroupId").val() == "0") {//Create
                $("#lblTeamOutOfOfficeGroupActiveMsg").text("There is already one active group for this Client SubUnit. Please update the current active group.");
            } else {
                if ($("#TeamOutOfOfficeGroupName").val() != "") {
                    $("#lblTeamOutOfOfficeGroupActiveMsg").text("There is already one active group for this Client SubUnit. Please update the current active group.");
                }
            }
            return false;
        } else {
            $("#lblTeamOutOfOfficeGroupActiveMsg").text("");
        }

        //Check Group Name
		var validGroupName = false;

        if ($("#TeamOutOfOfficeGroupName").val() != '') {
            jQuery.ajax({
                type: "POST",
                url: "/GroupNameBuilder.mvc/IsAvailableTeamOutOfOfficeGroupName",
                data: { groupName: $("#TeamOutOfOfficeGroupName").val(), id: $("#TeamOutOfOfficeGroupId").val() },
                success: function (data) {
                    validGroupName = data;
                },
                dataType: "json",
                async: false
            });

            if (!validGroupName) {

                $("#lblTeamOutOfOfficeGroupNameMsg").removeClass('field-validation-valid');
                $("#lblTeamOutOfOfficeGroupNameMsg").addClass('field-validation-error');
                if ($("#TeamOutOfOfficeGroupId").val() == "0") {//Create
                    $("#lblTeamOutOfOfficeGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
                } else {
                    if ($("#TeamOutOfOfficeGroupName").val() != "") {
                        $("#lblTeamOutOfOfficeGroupNameMsg").text("This name has already been used, please choose a different name.");
                    }
                }
                return false;
            } else {
                $("#lblTeamOutOfOfficeGroupNameMsg").text("");
            }
        }

		if (!$(this).valid()) {
			return false;
		}

		if (validItem) {
			if ($('#EnabledDate').val() == "No Enabled Date") {
				$('#EnabledDate').val("");
			}
			if ($('#ExpiryDate').val() == "No Expiry Date") {
				$('#ExpiryDate').val("");
			}
			return true;
		} else {
			return false
		};
	});

	$("#HierarchyItem").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Client Detail', resultCount: 5000 },
                success: function (data) {
                    response($.map(data, function (item) {
						if ($("#HierarchyType").val() == "ClientSubUnitGUID") {
							return {
								label: item.HierarchyCode + ", " + item.HierarchyName,
								value: item.HierarchyCode,
								id: item.HierarchyCode,
								text: item.HierarchyCode + ", " + item.HierarchyName,
								clientSubUnitName: item.HierarchyName
							}
                    	} else {
                    		return {
                    			label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
                    			value: item.HierarchyName,
                    			id: item.HierarchyCode,
                    			text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                    		}
                    	}
                    }));
                }
            });
        },
		select: function (event, ui) {
			$("#lblHierarchyItemMsg").text(ui.item.text);
			$("#HierarchyItem").val(ui.item.value);
			$("#HierarchyCode").val(ui.item.id);
			$("#ClientSubUnitName").val(ui.item.clientSubUnitName);

            var validTeamOutOfOfficeGroupClientSubUnit = false;

            jQuery.ajax({
                type: "POST",
                url: "/TeamOutOfOfficeGroup.mvc/CanTeamOutOfOfficeGroupBeActive",
                data: { id: $("#TeamOutOfOfficeGroupId").val(), clientSubUnitGuid: $("#HierarchyCode").val() },
                success: function (data) {
                    validTeamOutOfOfficeGroupClientSubUnit = data;
                },
                dataType: "json",
                async: false
            });

            if (!validTeamOutOfOfficeGroupClientSubUnit) {

                $("#lblTeamOutOfOfficeGroupActiveMsg").removeClass('field-validation-valid');
                $("#lblTeamOutOfOfficeGroupActiveMsg").addClass('field-validation-error');
                if ($("#TeamOutOfOfficeGroupId").val() == "0") {//Create
                    $("#lblTeamOutOfOfficeGroupActiveMsg").text("There is already one active group for this Client SubUnit. Please update the current active group.");
                } else {
                    if ($("#TeamOutOfOfficeGroupName").val() != "") {
                        $("#lblTeamOutOfOfficeGroupActiveMsg").text("There is already one active group for this Client SubUnit. Please update the current active group.");
                    }
                }
                return false;
            } else {

                $("#lblTeamOutOfOfficeGroupActiveMsg").text("");

                htft = ShortenHierarchyType($("#HierarchyType").val());

                if ($("#TeamOutOfOfficeGroupId").val() == "0") {//Create

                    //to get number for GroupName
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
                        data: { hierarchyType: UpdateHierarchyType($("#HierarchyType").val()), hierarchyItem: $("#HierarchyCode").val(), group: "Team Out of Office" },
                        success: function (data) {
                            //Allow all 50 characters in name, as Team Out of Office group name is 62 in length
                            var maxNameSize = 50;
                            var suffix = "_" + data + "_" + htft + "_OOO";
                            var name = ($("#HierarchyType option:selected").text() == "ClientSubUnitGUID") ? ui.item.clientSubUnitName : ui.item.value;
                            var autoName = replaceSpecialChars(name).substring(0, maxNameSize) + suffix;

                            $("#lblAuto").text(autoName);
                            $("#TeamOutOfOfficeGroupName").val(autoName);
                            $("#lblTeamOutOfOfficeGroupNameMsg").text("");
                        }
                    })
                }
            }
		}
	});

	$('.ui-autocomplete').addClass('widget-overflow');
});

function UpdateHierarchyType(hierarchyType) {
	if (hierarchyType == 'ClientSubUnitGUID') {
		hierarchyType = 'ClientSubUnit'
	}
	return hierarchyType;
}

function ShortenHierarchyType(hierarchyType) {
	switch (hierarchyType) {
		case "ClientTopUnit":
			shortversion = "CTU";
			break;
		case "ClientSubUnit":
			shortversion = "CSU";
			break;
		case "ClientSubUnitGUID":
			shortversion = "CSU";
			break;
		case "ClientSubUnitTravelerType":
			shortversion = "CSUTT";
			break;
		case "GlobalSubRegion":
			shortversion = "GSR";
			break;
		case "GlobalRegion":
			shortversion = "GR";
			break;
		case "CountryRegion":
			shortversion = "CR";
			break;
		default:
			shortversion = hierarchyType;
	}


	return shortversion;
}