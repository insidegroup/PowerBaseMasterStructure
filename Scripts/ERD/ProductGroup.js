/*
OnReady
*/
$(document).ready(function () {

    //Navigation
    $('#menu_passivesegmentbuilder').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#ProductGroup_ExpiryDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    $('#ProductGroup_EnabledDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });
    if ($('#ProductGroup_EnabledDate').val() == "") {
        $('#ProductGroup_EnabledDate').val("No Enabled Date")
    }
    if ($('#ProductGroup_ExpiryDate').val() == "") {
        $('#ProductGroup_ExpiryDate').val("No Expiry Date")
    }
    //Hierarchy Disable/Enable OnLoad
    if ($("#ProductGroup_HierarchyType").val() == "") {
        $("#ProductGroup_HierarchyItem").val("");
        $("#ProductGroup_HierarchyItem").attr("disabled", true);
    } else {
        $("#ProductGroup_HierarchyItem").removeAttr("disabled");
    }


    //SubProducts
    $('#btnMoveToCurrent').click(function (e) {
        var selectedOpts = $('#AvailableSubProducts option:selected');
        if (selectedOpts.length == 0) {
            alert("Nothing to move.");
            e.preventDefault();
        }
        //selected items added to hidden fields
        $("#AvailableSubProducts option:selected").each(function () {
        	$("<input type='hidden' name='SubProducts[" + $('.subproductcounter').length + "].Value' value='" + escapeInput($(this).val()) + "' class='subproductcounter'>").appendTo('#hiddenSubProducts');
        });

        //selected items moved to other list
        $('#SubProducts').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#AvailableSubProducts').dblclick(function () {

        //selected items added to hidden fields
        $("#AvailableSubProducts option:selected").each(function () {
        	$("<input type='hidden' name='SubProducts[" + $('.subproductcounter').length + "].Value' value='" + escapeInput($(this).val()) + "' class='subproductcounter'>").appendTo('#hiddenSubProducts');
        });

        $('#AvailableSubProducts :selected').clone().appendTo("#SubProducts");
        $('#AvailableSubProducts :selected').remove();
    });


    $('#btnMoveToAvailable').click(function (e) {
        var selectedOpts = $('#SubProducts option:selected');
        if (selectedOpts.length == 0) {
            alert("Nothing to move.");
            e.preventDefault();
        }

        //selected items added to hidden fields
        $("#SubProducts option:selected").each(function () {
            $('#hiddenSubProducts').children("input[value='" + $(this).val() + "']").remove();

            var i = 0;
            $(".subproductcounter").each(function () {
                $(this).attr({ name: "SubProducts[" + (i++) + "]\.Value" });
            })
        });

        $('#AvailableSubProducts').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#SubProducts').dblclick(function () {

        //selected items added to hidden fields
        $("#SubProducts option:selected").each(function () {
            $('#hiddenSubProducts').children("input[value='" + $(this).val() + "']").remove();

            var i = 0;
            $(".subproductcounter").each(function () {
                $(this).attr({ name: "SubProducts[" + (i++) + "]\.Value" });
            })
        });
            
        $('#SubProducts :selected').clone().appendTo("#AvailableSubProducts");
        $('#SubProducts :selected').remove();
    });

    //show SubProducts if "Other" checked
    $("input[type='checkbox']:checked").each(function () {
        
        if ($(this).attr('text') == "13") {
            if ($(this).is(':checked')) {
                $('#subproducts').show();
            }
        }
    });


    //show/hide subproducts if "OTHER" products is checked/unchecked
    $(':checkbox').click(function () {
        if ($(this).attr('text') == "13") {
            if ($(this).is(':checked')) {
                $('#subproducts').show();
            } else {
                $('#subproducts').hide();
            }
        }
    });

    //Hierarchy Disable/Enable OnChange
    $("#ProductGroup_HierarchyType").change(function () {
        $("#lblHierarchyItemMsg").text("");
        $("#ProductGroup_HierarchyItem").val("");
        if ($("#ProductGroup_QueueMinderGroupId").val() == "0") {
            $("#lblAuto").text("");
            $("#ProductGroup_QueueMinderGroupName").val("");
        }
        if ($("#ProductGroup_HierarchyType").val() == "") {
            $("#ProductGroup_HierarchyItem").attr("disabled", true);
            $('#TravelerType').css('display', 'none');
        } else {
            $("#ProductGroup_HierarchyItem").removeAttr("disabled");
            $("#lblHierarchyItem").text($("#ProductGroup_HierarchyType").val());
            $("#ProductGroup_HierarchyCode").val("");
            $('#TravelerType').css('display', 'none');

        }
    });

    $.validator.setDefaults({
        submitHandler: function (form) {
        }
    });
    /*
    Submit Form Validation
    */
    $('#form0').submit(function () {

        var validItem = false;
        var validTravelerType = true;

        if ($("#ProductGroup_HierarchyItem").val() == "Multiple") {
            validItem = true;
        } else {
            if ($("#ProductGroup_HierarchyType").val() != "Multiple") {
                jQuery.ajax({

                    type: "POST",
                    url: "/Hierarchy.mvc/IsValid" + $("#ProductGroup_HierarchyType").val(),
                    data: { searchText: encodeURIComponent($("#ProductGroup_HierarchyItem").val()) },
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
                    if ($("#lblAuto").length) { $("#lblAuto").text("") };
                } else {
                    $("#lblHierarchyItemMsg").text("");
                }
            }
        }



        //wait for this name to be populated, dont show message
        if ($("#ProductGroup_ProductGroupId").val() == "0") {
            if ($("#lblAuto").text() == "") {
                return false;
            }
        } else {
            if (jQuery.trim($("#ProductGroup_ProductGroupName").val()) == "") {
                $("#ProductGroup_ProductGroupName_validationMessage").removeClass('field-validation-valid');
                $("#ProductGroup_ProductGroupName_validationMessage").addClass('field-validation-error');
                $("#ProductGroup_ProductGroupName_validationMessage").text("Product Group Name Required.");
                return false;
            } else {
                $("#ProductGroupName_validationMessage").text("");
            }
        }

        //GroupName Begin
        var validGroupName = false;

        jQuery.ajax({
            type: "POST",
            url: "/GroupNameBuilder.mvc/IsAvailableProductGroupName",
            data: { groupName: $("#ProductGroup_ProductGroupName").val(), id: $("#ProductGroup_ProductGroupId").val() },
            success: function (data) {

                validGroupName = data;
            },
            dataType: "json",
            async: false
        });

        if (!validGroupName) {

            $("#lblProductGroupNameMsg").removeClass('field-validation-valid');
            $("#lblProductGroupNameMsg").addClass('field-validation-error');
            if ($("#ProductGroup_ProductGroupId").val() == "0") {//Create
                $("#lblProductGroupNameMsg").text("This name has already been used, please reselect " + $("#lblHierarchyItem").text() + " to update the name.");
            } else {
                if ($("#ProductGroup_ProductGroupName").val() != "") {
                    $("#lblProductGroupNameMsg").text("This name has already been used, please choose a different name.");
                }
            } return false;
        } else {
            $("#lblProductGroupNameMsg").text("");
        }
        //GroupName End
        if (!$(this).valid()) {
            return false;
        }

        if (validItem && validTravelerType) {
            if ($('#ProductGroup_ExpiryDate').val() == "No Expiry Date") {
                $('#ProductGroup_ExpiryDate').val("");
            }
            if ($('#ProductGroup_EnabledDate').val() == "No Enabled Date") {
                $('#ProductGroup_EnabledDate').val("");
            }
            return true;
        } else {
            return false
        };
    });



});

$(function() {


    $("#ProductGroup_HierarchyItem").autocomplete({
        source: function(request, response) {

                $.ajax({
                    url: "/AutoComplete.mvc/AvailableHierarchies", type: "POST", dataType: "json",
                    data: { searchText: request.term, hierarchyItem: $("#ProductGroup_HierarchyType").val(), domainName: 'Passive Segment Builder' },
                    success: function (data) {
                        response($.map(data, function(item) {
                            if (
                                    $("#ProductGroup_HierarchyType").val() == "GlobalRegion" ||
                                    $("#ProductGroup_HierarchyType").val() == "GlobalSubRegion" ||
                                    $("#ProductGroup_HierarchyType").val() == "Country"
                                ) {
                                return {
                                    label: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + " (" + item.HierarchyCode + ")",
                                    value: item.HierarchyName,
                                    id: item.HierarchyCode,
                                    text: item.HierarchyName + (item.ParentName == "" ? "" : ", " + item.ParentName) + (item.GrandParentName == "" ? "" : ", " + item.GrandParentName)
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

        },
        select: function(event, ui) {
            $("#lblHierarchyItemMsg").text(ui.item.text);
            $("#ProductGroup_HierarchyItem").val(ui.item.value);
            $("#ProductGroup_HierarchyCode").val(ui.item.id);
            $("#ProductGroup_SourceSystemCode").val(ui.item.ssc);

             if ($("#ProductGroup_ProductGroupId").val() == "0") {//Create

                htft = ShortenHierarchyType($("#ProductGroup_HierarchyType").val());

                var maxNameSize = 50 - (htft.length + 14);
                var autoName = replaceSpecialChars(ui.item.value)
                autoName = autoName.substring(0, maxNameSize) + "_" + htft + "_ProductGroup";
                $("#lblAuto").text(autoName);
                $("#ProductGroup_ProductGroupName").val(autoName);
                $("#lblProductGroupNameMsg").text("");


                    /*$.ajax({
                        url: "/GroupNameBuilder.mvc/BuildGroupName", type: "POST", dataType: "json",
                        data: { hierarchyType: $("#ProductGroup_HierarchyType").val(), hierarchyItem: $("#ProductGroup_HierarchyCode").val(), group: "ProductGroup" },
                        success: function(data) {
                            var maxNameSize = 50 - (htft.length + 18);
                            var autoName = replaceSpecialChars(ui.item.value)
                            autoName = autoName.substring(0, maxNameSize) + "_" + data + "_" + htft + "_ProductGroup";
                            $("#lblAuto").text(autoName);
                            $("#ProductGroup_ProductGroupName").val(autoName);
                            $("#lblProductGroupNameMsg").text("");
                        }
                    })*/
                
            }
        }
    });   
});


function ShortenHierarchyType(hierarchyType) {
    switch (hierarchyType) {
        case "GlobalSubRegion":
            shortversion = "GSR";
            break;
        case "GlobalRegion":
            shortversion = "GR";
            break;
        case "CountryRegion":
            shortversion = "CR";
            break;
        default:
            shortversion = hierarchyType;
    }

    return shortversion
}

function escapeHTML(s) {

	s = s.replace(/&/g, '&amp;');
	s = s.replace(/</g, '&lt;');
	s = s.replace(/>/g, '&gt;');
	return s;
}