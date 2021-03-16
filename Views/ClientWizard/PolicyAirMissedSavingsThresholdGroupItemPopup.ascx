<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.PolicyAirMissedSavingsThresholdGroupItemVM>" %>
<% Html.EnableClientValidation(); %>
<% using (Html.BeginForm()) {%>
    <%= Html.AntiForgeryToken() %>
    <%= Html.ValidationSummary(true) %>
    <table cellpadding="0" cellspacing="0" width="100%"> 
        <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_Amount">Amount</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.MissedThresholdAmount, new { maxlength = "11" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.MissedThresholdAmount)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_CurrencyCode">Currency</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.CurrencyCode, Model.Currencies, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.CurrencyCode)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_RoutingCode">Routing</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.RoutingCode, Model.RoutingCodes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.RoutingCode)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_EnabledFlag">Enabled?</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.EnabledFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.EnabledFlag)%></td>
                </tr>  
                 <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.TravelDateValidFrom)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo">Travel Date Valid To</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.TravelDateValidTo)%></td>
                </tr>   
        <tr>
            <td width="30%" class="row_footer_left"></td>
            <td width="40%" class="row_footer_centre"></td>
            <td width="30%" class="row_footer_right"></td>
        </tr>
    </table>
    <%=Html.HiddenFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.PolicyAirMissedSavingsThresholdGroupItemId)%>
    <%=Html.HiddenFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.PolicyProhibitedFlag)%>
    <%=Html.HiddenFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.SavingsZeroedOutFlag)%>
    <%=Html.HiddenFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.PolicyGroupId)%>
    <%=Html.HiddenFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.VersionNumber)%>
