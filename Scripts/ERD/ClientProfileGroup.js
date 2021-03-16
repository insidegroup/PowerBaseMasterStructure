/*
OnReady
*/
$(document).ready(function() {

    $('#menu_clientprofiles').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Hierarchy Disable/Enable OnChange
    $("#ClientProfileGroup_HierarchyType").change(function () {
        $("#lblHierarchyItemMsg").text("");
        $("#ClientProfileGroup_HierarchyItem").val("");
        if ($("#ClientProfileGroup_HierarchyType").val() == "") {
            $("#ClientProfileGroup_HierarchyItem").attr("disabled", true);
        } else {
            $("#ClientProfileGroup_HierarchyItem").removeAttr("disabled");
            $("#lblHierarchyItem").text($("#ClientProfileGroup_HierarchyType").val());
            $("#ClientProfileGroup_HierarchyCode").val("");
            if ($("#ClientProfileGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
                $("#lblHierarchyItem").text("ClientSubUnit");
                $("#ClientProfileGroup_TravelerTypeName").val("");
                $("#ClientProfileGroup_TravelerTypeGuid").val("");
            }
        }
    });

	//Uppercase Profile Name
    $('#ClientProfileGroup_ClientProfileGroupName').blur(function () {
    	$(this).val($(this).val().toUpperCase());
    });

    /*
    Submit Form Validation
    */
    $('#form0').submit(function() {
    
    	//Uppercase Profile Name on save
    	$('#ClientProfileGroup_ClientProfileGroupName').val($('#ClientProfileGroup_ClientProfileGroupName').val().toUpperCase());

    	//Uppercase PCC on save
    	$('#ClientProfileGroup_PseudoCityOrOfficeId').val($('#ClientProfileGroup_PseudoCityOrOfficeId').val().toUpperCase());

    	//Uppercase Hierarchy Item on save
    	$('#ClientProfileGroup_HierarchyItem').val($('#ClientProfileGroup_HierarchyItem').val().toUpperCase());

    	var validItem = false;

        jQuery.ajax({
			type: "POST",
            url: "/Hierarchy.mvc/IsValid" + $("#ClientProfileGroup_HierarchyType").val(),
            data: { searchText: encodeURIComponent($("#ClientProfileGroup_HierarchyItem").val()) },
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

        //wait for this name to be populated, dont show message
        if ($("#ClientProfileGroup_ClientProfileGroupId").val() == "0") {
            if ($("#lblAuto").text() == "") {
                return false;
            }
        } else {
        	if (jQuery.trim($("#ClientProfileGroup_ClientProfileGroupName").val()) == "") {
        		$("#ClientProfileGroup_ClientProfileName_validationMessage").removeClass('field-validation-valid');
        		$("#ClientProfileGroup_ClientProfileName_validationMessage").addClass('field-validation-error');
        		$("#ClientProfileGroup_ClientProfileName_validationMessage").text("Client Profile Name Required.");
                return false;
            } else {
        		$("#ClientProfileName_validationMessage").text("");
            }
        }

    	//IsValid PCC/GDS
        validPCCGDS = false;
        $('#lblValidPccGDSMessage').text();
        jQuery.ajax({
        	type: "POST",
        	url: "/GroupNameBuilder.mvc/IsValidPccGDS",
        	data: { pcc: $("#ClientProfileGroup_PseudoCityOrOfficeId").val(), gds: $("#ClientProfileGroup_GDSCode").val() },
        	success: function (data) {

        		validPCCGDS = data;
        	},
        	dataType: "json",
        	async: false
        });

        if (!validPCCGDS) {
        	$('#lblValidPccGDSMessage')
					.addClass('field-validation-error')
					.text('The PCC/Office ID you have selected is not valid for this GDS.');
        	return false;
        }

        //GroupName Begin
        var validGroupName = false;

        var formattedGroupName = FormatClientProfileName(
				escapeInput($("#ClientProfileGroup_HierarchyType").val()),			//HierarchyType
				escapeInput($("#ClientProfileGroup_HierarchyItem").val()),			//HierarchyItem
				escapeInput($("#ClientProfileGroup_GDSCode").val()),				//GDS
				escapeInput($("#ClientProfileGroup_PseudoCityOrOfficeId").val()),	//PCC/Office ID
				escapeInput($("#ClientProfileGroup_ClientProfileGroupName").val())	//Profile Name
		);

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableClientProfileName",
            data: { groupName: formattedGroupName, id: escapeInput($("#ClientProfileGroup_ClientProfileGroupId").val()) },
            success: function(data) {

                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

        	$("#lblClientProfileNameMsg").removeClass('field-validation-valid');
        	$("#lblClientProfileNameMsg").addClass('field-validation-error');
            if ($("#ClientProfileGroup_ClientProfileGroupId").val() == "0") {//Create
            	$("#lblHierarchyItemMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
            } else {
            	if ($("#ClientProfileGroup_ClientProfileGroupName").val() != "") {
                	$("#lblHierarchyItemMsg").text("This name has already been used, please choose a different name.");
                }
            } return false;
        } else {
        	$("#lblHierarchyItemMsg").text("");
        	$("#ClientProfileGroup_UniqueName").val(formattedGroupName);
        }
    	//GroupName End

        if (!$(this).valid()) {
            return false;
        }

        if (validItem) {
            if ($('#ClientProfileGroup_ExpiryDate').val() == "No Expiry Date") {
                $('#ClientProfileGroup_ExpiryDate').val("");
            }
            if ($('#ClientProfileGroup_EnabledDate').val() == "No Enabled Date") {
                $('#ClientProfileGroup_EnabledDate').val("");
            }
            return true;
        } else {
            return false
        };
    });



});

$(function() {

    $("#ClientProfileGroup_HierarchyItem").autocomplete({
        source: function(request, response) {

                $.ajax({
                    url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: $("#ClientProfileGroup_HierarchyType").val(), domainName: 'Client Profile Builder' },
                    success: function (data) {
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
        select: function (event, ui) {
        	$("#lblHierarchyItemMsg").text(ui.item.text);
        	$("#ClientProfileGroup_HierarchyItem").val(ui.item.value);
        	$("#ClientProfileGroup_HierarchyCode").val(ui.item.id);
        }
    });
});


function FormatClientProfileName(hierarchyType, hierarchyItem, GDS, PCCOfficeID, profileName) {

	return replaceSpecialChars(
						hierarchyItem + "_" +
						ShortenHierarchyType(hierarchyType) + "_" +
						GDS + "_" +
						PCCOfficeID + "_" +
						profileName
					).replace(/ /g, '_');

}

function ShortenHierarchyType(hierarchyType) {
	shortversion = "";
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