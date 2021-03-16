

$(document).ready(function() {
    $('#menu_localoperatinghours').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

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

    //Hierarchy Disable/Enable OnChange
    $("#HierarchyType").change(function() {
        $("#lblHierarchyItemMsg").text("");
        $("#HierarchyItem").val("");
        if ($("#LocalOperatingHoursGroupId").val() == "0") {
            $("#lblAuto").text("");
            $("#LocalOperatingHoursGroupName").val("");
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

    /*
    Submit Form Validation
    */
    $('#form0').submit(function () {

        var validItem = false;
        var validTravelerType = true;

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
        if ($("#LocalOperatingHoursGroupId").val() == "0") {
            if ($("#lblAuto").text() == "") {
                return false;
            }
        } else {
            if (jQuery.trim($("#LocalOperatingHoursGroupName").val()) == "") {
                $("#LocalOperatingHoursGroupName_validationMessage").removeClass('field-validation-valid');
                $("#LocalOperatingHoursGroupName_validationMessage").addClass('field-validation-error');
                $("#LocalOperatingHoursGroupName_validationMessage").text("Local Operating Hours Group Name Required.");
                return false;
            } else {
                $("#LocalOperatingHoursGroupName_validationMessage").text("");
            }
        }

        //GroupName Begin
        var validGroupName = false;

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableLocalOperatingHoursGroupName",
            data: { groupName: $("#LocalOperatingHoursGroupName").val(), id: $("#LocalOperatingHoursGroupId").val() },
            success: function(data) {

                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblLocalOperatingHoursGroupNameMsg").removeClass('field-validation-valid');
            $("#lblLocalOperatingHoursGroupNameMsg").addClass('field-validation-error');
            if ($("#LocalOperatingHoursGroupId").val() == "0") {//Create
                $("#lblLocalOperatingHoursGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
            } else {
                if ($("#LocalOperatingHoursGroupName").val() != "") {
                    $("#lblLocalOperatingHoursGroupNameMsg").text("This name has already been used, please choose a different name.");
                }
            }
            return false;
        } else {
            $("#lblLocalOperatingHoursGroupNameMsg").text("");
        }
        //GroupName End
        if (!$(this).valid()) {
            return false;
        }
        

        if (validItem && validTravelerType) {
            return true;
        } else {
            return false
        };
    });



});

$(function() {


    $("#HierarchyItem").autocomplete({
        source: function(request, response) {

            if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
            	$.ajax({
            		url: "/LocalOperatingHoursGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
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
            	})
            } else if ($("#HierarchyType").val() == "TravelerType") {
            	$.ajax({
            		url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
            		data: { searchText: request.term, hierarchyItem: "TravelerType", domainName: 'Local Operating Hours', resultCount: 5000 },
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
            	})
            } else {
            	$.ajax({
            		url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
            		data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Local Operating Hours', resultCount: 5000 },
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
        select: function(event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#HierarchyItem").val(ui.item.value);
            $("#HierarchyCode").val(ui.item.id);
            $("#SourceSystemCode").val(ui.item.ssc);

            if ($("#LocalOperatingHoursGroupId").val() == "0") {//Create

                //to get number for GroupName
                if ($("#HierarchyType").val() == "ClientAccount") {
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupNameClientAccount", type: "POST", dataType: "json",
                        data: { clientAccountNumber: $("#HierarchyCode").val(), sourceSystemCode: $("#SourceSystemCode").val(), group: "Local Operating Hours" },
                        success: function(data) {
                            var maxNameSize = 50 - ($("#HierarchyType").val().length + 25);
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + $("#HierarchyType").val() + "_LocalOperatingHours";
                            $("#lblAuto").text(autoName);
                            $("#LocalOperatingHoursGroupName").val(autoName);
                            $("#lblLocalOperatingHoursGroupNameMsg").text("");
                        }
                    })
                } else {
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
                        data: { hierarchyType: $("#HierarchyType").val(), hierarchyItem: $("#HierarchyCode").val(), group: "Local Operating Hours" },
                        success: function(data) {
                        	var maxNameSize = 50 - ($("#HierarchyType").val().length + 25);
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + $("#HierarchyType").val() + "_LocalOperatingHours";
                            $("#lblAuto").text(autoName);
                            $("#LocalOperatingHoursGroupName").val(autoName);
                            $("#lblLocalOperatingHoursGroupNameMsg").text("");
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
                url: "/LocalOperatingHoursGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
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
