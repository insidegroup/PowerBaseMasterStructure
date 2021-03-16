/*
OnReady
*/
$(document).ready(function () {

	//Navigation
	$(".main-table tr th, .main-table tr td").css("background-color", "#fff");

	//Breadcrumbs
	$('#breadcrumb').css('width', 'auto');
	$('.full-width #search_wrapper').css('height', '22px');

	//Show DatePickers
	$('#ClientDefinedRuleGroup_ExpiryDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});
	$('#ClientDefinedRuleGroup_EnabledDate').datepicker({
		constrainInput: true,
		buttonImageOnly: true,
		showOn: 'button',
		buttonImage: '/Images/Common/Calendar.png',
		dateFormat: 'M dd yy',
		duration: 0
	});
	if ($('#ClientDefinedRuleGroup_EnabledDate').val() == "") {
		$('#ClientDefinedRuleGroup_EnabledDate').val("No Enabled Date")
	}
	if ($('#ClientDefinedRuleGroup_ExpiryDate').val() == "") {
		$('#ClientDefinedRuleGroup_ExpiryDate').val("No Expiry Date")
	}
	
	/*
    Business Rule Conditions
    */

	//Add button
	$('.clientDefinedRuleGroupLogicRow .btn-add').live('click', function (e) {

		e.preventDefault();

		var validForm = true;

		//Prevent adding new lines until existing lines filled in
		$('.clientDefinedRuleGroupLogicRow').each(function () {

			var row = $(this);
			var clientDefinedRuleBusinessEntityId = row.find('.ClientDefinedRuleBusinessEntityId').val();
			var clientDefinedRuleRelationalOperatorId = row.find('.ClientDefinedRuleRelationalOperatorId').val();
			var clientDefinedRuleLogicItemValue = row.find('.ClientDefinedRuleLogicItemValue').val(); //Can be blank (US4919)

			if (clientDefinedRuleBusinessEntityId == '' || clientDefinedRuleRelationalOperatorId == '') {
				alert('Please complete field before adding new one');
				validForm = false;
			}
		});

		if (!validForm) {
			return false;
		}

		//Clone last row and add to end
		var clonedItem = $('.clientDefinedRuleGroupLogicRow').last().clone();
		$('.clientDefinedRuleGroupLogicRow').last().after(clonedItem);

		var newItem = $('.clientDefinedRuleGroupLogicRow').last();
		var itemCounter = newItem.find('.ClientDefinedRuleGroupLogicRowLabel').text();
		var newId = parseInt(itemCounter) + 1;
		newItem.attr('id', 'ClientDefinedRuleGroupLogicRow_' + newId);

		var ClientDefinedRuleGroupLogicRowLabel = newItem.find(".ClientDefinedRuleGroupLogicRowLabel");
		ClientDefinedRuleGroupLogicRowLabel.attr('id', 'ClientDefinedRuleGroupLogicRowLabel_' + newId);
		ClientDefinedRuleGroupLogicRowLabel.text(newId);

		var ClientDefinedRuleBusinessEntityId = newItem.find(".ClientDefinedRuleBusinessEntityId");
		ClientDefinedRuleBusinessEntityId.attr('id', 'ClientDefinedRuleGroupLogic_ClientDefinedRuleLogicItem_ClientDefinedRuleBusinessEntityId_' + newId).val('');
		ClientDefinedRuleBusinessEntityId.attr('name', 'ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId_' + newId);

		var ClientDefinedRuleBusinessEntityName = newItem.find(".ClientDefinedRuleBusinessEntityName");
		ClientDefinedRuleBusinessEntityName.attr('id', 'ClientDefinedRuleGroupLogic_ClientDefinedRuleLogicItem_ClientDefinedRuleBusinessEntityName_' + newId).val('');
		ClientDefinedRuleBusinessEntityName.attr('name', 'ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName_' + newId);

		var ClientDefinedRuleRelationalOperatorId = newItem.find(".ClientDefinedRuleRelationalOperatorId");
		ClientDefinedRuleRelationalOperatorId.attr('id', 'ClientDefinedRuleGroupLogic_ClientDefinedRuleLogicItem_ClientDefinedRuleRelationalOperatorId_' + newId).val('');
		ClientDefinedRuleRelationalOperatorId.attr('name', 'ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId_' + newId);

		var ClientDefinedRuleLogicItemValue = newItem.find(".ClientDefinedRuleLogicItemValue");
		ClientDefinedRuleLogicItemValue.attr('id', 'ClientDefinedRuleGroupLogic_ClientDefinedRuleLogicItem_ClientDefinedRuleLogicItemValue_' + newId).val('');
		ClientDefinedRuleLogicItemValue.attr('name', 'ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue_' + newId);

		$('.clientDefinedRuleGroupLogicRow .btn-remove').each(function () {
			$(this).css('visibility', 'visible');
		});

		$(ClientDefinedRuleBusinessEntityName).autocomplete({
			source: function (request, response) {
				$.ajax({
					url: "/ClientDefinedRuleGroup.mvc/AutoCompleteClientDefinedRuleLogicBusinessEntities", type: "POST", dataType: "json",
					data: { searchText: request.term },
					success: function (data) {
						response($.map(data, function (item) {
							return {
								label: item.BusinessEntityDescription,
								value: item.ClientDefinedRuleBusinessEntityId,
								id: item.ClientDefinedRuleBusinessEntityId,
								text: item.BusinessEntityDescription,
								ClientDefinedRuleBusinessEntityId: item.ClientDefinedRuleBusinessEntityId,
								BusinessEntityDescription: item.BusinessEntityDescription,
							}
						}))
					}
				});
			},
			select: function (event, ui) {
				$(ClientDefinedRuleBusinessEntityId).val(ui.item.ClientDefinedRuleBusinessEntityId);
				$(ClientDefinedRuleBusinessEntityName).val(ui.item.BusinessEntityDescription);
				return false;
			}
		});

	});

	//Remove btn
	$('.clientDefinedRuleGroupLogicRow .btn-remove').live('click', function (e) {

		e.preventDefault();

		var row = $(this).parent().parent();

		if ($('.clientDefinedRuleGroupLogicRow').length == 1) {

			//Reset item
			var item = $('.clientDefinedRuleGroupLogicRow:eq(' + 0 + ')');

			var ClientDefinedRuleBusinessEntityId = item.find(".ClientDefinedRuleBusinessEntityId");
			ClientDefinedRuleBusinessEntityId.val('');

			var ClientDefinedRuleRelationalOperatorId = item.find(".ClientDefinedRuleRelationalOperatorId");
			ClientDefinedRuleRelationalOperatorId.val('');

			var ClientDefinedRuleLogicItemValue = item.find(".ClientDefinedRuleLogicItemValue");
			ClientDefinedRuleLogicItemValue.val('');

		} else {

			//Remove item
			row.remove();

			//If removed a middle one, update all numbers
			for (var i = 0; i < $('.clientDefinedRuleGroupLogicRow').length; i++) {

				var newId = i + 1;

				var item = $('.clientDefinedRuleGroupLogicRow:eq(' + i + ')');
				item.attr('id', 'ClientDefinedRuleGroupLogicRow_' + newId);

				var ClientDefinedRuleGroupLogicRowLabel = item.find(".ClientDefinedRuleGroupLogicRowLabel");
				ClientDefinedRuleGroupLogicRowLabel.attr('id', 'ClientDefinedRuleGroupLogicRowLabel_' + newId);
				ClientDefinedRuleGroupLogicRowLabel.attr('name', 'ClientDefinedRuleGroupLogicRowLabel_' + newId);
				ClientDefinedRuleGroupLogicRowLabel.text(newId);

				var ClientDefinedRuleBusinessEntityId = item.find(".ClientDefinedRuleBusinessEntityId");
				ClientDefinedRuleBusinessEntityId.attr('id', 'ClientDefinedRuleGroupLogic_ClientDefinedRuleLogicItem_ClientDefinedRuleBusinessEntityId_' + newId);
				ClientDefinedRuleBusinessEntityId.attr('name', 'ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId_' + newId);

				var ClientDefinedRuleRelationalOperatorId = item.find(".ClientDefinedRuleRelationalOperatorId");
				ClientDefinedRuleRelationalOperatorId.attr('id', 'ClientDefinedRuleGroupLogic_ClientDefinedRuleLogicItem_ClientDefinedRuleRelationalOperatorId_' + newId);
				ClientDefinedRuleRelationalOperatorId.attr('name', 'ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId_' + newId);

				var ClientDefinedRuleLogicItemValue = item.find(".ClientDefinedRuleLogicItemValue");
				ClientDefinedRuleLogicItemValue.attr('id', 'ClientDefinedRuleGroupLogic_ClientDefinedRuleLogicItem_ClientDefinedRuleLogicItemValue_' + newId);
				ClientDefinedRuleLogicItemValue.attr('name', 'ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue_' + newId);
			}
		}
	});


	/*
	Business Rule Results
	*/

	//Add button
	$('.clientDefinedRuleGroupResultRow .btn-add').live('click', function (e) {

		e.preventDefault();

		var validForm = true;

		//Prevent adding new lines until existing lines filled in
		$('.clientDefinedRuleGroupResultRow').each(function () {

			var row = $(this);
			var clientDefinedRuleBusinessEntityId = row.find('.ClientDefinedRuleBusinessEntityId').val();
			var clientDefinedRuleResultItemValue = row.find('.ClientDefinedRuleResultItemValue').val();

			if (clientDefinedRuleBusinessEntityId == '' || clientDefinedRuleResultItemValue == '') {
				alert('Please complete field before adding new one');
				validForm = false;
			}
		});

		if (!validForm) {
			return false;
		}

		//Clone last row and add to end
		var clonedItem = $('.clientDefinedRuleGroupResultRow').last().clone();
		$('.clientDefinedRuleGroupResultRow').last().after(clonedItem);

		var newItem = $('.clientDefinedRuleGroupResultRow').last();
		var itemCounter = newItem.attr('id').replace('ClientDefinedRuleGroupResultRow_', '');
		var newId = parseInt(itemCounter) + 1;
		newItem.attr('id', 'ClientDefinedRuleGroupResultRow_' + newId);

		var ClientDefinedRuleBusinessEntityId = newItem.find(".ClientDefinedRuleBusinessEntityId");
		ClientDefinedRuleBusinessEntityId.attr('id', 'ClientDefinedRuleGroupResult_ClientDefinedRuleResultItem_ClientDefinedRuleBusinessEntityId_' + newId).val('');
		ClientDefinedRuleBusinessEntityId.attr('name', 'ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId_' + newId);

		var ClientDefinedRuleBusinessEntityName = newItem.find(".ClientDefinedRuleBusinessEntityName");
		ClientDefinedRuleBusinessEntityName.attr('id', 'ClientDefinedRuleGroupResult_ClientDefinedRuleResultItem_ClientDefinedRuleBusinessEntityName_' + newId).val('');
		ClientDefinedRuleBusinessEntityName.attr('name', 'ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName_' + newId);

		var ClientDefinedRuleResultItemValue = newItem.find(".ClientDefinedRuleResultItemValue");
		ClientDefinedRuleResultItemValue.attr('id', 'ClientDefinedRuleGroupResult_ClientDefinedRuleResultItem_ClientDefinedRuleResultItemValue_' + newId).val('');
		ClientDefinedRuleResultItemValue.attr('name', 'ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue_' + newId);

		$(ClientDefinedRuleBusinessEntityName).autocomplete({
			source: function (request, response) {
				$.ajax({
					url: "/ClientDefinedRuleGroup.mvc/AutoCompleteClientDefinedRuleResultBusinessEntities", type: "POST", dataType: "json",
					data: { searchText: request.term },
					success: function (data) {
						response($.map(data, function (item) {
							return {
								label: item.BusinessEntityDescription,
								value: item.ClientDefinedRuleBusinessEntityId,
								id: item.ClientDefinedRuleBusinessEntityId,
								text: item.BusinessEntityDescription,
								ClientDefinedRuleBusinessEntityId: item.ClientDefinedRuleBusinessEntityId,
								BusinessEntityDescription: item.BusinessEntityDescription,
							}
						}))
					}
				});
			},
			select: function (event, ui) {
				$(ClientDefinedRuleBusinessEntityId).val(ui.item.ClientDefinedRuleBusinessEntityId);
				$(ClientDefinedRuleBusinessEntityName).val(ui.item.BusinessEntityDescription);
				return false;
			}
		});

	});

	//Remove btn
	$('.clientDefinedRuleGroupResultRow .btn-remove').live('click', function (e) {

		e.preventDefault();

		var row = $(this).parent().parent();

		if ($('.clientDefinedRuleGroupResultRow').length == 1) {

			//Reset item
			var item = $('.clientDefinedRuleGroupResultRow:eq(' + 0 + ')');

			var ClientDefinedRuleBusinessEntityId = item.find(".ClientDefinedRuleBusinessEntityId");
			ClientDefinedRuleBusinessEntityId.val('');

			var ClientDefinedRuleResultItemValue = item.find(".ClientDefinedRuleResultItemValue");
			ClientDefinedRuleResultItemValue.val('');

		} else {

			//Remove item
			row.remove();

			//If removed a middle one, update all numbers
			for (var i = 0; i < $('.clientDefinedRuleGroupResultRow').length; i++) {

				var newId = i + 1;

				var item = $('.clientDefinedRuleGroupResultRow:eq(' + i + ')');
				item.attr('id', 'ClientDefinedRuleGroupResultRow_' + newId);

				var ClientDefinedRuleBusinessEntityId = item.find(".ClientDefinedRuleBusinessEntityId");
				ClientDefinedRuleBusinessEntityId.attr('id', 'ClientDefinedRuleGroupResult_ClientDefinedRuleResultItem_ClientDefinedRuleBusinessEntityId_' + newId);
				ClientDefinedRuleBusinessEntityId.attr('name', 'ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId_' + newId);

				var ClientDefinedRuleResultItemValue = item.find(".ClientDefinedRuleResultItemValue");
				ClientDefinedRuleResultItemValue.attr('id', 'ClientDefinedRuleGroupResult_ClientDefinedRuleResultItem_ClientDefinedRuleResultItemValue_' + newId);
				ClientDefinedRuleResultItemValue.attr('name', 'ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue_' + newId);
			}
		}
	});


	/*
	Business Rule Triggers
	*/

	//Add button
	$('.clientDefinedRuleGroupTriggerRow .btn-add').live('click', function (e) {

		e.preventDefault();

		var validForm = true;

		//Prevent adding new lines until existing lines filled in
		$('.clientDefinedRuleGroupTriggerRow').each(function () {

			var row = $(this);
			var clientDefinedRuleWorkflowTriggerStateId = row.find('.ClientDefinedRuleWorkflowTriggerStateId').val();
			var clientDefinedRuleWorkflowTriggerApplicationModeId = row.find('ClientDefinedRuleWorkflowTriggerApplicationModeId').val();

			if (clientDefinedRuleWorkflowTriggerStateId == '' || clientDefinedRuleWorkflowTriggerApplicationModeId == '') {
				alert('Please complete field before adding new one');
				validForm = false;
			}
		});

		if (!validForm) {
			return false;
		}

		//Clone last row and add to end
		var clonedItem = $('.clientDefinedRuleGroupTriggerRow').last().clone();
		$('.clientDefinedRuleGroupTriggerRow').last().after(clonedItem);

		var newItem = $('.clientDefinedRuleGroupTriggerRow').last();
		var itemCounter = newItem.attr('id').replace('ClientDefinedRuleGroupTriggerRow_', '');
		var newId = parseInt(itemCounter) + 1;
		newItem.attr('id', 'ClientDefinedRuleGroupTriggerRow_' + newId);

		var ClientDefinedRuleWorkflowTriggerStateId = newItem.find(".ClientDefinedRuleWorkflowTriggerStateId");
		ClientDefinedRuleWorkflowTriggerStateId.attr('id', 'ClientDefinedRuleGroupTrigger_ClientDefinedRuleWorkflowTrigger_ClientDefinedRuleWorkflowTriggerStateId_' + newId).val('');
		ClientDefinedRuleWorkflowTriggerStateId.attr('name', 'ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId_' + newId);

		var ClientDefinedRuleWorkflowTriggerApplicationModeId = newItem.find(".ClientDefinedRuleWorkflowTriggerApplicationModeId");
		ClientDefinedRuleWorkflowTriggerApplicationModeId.attr('id', 'ClientDefinedRuleGroupTrigger_ClientDefinedRuleWorkflowTrigger_ClientDefinedRuleWorkflowTriggerApplicationModeId_' + newId).val('');
		ClientDefinedRuleWorkflowTriggerApplicationModeId.attr('name', 'ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId_' + newId);
	});

	//Remove btn
	$('.clientDefinedRuleGroupTriggerRow .btn-remove').live('click', function (e) {

		e.preventDefault();

		var row = $(this).parent().parent();

		if ($('.clientDefinedRuleGroupTriggerRow').length == 1) {

			//Reset item
			var item = $('.clientDefinedRuleGroupTriggerRow:eq(' + 0 + ')');

			var ClientDefinedRuleWorkflowTriggerStateId = item.find(".ClientDefinedRuleWorkflowTriggerStateId");
			ClientDefinedRuleWorkflowTriggerStateId.val('');

			var ClientDefinedRuleWorkflowTriggerApplicationModeId = item.find(".ClientDefinedRuleWorkflowTriggerApplicationModeId");
			ClientDefinedRuleWorkflowTriggerApplicationModeId.val('');

		} else {

			//Remove item
			row.remove();

			//If removed a middle one, update all numbers
			for (var i = 0; i < $('.clientDefinedRuleGroupTriggerRow').length; i++) {

				var newId = i + 1;

				var item = $('.clientDefinedRuleGroupTriggerRow:eq(' + i + ')');
				item.attr('id', 'ClientDefinedRuleGroupTriggerRow_' + newId);

				var ClientDefinedRuleWorkflowTriggerStateId = item.find(".ClientDefinedRuleWorkflowTriggerStateId");
				ClientDefinedRuleWorkflowTriggerStateId.attr('id', 'ClientDefinedRuleGroupTrigger_ClientDefinedRuleWorkflowTrigger_ClientDefinedRuleWorkflowTriggerStateId__' + newId);
				ClientDefinedRuleWorkflowTriggerStateId.attr('name', 'ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId_' + newId);

				var ClientDefinedRuleWorkflowTriggerApplicationModeId = item.find(".ClientDefinedRuleWorkflowTriggerApplicationModeId");
				ClientDefinedRuleWorkflowTriggerApplicationModeId.attr('id', 'ClientDefinedRuleGroupTrigger_ClientDefinedRuleWorkflowTrigger_ClientDefinedRuleWorkflowTriggerApplicationModeId_' + newId);
				ClientDefinedRuleWorkflowTriggerApplicationModeId.attr('name', 'ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId_' + newId);
			}
		}
	});

	/*
    Submit Form Validation
    */
	$('#form0').submit(function () {

		var validItem = true;

		//wait for this name to be populated, dont show message
		if ($("#ClientDefinedRuleGroup_ClientDefinedRuleGroupId").val() == "0") {
			if ($("#lblAuto").text() == "") {
				return false;
			}
		} else {
			if (jQuery.trim($("#ClientDefinedRuleGroup_ClientDefinedRuleGroupName").val()) == "") {
				$("#ClientDefinedRuleGroup_ClientDefinedRuleGroupName_validationMessage").removeClass('field-validation-valid');
				$("#ClientDefinedRuleGroup_ClientDefinedRuleGroupName_validationMessage").addClass('field-validation-error');
				$("#ClientDefinedRuleGroup_ClientDefinedRuleGroupName_validationMessage").text("Client Defined Rule Group Name Required.");
				return false;
			} else {
				$("#ClientDefinedRuleGroupName_validationMessage").text("");
			}
		}

		//GroupName Begin
		var validGroupName = false;
		var id = $("#ClientDefinedRuleGroup_ClientDefinedRuleGroupId").length > 0 ? $("#ClientDefinedRuleGroup_ClientDefinedRuleGroupId").val() : 0;
	
		jQuery.ajax({
			type: "POST",
			url: "/GroupNameBuilder.mvc/IsAvailableClientDefinedBusinessRuleGroupName",
			data: {
				groupName: $("#ClientDefinedRuleGroup_ClientDefinedRuleGroupName").val(),
				category: $("#ClientDefinedRuleGroup_Category").val(),
				id: id
			},
			success: function (data) {

				validGroupName = data;
			},
			dataType: "json",
			async: false
		});

		if (!validGroupName) {

			$("#lblClientDefinedRuleGroupNameMsg").removeClass('field-validation-valid');
			$("#lblClientDefinedRuleGroupNameMsg").addClass('field-validation-error');
			if ($("#ClientDefinedRuleGroup_ClientDefinedRuleGroupId").val() == "0") {//Create
				$("#lblClientDefinedRuleGroupNameMsg").text("This name has already been used with this category, please choose a different name.");
			} else {
				if ($("#ClientDefinedRuleGroup_ClientDefinedRuleGroupName").val() != "") {
					$("#lblClientDefinedRuleGroupNameMsg").text("This name has already been used with this category, please choose a different name.");
				}
			} return false;
		} else {
			$("#lblClientDefinedRuleGroupNameMsg").text("");
		}

		//GroupName End
		if (!$(this).valid()) {
			return false;
		}

		//The Unique constraints within Business Rule Conditions are the Conditions Description, Operator and Logic Values
		//A user cannot be allowed to duplicate conditions within the same group 
		var clientDefinedRuleGroupLogicRows = [];
		$('.clientDefinedRuleGroupLogicRow').each(function () {
			var row = $(this);
			var clientDefinedRuleBusinessEntityId = row.find('.ClientDefinedRuleBusinessEntityId').val();
			var clientDefinedRuleRelationalOperatorId = row.find('.ClientDefinedRuleRelationalOperatorId').val();
			var clientDefinedRuleLogicItemValue = row.find('.ClientDefinedRuleLogicItemValue').val();
			clientDefinedRuleGroupLogicRows.push(clientDefinedRuleBusinessEntityId + '_' + clientDefinedRuleRelationalOperatorId + '_' + clientDefinedRuleLogicItemValue);
		});

		var clientDefinedRuleGroupLogicRowsArray = clientDefinedRuleGroupLogicRows.sort();
		for (var i = 0; i < clientDefinedRuleGroupLogicRowsArray.length - 1; i++) {
			if (clientDefinedRuleGroupLogicRowsArray[i + 1] == clientDefinedRuleGroupLogicRowsArray[i]) {
				alert('Please define unique conditions for Business Rule Conditions');
				return false;
			}
		}

		//The Unique constraints within Business Rule Results are the Results Description, and Results Values
		//A user cannot be allowed to duplicate result items within the same group
		var clientDefinedRuleGroupResultRows = [];
		$('.clientDefinedRuleGroupResultRow').each(function () {
			var row = $(this);
			var clientDefinedRuleBusinessEntityId = row.find('.ClientDefinedRuleBusinessEntityId').val();
			var ClientDefinedRuleResultItemValue = row.find('.ClientDefinedRuleResultItemValue').val();
			clientDefinedRuleGroupResultRows.push(clientDefinedRuleBusinessEntityId + '_' + ClientDefinedRuleResultItemValue);
		});

		var clientDefinedRuleGroupResultRowsArray = clientDefinedRuleGroupResultRows.sort();
		for (var i = 0; i < clientDefinedRuleGroupResultRowsArray.length - 1; i++) {
			if (clientDefinedRuleGroupResultRowsArray[i + 1] == clientDefinedRuleGroupResultRowsArray[i]) {
				alert('Please define unique conditions for Business Rule Results');
				return false;
			}
		}

		//The Unique constraints within Business Rule Triggers are the Workflow Trigger and Application Mode
		//A user cannot be allowed to duplicate the same trigger items values     
		var clientDefinedRuleGroupTriggerRows = [];
		$('.clientDefinedRuleGroupTriggerRow').each(function () {
			var row = $(this);
			var ClientDefinedRuleWorkflowTriggerStateId = row.find('.ClientDefinedRuleWorkflowTriggerStateId').val();
			var ClientDefinedRuleWorkflowTriggerApplicationModeId = row.find('.ClientDefinedRuleWorkflowTriggerApplicationModeId').val();
			clientDefinedRuleGroupTriggerRows.push(ClientDefinedRuleWorkflowTriggerStateId + '_' + ClientDefinedRuleWorkflowTriggerApplicationModeId);
		});

		var clientDefinedRuleGroupTriggerRowsArray = clientDefinedRuleGroupTriggerRows.sort();
		for (var i = 0; i < clientDefinedRuleGroupTriggerRowsArray.length - 1; i++) {
			if (clientDefinedRuleGroupTriggerRowsArray[i + 1] == clientDefinedRuleGroupTriggerRowsArray[i]) {
				alert('Please define unique conditions for Business Rule Triggers');
				return false;
			}
		}

		//Check Required Business Rule Conditions Fields
		var validFields = true;
		var validDescriptions = true;
		$('.clientDefinedRuleGroupLogicRow').each(function () {
			var row = $(this);
			var clientDefinedRuleBusinessEntityId = row.find('.ClientDefinedRuleBusinessEntityId').val();
			var clientDefinedRuleBusinessEntityName = row.find('.ClientDefinedRuleBusinessEntityName').val();
			var clientDefinedRuleRelationalOperatorId = row.find('.ClientDefinedRuleRelationalOperatorId').val();
			var clientDefinedRuleLogicItemValue = row.find('.ClientDefinedRuleLogicItemValue').val(); //Can be blank (US4919)

			//Enforce selection of description
			if (clientDefinedRuleBusinessEntityName != "" && clientDefinedRuleBusinessEntityId == "") {
				validDescriptions = false;
			}

			if ((clientDefinedRuleBusinessEntityName == "" || clientDefinedRuleRelationalOperatorId == "") && !(clientDefinedRuleBusinessEntityName == "" && clientDefinedRuleRelationalOperatorId == "")) {
				validFields = false;
			}
		});

		if (!validFields) {
			alert('Please complete or remove any incomplete rows for Business Rule Conditions');
			return false;
		}

		if (!validDescriptions) {
			alert('Please select a valid description for Business Rule Conditions');
			return false;
		}

		//Check Required Business Rule Results Fields
		$('.clientDefinedRuleGroupResultRow').each(function () {
			var row = $(this);
			var clientDefinedRuleBusinessEntityId = row.find('.ClientDefinedRuleBusinessEntityId').val();
			var clientDefinedRuleBusinessEntityName = row.find('.ClientDefinedRuleBusinessEntityName').val();
            var clientDefinedRuleResultItemValue = row.find('.ClientDefinedRuleResultItemValue').val();

			//Enforce selection of description
            if (clientDefinedRuleBusinessEntityName != "" && clientDefinedRuleBusinessEntityId == "") {
            	validDescriptions = false;
            }

            if ((clientDefinedRuleBusinessEntityName == "" || clientDefinedRuleResultItemValue == "") && !(clientDefinedRuleBusinessEntityName == "" && clientDefinedRuleResultItemValue == "")) {
				validFields = false;
			}
		});

		if (!validFields) {
			alert('Please complete or remove all incomplete rows for Business Rule Results');
			return false;
		}

		if (!validDescriptions) {
			alert('Please select a valid description for Business Rule Results');
			return false;
		}

		//Check Required Business Rule Triggers Fields
		$('.clientDefinedRuleGroupTriggerRow').each(function () {
			var row = $(this);
			var clientDefinedRuleWorkflowTriggerStateId = row.find('.ClientDefinedRuleWorkflowTriggerStateId').val();
			var clientDefinedRuleWorkflowTriggerApplicationModeId = row.find('.ClientDefinedRuleWorkflowTriggerApplicationModeId').val();

			if ((clientDefinedRuleWorkflowTriggerStateId == "" || clientDefinedRuleWorkflowTriggerApplicationModeId == "") && !(clientDefinedRuleWorkflowTriggerStateId == "" && clientDefinedRuleWorkflowTriggerApplicationModeId == "")) {
				validFields = false;
			}
		});

		if (!validFields) {
			alert('Please complete or remove all incomplete rows for Business Rule Triggers');
			return false;
		}

		//In order to save the group they must have defined at least one result and one trigger
		var clientDefinedRuleBusinessEntityId = $('#ClientDefinedRuleGroupResult_ClientDefinedRuleResultItem_ClientDefinedRuleBusinessEntityId_1').val();
		var clientDefinedRuleResultItemValue = $('#ClientDefinedRuleGroupResult_ClientDefinedRuleResultItem_ClientDefinedRuleResultItemValue_1').val();
		var clientDefinedRuleWorkflowTriggerStateId = $('#ClientDefinedRuleGroupTrigger_ClientDefinedRuleWorkflowTrigger_ClientDefinedRuleWorkflowTriggerStateId_1').val();
		var clientDefinedRuleWorkflowTriggerApplicationModeId = $('#ClientDefinedRuleGroupTrigger_ClientDefinedRuleWorkflowTrigger_ClientDefinedRuleWorkflowTriggerApplicationModeId_1').val();

		if (clientDefinedRuleBusinessEntityId == "" || clientDefinedRuleResultItemValue == "" || clientDefinedRuleWorkflowTriggerStateId == "" || clientDefinedRuleWorkflowTriggerApplicationModeId == "") {
			alert('Please define at least one Business Rule Result and one Business Rule Trigger');
			return false;
		}

		if (validItem) {
			if ($('#ClientDefinedRuleGroup_ExpiryDate').val() == "No Expiry Date") {
				$('#ClientDefinedRuleGroup_ExpiryDate').val("");
			}
			if ($('#ClientDefinedRuleGroup_EnabledDate').val() == "No Enabled Date") {
				$('#ClientDefinedRuleGroup_EnabledDate').val("");
			}
			return true;
		} else {
			return false
		};
	});

});

/* Autocompletes */
$(function () {

	//ClientDefinedRuleLogicBusinessEntities
	$(".clientDefinedRuleGroupLogicRow .ClientDefinedRuleBusinessEntityName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/ClientDefinedRuleGroup.mvc/AutoCompleteClientDefinedRuleLogicBusinessEntities", type: "POST", dataType: "json",
				data: { searchText: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.BusinessEntityDescription,
							value: item.ClientDefinedRuleBusinessEntityId,
							id: item.ClientDefinedRuleBusinessEntityId,
							text: item.BusinessEntityDescription,
							ClientDefinedRuleBusinessEntityId: item.ClientDefinedRuleBusinessEntityId,
							BusinessEntityDescription: item.BusinessEntityDescription,
						}
					}))
				}
			});
		},
		select: function (event, ui) {
			$(this).val(ui.item.BusinessEntityDescription);
			$(this).prev().val(ui.item.ClientDefinedRuleBusinessEntityId);
			return false;
		}
	});

	//ClientDefinedRuleBusinessEntityName
	$(".clientDefinedRuleGroupResultRow .ClientDefinedRuleBusinessEntityName").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: "/ClientDefinedRuleGroup.mvc/AutoCompleteClientDefinedRuleResultBusinessEntities", type: "POST", dataType: "json",
				data: { searchText: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.BusinessEntityDescription,
							value: item.ClientDefinedRuleBusinessEntityId,
							id: item.ClientDefinedRuleBusinessEntityId,
							text: item.BusinessEntityDescription,
							ClientDefinedRuleBusinessEntityId: item.ClientDefinedRuleBusinessEntityId,
							BusinessEntityDescription: item.BusinessEntityDescription,
						}
					}))
				}
			});
		},
		select: function (event, ui) {
			$(this).val(ui.item.BusinessEntityDescription);
			$(this).prev().val(ui.item.ClientDefinedRuleBusinessEntityId);
			return false;
		}
	});
});