

$(document).ready(function () {
    //prevent IE from cahcing
    $.ajaxSetup({ cache: false });

    $("#tabs").tabs({ disabled: [1, 2, 3, 4] });
    ShowLocationSelectionScreen("","");
   
    $("#dialog-confirm").hide();

    //set function calls for each tab			
    $('#selectALocationLink').click(function () {
        $("#tabs").tabs({ disabled: [1, 2, 3, 4] });
        clearHiddenFormVariables();
        $('#currentLocation').html("");
        ShowLocationSelectionScreen("","");
    });

    $('#locationDetailsLink').click(function () {
        $("#tabs").tabs({ disabled: [2, 3, 4] });

    });
    $('#usersInLocationLink').click(function () {
        $("#tabs").tabs({ disabled: [3, 4] });
    });
	
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


  

});



/*
*Shows a List Of locations - can be filtered on LocationName
*/
function ShowLocationSelectionScreen(filter, filterField) {
	
    clearHiddenFormVariables();
    $('#currentLocation').html("");
		
	$('#waitingSpan').html("");
    //Setup
    var url = '/LocationWizard.mvc/LocationSelectionScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-1Content").html("<p>" + ajaxLoading + " Loading...</p>");

    //AJAX POST of filter
    $.ajax({
        type: "POST",
        data: { filter: filter, filterField: filterField },
        url: url,
        success: function (result) {
            var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 0);

            $("#tabs-1Content").html(result);
            $("#locationTable").tablesorter({ widgets: ['zebra'] });
            $("#tabs").tabs({ disabled: [1, 2, 3, 4] });
            $('#lastSearchL').html("");

            $("#SearchButton").button();
            $("#SearchButton").click(function () {
                $('#lastSearchL').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");

                var filter = $('#LFilter').val()
                var filterField = $('#LFilterField').val()
                ShowLocationSelectionScreen(filter, filterField);

            });

            $("#LFilter").keyup(function (e) {
                if (e.keyCode == 13) {
                    $('#lastSearchL').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");
                    var filter = $('#LFilter').val()
                    var filterField = $('#LFilterField').val()
                    ShowLocationSelectionScreen(filter, filterField);
                }
            });

            $("#LFilterField").keyup(function (e) {
                if (e.keyCode == 13) {
                    $('#lastSearchL').html("<img src='images/common/grid-loading.gif' align='left'>&nbsp; please wait ...");
                    var filter = $('#LFilter').val()
                    var filterField = $('#LFilterField').val()
                    ShowLocationSelectionScreen(filter, filterField);
                }
            });

            $("#createLocation").button();
            $("#createLocation").click(function () {
                SetWizardLocation(0);
            });
        }
    });
}

/*
*Called when a User Selects a Location from the List, it clears the current Location
*/
function SetWizardLocation(locationId) {
    var $inputs = $('#frmLocationWizard :input');
    var locationAlreadySelected = false;
    $inputs.each(function () {
        if ($(this).val() != "") {
            locationAlreadySelected = true;
        }
    });
    if (locationAlreadySelected) {
        if (!confirm("You have already selected a Location, if you continue you will lose unsaved information. Click OK to continue or Cancel to return to existing Location")) {
            return;
        }
    }
    clearHiddenFormVariables();
    $('#LocationId').val(locationId);
					
    $('#tabs').tabs("enable", 1);
    ShowLocationDetailsScreen();
}


