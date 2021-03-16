
$(document).ready(function () {
    //prevent IE from cahcing
    $.ajaxSetup({ cache: false });

    //add home menu button to header

    //$('#header').append('<span style="float:right; margin-right:6px; margin-top:95px;"><a href="../Home.mvc"><h4>MAIN MENU</h4></a></span>');

    $("#tabs").tabs({ disabled: [1, 2, 3, 4] });
    ShowTeamSelectionScreen("");
   

    $("#dialog-confirm").hide();

    //set function calls for each tab			

    $('#selectATeamLink').click(function () {
        $("#tabs").tabs({ disabled: [1, 2, 3, 4] });
        clearHiddenFormVariables();
        $('#currentTeam').html("");
        ShowTeamSelectionScreen("");


    });


    $('#teamDetailsLink').click(function () {

        $("#tabs").tabs({ disabled: [2, 3, 4] });
        //        $tabs.tabs('select', 1); // switch to 2ND tab	
        //		 if ($('#TeamId').val() == "") {
        //        alert("Please select a team first");
        //        ShowTeamSelectionScreen();
        //        return;
        //    }
        //		
    });
    $('#usersInTeamLink').click(function () {
        $("#tabs").tabs({ disabled: [3, 4] });
        //        $tabs.tabs('select', 2); // switch to 3RD tab
        //		
        //		if ($('#TeamId').val() == "") {
        //        alert("Please select a team first");
        //        ShowTeamSelectionScreen();
        //        return;
        //    }
        //		ValidateTeamDetails();		
    });
    $('#clientsInTeamLink').click(function () {
        $('#tabs').tabs("disable", 4);
        //        $tabs.tabs('select', 3); // switch to 4TH tab		
        //    });
        //    $('#finishLink').click(function () {
        //        var $tabs = $('#tabs').tabs();
        //        $tabs.tabs('select', 4); // switch to 5TH tab		
    });
	
	//quick switch menu selector
	
    $('#wizardMenu').change(function () {
    	var newSelection = escapeInput($(this).val());
    	var valid_locations = ["Home.mvc", "SystemUserWizard.mvc", "TeamWizard.mvc", "LocationWizard.mvc", "ClientWizard.mvc"];
    	if (newSelection != 0 && valid_locations.indexOf(newSelection) != -1) {
    		window.location = newSelection;
    	}
    	return false;
    });
	
	//prevent form action on enter
		$(window).keydown(function(event){
		
		if(!event) var event = window.event;
    if(event.keyCode == 13) {
      event.preventDefault();
      return false;
    }
  });

});

/*
 *Clears Hidden Form Variables (Team, SystemUserList, ClientSubUnitList)
 */
function clearHiddenFormVariables(){


    $('#frmTeamWizard input').each(function() {
		$(this).val("");
	});
	
	$('#removedTeamMembers tr').each(function () {
		$(this).remove();
	});
	$('#addedTeamMembers tr').each(function () {
		$(this).remove();
	});	
	
		$('#changedClientSubUnits tr').each(function () {
		$(this).remove();
	});	
	
	
}
/*
 *Shows a List Of teams - can be filtered on TeamName
 */
function ShowTeamSelectionScreen(filter) {
$('#waitingSpan').html("");
    //Setup
    var url = '/TeamWizard.mvc/TeamSelectionScreen';
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
			$("#currentTeam").html("");
            $("#teamTable").tablesorter({
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

            $("#tabs").tabs({ disabled: [1, 2, 3, 4] });
			$('#lastSearchTS').html("");
			
			 clearHiddenFormVariables();
			 

            $("#SearchButton").button();
       
            $("#SearchButton").click(function () {
			$('#lastSearchTS').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");
			
				var filter = $('#TSFilter').val()
				ShowTeamSelectionScreen(filter);

            });

            $("#TSFilter").keyup(function (e) {
                if (e.keyCode == 13) {
                    $('#lastSearchTS').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");
                    var filter = $('#TSFilter').val()
                    ShowTeamSelectionScreen(filter);
                }
            });

            $("#createTeam").button();
            $("#createTeam").click(function () {
                SetWizardTeam(0);
            });
        }
    });
}




/*
 *Choose a New Team
 *Called when a User Selects a Team from the List, it clears the current Team
 */
function SetWizardTeam(teamId) {
    var $inputs = $('#frmTeamWizard :input');
    var teamAlreadySelected = false;
    $inputs.each(function () {
        if ($(this).val() != "") {
            teamAlreadySelected = true;
        }
    });
        if(teamAlreadySelected){
            if (!confirm("You have already selected a Team, if you continue you will lose unsaved information. Click OK to continue or Cancel to return to existing Team")) {
            return;
        }
    }
    clearHiddenFormVariables();
    $('#TeamId').val(teamId);
    $('#tabs').tabs("enable", 1);
    ShowTeamDetailsScreen();
  }

