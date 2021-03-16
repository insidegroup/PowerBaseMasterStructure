/*Validate the required information */

function traveller_identity_service() {

    var success = false;

    //Testing
    //$('#ThirdPartyUser_TISUserId').attr('disabled', false).val('123456');
    //return true;

	//Validate TokenAuthenticationUrl
	var tokenAuthenticationUrl = $('#TokenAuthenticationUrl').val();
	if (tokenAuthenticationUrl === undefined || tokenAuthenticationUrl == null || tokenAuthenticationUrl == '') {
		console.log("TokenAuthenticationUrl not defined");
		showTravelerIdentityStoreError();
		return false;
	}

	//Validate TokenAuthorizationKey
	var tokenAuthorizationKey = $('#TokenAuthorizationKey').val();
	if (tokenAuthorizationKey === undefined || tokenAuthorizationKey == null || tokenAuthorizationKey == '') {
		console.log("TokenAuthorizationKey not defined");
		showTravelerIdentityStoreError();
		return false;
	}

	//Validate BrokerUrl
	var brokerUrl = $('#BrokerUrl').val();
	if (brokerUrl === undefined || tokenAuthorizationKey == null || brokerUrl == '') {
		console.log("BrokerUrl not defined");
		showTravelerIdentityStoreError();
		return false;
	}

	//Update TIS
	var traveller_user_id = $('#ThirdPartyUser_TISUserId').val();
	if (traveller_user_id != '') {

		var update_success = updateTraveler(tokenAuthenticationUrl, tokenAuthorizationKey, brokerUrl);
		if (update_success) {
			success = true;
		} else {
			logThirdPartyUserUpdateError();
			showTravelerIdentityStoreError();
			success = false;
		}

	//Create Traveller
	} else {

		var create_success = createTraveler(tokenAuthenticationUrl, tokenAuthorizationKey, brokerUrl);
		if (create_success) {
			success = true;
		} else {
			logThirdPartyUserCreateError();
			showTravelerIdentityStoreError();
			success = false;
		}
	}

	return success;
}

/*Call the TIS Enpoints */

function createTraveler(tokenAuthenticationUrl, tokenAuthorizationKey, brokerUrl) {

	var success = false;

	$.ajax({
		async: false,
		crossDomain: true,
		type: "POST",
		url: tokenAuthenticationUrl,
		contentType: "application/x-www-form-urlencoded",
		cacheControl: "no-cache",
		beforeSend: function (xhr) {
			xhr.setRequestHeader("Authorization", "Basic " + tokenAuthorizationKey);
		},
		complete: function (data) {

			if (data.response != null) {

				var authentication_response = jQuery.parseJSON(data.response);

				access_token = authentication_response.access_token;

				if (access_token != '') {

					var traveler_data = getTravelerData();

					traveler_data.userId = GetNewUserId();

					$.ajax({
						async: false,
						crossDomain: true,
						type: "POST",
						url: brokerUrl,
						data: JSON.stringify(traveler_data),
						dataType: 'json',
						contentType: "application/scim+json",
						cacheControl: "no-cache",
						beforeSend: function (xhr) {
							xhr.setRequestHeader("authorization", "Bearer " + access_token);
						},
						success: function (data) {

							//Get the new GUID for the Traveler
							if (data != null && data.id != null) {
								traveller_user_id = data.id;
								if (traveller_user_id != '') {
									$('#ThirdPartyUser_TISUserId').attr('disabled', false).val(traveller_user_id);
									success = true;
								}
							}
						},
						error: function (data) {
							success = false;
							if (data.response != null) {
								var error = jQuery.parseJSON(data.response);
								parseErrorMessage(error)
							}
						}
					});
				}
			}
		},
		error: function (data) {
			success = false;
			if (data.response != null) {
				var error = jQuery.parseJSON(data.response);
				parseErrorMessage(error)
			}
		}
	});

	return success;
}

