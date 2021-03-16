
$(document).ready(function() {

    //Navigation
    $('#menu_policies').click();
    $("tr:odd").not(":hidden").addClass("row_odd");
    $("tr:even").not(":hidden").addClass("row_even");

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

    if ($('#ExpiryDate').val() == "") {
        $('#ExpiryDate').val("No Expiry Date")
    }

    if ($('#EnabledDate').val() == "") {
        $('#EnabledDate').val("No Enabled Date")
    }

    //Hierarchy Disable/Enable OnLoad
    if ($("#HierarchyType").val() == "") {
        $("#HierarchyItem").val("");
        $("#HierarchyItem").attr("disabled", true);
        $("#HierarchyItem").attr("disabled", true);
    } else {
        $("#HierarchyItem").removeAttr("disabled");

        if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
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

    if ($('#HierarchyItem').val() == "") {
    	$('#MeetingID').val("").attr("disabled", true);
    }
});
    
//Hierarchy Disable/Enable OnChange
$("#HierarchyType").change(function() {
	$("#lblHierarchyItemMsg").text("");
	$("#HierarchyItem").val("");
	if ($("#PolicyGroupId").val() == "0") {
		$("#lblAuto").text("");
		$("#PolicyGroupName").val("");
	}
	if ($("#HierarchyType").val() == "") {
		$("#HierarchyItem").attr("disabled", true);
		$('#TravelerType').css('display', 'none');
	} else {
		$("#HierarchyItem").removeAttr("disabled");
		$("#lblHierarchyItem").text($("#HierarchyType").val());
		$("#HierarchyCode").val("");
		$('#TravelerType').css('display', 'none');
		if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
			$("#lblHierarchyItem").text("ClientSubUnit");
			$("#TravelerTypeName").val("");
			$("#TravelerTypeGuid").val("");
			$('#TravelerType').css('display', '');
		}
	}

	//Reset Dropdown
	$('#MeetingID')
		.attr("disabled", true)
		.empty()
		.append('<option value="">Please Select...</option>');

});

