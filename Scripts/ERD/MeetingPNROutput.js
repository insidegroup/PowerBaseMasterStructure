/*
OnReady
*/
$(document).ready(function() {

	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
	$("#breadcrumb").css("width", "auto");
	$('#search_wrapper').css('height', '24px');

	//The Remark Qualifier field will only be enabled if the Remark Type is General Remark, Phone Field or Phone Field-Email 
	function showGDSRemarkQualifier() {
		var value = $('#MeetingPNROutput_PNROutputRemarkTypeCode').val();
		if (value == '0' || value == '2' || value == '11') {
			$('#MeetingPNROutput_GDSRemarkQualifier').attr('disabled', false);
		} else {
			$('#MeetingPNROutput_GDSRemarkQualifier').attr('disabled', true).val('');
		}
	}

	//On Load
	showGDSRemarkQualifier();

	//On Change
	$('#MeetingPNROutput_PNROutputRemarkTypeCode').bind('change', function (e) {
		showGDSRemarkQualifier();
	});

});
