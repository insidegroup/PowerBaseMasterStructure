$(document).ready(function () {

	$("#content tr:odd").addClass("row_odd");
	$("#content tr:even").addClass("row_even");
	$('#search').hide();

    //Reset FeeLoadAmount on Create
    $('.FeeLoadAmountCreate').val('');

	//Breadcrumbs
	$('#breadcrumb').css('width', 'auto');
	$('.full-width #search_wrapper').css('height', '22px');

  
    if ($('#PointOfSaleFeeLoad_ClientSubUnitGuid').val() == '') {
        $('#PointOfSaleFeeLoad_ClientSubUnitName').attr('disabled', true);
    }

    if ($('#PointOfSaleFeeLoad_TravelerTypeGuid').val() == '') {
        $('#PointOfSaleFeeLoad_TravelerTypeName').attr('disabled', true);
    }

    if ($('#PointOfSaleFeeLoad_ClientTopUnitGuid').val() != '') {
        $('#PointOfSaleFeeLoad_ClientSubUnitName').attr('disabled', false);
    }

    if ($('#PointOfSaleFeeLoad_ClientSubUnitGuid').val() != '') {
        $('#PointOfSaleFeeLoad_TravelerTypeName').attr('disabled', false);
    }

    $("#PointOfSaleFeeLoad_ClientTopUnitName").change(function () {
        var clientTopUnitName = $(this).val();
        if (clientTopUnitName == '') {
            $("#PointOfSaleFeeLoad_ClientTopUnitGuid").val('');
        }
    });

    $("#PointOfSaleFeeLoad_ClientSubUnitName").change(function () {
        var clientTopUnitName = $(this).val();
        if (clientTopUnitName == '') {
            $("#PointOfSaleFeeLoad_ClientSubUnitGuid").val('');
        }
    });

    $("#PointOfSaleFeeLoad_TravelerTypeName").change(function () {
        var clientTopUnitName = $(this).val();
        if (clientTopUnitName == '') {
            $("#PointOfSaleFeeLoad_TravelerTypeGuid").val('');
        }
    });

    $(function () {

        $("#PointOfSaleFeeLoad_ClientTopUnitName").autocomplete({
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
                $("#PointOfSaleFeeLoad_ClientTopUnitGuid").val(ui.item.id);
                $('#PointOfSaleFeeLoad_ClientSubUnitName').attr('disabled', false);
                $('#PointOfSaleFeeLoad_ClientSubUnitGuid').val('');
                $('#PointOfSaleFeeLoad_TravelerTypeName').attr('disabled', false);
                $('#PointOfSaleFeeLoad_TravelerTypeGuid').val('');
            }
        });

        $("#PointOfSaleFeeLoad_ClientSubUnitName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/PointOfSaleFeeLoad.mvc/AutoCompleteClientTopUnitClientSubUnits", type: "POST", dataType: "json",
                    data: { searchText: request.term, clientTopUnitGuid: $("#PointOfSaleFeeLoad_ClientTopUnitGuid").val() },
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
                $("#PointOfSaleFeeLoad_ClientSubUnitGuid").val(ui.item.id);
            }
        });

        $("#PointOfSaleFeeLoad_TravelerTypeName").autocomplete({
            source: function (request, response) {
                if ($("#PointOfSaleFeeLoad_ClientSubUnitGuid").val() == '') {
                    $.ajax({
                        url: "/PointOfSaleFeeLoad.mvc/AutoCompleteClientTopUnitTravelerTypes", type: "POST", dataType: "json",
                        data: { searchText: request.term, domainName: 'Point of Sale Fee Administrator', clientTopUnitGuid: $("#PointOfSaleFeeLoad_ClientTopUnitGuid").val() },
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
                        data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#PointOfSaleFeeLoad_ClientSubUnitGuid").val() },
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
                $('#PointOfSaleFeeLoad_TravelerTypeName').val(ui.item.text);
                $('#PointOfSaleFeeLoad_TravelerTypeGuid').val(ui.item.id);
            }
        });
	});

	//Submit Form Validation
	$('#form0').submit(function () {

		var validItem = true;

        var validClientTopUnitGuid = true;
        var validClientSubUnitGuid = true;
        var validTravelerType = true;

        if ($('#PointOfSaleFeeLoad_PointOfSaleFeeLoadId').val() == '') {

            if ($("#PointOfSaleFeeLoad_ClientTopUnitName").val() != "") {
                jQuery.ajax({
                    type: "POST",
                    url: "/Hierarchy.mvc/IsValidClientTopUnit",
                    data: { searchText: $("#PointOfSaleFeeLoad_ClientTopUnitName").val() },
                    success: function (data) {

                        if (jQuery.isEmptyObject(data)) {
                            validClientTopUnitGuid = false;
                        }
                    },
                    dataType: "json",
                    async: false
                });
                if (!validClientTopUnitGuid) {
                    $("#lblPointOfSaleFeeLoad_ClientTopUnitGuid_Msg").removeClass('field-validation-valid');
                    $("#lblPointOfSaleFeeLoad_ClientTopUnitGuid_Msg").addClass('field-validation-error');
                    $("#lblPointOfSaleFeeLoad_ClientTopUnitGuid_Msg").text("This is not a valid entry.");
                } else {
                    $("#lblPointOfSaleFeeLoad_ClientTopUnitGuid_Msg").text("");
                }
            }

            if ($("#PointOfSaleFeeLoad_ClientSubUnitName").val() != "") {
                jQuery.ajax({
                    type: "POST",
                    url: "/Hierarchy.mvc/IsValidClientSubUnit",
                    data: { searchText: $("#PointOfSaleFeeLoad_ClientSubUnitName").val() },
                    success: function (data) {

                        if (jQuery.isEmptyObject(data)) {
                            validClientSubUnitGuid = false;
                        }
                    },
                    dataType: "json",
                    async: false
                });
                if (!validClientSubUnitGuid) {
                    $("#lblPointOfSaleFeeLoad_ClientSubUnitGuid_Msg").removeClass('field-validation-valid');
                    $("#lblPointOfSaleFeeLoad_ClientSubUnitGuid_Msg").addClass('field-validation-error');
                    $("#lblPointOfSaleFeeLoad_ClientSubUnitGuid_Msg").text("This is not a valid entry.");
                } else {
                    $("#lblPointOfSaleFeeLoad_ClientSubUnitGuid_Msg").text("");
                }
            }

            if ($("#PointOfSaleFeeLoad_TravelerTypeName").val() != "") {
                jQuery.ajax({
                    type: "POST",
                    url: "/Hierarchy.mvc/IsValidTravelerType",
                    data: { searchText: $("#PointOfSaleFeeLoad_TravelerTypeName").val() },
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

        //Valid Item
        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailablePointOfSaleFeeLoad",
            data: {
                id: $("#PointOfSaleFeeLoad_PointOfSaleFeeLoadId").val(),
                clientTopUnitGuid: $("#PointOfSaleFeeLoad_ClientTopUnitGuid").val(),
                clientSubUnitGuid: $("#PointOfSaleFeeLoad_ClientSubUnitGuid").val(),
                travelerTypeGuid: $("#PointOfSaleFeeLoad_TravelerTypeGuid").val(),
                feeLoadDescriptionTypeCode: $("#PointOfSaleFeeLoad_FeeLoadDescriptionTypeCode").val(),
                productId: $("#PointOfSaleFeeLoad_ProductId").val(),
                agentInitiatedFlag: $("#PointOfSaleFeeLoad_AgentInitiatedFlag").is(':checked'),
                travelIndicator: $("#PointOfSaleFeeLoad_TravelIndicator").val(),
            },
            success: function (data) {
                validItem = data;
            },
            dataType: "json",
            async: false
        });

        if (!validItem) {
            $("#PointOfSaleFeeLoad_Error").removeClass('field-validation-valid');
            $("#PointOfSaleFeeLoad_Error").addClass('field-validation-error');
            $("#PointOfSaleFeeLoad_Error").text("This combination has already been used, please choose a different combination.");
            return false;
        } else {
            $("#PointOfSaleFeeLoad_Error").text("");
        }

        if (validItem && validClientTopUnitGuid && validClientSubUnitGuid && validTravelerType) {
			return true;
		} else {
			return false
		};

	});
});