function updateTraveler(tokenAuthenticationUrl, tokenAuthorizationKey, brokerUrl) {

	var success = false;

	$.ajax({
		async: false,
		crossDomain: true,
		type: "POST",
		url: tokenAuthenticationUrl,
		contentType: "application/x-www-form-urlencoded",
		cacheControl: "no-cache",
		beforeSend: function (xhr) {
			xhr.setRequestHeader("Authorization", "Basic " + tokenAuthorizationKey);
		},
		complete: function (data) {

			if (data.response != null) {

				var authentication_response = jQuery.parseJSON(data.response);

				access_token = authentication_response.access_token;

				if (access_token != '') {

					var traveler_data = getTravelerData();

					traveler_data.userId = $('#ThirdPartyUser_TISUserId').val();

					$.ajax({
						async: false,
						crossDomain: true,
						type: "PUT",
						url: brokerUrl + traveler_data.userId,
						data: JSON.stringify(traveler_data),
						dataType: 'json',
						contentType: "application/scim+json",
						cacheControl: "no-cache",
						beforeSend: function (xhr) {
							xhr.setRequestHeader("authorization", "Bearer " + access_token);
						},
						success: function (data) {
							if (data != null && data.userId != null) {
								success = true;
							}
						},
						error: function (data) {
							success = false;
							if (data.response != null) {
								var error = jQuery.parseJSON(data.response);
								parseErrorMessage(error)
							}
						}
					});
				}
			}
		},
		error: function (data) {
			success = false;
			if (data.response != null) {
				var error = jQuery.parseJSON(data.response);
				parseErrorMessage(error)
			}
		}
	});

	return success;
}

/*Get Data from Page */

function getTravelerData() {

	var traveler_data =
	{
		"firstName": $('#ThirdPartyUser_FirstName').val(),
		"lastName": $('#ThirdPartyUser_LastName').val(),
		"email": $('#ThirdPartyUser_Email').val(),
		"manager": $('#ThirdPartyUser_CWTManager').val(),
		"address": $('#ThirdPartyUser_FirstAddressLine').val() + ' ' + $('#ThirdPartyUser_SecondAddressLine').val(),
		"city": $('#ThirdPartyUser_CityName').val(),
		"postalCode": $('#ThirdPartyUser_PostalCode').val(),		
		"state": $('#ThirdPartyUser_StateProvinceCode').val(),
		"thirdPartyName": $('#ThirdPartyUser_ThirdPartyName').val(),
		"userType": $('#ThirdPartyUser_ThirdPartyUserTypeId option:selected').text(),
		"schemas": [
			"urn:pingidentity:schemas:Traveler:1.0"
		]
	};

	//ThirdPartyUser_RoboticUser
	if ($('#ThirdPartyUser_RoboticUserFlagNonNullable').is(':checkbox')) {
		traveler_data.roboticUserFlg = $('#ThirdPartyUser_RoboticUserFlagNonNullable').is(":checked");
	} else {
		traveler_data.roboticUserFlg = $('#ThirdPartyUser_RoboticUserFlagNonNullable').val() == "True" ? true : false;
	}

	//MilitaryAndGovernmentUserFlag
	if ($('#ThirdPartyUser_MilitaryAndGovernmentUserFlagNonNullable').is(':checkbox')) {
		traveler_data.mgUserFlg = $('#ThirdPartyUser_MilitaryAndGovernmentUserFlagNonNullable').is(":checked");
	} else {
		traveler_data.mgUserFlg = $('#ThirdPartyUser_MilitaryAndGovernmentUserFlagNonNullable').val() == "True" ? true : false;
	}

	//ThirdPartyUser_IsActive
	if ($('#ThirdPartyUser_IsActiveFlagNonNullable').is(':checkbox')) {
		traveler_data.travelerStatus = $('#ThirdPartyUser_IsActiveFlagNonNullable').is(":checked") ? 'ACTIVE' : 'INACTIVE';
	} else {
		traveler_data.travelerStatus = $('#ThirdPartyUser_IsActiveFlagNonNullable').val() == "true" ? 'ACTIVE' : 'INACTIVE';
	}

	//Get a list of internal remarks
	var internal_remarks = getThirdPartyUserInternalRemarks();
	if (!jQuery.isEmptyObject(internal_remarks)) {
		traveler_data.comments = internal_remarks;
	} else {
		traveler_data.comments = [];
	}

	//Get a list of all entitlements from GDSAccessRights
	var thirdPartyUser_entitlements = getThirdPartyUserEntitlements();
	if (!jQuery.isEmptyObject(thirdPartyUser_entitlements)) {
		traveler_data.entitlements = thirdPartyUser_entitlements;
	} else {
		traveler_data.entitlements = [];
	}

	return traveler_data;
}

