
$(document).ready(function () {

    //Navigation
    $('#menu_pricetracking').click();
    $("#form0 > table > tbody > tr:odd").not(":hidden").addClass("row_odd");
    $("#form0 > table > tbody > tr:even").not(":hidden").addClass("row_even");

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

//PseudoCityOrOfficeId
$('#GDSCode').change(function () {
    BuildGroupName();
});

//PseudoCityOrOfficeId
$('#PseudoCityOrOfficeId').change(function () {
    BuildGroupName();
});

//SharedPseudoCityOrOfficeIdFlagSelectedValue
$('#SharedPseudoCityOrOfficeIdFlagSelectedValue').change(function () {
    var selectedValue = $(this).find('option:selected').val();
    if (selectedValue == "false" || selectedValue == "true") {
        $('#SharedPseudoCityOrOfficeIdFlag').val(selectedValue);
    } else {
        $('#SharedPseudoCityOrOfficeIdFlag').val("");
    }
});

//MidOfficeUsedForQCTicketingFlagSelectedValue
$('#MidOfficeUsedForQCTicketingFlagSelectedValue').change(function () {
    var selectedValue = $(this).find('option:selected').val();
    if (selectedValue == "false" || selectedValue == "true") {
        $('#MidOfficeUsedForQCTicketingFlag').val(selectedValue);
    } else {
        $('#MidOfficeUsedForQCTicketingFlag').val("");
    }
});

//SharedPseudoCityOrOfficeList 
$('.SharedPseudoCityOrOfficeList').each(function () {
    $(this).change(function () {
    var selectedValue = $(this).find('option:selected').val();
        if (selectedValue == "false" || selectedValue == "true") {
            $(this).next('.SharedPseudoCityOrOfficeIdFlag').val(selectedValue);
        } else {
            $(this).next('.SharedPseudoCityOrOfficeIdFlag').val("");
        }
    });
});

//Additional PCC/OID Add button
$('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item .btn-add').live('click', function (e) {

    e.preventDefault();

    //Clone last row
    var lastItem = $('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').last().clone();

    //If last item isn't filled in, prompt to complete
    var check_field = lastItem.find('.PseudoCityOrOfficeId');
    if (check_field.val() == '') {
        alert('Please complete last field before adding a new one');
        return false;
    }

    //Validate Duplicates
    var validateAdditionalPseudoCityOrOfficeIds = true;
    var additionalPseudoCityOrOfficeIds = [];
    $('.PseudoCityOrOfficeId').each(function () {
        if (jQuery.inArray($(this).val(), additionalPseudoCityOrOfficeIds) == -1) {
            additionalPseudoCityOrOfficeIds.push($(this).val());
        } else {
            alert('The PCC/Office IDs must be unique');
            validateAdditionalPseudoCityOrOfficeIds = false;
            return false;
        }
    });

    if (!validateAdditionalPseudoCityOrOfficeIds) {
        return false;
    }

    //Proceed to and cloned row to end
    $('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').last().after(lastItem);

    //Select the last item
    var newItem = $('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').last();

    //Increment Id
    var regExp = /\[([^\]]+)\]/;
    var first_field = newItem.find('.PseudoCityOrOfficeId');
    var first_field_name = first_field.attr('name');
    var first_field_id = regExp.exec(first_field_name);
    var new_id = Number(first_field_id[1]) + 1;

    //PseudoCityOrOfficeId
    var pseudoCityOrOfficeId = newItem.find('.PseudoCityOrOfficeId');
    pseudoCityOrOfficeId.attr('name', 'PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[' + new_id + '].PseudoCityOrOfficeId').val('');

    //SharedPseudoCityOrOfficeList
    var sharedPseudoCityOrOfficeList = newItem.find('.SharedPseudoCityOrOfficeList');
    sharedPseudoCityOrOfficeList.attr('name', 'PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[' + new_id + '].SharedPseudoCityOrOfficeList').val('false');

    //SharedPseudoCityOrOfficeIdFlag 
    var sharedPseudoCityOrOfficeIdFlag = newItem.find('.SharedPseudoCityOrOfficeIdFlag');
    sharedPseudoCityOrOfficeIdFlag.attr('name', 'PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[' + new_id + '].SharedPseudoCityOrOfficeIdFlag').val('false');
});

//Additional PCC/OID Remove btn
$('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item .btn-remove').live('click', function (e) {

    e.preventDefault();

    //Remove all items but clear last remaining ones
    var lineItemCount = $('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').length;
    if (lineItemCount > 1) {
        $(this).closest('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').remove();
    } else {
        $(this).closest('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').find('.PseudoCityOrOfficeId').val('');
        $(this).closest('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').find('.SharedPseudoCityOrOfficeList').val('false');
        $(this).closest('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').find('.SharedPseudoCityOrOfficeIdFlag').val('false');
    }

    //If removed a middle one, update all numbers
    for (var i = 0; i < $('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item').length; i++) {

        var item = $('.PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId_Line_Item:eq(' + i + ')');

        var new_id = i + 1;

        //PseudoCityOrOfficeId
        var pseudoCityOrOfficeId = item.find('.PseudoCityOrOfficeId');
        pseudoCityOrOfficeId.attr('name', 'PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[' + new_id + '].PseudoCityOrOfficeId');

        //SharedPseudoCityOrOfficeList
        var sharedPseudoCityOrOfficeList = item.find('.SharedPseudoCityOrOfficeList');
        sharedPseudoCityOrOfficeList.attr('name', 'PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[' + new_id + '].SharedPseudoCityOrOfficeList');

        //SharedPseudoCityOrOfficeIdFlag 
        var sharedPseudoCityOrOfficeIdFlag = item.find('.SharedPseudoCityOrOfficeIdFlag');
        sharedPseudoCityOrOfficeIdFlag.attr('name', 'PriceTrackingSetupGroupAdditionalPseudoCityOrOfficeId[' + new_id + '].SharedPseudoCityOrOfficeIdFlag');
    }

});

//Hierarchy Disable/Enable OnChange
$("#HierarchyType").change(function () {
    $("#lblHierarchyItemMsg").text("");
    $("#HierarchyItem").val("");
    if ($("#PriceTrackingSetupGroupId").val() == "0") {
        $("#lblAuto").text("");
        $("#PriceTrackingSetupGroupName").val("");
    }
    if ($("#HierarchyType").val() == "") {
        $("#HierarchyItem").attr("disabled", true);
        $('#TravelerType').css('display', 'none');
        ResetFields();
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
        ResetFields();
    }
});

//Exclduded TravelerTypes Add button
$('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item .btn-add').live('click', function (e) {

    e.preventDefault();

    //Clone last row
    var lastItem = $('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').last().clone();

    //If last item isn't filled in, prompt to complete
    var check_field = lastItem.find('.TravelerTypeGuid');
    if (check_field.val() == '') {
        alert('Please complete last field before adding a new one');
        return false;
    }

    //Validate Duplicates
    var validateAdditionalTravelerTypeGuids = true;
    var additionalTravelerTypeGuids = [];
    $('.TravelerTypeGuid').each(function () {
        if (jQuery.inArray($(this).val(), additionalTravelerTypeGuids) == -1) {
            additionalTravelerTypeGuids.push($(this).val());
        } else {
            alert('The TravelerTypes must be unique');
            validateAdditionalTravelerTypeGuids = false;
            return false;
        }
    });

    if (!validateAdditionalTravelerTypeGuids) {
        return false;
    }

    //Proceed to and cloned row to end
    $('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').last().after(lastItem);

    //Select the last item
    var newItem = $('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').last();

    //Increment Id
    var regExp = /\[([^\]]+)\]/;
    var first_field = newItem.find('.TravelerTypeGuid');
    var first_field_name = first_field.attr('name');
    var first_field_id = regExp.exec(first_field_name);
    var new_id = Number(first_field_id[1]) + 1;

    //TravelerTypeGuid
    var travelerTypeGuid = newItem.find('.TravelerTypeGuid');
    travelerTypeGuid.attr('name', 'PriceTrackingSetupGroupExcludedTravelerType[' + new_id + '].TravelerTypeGuid').val('');

    //TravelerTypeName
    var travelerTypeName = newItem.find('.TravelerTypeName');
    travelerTypeName.attr('name', 'PriceTrackingSetupGroupExcludedTravelerType[' + new_id + '].TravelerTypeName').val('');
    enableAutoCompleteTravelerTypes(travelerTypeName);

});

