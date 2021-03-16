/*
PolicyAirCabinGroupItem
	AddEditPolicyAirCabinGroupItem(policyAirCabinGroupItemId)
	AddPolicyAirCabinGroupItemPolicyRouting()
	PolicyAirCabinGroupItemValidation(addNewRouting)
	DeletePolicyAirCabinGroupItemPopup(PolicyAirCabinGroupItemId, VNBR)
PolicyAirMissedSavingsThresholdGroupItem
    AddEditPolicyAirMissedSavingsThresholdGroupItem
    DeletePolicyAirMissedSavingsThresholdGroupItemPopup
PolicyAirParameterGroupItem
	AddEditPolicyAirParameterGroupItem(policyAirParameterGroupItemId, policyAirParameterTypeId) )
	DeletePolicyAirParameterGroupItem(policyAirParameterGroupItemId, policyAirParameterTypeId, VNBR)
	PolicyAirParameterGroupItemValidation(addNewRouting)
PolicyAirVendorGroupItem
	AddEditPolicyAirVendorGroupItem(policyAirVendorGroupItemId)
	AddPolicyAirVendorGroupItemPolicyRouting()
	DeletePolicyAirVendorGroupItemPopup(PolicyAirVendorGroupItemId, VNBR)
	PolicyAirVendorGroupItemValidation(addNewRouting)
PolicyCarTypeGroupItem
	AddEditPolicyCarTypeGroupItem(policyCarTypeGroupItemId)
	DeletePolicyCarTypeGroupItemPopup(PolicyCarTypeGroupItemId, VNBR)
PolicyCarVendorGroupItem
	AddEditPolicyCarVendorGroupItem(policyCarVendorGroupItemId)
	DeletePolicyCarVendorGroupItemPopup(PolicyCarVendorGroupItemId, VNBR)
PolicyCityGroupItem
	AddEditPolicyCityGroupItem(policyCityGroupItemId)
	DeletePolicyCityGroupItemPopup(policyCityGroupItemId, VNBR)
PolicyCountryGroupItem
	AddEditPolicyCountryGroupItem(policyCountryGroupItemId)
	DeletePolicyCountryGroupItemPopup(policyCountryGroupItemId, VNBR)
PolicyHotelCapRateGroupItem
	AddEditPolicyHotelCapRateGroupItem(policyHotelCapRateGroupItemId)
	DeletePolicyHotelCapRateGroupItemPopup(policyHotelCapeRateItemId, VNBR)
PolicyHotelPropertyGroupItem
	AddEditPolicyHotelPropertyGroupItem(policyHotelPropertyGroupItemId)
	DeletePolicyHotelPropertyGroupItemPopup(PolicyHotelPropertyGroupItemId, VNBR)
PolicyHotelVendorGroupItem
	AddEditPolicyHotelVendorGroupItem(policyHotelVendorGroupItemId)
	DeletePolicyHotelVendorGroupItemPopup(PolicyHotelVendorGroupItemId, VNBR)
PolicySupplierDealCode
	AddEditPolicySupplierDealCode(policySupplierDealCodeId)
	DeletePolicySupplierDealCodePopup(PolicySupplierDealCodeId, VNBR)
PolicySupplierServiceInformation
	AddEditPolicySupplierServiceInformation(policySupplierServiceInformationId)
	DeletePolicySupplierServiceInformationPopup(PolicySupplierServiceInformationId, VNBR)
General Policy Functions
	ChoosePolicyScreen()
	ShowClientPolicyGroupScreen()
	ValidateClientPolicyGroup()
	ShowClientPoliciesScreen()
	SavePolicyAirParameterGroupItems()
	SavePolicyAirCabinGroupItems()
	SavePolicyAirVendorGroupItems()
	SavePolicyCarTypeGroupItems()
	SavePolicyCarVendorGroupItems()
	SavePolicyCountryGroupItems()
	SavePolicyHotelCapRateGroupItems()
	SavePolicyHotelPropertyGroupItems()
	SavePolicyHotelVendorGroupItems()
	SavePolicySupplierServiceInformations()
	SavePolicySupplierDealCodes()
*/

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyAirCabinGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyAirCabinGroupItem Popup
*/
function AddEditPolicyAirCabinGroupItem(policyAirCabinGroupItemId) {

    if (policyAirCabinGroupItemId == "0") {

        $("#dialog-confirm").html("");
        $("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirCabinGroupItemPopup?id=' + Math.random() +'&policyAirCabinGroupItemId=' + policyAirCabinGroupItemId).dialog({
            resizable: true,
            modal: true,
            height: 550,
            width: 600,
             buttons: {
                 "Save": function () {
                     PolicyAirCabinGroupItemValidation(false);
                 },
                 "Save and Add Another Routing": function () {
                     PolicyAirCabinGroupItemValidation(true);
                 },
                 "Close": function () {
                 
                     $("#dialog-confirm").dialog("close");
                 }
            }
        });
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Air Cabin Group Item");
	
    }else {
        $("#dialog-confirm").html("");
        $("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirCabinGroupItemPopup?id=' + Math.random() + '&policyAirCabinGroupItemId=' + policyAirCabinGroupItemId).dialog({
            resizable: true,
            modal: true,
            height: 550,
            width: 600,
             buttons: {
                 "Save": function () {
                     PolicyAirCabinGroupItemValidation(false);
                 },
                 "Close": function () {              
                     $("#dialog-confirm").dialog("close");
                 }
            }
        });
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Air Cabin Group Item", {zIndex:900});
    }
}


/*
Add Extra PolicyAirCabinGroupItem PolicyRouting Popup
*/
function AddPolicyAirCabinGroupItemPolicyRouting() {

	var PolicyAirCabinGroupItemId = escapeInput($("#PolicyAirCabinGroupItem_PolicyAirCabinGroupItemId").val());
	var PolicyGroupId = escapeInput($("#PolicyGroup_PolicyGroupId").val());				
	var AirlineCabinCode = $("#PolicyAirCabinGroupItem_AirlineCabinCode").val();
	var FlightDurationAllowedMin = escapeInput($("#PolicyAirCabinGroupItem_FlightDurationAllowedMin").val());
	var FlightDurationAllowedMax = escapeInput($("#PolicyAirCabinGroupItem_FlightDurationAllowedMax").val());
	var FlightMileageAllowedMin = escapeInput($("#PolicyAirCabinGroupItem_FlightMileageAllowedMin").val());
	var FlightMileageAllowedMax = escapeInput($("#PolicyAirCabinGroupItem_FlightMileageAllowedMax").val());
	var PolicyRoutingId = escapeInput($("#PolicyRouting_PolicyRoutingID").val());
	var EnabledDate = escapeInput($("#PolicyAirCabinGroupItem_EnabledDate").val());
	var ExpiryDate = escapeInput($("#PolicyAirCabinGroupItem_ExpiryDate").val());
	var TravelDateValidFrom = escapeInput($("#PolicyAirCabinGroupItem_TravelDateValidFrom").val());
	var TravelDateValidTo = escapeInput($("#PolicyAirCabinGroupItem_TravelDateValidTo").val());
	var VersionNumber = escapeInput($("#PolicyAirCabinGroupItem_VersionNumber").val());

	//Checkbox initially then hidden field for adding another policy routing
	var EnabledFlag = "";
	if ($("#PolicyAirCabinGroupItem_EnabledFlag").is(':checkbox')) {
		if ($("#PolicyAirCabinGroupItem_EnabledFlag").is(':checked')) {
			EnabledFlag = "True"
		} else {
			EnabledFlag = "False"
		}
	} else {
		if ($("#PolicyAirCabinGroupItem_EnabledFlag").val().toLowerCase() == 'true') {
			EnabledFlag = "True"
		} else {
			EnabledFlag = "False"
		}
	}

	//Checkbox initially then hidden field for adding another policy routing
	var PolicyProhibitedFlag = "";
	if ($("#PolicyAirCabinGroupItem_PolicyProhibitedFlag").is(':checkbox')) {
		if ($("#PolicyAirCabinGroupItem_PolicyProhibitedFlag").is(':checked')) {
			PolicyProhibitedFlag = "True"
		} else {
			PolicyProhibitedFlag = "False"
		}
	} else {
		if ($("#PolicyAirCabinGroupItem_PolicyProhibitedFlag").val().toLowerCase() == 'true') {
			PolicyProhibitedFlag = "True"
		} else {
			PolicyProhibitedFlag = "False"
		}
	}

	//Build Object to Store PolicyAirCabinGroupItem
	var policyAirCabinGroupItem = {
	    PolicyAirCabinGroupItemId: 0,
	    PolicyGroupId: $("#PolicyGroup_PolicyGroupId").val(),
	    AirlineCabinCode: $("#PolicyAirCabinGroupItem_AirlineCabinCode").val(),
	    FlightDurationAllowedMin: $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin").val(),
	    FlightDurationAllowedMax: $("#PolicyAirCabinGroupItem_FlightDurationAllowedMax").val(),
	    FlightMileageAllowedMin: $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin").val(),
	    FlightMileageAllowedMax: $("#PolicyAirCabinGroupItem_FlightMileageAllowedMax").val(),
	    PolicyProhibitedFlag: PolicyProhibitedFlag,
	    EnabledFlag: EnabledFlag,
	    EnabledDate: $("#PolicyAirCabinGroupItem_EnabledDate").val(),
	    ExpiryDate: $("#PolicyAirCabinGroupItem_ExpiryDate").val(),
	    TravelDateValidFrom: $("#PolicyAirCabinGroupItem_TravelDateValidFrom").val(),
	    TravelDateValidTo: $("#PolicyAirCabinGroupItem_TravelDateValidTo").val()
	};

	$("#dialog-confirm").html("");
	$("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirCabinGroupItemPolicyRoutingPopup?id=' + Math.random(), policyAirCabinGroupItem).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyAirCabinGroupItemValidation(false);
            },
            "Save and Add Another Routing": function () {
                PolicyAirCabinGroupItemValidation(true);
            },
            "Close": function () {
                
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    $('#dialog-confirm').dialog('option', 'title', "Add Policy Air Cabin Group Item");

}


/*
Validate PolicyAirCabinGroupItem and write to hidden tables or return validation errors
Parameter : addNewRouting : If true, reshow popup with blank routing information
*/
function PolicyAirCabinGroupItemValidation(addNewRouting) {

    /////////////////////////////////////////////////////////
    // BEGIN POLICYROUTING VALIDATION
    /////////////////////////////////////////////////////////
    var RoutingValidationOK = false;

    //PolicyRouting is mandatory on on PolicyAirCabinGroupItemPolicyRoutingPopup but not on PolicyAirCabinGroupItemPopup
    //if (escapeInput($("#MandatoryPolicyRouting").val()) == "0") {
    //    // if no Routing information filled in then valid
    //	if (escapeInput($("#PolicyRouting_Name").val()) == ""
    //            && !$("#PolicyRouting_FromGlobalFlag").is(':checked')
    //            && !$("#PolicyRouting_RoutingViceVersaFlag").is(':checked')
    //            && !$("#PolicyRouting_ToGlobalFlag").is(':checked')
    //            && escapeInput($("#PolicyRouting_FromCode").val()) == ""
    //            && escapeInput($("#PolicyRouting_ToCode").val()) == "") {
    //        RoutingValidationOK = true;
    //    }
    //}

    if (RoutingValidationOK == false) {

        var validFrom = false;
        var validTo = false;
        $("#lblFrom").text("");
        $("#lblTo").text("");

        if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
            validFrom = true;
        }
        if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
            validTo = true;
        }

        //if no "FROM" is set, should it be?
        if (!$("#PolicyRouting_FromGlobalFlag").is(':checked') && escapeInput($("#PolicyRouting_FromCode").val()) == "") {
            $("#lblFrom").removeClass('field-validation-valid');
            $("#lblFrom").addClass('field-validation-error');
            $("#lblFrom").text("Please enter a value or choose Global");
        }

        if (escapeInput($("#PolicyRouting_FromCode").val()) != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
                data: { fromto: $("#PolicyRouting_FromCode").val(), codetype: escapeInput($("#PolicyRouting_FromCodeType").val()) },
                success: function (data) {
                    if (!jQuery.isEmptyObject(data)) {
                        validFrom = true;
                    }
                },
                dataType: "json",
                async: false
            });

            if (!validFrom) {
                $("#lblFrom").removeClass('field-validation-valid');
                $("#lblFrom").addClass('field-validation-error');
                $("#lblFrom").text("This is not a valid entry");
            }
        };

        //if no "TO" is set, should it be?
        if (!$("#PolicyRouting_ToGlobalFlag").is(':checked') && escapeInput($("#PolicyRouting_ToCode").val()) == "") {
            $("#lblTo").removeClass('field-validation-valid');
            $("#lblTo").addClass('field-validation-error');
            $("#lblTo").text("Please enter a value or choose Global");
        }

        if (escapeInput($("#PolicyRouting_ToCode").val()) != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
                data: { fromto: $("#PolicyRouting_ToCode").val(), codetype: escapeInput($("#PolicyRouting_ToCodeType").val()) },
                success: function (data) {
                    if (!jQuery.isEmptyObject(data)) {
                        validTo = true;
                    }

                },
                dataType: "json",
                async: false
            });

            if (!validTo) {
                $("#lblTo").removeClass('field-validation-valid');
                $("#lblTo").addClass('field-validation-error');
                $("#lblTo").text("This is not a valid entry");
            }
        };

        //validation of PolicyRouting Failed
        if (!validFrom || !validTo) {
            return;
        }
    }
    /////////////////////////////////////////////////////////
    // END POLICYROUTING VALIDATION
    /////////////////////////////////////////////////////////

    //reset date fields - on PolicyAirCabinGroupItemPopup only
    if (escapeInput($("#MandatoryPolicyRouting").val()) == "0") {
    	if (escapeInput($('#PolicyAirCabinGroupItem_EnabledDate').val()) == "No Enabled Date") {
            $('#PolicyAirCabinGroupItem_EnabledDate').val("");
        }
    	if (escapeInput($('#PolicyAirCabinGroupItem_ExpiryDate').val()) == "No Expiry Date") {
            $('#PolicyAirCabinGroupItem_ExpiryDate').val("");
        }
        //reset date fields
    	if (escapeInput($('#PolicyAirCabinGroupItem_TravelDateValidFrom').val()) == "No Travel Date Valid From") {
            $('#PolicyAirCabinGroupItem_TravelDateValidFrom').val("");
        }
    	if (escapeInput($('#PolicyAirCabinGroupItem_TravelDateValidTo').val()) == "No Travel Date Valid To") {
            $('#PolicyAirCabinGroupItem_TravelDateValidTo').val("");
        }
    }
    //URL to post to
    var url = '/ClientWizard.mvc/PolicyAirCabinGroupItemValidation';

    //Build Object to Store PolicyAirCabinGroupItem
    var policyAirCabinGroupItem = {
        PolicyAirCabinGroupItemId: $("#PolicyAirCabinGroupItem_PolicyAirCabinGroupItemId").val(),
        PolicyGroupId: $("#PolicyAirCabinGroupItem_PolicyGroupId").val(),
        AirlineCabinCode: $("#PolicyAirCabinGroupItem_AirlineCabinCode").val(),
        FlightDurationAllowedMin: $("#PolicyAirCabinGroupItem_FlightDurationAllowedMin").val(),
        FlightDurationAllowedMax: $("#PolicyAirCabinGroupItem_FlightDurationAllowedMax").val(),
        FlightMileageAllowedMin: $("#PolicyAirCabinGroupItem_FlightMileageAllowedMin").val(),
        FlightMileageAllowedMax: $("#PolicyAirCabinGroupItem_FlightMileageAllowedMax").val(),
        PolicyRoutingId: $("#PolicyRouting_PolicyRoutingID").val(),
        PolicyProhibitedFlag: $("#PolicyAirCabinGroupItem_PolicyProhibitedFlag").is(':checked'),
        EnabledFlag: $("#PolicyAirCabinGroupItem_EnabledFlag").is(':checked'),
        EnabledDate: $("#PolicyAirCabinGroupItem_EnabledDate").val(),
        ExpiryDate: $("#PolicyAirCabinGroupItem_ExpiryDate").val(),
        TravelDateValidFrom: $("#PolicyAirCabinGroupItem_TravelDateValidFrom").val(),
        TravelDateValidTo: $("#PolicyAirCabinGroupItem_TravelDateValidTo").val(),
        VersionNumber: $("#PolicyAirCabinGroupItem_VersionNumber").val()
    };

    //Build Object to Store PolicyRouting
    var policyRouting = {
        Name: $("#PolicyRouting_Name").val(),
        FromGlobalFlag: $("#PolicyRouting_FromGlobalFlag").is(':checked'),
        FromCode: $("#PolicyRouting_FromCode").val(),
        FromCodeType: $("#PolicyRouting_FromCodeType").val(),
        ToGlobalFlag: $("#PolicyRouting_ToGlobalFlag").is(':checked'),
        ToCode: $("#PolicyRouting_ToCode").val(),
        ToCodeType: $("#PolicyRouting_ToCodeType").val(),
        RoutingViceVersaFlag: $("#PolicyRouting_RoutingViceVersaFlag").is(':checked')
    };

    //Build Object to Store Client Policy
    var clientPolicy = {
        PolicyAirCabinGroupItem: $.parseJSON(JSON.stringify(policyAirCabinGroupItem)),
        PolicyRouting: $.parseJSON(JSON.stringify(policyRouting))
    }

    //AJAX (JSON) POST of Client Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(clientPolicy),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            if (result.success) {

                //add RoutingName list of UnAvailable Names
                var arrUnAvailableNames = $("#UnAvailableRoutingNames").val().split(",");
                arrUnAvailableNames.push($("#PolicyRouting_Name").val());
                $("#UnAvailableRoutingNames").val(arrUnAvailableNames);

                var PolicyAirCabinGroupItemId = escapeInput($("#PolicyAirCabinGroupItem_PolicyAirCabinGroupItemId").val());
                var AddEditType = "";

                if (PolicyAirCabinGroupItemId == "0") {
                    AddEditType = "New";
                } else {
                    AddEditType = "Current";
                }

                var PolicyGroupId = escapeInput($("#PolicyGroup_PolicyGroupId").val());
                var AirlineCabinCode = escapeInput($("#PolicyAirCabinGroupItem_AirlineCabinCode").val());
                var FlightDurationAllowedMin = escapeInput($("#PolicyAirCabinGroupItem_FlightDurationAllowedMin").val());
                var FlightDurationAllowedMax = escapeInput($("#PolicyAirCabinGroupItem_FlightDurationAllowedMax").val());
                var FlightMileageAllowedMin = escapeInput($("#PolicyAirCabinGroupItem_FlightMileageAllowedMin").val());
                var FlightMileageAllowedMax = escapeInput($("#PolicyAirCabinGroupItem_FlightMileageAllowedMax").val());
                var PolicyRoutingId = escapeInput($("#PolicyRouting_PolicyRoutingId").val());
                var EnabledDate = escapeInput($("#PolicyAirCabinGroupItem_EnabledDate").val());
                var ExpiryDate = escapeInput($("#PolicyAirCabinGroupItem_ExpiryDate").val());
                var TravelDateValidFrom = escapeInput($("#PolicyAirCabinGroupItem_TravelDateValidFrom").val());
                var TravelDateValidTo = escapeInput($("#PolicyAirCabinGroupItem_TravelDateValidTo").val());
                var VersionNumber = escapeInput($("#PolicyAirCabinGroupItem_VersionNumber").val());

                var FromGlobalFlag = "";
                if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                    FromGlobalFlag = "Checked";
                } else {
                    FromGlobalFlag = "UnChecked";
                }

                var ToGlobalFlag = "";
                if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                    ToGlobalFlag = "Checked";
                } else {
                    ToGlobalFlag = "UnChecked";
                }

                var FromCode = escapeInput($("#PolicyRouting_FromCode").val());
                var FromCodeType = escapeInput($("#PolicyRouting_FromCodeType").val());
                var ToCode = escapeInput($("#PolicyRouting_ToCode").val());
                var ToCodeType = escapeInput($("#PolicyRouting_ToCodeType").val());

                var RoutingViceVersaFlag = "";
                if ($("#PolicyRouting_RoutingViceVersaFlag").is(':checked')) {
                    RoutingViceVersaFlag = "True"
                } else {
                    RoutingViceVersaFlag = "False"
                }                

            	//Checkbox initially then hidden field for adding another policy routing
                var EnabledFlag = "";
                if ($("#PolicyAirCabinGroupItem_EnabledFlag").is(':checkbox')) {
                	if ($("#PolicyAirCabinGroupItem_EnabledFlag").is(':checked')) {
                		EnabledFlag = "True"
                	} else {
                		EnabledFlag = "False"
                	}
                } else {
                	if ($("#PolicyAirCabinGroupItem_EnabledFlag").val().toLowerCase() == 'true') {
                		EnabledFlag = "True"
                	} else {
                		EnabledFlag = "False"
                	}
                }

            	//Checkbox initially then hidden field for adding another policy routing
                var PolicyProhibitedFlag = "";
                if ($("#PolicyAirCabinGroupItem_PolicyProhibitedFlag").is(':checkbox')) {
                	if ($("#PolicyAirCabinGroupItem_PolicyProhibitedFlag").is(':checked')) {
                		PolicyProhibitedFlag = "True"
                	} else {
                		PolicyProhibitedFlag = "False"
                	}
                } else {
                	if ($("#PolicyAirCabinGroupItem_PolicyProhibitedFlag").val().toLowerCase() == 'true') {
                		PolicyProhibitedFlag = "True"
                	} else {
                		PolicyProhibitedFlag = "False"
                	}
                }

                var PolicyRoutingName = escapeInput($("#PolicyRouting_Name").val());
                var ToCodeType = escapeInput($("#PolicyRouting_ToCodeType").val());
                var FromCodeType = escapeInput($("#PolicyRouting_FromCodeType").val());

                if (AddEditType == "Current") {
                    $('#currentPolicyAirCabinGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("PolicyAirCabinGroupItemId") == PolicyAirCabinGroupItemId) {
                            $(this).html("");
                            if (FromGlobalFlag == "Checked") {
                                if (ToGlobalFlag == "Checked") {
                                    $(this).html("<td>" + AirlineCabinCode + "</td><td>Custom</td><td>" + FlightDurationAllowedMin + "</td><td>" + FlightDurationAllowedMax + "</td><td>" + FlightMileageAllowedMin + "</td><td>" + FlightMileageAllowedMax + "</td><td>" + PolicyProhibitedFlag + "</td><td>Global to Global</td><td>" + RoutingViceVersaFlag + "</td><td>Modified</td>");
                                    $('#hiddenChangedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' AirlineCabinCode = '" + AirlineCabinCode + "' FlightDurationAllowedMin='" + FlightDurationAllowedMin + "' FlightDurationAllowedMax='" + FlightDurationAllowedMax + "' FlightMileageAllowedMin='" + FlightMileageAllowedMin + "' FlightMileageAllowedMax='" + FlightMileageAllowedMax + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' ToGlobalFlag='True' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='' ToCodeType='' FromCode='' ToCode=''></tr>")
                                } else {
                                    $(this).html("<td>" + AirlineCabinCode + "</td><td>Custom</td><td>" + FlightDurationAllowedMin + "</td><td>" + FlightDurationAllowedMax + "</td><td>" + FlightMileageAllowedMin + "</td><td>" + FlightMileageAllowedMax + "</td><td>" + PolicyProhibitedFlag + "</td><td>Global to " + ToCode + "</td><td>" + RoutingViceVersaFlag + "</td><td>Modified</td>");
                                    $('#hiddenChangedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' AirlineCabinCode = '" + AirlineCabinCode + "' FlightDurationAllowedMin='" + FlightDurationAllowedMin + "' FlightDurationAllowedMax='" + FlightDurationAllowedMax + "' FlightMileageAllowedMin='" + FlightMileageAllowedMin + "' FlightMileageAllowedMax='" + FlightMileageAllowedMax + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='true' ToGlobalFlag='False' FromCode = '' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='' ToCodeType='" + ToCodeType + "'></tr>")
                                }
                            } else {

                                if (ToGlobalFlag == "Checked") {
                                    $(this).html("<td>" + AirlineCabinCode + "</td><td>Custom</td><td>" + FlightDurationAllowedMin + "</td><td>" + FlightDurationAllowedMax + "</td><td>" + FlightMileageAllowedMin + "</td><td>" + FlightMileageAllowedMax + "</td><td>" + PolicyProhibitedFlag + "</td><td>" + FromCode + " to Global</td><td>" + RoutingViceVersaFlag + "</td><td>Modified</td>");
                                    $('#hiddenChangedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' AirlineCabinCode = '" + AirlineCabinCode + "' FlightDurationAllowedMin='" + FlightDurationAllowedMin + "' FlightDurationAllowedMax='" + FlightDurationAllowedMax + "' FlightMileageAllowedMin='" + FlightMileageAllowedMin + "' FlightMileageAllowedMax='" + FlightMileageAllowedMax + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode = '" + FromCode + "' ToCode='' ToGlobalFlag='True' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType=''></tr>")
                                } else {
                                    $(this).html("<td>" + AirlineCabinCode + "</td><td>Custom</td><td>" + FlightDurationAllowedMin + "</td><td>" + FlightDurationAllowedMax + "</td><td>" + FlightMileageAllowedMin + "</td><td>" + FlightMileageAllowedMax + "</td><td>" + PolicyProhibitedFlag + "</td><td>" + FromCode + " to " + ToCode + "</td><td>" + RoutingViceVersaFlag + "</td><td>Modified</td>");
                                    $('#hiddenChangedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' AirlineCabinCode = '" + AirlineCabinCode + "' FlightDurationAllowedMin='" + FlightDurationAllowedMin + "' FlightDurationAllowedMax='" + FlightDurationAllowedMax + "' FlightMileageAllowedMin='" + FlightMileageAllowedMin + "' FlightMileageAllowedMax='" + FlightMileageAllowedMax + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'></tr>")
                                }
                            }
                            //visual indicator to show it's been changed:
                            $(this).contents('td').css({ 'background-color': '#CCCCCC' });
                        }
                    });
                } else {
                    if (FromGlobalFlag == "Checked") {
                        if (ToGlobalFlag == "Checked") {
                            $('#currentPolicyAirCabinGroupItemsTable tbody').append("<tr><td>" + AirlineCabinCode + "</td><td>Custom</td><td>" + FlightDurationAllowedMin + "</td><td>" + FlightDurationAllowedMax + "</td><td>" + FlightMileageAllowedMin + "</td><td>" + FlightMileageAllowedMax + "</td><td>" + PolicyProhibitedFlag + "</td><td>Global to Global</td><td>" + RoutingViceVersaFlag + "</td><td>Added</td></tr>");
                            $('#hiddenAddedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' AirlineCabinCode = '" + AirlineCabinCode + "' FlightDurationAllowedMin='" + FlightDurationAllowedMin + "' FlightDurationAllowedMax='" + FlightDurationAllowedMax + "' FlightMileageAllowedMin='" + FlightMileageAllowedMin + "' FlightMileageAllowedMax='" + FlightMileageAllowedMax + "' PolicyRoutingName='" + PolicyRoutingName + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' ToGlobalFlag='True' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='' ToCodeType='' FromCode='' ToCode=''></tr>")
                        } else {
                            $('#currentPolicyAirCabinGroupItemsTable tbody').append("<tr><td>" + AirlineCabinCode + "</td><td>Custom</td><td>" + FlightDurationAllowedMin + "</td><td>" + FlightDurationAllowedMax + "</td><td>" + FlightMileageAllowedMin + "</td><td>" + FlightMileageAllowedMax + "</td><td>" + PolicyProhibitedFlag + "</td><td>Global to " + ToCode + "</td><td>" + RoutingViceVersaFlag + "</td><td>Added</td></tr>");
                            $('#hiddenAddedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' AirlineCabinCode = '" + AirlineCabinCode + "' FlightDurationAllowedMin='" + FlightDurationAllowedMin + "' FlightDurationAllowedMax='" + FlightDurationAllowedMax + "' FlightMileageAllowedMin='" + FlightMileageAllowedMin + "' FlightMileageAllowedMax='" + FlightMileageAllowedMax + "' PolicyRoutingName='" + PolicyRoutingName + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='true' ToGlobalFlag='False' FromCode = '' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='' ToCodeType='" + ToCodeType + "'></tr>")
                        }

                    } else {
                        if (ToGlobalFlag == "Checked") {
                            $('#currentPolicyAirCabinGroupItemsTable tbody').append("<tr><td>" + AirlineCabinCode + "</td><td>Custom</td><td>" + FlightDurationAllowedMin + "</td><td>" + FlightDurationAllowedMax + "</td><td>" + FlightMileageAllowedMin + "</td><td>" + FlightMileageAllowedMax + "</td><td>" + PolicyProhibitedFlag + "</td><td>" + FromCode + " to Global</td><td>" + RoutingViceVersaFlag + "</td><td>Added</td></tr>");
                            $('#hiddenAddedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' AirlineCabinCode = '" + AirlineCabinCode + "' FlightDurationAllowedMin='" + FlightDurationAllowedMin + "' FlightDurationAllowedMax='" + FlightDurationAllowedMax + "' FlightMileageAllowedMin='" + FlightMileageAllowedMin + "' FlightMileageAllowedMax='" + FlightMileageAllowedMax + "' PolicyRoutingName='" + PolicyRoutingName + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode = '" + FromCode + "' ToCode='' ToGlobalFlag='True' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType=''></tr>")
                        } else {
                            $('#currentPolicyAirCabinGroupItemsTable tbody').append("<tr><td>" + AirlineCabinCode + "</td><td>Custom</td><td>" + FlightDurationAllowedMin + "</td><td>" + FlightDurationAllowedMax + "</td><td>" + FlightMileageAllowedMin + "</td><td>" + FlightMileageAllowedMax + "</td><td>" + PolicyProhibitedFlag + "</td><td>" + FromCode + " to " + ToCode + "</td><td>" + RoutingViceVersaFlag + "</td><td>Added</td></tr>");
                            $('#hiddenAddedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' AirlineCabinCode = '" + AirlineCabinCode + "' FlightDurationAllowedMin='" + FlightDurationAllowedMin + "' FlightDurationAllowedMax='" + FlightDurationAllowedMax + "' FlightMileageAllowedMin='" + FlightMileageAllowedMin + "' FlightMileageAllowedMax='" + FlightMileageAllowedMax + "' PolicyRoutingName='" + PolicyRoutingName + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'></tr>")
                        }
                    }
                }

                //show PolicyRouting Modal or close
                if (addNewRouting) {
                    AddPolicyAirCabinGroupItemPolicyRouting();
                } else {
                    $("#dialog-confirm").dialog("close");
                }

            } else {
                $("#dialog-confirm").html(result.html);
            }
        },
        error: function () {
            alert("ERR");
            $("#dialog-confirm").html("There was an error.");
        }
    });
}

/*Delete a PolicyAirCabinGroupItem*/
function DeletePolicyAirCabinGroupItemPopup(PolicyAirCabinGroupItemId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");
                //remove from visual table

                $('#currentPolicyAirCabinGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("PolicyAirCabinGroupItemId") == PolicyAirCabinGroupItemId) {
                        $(this).remove();
                    }
                });
                //also add to hidden removed table
                $('#hiddenRemovedPolicyAirCabinGroupItemsTable').append("<tr PolicyAirCabinGroupItemId='" + PolicyAirCabinGroupItemId + "' versionNumber='" + VNBR + "'></tr>")
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Air Cabin Group Item", { zIndex: 900 });
}

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyAirMissedSavingsThresholdGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyAirMissedSavingsThresholdGroupItem Popup
*/
function AddEditPolicyAirMissedSavingsThresholdGroupItem(policyAirMissedSavingsThresholdGroupItemId) {

    if (policyAirMissedSavingsThresholdGroupItemId == "0") {

        $("#dialog-confirm").html("");
        $("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirMissedSavingsThresholdGroupItemPopup?id=' + Math.random() + '&policyAirMissedSavingsThresholdGroupItemId=' + policyAirMissedSavingsThresholdGroupItemId).dialog({
            resizable: true,
            modal: true,
            height: 550,
            width: 600,
            buttons: {
                "Save": function () {
                    PolicyAirMissedSavingsThresholdGroupItemValidation();
                },
                "Close": function () {

                    $("#dialog-confirm").dialog("close");
                }
            }
        });
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Air Missed Savings Threshold Group Item");

    } else {
        $("#dialog-confirm").html("");
        $("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirMissedSavingsThresholdGroupItemPopup?id=' + Math.random() + '&policyAirMissedSavingsThresholdGroupItemId=' + policyAirMissedSavingsThresholdGroupItemId).dialog({
            resizable: true,
            modal: true,
            height: 550,
            width: 600,
            buttons: {
                "Save": function () {
                    PolicyAirMissedSavingsThresholdGroupItemValidation(false);
                },
                "Close": function () {
                    $("#dialog-confirm").dialog("close");
                }
            }
        });
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Air Missed Savings Threshold Group Item");
    }
}


