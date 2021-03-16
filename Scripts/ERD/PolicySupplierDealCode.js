//////////////////////////////////
//Required If
//////////////////////////////////
$.validator.addMethod('requiredif',
function (value, element, parameters) {

	var id = '#' + parameters['dependentproperty'];

	// get the target value (as a string, 
	// as that's what actual value will be)
	var targetvalue = parameters['targetvalue'];
	targetvalue = (targetvalue == null ? '' : targetvalue).toString();

	// get the actual value of the target control
	// note - this probably needs to cater for more 
	// control types, e.g. radios
	var control = $(id);
	var controltype = control.attr('type');
	var actualvalue =
        controltype === 'checkbox' ?
        control.attr('checked').toString() :
        control.val();

	// if the condition is true, reuse the existing 
	// required field validator functionality
	if (targetvalue === actualvalue)
		return $.validator.methods.required.call(this, value, element, parameters);

	return true;
});

$.validator.unobtrusive.adapters.add(
	'requiredif',
	['dependentproperty', 'targetvalue'],
	function (options) {
		options.rules['requiredif'] = {
			dependentproperty: options.params['dependentproperty'],
			targetvalue: options.params['targetvalue']
		};
		options.messages['requiredif'] = options.message;
	}
);

$(document).ready(function () {
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

	//Hide TourCodeTypes until GDS Selected
    if ($('#TourCodeTypeId option:selected').val() == "") {
    	$('#TourCodeTypeId').attr('disabled', true);
    }
   
    showFields();

    $('#PolicySupplierDealCodeTypeId').change(function () {
    	showFields();
    });

    function showFields() {
    	if ($('#PolicySupplierDealCodeTypeId').val() == "9") {
    		$('.tourcode-row').show();
    		if ($('#OSIFlagNonNullable').is(':checked')) {
    			$('.PolicySupplierDealCodeOSILine').show();
    		}
    		LoadTourCodeTypes();
    	} else {
    		$('#TravelIndicator').val('');
    		$('#TourCodeTypeId').val('').attr('disabled', true);
    		$('#Endorsement').val('');
    		$('#EndorsementOverride').val('');
    		$('#OSIFlagNonNullable').attr('checked', false);
    		resetPolicySupplierDealCodeOSI();
    		$('.tourcode-row').hide();
    		$('.PolicySupplierDealCodeOSILine').hide();
    		$('#countEndorsement').text('120');
    		$('#countEndorsementOverride').text('120');
    	}
    }

    function LoadTourCodeTypes() {

    	var existingSelection = $('#TourCodeTypeId option:selected').val();

    	var gdsCode = $('#GDSCode').val();

    	if (gdsCode != "") {

    		jQuery.ajax({
    			type: "POST",
    			url: "/PolicySupplierDealCode.mvc/GetTourCodeTypes",
    			data: { gdsCode: gdsCode },
    			success: function (data) {
    				$("#TourCodeTypeId").get(0).options.length = 0;
    				$("#TourCodeTypeId").get(0).options[0] = new Option("Please Select...", "");

    				$.each(data, function (index, item) {
    					$("#TourCodeTypeId").get(0).options[$("#TourCodeTypeId").get(0).options.length] = new Option(item.TourCodeTypeDescription, item.TourCodeTypeId);
    				});

    				if (data.length != 0) {
    					$('#TourCodeTypeId').attr('disabled', false);
    				} else {
    					$('#TourCodeTypeId').val("").attr('disabled', true);
    				}

					//Restore selection on edit
    				if (existingSelection != null) {
    					$('#TourCodeTypeId').val(existingSelection);
    				}
    			},
    			dataType: "json",
    			async: false
    		});
    	}
    }
	//AJAX TourCodeTypes when GDS is chosen
    $('#GDSCode').change(function () {
    	LoadTourCodeTypes();
    });

	//Character Counts
    $('#Endorsement').NobleCount('#countEndorsement', {
    	on_negative: function () {
    		$('#countEndorsement').css('color', 'red')
    	}, on_positive: function () {
    		$('#countEndorsement').css('color', 'black')
    	},
    	max_chars: 120,
    	block_negative: true
    });

    $('#EndorsementOverride').NobleCount('#countEndorsementOverride', {
    	on_negative: function () {
    		$('#countEndorsementOverride').css('color', 'red')
    	}, on_positive: function () {
    		$('#countEndorsementOverride').css('color', 'black')
    	},
    	max_chars: 120,
    	block_negative: true
    });

	//Show OSI Boxes on edit
    $('.PolicySupplierDealCodeOSILine').hide();

    if ($('#OSIFlagNonNullable').is(':checked')) {

    	$('.PolicySupplierDealCodeOSILine').show();

    	//Character Count for each
    	$('.PolicySupplierDealCodeOSILine').each(function () {
			
    		var textarea = $(this).find('textarea');
    		var id = textarea.attr('id');
    		var number = id.replace('PolicySupplierDealCodeOSI_', '');

    		$('#PolicySupplierDealCodeOSI_' + number).NobleCount('#PolicySupplierDealCodeOSI_Count_' + number, {
    			on_negative: function () {
    				$('#PolicySupplierDealCodeOSI_Count_' + number).css('color', 'red')
    			}, on_positive: function () {
    				$('#PolicySupplierDealCodeOSI_Count_' + number).css('color', 'black')
    			},
    			max_chars: 80,
    			block_negative: true
    		});
    	});

    } else {
    	resetPolicySupplierDealCodeOSI();
    }

	//Show OSI Boxes when OSI Flag is ticked
    $('#OSIFlagNonNullable').change(function () {
    	if ($(this).is(':checked')) {

    		$('.PolicySupplierDealCodeOSILine').show();

    		//Character Count
    		$('#PolicySupplierDealCodeOSI_1').NobleCount('#PolicySupplierDealCodeOSI_Count_1', {
    			on_negative: function () {
    				$('#PolicySupplierDealCodeOSI_Count_1').css('color', 'red')
    			}, on_positive: function () {
    				$('#PolicySupplierDealCodeOSI_Count_1').css('color', 'black')
    			},
    			max_chars: 80,
    			block_negative: true
    		});

    	} else {
    		resetPolicySupplierDealCodeOSI();
    	}
    });

	//Clear and hide all options
    function resetPolicySupplierDealCodeOSI() {
    	$('.PolicySupplierDealCodeOSILine').slice(1).remove();
    	$('.PolicySupplierDealCodeOSILine').hide();
    	$('.PolicySupplierDealCodeOSILine .osi-input').val("");
    }

	//Add button
    $('.btn-add').live('click', function (e) {

    	e.preventDefault();

    	//Prevent adding new lines until existing filled in
    	var value = $(this).closest('.PolicySupplierDealCodeOSILine').find('.osi-input').val();
    	if (value == '') {
    		alert('Please complete field before adding new one');
    		return false;
    	}

    	//Clone last row and add to end
    	var lastItem = $('.PolicySupplierDealCodeOSILine').last().clone();
    	$('.PolicySupplierDealCodeOSILine').last().after(lastItem);

    	//Increment the id and label
    	var newItem = $('.PolicySupplierDealCodeOSILine').last();
    	var textarea = newItem.find('textarea');
    	var id = textarea.attr('id');
    	var number = id.replace('PolicySupplierDealCodeOSI_', '');
    	var newId = parseInt(number) + 1;

    	textarea.attr('id', 'PolicySupplierDealCodeOSI_' + newId);
    	textarea.attr('name', 'PolicySupplierDealCodeOSI_' + newId);
    	textarea.val('');

    	var label = newItem.find('label');
    	label.text('OSI ' + newId);
    	label.attr('id', 'PolicySupplierDealCodeOSILabel_' + newId);

    	var character_count = newItem.find('.character_count');
    	character_count.attr('id', 'PolicySupplierDealCodeOSI_Count_' + newId);

    	$('#PolicySupplierDealCodeOSI_' + newId).NobleCount('#PolicySupplierDealCodeOSI_Count_' + newId, {
    		on_negative: function () {
    			$('#PolicySupplierDealCodeOSI_Count_' + newId).css('color', 'red')
    		}, on_positive: function () {
    			$('#PolicySupplierDealCodeOSI_Count_' + newId).css('color', 'black')
    		},
    		max_chars: 80,
    		block_negative: true
    	});

    	updateRowColours();

    });

	//Remove btn
    $('.btn-remove').live('click', function (e) {

    	e.preventDefault();

    	//Remove all items but clear last remaining ones
    	var osi_count = $('.PolicySupplierDealCodeOSILine').length;
    	if (osi_count > 1) {
    		$(this).closest('.PolicySupplierDealCodeOSILine').remove();
    	} else {
    		$(this).closest('.PolicySupplierDealCodeOSILine').find('.osi-input').val('');
    	}

    	//If removed a middle one, update all numbers
    	for (var i = 0; i < $('.PolicySupplierDealCodeOSILine').length; i++) {

    		var item = $('.PolicySupplierDealCodeOSILine:eq(' + i + ')');

    		var newId = i + 1;

    		var textarea = item.find('textarea');
    		textarea.attr('id', 'PolicySupplierDealCodeOSI_' + newId);
    		textarea.attr('name', 'PolicySupplierDealCodeOSI_' + newId);

    		var label = item.find('label');
    		label.text('OSI ' + newId);
    		label.attr('id', 'PolicySupplierDealCodeOSILabel_' + newId);

    		var character_count = item.find('.character_count');
    		character_count.attr('id', 'PolicySupplierDealCodeOSI_Count_' + newId);
    	}

    	updateRowColours();

    });

    function updateRowColours() {
    	$("tr").removeClass("row_odd");
    	$("tr").removeClass("row_even");
    	$("tr:odd").addClass("row_odd");
    	$("tr:even").addClass("row_even");
    }

    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');

    //Show DatePickers
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

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#ProductId").val() == "") {
        $("#SupplierName").attr("disabled", true);
    } else {
        $("#SupplierName").removeAttr("disabled");
    }

    $("#ProductId").change(function () {
        $("#SupplierName").val("");
        $("#SupplierCode").val("");
        if ($("#ProductId").val() == "") {
            $("#SupplierName").attr("disabled", true);
        } else {
            $("#SupplierName").removeAttr("disabled");
        }
    });

    $("#SupplierName").change(function () {
        if ($("#SupplierName").val() == "") {
            $("#SupplierCode").val("");
        }
    });

    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
});
/////////////////////////////////////////////////////////
// BEGIN PRODUCT/SUPPLIER AUTOCOMPLETE
/////////////////////////////////////////////////////////
$(function () {
    $("#SupplierName").autocomplete({

        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                data: { searchText: request.term, productId: $("#ProductId").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            id: item.SupplierCode,
                            value: item.SupplierName
                        }
                    }))
                }
            })
        },
        mustMatch: true,
        select: function (event, ui) {
            $("#SupplierName").val(ui.item.value);
            $("#SupplierCode").val(ui.item.id);
            $("#lblSupplierNameMsg").text("");
        }

    });
});
/////////////////////////////////////////////////////////
// END PRODUCT/SUPPLIER AUTOCOMPLETE
/////////////////////////////////////////////////////////


