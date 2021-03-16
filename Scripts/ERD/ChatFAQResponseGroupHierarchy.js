$(document).ready(function () {
    $('#menu_chatmessages').click();
});

function addRemoveClientSubUnit(guid) {
    $("#ClientSubUnitGuid").val(guid)
    document.forms["hierarchyform"].submit();
}