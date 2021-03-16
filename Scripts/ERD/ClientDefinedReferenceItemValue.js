/*
OnReady
*/
$(document).ready(function() {

    //Navigation
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    $("#breadcrumb").css("width", "auto");


    /*
    Submit Form Validation
    */
    $('form').submit(function() {
    
        var validItem = false;
       
        //GroupName Begin
        var validGroupName = false;

        $("#lblClientDefinedReferenceItem_Value").text("");

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableClientDefinedReferenceValueItem",
            data: {
            	name: $("#ClientDefinedReferenceItemValue_Value").val(),
            	id: $("#ClientDefinedReferenceItemValue_ClientDefinedReferenceItemId").val(),
            	currentId: $("#ClientDefinedReferenceItemValue_ClientDefinedReferenceItemValueId").val()
            },
            success: function(data) {

                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblClientDefinedReferenceItem_Value").removeClass('field-validation-valid');
            $("#lblClientDefinedReferenceItem_Value").addClass('field-validation-error');
            $("#lblClientDefinedReferenceItem_Value").text("This name has already been used, please choose a different name.");
			return false;
        } else {
            $("#lblClientDefinedReferenceItem_Value").text("");
        }
        //GroupName End
        if (!$(this).valid()) {
            return false;
        }
    });



});

$(function() {


    $("#ClientFeeGroup_HierarchyItem").autocomplete({
        source: function(request, response) {

            if ($("#ClientFeeGroup_HierarchyType").val() != "ClientSubUnitTravelerType") {
                $.ajax({
                    url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: $("#ClientFeeGroup_HierarchyType").val(), domainName: 'ClientFee' },
                    success: function (data) {
                        response($.map(data, function(item) {
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
                                    label: item.HierarchyName,
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
                })
            } else {
                $.ajax({
                    url: "/ClientFeeGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#ClientFeeGroup_TravelerTypeGuid").val() },
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
                            $("#lblClientDefinedReferenceItem_Value").text("");
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
                            $("#lblClientDefinedReferenceItem_Value").text("");
                        }
                    })
                }
            }
        }
    });


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