//Exclduded TravelerTypes Remove btn
$('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item .btn-remove').live('click', function (e) {

    e.preventDefault();

    //Remove all items but clear last remaining ones
    var lineItemCount = $('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').length;
    if (lineItemCount > 1) {
        $(this).closest('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').remove();
    } else {
        $(this).closest('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').find('.TravelerTypeGuid').val('');
        $(this).closest('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').find('.TravelerTypeName').val('');
    }

    //If removed a middle one, update all numbers
    for (var i = 0; i < $('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').length; i++) {

        var item = $('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item:eq(' + i + ')');

        var new_id = i + 1;

        //TravelerTypeGuid
        var travelerTypeGuid = item.find('.TravelerTypeGuid');
        travelerTypeGuid.attr('name', 'PriceTrackingSetupGroupExcludedTravelerType[' + new_id + '].TravelerTypeGuid');

        //TravelerTypeName
        var travelerTypeName = item.find('.TravelerTypeName');
        travelerTypeName.attr('name', 'PriceTrackingSetupGroupExcludedTravelerType[' + new_id + '].TravelerTypeName');
    }

});

function ResetFields() {

    //Excluded Traveler Types
    $('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').slice(1).remove();
    var firstItem = $('.PriceTrackingSetupGroupExcludedTravelerType_Line_Item').first();
    firstItem.find('.TravelerTypeName').val('');
    firstItem.find('.TravelerTypeGuid').val('');
}