//////////////////////////////////
//onSubmit
//////////////////////////////////
$.validator.setDefaults({
	submitHandler: function (form) {

		/////////////////////////////////////////////////////////
		// BEGIN PRODUCT/SUPPLIER VALIDATION
		// 1. Check Text for Supplier to see if valid
		// 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
		/////////////////////////////////////////////////////////
		var validSupplier = false;
		var validSupplierProduct = false;

		if ($("#SupplierName").val() != "") {
			jQuery.ajax({
				type: "POST",
				url: "/Validation.mvc/IsValidSupplierName",
				data: { supplierName: $("#SupplierName").val() },
				success: function (data) {

					if (!jQuery.isEmptyObject(data)) {
						validSupplier = true;

						//user may not use auto, need to populate ID field
						if ($("#SupplierCode").val() == "") {
							$("#SupplierCode").val(data[0].SupplierCode);
						}
					}
				},
				dataType: "json",
				async: false
			});

			if (!validSupplier) {
				$("#SupplierCode").val("");
				$("#lblSupplierNameMsg").removeClass('field-validation-valid');
				$("#lblSupplierNameMsg").addClass('field-validation-error');
				$("#lblSupplierNameMsg").text("This is not a valid Supplier");
				return false;
			} else {
				$("#lblSupplierNameMsg").text("");
			}

			jQuery.ajax({
				type: "POST",
				url: "/Validation.mvc/IsValidSupplierProduct",
				data: { supplierCode: $("#SupplierCode").val(), productId: $("#ProductId").val() },
				success: function (data) {

					if (!jQuery.isEmptyObject(data)) {
						validSupplierProduct = true;
					}
				},
				dataType: "json",
				async: false
			});
			if (!validSupplierProduct) {
				$("#lblSupplierNameMsg").removeClass('field-validation-valid');
				$("#lblSupplierNameMsg").addClass('field-validation-error');
				$("#lblSupplierNameMsg").text("This is not a valid Supplier");
				return false;
			} else {
				$("#lblSupplierNameMsg").text("");

				form.submit();
				return;
			}
		}

		/////////////////////////////////////////////////////////
		// END PRODUCT/SUPPLIER VALIDATION
		/////////////////////////////////////////////////////////
	}
});