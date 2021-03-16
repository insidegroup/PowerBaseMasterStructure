$(document).ready(function () {
    $('#menu_triptypegroups').click();
});

function addRemoveClientSubUnit(guid) {
    $("#ClientSubUnitGuid").val(guid)
    document.forms["hierarchyform"].submit();
}