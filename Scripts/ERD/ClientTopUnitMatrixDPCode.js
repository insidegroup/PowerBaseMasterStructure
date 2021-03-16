
$(document).ready(function () {

    //Navigation
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    $('#HierarchyType').change(function() {

        var hierarchyType = $(this).val();

        if (hierarchyType == "") {
            $('#HierarchyItem').val('').attr('disabled', 'disabled');
        } else {
            $('#HierarchyItem').val('').attr('disabled', '');
        }
    });

    $("#HierarchyItem").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/ClientTopUnitMatrixDPCodes", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyType: $("#HierarchyType").val(), clientTopUnitGuid: $("#ClientTopUnit_ClientTopUnitGuid").val(), resultCount: 5000 },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: "<span class=\"tt-name\">" + item.HierarchyName + "</span>",
                            value: item.HierarchyName,
                            id: item.HierarchyCode,
                            text: item.HierarchyName
                        }
                    }))
                }
            });
        },
        select: function (event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#HierarchyItem").val(ui.item.value);
            $("#HierarchyCode").val(ui.item.id);
        }
    });

    //Submit Form Validation
    $('#form0').submit(function () {

        var validItem = false;

        if ($("#HierarchyType").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValid" + $("#HierarchyType").val(),
                data: { searchText: encodeURIComponent($("#HierarchyItem").val()) },
                success: function (data) {

                    if (!jQuery.isEmptyObject(data)) {
                        validItem = true;
                    }
                },
                dataType: "json",
                async: false
            });
            if (!validItem) {
                $("#lblHierarchyItemMsg").removeClass('field-validation-valid');
                $("#lblHierarchyItemMsg").addClass('field-validation-error');
                $("#lblHierarchyItemMsg").text("This is not a valid entry.");
            } else {
                $("#lblHierarchyItemMsg").text("");
            }
        }

        if (!$(this).valid()) {
            return false;
        }

        if (validItem) {
            return true;
        } else {
            return false
        };
    });
});