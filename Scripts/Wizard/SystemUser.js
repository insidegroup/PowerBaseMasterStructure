$(document).ready(function () {

    //prevent IE from caching
    $.ajaxSetup({ cache: false });

	$('#selectASystemUser').click(function(){
	    $('#tabs').tabs('enable', 0);
	    $("#tabs").tabs({ disabled: [1, 2, 3] });
   	    $('#tabs').tabs('select', 0);
   	    ShowSystemUserSelectionScreen("");
	});

   	$("#tabs").tabs({ disabled: [1, 2, 3] });

    ShowSystemUserSelectionScreen("");
	
    $('#wizardMenu').change(function () {
    	var newSelection = escapeInput($(this).val());
    	var valid_locations = ["Home.mvc", "SystemUserWizard.mvc", "TeamWizard.mvc", "LocationWizard.mvc", "ClientWizard.mvc"];
    	if (newSelection != 0 && valid_locations.indexOf(newSelection) != -1) {
    		window.location = newSelection;
    	}
    	return false;
    });
	
	//prevent default form action on enter for IE	
	$(window).keydown(function(event){
		
		if(!event) var event = window.event;
        if(event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

    $("#Filter").keyup(function(e) {
        if(e.keyCode == 13) {
            $('#lastSearchL').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");
            DoSystemUserSearch();
		}
	});

    $("#dialog-confirm").hide();

});



/*
Show lsiting of SystemUsers and Sewrch OPtions
*/
function ShowSystemUserSelectionScreen(filter) {
	$('#waitingSpan').html("");
    //page title
    $('#currentUser').html("");

	//set inputs to start values
    ClearHiddenFormVariables();
	
	$("#tabs").tabs({ disabled: [1, 2, 3] });
   	$('#tabs').tabs('select', 0);
	
    var url = '/SystemUserWizard.mvc/SystemUserSelectionScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-1Content").html("<p>" + ajaxLoading + " Loading...</p>");

    //AJAX POST of filter
    $.ajax({
        type: "POST",
        data: { filter: encodeURI(filter) },
        url: url,
        success: function (result) {
            var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 0);

            $("#tabs-1Content").html(result);
            $('#SearchButton').button();

            $("#Filter").keyup(function (e) {
                if (e.keyCode == 13) {
                    $('#lastSearchL').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");
                    DoSystemUserSearch();
                }
            });

			$('#SearchButton').click(function(){
				DoSystemUserSearch();
			});
		},
		error: function () {
			var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 0);

            $("#tabs-1Content").html("There was an error retrieving system users");
		}
	});
}

/*
Do Search
*/
function DoSystemUserSearch() {
	$('#Filter').removeClass("ui-state-active");
	
	var userFilter = $('#Filter').val();
	if(!userFilter) {
		$('#Filter').addClass("ui-state-active");
		$('#Filter').focus();	
		alert("Please supply something - at least 1 character - for the user filter value!");
		return;
	}
	
    $("#lastSearchTS").html("<th colspan='5'><img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...</th>");
    var url = '/SystemUserWizard.mvc/SystemUserSearch';

    $.ajax({
        type: "POST",
        data: { filterField: $('#FilterField').val(), filter: encodeURI($('#Filter').val()) },
        url: url,
        success: function (result) {
			
            $("#SystemUserSearchResults").html(result);
			$("#teamUsersSearchResult").tablesorter({ widgets: ['zebra'] });
			
			$("#lastSearchTS").html("");
		},
		error: function () {
			$("#lastSearchTS").html("Apologies but an error occured.");
		}
	});
}

/*
When User selects A SystemUser
*/
function setWizardUser(userID) {
	
	$('#SystemUserGuid').val(userID);
    $('#tabs').tabs("enable", 1);
    ShowSystemUserDetailsScreen();	
}

/*
Detail of SystemUser
*/
function ShowSystemUserDetailsScreen() {

	$('#tabs').tabs("select", 1);
	var url = '/SystemUserWizard.mvc/SystemUserDetailsScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-2Content").html("<p>" + ajaxLoading + " Loading...</p>");

    $.ajax({
        type: "POST",
        data: { systemUserGuid: $('#SystemUserGuid').val() },
        url: url,
        success: function (result) {


            $("#tabs-2Content").html(result);
            $("#allGDSs").tablesorter({ widgets: ['zebra'] });


            $('#BackOfficeAgentID').change(function () {

                var whiteSpaceExp = /\s/g;
                var BackOfficeAgentID = $('#BackOfficeAgentID').val();
                BackOfficeAgentID = $.trim(BackOfficeAgentID);

                if (BackOfficeAgentID.length > 0) {

                    alert("you've changed back office agent id");
                    if (whiteSpaceExp.test($('#BackOfficeAgentID').val())) {
                        alert("it contains spaces");

                    }
                }
            });

            //mark or unmark PseudoCityOrOfficeId as required if necessary
            $('input[id^="GDSSignOnId_"]').change(function () {
                var inputName = $(this).attr('id');
                var lblPseudoCity = "#lblPseudoCityOrOfficeId_" + inputName.split("GDSSignOnId_")[1];
                if ($(this).val() == "") {
                    $(lblPseudoCity).text("");
                } else {
                    $(lblPseudoCity).text("*");
                }
            });

            //INCLUDED IN BOTH ShowSystemUserDetailsScreen() and ValidatesytemUserDetails();
            //AutoComplete of Lcoations
            $("#SystemUser_LocationName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/AutoComplete.mvc/SystemUserLocations", type: "POST", dataType: "json",
                        data: { searchText: request.term },
                        success: function (data) {
                            response($.map(data, function (item) {

                                return {
                                    label: escapeHTML(item.ParentName) + " - " + escapeHTML(item.HierarchyName),
                                    value: item.HierarchyName,
                                    id: item.HierarchyCode
                                }
                            }))
                        }
                    })
                },
                search: function (event, ui) {
                    $("#SystemUser_LocationId").val("");
                },
                select: function (event, ui) {
                    $("#SystemUser_LocationId").val(ui.item.id);
                }
            });


            //INCLUDED IN BOTH ShowSystemUserDetailsScreen() and ValidatesytemUserDetails();
            $("#userGDSChecks").html("<img src='images/common/grid-loading.gif' align='left'>");
            //now check which of the "ALL GDSs" the user is using by looping through the hidden userGDS table:

            $('#userCurrentGDSs tr').each(function () {
                var userGDSCode = $(this).attr("id");
                var PseudoCityOrOfficeId = $(this).attr("pseudocityorofficeid");
                var GDSSignOnId = $(this).attr("gdssignonid");
                var isGDSDefault = $(this).attr("isDefault");

                $('#allGDSs tbody tr input').each(function () {

                    $("#PseudoCityOrOfficeId_" + userGDSCode).val(PseudoCityOrOfficeId);
                    $("#GDSSignOnId_" + userGDSCode).val(GDSSignOnId);

                    var allGDSCode = $(this).attr("gdsCode");
                    if (allGDSCode) {
                        var radiobox = "#radio_" + allGDSCode



                        if (allGDSCode == userGDSCode) {

                            $(this).attr("checked", "checked");

                            if (isGDSDefault == "True") {
                                //var radiobox = "#radio_"+allGDSCode
                                $(radiobox).attr("checked", "checked");
                            }
                        }
                    }
                });

            });

            //ensure that only radio buttons next to check boxes that are checked, are enabled
            $('#allGDSs tbody tr input').each(function () {
                if ($(this).attr("type") == "radio") {

                    //check if the associating checkbox is checked. if not, user cannot make this selection (cannot have default if not ticked)

                    var gdsCode = $(this).attr("radiogdsCode");
                    if ($("#" + gdsCode).is(":checked")) {
                        //great, all ok
                    }
                    else {

                        $(this).attr("disabled", "disabled");
                        $("#PseudoCityOrOfficeId_" + gdsCode).attr("disabled", "disabled");
                        $("#GDSSignOnId_" + gdsCode).attr("disabled", "disabled");

                    }


                }

            });

            // check on change. first update userGDSChanges to 1, then ensure if it's a checkbox the radio is enabled, if checked, and vice versa if not
            $('#allGDSs tbody tr input').change(function () {

            	$('#userGDSChanges').val("1");

            	if ($(this).attr("type") == "checkbox") {

            		var gdsCode = $(this).attr("gdsCode");
            		var radioB = "#radio_" + gdsCode;
            		var pseudoCityOrOfficeIdTxt = "#PseudoCityOrOfficeId_" + gdsCode;
            		var gdsSignOnIdTxt = "#GDSSignOnId_" + gdsCode;

            		if ($(this).is(":checked")) {
            			$(radioB).attr("disabled", "");
            			$(pseudoCityOrOfficeIdTxt).attr("disabled", "");
            			$(gdsSignOnIdTxt).attr("disabled", "");
            		}
            		else {
            			$(radioB).attr("checked", "");
            			$(radioB).attr("disabled", "disabled");
            			$(pseudoCityOrOfficeIdTxt).val("");
            			$(pseudoCityOrOfficeIdTxt).attr("disabled", "disabled");
            			$(gdsSignOnIdTxt).val("");
            			$(gdsSignOnIdTxt).attr("disabled", "disabled");
            		}
            	}
            });
            $("#userGDSChecks").html("");


        	//ExternalSystemLoginTable
            $('#ExternalSystemLoginTable .IsDefaultFlag').click(function () {
            	$('#ExternalSystemLoginTable .IsDefaultFlag').attr('checked', false);
            	$(this).attr('checked', 'checked');
            });

        	//Country Code Selection Reset
            $('#ExternalSystemLoginTable .countryCode').each(function (e) {

            	var selected = $('option:selected', this).val();
            	if (selected == "") {
            		$(this).find('option').attr('disabled', false);
            	}

            	//Disable same option in other select boxes
            	$('#ExternalSystemLoginTable .countryCode').not(this).find('option').each(function () {
            		if ($(this).val() == selected && $(this).val() != '') {
            			$(this).attr('disabled', true);
            		}
            	});
            });

        	//Country Code Selection Reset
            $('#ExternalSystemLoginTable .countryCode').live('change', function (e) {

            	var selected = $('option:selected', this).val();
            	if (selected == "") {
            		$(this).find('option').attr('disabled', false);
            	}

				//Disable same option in other select boxes
            	$('#ExternalSystemLoginTable .countryCode').not(this).find('option').each(function () {
            		if ($(this).val() == selected && $(this).val() != '') {
            			$(this).attr('disabled', true);
            		}
            	});
            });

			//Add btn
            $('#ExternalSystemLoginTable .btn-add').live('click', function (e) {

            	e.preventDefault();
            	$('#ExternalSystemLoginRegexMessage').hide();
            	$('#ExternalSystemLoginDuplicateMessage').hide();

            	var validForm = true;
            	var validExternalSystemLoginNames = true;

            	//Prevent adding new lines until existing lines filled in
            	$('#ExternalSystemLoginTable tbody tr').each(function () {

            		var row = $(this);
            		var externalSystemLoginName = row.find('.externalSystemLoginName').val();
            		var countryCode = row.find('.countryCode option:selected').val();

            		if (externalSystemLoginName == '' || countryCode == '') {
            			validExternalSystemLoginNames = false;
            			$('#ExternalSystemLoginDuplicateMessage').show();
            		}
            	});
				
            	if (validExternalSystemLoginNames) {

            		//Check alphanumeric
            		$('#ExternalSystemLoginTable tbody tr').each(function () {

            			var row = $(this);
            			var externalSystemLoginName = row.find('.externalSystemLoginName').val();

            			var validExternalSystemLogins_regex = new RegExp("^[a-zA-Z0-9]*$");
            			if (!externalSystemLoginName.match(validExternalSystemLogins_regex)) {
            				validForm = false;
            				$('#ExternalSystemLoginRegexMessage').show();
            			}
            		});

            		//Show Errors
            		if (validForm) {

            			//Clone last row and reset values if existing items complete
            			var clonedItem = $('#ExternalSystemLoginTable tbody tr').last().clone();
            			clonedItem.find(".externalSystemLoginName").val('');
            			clonedItem.find(".isDefaultFlag").attr('checked', false);
            			clonedItem.find(".versionNumber").val('1');
            			$('#ExternalSystemLoginTable tbody tr').last().after(clonedItem);
            			
            			//Update Country Code
            			var countryCode = clonedItem.find('.countryCode');
            			$('#ExternalSystemLoginTable .countryCode').not(countryCode).find('option:selected').each(function () {
            				var selectedItem = $(this).val();
            				$('option', countryCode).each(function () {
            					if ($(this).val() == selectedItem && $(this).val() != '') {
            						$(this).attr('disabled', true);
            					}
            				});
            			});

            			//Reset Dropdown
            			countryCode.val("");
            		}
            	}

            	return false;
            });

        	//Remove btn
            $('#ExternalSystemLoginTable .btn-remove').live('click', function (e) {
            	e.preventDefault();

            	$('#ExternalSystemLoginDuplicateMessage').hide();

            	var row = $(this).parent().parent();

            	//Reset Country
            	var selectedItem = row.find('.countryCode option:selected').val();
            	if (selectedItem != '') {
            		$('#ExternalSystemLoginTable .countryCode option').each(function () {
            			if ($(this).val() == selectedItem) {
            				$(this).attr('disabled', false);
            			}
            		});
            	}

            	//Reset row if only row, otherwise delete row
            	var number_of_rows = $('#ExternalSystemLoginTable tbody tr').length;
            	if (number_of_rows == 1) {

            		//Reset item
            		var item = $('#ExternalSystemLoginTable tbody tr:eq(' + 0 + ')');
            		item.find(".externalSystemLoginName").val('');
            		item.find(".countryCode").val('');
            		item.find(".isDefaultFlag").attr('checked', false);
            		item.find(".versionNumber").val('1');

            	} else {

            		//Remove item
            		row.remove();
            	}

            	return false;

            });

            $('#systemUserDetailsBackButton').button();
            $('#systemUserDetailsBackButton').click(function () {
                $('#tabs').tabs("select", 0);

            });

            $('#systemUserDetailsNextButton').button();
            $('#systemUserDetailsNextButton').click(function () {
                ValidateSystemUserDetails();
            });


            //grab the system user data for the header
            var systemUserHeader = escapeInput($('#systemUserHeader').val());
            $('#currentUser').html(": " + systemUserHeader);

            $("#systemUserEditTable").tablesorter({ widgets: ['zebra'] });

        },
        error: function () {
        	$("#tabs-2Content").html("Sorry but an error occured.");
        }

    });
}

