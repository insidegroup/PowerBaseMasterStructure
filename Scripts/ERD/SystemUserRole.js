

$(document).ready(function() {
    //Navigation
$('#menu_teams').click();
$("tr:odd").addClass("row_odd");
$("tr:even").addClass("row_even");
});

//Submit Form Validation
$('#form0').submit(function() {
    if (!$(this).valid()) {
        return;
    }
    var selectedRole = $("#AdministratorRoleHierarchyLevelTypeName").val();
    if (selectedRole != "") {
        var arrSelectedRole = selectedRole.split("_");
        $("#AdministratorRoleId").val(arrSelectedRole[0]);
        $("#HierarchyLevelTypeId").val(arrSelectedRole[1]);
        return true;
    } else {
        return false;
    }
});