<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.PolicyHotelVendorGroupItem>" %>
<% Html.EnableClientValidation(); %>
<% using (Html.BeginForm()) {%>
    <%= Html.AntiForgeryToken() %>
    <%= Html.ValidationSummary(true) %>
    <table cellpadding="0" cellspacing="0" width="100%"> 
		<tr>
            <td> <label for="PolicyLocationId">Policy Location</label></td>
            <td><%= Html.DropDownList("PolicyLocationId", ViewData["PolicyLocationList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.PolicyLocationId)%></td>
        </tr> 
        <tr>
            <td> <label for="PolicyHotelStatusId">Policy Hotel Status</label></td>
            <td><%= Html.DropDownList("PolicyHotelStatusId", ViewData["PolicyHotelStatusList"] as SelectList, "None")%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.PolicyHotelStatusId)%></td>
        </tr> 
        <tr>
            <td> <label for="EnabledFlag">Enabled?</label></td>
            <td><%= Html.CheckBoxFor(model => model.EnabledFlag)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledFlag)%></td>
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
            <td><label for="TravelDateValidFrom">Travel Date Valid From</label></td>
            <td><%= Html.EditorFor(model => model.TravelDateValidFrom)%></td>
            <td><%= Html.ValidationMessageFor(model => model.TravelDateValidFrom)%></td>
        </tr> 
        <tr>
            <td><label for="TravelDateValidTo">Travel Date Valid To</label> </td>
            <td> <%= Html.EditorFor(model => model.TravelDateValidTo)%></td>
            <td><%= Html.ValidationMessageFor(model => model.TravelDateValidTo)%></td>
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
    </table>
    <%=Html.HiddenFor(model => model.PolicyHotelVendorGroupItemId)%>
    <%=Html.HiddenFor(model => model.PolicyGroupId)%>
    <%=Html.HiddenFor(model => model.VersionNumber)%>
    <%=Html.Hidden("ProductId","2") %><!--Hotel-->
<% } %>
<script type="text/javascript">
$(document).ready(function () {

    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP - Version: No Product Select
    /////////////////////////////////////////////////////////

    $("#SupplierName").change(function () {
        if ($("#SupplierName").val() == "") {
            $("#SupplierCode").val("");
        }
    });

    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////

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
		
	new JsDatePick({
			useMode:2,
			isStripped:false,
		    target:"TravelDateValidFrom",
			dateFormat:"%M %d %Y",
			cellColorScheme:"beige"
		});
		
	new JsDatePick({
			useMode:2,
			isStripped:false,
		    target:"TravelDateValidTo",
			dateFormat:"%M %d %Y",
			cellColorScheme:"beige"
		});

    if ($('#EnabledDate').val() == "") {
        $('#EnabledDate').val("No Enabled Date");
    }
    if ($('#ExpiryDate').val() == "") {
        $('#ExpiryDate').val("No Expiry Date");
    }
    if ($('#TravelDateValidFrom').val() == "") {
        $('#TravelDateValidFrom').val("No Travel Date Valid From");
    }
    if ($('#TravelDateValidTo').val() == "") {
        $('#TravelDateValidTo').val("No Travel Date Valid To");
    }
    /////////////////////////////////////////////////////////
    // END DATES SETUP
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
Validate PolicyHotelVendorGroupItem and write to hidden tables or return validation errors
*/
function PolicyHotelVendorGroupItemValidation() {

    //reset date fields
    if ($('#EnabledDate').val() == "No Enabled Date") {
        $('#EnabledDate').val("");
    }
    if ($('#ExpiryDate').val() == "No Expiry Date") {
        $('#ExpiryDate').val("");
    }
    //reset date fields
    if ($('#TravelDateValidFrom').val() == "No Travel Date Valid From") {
        $('#TravelDateValidFrom').val("");
    }
    if ($('#TravelDateValidTo').val() == "No Travel Date Valid To") {
        $('#TravelDateValidTo').val("");
    }


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
            $("#SupplierName_validationMessage").html("");
            $("#lblSupplierNameMsg").removeClass('field-validation-valid');
            $("#lblSupplierNameMsg").addClass('field-validation-error');
            $("#lblSupplierNameMsg").text("This is not a valid Supplier");
            return;
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
            return;
        } else {
            $("#lblSupplierNameMsg").text("");
        }

    }

    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER VALIDATION
    /////////////////////////////////////////////////////////

    var url = '/ClientWizard.mvc/PolicyHotelVendorGroupItemValidation';

    var dialogueWindow = $("#dialog-confirm");

    //Build Object to Store PolicyHotelVendorGroupItem
    var policyHotelVendorGroupItem = {
        PolicyHotelVendorItemId: $("#PolicyHotelVendorItemId").val(),
        PolicyGroupId: $("#PolicyGroupId").val(),
        PolicyLocationId: $("#PolicyLocationId").val(),
        SupplierCode: $("#SupplierCode").val(),
        SupplierName: $("#SupplierName").val(),
        ProductId: $("#ProductId").val(),
        PolicyHotelStatusId: $("#PolicyHotelStatusId").val(),
        EnabledFlag: $('#EnabledFlag').is(':checked'),
        EnabledDate: $("#EnabledDate").val(),
        ExpiryDate: $("#ExpiryDate").val(),
        TravelDateValidFrom: $("#TravelDateValidFrom").val(),
        TravelDateValidTo: $("#TravelDateValidTo").val(),
        VersionNumber: $("#VersionNumber", dialogueWindow).val()
    };

    //AJAX (JSON) POST of PolicyHotelVendorGroupItem Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(policyHotelVendorGroupItem),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.success) {

                //write to tables
                if ($("#PolicyHotelVendorGroupItemId").val() == "0") { //new             
                   //hidden table
                    $('#hiddenAddedPolicyHotelVendorGroupItemsTable').append("<tr PolicyHotelVendorGroupItemId='" + $("#PolicyHotelVendorGroupItemId").val() +
                    "' PolicyGroupId='" + $("#PolicyGroupId").val() +
                    "' PolicyLocationId='" + $("#PolicyLocationId").val() +
                    "' SupplierCode='" + $("#SupplierCode").val() +
                    "' SupplierName='" + $("#SupplierName").val() +
                    "' ProductId='" + $("#ProductId").val() +
                    "' PolicyHotelStatusId='" + $("#PolicyHotelStatusId").val() +
                    "' EnabledFlag='" + $('#EnabledFlag').is(':checked') +
                    "' EnabledDate='" + $("#EnabledDate").val() +
                    "' ExpiryDate='" + $("#ExpiryDate").val() +
                    "' TravelDateValidFrom='" + $("#TravelDateValidFrom").val() +
                    "' TravelDateValidTo='" + $("#TravelDateValidTo").val() +
                    "' VersionNumber='" + $("#VersionNumber", dialogueWindow).val() + "' ></tr>");

                    //visual table
                    $('#currentPolicyHotelVendorGroupItemsTable tbody').append("<tr><td>" +
                    $('#SupplierName').val() + "</td><td>Custom</td><td>" +
                    $('#PolicyHotelStatusId :selected').text() + "</td><td>" +
                     $('#PolicyLocationId :selected').text() + "</td><td>" +                  
                    $("#TravelDateValidFrom").val() + "</td><td>" + 
                    $("#TravelDateValidTo").val() + "</td><td>Added</td></tr>")
                } else { //existing  with change

                    //hidden table
                    $('#hiddenChangedPolicyHotelVendorGroupItemsTable').append("<tr PolicyHotelVendorGroupItemId='" + $("#PolicyHotelVendorGroupItemId").val() +
                    "' PolicyGroupId='" + $("#PolicyGroupId").val() +
                    "' PolicyLocationId='" + $("#PolicyLocationId").val() +
                    "' SupplierCode='" + $("#SupplierCode").val() +
                    "' SupplierName='" + $("#SupplierName").val() +
                    "' ProductId='" + $("#ProductId").val() +
                    "' PolicyHotelStatusId='" + $("#PolicyHotelStatusId").val() +
                    "' EnabledFlag='" + $('#EnabledFlag').is(':checked') +
                    "' EnabledDate='" + $("#EnabledDate").val() +
                    "' ExpiryDate='" + $("#ExpiryDate").val() +
                    "' TravelDateValidFrom='" + $("#TravelDateValidFrom").val() +
                    "' TravelDateValidTo='" + $("#TravelDateValidTo").val() +
                    "' VersionNumber='" + $("#VersionNumber", dialogueWindow).val() + "' ></tr>");

                    //visual table
                    $('#currentPolicyHotelVendorGroupItemsTable tbody tr').each(function () {

                        if ($(this).attr("PolicyHotelVendorGroupItemId") == $('#PolicyHotelVendorGroupItemId').val()) {

                            //amend in visual
                            $(this).html("<td>" + $('#SupplierName').val() + "</td><td>Custom</td><td>" +
                            $('#PolicyHotelStatusId :selected').text() + "</td><td>" +
                             $('#PolicyLocationId :selected').text() + "</td><td>" +
                            
                            $("#TravelDateValidFrom").val() + "</td><td>" + 
                            $("#TravelDateValidTo").val() + "</td><td>Modified</td>");

                            $(this).contents('td').css({ 'background-color': '#CCCCCC' });
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