//Hierarchy
$(function () {

    $("#HierarchyItem").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Price Tracking Setup Administrator', resultCount: 5000 },
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

        },
        select: function (event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#HierarchyItem").val(ui.item.value);
            $("#HierarchyCode").val(ui.item.id);
            $("#SourceSystemCode").val(ui.item.ssc);
            $("#TT_CSU").val(ui.item.ttcsu);
            BuildGroupName();
            ResetFields();
        }
    });
    $('.ui-autocomplete').addClass('widget-overflow');
});

//Update FIQID
//Updates on GDS/PCC/Hierarchy change, or form submit if blank (Edit page is hidden or editable with role)
function BuildFIQID() {

    //Can be changed on edit page so don't overwrite if set
	if ($("#PriceTrackingSetupGroupId").val() == "0") {

    	$("#lblFIQIDMsg").text("");

        var autoName = replaceSpecialChars($('#HierarchyItem').val());
        var gdsCode = replaceSpecialChars($('#GDSCode').val());
        var pseudoCityOrOfficeId = replaceSpecialChars($('#PseudoCityOrOfficeId').val());

        if (autoName != '' && gdsCode != '' && pseudoCityOrOfficeId != '') {

            var fIQID = autoName + "_" + gdsCode + "_" + pseudoCityOrOfficeId;

            $("#lblFIQID").text(fIQID);
            $("#FIQID").val(fIQID);
        }
    }
}

//Update Group Name
function BuildGroupName() {

    var autoName = replaceSpecialChars($('#HierarchyItem').val());
    var gdsCode = replaceSpecialChars($('#GDSCode').val());
    var pseudoCityOrOfficeId = replaceSpecialChars($('#PseudoCityOrOfficeId').val());

    if ($("#PriceTrackingSetupGroupId").val() == "0") {

        if (autoName != '' && gdsCode != '' && pseudoCityOrOfficeId != '') {

            var suffix = ($("#HierarchyType").val() == 'ClientTopUnit') ? "CTU" : "CSU";

            var priceTrackingSetupGroupName = autoName + "_" + gdsCode + "_" + pseudoCityOrOfficeId + "_" + suffix;

            $("#lblAuto").text(priceTrackingSetupGroupName);
            $("#PriceTrackingSetupGroupName").val(priceTrackingSetupGroupName);
            $("#lblPriceTrackingSetupGroupNameMsg").text("");
        }

        BuildFIQID();
    }
}

//Autocomplete for Traveler Types
function enableAutoCompleteTravelerTypes(field) {
    $(field).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/PriceTrackingSetupGroup.mvc/AutoCompleteTravelerTypes", type: "POST", dataType: "json",
                data: {
                    searchText: request.term,
                    hierarchyType: $("#HierarchyType").val(),
                    hierarchyItem: $("#HierarchyCode").val(),
                    priceTrackingSetupGroupId: $("#PriceTrackingSetupGroupId").val() != "" ? $("#PriceTrackingSetupGroupId").val() : 0
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.TravelerTypeName + (item.ParentName == "" ? "" : ", " + item.ParentName),
                            value: item.TravelerTypeName,
                            id: item.TravelerTypeGuid,
                            text: item.TravelerTypeName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                        }
                    }))
                }
            })
        },
        select: function (event, ui) {
            $(this).val(ui.item.value);
            $(this).parent().find(".TravelerTypeGuid").val(ui.item.id);
        }
    });
}

