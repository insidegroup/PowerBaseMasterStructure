<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.PolicySupplierDealCode>" %>
<% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr>
                    <td><label for="PolicySupplierDealCodeValue">DealCode Value</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicySupplierDealCodeValue)%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicySupplierDealCodeValue)%></td>
                </tr> 
                 <tr>
                    <td><label for="PolicySupplierDealCodeDescription">DealCode Description</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicySupplierDealCodeDescription)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicySupplierDealCodeDescription)%></td>
                </tr> 
                <tr>
                    <td><label for="GDSCode">GDS</label></td>
                    <td><%= Html.DropDownList("GDSCode", ViewData["GDSList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSCode)%></td>
                </tr> 
               <tr>
                    <td><label for="PolicySupplierDealCodeTypeId">DealCode Type</label></td>
                    <td><%= Html.DropDownList("PolicySupplierDealCodeTypeId", ViewData["PolicySupplierDealCodeTypeList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicySupplierDealCodeTypeId)%></td>
                </tr> 
                <tr>
                <td> <label for="EnabledFlag">Enabled?</label></td>
                <td><%= Html.CheckBoxFor(model => model.EnabledFlagNonNullable)%></td>
                <td><%= Html.ValidationMessageFor(model => model.EnabledFlagNonNullable)%></td>
            </tr>  
            <tr>
                <td><label for="EnabledDate">Enabled Date</label></td>
                <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
                <td><%= Html.ValidationMessageFor(model => model.EnabledDate) %></td>
            </tr> 
            <tr>
                <td><label for="ExpiryDate">Expiry Date</label> </td>
                <td> <%= Html.EditorFor(model => model.ExpiryDate)%></td>
                <td><%= Html.ValidationMessageFor(model => model.ExpiryDate)%></td>
            </tr> 
            <tr>
                <td><label for="ProductId">Product</label></td>
                <td><%= Html.DropDownList("ProductId", ViewData["ProductList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
            </tr> 
            <tr>
                <td><label for="SupplierName">Supplier</label></td>
                <td> <%= Html.TextBoxFor(model => model.SupplierName)%><span class="error"> *</span></td>
                <td>
                    <%= Html.ValidationMessageFor(model => model.SupplierName)%>
                    <%= Html.HiddenFor(model => model.SupplierCode)%>
                    <label id="lblSupplierNameMsg"/>
                </td>
            </tr>
             <tr>
                <td><label for="PolicyLocationId">Policy Location</label></td>
                <td><%= Html.DropDownList("PolicyLocationId", ViewData["PolicyLocationList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyLocationId)%></td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            </table>
            <%=Html.HiddenFor(model => model.PolicySupplierDealCodeId)%>
            <%=Html.HiddenFor(model => model.PolicyGroupId)%>
             <%=Html.HiddenFor(model => model.VersionNumber)%>

    <% } %>
<script type="text/javascript">
 $(document).ready(function () {
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    /////////////////////////////////////////////////////////
    // BEGIN DATES SETUP
    /////////////////////////////////////////////////////////
    new JsDatePick({
			useMode:2,
			isStripped:false,
		    target:"ExpiryDate",
			dateFormat:"%M %d %Y",
			cellColorScheme:"beige"
		});
		
	new JsDatePick({
			useMode:2,
			isStripped:false,
		    target:"EnabledDate",
			dateFormat:"%M %d %Y",
			cellColorScheme:"beige"
		});

    if ($('#EnabledDate').val() == "") {
        $('#EnabledDate').val("No Enabled Date");
    }
    if ($('#ExpiryDate').val() == "") {
        $('#ExpiryDate').val("No Expiry Date");
    }

    /////////////////////////////////////////////////////////
    // END DATES SETUP
    /////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////
    if ($("#ProductId").val() == "") {
        $("#SupplierName").attr("disabled", true);
    } else {
        $("#SupplierName").removeAttr("disabled");
    }

    $("#ProductId").change(function () {
        $("#SupplierName").val("");
        $("#SupplierCode").val("");
        if ($("#ProductId").val() == "") {
            $("#SupplierName").attr("disabled", true);
        } else {
            $("#SupplierName").removeAttr("disabled");
        }
    });

    $("#SupplierName").change(function () {
        if ($("#SupplierName").val() == "") {
            $("#SupplierCode").val("");
        }
    });

    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER AUTOCOMPLETE
    /////////////////////////////////////////////////////////
    $(function () {
        $("#SupplierName").autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#ProductId").val() },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                id: item.SupplierCode,
                                value: item.SupplierName
                            }
                        }))
                    }
                })
            },
            mustMatch: true,
            select: function (event, ui) {
                $("#SupplierName").val(ui.item.value);
                $("#SupplierCode").val(ui.item.id);
                $("#lblSupplierNameMsg").text("");
            }

        });
    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER AUTOCOMPLETE
    /////////////////////////////////////////////////////////
});


