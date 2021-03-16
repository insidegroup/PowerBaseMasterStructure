

$(document).ready(function () {

	var allowedChars = /^([\w\s-()\.\,@+!#*\¤\ǂ\‡\☨\¥\:\/\$\£\"]+)$/;

	$('#menu_clientprofiles').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

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

	//Hide custom remarks
    for (var i = 1; i < 6; i++) {
    	$(".clientProfileDataElementName").each(function () {
    		if ($(this).text() == "Remark " + i) {
    			$(this).closest('tr').hide();
    		}
    	});
    }

	//Validate DefaultGDSCommandFormat
    function IsCommandFormatValid(defaultGDSCommandFormat, clientProfileMoveStatusId) {

    	//QC2832 - Don't validate if ClientProfileMoveStatusId = 2 (Never Move)
    	if (clientProfileMoveStatusId == 2) {
    		return true;
    	}

    	var validItem = false;
    	var gdsCode = $('#ClientProfileAdminGroupGDSCode').val();

    	jQuery.ajax({

    		type: "POST",
    		url: "/Validation.mvc/IsValidGDSCommandFormat",
    		data: { GDSCommandFormat : defaultGDSCommandFormat, GDSCode : gdsCode },
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
    $('#form0').submit(function () {

    	//set text to uppercase
    	$('.uppercase input[type=text]').each(function () {
    		$(this).val($(this).val().toUpperCase());
    	});

    	var isValid = true;
        var count = document.getElementById("count0").value;
        for (i = 0; i < count; i++) {
            var DefaultGDSCommandFormat = document.getElementById("ClientProfileAdminItemsClientDetails_" + i + "__ClientProfileAdminItem_DefaultGDSCommandFormat").value;
            var DefaultRemark = document.getElementById("ClientProfileAdminItemsClientDetails_" + i + "__ClientProfileAdminItem_DefaultRemark").value;
            var ToolTip = document.getElementById("ClientProfileAdminItemsClientDetails_" + i + "__ClientProfileAdminItem_ToolTip").value;
            var ClientProfileMoveStatusId = document.getElementById("ClientProfileAdminItemsClientDetails_" + i + "__ClientProfileAdminItem_ClientProfileMoveStatusId").value;
            var MandatoryFlagChecked = document.getElementById("ClientProfileAdminItemsClientDetails_" + i + "__ClientProfileAdminItem_MandatoryFlag").checked;

            
            if (DefaultGDSCommandFormat != "" && !allowedChars.test(DefaultGDSCommandFormat)) {
                $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {

            	//Validate GDSCommandFormat
            	if (DefaultGDSCommandFormat != '' && !IsCommandFormatValid(DefaultGDSCommandFormat, ClientProfileMoveStatusId)) {
            		$("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
            		$("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("GDS Format not valid.");
            		isValid = false;
            	//} else if (DefaultGDSCommandFormat == "" && (DefaultRemark != "" || ToolTip != "" || ClientProfileMoveStatusId != "" || MandatoryFlagChecked)) {
                //    $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                //    $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Format Required.");
                //    isValid = false;
                } else {
                    $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("");
                }
            }
            if (DefaultRemark != "" && !allowedChars.test(DefaultRemark)) {
                $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultRemark" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultRemark" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultRemark" + i).text("");
            }
            if (ToolTip != "" && !allowedChars.test(ToolTip)) {
                $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_ToolTip" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_ToolTip" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_ToolTip" + i).text("");
            }
           
        }
        if (!isValid) {
            return false;
        }
        //$("#form0 input[name=ClientProfilePanelId]").val("0");
    });
    $('#form1').submit(function () {

    	//set text to uppercase
    	$('.uppercase input[type=text]').each(function () {
    		$(this).val($(this).val().toUpperCase());
    	});

    	var isValid = true;
        var count = document.getElementById("count1").value;
        for (i = 0; i < count; i++) {
            var DefaultGDSCommandFormat = document.getElementById("ClientProfileAdminItemsMidOffice_" + i + "__ClientProfileAdminItem_DefaultGDSCommandFormat").value;
            var DefaultRemark = document.getElementById("ClientProfileAdminItemsMidOffice_" + i + "__ClientProfileAdminItem_DefaultRemark").value;
            var ToolTip = document.getElementById("ClientProfileAdminItemsMidOffice_" + i + "__ClientProfileAdminItem_ToolTip").value;
            var ClientProfileMoveStatusId = document.getElementById("ClientProfileAdminItemsMidOffice_" + i + "__ClientProfileAdminItem_ClientProfileMoveStatusId").value;
            var MandatoryFlagChecked = document.getElementById("ClientProfileAdminItemsMidOffice_" + i + "__ClientProfileAdminItem_MandatoryFlag").checked;


            if (DefaultGDSCommandFormat != "" && !allowedChars.test(DefaultGDSCommandFormat)) {
                $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {

            	//Validate GDSCommandFormat
            	if (DefaultGDSCommandFormat != '' && !IsCommandFormatValid(DefaultGDSCommandFormat, ClientProfileMoveStatusId)) {
            		$("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
            		$("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("GDS Format not valid.");
            		isValid = false;
            	//} else if (DefaultGDSCommandFormat == "" && (DefaultRemark != "" || ToolTip != "" || ClientProfileMoveStatusId != "" || MandatoryFlagChecked)) {
                //    $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                //    $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Format Required.");
                //    isValid = false;
                } else {
                    $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("");
                }
            }
            if (DefaultRemark != "" && !allowedChars.test(DefaultRemark)) {
                $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultRemark" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultRemark" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultRemark" + i).text("");
            }
            if (ToolTip != "" && !allowedChars.test(ToolTip)) {
                $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_ToolTip" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_ToolTip" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_ToolTip" + i).text("");
            }

        }
        if (!isValid) {
            return false;
        }
        //$("#form1 input[name=ClientProfilePanelId]").val("1");
    });
    $('#form2').submit(function () {

    	//set text to uppercase
    	$('.uppercase input[type=text]').each(function () {
    		$(this).val($(this).val().toUpperCase());
    	});

    	var isValid = true;
        var count = document.getElementById("count2").value;
        for (i = 0; i < count; i++) {
            var DefaultGDSCommandFormat = document.getElementById("ClientProfileAdminItemsBackOffice_" + i + "__ClientProfileAdminItem_DefaultGDSCommandFormat").value;
            var DefaultRemark = document.getElementById("ClientProfileAdminItemsBackOffice_" + i + "__ClientProfileAdminItem_DefaultRemark").value;
            var ToolTip = document.getElementById("ClientProfileAdminItemsBackOffice_" + i + "__ClientProfileAdminItem_ToolTip").value;
            var ClientProfileMoveStatusId = document.getElementById("ClientProfileAdminItemsBackOffice_" + i + "__ClientProfileAdminItem_ClientProfileMoveStatusId").value;
            var MandatoryFlagChecked = document.getElementById("ClientProfileAdminItemsBackOffice_" + i + "__ClientProfileAdminItem_MandatoryFlag").checked;


            if (DefaultGDSCommandFormat != "" && !allowedChars.test(DefaultGDSCommandFormat)) {
                $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
            	//Validate GDSCommandFormat
            	if (DefaultGDSCommandFormat != '' && !IsCommandFormatValid(DefaultGDSCommandFormat, ClientProfileMoveStatusId)) {
            		$("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
            		$("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("GDS Format not valid.");
            		isValid = false;
            	//} else if (DefaultGDSCommandFormat == "" && (DefaultRemark != "" || ToolTip != "" || ClientProfileMoveStatusId != "" || MandatoryFlagChecked)) {
                //    $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                //    $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Format Required.");
                //    isValid = false;
                } else {
                    $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("");
                }
            }
            if (DefaultRemark != "" && !allowedChars.test(DefaultRemark)) {
                $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultRemark" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultRemark" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultRemark" + i).text("");
            }
            if (ToolTip != "" && !allowedChars.test(ToolTip)) {
                $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_ToolTip" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_ToolTip" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_ToolTip" + i).text("");
            }

        }
        if (!isValid) {
            return false;
        }
        //$("#form2 input[name=ClientProfilePanelId]").val("2");
    });
    $('#form3').submit(function () {

        //set text to uppercase
    	$('.uppercase input[type=text]').each(function () {
    		$(this).val($(this).val().toUpperCase());
    	});

    	var isValid = true;
        var count = document.getElementById("count3").value;
        for (i = 0; i < count; i++) {
            var DefaultGDSCommandFormat = document.getElementById("ClientProfileAdminItemsAirRailPolicy_" + i + "__ClientProfileAdminItem_DefaultGDSCommandFormat").value;
            var DefaultRemark = document.getElementById("ClientProfileAdminItemsAirRailPolicy_" + i + "__ClientProfileAdminItem_DefaultRemark").value;
            var ToolTip = document.getElementById("ClientProfileAdminItemsAirRailPolicy_" + i + "__ClientProfileAdminItem_ToolTip").value;
            var ClientProfileMoveStatusId = document.getElementById("ClientProfileAdminItemsAirRailPolicy_" + i + "__ClientProfileAdminItem_ClientProfileMoveStatusId").value;
            var MandatoryFlagChecked = document.getElementById("ClientProfileAdminItemsAirRailPolicy_" + i + "__ClientProfileAdminItem_MandatoryFlag").checked;


            if (DefaultGDSCommandFormat != "" && !allowedChars.test(DefaultGDSCommandFormat)) {
                $("#ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {

            	//Validate GDSCommandFormat
            	if (DefaultGDSCommandFormat != '' && !IsCommandFormatValid(DefaultGDSCommandFormat, ClientProfileMoveStatusId)) {
            		$("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
            		$("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("GDS Format not valid.");
            		isValid = false;
            	//} else if (DefaultGDSCommandFormat == "" && (DefaultRemark != "" || ToolTip != "" || ClientProfileMoveStatusId != "" || MandatoryFlagChecked)) {
                //	$("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                //    $("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Format Required.");
                //    isValid = false;
                } else {
                	$("#ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("");
                }
            }
            if (DefaultRemark != "" && !allowedChars.test(DefaultRemark)) {
                $("#ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_DefaultRemark" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_DefaultRemark" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_DefaultRemark" + i).text("");
            }
            if (ToolTip != "" && !allowedChars.test(ToolTip)) {
                $("#ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_ToolTip" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_ToolTip" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_ToolTip" + i).text("");
            }

        }
        if (!isValid) {
            return false;
        }
        //$("#form3 input[name=ClientProfilePanelId]").val("3");
    });
    $('#form4').submit(function () {

    	//set text to uppercase
    	$('.uppercase input[type=text]').each(function () {
    		$(this).val($(this).val().toUpperCase());
    	});

    	var isValid = true;
        var count = document.getElementById("count4").value;
        for (i = 0; i < count; i++) {
            var DefaultGDSCommandFormat = document.getElementById("ClientProfileAdminItemsItinerary_" + i + "__ClientProfileAdminItem_DefaultGDSCommandFormat").value;
            var DefaultRemark = document.getElementById("ClientProfileAdminItemsItinerary_" + i + "__ClientProfileAdminItem_DefaultRemark").value;
            var ToolTip = document.getElementById("ClientProfileAdminItemsItinerary_" + i + "__ClientProfileAdminItem_ToolTip").value;
            var ClientProfileMoveStatusId = document.getElementById("ClientProfileAdminItemsItinerary_" + i + "__ClientProfileAdminItem_ClientProfileMoveStatusId").value;
            var MandatoryFlagChecked = document.getElementById("ClientProfileAdminItemsItinerary_" + i + "__ClientProfileAdminItem_MandatoryFlag").checked;


            if (DefaultGDSCommandFormat != "" && !allowedChars.test(DefaultGDSCommandFormat)) {
                $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {

            	//Validate GDSCommandFormat
            	if (DefaultGDSCommandFormat != '' && !IsCommandFormatValid(DefaultGDSCommandFormat, ClientProfileMoveStatusId)) {
            		$("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
            		$("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("GDS Format not valid.");
            		isValid = false;
            	//} else if (DefaultGDSCommandFormat == "" && (DefaultRemark != "" || ToolTip != "" || ClientProfileMoveStatusId != "" || MandatoryFlagChecked)) {
                //    $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                //    $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Format Required.");
                //    isValid = false;
                } else {
                    $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("");
                }
            }
            if (DefaultRemark != "" && !allowedChars.test(DefaultRemark)) {
                $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultRemark" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultRemark" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultRemark" + i).text("");
            }
            if (ToolTip != "" && !allowedChars.test(ToolTip)) {
                $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_ToolTip" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_ToolTip" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItemsItinerary_ClientProfileAdminItem_ToolTip" + i).text("");
            }

        }
        if (!isValid) {
            return false;
        }
        //$("#form4 input[name=ClientProfilePanelId]").val("4");
    });
    $('#form5').submit(function () {

    	//set text to uppercase
    	$('.uppercase input[type=text]').each(function () {
    		$(this).val($(this).val().toUpperCase());
    	});

    	var isValid = true;
        var count = document.getElementById("count5").value;
        for (i = 0; i < count; i++) {
            var DefaultGDSCommandFormat = document.getElementById("ClientProfileAdminItems24Hours_" + i + "__ClientProfileAdminItem_DefaultGDSCommandFormat").value;
            var DefaultRemark = document.getElementById("ClientProfileAdminItems24Hours_" + i + "__ClientProfileAdminItem_DefaultRemark").value;
            var ToolTip = document.getElementById("ClientProfileAdminItems24Hours_" + i + "__ClientProfileAdminItem_ToolTip").value;
            var ClientProfileMoveStatusId = document.getElementById("ClientProfileAdminItems24Hours_" + i + "__ClientProfileAdminItem_ClientProfileMoveStatusId").value;
            var MandatoryFlagChecked = document.getElementById("ClientProfileAdminItems24Hours_" + i + "__ClientProfileAdminItem_MandatoryFlag").checked;


            if (DefaultGDSCommandFormat != "" && !allowedChars.test(DefaultGDSCommandFormat)) {
                $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {

            	//Validate GDSCommandFormat
            	if (DefaultGDSCommandFormat != '' && !IsCommandFormatValid(DefaultGDSCommandFormat, ClientProfileMoveStatusId)) {
            		$("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
            		$("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("GDS Format not valid.");
            		isValid = false;
            	//} else if (DefaultGDSCommandFormat == "" && (DefaultRemark != "" || ToolTip != "" || ClientProfileMoveStatusId != "" || MandatoryFlagChecked)) {
                //    $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
                //    $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Format Required.");
                //    isValid = false;
                } else {
                    $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("");
                }
            }
            if (DefaultRemark != "" && !allowedChars.test(DefaultRemark)) {
                $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultRemark" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultRemark" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultRemark" + i).text("");
            }
            if (ToolTip != "" && !allowedChars.test(ToolTip)) {
                $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_ToolTip" + i).addClass('field-validation-error');
                $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_ToolTip" + i).text("Special Characters not allowed.");
                isValid = false;
            } else {
                $("#ClientProfileAdminItems24Hours_ClientProfileAdminItem_ToolTip" + i).text("");
            }

        }
        if (!isValid) {
            return false;
        }
        //$("#form5 input[name=ClientProfilePanelId]").val("5");
    });
    $('#form6').submit(function () {

    	//set text to uppercase
    	$('.uppercase input[type=text]').each(function () {
    		$(this).val($(this).val().toUpperCase());
    	});

    	var isValid = true;
    	var count = document.getElementById("count6").value;
    	for (i = 0; i < count; i++) {
    		var DefaultGDSCommandFormat = document.getElementById("ClientProfileAdminItemsAmadeusTPM_" + i + "__ClientProfileAdminItem_DefaultGDSCommandFormat").value;
    		var DefaultRemark = document.getElementById("ClientProfileAdminItemsAmadeusTPM_" + i + "__ClientProfileAdminItem_DefaultRemark").value;
    		var ToolTip = document.getElementById("ClientProfileAdminItemsAmadeusTPM_" + i + "__ClientProfileAdminItem_ToolTip").value;
    		var ClientProfileMoveStatusId = document.getElementById("ClientProfileAdminItemsAmadeusTPM_" + i + "__ClientProfileAdminItem_ClientProfileMoveStatusId").value;
    		var MandatoryFlagChecked = document.getElementById("ClientProfileAdminItemsAmadeusTPM_" + i + "__ClientProfileAdminItem_MandatoryFlag").checked;


    		if (DefaultGDSCommandFormat != "" && !allowedChars.test(DefaultGDSCommandFormat)) {
    			$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
    			$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Special Characters not allowed.");
    			isValid = false;
    		} else {

    			//Validate GDSCommandFormat
    			if (DefaultGDSCommandFormat != '' && !IsCommandFormatValid(DefaultGDSCommandFormat, ClientProfileMoveStatusId)) {
    				$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
    				$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("GDS Format not valid.");
    				isValid = false;
    				//} else if (DefaultGDSCommandFormat == "" && (DefaultRemark != "" || ToolTip != "" || ClientProfileMoveStatusId != "" || MandatoryFlagChecked)) {
    				//    $("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).addClass('field-validation-error');
    				//    $("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("Format Required.");
    				//    isValid = false;
    			} else {
    				$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultGDSCommandFormat" + i).text("");
    			}
    		}
    		if (DefaultRemark != "" && !allowedChars.test(DefaultRemark)) {
    			$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultRemark" + i).addClass('field-validation-error');
    			$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultRemark" + i).text("Special Characters not allowed.");
    			isValid = false;
    		} else {
    			$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultRemark" + i).text("");
    		}
    		if (ToolTip != "" && !allowedChars.test(ToolTip)) {
    			$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_ToolTip" + i).addClass('field-validation-error');
    			$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_ToolTip" + i).text("Special Characters not allowed.");
    			isValid = false;
    		} else {
    			$("#ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_ToolTip" + i).text("");
    		}

    	}
    	if (!isValid) {
    		return false;
    	}
    	//$("#form6 input[name=ClientProfilePanelId]").val("6");
    });

});