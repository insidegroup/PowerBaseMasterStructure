<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.PolicySupplierServiceInformation>" %>
<% Html.EnableClientValidation(); %>
<% using (Html.BeginForm()) {%>
    <%= Html.AntiForgeryToken() %>
    <%= Html.ValidationSummary(true) %>
    <table cellpadding="0" cellspacing="0" width="100%"> 
        <tr>
            <td><label for="PolicySupplierServiceInformationValue">Value</label></td>
            <td><%= Html.TextBoxFor(model => model.PolicySupplierServiceInformationValue)%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.PolicySupplierServiceInformationValue)%></td>
        </tr> 
        <tr>
            <td><label for="PolicySupplierServiceInformationTypeId">Type</label></td>
            <td><%= Html.DropDownList("PolicySupplierServiceInformationTypeId", ViewData["PolicySupplierServiceInformationList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.PolicySupplierServiceInformationTypeId)%></td>
        </tr> 
        <tr>
            <td><label for="EnabledFlag">Enabled?</label></td>
            <td><%= Html.CheckBoxFor(model => model.EnabledFlagNonNullable)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledFlagNonNullable)%></td>
        </tr>  
        <tr>
            <td><label for="EnabledDate">Enabled Date</label></td>
            <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledDate)%></td>
        </tr> 
        <tr>
            <td><label for="ExpiryDate">Expiry Date</label></td>
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
            <td width="30%" class="row_footer_left"></td>
            <td width="40%" class="row_footer_centre"></td>
            <td width="30%" class="row_footer_right"></td>
        </tr>
    </table>
    <%=Html.HiddenFor(model => model.PolicySupplierServiceInformationId)%>
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
Validate PolicySupplierServiceInformation and write to hidden tables or return validation errors
*/
function PolicySupplierServiceInformationValidation() {

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
                    $("#SupplierCode").val(data[0].SupplierCode);
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

    var url = '/ClientWizard.mvc/PolicySupplierServiceInformationValidation';

    var dialogueWindow = $("#dialog-confirm");

    //Build Object to Store PolicySupplierServiceInformation
    var policySupplierServiceInformation = {
        PolicySupplierServiceInformationId: $("#PolicySupplierServiceInformationId").val(),
        PolicyGroupId: $("#PolicyGroupId").val(),
        PolicySupplierServiceInformationValue: $("#PolicySupplierServiceInformationValue").val(),
        PolicySupplierServiceInformationTypeId: $("#PolicySupplierServiceInformationTypeId").val(),
        ProductId: $("#ProductId").val(),
        SupplierCode: $("#SupplierCode").val(),
        SupplierName: $("#SupplierName").val(),
        EnabledFlag: $("#EnabledFlagNonNullable").is(':checked'),
        EnabledFlagNonNullable: $("#EnabledFlagNonNullable").is(':checked'),
        EnabledDate: $("#EnabledDate").val(),
        ExpiryDate: $("#ExpiryDate").val(),
        VersionNumber: $("#VersionNumber", dialogueWindow).val()
    };


    //AJAX (JSON) POST of PolicySupplierServiceInformation Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(policySupplierServiceInformation),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.success) {
                if ($("#PolicySupplierServiceInformationId").val() == "0") {//new

                    //add to hidden table
                    $('#hiddenAddedPolicySupplierServiceInformationsTable').append("<tr PolicySupplierServiceInformationId='" +  $('#PolicySupplierServiceInformationId').val() +
                        "' PolicyGroupId='" + $('#PolicyGroupId').val() +
                        "' PolicySupplierServiceInformationValue='" + escape($('#PolicySupplierServiceInformationValue').val()) +
                        "' PolicySupplierServiceInformationTypeId='" + $('#PolicySupplierServiceInformationTypeId').val() + 
                        "' ProductId='" +  $('#ProductId').val() + 
                        "' SupplierCode='" + $('#SupplierCode').val() + 
                        "' EnabledFlag='" + $('#EnabledFlagNonNullable').is(':checked') + 
                        "' EnabledDate='" + $('#EnabledDate').val() + 
                        "' ExpiryDate='" + $('#ExpiryDate').val() + 
                        "' VersionNumber='" + $('#VersionNumber', dialogueWindow).val() + "'></tr>");

                    //add to visual table
                    $('#currentPolicySupplierServiceInformationsTable tbody').append("<tr><td>" +
                        $('#ProductId :selected').text() + "</td><td>" +
                        $('#SupplierName').val() + "</td><td>Custom</td><td>" +
                        $('#PolicySupplierServiceInformationTypeId :selected').text() + "</td><td>" +
                        escapeHTML($('#PolicySupplierServiceInformationValue').val()) + "</td><td>Added</td></tr>");
                } else {
                    //modification
                    //find it in display table first and change to show modified

                    $('#currentPolicySupplierServiceInformationsTable tbody tr').each(function () {

                        if ($(this).attr("PolicySupplierServiceInformationId") == $('#PolicySupplierServiceInformationId').val()) {

                            //edit visual table
                            $(this).html("<td>" +
                                $('#ProductId :selected').text() + "</td><td>" +
                                $('#SupplierName').val() + "</td><td>Custom</td><td>" +
                                $('#PolicySupplierServiceInformationTypeId :selected').text() + "</td><td>" +
                                escapeHTML($('#PolicySupplierServiceInformationValue').val()) + "</td><td>Modified</td>");

                            $(this).contents('td').css({ 'background-color': '#CCCCCC' });

                            //add to hidden table
                            $('#hiddenChangedPolicySupplierServiceInformationsTable').append("<tr PolicySupplierServiceInformationId='" + $('#PolicySupplierServiceInformationId').val() +
                                "' PolicyGroupId='" + $('#PolicyGroupId').val() +
                                "' PolicySupplierServiceInformationValue='" + escape($('#PolicySupplierServiceInformationValue').val()) + 
                                "' PolicySupplierServiceInformationTypeId='" + $('#PolicySupplierServiceInformationTypeId').val() + 
                                "' ProductId='" + $('#ProductId').val() + 
                                "' SupplierCode='" + $('#SupplierCode').val() + 
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
