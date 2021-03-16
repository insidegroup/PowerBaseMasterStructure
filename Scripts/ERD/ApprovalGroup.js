

$(document).ready(function () {
    //Navigation
    $('#menu_approvalgroups').click();
    $("#form0 > table > tbody > tr:visible:odd").addClass("row_odd");
    $("#form0 > table > tbody > tr:visible:even").addClass("row_even");

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

    //Hierarchy Disable/Enable OnChange
    $("#HierarchyType").change(function () {
        $("#lblHierarchyItemMsg").text("");
        $("#HierarchyItem").val("");
        if ($("#ApprovalGroupId").val() == "0") {
            $("#lblAuto").text("");
            $("#ApprovalGroupName").val("");
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

        //GroupName Begin
        var validGroupName = false;

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableApprovalGroupName",
            data: { groupName: $("#ApprovalGroupName").val(), id: $("#ApprovalGroupId").val() },
            success: function (data) {
                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblApprovalGroupNameMsg").removeClass('field-validation-valid');
            $("#lblApprovalGroupNameMsg").addClass('field-validation-error');
            if ($("#ApprovalGroupId").val() == "0") {//Create
                $("#lblApprovalGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
            } else {
                if ($("#ApprovalGroupName").val() != "") {
                    $("#lblApprovalGroupNameMsg").text("This name has already been used, please choose a different name.");
                }
            }
            return false;
        } else {
            $("#lblApprovalGroupNameMsg").text("");
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
            return false;
        }
    });

    $("#HierarchyItem").autocomplete({
        source: function (request, response) {

            if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
                $.ajax({
                    url: "/ApprovalGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
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
                    data: { searchText: request.term, hierarchyItem: "TravelerType", domainName: 'Approval Group Administrator', resultCount: 5000 },
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
                    data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Approval Group Administrator', resultCount: 5000 },
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

            htft = ShortenHierarchyType($("#HierarchyType").val());

            if ($("#ApprovalGroupId").val() == "0") {//Create

                //to get number for GroupName
                if ($("#HierarchyType").val() == "ClientAccount") {
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupNameClientAccount", type: "POST", dataType: "json",
                        data: { clientAccountNumber: $("#HierarchyCode").val(), sourceSystemCode: $("#SourceSystemCode").val(), group: "Approval Group Administrator" },
                        success: function (data) {
                            var maxNameSize = 50 - (htft.length + 16);
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft + "_Approval";
                            $("#lblAuto").text(autoName);
                            $("#ApprovalGroupName").val(autoName);
                            $("#lblApprovalGroupNameMsg").text("");
                        }
                    })
                } else {
                    $.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
                        data: { hierarchyType: $("#HierarchyType").val(), hierarchyItem: $("#HierarchyCode").val(), group: "Approval Group Administrator" },
                        success: function (data) {
                            var maxNameSize = 50 - (htft.length + 16);  //DB-
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft + "_Approval";
                            $("#lblAuto").text(autoName);
                            $("#ApprovalGroupName").val(autoName);
                            $("#lblApprovalGroupNameMsg").text("");
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
                url: "/ApprovalGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
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

    //Add button
    $('.btn-add').live('click', function (e) {

        e.preventDefault();

        //Prevent adding new lines until existing filled in
        var input_value = $(this).closest('.approvalGroupApprovalTypeItem').find('.item-input').val();
        var select_value = $(this).closest('.approvalGroupApprovalTypeItem').find('.item-select').val();
        if (input_value == '' || select_value == '') {
            alert('Please complete field before adding new one');
            return false;
        }

        //Clone last row and add to end
        var lastItem = $('.approvalGroupApprovalTypeItem').last().clone();
        $('.approvalGroupApprovalTypeItem').last().after(lastItem);

        //Increment the id and label
        var newItem = $('.approvalGroupApprovalTypeItem').last();

        var input = newItem.find('input');
        var id = input.attr('id');
        var number = id.replace('ApprovalGroupApprovalTypeItem_', '');
        var newId = parseInt(number) + 1;

        input.attr('id', 'ApprovalGroupApprovalTypeItem_' + newId);
        input.attr('name', 'ApprovalGroupApprovalTypeItem_' + newId);
        input.val('');

        var select = newItem.find('select');
        select.attr('id', 'ApprovalGroupApprovalTypeItem_' + newId);
        select.attr('name', 'ApprovalGroupApprovalTypeItem_' + newId);
        select.val('');
    });

    //Remove btn
    $('.btn-remove').live('click', function (e) {

        e.preventDefault();

        //Remove all items but clear last remaining ones
        var command_count = $('.approvalGroupApprovalTypeItem').length;
        if (command_count > 1) {
            $(this).closest('.approvalGroupApprovalTypeItem').remove();
        } else {
            $(this).closest('.approvalGroupApprovalTypeItem').find('.item-input').val('');
            $(this).closest('.approvalGroupApprovalTypeItem').find('.item-select').val('');
        }

        //If removed a middle one, update all numbers
        for (var i = 0; i < $('.approvalGroupApprovalTypeItem').length; i++) {

            var item = $('.approvalGroupApprovalTypeItem:eq(' + i + ')');

            var newId = i + 1;

            var input = item.find('input');
            input.attr('id', 'ApprovalGroupApprovalTypeItem_' + newId);
            input.attr('name', 'ApprovalGroupApprovalTypeItem_' + newId);

            var select = item.find('select');
            select.attr('id', 'ApprovalGroupApprovalTypeItem_' + newId);
            select.attr('name', 'ApprovalGroupApprovalTypeItem_' + newId);
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


	return shortversion;
}