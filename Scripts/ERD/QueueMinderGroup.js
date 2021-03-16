

$(document).ready(function() {

    //Navigation
    $('#menu_ticketqueuegroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#QueueMinderGroup_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#QueueMinderGroup_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    if ($('#EnabledDate').val() == "") {
        $('#EnabledDate').val("No Enabled Date")
    }
    if ($('#ExpiryDate').val() == "") {
        $('#ExpiryDate').val("No Expiry Date")
    }

    //Hierarchy Disable/Enable OnLoad
    if ($("#QueueMinderGroup_HierarchyType").val() == "") {
        $("#QueueMinderGroup_HierarchyItem").val("");
        $("#QueueMinderGroup_HierarchyItem").attr("disabled", true);
    } else {
        $("#QueueMinderGroup_HierarchyItem").removeAttr("disabled");

        if ($("#QueueMinderGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
            $("#lblHierarchyItem").text("ClientSubUnit");
            $('#TravelerType').css('display', '');
        }
    }

	//Meetings
    if (
		$("#HierarchyType").val() != "ClientTopUnit" &&
		$("#HierarchyType").val() != "ClientSubUnit" &&
		$("#HierarchyType").val() != "ClientAccount" &&
		$("#HierarchyType").val() != "TravelerType" &&
		$("#HierarchyType").val() != "ClientSubUnitTravelerType"
	) {
    	$('#MeetingID').val("").attr("disabled", true);
    }

    if ($('#QueueMinderGroup_HierarchyItem').val() == "") {
    	$('#QueueMinderGroup_MeetingID').val("").attr("disabled", true);
    }

    //Hierarchy Disable/Enable OnChange
    $("#QueueMinderGroup_HierarchyType").change(function () {
        $("#lblHierarchyItemMsg").text("");
        $("#QueueMinderGroup_HierarchyItem").val("");
        if ($("#QueueMinderGroup_QueueMinderGroupId").val() == "0") {
            $("#lblAuto").text("");
            $("#QueueMinderGroup_QueueMinderGroupName").val("");
        }
        if ($("#QueueMinderGroup_HierarchyType").val() == "") {
            $("#QueueMinderGroup_HierarchyItem").attr("disabled", true);
            $('#TravelerType').css('display', 'none');
        } else {
            $("#QueueMinderGroup_HierarchyItem").removeAttr("disabled");
            $("#lblHierarchyItem").text($("#HierarchyType").val());
            $("#QueueMinderGroup_HierarchyCode").val("");
            $('#TravelerType').css('display', 'none');
            if ($("#QueueMinderGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
                $("#lblHierarchyItem").text("ClientSubUnit");
                $("#QueueMinderGroup_TravelerTypeName").val("");
                $("#QueueMinderGroup_TravelerTypeGuid").val("");
                $('#TravelerType').css('display', '');
            }
        }

    	//Reset Dropdown
    	$('#QueueMinderGroup_MeetingID')
			.attr("disabled", true)
			.empty()
			.append('<option value="">Please Select...</option>');
    });


    //Submit Form Validation
    $('#form0').submit(function () {

        var validItem = false;
        var validTravelerType = true;

        if ($("#QueueMinderGroup_IsMultipleHierarchy").val() == "True") {
        	validItem = true;
        } else {
        if ($("#HierarchyType").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValid" + $("#QueueMinderGroup_HierarchyType").val(),
                data: { searchText: encodeURIComponent($("#QueueMinderGroup_HierarchyItem").val()) },
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
        }

        if ($("#TravelerType").is(":visible")) {

            if ($("#QueueMinderGroup_HierarchyType").val() != "") {
                jQuery.ajax({
                    type: "POST",
                    url: "/Hierarchy.mvc/IsValidTravelerType",
                    data: { searchText: $("#QueueMinderGroup_TravelerTypeName").val() },
                    success: function (data) {

                        if (jQuery.isEmptyObject(data)) {
                            validTravelerType = false;
                        }
                    },
                    dataType: "json",
                    async: false
                });
                if (!validTravelerType) {
                    $("#lblTravelerTypeMsg").removeClass('field-validation-valid');
                    $("#lblTravelerTypeMsg").addClass('field-validation-error');
                    $("#lblTravelerTypeMsg").text("This is not a valid entry.");
                   if ($("#lblAuto").length) { $("#lblAuto").text("") };
                } else {
                    $("#lblTravelerTypeMsg").text("");
                }
            }
        }

        //wait for this name to be populated, dont show message
        if ($("#QueueMinderGroup_QueueMinderGroupId").val() == "0") {
            if ($("#lblAuto").text() == "") {
                return false;
            }
        } else {
            if (jQuery.trim($("#QueueMinderGroup_QueueMinderGroupName").val()) == "") {
                $("#QueueMinderGroup_QueueMinderGroupName_validationMessage").removeClass('field-validation-valid');
                $("#QueueMinderGroup_QueueMinderGroupName_validationMessage").addClass('field-validation-error');
                $("#QueueMinderGroup_QueueMinderGroupName_validationMessage").text("Group Name Required.");
                return false;
            } else {
                $("#QueueMinderGroup_QueueMinderGroupName_validationMessage").text("");
            }
        }

        //GroupName Begin
        var validGroupName = false;
        
        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableQueueMinderGroupName",
            data: { groupName: $("#QueueMinderGroup_QueueMinderGroupName").val(), id: $("#QueueMinderGroup_QueueMinderGroupId").val() },
            success: function(data) {

                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {
            $("#lblQueueMinderGroupNameMsg").removeClass('field-validation-valid');
            $("#lblQueueMinderGroupNameMsg").addClass('field-validation-error');
            if ($("#QueueMinderGroupId").val() == "0") {//Create
                $("#lblQueueMinderGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
            } else {
                if ($("#QueueMinderGroup_QueueMinderGroupName").val() != "") {
                    $("#lblQueueMinderGroupNameMsg").text("This name has already been used, please choose a different name.");
                }
            }
            return false;
        } else {
            $("#lblQueueMinderGroupNameMsg").text("");
        }
        //GroupName End

        if (!$(this).valid()) {
            return false;
        }

        if (validItem && validTravelerType) {
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


    $("#QueueMinderGroup_HierarchyItem").autocomplete({
        source: function(request, response) {

            if ($("#QueueMinderGroup_HierarchyType").val() != "ClientSubUnitTravelerType") {
                $.ajax({
                    url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: $("#QueueMinderGroup_HierarchyType").val(), domainName: 'Queue', resultCount: 5000 },
                    success: function (data) {
                        response($.map(data, function(item) {
                            if (
                                    $("#QueueMinderGroup_HierarchyType").val() == "GlobalRegion" ||
                                    $("#QueueMinderGroup_HierarchyType").val() == "GlobalSubRegion" ||
                                    $("#QueueMinderGroup_HierarchyType").val() == "Country"
                                ) {
                                return {
                                    label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + " (" + item.HierarchyCode + ")",
                                    value: item.HierarchyName,
                                    id: item.HierarchyCode,
                                    text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                                }
                            } else if ($("#QueueMinderGroup_HierarchyType").val() == "ClientAccount") {
                                return {
                                    label: "<span class=\"ca-name\">" + item.HierarchyName + "</span><span class=\"ca-number\">" + item.ClientAccountNumber + "</span><span class=\"ca-ssc\">" + item.SourceSystemCode + "</span>",
                                    value: item.HierarchyName,
                                    id: item.ClientAccountNumber,
                                    ssc: item.SourceSystemCode,
                                    text: ""
                                }
                            } else if ($("#QueueMinderGroup_HierarchyType").val() == "TravelerType") {
                            	return {
                            		label: "<span class=\"tt-name\">" + item.HierarchyName + "</span>" + (item.ClientSubUnitName == "" ? "" : "<span class=\"tt-csu-name\">" + item.ClientSubUnitName + "</span>"),
                            		value: item.HierarchyName,
                            		id: item.HierarchyCode,
                            		text: item.HierarchyName + (item.ClientSubUnitName == "" ? "" : ", " + item.ClientSubUnitName),
                            		ttcsu: item.ClientSubUnitName
                            	}
                            } else {
                                return {
                                    label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
                                    value: item.HierarchyName,
                                    id: item.HierarchyCode,
                                    text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                                }
                            }

                        }))
                    }
                })
            } else {
                $.ajax({
                    url: "/FollowUpQueueGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#QueueMinderGroup_TravelerTypeGuid").val(), resultCount: 5000 },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return {
                                label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
                                value: item.HierarchyName,
                                id: item.HierarchyCode,
                                text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                            }
                        }))
                    }
                })

            }

        },
        select: function(event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#QueueMinderGroup_HierarchyItem").val(ui.item.value);
            $("#QueueMinderGroup_HierarchyCode").val(ui.item.id);
            $("#QueueMinderGroup_SourceSystemCode").val(ui.item.ssc);
            $("#TT_CSU").val(ui.item.ttcsu);

            if ($("#QueueMinderGroup_QueueMinderGroupId").val() == "0") {//Create

                //to get number for GroupName
                if ($("#QueueMinderGroup_HierarchyType").val() == "ClientAccount") {
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupNameClientAccount", type: "POST", dataType: "json",
                        data: { clientAccountNumber: $("#QueueMinderGroup_HierarchyCode").val(), sourceSystemCode: $("#QueueMinderGroup_SourceSystemCode").val(), group: "QueueMinder" },
                        success: function(data) {
                            var maxNameSize = 50 - ($("#QueueMinderGroup_HierarchyType").val().length + 17);
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + $("#QueueMinderGroup_HierarchyType").val() + "_QueueMinder";
                            $("#lblAuto").text(autoName);
                            $("#QueueMinderGroup_QueueMinderGroupName").val(autoName);
                            $("#lblQueueMinderGroupNameMsg").text("");
                        }
                    })
                } else {
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
                        data: { hierarchyType: $("#QueueMinderGroup_HierarchyType").val(), hierarchyItem: $("#QueueMinderGroup_HierarchyCode").val(), group: "QueueMinder" },
                        success: function(data) {
                            var maxNameSize = 50 - ($("#QueueMinderGroup_HierarchyType").val().length + 17);
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + $("#QueueMinderGroup_HierarchyType").val() + "_QueueMinder";
                            $("#lblAuto").text(autoName);
                            $("#QueueMinderGroup_QueueMinderGroupName").val(autoName);
                            $("#lblQueueMinderGroupNameMsg").text("");
                        }
                    })
                }
            }
            LoadMeetings();
        }
    });

    $('.ui-autocomplete').addClass('widget-overflow');

    $("#QueueMinderGroup_TravelerTypeName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/FollowUpQueueGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#QueueMinderGroup_HierarchyCode").val() },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                            label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
                            value: item.HierarchyName,
                            id: item.HierarchyCode,
                            text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#lblTravelerTypeMsg").text(ui.item.text);
            $("#QueueMinderGroup_TravelerTypeName").val(ui.item.value);
            $("#QueueMinderGroup_TravelerTypeGuid").val(ui.item.id);
            LoadMeetings();
        }
    });
});

