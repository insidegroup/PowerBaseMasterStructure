<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.PolicyCarTypeGroupItem>" %>
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
            <td> <label for="PolicyCarStatusId">Policy Car Status</label></td>
            <td><%= Html.DropDownList("PolicyCarStatusId", ViewData["PolicyCarStatusList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.PolicyCarStatusId)%></td>
        </tr>  
        <tr>
            <td><label for="CarTypeCategoryId">Car Type Category</label></td>
            <td><%= Html.DropDownList("CarTypeCategoryId", ViewData["CarTypeCategoryList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.CarTypeCategoryId)%></td>
        </tr> 
         <tr>
            <td><label for="EnabledFlag">Enabled?</label></td>
            <td><%= Html.CheckBoxFor(model => model.EnabledFlag)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledFlag)%></td>
        </tr> 
        <tr>
            <td><label for="EnabledDate">Enabled Date</label></td>
            <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledDate)%></td>
        </tr> 
        <tr>
            <td><label for="ExpiryDate">Expiry Date</label> </td>
            <td> <%= Html.EditorFor(model => model.ExpiryDate)%></td>
            <td><%= Html.ValidationMessageFor(model => model.ExpiryDate)%></td>
        </tr> 
        <tr>
            <td><label for="TravelDateValidFrom">Travel Date Valid From</label> </td>
            <td><%= Html.EditorFor(model => model.TravelDateValidFrom)%></td>
            <td><%= Html.ValidationMessageFor(model => model.TravelDateValidFrom)%></td>
        </tr> 
            <tr>
            <td><label for="TravelDateValidTo">Travel Date Valid To</label></td>
            <td><%= Html.EditorFor(model => model.TravelDateValidTo)%></td>
            <td><%= Html.ValidationMessageFor(model => model.TravelDateValidTo)%></td>
        </tr> 
        <tr>
            <td width="30%" class="row_footer_left"></td>
            <td width="40%" class="row_footer_centre"></td>
            <td width="30%" class="row_footer_right"></td>
        </tr>
    </table>
    <%=Html.HiddenFor(model => model.PolicyCarTypeGroupItemId)%>
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
Validate PolicyCarTypeGroupItem and write to hidden tables or return validation errors
*/
function PolicyCarTypeGroupItemValidation() {

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

    var url = '/ClientWizard.mvc/PolicyCarTypeGroupItemValidation';

    var dialogueWindow = $("#dialog-confirm");

	//Build Object to Store PolicyCarTypeGroupItem
    var policyCarTypeGroupItem = {
        PolicyCarTypeGroupItemId: $("#PolicyCarTypeGroupItemId").val(),
        PolicyGroupId: $("#PolicyGroupId").val(),
        PolicyLocationId: $("#PolicyLocationId").val(),
        PolicyCarStatusId: $("#PolicyCarStatusId").val(),
        CarTypeCategoryId: $("#CarTypeCategoryId").val(),
        EnabledFlag: $("#EnabledFlag").is(':checked'),
        EnabledDate: $("#EnabledDate").val(),
        ExpiryDate: $("#ExpiryDate").val(),
        TravelDateValidFrom: $("#TravelDateValidFrom").val(),
        TravelDateValidTo: $("#TravelDateValidTo").val(),
        VersionNumber: $("#VersionNumber", dialogueWindow).val()
    };

    //AJAX (JSON) POST of PolicyCarTypeGroupItem Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(policyCarTypeGroupItem),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.success) {
				
				//write to tables
				//alert($("#PolicyCarTypeGroupItemId").val());
				
				if($("#PolicyCarTypeGroupItemId").val()=="0") {
				
					//new
					
					//add to hidden table
					$('#hiddenAddedPolicyCarTypeGroupItemsTable').append("<tr PolicyCarTypeGroupItemId='" + $('#PolicyCarTypeGroupItemId').val() + "' PolicyGroupId='" + $('#PolicyGroupId').val() + "' PolicyLocationId='" + $('#PolicyLocationId').val() + "' PolicyCarStatusId='" + $('#PolicyCarStatusId').val() + "' CarTypeCategoryId='" + $('#CarTypeCategoryId').val() + "' EnabledFlag='" + $('#EnabledFlag').val() + "' EnabledDate='" + $('#EnabledDate').val() + "' ExpiryDate='" + $('#ExpiryDate').val() + "' TravelDateValidFrom='" + $('#TravelDateValidFrom').val() + "' TravelDateValidTo='" + $('#TravelDateValidTo').val() + "' VersionNumber='" + $('#VersionNumber', dialogueWindow).val() + "'></tr>");
					
					//add to visual table
					$('#currentPolicyCarTypeGroupItemsTable tbody').append("<tr><td>"+$('#CarTypeCategoryId :selected').text()+"</td><td>Custom</td><td>"+$('#PolicyCarStatusId :selected').text()+"</td><td>"+$('#PolicyLocationId :selected').text()+"</td><td>"+$("#TravelDateValidFrom").val()+"</td><td>"+$("#TravelDateValidTo").val()+"</td><td>Added</td></tr>")					
					
					
											
				}
				
				else
				{
					//modification
					//find it in display table first and change to show modified
					
					$('#currentPolicyCarTypeGroupItemsTable tbody tr').each(function(){
						
						if($(this).attr("PolicyCarTypeGroupItemId")==$('#PolicyCarTypeGroupItemId').val()){
							
							$(this).html("<td>"+$('#CarTypeCategoryId :selected').text()+"</td><td>Custom</td><td>"+$('#PolicyCarStatusId :selected').text()+"</td><td>"+$('#PolicyLocationId :selected').text()+"</td><td>"+$("#TravelDateValidFrom").val()+"</td><td>"+$("#TravelDateValidTo").val()+"</td><td>Modified</td>")
							 $(this).contents('td').css({ 'background-color': '#CCCCCC' });

							 $('#hiddenChangedPolicyCarTypeGroupItemsTable').append("<tr PolicyCarTypeGroupItemId='" + $('#PolicyCarTypeGroupItemId').val() + "' PolicyGroupId='" + $('#PolicyGroupId').val() + "' PolicyLocationId='" + $('#PolicyLocationId').val() + "' PolicyCarStatusId='" + $('#PolicyCarStatusId').val() + "' CarTypeCategoryId='" + $('#CarTypeCategoryId').val() + "' EnabledFlag='" + $('#EnabledFlag').val() + "' EnabledDate='" + $('#EnabledDate').val() + "' ExpiryDate='" + $('#ExpiryDate').val() + "' TravelDateValidFrom='" + $('#TravelDateValidFrom').val() + "' TravelDateValidTo='" + $('#TravelDateValidTo').val() + "' VersionNumber='" + $('#VersionNumber', dialogueWindow).val() + "'></tr>");
							
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
