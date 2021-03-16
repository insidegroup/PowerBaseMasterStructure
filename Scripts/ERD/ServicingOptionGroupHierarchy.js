$(document).ready(function () {
    $('#menu_servicingoptions').click();
});

function addRemoveClientSubUnit(guid) {
    $("#ClientSubUnitGuid").val(guid)
    document.forms["hierarchyform"].submit();
}