//Meetings
function LoadMeetings() {

	//Meetings
	if (
		$("#QueueMinderGroup_HierarchyType").val() == "ClientTopUnit" ||
		$("#QueueMinderGroup_HierarchyType").val() == "ClientSubUnit" ||
		$("#QueueMinderGroup_HierarchyType").val() == "ClientAccount" ||
		$("#QueueMinderGroup_HierarchyType").val() == "TravelerType" ||
		$("#QueueMinderGroup_HierarchyType").val() == "ClientSubUnitTravelerType"
	) {

		//Reset Dropdown
		$('#QueueMinderGroup_MeetingID')
			.attr("disabled", false)
			.empty()
			.append('<option value="">Please Select...</option>');

		$('#QueueMinderGroup_MeetingID').val("");

		if ($("#QueueMinderGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
			$.ajax({
				url: "/AutoComplete.mvc/AutoCompleteAvailableMeetings", type: "POST", dataType: "json",
				data: { hierarchyType: "ClientSubUnitTravelerType", hierarchyItem: $('#QueueMinderGroup_HierarchyCode').val(), travelerTypeGuid: $("#QueueMinderGroup_TravelerTypeGuid").val(), resultCount: 5000 },
				success: function (data) {
					$.each(data, function (index, item) {
						$("#QueueMinderGroup_MeetingID").append(
							$("<option></option>")
								.text(item.MeetingName + " - " + item.MeetingReferenceNumber)
								.val(item.MeetingID)
						);
					});
				}
			})
		} else if ($("#QueueMinderGroup_HierarchyType").val() == "TravelerType") {
			$.ajax({
				url: "/AutoComplete.mvc/AutoCompleteAvailableMeetings", type: "POST", dataType: "json",
				data: { hierarchyType: "TravelerType", hierarchyItem: $('#TT_CSU').val(), travelerTypeGuid: $('#QueueMinderGroup_HierarchyCode').val(), resultCount: 5000 },
				success: function (data) {
					$.each(data, function (index, item) {
						$("#QueueMinderGroup_MeetingID").append(
							$("<option></option>")
								.text(item.MeetingName + " - " + item.MeetingReferenceNumber)
								.val(item.MeetingID)
						);
					});
				}
			});
		} else if ($("#QueueMinderGroup_HierarchyType").val() == "ClientAccount") {
			$.ajax({
				url: "/AutoComplete.mvc/AutoCompleteAvailableMeetings", type: "POST", dataType: "json",
				data: { hierarchyType: "ClientAccount", clientAccountNumber: $('#QueueMinderGroup_HierarchyCode').val(), sourceSystemCode: $('#QueueMinderGroup_SourceSystemCode').val(), resultCount: 5000 },
				success: function (data) {
					$.each(data, function (index, item) {
						$("#QueueMinderGroup_MeetingID").append(
							$("<option></option>")
								.text(item.MeetingName + " - " + item.MeetingReferenceNumber)
								.val(item.MeetingID)
						);
					});
				}
			});
		} else {
			$.ajax({
				url: "/AutoComplete.mvc/AutoCompleteAvailableMeetings", type: "POST", dataType: "json",
				data: { hierarchyItem: $('#QueueMinderGroup_HierarchyCode').val(), hierarchyType: $("#QueueMinderGroup_HierarchyType").val(), resultCount: 5000 },
				success: function (data) {
					$.each(data, function (index, item) {
						$("#QueueMinderGroup_MeetingID").append(
							$("<option></option>")
								.text(item.MeetingName + " - " + item.MeetingReferenceNumber)
								.val(item.MeetingID)
						);
					});
				}
			});
		}

	} else {
		$('#QueueMinderGroup_MeetingID').val("").attr("disabled", true);
	}
}