/*
Validate PolicySupplierDealCode and write to hidden tables or return validation errors
*/
function PolicySupplierDealCodeValidation() {

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER VALIDATION
    // 1. Check Text for Supplier to see if valid
    // 2. If yes, update the SupplierCode field and check the SupplierCode/ProductId combo
    /////////////////////////////////////////////////////////
    var validSupplier = false;
    var validSupplierProduct = false;

    if ($("#SupplierName").val() != "") {
        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierName",
            data: { supplierName: $("#SupplierName").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validSupplier = true;

                    //user may not use auto, need to populate ID field
                    if ($("#SupplierCode").val() == "") {
                    	$("#SupplierCode").val(data[0].SupplierCode);
                    }
                }
            },
            dataType: "json",
            async: false
        });

        if (!validSupplier) {
            $("#SupplierCode").val("");
            $("#lblSupplierNameMsg").removeClass('field-validation-valid');
            $("#lblSupplierNameMsg").addClass('field-validation-error');
            $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            return false;
        } else {
            $("#lblSupplierNameMsg").text("");
        }

        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidSupplierProduct",
            data: { supplierCode: $("#SupplierCode").val(), productId: $("#ProductId").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validSupplierProduct = true;
                }
            },
            dataType: "json",
            async: false
        });
        if (!validSupplierProduct) {
            $("#lblSupplierNameMsg").removeClass('field-validation-valid');
            $("#lblSupplierNameMsg").addClass('field-validation-error');
            $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            alert("!")
            return false;
        } else {
            $("#lblSupplierNameMsg").text("");
        }
    }

    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER VALIDATION
    /////////////////////////////////////////////////////////
    //reset date fields
    if ($('#EnabledDate').val() == "No Enabled Date") {
        $('#EnabledDate').val("");
    }
    if ($('#ExpiryDate').val() == "No Expiry Date") {
        $('#ExpiryDate').val("");
    }

    var url = '/ClientWizard.mvc/PolicySupplierDealCodeValidation';

    var dialogueWindow = $("#dialog-confirm");

	//Build Object to Store PolicySupplierDealCode
    var policySupplierDealCode = {
        PolicySupplierDealCodeId: $("#PolicySupplierDealCodeId").val(),
        PolicyGroupId: $("#PolicyGroupId").val(),
        PolicySupplierDealCodeValue: $("#PolicySupplierDealCodeValue").val(),
        PolicySupplierDealCodeDescription: $("#PolicySupplierDealCodeDescription").val(),
        PolicySupplierDealCodeTypeId: $("#PolicySupplierDealCodeTypeId").val(),
        GDSCode: $("#GDSCode").val(),
        PolicyLocationId: $("#PolicyLocationId").val(),
        ProductId: $("#ProductId").val(),
        SupplierCode: $("#SupplierCode").val(),
        SupplierName: $("#SupplierName").val(),
        EnabledFlag: $("#EnabledFlagNonNullable").is(':checked'),
        EnabledFlagNonNullable: $("#EnabledFlagNonNullable").is(':checked'),
        EnabledDate: $("#EnabledDate").val(),
        ExpiryDate: $("#ExpiryDate").val(),
        VersionNumber: $("#VersionNumber", dialogueWindow).val()
    };


    //AJAX (JSON) POST of PolicySupplierDealCode Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(policySupplierDealCode),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.success) {
                if ($("#PolicySupplierDealCodeId").val() == "0") {//new

                    //add to hidden table
                    $('#hiddenAddedPolicySupplierDealCodesTable').append("<tr PolicySupplierDealCodeId='" + $('#PolicySupplierDealCodeId').val() +
                        "' PolicyGroupId='" + $('#PolicyGroupId').val() +
                                "' PolicySupplierDealCodeValue='" + escape($('#PolicySupplierDealCodeValue').val()) +
                                "' PolicySupplierDealCodeDescription='" + $('#PolicySupplierDealCodeDescription').val() +
                                "' PolicySupplierDealCodeTypeId='" + $('#PolicySupplierDealCodeTypeId').val() +
                                "' GDSCode='" + $('#GDSCode').val() +
                                "' ProductId='" + $('#ProductId').val() +
                                "' SupplierCode='" + $('#SupplierCode').val() +
                                "' PolicyLocationId='" + $('#PolicyLocationId').val() +
                                "' EnabledFlag='" + $('#EnabledFlagNonNullable').is(':checked') +
                                "' EnabledDate='" + $('#EnabledDate').val() +
                                "' ExpiryDate='" + $('#ExpiryDate').val() +
                        "' VersionNumber='" + $('#VersionNumber', dialogueWindow).val() + "'></tr>");

                    //add to visual table
                    $('#currentPolicySupplierDealCodesTable tbody').append("<tr><td>" +
                        $('#ProductId :selected').text() + "</td><td>" +
                                $('#SupplierName').val() + "</td><td>Custom</td><td>" +
                        $('#GDSCode :selected').text() + "</td><td>" +
                        $('#PolicySupplierDealCodeTypeId :selected').text() + "</td><td>" +
                        $('#PolicyLocationId :selected').text() + "</td><td>" +
                        $('#PolicySupplierDealCodeValue').val() + "</td><td>Added</td></tr>");
                } else {
                    //modification
                    //find it in display table first and change to show modified

                    $('#currentPolicySupplierDealCodesTable tbody tr').each(function () {

                        if ($(this).attr("PolicySupplierDealCodeId") == $('#PolicySupplierDealCodeId').val()) {

                            //edit visual table
                            $(this).html("<td>" +
                               $('#ProductId :selected').text() + "</td><td>" +
                                $('#SupplierName').val() + "</td><td>Custom</td><td>" +
                                $('#GDSCode :selected').text() + "</td><td>" +
                                $('#PolicySupplierDealCodeTypeId :selected').text() + "</td><td>" +
                                 $('#PolicyLocationId :selected').text() + "</td><td>" +
                                $('#PolicySupplierDealCodeValue').val() + "</td><td>Modified</td>");

                            $(this).contents('td').css({ 'background-color': '#CCCCCC' });

                            //add to hidden table
                            $('#hiddenChangedPolicySupplierDealCodesTable').append("<tr PolicySupplierDealCodeId='" + $('#PolicySupplierDealCodeId').val() +
                                "' PolicyGroupId='" + $('#PolicyGroupId').val() +
                                "' PolicySupplierDealCodeValue='" + escape($('#PolicySupplierDealCodeValue').val()) +
                                "' PolicySupplierDealCodeDescription='" + $('#PolicySupplierDealCodeDescription').val() +
                                "' PolicySupplierDealCodeTypeId='" + $('#PolicySupplierDealCodeTypeId').val() +
                                "' GDSCode='" + $('#GDSCode').val() +
                                "' ProductId='" + $('#ProductId').val() +
                                "' SupplierCode='" + $('#SupplierCode').val() +
                                "' PolicyLocationId='" + $('#PolicyLocationId').val() +
                                "' EnabledFlag='" + $('#EnabledFlagNonNullable').is(':checked') +
                                "' EnabledDate='" + $('#EnabledDate').val() +
                                "' ExpiryDate='" + $('#ExpiryDate').val() +
                                "' VersionNumber='" + $('#VersionNumber', dialogueWindow).val() + "'></tr>");
                        }
                    });
                }
                $("#dialog-confirm").dialog("close");
            } else {
                $("#dialog-confirm").html(result.html);
            }
        },
        error: function () {
            $("#dialog-confirm").html("There was an error.");
        }
    });
}


</script>
