$(document).ready(function () {
    $('#menu_policies').click();
});

function addRemoveClientSubUnit(guid) {
    $("#ClientSubUnitGuid").val(guid)
    document.forms["hierarchyform"].submit();
}