$(function () {
    $(".TravelerTypeName").each(function () {
        enableAutoCompleteTravelerTypes($(this));
    });
});

//Submit Form Validation
$('#form0').submit(function () {

    //Hierarchy
    var validItem = false;

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

    //wait for this name to be populated, dont show message
    if ($("#PriceTrackingSetupGroupId").val() == "0") {
        if ($("#lblAuto").text() == "") {
            return false;
        }
    } else {
        if (jQuery.trim($("#PriceTrackingSetupGroupName").val()) == "") {
            $("#PriceTrackingSetupGroupName_validationMessage").removeClass('field-validation-valid');
            $("#PriceTrackingSetupGroupName_validationMessage").addClass('field-validation-error');
            $("#PriceTrackingSetupGroupName_validationMessage").text("Price Tracking Setup Name Required.");
            return false;
        } else {
            $("#PriceTrackingSetupGroupName_validationMessage").text("");
        }
    }

    //Set FIQID if empty
    BuildFIQID();

    //GroupName Begin
    //The combination of GDS / PseudoCityOrOfficeId / Hierarchy should be unique across all PriceTrackingSetupGroups (UnDeleted and Deleted)

    var validGroupName = false;

    if ($("#IsMultipleHierarchy").val() == "True") {
        validGroupName = true;
    } else {
        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailablePriceTrackingSetupGroup",
            data: {
                id: $("#PriceTrackingSetupGroupId").val(),
                pseudoCityOrOfficeId: $("#PseudoCityOrOfficeId").val(),
                gdsCode: $("#GDSCode").val(),
                hierarchyType: $("#HierarchyType").val(),
                hierarchyCode: $("#HierarchyCode").val()
            },
            success: function (data) {
                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblPriceTrackingSetupGroupNameMsg").removeClass('field-validation-valid');
            $("#lblPriceTrackingSetupGroupNameMsg").addClass('field-validation-error');
            if ($("#PriceTrackingSetupGroupId").val() == "0") {//Create
                $("#lblPriceTrackingSetupGroupNameMsg").text("This combination of GDS / PseudoCityOrOfficeId / Hierarchy has already been used, please reselect " + $("#lblHierarchyItem").text() + " or edit the existing group.");
            } else {
                if ($("#PriceTrackingSetupGroupName").val() != "") {
                    $("#lblPriceTrackingSetupGroupNameMsg").text("This combination of GDS / PseudoCityOrOfficeId / Hierarchy has already been used, please choose a different combination or edit the existing group.");
                }
            }
            return false;
        } else {
            $("#lblPriceTrackingSetupGroupNameMsg").text("");
        }
    }
    //GroupName End

    //IsValid PseudoCityOrOfficeId/GDS
    var validBookingPCCGDS = true;
    var pseudoCityOrOfficeId = $("#PseudoCityOrOfficeId").val();
    var gds = $("#GDSCode").val();

    $('#lblValidBookingPseudoCityOrOfficeIdMessage').text("");

    if (pseudoCityOrOfficeId !== '' && gds !== '') {
        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsValidPccGDS",
            data: { pcc: pseudoCityOrOfficeId, gds: gds },
            success: function (data) {
                validBookingPCCGDS = data;
            },
            dataType: "json",
            async: false
        });

        if (!validBookingPCCGDS) {
            $('#lblValidBookingPseudoCityOrOfficeIdMessage')
                .addClass('field-validation-error')
                .text('The PCC/Office ID you have selected is not valid for this GDS.');
            validItem = false;
            return false;
        }
    }

    //AdditionalPseudoCityOrOfficeId - Check for duplicates and valid entries
    var validateAdditionalPseudoCityOrOfficeIds = true;
    var additionalPseudoCityOrOfficeIds = [];

    $('.PseudoCityOrOfficeId').each(function () {

        var pcc = $(this).val();

        if (pcc != '') {

            //Reset errors
            $('.PseudoCityOrOfficeId').removeClass('field-validation-error');

            //Validate Duplicates
            if (jQuery.inArray($(this).val(), additionalPseudoCityOrOfficeIds) == -1) {
                additionalPseudoCityOrOfficeIds.push($(this).val());
            } else {
                $(this).addClass('field-validation-error');
                alert('The PCC/Office IDs must be unique');
                validateAdditionalPseudoCityOrOfficeIds = false;
                return false;
            }

            //Validate PCC
            jQuery.ajax({
                type: "POST",
                url: "/GroupNameBuilder.mvc/IsValidPccGDS",
                data: { pcc: pcc, gds: gds },
                success: function (data) {
                    validateAdditionalPseudoCityOrOfficeIds = data;
                },
                dataType: "json",
                async: false
            });

            if (!validateAdditionalPseudoCityOrOfficeIds) {
                $(this).addClass('field-validation-error');
                alert('The PCC/Office ID ' + pcc + ' is not valid for this GDS.');
                validateAdditionalPseudoCityOrOfficeIds = false;
                return false;
            }
        }
    });

    //Account Numbers - Check for duplicates and valid entries
    var validateAccountNumbers = true;
    var accountNumbers = [];

    $('.ClientAccountName').each(function () {

        var clientAccountName = $(this).val();
        var clientAccountNumber = $(this).parent().find(".ClientAccountNumber").val();
        var sourceSystemCode = $(this).parent().find(".SourceSystemCode").val();

        if (clientAccountNumber != '' && sourceSystemCode != '') {

            var key = clientAccountNumber + "_" + sourceSystemCode;

            //Reset errors
            $('.ClientAccountName').removeClass('field-validation-error');

            //Validate Duplicates
            if (jQuery.inArray(key, accountNumbers) == -1) {

                //Add to list
                accountNumbers.push(key);

                //Validate Account
                jQuery.ajax({
                    type: "POST",
                    url: "/Hierarchy.mvc/IsValidClientAccount",
                    data: { searchText: encodeURIComponent(clientAccountName) },
                    success: function (data) {

                        if (!jQuery.isEmptyObject(data)) {
                            validItem = true;
                        }
                    },
                    dataType: "json",
                    async: false
                });

                if (!validItem) {
                    $(this).addClass('field-validation-error');
                    alert('The Account "' + clientAccountName + '" is not valid.');
                    validateAccountNumbers = false;
                    return false;
                }

            } else {
                $(this).addClass('field-validation-error');
                alert('The Account Numbers must be unique');
                validateAccountNumbers = false;
                return false;
            }
        }
    });

    if (!validateAccountNumbers) {
        return false;
    }

    //TravelerTypes - Check for duplicates and valid entries
    var validateExcludedTravelerTypeGuids = true;
    var excludedTravelerTypes = [];

    $('.TravelerTypeName').each(function () {

        var travelerTypeName = $(this).val();
        var travelerTypeGuid = $(this).parent().find(".TravelerTypeGuid").val();

        if (travelerTypeGuid != '') {

            //Reset errors
            $('.TravelerTypeName').removeClass('field-validation-error');

            //Validate Duplicates
            if (jQuery.inArray(travelerTypeGuid, excludedTravelerTypes) == -1) {
                excludedTravelerTypes.push(travelerTypeGuid);
            } else {
                $(this).addClass('field-validation-error');
                alert('The Traveler Types must be unique');
                validateExcludedTravelerTypeGuids = false;
                return false;
            }

            //Validate TravelerType
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValidTravelerType",
                data: { searchText: encodeURIComponent(travelerTypeGuid) },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validItem = true;
                    }
                },
                dataType: "json",
                async: false
            });

            if (!validItem) {
                $(this).addClass('field-validation-error');
                alert('The Traveler Type "' + travelerTypeName + '" is not valid.');
                validateExcludedTravelerTypeGuids = false;
                return false;
            }
        }
    });

    if (!validateExcludedTravelerTypeGuids) {
        return false;
    }

    if (!$(this).valid()) {
        return false;
    }

    if (validItem && validGroupName && validateAdditionalPseudoCityOrOfficeIds && validateAccountNumbers && validateExcludedTravelerTypeGuids) {
        return true;
    } else {
        return false;
    };
});
