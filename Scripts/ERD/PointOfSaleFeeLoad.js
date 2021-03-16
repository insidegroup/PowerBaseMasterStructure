$(document).ready(function () {

	$("#content tr:odd").addClass("row_odd");
	$("#content tr:even").addClass("row_even");
	$('#search').hide();

    //Breadcrumbs
    $('#breadcrumb').css('width', 'auto');
    $('.full-width #search_wrapper').css('height', '22px');

    $('#SearchButton').button();

    $("#SearchButton").click(function () {
        $('#form0').submit();
    });

    //Clear Fields when remove text
    $("#ClientTopUnitName").change(function () {
        var clientTopUnitName = $(this).val();
        if (clientTopUnitName == '') {
            $("#ClientTopUnitGuid").val('');
        }
    });

    $("#ClientSubUnitName").change(function () {
        var clientTopUnitName = $(this).val();
        if (clientTopUnitName == '') {
            $("#ClientSubUnitGuid").val('');
        }
    });

    $("#TravelerTypeName").change(function () {
        var clientTopUnitName = $(this).val();
        if (clientTopUnitName == '') {
            $("#TravelerTypeGuid").val('');
        }
    });

    //Autocompletes
    $(function () {

        $("#ClientTopUnitName").autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
        			url: "/AutoComplete.mvc/ClientTopUnitName", type: "POST", dataType: "json",
        			data: { searchText: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
        						label: item.ClientTopUnitName,
        						value: item.ClientTopUnitName,
        						id: item.ClientTopUnitGuid,
        						text: item.ClientTopUnitName
                            }
                        }))
                    }
                });
            },
            select: function (event, ui) {
                $("#ClientTopUnitGuid").val(ui.item.id);
                $('#ClientSubUnitName').val('');
                $('#ClientSubUnitGuid').val('');
                $("#lblClientTopUnitGuid_Msg").text("");
            }
        });

        $("#ClientSubUnitName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/PointOfSaleFeeLoad.mvc/AutoCompleteClientTopUnitClientSubUnits", type: "POST", dataType: "json",
                    data: { searchText: request.term, clientTopUnitGuid: $("#ClientTopUnitGuid").val() },
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
               $("#ClientSubUnitGuid").val(ui.item.id);
            }
        });

        $("#TravelerTypeName").autocomplete({
            source: function (request, response) {
                if ($("#ClientSubUnitGuid").val() == '') {
                    $.ajax({
                        url: "/PointOfSaleFeeLoad.mvc/AutoCompleteClientTopUnitTravelerTypes", type: "POST", dataType: "json",
                        data: { searchText: request.term, domainName: 'Point of Sale Fee Administrator', clientTopUnitGuid: $("#ClientTopUnitGuid").val() },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return {
                                    label: item.HierarchyName + (item.ClientSubUnitName == "" ? "" : ", " + item.ClientSubUnitName),
                                    value: item.HierarchyName,
                                    id: item.HierarchyCode,
                                    text: item.HierarchyName + (item.ClientSubUnitName == "" ? "" : ", " + item.ClientSubUnitName)
                                }
                            }))
                        }
                    })
                } else {
                    $.ajax({
                        url: "/PointOfSaleFeeLoad.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                        data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#ClientSubUnitGuid").val() },
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
                }
            },
            select: function (event, ui) {
                $('#TravelerTypeName').val(ui.item.text);
                $('#TravelerTypeGuid').val(ui.item.id);
            }
        });
    });

    //Submit Form Validation
    $('#form0').submit(function () {

        var validItem = true;
        var validClientTopUnitGuid = true;
        var validClientSubUnitGuid = true;
        var validTravelerType = true;

        //Search field must have a client top selected
        if ($("#ClientTopUnitGuid").val() == "") {
            $("#lblClientTopUnitGuid_Msg").removeClass('field-validation-valid');
            $("#lblClientTopUnitGuid_Msg").addClass('field-validation-error');
            $("#lblClientTopUnitGuid_Msg").text("Client TopUnit is required.");
            return false;
        }

        //Validate ClientTopUnitName
        if ($("#ClientTopUnitName").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValidClientTopUnit",
                data: { searchText: $("#ClientTopUnitName").val() },
                success: function (data) {

                    if (jQuery.isEmptyObject(data)) {
                        validClientTopUnitGuid = false;
                    }
                },
                dataType: "json",
                async: false
            });
            if (!validClientTopUnitGuid) {
                $("#lblClientTopUnitGuid_Msg").removeClass('field-validation-valid');
                $("#lblClientTopUnitGuid_Msg").addClass('field-validation-error');
                $("#lblClientTopUnitGuid_Msg").text("This is not a valid entry.");
            } else {
                $("#lblClientTopUnitGuid_Msg").text("");
            }
        }

        //Validate ClientSubUnitName
        if ($("#ClientSubUnitName").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValidClientSubUnit",
                data: { searchText: $("#ClientSubUnitName").val() },
                success: function (data) {

                    if (jQuery.isEmptyObject(data)) {
                        validClientSubUnitGuid = false;
                    }
                },
                dataType: "json",
                async: false
            });
            if (!validClientSubUnitGuid) {
                $("#lblClientSubUnitGuid_Msg").removeClass('field-validation-valid');
                $("#lblClientSubUnitGuid_Msg").addClass('field-validation-error');
                $("#lblClientSubUnitGuid_Msg").text("This is not a valid entry.");
            } else {
                $("#lblClientSubUnitGuid_Msg").text("");
            }
        }

        //Validate TravelerTypeName
        if ($("#TravelerTypeName").val() != "") {
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

        if (validItem && validClientTopUnitGuid && validClientSubUnitGuid && validTravelerType) {
            return true;
        } else {
            return false
        };

    });

});