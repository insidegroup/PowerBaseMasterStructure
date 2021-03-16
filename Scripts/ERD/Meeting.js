$(function() {
	$('#MeetingArriveByTime, #MeetingLeaveAfterTime').timepicker({
		interval: 15,
		dynamic: false,
		dropdown: true
	});
});

$(document).ready(function () {

	//Navigation
	$('#menu_meetings').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$('#search_wrapper').css('height', '24px');

	if ($('#MeetingStartDate').val() == "Jan 01 0001") {
		$('#MeetingStartDate').val("Select Date");
	}

	if ($('#MeetingEndDate').val() == "Jan 01 0001") {
		$('#MeetingEndDate').val("Select Date");
	}

	//Show DatePickers
	$('#MeetingStartDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	$('#MeetingEndDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	$('#MeetingArriveByDateTime').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	$('#MeetingLeaveAfterDateTime').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	$('#EnabledDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});
	
	$('#ExpiryDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});

	loadTimeBoxes();

	$('#MeetingArriveByDateTime').change(function () {
		loadTimeBoxes();
	});

	$('#MeetingLeaveAfterDateTime').change(function () {
		loadTimeBoxes();
	});
	
	function loadTimeBoxes() {

		if ($('#MeetingArriveByDateTime').val() == '') {
			$('#MeetingArriveByTime').val('').attr('disabled', true);
		} else {
			$('#MeetingArriveByTime').attr('disabled', false);
		}

		if ($('#MeetingLeaveAfterDateTime').val() == '') {
			$('#MeetingLeaveAfterTime').val('').attr('disabled', true);
		} else {
			$('#MeetingLeaveAfterTime').attr('disabled', false);
		}
	}
});

//City Lookup
$(function () {
	$("#CityCode_validationMessage").text("");
	$("#CityName").autocomplete({

		source: function (request, response) {
			$.ajax({
				url: "/AutoComplete.mvc/Cities", type: "POST", dataType: "json",
				data: { searchText: request.term, maxResults: 10 },
				success: function (data) {

					response($.map(data, function (item) {
						return {
							label: item.Name + " (" + item.CityCode + ")",
							value: item.Name,
							name: item.CityCode
						}
					}))
				}
			})
		},
		select: function (event, ui) {
			$("#CityCode").val(ui.item.name); //textbox
		}
	});

});

//Submit Form Validation
$('#form0').submit(function () {

	if ($('#MeetingStartDate').val() == "Select Date") {
		$('#MeetingStartDate').val("");
	}

	if ($('#MeetingEndDate').val() == "Select Date") {
		$('#MeetingEndDate').val("");
	}

	var validItem = false;
	var validCity = true;

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
		return false;
	} else {
		$("#CityCode").val($("#CityCode").val().toUpperCase())
		$("#lblCityCode").text("");
	}

	if ($("#HierarchyType").val() != "") {
		jQuery.ajax({
			type: "POST",
			url: "/Hierarchy.mvc/IsValid" + $("#HierarchyType").val(),
			data: { searchText: encodeURIComponent($("#HierarchyItem").val()) },
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
			if ($("#lblAuto").length) { $("#lblAuto").text("") };

		} else {
			$("#lblHierarchyItemMsg").text("");
		}
	}

	//wait for this name to be populated, dont show message
	if ($("#MeetingId").val() == "0") {

		if ($("#lblAuto").text() == "") {
			return false;
		}
	} else {
		if (jQuery.trim($("#MeetingName").val()) == "") {
		$("#MeetingName_validationMessage").removeClass('field-validation-valid');
		$("#MeetingName_validationMessage").addClass('field-validation-error');
		$("#MeetingName_validationMessage").text("Meeting Name Required.");
			return false;
		} else {
			$("#MeetingName_validationMessage").text("");
		}
	}

	//GroupName Begin
	var validGroupName = false;

	jQuery.ajax({
		type: "POST",
		url: "/GroupNameBuilder.mvc/IsAvailableMeetingName",
		data: { groupName: $("#MeetingName").val(), id: $("#MeetingID").val() },
		success: function (data) {

			validGroupName = data;
		},
		dataType: "json",
		async: false
	});

	if (!validGroupName) {

		$("#lblMeetingNameMsg").removeClass('field-validation-valid');
		$("#lblMeetingNameMsg").addClass('field-validation-error');
		if ($("#MeetingId").val() == "0") {//Create
			$("#lblMeetingNameMsg").text("This name has already been used, please choose a different name.");
		} else {
			if ($("#MeetingName").val() != "") {
				$("#lblMeetingNameMsg").text("This name has already been used, please choose a different name.");
			}
		}
		return false;
	} else {
		$("#lblMeetingNameMsg").text("");
	}

	//GroupName End
	if (!$(this).valid()) {
		return false;
	}

	if (validItem && validCity) {
		return true;
	}

});

$("#HierarchyItem").autocomplete({
	source: function (request, response) {
    	$.ajax({
    		url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
    		data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'Meeting Group Administrator', resultCount: 5000 },
    		success: function (data) {
    			response($.map(data, function (item) {
    				return {
    					label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
    					value: item.HierarchyName,
    					id: item.HierarchyCode,
    					text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
    				}
    			}));
    		}
    	});
	},
	select: function (event, ui) {
    	$("#lblHierarchyItemMsg").text(ui.item.text);
    	$("#HierarchyItem").val(ui.item.value);
    	$("#HierarchyCode").val(ui.item.id);
	}
});