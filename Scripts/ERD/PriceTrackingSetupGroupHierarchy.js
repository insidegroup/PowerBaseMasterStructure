$(document).ready(function () {
    $('#menu_pricetracking').click();
});

function addRemoveClientSubUnit(guid) {
    $("#ClientSubUnitGuid").val(guid)
    document.forms["hierarchyform"].submit();
}