/*Delete a PolicyAirMissedSavingsThresholdGroupItem*/
function DeletePolicyAirMissedSavingsThresholdGroupItemPopup(PolicyAirMissedSavingsThresholdGroupItemId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");
                //remove from visual table

                $('#currentPolicyAirMissedSavingsThresholdGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("PolicyAirMissedSavingsThresholdGroupItemId") == PolicyAirMissedSavingsThresholdGroupItemId) {
                        $(this).remove();
                    }
                });
                //also add to hidden removed table
                $('#hiddenRemovedPolicyAirMissedSavingsThresholdGroupItemsTable').append("<tr PolicyAirMissedSavingsThresholdGroupItemId='" + PolicyAirMissedSavingsThresholdGroupItemId + "' versionNumber='" + VNBR + "'></tr>")
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Air Missed Savings Threshold Group Item");
}

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyAirParameterGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyAirParameterGroupItem Popup
*/
function AddEditPolicyAirParameterGroupItem(policyAirParameterGroupItemId, policyAirParameterTypeId) {

	var typeName = "";
	if (policyAirParameterTypeId == 1) {
		typeName = "Policy Air Time Window Group Item";
	} else if (policyAirParameterTypeId == 2) {
		typeName = "Policy Air Advance Purchase Group Item";
	}

	if (policyAirParameterGroupItemId == '0') {

		$("#dialog-confirm").html("");
		$("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirParameterGroupItemPopup?id=' + Math.random() + '&policyAirParameterGroupItemId=' + policyAirParameterGroupItemId + '&policyAirParameterTypeId=' + policyAirParameterTypeId).dialog({
			resizable: true,
			modal: true,
			height: 550,
			width: 600,
			buttons: {
				"Save": function () {
					PolicyAirParameterGroupItemValidation(false);
				},
				"Close": function () {
					$("#dialog-confirm").dialog("close");
				}
			}
		});
		$('#dialog-confirm').dialog('option', 'title', "Add " + typeName);
	}
	else {
		$("#dialog-confirm").html("");
		$("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirParameterGroupItemPopup?id=' + Math.random() + '&policyAirParameterGroupItemId=' + policyAirParameterGroupItemId + '&policyAirParameterTypeId=' + policyAirParameterTypeId).dialog({
			resizable: true,
			modal: true,
			height: 550,
			width: 600,
			buttons: {
				"Save": function () {
					PolicyAirParameterGroupItemValidation(false);
				},
				"Close": function () {
					$("#dialog-confirm").dialog("close");
				}
			}
		});

		$('#dialog-confirm').dialog('option', 'title', "Edit " + typeName);

	}
}