//Shows Location Details for Edit/Add/Delete
function ShowLocationDetailsScreen() {
    if ($('#LocationId').val() == "") {
        alert("Please select a location first");
        ShowLocationSelectionScreen("");
        return;
    }

    var $tabs = $('#tabs').tabs();
    $tabs.tabs('select', 1); // switch to 2nd tab

    var url = '/LocationWizard.mvc/LocationDetailsScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-2Content").html("<p>" + ajaxLoading + " Loading...</p>");


    $.ajax({
        type: "POST",
        data: { locationId: $('#LocationId').val() },
        url: url,
        success: function (result) {

            $("#tabs-2Content").html(result);

            /* State/Provinces */
            if ($("#Location_CountryCode").val() !== "") {
                $('#Address_StateProvinceCode').attr('disabled', false);
            } else {
                $('#Address_StateProvinceCode').attr('disabled', true);
            }

            if ($("#Address_StateProvinceCode option").length > 1) {
                $('.stateProvinceCodeError').show();
            } else {
                $('.stateProvinceCodeError').hide();
            }

            if ($('#LocationId').val() == 0) {
                $('#Location_CountryRegionId').attr('disabled', 'disabled');
            } else {

                //Build Objects to Store Location - Need VersionNumber for Delete
                var location = {
                    LocationId: $("#Location_LocationId").val(),
                    VersionNumber: $("#Location_VersionNumber").val()
                }
                $("#Location").val(JSON.stringify(location));

                $("#searchTextBox").keypress(function () {
                    var code = (e.keyCode ? e.keyCode : e.which);
                    if (code == 13) { //Enter keycode
                        alert("EE");
                    }
                });


                //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
                //Load CountryRegions based on Selected Country
                $("#Location_CountryRegionId").find('option').remove();
                $.ajax({
                    url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
                    data: { countryCode: $("#Location_CountryCode").val() },
                    success: function (data) {
                        $(data).each(function () {
                            if ($("#CountryRegionId").val() == this.CountryRegionId) {
                                $("<option value=" + this.CountryRegionId + " selected>" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                            } else {
                                $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                            }
                        });
                    }

                })

            }

            //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
            //AutoComplete of CountryName
            $("#Location_CountryName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/AutoComplete.mvc/LocationCountries", type: "POST", dataType: "json",
                        data: { searchText: request.term },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return {
                                    label: item.CountryName,
                                    id: item.CountryCode
                                }
                            }))
                        }
                    })
                },
                select: function (event, ui) {
                    $("#Location_CountryCode").val(ui.item.id);
                    if ($('#LocationId').val() == 0) {
                        $('#Location_CountryRegionId').attr('disabled', '');
                    }

                    //Load Regions
                    $("#Location_CountryRegionId").find('option').remove();
                    $.ajax({
                        url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
                        data: { countryCode: $("#Location_CountryCode").val() },
                        success: function (data) {
                            $(data).each(function () {
                                $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                            });
                        }
                    });

                    //Load State/Provinces
                    var selected = $("#Address_StateProvinceCode option:selected").val();

                    $.ajax({
                        url: "/AutoComplete.mvc/GetStateProvincesByCountryCode", type: "POST", dataType: "json",
                        data: { countryCode: $("#Location_CountryCode").val() },
                        success: function (data) {

                            // Clear the old options
                            $("#Address_StateProvinceCode").find('option').remove();

                            // Add a default
                            $("<option value=''>Please Select...</option>").appendTo($("#Address_StateProvinceCode"));

                            // Load the new options
                            $(data).each(function () {
                            	$("<option value=" + this.StateProvinceCode + ">" + this.Name + "</option>").appendTo($("#Address_StateProvinceCode"));
                            });

                            // Show dropdown
                            if ($("#Address_StateProvinceCode option").length > 1) {
                                $('#Address_StateProvinceCode').attr('disabled', false);
                                $('.stateProvinceCodeError').show();

                                //Reapply Edit
                                if (selected != null) {
                                    $("#Address_StateProvinceCode").val(selected)
                                }

                            } else {
                                $('#Address_StateProvinceCode').attr('disabled', true);
                                $('.stateProvinceCodeError').hide();
                            }

                        }
                    });
                }
            });


            //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
            //Clear CountryRegion list when Country changes
            $("#Location_CountryName").change(function () {

                $("#Location_CountryCode").val("");
                $("#Location_CountryRegionId").find('option').remove();

                if ($("#Location_CountryName").val() != "") {
                    jQuery.ajax({
                        type: "POST",
                        url: "/Validation.mvc/IsValidAdminUserCountry",
                        data: { countryName: $("#Location_CountryName").val() },
                        success: function (data) {

                            if (!jQuery.isEmptyObject(data)) {
                                validItem = true;
                                $("#Location_CountryCode").val(data.CountryCode);
                            }
                        },
                        dataType: "json",
                        async: false
                    });

                }
                $("#Location_CountryRegionId").find('option').remove();
                $.ajax({
                    url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
                    data: { countryCode: $("#Location_CountryCode").val() },
                    success: function (data) {
                        $(data).each(function () {
                            $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                        });
                    }
                });

            });

            var currentLocation = "New Location";
            if ($('#LocationId').val() > 0) {
            	currentLocation = escapeInput($('#Location_LocationName').val());
            }
            else {
                currentLocation = "New Location"
            }

            $('#currentLocation').html(": " + currentLocation);
            $("#locationEditTable").tablesorter({ widgets: ['zebra'] });
            $("#tabs").tabs({ disabled: [2, 3, 4] });
            $('#editLocationTR').addClass("trTD");

            //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
            //Next Button
            $("#locationDetailsNextButton").button();
            $("#locationDetailsNextButton").click(function () {
                ValidateLocationDetails();
            });

            //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
            //Back Button
            $("#locationDetailsBackButton").button();
            $("#locationDetailsBackButton").click(function () {
                ShowLocationSelectionScreen("");
            });

            //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
            if ($('#LocationId').val() == "0") {
                //Delete Button
                $("#deleteLocation").hide();
            } else {

                //Delete Button
                $("#deleteLocation").button();
                $("#deleteLocation").click(function (e) {

                    //different buttons depending on SystemUserCount
                    var arrButtons = {};
                    
                    if ($('#SystemUserCount').val() == "0") {

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

                    var locationId = escapeInput($('#LocationId').val());

                    $("#dialog-confirm").load('../LocationWizard.mvc/ShowConfirmDelete?locationId=' + locationId).dialog({
                        resizable: true,
                        modal: true,
                        height: 500,
                        width: 600,
                        buttons:  arrButtons
                        
                    }, 'open');

                });

            }
        }
    });






}
/*
 *Clear hidden data - so we can restart wizard
 */
