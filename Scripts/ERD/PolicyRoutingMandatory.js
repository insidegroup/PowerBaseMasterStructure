
$(document).ready(function () {

    //Navigation
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
});

if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
    $("#PolicyRouting_FromCode").attr('disabled', 'disabled');
}
if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
    $("#PolicyRouting_ToCode").attr('disabled', 'disabled');
}

$("#PolicyRouting_FromGlobalFlag").click(function() {
    if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
        $("#PolicyRouting_FromCode").val("");
        $("#PolicyRouting_FromCode").attr('disabled', 'disabled');
        $("#PolicyRouting_FromCode_validationMessage").text("");
        $("#lblFrom").text("");

        var to = $("#PolicyRouting_ToCode").val();
        if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
            to = "Global";
        }
        BuildRoutingName("Global", to) ;
    } else {
        $("#PolicyRouting_FromCode").removeAttr('disabled');
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");
    }
});
$("#PolicyRouting_ToGlobalFlag").click(function() {
    if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
        $("#PolicyRouting_ToCode").val("");
        $("#PolicyRouting_ToCode").attr('disabled', 'disabled');
        $("#PolicyRouting_ToCode_validationMessage").text("");
        $("#lblTo").text("");

        var from = $("#PolicyRouting_FromCode").val();
        if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
            from = "Global";
        }
        BuildRoutingName(from, "Global") ;
    } else {
        $("#PolicyRouting_ToCode").removeAttr('disabled');
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");
    }

   
    
});
$(function() {
    $("#PolicyRouting_ToCode").autocomplete({

        source: function(request, response) {
            $.ajax({
                url: "/PolicyRouting.mvc/AutoCompletePolicyRoutingFromTo", type: "POST", dataType: "json",
                data: { searchText: request.term, maxResults: 10 },
                success: function(data) {

                    response($.map(data, function(item) {
                        return {
                            label: item.Name + "," + item.Parent,
                            value: item.Code,
                            id: item.CodeType
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#lblTo").text(ui.item.label + ' (' + ui.item.value + ')');
            $("#PolicyRouting_ToCodeType").val(ui.item.id);
            $("#PolicyRouting_ToGlobalFlag").attr('checked', false);


            var from = $("#PolicyRouting_FromCode").val();
            if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                from = "Global";
            }
            BuildRoutingName(from, ui.item.value);
        }
    });

});

$(function() {
    $("#PolicyRouting_FromCode").autocomplete({

        source: function(request, response) {
            $.ajax({
                url: "/PolicyRouting.mvc/AutoCompletePolicyRoutingFromTo", type: "POST", dataType: "json",
                data: { searchText: request.term, maxResults: 10 },
                success: function(data) {

                    response($.map(data, function(item) {
                        return {
                            label: item.Name + "," + item.Parent,
                            value: item.Code,
                            id: item.CodeType
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#lblFrom").text(ui.item.label + ' (' + ui.item.value + ')');
            $("#PolicyRouting_FromCodeType").val(ui.item.id);
            $("#PolicyRouting_FromGlobalFlag").attr('checked', false);

            var to = $("#PolicyRouting_ToCode").val();
            if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                to = "Global";
            }
            BuildRoutingName(ui.item.value, to);
        }
    });

});


$('#form0').submit(function() {
    if ($("#PolicyRouting_Name").val() == "") {
        return true;
    } else {

        var validFrom = false;
        var validTo = false;
        $("#lblFrom").text("");
        $("#lblTo").text("");
        if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
            validFrom = true;
        }
        if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
            validTo = true;
        }
        if (!$("#PolicyRouting_FromGlobalFlag").is(':checked') && $("#PolicyRouting_FromCode").val() == "") {
            $("#lblFrom").removeClass('field-validation-valid');
            $("#lblFrom").addClass('field-validation-error');
            $("#lblFrom").text("Please enter a value or choose Global");
        }
        if ($("#PolicyRouting_FromCode").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/PolicyRouting.mvc/IsValidPolicyRoutingFromTo",
                data: { fromto: $("#PolicyRouting_FromCode").val() },
                success: function(data) {
                    validFrom = data;
                },
                dataType: "json",
                async: false
            });

            if (!validFrom) {
                $("#lblFrom").removeClass('field-validation-valid');
                $("#lblFrom").addClass('field-validation-error');
                $("#lblFrom").text("This is not a valid entry");
            }
        };

        if (!$("#PolicyRouting_ToGlobalFlag").is(':checked') && $("#PolicyRouting_ToCode").val() == "") {
            $("#lblTo").removeClass('field-validation-valid');
            $("#lblTo").addClass('field-validation-error');
            $("#lblTo").text("Please enter a value or choose Global");
        }
        if ($("#PolicyRouting_ToCode").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/PolicyRouting.mvc/IsValidPolicyRoutingFromTo",
                data: { fromto: $("#PolicyRouting_ToCode").val() },
                success: function(data) {
                    validTo = data;
                },
                dataType: "json",
                async: false
            });

            if (!validTo) {
                $("#lblTo").removeClass('field-validation-valid');
                $("#lblTo").addClass('field-validation-error');
                $("#lblTo").text("This is not a valid entry");
            }
        };
        if (validFrom && validTo) {
            return true;
        } else {
            return false
        };
    };

});

function BuildRoutingName(from, to) {
    if ($("#SupplierCode").val() == "" || from == "" || to == "") {
        $("#lblAuto").text("");
        $("#PolicyRouting_Name").val("");
        $("#lblPolicyRoutingNameMsg").text("");
        return;
    }
    var autoName = from + "_to_" + to + "_on_" + $("#SupplierCode").val();
    autoName = autoName.substring(0, 95);
    
        $.ajax({
            url: "/PolicyRouting.mvc/BuildRoutingName", type: "POST", dataType: "json",
            data: { routingName: autoName },
            success: function(data) {
                $("#lblAuto").text(data);
                $("#PolicyRouting_Name").val(data);
                $("#lblPolicyRoutingNameMsg").text("");
            }
        })
}