<% } %>
<script type="text/javascript">
/*
Validate PolicyAirMissedSavingsThresholdGroupItem and write to hidden tables or return validation errors
*/
function PolicyAirMissedSavingsThresholdGroupItemValidation() {

    //reset date fields
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val() == "No Enabled Date") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val("");
    }
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val() == "No Expiry Date") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val("");
    }
    //reset date fields
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val() == "No Travel Date Valid From") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val("");
    }
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val() == "No Travel Date Valid To") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val("");
    }

    var url = '/ClientWizard.mvc/PolicyAirMissedSavingsThresholdGroupItemValidation';

    //handle date messages
    var dEnabledDate = $('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val();
    var dExpiryDate = $('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val();
    var dTravelDateValidFrom = $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val();
    var dTravelDateValidTo = $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val()

    if (dExpiryDate == "No Expiry Date") {
        dExpiryDate = "";
    }
    if (dEnabledDate == "No Enabled Date") {
        dEnabledDate = "";
    }
    if (dTravelDateValidFrom == "No Travel Date Valid From") {
        dTravelDateValidFrom = "";
    }
    if (dTravelDateValidTo == "No Travel Date Valid To") {
        dTravelDateValidTo = "";
    }
    //Build Object to Store PolicyAirMissedSavingsThresholdGroupItem
    var policyAirMissedSavingsThresholdGroupItem = {
        PolicyAirMissedSavingsThresholdGroupItemId: $("#PolicyAirMissedSavingsThresholdGroupItem_PolicyAirMissedSavingsThresholdGroupItemId").val(),
        PolicyGroupId: $("#PolicyAirMissedSavingsThresholdGroupItem_PolicyGroupId").val(),
        MissedThresholdAmount: $("#PolicyAirMissedSavingsThresholdGroupItem_MissedThresholdAmount").val().replace(/,/g, ''), //Remove commas to allow to save
        CurrencyCode: $("#PolicyAirMissedSavingsThresholdGroupItem_CurrencyCode").val(),
        RoutingCode: $("#PolicyAirMissedSavingsThresholdGroupItem_RoutingCode").val(),
        SavingsZeroedOutFlag: false,
        PolicyProhibitedFlag: false,
        EnabledFlag: $("#PolicyAirMissedSavingsThresholdGroupItem_EnabledFlag:checked").val(),
        EnabledDate: dEnabledDate,
        ExpiryDate: dExpiryDate,
        TravelDateValidFrom: dTravelDateValidFrom,
        TravelDateValidTo: dTravelDateValidTo,
        VersionNumber: $("#PolicyAirMissedSavingsThresholdGroupItem_VersionNumber").val()
    };


    var policyAirMissedSavingsThresholdGroupItemVM = {
        PolicyAirMissedSavingsThresholdGroupItem: policyAirMissedSavingsThresholdGroupItem
    }


    //AJAX (JSON) POST of PolicyAirMissedSavingsThresholdGroupItem Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(policyAirMissedSavingsThresholdGroupItemVM),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.success) {
				
				//add to tables
                var SavingsZeroedOutFlag = "False"
                var PolicyProhibitedFlag = "False"

                var EnabledFlag = "";
                if ($("#PolicyAirMissedSavingsThresholdGroupItem_EnabledFlag").is(':checked')) {
                	EnabledFlag = "True"
                } else {
                	EnabledFlag = "False"
                }

				// Remove Commas otherwise DB uses NULL
				var PolicyAirMissedSavingsThresholdGroupItem_MissedThresholdAmount = $("#PolicyAirMissedSavingsThresholdGroupItem_MissedThresholdAmount").val().replace(/,/g, '');

                if ($("#PolicyAirMissedSavingsThresholdGroupItem_PolicyAirMissedSavingsThresholdGroupItemId").val() == "0") {

                	//new item

                	//hidden
                	$('#hiddenAddedPolicyAirMissedSavingsThresholdGroupItemsTable').append("<tr PolicyAirMissedSavingsThresholdGroupItemId='" + $("#PolicyAirMissedSavingsThresholdGroupItem_PolicyAirMissedSavingsThresholdGroupItemId").val() + "' PolicyGroupId = '" + $("#PolicyAirMissedSavingsThresholdGroupItem_PolicyGroupId").val() + "' MissedThresholdAmount='" + PolicyAirMissedSavingsThresholdGroupItem_MissedThresholdAmount + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + dEnabledDate + "' ExpiryDate='" + dExpiryDate + "' TravelDateValidFrom='" + dTravelDateValidFrom + "' TravelDateValidTo='" + dTravelDateValidTo + "' CurrencyCode='" + $("#PolicyAirMissedSavingsThresholdGroupItem_CurrencyCode").val() + "' RoutingCode='" + $("#PolicyAirMissedSavingsThresholdGroupItem_RoutingCode").val() + "' SavingsZeroedOutFlag='" + SavingsZeroedOutFlag + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' VersionNumber='" + $("#PolicyAirMissedSavingsThresholdGroupItem_VersionNumber").val() + "'></tr>")
				
				    //visible
                    $('#currentPolicyAirMissedSavingsThresholdGroupItemsTable tbody').append("<tr PolicyAirMissedSavingsThresholdGroupItemId='" + $("#PolicyAirMissedSavingsThresholdGroupItem_PolicyAirMissedSavingsThresholdGroupItemId").val() + "' Source='Custom'><td>" + $('#PolicyAirMissedSavingsThresholdGroupItem_MissedThresholdAmount').val() + "</td><td>" + $('#PolicyAirMissedSavingsThresholdGroupItem_CurrencyCode :selected').text() + "</td><td>Custom</td><td>" + $('#PolicyAirMissedSavingsThresholdGroupItem_RoutingCode :selected').text() + "</td><td>" + dTravelDateValidFrom + "</td><td>" + dTravelDateValidTo + "</td><td>Added</td></tr>")
				}
				
				else {
					
					//current with ammend
					
					//hidden
                	$('#hiddenChangedPolicyAirMissedSavingsThresholdGroupItemsTable').append("<tr PolicyAirMissedSavingsThresholdGroupItemId='" + $("#PolicyAirMissedSavingsThresholdGroupItem_PolicyAirMissedSavingsThresholdGroupItemId").val() + "' PolicyGroupId = '" + $("#PolicyAirMissedSavingsThresholdGroupItem_PolicyGroupId").val() + "' MissedThresholdAmount='" + PolicyAirMissedSavingsThresholdGroupItem_MissedThresholdAmount + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + dEnabledDate + "' ExpiryDate='" + dExpiryDate + "' TravelDateValidFrom='" + dTravelDateValidFrom + "' TravelDateValidTo='" + dTravelDateValidTo + "' CurrencyCode='" + $("#PolicyAirMissedSavingsThresholdGroupItem_CurrencyCode").val() + "' RoutingCode='" + $("#PolicyAirMissedSavingsThresholdGroupItem_RoutingCode").val() + "' PolicyProhibitedFlag='" + PolicyProhibitedFlag + "' SavingsZeroedOutFlag='" + SavingsZeroedOutFlag + "' VersionNumber='" + $("#PolicyAirMissedSavingsThresholdGroupItem_VersionNumber").val() + "'></tr>")

                	//visible
					$('#currentPolicyAirMissedSavingsThresholdGroupItemsTable tbody tr').each(function(){

					    if ($(this).attr("PolicyAirMissedSavingsThresholdGroupItemId") == $('#PolicyAirMissedSavingsThresholdGroupItem_PolicyAirMissedSavingsThresholdGroupItemId').val()) {


					        $(this).html("<td>" + $('#PolicyAirMissedSavingsThresholdGroupItem_MissedThresholdAmount').val() + "</td><td>" + $('#PolicyAirMissedSavingsThresholdGroupItem_CurrencyCode :selected').text() + "</td><td>Custom</td><td>" + $('#PolicyAirMissedSavingsThresholdGroupItem_RoutingCode :selected').text() + "</td><td>" + dTravelDateValidFrom + "</td><td>" + dTravelDateValidTo + "</td><td>Modified</td>")
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

$(document).ready(function () {

    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    /////////////////////////////////////////////////////////
    // BEGIN DATES SETUP
    /////////////////////////////////////////////////////////

    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "PolicyAirMissedSavingsThresholdGroupItem_EnabledDate",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });

    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });

   

    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });

    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });


    if ($('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val() == "") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_EnabledDate').val("No Enabled Date");
    }
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val() == "") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate').val("No Expiry Date");
    }
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val() == "") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom').val("No Travel Date Valid From");
    }
    if ($('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val() == "") {
        $('#PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo').val("No Travel Date Valid To");
    }
    /////////////////////////////////////////////////////////
    // END DATES SETUP
    /////////////////////////////////////////////////////////


   
});
</script>
