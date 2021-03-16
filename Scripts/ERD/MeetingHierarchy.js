$(document).ready(function () {
	$('#menu_meetings').click();
	$('#search_wrapper').css('height', '24px');
});

function addRemoveClientSubUnit(guid) {
    $("#ClientSubUnitGuid").val(guid)
    document.forms["hierarchyform"].submit();
}