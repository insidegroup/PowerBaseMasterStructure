$(document).ready(function () {
    //prevent IE from cahcing
    $.ajaxSetup({ cache: false });

    $("#tabs").tabs({ disabled: [1, 2, 3, 4, 5, 6, 7] });
    ShowClientSelectionScreen("", "");
    $('#mainMenuButton').button();
    $("#dialog-confirm").hide();


    //set function calls for each tab			
    $('#selectAClientLink').click(function () {
        $("#tabs").tabs({ disabled: [1, 2, 3, 4, 5, 6, 7] });
        ClearHiddenFormVariables();
        $('#currentClient').html("");
        $('#clientSubUnitName').html("");
        ShowClientSelectionScreen();
    });

    $('#selectAClientConfirmationLink').click(function () {
        $("#tabs").tabs({ disabled: [2, 3, 4, 5, 6, 7] });
    });
    $('#clientTeamsLink').click(function () {
        $("#tabs").tabs({ disabled: [3, 4, 5, 6, 7] });
        ShowClientServicingTeamsScreen();
    });
    $('#clientsAccountsLink').click(function () {
        $("#tabs").tabs({ disabled: [4, 5, 6, 7] });
    });
    $('#clientServicingOptionsLink').click(function () {
        $("#tabs").tabs({ disabled: [5, 6, 7] });
        ShowClientServicingOptionsScreen();
    });
    $('#clientReasonCodesLink').click(function () {
        $("#tabs").tabs({ disabled: [6, 7] });
        ShowReasonCodesScreen(true);
    });
    $('#clientPolicyGroupsLink').click(function () {
        $("#tabs").tabs({ disabled: [7] });
    });
    //debugger;
    //cater for enter on search fields

  

   
    document.addEventListener('DOMContentLoaded', function (event) {
        //the event occurred
    });

    $(window).keydown(function (event) {
        if (!event) var event = window.event;
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

    $('#wizardMenu').change(function () {

        let activeTabs = $('.ui-tabs-nav').children().toArray().filter(function(t) { return $(t).hasClass("ui-state-active") === true });
        if (activeTabs.length > 1 || activeTabs.length == 0) {
            OnMenuChanged(this);
        } else {
            let activeTab = $(activeTabs[0]).text();

            switch (activeTab) {
                case "Find":
                    FindOnMenuChanged(this);
                    break;
                case "Client Details":
                    ClientDetailsOnMenuChanged(this);
                    break;
                case "Client Servicing Teams":
                    ClientServicingTeamsOnMenuChanged(this);
                    break;
                case "Client Accounts":
                    ClientAccountsOnMenuChanged(this);
                    break;
                case "Client Service Options":
                    ClientServiceOptionsOnMenuChanged(this);
                    break;
                case "Client Reason Codes":
                    ClientReasonCodesOnMenuChanged(this);
                    break;
                case "Policies":
                    ClientPoliciesOnMenuChanged(this);
                    break;
                default:
                    return;
            }
        }
        
    });

    $('#dialog-confirm').bgiframe();
});

function OnMenuChanged(menu) {
    var newSelection = escapeInput($(menu).val());
    var valid_locations = ["Home.mvc", "SystemUserWizard.mvc", "TeamWizard.mvc", "LocationWizard.mvc", "ClientWizard.mvc"];
    if (newSelection != 0 && valid_locations.indexOf(newSelection) != -1) {
        window.location = newSelection;
    }

    return false;
}

function ClientDetailsOnMenuChanged(menu) {
    var currentForm = $('#form0').serialize();
    if (originalForm != currentForm) {
        showSaveDialog(function () {
            ValidateClient();
            OnMenuChanged(menu);
        }, ShowClientDetailsScreen);
    }
    else {
        OnMenuChanged(menu);
    }
}

function FindOnMenuChanged(menu) {
    
        OnMenuChanged(menu);
    
}

function ClientServicingTeamsOnMenuChanged(menu) {
    if (hasPageBeenChanged("ClientTeams")) {
        showSaveDialog(function () {
            SaveClientTeams();
            CommitTeamsChanges(function () {
                OnMenuChanged(menu);
            });
        }, function () { ShowClientServicingTeamsScreen(); });
    } else {
        OnMenuChanged(menu);
    }
}

function ClientAccountsOnMenuChanged(menu) {
    if (hasPageBeenChanged("ClientAccounts")) {
        showSaveDialog(function () {
            SaveClientAccounts();
            CommitClientAccountChanges(function () {
                OnMenuChanged(menu);
            })
        },ShowClientAccountsScreen);
    } else {
        OnMenuChanged(menu);
    }
}

function ClientServiceOptionsOnMenuChanged(menu) {
    if (hasPageBeenChanged("ServicingOptions")) {
        showSaveDialog(function () {
            UpdateServiceOptionsSettings(serviceOptionChanges, serviceOptionChangedItems);
            SaveClientServicingOptions();
            CommitServiceOptionsChanges(function () {
                OnMenuChanged(menu);
            });
        },ShowClientServicingOptionsScreen);
    } else {
        OnMenuChanged(menu);
    }
}

function ClientReasonCodesOnMenuChanged(menu) {
    if (hasPageBeenChanged("ClientReasonCodes")) {
        showSaveDialog(function () {
            SaveClientReasonCodes();
            CommitClientReasonCodeChanges(function () {
                OnMenuChanged(menu);
            })
        }, function () { ShowReasonCodesScreen(true); });
    } else {
        OnMenuChanged(menu);
    }
}

function ClientPoliciesOnMenuChanged(menu) {
    if (hasPageBeenChanged("Policy")) {
        showSaveDialog(function () {
            SavePolicyAirParameterGroupItems();
            CommitClientPolicyChanges(function () {
                OnMenuChanged(menu);
            });
        }, ShowClientPoliciesScreen)
    } else {
        OnMenuChanged(menu);
    }
}



/*
*Shows Search Options
*/
function ShowClientSelectionScreen() {

    //clean up hidden tables
    ClearHiddenFormVariables();

    //Setup
    var url = '/ClientWizard.mvc/ClientSelectionScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-1Content").html("<p>" + ajaxLoading + " Loading...</p>");

    //AJAX POST of filter
    $.ajax({
        type: "POST",
        url: url,
        success: function (result) {
            var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 0);

            $('#currentClient').html("");
            $('#clientSubUnitName').html("");

            $("#tabs-1Content").html(result);
            $("#clientTable").tablesorter({ widgets: ['zebra'] });
            $("#tabs").tabs({ disabled: [1, 2, 3, 4, 5, 6, 7] });
            $('#lastSearchL').html("");




            $("#SearchButton").button();
            $("#CFilter").keyup(function (e) {
                if (e.keyCode == 13) {
                    $('#lastSearchL').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");
                    DoClientSearch();
                }
            });
            $("#SearchButton").click(function () {
                $('#lastSearchL').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");
                DoClientSearch();

            });
        }
    });
}
/*
Do Search
*/
function DoClientSearch() {

    var clientFilterField = $('#CFilterField').val();
    var clientFilter = $('#CFilter').val();

    $('#CFilter').removeClass("ui-state-active");

    if (clientFilterField == "ClientTopUnitName" || clientFilterField == "ClientTopUnitGuid") {

		if (clientFilter.length < 2) {

            $('#CFilter').addClass("ui-state-active");
            $('#CFilter').focus();
            
			//PCI = Enforce minimum length to reduce timeouts
            alert("Please supply at least 2 characters or more for a search on a top unit!");
            
            return;
        }

    }

    else {



        if (!clientFilter) {
            $('#CFilter').addClass("ui-state-active");
            $('#CFilter').focus();
            alert("Please supply something - at least 1 character - for the client filter value!");
            return;
        }

    }


    $("#lastSearchTS").html("<th colspan='5'><img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...</th>");
    var url = '/ClientWizard.mvc/ClientSearch';

    $.ajax({
        type: "POST",
        data: { filterField: $('#CFilterField').val(), filter: escapeInput($('#CFilter').val()) },
        url: url,
        timeout: 30000, //Add a timeout of 30 seconds
        success: function (result) {

            $('#ClientTopUnitGuid').val("");
            $('#ClientSubUnitGuid').val("");



            //now put some text in the TH fields according to the filter type


            //ClientTopUnit Search
            if (clientFilterField == "ClientTopUnitName" || clientFilterField == "ClientTopUnitGuid") {

                $("#ClientTopUnitSearchResults").html(result);
                $("#ClientTopUnitSearchResults").tablesorter({ widgets: ['zebra'] });
                $("#ClientSubUnitSearchResults").html("");

                //if there are any search results
                if ($('#clientTable')) {

                    $('#csTH1').html("ClientTopUnit Name");
                    $('#csTH2').html("ClientTopUnit GUID");
                }
                //ClientSubUnit Search
            } else {
                $("#ClientTopUnitSearchResults").html("");
                $("#ClientSubUnitSearchResults").html(result);
                $("#ClientSubUnitSearchResults").tablesorter({ widgets: ['zebra'] });

                $('#csTH1').html("ClientSubUnit Name");
                $('#csTH2').html("ClientSubUnit GUID");
                $('#csTH5').html("Country");

            }
            $("#clientTable").tablesorter({ widgets: ['zebra'] });

            $("#lastSearchTS").html("");
        },
        error: function (objAJAXRequest, errorMessage) {
        	if (errorMessage == "timeout") {
        		$("#ClientAccountSearchResults").html("Apologies but the search query timed out.");
        	} else {
        		$("#lastSearchTS").html("Apologies but an error occured.");
        	}
        }
    });
}

/*
* Clears Hidden Form and hidden Tables
*/
function ClearHiddenFormVariables() {
    $('#UnAvailableRoutingNames').val("");
    $('#source').val("");
    $('#groupCount').val("");

    $('#frmClientWizard input').each(function () { $(this).val(""); });

    $('#hiddenTables table').each(function () {
        $(this).find("tr").each(function () {
            $(this).remove();
        });
    });
}

/*
* Escape HTML characters for display of titles
*/
function escapeHTML(s) {

    s = s.replace(/&/g, '&amp;');
    s = s.replace(/</g, '&lt;');
    s = s.replace(/>/g, '&gt;');
    return s;
}

/*
* Set Wizard ClientTopUnit - chenge bottom list to a list of SubUnits for this TopUnit
*/
function SetWizardClientTopUnit(topunitName, clientTopUnitGuid) {

    $('#currentClient').html(escapeHTML((topunitName)));
    $('#ClientTopUnitSearchResults').html("<h4>Available client subunits:</h4>");

    ClearHiddenFormVariables();

    $('#ClientTopUnitGuid').val(clientTopUnitGuid);


    var url = '/ClientWizard.mvc/ListClientSubUnits';

    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#ClientSubUnitSearchResults").html("<p>" + ajaxLoading + " Loading...</p>");

    $.ajax({
        type: "POST",
        data: { clientTopUnitGuid: clientTopUnitGuid },
        url: url,
        success: function (result) {
            $("#ClientSubUnitSearchResults").html(result);
            $("#clientTable").tablesorter({ widgets: ['zebra'] });
        },
        error: function () {
            $("#ClientSubUnitSearchResults").html("<h4>Apologies but an error occured.</h4>");
        }
    });



    //  $('#tabs').tabs("enable", 1);
    //confirmClientFind();
}
/*
* Set Wizard ClientSubUnit
*/
function SetWizardClientSubUnit(clientSubUnitGuid) {


    var clientAlreadySelected = false;
    if ($('#ClientSubUnitGuid').val() != "") {
        clientAlreadySelected = true;
    }
    if (clientAlreadySelected) {
        if (!confirm("You have already selected an item, if you continue you will lose unsaved information. Click OK to continue or Cancel to return to existing Item")) {
            return;
        }
    }
    //Clear hidden variables except topUnit
    var clientTopUnitGuid = $('#ClientTopUnitGuid').val();
    ClearHiddenFormVariables();

    $('#ClientSubUnitGuid').val(clientSubUnitGuid);
    $('#ClientTopUnitGuid').val(clientTopUnitGuid);

    $('#tabs').tabs("enable", 1);
    ShowClientDetailsScreen();
}


/*
*Shows CLient Details Screen
*If a ClientSubUnit was selected, we show 1 TopUNit+ 1 SubUnit
*If a ClientTopUnit was selected, we show 1 TopUnit+ All SubUnits
*/
function ShowClientDetailsScreen() {

    
    $('#tabs').tabs('enable', 1);
    $('#tabs').tabs('select', 1);

    var url = '/ClientWizard.mvc/ClientDetailsScreen';

    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-2Content").html("<p>" + ajaxLoading + " Loading...</p>");

    $.ajax({
        type: "POST",
        data: { clientSubUnitGuid: $('#ClientSubUnitGuid').val() },
        url: url,
        success: function (result) {
            $("#tabs-2Content").html(result);

            //this is to put the top unit name in the banner if the search was done on subunit fields
            var topUnitName = escapeInput($('#topUnitName').val());

            $('#currentClient').html(topUnitName);

            //add again needs to be added again - if we started with a ClientSubUnit, ClientTopUnitGuid would be blank
            $('#ClientTopUnitGuid').val($('#TopUnitGuid').val());

            var clientSubUnitName = escapeInput($('#subUnitName').val());

            $('#clientSubUnitName').html(clientSubUnitName);


            $("#clientDetailsTable").tablesorter({ widgets: ['zebra'] });

        	//Feedbackify Tracking
            $('#Tracking_ClientTopUnitGUID').val(escapeInput($('#ClientTopUnitGuid').val()));
            $('#Tracking_ClientSubUnitGUID').val(escapeInput($('#ClientSubUnitGuid').val()));

            $('#clientDetailsCancelButton').button();
            $('#clientDetailsCancelButton').click(function () {
                ShowClientDetailsScreen();
            });

            $('#clientDetailsSaveButton').button();
            $('#clientDetailsSaveButton').click(function () {
                ValidateClient();
            });

            $('#clientDetailsBackButton').button();
            $('#clientDetailsBackButton').click(function () {
                var currentForm = $('#form0').serialize();
                if (originalForm != currentForm) {
                    showSaveDialog(function () {
                        $("#tabs").tabs({ disabled: [1, 2, 3, 4, 5, 6, 7] });
                        $('#currentClient').html("");
                        ValidateClient();
                        ShowClientSelectionScreen();
                    }, ShowClientDetailsScreen);
                }
                    else {
                    ShowClientSelectionScreen();
                }
            });

            $('#clientDetailsNextButton').button();
            $('#clientDetailsNextButton').click(function () {
                var currentForm = $('#form0').serialize();
                if (originalForm != currentForm) {
                    showSaveDialog(ValidateClient, ShowClientDetailsScreen);
                }
                else {
                    ValidateClient();
                }
            });

        },
        error: function () {
            $("#tabs-2Content").html("<h4>Apologies but an error occured.</h4>");
        }
    });
}

/*
*Validate Client - If successful will bring you to ClientServicing Teams
*/
function ValidateClient() {

	/*Reset Error Messages*/

	var ClientSubUnit_ClientSubUnitDisplayName_validationMessage = $('#ClientSubUnit_ClientSubUnitDisplayName_validationMessage');
	ClientSubUnit_ClientSubUnitDisplayName_validationMessage.text('').addClass('error').hide();

	var ClientSubUnit_LineOfBusinessId_validationMessage = $('#ClientSubUnit_LineOfBusinessId_validationMessage');
	ClientSubUnit_LineOfBusinessId_validationMessage

	var ClientSubUnit_BranchContactNumber_validationMessage = $('#ClientSubUnit_BranchContactNumber_validationMessage');
	ClientSubUnit_BranchContactNumber_validationMessage.text('').addClass('error').hide();

	var ClientSubUnit_BranchFaxNumber_validationMessage = $('#ClientSubUnit_BranchFaxNumber_validationMessage');
	ClientSubUnit_BranchFaxNumber_validationMessage.text('').addClass('error').hide();

	var ClientSubUnit_ClientIATA_validationMessage = $('#ClientSubUnit_ClientIATA_validationMessage');
	ClientSubUnit_ClientIATA_validationMessage.text('').addClass('error').hide();

    var ClientSubUnit_DialledNumber24HSC_validationMessage = $('#ClientSubUnit_DialledNumber24HSC_validationMessage');
    ClientSubUnit_DialledNumber24HSC_validationMessage.text('').addClass('error').hide();

    var ClientSubUnit_PortraitStatusDescription_validationMessage = $('#ClientSubUnit_PortraitStatusDescription_validationMessage');
    ClientSubUnit_PortraitStatusDescription_validationMessage.text('').addClass('error').hide();

    var ClientSubUnit_ClientBusinessDescription_validationMessage = $('#ClientSubUnit_ClientBusinessDescription_validationMessage');
    ClientSubUnit_ClientBusinessDescription_validationMessage.text('').addClass('error').hide();

	var ClientSubUnit_DialledNumber_validationMessage = $('#ClientSubUnit_DialledNumber_validationMessage');
	ClientSubUnit_DialledNumber_validationMessage.text('').addClass('error').hide();

	/* Check ClientSubUnitDisplayName is provided */
	var ClientSubUnit_ClientSubUnitDisplayName = $("#ClientSubUnit_ClientSubUnitDisplayName");
	if(ClientSubUnit_ClientSubUnitDisplayName.val() == '') {
		ClientSubUnit_ClientSubUnitDisplayName_validationMessage
			.text('Display Name required')
			.show()
		return false;
	}

	/* Check Line of Business is provided */
	var ClientSubUnit_LineOfBusinessId = $("#ClientSubUnit_LineOfBusinessId");
	if (ClientSubUnit_LineOfBusinessId.val() == '') {
		ClientSubUnit_LineOfBusinessId_validationMessage
			.text('Line of Business required')
			.show();
		return false;
	}

	/* Check Branch Contact Number is numeric with no spaces if entered */
	var ClientSubUnit_BranchContactNumber = $("#ClientSubUnit_BranchContactNumber").val();
	var ClientSubUnit_BranchContactNumber_Reg = /^\d{0,20}$/;
	if (ClientSubUnit_BranchContactNumber != '' && !ClientSubUnit_BranchContactNumber_Reg.test(ClientSubUnit_BranchContactNumber)) {
		ClientSubUnit_BranchContactNumber_validationMessage
			.text('Branch Contact Number must be numberical if entered (no spaces)')
			.show();
		return false;
	}

	/* Check Branch Fax Number is numeric with no spaces if entered */
	var ClientSubUnit_BranchFaxNumber = $("#ClientSubUnit_BranchFaxNumber").val();
	var ClientSubUnit_BranchFaxNumber_Reg = /^\d{0,20}$/;
	if (ClientSubUnit_BranchFaxNumber != '' && !ClientSubUnit_BranchFaxNumber_Reg.test(ClientSubUnit_BranchFaxNumber)) {
		ClientSubUnit_BranchFaxNumber_validationMessage
			.text('Branch Fax Number must be numberical if entered (no spaces)')
			.show();
		return false;
	}

	/* Check Client IATA is 8 characters if entered */
	var ClientSubUnit_ClientIATA = $("#ClientSubUnit_ClientIATA").val();
	var ClientSubUnit_ClientIATA_Reg = /^[0-9]{8}$/;
	if (ClientSubUnit_ClientIATA != '' && !ClientSubUnit_ClientIATA_Reg.test(ClientSubUnit_ClientIATA)) {
		ClientSubUnit_ClientIATA_validationMessage
			.text('Client IATA must be 8 numbers if entered')
			.show();
		return false;
	}

	/* Check 24HSC Dialled Number is numeric with no spaces if entered */
	var ClientSubUnit_DialledNumber24HSC = $("#ClientSubUnit_DialledNumber24HSC").val();
	var ClientSubUnit_DialledNumber24HSC_Reg = /^\d{0,20}$/;
	if (ClientSubUnit_DialledNumber24HSC != '' && !ClientSubUnit_DialledNumber24HSC_Reg.test(ClientSubUnit_DialledNumber24HSC)) {
		ClientSubUnit_DialledNumber24HSC_validationMessage
			.text('24HSC Dialled Number must be numberical if entered (no spaces)')
			.show();
		return false;
	}

    /* Check Dialled Number is numeric with no spaces if entered */
    var ClientSubUnit_DialledNumber = $("#DialledNumber").val();
    var ClientSubUnit_DialledNumber_Reg = /^\d{0,20}$/;
    if (ClientSubUnit_DialledNumber != '' && !ClientSubUnit_DialledNumber_Reg.test(ClientSubUnit_DialledNumber)) {
        ClientSubUnit_DialledNumber_validationMessage
            .text(' Dialled Number must be numberical if entered (no spaces)')
            .show();
        return false;
    }

    /* Check Portrait Status Description is valid */
    var ClientSubUnit_PortraitStatusDescription = $("#ClientSubUnit_PortraitStatusDescription").val();
    var ClientSubUnit_PortraitStatusDescription_Reg = /^([À-ÿ\w\s*-_.()]+)$/;
    if (ClientSubUnit_PortraitStatusDescription != '' && !ClientSubUnit_PortraitStatusDescription_Reg.test(ClientSubUnit_PortraitStatusDescription)) {
        ClientSubUnit_PortraitStatusDescription_validationMessage
            .text(' Special character not allowed for Portrait Status Description')
            .show();
        return false;
    }

    /* Check Client Business Description is valid */
    var ClientSubUnit_ClientBusinessDescription = $("#ClientSubUnit_ClientBusinessDescription").val();
    var ClientSubUnit_ClientBusinessDescription_Reg = /^([À-ÿ\w\s\&\*\\\:\,\#\/.\'\‘\-\_()]+)$/;
    if (ClientSubUnit_ClientBusinessDescription != '' && !ClientSubUnit_ClientBusinessDescription_Reg.test(ClientSubUnit_ClientBusinessDescription)) {
        ClientSubUnit_ClientBusinessDescription_validationMessage
            .text(' Special character not allowed for Client Business Description')
            .show();
        return false;
    }

	/*Update PolicyGroup fields for later use*/
    var policyGroup = {
        PolicyGroupId: escapeInput($("#PolicyGroup_PolicyGroupId").val()),
        VersionNumber: escapeInput($("#PolicyGroup_VersionNumber").val())
    };
    $("#PolicyGroup").val(JSON.stringify(policyGroup));


    var url = '/ClientWizard.mvc/ValidateClient?id=' + Math.random();

    //Build Object to Store ClientSubUnit
    var clientSubUnit = {
        ClientSubUnitGuid: escapeInput($("#ClientSubUnitGuid").val()),
        ClientSubUnitDisplayName: escapeInput(ClientSubUnit_ClientSubUnitDisplayName.val()),
        RestrictedClient: $('#ClientSubUnit_RestrictedClient').is(":checked"),
        PrivateClient: $('#ClientSubUnit_PrivateClient').is(":checked"),
        CubaBookingAllowed: $('#ClientSubUnit_CubaBookingAllowed').is(":checked"),
        InCountryServiceOnly: $('#ClientSubUnit_InCountryServiceOnly').is(":checked"),
        DialledNumber24HSC: escapeInput($('#ClientSubUnit_DialledNumber24HSC').val()),
        VersionNumber: escapeInput($("#ClientSubUnit_VersionNumber").val()),
        PortraitStatusId: escapeInput($("#ClientSubUnit_PortraitStatusId").val()),
        PortraitStatusDescription: escapeInput($("#ClientSubUnit_PortraitStatusDescription").val()),
        ClientBusinessDescription: escapeInput($("#ClientSubUnit_ClientBusinessDescription").val()),
        LineOfBusinessId: escapeInput($("#ClientSubUnit_LineOfBusinessId").val()),
        BranchContactNumber: escapeInput($("#ClientSubUnit_BranchContactNumber").val()),
        BranchFaxNumber: escapeInput($("#ClientSubUnit_BranchFaxNumber").val()),
        BranchEmail: escapeInput($("#ClientSubUnit_BranchEmail").val()),
        ClientIATA: escapeInput($("#ClientSubUnit_ClientIATA").val())
    };
    $("#ClientSubUnit").val(JSON.stringify(clientSubUnit));

    //Build Object to Store ClientTopUnit
    var clientTopUnit = {
        ClientTopUnitGuid: escapeInput($("#ClientTopUnitGuid").val()),
        //ClientTopUnitDisplayName: escapeInput($("#ClientTopUnit_ClientSubUnitDisplayName").val()),
        VersionNumber: escapeInput($("#ClientTopUnit_VersionNumber").val()),
        //PortraitStatusId: escapeInput($("#ClientTopUnit_PortraitStatusId").val())
    };
    $("#ClientTopUnit").val(JSON.stringify(clientTopUnit));

    //Build Object to Store Client
    var client = {
        ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
        ClientTopUnit: $.parseJSON(escapeInput($("#ClientTopUnit").val()))
    }

    //AJAX (JSON) POST of Client Object to validate 
    $.ajax({
        type: "POST",
        data: JSON.stringify(client),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (!result.success) {
                $("#tabs-2Content").html(result.message);
            } else {

                CommitClientChanges(result, JSON.stringify(client));
            }
        },
        error: function () {

            $('#tabs').tabs("enable", 3);
            $('#tabs').tabs('select', 3); // switch to 3rd tab
            $("#tabs-4Content").html("There was an error.");
        }
    });
}

function showErrorDialog(result) {
    // show errors
    let errorText = "";
    for (let i = 0; i < result.errorMessages.Messages.length; i++)
        errorText += result.errorMessages.Messages[i].message + '\n';

    $('#error-list').text(errorText);

    $("#dialog-show-errors").dialog({
        resizable: true,
        modal: true,
        height: 200,
        width: 300,
        title: "Error List",
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        }
    }, 'open');
}

function CommitClientChanges(validationResult, clientData) {

    $.ajax({
        type: "POST",
        data: clientData,
        url: '/ClientWizard.mvc/CommitClientDetails?id=' + Math.random(),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            if (!result.success) {
                showErrorDialog(result);
            } else {
                ShowClientServicingTeamsScreen(validationResult);
            }
        },
        error: function () {

            $('#tabs').tabs("enable", 3);
            $('#tabs').tabs('select', 3); // switch to 3rd tab
            $("#tabs-4Content").html("There was an error.");
        }
    });
}