//Get a list of Entitlements
function getThirdPartyUserEntitlements() {

	var entitlements = [];

	var thirdPartyUser_entitlements = escapeInput($('#ThirdPartyUser_Entitlements').val());

	if (thirdPartyUser_entitlements != null && thirdPartyUser_entitlements != '') {

		var obj = jQuery.parseJSON(thirdPartyUser_entitlements);

		$.each(obj, function (key, value) {

			entitlements.push(
				{
					"tpServiceID": escapeInput(value.tpServiceID),
					"tpAgentID": escapeInput(value.tpAgentID),
					"tpPCC": escapeInput(value.tpPCC),
					"deletedFlag" : escapeInput(value.DeletedFlag),
					"deletedTimestamp" : escapeInput(value.DeletedTimestamp)
				}
			);
		});
	}

	//If on a ThirdPartyUserGDSAccessRight page, add in the current item
	
	var submit_text = $('input[type="submit"]').val();

	//Date as YYYY-MM-DD HH:MM:SS.SSS
	var current_date = new Date();
	var formatted_date = current_date.toISOString();

	if (submit_text == "Create GDS Access Rights" || submit_text == "Edit GDS Access Rights") {

		entitlements.push(
			{
				"tpServiceID": $('#ThirdPartyUserGDSAccessRight_GDSCode option:selected').text(),
				"tpAgentID": $('#ThirdPartyUserGDSAccessRight_GDSSignOnID').val(),
				"tpPCC": $('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').val(),
				"deletedFlag": false,
				"deletedTimestamp": ""
			}
		);

	} else if(submit_text == "Confirm Delete") {

		entitlements.push(
			{
				"tpServiceID": $('#ThirdPartyUserGDSAccessRight_GDS_GDSName').val(),
				"tpAgentID": $('#ThirdPartyUserGDSAccessRight_GDSSignOnID').val(),
				"tpPCC": $('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').val(),
				"deletedFlag" : true,
				"deletedTimestamp": formatted_date
			}
		);

	} else if (submit_text == "Confirm UnDelete") {

		entitlements.push(
			{
				"tpServiceID": $('#ThirdPartyUserGDSAccessRight_GDS_GDSName').val(),
				"tpAgentID": $('#ThirdPartyUserGDSAccessRight_GDSSignOnID').val(),
				"tpPCC": $('#ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId').val(),
				"deletedFlag": false,
				"deletedTimestamp": ""
			}
		);
	}

	//Check for duplicates as it will cause an error on the TIS
	var filtered_entitlements = [];

	if (entitlements != null) {

		$.each(entitlements, function (key, value) {

			var containsItem = false;

			//Check if this item is already in the filtered items
			$.each(filtered_entitlements, function (item_key, item_value) {
				if (item_value.tpServiceID == value.tpServiceID && item_value.tpAgentID == value.tpAgentID && item_value.tpPCC == value.tpPCC) 
				{
					containsItem = true;
				}
			});
			
			if(!containsItem) {
				filtered_entitlements.push(
					{
						"tpServiceID": value.tpServiceID,
						"tpAgentID": value.tpAgentID,
						"tpPCC": value.tpPCC,
						"deletedFlag": value.deletedFlag,
						"deletedTimestamp": value.deletedTimestamp
					}
				);
			}
		});
	}

	return filtered_entitlements;
}