function clearHiddenFormVariables(){

    $('#frmLocationWizard input').each(function() {
		$(this).val("");
	});	
	$('#removedLocationMembers tr').each(function () {
		$(this).remove();
	});
	$('#addedLocationMembers tr').each(function () {
		$(this).remove();
	});	
	
}

/*
*Validate Location Details 
*/
function ValidateLocationDetails() {
    
    var validItem = false;

    if ($("#Location_CountryName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidAdminUserCountry",
            data: { countryName: $("#Location_CountryName").val() },
            success: function(data) {

                if (!jQuery.isEmptyObject(data)) {
                    validItem = true;
                    $("#Location_CountryCode").val(data.CountryCode);
                }
            },
            dataType: "json",
            async: false
        });
        if (!validItem) {
            
            $("#lblLocation_CountryNameMsg").removeClass('field-validation-valid');
            $("#lblLocation_CountryNameMsg").addClass('field-validation-error');
            $("#Location_CountryName_validationMessage").text("");
            $("#lblLocation_CountryNameMsg").text("This is not a valid country.");
            $("#Location_CountryRegionId").find('option').remove();
            return false;
        } else {
            $("#lblLocation_CountryNameMsg").text("");
        }
    }   

    //The State/Province should be mandatory if of a country in state/province table
    $("#lblStateProvinceCodeMsg").hide();

    if ($("#Address_StateProvinceCode option").length > 1 && $("#Address_StateProvinceCode").val() == "") {
        $("#lblStateProvinceCodeMsg").removeClass('field-validation-valid');
        $("#lblStateProvinceCodeMsg").addClass('field-validation-error');
        $("#lblStateProvinceCodeMsg").text("State/Province is required.");
        $("#lblStateProvinceCodeMsg").show();
        return false;
    }

    if ($("#Address_StateProvinceCode option:selected").val() != "") {

        jQuery.ajax({
            type: "POST",
            url: "/StateProvince.mvc/IsValidStateProvince",
            data: {
                searchText: $("#Address_StateProvinceCode option:selected").text(),
                countryCode: $("#Location_CountryCode").val()
            },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validItem = true;
                }
            },
            dataType: "json",
            async: false
        });
        if (!validItem) {
            $("#lblStateProvinceCodeMsg").removeClass('field-validation-valid');
            $("#lblStateProvinceCodeMsg").addClass('field-validation-error');
            $("#lblStateProvinceCodeMsg").text("This is not a valid entry.");
            $("#lblStateProvinceCodeMsg").show();
            return false;
        } else {
            $("#lblStateProvinceCodeMsg").text("");
        }
    }

    var url = '/LocationWizard.mvc/ValidateLocation';

    var Address_ReplicatedFromClientMaintenanceFlag = false;
    if ($("#Address_ReplicatedFromClientMaintenanceFlag").is(':checked')) {
        Address_ReplicatedFromClientMaintenanceFlag = true;
    }

    //Build Objects to Store Location
    var location = {
		LocationId: escapeInput($("#Location_LocationId").val()),
		LocationName: escapeInput($("#Location_LocationName").val()),
		CountryRegionId: escapeInput($("#Location_CountryRegionId").val()),
		CountryName: escapeInput($("#Location_CountryName").val()),
		CountryCode: escapeInput($("#Location_CountryCode").val()),
		VersionNumber: escapeInput($("#Location_VersionNumber").val())
    }

    var address = {
    	AddressId: escapeInput($("#Address_AddressId").val()),
    	FirstAddressLine: escapeInput($("#Address_FirstAddressLine").val()),
    	SecondAddressLine: escapeInput($("#Address_SecondAddressLine").val()),
    	CityName: escapeInput($("#Address_CityName").val()),
    	CountyName: escapeInput($("#Address_CountyName").val()),
    	StateProvinceCode: escapeInput($("#Address_StateProvinceCode option:selected").val()),
        LatitudeDecimal: escapeInput($("#Address_LatitudeDecimal").val()),
        LongitudeDecimal: escapeInput($("#Address_LongitudeDecimal").val()),
        MappingQualityCode: escapeInput($("#Address_MappingQualityCode").val()),
        PostalCode: escapeInput($("#Address_PostalCode").val()),
        ReplicatedFromClientMaintenanceFlag: Address_ReplicatedFromClientMaintenanceFlag,
        CountryCode: escapeInput($("#Location_CountryCode").val()),
        VersionNumber: escapeInput($("#Address_VersionNumber").val())
    };

    $("#Location").val(JSON.stringify(location));
    $("#Address").val(JSON.stringify(address));

    var locationAddress = {
    	Location: location,
    	Address: address
    }

    //AJAX (JSON) POST of Location Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(locationAddress),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            var $tabs = $('#tabs').tabs();

            if (!result.success) {
                /*
                Validation Failed - show LocationDetails Screen Again with Errors
                */

                //Setup
                $("#tabs-2Content").html(result.html);
                $("#locationEditTable").tablesorter({ widgets: ['zebra'] });

                //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
                //Next Button
                $("#locationDetailsNextButton").button();
                $("#locationDetailsNextButton").click(function () {
                    ValidateLocationDetails();
                });

                //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
                //Back Button
                $("#locationDetailsBackButton").button();
                $("#locationDetailsBackButton").click(function () {
                    ShowLocationSelectionScreen("");
                });

                /* State/Provinces */
                if ($("#Location_CountryCode").val() !== "") {
                    $('#Address_StateProvinceCode').attr('disabled', false);
                } else {
                    $('#Address_StateProvinceCode').attr('disabled', true);
                }

                if ($("#Address_StateProvinceCode option").length > 1) {
                    $('.stateProvinceCodeError').show();
                } else {
                    $('.stateProvinceCodeError').hide();
                }

                //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
                //Load CountryRegions based on Selected Country
                $("#Location_CountryRegionId").find('option').remove();
                $.ajax({
                    url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
                    data: { countryCode: $("#Location_CountryCode").val() },
                    success: function (data) {
                        $(data).each(function () {
                            if ($("#CountryRegionId").val() == this.CountryRegionId) {
                                $("<option value=" + this.CountryRegionId + " selected>" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                            } else {
                                $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                            }
                        });
                    }

                })
                //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
                //AutoComplete of CountryName
                $("#Location_CountryName").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: "/AutoComplete.mvc/LocationCountries", type: "POST", dataType: "json",
                            data: { searchText: request.term },
                            success: function (data) {
                                response($.map(data, function (item) {
                                    return {
                                        label: item.CountryName,
                                        id: item.CountryCode
                                    }
                                }))
                            }
                        })
                    },
                    select: function (event, ui) {
                        $("#Location_CountryCode").val(ui.item.id);
                        if ($('#LocationId').val() == 0) {
                            $('#Location_CountryRegionId').attr('disabled', '');
                        }

                        //Load Regions
                        $("#Location_CountryRegionId").find('option').remove();
                        $.ajax({
                            url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
                            data: { countryCode: $("#Location_CountryCode").val() },
                            success: function (data) {
                                $(data).each(function () {
                                    $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                                });
                            }
                        });

                        //Load State/Provinces
                        var selected = $("#Address_StateProvinceCode option:selected").val();

                        $.ajax({
                            url: "/AutoComplete.mvc/GetStateProvincesByCountryCode", type: "POST", dataType: "json",
                            data: { countryCode: $("#Location_CountryCode").val() },
                            success: function (data) {

                                // Clear the old options
                                $("#Address_StateProvinceCode").find('option').remove();

                                // Add a default
                                $("<option value=''>Please Select...</option>").appendTo($("#Address_StateProvinceCode"));

                                // Load the new options
                                $(data).each(function () {
                                    $("<option value=" + this.Name + ">" + this.Name + "</option>").appendTo($("#Address_StateProvinceCode"));
                                });

                                // Show dropdown
                                if ($("#Address_StateProvinceCode option").length > 1) {
                                    $('#Address_StateProvinceCode').attr('disabled', false);
                                    $('.stateProvinceCodeError').show();

                                    //Reapply Edit
                                    if (selected != null) {
                                        $("#Address_StateProvinceCode").val(selected)
                                    }

                                } else {
                                    $('#Address_StateProvinceCode').attr('disabled', true);
                                    $('.stateProvinceCodeError').hide();
                                }

                            }
                        });
                    }
                });

                //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
                //Clear CountryRegion list when Country changes
                $("#Location_CountryName").change(function () {

                    $("#Location_CountryRegionId").find('option').remove();
                    $.ajax({
                        url: "/AutoComplete.mvc/LocationCountryRegions", type: "POST", dataType: "json",
                        data: { countryCode: $("#Location_CountryCode").val() },
                        success: function (data) {
                            $(data).each(function () {
                                $("<option value=" + this.CountryRegionId + ">" + this.CountryRegionName + "</option>").appendTo($("#Location_CountryRegionId"));
                            });
                        }
                    })

                });

                //INCLUDED IN BOTH ShowLocationDetailsScreen() and ValidateLocationDetails();
                if ($('#LocationId').val() == "0") {
                    //Delete Button
                    $("#deleteLocation").hide();
                } else {

                    //Delete Button
                    $("#deleteLocation").button();
                    $("#deleteLocation").click(function (e) {

                        //different buttons depending on SystemUserCount
                        var arrButtons = {};
                        
                        if ($('#SystemUserCount').val() == "0") {

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

                        var locationId = escapeInput($('#LocationId').val());

                        $("#dialog-confirm").load('../LocationWizard.mvc/ShowConfirmDelete?locationId=' + locationId).dialog({
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
                Validation Passed - show LocationUsers Screen
                */

                $('#tabs').tabs("enable", 2);
                $tabs.tabs('select', 2); // switch to 5th tab
                $("#tabs-3Content").html(result.html);
				var locationUsersCount = $('#locationUsersCount').val();
				if(locationUsersCount<1) {
					
					$('#noLocationUsers').html("<h4>Currently there are no users associated with this location.</h4>");
					
					
				}
				
                $("#locationCurrentUsers").tablesorter({ widthFixed: true, widgets: ['zebra'] });
			

                $('#locationCurrentUsers img').click(function (e) {

                    var userStatus = $(this).parent().parent().attr("userStatus");
                    var userGUID = $(this).parent().parent().attr("userGUID");
                    var userLogin = $(this).parent().parent().attr("id");

                    visualRemoveUserFromLocation(userStatus, userLogin, userGUID);

                });

                //Next Button
                $("#locationUsersNextButton").button();
                $("#locationUsersNextButton").click(function (e) {
                    SaveLocationSystemUsers();
                });

                //Back Button
                $("#locationUsersBackButton").button();
                $("#locationUsersBackButton").click(function (e) {
                    $tabs.tabs('select', 1);
                    $("#tabs").tabs({ disabled: [2, 3, 4] });
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
 * Search for Users
 */
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
    var url = '/LocationWizard.mvc/SystemUserSearch';
    $.ajax({
        type: "POST",
        data: { filterField: $('#FilterField').val(), filter: $('#Filter').val() },
        url: url,
        success: function (result) {
            $("#SystemUserSearchResults").html(result);
  		    $("#lastSearchTR").html("");
            

            $("#locationUsersSearchResult").tablesorter({ widgets: ['zebra'] });

 
            $('#locationUsersSearchResult tr img').click(function (e) {

                var firstName = $(this).parent().parent().attr("userFirstName");
                var lastName = $(this).parent().parent().attr("userLastName");
                var userGUID = $(this).parent().parent().attr("userGUID");
                var networkLogin = $(this).parent().parent().attr("networkLogin");
                var userProfileIdentifier = $(this).parent().parent().attr("userProfileIdentifier");
				
 	           //first check if this user is not already in the current users table

                var currentUser = 0;

                $('#locationCurrentUsers tr').each(function () {

                    if ($(this).attr("userGUID") == userGUID) {

                        currentUser = 1;
                    }
                });

                if (currentUser == 0) {


                    $(this).parent().parent().remove();
                    //alert(userGUID);
					
					$('#noLocationUsers').html("");

                    //add to hidden table
                    $('#addedLocationMembers').append("<tr id='" + networkLogin + "' userGUID='" + userGUID + "'><td>" + userGUID + "</td></tr>");

                    $('#locationCurrentUsers').append("<tr userStatus='NotCurrent' id='" + networkLogin + "' userGUID='" + userGUID + "'><td>" + escapeHTML(lastName) + ", " + escapeHTML(firstName) + "</td><td>" + networkLogin + "</td><td>" + userProfileIdentifier + "</td><td>N/A</td><td>N/A</td> <td><img src='../../images/remove.png' border='0' /></td></tr>");

                    $("#locationCurrentUsers").tablesorter({ widthFixed: false, widgets: ['zebra'] });

                    $('#locationCurrentUsers img').click(function (e) {

                        var userStatus = $(this).parent().parent().attr("userStatus");
                        var userGUID = $(this).parent().parent().attr("userGUID");
                        var userLogin = $(this).parent().parent().attr("id");

                        visualRemoveUserFromLocation(userStatus, userLogin, userGUID);

                    });
                }

                else {
                    alert("user already in location!");
                }
            });



        },
        error: function () {
            var $tabs = $('#tabs').tabs();
            $tabs.tabs('select', 3); // switch to 1st tab

            $("#lastSearchL").html("Sorry, but an error occured with the search for available Systemusers.");
        }


    });
}

function visualRemoveUserFromLocation(ADDSTATUS, ULOGIN, UGUID){

	// remove from main table
	$('#locationCurrentUsers tr').each(function () {
		if ($(this).attr('userGUID') == UGUID) {
			$(this).remove();
		}
	});

	if (ADDSTATUS == "Current") {

		// add into the remove table
		$('#removedLocationMembers').append("<tr id='" + ULOGIN + "' userGUID='" + UGUID + "'><td>" + UGUID + "</td></tr>");
	}
	else {

		//do it again for the hidden table
		$('#addedLocationMembers tr').each(function () {
			if ($(this).attr('userGUID') == UGUID) {
				$(this).remove();
			}
		});
	}	
}

/*
 * Save Users to Hidden Fields
 */
function SaveLocationSystemUsers(){

    //Setup
    $('#tabs').tabs("enable", 3);

    //Added Users
    var AddedUsers = [];
    $('#addedLocationMembers tr').each(function () {
        var userGUID = $(this).attr("userguid");
        AddedUsers.push({ SystemUserGuid: userGUID });
    });
    $('#AddedUsers').val(JSON.stringify(AddedUsers))

    //Removed Users
    var RemovedUsers = [];
    $('#removedLocationMembers tr').each(function () {
        var userGUID = $(this).attr("userguid");
        RemovedUsers.push({ SystemUserGuid: userGUID });
    });
    $('#RemovedUsers').val(JSON.stringify(RemovedUsers))

    //Next Step
    ShowConfirmChangesScreen();
}


/*
* Delete A Location
*/
function ConfirmDelete2() {
    
    //different buttons depending on HasAttachedItems
    var arrButtons = {};
    if ($("#LocationTeamCount").val() == "0") {
        arrButtons["Continue"] = function () {
            $(this).dialog("close");
            ConfirmDelete3();
        };
    }
    arrButtons["Cancel"] = function () {
        $(this).dialog("close");
    };
   

    var locationId = escapeInput($('#LocationId').val());
    var showConfirmDeleteUrl = '../LocationWizard.mvc/ShowConfirmDelete2?locationId=' + locationId;

    $("#dialog-confirm").load(showConfirmDeleteUrl).dialog({
        resizable: true,
        modal: true,
        height: 500,
        width: 600,
        buttons: arrButtons

    }, 'open');
}
/*
* Delete A Location
*/
function ConfirmDelete3() {

    //different buttons depending on HasAttachedItems
    var arrButtons = {};
    if ($("#LinkedItemsCount").val() == "0") {
        arrButtons["Confirm Delete"] = function () {
            $(this).dialog("close");
            DeleteLocation();
        };
    }
    arrButtons["Cancel"] = function () {
        $(this).dialog("close");
    };


    var locationId = escapeInput($('#LocationId').val());
    var showConfirmDeleteUrl = '../LocationWizard.mvc/ShowConfirmDelete3?locationId=' + locationId;

    $("#dialog-confirm").load(showConfirmDeleteUrl).dialog({
        resizable: true,
        modal: true,
        height: 500,
        width: 600,
        buttons: arrButtons

    }, 'open');
}
/*
* Delete A Location
*/
function DeleteLocation() {

    //Setup
    var url = '/LocationWizard.mvc/DeleteLocation';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-1Content").html("<p>" + ajaxLoading + " Loading...</p>");


	//Location
    var location = {};
    var locationValue = escapeInput($("#Location").val());
    if (locationValue !== '') {
    	var locationJSON = $.parseJSON(locationValue);
    	location.Location = {
    		"LocationId": locationJSON.LocationId,
    		"VersionNumber": locationJSON.VersionNumber
    	}
    }

    //AJAX (JSON) POST of Team Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(location.Location),
        url: url,
        dataType: "json",
        success: function (result) {

            //Disable UnUsed Tabs
            var $tabs = $('#tabs').tabs();
            $("#tabs").tabs({ disabled: [1, 2] });

            //Display Result in 4th Tab
            $('#tabs').tabs("enable", 3);
            $tabs.tabs('select', 3);
            $("#tabs-4Content").html(result.html);
            $("#BackToStart").button();
            $("#BackToStart").click(function () {
                ShowLocationSelectionScreen("");
            });
        }

    });
}
/*
 * Show a list of Changes made by the user
 */
function ShowConfirmChangesScreen() {

    //Setup
    var url = '/LocationWizard.mvc/ConfirmChangesScreen';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
	var $tabs = $('#tabs').tabs();
    $tabs.tabs('select', 3); // switch to 5th tab
    $("#tabs-4Content").html("<p>" + ajaxLoading + " Loading...</p>");

	//Build Object to Store Location Changes
    var locationChanges = {
    	Location: {},
    	Address: {},
    	SystemUsersRemoved: [],
    	SystemUsersAdded: []
    };

	//Location
    var locationValue = escapeInput($("#Location").val());
    if (locationValue !== '') {
    	var locationJSON = $.parseJSON(locationValue);
    	locationChanges.Location = {
    		"CountryCode": locationJSON.CountryCode,
    		"CountryName": locationJSON.CountryName,
    		"CountryRegionId": locationJSON.CountryRegionId,
    		"LocationId": locationJSON.LocationId,
    		"LocationName": locationJSON.LocationName,	
    		"VersionNumber": locationJSON.VersionNumber
    	}
    }

	//Address
    var addressValue = escapeInput($("#Address").val());
    if (addressValue !== '') {
    	var addressJSON = $.parseJSON(addressValue);
    	locationChanges.Address = {
    		"AddressId": addressJSON.AddressId,
    		"CityName": addressJSON.CityName,
    		"CountryCode": addressJSON.CountryCode,
    		"CountyName": addressJSON.CountyName,
    		"FirstAddressLine": addressJSON.FirstAddressLine,
    		"LatitudeDecimal": addressJSON.LatitudeDecimal,	
    		"LongitudeDecimal": addressJSON.LongitudeDecimal,	
    		"MappingQualityCode": addressJSON.MappingQualityCode,	
    		"PostalCode": addressJSON.PostalCode,
    		"ReplicatedFromClientMaintenanceFlag": addressJSON.ReplicatedFromClientMaintenanceFlag,
    		"SecondAddressLine": addressJSON.SecondAddressLine,
    		"StateProvinceCode": addressJSON.StateProvinceCode,	
    		"VersionNumber": addressJSON.VersionNumber
    	}
    }

	//Removed Users
    var removedUsersValue = escapeInput($("#RemovedUsers").val());
    if (removedUsersValue !== '') {
    	var removedUsersJSON = $.parseJSON(removedUsersValue);
    	var length = removedUsersJSON.length;
    	for (var i = 0; i < length; i++) {
    		locationChanges.SystemUsersRemoved.push({
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
    		locationChanges.SystemUsersAdded.push({
    			"SystemUserGuid": addedUsersJSON[i].SystemUserGuid
    		});
    	}
    }

    //AJAX (JSON) POST of Location Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(locationChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
		error: function(){
			
			
            $("#tabs-4Content").html("An error occured");
			$("#tabs").tabs({ disabled: [1, 2] });
			
		
		},	
        success: function (result) {

            //Display Result in Current tab
            
            $("#tabs-4Content").html(result.html);
			$("#tabs").tabs({ disabled: [1, 2] });
			
	
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
                    $('#tabs').tabs("enable", 2);
                    $tabs.tabs('select', 2);
                    $("#tabs").tabs({ disabled: [1, 3] });
					$('#waitingSpan').html("");
                });
			
                //Cancel Button
                $("#cancelAllChanges").button();
			    $("#cancelAllChanges").click(function (e) {
			        ShowLocationSelectionScreen("");
			    });
			}else {
			    $('#changesSummary').html("<h3>You made no changes to this location.</h3> <br /><br /><span id='BackToStart'><small>Location Wizard Home</small></span>");
			    $("#confirmChangesNextButton").html("");
			   $("#confirmChangesBackButton").button();
				$("#confirmChangesBackButton").click(function (e) {
					$tabs.tabs('enable', 2);
					$tabs.tabs('select', 2);
					$("#tabs").tabs({ disabled: [1, 3] });
				});
			
			    $("#BackToStart").button();
			    $("#BackToStart").click(function () {
			        ShowLocationSelectionScreen("");
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
    var url = '/LocationWizard.mvc/CommitChanges';
    var ajaxLoading = "<img src='images/common/grid-loading.gif' align='left'>";
    $("#tabs-5Content").html("<p>" + ajaxLoading + " Loading...</p>");

	//Build Object to Store Location Changes
     var locationChanges = {
    	Location: {},
    	Address: {},
    	SystemUsersRemoved: [],
    	SystemUsersAdded: []
    };

	//Location
    var locationValue = escapeInput($("#Location").val());
    if (locationValue !== '') {
    	var locationJSON = $.parseJSON(locationValue);
    	locationChanges.Location = {
    		"CountryCode": locationJSON.CountryCode,
    		"CountryName": locationJSON.CountryName,
    		"CountryRegionId": locationJSON.CountryRegionId,
    		"LocationId": locationJSON.LocationId,
    		"LocationName": locationJSON.LocationName,
    		"VersionNumber": locationJSON.VersionNumber
    	}
    }

	//Address
    var addressValue = escapeInput($("#Address").val());
    if (addressValue !== '') {
    	var addressJSON = $.parseJSON(addressValue);
    	locationChanges.Address = {
    		"AddressId": addressJSON.AddressId,
    		"CityName": addressJSON.CityName,
    		"CountryCode": addressJSON.CountryCode,
    		"CountyName": addressJSON.CountyName,
    		"FirstAddressLine": addressJSON.FirstAddressLine,
    		"LatitudeDecimal": addressJSON.LatitudeDecimal,
    		"LongitudeDecimal": addressJSON.LongitudeDecimal,
    		"MappingQualityCode": addressJSON.MappingQualityCode,
    		"PostalCode": addressJSON.PostalCode,
    		"ReplicatedFromClientMaintenanceFlag": addressJSON.ReplicatedFromClientMaintenanceFlag,
    		"SecondAddressLine": addressJSON.SecondAddressLine,
    		"StateProvinceCode": addressJSON.StateProvinceCode,
    		"VersionNumber": addressJSON.VersionNumber
    	}
    }

	//Removed Users
    var removedUsersValue = escapeInput($("#RemovedUsers").val());
    if (removedUsersValue !== '') {
    	var removedUsersJSON = $.parseJSON(removedUsersValue);
    	var length = removedUsersJSON.length;
    	for (var i = 0; i < length; i++) {
    		locationChanges.SystemUsersRemoved.push({
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
    		locationChanges.SystemUsersAdded.push({
    			"SystemUserGuid": addedUsersJSON[i].SystemUserGuid
    		});
    	}
    }

    //AJAX (JSON) POST of Location Changes Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(locationChanges),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            if (result.success) {
                clearHiddenFormVariables();

            }
            //Disable UnUsed Tabs
            var $tabs = $('#tabs').tabs();
            $("#tabs").tabs({ disabled: [1, 2, 3] });

            //Display Result in Current tab
            $("#tabs-4Content").html(result.html);
            $("#BackToStart").button();
            $("#BackToStart").click(function () {
                 ShowLocationSelectionScreen("");
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