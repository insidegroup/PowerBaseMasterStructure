
$(document).ready(function() {

    //Navigation
	$('#menu_fopadvicemessages').click();
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
});
    
//Hierarchy Disable/Enable OnChange
$("#HierarchyType").change(function() {
	$("#lblHierarchyItemMsg").text("");
	$("#HierarchyItem").val("");
	if ($("#FormOfPaymentAdviceMessageGroupID").val() == "0") {
		$("#lblAuto").text("");
		$("#FormOfPaymentAdviceMessageGroupName").val("");
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
});

//Hierarchy
$(function() {

	$("#HierarchyItem").autocomplete({
        source: function(request, response) {

        	if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
        		$.ajax({
                    url: "/FormOfPaymentAdviceMessageGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
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
        			data: { searchText: request.term, hierarchyItem: "TravelerType", domainName: 'Client Detail', resultCount: 5000 },
        			success: function (data) {
        				response($.map(data, function (item) {
        					return {
                                label: "<span class=\"tt-name\">" + item.HierarchyName + "</span>" + (item.ClientTopUnitName == "" ? "" : "<span class=\"tt-csu-name\">" + item.ClientTopUnitName + "</span>"),
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
                    data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Client Detail', resultCount: 5000 },
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
                            } else if ($("#HierarchyType").val() == "ClientSubUnit") {
                                return {
                                    label: "<span class=\"tt-name\">" + item.HierarchyName + "</span>" + (item.ParentName == "" ? "" : "<span class=\"tt-csu-name\">" + item.ParentName + "</span>"),
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

            htft = ShortenHierarchyType($("#HierarchyType").val());

        	if ($("#FormOfPaymentAdviceMessageGroupID").val() == "0") {//Create
        		//to get number for GroupName
        		if ($("#HierarchyType").val() == "ClientAccount") {
        			$.ajax({
        				url: "/GroupNameBuilder.mvc/BuildGroupNameClientAccount", type: "POST", dataType: "json",
        				data: { clientAccountNumber: $("#HierarchyCode").val(), sourceSystemCode: $("#SourceSystemCode").val(), group: "Form of Payment Advice Message" },
        				success: function (data) {
                            var maxNameSize = 170 - (htft.length + 16);
        					var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft + "_FOPAdviceMessage";
        					$("#lblAuto").text(autoName);
        					$("#FormOfPaymentAdviceMessageGroupName").val(autoName);
        					$("#lblFormOfPaymentAdviceMessageGroupNameMsg").text("");
        				}
        			})
        		} else {
        			$.ajax({
        				url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
        				data: { hierarchyType: $("#HierarchyType").val(), hierarchyItem: $("#HierarchyCode").val(), group: "Form of Payment Advice Message" },
        				success: function (data) {
                            var maxNameSize = 170 - (htft.length + 16);
        					var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft + "_FOPAdviceMessage";
        					$("#lblAuto").text(autoName);
        					$("#FormOfPaymentAdviceMessageGroupName").val(autoName);
        					$("#lblFormOfPaymentAdviceMessageGroupNameMsg").text("");
        				}
        			})
        		}
        	}
        }	
    });

    $('.ui-autocomplete').addClass('widget-overflow');

    $("#TravelerTypeName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/FormOfPaymentAdviceMessageGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
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
        }
    });

   
});



//Submit Form Validation
$('#form0').submit(function () {

    var validItem = false;
    var validTravelerType = true;

    var hierarchyType = $("#HierarchyType").val();
    
    if (hierarchyType == "Multiple") {
    	validItem = true
    } else {
    	jQuery.ajax({
    		type: "POST",
    		url: "/Hierarchy.mvc/IsValid" + hierarchyType,
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

    	if ($("#TravelerType").is(":visible")) {

    		if (hierarchyType != "") {
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
    }

    //wait for this name to be populated, dont show message
    if ($("#FormOfPaymentAdviceMessageGroupID").val() == "0") {
        if ($("#lblAuto").text() == "") {
            return false;
        }
    } else {
        if (jQuery.trim($("#FormOfPaymentAdviceMessageGroupName").val()) == "") {
            $("#FormOfPaymentAdviceMessageGroupName_validationMessage").removeClass('field-validation-valid');
            $("#FormOfPaymentAdviceMessageGroupName_validationMessage").addClass('field-validation-error');
            $("#FormOfPaymentAdviceMessageGroupName_validationMessage").text("Policy Group Name Required.");
            return false;
        } else {
            $("#FormOfPaymentAdviceMessageGroupName_validationMessage").text("");
        }
    }

    //GroupName Begin
    var validGroupName = false;

    jQuery.ajax({
        type: "POST",
        url: "/GroupNameBuilder.mvc/IsAvailableFormOfPaymentAdviceMessageGroupName",
        data: { groupName: $("#FormOfPaymentAdviceMessageGroupName").val(), id: $("#FormOfPaymentAdviceMessageGroupID").val() },
        success: function (data) {

            validGroupName = data;
        },
        dataType: "json",
        async: false
    });

    if (!validGroupName) {

        $("#lblFormOfPaymentAdviceMessageGroupNameMsg").removeClass('field-validation-valid');
        $("#lblFormOfPaymentAdviceMessageGroupNameMsg").addClass('field-validation-error');
        if ($("#FormOfPaymentAdviceMessageGroupID").val() == "0") {//Create
            $("#lblFormOfPaymentAdviceMessageGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
        } else {
            if ($("#FormOfPaymentAdviceMessageGroupName").val() != "") {
                $("#lblFormOfPaymentAdviceMessageGroupNameMsg").text("This name has already been used, please choose a different name.");
            }
        }
        return false;
    } else {
        $("#lblFormOfPaymentAdviceMessageGroupNameMsg").text("");
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


function ShortenHierarchyType(hierarchyType) {
    switch (hierarchyType) {
        case "ClientTopUnit":
            shortversion = "CTU";
            break;
        case "ClientSubUnit":
            shortversion = "CSU";
            break;
        case "ClientSubUnitTravelerType":
            shortversion = "CSUTT";
            break;
        case "TravelerType":
            shortversion = "TT";
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