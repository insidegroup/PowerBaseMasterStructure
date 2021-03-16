$(document).ready(function() {
    $('#menu_cdrgroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})



$(function() {
    $("#SetCDRDisplayName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/CDRItem.mvc/AutoCompleteCDRItems", type: "POST", dataType: "json",
                data: { searchText: request.term },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                            label: item.CDRDisplayName + "(" + item.CDRItemId + ")",
                            value: item.CDRDisplayName,
                            id: item.CDRItemId
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {

            $("#SetCDRItemId").val(ui.item.id);
            $("#lblSetCDRDisplayNameMsg").text(ui.item.id);
        }
    });
});


$('#form0').submit(function() {

    $("#lblSetCDRDisplayNameMsg").text("");
    $("#lblSourceCDRDisplayNameMsg").text("");
    $("#lblSetCDRItemValueMsg").text("");

    var validSetCDRItem = false;
    var validSourceCDRItem = false;
    var validCDRLinkedItem = false;

    if ($("#SetCDRDisplayName").val() == "") {
        return false;
    }
    jQuery.ajax({
        type: "POST",
        url: "/CDRItem.mvc/IsValidCDRItem",
        data: { cdrDisplayName: $("#SetCDRDisplayName").val(), cdrItemId: $("#SetCDRItemId").val() },
        success: function(data) {

            if (!jQuery.isEmptyObject(data)) {
                validSetCDRItem = true;
            }
        },
        dataType: "json",
        async: false
    });

    if (!validSetCDRItem) {
        $("#SourceCDRItemId").val("");
        $("#lblSetCDRDisplayNameMsg").removeClass('field-validation-valid');
        $("#lblSetCDRDisplayNameMsg").addClass('field-validation-error');
        $("#lblSetCDRDisplayNameMsg").text("This is not a valid CDR Item.");
        return false;
    } else {
        $("#lblSourceCDRDisplayNameMsg").text("");
    }

    jQuery.ajax({
        type: "POST",
        url: "/CDRItem.mvc/IsValidCDRItem",
        data: { cdrDisplayName: $("#SourceCDRDisplayName").val(), cdrItemId: $("#OriginalSourceCDRItemId").val() },
        success: function(data) {

            if (!jQuery.isEmptyObject(data)) {
                validSourceCDRItem = true;
            }
        },
        dataType: "json",
        async: false
    });

    if (!validSourceCDRItem) {
        $("#SetCDRItemId").val("");
        $("#lblSourceCDRDisplayNameMsg").removeClass('field-validation-valid');
        $("#lblSourceCDRDisplayNameMsg").addClass('field-validation-error');
        $("#lblSourceCDRDisplayNameMsg").text("This is not a valid CDR Item.");
        return false;
    } else {
        $("#lblSetCDRDisplayNameMsg").text("");
    }


    if (    //if unchanged
        ($("#SourceCDRItemId").val() == $("#OriginalSourceCDRItemId").val()) &&
        ($("#SetCDRItemId").val() == $("#OriginalSetCDRItemId").val()) &&
        ($("#SourceCDRItemValue").val() == $("#OriginalSourceCDRItemValue").val())

        ) {
        validCDRLinkedItem = true;
    } else {
        jQuery.ajax({
            type: "POST",
            url: "/CDRLinkedItem.mvc/IsValidCDRLinkedItem",
            data: {
                sourceCDRItemId: $("#OriginalSourceCDRItemId").val(),
                setCDRItemId: $("#SetCDRItemId").val(),
                sourceCDRItemValue: $("#SourceCDRItemValue").val()
            },
            success: function(data) {

                if (jQuery.isEmptyObject(data)) {
                    validCDRLinkedItem = true;
                }
            },
            dataType: "json",
            async: false
        });
    }


    if (!validCDRLinkedItem) {
        $("#lblSetCDRItemValueMsg").removeClass('field-validation-valid');
        $("#lblSetCDRItemValueMsg").addClass('field-validation-error');
        $("#lblSetCDRItemValueMsg").text("This value is in use.");
        return false;
    } else {
        $("#lblSetCDRItemValueMsg").text("");
    }

});