/*
Clear Hidden Form Variables
*/
function ClearHiddenFormVariables() {

	$('#frmSystemUserWizard input').each(function(){
		$(this).val("");
	});
    $('#userGDSChanges').val("0");
    $('#addedTeams tr').each(function () {
        $(this).remove();
    });
    $('#removedTeams tr').each(function () {
        $(this).remove();
    });
}

//Default Profile
$(function () {
	$("#DefaultProfileIdentifier").live("change", function () {
		if (this.checked) {
			if (!$('#lblDefaultProfileIdentifierMsg').is(":visible")) {
				checkDefaultProfile();
			}
		} else {
			$('#lblDefaultProfileIdentifierMsg').text('').hide();
		}
	});

});

function checkDefaultProfile() {
	$.ajax({
		url: "/SystemUserWizard.mvc/DefaultProfileIdentifierExist",
		type: "POST",
		dataType: "json",
		data: { systemUserGuid: $('#SystemUserGuid').val() },
		success: function (data) {

			//Returns the profile with the default profile flag checked (if set)
			if (data == '') {
				$('#lblDefaultProfileIdentifierMsg').text('').hide();
			} else {
				$('#lblDefaultProfileIdentifierMsg')
					.text('A default profile is already set for ' + data + '. Check this box to change the default to this profile.')
					.show();
				$("#DefaultProfileIdentifier").attr('checked', false);
				return false;
			}
		}
	});
}