function getThirdPartyUserInternalRemarks() {

	var internal_remarks = [];

	//Existing Internal Remarks
	var thirdPartyUser_internal_remarks = escapeInput($('#ThirdPartyUser_InternalRemarks').val());
	if (thirdPartyUser_internal_remarks != null && thirdPartyUser_internal_remarks != '') {

		var obj = jQuery.parseJSON(thirdPartyUser_internal_remarks);

		$.each(obj, function (key, value) {

			var InternalRemark = escapeInput(value.InternalRemark);
			var CreationTimestamp = escapeInput(value.CreationTimestamp);

			internal_remarks.push(InternalRemark + ' - ' + CreationTimestamp.replace('T', ' '));
		});
	}

	//New Internal Remark
	var thirdPartyUser_internal_remark = escapeInput($('#ThirdPartyUser_InternalRemark').val());
	if (thirdPartyUser_internal_remark != '') {

		//Date as YYYY-MM-DD HH:MM:SS.SSS
		var current_date = new Date();
		var formatted_date = current_date.toISOString();

		if (thirdPartyUser_internal_remark != '') {
			internal_remarks.push(thirdPartyUser_internal_remark + ' - ' + formatted_date.replace('T', ' '));
		}
	}

	return internal_remarks;
}

/* Utilities */

function GetNewUserId() {

	//The Date.now() method returns the number of milliseconds elapsed since 1 January 1970 00:00:00 UTC.
	return Date.now().toString();
}

/* Logging */

function logThirdPartyUserCreateError() {
	$.ajax({
		url: "/ThirdPartyUser.mvc/LogCreateError",
		type: "POST",
		dataType: "json",
		async: false,
		data: {
			thirdPartyName: $("#ThirdPartyUser_ThirdPartyName").val(),
			firstName: $("#ThirdPartyUser_FirstName").val(),
			lastName: $("#ThirdPartyUser_LastName").val(),
			email: $("#ThirdPartyUser_Email").val()
		}
	});
}

function logThirdPartyUserUpdateError() {
	$.ajax({
		url: "/ThirdPartyUser.mvc/LogUpdateError",
		type: "POST",
		dataType: "json",
		async: false,
		data: {
			tISUserId: $("#ThirdPartyUser_TISUserId").val(),
			thirdPartyName: $("#ThirdPartyUser_ThirdPartyName").val(),
			firstName: $("#ThirdPartyUser_FirstName").val(),
			lastName: $("#ThirdPartyUser_LastName").val(),
			email: $("#ThirdPartyUser_Email").val()
		}
	});
}

/* Error Handling */
function parseErrorMessage(error) {

	var error_message = 'The request could not be processed. Please try again later';

	if (error != null && error.status != null) {

		switch (error.status) {

			case 404:
				//Traveler not found - could have been deleted
				error_message = 'Traveler not found with that ID';
				break;

			case 400:

				//Validation error returned from the TIS
				if (error.detail.indexOf("A unique attribute conflict was detected for attribute mail") !== -1) {

					//Email address must be unique
					error_message = 'Email Address must be unique';

				} else {
					error_message = error.detail;
				}

				break;

			case 500:

				//Server error returned from the TIS
				if (error.detail.indexOf("The provided LDAP attribute tpEntitlements contains duplicate values") !== -1) {

					//GDS Access Rights must be unique
					error_message = 'GDS Access Rights must be unique';
				}
				break;

			default:
				console.log(error.detail);
				break;
		}
	}

	//Set error message text
	$('#lblTISUserIdMsg').text(error_message);
}

//In the event the Third Party User cannot be created in the Traveler Identity Store an error message will be displayed to the user
function showTravelerIdentityStoreError() {
	$('#lblTISUserIdMsg').show()
}