/*
Delete PolicyAirParameterGroupItem Popup
*/
function DeletePolicyAirParameterGroupItemPopup(policyAirParameterGroupItemId, policyAirParameterTypeId, VNBR) {

	var typeName = "";
	var tableName = "";

	if (policyAirParameterTypeId == 1) {
		tableName = "PolicyAirTimeWindowGroupItem";
		typeName = "Policy Air Time Window Group Item";
	} else if (policyAirParameterTypeId == 2) {
		tableName = "PolicyAirAdvancePurchaseGroupItem";
		typeName = "Policy Air Vendor Group Item";
	}

	$("#dialog-delete").show();
	$("#dialog-delete").dialog({
		resizable: false,
		height: 180,
		modal: true,
		buttons: {
			"Remove": function () {
				$("#dialog-delete").dialog("close");
				//remove from visual table

				$('#current' + tableName + 'sTable tbody tr').each(function () {
					if ($(this).attr("PolicyAirParameterGroupItemId") == policyAirParameterGroupItemId) {
						$(this).remove();
					}
				});
				//also add to hidden removed table
				$('#hiddenRemovedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + policyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + policyAirParameterTypeId + "' versionNumber='" + VNBR + "'></tr>")
			},
			"Cancel": function () {
				$("#dialog-delete").dialog("close");
			}
		}
	});
	$('#dialog-delete').dialog('option', 'title', "Remove " + name, { zIndex: 900 });
}

/*
Validate PolicyAirParameterGroupItem and write to hidden tables or return validation errors
*/
function PolicyAirParameterGroupItemValidation() {


	/////////////////////////////////////////////////////////
	// BEGIN VALIDATION
	// 1. Check Text for Supplier to see if valid
	/////////////////////////////////////////////////////////

	var validForm = true;

	//Reset Errors if filled in
	var PolicyAirParameterGroupItem_PolicyAirParameterValue = escapeInput($("#PolicyAirParameterGroupItem_PolicyAirParameterValue").val());

	if (PolicyAirParameterGroupItem_PolicyAirParameterValue != "") {
		$('#PolicyAirParameterGroupItem_PolicyAirParameterValue_validationMessage').text("").hide();
	}

	//Must be numerical only up to 4 numbers
	var reg = /^\d{0,4}$/;
	if (!reg.test(PolicyAirParameterGroupItem_PolicyAirParameterValue)) {
		$('#PolicyAirParameterGroupItem_PolicyAirParameterValue_validationMessage').addClass("error").text("Please provide a numerical value - maximum of 4 numbers.").show();
		$("#PolicyAirParameterGroupItem_PolicyAirParameterValue").focus();
		return false;
	}

	/////////////////////////////////////////////////////////
	// BEGIN POLICYROUTING VALIDATION
	/////////////////////////////////////////////////////////

	var validFrom = false;
	var validTo = false;

	$("#lblFrom").text("");
	$("#lblTo").text("");

	if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
		validFrom = true;
	}
	if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
		validTo = true;
	}

	//if no "FROM" is set, should it be?
	if (!$("#PolicyRouting_FromGlobalFlag").is(':checked') && escapeInput($("#PolicyRouting_FromCode").val()) == "") {
		$("#lblFrom").removeClass('field-validation-valid');
		$("#lblFrom").addClass('field-validation-error');
		$("#lblFrom").text("Please enter a value or choose Global");
	}

	if (escapeInput($("#PolicyRouting_FromCode").val()) != "") {
		jQuery.ajax({
			type: "POST",
			url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
			data: { fromto: escapeInput($("#PolicyRouting_FromCode").val()), codetype: escapeInput($("#PolicyRouting_FromCodeType").val()) },
			success: function (data) {
				if (!jQuery.isEmptyObject(data)) {
					validFrom = true;
				}
			},
			dataType: "json",
			async: false
		});

		if (!validFrom) {
			$("#lblFrom").removeClass('field-validation-valid');
			$("#lblFrom").addClass('field-validation-error');
			$("#lblFrom").text("This is not a valid entry");
		}
	};

	//if no "TO" is set, should it be?
	if (!$("#PolicyRouting_ToGlobalFlag").is(':checked') && escapeInput($("#PolicyRouting_ToCode").val()) == "") {
		$("#lblTo").removeClass('field-validation-valid');
		$("#lblTo").addClass('field-validation-error');
		$("#lblTo").text("Please enter a value or choose Global");
	}

	if (escapeInput($("#PolicyRouting_ToCode").val()) != "") {
		jQuery.ajax({
			type: "POST",
			url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
			data: { fromto: escapeInput($("#PolicyRouting_ToCode").val()), codetype: escapeInput($("#PolicyRouting_ToCodeType").val()) },
			success: function (data) {
				if (!jQuery.isEmptyObject(data)) {
					validTo = true;
				}

			},
			dataType: "json",
			async: false
		});

		if (!validTo) {
			$("#lblTo").removeClass('field-validation-valid');
			$("#lblTo").addClass('field-validation-error');
			$("#lblTo").text("This is not a valid entry");
		}
	};

	//validation of PolicyRouting Failed
	if (!validFrom || !validTo) {
		return;
	}
	/////////////////////////////////////////////////////////
	// END POLICYROUTING VALIDATION
	/////////////////////////////////////////////////////////

	//reset date fields - on PolicyAirParameterGroupItemPopup only
	if (escapeInput($('#PolicyAirParameterGroupItem_EnabledDate').val()) == "No Enabled Date") {
		escapeInput($('#PolicyAirParameterGroupItem_EnabledDate').val(""));
	}
	if (escapeInput($('#PolicyAirParameterGroupItem_ExpiryDate').val()) == "No Expiry Date") {
		escapeInput($('#PolicyAirParameterGroupItem_ExpiryDate').val(""));
	}
	//reset date fields
	if (escapeInput($('#PolicyAirParameterGroupItem_TravelDateValidFrom').val()) == "No Travel Date Valid From") {
		escapeInput($('#PolicyAirParameterGroupItem_TravelDateValidFrom').val(""));
	}
	if (escapeInput($('#PolicyAirParameterGroupItem_TravelDateValidTo').val()) == "No Travel Date Valid To") {
		escapeInput($('#PolicyAirParameterGroupItem_TravelDateValidTo').val(""));
	}
	var url = '/ClientWizard.mvc/PolicyAirParameterGroupItemValidation';

	//Build Object to Store PolicyAirParameterGroupItem
	var policyAirParameterGroupItem = {
		PolicyAirParameterGroupItemId: escapeInput($("#PolicyAirParameterGroupItem_PolicyAirParameterGroupItemId").val()),
		PolicyAirParameterValue: escapeInput($("#PolicyAirParameterGroupItem_PolicyAirParameterValue").val()),
		PolicyAirParameterTypeId: escapeInput($("#PolicyAirParameterGroupItem_PolicyAirParameterTypeId").val()),
		PolicyGroupId: escapeInput($("#PolicyAirParameterGroupItem_PolicyGroupId").val()),
		EnabledFlag: $("#PolicyAirParameterGroupItem_EnabledFlag").is(':checked'),
		EnabledDate:escapeInput( $("#PolicyAirParameterGroupItem_EnabledDate").val()),
		ExpiryDate: escapeInput($("#PolicyAirParameterGroupItem_ExpiryDate").val()),
		PolicyRoutingId: escapeInput($("#PolicyRouting_PolicyRoutingId").val()),
		TravelDateValidFrom: escapeInput($("#PolicyAirParameterGroupItem_TravelDateValidFrom").val()),
		TravelDateValidTo: escapeInput($("#PolicyAirParameterGroupItem_TravelDateValidTo").val()),
		VersionNumber: escapeInput($("#PolicyAirParameterGroupItem_VersionNumber").val())
	};

	//Build Object to Store PolicyRouting
	var policyRouting = {
		Name: escapeInput($("#PolicyRouting_Name").val()),
		FromGlobalFlag: $("#PolicyRouting_FromGlobalFlag").is(':checked'),
		FromCode: escapeInput($("#PolicyRouting_FromCode").val()),
		FromCodeType: escapeInput($("#PolicyRouting_FromCodeType").val()),
		ToGlobalFlag: $("#PolicyRouting_ToGlobalFlag").is(':checked'),
		ToCode: escapeInput($("#PolicyRouting_ToCode").val()),
		ToCodeType: escapeInput($("#PolicyRouting_ToCodeType").val()),
		RoutingViceVersaFlag: $("#PolicyRouting_RoutingViceVersaFlag").is(':checked')
	};

	//Build Object to Store Client Policy
	var clientPolicy = {
		PolicyAirParameterGroupItem: $.parseJSON(JSON.stringify(policyAirParameterGroupItem)),
		PolicyRouting: $.parseJSON(JSON.stringify(policyRouting))
	}

	//AJAX (JSON) POST of Client Object
	//$("#dialog-confirm").html("");

	$.ajax({
		type: "POST",
		data: JSON.stringify(clientPolicy),
		url: url,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (result) {

			if (result.success) {

				//do stuff here to save
				var PolicyAirParameterGroupItemId = escapeInput($("#PolicyAirParameterGroupItem_PolicyAirParameterGroupItemId").val());
				var PolicyAirParameterTypeId = escapeInput($("#PolicyAirParameterGroupItem_PolicyAirParameterTypeId").val());
				var PolicyAirParameterValue = escapeInput($("#PolicyAirParameterGroupItem_PolicyAirParameterValue").val());
				var PolicyGroupId = escapeInput($("#PolicyAirParameterGroupItem_PolicyGroupId").val());
				var EnabledFlagObj = $("#PolicyAirParameterGroupItem_EnabledFlag");
				var EnabledFlag = (EnabledFlagObj.is(':checkbox')) ? EnabledFlagObj.is(':checked') : escapeInput(EnabledFlagObj.val());
				var EnabledDate = escapeInput($("#PolicyAirParameterGroupItem_EnabledDate").val());
				var ExpiryDate = escapeInput($("#PolicyAirParameterGroupItem_ExpiryDate").val());
				var PolicyRoutingId = escapeInput($("#PolicyAirParameterGroupItem_PolicyRoutingId").val());
				var ProductId = escapeInput($("#PolicyAirParameterGroupItem_ProductId").val());
				var TravelDateValidFrom = escapeInput($("#PolicyAirParameterGroupItem_TravelDateValidFrom").val());
				var TravelDateValidTo = escapeInput($("#PolicyAirParameterGroupItem_TravelDateValidTo").val());
				var VersionNumber = escapeInput($("#PolicyAirParameterGroupItem_VersionNumber").val());

				var PolicyRoutingName = escapeInput($("#PolicyRouting_Name").val());
				var FromGlobalFlag = $("#PolicyRouting_FromGlobalFlag").is(':checked');
				var FromCode = escapeInput($("#PolicyRouting_FromCode").val());
				var FromCodeType = escapeInput($("#PolicyRouting_FromCodeType").val());
				var ToGlobalFlag = $("#PolicyRouting_ToGlobalFlag").is(':checked');
				var ToCode = escapeInput($("#PolicyRouting_ToCode").val());
				var ToCodeType = escapeInput($("#PolicyRouting_ToCodeType").val());
				var RoutingViceVersaFlag = $("#PolicyRouting_RoutingViceVersaFlag").is(':checked');
				var PolicyRoutingId = escapeInput($("#PolicyRouting_PolicyRoutingId").val());

				var AddEditType = "";
				if (PolicyAirParameterGroupItemId == "0") {
					AddEditType = "New";
				} else {
					AddEditType = "Current";
				}

				var FromGlobalFlag = "";
				if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
					FromGlobalFlag = "Checked";
				} else {
					FromGlobalFlag = "UnChecked";
				}

				var ToGlobalFlag = "";
				if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
					ToGlobalFlag = "Checked";
				} else {
					ToGlobalFlag = "UnChecked";
				}

                var FromCode = escapeInput($("#PolicyRouting_FromCode").val());
				var FromCodeType = escapeInput($("#PolicyRouting_FromCodeType").val());
				var ToCode = escapeInput($("#PolicyRouting_ToCode").val());
				var ToCodeType = escapeInput($("#PolicyRouting_ToCodeType").val());

				var RoutingViceVersaFlag = "";
				if ($("#PolicyRouting_RoutingViceVersaFlag").is(':checked')) {
					RoutingViceVersaFlag = "True"
				} else {
					RoutingViceVersaFlag = "False"
				}

				var tableName = "";
				if (PolicyAirParameterTypeId == 1) {
					tableName = "PolicyAirTimeWindowGroupItem";
				} else if (PolicyAirParameterTypeId == 2) {
					tableName = "PolicyAirAdvancePurchaseGroupItem";
				}

				if (AddEditType == "Current") {
					$('#current' + tableName + 'sTable tbody tr').each(function () {
						if ($(this).attr("PolicyAirParameterGroupItemId") == PolicyAirParameterGroupItemId) {
							$(this).html("");
							if (FromGlobalFlag == "Checked") {
								if (ToGlobalFlag == "Checked") {
									$(this).html("<td>" + PolicyAirParameterValue + "</td><td>Custom</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>Global to Global</td><td>Modified</td>");
									$('#hiddenChangedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + PolicyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + PolicyAirParameterTypeId + "' PolicyAirParameterValue='" + PolicyAirParameterValue + "' PolicyGroupId='" + PolicyGroupId + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' FromCode='" + FromCode + "' ToGlobalFlag='True' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
								} else {
									$(this).html("<td>" + PolicyAirParameterValue + "</td><td>Custom</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>Global to " + ToCode + "</td><td>Modified</td>");
									$('#hiddenChangedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + PolicyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + PolicyAirParameterTypeId + "' PolicyAirParameterValue='" + PolicyAirParameterValue + "' PolicyGroupId='" + PolicyGroupId + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
								}
							} else {
								if (ToGlobalFlag == "Checked") {
									$(this).html("<td>" + PolicyAirParameterValue + "</td><td>Custom</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>" + FromCode + " to Global</td><td>Modified</td>");
									$('#hiddenChangedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + PolicyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + PolicyAirParameterTypeId + "' PolicyAirParameterValue='" + PolicyAirParameterValue + "' PolicyGroupId='" + PolicyGroupId + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='True' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
								} else {
									$(this).html("<td>" + PolicyAirParameterValue + "</td><td>Custom</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>" + FromCode + " to " + ToCode + "</td><td>Modified</td>");
									$('#hiddenChangedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + PolicyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + PolicyAirParameterTypeId + "' PolicyAirParameterValue='" + PolicyAirParameterValue + "' PolicyGroupId='" + PolicyGroupId + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
								}
							}
							$(this).contents('td').css({ 'background-color': '#CCCCCC' });
						}
					});
				} else {

					//addedittype=New
					var newLine = "";
					if (FromGlobalFlag == "Checked") {
						if (ToGlobalFlag == "Checked") {
							newLine = "<tr><td>" + PolicyAirParameterValue + "</td><td>Custom</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>Global to Global</td><td>Added</td></tr>";
							$('#hiddenAddedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + PolicyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + PolicyAirParameterTypeId + "' PolicyAirParameterValue='" + PolicyAirParameterValue + "' PolicyGroupId='" + PolicyGroupId + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' FromCode='" + FromCode + "' ToGlobalFlag='True' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
						} else {
							newLine = "<tr><td>" + PolicyAirParameterValue + "</td><td>Custom</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>Global to " + ToCode + "</td><td>Added</td></tr>";
							$('#hiddenAddedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + PolicyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + PolicyAirParameterTypeId + "' PolicyAirParameterValue='" + PolicyAirParameterValue + "' PolicyGroupId='" + PolicyGroupId + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
						}
					} else {

						if (ToGlobalFlag == "Checked") {
							newLine = "<tr><td>" + PolicyAirParameterValue + "</td><td>Custom</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>" + FromCode + " to Global</td><td>Added</td></tr>";
							$('#hiddenAddedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + PolicyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + PolicyAirParameterTypeId + "' PolicyAirParameterValue='" + PolicyAirParameterValue + "' PolicyGroupId='" + PolicyGroupId + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='True' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
						} else {
							newLine = "<tr><td>" + PolicyAirParameterValue + "</td><td>Custom</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>" + FromCode + " to " + ToCode + "</td><td>Added</td></tr>";
							$('#hiddenAddedPolicyAirParameterGroupItemsTable').append("<tr PolicyAirParameterGroupItemId='" + PolicyAirParameterGroupItemId + "' PolicyAirParameterTypeId='" + PolicyAirParameterTypeId + "' PolicyAirParameterValue='" + PolicyAirParameterValue + "' PolicyGroupId='" + PolicyGroupId + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
						}
					}
					$('#current' + tableName + 'sTable').append(newLine);
				}

				//close modal
				$("#dialog-confirm").dialog("close");

			} else {
				$("#dialog-confirm").html(result.html);
			}
		},
		error: function () {
			alert("ERR");
			$("#dialog-confirm").html("There was an error.");
		}
	});
}

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyAirVendorGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////

/*
Edit PolicyAirVendorGroupItem Popup
*/
function AddEditPolicyAirVendorGroupItem(policyAirVendorGroupItemId) {

    if (policyAirVendorGroupItemId == '0') {

	    $("#dialog-confirm").html("");
	    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirVendorGroupItemPopup?id=' + Math.random() + '&policyAirVendorGroupItemId=' + policyAirVendorGroupItemId).dialog({
            resizable: true,
            modal: true,
            height: 550,
            width: 600,
            buttons: {
                "Save": function () {
                    PolicyAirVendorGroupItemValidation(false);
                },
                "Save and Add Another Routing": function () {
                    PolicyAirVendorGroupItemValidation(true);
                },
                "Close": function () {
                    $("#dialog-confirm").dialog("close");
                }
            }
        });
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Air Vendor Group Item");
	}
	else {
	    $("#dialog-confirm").html("");
	    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirVendorGroupItemPopup?id=' + Math.random() + '&policyAirVendorGroupItemId=' + policyAirVendorGroupItemId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyAirVendorGroupItemValidation(false);
            },
            "Close": function () {               
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    $('#dialog-confirm').dialog('option', 'title', "Edit Policy Air Vendor Group Item");

	}
}

/*
Add Extra PolicyAirVendorGroupItem PolicyRouting Popup
*/
function AddPolicyAirVendorGroupItemPolicyRouting() {


    if ($("#PolicyAirVendorGroupItem_EnabledFlag").attr('type') == "checkbox") {
        var enabledFlag = $("#PolicyAirVendorGroupItem_EnabledFlag").is(':checked');
    } else {
    	var enabledFlag = escapeInput($("#PolicyAirVendorGroupItem_EnabledFlag").val());
    }
    //Build Object to Store PolicyAirVendorGroupItem
    var policyAirVendorGroupItem = {
		PolicyAirVendorGroupItemId: escapeInput($("#PolicyAirVendorGroupItem_PolicyAirVendorGroupItemId").val()),
		PolicyGroupId: escapeInput($("#PolicyAirVendorGroupItem_PolicyGroupId").val()),
		PolicyAirStatusId: escapeInput($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val()),
        EnabledFlag: enabledFlag,
		EnabledDate: escapeInput($("#PolicyAirVendorGroupItem_EnabledDate").val()),
		ExpiryDate: escapeInput($("#PolicyAirVendorGroupItem_ExpiryDate").val()),
		PolicyRoutingId: escapeInput($("#PolicyRouting_PolicyRoutingId").val()),
		TravelDateValidFrom:escapeInput( $("#PolicyAirVendorGroupItem_TravelDateValidFrom").val()),
		SupplierName: escapeInput($("#PolicyAirVendorGroupItem_SupplierName").val()),
		SupplierCode: escapeInput($("#PolicyAirVendorGroupItem_SupplierCode").val()),
		ProductName: escapeInput($("#PolicyAirVendorGroupItem_ProductName").val()),
		ProductId: escapeInput($("#PolicyAirVendorGroupItem_ProductId").val()),
		AirVendorRanking: escapeInput($("#PolicyAirVendorGroupItem_AirVendorRanking").val()),
		VersionNumber: escapeInput($("#PolicyAirVendorGroupItem_VersionNumber").val())

    };

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyAirVendorGroupItemPolicyRoutingPopup?id=' + Math.random(), policyAirVendorGroupItem).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyAirVendorGroupItemValidation(false);
            },
            "Save and Add Another Routing": function () {
                PolicyAirVendorGroupItemValidation(true);
            },
            "Close": function () {
                
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    $('#dialog-confirm').dialog('option', 'title', "Add Policy Air Vendor Group Item");
}

/*
Delete PolicyAirVendorGroupItem Popup
*/
function DeletePolicyAirVendorGroupItemPopup(PolicyAirVendorGroupItemId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");
                //remove from visual table

                $('#currentPolicyAirVendorGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("PolicyAirVendorGroupItemId") == PolicyAirVendorGroupItemId) {
                        $(this).remove();
                    }
                });
                //also add to hidden removed table
                $('#hiddenRemovedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' versionNumber='" + VNBR + "'></tr>")
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Air Vendor Group Item", { zIndex: 900 });
}

/*
Validate PolicyAirVendorGroupItem and write to hidden tables or return validation errors
*/
function PolicyAirVendorGroupItemValidation(addNewRouting) {


    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER/AIRVENDORRANKING VALIDATION
    // 1. Check Text for Supplier to see if valid
    // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
    // 3. AirVendorRanking is mandatory if Status=Preferred
    /////////////////////////////////////////////////////////
    var validSupplier = false;
    var validSupplierProduct = false;
    var validAirVendorRanking = false;
    
    if (escapeInput($("#PolicyAirVendorGroupItem_SupplierName").val()) != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierProductName",
            data: { supplierName: escapeInput($("#PolicyAirVendorGroupItem_SupplierName").val()), productId: escapeInput($("#PolicyAirVendorGroupItem_ProductId").val()) },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validSupplier = true;

                    //user may not use auto, need to populate ID field
                    if (escapeInput($("#SupplierCode").val()) == "") {
                    	$("#PolicyAirVendorGroupItem_SupplierCode").val(data[0].SupplierCode);
                    }
                }
            },
            dataType: "json",
            async: false
        });
        if (!validSupplier) {
            $("#PolicyAirVendorGroupItem_SupplierCode").val("");
            $("#PolicyAirVendorGroupItem_SupplierName_validationMessage").html("");
            $("#lblSupplierNameMsg").removeClass('field-validation-valid');
            $("#lblSupplierNameMsg").addClass('field-validation-error');
            $("#lblSupplierNameMsg").text("This is not a valid Supplier");
        } else {
            $("#lblSupplierNameMsg").text("");
        }
        
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierProduct",
            data: { supplierCode: escapeInput($("#PolicyAirVendorGroupItem_SupplierCode").val()), productId: escapeInput($("#PolicyAirVendorGroupItem_ProductId").val()) },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validSupplierProduct = true;
                }
            },
            dataType: "json",
            async: false
        });
       
    }

    if (!validSupplierProduct) {
        $("#PolicyAirVendorGroupItem_SupplierCode").val("");
        $("#PolicyAirVendorGroupItem_SupplierName_validationMessage").html("");
        $("#lblSupplierNameMsg").removeClass('field-validation-valid');
        $("#lblSupplierNameMsg").addClass('field-validation-error');
        $("#lblSupplierNameMsg").text("This is not a valid Supplier");
    } else {
        $("#lblSupplierNameMsg").text("");
    }

    if (escapeInput($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val()) != "1") {
        $("#PolicyAirVendorGroupItem_AirVendorRanking").val("");
        $("#PolicyAirVendorGroupItem_AirVendorRanking").attr("disabled", true);
        validAirVendorRanking = true;
    } else {
    	if (!escapeInput($("#PolicyAirVendorGroupItem_AirVendorRanking").val()) == "") {
            validAirVendorRanking = true;
        }
    }

    if (!validAirVendorRanking) {
        $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").removeClass('field-validation-valid');
        $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").addClass('field-validation-error');
        $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").text("Ranking Required for Preferred Items");
    } else {
        $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").text("");
    }
    if (!validSupplierProduct || !validAirVendorRanking) {
        return false;
    }

    if (!validSupplierProduct || !validAirVendorRanking) {
        return false;
    }
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER VALIDATION
    /////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////
    // BEGIN POLICYROUTING VALIDATION
    /////////////////////////////////////////////////////////
    var validFrom = false;
    var validTo = false;
    $("#lblFrom").text("");
    $("#lblTo").text("");

    if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
        validFrom = true;
    }
    if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
        validTo = true;
    }

    //if no "FROM" is set, should it be?
    if (!$("#PolicyRouting_FromGlobalFlag").is(':checked') && escapeInput($("#PolicyRouting_FromCode").val()) == "") {
        $("#lblFrom").removeClass('field-validation-valid');
        $("#lblFrom").addClass('field-validation-error');
        $("#lblFrom").text("Please enter a value or choose Global");
    }

    if (escapeInput($("#PolicyRouting_FromCode").val()) != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
            data: { fromto: escapeInput($("#PolicyRouting_FromCode").val()), codetype: escapeInput($("#PolicyRouting_FromCodeType").val()) },
            success: function (data) {
                if (!jQuery.isEmptyObject(data)) {
                    validFrom = true;
                }
            },
            dataType: "json",
            async: false
        });

        if (!validFrom) {
            $("#lblFrom").removeClass('field-validation-valid');
            $("#lblFrom").addClass('field-validation-error');
            $("#lblFrom").text("This is not a valid entry");
        }
    };

    //if no "TO" is set, should it be?
    if (!$("#PolicyRouting_ToGlobalFlag").is(':checked') && escapeInput($("#PolicyRouting_ToCode").val()) == "") {
        $("#lblTo").removeClass('field-validation-valid');
        $("#lblTo").addClass('field-validation-error');
        $("#lblTo").text("Please enter a value or choose Global");
    }

    if (escapeInput($("#PolicyRouting_ToCode").val()) != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
            data: { fromto: escapeInput($("#PolicyRouting_ToCode").val()), codetype: escapeInput($("#PolicyRouting_ToCodeType").val()) },
            success: function (data) {
                if (!jQuery.isEmptyObject(data)) {
                    validTo = true;
                }

            },
            dataType: "json",
            async: false
        });

        if (!validTo) {
            $("#lblTo").removeClass('field-validation-valid');
            $("#lblTo").addClass('field-validation-error');
            $("#lblTo").text("This is not a valid entry");
        }
    };

    //validation of PolicyRouting Failed
    if (!validFrom || !validTo) {
        return;
    }
    /////////////////////////////////////////////////////////
    // END POLICYROUTING VALIDATION
    /////////////////////////////////////////////////////////

    //reset date fields - on PolicyAirVendorGroupItemPopup only
    if (escapeInput($('#PolicyAirVendorGroupItem_EnabledDate').val()) == "No Enabled Date") {
        $('#PolicyAirVendorGroupItem_EnabledDate').val("");
    }
    if (escapeInput($('#PolicyAirVendorGroupItem_ExpiryDate').val()) == "No Expiry Date") {
        $('#PolicyAirVendorGroupItem_ExpiryDate').val("");
    }
    //reset date fields
    if (escapeInput($('#PolicyAirVendorGroupItem_TravelDateValidFrom').val()) == "No Travel Date Valid From") {
        $('#PolicyAirVendorGroupItem_TravelDateValidFrom').val("");
    }
    if (escapeInput($('#PolicyAirVendorGroupItem_TravelDateValidTo').val()) == "No Travel Date Valid To") {
        $('#PolicyAirVendorGroupItem_TravelDateValidTo').val("");
    }
    var url = '/ClientWizard.mvc/PolicyAirVendorGroupItemValidation';

    //Build Object to Store PolicyAirVendorGroupItem
    var policyAirVendorGroupItem = {
    	PolicyAirVendorGroupItemId: escapeInput($("#PolicyAirVendorGroupItem_PolicyAirVendorGroupItemId").val()),
    	PolicyAirStatusId: escapeInput($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val()),
    	PolicyGroupId: escapeInput($("#PolicyAirVendorGroupItem_PolicyGroupId").val()),
        EnabledFlag: $("#PolicyAirVendorGroupItem_EnabledFlag").is(':checked'),
    	EnabledDate: escapeInput($("#PolicyAirVendorGroupItem_EnabledDate").val()),
    	ExpiryDate: escapeInput($("#PolicyAirVendorGroupItem_ExpiryDate").val()),
    	PolicyRoutingId: escapeInput($("#PolicyRouting_PolicyRoutingId").val()),
    	ProductName: escapeInput($("#PolicyAirVendorGroupItem_ProductName").val()),
    	ProductId: escapeInput($("#PolicyAirVendorGroupItem_ProductId").val()),
    	SupplierCode: escapeInput($("#PolicyAirVendorGroupItem_SupplierCode").val()),
    	SupplierName: escapeInput($("#PolicyAirVendorGroupItem_SupplierName").val()),
    	AirVendorRanking: escapeInput($("#PolicyAirVendorGroupItem_AirVendorRanking").val()),
    	TravelDateValidFrom: escapeInput($("#PolicyAirVendorGroupItem_TravelDateValidFrom").val()),
    	TravelDateValidTo: escapeInput($("#PolicyAirVendorGroupItem_TravelDateValidTo").val()),
    	VersionNumber: escapeInput($("#PolicyAirVendorGroupItem_VersionNumber").val())
    };

    //Build Object to Store PolicyRouting
    var policyRouting = {
    	Name: escapeInput($("#PolicyRouting_Name").val()),
        FromGlobalFlag: $("#PolicyRouting_FromGlobalFlag").is(':checked'),
        FromCode: escapeInput($("#PolicyRouting_FromCode").val()),
        FromCodeType: escapeInput($("#PolicyRouting_FromCodeType").val()),
        ToGlobalFlag: $("#PolicyRouting_ToGlobalFlag").is(':checked'),
        ToCode: escapeInput($("#PolicyRouting_ToCode").val()),
        ToCodeType: escapeInput($("#PolicyRouting_ToCodeType").val()),
        RoutingViceVersaFlag: $("#PolicyRouting_RoutingViceVersaFlag").is(':checked')
    };

    //Build Object to Store Client Policy
    var clientPolicy = {
        PolicyAirVendorGroupItem: $.parseJSON(JSON.stringify(policyAirVendorGroupItem)),
        PolicyRouting: $.parseJSON(JSON.stringify(policyRouting))
    }

    //AJAX (JSON) POST of Client Object
    //$("#dialog-confirm").html("");
    $.ajax({
        type: "POST",
        data: JSON.stringify(clientPolicy),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            if (result.success) {
                //do stuff here to save
            	var PolicyAirVendorGroupItemId = escapeInput($("#PolicyAirVendorGroupItem_PolicyAirVendorGroupItemId").val());
            	var PolicyAirStatusId = escapeInput($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val());
            	var PolicyStatus = $("#PolicyAirVendorGroupItem_PolicyAirStatusId option[value='" + PolicyAirStatusId + "']").text();
            	if (PolicyStatus == '') { PolicyStatus = escapeInput($('#PolicyAirVendorGroupItem_PolicyAirStatus').val()); }

            	var PolicyGroupId = escapeInput($("#PolicyAirVendorGroupItem_PolicyGroupId").val());
            	var EnabledFlagObj = $("#PolicyAirVendorGroupItem_EnabledFlag");
                var EnabledFlag = (EnabledFlagObj.is(':checkbox')) ? EnabledFlagObj.is(':checked') : EnabledFlagObj.val();
                var EnabledDate = escapeInput($("#PolicyAirVendorGroupItem_EnabledDate").val());
                var ExpiryDate = escapeInput($("#PolicyAirVendorGroupItem_ExpiryDate").val());
                var PolicyRoutingId = escapeInput($("#PolicyAirVendorGroupItem_PolicyRoutingId").val());
                var ProductId = escapeInput($("#PolicyAirVendorGroupItem_ProductId").val());
                var SupplierCode = escapeInput($("#PolicyAirVendorGroupItem_SupplierCode").val());
                var SupplierName = escapeInput($("#PolicyAirVendorGroupItem_SupplierName").val());
                var AirVendorRanking = escapeInput($("#PolicyAirVendorGroupItem_AirVendorRanking").val());
                var TravelDateValidFrom = escapeInput($("#PolicyAirVendorGroupItem_TravelDateValidFrom").val());
                var TravelDateValidTo = escapeInput($("#PolicyAirVendorGroupItem_TravelDateValidTo").val());
                var VersionNumber = escapeInput($("#PolicyAirVendorGroupItem_VersionNumber").val());

                var PolicyRoutingName = escapeInput($("#PolicyRouting_Name").val());
                var FromGlobalFlag = $("#PolicyRouting_FromGlobalFlag").is(':checked');
                var FromCode = escapeInput($("#PolicyRouting_FromCode").val());
                var FromCodeType = escapeInput($("#PolicyRouting_FromCodeType").val());
                var ToGlobalFlag = $("#PolicyRouting_ToGlobalFlag").is(':checked');
                var ToCode = escapeInput($("#PolicyRouting_ToCode").val());
                var ToCodeType = escapeInput($("#PolicyRouting_ToCodeType").val());
                var RoutingViceVersaFlag = $("#PolicyRouting_RoutingViceVersaFlag").is(':checked');
                var PolicyRoutingId = escapeInput($("#PolicyRouting_PolicyRoutingId").val());
                //var PolicyRoutingName;
                var VendorName = escapeInput($('#PolicyAirVendorGroupItem_SupplierName').val());
                var ProductId = escapeInput($('#PolicyAirVendorGroupItem_ProductId').val());
                var ProductTypeName = 'Air';

                var AddEditType = "";
                if (PolicyAirVendorGroupItemId == "0") {
                    AddEditType = "New";
                } else {
                    AddEditType = "Current";
                }

                var FromGlobalFlag = "";
                if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                    FromGlobalFlag = "Checked";
                } else {
                    FromGlobalFlag = "UnChecked";
                }


                var ToGlobalFlag = "";
                if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                    ToGlobalFlag = "Checked";
                } else {
                    ToGlobalFlag = "UnChecked";
                }

                var FromCode = escapeInput($("#PolicyRouting_FromCode").val());
                var FromCodeType = escapeInput($("#PolicyRouting_FromCodeType").val());
                var ToCode = escapeInput($("#PolicyRouting_ToCode").val());
                var ToCodeType = escapeInput($("#PolicyRouting_ToCodeType").val());

                var RoutingViceVersaFlag = "";
                if ($("#PolicyRouting_RoutingViceVersaFlag").is(':checked')) {
                    RoutingViceVersaFlag = "True"
                } else {
                    RoutingViceVersaFlag = "False"
                }

                if (AddEditType == "Current") {
                    $('#currentPolicyAirVendorGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("PolicyAirVendorGroupItemId") == PolicyAirVendorGroupItemId) {
                            $(this).html("");
                            if (FromGlobalFlag == "Checked") {
                                if (ToGlobalFlag == "Checked") {
                                    $(this).html("<td>" + VendorName + "</td><td>Custom</td><td>" + PolicyStatus + "</td><td>" + AirVendorRanking + "</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>Global to Global</td><td>Modified</td>");
                                    $('#hiddenChangedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' PolicyAirStatusId='" + PolicyAirStatusId + "' AirVendorRanking='" + AirVendorRanking + "' ProductId='" + ProductId + "' SupplierCode='" + SupplierCode + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' FromCode='" + FromCode + "' ToGlobalFlag='True' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
                                } else {
                                    $(this).html("<td>" + VendorName + "</td><td>Custom</td><td>" + PolicyStatus + "</td><td>" + AirVendorRanking + "</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>Global to " + ToCode + "</td><td>Modified</td>");
                                    $('#hiddenChangedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' PolicyAirStatusId='" + PolicyAirStatusId + "' AirVendorRanking='" + AirVendorRanking + "' ProductId='" + ProductId + "' SupplierCode='" + SupplierCode + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
                                }
                            } else {
                                if (ToGlobalFlag == "Checked") {
                                    $(this).html("<td>" + VendorName + "</td><td>Custom</td><td>" + PolicyStatus + "</td><td>" + AirVendorRanking + "</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>" + FromCode + " to Global</td><td>Modified</td>");
                                    $('#hiddenChangedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' PolicyAirStatusId='" + PolicyAirStatusId + "' AirVendorRanking='" + AirVendorRanking + "' ProductId='" + ProductId + "' SupplierCode='" + SupplierCode + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='True' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
                                } else {
                                    $(this).html("<td>" + VendorName + "</td><td>Custom</td><td>" + PolicyStatus + "</td><td>" + AirVendorRanking + "</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>" + FromCode + " to " + ToCode + "</td><td>Modified</td>");
                                    $('#hiddenChangedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' PolicyAirStatusId='" + PolicyAirStatusId + "' AirVendorRanking='" + AirVendorRanking + "' ProductId='" + ProductId + "' SupplierCode='" + SupplierCode + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
                                }
                            }
                            $(this).contents('td').css({ 'background-color': '#CCCCCC' });
                        }
                    });
                } else {

                    //addedittype=New
                    var newLine = "";
                    if (FromGlobalFlag == "Checked") {
                        if (ToGlobalFlag == "Checked") {
                            newLine = "<tr><td>" + VendorName + "</td><td>Custom</td><td>" + PolicyStatus + "</td><td>" + AirVendorRanking + "</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>Global to Global</td><td>Added</td></tr>";
                            $('#hiddenAddedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' PolicyAirStatusId='" + PolicyAirStatusId + "' AirVendorRanking='" + AirVendorRanking + "' ProductId='" + ProductId + "' SupplierCode='" + SupplierCode + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' FromCode='" + FromCode + "' ToGlobalFlag='True' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
                        } else {
                            newLine = "<tr><td>" + VendorName + "</td><td>Custom</td><td>" + PolicyStatus + "</td><td>" + AirVendorRanking + "</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>Global to " + ToCode + "</td><td>Added</td></tr>";
                            $('#hiddenAddedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' PolicyAirStatusId='" + PolicyAirStatusId + "' AirVendorRanking='" + AirVendorRanking + "' ProductId='" + ProductId + "' SupplierCode='" + SupplierCode + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='True' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
                        }
                    } else {
                        if (ToGlobalFlag == "Checked") {
                            newLine = "<tr><td>" + VendorName + "</td><td>Custom</td><td>" + PolicyStatus + "</td><td>" + AirVendorRanking + "</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>" + FromCode + " to Global</td><td>Added</td></tr>";
                            $('#hiddenAddedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' PolicyAirStatusId='" + PolicyAirStatusId + "' AirVendorRanking='" + AirVendorRanking + "' ProductId='" + ProductId + "' SupplierCode='" + SupplierCode + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='True' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
                        } else {
                            newLine = "<tr><td>" + VendorName + "</td><td>Custom</td><td>" + PolicyStatus + "</td><td>" + AirVendorRanking + "</td><td>" + TravelDateValidFrom + "</td><td>" + TravelDateValidTo + "</td><td>" + FromCode + " to " + ToCode + "</td><td>Added</td></tr>";
                            $('#hiddenAddedPolicyAirVendorGroupItemsTable').append("<tr PolicyAirVendorGroupItemId='" + PolicyAirVendorGroupItemId + "' PolicyGroupId='" + PolicyGroupId + "' PolicyAirStatusId='" + PolicyAirStatusId + "' AirVendorRanking='" + AirVendorRanking + "' ProductId='" + ProductId + "' SupplierCode='" + SupplierCode + "' PolicyRoutingId = '" + PolicyRoutingId + "' PolicyRoutingName='" + PolicyRoutingName + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + EnabledDate + "' ExpiryDate='" + ExpiryDate + "' TravelDateValidFrom='" + TravelDateValidFrom + "' TravelDateValidTo='" + TravelDateValidTo + "' VersionNumber='" + VersionNumber + "' FromGlobalFlag='False' FromCode='" + FromCode + "' ToGlobalFlag='False' ToCode='" + ToCode + "' RoutingViceVersaFlag='" + RoutingViceVersaFlag + "' FromCodeType='" + FromCodeType + "' ToCodeType='" + ToCodeType + "'> </tr>");
                        }
                    }
                    $('#currentPolicyAirVendorGroupItemsTable').append(newLine);
                }

                //show PolicyRouting Modal or close
                if (addNewRouting) {
                    AddPolicyAirVendorGroupItemPolicyRouting();
                } else {
                    $("#dialog-confirm").dialog("close");
                }

            } else {
                $("#dialog-confirm").html(result.html);
            }
        },
        error: function () {
            alert("ERR");
            $("#dialog-confirm").html("There was an error.");
        }
    });
}

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyCarTypeGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////

/*
Validate PolicyCarTypeGroupItem and write to hidden tables or return validation errors
THIS IS MOVED TO THE POPUP
*/

/*
Edit PolicyCarTypeGroupItem Popup
*/
function AddEditPolicyCarTypeGroupItem(policyCarTypeGroupItemId) {

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyCarTypeGroupItemPopup?id=' + Math.random() + '&policyCarTypeGroupItemId=' + policyCarTypeGroupItemId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyCarTypeGroupItemValidation();
            },
            "Close": function () {               
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policyCarTypeGroupItemId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Car Type Group Item");
    }else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Car Type Group Item");
    }
}

