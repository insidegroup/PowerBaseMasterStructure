

$(document).ready(function() {
    $('#menu_teams').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

});

$(function() {


$("#ClientSubUnitName").autocomplete({
    source: function(request, response) {

        $.ajax({
        url: "/Team.mvc/AutoCompleteClientSubUnits", type: "POST", dataType: "json",
            data: { searchText: request.term},
            success: function(data) {
                response($.map(data, function(item) {
                    return {
                        label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
                        value: item.HierarchyName,
                        id: item.HierarchyCode,
                        text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                    }
                }))
            }
        })  
            
    },
        select: function(event, ui) {
            $("#lblClientSubUnitMsg").text(ui.item.text);
            $("#ClientSubUnitName").val(ui.item.value);
            $("#ClientSubUnitGuid").val(ui.item.id);
        }
    });
});