let clientServicingTeamResult = null;

function ShowClientServicingTeamsScreen(result) {

    if (typeof result === 'undefined') {
        result = clientServicingTeamResult;
    } else {
        clientServicingTeamResult = result;
    }

    $("#hiddenAddedClientTeamsTable").empty();
    $("#hiddenAlteredClientTeamsTable").empty();
    $("#hiddenRemovedClientTeamsTable").empty();
    //Added and Removed Telephonies
    var addedTelphonies = [];
    $('#hiddenAddedTelephoniesTable tr').each(function () {
        var dialledNumber = $(this).attr("dialledNumber");
        var CallerEnteredDigitDefinitionTypeId = $(this).attr("CallerEnteredDigitDefinitionTypeId");
        addedTelphonies.push({ DialedNumber: dialledNumber, CallerEnteredDigitDefinitionTypeId: CallerEnteredDigitDefinitionTypeId });
    });
    $('#AddedTelephonies').val(JSON.stringify(addedTelphonies))

    var removedTelephonies = [];
    $('#hiddenRemovedTelephoniesTable tr').each(function () {

        var dialledNumber = $(this).attr("dialledNumber");
        var CallerEnteredDigitDefinitionTypeId = $(this).attr("CallerEnteredDigitDefinitionTypeId");
        var versionNumber = $(this).attr("versionNumber");
        removedTelephonies.push({ DialedNumber: dialledNumber, CallerEnteredDigitDefinitionTypeId: CallerEnteredDigitDefinitionTypeId, VersionNumber: versionNumber });
    });
    $('#RemovedTelephonies').val(JSON.stringify(removedTelephonies))


    $('#tabs').tabs("enable", 2);
    $('#tabs').tabs('select', 2); // switch to 4th tab
    $("#tabs-3Content").html(result.html);

    //Update IsPrimaryTeamForSub Flag
    $("#currentTeamsTable .UpdateIsPrimaryTeamForSub").bind("click", UpdateIsPrimaryTeamForSub);

    //Remove
    $("#currentTeamsTable img").click(function () {

        var team = $(this)
        var teamStatus = $(this).parent().parent().attr("teamStatus");
        var teamID = $(this).parent().parent().attr("id");
        var isPrimaryTeamForSub = team.parent().parent().attr("isPrimaryTeamForSub");

        if (teamStatus == "Current") {
            var versionNumber = $(this).parent().parent().attr("versionNumber");
            $('#hiddenRemovedClientTeamsTable').append("<tr teamID='" + teamID + "' isPrimaryTeamForSub='" + isPrimaryTeamForSub + "' versionNumber='" + versionNumber + "'></tr>");
        }
        else {
            $('#hiddenAddedClientTeamsTable tr').each(function () {
                var teamID2 = $(this).attr("teamID");
                if (teamID2 == teamID) {
                    $(this).remove();
                }
            });
        }

        team.parent().parent().remove();

        //Remove any altered teams with same ID
        $('#hiddenAlteredClientTeamsTable tr').each(function () {
            var teamID2 = $(this).attr("teamID");
            if (teamID2 == teamID) {
                $(this).remove();
            }
        });
    });

    //buttons
    $('#clientTeamsBackButton').button();
    $('#clientTeamsBackButton').click(function () {
        if (hasPageBeenChanged("ClientTeams")) {
            showSaveDialog(function () {
                $('#tabs').tabs("select", 1);
                $('#tabs').tabs("disable", 2);

                SaveClientTeams();
                CommitTeamsChanges(function () {
                    ShowClientDetailsScreen();
                });
            }, ShowClientServicingTeamsScreen);
        } else {
            ShowClientDetailsScreen();
        }
    });

    $('#clientTeamsNextButton').button();
    $('#clientTeamsNextButton').click(function () {
        if (hasPageBeenChanged("ClientTeams")) {
            showSaveDialog(function () {
                SaveClientTeams();
                CommitTeamsChanges(function () {
                    if (!$('#IsPrimaryTeamForSubError').is(':visible')) {
                        ShowClientAccountsScreen();
                    }
                    
                });
            }, ShowClientServicingTeamsScreen);
        } else {
            ShowClientAccountsScreen();
        }
    });
    $('#clientTeamsSearchButton').button();
    $('#clientTeamsSearchButton').click(function () {
        DoTeamSearch();
    });

    $('#clientTeamsSaveButton').button();
    $('#clientTeamsSaveButton').click(function () {
        SaveClientTeams();
        CommitTeamsChanges(function () {
            if (!$('#IsPrimaryTeamForSubError').is(':visible')) {
                ShowClientAccountsScreen();
            }
        });
    });

    $('#clientTeamsCancelButton').button();
    $('#clientTeamsCancelButton').click(function () {
        ShowClientServicingTeamsScreen();
    });
}