/*
*Validate System User Details 
*/
function ValidateSystemUserDetails() {

	var validLocation = false;
	var validPseudoCityOrOfficeId = true;
	var validGDSSignOnId = true;
	var profileData = false;
	var validExternalSystemLogins = true;

    //loop PseudoCityOrOfficeId fields and check against regex (optional - can be blank or alpahanumeric)
    var re = new RegExp("^[a-zA-Z0-9]*$");
    $("[id^=PseudoCityOrOfficeId_]").each(function () {
        var labelID = "#lbl" + $(this).attr('id');
        if (!$(this).val().match(re)) {
            $(labelID).text("must be alphanumeric");
            validPseudoCityOrOfficeId = false;
        } else {
            $(labelID).text("");
        }
    });

    //PseudoCityOrOfficeId is optional (can be blank or 3-9 alpahanumeric)
    //GDS Sign On is optional (can be blank or 2-10 alpahanumeric)
    //if GDS Sign On has value, then PseudoCityOrOfficeId is mandatory (3-9 alpahanumeric)

    var re = new RegExp("^[a-zA-Z0-9]*$");
    $("[id^=GDSSignOnId_]").each(function () {                              //loop GDS
        if ($(this).val() != "") {                                          //if GDS non-blank then PseudoCityOrOfficeId
            var labelID = "#lbl" + $(this).attr('id');
            if (!$(this).val().match(re)) {
                $(labelID).text("must be alphanumeric");                    //check alphanumeric
                validGDSSignOnId = false;
            } else {
                $(labelID).text("");
            }
            if ($(this).val().length < 2) {
                $(labelID).text("Required. must be > 2 characters");        //check length
                validGDSSignOnId = false;
            }

            //check corresponding PseudoCityOrOfficeId - begin
            PseudoCityOrOfficeLabelId = "#PseudoCityOrOfficeId" + labelID.substring(15);

            if ($(PseudoCityOrOfficeLabelId).val() == "") {
                var label2ID = "#lblPseudoCityOrOfficeId" + labelID.substring(15);
                $(label2ID).text("Required. Must be alphanumeric");                 //check PCC alphanumeric
                validPseudoCityOrOfficeId = false;
            }
            if ($(PseudoCityOrOfficeLabelId).val().length < 2) {
                var label2ID = "#lblPseudoCityOrOfficeId" + labelID.substring(15);           //check PCC length
                $(label2ID).text("Required. Must be > 2 characters");
                validPseudoCityOrOfficeId = false;
            }


        } else {

            //if GDS blank then PseudoCityOrOfficeId is non-mandatory
            var labelID = "#lbl" + $(this).attr('id');
            PseudoCityOrOfficeLabelId = "#PseudoCityOrOfficeId" + labelID.substring(15);
            if ($(PseudoCityOrOfficeLabelId).val() != "") {
                if (!$(PseudoCityOrOfficeLabelId).val().match(re)) {
                    var label2ID = "#lblPseudoCityOrOfficeId" + labelID.substring(15);
                    $(label2ID).text("Must be alphanumeric");                               //check PCC alphanumeric
                    validPseudoCityOrOfficeId = false;
                } else {
                    if ($(PseudoCityOrOfficeLabelId).val().length < 2) {
                        var label2ID = "#lblPseudoCityOrOfficeId" + labelID.substring(15);           //check PCC length
                        $(label2ID).text("Must be > 2 characters");
                        validPseudoCityOrOfficeId = false;
                    }
                }
            }
        }
    });


    $("#SystemUser_LocationName").removeClass("ui-state-active");
    
    if ($("#SystemUser_LocationName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Hierarchy.mvc/IsValidLocation",
            data: { searchText: encodeURI($("#SystemUser_LocationName").val()) },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validLocation = true;
                    $("#SystemUser_LocationId").val(data[0].HierarchyCode);
                }
            },
            dataType: "json",
            async: false
        });
    }

    if (!validLocation) {
        $("#SystemUser_LocationName_validationMessage").removeClass('field-validation-valid');
        $("#SystemUser_LocationName_validationMessage").addClass('field-validation-error');
        if ($("#SystemUser_LocationName").val() != "") {
			$("#SystemUser_LocationName").addClass("ui-state-active");
			$("#SystemUser_LocationName").focus();
            $("#SystemUser_LocationName_validationMessage").text("This is not a valid Location.");
        } else {
			$("#SystemUser_LocationName").addClass("ui-state-active");
			$("#SystemUser_LocationName").focus();
            $("#SystemUser_LocationName_validationMessage").text("Location is Required.");
        }
        //return;
    } else {
        $("#SystemUser_LocationName_validationMessage").text("");
    }

	//ExternalSystemLogins
    $('#ExternalSystemLoginMessage').hide();
    $('#ExternalSystemLoginRegexMessage').hide();
    $('#ExternalSystemLoginTable tbody tr').each(function () {

    	var row = $(this);
    	var externalSystemLoginName = row.find('.externalSystemLoginName').val();
    	var countryCode = row.find('.countryCode option:selected').val();

    	//Check alphanumeric
    	var validExternalSystemLogins_regex = new RegExp("^[a-zA-Z0-9]*$");
    	if (!externalSystemLoginName.match(validExternalSystemLogins_regex)) {
    		$('#ExternalSystemLoginRegexMessage').show();
    		validExternalSystemLogins = false;
    		return;
    	}

    	if (externalSystemLoginName == '' && countryCode == '') {
    		if ($('#ExternalSystemLoginTable tbody tr').length > 1) {
    			row.remove();
    		}
    	} else if ((externalSystemLoginName != '' && countryCode == '') ||( externalSystemLoginName == '' && countryCode != '')) {
    		validExternalSystemLogins = false;
    		$('#ExternalSystemLoginMessage').show();
    	}
    });

    if (!validLocation || !validPseudoCityOrOfficeId || !validGDSSignOnId || !validExternalSystemLogins) {
        return;
    }

    // get the changes to user GDS, if any
    var userGDSChanges = $("#userGDSChanges").val();
    if(userGDSChanges > 0){
	    var defaultGDSCount = 0;
	    var userGDSs = [];
		
	    $('#allGDSs tbody tr input').each(function(){			
	    	if ($(this).attr("type") == "checkbox") {
	    		if ($(this).is(":checked")) {

	    			var gdsCode = $(this).attr("gdsCode");
	    			var radioB = "#radio_" + gdsCode;
	    			var PseudoCityOrOfficeId = $("#PseudoCityOrOfficeId_" + gdsCode).val();
	    			var GDSSignOn = $("#GDSSignOnId_" + gdsCode).val();

	    			//find the radio button and see if it's checked				
	    			if ($(radioB).is(":checked")) {
	    				userGDSs.push({ GDSCode: gdsCode, PseudoCityOrOfficeId: PseudoCityOrOfficeId, GDSSignOn: GDSSignOn, DefaultGDS: true })
	    				defaultGDSCount = 1;
	    			} else {
	    				userGDSs.push({ GDSCode: gdsCode, PseudoCityOrOfficeId: PseudoCityOrOfficeId, GDSSignOn: GDSSignOn, DefaultGDS: false })
	    			}
	    		}
	    	}
			
		    if(userGDSs.length>0){			
			    $('#SystemUserGDSs').val(JSON.stringify(userGDSs))				
		    }
	
	    });			
    }

	//Build Object to Store ExternalSystemLogins
    var userExternalSystemLogins = [];
    $('#ExternalSystemLoginTable tbody tr').each(function () {

    	var externalSystemLoginName = $(this).find(".externalSystemLoginName").val();
    	var countryCode = $(this).find(".countryCode").val();
    	var isDefaultFlag = $(this).find(".isDefaultFlag").is(":checked");
    	var versionNumber = $(this).find(".versionNumber").val();

    	if (externalSystemLoginName != '') {
    		userExternalSystemLogins.push({
    			ExternalSystemLoginName: externalSystemLoginName,
    			CountryCode: countryCode,
    			IsDefaultFlag: isDefaultFlag,
    			VersionNumber: versionNumber
    		});
    	}

    });

    $("#ExternalSystemLogins").val(JSON.stringify(userExternalSystemLogins));


    var url = '/SystemUserWizard.mvc/ValidateSystemUser';

    //Build Object to Store SystemUser
    var systemUser = {
    	SystemUserGuid: $("#SystemUserGuid").val(),
    	DefaultProfileIdentifier: $("#DefaultProfileIdentifier").is(":checked"),
        CubaBookingAllowanceIndicator: $("#CubaBookingAllowanceIndicator").is(":checked"),
        RestrictedFlag: $("#RestrictedFlag").is(":checked"),
        VersionNumber: $("#SystemUser_VersionNumber").val()
    };
    $("#SystemUser").val(JSON.stringify(systemUser));

    //Build Object to Store SystemUserLocation
    var systemUserLocation = {
        SystemUserGuid: $("#SystemUserGuid").val(),
        LocationId: $("#SystemUser_LocationId").val(),
        VersionNumber: $("#SystemUserLocation_VersionNumber").val()
    };
    $("#SystemUserLocation").val(JSON.stringify(systemUserLocation));

    //Build Object to Store All Items
    var systemUserChanges = {
    	SystemUserLocation: $.parseJSON(escapeInput($("#SystemUserLocation").val())),
        SystemUser: $.parseJSON(escapeInput($("#SystemUser").val())),
        ExternalSystemLoginSystemUserCountries: $.parseJSON(escapeInput($("#ExternalSystemLogins").val()))
    };    

    //AJAX (JSON) POST of Team Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(systemUserChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            
            
            if (!result.success) {
				
				
                /*
                Validation Failed - show Details Screen Again with Errors
                */
                $('#tabs').tabs("enable", 1);
                $('#tabs').tabs("select", 1);
                $("#systemUserDetailsDiv").html(result.html);


                $('#systemUserDetailsBackButton').button();
                $('#systemUserDetailsBackButton').click(function () {
                    $('#tabs').tabs("select", 0);

                });

                $('#systemUserDetailsNextButton').button();
                $('#systemUserDetailsNextButton').click(function () {
                    ValidateSystemUserDetails();
                });

                //INCLUDED IN BOTH ShowSystemUserDetailsScreen() and ValidatesytemUserDetails();
                //AutoComplete of Lcoations
                $("#SystemUser_LocationName").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "/AutoComplete.mvc/SystemUserLocations", type: "POST", dataType: "json",
                            data: { searchText: request.term },
                            success: function (data) {
                                response($.map(data, function (item) {

                                    return {
                                        label: escapeHTML(item.ParentName) + " - " + escapeHTML(item.HierarchyName),
                                        value: item.HierarchyName,
                                        id: item.HierarchyCode
                                    }
                                }))
                            }
                        })
                    },
                    search: function (event, ui) {
                        $("#SystemUser_LocationId").val("");
                    },
                    select: function (event, ui) {
                        $("#SystemUser_LocationId").val(ui.item.id);
                    }
                });

                $("#allGDSs").tablesorter({ widgets: ['zebra'] });

            } else {
                /*
                Validation Passed - show SystemUserTeams Screen
                */

                $('#tabs').tabs("enable", 2);
                $('#tabs').tabs('select', 2); // switch to 5th tab

                $("#tabs-3Content").html("please wait");
                $("#tabs-3Content").html(result.html);

				$("#currentTeamsTable").tablesorter({ widgets: ['zebra'] });

				 $('#currentTeamsTable img').click(function (e) {

                    var teamID = $(this).parent().parent().attr("id");
                    var teamStatus = $(this).parent().parent().attr("teamStatus");
					$(this).parent().parent().remove();
						
					$("#currentTeamsTable").tablesorter({ widthFixed: false, widgets: ['zebra'] });
						 
					if(teamStatus=="Current") {
	 	                    $('#removedTeams').append("<tr id='" + teamID + "'<td>test</td></tr>");						
					}else {
						//also remove from hidden addedTeams tables, which it will be in if status = NotCurrent (added above)
						$('#addedTeams tr').each(function(){
							if($(this).attr("id")==teamID){
								$(this).remove();
							}
						});
					}
                });

                //Next Button
                $("#temasInSystemUserNextButton").button();
                $("#temasInSystemUserNextButton").click(function (e) {
                    SaveSystemUserTeams();
                });

                //Back Button
                $("#temasInSystemUserBackButton").button();
                $("#temasInSystemUserBackButton").click(function (e) {
                    $('#tabs').tabs("enable", 1);
	                $('#tabs').tabs('select', 1); // switch to 5th tab
					$('#tabs').tabs("disable", 2);
                });

                //Search Button
                $("#TeamSearchButton").button();
                $("#TeamSearchButton").click(function () {
                    DoTeamSearch();
                });

            }
        },
		error: function(){
			
			$('#tabs').tabs("enable", 2);
	        $('#tabs').tabs('select', 2); // switch to 5th tab
			 $("#tabs-3Content").html("There was an error.");
		}
    });
}

