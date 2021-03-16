/*
FOR CWT Credit Cards
*/



$(document).ready(function() {

    //Navigation
    $('#menu_creditcards').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#CreditCardValidFrom').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M yy',
         onClose: function(dateText, inst) { 
            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, month, 1));
        },
        duration: 0
    });
    //Show DatePickers
    $('#CreditCardValidTo').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M yy',
        onClose: function(dateText, inst) {
            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, month, 1));
        },
        duration: 0
    });

    if ($('#CreditCardValidFrom').val() == "") {
        $('#CreditCardValidFrom').val("No Valid From Date")
    }

    //Hierarchy Disable/Enable OnLoad
    if ($("#HierarchyType").val() == "") {
        $("#HierarchyItem").val("");
        $("#HierarchyItem").attr("disabled", true);
    } else {
        $("#HierarchyItem").removeAttr("disabled");
    }


});

//Hierarchy Disable/Enable OnChange
$("#HierarchyType").change(function () {
    $("#lblHierarchyItemMsg").text("");
    $("#HierarchyItem").val("");
    if ($("#PolicyGroupId").val() == "0") {
        $("#lblAuto").text("");
        $("#PolicyGroupName").val("");
    }
    if ($("#HierarchyType").val() == "") {
        $("#HierarchyItem").attr("disabled", true);
        $('#TravelerType').css('display', 'none');
    } else {
        $("#HierarchyItem").removeAttr("disabled");
        $("#lblHierarchyItem").text($("#HierarchyType").val());
        $("#HierarchyCode").val("");
        $('#TravelerType').css('display', 'none');
        if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
            $("#lblHierarchyItem").text("ClientSubUnit");
            $("#TravelerTypeName").val("");
            $("#TravelerTypeGuid").val("");
            $('#TravelerType').css('display', '');
        }
    }
});


$(function () {


    $("#HierarchyItem").autocomplete({
        source: function (request, response) {

            if ($("#HierarchyType").val() != "ClientSubUnitTravelerType") {
                $.ajax({
                    url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'CWT Credit Card' },
                    success: function (data) {
                        response($.map(data, function (item) {
                            if (
                                    $("#HierarchyType").val() == "GlobalRegion" ||
                                    $("#HierarchyType").val() == "GlobalSubRegion" ||
                                    $("#HierarchyType").val() == "Country"
                                ) {
                                return {
                                    label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + " (" + item.HierarchyCode + ")",
                                    value: item.HierarchyName,
                                    id: item.HierarchyCode,
                                    text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                                }
                            } else if ($("#HierarchyType").val() == "ClientAccount") {
                                return {
                                    label: "<span class=\"ca-name\">" + item.HierarchyName + "</span><span class=\"ca-number\">" + item.ClientAccountNumber + "</span><span class=\"ca-ssc\">" + item.SourceSystemCode + "</span>",
                                    value: item.HierarchyName,
                                    id: item.ClientAccountNumber,
                                    ssc: item.SourceSystemCode,
                                    text: ""
                                }
                            } else {
                                return {
                                    label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
                                    value: item.HierarchyName,
                                    id: item.HierarchyCode,
                                    text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                                }
                            }

                        }))
                    }
                })
            } else {
                $.ajax({
                    url: "/PolicyGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#TravelerTypeGuid").val() },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName),
                                value: item.HierarchyName,
                                id: item.HierarchyCode,
                                text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
                            }
                        }))
                    }
                })

            }

        },
        select: function (event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#HierarchyItem").val(ui.item.value);
            $("#HierarchyCode").val(ui.item.id);
            $("#SourceSystemCode").val(ui.item.ssc);

           
        }
    });


    $("#TravelerTypeName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/PolicyGroup.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#HierarchyCode").val() },
                success: function (data) {
                    response($.map(data, function (item) {
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
        select: function (event, ui) {
            $("#lblTravelerTypeMsg").text(ui.item.text);
            $("#TravelerTypeName").val(ui.item.value);
            $("#TravelerTypeGuid").val(ui.item.id);
        }
    });


    $("#ClientTopUnitName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete.mvc/ClientTopUnitName", type: "POST", dataType: "json",
                data: { searchText: request.term},
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            value: item.ClientTopUnitName,
                            id: item.ClientTopUnitGuid,
                            text: item.ClientTopUnitName
                        }
                    }))
                }
            })
        },
        select: function (event, ui) {
            $("#ClientTopUnitName").val(ui.item.value);
            $("#ClientTopUnitGuid").val(ui.item.id);
        }
    });
});



//Submit Form Validation
$('#form0').submit(function () {


    var validItem = false;
    var validTravelerType = true;

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

    if ($("#TravelerType").is(":visible")) {

        if ($("#HierarchyType").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValidTravelerType",
                data: { searchText: $("#TravelerTypeName").val() },
                success: function (data) {

                    if (jQuery.isEmptyObject(data)) {
                        validTravelerType = false;
                    }
                },
                dataType: "json",
                async: false
            });
            if (!validTravelerType) {
                $("#lblTravelerTypeMsg").removeClass('field-validation-valid');
                $("#lblTravelerTypeMsg").addClass('field-validation-error');
                $("#lblTravelerTypeMsg").text("This is not a valid entry.");
            } else {
                $("#lblTravelerTypeMsg").text("");
            }
        }
    }


    if (validItem && validTravelerType) {
        if ($('#CreditCardValidFrom').val() == "No Valid From Date") {
            $('#CreditCardValidFrom').val("");
        }
        return true;
    } else {
        return false
    };
});


