/*
OnReady
*/
$(document).ready(function() {

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$("#breadcrumb").css("width", "auto");

	////The Remark Qualifier field should be disabled if the Remark Type is Itinerary Remark or Back Office Remark
	function showGDSRemarkQualifier() {
		var value = $('#ClientDefinedReferenceItemPNROutput_PNROutputRemarkTypeCode').val();
		if (value == '' || value == '1' || value == '3') {
			$('#ClientDefinedReferenceItemPNROutput_GDSRemarkQualifier').attr('disabled', true).val('');
		} else {
			$('#ClientDefinedReferenceItemPNROutput_GDSRemarkQualifier').attr('disabled', false);
		}
	}

	//On Load
	showGDSRemarkQualifier();

	//On Change
	$('#ClientDefinedReferenceItemPNROutput_PNROutputRemarkTypeCode').bind('change', function (e) {
		showGDSRemarkQualifier();
	});

});
