/// <reference path="../../Views/OptionalFieldGroup/HierarchySearch.aspx" />
/*
OnReady
*/
$(document).ready(function () {

	//Navigation
	$('#menu_passivesegmentbuilder').click();
	$('#menu_passivesegmentbuilder_optionalfields').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	//Show DatePickers
	$('#OptionalFieldGroup_ExpiryDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});
	$('#OptionalFieldGroup_EnabledDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});
	if ($('#OptionalFieldGroup_EnabledDate').val() == "") {
		$('#OptionalFieldGroup_EnabledDate').val("No Enabled Date")
	}
	if ($('#OptionalFieldGroup_ExpiryDate').val() == "") {
		$('#OptionalFieldGroup_ExpiryDate').val("No Expiry Date")
	}
	//Hierarchy Disable/Enable OnLoad
	if ($("#OptionalFieldGroup_HierarchyType").val() == "") {
		$("#OptionalFieldGroup_HierarchyItem").val("");
		$("#OptionalFieldGroup_HierarchyItem").attr("disabled", true);
	} else {
		$("#OptionalFieldGroup_HierarchyItem").removeAttr("disabled");
		if ($("#OptionalFieldGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
			$("#lblHierarchyItem").text("ClientSubUnit");
			$('#TravelerType').css('display', '');
		}
	}

	//Hierarchy Disable/Enable OnChange
	$("#OptionalFieldGroup_HierarchyType").change(function () {
		$("#lblHierarchyItemMsg").text("");
		$("#OptionalFieldGroup_HierarchyItem").val("");
		if ($("#OptionalFieldGroup_QueueMinderGroupId").val() == "0") {
			$("#lblAuto").text("");
			$("#OptionalFieldGroup_QueueMinderGroupName").val("");
		}
		if ($("#OptionalFieldGroup_HierarchyType").val() == "") {
			$("#OptionalFieldGroup_HierarchyItem").attr("disabled", true);
			$('#TravelerType').css('display', 'none');
		} else {
			$("#OptionalFieldGroup_HierarchyItem").removeAttr("disabled");
			$("#lblHierarchyItem").text($("#OptionalFieldGroup_HierarchyType").val());
			$("#OptionalFieldGroup_HierarchyCode").val("");
			$('#TravelerType').css('display', 'none');
			if ($("#OptionalFieldGroup_HierarchyType").val() == "ClientSubUnitTravelerType") {
				$("#lblHierarchyItem").text("ClientSubUnit");
				$("#OptionalFieldGroup_TravelerTypeName").val("");
				$("#OptionalFieldGroup_TravelerTypeGuid").val("");
				$('#TravelerType').css('display', '');
			}
		}
	});

	/*
    Submit Form Validation
    */
	$('#form0').submit(function () {

		var validItem = false;
		var validTravelerType = true;

		if ($("#OptionalFieldGroup_HierarchyItem").val() == "Multiple") {
			validItem = true;
		} else {
			if ($("#OptionalFieldGroup_HierarchyType").val() != "Multiple") {
				jQuery.ajax({

					type: "POST",
					url: "/Hierarchy.mvc/IsValid" + $("#OptionalFieldGroup_HierarchyType").val(),
					data: { searchText: encodeURIComponent($("#OptionalFieldGroup_HierarchyItem").val()) },
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
		}

		if ($("#TravelerType").is(":visible")) {

			if ($("#OptionalFieldGroup_HierarchyType").val() != "") {
				jQuery.ajax({
					type: "POST",
					url: "/Hierarchy.mvc/IsValidTravelerType",
					data: { searchText: $("#OptionalFieldGroup_TravelerTypeName").val() },
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

		//wait for this name to be populated, dont show message
		if ($("#OptionalFieldGroup_OptionalFieldGroupId").val() == "0") {
			if ($("#lblAuto").text() == "") {
				return false;
			}
		} else {
			if (jQuery.trim($("#OptionalFieldGroup_OptionalFieldGroupName").val()) == "") {
				$("#OptionalFieldGroup_OptionalFieldGroupName_validationMessage").removeClass('field-validation-valid');
				$("#OptionalFieldGroup_OptionalFieldGroupName_validationMessage").addClass('field-validation-error');
				$("#OptionalFieldGroup_OptionalFieldGroupName_validationMessage").text("Optional Field Group Name Required.");
				return false;
			} else {
				$("#OptionalFieldGroupName_validationMessage").text("");
			}
		}

		//GroupName Begin
		var validGroupName = false;

		jQuery.ajax({
			type: "POST",
			url: "/GroupNameBuilder.mvc/IsAvailableOptionalFieldGroupName",
			data: { groupName: $("#OptionalFieldGroup_OptionalFieldGroupName").val(), id: $("#OptionalFieldGroup_OptionalFieldGroupId").val() },
			success: function (data) {

				validGroupName = data;
			},
			dataType: "json",
			async: false
		});

		if (!validGroupName) {

			$("#lblOptionalFieldGroupNameMsg").removeClass('field-validation-valid');
			$("#lblOptionalFieldGroupNameMsg").addClass('field-validation-error');
			if ($("#OptionalFieldGroup_OptionalFieldGroupId").val() == "0") {//Create
				$("#lblOptionalFieldGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
			} else {
				if ($("#OptionalFieldGroup_OptionalFieldGroupName").val() != "") {
					$("#lblOptionalFieldGroupNameMsg").text("This name has already been used, please choose a different name.");
				}
			} return false;
		} else {
			$("#lblOptionalFieldGroupNameMsg").text("");
		}
		//GroupName End
		if (!$(this).valid()) {
			return false;
		}

		if (validItem && validTravelerType) {
			if ($('#OptionalFieldGroup_ExpiryDate').val() == "No Expiry Date") {
				$('#OptionalFieldGroup_ExpiryDate').val("");
			}
			if ($('#OptionalFieldGroup_EnabledDate').val() == "No Enabled Date") {
				$('#OptionalFieldGroup_EnabledDate').val("");
			}
			return true;
		} else {
			return false
		};
	});



});

$(function () {


	$("#OptionalFieldGroup_HierarchyItem").autocomplete({
		source: function (request, response) {

			//if ($("#OptionalFieldGroup_HierarchyType").val() != "ClientSubUnitTravelerType") {
			$.ajax({
				url: "/AutoComplete.mvc/Hierarchies",
				type: "POST",
				dataType: "json",
				data: { searchText: request.term, hierarchyItem: $("#OptionalFieldGroup_HierarchyType").val(), domainName: 'Passive Segment Builder' },
				success: function(data) {
					response($.map(data, function(item) {
						console.log(item);
						if (
							$("#OptionalFieldGroup_HierarchyType").val() == "GlobalRegion" ||
								$("#OptionalFieldGroup_HierarchyType").val() == "GlobalSubRegion" ||
								$("#OptionalFieldGroup_HierarchyType").val() == "Country"
						) {
							return {
								label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + " (" + item.HierarchyCode + ")",
								value: item.HierarchyName,
								id: item.HierarchyCode,
								text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
							}
						} else if ($("#OptionalFieldGroup_HierarchyType").val() == "ClientAccount") {
							return {
								label: item.HierarchyName,
								value: item.HierarchyName,
								id: item.ClientAccountNumber,
								ssc: item.SourceSystemCode,
								text: ""
							}
						} else {
							return {
								label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
								value: item.HierarchyName,
								id: item.HierarchyCode,
								text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
							}
						}

					}));
				}
			});
			//} else {
			//	$.ajax({
			//		url: "/OptionalFieldGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
			//		data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#OptionalFieldGroup_TravelerTypeGuid").val() },
			//		success: function (data) {
			//			response($.map(data, function (item) {
			//				return {
			//					label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
			//					value: item.HierarchyName,
			//					id: item.HierarchyCode,
			//					text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
			//				}
			//			}))
			//		}
			//	})

			//}

		},
		select: function (event, ui) {
			$("#lblHierarchyItemMsg").text(ui.item.text);
			$("#OptionalFieldGroup_HierarchyItem").val(ui.item.value);
			$("#OptionalFieldGroup_HierarchyCode").val(ui.item.id);
			//$("#OptionalFieldGroup_SourceSystemCode").val(ui.item.ssc);

			if ($("#OptionalFieldGroup_OptionalFieldGroupId").val() == "0") {//Create

				var htft = ShortenHierarchyType($("#OptionalFieldGroup_HierarchyType").val());
				
				//to get number for GroupName
				//if ($("#OptionalFieldGroup_HierarchyType").val() == "ClientAccount") {
				//	$.ajax({
				//		url: "/GroupNameBuilder.mvc/BuildGroupNameClientAccount",
				//		type: "POST",
				//		dataType: "json",
				//		data: { clientAccountNumber: $("#OptionalFieldGroup_HierarchyCode").val(), sourceSystemCode: $("#OptionalFieldGroup_SourceSystemCode").val(), group: "Passive Builder" },
				//		success: function(data) {
				//			var maxNameSize = 100 - (htft.length + 5);
				//			var autoName = replaceSpecialChars(ui.item.value)
				//			autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft;
				//			$("#lblAuto").text(autoName);
				//			$("#OptionalFieldGroup_OptionalFieldGroupName").val(autoName);
				//			$("#lblOptionalFieldGroupNameMsg").text("");
				//		}
				//	})
				//} else {
				$.ajax({
					url: "/GroupNameBuilder.mvc/BuildGroupName",
					type: "POST",
					dataType: "json",
					data: { hierarchyType: $("#OptionalFieldGroup_HierarchyType").val(), hierarchyItem: $("#OptionalFieldGroup_HierarchyCode").val(), group: "Passive Segment Builder" },
					success: function(data) {
						var maxNameSize = 100 - (htft.length + 5);
						var autoName = replaceSpecialChars(ui.item.value);
						autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft + "_PSBOptionFld";
						$("#lblAuto").text(autoName);
						$("#OptionalFieldGroup_OptionalFieldGroupName").val(autoName);
						$("#lblOptionalFieldGroupNameMsg").text("");
					}
				});
				//}
			}
		}
	});


	$("#OptionalFieldGroup_TravelerTypeName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/OptionalFieldGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
				data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#OptionalFieldGroup_HierarchyCode").val() },
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
		},
		select: function (event, ui) {
			$("#lblTravelerTypeMsg").text(ui.item.text);
			$("#OptionalFieldGroup_TravelerTypeName").val(ui.item.value);
			$("#OptionalFieldGroup_TravelerTypeGuid").val(ui.item.id);
		}
	});
});


function ShortenHierarchyType(hierarchyType) {
	switch (hierarchyType) {
		case "ClientTopUnit":
			shortversion = "CTU";
			break;
		case "ClientSubUnit":
			shortversion = "CSU";
			break;
		case "ClientSubUnitTravelerType":
			shortversion = "CSUTT";
			break;
		case "GlobalSubRegion":
			shortversion = "GSR";
			break;
		case "GlobalRegion":
			shortversion = "GR";
			break;
		case "CountryRegion":
			shortversion = "CR";
			break;
		default:
			shortversion = hierarchyType;
	}

	//switch ($("#OptionalFieldGroup_FeeTypeId").val()) {
	//	case "1":
	//		ft = "_SupFee";
	//		break;
	//	case "2":
	//		ft = "_TransFee";
	//		break;
	//	case "3":
	//		ft = "_MidOffTransFee";
	//		break;
	//	case "4":
	//		ft = "_MerchantFee";
	//		break;
	//}


	return shortversion; // + ft;
}