/*Delete PolicyCarTypeGroupItem*/
function DeletePolicyCarTypeGroupItemPopup(PolicyCarTypeGroupItemId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");

                //remove from visual table
                $('#currentPolicyCarTypeGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("PolicyCarTypeGroupItemId") == PolicyCarTypeGroupItemId) {
                        $(this).remove();
                    }
                });

                //also add to hidden removed table
                $('#hiddenRemovedPolicyCarTypeGroupItemsTable').append("<tr PolicyCarTypeGroupItemId='" + PolicyCarTypeGroupItemId + "' versionNumber='" + VNBR + "'></tr>")


            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Car Type Group Item", { zIndex: 900 });


}



/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyCarVendorGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyCarVendorGroupItem Popup
*/
function AddEditPolicyCarVendorGroupItem(policyCarVendorGroupItemId) {

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyCarVendorGroupItemPopup?id=' + Math.random() + '&policyCarVendorGroupItemId=' + policyCarVendorGroupItemId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyCarVendorGroupItemValidation();
            },
            "Close": function () {
                
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policyCarVendorGroupItemId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Car Vendor Group Item");
    } else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Car Vendor Group Item");
    }
}

/*
Validate PolicyCarVendorGroupItem and write to hidden tables or return validation errors
MOVED TO POPUP
*/

