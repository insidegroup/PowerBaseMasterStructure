
$(document).ready(function() {
    //Navigation
    $('#menu_teams').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

	//Only allow locations (US1774)

	////Hierarchy Disable/Enable OnLoad
    //if ($("#HierarchyType").val() == "") {
    //    $("#HierarchyItem").val("");
    //    $("#HierarchyItem").attr("disabled", true);
    //} else {
    //    $("#HierarchyItem").removeAttr("disabled");
    //}

    ////Hierarchy Disable/Enable OnChange
    //$("#HierarchyType").change(function() {
    //    $("#lblHierarchyItemMsg").text("");
    //    $("#HierarchyItem").val("");

    //    if ($("#HierarchyType").val() == "") {
    //        $("#HierarchyItem").attr("disabled", true);
    //    } else {
    //        $("#HierarchyItem").removeAttr("disabled");
    //        $("#lblHierarchyItem").text($("#HierarchyType").val());
    //        $("#HierarchyCode").val("");
    //    }
	//});
	
    $("#HierarchyItem").removeAttr("disabled");
	
	//End Only allow locations (US1774)

    $(function () {
        $("#CityCode_validationMessage").text("");
        $("#CityCode").autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/Cities", type: "POST", dataType: "json",
                    data: { searchText: request.term, maxResults: 10 },
                    success: function (data) {

                        response($.map(data, function (item) {
                            return {
                                label: item.Name + " (" + item.CityCode+ ")",
                                value: item.CityCode,
                                name: item.Name
                            }
                        }))
                    }
                })
            },
            select: function (event, ui) {
                $("#lblCityCode").text(ui.item.name); //label
                $("#CityCode").val(ui.item.value); //textbox
            }
        });

    });


    //Submit Form Validation
    $('#form0').submit(function() {

        var validItem = false;
        var validCity = true;

        if ($("#CityCode").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidCityCode",
                data: { cityCode: $("#CityCode").val() },
                success: function(data) {

                    if (jQuery.isEmptyObject(data)) {
                        validCity = false;
                    }
                },
                dataType: "json",
                async: false
            });
        }
        if (!validCity) {
            $("#lblCityCode").removeClass('field-validation-valid');
            $("#lblCityCode").addClass('field-validation-error');
            $("#lblCityCode").text("This is not a valid City Code.");
        } else {
            $("#CityCode").val($("#CityCode").val().toUpperCase())
        
            $("#lblCityCode").text("");
        }

        if ($("#HierarchyType").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValid" + $("#HierarchyType").val(),
                data: { searchText: encodeURIComponent($("#HierarchyItem").val()) },
                success: function(data) {

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
            } else {
                $("#lblHierarchyItemMsg").text("");
            }
        }

        

        if (validItem && validCity) {
            return true;
        } else {
            return false
        };
    });



});

$(function() {

    $("#HierarchyItem").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Role Based Access Team' },
                success: function (data) {
                response($.map(data, function(item) {
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
        },
        select: function(event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#HierarchyItem").val(ui.item.value);
            $("#HierarchyCode").val(ui.item.id);
        }
    });
});
