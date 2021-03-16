<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.PolicyHotelCapRateGroupItem>" %>
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
            <td> <label for="EnabledFlag">Enabled</label></td>
           <td><%= Html.CheckBoxFor(model => model.EnabledFlag)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledFlag)%></td>
        </tr>  
            <tr>
            <td> <label for="CurrencyCode">Currency</label></td>
            <td><%= Html.DropDownList("CurrencyCode", ViewData["CurrencyList"] as SelectList, "None")%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.CurrencyCode)%></td>
        </tr> 
        <tr>
            <td> <label for="CapRate">Cap Rate</label></td>
            <td> <%= Html.TextBoxFor(model => model.CapRate)%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.CapRate)%></td>
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
            <td><label for="PolicyProhibitedFlag">Tax Inclusive?</label> </td>
            <td><%= Html.CheckBoxFor(model => model.TaxInclusiveFlag)%></td>
            <td><%= Html.ValidationMessageFor(model => model.TaxInclusiveFlag)%></td>
        </tr> 
        <tr>
            <td width="30%" class="row_footer_left"></td>
            <td width="40%" class="row_footer_centre"></td>
            <td width="30%" class="row_footer_right"></td>
        </tr>
    </table>
    <%=Html.HiddenFor(model => model.PolicyHotelCapRateItemId)%>
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
});


/*
Validate PolicyHotelCapRateGroupItem and write to hidden tables or return validation errors
*/
function PolicyHotelCapRateGroupItemValidation() {

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

    var url = '/ClientWizard.mvc/PolicyHotelCapRateGroupItemValidation';

    var dialogueWindow = $("#dialog-confirm");

    //Build Object to Store PolicyHotelCapRateGroupItem
    var policyHotelCapRateGroupItem = {
        PolicyHotelCapRateItemId: $("#PolicyHotelCapRateItemId").val(),
        PolicyGroupId: $("#PolicyGroupId").val(),
        PolicyLocationId: $("#PolicyLocationId").val(),
        EnabledFlag: $('#EnabledFlag').is(':checked'),
        EnabledDate: $("#EnabledDate").val(),
        ExpiryDate: $("#ExpiryDate").val(),
        TravelDateValidFrom: $("#TravelDateValidFrom").val(),
        TravelDateValidTo: $("#TravelDateValidTo").val(),
        CurrencyCode: $("#CurrencyCode").val(),
        CapRate: $("#CapRate").val(),
        //PolicyProhibitedFlag: $("#PolicyProhibitedFlag:checked").val(),
        TaxInclusiveFlag: $("#TaxInclusiveFlag:checked").val(),
        VersionNumber: $("#VersionNumber", dialogueWindow).val()
    };


    //AJAX (JSON) POST of PolicyHotelCapRateGroupItem Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(policyHotelCapRateGroupItem),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.success) {
				
				var EnabledFlag = "";
				
				if($("#EnabledFlag").is(':checked')) {
				    EnabledFlag = "True"	
				}else {
				    EnabledFlag = "False"
				}			
				if($("#PolicyHotelCapRateItemId").val()=="0") {
				
				    //new
				    //hidden table
				        $('#hiddenAddedPolicyHotelCapRateGroupItemsTable').append("<tr PolicyHotelCapRateItemId = '" + $("#PolicyHotelCapRateItemId").val() +
                    "' PolicyGroupId = '" + $("#PolicyGroupId").val() +
                    "' PolicyLocationId='" + $("#PolicyLocationId").val() +
                    "' EnabledFlag='" + EnabledFlag + 
                    "' EnabledDate='" + $("#EnabledDate").val() +
                    "' ExpiryDate='" + $("#ExpiryDate").val() +
                    "' TravelDateValidFrom='" + $("#TravelDateValidFrom").val() +
                    "' TravelDateValidTo='" + $("#TravelDateValidTo").val() +
                    "' CurrencyCode='" + $("#CurrencyCode").val() +
                    "' CapRate='" + $("#CapRate").val() +
                    //"' PolicyProhibitedFlag='" + $("#PolicyProhibitedFlag").is(':checked') +
					"' TaxInclusiveFlag='" + $("#TaxInclusiveFlag").is(':checked') +
                    "' VersionNumber='" + $("#VersionNumber", dialogueWindow).val() + "' ></tr>")
					
				    //visual table
				
				        $('#currentPolicyHotelCapRateGroupItemsTable tbody').append("<tr  PolicyHotelCapRateItemId='" + $("#PolicyHotelCapRateItemId").val() + "' Source='Custom'><td>" + $('#PolicyLocationId :selected').text() + "</td><td>Custom</td><td>" + $('#CapRate').val() + "</td><td>" + $('#CurrencyCode :selected').text() + "</td><td>" + ($('#TaxInclusiveFlag').is(':checked') ? 'Yes' : 'No') + "</td><td>" + $("#TravelDateValidFrom").val() + "</td><td>" + $("#TravelDateValidTo").val() + "</td><td>Added</td></tr>")
				}else {
					
					
				    //existing with change
				    //hidden
				        $('#hiddenChangedPolicyHotelCapRateGroupItemsTable').append("<tr PolicyHotelCapRateItemId = '" + $("#PolicyHotelCapRateItemId").val() +
                    "' PolicyGroupId = '" + $("#PolicyGroupId").val() +
                    "' PolicyLocationId='" + $("#PolicyLocationId").val() +
                    "' EnabledFlag='" + EnabledFlag + 
                    "' EnabledDate='" + $("#EnabledDate").val() +
                    "' ExpiryDate='" + $("#ExpiryDate").val() +
                    "' TravelDateValidFrom='" + $("#TravelDateValidFrom").val() +
                    "' TravelDateValidTo='" + $("#TravelDateValidTo").val() +
                    "' CurrencyCode='" + $("#CurrencyCode").val() + 
                    "' CapRate='" + $("#CapRate").val() +
                    //"' PolicyProhibitedFlag='" + $("#PolicyProhibitedFlag").is(':checked') +
					"' TaxInclusiveFlag='" + $("#TaxInclusiveFlag").is(':checked') +
                    "' VersionNumber='" + $("#VersionNumber", dialogueWindow).val() + "' ></tr>")
					
				    //visible
				
				    $('#currentPolicyHotelCapRateGroupItemsTable tbody tr').each(function(){
					
					    if($(this).attr("PolicyHotelCapRateItemId")==$("#PolicyHotelCapRateItemId").val()) {
						    //match
					    	$(this).html("<td>" + $('#PolicyLocationId :selected').text() + "</td><td>Custom</td><td>" + $('#CapRate').val() + "</td><td>" + $('#CurrencyCode :selected').text() + "</td><td>" + ($('#TaxInclusiveFlag').is(':checked') ? 'Yes' : 'No') + "</td><td>" + $("#TravelDateValidFrom").val() + "</td><td>" + $("#TravelDateValidTo").val() + "</td><td>Modified</td>")
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