function DeletePolicyCarVendorGroupItemPopup(PolicyCarVendorGroupItemId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");
                //remove from visual table

                $('#currentPolicyCarVendorGroupItemsTable tbody tr').each(function () {

                    if ($(this).attr("PolicyCarVendorGroupItemId") == PolicyCarVendorGroupItemId) {

                        $(this).remove();

                    }
                });
                //also add to hidden removed table
                $('#hiddenRemovedPolicyCarVendorGroupItemsTable').append("<tr PolicyCarVendorGroupItemId='" + PolicyCarVendorGroupItemId + "' versionNumber='" + VNBR + "'></tr>")


            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Car Vendor Group Item", { zIndex: 900 });


}


/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyCityGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyCityGroupItem Popup
*/
function AddEditPolicyCityGroupItem(policyCityGroupItemId) {

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyCityGroupItemPopup?id=' + Math.random() + '&policyCityGroupItemId=' + policyCityGroupItemId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyCityGroupItemValidation();
            },
            "Close": function () {
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policyCityGroupItemId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy City Group Item");
    } else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy City Group Item");
    }

}


function DeletePolicyCityGroupItemPopup(policyCityGroupItemId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");
                //remove from visual table

                $('#currentPolicyCityGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("policyCityGroupItemId") == policyCityGroupItemId) {
                        $(this).remove();
                    }
                });
                //also add to hidden removed table
                $('#hiddenRemovedPolicyCityGroupItemsTable').append("<tr policyCityGroupItemId='" + policyCityGroupItemId + "' versionNumber='" + VNBR + "'></tr>")
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy City Group Item", { zIndex: 900 });
}

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyCountryGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyCountryGroupItem Popup
*/
function AddEditPolicyCountryGroupItem(policyCountryGroupItemId) {

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyCountryGroupItemPopup?id=' + Math.random() + '&policyCountryGroupItemId=' + policyCountryGroupItemId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyCountryGroupItemValidation();
				hideInheritIfCustomCountryPolicyItemOnView();
            },
            "Close": function () {               
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policyCountryGroupItemId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Country Group Item");
    } else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Country Group Item");
    }

}

/*
Validate PolicyCountryGroupItem and write to hidden tables or return validation errors
MOVED TO POPUP
*/

function DeletePolicyCountryGroupItemPopup(policyCountryGroupItemId, VNBR) {
	
    $( "#dialog-delete" ).show();
    $( "#dialog-delete" ).dialog({
		resizable: false,
		height:180,
		modal: true,
		buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");
				//remove from visual table
					
				$('#currentPolicyCountryGroupItemsTable tbody tr').each(function(){						
					if($(this).attr("policyCountryGroupItemId")==policyCountryGroupItemId){							
						$(this).remove();								
					}
				});
				//also add to hidden removed table
				$('#hiddenRemovedPolicyCountryGroupItemsTable').append("<tr policyCountryGroupItemId='"+policyCountryGroupItemId+"' versionNumber='"+VNBR+"'></tr>")					
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
	});
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Country Group Item", { zIndex: 900 });	
}

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyHotelCapRateGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyHotelCapRateGroupItem Popup
*/
function AddEditPolicyHotelCapRateGroupItem(policyHotelCapRateGroupItemId) {

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyHotelCapRateGroupItemPopup?id=' + Math.random() + '&policyHotelCapRateGroupItemId=' + policyHotelCapRateGroupItemId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyHotelCapRateGroupItemValidation();
            },
            "Close": function () {
                
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policyHotelCapRateGroupItemId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Hotel Cap Rate Group Item");
    } else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Hotel Cap Rate Group Item");
    }
}

/*
Validate PolicyHotelCapRateGroupItem and write to hidden tables or return validation errors
MOVED TO POPUP
*/

function DeletePolicyHotelCapRateGroupItemPopup(policyHotelCapeRateItemId, VNBR) {
	
    $( "#dialog-delete" ).show();
    $( "#dialog-delete" ).dialog({
		resizable: false,
		height:180,
		modal: true,
		buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");
				//remove from visual table
					
				$('#currentPolicyHotelCapRateGroupItemsTable tbody tr').each(function(){
					if($(this).attr("PolicyHotelCapRateItemId")==policyHotelCapeRateItemId){
						$(this).remove();
					}
				});
				//also add to hidden removed table
				$('#hiddenRemovedPolicyHotelCapRateGroupItemsTable').append("<tr PolicyHotelCapRateItemId='"+policyHotelCapeRateItemId+"' versionNumber='"+VNBR+"'></tr>")
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
	});
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Hotel Cap Rate Group Item", { zIndex: 900 });	
}

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyHotelPropertyGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyHotelPropertyGroupItem Popup
*/
function AddEditPolicyHotelPropertyGroupItem(policyHotelPropertyGroupItemId) {

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyHotelPropertyGroupItemPopup?id=' + Math.random() + '&policyHotelPropertyGroupItemId=' + policyHotelPropertyGroupItemId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyHotelPropertyGroupItemValidation();
            },
            "Close": function () {
                
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policyHotelPropertyGroupItemId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Hotel Property Group Item");
    } else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Hotel Property Group Item");
    }
}

/*
Validate PolicyHotelPropertyGroupItem and write to hidden tables or return validation errors
MOVED TO POPUP
*/


function DeletePolicyHotelPropertyGroupItemPopup(PolicyHotelPropertyGroupItemId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");

                //remove from visual table
                $('#currentPolicyHotelPropertyGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("PolicyHotelPropertyGroupItemId") == PolicyHotelPropertyGroupItemId) {
                        $(this).remove();
                    }
                });

                //also add to hidden removed table
                $('#hiddenRemovedPolicyHotelPropertyGroupItemsTable').append("<tr PolicyHotelPropertyGroupItemId='" + PolicyHotelPropertyGroupItemId + "' versionNumber='" + VNBR + "'></tr>")
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Hotel Property Group Item");
}
/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicyHotelVendorGroupItem
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicyHotelVendorGroupItem Popup
*/
function AddEditPolicyHotelVendorGroupItem(policyHotelVendorGroupItemId) {

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicyHotelVendorGroupItemPopup?id=' + Math.random() + '&policyHotelVendorGroupItemId=' + policyHotelVendorGroupItemId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicyHotelVendorGroupItemValidation();
            },
            "Close": function () {
                
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policyHotelVendorGroupItemId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Hotel Vendor Group Item");
    } else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Hotel Vendor Group Item");
    }

}

/*
Validate PolicyHotelVendorGroupItem and write to hidden tables or return validation errors
MOVED TO POPUP
*/

function DeletePolicyHotelVendorGroupItemPopup(PolicyHotelVendorGroupItemId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");

                //remove from visual table
                $('#currentPolicyHotelVendorGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("PolicyHotelVendorGroupItemId") == PolicyHotelVendorGroupItemId) {
                        $(this).remove();
                    }
                });

                //also add to hidden removed table
                $('#hiddenRemovedPolicyHotelVendorGroupItemsTable').append("<tr PolicyHotelVendorGroupItemId='" + PolicyHotelVendorGroupItemId + "' versionNumber='" + VNBR + "'></tr>")
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Hotel Vendor Group Item");
}


/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicySupplierDealCode
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicySupplierDealCode Popup
*/
function AddEditPolicySupplierDealCode(policySupplierDealCodeId) {

    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicySupplierDealCodePopup?id=' + Math.random() + '&policySupplierDealCodeId=' + policySupplierDealCodeId).dialog({
        resizable: true,
        modal: true,
        height: 550,
        width: 600,
        buttons: {
            "Save": function () {
                PolicySupplierDealCodeValidation();
            },
            "Close": function () {
                
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policySupplierDealCodeId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Supplier Deal Code");
    } else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Supplier Deal Code");
    }
}

/*
Validate PolicySupplierDealCode and write to hidden tables or return validation errors
MOVED TO POPUP
*/

function DeletePolicySupplierDealCodePopup(PolicySupplierDealCodeId, VNBR) {

    $("#dialog-delete").show();
    $("#dialog-delete").dialog({
        resizable: false,
        height: 180,
        modal: true,
        buttons: {
            "Remove": function () {
                $("#dialog-delete").dialog("close");

                //remove from visual table
                $('#currentPolicySupplierDealCodesTable tbody tr').each(function () {
                    if ($(this).attr("PolicySupplierDealCodeId") == PolicySupplierDealCodeId) {
                        $(this).remove();
                    }
                });

                //also add to hidden removed table
                $('#hiddenRemovedPolicySupplierDealCodesTable').append("<tr PolicySupplierDealCodeId='" + PolicySupplierDealCodeId + "' versionNumber='" + VNBR + "'></tr>")
            },
            "Cancel": function () {
                $("#dialog-delete").dialog("close");
            }
        }
    });
    $('#dialog-delete').dialog('option', 'title', "Remove Policy Supplier Deal Code Item");
}

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// PolicySupplierServiceInformation
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
Edit PolicySupplierServiceInformation Popup
*/
function AddEditPolicySupplierServiceInformation(policySupplierServiceInformationId) {
    $("#dialog-confirm").html("");
    $("#dialog-confirm").load('../ClientWizard.mvc/PolicySupplierServiceInformationPopup?id=' + Math.random() + '&policySupplierServiceInformationId=' + policySupplierServiceInformationId).dialog({
        resizable: true,
        modal: true,
        height: 500,
        width: 600,
        buttons: {
            "Save": function () {
                PolicySupplierServiceInformationValidation();
            },
            "Close": function () {
                
                $("#dialog-confirm").dialog("close");
            }
        }
    });
    if (policySupplierServiceInformationId == "0") {
        $('#dialog-confirm').dialog('option', 'title', "Add Policy Supplier Service Information");
    } else {
        $('#dialog-confirm').dialog('option', 'title', "Edit Policy Supplier Service Information");
    }
}

/*Delete Policy Supplier Service Information*/
function DeletePolicySupplierServiceInformationPopup(PolicySupplierServiceInformationId, VNBR) {

$("#dialog-delete").show();
$("#dialog-delete").dialog({
resizable: false,
height: 180,
modal: true,
buttons: {
"Remove": function () {
$("#dialog-delete").dialog("close");

//remove from visual table
$('#currentPolicySupplierServiceInformationsTable tbody tr').each(function () {
if ($(this).attr("PolicySupplierServiceInformationId") == PolicySupplierServiceInformationId) {
$(this).remove();
}
});

//also add to hidden removed table
$('#hiddenRemovedPolicySupplierServiceInformationsTable').append("<tr PolicySupplierServiceInformationId='" + PolicySupplierServiceInformationId + "' versionNumber='" + VNBR + "'></tr>")
},
"Cancel": function () {
$("#dialog-delete").dialog("close");
}
}
});
$('#dialog-delete').dialog('option', 'title', "Remove Policy Supplier Service Information Item");
}






/*
Validate PolicySupplierServiceInformation and write to hidden tables or return validation errors
MOVED TO POUP
*/

/////////////////////////////////////////////////////////////////////////////////////////////////
//
// General Policy Functions
//
/////////////////////////////////////////////////////////////////////////////////////////////////
/*
We show ClientPolicyGroup screen if PolicyGroup does not exist and user has Write Access, otherwise we show ClientPolicies screen
*/
function ChoosePolicyScreen() {

	if (escapeInput($('#ShowPolicyGroupScreen').val()) == "False") {
        ShowClientPoliciesScreen();
    } else {

		if (escapeInput($("#PolicyGroup").val()) != "") {
			var PolicyGroup = $.parseJSON(escapeInput($("#PolicyGroup").val()));
            var policyGroupId = PolicyGroup['PolicyGroupId']

            if (policyGroupId == "" || policyGroupId == "0") {
                ShowClientPolicyGroupScreen();
            } else {
                ShowClientPoliciesScreen();
            };
        } else {
            ShowClientPolicyGroupScreen();
        }
    }
}

/*
Show Client Policy Group (only shown if PolicyGroup does not exist)
*/
function ShowClientPolicyGroupScreen() {
    var url = '/ClientWizard.mvc/PolicyGroupScreen';
    $.ajax({
        type: "POST",
        data: { clientSubUnitGuid: escapeInput($('#ClientSubUnitGuid').val()) },
        url: url,
        error: function () {
            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 6);
            $tabs.tabs('select', 6); // switch to 8th tab
            $("#tabs-7Content").html("Apologies but an error occured.");
        },
        success: function (result) {

            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 6);
            $tabs.tabs('select', 6); // switch to 7th tab
            $("#tabs-7Content").html(result);

         

            //buttons
            $('#clientPolicyGroupBackButton').button();
            $('#clientPolicyGroupBackButton').click(function () {
                $('#tabs').tabs('enable', 5);
                $tabs.tabs('select', 5); // switch to 6th tab
                ShowReasonCodesScreen(true);
            });
            $('#clientPolicyGroupNextButton').button();
            $('#clientPolicyGroupNextButton').click(function () {
                $("#PolicyGroup").val("");
                ShowConfirmChangesScreen();
            });
            $('#clientPolicyGroupCreateButton').button();
            $('#clientPolicyGroupCreateButton').click(function () {
                ValidateClientPolicyGroup();    //save policygroup
            });
        }
    });
}