/*
Search for Teams
*/
function DoTeamSearch() {
    //question: do we want to insist on at least one char?

    $("#lastSearchTRTeam").html("<th colspan='5'><img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...</th>");
    var url = '/SystemUserWizard.mvc/TeamSearch';
    $.ajax({
        type: "POST",
        data: { filterField: $('#TeamFilterField').val(), filter: encodeURI($('#TeamFilter').val()) },
        url: url,
        success: function (result) {
            $("#TeamSearchResults").html(result);
            $("#lastSearchTRTeam").html("");

            $("#teamUsersSearchResultTable").tablesorter({ widgets: ['zebra'] });

            
            $('#teamUsersSearchResultTable tr img').click(function (e) {
			
                var teamName = $(this).parent().parent().attr("teamName");
                var teamEmail = $(this).parent().parent().attr("teamEmail");
                var teamPhone = $(this).parent().parent().attr("teamPhone");
                var teamHierarchy = $(this).parent().parent().attr("teamHierarchy");
                var teamID = $(this).parent().parent().attr("id");

                //first check if this team is not already in the current users table

                var currentTeam = 0;

                $('#currentTeamsTable tr').each(function () {

                    if ($(this).attr("id") == teamID) {

                        currentTeam = 1;
                    }
                });

                if (currentTeam == 0) {


                    $(this).parent().parent().remove();
                    //alert(userGUID);

                    //add to hidden table
                    $('#addedTeams').append("<tr id='" + teamID + "'<td>test</td></tr>");
					$('#currentTeamsTable tbody tr').each(function() {
					if($(this).attr("id")=="noTeams") {
						$(this).remove();
					}
					});

$('#currentTeamsTable').append("<tr teamStatus='NotCurrent' id='" + teamID + "'><td>" + escapeHTML(teamName) + "</td><td>" + teamHierarchy + "</td><td><img src='../../images/remove.png' border='0' /></td></tr>");


                    $("#currentTeamsTable").tablesorter({ widthFixed: false, widgets: ['zebra'] });

                    $('#currentTeamsTable img').click(function (e) {

                        var teamID = $(this).parent().parent().attr("id");
                        var teamStatus = $(this).parent().parent().attr("teamStatus");
						$(this).parent().parent().remove();
						
						 $("#currentTeamsTable").tablesorter({ widthFixed: false, widgets: ['zebra'] });
						 
						if(teamStatus=="Current") {
							
	 	                     $('#removedTeams').append("<tr id='" + teamID + "'<td>test</td></tr>");						
							
						}
						else {
						//also remove from hidden addedTeams tables, which it will be in if status = NotCurrent (added above)
						
						$('#addedTeams tr').each(function(){
							if($(this).attr("id")==teammID){
							
								$(this).remove();
							}
						});
							
							
						}


                    });
                }

                else {
                    alert("user already in team!");
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
function SaveSystemUserTeams() {

    

	//addedTeams
	
	var addedTeamsAr = [];
	var at = 0;
	$('#addedTeams tr').each(function() {
		
		var teamID = $(this).attr("id");
		
			addedTeamsAr.push({teamId: teamID})
			at = at + 1;
		
	});
	
	if(at>0) {
		
		$('#AddedTeams').val(JSON.stringify(addedTeamsAr));
	}
	
	//removedTeams
	
	var removedTeamsAr = [];
	var rt = 0;
	$('#removedTeams tr').each(function() {
		
		var teamID = $(this).attr("id");
		
			removedTeamsAr.push({teamId: teamID})
			rt = rt + 1;
		
	});
	
	if(rt>0) {
		
		$('#RemovedTeams').val(JSON.stringify(removedTeamsAr));
	}

	
	ShowConfirmChangesScreen()
	
}


/*
 *Show a list of Changes made by the user
 */
function ShowConfirmChangesScreen() {

    //Setup
    var url = '/SystemUserWizard.mvc/ConfirmChangesScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-5Content").html("<p>" + ajaxLoading + " Loading...</p>");


     //Build Object to Store All Items
    var systemUserChanges = {
        SystemUserLocation: $.parseJSON($("#SystemUserLocation").val()),
        SystemUser: $.parseJSON($("#SystemUser").val()),
        ExternalSystemLoginSystemUserCountries: $.parseJSON($("#ExternalSystemLogins").val()),
		SystemUserGDSs: $.parseJSON($("#SystemUserGDSs").val()),
		TeamsRemoved: $.parseJSON($("#RemovedTeams").val()),
		TeamsAdded: $.parseJSON($("#AddedTeams").val()),
		GDSChanged: $.parseJSON($("#userGDSChanges").val())
		
    };

    //AJAX (JSON) POST of Team Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(systemUserChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            //Display Result in Current tab
            var $tabs = $('#tabs').tabs();
			$('#tabs').tabs("enable", 3);
            $tabs.tabs('select', 3); // switch to 4th tab
            $("#tabs-4Content").html(result.html);
			 $("#tabs").tabs({ disabled: [1, 2] });
			
			
			var changesMade = $('#changesMade').val();
			
			if(changesMade>0) {
			
            //Next Button
            $("#confirmChangesNextButton").button();
            $("#confirmChangesNextButton").click(function (e) {
			$('#waitingSpan').html("<img src='images/common/grid-loading.gif' align='left'> Please wait ...");
                CommitChanges();
            });

            //Confirm Button
            $("#confirmChangesBackButton").button();
            $("#confirmChangesBackButton").click(function (e) {
                $('#tabs').tabs('enable', 2);
                $tabs.tabs('select', 2);
                $("#tabs").tabs({ disabled: [1, 3] });
				$('#waitingSpan').html("");
            });
			
            //Cancel Button
            $("#cancelAllChanges").button();
			$("#cancelAllChanges").click(function (e) {
				
			    ShowSystemUserSelectionScreen("");
			});
			}
			else {
			$('#changesSummary').html("<h3>You made no changes to this user.</h3> <br /><br /><span id='BackToStart'><small>SystemUser Wizard Home</small></span>");
			$("#confirmChangesNextButton").html("");
			$("#confirmChangesBackButton").button();
            $("#confirmChangesBackButton").click(function (e) {
                $tabs.tabs('enable', 2);
			    $tabs.tabs('select', 2);
                $("#tabs").tabs({ disabled: [1, 3] });
            });
			
			$("#BackToStart").button();
			$("#BackToStart").click(function () {
			    ShowSystemUserSelectionScreen("");
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
    var url = '/SystemUserWizard.mvc/CommitChanges';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-5Content").html("<p>" + ajaxLoading + " Loading...</p>");

    //Build Object to Store All Items
    var systemUserChanges = {
        SystemUserLocation: $.parseJSON($("#SystemUserLocation").val()),
        SystemUser: $.parseJSON($("#SystemUser").val()),
        ExternalSystemLoginSystemUserCountries: $.parseJSON($("#ExternalSystemLogins").val()),
        SystemUserGDSs: $.parseJSON($("#SystemUserGDSs").val()),
        TeamsRemoved: $.parseJSON($("#RemovedTeams").val()),
        TeamsAdded: $.parseJSON($("#AddedTeams").val()),
        GDSChanged: $.parseJSON($("#userGDSChanges").val())
    };

    //AJAX (JSON) POST of Team Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(systemUserChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            if (result.success) {
                ClearHiddenFormVariables();

            }
            //Disable UnUsed Tabs
            var $tabs = $('#tabs').tabs();
            $("#tabs").tabs({ disabled: [1, 2] });

            //Display Result in Current tab
            $("#tabs-4Content").html(result.html);
            $("#BackToStart").button();
            $("#BackToStart").click(function () {
                ShowSystemUserSelectionScreen("");
            });
        }
    });
}

function escapeHTML(s) {

	s = s.replace(/&/g, '&amp;');
	s = s.replace(/</g, '&lt;');
	s = s.replace(/>/g, '&gt;');
	return s;
}