//Meetings
function LoadMeetings() {

	//Meetings
	if (
		$("#HierarchyType").val() == "ClientTopUnit" ||
		$("#HierarchyType").val() == "ClientSubUnit" ||
		$("#HierarchyType").val() == "ClientAccount" ||
		$("#HierarchyType").val() == "TravelerType" ||
		$("#HierarchyType").val() == "ClientSubUnitTravelerType"
	) {

		//Reset Dropdown
		$('#MeetingID')
			.attr("disabled", false)
			.empty()
			.append('<option value="">Please Select...</option>');			

		$('#MeetingID').val("");

		if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
			$.ajax({
				url: "/AutoComplete.mvc/AutoCompleteAvailableMeetings", type: "POST", dataType: "json",
				data: { hierarchyType: "ClientSubUnitTravelerType", hierarchyItem: $('#HierarchyCode').val(), travelerTypeGuid: $("#TravelerTypeGuid").val(), resultCount: 5000 },
				success: function (data) {
					$.each(data, function (index, item) {
						$("#MeetingID").append(
							$("<option></option>")
								.text(item.MeetingName + " - " + item.MeetingReferenceNumber)
								.val(item.MeetingID)
						);
					});
				}
			})
		} else if ($("#HierarchyType").val() == "TravelerType") {
			$.ajax({
				url: "/AutoComplete.mvc/AutoCompleteAvailableMeetings", type: "POST", dataType: "json",
				data: { hierarchyType: "TravelerType", hierarchyItem: $('#TT_CSU').val(), travelerTypeGuid: $('#HierarchyCode').val(), resultCount: 5000 },
				success: function (data) {
					$.each(data, function (index, item) {
						$("#MeetingID").append(
							$("<option></option>")
								.text(item.MeetingName + " - " + item.MeetingReferenceNumber)
								.val(item.MeetingID)
						);
					});
				}
			});
		} else if ($("#HierarchyType").val() == "ClientAccount") {
			$.ajax({
				url: "/AutoComplete.mvc/AutoCompleteAvailableMeetings", type: "POST", dataType: "json",
				data: { hierarchyType: "ClientAccount", clientAccountNumber: $('#HierarchyCode').val(), sourcesystemCode: $('#SourceSystemCode').val(), resultCount: 5000 },
				success: function (data) {
					$.each(data, function (index, item) {
						$("#MeetingID").append(
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
				data: { hierarchyItem: $('#HierarchyCode').val(), hierarchyType: $("#HierarchyType").val(), resultCount: 5000 },
				success: function (data) {
					$.each(data, function (index, item) {
						$("#MeetingID").append(
							$("<option></option>") 
								.text(item.MeetingName + " - " + item.MeetingReferenceNumber)
								.val(item.MeetingID)
						);
					});
				}
			});
		}

	} else {
		$('#MeetingID').val("").attr("disabled", true);
	}
}

//Hierarchy
$(function() {

	$("#HierarchyItem").autocomplete({
        source: function(request, response) {

        	if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
        		$.ajax({
        			url: "/PolicyGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
        			data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#TravelerTypeGuid").val() },
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
        	} else if ($("#HierarchyType").val() == "TravelerType") {
        		$.ajax({
        			url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
        			data: { searchText: request.term, hierarchyItem: "TravelerType", domainName: 'Policy Hierarchy', resultCount: 5000 },
        			success: function (data) {
        				response($.map(data, function (item) {
        					return {
        						label: "<span class=\"tt-name\">" + item.HierarchyName + "</span>" + (item.ClientSubUnitName == "" ? "" : "<span class=\"tt-csu-name\">" + item.ClientSubUnitName + "</span>"),
        						value: item.HierarchyName,
        						id: item.HierarchyCode,
        						text: item.HierarchyName,
        						ttcsu: item.ClientSubUnitName
        					}
        				}))
        			}
        		});
			} else {
        		$.ajax({
        			url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
        			data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Policy Hierarchy', resultCount: 5000 },
        			success: function (data) {
        				response($.map(data, function (item) {
        					if (
                                    $("#HierarchyType").val() == "GlobalRegion" ||
                                    $("#HierarchyType").val() == "GlobalSubRegion" ||
                                    $("#HierarchyType").val() == "Country"
                                ) {
        						return {
        							label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + " (" + item.HierarchyCode + ")",
        							value: item.HierarchyName,
        							id: item.HierarchyCode,
        							text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
        						}
        					} else if ($("#HierarchyType").val() == "ClientAccount") {
        						return {
                                    label: "<span class=\"ca-name\">" + item.HierarchyName + "</span><span class=\"ca-number\">" + item.ClientAccountNumber + "</span><span class=\"ca-ssc\">" + item.SourceSystemCode + "</span>",
        							value: item.HierarchyName,
        							id: item.ClientAccountNumber,
        							ssc: item.SourceSystemCode,
        							text: ""
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
        		});
            }

        },
        select: function (event, ui) {
        	$("#lblHierarchyItemMsg").text(ui.item.text);
        	$("#HierarchyItem").val(ui.item.value);
        	$("#HierarchyCode").val(ui.item.id);
        	$("#SourceSystemCode").val(ui.item.ssc);
        	$("#TT_CSU").val(ui.item.ttcsu);

        	if ($("#PolicyGroupId").val() == "0") {//Create
        		//to get number for GroupName
        		if ($("#HierarchyType").val() == "ClientAccount") {
        			$.ajax({
        				url: "/GroupNameBuilder.mvc/BuildGroupNameClientAccount", type: "POST", dataType: "json",
        				data: { clientAccountNumber: $("#HierarchyCode").val(), sourceSystemCode: $("#SourceSystemCode").val(), group: "Policy Hierarchy" },
        				success: function (data) {
        					var maxNameSize = 100 - ($("#HierarchyType").val().length + 12);
        					var autoName = replaceSpecialChars(ui.item.value)
        					autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + $("#HierarchyType").val() + "_Policy";
        					$("#lblAuto").text(autoName);
        					$("#PolicyGroupName").val(autoName);
        					$("#lblPolicyGroupNameMsg").text("");
        				}
        			})
        		} else {
        			$.ajax({
        				url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
        				data: { hierarchyType: $("#HierarchyType").val(), hierarchyItem: $("#HierarchyCode").val(), group: "Policy Hierarchy" },
        				success: function (data) {
        					var maxNameSize = 100 - ($("#HierarchyType").val().length + 12);
        					var autoName = replaceSpecialChars(ui.item.value)
        					autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + $("#HierarchyType").val() + "_Policy";
        					$("#lblAuto").text(autoName);
        					$("#PolicyGroupName").val(autoName);
        					$("#lblPolicyGroupNameMsg").text("");
        				}
        			})
        		}
        	}
        	LoadMeetings();
        }	
    });

    $('.ui-autocomplete').addClass('widget-overflow');

    $("#TravelerTypeName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/PolicyGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#HierarchyCode").val() },
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
            $("#TravelerTypeName").val(ui.item.value);
            $("#TravelerTypeGuid").val(ui.item.id);
            LoadMeetings();
        }
    });

   
});



//Submit Form Validation
$('#form0').submit(function () {

    var validItem = false;
    var validTravelerType = true;

    if ($("#IsMultipleHierarchy").val() == "True") {
    	validItem = true;
    } else {
    if ($("#HierarchyType").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Hierarchy.mvc/IsValid" + $("#HierarchyType").val(),
            data: { searchText: encodeURIComponent($("#HierarchyItem").val()) },
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

        if ($("#HierarchyType").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValidTravelerType",
                data: { searchText: $("#TravelerTypeName").val() },
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
    if ($("#PolicyGroupId").val() == "0") {
        if ($("#lblAuto").text() == "") {
            return false;
        }
    } else {
        if (jQuery.trim($("#PolicyGroupName").val()) == "") {
            $("#PolicyGroupName_validationMessage").removeClass('field-validation-valid');
            $("#PolicyGroupName_validationMessage").addClass('field-validation-error');
            $("#PolicyGroupName_validationMessage").text("Policy Group Name Required.");
            return false;
        } else {
            $("#PolicyGroupName_validationMessage").text("");
        }
    }

    //GroupName Begin
    var validGroupName = false;

    jQuery.ajax({
        type: "POST",
        url: "/GroupNameBuilder.mvc/IsAvailablePolicyGroupName",
        data: { groupName: $("#PolicyGroupName").val(), id: $("#PolicyGroupId").val() },
        success: function (data) {

            validGroupName = data;
        },
        dataType: "json",
        async: false
    });

    if (!validGroupName) {

        $("#lblPolicyGroupNameMsg").removeClass('field-validation-valid');
        $("#lblPolicyGroupNameMsg").addClass('field-validation-error');
        if ($("#PublicHolidayGroupId").val() == "0") {//Create
            $("#lblPolicyGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
        } else {
            if ($("#PolicyGroupName").val() != "") {
                $("#lblPolicyGroupNameMsg").text("This name has already been used, please choose a different name.");
            }
        }
        return false;
    } else {
        $("#lblPolicyGroupNameMsg").text("");
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