/*
* Validate POlicyGroup - If successful will bring you to Policies Screen
* THis funtion is only used when there was no existing Policygroup
*/
function ValidateClientPolicyGroup() {

    //reset date fields
	if (escapeInput($('#EnabledDate').val()) == "No Enabled Date") {
        $('#EnabledDate').val("");
    }
	if (escapeInput($('#ExpiryDate').val()) == "No Expiry Date") {
        $('#ExpiryDate').val("");
    }

    /*Update PolicyGroup fields for later use*/
    var policyGroup = {
        PolicyGroupId: "0",
        PolicyGroupName: escapeInput($("#PolicyGroupName").val()),
        EnabledFlag: $("#EnabledFlag").is(':checked'),
        EnabledDate: escapeInput($("#EnabledDate").val()),
        ExpiryDate: escapeInput($("#ExpiryDate").val()),
        InheritFromParentFlag: $("#InheritFromParentFlag").is(':checked'),
        TripTypeId:escapeInput( $("#TripTypeId").val())
    };
    $("#PolicyGroup").val(JSON.stringify(policyGroup));

     //Build Object to Store Client
    var client = {
    	ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
    	PolicyGroup: $.parseJSON(escapeInput($("#PolicyGroup").val()))
    }

    var url = '/ClientWizard.mvc/ValidateClientPolicyGroup';

     //AJAX (JSON) POST of Client Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(client),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 6);
            $tabs.tabs('select', 6); // switch to 8th tab
            $("#tabs-7Content").html(result.html);


            $("#currentPolicyAirCabinGroupItemsTable").tablesorter({
                headers: {

                    0: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },

                    1: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    }


                },

                widgets: ['zebra']
            });

            $("#currentPolicyAirVendorGroupItemsTable").tablesorter({
                headers: {

                    0: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },

                    1: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    }


                },

                widgets: ['zebra']
            });





            var Inherited = escapeInput($('#Inherited').val());
            if (Inherited == "Yes") {
                $('#PolicyGroup_InheritFromParentFlag').attr('checked', 'checked')
            } else {
                $('#PolicyGroup_InheritFromParentFlag').attr('checked', '')

                //the model returns inherited items even if Model.Inherit = False
                //so, if Model.Inherit = False, hide all the inherit stuff
                $('#currentPolicyAirCabinGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyAirMissedSavingsThresholdGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyAirVendorGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyCarTypeGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyCarVendorGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyCityGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyCountryGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyHotelCapRateGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyHotelPropertyGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicyHotelVendorGroupItemsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicySupplierDealCodesTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
                $('#currentPolicySupplierServiceInformationsTable tbody tr').each(function () {
                    if ($(this).attr("Source") != "Custom") {
                        $(this).hide();
                    }
                });
            }


            //hide the inherit change loading image / span			
            $('#inheritCBChange').hide();

            //listener for when inherit checkbox changes			
            $('#PolicyGroup_InheritFromParentFlag').change(function () {
                //keep track of the fact that it's been changed
                $('#PolicyInheritCheckChange').val("Yes");
                $('#inheritCBChange').show();
                var Checked = "";

                if (($(this)).is(':checked')) {

                    //keep track of this for validate / save
                    Checked = "Yes";
                    $('#Inherited').val("Yes");

                    // show waiting
                    
                    // run through table and show inherited					
                    $('#currentPolicyAirCabinGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicyAirVendorGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicyCarTypeGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicyCarVendorGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicyCityGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicyCountryGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicyHotelCapRateGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicyHotelPropertyGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicyHotelVendorGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicySupplierDealCodesTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });
                    $('#currentPolicySupplierServiceInformationsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).show();
                        }
                    });

                } else {

                    Checked = "No";
                    $('#Inherited').val("No");

                    $('#currentPolicyAirCabinGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicyAirVendorGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicyCarTypeGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicyCarVendorGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicyCityGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicyCountryGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicyHotelCapRateGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicyHotelPropertyGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicyHotelVendorGroupItemsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicySupplierDealCodesTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                    $('#currentPolicySupplierServiceInformationsTable tbody tr').each(function () {
                        if ($(this).attr("Source") != "Custom") {
                            $(this).hide();
                        }
                    });
                }
                $('#inheritCBChange').hide();

            });

            //buttons
            $('#clientPoliciesSaveButton').button();
            $('#clientPoliciesSaveButton').click(function () {
                if (hasPageBeenChanged("Policy")) {
                    showSaveDialog(function () {
                        SavePolicyAirParameterGroupItems();
                        CommitClientPolicyChanges(function () {
                            ShowClientSelectionScreen();
                        });
                    },ShowClientPoliciesScreen);
                } else {
                    SavePolicyAirParameterGroupItems();
                }
            });

            $('#clientPoliciesCancelButton').button();
            $('#clientPoliciesCancelButton').click(function () {
                ShowClientPoliciesScreen();
            });

            $('#clientPoliciesBackButton').button();
            $('#clientPoliciesBackButton').click(function () {

                if (hasPageBeenChanged("Policy")) {
                    showSaveDialog(function () {
                        SavePolicyAirParameterGroupItems();
                        CommitClientPolicyChanges(function () {
                            //SHow ReasonCodesScreen
                            $('#tabs').tabs('enable', 5);
                            $tabs.tabs('select', 5); // switch to 7th tab
                            ShowReasonCodesScreen(true);
                        });
                    }, ShowClientPoliciesScreen)
                } else {
                    $('#tabs').tabs('enable', 5);
                    $tabs.tabs('select', 5); // switch to 7th tab
                }

            });

            $('#clientPoliciesNextButton').button();
            $('#clientPoliciesNextButton').click(function () {

                showSaveDialog(function () {
                    SavePolicyAirParameterGroupItems();
                    CommitClientPolicyChanges(function () {
                        ShowClientSelectionScreen();
                    });
                }, ShowClientPoliciesScreen);            
            });

            $('#currentAccountsTable img').click(function () {

                if ($(this).parent().parent().attr("accountStatus") == "Current") {

                    var clientAccount = $(this).parent().parent().attr("id");
                    var SSC = $(this).parent().parent().attr("SSC");

                    var versionNumber = $(this).parent().parent().attr("versionNumber");


                    $('#hiddenRemovedClientAccountsTable').append("<tr clientAccount='" + clientAccount + "' SSC='" + SSC + "' versionNumber='" + versionNumber + "'></tr>");


                }

                else {
                    //it's not current so also remove from added client accounts

                    $('#hiddenAddedClientAccountsTable tr').each(function () {

                        var clientAccount2 = $(this).attr("clientAccount");
                        var SSC2 = $(this).attr("SSC");

                        if (clientAccount2 == clientAccount) {

                            if (SSC2 == SSC) {

                                $(this).remove();
                            }
                        }
                    });

                }

                $(this).parent().parent().remove();

            });
        }
    });

}
/*
Show Client Policies
*/
function ShowClientPoliciesScreen() {

    $("hiddenRemovedPolicyAirCabinGroupItemsTable").empty();
    $("hiddenAddedPolicyAirCabinGroupItemsTable").empty();
    $("hiddenChangedPolicyAirMissedSavingsThresholdGroupItemsTable").empty();
    $("hiddenRemovedPolicyAirMissedSavingsThresholdGroupItemsTable").empty();
    $("hiddenAddedPolicyAirMissedSavingsThresholdGroupItemsTable").empty();
    $("hiddenChangedPolicyAirParameterGroupItemsTable").empty();
    $("hiddenRemovedPolicyAirParameterGroupItemsTable").empty();
    $("hiddenAddedPolicyAirParameterGroupItemsTable").empty();
    $("hiddenChangedPolicyAirVendorGroupItemsTable").empty();
    $("hiddenRemovedPolicyAirVendorGroupItemsTable").empty();
    $("hiddenAddedPolicyAirVendorGroupItemsTable").empty();
    $("hiddenAddedPolicyCarTypeGroupItemsTable").empty();
    $("hiddenChangedPolicyCarTypeGroupItemsTable").empty();
    $("hiddenRemovedPolicyCarTypeGroupItemsTable").empty();
    $("hiddenAddedPolicyCarVendorGroupItemsTable").empty();
    $("hiddenChangedPolicyCarVendorGroupItemsTable").empty();
    $("hiddenRemovedPolicyCarVendorGroupItemsTable").empty();
    $("hiddenAddedPolicyHotelCapRateGroupItemsTable").empty();
    $("hiddenChangedPolicyHotelCapRateGroupItemsTable").empty();
    $("hiddenRemovedPolicyHotelCapRateGroupItemsTable").empty();
    $("hiddenChangedPolicyCityGroupItemsTable").empty();
    $("hiddenAddedPolicyCityGroupItemsTable").empty();
    $("hiddenRemovedPolicyCityGroupItemsTable").empty();
    $("hiddenChangedPolicyCountryGroupItemsTable").empty();
    $("hiddenAddedPolicyCountryGroupItemsTable").empty();
    $("hiddenRemovedPolicyCountryGroupItemsTable").empty();
    $("hiddenAddedPolicyHotelPropertyGroupItemsTable").empty();
    $("hiddenChangedPolicyHotelPropertyGroupItemsTable").empty();
    $("hiddenRemovedPolicyHotelPropertyGroupItemsTable").empty();
    $("hiddenAddedPolicyHotelVendorGroupItemsTable").empty();
    $("hiddenChangedPolicyHotelVendorGroupItemsTable").empty();
    $("hiddenRemovedPolicyHotelVendorGroupItemsTable").empty();
    $("hiddenAddedPolicySupplierDealCodesTable").empty();
    $("hiddenChangedPolicySupplierDealCodesTable").empty();
    $("hiddenRemovedPolicySupplierDealCodesTable").empty();
    $("hiddenAddedPolicySupplierServiceInformationsTable").empty();
    $("hiddenChangedPolicySupplierServiceInformationsTable").empty();
    $("hiddenRemovedPolicySupplierServiceInformationsTable").empty();

    var url = '/ClientWizard.mvc/PoliciesScreen';
    $.ajax({
        type: "POST",
        data: { clientSubUnitGuid: escapeInput($('#ClientSubUnitGuid').val()) },
        url: url,
        error: function () {
            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 6);
            $tabs.tabs('select', 6); // switch to 8th tab
            $("#tabs-7Content").html("Apologies but an error occured.");
        },
        success: function (result) {

            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 6);
            $tabs.tabs('select', 6); // switch to 8th tab
            $("#tabs-7Content").html(result);

            hideInheritIfCustomCountryPolicyItemOnView();
			
            $("#currentPolicyAirCabinGroupItemsTable").tablesorter({
                headers: {

                    0: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
					
                    1: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    }
					

                },

                widgets: ['zebra']
            });
			
			 $("#currentPolicyAirVendorGroupItemsTable").tablesorter({
                headers: {

                    0: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
					
                    1: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    }
					

                },

                widgets: ['zebra']
            });
			
			
			var Inherited = escapeInput($('#Inherited').val());
			if(Inherited=="Yes") {
			    $('#PolicyGroup_InheritFromParentFlag').attr('checked', 'checked')	
			}else {
			    $('#PolicyGroup_InheritFromParentFlag').attr('checked', '')

			    //the model returns inherited items even if Model.Inherit = False
			    //so, if Model.Inherit = False, hide all the inherit stuff
			    $('#currentPolicyAirCabinGroupItemsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicyAirVendorGroupItemsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicyCarTypeGroupItemsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicyCarVendorGroupItemsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicyCountryGroupItemsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicyHotelCapRateGroupItemsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicyHotelPropertyGroupItemsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicyHotelVendorGroupItemsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicySupplierDealCodesTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });
			    $('#currentPolicySupplierServiceInformationsTable tbody tr').each(function () {
			        if ($(this).attr("Source") != "Custom") {
			            $(this).hide();
			        }
			    });			
			}
			
			
			//hide the inherit change loading image / span			
			$('#inheritCBChange').hide();

			//listener for when inherit checkbox changes			
			$('#PolicyGroup_InheritFromParentFlag').change(function () {         
			    //keep track of the fact that it's been changed
				$('#PolicyInheritCheckChange').val("Yes");		
				$('#inheritCBChange').show();			
				var Checked = "";
									
				if(($(this)).is(':checked')) {				

					//keep track of this for validate / save
                    Checked = "Yes";
					$('#Inherited').val("Yes");

					// show waiting

					// run through table and show inherited					
					$('#currentPolicyAirCabinGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyAirAdvancePurchaseGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyAirTimeWindowGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyAirMissedSavingsThresholdGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyAirVendorGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyCarTypeGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyCarVendorGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyCityGroupItemsTable tbody tr').each(function () {
						if ($(this).attr("Source") != "Custom") {
							$(this).show();
						}
					});
					$('#currentPolicyCountryGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyHotelCapRateGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyHotelPropertyGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicyHotelVendorGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicySupplierDealCodesTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});
					$('#currentPolicySupplierServiceInformationsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).show();
					    }
					});	
				}else {
					
					Checked = "No";
					$('#Inherited').val("No");

					$('#currentPolicyAirCabinGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicyAirTimeWindowGroupItemsTable tbody tr').each(function () {
						if ($(this).attr("Source") != "Custom") {
							$(this).hide();
						}
					});
					$('#currentPolicyAirAdvancePurchaseGroupItemsTable tbody tr').each(function () {
						if ($(this).attr("Source") != "Custom") {
							$(this).hide();
						}
					});
					$('#currentPolicyAirMissedSavingsThresholdGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicyAirVendorGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicyCarTypeGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicyCarVendorGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicyCityGroupItemsTable tbody tr').each(function () {
						if ($(this).attr("Source") != "Custom") {
							$(this).hide();
						}
					});
					$('#currentPolicyCountryGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicyHotelCapRateGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicyHotelPropertyGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicyHotelVendorGroupItemsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicySupplierDealCodesTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
					$('#currentPolicySupplierServiceInformationsTable tbody tr').each(function () {
					    if ($(this).attr("Source") != "Custom") {
					        $(this).hide();
					    }
					});
				}
				$('#inheritCBChange').hide();
			});

            //buttons
            $('#clientPoliciesSaveButton').button();
            $('#clientPoliciesSaveButton').click(function () {
                showSaveDialog(function () {
                    SavePolicyAirParameterGroupItems();
                    CommitClientPolicyChanges(function () {
                        ShowClientSelectionScreen();
                    });
                },ShowClientPoliciesScreen);
            });

            $('#clientPoliciesCancelButton').button();
            $('#clientPoliciesCancelButton').click(function () {
                ShowClientPoliciesScreen();
            });

            $('#clientPoliciesBackButton').button();
            $('#clientPoliciesBackButton').click(function () {
                showSaveDialog(function () {
                    SavePolicyAirParameterGroupItems();
                    CommitClientPolicyChanges(function () {
                        //SHow ReasonCodesScreen
                        $('#tabs').tabs('enable', 5);
                        $tabs.tabs('select', 5); // switch to 7th tab
                        ShowReasonCodesScreen(true);
                    });
                },ShowClientPoliciesScreen);
            });
            $('#clientPoliciesNextButton').button();
            $('#clientPoliciesNextButton').click(function () {
                showSaveDialog(function () {
                    SavePolicyAirParameterGroupItems();
                    CommitClientPolicyChanges(function () {
                        ShowClientSelectionScreen();
                    });
                }, ShowClientPoliciesScreen);
            });


            $('#currentAccountsTable img').click(function () {

                if ($(this).parent().parent().attr("accountStatus") == "Current") {

                    var clientAccount = $(this).parent().parent().attr("id");
                    var SSC = $(this).parent().parent().attr("SSC");

                    var versionNumber = $(this).parent().parent().attr("versionNumber");


                    $('#hiddenRemovedClientAccountsTable').append("<tr clientAccount='" + clientAccount + "' SSC='" + SSC + "' versionNumber='" + versionNumber + "'></tr>");


                }

                else {
                    //it's not current so also remove from added client accounts

                    $('#hiddenAddedClientAccountsTable tr').each(function () {

                        var clientAccount2 = $(this).attr("clientAccount");
                        var SSC2 = $(this).attr("SSC");

                        if (clientAccount2 == clientAccount) {

                            if (SSC2 == SSC) {

                                $(this).remove();
                            }
                        }
                    });

                }

                $(this).parent().parent().remove();

            });
        }
    });
}
//save function starts here

function SavePolicyAirParameterGroupItems() {

	var ChangedPolicyAirParameterGroupItem = 0;
	var DeletedPolicyAirParameterGroupItem = 0;
	var AddedPolicyAirParameterGroupItem = 0;

	var AlteredPolicyAirParameterGroupItemsArray = [];
	var DeletedPolicyAirParameterGroupItemsArray = [];
	var AddedPolicyAirParameterGroupItemsArray = [];

	//AirVendorPolicy - Altered
	$('#hiddenChangedPolicyAirParameterGroupItemsTable tbody tr').each(function () {

		//Build Object to Store PolicyAirCabinGroupItem
		var PolicyAirParameterGroupItem = {
			PolicyAirParameterGroupItemId: $(this).attr("PolicyAirParameterGroupItemId"),
			PolicyAirParameterTypeId: $(this).attr("PolicyAirParameterTypeId"),
			PolicyAirParameterValue: $(this).attr("PolicyAirParameterValue"),
			PolicyGroupId: $(this).attr("PolicyGroupId"),
			PolicyRoutingId: $(this).attr("PolicyRoutingId"),
			EnabledFlag: $(this).attr("EnabledFlag"),
			EnabledDate: $(this).attr("EnabledDate"),
			ExpiryDate: $(this).attr("ExpiryDate"),
			TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
			TravelDateValidTo: $(this).attr("TravelDateValidTo"),
			VersionNumber: $(this).attr("VersionNumber")
		};

		//Build Object to Store PolicyRouting
		var policyRouting = {
			Name: $(this).attr("PolicyRoutingName"),
			FromGlobalFlag: $(this).attr("FromGlobalFlag"),
			FromCode: $(this).attr("FromCode"),
			FromCodeType: $(this).attr("FromCodeType"),
			ToGlobalFlag: $(this).attr("ToGlobalFlag"),
			ToCode: $(this).attr("ToCode"),
			ToCodeType: $(this).attr("ToCodeType"),
			RoutingViceVersaFlag: $(this).attr("RoutingViceVersaFlag")
		};

		AlteredPolicyAirParameterGroupItemsArray.push({ PolicyAirParameterGroupItem: PolicyAirParameterGroupItem, PolicyRouting: policyRouting });
		ChangedPolicyAirParameterGroupItem++;
	});

	if (ChangedPolicyAirParameterGroupItem > 0) {
		$('#AlteredPolicyAirParameterGroupItems').val(JSON.stringify(AlteredPolicyAirParameterGroupItemsArray));
	}

	//AirVendorPolicy - Added
	$('#hiddenAddedPolicyAirParameterGroupItemsTable tbody tr').each(function () {
		var PolicyAirParameterGroupItem = {
			PolicyAirParameterGroupItemId: $(this).attr("PolicyAirParameterGroupItemId"),
			PolicyAirParameterTypeId: $(this).attr("PolicyAirParameterTypeId"),
			PolicyAirParameterValue: $(this).attr("PolicyAirParameterValue"),
			PolicyGroupId: $(this).attr("PolicyGroupId"),
			PolicyRoutingId: $(this).attr("PolicyRoutingId"),
			EnabledFlag: $(this).attr("EnabledFlag"),
			EnabledDate: $(this).attr("EnabledDate"),
			ExpiryDate: $(this).attr("ExpiryDate"),
			TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
			TravelDateValidTo: $(this).attr("TravelDateValidTo"),
			VersionNumber: $(this).attr("VersionNumber")
		};

		//Build Object to Store PolicyRouting
		var policyRouting = {
			Name: $(this).attr("PolicyRoutingName"),
			FromGlobalFlag: $(this).attr("FromGlobalFlag"),
			FromCode: $(this).attr("FromCode"),
			FromCodeType: $(this).attr("FromCodeType"),
			ToGlobalFlag: $(this).attr("ToGlobalFlag"),
			ToCode: $(this).attr("ToCode"),
			ToCodeType: $(this).attr("ToCodeType"),
			RoutingViceVersaFlag: $(this).attr("RoutingViceVersaFlag")
		};
		AddedPolicyAirParameterGroupItemsArray.push({ PolicyAirParameterGroupItem: PolicyAirParameterGroupItem, PolicyRouting: policyRouting });
		AddedPolicyAirParameterGroupItem++;
	});
	if (AddedPolicyAirParameterGroupItem > 0) {
		$('#AddedPolicyAirParameterGroupItems').val(JSON.stringify(AddedPolicyAirParameterGroupItemsArray));
	}

	//AirVendorPolicy - Removed
	$('#hiddenRemovedPolicyAirParameterGroupItemsTable tbody tr').each(function () {
		var policyAirParameterGroupItemId = $(this).attr("PolicyAirParameterGroupItemId");
		var policyAirParameterTypeId = $(this).attr("PolicyAirParameterTypeId");
		var versionNumber = $(this).attr("versionNumber");
		DeletedPolicyAirParameterGroupItemsArray.push({
			PolicyAirParameterGroupItemId: policyAirParameterGroupItemId,
			PolicyAirParameterTypeId: policyAirParameterTypeId,
			versionNumber: versionNumber
		})
		DeletedPolicyAirParameterGroupItem++;
	});
	if (DeletedPolicyAirParameterGroupItem > 0) {
		$('#RemovedPolicyAirParameterGroupItems').val(JSON.stringify(DeletedPolicyAirParameterGroupItemsArray));
	}

	SavePolicyAirCabinGroupItems()
}

