$(document).ready(function () {
	//Navigation
	$('#menu_admin').click();
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");

	function updateRowColours() {
		$("tr").removeClass("row_odd");
		$("tr").removeClass("row_even");
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	}

	//Hide panel initially if not set to AutomatedCommand (Edit Page)
	var defaultValue = $('#GDSEndWarningConfiguration_GDSEndWarningBehaviorTypeId option:selected').text();
	if (defaultValue != 'AutomatedCommand') {
		resetAutomatedCommands();
	}

	//Display AutomatedCommandsPanel when AutomatedCommand is selected from Behaviour dropdown 
	$('#GDSEndWarningConfiguration_GDSEndWarningBehaviorTypeId').change(function () {
		var selectedValue = $(this).find("option:selected").text();
		if (selectedValue == 'AutomatedCommand') {
			$('.AutomatedCommandsLine').show();
		} else {
			resetAutomatedCommands();
		}
	});

	function resetAutomatedCommands() {
		//Clear and hide all options
		$('.AutomatedCommandsLine').slice(1).remove();
		$('.AutomatedCommandsLine').hide();
		$('.AutomatedCommandsLine .automated-input').val("");
	}

	//Add button
	$('.btn-add').live('click', function (e) {

		e.preventDefault();

		////Prevent adding new lines until existing filled in
		//var value = $(this).closest('.AutomatedCommandsLine').find('.automated-input').val();
		//if (value == '') {
		//	alert('Please complete field before adding new one');
		//	return false;
		//}

		//Clone last row and add to end
		var lastItem = $('.AutomatedCommandsLine').last().clone();
		$('.AutomatedCommandsLine').last().after(lastItem);

		//Increment the id and label
		var newItem = $('.AutomatedCommandsLine').last();
		var input = newItem.find('input');
		var id = input.attr('id');
		var number = id.replace('AutomatedCommand_', '');
		var newId = parseInt(number) + 1;
		input.attr('id', 'AutomatedCommand_' + newId);
		input.attr('name', 'AutomatedCommand_' + newId);
		input.val('');
		var label = newItem.find('label');
		label.text('Automated Command ' + newId);
		label.attr('id', 'GDSEndWarningConfiguration_AutomatedCommand_' + newId);

		updateRowColours();
	});

	//Remove btn
	$('.btn-remove').live('click', function (e) {

		e.preventDefault();

		//Remove all items but clear last remaining ones
		var command_count = $('.AutomatedCommandsLine').length;
		if (command_count > 1) {
			$(this).closest('.AutomatedCommandsLine').remove();
		} else {
			$(this).closest('.AutomatedCommandsLine').find('.automated-input').val('');
		}

		//If removed a middle one, update all numbers
		for (var i = 0; i < $('.AutomatedCommandsLine').length; i++) {

			var item = $('.AutomatedCommandsLine:eq(' + i + ')');

			var newId = i + 1;

			var input = item.find('input');
			input.attr('id', 'AutomatedCommand_' + newId);
			input.attr('name', 'AutomatedCommand_' + newId);

			var label = item.find('label');
			label.text('Automated Command ' + newId);
			label.attr('id', 'GDSEndWarningConfiguration_AutomatedCommand_' + newId);
		}

		updateRowColours();

	});

	//Submit Form Validation
	$('#form0').submit(function () {

		var isValid = true;

		//Prevent submitting blank lines
		$('.automated-error').remove();
		$('.automated-input:visible').each(function () {
			if ($(this).val() == '') {
				$(this).closest('td').next().append('<span class="error automated-error">Required</span>');
				isValid = false;
			}
		});

		return isValid;
	});
});