function hasPageBeenChanged(pageName) {

    let tables = $('#hiddenTables').children().toArray().filter(function (t) { return $(t).attr('id').indexOf(pageName) !== -1 });

    let changes = false;

    tables.forEach(function(t) {
        let tableId = $(t).attr('id');
        let trFields = $('#' + tableId + ' tr').toArray();

        if (trFields.length > 0)
            changes = true;
    });

    return changes;
}

function showSaveDialog(okCallback, cancelCallback) {
    $("#dialog-save-changes").dialog({
        resizable: true,
        modal: true,
        height: 200,
        width: 300,
        title: "Save Changes",
        buttons: {
            "OK": function () {
                okCallback();
                $(this).dialog("close");
            },
            Close: function () {
                cancelCallback();
                $(this).dialog("close");
            }
        }
    }, 'open');
}

function CommitTeamsChanges(successCb) {

    var teamsChanges = {
        ClientTopUnit: $.parseJSON(escapeInput($("#ClientTopUnit").val())),
        ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
        TeamsAdded: $.parseJSON(escapeInput($("#AddedTeams").val())),
        TeamsRemoved: $.parseJSON(escapeInput($("#RemovedTeams").val())),
        TeamsAltered: $.parseJSON(escapeInput($("#AlteredTeams").val()))//clear pop-up form
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(teamsChanges),
        url: '/ClientWizard.mvc/CommitTeamsChanges?id=' + Math.random(),
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

function UpdateIsPrimaryTeamForSub(e) {

	//e.preventDefault();

	var checkbox = $(this);
	var team = $(this).parent().parent();
	var teamID = team.attr("id");
	var isPrimaryTeamForSub = checkbox.is(':checked') ? "true" : "false";
	var teamstatus = team.attr("teamstatus");
	var originalIsPrimaryTeamForSub = team.attr("isPrimaryTeamForSub");
	var includeInClientDroplistFlag = team.attr("includeInClientDroplistFlag");
	var versionNumber = team.attr("versionNumber");

	//Remove any duplicate rows with same ID
	$('#hiddenAlteredClientTeamsTable tr').each(function () {
		var duplicate_TeamID = $(this).attr("teamID");
		if (duplicate_TeamID == teamID) {
			$(this).remove();
		}
	});

	//Add Primary Team to Alter Table
	if (teamstatus == "Current") {
		$('#hiddenAlteredClientTeamsTable').append("<tr teamID='" + teamID + "' originalIsPrimaryTeamForSub='" + originalIsPrimaryTeamForSub + "' isPrimaryTeamForSub='" + isPrimaryTeamForSub + "' includeInClientDroplistFlag='" + includeInClientDroplistFlag + "' versionNumber='" + versionNumber + "'></tr>");
	} 

	//Update isPrimaryTeamForSub to newly added items
	$('#hiddenAddedClientTeamsTable tr').each(function () {
		var added_TeamID = $(this).attr("teamID");
		if (added_TeamID == teamID) {
			$(this).attr('isPrimaryTeamForSub', isPrimaryTeamForSub);
		} else {
			$(this).attr('isPrimaryTeamForSub', 'false');
		}
	});

	//Untick previously Primary team
	$(".UpdateIsPrimaryTeamForSub:checked").each(function () {

		var existingTeam = $(this).parent().parent();
		var existingTeam_ID = existingTeam.attr("id");
		var existingTeam_OriginalIsPrimaryTeamForSub = existingTeam.attr("isPrimaryTeamForSub");
		var existingTeam_IsPrimaryTeamForSub = "false";
		var existingTeam_VersionNumber = existingTeam.attr("versionNumber");
		var existingTeam_IncludeInClientDroplistFlag = existingTeam.attr("includeInClientDroplistFlag");
		if (existingTeam_ID != teamID) {
			$(this).attr('checked', false);
			$('#hiddenAlteredClientTeamsTable').append("<tr teamID='" + existingTeam_ID + "' originalIsPrimaryTeamForSub='" + existingTeam_OriginalIsPrimaryTeamForSub + "' isPrimaryTeamForSub='" + existingTeam_IsPrimaryTeamForSub + "' includeInClientDroplistFlag='" + existingTeam_IncludeInClientDroplistFlag + "' versionNumber='" + existingTeam_VersionNumber + "'></tr>");
		}
	});

	//Remove teams set back to default values
	$('#hiddenAlteredClientTeamsTable tr').each(function () {
		var revertedTeam = $(this);
		var revertedTeam_ID = revertedTeam.attr("teamID");
		var revertedTeam_OriginalIsPrimaryTeamForSub = revertedTeam.attr("OriginalIsPrimaryTeamForSub");
		var revertedTeam_IsPrimaryTeamForSub = revertedTeam.attr("isPrimaryTeamForSub");
		if ((revertedTeam_ID != teamID && revertedTeam_OriginalIsPrimaryTeamForSub == "false" && revertedTeam_IsPrimaryTeamForSub == "true") ||
			revertedTeam_OriginalIsPrimaryTeamForSub == revertedTeam_IsPrimaryTeamForSub) {
			$(this).remove();
		}
	});           	
}

/*
Search for Teams
*/
function DoTeamSearch() {


    $("#lastSearchTRTeam").html("<th colspan='5'><img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...</th>");
    var url = '/ClientWizard.mvc/TeamSearch?id=' + Math.random();
    $.ajax({
        type: "POST",
        data: { filterField: $('#TeamFilterField').val(), filter: escapeInput($('#TeamFilter').val()) },
        url: url,
        success: function (result) {
            $("#lastSearchTRTeam").html("");
            $("#TeamSearchResults").html(result);
            $("#teamUsersSearchResultTable").tablesorter({ widgets: ['zebra'] });
            var teamAlreadyInList = 0;
            $("#teamUsersSearchResultTable img").click(function () {
                teamAlreadyInList = 0;
                $('#noTeams').remove();

                var teamName = $(this).parent().parent().attr("teamName");
                var teamEmail = $(this).parent().parent().attr("teamEmail");
                var teamID = $(this).parent().parent().attr("id");
                var teamPhone = $(this).parent().parent().attr("teamPhone");

                $('#currentTeamsTable tbody tr').each(function () {

                    //check if it's there already, don't allow

                    var currentTeamID = $(this).attr("id");

                    if (teamID == currentTeamID) {

                        alert("Team already in list, no action taken.");
                        teamAlreadyInList = 1;
                        return;
                    }
                });

                if (teamAlreadyInList == 0) {

                	$('#currentTeamsTable').append("<tr teamStatus='notCurrent' id='" + teamID + "' isprimaryteamforsub='false' versionNumber='1' includeInClientDroplistFlag='false'><td>" + escapeHTML(teamName) + "</td><td>" + escapeHTML(teamEmail) + "</td><td>" + escapeHTML(teamPhone) + "</td><td><input type='checkbox' class='UpdateIsPrimaryTeamForSub' value='true'></td><td><img src='../../images/remove.png' border='0' alt='remove'/></td></tr>");
                    $('#hiddenAddedClientTeamsTable').append("<tr teamID='" + teamID + "' IncludeInClientDroplistFlag='true'></tr>");

                    $("#currentTeamsTable").tablesorter({
                        headers: {

                            0: {
                                // disable it by setting the property sorter to false
                                sorter: false
                            },
                            1: {
                                // disable it by setting the property sorter to false
                                sorter: false
                            },
                            2: {
                                // disable it by setting the property sorter to false
                                sorter: false
                            }

                        },

                        widgets: ['zebra']
                    });


                    $(this).parent().parent().remove();

                	//Update IsPrimaryTeamForSub Flag
                    $("#currentTeamsTable .UpdateIsPrimaryTeamForSub").bind("click",UpdateIsPrimaryTeamForSub);

                	//Remove
                    $("#currentTeamsTable img").click(function () {

                        var team = $(this)

                        var teamStatus = $(this).parent().parent().attr("teamStatus");
                        var teamID = $(this).parent().parent().attr("id");



                        if (teamStatus == "Current") {

                            $('#hiddenRemovedClientTeamsTable').append("<tr teamID='" + teamID + "'></tr>");


                        }

                        $('#hiddenAddedClientTeamsTable tr').each(function () {

                            if (teamID == $(this).attr("teamID")) {

                                $(this).remove();

                            }

                        });

                        $('#hiddenAddedClientTeamsTable tr').each(function () {

                            var teamID2 = $(this).attr("teamID");

                            if (teamID2 == teamID) {
                                $(this).remove();
                            }
                        });



                        team.parent().parent().remove();


                    });
                }

            });


        },
        error: function () {
            var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 3); // switch to 1st tab

            $("#lastSearchTR").html("Sorry, but an error occured with the search for available Teams.");
        }




    });



}




/*
Save Teams to Hidden Inputs
*/
function SaveClientTeams() {

	//If there is only one team associated to a client SubUnit and the user moves to the Next tab, 
	//then the system shall check the Primary checkbox behind the scenes.  
	if ($(".UpdateIsPrimaryTeamForSub").length == 1) {
		$('#currentTeamsTable tbody tr').each(function () {

			var team = $(this)
			var teamID = team.attr("id");
			var versionNumber = team.attr("versionNumber");
			var checkbox = $(this).find('.UpdateIsPrimaryTeamForSub');
			var includeInClientDroplistFlag = true;

			var counter = 0;

			if (!checkbox.is(':checked')) {

				checkbox.attr('checked', true);
		
				//Newly added team
				$('#hiddenAddedClientTeamsTable tr').each(function () {
					var teamID2 = $(this).attr("teamID");
					if (teamID2 == teamID) {
						$(this).attr('isprimaryteamforsub', 'true');
						counter++;
					}
				});
				
				//Existing team
				if (counter == 0) {
					$('#hiddenAlteredClientTeamsTable tr').each(function () {
						var teamID2 = $(this).attr("teamID");
						if (teamID2 == teamID) {
							$(this).remove();
						}
					});

					$('#hiddenAlteredClientTeamsTable').append("<tr teamID='" + teamID + "' isPrimaryTeamForSub='true' includeInClientDroplistFlag='" + includeInClientDroplistFlag + "' versionNumber='" + versionNumber + "'></tr>");
				}
			}
		});
	}

	//If a user attempts to move to the next tab and one of the Teams is not checked as Primary
	//then the user will get an error message that one of the Teams needs to be made mandatory
	$('#IsPrimaryTeamForSubError').hide();
	var availableTeams = $('.UpdateIsPrimaryTeamForSub').length;
	var checkedTeams = $(".UpdateIsPrimaryTeamForSub:checked").length;
	if (checkedTeams == 0 && availableTeams != 0) {
		$('#IsPrimaryTeamForSubError').show();
		return false;
	}

    //CODE TO ADD TEAMS TO HIDDEN FIELDS HERE

    var addedTeams = [];

    $('#hiddenAddedClientTeamsTable tr').each(function () {

    	var teamID = $(this).attr("teamID");
    	var isPrimaryTeamForSub = $(this).attr("IsPrimaryTeamForSub");
    	var includeInClientDroplistFlag = $(this).attr("includeInClientDroplistFlag");

    	addedTeams.push({ TeamId: teamID, IncludeInClientDroplistFlag: includeInClientDroplistFlag, IsPrimaryTeamForSub: isPrimaryTeamForSub });

    });

    $('#AddedTeams').val(JSON.stringify(addedTeams))


    var removedTeams = [];

    $('#hiddenRemovedClientTeamsTable tr').each(function () {

    	var teamID = $(this).attr("teamID");
    	var versionNumber = $(this).attr("versionNumber");
    	var isPrimaryTeamForSub = $(this).attr("IsPrimaryTeamForSub");
    	var includeInClientDroplistFlag = $(this).attr("includeInClientDroplistFlag");

    	removedTeams.push({ TeamId: teamID, IsPrimaryTeamForSub: isPrimaryTeamForSub, IncludeInClientDroplistFlag: includeInClientDroplistFlag, VersionNumber: versionNumber });

    });

    $('#RemovedTeams').val(JSON.stringify(removedTeams))


    var alteredTeams = [];

    $('#hiddenAlteredClientTeamsTable tr').each(function () {

    	var teamID = $(this).attr("teamID");
    	var versionNumber = $(this).attr("versionNumber");
    	var isPrimaryTeamForSub = $(this).attr("IsPrimaryTeamForSub");
    	var includeInClientDroplistFlag = $(this).attr("IncludeInClientDroplistFlag");

    	alteredTeams.push({ TeamId: teamID, IsPrimaryTeamForSub: isPrimaryTeamForSub, IncludeInClientDroplistFlag: includeInClientDroplistFlag, VersionNumber: versionNumber });

    });

    $('#AlteredTeams').val(JSON.stringify(alteredTeams))

}
/*
Show Client Accounts
*/
function ShowClientAccountsScreen() {

    $("#hiddenAddedClientAccountsTable").empty();
    $("#hiddenRemovedClientAccountsTable").empty();

    var url = '/ClientWizard.mvc/ClientAccountsScreen?id=' + Math.random();
    $.ajax({
        type: "POST",
        data: { clientSubUnitGuid: $('#ClientSubUnitGuid').val() },
        url: url,
        error: function () {
            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 3);
            $tabs.tabs('select', 3); // switch to 4th tab
            $("#tabs-4Content").html("Apologies but an error occured.");
        },
        success: function (result) {

            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 3);
            $tabs.tabs('select', 3); // switch to 5th tab
            $("#tabs-4Content").html(result);

            $("#currentAccountsTable").tablesorter({
                headers: {

                    0: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
                    1: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
                    2: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
                    3: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
                    4: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    }

                },

                widgets: ['zebra']
            });






            //buttons
            $('#clientAccountsBackButton').button();
            $('#clientAccountsBackButton').click(function () {
                if (hasPageBeenChanged("ClientAccounts")) {
                    showSaveDialog(function () {
                        SaveClientAccounts();
                        CommitClientAccountChanges(function () {
                            $('#tabs').tabs("select", 2);
                        })
                    }, ShowClientAccountsScreen);
                } else {
                    $('#tabs').tabs("select", 2);
                }
            });
            $('#clientAccountsNextButton').button();
            $('#clientAccountsNextButton').click(function () {

                if (hasPageBeenChanged("ClientAccounts")) {
                    showSaveDialog(function () {
                        SaveClientAccounts();
                        CommitClientAccountChanges(function () {

                            ShowClientServicingOptionsScreen();
                        })
                    }, function () { });
                } else {
                    ShowClientServicingOptionsScreen();
                }
            });

            $('#clientAccountsSearchButton').button();
            $('#clientAccountsSearchButton').click(function () {
                DoClientAccountSearch();
            });

            $('#clientAccountsSaveButton').button();
            $('#clientAccountsSaveButton').click(function () {
                showSaveDialog(function () {
                    SaveClientAccounts();
                    CommitClientAccountChanges(function () {
                        ShowClientServicingOptionsScreen();
                    })
                }, function () { });
            });

            $('#clientAccountsCancelButton').button();
            $('#clientAccountsCancelButton').click(function () {
                ShowClientAccountsScreen();
            });

            $('#currentAccountsTable img').click(function () {
                if ($(this).parent().parent().attr("accountStatus") == "Current") {

                    var clientAccount = $(this).parent().parent().attr("id");
                    var SSC = $(this).parent().parent().attr("SSC");

                    var versionNumber = $(this).parent().parent().attr("versionNumber");

                    $('#hiddenRemovedClientAccountsTable').append("<tr clientAccount='" + clientAccount + "' SSC='" + SSC + "' versionNumber='" + versionNumber + "'></tr>");

                } else {
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

function CommitClientAccountChanges(successCb) {

    //Build Object to Store All Items
    var clientAccountChanges = {
        ClientTopUnit: $.parseJSON(escapeInput($("#ClientTopUnit").val())),
        ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
        ClientAccountsAdded: $.parseJSON(escapeInput($("#AddedClientAccounts").val())),
        ClientAccountsRemoved: $.parseJSON(escapeInput($("#RemovedClientAccounts").val()))
    }

    $.ajax({
        type: "POST",
        data: JSON.stringify(clientAccountChanges),
        url: '/ClientWizard.mvc/CommitClientAccountsChanges?id=' + Math.random(),
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


/*
Search ClientAccounts
*/
function DoClientAccountSearch() {

	$("#lastSearchAccount").html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");

    var url = '/ClientWizard.mvc/ClientAccountSearch?id=' + Math.random();

    var filterField1, filterField2, filterField3;
    if ($('#ClientAccountFilter1').val() != "") {
        if ($('#ClientAccountFilterField1').val() != "PleaseSelect") {
        	filterField1 = $('#ClientAccountFilterField1').val();
        } else {
            alert("Please select a filter field for your first filter!");
            $('#lastSearchAccount').html("");
            return;
        }
    } else {
        alert("Please provide at least one filter");
        $('#lastSearchAccount').html("");
        return;
    }

    if ($('#ClientAccountFilter2').val() != "") {
        if ($('#ClientAccountFilterField2').val() != "PleaseSelect") {
        	filterField2 = $('#ClientAccountFilterField2').val();
        } else {
            alert("Please select a filter field for your second filter!");
            $('#lastSearchAccount').html("");
            return;
        }
    }
    if ($('#ClientAccountFilter3').val() != "") {
        if ($('#ClientAccountFilterField3').val() != "PleaseSelect") {
        	filterField3 = $('#ClientAccountFilterField3').val();
        } else {
            alert("Please select a filter field for your third filter!");
            $('#lastSearchAccount').html("");
            return;
        }
    }

    var filterField1Value, filterField2Value, filterField3Value;

    filterField1Value = escapeInput($('#ClientAccountFilter1').val());
    filterField2Value = escapeInput($('#ClientAccountFilter2').val());
    filterField3Value = escapeInput($('#ClientAccountFilter3').val());

	//PCI = Enforce minimum length to reduce timeouts
	if ((filterField1Value != "" && filterField1Value.length < 2) ||
		(filterField2Value != "" && filterField2Value.length < 2) ||
		(filterField3Value != "" && filterField3Value.length < 2)) {
    	alert("Please ensure a minimum of 2 characters per filter!");
    	$('#lastSearchAccount').html("");
    	return;
    }

    $.ajax({
        type: "POST",
        data: { 
        	filter1: filterField1Value, 
        	filterField1: filterField1, 

        	filter2: filterField2Value,
        	filterField2: filterField2, 

        	filter3: filterField3Value,
        	filterField3: filterField3
        },
        url: url,
        timeout: 30000, //Add a timeout of 30 seconds
		error: function (objAJAXRequest, errorMessage) {
			if (errorMessage == "timeout") {
				$("#ClientAccountSearchResults").html("Apologies but the search query timed out.");
			} else {
				$("#ClientAccountSearchResults").html("Apologies but an error occurred");
			}
            $('#lastSearchAccount').html("");
        },
        success: function (result) {

            $("#ClientAccountSearchResults").html(result);
            $("#searchResultClientTable").tablesorter({ widgets: ['zebra'] });
            $('#lastSearchAccount').html("");

            $('#searchResultClientTable img').click(function () {

                var currentAccount = 0;
                var accountNum = $(this).parent().parent().attr("id");
                var combinedAccSSC = $(this).parent().parent().attr("combinedAccSSC");
                var accountCountry = $(this).parent().parent().attr("country");
                var sourceSystemCode = $(this).parent().parent().attr("sourceSystemCode");
                var accountName=$(this).parent().parent().attr("accountName");
                var defaultFlag = "False"; //Unable to get until link with CSU setup

                $('#currentAccountsTable tbody tr').each(function () {
                    var currentItemAccountSSCNumber = $(this).attr("combinedAccSSC");
                    if (combinedAccSSC == currentItemAccountSSCNumber) {
                        alert("Account already in current list.");
                        currentAccount = 1;
                    }
                });
                if (currentAccount == 0) {
                    $('#currentAccountsTable').append("<tr accountStatus='NotCurrent' id='" + accountNum + "' combinedAccSSC='" + accountNum + sourceSystemCode + "' country='" + accountCountry + "' sourceSystemCode='" + sourceSystemCode + "' defaultFlag='" + defaultFlag + "'><td>" + accountName + "</td><td>" + accountNum + "</td><td>" + sourceSystemCode + "</td><td>" + accountCountry + "</td><td>" + defaultFlag + "</td> <td><img src='../../images/remove.png' /></td></tr>");
                    $('#hiddenAddedClientAccountsTable').append("<tr clientAccount='" + accountNum + "' SSC='" + sourceSystemCode + "'></tr>");
                }

                $("#currentAccountsTable").tablesorter({
                    headers: {
                        0: {
                            // disable it by setting the property sorter to false
                            sorter: false
                        },
                        1: {
                            // disable it by setting the property sorter to false
                            sorter: false
                        },
                        2: {
                            // disable it by setting the property sorter to false
                            sorter: false
                        },
                        3: {
                            // disable it by setting the property sorter to false
                            sorter: false
                        },
                        4: {
                            // disable it by setting the property sorter to false
                            sorter: false
                        }
                    },
                    widgets: ['zebra']
                });

                $('#currentAccountsTable img').click(function () {

                    if ($(this).parent().parent().attr("accountStatus") == "Current") {

                        //current, therefore add to removedAccounts hidden table

                        var clientAccount = $(this).parent().parent().attr("id");
                        var SSC = $(this).parent().parent().attr("SSC");
                        var versionNumber = $(this).parent().parent().attr("versionNumber");
                        $('#hiddenRemovedClientAccountsTable').append("<tr clientAccount='" + clientAccount + "' SSC='" + SSC + "' versionNumber='" + versionNumber + "'></tr>");

                    } else {
                        //it's not current so remove from addedAccounts hidden table
                        var clientAccount = $(this).parent().parent().attr("id");
                        var SSC = $(this).parent().parent().attr("sourcesystemcode");
                        $('#hiddenAddedClientAccountsTable tr').each(function () {

                            var clientAccount2 = $(this).attr("clientAccount");
                            var SSC2 = $(this).attr("SSC");
                            if ((clientAccount2 == clientAccount) && (SSC2 == SSC)) {
                                $(this).remove();
                            }
                        });

                    }
                    $(this).parent().parent().remove();
                });

            });
        }
    });
}

/*
Save ClientAccounts to Hidden Inputs
*/
function SaveClientAccounts() {

    //added client accounts
    $('#AddedClientAccounts').val("");
    var addedClientAccounts = [];
    $('#hiddenAddedClientAccountsTable tr').each(function () {
        var SourceSystemCode = $(this).attr("SSC");
        var ClientAccountNumber = $(this).attr("clientAccount");
        var ConfidenceLevelForLoadId = '0';
        addedClientAccounts.push({ SourceSystemCode: SourceSystemCode, ClientAccountNumber: ClientAccountNumber, ConfidenceLevelForLoadId: ConfidenceLevelForLoadId })

    });
    $('#AddedClientAccounts').val(JSON.stringify(addedClientAccounts));

    //removed client accounts
    $('#RemovedClientAccounts').val("");
    var removedClientAccounts = [];
    $('#hiddenRemovedClientAccountsTable tr').each(function () {
        var SourceSystemCode = $(this).attr("SSC");
        var ClientAccountNumber = $(this).attr("clientAccount");
        var VersionNumber = $(this).attr("versionNumber");
        removedClientAccounts.push({ ClientAccountNumber: ClientAccountNumber, SourceSystemCode: SourceSystemCode, VersionNumber: VersionNumber });
    });
    $('#RemovedClientAccounts').val(JSON.stringify(removedClientAccounts));

}

/*
Show Servicing Options
*/
function ShowClientServicingOptionsScreen() {
	var serviceOptionChanges = 0;
    var serviceOptionChangedItems = [];
    $("#hiddenRemovedServicingOptionsTable").empty();
    $("#hiddenAddedServicingOptionsTable").empty();
    $("#hiddenAlteredServicingOptionsTable").empty();
    
    //Show Loader
    var $tabs = $('#tabs').tabs();
    $('#tabs').tabs('enable', 4);
    $tabs.tabs('select', 4); // switch to 5th tab
    $("#tabs-Content").html('<img src="images/common/grid-loading.gif" alt=""/> Please wait...');

	var url = '/ClientWizard.mvc/ServicingOptionsScreen?id=' + Math.random();
	$.ajax({
		type: "POST",
		data: { clientSubUnitGuid: $('#ClientSubUnitGuid').val() },
		url: url,
		success: function (result) {

			var $tabs = $('#tabs').tabs();
			$('#tabs').tabs('enable', 4);
			$tabs.tabs('select', 4); // switch to 5th tab
			$("#tabs-5Content").html(result);

            $('#clientServicingOptionsCancelButton').button();
            $('#clientServicingOptionsCancelButton').click(function () {
                ShowClientServicingOptionsScreen();
            });

            $('#clientServicingOptionsSaveButton').button();
            $('#clientServicingOptionsSaveButton').click(function () {
                UpdateServiceOptionsSettings(serviceOptionChanges, serviceOptionChangedItems);
                SaveClientServicingOptions();
                CommitServiceOptionsChanges(function () {
                    ShowReasonCodesScreen(true);
                });
            });

			//buttons
			$('#clientServicingOptionsBackButton').button();
            $('#clientServicingOptionsBackButton').click(function () {
                if (hasPageBeenChanged("ServicingOptions")) {
                    showSaveDialog(function () {
                        UpdateServiceOptionsSettings(serviceOptionChanges, serviceOptionChangedItems);
                        SaveClientServicingOptions();
                        CommitServiceOptionsChanges(function () {
                            ShowClientAccountsScreen();
                            $tabs.tabs('select', 3);
                        });
                    }, ShowClientServicingOptionsScreen);
                } else {
                    ShowClientAccountsScreen();
                    $tabs.tabs('select', 3);
                }
			});

			$('#clientServicingOptionsNextButton').button();
			$('#clientServicingOptionsNextButton').click(function () {

                if (hasPageBeenChanged("ServicingOptions")) {
                    showSaveDialog(function () {
                        UpdateServiceOptionsSettings(serviceOptionChanges, serviceOptionChangedItems);
                        SaveClientServicingOptions();
                        CommitServiceOptionsChanges(function () {
                            ShowReasonCodesScreen(true);
                        });
                    }, ShowClientServicingOptionsScreen);
                } else {
                    ShowReasonCodesScreen(true);
                }
			});

			//Edit
			$('#currentServicingOptions .edit').click(function () {
				var serviceItemID = escapeInput($(this).parent().parent().attr("serviceItemID"));
				var serviceItemOptionID = escapeInput($(this).parent().parent().attr("serviceOptionId"));
				var clientSubUnitGuid = escapeInput($('#ClientSubUnitGuid').val());
				var servicingOptionPopupEditUrl = '../ClientWizard.mvc/ServicingOptionPopupEdit?servicingOptionItemId=' + serviceItemID + '&csu=' + clientSubUnitGuid;

				$("#dialog-confirm").load(servicingOptionPopupEditUrl).dialog({
					resizable: true,
					modal: true,
					height: 400,
					width: 600,
					title: "Service Options",
					buttons: {
						"Edit Item": function () {
							LoadServicingOptionPopupEdit(serviceItemID, serviceItemOptionID);
						},
						Close: function () {
							$(this).dialog("close");
						}
					}
				}, 'open');
			});

			//Remove
			$('#currentServicingOptions .remove').click(function () {

				var serviceItemID = $(this).parent().parent().attr("serviceItemID");


				var optionStatus = $(this).parent().parent().attr("optionStatus");


				if (optionStatus == "Current") {
					//only current will have version number
					var versionNumber = $(this).parent().parent().attr("versionNumber");
					var ServicingOptionId = $(this).parent().parent().attr("serviceOptionID")

					//write to RemovedServicingOptions

					$('#hiddenRemovedServicingOptionsTable').append("<tr itemID='" + serviceItemID + "' versionNumber='" + versionNumber + "' serviceOptionID ='" + ServicingOptionId + "'></tr>");

				}
				else {

					//also remove from hidden addedd table if not current

					$('#hiddenAddedServicingOptionsTable tr').each(function () {

						var serviceItemID2 = $(this).attr("serviceItemID")
						if (serviceItemID2 == serviceItemID) {
							$(this).remove();
						}

					});

				}

				$(this).parent().parent().remove();
				$("#currentServicingOptions").tablesorter({
					headers: {

						0: {
							// disable it by setting the property sorter to false
							sorter: false
						},
						1: {
							// disable it by setting the property sorter to false
							sorter: false
						},
						2: {
							// disable it by setting the property sorter to false
							sorter: false
						},
						3: {
							// disable it by setting the property sorter to false
							sorter: false
						},
						4: {
							// disable it by setting the property sorter to false
							sorter: false
						}
                        
					},

					widgets: ['zebra']
				});



			});

			$('#currentServicingOptions input').change(function () {

				//only for current ones.

				if ($(this).parent().parent().attr("optionStatus") == "Current") {

					serviceOptionChanges = 1;
					//serviceOptionChangedItems.pop($(this).parent().parent().attr("serviceItemID"))
					serviceOptionChangedItems.push($(this).parent().parent().attr("serviceItemID"));

				}
			});


			$('#currentServicingOptions select').change(function () {

				//only for current ones.

				if ($(this).parent().parent().attr("optionStatus") == "Current") {

					serviceOptionChanges = 1;
					//			serviceOptionChangedItems.pop($(this).parent().parent().attr("serviceItemID"))
					serviceOptionChangedItems.push($(this).parent().parent().attr("serviceItemID"));

				}
			});


			$('#clientServicingOptionsAddButton').button();
			$('#clientServicingOptionsAddButton').click(function () {
				var clientSubUnitGuid = escapeInput($('#ClientSubUnitGuid').val());
				var servicingOptionPopupUrl = '../ClientWizard.mvc/ServicingOptionPopup?csu=' + clientSubUnitGuid;
				$("#dialog-confirm").load(servicingOptionPopupUrl).dialog({
					resizable: true,
					modal: true,
					height: 400,
					width: 600,
					title: "Service Options",
					buttons: {
						"Add Item": function () {
							LoadServicingOptionPopup();
						},
						Close: function () {
							$(this).dialog("close");
						}
					}
				}, 'open');
			});
		}
	});
}

function LoadServicingOptionPopup() {

	var SelectOrText = "";

	//check if the value for the item is a select or text field.
	if ($('#trSelectListServicingOptionItemValue').is(":visible")) {
		SelectOrText = "Select";
	}

	if ($('#trTextboxServicingOptionItemValue').is(":visible")) {
		SelectOrText = "Text";
	}

	if ($("#txtServicingOptionItemValue").length > 50) {
		alert("Max length for item value exceeded");
		$("#txtServicingOptionItemValue").focus();
		return;
	}

	if ($("#ServicingOptionInstruction").length > 100) {
		alert("Max length for item instruction exceeded");
		$("#ServicingOptionInstruction").focus();
		return;
	}

	if ($("#ServicingOptionId :selected").text() == "Please Select...") {
		alert("please make a valid selection, or close");
		$("#ServicingOptionId").focus();
		return;
	}

	if (SelectOrText == "Text") {

		var optionItemValue = $("#txtServicingOptionItemValue").val();
		if (!optionItemValue) {
			alert("Please provide a value for the item value.");
			$("#txtServicingOptionItemValue").focus();
			return;
		}
	}
	if (SelectOrText == "Select") {
		if ($("#selServicingOptionItemValue :selected").text() == "Please Select...") {
			alert("Please provide a value for the item value.");
			$("#selServicingOptionItemValue").focus();
			return;
		}
	}

	//  ensure GDS is clear if not used
	if ($('#trGDSs').is(":visible")) {
		gdsCode = escapeInput($('#GDSCode').val());
		gdsName = escapeInput($('#GDSCode :selected').text());
		if (gdsName == "Please Select...") {
			alert("Please provide a value for GDS");
			$("#GDSCode").focus();
			return;
		}
	} else {
		gdsCode = "";
		gdsName = "";
	}

	// ensure parameters are clear if not used
	if (!$('.parameter-fields').is(":visible")) {
		$('#DepartureTimeWindowMinutes').val("");
		$('#ArrivalTimeWindowMinutes').val("");
		$('#MaximumStops').val("");
		$('#txtMaximumConnectionTimeMinutes').val("");
		$('#UseAlternateAirportFlag').val("");
		$('#NoPenaltyFlag').val("");
		$('#NoRestrictionsFlag').val("");
	}

	var optionInstruction = $("#ServicingOptionItemInstruction").val()

	if (!optionInstruction) {
		alert("Please provide a value for the item instruction.");
		$("#ServicingOptionItemInstruction").focus();
		return;
	}


	if (SelectOrText == "Text") {
		if (!$("#txtServicingOptionItemValue").val()) {
			alert("Please provide a value for the item value.");
			$("#txtServicingOptionItemValue").focus();
			return;
		}
	}

	//Must be numerical only
	var maximumConnectionTimeMinutesRegex = $("#txtMaximumConnectionTimeMinutes").val();
	var reg = /^[0-9]*$/;
	if (!reg.test(maximumConnectionTimeMinutesRegex)) {
		alert("Please provide a numeric value for Maximum Connection Time.");
		$("#txtMaximumConnectionTimeMinutes").focus();
		return;
	}


	var ServicingOptionItemSequence = "";

	if (SelectOrText == "Text") {

		//Check if ServicingOption requires a GDS adding
		$.ajax({
			url: "/ServicingOptionItemValue.mvc/GetServicingOptionGDSRequired", type: "POST", dataType: "json",
			data: { servicingOptionId: $("#ServicingOptionId").val() },
			success: function (data) {
				ServicingOptionItemSequence = "N/A";
			}
		});

		var itemValue = escapeInput($("#txtServicingOptionItemValue").val());
		var itemInstruction = escapeInput($("#ServicingOptionItemInstruction").val());
		var departureTimeWindowMinutes = escapeInput($("#DepartureTimeWindowMinutes").val());
		var arrivalTimeWindowMinutes = escapeInput($("#ArrivalTimeWindowMinutes").val());
		var maximumStops = escapeInput($("#MaximumStops").val());
		var maximumConnectionTimeMinutes = escapeInput($("#txtMaximumConnectionTimeMinutes").val());
		var useAlternateAirportFlag = $("#UseAlternateAirportFlag").is(':checked');
		var noPenaltyFlag = $("#NoPenaltyFlag").is(':checked');
		var noRestrictionsFlag = $("#NoRestrictionsFlag").is(':checked');
        var servicingOptionId = escapeInput($('#ServicingOptionId').val());
        var servicingOptionIdText = escapeInput($('#ServicingOptionId :selected').text());
        var servicingOptionItemValue = escapeInput($("#txtServicingOptionItemValue").val());
        var servicingOptionItemInstruction = escapeInput($("#ServicingOptionItemInstruction").val());

        $('#currentServicingOptions').append("<tr serviceItemID='" + servicingOptionId + "' servicingOptionName = '" + servicingOptionIdText + "' optionStatus='NotCurrent' itemValue='" + servicingOptionItemValue + "' itemInstruction='" + servicingOptionItemInstruction + "' gdsCode='" + gdsCode + "'><td>" + servicingOptionIdText + "</td><td>" + servicingOptionItemValue + "</td><td>" + gdsName + "</td><td>" + ServicingOptionItemSequence + "</td><td>" + unescapeInput(itemInstruction) + "</td><td></td><td></td><td></td><td><img src='../../images/remove.png' border='0' /></td></tr>");
        $('#hiddenAddedServicingOptionsTable').append("<tr  serviceItemID='" + servicingOptionId + "' servicingOptionName = '" + servicingOptionIdText + "' optionStatus='NotCurrent' itemValue='" + itemValue + "' itemInstruction='" + itemInstruction +  "' gdscode='" + gdsCode + "' departureTimeWindowMinutes='" + departureTimeWindowMinutes + "' arrivalTimeWindowMinutes='" + arrivalTimeWindowMinutes + "' maximumStops='" + maximumStops + "' maximumConnectionTimeMinutes='" + maximumConnectionTimeMinutes + "' useAlternateAirportFlag='" + useAlternateAirportFlag + "' noPenaltyFlag='" + noPenaltyFlag + "' noRestrictionsFlag='" + noRestrictionsFlag + "'></tr>");

		//clear pop-up form
		$('#txtServicingOptionItemValue').val("");
		$('#ServicingOptionItemInstruction').val("");
		$('#GDSCode option[value=""]').attr('selected', 'selected');
		$('#ServicingOptionId option[value=""]').attr('selected', 'selected');
		$("#DepartureTimeWindowMinutes").val("");
		$("#ArrivalTimeWindowMinutes").val("");
		$("#MaximumStops").val("");
		$("#txtMaximumConnectionTimeMinutes").val("");
		$("#UseAlternateAirportFlag").attr('checked', false);
		$("#NoPenaltyFlag").attr('checked', false);
		$("#NoRestrictionsFlag").attr('checked', false);
	}

	if (SelectOrText == "Select") {

		// add N/A for default ordering
		if ($('#ServicingOptionId').val() == 45) { //Acceptable Forms of Payment
			ServicingOptionItemSequence = "N/A";
		}

		var itemValue = escapeInput($("#selServicingOptionItemValue :selected").text());
		var itemInstruction = escapeInput($("#ServicingOptionItemInstruction").val());
        var servicingOptionId = escapeInput($('#ServicingOptionId').val());
        var servicingOptionIdText = escapeInput($('#ServicingOptionId :selected').text());

        $('#currentServicingOptions').append("<tr serviceItemID='" + servicingOptionId + "' servicingOptionName = '" + servicingOptionIdText + "' optionStatus='NotCurrent' itemValue='" + itemValue + "' itemInstruction='" + itemInstruction + "' gdsCode='" + gdsCode + "'><td>" + servicingOptionIdText + "</td><td>" + itemValue + "</td><td>" + gdsName + "</td><td>" + ServicingOptionItemSequence + "</td><td>" + unescapeInput(itemInstruction) + "</td><td></td><td></td><td></td><td><img src='../../images/remove.png' border='0' /></td></tr>");


		//Parameters
		var departureTimeWindowMinutes = escapeInput($("#DepartureTimeWindowMinutes").val());
		var arrivalTimeWindowMinutes = escapeInput($("#ArrivalTimeWindowMinutes").val());
		var maximumStops = escapeInput($("#MaximumStops").val());
		var maximumConnectionTimeMinutes = escapeInput($("#txtMaximumConnectionTimeMinutes").val());
		var useAlternateAirportFlag = $("#UseAlternateAirportFlag").is(':checked');
		var noPenaltyFlag = $("#NoPenaltyFlag").is(':checked');
		var noRestrictionsFlag = $("#NoRestrictionsFlag").is(':checked');
        var servicingOptionId = escapeInput($('#ServicingOptionId').val());
        var servicingOptionIdText = escapeInput($('#ServicingOptionId :selected').text());

        $('#hiddenAddedServicingOptionsTable').append("<tr serviceItemID='" + servicingOptionId + "' servicingOptionName = '" + servicingOptionIdText + "' optionStatus='NotCurrent' itemValue='" + itemValue + "' itemInstruction='" + itemInstruction +  "' gdscode='" + gdsCode + "' departureTimeWindowMinutes='" + departureTimeWindowMinutes + "' arrivalTimeWindowMinutes='" + arrivalTimeWindowMinutes + "' maximumStops='" + maximumStops + "' maximumConnectionTimeMinutes='" + maximumConnectionTimeMinutes + "' useAlternateAirportFlag='" + useAlternateAirportFlag + "' noPenaltyFlag='" + noPenaltyFlag + "' noRestrictionsFlag='" + noRestrictionsFlag + "'></tr>");

		//only allowed once
		if ($('#ServicingOptionId').val() == 180) {
			$("#ServicingOptionId option[value='180']").remove();
		}

		if ($('#ServicingOptionId').val() == 181) {
			$("#ServicingOptionId option[value='181']").remove();
		}

		//clear pop-up form
		$('#selServicingOptionItemValue option[value=""]').attr('selected', 'selected');
		$('#ServicingOptionItemInstruction').val("");
		$('#GDSCode option[value=""]').attr('selected', 'selected');
		$('#ServicingOptionId option[value=""]').attr('selected', 'selected');
		$("#DepartureTimeWindowMinutes").val("");
		$("#ArrivalTimeWindowMinutes").val("");
		$("#MaximumStops").val("");
		$("#txtMaximumConnectionTimeMinutes").val("");
		$("#UseAlternateAirportFlag").attr('checked', false);
		$("#NoPenaltyFlag").attr('checked', false);
		$("#NoRestrictionsFlag").attr('checked', false);
	}

	$("#currentServicingOptions").tablesorter({
		headers: {

			0: {
				// disable it by setting the property sorter to false
				sorter: false
			},
			1: {
				// disable it by setting the property sorter to false
				sorter: false
			},
			2: {
				// disable it by setting the property sorter to false
				sorter: false
			},
			3: {
				// disable it by setting the property sorter to false
				sorter: false
			},
			4: {
				// disable it by setting the property sorter to false
				sorter: false
			}

		},
		widgets: ['zebra']
	});

	$('#currentServicingOptions .remove').click(function () {

		var serviceItemID = $(this).parent().parent().attr("serviceItemID");
		var optionStatus = $(this).parent().parent().attr("optionStatus");
		if (optionStatus == "Current") {


			//write to RemovedServicingOptions
			var versionNumber = $(this).parent().parent().attr("versionNumber");

			//write to RemovedServicingOptions
			var ServicingOptionId = $(this).parent().parent().attr("serviceOptionID")

			$('#hiddenRemovedServicingOptionsTable').append("<tr itemID='" + serviceItemID + "' versionNumber='" + versionNumber + "' serviceOptionId='" + ServicingOptionId + "'></tr>");

		} else {

			//also remove from hidden addedd table if not current				
			$('#hiddenAddedServicingOptionsTable tr').each(function () {
				var serviceItemID2 = $(this).attr("serviceItemID")
				if (serviceItemID2 == serviceItemID) {
					$(this).remove();

				}
			});
		}

		$(this).parent().parent().remove();
		$("#currentServicingOptions").tablesorter({
			headers: {

				0: {
					// disable it by setting the property sorter to false
					sorter: false
				},
				1: {
					// disable it by setting the property sorter to false
					sorter: false
				},
				2: {
					// disable it by setting the property sorter to false
					sorter: false
				},
				3: {
					// disable it by setting the property sorter to false
					sorter: false
				},
				4: {
					// disable it by setting the property sorter to false
					sorter: false
				}

			},

			widgets: ['zebra']
		});
	});
}

function LoadServicingOptionPopupEdit(servicingItemId, servicingOptionId) {

	var SelectOrText = "";

	//check if the value for the item is a select or text field.
	if ($('#trSelectListServicingOptionItemValue').is(":visible")) {
		SelectOrText = "Select";
	}

	if ($('#trTextboxServicingOptionItemValue').is(":visible")) {
		SelectOrText = "Text";
	}

	if ($("#ServicingOptionItem_ServicingOptionItemValue").length > 50) {
		alert("Max length for item value exceeded");
		$("#ServicingOptionItemValue").focus();
		return;
	}

	if ($("#ServicingOptionItem_ServicingOptionItemInstruction").length > 100) {
		alert("Max length for item instruction exceeded");
		$("#ServicingOptionInstruction").focus();
		return;
	}

	if ($("#ServicingOptionId:selected").text() == "Please Select...") {
		alert("please make a valid selection, or close");
		$("#ServicingOptionId").focus();
		return;
	}

	if (SelectOrText == "Text") {

		var optionItemValue = $("#ServicingOptionItem_ServicingOptionItemValue").val();
		if (!optionItemValue) {
			alert("Please provide a value for the item value.");
			$("#ServicingOptionItem_ServicingOptionItemValue").focus();
			return;
		}
	}

	if (SelectOrText == "Select") {
		if ($("#selServicingOptionItemValue :selected").text() == "Please Select...") {
			alert("Please provide a value for the item value.");
			$("#selServicingOptionItemValue").focus();
			return;
		}
	}

	//  ensure GDS is clear if not used
	if ($('#trGDSs').is(":visible")) {
		gdsCode = escapeInput($('#ServicingOptionItem_GDSCode').val());
		gdsName = escapeInput($('#ServicingOptionItem_GDSCode :selected').text());
		if (gdsName == "Please Select...") {
			alert("Please provide a value for GDS");
			$("#ServicingOptionItem_GDSCode").focus();
			return;
		}
	} else {
		gdsCode = "";
		gdsName = "";
	}

	// ensure parameters are clear if not used
	if (!$('.parameter-fields').is(":visible")) {
		$("#ServicingOptionItem_DepartureTimeWindowMinutes").val("");
		$("#ServicingOptionItem_ArrivalTimeWindowMinutes").val("");
		$("#ServicingOptionItem_MaximumStops").val("");
		$("#ServicingOptionItem_MaximumConnectionTimeMinutes").val("");
		$("#UseAlternateAirportFlag").attr('checked', false);
		$("#NoPenaltyFlag").attr('checked', false);
		$("#NoRestrictionsFlag").attr('checked', false);
	}

	var optionInstruction = $("#ServicingOptionItem_ServicingOptionItemInstruction").val()

	if (!optionInstruction) {
		alert("Please provide a value for the item instruction.");
		$("#ServicingOptionItem_ServicingOptionItemInstruction").focus();
		return;
	}


	if (SelectOrText == "Text") {
		if (!$("#ServicingOptionItem_ServicingOptionItemValue").val()) {
			alert("Please provide a value for the item value.");
			$("#ServicingOptionItem_ServicingOptionItemValue").focus();
			return;
		}
	}

	//Must be numerical only
	var maximumConnectionTimeMinutesRegex = $("#ServicingOptionItem_MaximumConnectionTimeMinutes").val();
	var reg = /^[0-9]*$/;
	if (!reg.test(maximumConnectionTimeMinutesRegex)) {
		alert("Please provide a numeric value for Maximum Connection Time.");
		$("#ServicingOptionItem_MaximumConnectionTimeMinutes").focus();
		return;
	}

	var ServicingOptionItemSequence = "";

	if (SelectOrText == "Text") {

		//Check if ServicingOption requires a GDS adding
		$.ajax({
			url: "/ServicingOptionItemValue.mvc/GetServicingOptionGDSRequired", type: "POST", dataType: "json",
			data: { servicingOptionId: servicingOptionId },
			success: function (data) {
				ServicingOptionItemSequence = "N/A";
			}
		});

		//Update row by removing old one and adding new one in
		$('#currentServicingOptions tr').each(function () {
			if ($(this).attr('serviceitemid') == servicingItemId) {
				$(this).remove();
			}
		});

		var itemValue = escapeInput($("#ServicingOptionItem_ServicingOptionItemValue").val());
		var itemInstruction = escapeInput($("#ServicingOptionItem_ServicingOptionItemInstruction").val());
		var servicingOptionName =  escapeInput($('#ServicingOptionId :selected').text());
		var departureTimeWindowMinutes = escapeInput($("#ServicingOptionItem_DepartureTimeWindowMinutes").val());
		var arrivalTimeWindowMinutes = escapeInput($("#ServicingOptionItem_ArrivalTimeWindowMinutes").val());
		var maximumStops = escapeInput($("#ServicingOptionItem_MaximumStops").val());
		var maximumConnectionTimeMinutes = escapeInput($("#ServicingOptionItem_MaximumConnectionTimeMinutes").val());
		var useAlternateAirportFlag = $("#UseAlternateAirportFlag").is(':checked');
		var noPenaltyFlag = $("#NoPenaltyFlag").is(':checked');
		var noRestrictionsFlag = $("#NoRestrictionsFlag").is(':checked');
		var versionNumber = escapeInput($("#ServicingOptionItem_VersionNumber").val());

        $('#currentServicingOptions').append("<tr serviceItemID='" + servicingItemId + "' serviceoptionid='" + servicingOptionId + "' servicingOptionName = '" + servicingOptionName + "' optionStatus='NotCurrent' itemValue='" + itemValue + "' itemInstruction='" + itemInstruction + "' gdsCode='" + gdsCode + "' departureTimeWindowMinutes='" + departureTimeWindowMinutes + "' arrivalTimeWindowMinutes='" + arrivalTimeWindowMinutes + "' maximumStops='" + maximumStops + "' maximumConnectionTimeMinutes='" + maximumConnectionTimeMinutes + "' useAlternateAirportFlag='" + useAlternateAirportFlag + "' noPenaltyFlag='" + noPenaltyFlag + "' noRestrictionsFlag='" + noRestrictionsFlag + "'><td>" + servicingOptionName + "</td><td>" + itemValue + "</td><td>" + gdsName + "</td><td>" + ServicingOptionItemSequence + "</td><td>" + unescapeInput(itemInstruction) + "</td><td></td><td></td><td></td><td><img src='../../images/remove.png' border='0' /></td></tr>");
		$('#hiddenAlteredServicingOptionsTable').append("<tr serviceItemID='" + servicingItemId + "' serviceoptionid='" + servicingOptionId + "' servicingOptionName = '" + servicingOptionName + "' optionStatus='NotCurrent' itemValue='" + itemValue + "' itemInstruction='" + itemInstruction  + "' gdscode='" + gdsCode + "' departureTimeWindowMinutes='" + departureTimeWindowMinutes + "' arrivalTimeWindowMinutes='" + arrivalTimeWindowMinutes + "' maximumStops='" + maximumStops + "' maximumConnectionTimeMinutes='" + maximumConnectionTimeMinutes + "' useAlternateAirportFlag='" + useAlternateAirportFlag + "' noPenaltyFlag='" + noPenaltyFlag + "' noRestrictionsFlag='" + noRestrictionsFlag + "' versionNumber='" + versionNumber + "'></tr>");

		//clear pop-up form
		$('#ServicingOptionItem_ServicingOptionItemValue').val("");
		$('#ServicingOptionItem_ServicingOptionItemInstruction').val("");
		$('#ServicingOptionItem_GDSCode option[value=""]').attr('selected', 'selected');
		$('#ServicingOptionId option[value=""]').attr('selected', 'selected');
		$("#ServicingOptionItem_DepartureTimeWindowMinutes").val("");
		$("#ServicingOptionItem_ArrivalTimeWindowMinutes").val("");
		$("#ServicingOptionItem_MaximumStops").val("");
		$("#ServicingOptionItem_MaximumConnectionTimeMinutes").val("");
		$("#UseAlternateAirportFlag").attr('checked', false);
		$("#NoPenaltyFlag").attr('checked', false);
		$("#NoRestrictionsFlag").attr('checked', false);
	}

	if (SelectOrText == "Select") {

		// add N/A for default ordering
		if ($('#ServicingOptionId').val() == 45) { //Acceptable Forms of Payment
			ServicingOptionItemSequence = "N/A";
		}

		//Update row by removing old one and adding new one in
		$('#currentServicingOptions tr').each(function () {
			if ($(this).attr('serviceitemid') == servicingItemId) {
				$(this).remove();
			}
		});

		var itemValue = escapeInput($("#selServicingOptionItemValue :selected").text());
		var itemInstruction = escapeInput($("#ServicingOptionItem_ServicingOptionItemInstruction").val());
		var servicingOptionName = escapeInput($('#ServicingOptionId :selected').text());
		var departureTimeWindowMinutes = escapeInput($("#ServicingOptionItem_DepartureTimeWindowMinutes").val());
		var arrivalTimeWindowMinutes = escapeInput($("#ServicingOptionItem_ArrivalTimeWindowMinutes").val());
		var maximumStops = escapeInput($("#ServicingOptionItem_MaximumStops").val());
		var maximumConnectionTimeMinutes = escapeInput($("#ServicingOptionItem_MaximumConnectionTimeMinutes").val());
		var useAlternateAirportFlag = $("#UseAlternateAirportFlag").is(':checked');
		var noPenaltyFlag = $("#NoPenaltyFlag").is(':checked');
		var noRestrictionsFlag = $("#NoRestrictionsFlag").is(':checked');
		var versionNumber = escapeInput($("#ServicingOptionItem_VersionNumber").val());

        $('#currentServicingOptions').append("<tr serviceItemID='" + servicingItemId + "' serviceoptionid='" + servicingOptionId + "' servicingOptionName = '" + servicingOptionName + "' optionStatus='NotCurrent' itemValue='" + itemValue + "' itemInstruction='" + itemInstruction + "' gdsCode='" + gdsCode + "' departureTimeWindowMinutes='" + departureTimeWindowMinutes + "' arrivalTimeWindowMinutes='" + arrivalTimeWindowMinutes + "' maximumStops='" + maximumStops + "' maximumConnectionTimeMinutes='" + maximumConnectionTimeMinutes + "' useAlternateAirportFlag='" + useAlternateAirportFlag + "' noPenaltyFlag='" + noPenaltyFlag + "' noRestrictionsFlag='" + noRestrictionsFlag + "'><td>" + servicingOptionName + "</td><td>" + itemValue + "</td><td>" + gdsName + "</td><td>" + ServicingOptionItemSequence + "</td><td>" + itemInstruction + "</td><td></td><td></td><td></td><td><img src='../../images/remove.png' border='0' /></td></tr>");
		$('#hiddenAlteredServicingOptionsTable').append("<tr serviceItemID='" + servicingItemId + "' serviceoptionid='" + servicingOptionId + "' servicingOptionName = '" + servicingOptionName + "' optionStatus='NotCurrent' itemValue='" + itemValue + "' itemInstruction='" + itemInstruction + "' gdscode='" + gdsCode + "' departureTimeWindowMinutes='" + departureTimeWindowMinutes + "' arrivalTimeWindowMinutes='" + arrivalTimeWindowMinutes + "' maximumStops='" + maximumStops + "' maximumConnectionTimeMinutes='" + maximumConnectionTimeMinutes + "' useAlternateAirportFlag='" + useAlternateAirportFlag + "' noPenaltyFlag='" + noPenaltyFlag + "' noRestrictionsFlag='" + noRestrictionsFlag + "' versionNumber='" + versionNumber + "'></tr>");

		//only allowed once
		if ($('#ServicingOptionId').val() == 180) {
			$("#ServicingOptionId option[value='180']").remove();
		}

		if ($('#ServicingOptionId').val() == 181) {
			$("#ServicingOptionId option[value='181']").remove();
		}

		//clear pop-up form
		$('#selServicingOptionItemValue option[value=""]').attr('selected', 'selected');
		$('#ServicingOptionItemInstruction').val("");
		$('#ServicingOptionItem_GDSCode option[value=""]').attr('selected', 'selected');
		$('#ServicingOptionId option[value=""]').attr('selected', 'selected');
		$("#ServicingOptionItem_DepartureTimeWindowMinutes").val("");
		$("#ServicingOptionItem_ArrivalTimeWindowMinutes").val("");
		$("#ServicingOptionItem_MaximumStops").val("");
		$("#ServicingOptionItem_MaximumConnectionTimeMinutes").val("");
		$("#UseAlternateAirportFlag").attr('checked', false);
		$("#NoPenaltyFlag").attr('checked', false);
		$("#NoRestrictionsFlag").attr('checked', false);
	}

	$("#currentServicingOptions").tablesorter({
		headers: {

			0: {
				// disable it by setting the property sorter to false
				sorter: false
			},
			1: {
				// disable it by setting the property sorter to false
				sorter: false
			},
			2: {
				// disable it by setting the property sorter to false
				sorter: false
			},
			3: {
				// disable it by setting the property sorter to false
				sorter: false
			},
			4: {
				// disable it by setting the property sorter to false
				sorter: false
			}

		},
		widgets: ['zebra']
	});

	$('#currentServicingOptions .remove').click(function () {

		var serviceItemID = $(this).parent().parent().attr("serviceItemID");
		var optionStatus = $(this).parent().parent().attr("optionStatus");
		if (optionStatus == "Current") {


			//write to RemovedServicingOptions
			var versionNumber = $(this).parent().parent().attr("versionNumber");

			//write to RemovedServicingOptions
			var ServicingOptionId = $(this).parent().parent().attr("serviceOptionID")

			$('#hiddenRemovedServicingOptionsTable').append("<tr itemID='" + serviceItemID + "' versionNumber='" + versionNumber + "' serviceOptionId='" + ServicingOptionId + "'></tr>");

		} else {

			//also remove from hidden addedd table if not current				
			$('#hiddenAddedServicingOptionsTable tr').each(function () {
				var serviceItemID2 = $(this).attr("serviceItemID")
				if (serviceItemID2 == serviceItemID) {
					$(this).remove();

				}
			});
		}

		$(this).parent().parent().remove();
		$("#currentServicingOptions").tablesorter({
			headers: {

				0: {
					// disable it by setting the property sorter to false
					sorter: false
				},
				1: {
					// disable it by setting the property sorter to false
					sorter: false
				},
				2: {
					// disable it by setting the property sorter to false
					sorter: false
				},
				3: {
					// disable it by setting the property sorter to false
					sorter: false
				},
				4: {
					// disable it by setting the property sorter to false
					sorter: false
				}

			},

			widgets: ['zebra']
		});
	});

	//Close
	$("#dialog-confirm").dialog("close");

}

function UpdateServiceOptionsSettings(serviceOptionChanges, serviceOptionChangedItems) {
    //check if serviceOptionChange = 1, if so, write to append table
    if (serviceOptionChanges == 1) {
        for (i = 0; i < serviceOptionChangedItems.length; i++) {

            $('#currentServicingOptions tbody tr').each(function () {

                if (serviceOptionChangedItems[i] == $(this).attr("serviceItemID")) {
                    

                    var selectString = "#soItemValSelect_" + escapeInput($(this).attr("serviceItemID"));
                    var textString = "#soItemVal_" + escapeInput($(this).attr("serviceItemID"));
                    var instructionString = "#soInstruction_" + escapeInput($(this).attr("serviceItemID"));
                    var serviceItemID = escapeInput($(this).attr("serviceItemID"));
                    var itemName = escapeInput($('#itemName').val());

                    var itemValue = escapeInput($(selectString).val());
                    if (itemValue === "undefined" || itemValue === "") {
                        itemValue = escapeInput($(textString).val());
                    }
                    var itemInstruction = escapeInput($(instructionString).val());
                    var versionNumber = escapeInput($(this).attr("versionNumber"));
                    var serviceOptionID = escapeInput($(this).attr("serviceOptionID"));
                    var gdsCodeSelect = "#gdsCode_" + escapeInput($(this).attr("serviceItemID"));
                    gdsCode = escapeInput($(gdsCodeSelect).val());

                    var departureTimeWindowMinutes = escapeInput($(this).attr("DepartureTimeWindowMinutes"));
                    var arrivalTimeWindowMinutes = escapeInput($(this).attr("ArrivalTimeWindowMinutes"));
                    var maximumStops = escapeInput($(this).attr("MaximumStops"));
                    var maximumConnectionTimeMinutes = escapeInput($(this).attr("MaximumConnectionTimeMinutes"));
                    var useAlternateAirportFlag = escapeInput($(this).attr("#UseAlternateAirportFlag"));
                    var noPenaltyFlag = escapeInput($(this).attr("NoPenaltyFlag"));
                    var noRestrictionsFlag = escapeInput($(this).attr("NoRestrictionsFlag"));

                    //to cater for a bug that adds the item twice
                    $('#hiddenAlteredServicingOptionsTable tbody tr').each(function () {
                        if ($(this).attr("serviceItemID") == serviceItemID) {
                            $(this).remove();
                        }
                    });

                    $('#hiddenAlteredServicingOptionsTable').append("<tr serviceItemID='" + serviceItemID + "' serviceOptionID='" + serviceOptionID + "' servicingOptionName='" + itemName + "' versionNumber='" + versionNumber + "' itemValue='" + itemValue + "' itemInstruction='" + itemInstruction + "' gdsCode='" + gdsCode + "' departureTimeWindowMinutes='" + departureTimeWindowMinutes + "' arrivalTimeWindowMinutes='" + arrivalTimeWindowMinutes + "' maximumStops='" + maximumStops + "' maximumConnectionTimeMinutes='" + maximumConnectionTimeMinutes + "' useAlternateAirportFlag='" + useAlternateAirportFlag + "' noPenaltyFlag='" + noPenaltyFlag + "' noRestrictionsFlag='" + noRestrictionsFlag + "'></tr>");

                }

            });


        }
        //end of if serviceoptionchanges = 1


    }

}

/*
Save ServicingOption to Hidden Inputs
*/
function SaveClientServicingOptions() {

    //CODE TO ADD ServicingOptions TO HIDDEN FIELDS HERE

    //edit servicingoptions
    var alteredServicingOptions = [];
    $('#hiddenAlteredServicingOptionsTable tr').each(function () {
    	var ServicingOptionItemId = $(this).attr("serviceitemid");
    	var ServicingOptionId = $(this).attr("serviceoptionid");
    	var ServicingOptionName = $(this).attr("servicingoptionname");
		var gdsCode = $(this).attr("gdsCode");
        if (gdsCode == "undefined") {
            gdsCode = "";
        }
        var ServicingOptionItemInstruction = unescapeInput($(this).attr("itemInstruction"));
        var ServicingOptionItemValue = unescapeInput($(this).attr("itemvalue"));
        var DepartureTimeWindowMinutes = $(this).attr("DepartureTimeWindowMinutes");
        var ArrivalTimeWindowMinutes = $(this).attr("ArrivalTimeWindowMinutes");
        var MaximumStops = $(this).attr("MaximumStops");
        var MaximumConnectionTimeMinutes = $(this).attr("MaximumConnectionTimeMinutes");
        var UseAlternateAirportFlag = $(this).attr("UseAlternateAirportFlag");
        var NoPenaltyFlag = $(this).attr("NoPenaltyFlag");
        var NoRestrictionsFlag = $(this).attr("NoRestrictionsFlag");
        var VersionNumber = $(this).attr("versionNumber");
        alteredServicingOptions.push({
        	ServicingOptionItemId: ServicingOptionItemId,
        	ServicingOptionId: ServicingOptionId,
        	ServicingOptionName: ServicingOptionName,
        	ServicingOptionItemValue: ServicingOptionItemValue,
        	ServicingOptionItemInstruction: ServicingOptionItemInstruction,
        	gdsCode: gdsCode,
        	DepartureTimeWindowMinutes: DepartureTimeWindowMinutes,
        	ArrivalTimeWindowMinutes: ArrivalTimeWindowMinutes,
        	MaximumStops: MaximumStops,
        	MaximumConnectionTimeMinutes: MaximumConnectionTimeMinutes,
        	UseAlternateAirportFlag: UseAlternateAirportFlag,
        	NoPenaltyFlag: NoPenaltyFlag,
        	NoRestrictionsFlag: NoRestrictionsFlag,
        	VersionNumber: VersionNumber
        });
    });
    $('#AlteredServicingOptions').val(JSON.stringify(alteredServicingOptions));

    //added servicingtopions
    var addedServicingOptions = [];
    $('#hiddenAddedServicingOptionsTable tr').each(function () {

        var ServicingOptionId = $(this).attr("serviceItemID");
        var gdsCode = $(this).attr("gdsCode");
        var ServicingOptionItemValue = unescapeInput($(this).attr("itemvalue"));
        var ServicingOptionItemInstruction = unescapeInput($(this).attr("itemInstruction"));
        
        var DepartureTimeWindowMinutes = $(this).attr("DepartureTimeWindowMinutes");
        var ArrivalTimeWindowMinutes = $(this).attr("ArrivalTimeWindowMinutes");
        var MaximumStops = $(this).attr("MaximumStops");
        var MaximumConnectionTimeMinutes = $(this).attr("MaximumConnectionTimeMinutes");
        var UseAlternateAirportFlag = $(this).attr("UseAlternateAirportFlag");
        var NoPenaltyFlag = $(this).attr("NoPenaltyFlag");
        var NoRestrictionsFlag = $(this).attr("NoRestrictionsFlag");

        addedServicingOptions.push({
        	ServicingOptionId: ServicingOptionId,
        	ServicingOptionItemValue: ServicingOptionItemValue,
        	ServicingOptionItemInstruction: ServicingOptionItemInstruction,
        	
        	gdsCode: gdsCode,
        	DepartureTimeWindowMinutes: DepartureTimeWindowMinutes,
        	ArrivalTimeWindowMinutes: ArrivalTimeWindowMinutes,
        	MaximumStops: MaximumStops,
        	MaximumConnectionTimeMinutes: MaximumConnectionTimeMinutes,
        	UseAlternateAirportFlag: UseAlternateAirportFlag,
        	NoPenaltyFlag: NoPenaltyFlag,
        	NoRestrictionsFlag: NoRestrictionsFlag
        });
    });
    $('#AddedServicingOptions').val(JSON.stringify(addedServicingOptions))

    //removedServicingOptions
    var removedServicingOptions = [];
    $('#hiddenRemovedServicingOptionsTable tr').each(function () {
        var ServicingOptionItemId = $(this).attr("itemID");
        var ServicingOptionId = $(this).attr("serviceOptionID");
        var VersionNumber = $(this).attr("versionNumber");
        removedServicingOptions.push({ ServicingOptionItemId: ServicingOptionItemId, ServicingOptionId: ServicingOptionId, VersionNumber: VersionNumber });
    });
    $('#RemovedServicingOptions').val(JSON.stringify(removedServicingOptions))

}

function CommitServiceOptionsChanges(successCb) {

    var serviceOptionsChanges = {
        ClientTopUnit: $.parseJSON(escapeInput($("#ClientTopUnit").val())),
        ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
        ServicingOptionItemsAdded: $.parseJSON(escapeInput($("#AddedServicingOptions").val())),
        ServicingOptionItemsRemoved: $.parseJSON(escapeInput($("#RemovedServicingOptions").val())),
        ServicingOptionItemsAltered: $.parseJSON(escapeInput($("#AlteredServicingOptions").val()))
    }

    $.ajax({
        type: "POST",
        data: JSON.stringify(serviceOptionsChanges),
        url: '/ClientWizard.mvc/CommitClientServicesChanges?id=' + Math.random(),
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


function ShowReasonCodesScreen(inherited) {

    var reasonCodesInherited = [];
    if (inherited) {
        $('#ReasonCodesInherited').val("true");
    } else {
        $('#ReasonCodesInherited').val("false");
    }

    $("#hiddenChangedClientReasonCodesTable").empty();
    $("#hiddenAddedClientReasonCodesTable").empty();
    $("#hiddenRemovedClientReasonCodesTable").empty();

    
	//Show Loader
    var $tabs = $('#tabs').tabs();
    $('#tabs').tabs('enable', 5);
    $tabs.tabs('select', 5); // switch to 5th tab
    $("#tabs-6Content").html('<img src="images/common/grid-loading.gif" alt=""/> Please wait...');

    var url = '/ClientWizard.mvc/ReasonCodesScreen?id=' + Math.random();
    $.ajax({
        type: "POST",
        data: { clientSubUnitGuid: $('#ClientSubUnitGuid').val(), inherited: inherited },
        url: url,
        error: function () {
            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 5);
            $tabs.tabs('select', 5); // switch to 5th tab
            $("#tabs-6Content").html("Apologies but an error occurred.");

        },
        success: function (result) {

            $("#tabs-6Content").html(result);
            $('#ShowPolicyGroupScreen').val($('#PolicyGroupWriteAccess').val());
            $("#currentReasonCodesTable").tablesorter({
                headers: {

                    0: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
                    1: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
                    2: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    },
                    3: {
                        // disable it by setting the property sorter to false
                        sorter: false
                    }
                },
                widgets: ['zebra']
            });


            // Add values in main table that have been added
          /*  $('#hiddenAddedClientReasonCodesTable tr').each(function () {

                var reasonCode = $(this).attr("reasonCode");
                var productName = $(this).attr("productName");
                var reasonCodeTypeDescription = $(this).attr("reasonCodeTypeDescription");

                //Add row in main table
                var productNameId = productName.replace(/ /g, '');
                var reasonCodeTypeDescriptionId = reasonCodeTypeDescription.replace(/ /g, '');
                var tableId = '#currentReasonCode' + productNameId + reasonCodeTypeDescriptionId + 'ItemsTable';
                var targetTable = $(tableId);
                targetTable.find('tr:last')
                    .after('<tr>' +
                        '<td>&nbsp;</td>' +
                        '<td>' + reasonCode + '</td>' +
                        '<td>' + reasonCodeTypeDescription + '</td>' +
                        '<td>&nbsp;</td>' +
                        '<td>&nbsp;</td>' +
                        '<td>Added</td>' +
                        '</tr>');
            }); */

            // Remove values in main table that have been removed
        /*    $('#hiddenRemovedClientReasonCodesTable tr').each(function () {

                var reasonCodeItemId = $(this).attr("reasonCodeItemId");
                $('#' + reasonCodeItemId).hide();
            }); */

            // Update values in main table that have been changed
        /*    $('#hiddenChangedClientReasonCodesTable tr').each(function () {

                var reasonCode = $(this).attr("reasonCode");
                var reasonCodeItemId = $(this).attr("reasonCodeItemId");

                //Update row in main table
                if (reasonCode != null && reasonCodeItemId != null) {
                    $('#' + reasonCodeItemId).find('td:last').html('Modified');
                    $('#' + reasonCodeItemId).find('.reason_code').html(reasonCode);
                }
            }); */

            //buttons
            $('#clientReasonCodesBackButton').button();
            $('#clientReasonCodesBackButton').click(function () {
                if (hasPageBeenChanged("ClientReasonCodes")) {
                    showSaveDialog(function () {
                        SaveClientReasonCodes();
                        CommitClientReasonCodeChanges(function () {
                            ShowClientServicingOptionsScreen();
                        })
                    }, function () { ShowReasonCodesScreen(true); });
                } else {
                    ShowClientServicingOptionsScreen();
                }
            });
            $('#clientReasonCodesNextButton').button();
            $('#clientReasonCodesNextButton').click(function () {

                if (hasPageBeenChanged("ClientReasonCodes")) {
                    showSaveDialog(function () {
                        SaveClientReasonCodes();
                        CommitClientReasonCodeChanges(function () {
                            ChoosePolicyScreen();
                        })
                    }, function () { ShowReasonCodesScreen(true);});
                } else {
                    ChoosePolicyScreen();
                }
            });

            $('#clientReasonCodesSaveButton').button();
            $('#clientReasonCodesSaveButton').click(function () {
                showSaveDialog(function () {
                    SaveClientReasonCodes();
                    CommitClientReasonCodeChanges(function () {
                        ChoosePolicyScreen();
                    })
                }, function () { });
            });

            $('#clientReasonCodesCancelButton').button();
            $('#clientReasonCodesCancelButton').click(function () {
                ShowReasonCodesScreen(true);
            });



        	//$('#clientReasonCodesEditButton').button();
            //$('#clientReasonCodesEditButton').click(function () {
            //    $('#clientReasonCodesEditButton').html("<small>Please wait..</small>");
            //    ShowReasonCodesGridScreen();
            //});

            $('#clientReasonCodesSearchButton').button();
            $('#clientReasonCodesSearchButton').click(function () {
                DoClientReasonCodesSearch();
            });

            $("#InheritReasonCodesSwitch").click(function () {

                // If checked
                if ($("#InheritReasonCodesSwitch").is(":checked")) {
                    ShowReasonCodesScreen(true);
                } else {
                    ShowReasonCodesScreen(false);
                }
            });
        }
    });
}

/*
Show ReasonCodes GridScreen
Screen Removed 2.09.1

function ShowReasonCodesGridScreen() {
}
*/

/*
Add Loading Icon to Popups
*/
function loadingScreen() {
	return '<div style="text-align:center;margin: 40px 0;"><img src="/images/common/grid-loading.gif" alt="Loading"></div>';
}

/*
Add ProductReasonCodeType
*/
function AddProductReasonCodeType(groupId, productId, reasonCodeTypeId, product, reasonCodeType) {

    $('#source').val(source);
    $('#groupCount').val(parseInt(groupCount, 10) + 1);

    $("#dialog-confirm").load('../ClientWizard.mvc/ReasonCodesPopup?groupId=' + groupId + '&productId=' + productId + '&reasonCodeTypeId=' + reasonCodeTypeId).dialog({
        resizable: true,
        modal: true,
        height: 300,
        width: 600,
        buttons: {
			"Save": function () {
				SaveReasonCodeItem();
				$('#popupBody').html("");
				$(this).dialog("close");
			},
			"Close": function () {
				$('#popupBody').html("");
				$(this).dialog("close");
			}
		}
	});
	var modalTitle = "Add Reason Code Item";
	$('#dialog-confirm').dialog('option', 'title', modalTitle).html(loadingScreen());

}

/*
Edit ProductReasonCodeType
*/
function EditProductReasonCodeType(groupId, productId, reasonCodeTypeId, product, reasonCodeType, source, groupCount, reasonCode, reasonCodeItemId) {

	$("#dialog-confirm").load('../ClientWizard.mvc/ReasonCodesPopup?groupId=' + groupId + '&productId=' + productId + '&reasonCodeTypeId=' + reasonCodeTypeId + '&reasonCode=' + reasonCode).dialog({
		resizable: true,
		modal: true,
		height: 300,
		width: 600,
		buttons: {
			"Save": function () {
				EditReasonCodeItem(reasonCodeItemId);
				$('#popupBody').html("");
				$(this).dialog("close");
			},
			"Close": function () {
				$('#popupBody').html("");
				$(this).dialog("close");
			}
		}
	});
	var modalTitle = "Edit Reason Code Item";
	$('#dialog-confirm').dialog('option', 'title', modalTitle).html(loadingScreen());

}

/*
Remove ProductReasonCodeType
*/


//function RemoveProductReasonCodeType(groupId, productId, reasonCodeTypeId, product, reasonCodeType, source, groupCount, reasonCode, reasonCodeItemId, versionNumber, reasonCodeDescription, rowID, reasonCodeStatus) {
function RemoveProductReasonCodeType(groupId, productId, reasonCodeTypeId, reasonCode, versionNumber, reasonCodeItemId) {

	$('#groupCount').val(parseInt(groupCount, 10) + 1);

	$("#dialog-confirm").html("");
	$('#dialog-confirm').dialog('option', 'title', 'Remove Reason Code Item').html(loadingScreen());

    $("#dialog-confirm").load('../ClientWizard.mvc/ReasonCodesPopupDelete?id=' + Math.random() + '&groupId=' + groupId + '&productId=' + productId + '&reasonCodeTypeId=' + reasonCodeTypeId + '&reasonCode=' + reasonCode).dialog({
	    resizable: true,
		modal: true,
		height: 200,
		width: 400,
		buttons: {
			"Remove": function () {
			    RemoveReasonCodeItem(reasonCodeItemId);
			    $("#dialog-confirm").dialog("close");
			},
			"Close": function () {
				$("#dialog-confirm").dialog("close");
            }
        }
    });
}

/*
Save ReasonCodes to Hidden Inputs
*/
function SaveClientReasonCodes() {

    //addedClientReasonCodes	
    var addedClientReasonCodes = [];
    $('#hiddenAddedClientReasonCodesTable tr').each(function () {
        var ReasonCode = $(this).attr("reasonCode");
        var ProductId = $(this).attr("productID");
        var ReasonCodeTypeId = $(this).attr("reasonCodeTypeID");
        var ReasonCodeGroupId = $(this).attr("reasonCodeGroupID");

        addedClientReasonCodes.push({ ReasonCode: ReasonCode, ProductId: ProductId, ReasonCodeTypeId: ReasonCodeTypeId, ReasonCodeGroupId: ReasonCodeGroupId })
    });
    $('#AddedReasonCodes').val(JSON.stringify(addedClientReasonCodes));


	//removedClientReasonCodes	
    var removedClientReasonCodes = [];
    $('#hiddenRemovedClientReasonCodesTable tr').each(function () {
    	var VersionNumber = $(this).attr("versionNumber");
    	var ReasonCodeItemID = $(this).attr("reasonCodeItemID");

    	removedClientReasonCodes.push({ ReasonCodeItemID: ReasonCodeItemID, VersionNumber: VersionNumber });
    });

    $('#RemovedReasonCodes').val(JSON.stringify(removedClientReasonCodes));

	//removedClientReasonCodes	
    var changedClientReasonCodes = [];
    $('#hiddenChangedClientReasonCodesTable tr').each(function () {
    	var ReasonCode = $(this).attr("reasonCode");
    	var ProductId = $(this).attr("productID");
    	var ReasonCodeTypeId = $(this).attr("reasonCodeTypeID");
    	var ReasonCodeGroupId = $(this).attr("reasonCodeGroupID");
    	var VersionNumber = $(this).attr("versionNumber");
    	var ReasonCodeItemID = $(this).attr("reasonCodeItemID");

    	changedClientReasonCodes.push({ ReasonCode: ReasonCode, ProductId: ProductId, ReasonCodeTypeId: ReasonCodeTypeId, ReasonCodeGroupId: ReasonCodeGroupId, VersionNumber: VersionNumber, ReasonCodeItemID: ReasonCodeItemID });
    });

    $('#AlteredReasonCodes').val(JSON.stringify(changedClientReasonCodes));
}

function CommitClientReasonCodeChanges(successCb) {
    //Build Object to Store All Items
    var reasonCodeChanges = {
        ClientTopUnit: $.parseJSON(escapeInput($("#ClientTopUnit").val())),
        ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
        ReasonCodesAdded: $.parseJSON(escapeInput($("#AddedReasonCodes").val())),
        ReasonCodeItemsRemoved: $.parseJSON(escapeInput($("#RemovedReasonCodes").val())),
        ReasonCodesInherited: $.parseJSON(escapeInput($("#ReasonCodesInherited").val())),
        ReasonCodeItemsAltered: $.parseJSON(escapeInput($("#AlteredReasonCodes").val()))
    }

    $.ajax({
        type: "POST",
        data: JSON.stringify(reasonCodeChanges),
        url: '/ClientWizard.mvc/CommitReasonCodeChanges?id=' + Math.random(),
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

/*
* Show a list of Changes made by the user
*/
function ShowConfirmChangesScreen() {

    //Setup
    var url = '/ClientWizard.mvc/ConfirmChangesScreen?id=' + Math.random();
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-5Content").html("<p>" + ajaxLoading + " Loading...</p>");

    //Build Object to Store All Items
    var clientChanges = {
        ClientTopUnit: $.parseJSON(escapeInput($("#ClientTopUnit").val())),
        ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
        TeamsAdded: $.parseJSON(escapeInput($("#AddedTeams").val())),
        TeamsRemoved: $.parseJSON(escapeInput($("#RemovedTeams").val())),
        TeamsAltered: $.parseJSON(escapeInput($("#AlteredTeams").val())),
        ClientAccountsAdded: $.parseJSON(escapeInput($("#AddedClientAccounts").val())),
        ClientAccountsRemoved: $.parseJSON(escapeInput($("#RemovedClientAccounts").val())),
        ServicingOptionItemsAdded: $.parseJSON(escapeInput($("#AddedServicingOptions").val())),
        ServicingOptionItemsRemoved: $.parseJSON(escapeInput($("#RemovedServicingOptions").val())),
        ServicingOptionItemsAltered: $.parseJSON(escapeInput($("#AlteredServicingOptions").val())),
        ReasonCodesAdded: $.parseJSON(escapeInput($("#AddedReasonCodes").val())),
        ReasonCodeItemsRemoved: $.parseJSON(escapeInput($("#RemovedReasonCodes").val())),
        ReasonCodesInherited: $.parseJSON(escapeInput($("#ReasonCodesInherited").val())),
        ReasonCodeItemsAltered: $.parseJSON(escapeInput($("#AlteredReasonCodes").val())),
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

    //AJAX (JSON) POST of Client Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(clientChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        error: function () {

            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 6);
            $tabs.tabs('select', 6); // switch to 5th tab
            $("#tabs-7Content").html("Apologies but an error occurred");

        },
        success: function (result) {

            var $tabs = $('#tabs').tabs();
            $('#tabs').tabs('enable', 7);
            $tabs.tabs('select', 7); // switch to 5th tab
            $("#tabs-8Content").html(result.html);

            var changesMade = $('#changesMade').val();

            if (changesMade > 0) {

                //Next Button
                $("#confirmChangesNextButton").button();
                $("#confirmChangesNextButton").click(function (e) {

                    $('#waitingSpan').html("<img src='images/common/grid-loading.gif' align='left'>...please wait");

                    CommitChanges();
                });

                //Confirm Button
                $("#confirmChangesBackButton").button();
                $("#confirmChangesBackButton").click(function (e) {
                    ChoosePolicyScreen()
                });

                //Cancel Button
                $("#cancelAllChanges").button();
                $("#cancelAllChanges").click(function (e) {
                    ShowClientSelectionScreen();
                });
            } else {
                $('#changesSummary').html("<h3>You made no changes to this client.</h3> <br /><br /><span id='BackToStart'><small>Client Wizard Home</small></span>");
                $("#confirmChangesNextButton").html("");
                $("#confirmChangesBackButton").button();
                $("#confirmChangesBackButton").click(function (e) {
                    ChoosePolicyScreen()
                });

                $("#BackToStart").button();
                $("#BackToStart").click(function () {
                    ShowClientSelectionScreen();
                });
            }

        }
    });
}



/*
*Permanently Commits Changes made in Wizard
*/
function CommitChanges() {

    //Setup
    var url = '/ClientWizard.mvc/CommitChanges';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-8Content").html("<p>" + ajaxLoading + " Loading...</p>");

    //Build Object to Store All Items
    var clientChanges = {
        ClientTopUnit: $.parseJSON(escapeInput($("#ClientTopUnit").val())),
        ClientSubUnit: $.parseJSON(escapeInput($("#ClientSubUnit").val())),
        TeamsAdded: $.parseJSON(escapeInput($("#AddedTeams").val())),
        TeamsRemoved: $.parseJSON(escapeInput($("#RemovedTeams").val())),
        TeamsAltered: $.parseJSON(escapeInput($("#AlteredTeams").val())),//clear pop-up form
        ClientAccountsAdded: $.parseJSON(escapeInput($("#AddedClientAccounts").val())),
        ClientAccountsRemoved: $.parseJSON(escapeInput($("#RemovedClientAccounts").val())),
        ServicingOptionItemsAdded: $.parseJSON(escapeInput($("#AddedServicingOptions").val())),
        ServicingOptionItemsRemoved: $.parseJSON(escapeInput($("#RemovedServicingOptions").val())),
        ServicingOptionItemsAltered: $.parseJSON(escapeInput($("#AlteredServicingOptions").val())),
        ReasonCodesAdded: $.parseJSON(escapeInput($("#AddedReasonCodes").val())),
        ReasonCodeItemsRemoved: $.parseJSON(escapeInput($("#RemovedReasonCodes").val())),
        ReasonCodesInherited: $.parseJSON(escapeInput($("#ReasonCodesInherited").val())),
        ReasonCodeItemsAltered: $.parseJSON(escapeInput($("#AlteredReasonCodes").val())),
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

    //AJAX (JSON) POST of Team Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(clientChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            if (result.success) {
                ClearHiddenFormVariables();
            }
            //Disable UnUsed Tabs
            var $tabs = $('#tabs').tabs();
            $("#tabs").tabs({ disabled: [1, 2, 3, 4, 5, 6] });

            //Display Result in Current tab
            $("#tabs-8Content").html(result.html);
            $("#BackToStart").button();
            $("#BackToStart").click(function () {
                ShowClientSelectionScreen("");
            });
        }
    });
}

/*
For Popup, when a ServicingOption is selected
Load ServicingOptionValues into a SelectList, if no Values, show the textboxes for freetext entry
*/
function LoadServicingOptionValues() {

    //no item selected
    if ($("#ServicingOptionId").val() == "") {
        $('#trGDSs').css('display', 'none');
        $("#selServicingOptionItemValue").find('option').remove();
        $('#txtServicingOptionItemValue').val("");
        $('#txtServicingOptionItemValue').attr('disabled', 'disabled');
        return;
    }

	//Check if ServicingOption requires a GDS adding
    $.ajax({
    	url: "/ServicingOptionItemValue.mvc/GetServicingOptionGDSRequired", type: "POST", dataType: "json",
    	data: { servicingOptionId: $("#ServicingOptionId").val() },
    	success: function (data) {
    		if (data == true) {
    			$('#trGDSs').css('display', '');
    		} else {
    			$('#trGDSs').css('display', 'none');
    		}
    	}
    });

	//Check if ServicingOption requires Parameters adding
    $.ajax({
    	url: "/ServicingOptionItemValue.mvc/GetServicingOptionParametersRequired", type: "POST", dataType: "json",
    	data: { servicingOptionId: $("#ServicingOptionId").val() },
    	success: function (data) {
    		if (data == true) {
    			$('.parameter-fields').css('display', '');
    		} else {
    			$('.parameter-fields').css('display', 'none');
    		}
    	}
    });

    //item selected
    $.ajax({
        url: "/ServicingOptionItemValue.mvc/GetClientSubUnitServicingOptionItemValues", type: "POST", dataType: "json",
        data: { servicingOptionId: $("#ServicingOptionId").val(), id: $('#ClientSubUnitGuid').val() },
        success: function (data) {
            if (data.length == 0) { //show text box
                $('#txtServicingOptionItemValue').attr('disabled', '');
                $('#trTextboxServicingOptionItemValue').css('display', '');
                $('#trSelectListServicingOptionItemValue').css('display', 'none');
                $("#selServicingOptionItemValue").find('option').remove();
            } else { //show select listbox
                $('#txtServicingOptionItemValue').attr('disabled', 'disabled');
                $('#trTextboxServicingOptionItemValue').css('display', 'none');
                $('#trSelectListServicingOptionItemValue').css('display', '');

                $("#selServicingOptionItemValue").find('option').remove();
                $("<option value=''>Please Select...</option>").appendTo($("#selServicingOptionItemValue"));
                $(data).each(function () {
                    //$("<option value=" + this.ServicingOptionId + ">" + this.ServicingOptionItemValue1 + "</option>").appendTo($("#selServicingOptionItemValue"));
                    if ($("#ServicingOptionId").val() == "180" && this.ServicingOptionItemValue1 == "Exact" && $('#ServicingOptionItemValue').val() == "") {

                        $("<option value=" + this.ServicingOptionId + " selected>" + this.ServicingOptionItemValue1 + "</option>").appendTo($("#selServicingOptionItemValue"));

                    } else {
                        $("<option value=" + this.ServicingOptionId + ">" + this.ServicingOptionItemValue1 + "</option>").appendTo($("#selServicingOptionItemValue"));
                    }
                });
            }
        }
    })

}