function SavePolicyAirCabinGroupItems() {


    /*BEGIN - Update PolicyGroup inherit value*/
	var PolicyGroup = $.parseJSON(escapeInput($("#PolicyGroup").val()));
    //alert(PolicyGroup["PolicyGroupId"])
    if (PolicyGroup["PolicyGroupId"] == "0") {
        //Info Needed for new PolicyGroup (rebuild of existing PolicyGroup JSON)
        var policyGroup = {
            PolicyGroupId: "0",
            PolicyGroupName: PolicyGroup["PolicyGroupName"],
            EnabledFlag: PolicyGroup["EnabledFlag"],
            EnabledDate: PolicyGroup["EnabledDate"],
            ExpiryDate: PolicyGroup["ExpiryDate"],
            InheritFromParentFlag: $("#PolicyGroup_InheritFromParentFlag").is(':checked'),
            TripTypeId: PolicyGroup["TripTypeId"]
        };
        $("#PolicyGroup").val(JSON.stringify(policyGroup));
    } else {
        //Info Needed for existing PolicyGroup
        var policyGroup = {
        	PolicyGroupId: escapeInput($("#PolicyGroup_PolicyGroupId").val()),
            InheritFromParentFlag: $("#PolicyGroup_InheritFromParentFlag").is(':checked'),
            VersionNumber: escapeInput($("#PolicyGroup_VersionNumber").val())
        };
        $("#PolicyGroup").val(JSON.stringify(policyGroup));

    }

    /*END - Update PolicyGroup inherit value*/
    var airCabinChanges = 0;
    var deletedAirCabinItems = 0;
    var addedCabinPolicies = 0;

    var AlteredPolicyAirCabinGroupItemsArray = [];
    var DeletedPolicyAirCabinGroupItemsArray = [];
    var AddedPolicyAirCabinGroupItemsArray = [];

    //AirCabinPolicy - Altered
    $('#hiddenChangedPolicyAirCabinGroupItemsTable tbody tr').each(function () {

        //Build Object to Store PolicyAirCabinGroupItem
        var policyAirCabinGroupItem = {
            PolicyAirCabinGroupItemId: $(this).attr("PolicyAirCabinGroupItemId"),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            AirlineCabinCode: $(this).attr("AirlineCabinCode"),
            FlightDurationAllowedMin: $(this).attr("FlightDurationAllowedMin"),
            FlightDurationAllowedMax: $(this).attr("FlightDurationAllowedMax"),
            FlightMileageAllowedMin: $(this).attr("FlightMileageAllowedMin"),
            FlightMileageAllowedMax: $(this).attr("FlightMileageAllowedMax"),
            PolicyRoutingId: $(this).attr("PolicyRoutingId"),
            PolicyProhibitedFlag: $(this).attr("PolicyProhibitedFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };

        //Build Object to Store PolicyRouting
        var policyRouting = {
            Name: $(this).attr("PolicyRoutingName"),
            FromGlobalFlag: $(this).attr("FromGlobalFlag"),
            FromCode: $(this).attr("FromCode"),
            FromCodeType: $(this).attr("FromCodeType"),
            ToGlobalFlag: $(this).attr("ToGlobalFlag"),
            ToCode: $(this).attr("ToCode"),
            ToCodeType: $(this).attr("ToCodeType"),
            RoutingViceVersaFlag: $(this).attr("RoutingViceVersaFlag")
        };

        AlteredPolicyAirCabinGroupItemsArray.push({ PolicyAirCabinGroupItem: policyAirCabinGroupItem, PolicyRouting: policyRouting });
        airCabinChanges++;

    });

    if (airCabinChanges > 0) {
        $('#AlteredPolicyAirCabinGroupItems').val(JSON.stringify(AlteredPolicyAirCabinGroupItemsArray));
    }

    //AirCabinPolicy - Added
    $('#hiddenAddedPolicyAirCabinGroupItemsTable tbody tr').each(function () {

        //Build Object to Store PolicyAirCabinGroupItem
        var policyAirCabinGroupItem = {
            PolicyAirCabinGroupItemId: $(this).attr("PolicyAirCabinGroupItemId"),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            AirlineCabinCode: $(this).attr("AirlineCabinCode"),
            FlightDurationAllowedMin: $(this).attr("FlightDurationAllowedMin"),
            FlightDurationAllowedMax: $(this).attr("FlightDurationAllowedMax"),
            FlightMileageAllowedMin: $(this).attr("FlightMileageAllowedMin"),
            FlightMileageAllowedMax: $(this).attr("FlightMileageAllowedMax"),
            PolicyRoutingId: $(this).attr("PolicyRoutingId"),
            PolicyProhibitedFlag: $(this).attr("PolicyProhibitedFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };

        //Build Object to Store PolicyRouting
        var policyRouting = {
            Name: $(this).attr("PolicyRoutingName"),
            FromGlobalFlag: $(this).attr("FromGlobalFlag"),
            FromCode: $(this).attr("FromCode"),
            FromCodeType: $(this).attr("FromCodeType"),
            ToGlobalFlag: $(this).attr("ToGlobalFlag"),
            ToCode: $(this).attr("ToCode"),
            ToCodeType: $(this).attr("ToCodeType"),
            RoutingViceVersaFlag: $(this).attr("RoutingViceVersaFlag")
        };

        AddedPolicyAirCabinGroupItemsArray.push({ PolicyAirCabinGroupItem: policyAirCabinGroupItem, PolicyRouting: policyRouting });
        addedCabinPolicies++;
    });

    if (addedCabinPolicies > 0) {
        $('#AddedPolicyAirCabinGroupItems').val(JSON.stringify(AddedPolicyAirCabinGroupItemsArray));
    }

    //AirCabinPolicy - Deleted
    $('#hiddenRemovedPolicyAirCabinGroupItemsTable tbody tr').each(function () {

        var policyAirCabinGroupItemId = $(this).attr("policyAirCabinGroupItemId");
        var versionNumber = $(this).attr("versionNumber");

        DeletedPolicyAirCabinGroupItemsArray.push({ policyAirCabinGroupItemId: policyAirCabinGroupItemId, versionNumber: versionNumber })
        deletedAirCabinItems++;
    });

    if (deletedAirCabinItems > 0) {
        $('#RemovedPolicyAirCabinGroupItems').val(JSON.stringify(DeletedPolicyAirCabinGroupItemsArray));
    }

    SavePolicyAirMissedSavingsThresholdGroupItems();
}

function SavePolicyAirMissedSavingsThresholdGroupItems() {

    var changedAirMSTItems = 0;
    var deletedAirMSTItems = 0;
    var addedAirMSTItems = 0;
    var AlteredPolicyAirMSTItemsArray = [];
    var DeletedPolicyAirMSTItemsArray = [];
    var AddedPolicyAirMSTItemsArray = [];

    $('#hiddenAddedPolicyAirMissedSavingsThresholdGroupItemsTable tr').each(function () {

        var policyAirMSTGroupItem = {
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            MissedThresholdAmount: $(this).attr("MissedThresholdAmount"),
            CurrencyCode: $(this).attr("CurrencyCode"),
            RoutingCode: $(this).attr("RoutingCode"),
            SavingsZeroedOutFlag: $(this).attr("SavingsZeroedOutFlag"),
            PolicyProhibitedFlag: $(this).attr("PolicyProhibitedFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };

        AddedPolicyAirMSTItemsArray.push(policyAirMSTGroupItem)
        addedAirMSTItems++

    });
    if (addedAirMSTItems > 0) {
        $('#AddedPolicyAirMissedSavingsThresholdGroupItems').val(JSON.stringify(AddedPolicyAirMSTItemsArray))
    }

    $('#hiddenChangedPolicyAirMissedSavingsThresholdGroupItemsTable tr').each(function () {

        var policyAirMSTGroupItem = {
            PolicyAirMissedSavingsThresholdGroupItemId: $(this).attr('PolicyAirMissedSavingsThresholdGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            MissedThresholdAmount: $(this).attr("MissedThresholdAmount"),
            CurrencyCode: $(this).attr("CurrencyCode"),
            RoutingCode: $(this).attr("RoutingCode"),
            SavingsZeroedOutFlag: $(this).attr("SavingsZeroedOutFlag"),
            PolicyProhibitedFlag: $(this).attr("PolicyProhibitedFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };

        AlteredPolicyAirMSTItemsArray.push(policyAirMSTGroupItem)
        changedAirMSTItems++

    });

    if (changedAirMSTItems > 0) {
        $('#AlteredPolicyAirMissedSavingsThresholdGroupItems').val(JSON.stringify(AlteredPolicyAirMSTItemsArray))
    }

    $('#hiddenRemovedPolicyAirMissedSavingsThresholdGroupItemsTable tr').each(function () {

        var PolicyAirMissedSavingsThresholdGroupItemId = $(this).attr("PolicyAirMissedSavingsThresholdGroupItemId");
        var versionNumber = $(this).attr("VersionNumber");
        DeletedPolicyAirMSTItemsArray.push({ policyAirMissedSavingsThresholdGroupItemId: PolicyAirMissedSavingsThresholdGroupItemId, versionNumber: versionNumber })
        deletedAirMSTItems++
    });

    if (deletedAirMSTItems > 0) {
        $('#RemovedPolicyAirMissedSavingsThresholdGroupItems').val(JSON.stringify(DeletedPolicyAirMSTItemsArray))
    }

    SavePolicyAirVendorGroupItems();

}

function SavePolicyAirVendorGroupItems() {

	var airVendorChanges = 0;
	var deletedAirVendorItems = 0;
	var addedAirVendorItems = 0;

	var AlteredPolicyAirVendorGroupItemsArray = [];
	var DeletedPolicyAirVendorGroupItemsArray = [];
	var AddedPolicyAirVendorGroupItemsArray = [];

	//AirVendorPolicy - Altered
	$('#hiddenChangedPolicyAirVendorGroupItemsTable tbody tr').each(function () {

		//Build Object to Store PolicyAirCabinGroupItem
		var policyAirVendorGroupItem = {
			PolicyAirVendorGroupItemId: $(this).attr("PolicyAirVendorGroupItemId"),
			ProductId: $(this).attr("ProductId"),
			PolicyAirStatusId: $(this).attr("PolicyAirStatusId"),
			PolicyGroupId: $(this).attr("PolicyGroupId"),
			SupplierCode: $(this).attr("SupplierCode"),
			AirVendorRanking: $(this).attr("AirVendorRanking"),
			PolicyRoutingId: $(this).attr("PolicyRoutingId"),
			EnabledFlag: $(this).attr("EnabledFlag"),
			EnabledDate: $(this).attr("EnabledDate"),
			ExpiryDate: $(this).attr("ExpiryDate"),
			TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
			TravelDateValidTo: $(this).attr("TravelDateValidTo"),
			VersionNumber: $(this).attr("VersionNumber")
		};

		//Build Object to Store PolicyRouting
		var policyRouting = {
			Name: $(this).attr("PolicyRoutingName"),
			FromGlobalFlag: $(this).attr("FromGlobalFlag"),
			FromCode: $(this).attr("FromCode"),
			FromCodeType: $(this).attr("FromCodeType"),
			ToGlobalFlag: $(this).attr("ToGlobalFlag"),
			ToCode: $(this).attr("ToCode"),
			ToCodeType: $(this).attr("ToCodeType"),
			RoutingViceVersaFlag: $(this).attr("RoutingViceVersaFlag")
		};
		AlteredPolicyAirVendorGroupItemsArray.push({ PolicyAirVendorGroupItem: policyAirVendorGroupItem, PolicyRouting: policyRouting });
		airVendorChanges++;
	});

	if (airVendorChanges > 0) {
		$('#AlteredPolicyAirVendorGroupItems').val(JSON.stringify(AlteredPolicyAirVendorGroupItemsArray));
	}

	//AirVendorPolicy - Added
	$('#hiddenAddedPolicyAirVendorGroupItemsTable tbody tr').each(function () {
		var policyAirVendorGroupItem = {
			PolicyAirVendorGroupItemId: $(this).attr("PolicyAirVendorGroupItemId"),
			PolicyGroupId: $(this).attr("PolicyGroupId"),
			ProductId: $(this).attr("ProductId"),
			PolicyAirStatusId: $(this).attr("PolicyAirStatusId"),
			SupplierCode: $(this).attr("SupplierCode"),
			AirVendorRanking: $(this).attr("AirVendorRanking"),
			PolicyRoutingId: $(this).attr("PolicyRoutingId"),
			EnabledFlag: $(this).attr("EnabledFlag"),
			EnabledDate: $(this).attr("EnabledDate"),
			ExpiryDate: $(this).attr("ExpiryDate"),
			TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
			TravelDateValidTo: $(this).attr("TravelDateValidTo"),
			VersionNumber: $(this).attr("VersionNumber")
		};

		//Build Object to Store PolicyRouting
		var policyRouting = {
			Name: $(this).attr("PolicyRoutingName"),
			FromGlobalFlag: $(this).attr("FromGlobalFlag"),
			FromCode: $(this).attr("FromCode"),
			FromCodeType: $(this).attr("FromCodeType"),
			ToGlobalFlag: $(this).attr("ToGlobalFlag"),
			ToCode: $(this).attr("ToCode"),
			ToCodeType: $(this).attr("ToCodeType"),
			RoutingViceVersaFlag: $(this).attr("RoutingViceVersaFlag")
		};
		AddedPolicyAirVendorGroupItemsArray.push({ PolicyAirVendorGroupItem: policyAirVendorGroupItem, PolicyRouting: policyRouting });
		addedAirVendorItems++;
	});
	if (addedAirVendorItems > 0) {
		$('#AddedPolicyAirVendorGroupItems').val(JSON.stringify(AddedPolicyAirVendorGroupItemsArray));
	}

	//AirVendorPolicy - Removed
	$('#hiddenRemovedPolicyAirVendorGroupItemsTable tbody tr').each(function () {
		var policyAirVendorGroupItemId = $(this).attr("policyAirVendorGroupItemId");
		var versionNumber = $(this).attr("versionNumber");

		DeletedPolicyAirVendorGroupItemsArray.push({ policyAirVendorGroupItemId: policyAirVendorGroupItemId, versionNumber: versionNumber })
		deletedAirVendorItems++;

	});
	if (deletedAirVendorItems > 0) {
		$('#RemovedPolicyAirVendorGroupItems').val(JSON.stringify(DeletedPolicyAirVendorGroupItemsArray));
	}

	SavePolicyCarTypeGroupItems();
}

function SavePolicyCarTypeGroupItems(){
  
	var changedCarTypeItems = 0;
    var deletedCarTypeItems = 0;
    var addedCarTypeItems = 0;

    var AlteredPolicyCarTypeItemsArray = [];
    var DeletedPolicyCarTypeItemsArray = [];
    var AddedPolicyCarTypeItemsArray = [];
	
	$('#hiddenAddedPolicyCarTypeGroupItemsTable tr').each(function(){
				
	    var policyCarTypeGroupItem = {
            PolicyCarTypeGroupItemId: $(this).attr('PolicyCarTypeGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            PolicyCarStatusId: $(this).attr("PolicyCarStatusId"),
            CarTypeCategoryId: $(this).attr("CarTypeCategoryId"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AddedPolicyCarTypeItemsArray.push(policyCarTypeGroupItem)
	    addedCarTypeItems++
	});
	
	if(addedCarTypeItems>0){
		$('#AddedPolicyCarTypeGroupItems').val(JSON.stringify(AddedPolicyCarTypeItemsArray))		
	}
	
	$('#hiddenChangedPolicyCarTypeGroupItemsTable tr').each(function(){
		
		var policyCarTypeGroupItem = {
            PolicyCarTypeGroupItemId: $(this).attr('PolicyCarTypeGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            PolicyCarStatusId: $(this).attr("PolicyCarStatusId"),
            CarTypeCategoryId: $(this).attr("CarTypeCategoryId"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AlteredPolicyCarTypeItemsArray.push(policyCarTypeGroupItem)	
	    changedCarTypeItems++
		
	});
	
	if(changedCarTypeItems>0){
		$('#AlteredPolicyCarTypeGroupItems').val(JSON.stringify(AlteredPolicyCarTypeItemsArray))
	}
	
	$('#hiddenRemovedPolicyCarTypeGroupItemsTable tr').each(function(){		
        var PolicyCarTypeGroupItemId = $(this).attr("PolicyCarTypeGroupItemId");
        var versionNumber = $(this).attr("VersionNumber");		
        DeletedPolicyCarTypeItemsArray.push({policyCarTypeGroupItemId: PolicyCarTypeGroupItemId, versionNumber: versionNumber })		
		deletedCarTypeItems++
	});
	
	if(deletedCarTypeItems>0){		
		$('#RemovedPolicyCarTypeGroupItems').val(JSON.stringify(DeletedPolicyCarTypeItemsArray))
	}
		
    SavePolicyCarVendorGroupItems();
}

function SavePolicyCarVendorGroupItems(){
	
	var changedCarVendorItems = 0;
    var deletedCarVendorItems = 0;
    var addedCarVendorItems = 0;
    var AlteredPolicyCarVendorItemsArray = [];
    var DeletedPolicyCarVendorItemsArray = [];
    var AddedPolicyCarVendorItemsArray = [];
	
	$('#hiddenAddedPolicyCarVendorGroupItemsTable tr').each(function(){
		
	    var policyCarVendorGroupItem = {
            PolicyCarVendorGroupItemId: $(this).attr('PolicyCarVendorGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            PolicyCarStatusId: $(this).attr("PolicyCarStatusId"),
            CarVendorCategoryId: $(this).attr("CarVendorCategoryId"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
		    SupplierCode: $(this).attr("SupplierCode"),
		    ProductId: $(this).attr("ProductId"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AddedPolicyCarVendorItemsArray.push(policyCarVendorGroupItem)
	    addedCarVendorItems++
	});
	
	if(addedCarVendorItems>0){
		$('#AddedPolicyCarVendorGroupItems').val(JSON.stringify(AddedPolicyCarVendorItemsArray))	
	}
	
	$('#hiddenChangedPolicyCarVendorGroupItemsTable tr').each(function(){
		
		var policyCarVendorGroupItem = {
            PolicyCarVendorGroupItemId: $(this).attr('PolicyCarVendorGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            PolicyCarStatusId: $(this).attr("PolicyCarStatusId"),
            CarVendorCategoryId: $(this).attr("CarVendorCategoryId"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
		    SupplierCode: $(this).attr("SupplierCode"),
		    ProductId: $(this).attr("ProductId"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AlteredPolicyCarVendorItemsArray.push(policyCarVendorGroupItem)
	    changedCarVendorItems++
		
	});
	
	if(changedCarVendorItems>0){
		
		$('#AlteredPolicyCarVendorGroupItems').val(JSON.stringify(AlteredPolicyCarVendorItemsArray))
	}
	
	$('#hiddenRemovedPolicyCarVendorGroupItemsTable tr').each(function(){
        var PolicyCarVendorGroupItemId = $(this).attr("PolicyCarVendorGroupItemId");
        var versionNumber = $(this).attr("VersionNumber");
		
        DeletedPolicyCarVendorItemsArray.push({policyCarVendorGroupItemId: PolicyCarVendorGroupItemId, versionNumber: versionNumber })
		deletedCarVendorItems++
		
	});
	
	if(deletedCarVendorItems>0){
		$('#RemovedPolicyCarVendorGroupItems').val(JSON.stringify(DeletedPolicyCarVendorItemsArray))
	}
    SavePolicyCityGroupItems()
	
}

function SavePolicyCityGroupItems(){
    
	var changedCityItems = 0;
    var deletedCityItems = 0;
    var addedCityItems = 0;
    var AlteredPolicyCityItemsArray = [];
    var DeletedPolicyCityItemsArray = [];
    var AddedPolicyCityItemsArray = [];
	
	$('#hiddenAddedPolicyCityGroupItemsTable tr').each(function(){
		
	    var policyCityGroupItem = {
            PolicyCityGroupItemId: $(this).attr('PolicyCityGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyCityStatusId: $(this).attr("PolicyCityStatusId"),
            CityCode: $(this).attr("CityCode"),
            InheritFromParentFlag: $(this).attr("InheritFromParentFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AddedPolicyCityItemsArray.push(policyCityGroupItem)
	    addedCityItems++
      
	});
	
	if(addedCityItems>0){
		$('#AddedPolicyCityGroupItems').val(JSON.stringify(AddedPolicyCityItemsArray))	
	}
	
	$('#hiddenChangedPolicyCityGroupItemsTable tr').each(function(){

	    var policyCityGroupItem = {
	        PolicyCityGroupItemId: $(this).attr('PolicyCityGroupItemId'),
	        PolicyGroupId: $(this).attr("PolicyGroupId"),
	        PolicyCityStatusId: $(this).attr("PolicyCityStatusId"),
	        CityCode: $(this).attr("CityCode"),
	        InheritFromParentFlag: $(this).attr("InheritFromParentFlag"),
	        EnabledFlag: $(this).attr("EnabledFlag"),
	        EnabledDate: $(this).attr("EnabledDate"),
	        ExpiryDate: $(this).attr("ExpiryDate"),
	        TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
	        TravelDateValidTo: $(this).attr("TravelDateValidTo"),
	        VersionNumber: $(this).attr("VersionNumber")
	    };
	
	    AlteredPolicyCityItemsArray.push(policyCityGroupItem)
	    changedCityItems++
		
	});
	
	if(changedCityItems>0){
		$('#AlteredPolicyCityGroupItems').val(JSON.stringify(AlteredPolicyCityItemsArray))
	}

	$('#hiddenRemovedPolicyCityGroupItemsTable tr').each(function(){
	
        var PolicyCityGroupItemId = $(this).attr("PolicyCityGroupItemId");
        var versionNumber = $(this).attr("VersionNumber");	
        DeletedPolicyCityItemsArray.push({policyCityGroupItemId: PolicyCityGroupItemId, versionNumber: versionNumber })	
		deletedCityItems++	
	});
	
	if(deletedCityItems>0){
		$('#RemovedPolicyCityGroupItems').val(JSON.stringify(DeletedPolicyCityItemsArray))
	}

    SavePolicyCountryGroupItems();
	
}

function SavePolicyCountryGroupItems() {

    var changedCountryItems = 0;
    var deletedCountryItems = 0;
    var addedCountryItems = 0;
    var AlteredPolicyCountryItemsArray = [];
    var DeletedPolicyCountryItemsArray = [];
    var AddedPolicyCountryItemsArray = [];

    $('#hiddenAddedPolicyCountryGroupItemsTable tr').each(function () {

        var policyCountryGroupItem = {
            PolicyCountryGroupItemId: $(this).attr('PolicyCountryGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyCountryStatusId: $(this).attr("PolicyCountryStatusId"),
            CountryCode: $(this).attr("CountryCode"),
            InheritFromParentFlag: $(this).attr("InheritFromParentFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };

        AddedPolicyCountryItemsArray.push(policyCountryGroupItem)
        addedCountryItems++
    });

    if (addedCountryItems > 0) {
        $('#AddedPolicyCountryGroupItems').val(JSON.stringify(AddedPolicyCountryItemsArray))
    }

    $('#hiddenChangedPolicyCountryGroupItemsTable tr').each(function () {

        var policyCountryGroupItem = {
            PolicyCountryGroupItemId: $(this).attr('PolicyCountryGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyCountryStatusId: $(this).attr("PolicyCountryStatusId"),
            CountryCode: $(this).attr("CountryCode"),
            InheritFromParentFlag: $(this).attr("InheritFromParentFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };

        AlteredPolicyCountryItemsArray.push(policyCountryGroupItem)
        changedCountryItems++

    });

    if (changedCountryItems > 0) {
        $('#AlteredPolicyCountryGroupItems').val(JSON.stringify(AlteredPolicyCountryItemsArray))
    }

    $('#hiddenRemovedPolicyCountryGroupItemsTable tr').each(function () {

        var PolicyCountryGroupItemId = $(this).attr("PolicyCountryGroupItemId");
        var versionNumber = $(this).attr("VersionNumber");
        DeletedPolicyCountryItemsArray.push({ policyCountryGroupItemId: PolicyCountryGroupItemId, versionNumber: versionNumber })
        deletedCountryItems++
    });

    if (deletedCountryItems > 0) {
        $('#RemovedPolicyCountryGroupItems').val(JSON.stringify(DeletedPolicyCountryItemsArray))
    }

    SavePolicyHotelCapRateGroupItems();

}

function SavePolicyHotelCapRateGroupItems(){
	
	var changedCapRateItems = 0;
    var deletedCapRateItems = 0;
    var addedCapRateItems = 0;

    var AlteredPolicyCapRateItemsArray = [];
    var DeletedPolicyCapRateItemsArray = [];
    var AddedPolicyCapRateItemsArray = [];
	
	$('#hiddenAddedPolicyHotelCapRateGroupItemsTable tr').each(function(){
		
		
	    var policyCapRateItem = {
            PolicyHotelCapRateItemId: $(this).attr('PolicyHotelCapRateItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            CurrencyCode: $(this).attr("CurrencyCode"),
            CapRate: $(this).attr("CapRate"),
            //PolicyProhibitedFlag: $(this).attr("PolicyProhibitedFlag"),
            TaxInclusiveFlag: $(this).attr("TaxInclusiveFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };

        AddedPolicyCapRateItemsArray.push(policyCapRateItem)
	    addedCapRateItems++
	});
	
	if(addedCapRateItems>0){
		$('#AddedPolicyHotelCapRateGroupItems').val(JSON.stringify(AddedPolicyCapRateItemsArray))	
	}
	
	$('#hiddenChangedPolicyHotelCapRateGroupItemsTable tr').each(function(){

	    var policyCapRateItem = {
            PolicyHotelCapRateItemId: $(this).attr('PolicyHotelCapRateItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            CurrencyCode: $(this).attr("CurrencyCode"),
            CapRate: $(this).attr("CapRate"),
            //PolicyProhibitedFlag: $("PolicyProhibitedFlag"),
            TaxInclusiveFlag: $(this).attr("TaxInclusiveFlag"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AlteredPolicyCapRateItemsArray.push(policyCapRateItem)
	    changedCapRateItems++
	});
	
	if(changedCapRateItems>0){
		$('#AlteredPolicyHotelCapRateGroupItems').val(JSON.stringify(AlteredPolicyCapRateItemsArray))
	}
	
	
	$('#hiddenRemovedPolicyHotelCapRateGroupItemsTable tr').each(function(){
	    var PolicyHotelCapRateItemId = $(this).attr("PolicyHotelCapRateItemId");
        var versionNumber = $(this).attr("VersionNumber");
        DeletedPolicyCapRateItemsArray.push({ PolicyHotelCapRateItemId: PolicyHotelCapRateItemId, versionNumber: versionNumber })
		deletedCapRateItems++
		
	});
	
	if(deletedCapRateItems>0){
		$('#RemovedPolicyHotelCapRateGroupItems').val(JSON.stringify(DeletedPolicyCapRateItemsArray))
	}
	
	SavePolicyHotelPropertyGroupItems();
	
}

function SavePolicyHotelPropertyGroupItems(){
	
	var changedHotelPropertyGroupItems = 0;
    var deletedHotelPropertyGroupItems = 0;
    var addedHotelPropertyGroupItems = 0;

    var AlteredPolicyHotelPropertyGroupItemsArray = [];
    var DeletedPolicyHotelPropertyGroupItemsArray = [];
    var AddedPolicyHotelPropertyGroupItemsArray = [];
	
	$('#hiddenAddedPolicyHotelPropertyGroupItemsTable tr').each(function(){
	    var policyHotelPropertyGroupItem = {
            PolicyHotelPropertyGroupItemId: $(this).attr('PolicyHotelPropertyGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyHotelStatusId: $(this).attr("PolicyHotelStatusId"),
            HarpHotelId: escapeInput($("#HarpHotelId").val()),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AddedPolicyHotelPropertyGroupItemsArray.push(policyHotelPropertyGroupItem)
	    addedHotelPropertyGroupItems++
	});
	
	if(addedHotelPropertyGroupItems>0){
		$('#AddedPolicyHotelPropertyGroupItems').val(JSON.stringify(AddedPolicyHotelPropertyGroupItemsArray))	
	}
	
	$('#hiddenChangedPolicyHotelPropertyGroupItemsTable tr').each(function(){

	   var policyHotelPropertyGroupItem = {
	       PolicyHotelPropertyGroupItemId: $(this).attr('PolicyHotelPropertyGroupItemId'),
	       PolicyGroupId: $(this).attr("PolicyGroupId"),
	       PolicyHotelStatusId: $(this).attr("PolicyHotelStatusId"),
	       HarpHotelId: escapeInput($("#HarpHotelId").val()),
	       EnabledFlag: $(this).attr("EnabledFlag"),
	       EnabledDate: $(this).attr("EnabledDate"),
	       ExpiryDate: $(this).attr("ExpiryDate"),
	       TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
	       TravelDateValidTo: $(this).attr("TravelDateValidTo"),
	       VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AlteredPolicyHotelPropertyGroupItemsArray.push(policyHotelPropertyGroupItem)
	    changedHotelPropertyGroupItems++
	});
	
	if(changedHotelPropertyGroupItems>0){
		$('#AlteredPolicyHotelPropertyGroupItems').val(JSON.stringify(AlteredPolicyHotelPropertyGroupItemsArray))
	}
	
	$('#hiddenRemovedPolicyHotelPropertyGroupItemsTable tr').each(function(){
        var PolicyHotelPropertyGroupItemId = $(this).attr("PolicyHotelPropertyGroupItemId");
        var versionNumber = $(this).attr("VersionNumber");
		
        DeletedPolicyHotelPropertyGroupItemsArray.push({policyHotelPropertyGroupItemId: PolicyHotelPropertyGroupItemId, versionNumber: versionNumber })
		deletedHotelPropertyGroupItems++
		
	});
	
	if(deletedHotelPropertyGroupItems>0){
		$('#RemovedPolicyHotelPropertyGroupItems').val(JSON.stringify(DeletedPolicyHotelPropertyGroupItemsArray))
	}

	SavePolicyHotelVendorGroupItems();
}

function SavePolicyHotelVendorGroupItems() {
	
	
	var changedHotelVendorGroupItems = 0;
    var deletedHotelVendorGroupItems = 0;
    var addedHotelVendorGroupItems = 0;

    var AlteredPolicyHotelVendorGroupItemsArray = [];
    var DeletedPolicyHotelVendorGroupItemsArray = [];
    var AddedPolicyHotelVendorGroupItemsArray = [];
	
	$('#hiddenAddedPolicyHotelVendorGroupItemsTable tr').each(function(){
	    var policyHotelVendorGroupItem = {
            PolicyHotelVendorGroupItemId: $(this).attr('PolicyHotelVendorGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            PolicyHotelStatusId: $(this).attr("PolicyHotelStatusId"),
            ProductId: escapeInput($("#ProductId").val()),
            SupplierCode: $(this).attr("SupplierCode"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AddedPolicyHotelVendorGroupItemsArray.push(policyHotelVendorGroupItem);
	    addedHotelVendorGroupItems++;
	});
	
	if(addedHotelVendorGroupItems>0){
		
		$('#AddedPolicyHotelVendorGroupItems').val(JSON.stringify(AddedPolicyHotelVendorGroupItemsArray))	
		
	}
	
	$('#hiddenChangedPolicyHotelVendorGroupItemsTable tr').each(function(){

	   var policyHotelVendorGroupItem = {
            PolicyHotelVendorGroupItemId: $(this).attr('PolicyHotelVendorGroupItemId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            PolicyHotelStatusId: $(this).attr("PolicyHotelStatusId"),
            ProductId: $(this).attr("ProductId"),
            SupplierCode: $(this).attr("SupplierCode"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            TravelDateValidFrom: $(this).attr("TravelDateValidFrom"),
            TravelDateValidTo: $(this).attr("TravelDateValidTo"),
            VersionNumber: $(this).attr("VersionNumber")
        };
	
	    AlteredPolicyHotelVendorGroupItemsArray.push(policyHotelVendorGroupItem);
	    changedHotelVendorGroupItems++;
	});
	
	if(changedHotelVendorGroupItems>0){
		$('#AlteredPolicyHotelVendorGroupItems').val(JSON.stringify(AlteredPolicyHotelVendorGroupItemsArray))
	}
	
	
	$('#hiddenRemovedPolicyHotelVendorGroupItemsTable tr').each(function(){
        var PolicyHotelVendorGroupItemId = $(this).attr("PolicyHotelVendorGroupItemId");
        var versionNumber = $(this).attr("VersionNumber");
	
        DeletedPolicyHotelVendorGroupItemsArray.push({policyHotelVendorGroupItemId: PolicyHotelVendorGroupItemId, versionNumber: versionNumber })
		deletedHotelVendorGroupItems++
	});
    if (deletedHotelVendorGroupItems > 0) {
        $('#RemovedPolicyHotelVendorGroupItems').val(JSON.stringify(DeletedPolicyHotelVendorGroupItemsArray))
    }

    SavePolicySupplierDealCodes();
}

function SavePolicySupplierServiceInformations() {

    /*setup*/
    var changedPolicySuppliers = 0;
    var deletedPolicySuppliers = 0;
    var addedPolicySuppliers = 0;
    var AlteredPolicySupplierServiceInformationsArray = [];
    var DeletedPolicySupplierServiceInformationsArray = [];
    var AddedPolicySupplierServiceInformationsArray = [];

    /*added*/
    $('#hiddenAddedPolicySupplierServiceInformationsTable tr').each(function () {

        var policySupplierServiceInformation = {
            PolicySupplierServiceInformationId: $(this).attr('PolicySupplierServiceInformationId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicySupplierServiceInformationValue: $(this).attr("PolicySupplierServiceInformationValue"),
            PolicySupplierServiceInformationTypeId: $(this).attr("PolicySupplierServiceInformationTypeId"),
            SupplierCode: $(this).attr("SupplierCode"),
            ProductId: $(this).attr("ProductId"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            VersionNumber: $(this).attr("VersionNumber")
        };
        AddedPolicySupplierServiceInformationsArray.push(policySupplierServiceInformation);
        addedPolicySuppliers++;
    });
    if (addedPolicySuppliers > 0) {
        $('#AddedPolicySupplierServiceInformations').val(JSON.stringify(AddedPolicySupplierServiceInformationsArray))
    }

    /*altered*/
    $('#hiddenChangedPolicySupplierServiceInformationsTable tr').each(function () {
        var policySupplierServiceInformation = {
            PolicySupplierServiceInformationId: $(this).attr('PolicySupplierServiceInformationId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicySupplierServiceInformationValue: $(this).attr("PolicySupplierServiceInformationValue"),
            PolicySupplierServiceInformationTypeId: $(this).attr("PolicySupplierServiceInformationTypeId"),
            SupplierCode: $(this).attr("SupplierCode"),
            ProductId: $(this).attr("ProductId"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            VersionNumber: $(this).attr("VersionNumber")
        };
        AlteredPolicySupplierServiceInformationsArray.push(policySupplierServiceInformation);
        changedPolicySuppliers++;
    });
    if (changedPolicySuppliers > 0) {
        $('#AlteredPolicySupplierServiceInformations').val(JSON.stringify(AlteredPolicySupplierServiceInformationsArray))
    }

    /*removed*/
    $('#hiddenRemovedPolicySupplierServiceInformationsTable tr').each(function () {
        var PolicySupplierServiceInformationId = $(this).attr("PolicySupplierServiceInformationId");
        var VersionNumber = $(this).attr("VersionNumber");
        DeletedPolicySupplierServiceInformationsArray.push({ PolicySupplierServiceInformationId: PolicySupplierServiceInformationId, VersionNumber: VersionNumber });
        deletedPolicySuppliers++;
    });
    if (deletedPolicySuppliers > 0) {
        $('#RemovedPolicySupplierServiceInformations').val(JSON.stringify(DeletedPolicySupplierServiceInformationsArray))
    }
}

function CommitClientPolicyChanges(successCb) {

    var clientPolicyChanges = {
        ClientTopUnit: $.parseJSON(escapeInput($("#ClientTopUnit").val())),
        ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
        PolicyAirParameterGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyAirParameterGroupItems").val())),
        PolicyAirParameterGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyAirParameterGroupItems").val())),
        PolicyAirParameterGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyAirParameterGroupItems").val())),
        PolicyAirCabinGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyAirCabinGroupItems").val())),
        PolicyAirCabinGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyAirCabinGroupItems").val())),
        PolicyAirCabinGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyAirCabinGroupItems").val())),
        PolicyAirMissedSavingsThresholdGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyAirMissedSavingsThresholdGroupItems").val())),
        PolicyAirMissedSavingsThresholdGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyAirMissedSavingsThresholdGroupItems").val())),
        PolicyAirMissedSavingsThresholdGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyAirMissedSavingsThresholdGroupItems").val())),
        PolicyAirVendorGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyAirVendorGroupItems").val())),
        PolicyAirVendorGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyAirVendorGroupItems").val())),
        PolicyAirVendorGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyAirVendorGroupItems").val())),
        PolicyCarTypeGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyCarTypeGroupItems").val())),
        PolicyCarTypeGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyCarTypeGroupItems").val())),
        PolicyCarTypeGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyCarTypeGroupItems").val())),
        PolicyCarVendorGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyCarVendorGroupItems").val())),
        PolicyCarVendorGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyCarVendorGroupItems").val())),
        PolicyCarVendorGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyCarVendorGroupItems").val())),
        PolicyCityGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyCityGroupItems").val())),
        PolicyCityGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyCityGroupItems").val())),
        PolicyCityGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyCityGroupItems").val())),
        PolicyCountryGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyCountryGroupItems").val())),
        PolicyCountryGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyCountryGroupItems").val())),
        PolicyCountryGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyCountryGroupItems").val())),
        PolicyHotelCapRateGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyHotelCapRateGroupItems").val())),
        PolicyHotelCapRateGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyHotelCapRateGroupItems").val())),
        PolicyHotelCapRateGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyHotelCapRateGroupItems").val())),
        PolicyHotelPropertyGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyHotelPropertyGroupItems").val())),
        PolicyHotelPropertyGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyHotelPropertyGroupItems").val())),
        PolicyHotelPropertyGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyHotelPropertyGroupItems").val())),
        PolicyHotelVendorGroupItemsAdded: $.parseJSON(escapeInput($("#AddedPolicyHotelVendorGroupItems").val())),
        PolicyHotelVendorGroupItemsAltered: $.parseJSON(escapeInput($("#AlteredPolicyHotelVendorGroupItems").val())),
        PolicyHotelVendorGroupItemsRemoved: $.parseJSON(escapeInput($("#RemovedPolicyHotelVendorGroupItems").val())),
        PolicySupplierDealCodesAdded: $.parseJSON(escapeInput($("#AddedPolicySupplierDealCodes").val())),
        PolicySupplierDealCodesAltered: $.parseJSON(escapeInput($("#AlteredPolicySupplierDealCodes").val())),
        PolicySupplierDealCodesRemoved: $.parseJSON(escapeInput($("#RemovedPolicySupplierDealCodes").val())),
        PolicySupplierServiceInformationsAdded: $.parseJSON(escapeInput($("#AddedPolicySupplierServiceInformations").val())),
        PolicySupplierServiceInformationsAltered: $.parseJSON(escapeInput($("#AlteredPolicySupplierServiceInformations").val())),
        PolicySupplierServiceInformationsRemoved: $.parseJSON(escapeInput($("#RemovedPolicySupplierServiceInformations").val())),
        ClientSubUnitTelephoniesAdded: $.parseJSON(escapeInput($("#AddedTelephonies").val())),
        ClientSubUnitTelephoniesRemoved: $.parseJSON(escapeInput($("#RemovedTelephonies").val())),
        PolicyGroup: $.parseJSON(escapeInput($("#PolicyGroup").val()))
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(clientPolicyChanges),
        url: '/ClientWizard.mvc/CommitPolicyChanges?id=' + Math.random(),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (!result.success) {
                showErrorDialog(result);
            } else {
                successCb();
            }
        },
        error: function (result) {
            showErrorDialog(result);
        }
    });
}


function SavePolicySupplierDealCodes() {

    /*setup*/
    var changedPolicySuppliers = 0;
    var deletedPolicySuppliers = 0;
    var addedPolicySuppliers = 0;
    var AlteredPolicySupplierDealCodesArray = [];
    var DeletedPolicySupplierDealCodesArray = [];
    var AddedPolicySupplierDealCodesArray = [];

    /*added*/
    $('#hiddenAddedPolicySupplierDealCodesTable tr').each(function () {

        var policySupplierDealCode = {
            PolicySupplierDealCodeId: $(this).attr('PolicySupplierDealCodeId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicySupplierDealCodeValue: $(this).attr("PolicySupplierDealCodeValue"),
            PolicySupplierDealCodeDescription: $(this).attr("PolicySupplierDealCodeDescription"),
            PolicySupplierDealCodeTypeId: $(this).attr("PolicySupplierDealCodeTypeId"),
            SupplierCode: $(this).attr("SupplierCode"),
            ProductId: $(this).attr("ProductId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            GDSCode: $(this).attr("GDSCode"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            VersionNumber: $(this).attr("VersionNumber")
        };
        AddedPolicySupplierDealCodesArray.push(policySupplierDealCode);
        addedPolicySuppliers++;
    });
    if (addedPolicySuppliers > 0) {
        $('#AddedPolicySupplierDealCodes').val(JSON.stringify(AddedPolicySupplierDealCodesArray))
    }

    /*altered*/
    $('#hiddenChangedPolicySupplierDealCodesTable tr').each(function () {
        var policySupplierDealCode = {
            PolicySupplierDealCodeId: $(this).attr('PolicySupplierDealCodeId'),
            PolicyGroupId: $(this).attr("PolicyGroupId"),
            PolicySupplierDealCodeValue: $(this).attr("PolicySupplierDealCodeValue"),
            PolicySupplierDealCodeDescription: $(this).attr("PolicySupplierDealCodeDescription"),
            PolicySupplierDealCodeTypeId: $(this).attr("PolicySupplierDealCodeTypeId"),
            SupplierCode: $(this).attr("SupplierCode"),
            ProductId: $(this).attr("ProductId"),
            PolicyLocationId: $(this).attr("PolicyLocationId"),
            GDSCode: $(this).attr("GDSCode"),
            EnabledFlag: $(this).attr("EnabledFlag"),
            EnabledDate: $(this).attr("EnabledDate"),
            ExpiryDate: $(this).attr("ExpiryDate"),
            VersionNumber: $(this).attr("VersionNumber")
        };
        AlteredPolicySupplierDealCodesArray.push(policySupplierDealCode);
        changedPolicySuppliers++;
    });
    if (changedPolicySuppliers > 0) {
        $('#AlteredPolicySupplierDealCodes').val(JSON.stringify(AlteredPolicySupplierDealCodesArray))
    }

    /*removed*/
    $('#hiddenRemovedPolicySupplierDealCodesTable tr').each(function () {
        var PolicySupplierDealCodeId = $(this).attr("PolicySupplierDealCodeId");
        var VersionNumber = $(this).attr("VersionNumber");
        DeletedPolicySupplierDealCodesArray.push({ PolicySupplierDealCodeId: PolicySupplierDealCodeId, VersionNumber: VersionNumber });
        deletedPolicySuppliers++;
    });
    if (deletedPolicySuppliers > 0) {
        $('#RemovedPolicySupplierDealCodes').val(JSON.stringify(DeletedPolicySupplierDealCodesArray))
    }

    SavePolicySupplierServiceInformations();
}

function hideInheritIfCustomCountryPolicyItemOnView(){
    $('#currentPolicyCountryGroupItemsTable tbody tr').each(function(){
	    //outer loop
		if($(this).attr("source")=="Custom") {
			if($(this).attr("InheritFlag")=="False"){
				var country1 = $(this).attr("id");
			    $('#currentPolicyCountryGroupItemsTable tbody tr').each(function(){
				    //inner loop
					if($(this).attr("source")!="Custom") {
						if($(this).attr("id")==country1){
							$(this).hide();	
						}
					}
		        });
	        //end of inherit = false	
			}
		//end of source = custom
		}
    });
}


