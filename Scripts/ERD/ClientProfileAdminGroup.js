/*
OnReady
*/
$(document).ready(function() {

    $("#ClientProfileAdminGroup_GDSCode").change(function () {
            nameGroup();
    });
    $("#ClientProfileAdminGroup_BackOfficeSytemId").change(function () {
            nameGroup();
    });

    
	//Navigation
	$('#menu_clientprofiles').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Hierarchy Disable/Enable OnChange
    $("#ClientProfileAdminGroup_HierarchyType").change(function () {
        $("#lblHierarchyItemMsg").text("");
        $("#ClientProfileAdminGroup_HierarchyItem").val("");
        if ($("#ClientProfileAdminGroup_HierarchyType").val() == "") {
            $("#ClientProfileAdminGroup_HierarchyItem").attr("disabled", true);
        } else {
        	$("#ClientProfileAdminGroup_HierarchyItem").removeAttr("disabled");
            $("#lblHierarchyItem").text($("#ClientProfileAdminGroup_HierarchyType").val());
            $("#ClientProfileAdminGroup_HierarchyCode").val("");
        }
    });

    /*
    Submit Form Validation
    */
    $('#form0').submit(function() {
    
        
        var validItem = false;

        jQuery.ajax({
            type: "POST",
            url: "/Hierarchy.mvc/IsValid" + $("#ClientProfileAdminGroup_HierarchyType").val(),
            data: { searchText: encodeURIComponent($("#ClientProfileAdminGroup_HierarchyItem").val()) },
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
            $("#lblAuto").text("");
            return false;

        } else {
            $("#lblHierarchyItemMsg").text("");
        }

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableClientProfileAdminGroupName",
            data: { groupName: $("#ClientProfileAdminGroup_ClientProfileGroupName").val(), id: $("#ClientProfileAdminGroup_ClientProfileAdminGroupId").val() },
            success: function (data) {

                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblClientProfileAdminGroupNameMsg").removeClass('field-validation-valid');
            $("#lblClientProfileAdminGroupNameMsg").addClass('field-validation-error');
            $("#lblClientProfileAdminGroupNameMsg").text("This combination has already been used in an existing group (group may be marked as deleted). Please choose a different combination of fields, or undelete existing group.");
            return false;
        } else {
            $("#lblClientProfileAdminGroupNameMsg").text("");
        }


    });



});

$(function() {

	$("#ClientProfileAdminGroup_HierarchyItem").autocomplete({
        source: function(request, response) {

                $.ajax({
                    url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: $("#ClientProfileAdminGroup_HierarchyType").val(), domainName: 'Client Profile Builder Administrator' },
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
        	$("#ClientProfileAdminGroup_HierarchyItem").val(ui.item.value);
        	$("#ClientProfileAdminGroup_HierarchyCode").val(ui.item.id);
        	nameGroup();
        }
    });
});

function ShortenHierarchyType(hierarchyType) {
	shortversion = "";
	switch (hierarchyType) {
		case "GlobalRegion":
			shortversion = "GRegion";
			break;
		case "GlobalSubRegion":
			shortversion = "GSubRegion";
			break;
		default:
			shortversion = hierarchyType;
	}

	return shortversion;
}

function ShortenGDSCode(gdsCode) {
	shortversion = "";
	switch (gdsCode) {
		case "Amadeus":
			shortversion = "1A";
			break;
		case "Apollo":
			shortversion = "1V";
			break;
		case "Galileo":
			shortversion = "1G";
			break;
		case "Sabre":
			shortversion = "1S";
			break;
		default:
			shortversion = gdsCode;
	}

	return shortversion;
}

function nameGroup(hierarchyItem) {

	var htft = ShortenHierarchyType(escapeInput($("#ClientProfileAdminGroup_HierarchyType").val()));
    var gds = ShortenGDSCode(escapeInput($("#ClientProfileAdminGroup_GDSCode").val()));
    var backOffice = $("#ClientProfileAdminGroup_BackOfficeSytemId option:selected").text();

    var hierarchyItem = escapeInput($("#ClientProfileAdminGroup_HierarchyItem").val());
    hierarchyItem = replaceSpecialChars(hierarchyItem).replace(/\s/g, "-");

    if (htft != "" && gds != "" && hierarchyItem != "" && backOffice != "") {
        var autoName = hierarchyItem + "_" + gds + "_" + backOffice + "_" + htft;
        $("#lblAuto").text(autoName);
        $("#ClientProfileAdminGroup_ClientProfileGroupName").val(autoName);

    }
}