

$(document).ready(function () {

	var allowedChars = /^([\w\s-()\.\,@+!#*\¤\ǂ\‡\☨\¥\:\/\$\£\"]+)$/;

	//$('#menu_clientprofiles').click();

    function setTableRows() {
		$("tr:visible:odd").addClass("row_odd");
		$("tr:visible:even").addClass("row_even");
	}

	setTableRows();

	//if a checkbox is clicked - flag form as changed
	$("input:checkbox").change(function () {
		$("#dataChanged").val("1");
	});
	//if a text input is clicked - flag form as changed
	$("input:text").change(function () {
		$("#dataChanged").val("1");
	});
	//if a select input is clicked - flag form as changed
	$("select").change(function () {
		$("#dataChanged").val("1");
	});

	//set text to uppercase
	$('.uppercase input[type=text]').blur(function () {
		$(this).val($(this).val().toUpperCase());
		$("#dataChanged").val("1");
	});

	//custom remarks
	for (var i = 1; i < 6; i++) {
		$(".clientProfileDataElementName").each(function () {

			if ($(this).text() == "Remark " + i) {

				var has_values = false;
				var tr = $(this).closest('tr');

				$('select, input[type="text"]', tr).each(function (i) {
					if ($(this).val() != '') {
						has_values = true;
					} else {
						$(this).attr('disabled', 'true');
					}
				});

				tr.attr('id', 'remark-' + i)
					.addClass('remark-line')
					.find('td:last-child').append('<a href="#" class="remove-remark"><img src="/images/remove.png" alt=""></a>');

				if (!has_values) {
					tr.hide();
				}
			}

		});
	}

	//if the add remark button button is clicked
	$('.add-remark').click(function (e) {

		e.preventDefault();

		var form = $(this).closest('form');

		var visibleRemarksIncomplete = false;

		//check to see if all visible remarks are complete
		$('.remark-line:visible select, .remark-line:visible input', form).not(':button, :hidden').each(function (i) {
			if ($(this).val() == '') {
				visibleRemarksIncomplete = true;
			}
		});

		if (visibleRemarksIncomplete) {
			alert('Please complete the existing remarks before adding a new one');
			return false;
		}

		var total_visible = $('.remark-line:visible', form).length;

		if (total_visible >= 5) {
			alert('Only 5 customizable Remark fields are allowed');
		} else {
			for (var i = 1; i < (total_visible + 2) ; i++) {
				var item = $("#remark-" + i, form);
				if (!item.is(':visible')) {
					item.show().find('select, input').not(':button,:hidden').each(function (i) {
						$(this).val('');
						$(this).removeAttr('disabled');
					})
				}
			}
		}

		setTableRows();

	});

	//if the remove remark button button is clicked
	$('.remove-remark').click(function (e) {

		e.preventDefault();

		var row = $(this).parent().parent();

		row.find('select, input').not(':button,:hidden').each(function (i) {
			$(this).val('');
			$(this).attr('disabled', 'true');
		});

		row.hide();

		setTableRows();

	});

    //if the home button is clicked
    $('.home_button a').click(function () {
        return confirmChanges(-1);
    });

    //if the home button is clicked
    $('.home_button a').click(function () {
        return confirmChanges(-1);
    });

    var clientProfilePanelId = $("#ClientProfilePanelId").val();

    //if moving tabs
    $("#tabs").tabs({
        selected: clientProfilePanelId,
        select: function (event, ui) {
            return confirmChanges(ui.index);
        }
    });

    function confirmChanges(nextTab = 0) {
        if ($("#dataChanged").val() == "1") {
            if (confirm("There are unsaved changes. Please select OK to Save the changes or Cancel to ignore any changes")) {

                //Get current form
                var curTabEx01 = $('#tabs .ui-tabs-panel:not(.ui-tabs-hide)');
                var index = curTabEx01.index() - 1;

                //Save and move tab
                if (nextTab >= 0) {
                    $("#form" + index).find("input[name=ClientProfilePanelId]").val(nextTab);
                    $("#form" + index).submit();
                    return false;

                    //Save and Exit
                } else {
                    $('#redirectToHome' + index).val('1');
                    $("#form" + index).submit();
                    return false;
                }

            } else {
                //Reset forms
                var allForms = document.forms;
                for (var i = 0; i < allForms.length; i++) {
                    allForms[i].reset();
                    $('#redirectToHome' + i).val('0');
                }
                //Clear error messages
                $(".field-validation-error").text("");
                $("#dataChanged").val("0");
                return true;
            }
        }
    }

	/*
	Check if profile has been saved while editing
	*/
	setInterval(function() {

		//Check ClientProfileGroupId
		var clientProfileGroupId = 0;
		if ($("#ClientProfileGroupId") != null) {
			var clientProfileGroupIdValue = $("#ClientProfileGroupId").val();
			if (clientProfileGroupIdValue != null) {
				clientProfileGroupId = parseInt(clientProfileGroupIdValue, 10);
			}
		}

		//Check LastUpdateTimestamp
		var lastUpdateTimestamp = "";
		if ($("#ClientProfileGroup_LastUpdateTimestamp") != null) {
			var lastUpdateTimestampValue = $("#ClientProfileGroup_LastUpdateTimestamp").val();
			if (lastUpdateTimestampValue != null && lastUpdateTimestampValue != "") {
				lastUpdateTimestamp = lastUpdateTimestampValue;
			}
		}

		//Ajax Call
		if (clientProfileGroupId != 0 && lastUpdateTimestamp != "") {
			$.ajax({
				url: "/ClientProfileGroup.mvc/HasClientProfileGroupBeenUpdatedSinceLoad", type: "POST", dataType: "json",
				data: { ClientProfileGroupId: clientProfileGroupId, LastUpdateTimestamp: lastUpdateTimestamp },
				success: function (data) {
					if (data == "true") {
						alert("Client Profile elements have changed. \n Any changes since your last Save will be discarded. \n Reopen Client Profile Items for current elements.");
					}
				}
			});
		}
	}, 1000 * 60 * 1); //every minute

	/*
	Validate DefaultGDSCommandFormat
	*/

	function IsCommandFormatValid(defaultGDSCommandFormat, clientProfileMoveStatusId) {

		//QC2832 - Don't validate if ClientProfileMoveStatusId = 2 (Never Move)
		if (clientProfileMoveStatusId == 2) {
			return true;
		}

		var validItem = false;
		var gdsCode = $('#ClientProfileGroup_GDSCode').val();

		jQuery.ajax({

			type: "POST",
			url: "/Validation.mvc/IsValidGDSCommandFormat",
			data: { GDSCommandFormat: defaultGDSCommandFormat, GDSCode: gdsCode },
			success: function (data) {
				if (data == "true") {
					validItem = true;
				}
			},
			dataType: "json",
			async: false
		});

		return validItem;

	}

	/*
    Submit Form Validation	
	*/

	function validateLineItems(sectionId, itemId) {

		//Set text to uppercase
		$('.uppercase input[type=text]').each(function () {
			$(this).val($(this).val().toUpperCase());
		});

		var isValid = true;

		//Don't check hidden rows (remarks)
		var row = $("#ClientProfileItems" + sectionId + "_" + itemId + "__ClientProfileItem_GDSCommandFormat").closest("tr");
		if (!row.is(":visible")) {
			return true
		}

		//Get values
		var Format = $("#ClientProfileItems" + sectionId + "_" + itemId + "__ClientProfileItem_GDSCommandFormat").val()
		var Remark = $("#ClientProfileItems" + sectionId + "_" + itemId + "__ClientProfileItem_Remark").val();
		var ClientProfileMoveStatusId = document.getElementById("ClientProfileItems" + sectionId + "_" + itemId + "__ClientProfileItem_ClientProfileMoveStatusId").value;

		var InheritedFormat = $("#ClientProfileItems" + sectionId + "_" + itemId + "__ClientProfileItem_GDSCommandFormat").is(':disabled');
		var InheritedRemark = $("#ClientProfileItems" + sectionId + "_" + itemId + "__ClientProfileItem_Remark").is(':disabled');
		var InheritedMoveStatus = $("#ClientProfileItems" + sectionId + "_" + itemId + "__ClientProfileItem_ClientProfileMoveStatusId").is(':disabled');
		var InheritedLine = (InheritedMoveStatus || InheritedFormat || InheritedRemark);

		var MandatoryFlagChecked = ($("#ClientProfileItems" + sectionId + "_" + itemId + "__ClientProfileItem_MandatoryFlag").val() == "True");

		//If a client profile creator has selected a Move Status then a Format value is required.
		if (!InheritedLine && ClientProfileMoveStatusId != "" && Format == "") {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).addClass('field-validation-error');
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).text("Field Required.");
			isValid = false;
		} else {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).removeClass('field-validation-error');
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).text("");
		}

		//If a client profile creator has selected a Move Status and entered a Format, then a Remark value is required.
		if (!InheritedLine && ClientProfileMoveStatusId != "" && Format != "" && Remark == "") {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).addClass('field-validation-error');
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).text("Field Required.");
			isValid = false;
		} else {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).removeClass('field-validation-error');
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).text("");
		}

		//If a Remark is present but not a Move Status or Format, then the line is required.
		if ((ClientProfileMoveStatusId == "" || Format == "") && (InheritedRemark || Remark != "")) {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).addClass('field-validation-error');
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).text("* Required");
			isValid = false;

			//If a Format but not entered a Move Status or Remark then the line is required.
		} else if ((ClientProfileMoveStatusId == "" || Remark == "") && (InheritedFormat || Format != "")) {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).addClass('field-validation-error');
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).text("* Required");
			isValid = false;

			//If a field is flagged as mandatory (*) the Move Status, the Format and the Remark are required. 
		} else if (MandatoryFlagChecked && (ClientProfileMoveStatusId == "" || Format == "" || Remark == "")) {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).addClass('field-validation-error');
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).text("* Required");
			isValid = false;

		} else {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).removeClass('field-validation-error');
			if (MandatoryFlagChecked) {
				$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).text("*");
			} else {
				$("#ClientProfileItems" + sectionId + "_ClientProfileItem_ClientProfileDataElementName" + itemId).text("");
			}
		}

		//Check for Special Characters in Format
		if (Format != "" && !allowedChars.test(Format)) {
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).addClass('field-validation-error');
			$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).text("Special Characters not allowed.");
			isValid = false;
		} else {

			//Check for Special Characters in Remark
			if (Remark != "" && !allowedChars.test(Remark)) {
				$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).addClass('field-validation-error');
				$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).text("Special Characters not allowed.");
				isValid = false;
			} else {

				//Validate GDSCommandFormat
				if (Format != '' && !IsCommandFormatValid(Format, ClientProfileMoveStatusId)) {
					$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).addClass('field-validation-error');
					$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).text("GDS Format not valid.");
					isValid = false;
				} else {
					$("#ClientProfileItems" + sectionId + "_ClientProfileItem_GDSCommandFormat" + itemId).text("");
				}
			}
		}

		if (isValid && !InheritedLine && Format != "" && Remark != "" && ClientProfileMoveStatusId != "") {
			//Need to validate GDS Specific information
			var clientProfileItemRow = {
				GDSCode: $("#ClientProfileGroup_GDSCode").val(),
				GDSCommandFormat: Format,
				Remark: Remark,
				ClientProfileMoveStatusId: ClientProfileMoveStatusId
			};

			//AJAX (JSON) POST of Team Changes Object
			$.ajax({
				type: "POST",
				data: JSON.stringify(clientProfileItemRow),
				url: '/Validation.mvc/IsValidClientProfileRowItem',
				async: false,
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {

					if (result != "OK") {
						alert(result)
						$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).addClass('field-validation-error');
						$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).text(result);
						isValid = false;
					} else {
						$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).removeClass('field-validation-error');
						$("#ClientProfileItems" + sectionId + "_ClientProfileItem_Remark" + itemId).text("");
					}

				}
			});
		}
		return isValid;
	}

	$('#form0').submit(function () {
		var isValid = true;
		var count = document.getElementById("count0").value;
		for (i = 0; i < count; i++) {
			if (validateLineItems("ClientDetails", i) == false) {
				isValid = false;
			}
		}
		if (!isValid) {
			return false;
		}
		//clearInterval(myTimer);
		//$("#form0 input[name=ClientProfilePanelId]").val("0");

	});

	$('#form1').submit(function () {
		var isValid = true;
		var count = document.getElementById("count1").value;
		for (i = 0; i < count; i++) {
			if (validateLineItems("MidOffice", i) == false) {
				isValid = false;
			}
		}
		if (!isValid) {
			return false;
		}
		//$("#form1 input[name=ClientProfilePanelId]").val("1");
	});

	$('#form2').submit(function () {
		var isValid = true;
		var count = document.getElementById("count2").value;
		for (i = 0; i < count; i++) {
			if (validateLineItems("BackOffice", i) == false) {
				isValid = false;
			}
		}
		if (!isValid) {
			return false;
		}
		//clearInterval(myTimer);
		//$("#form2 input[name=ClientProfilePanelId]").val("2");
	});

	$('#form3').submit(function () {
		var isValid = true;
		var count = document.getElementById("count3").value;
		for (i = 0; i < count; i++) {
			if (validateLineItems("AirRailPolicy", i) == false) {
				isValid = false;
			}
		}
		if (!isValid) {
			return false;
		}
		//$("#form3 input[name=ClientProfilePanelId]").val("3");
	});

	$('#form4').submit(function () {
		var isValid = true;
		var count = document.getElementById("count4").value;
		for (i = 0; i < count; i++) {
			if (validateLineItems("Itinerary", i) == false) {
				isValid = false;
			}
		}
		if (!isValid) {
			return false;
		}
		//clearInterval(myTimer);
		//$("#form4 input[name=ClientProfilePanelId]").val("4");
	});

	$('#form5').submit(function () {
		var isValid = true;
		var count = document.getElementById("count5").value;
		for (i = 0; i < count; i++) {
			if (validateLineItems("24Hours", i) == false) {
				isValid = false;
			}
		}
		if (!isValid) {
			return false;
		}
		//clearInterval(myTimer);
		//$("#form5 input[name=ClientProfilePanelId]").val("5");
	});

	$('#form6').submit(function () {
		var isValid = true;
		var count = document.getElementById("count6").value;
		for (i = 0; i < count; i++) {
			if (validateLineItems("AmadeusTPM", i) == false) {
				isValid = false;
			}
		}
		if (!isValid) {
			return false;
		}
		//clearInterval(myTimer);
		//$("#form6 input[name=ClientProfilePanelId]").val("6");
	});

});