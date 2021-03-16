/*
OnReady
*/
$(document).ready(function () {
    //navigation
    $('#menu_clientfeegroups').click();

    $("#hierarchysearchform").submit(function () {
        if ($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val() == "") {
            $("#errorMessageSearchProperty").text("Please choose a Hierarchy Level");
            return false;
        }
        if ($("#hierarchysearchform input[name='FilterHierarchySearchText']").val().trim().length < 2) {
            $("#errorMessageSearchText").text("Please enter a minimum of 2 characters");
            return false;
        }
    });
});



function addRemoveClientAccount(ClientAccountNumber, SourceSystemCode) {
    $("#hierarchyform input[name='FilterHierarchySearchText']").val($("#hierarchysearchform input[name='FilterHierarchySearchText']").val())
    $("#hierarchyform input[name='FilterHierarchySearchProperty']").val($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val())
    $("#hierarchyform input[name='HierarchyType']").val("ClientAccount")
    $("#HierarchyCode").val(ClientAccountNumber)
    $("#SourceSystemCode").val(SourceSystemCode)
    document.forms["hierarchyform"].submit();
}
function addRemoveClientSubUnitTravelerType(ClientSubUnitGuid, TravelerTypeGuid) {
    $("#hierarchyform input[name='FilterHierarchySearchText']").val($("#hierarchysearchform input[name='FilterHierarchySearchText']").val())
    $("#hierarchyform input[name='FilterHierarchySearchProperty']").val($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val())
    $("#hierarchyform input[name='HierarchyType']").val("ClientSubUnitTravelerType")
    $("#ClientSubUnitGuid").val(ClientSubUnitGuid)
    $("#TravelerTypeGuid").val(TravelerTypeGuid)
    document.forms["hierarchyform"].submit();
}
function addRemoveHierarchy(HierarchyType, HierarchyCode) {
    $("#hierarchyform input[name='FilterHierarchySearchText']").val($("#hierarchysearchform input[name='FilterHierarchySearchText']").val())
    $("#hierarchyform input[name='FilterHierarchySearchProperty']").val($("#hierarchysearchform select[name='FilterHierarchySearchProperty']").val())
    $("#hierarchyform input[name='HierarchyType']").val(HierarchyType);
    $("#HierarchyCode").val(HierarchyCode)
    document.forms["hierarchyform"].submit();
}
