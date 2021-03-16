/*
OnReady
*/
$(document).ready(function() {

    //Navigation
    $('#menu_clientfeegroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#ClientFeeGroup_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#ClientFeeGroup_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    if ($('#ClientFeeGroup_EnabledDate').val() == "") {
        $('#ClientFeeGroup_EnabledDate').val("No Enabled Date")
    }
    if ($('#ClientFeeGroup_ExpiryDate').val() == "") {
        $('#ClientFeeGroup_ExpiryDate').val("No Expiry Date")
    }
    //Hierarchy Disable/Enable OnLoad
    if ($("#ClientFeeGroup_HierarchyType").val() == "") {
        $("#ClientFeeGroup_HierarchyItem").val("");
        $("#ClientFeeGroup_HierarchyItem").attr("disabled", true);
    } else {
        $("#ClientFeeGroup_HierarchyItem").removeAttr("disabled");
        if ($("#ClientFeeGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
            $("#lblHierarchyItem").text("ClientSubUnit");
            $('#TravelerType').css('display', '');
        }
    }

    //Hierarchy Disable/Enable OnChange
    $("#ClientFeeGroup_HierarchyType").change(function () {
        $("#lblHierarchyItemMsg").text("");
        $("#ClientFeeGroup_HierarchyItem").val("");
        if ($("#ClientFeeGroup_QueueMinderGroupId").val() == "0") {
            $("#lblAuto").text("");
            $("#ClientFeeGroup_QueueMinderGroupName").val("");
        }
        if ($("#ClientFeeGroup_HierarchyType").val() == "") {
            $("#ClientFeeGroup_HierarchyItem").attr("disabled", true);
            $('#TravelerType').css('display', 'none');
        } else {
            $("#ClientFeeGroup_HierarchyItem").removeAttr("disabled");
            $("#lblHierarchyItem").text($("#ClientFeeGroup_HierarchyType").val());
            $("#ClientFeeGroup_HierarchyCode").val("");
            $('#TravelerType').css('display', 'none');
            if ($("#ClientFeeGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
                $("#lblHierarchyItem").text("ClientSubUnit");
                $("#ClientFeeGroup_TravelerTypeName").val("");
                $("#ClientFeeGroup_TravelerTypeGuid").val("");
                $('#TravelerType').css('display', '');
            }
        }
    });

    /*
    Submit Form Validation
    */
    $('#form0').submit(function() {
    
        var validItem = false;
        var validTravelerType = true;

    	if ($("#ClientFeeGroup_IsMultipleHierarchy").val() == "True") {
            validItem = true;
        }else{
            if ($("#ClientFeeGroup_HierarchyType").val() != "Multiple") {
                jQuery.ajax({

                    type: "POST",
                    url: "/Hierarchy.mvc/IsValid" + $("#ClientFeeGroup_HierarchyType").val(),
                    data: { searchText: encodeURIComponent($("#ClientFeeGroup_HierarchyItem").val()) },
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

            if ($("#ClientFeeGroup_HierarchyType").val() != "") {
                jQuery.ajax({
                    type: "POST",
                    url: "/Hierarchy.mvc/IsValidTravelerType",
                    data: { searchText: $("#ClientFeeGroup_TravelerTypeName").val() },
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
        if ($("#ClientFeeGroup_ClientFeeGroupId").val() == "0") {
            if ($("#lblAuto").text() == "") {
                return false;
            }
        } else {
            if (jQuery.trim($("#ClientFeeGroup_ClientFeeGroupName").val()) == "") {
                $("#ClientFeeGroup_ClientFeeGroupName_validationMessage").removeClass('field-validation-valid');
                $("#ClientFeeGroup_ClientFeeGroupName_validationMessage").addClass('field-validation-error');
                $("#ClientFeeGroup_ClientFeeGroupName_validationMessage").text("Client Fee Group Name Required.");
                return false;
            } else {
                $("#ClientFeeGroupName_validationMessage").text("");
            }
        }

        //GroupName Begin
        var validGroupName = false;

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableClientFeeGroupName",
            data: { groupName: $("#ClientFeeGroup_ClientFeeGroupName").val(), id: $("#ClientFeeGroup_ClientFeeGroupId").val() },
            success: function(data) {

                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblClientFeeGroupNameMsg").removeClass('field-validation-valid');
            $("#lblClientFeeGroupNameMsg").addClass('field-validation-error');
            if ($("#ClientFeeGroup_ClientFeeGroupId").val() == "0") {//Create
                $("#lblClientFeeGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
            } else {
                if ($("#ClientFeeGroup_ClientFeeGroupName").val()!="") {
                    $("#lblClientFeeGroupNameMsg").text("This name has already been used, please choose a different name.");
                }
            } return false;
        } else {
            $("#lblClientFeeGroupNameMsg").text("");
        }

        //GroupName End
        if (!$(this).valid()) {
            return false;
        }

        if (validItem && validTravelerType) {
            if ($('#ClientFeeGroup_ExpiryDate').val() == "No Expiry Date") {
                $('#ClientFeeGroup_ExpiryDate').val("");
            }
            if ($('#ClientFeeGroup_EnabledDate').val() == "No Enabled Date") {
                $('#ClientFeeGroup_EnabledDate').val("");
            }
            return true;
        } else {
            return false
        };
    });



});

$(function() {


    $("#ClientFeeGroup_HierarchyItem").autocomplete({
        source: function(request, response) {

        	if ($("#ClientFeeGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
        		$.ajax({
        			url: "/ClientFeeGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
        			data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#ClientFeeGroup_TravelerTypeGuid").val(), resultCount: 5000 },
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
        	} else if ($("#ClientFeeGroup_HierarchyType").val() == "TravelerType") {
        		$.ajax({
        			url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
        			data: { searchText: request.term, hierarchyItem: "TravelerType", domainName: 'ClientFee', resultCount: 5000 },
        			success: function (data) {
        				response($.map(data, function (item) {
        					return {
        						label: "<span class=\"tt-name\">" + item.HierarchyName + "</span>" + (item.ClientSubUnitName == "" ? "" : "<span class=\"tt-csu-name\">" + item.ClientSubUnitName + "</span>"),
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
        			data: { searchText: request.term, hierarchyItem: $("#ClientFeeGroup_HierarchyType").val(), domainName: 'ClientFee', resultCount: 5000 },
        			success: function (data) {
        				response($.map(data, function (item) {
        					if (
                                    $("#ClientFeeGroup_HierarchyType").val() == "GlobalRegion" ||
                                    $("#ClientFeeGroup_HierarchyType").val() == "GlobalSubRegion" ||
                                    $("#ClientFeeGroup_HierarchyType").val() == "Country"
                                ) {
        						return {
        							label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + " (" + item.HierarchyCode + ")",
        							value: item.HierarchyName,
        							id: item.HierarchyCode,
        							text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
        						}
        					} else if ($("#ClientFeeGroup_HierarchyType").val() == "ClientAccount") {
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
        select: function(event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#ClientFeeGroup_HierarchyItem").val(ui.item.value);
            $("#ClientFeeGroup_HierarchyCode").val(ui.item.id);
            $("#ClientFeeGroup_SourceSystemCode").val(ui.item.ssc);

             if ($("#ClientFeeGroup_ClientFeeGroupId").val() == "0") {//Create

                htft = ShortenHierarchyType($("#ClientFeeGroup_HierarchyType").val());

                //to get number for GroupName
                if ($("#ClientFeeGroup_HierarchyType").val() == "ClientAccount") {
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupNameClientAccount", type: "POST", dataType: "json",
                        data: { clientAccountNumber: $("#ClientFeeGroup_HierarchyCode").val(), sourceSystemCode: $("#ClientFeeGroup_SourceSystemCode").val(), group: "ClientFee" },
                        success: function(data) {
                            var maxNameSize = 100 - (htft.length + 5);
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft;
                            $("#lblAuto").text(autoName);
                            $("#ClientFeeGroup_ClientFeeGroupName").val(autoName);
                            $("#lblClientFeeGroupNameMsg").text("");
                        }
                    })
            } else {
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
                        data: { hierarchyType: $("#ClientFeeGroup_HierarchyType").val(), hierarchyItem: $("#ClientFeeGroup_HierarchyCode").val(), group: "ClientFee" },
                        success: function(data) {
                            var maxNameSize = 100 - (htft.length + 5);
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft;
                            $("#lblAuto").text(autoName);
                            $("#ClientFeeGroup_ClientFeeGroupName").val(autoName);
                            $("#lblClientFeeGroupNameMsg").text("");
                        }
                    })
                }
            }
        }
    });

    $('.ui-autocomplete').addClass('widget-overflow');

    $("#ClientFeeGroup_TravelerTypeName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/ClientFeeGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#ClientFeeGroup_HierarchyCode").val() },
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
            $("#ClientFeeGroup_TravelerTypeName").val(ui.item.value);
            $("#ClientFeeGroup_TravelerTypeGuid").val(ui.item.id);
        }
    });
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

    switch ($("#ClientFeeGroup_FeeTypeId").val()) {
        case "1":
            ft = "_SupFee";
            break;
        case "2":
            ft = "_TransFee";
            break;
        case "3":
            ft = "_MidOffTransFee";
            break;
        case "4":
            ft = "_MerchantFee";
            break;
    }
    

    return shortversion + ft;
}