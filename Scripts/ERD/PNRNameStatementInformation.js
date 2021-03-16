/*
OnReady
*/
$(document).ready(function() {

	$('tr:odd').addClass('row_odd');
	$('tr:even').addClass('row_even');
	$('#breadcrumb').css('width', 'auto');


	$('.referToRecordIdentifier').change(function () {
	
		var selectedItem = $(this).find('option:selected').text();

		console.log(selectedItem);

		var id = $(this).attr('id');
		id = id.replace('PNRNameStatementInformation_Field', '');
		id = id.replace('_ReferToRecordIdentifier', '');

		console.log(id);

		var displayName = $('#PNRNameStatementInformation_Field' + id + '_DisplayName');

		console.log(displayName);

		if (selectedItem == 'Please Select...') {
			displayName.val('')
		} else if (selectedItem != null) {
			displayName.val(selectedItem)
		}

		//Choice of CDR in the Statement Info box will result in a PNR Mapping Code Type of 1
		//Choice of External System ID will result in a PNR Mapping Code Type of 2
		var PNRMappingTypeCode = $('#PNRNameStatementInformation_Field' + id + '_PNRMappingTypeCode');

		if (selectedItem == 'Please Select...') {
			PNRMappingTypeCode.val('');
		} else if (selectedItem == 'External System ID') {
			PNRMappingTypeCode.val('2');
		} else {
			PNRMappingTypeCode.val('1');
		}

	});
});