//Shows Team Details for Edit/Add/Delete
function ShowTeamDetailsScreen() {
    //alert("fn ShowTeamDetailsScreens")
    //we can't just check for empty team id in request..what if the user selected a team but wants to check the list again then go back to edit screen?
    if ($('#TeamId').val() == "") {
        alert("Please select a team first");
        ShowTeamSelectionScreen("");
        return;
    }


    var $tabs = $('#tabs').tabs();
    $tabs.tabs('select', 1); // switch to 2nd tab

    var url = '/TeamWizard.mvc/TeamDetailsScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-2Content").html("<p>" + ajaxLoading + " Loading...</p>");


    $.ajax({
        type: "POST",
        data: { teamId: $('#TeamId').val() },
        url: url,
		error: function () {
			$("#tabs-2Content").html("Apologies but an error occured.");
		},
        success: function (result) {

            $("#tabs-2Content").html(result);
			
            //$("#teamEditTable").ready(function () {
                
                //Hierarchy Disable/Enable OnLoad
                if ($("#HierarchyType").val() == "") {
                    $("#HierarchyItem").val("");
                    ///$("#HierarchyItem").attr("disabled", true);
                } else {
                    $("#HierarchyItem").removeAttr("disabled");
                }
                //Hierarchy Disable/Enable OnChange
                $("#HierarchyType").change(function () {
                    $("#lblHierarchyItemMsg").text("");
                    $("#HierarchyItem").val("");

                    if ($("#HierarchyType").val() == "") {
                        ///$("#HierarchyItem").attr("disabled", true);
                    } else {
                        ///$("#HierarchyItem").removeAttr("disabled");
                        $("#lblHierarchyItem").text($("#HierarchyType").val());
                        $("#HierarchyCode").val("");
                    }
                });

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
                                            label: item.Name + " (" + item.CityCode + ")",
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

				//setting a value for this so we can change it to 1 if select is from match
					var locationAutoCompleteSelect = 0;
					
                    $("#HierarchyItem").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: "/Team.mvc/AutoCompleteHierarchies", type: "POST", dataType: "json",
                                data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val() },
                                success: function (data) {
									//alert(response);
                                    response($.map(data, function (item) {
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
                        select: function (event, ui) {
                            $("#lblHierarchyItemMsg").text(ui.item.text);
                            $("#HierarchyItem").val(ui.item.value);
                            $("#HierarchyCode").val(ui.item.id);
							locationAutoCompleteSelect = 1;
							
                        },
						 mustMatch: true,
						 autoFill: true
                    //});
                });

            

            var currentTeam = "New Team";
            if ($('#TeamId').val() > 0 ) {
            	var currentTeam = escapeInput($('#TeamName').val());
            }
            $('#currentTeam').html(": " + escapeHTML(currentTeam));


            $("#teamEditTable").tablesorter({
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

		
            //$("#tabs").tabs({ disabled: [2, 3, 4] });
			
	
            //make the next span a button
            $("#teamDetailsNextButton").button();
            $("#teamDetailsNextButton").click(function () {
                ValidateTeamDetails();
            });
            $("#teamDetailsBackButton").button();
            $("#teamDetailsBackButton").click(function () {
                ShowTeamSelectionScreen("");
            });

            //make the delete team span a nice button

            if ($('#TeamId').val() == "0") {
                $("#deleteTeam").hide();
            }
            else {

                $("#deleteTeam").button();
                $("#deleteTeam").click(function (e) {


                    //different buttons depending on SystemUserCount
                    var arrButtons = {};
                    if ($('#SystemUserCount').val() == "0" && $('#ClientSubUnitCount').val() == "0") {

                        arrButtons["Continue"] = function () {
                            $(this).dialog("close");
                            ConfirmDelete2();
                        };
                        arrButtons["Cancel"] = function () {
                            $(this).dialog("close");
                        };
                    } else {
                        arrButtons["Cancel"] = function () {
                            $(this).dialog("close");
                        };
                    }

                    var teamId = escapeInput($('#TeamId').val());

                    $("#dialog-confirm").load('../TeamWizard.mvc/ShowConfirmDelete?teamId=' + teamId).dialog({
                        resizable: true,
                        modal: true,
						height: 500,
						width: 600,
						buttons: arrButtons
		               }, 'open');


                });

            }
        }
    });
	
	




}



/*
 *Validate Team Details 
 */
function ValidateTeamDetails() {

    var validItem = true;
    var validCity = true;

	//Validation of Phone
    var TeamPhoneNumber_validationMessage = $('#TeamPhoneNumber_validationMessage');
    TeamPhoneNumber_validationMessage.text('').addClass('error').hide();
    var TeamPhoneNumber = $("#TeamPhoneNumber").val();
    var TeamPhoneNumber_Reg = /^\d{0,20}$/;
    if (TeamPhoneNumber != '' && !TeamPhoneNumber_Reg.test(TeamPhoneNumber)) {
    	TeamPhoneNumber_validationMessage
			.text('Team Phone Number must be numberical (no spaces)')
			.show();
    	return false;
    }

	//Validation of City
    if ($("#CityCode").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidCityCode",
            data: { cityCode: $("#CityCode").val() },
            success: function (data) {

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

    //Validation of Hierarchy
    if ($("#HierarchyType").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Hierarchy.mvc/IsValid" + $("#HierarchyType").val(),
            data: { searchText: encodeURIComponent($("#HierarchyItem").val()) },
            success: function (data) {

                if (jQuery.isEmptyObject(data)) {
                    validItem = false;
                    $("#HierarchyCode").val(data[0].HierarchyCode);
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

    if (!validItem || !validCity) {
       return false;
    }
    var url = '/TeamWizard.mvc/ValidateTeam';

    //Build Object to Store Team
    var team = {
        TeamId: $("#TeamId").val(),
        TeamName: $("#TeamName").val(),
        TeamEmail: $("#TeamEmail").val(),
        TeamPhoneNumber: $("#TeamPhoneNumber").val(),
        TeamQueue: $("#TeamQueue").val(),
        TeamTypeCode: $("#TeamTypeCode").val(),
        CityCode: $("#CityCode").val(),
        HierarchyType: $("#HierarchyType").val(),
        HierarchyItem: $("#HierarchyItem").val(),
        HierarchyCode: $("#HierarchyCode").val(),
        TravelerTypeGuid: $("#TravelerTypeGuid").val(),
        ClientSubUnitGuid: $("#ClientSubUnitGuid").val(),
        TravelerTypeName: $("#TravelerTypeName").val(),
        ClientSubUnitName: $("#ClientSubUnitName").val(),
        SourceSystemCode: $("#SourceSystemCode").val(),
        VersionNumber: $("#VersionNumber").val()
    };
    $("#Team").val(JSON.stringify(team));

    //AJAX (JSON) POST of Team Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(team),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            var $tabs = $('#tabs').tabs();

            if (!result.success) {

                /*
                Validation Failed - show TeamDetails Screen Again with Errors
                */
                $("#tabs-2Content").html(result.html);
                $("#teamEditTable").tablesorter({
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
                                            label: item.Name + " (" + item.CityCode + ")",
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

                //setting a value for this so we can change it to 1 if select is from match
                var locationAutoCompleteSelect = 0;

                $("#HierarchyItem").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "/Team.mvc/AutoCompleteHierarchies", type: "POST", dataType: "json",
                            data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val() },
                            success: function (data) {
                                //alert(response);
                                response($.map(data, function (item) {
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
                    select: function (event, ui) {
                        $("#lblHierarchyItemMsg").text(ui.item.text);
                        $("#HierarchyItem").val(ui.item.value);
                        $("#HierarchyCode").val(ui.item.id);
                        locationAutoCompleteSelect = 1;

                    },
                    mustMatch: true,
                    autoFill: true
                    //});
                });

              
                //Next Button
                $("#teamDetailsNextButton").button();
                $("#teamDetailsNextButton").click(function () {
                    ValidateTeamDetails();
                });

                //Back Button
                $("#teamDetailsBackButton").button();
                $("#teamDetailsBackButton").click(function () {
                    ShowTeamSelectionScreen("");
                });

                if ($('#TeamId').val() == "0") {
                    $("#deleteTeam").hide();
                }
                else {
                    $("#deleteTeam").button();
                    $("#deleteTeam").click(function (e) {

                        //different buttons depending on SystemUserCount
                        var arrButtons = {};
                        if ($('#SystemUserCount').val() == "0" || $('#ClientSubUnitCount').val() == "0") {

                            arrButtons["Continue"] = function () {
                                $(this).dialog("close");
                                ConfirmDelete2();
                            };
                            arrButtons["Cancel"] = function () {
                                $(this).dialog("close");
                            };
                        } else {
                            arrButtons["Cancel"] = function () {
                                $(this).dialog("close");
                            };
                        }

                        var teamId = escapeInput($('#TeamId').val());

                        $("#dialog-confirm").load('../TeamWizard.mvc/ShowConfirmDelete?teamId=' + teamId).dialog({
                            resizable: true,
                            modal: true,
                            height: 500,
                            width: 600,
                            buttons: arrButtons
                        }, 'open');
                    });
                }
            } else {
                /*
                Validation Passed - show TeamUsers Screen
                */

                $tabs.tabs("enable", 2);
                $tabs.tabs('select', 2); // switch to 5th tab

                $("#tabs-3Content").html("please wait");
                //alert("test");
                $("#tabs-3Content").html(result.html);

                $("#teamCurrentUsers").tablesorter({
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



                $('#teamCurrentUsers img').click(function (e) {

                    var userStatus = $(this).parent().parent().attr("userStatus");
                    var userGUID = $(this).parent().parent().attr("userGUID");
                    var userLogin = $(this).parent().parent().attr("id");

                    visualRemoveUserFromTeam(userStatus, userLogin, userGUID);

                });

                //Next Button
                $("#teamUsersNextButton").button();
                $("#teamUsersNextButton").click(function (e) {
                    SaveTeamSystemUsers();
                });

                //Back Button
                $("#teamUsersBackButton").button();
                $("#teamUsersBackButton").click(function (e) {
                    $tabs.tabs('select', 1);
                    //$("#tabs").tabs({ disabled: [2, 3, 4] });
                });

                //Search Button
                $("#searchForSystemUserButton").button();
                $("#searchForSystemUserButton").click(function () {
                    DoSystemUserSearch();
                });

            }
        }
    });
}


/*
* Delete A Team
*/
function ConfirmDelete2() {

    //different buttons depending on SystemUserCount
    var arrButtons = {};

    arrButtons["Confirm Delete"] = function () {
        $(this).dialog("close");
        DeleteTeam();
    };
    arrButtons["Cancel"] = function () {
        $(this).dialog("close");
    };

    var teamId = escapeInput($('#TeamId').val());
    var showConfirmDeleteUrl = '../TeamWizard.mvc/ShowConfirmDelete2?teamId=' + teamId;

    $("#dialog-confirm").load(showConfirmDeleteUrl).dialog({
        resizable: true,
        modal: true,
        height: 500,
        width: 600,
        buttons: arrButtons

    }, 'open');
}
function SaveTeamSystemUsers() {
   
    $('#tabs').tabs("enable", 3);
	
	//demo function to show how to get data from removedTeamMembers and addedTeamMembers
		
	//Added Users
	var AddedUsers = [];
	$('#addedTeamMembers tr').each(function(){		
		var userGUID = $(this).attr("userguid");
		AddedUsers.push({ SystemUserGuid: userGUID });
	});
    $('#AddedUsers').val(JSON.stringify(AddedUsers))

    //Removed Users
    var RemovedUsers = [];
    $('#removedTeamMembers tr').each(function () {
        var userGUID = $(this).attr("userguid");
        RemovedUsers.push({ SystemUserGuid: userGUID });
    });
    $('#RemovedUsers').val(JSON.stringify(RemovedUsers))

    ShowTeamClientSubUnitsScreen();
}



//Show Team Clients
function ShowTeamClientSubUnitsScreen() {

    //ensure that all hidden tables are cleared

    $('#removedClientSubUnits tr').each(function(){
	    $(this).remove();
    });

    $('#addedClientSubUnits tr').each(function(){
	    $(this).remove();
    });

    //alert("fn ShowTeamClientSubUnitsScreen")
    var url = '/TeamWizard.mvc/TeamClientSubUnitsScreen';

    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";

    $("#tabs-1Content").html("<p>" + ajaxLoading + " Loading...</p>");

    $.ajax({
        type: "POST",
        data: { teamId: $("#TeamId").val() },
        url: url,
        success: function (result) {


            $("#tabs").tabs('select', 3); // switch to 1st tab
            $("#tabs").tabs({ disabled: [1, 2, 4] });

            $("#tabs-4Content").html(result);
            $("#teamTable").tablesorter({
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


            $("#clientsInTeamNextButton").button();

            $("#clientsInTeamNextButton").click(function (e) {
                SaveTeamClientSubUnits();
            });


            $("#clientsInTeamBackButton").button();

            $("#clientsInTeamBackButton").click(function (e) {
                var $tabs = $('#tabs').tabs();
                $tabs.tabs('enable', 2);
                $tabs.tabs('select', 2);
                $("#tabs").tabs({ disabled: [3, 4] });
            });

            $("#ClientSearchButton").button();

            $("#ClientSearchButton").click(function (e) {
                DoClientSubUnitSearch();
            });

            //$("#currentClientsTable").tablesorter();

            $("#currentClientsTable").tablesorter({
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



            $('#currentClientsTable img').click(function () {


                var addStatus = $(this).parent().parent().attr("clientSubUnitStatus");

                var clientSubUnitName = $(this).parent().parent().attr("clientSubUnitName");
                var clientCountryName = $(this).parent().attr("clientCountryName");
                var clientJSSubUnitGuid = $(this).parent().parent().attr("id");
                var clientSubUnitGuid = $(this).parent().parent().attr("ClientSubUnitGuid");

                var inClientDropListCB = $(this).parent().parent().children().find("#IncludeInClientDroplist");

                var clientSubUnitVersionNumber = $(this).parent().parent().attr("clientSubUnitVersion");

                if ($(inClientDropListCB).is(':checked')) {

                    var inClientDropList = true;
                }

                else {
                    var inClientDropList = false;
                }


                visualRemoveClientFromTeam(addStatus, clientJSSubUnitGuid, clientSubUnitGuid, inClientDropList, clientSubUnitVersionNumber);



            });


            $('#currentClientsTable tr input').change(function () {

                var addStatus = $(this).parent().parent().attr("clientSubUnitStatus");

                if (addStatus == 'Current') {


                    //the client sub unit is current, so we need to make note of a change in INCL DROP LIST
                    var inClientDropListCB = $(this).parent().parent().children().find("#IncludeInClientDroplist");

                    var clientSubUnitVersionNumber = $(this).parent().parent().attr("clientSubUnitVersion");

                    if ($(inClientDropListCB).is(':checked')) {

                        var inClientDropList = true;
                    }

                    else {
                        var inClientDropList = false;
                    }

                    var clientJSSubUnitGuid = $(this).parent().parent().attr("id");
                    var clientSubUnitGuid = $(this).parent().parent().attr("ClientSubUnitGuid");

                    ///check if it's in the change table already. if so, remove it.
                    $('#changedClientSubUnits tr').each(function () {

                        var trid = $(this).attr("id");
                        if (trid == clientJSSubUnitGuid) {

                            $(this).remove();
                        }
                    });

                    $('#changedClientSubUnits').append("<tr id='" + clientJSSubUnitGuid + "' clientSubUnitGuid='" + clientSubUnitGuid + "' inClientDropList='" + inClientDropList + "' clientSubUnitVersionNumber='" + clientSubUnitVersionNumber + "'></tr>");


                }
                if (addStatus == 'New') {


                    var inClientDropListCB = $(this).parent().parent().children().find("#IncludeInClientDroplist");

                    if ($(inClientDropListCB).is(':checked')) {

                        var inClientDropList = true;
                    }

                    else {
                        var inClientDropList = false;
                    }
                    var clientJSSubUnitGuid = $(this).parent().parent().attr("id");
                    var clientSubUnitGuid = $(this).parent().parent().attr("ClientSubUnitGuid");


                    $('#addedClientSubUnits tbody tr').each(function () {

                        var trid = $(this).attr("id");

                        if (trid == clientJSSubUnitGuid) {

                            $(this).attr("inClientDropList", inClientDropList);
                        }
                    });

                }


            });


        },
        error: function () {
            var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 3); // switch to 1st tab

            $("#tabs-4Content").html("An error occured");
        }
    });
}

function SaveTeamClientSubUnits() {
	
		$('#AddedClientSubUnits').val("");
	
	//Added ClientSubUnits
	var AddedClientSubUnits = [];

	var inClientDropList = '';

	$('#currentClientsTable tbody tr').each(function() {
	    
		var addStatus = $(this).attr("clientSubUnitStatus");
		
	    if (typeof addStatus != 'undefined'){

	        if ($(this).children().find("#IncludeInClientDroplist").is(':checked')) {
	            inClientDropList = 1;
	        } else {
	            inClientDropList = 0;
	        }
	    	
			
	        if (addStatus == 'New') {
				
					var clientSubUnitGuid = $(this).attr("clientSubUnitGuid");
					var clientSubUnitVersionNumber = "1";
					AddedClientSubUnits.push({ ClientSubUnitGuid: clientSubUnitGuid, VersionNumber: clientSubUnitVersionNumber, IncludeInClientDroplistFlag: inClientDropList });
	        }
	    }
	
	});
	
	$('#AddedClientSubUnits').val(JSON.stringify(AddedClientSubUnits))
	//alert(i);

	//Removed ClientSubUnits
	var RemovedClientSubUnits = [];
	$('#removedClientSubUnits tbody tr').each(function () {

	    var inClientDropList = $(this).attr("inClientDropList");
	    var clientSubUnitGuid = $(this).attr("clientSubUnitGUID");
	    var clientSubUnitVersionNumber = $(this).attr("clientSubUnitVersionNumber");
	    RemovedClientSubUnits.push({ ClientSubUnitGuid: clientSubUnitGuid, VersionNumber: 1, IncludeInClientDroplistFlag: inClientDropList });
	});
	$('#RemovedClientSubUnits').val(JSON.stringify(RemovedClientSubUnits))

	//Altered ClientSubUnits
	var AlteredClientSubUnits = [];
	$('#changedClientSubUnits tbody tr').each(function () {

	    var inClientDropList = $(this).attr("inClientDropList");
	    var clientSubUnitGuid = $(this).attr("clientSubUnitGUID");
	    var clientSubUnitVersionNumber = $(this).attr("clientSubUnitVersionNumber");
	    AlteredClientSubUnits.push({ ClientSubUnitGuid: clientSubUnitGuid, VersionNumber: 1, IncludeInClientDroplistFlag: inClientDropList });
	});
	$('#AlteredClientSubUnits').val(JSON.stringify(AlteredClientSubUnits))
			
    $('#tabs').tabs("enable", 4);
    ShowConfirmChangesScreen();
}

function ShowFinishScreen() {
	
    //alert("fn ShowFinishScreen")
    var url = '/TeamWizard.mvc/FinishScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-5Content").html("<p>" + ajaxLoading + " Loading...</p>");

    var teamChanges = {
        Team: $.parseJSON($("#Team").val()),
        SystemUsersRemoved: $("#RemovedUsers").val(),
        SystemUsersAdded: $("#AddedUsers").val(),
        ClientSubUnitsRemoved: $("#RemovedClientSubUnits").val(),
        ClientSubUnitsAdded: $("#AddedClientSubUnits").val()
    };
	
	

    $.ajax({
        type: "POST",
        data: JSON.stringify(teamChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
//            var $tabs = $('#tabs').tabs();
	       $("#tabs").tabs({ disabled: [1, 2, 3] });

            $tabs.tabs('select', 4); // switch to 1st tab
            $("#tabs-5Content").html(result.html);
        }
    });
}

//User Search
function DoSystemUserSearch() {
	$('#Filter').removeClass("ui-state-active");
	var filterValue = $('#Filter').val();
	if(!filterValue) {
		$('#Filter').focus();
		$('#Filter').addClass("ui-state-active");
		alert("Enter something for the system user search value. At least 1 character required.");
		return;
	}
	

    $("#lastSearchTR").html("<th colspan='5'><img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...</th>");
    var url = '/TeamWizard.mvc/SystemUserSearch';
    $.ajax({
        type: "POST",
        data: { filterField: $('#FilterField').val(), filter: encodeURI($('#Filter').val()), teamId: $("#TeamId").val() },
        url: url,
        success: function (result) {
            $("#SystemUserSearchResults").html(result);
            $("#lastSearchTR").html("");

            $("#teamUsersSearchResult").tablesorter({
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


            $("#teamUsersSearchResult tr").mouseover(function () { $(this).addClass("selectedRow"); });

            // Removes "over" class from rows on mouseout

            $("#teamUsersSearchResult tr").mouseout(function () { $(this).removeClass("selectedRow"); });

            $('#teamUsersSearchResult tr img').click(function (e) {

                var firstName = $(this).parent().parent().attr("userFirstName");
                var lastName = $(this).parent().parent().attr("userLastName");
                var userGUID = $(this).parent().parent().attr("userGUID");
				var networkLogin = $(this).parent().parent().attr("networkLogin");

				//first check if this user is not already in the current users table
				
				var currentUser = 0;
				
				$('#teamCurrentUsers tr').each(function(){
					
					if($(this).attr("userGUID")==userGUID){
					
						currentUser = 1;	
					}
				});
				
				if(currentUser==0){
					
				
				$(this).parent().parent().remove();


			//check to see if not in removed users table. to fix bug as reported by Luke where user is removed, then re-added, in stame step.
			
			$('#removedTeamMembers tr').each(function(){
				
				var tempID=$(this).attr("id");
				
				if(tempID==networkLogin) {
					
					$(this).remove();
					
				}
				
				
			});

			//add to hidden table
			$('#addedTeamMembers').append("<tr id='"+networkLogin+"' userGUID='"+userGUID+"'><td>"+userGUID+"</td></tr>");

                $('#teamCurrentUsers').append("<tr userStatus='NotCurrent' id='" + networkLogin + "' userGUID='"+escapeHTML(userGUID)+"'><td>" + escapeHTML(lastName) + ", " + escapeHTML(firstName) + "</td><td>" + networkLogin + "</td><td>N/A</td><td>N/A</td> <td><img src='../../images/remove.png' border='0' /></td></tr>");
				           
			
$("#teamCurrentUsers").tablesorter({
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


 $('#teamCurrentUsers img').click(function (e) {
					
					var userStatus = $(this).parent().parent().attr("userStatus");
					var userGUID = $(this).parent().parent().attr("userGUID");
					var userLogin = $(this).parent().parent().attr("id");
					
					visualRemoveUserFromTeam(userStatus, userLogin, userGUID);
					
				 });
				}
				
				else {
				alert("That user is already in the team!");	
				}
            });



        },
		error: function() {
			 var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 3); // switch to 1st tab

            $("#lastSearchTR").html("Sorry, but an error occured with the search.");
		}
		
		
    });
}

function visualRemoveUserFromTeam(ADDSTATUS, NEWUSERID, USERGUID) {
   	
	//first check to see if user was added in this process...cause then all we do is remove from addedusers table and do no have to add to removed users table
	
	
	if(ADDSTATUS=="Current") {
		//do it for the current users table
		$("#" + NEWUSERID).remove();
		$('#removedTeamMembers').append("<tr id='"+NEWUSERID+"' userGUID='"+USERGUID+"'><td>"+USERGUID+"</td></tr>");
	}
	else {
		
    //do it for the current users table
	$("#" + NEWUSERID).remove();
	
	//do it again for the hidden table
	$("#" + NEWUSERID).remove();
	}
	
	
	
}


//ClientSubUnit Search
function DoClientSubUnitSearch() {
	
	$('#ClientFilter').removeClass("ui-state-active");
	var clientSearchFilter = $('#ClientFilter').val();
	
	if(!clientSearchFilter) {
	$('#ClientFilter').addClass("ui-state-active");
	$('#ClientFilter').focus();
	alert("Enter at least something for the client search value.");
	
	
	
	return;
		
	}
	
 $("#lastSearchTRClient").html("<th colspan='5'><img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...</th>");

    var url = '/TeamWizard.mvc/ClientSubUnitSearch';
    $.ajax({
        type: "POST",
        data: { filterField: $('#ClientFilterField').val(), filter: encodeURI($('#ClientFilter').val()) },
        url: url,
        success: function (result) {
            $("#ClientSubUnitSearchResults").html(result);
            $("#lastSearchTRClient").html("");
            $("#clientSearchResultTable").tablesorter({
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




            $('#clientSearchResultTable img').click(function (e) {
                //adding client
                //alert($(this).parent().parent().attr("clientSubUnitName"))
                var clientSubUnitName = $(this).parent().parent().attr("clientSubUnitName").replace(/\//g, "%2F");
                //alert(clientSubUnitName)
                var clientCountryName = $(this).parent().parent().attr("clientCountryName");
                var clientSubUnitGuid = $(this).parent().parent().attr("id");
                var clientJSSubUnitGuid = $(this).parent().parent().attr("clientSubJSUnitGuid"); // js friendly guid without the :
                var inClientDropList = true;


                var currentClientSubUnit = 0;

                $('#currentClientsTable tr').each(function () {

                    if ($(this).attr("id") == clientJSSubUnitGuid) {

                        currentClientSubUnit = 1;
                    }
                });




                if (currentClientSubUnit == 0) {

                    $(this).parent().parent().remove();
                    $("#clientSearchResultTable").tablesorter({
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


                    //check if in removedClients (removed then added in same step (bug as reported by Luke)

                    $('#removedClientSubUnits tr').each(function () {

                        var tempID = $(this).attr("id");

                        if (tempID == clientJSSubUnitGuid) {

                            $(this).remove();

                        }

                    });

                    //add to hidden table
                    $('#addedClientSubUnits').append("<tr id='" + clientJSSubUnitGuid + "' SUGUID='" + clientSubUnitGuid + "' inClientDropList='" + inClientDropList + "'><td>" + clientSubUnitName + "</td></tr>");

                    $('#currentClientsTable').append("<tr clientSubUnitStatus='New' id='" + clientJSSubUnitGuid + "' clientSubUnitName='" + escape(clientSubUnitName) + "' clientSubUnitGuid='" + clientSubUnitGuid + "'><td>" + clientSubUnitName + "</td><td>" + clientCountryName + "</td><td>" + clientSubUnitGuid + "</td><td><input type='checkbox' id='IncludeInClientDroplist' checked='checked' /> </td><td><img src='../../images/remove.png' border='0' /></td></tr>");

                    //re-invoke the upon click

                    $('#currentClientsTable img').click(function () {

                        $(this).parent().parent().remove();


                    });



                    //end if currentClientSubUnit = 0 (e.g. client is not in current clients)	
                }
                else {

                    alert("Client already in existing list!");

                }



                //make sure that if a change is made to the include in client drop list check box on new, it is updated to the hidden changed client sub units table
                $('#currentClientsTable tr input').change(function () {

                    var addStatus = $(this).parent().parent().attr("clientSubUnitStatus");

                    if (addStatus == 'New') {


                        var inClientDropListCB = $(this).parent().parent().children().find("#IncludeInClientDroplist");

                        if ($(inClientDropListCB).is(':checked')) {

                            var inClientDropList = true;
                        }

                        else {
                            var inClientDropList = false;
                        }

                        var clientJSSubUnitGuid = $(this).parent().parent().attr("id");
                        var clientSubUnitGuid = $(this).parent().parent().attr("ClientSubUnitGuid");


                        $('#addedClientSubUnits tbody tr').each(function () {

                            var trid = $(this).attr("id");

                            if (trid == clientJSSubUnitGuid) {

                                $(this).attr("inClientDropList", inClientDropList);
                            }
                        });

                    }


                });


            });



        }


    });
	
	
	
}


function visualRemoveClientFromTeam(ADDSTATUS, SUJSGUID, SUGUID, DROPLIST, SUVERSION) {
   	
	//first check to see if user was added in this process...cause then all we do is remove from addedusers table and do no have to add to removed users table
	
	//alert(SUGUID);
	
	if(ADDSTATUS=="Current") {
		$("#" + SUJSGUID).remove();
		
		$("#" + SUJSGUID).html("");
		
		$('#removedClientSubUnits').append("<tr id='"+SUJSGUID+"' clientSubUnitGUID='"+SUGUID+"' inClientDropList='"+DROPLIST+"' clientSubUnitVersionNumber='"+SUVERSION+"'><td>"+SUGUID+"</td></tr>");
	}
	
	else {
		
    $("#" + SUJSGUID).remove();
	$("#" + SUJSGUID).remove();
	}
	
	
	
}



/*
*Delete A Team
*/
function DeleteTeam() {

    //Setup
    var url = '/TeamWizard.mvc/DeleteTeam';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-1Content").html("<p>" + ajaxLoading + " Loading...</p>");

    //Build Object to Store Team, we don't need all information
    var team = {
        TeamId: $("#TeamId").val(),
        VersionNumber: $("#VersionNumber").val()
    };
    $("#Team").val(JSON.stringify(team));

    //AJAX (JSON) POST of Team Changes Object
    $.ajax({
        type: "POST",
        data: $.parseJSON($("#Team").val()),
        url: url,
        dataType: "json",
        success: function (result) {

            //Disable UnUsed Tabs
            var $tabs = $('#tabs').tabs();
            $("#tabs").tabs({ disabled: [1, 2, 3] });

            //Display Result in 5th Tab
            $('#tabs').tabs("enable", 4);
            $tabs.tabs('select', 4);
            $("#tabs-5Content").html(result.html);           
			
			$("#BackToStart").button();
			$("#BackToStart").click(function () {
			 ShowTeamSelectionScreen("");
			});
			
        }

    });
}


/*
 *Show a list of Changes made by the user
 */
function ShowConfirmChangesScreen() {

    //Setup
    var url = '/TeamWizard.mvc/ConfirmChangesScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-5Content").html("<p>" + ajaxLoading + " Loading...</p>");

    //Build Object to Store Team Changes
    var teamChanges = {
    	Team: {},
    	SystemUsersRemoved: [],
    	SystemUsersAdded: [],
    	ClientSubUnitsRemoved: [],
    	ClientSubUnitsAdded: [],
    	ClientSubUnitsAltered: []
    };

	//Team
    var teamValue = escapeInput($("#Team").val());
    if (teamValue !== '') {
    	var teamJSON = $.parseJSON(teamValue);
    	teamChanges.Team = {
    		"CityCode": teamJSON.CityCode,
    		"HierarchyCode": teamJSON.HierarchyCode,
    		"HierarchyItem": teamJSON.HierarchyItem,
    		"HierarchyType": teamJSON.HierarchyType,
    		"SourceSystemCode": teamJSON.SourceSystemCode,
    		"TeamEmail": teamJSON.TeamEmail,
    		"TeamId": teamJSON.TeamId,
    		"TeamName": teamJSON.TeamName,
    		"TeamPhoneNumber": teamJSON.TeamPhoneNumber,
    		"TeamQueue": teamJSON.TeamQueue,
    		"TeamTypeCode": teamJSON.TeamTypeCode,
    		"VersionNumber": teamJSON.VersionNumber
    	}
    }

	//Removed Users
    var removedUsersValue = escapeInput($("#RemovedUsers").val());
    if (removedUsersValue !== '') {
    	var removedUsersJSON = $.parseJSON(removedUsersValue);
    	var length = removedUsersJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.SystemUsersRemoved.push({
    			"SystemUserGuid": removedUsersJSON[i].SystemUserGuid
    		});
    	}
    }

	//Added Users
    var addedUsersValue = escapeInput($("#AddedUsers").val());
    if (addedUsersValue !== '') {
    	var addedUsersJSON = $.parseJSON(addedUsersValue);
    	var length = addedUsersJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.SystemUsersAdded.push({
    			"SystemUserGuid": addedUsersJSON[i].SystemUserGuid
    		});
    	}
    }

	//Removed ClientSubUnits
    var removedClientSubUnitsValue = escapeInput($("#RemovedClientSubUnits").val());
    if (removedClientSubUnitsValue !== '') {
    	var removedClientSubUnitsJSON = $.parseJSON(removedClientSubUnitsValue);
    	var length = removedClientSubUnitsJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.ClientSubUnitsRemoved.push({
    			"ClientSubUnitGuid": removedClientSubUnitsJSON[i].ClientSubUnitGuid,
    			"VersionNumber": removedClientSubUnitsJSON[i].VersionNumber,
    			"IncludeInClientDroplistFlag": removedClientSubUnitsJSON[i].IncludeInClientDroplistFlag
    		});
    	}
    }

	//Added ClientSubUnits
    var addedClientSubUnitsValue = escapeInput($("#AddedClientSubUnits").val());
    if (addedClientSubUnitsValue !== '') {
    	var addedClientSubUnitsJSON = $.parseJSON(addedClientSubUnitsValue);
    	var length = addedClientSubUnitsJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.ClientSubUnitsAdded.push({
    			"ClientSubUnitGuid": addedClientSubUnitsJSON[i].ClientSubUnitGuid,
    			"VersionNumber": addedClientSubUnitsJSON[i].VersionNumber,
    			"IncludeInClientDroplistFlag": addedClientSubUnitsJSON[i].IncludeInClientDroplistFlag
    		});
    	}
    }

	//Altered ClientSubUnits
    var alteredClientSubUnitsValue = escapeInput($("#AlteredClientSubUnits").val());
    if (alteredClientSubUnitsValue !== '') {
    	var alteredClientSubUnitsJSON = $.parseJSON(alteredClientSubUnitsValue);
    	var length = alteredClientSubUnitsJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.ClientSubUnitsAltered.push({
    			"ClientSubUnitGuid": alteredClientSubUnitsJSON[i].ClientSubUnitGuid,
    			"VersionNumber": alteredClientSubUnitsJSON[i].VersionNumber,
    			"IncludeInClientDroplistFlag": alteredClientSubUnitsJSON[i].IncludeInClientDroplistFlag
    		});
    	}
    }

    //AJAX (JSON) POST of Team Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(teamChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            //Display Result in Current tab
            var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 4); // switch to 5th tab
            $("#tabs-5Content").html(result.html);
			 $("#tabs").tabs({ disabled: [1, 2, 3] });
			
			
			var changesMade = $('#changesMade').val();
			
			if(changesMade>0) {
			
            //Next Button
            $("#confirmChangesNextButton").button();
            $("#confirmChangesNextButton").click(function (e) {
				$('#waitingSpan').html("<img src='images/common/grid-loading.gif' align='left'>...please wait");
                CommitChanges();
            });

            //Confirm Button
            $("#confirmChangesBackButton").button();
            $("#confirmChangesBackButton").click(function (e) {
				 $tabs.tabs('enable', 3);
                $tabs.tabs('select', 3);
                $("#tabs").tabs({ disabled: [1, 2, 4] });
				$('#waitingSpan').html("");
            });
			
            //Cancel Button
            $("#cancelAllChanges").button();
			$("#cancelAllChanges").click(function (e) {
			 ShowTeamSelectionScreen("");
			});
			}
			else {
			$('#changesSummary').html("<h3>You made no changes to this team.</h3> <br /><br /><span id='BackToStart'><small>Team Wizard Home</small></span>");
			$("#confirmChangesNextButton").html("");
			$("#confirmChangesBackButton").button();
            $("#confirmChangesBackButton").click(function (e) {
                $tabs.tabs('enable', 3);
			    $tabs.tabs('select', 3);
                $("#tabs").tabs({ disabled: [1, 2, 4] });
            });
			
			$("#BackToStart").button();
			$("#BackToStart").click(function () {
			 ShowTeamSelectionScreen("");
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
    var url = '/TeamWizard.mvc/CommitChanges';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-5Content").html("<p>" + ajaxLoading + " Loading...</p>");

	//Build Object to Store Team Changes
    var teamChanges = {
    	Team: {},
    	SystemUsersRemoved: [],
    	SystemUsersAdded: [],
    	ClientSubUnitsRemoved: [],
    	ClientSubUnitsAdded: [],
    	ClientSubUnitsAltered: []
    };

	//Team
    var teamValue = escapeInput($("#Team").val());
    if (teamValue !== '') {
    	var teamJSON = $.parseJSON(teamValue);
    	teamChanges.Team = {
    		"CityCode": teamJSON.CityCode,
    		"HierarchyCode": teamJSON.HierarchyCode,
    		"HierarchyItem": teamJSON.HierarchyItem,
    		"HierarchyType": teamJSON.HierarchyType,
    		"SourceSystemCode": teamJSON.SourceSystemCode,
    		"TeamEmail": teamJSON.TeamEmail,
    		"TeamId": teamJSON.TeamId,
    		"TeamName": teamJSON.TeamName,
    		"TeamPhoneNumber": teamJSON.TeamPhoneNumber,
    		"TeamQueue": teamJSON.TeamQueue,
    		"TeamTypeCode": teamJSON.TeamTypeCode,
    		"VersionNumber": teamJSON.VersionNumber
    	}
    }

	//Removed Users
    var removedUsersValue = escapeInput($("#RemovedUsers").val());
    if (removedUsersValue !== '') {
    	var removedUsersJSON = $.parseJSON(removedUsersValue);
    	var length = removedUsersJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.SystemUsersRemoved.push({
    			"SystemUserGuid": removedUsersJSON[i].SystemUserGuid
    		});
    	}
    }

	//Added Users
    var addedUsersValue = escapeInput($("#AddedUsers").val());
    if (addedUsersValue !== '') {
    	var addedUsersJSON = $.parseJSON(addedUsersValue);
    	var length = addedUsersJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.SystemUsersAdded.push({
    			"SystemUserGuid": addedUsersJSON[i].SystemUserGuid
    		});
    	}
    }

	//Removed ClientSubUnits
    var removedClientSubUnitsValue = escapeInput($("#RemovedClientSubUnits").val());
    if (removedClientSubUnitsValue !== '') {
    	var removedClientSubUnitsJSON = $.parseJSON(removedClientSubUnitsValue);
    	var length = removedClientSubUnitsJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.ClientSubUnitsRemoved.push({
    			"ClientSubUnitGuid": removedClientSubUnitsJSON[i].ClientSubUnitGuid,
    			"VersionNumber": removedClientSubUnitsJSON[i].VersionNumber,
    			"IncludeInClientDroplistFlag": removedClientSubUnitsJSON[i].IncludeInClientDroplistFlag
    		});
    	}
    }

	//Added ClientSubUnits
    var addedClientSubUnitsValue = escapeInput($("#AddedClientSubUnits").val());
    if (addedClientSubUnitsValue !== '') {
    	var addedClientSubUnitsJSON = $.parseJSON(addedClientSubUnitsValue);
    	var length = addedClientSubUnitsJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.ClientSubUnitsAdded.push({
    			"ClientSubUnitGuid": addedClientSubUnitsJSON[i].ClientSubUnitGuid,
    			"VersionNumber": addedClientSubUnitsJSON[i].VersionNumber,
    			"IncludeInClientDroplistFlag": addedClientSubUnitsJSON[i].IncludeInClientDroplistFlag
    		});
    	}
    }

	//Altered ClientSubUnits
    var alteredClientSubUnitsValue = escapeInput($("#AlteredClientSubUnits").val());
    if (alteredClientSubUnitsValue !== '') {
    	var alteredClientSubUnitsJSON = $.parseJSON(alteredClientSubUnitsValue);
    	var length = alteredClientSubUnitsJSON.length;
    	for (var i = 0; i < length; i++) {
    		teamChanges.ClientSubUnitsAltered.push({
    			"ClientSubUnitGuid": alteredClientSubUnitsJSON[i].ClientSubUnitGuid,
    			"VersionNumber": alteredClientSubUnitsJSON[i].VersionNumber,
    			"IncludeInClientDroplistFlag": alteredClientSubUnitsJSON[i].IncludeInClientDroplistFlag
    		});
    	}
    }

    //AJAX (JSON) POST of Team Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(teamChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
	
	            if(result.success){
                clearHiddenFormVariables();
		
            } 
            //Disable UnUsed Tabs
            var $tabs = $('#tabs').tabs();
            $("#tabs").tabs({ disabled: [1, 2, 3, 4] });

            //Display Result in Current tab
            $("#tabs-5Content").html(result.html); 
			  $("#BackToStart").button();
				$("#BackToStart").click(function () {
			 ShowTeamSelectionScreen("");
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