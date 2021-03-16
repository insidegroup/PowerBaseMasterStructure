

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

        if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
            $("#lblHierarchyItem").text("ClientSubUnit");
            $('#TravelerType').css('display', '');
        }
    }

	//Filter Hierarchy Item
    $('#HierarchyItem').keyup(function () {
    	if ($("#HierarchyType").val() == 'ClientSubUnitGUID') {
    		$(this).val($(this).val().replace(/[^a-z0-9\:]/gi, ''));
    	}
    });

	//Reorder Hierarchy Types
    $("#HierarchyType option[value='ClientSubUnit']").after($("#HierarchyType option[value='ClientSubUnitGUID']"));

    //Hierarchy Disable/Enable OnChange
    $("#HierarchyType").change(function () {
        $("#lblHierarchyItemMsg").text("");
        $("#HierarchyItem").val("");
        if ($("#ChatFAQResponseGroupId").val() == "0") {
            $("#lblAuto").text("");
            $("#ChatFAQResponseGroupName").val("");
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

    	//Set Autocomplete minimum length based on Hierarchy Type
        if ($("#HierarchyType").val() == 'ClientSubUnitGUID') {
        	$("#HierarchyItem").autocomplete('option', 'minLength', 5);
        } else {
        	$("#HierarchyItem").autocomplete('option', 'minLength', 0);
        }
    });

    //Submit Form Validation
    $('#form0').submit(function () {

        var validItem = false;
        var validTravelerType = true;

        if ($("#IsMultipleHierarchy").val() == "True") {
        	validItem = true;
        } else {
        if ($("#HierarchyType").val() != "") {
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
        if ($("#ChatFAQResponseGroupId").val() == "0") {
            if ($("#lblAuto").text() == "") {
                return false;
            }
        } else {
            if (jQuery.trim($("#ChatFAQResponseGroupName").val()) == "") {
                $("#ChatFAQResponseGroupName_validationMessage").removeClass('field-validation-valid');
                $("#ChatFAQResponseGroupName_validationMessage").addClass('field-validation-error');
                $("#ChatFAQResponseGroupName_validationMessage").text("Chat FAQ Response Group Name Required.");
                return false;
            } else {
                $("#ChatFAQResponseGroupName_validationMessage").text("");
            }
        }

        //GroupName Begin
        var validGroupName = false;

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableChatFAQResponseGroupName",
            data: { groupName: $("#ChatFAQResponseGroupName").val(), id: $("#ChatFAQResponseGroupId").val() },
            success: function (data) {
                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblChatFAQResponseGroupNameMsg").removeClass('field-validation-valid');
            $("#lblChatFAQResponseGroupNameMsg").addClass('field-validation-error');
            if ($("#ChatFAQResponseGroupId").val() == "0") {//Create
                $("#lblChatFAQResponseGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
            } else {
                if ($("#ChatFAQResponseGroupName").val() != "") {
                    $("#lblChatFAQResponseGroupNameMsg").text("This name has already been used, please choose a different name.");
                }
            }
            return false;
        } else {
            $("#lblChatFAQResponseGroupNameMsg").text("");
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


    $("#HierarchyItem").autocomplete({
        source: function (request, response) {

            if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
            	$.ajax({
            		url: "/ChatFAQResponseGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
            		data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#TravelerTypeGuid").val(), resultCount: 5000 },
            		success: function (data) {
            			response($.map(data, function (item) {
            				return {
            					label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
            					value: item.HierarchyName,
            					id: item.HierarchyCode,
            					text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
            				}
            			}))
            		}
            	});
            } else if ($("#HierarchyType").val() == "TravelerType") {
            	$.ajax({
            		url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: "TravelerType", domainName: 'Chat FAQ Administrator', resultCount: 5000 },
            		success: function (data) {
            			response($.map(data, function (item) {
            				return {
                                label: "<span class=\"tt-name\">" + item.HierarchyName + "</span>" + (item.ClientTopUnitName == "" ? "" : "<span class=\"tt-csu-name\">" + item.ClientTopUnitName + "</span>"),
            					value: item.HierarchyName,
            					id: item.HierarchyCode,
            					text: item.HierarchyName
            				}
            			}))
            		}
            	});
            } else {
            	$.ajax({
            		url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Chat FAQ Administrator', resultCount: 5000 },
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
            				} else if ($("#HierarchyType").val() == "ClientSubUnitGUID") {
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
            $("#ClientSubUnitName").val(ui.item.clientSubUnitName);

            htft = ShortenHierarchyType($("#HierarchyType").val());

            if ($("#ChatFAQResponseGroupId").val() == "0") {//Create

                //to get number for GroupName
            	if ($("#HierarchyType").val() == "ClientAccount") {
            		$.ajax({
            			url: "/GroupNameBuilder.mvc/BuildGroupNameClientAccount", type: "POST", dataType: "json",
            			data: { clientAccountNumber: $("#HierarchyCode").val(), sourceSystemCode: $("#SourceSystemCode").val(), group: "FAQ Response" },
            			success: function (data) {
            				var maxNameSize = 50 - (htft.length + 16);
            				var autoName = replaceSpecialChars(ui.item.value)
            				autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft + "_FAQResponse";
            				$("#lblAuto").text(autoName);
            				$("#ChatFAQResponseGroupName").val(autoName);
            				$("#lblChatFAQResponseGroupNameMsg").text("");
            			}
            		})
            	} else {
            		$.ajax({
            			url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
            			data: { hierarchyType: UpdateHierarchyType($("#HierarchyType").val()), hierarchyItem: $("#HierarchyCode").val(), group: "FAQ Response" },
            			success: function (data) {
            				var maxNameSize = 50 - (htft.length + 16);  //DB-
            				var name = ($("#HierarchyType option:selected").text() == "ClientSubUnitGUID") ? ui.item.clientSubUnitName : ui.item.value;
            				var autoName = replaceSpecialChars(name)
            				autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft + "_FAQResponse";
            				$("#lblAuto").text(autoName);
            				$("#ChatFAQResponseGroupName").val(autoName);
            				$("#lblChatFAQResponseGroupNameMsg").text("");
            			}
            		})
            	}
            }
        }
    });

    $('.ui-autocomplete').addClass('widget-overflow');

    $("#TravelerTypeName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/ChatFAQResponseGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#HierarchyCode").val() },
                success: function (data) {
                    response($.map(data, function (item) {
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
        select: function (event, ui) {
            $("#lblTravelerTypeMsg").text(ui.item.text);
            $("#TravelerTypeName").val(ui.item.value);
            $("#TravelerTypeGuid").val(ui.item.id);
        }
    });
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