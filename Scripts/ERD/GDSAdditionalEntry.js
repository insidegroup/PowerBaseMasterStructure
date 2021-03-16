

$(document).ready(function() {
    //Navigation
    $('#menu_gdsadditionalentries').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });

    if ($('#EnabledDate').val() == "") {
        $('#EnabledDate').val("No Enabled Date");
    }
    if ($('#ExpiryDate').val() == "") {
        $('#ExpiryDate').val("No Expiry Date");
    }
    //Hierarchy Disable/Enable OnLoad
    if ($("#HierarchyType").val() == "") {
        $("#HierarchyItem").val("");
        $("#HierarchyItem").attr("disabled", true);
    } else {
        $("#HierarchyItem").removeAttr("disabled");

        if ($("#HierarchyType").val() == "ClientSubUnitTravelerType") {
            $("#lblHierarchyItem").text("ClientSubUnit");
            $('#TravelerType').css('display', '');
        }
    }

    //Hierarchy Disable/Enable OnChange
    $("#HierarchyType").change(function() {

        $("#lblHierarchyItemMsg").text("");
        $("#HierarchyItem").val("");

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

    $('#SubProductName').change(function () {
        $("#SubProductId").val($("#SubProductName").val());
    });
    if($("#SubProductId").val() != 0){
        //item selected
        $.ajax({
            url: "/SubProduct.mvc/ProductSubProducts", type: "POST", dataType: "json",
            data: { productId: $("#ProductId").val() },
            success: function (data) {
                if (data.length != 0) { //show text box

                    $("#SubProductName").find('option').remove();
                    $("<option value=''>Please Select...</option>").appendTo($("#SubProductName"));
                    $(data).each(function () {
                        if ($("#SubProductId").val() == this.SubProductId) {
                        $("<option value=" + this.SubProductId + " selected>" + this.SubProductName + "</option>").appendTo($("#SubProductName"));
                    }else{
                        $("<option value=" + this.SubProductId + ">" + this.SubProductName + "</option>").appendTo($("#SubProductName"));
                    }
                    });
                }

            }

        })
    }
    //Submit Form Validation
    $('#form0').submit(function() {
        var validItem = false;
        var validTravelerType = true;

        if ($("#HierarchyType").val() != "") {
            jQuery.ajax({
                type: "POST",
                url: "/Hierarchy.mvc/IsValid" + $("#HierarchyType").val(),
                data: { searchText: encodeURIComponent($("#HierarchyItem").val()) },
                success: function(data) {

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
                    url: "/Hierarchy/IsValidTravelerType",
                    data: { searchText: $("#TravelerTypeName").val() },
                    success: function(data) {

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

        if (!$(this).valid()) {
            return false;
        }

        if (validItem && validTravelerType) {
            if ($('#EnabledDate').val() == "No Enabled Date") {
                $('#EnabledDate').val("");
            }
            if ($('#ExpiryDate').val() == "No Expiry Date") {
                $('#ExpiryDate').val("");
            }
            return true;
        } else {
            return false
        };
    });



});

/*
When a Product is selected, load SubProducts
*/
function LoadSubProducts() {

    //no item selected
    if ($("#ProductId").val() == "") {
        $("#SubProductName").find('option').remove();
        return;
    }

    //item selected
    $.ajax({
        url: "/SubProduct.mvc/ProductSubProducts", type: "POST", dataType: "json",
        data: { productId: $("#ProductId").val() },
        success: function (data) {
            if (data.length != 0) { //show text box

                $("#SubProductName").find('option').remove();
                $("<option value=''>Please Select...</option>").appendTo($("#SubProductName"));
                $(data).each(function () {
                    $("<option value=" + this.SubProductId + ">" + this.SubProductName + "</option>").appendTo($("#SubProductName"));
                });
            }

        }

    })

}

$(function() {


    $("#HierarchyItem").autocomplete({
        source: function(request, response) {

        if ($("#HierarchyType").val() != "ClientSubUnitTravelerType") {
                $.ajax({
                    url: "/AutoComplete.mvc/Hierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: $("#HierarchyType").val(), domainName: 'GDS Additional Entry' },
                    success: function (data) {
                    response($.map(data, function(item) {
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
            url: "/GDSAdditionalEntry.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
            data: { searchText: request.term, hierarchyItem: "ClientSubUnit", filterText: $("#TravelerTypeGuid").val() },
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
            
        }
            
        },
        select: function(event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#HierarchyItem").val(ui.item.value);
            $("#HierarchyCode").val(ui.item.id);
            $("#SourceSystemCode").val(ui.item.ssc);
        }
    });


    // AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)

    $("#TravelerTypeName").autocomplete({
        source: function(request, response) {
            $.ajax({
            url: "/GDSAdditionalEntry.mvc/AutoCompleteClientSubUnitTravelerTypes", type: "POST", dataType: "json",
                data: { searchText: request.term, hierarchyItem: "TravelerType", filterText: $("#HierarchyCode").val() },
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
            $("#lblTravelerTypeMsg").text(ui.item.text);
            $("#TravelerTypeName").val(ui.item.value);
            $("#TravelerTypeGuid").val(ui.item.id);
        }
    });
});
