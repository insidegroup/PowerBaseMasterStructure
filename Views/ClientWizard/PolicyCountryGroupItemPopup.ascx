<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.PolicyCountryGroupItem>" %>
<% Html.EnableClientValidation(); %>
<% using (Html.BeginForm()) {%>
    <%= Html.AntiForgeryToken() %>
    <%= Html.ValidationSummary(true) %>
    <table cellpadding="0" cellspacing="0" width="100%"> 
        <tr>
            <td><label for="CountryCode">Country</label></td>
            <td> <%= Html.TextBoxFor(model => model.CountryName)%><span class="error"> *</span></td>
            <td>
                <%= Html.ValidationMessageFor(model => model.CountryCode)%>
                <%= Html.Hidden("CountryCode")%>
                <label id="lblCountryNameMsg"/>
            </td>
        </tr> 
        <tr>
            <td><label for="PolicyCountryStatusId">Country Status</label></td>
            <td><%= Html.DropDownList("PolicyCountryStatusId", ViewData["PolicyCountryStatusList"] as SelectList, "None")%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.PolicyCountryStatusId)%></td>
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
            <td><label for="EnabledFlag">Enabled?</label></td>
            <td><%= Html.CheckBoxFor(model => model.EnabledFlag)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledFlag)%></td>
        </tr>  
        <tr>
            <td><label for="InheritFromParentFlag">Inherit From Parent?</label></td>
            <td><%= Html.CheckBoxFor(model => model.InheritFromParentFlag)%></td>
            <td><%= Html.ValidationMessageFor(model => model.InheritFromParentFlag)%></td>
        </tr>
        <tr>
            <td width="30%" class="row_footer_left"></td>
            <td width="40%" class="row_footer_centre"></td>
            <td width="30%" class="row_footer_right"></td>
        </tr>
    </table>
    <%=Html.HiddenFor(model => model.PolicyCountryGroupItemId)%>
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


    /////////////////////////////////////////////////////////
    // BEGIN COUNTRY AUTOCOMPLETE
    /////////////////////////////////////////////////////////
    $(function () {
        $("#CountryName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/Countries", type: "POST", dataType: "json",
                    data: { searchText: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.CountryName,
                                id: item.CountryCode
                            }
                        }))
                    }
                })
            },
            mustMatch: true,
            select: function (event, ui) {
                $("#CountryCode").val(ui.item.id);
            },
            search: function (event, ui) {
                $("#CountryCode").val("");
            }
        });
    });
    /////////////////////////////////////////////////////////
    // END COUNTRY AUTOCOMPLETE
    /////////////////////////////////////////////////////////
});


/*
Validate PolicyCountryGroupItem and write to hidden tables or return validation errors
*/
function PolicyCountryGroupItemValidation() {

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
    var validItem = false;

        jQuery.ajax({
            type: "POST",
            url: "/Hierarchy.mvc/IsValidCountry",
            data: { searchText: $("#CountryName").val() },
            success: function (data) {

                if (!jQuery.isEmptyObject(data)) {
                    validItem = true;
                }
            },
            dataType: "json",
            async: false
        });
        if (!validItem) {
            $("#lblCountryNameMsg").removeClass('field-validation-valid');
            $("#lblCountryNameMsg").addClass('field-validation-error');
            $("#lblCountryNameMsg").text("This is not a valid country.");
            return;
        }

    var url = '/ClientWizard.mvc/PolicyCountryGroupItemValidation';

    //handle date messages
    var dExpiryDate = $('#ExpiryDate').val();
    var dEnabledDate = $('#EnabledDate').val()
    if ($('#ExpiryDate').val() == "No Expiry Date") {
        dExpiryDate = "";
    }
    if ($('#EnabledDate').val() == "No Enabled Date") {
        dEnabledDate = "";
    }

    var dialogueWindow = $("#dialog-confirm");

    //Build Object to Store PolicyCountryGroupItem
    var policyCountryGroupItem = {
        PolicyCountryGroupItemId: $("#PolicyCountryGroupItemId").val(),
        PolicyGroupId: $("#PolicyGroupId").val(),
        PolicyCountryStatusId: $("#PolicyCountryStatusId").val(),
        EnabledFlag: $("#EnabledFlag:checked").val(),
        EnabledDate: dEnabledDate,
        ExpiryDate: dExpiryDate,
        TravelDateValidFrom: $("#TravelDateValidFrom").val(),
        TravelDateValidTo: $("#TravelDateValidTo").val(),
        CountryName: $("#CountryName").val(),
        CountryCode: $("#CountryCode").val(),
        InheritFromParentFlag: $("#InheritFromParentFlag").is(":checked"),
        VersionNumber: $("#VersionNumber", dialogueWindow).val()
    };


    //AJAX (JSON) POST of PolicyCountryGroupItem Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(policyCountryGroupItem),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.success) {
				
				//add to tables
				var InheritFromParentFlag = "";
				
				if($('#InheritFromParentFlag').is(":checked")){
					
					InheritFromParentFlag = "True"
				}
				
				else {
					
					InheritFromParentFlag = "False"
				}
				
				var EnabledFlag = "";
				
				if($("#EnabledFlag").is(':checked')) {
				EnabledFlag = "True"	
						
				}
				else {
					
				EnabledFlag = "False"
					
				}
				
				if($("#PolicyCountryGroupItemId").val()=="0") {
				
				//new item
				//hidden
					$('#hiddenAddedPolicyCountryGroupItemsTable').append("<tr PolicyCountryGroupItemId='" + $("#PolicyCountryGroupItemId").val() + "' PolicyGroupId = '" + $("#PolicyGroupId").val() + "' PolicyCountryStatusId='" + $("#PolicyCountryStatusId").val() + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + dEnabledDate + "' ExpiryDate='" + dExpiryDate + "' TravelDateValidFrom='" + $("#TravelDateValidFrom").val() + "' TravelDateValidTo='" + $("#TravelDateValidTo").val() + "' CountryName='" + $("#CountryName").val() + "' CountryCode='" + $("#CountryCode").val() + "' InheritFromParentFlag='" + InheritFromParentFlag + "' VersionNumber='" + $("#VersionNumber", dialogueWindow).val() + "'></tr>")
				
				
				//visible
				
$('#currentPolicyCountryGroupItemsTable tbody').append("<tr PolicyCountryGroupItemId='"+$("#PolicyCountryGroupItemId").val()+"' Source='Custom'><td>"+$('#CountryName').val()+"</td><td>Custom</td><td>"+$('#PolicyCountryStatusId :selected').text()+"</td><td>"+$("#TravelDateValidFrom").val()+"</td><td>"+$("#TravelDateValidTo").val()+"</td><td>Added</td></tr>")
				}
				
				else {
					
					//current with ammend
					
					//hidden
					
					$('#hiddenChangedPolicyCountryGroupItemsTable').append("<tr PolicyCountryGroupItemId='" + $("#PolicyCountryGroupItemId").val() + "' PolicyGroupId = '" + $("#PolicyGroupId").val() + "' PolicyCountryStatusId='" + $("#PolicyCountryStatusId").val() + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + dEnabledDate + "' ExpiryDate='" + dExpiryDate + "' TravelDateValidFrom='" + $("#TravelDateValidFrom").val() + "' TravelDateValidTo='" + $("#TravelDateValidTo").val() + "' CountryName='" + $("#CountryName").val() + "' CountryCode='" + $("#CountryCode").val() + "' InheritFromParentFlag='" + InheritFromParentFlag + "' VersionNumber='" + $("#VersionNumber", dialogueWindow).val() + "'></tr>")
					//visible
					
					$('#currentPolicyCountryGroupItemsTable tbody tr').each(function(){
						
						if($(this).attr("PolicyCountryGroupItemId")==$('#PolicyCountryGroupItemId').val()){
							
						
							$(this).html("<td>"+$('#CountryName').val()+"</td><td>Custom</td><td>"+$('#PolicyCountryStatusId :selected').text()+"</td><td>"+$("#TravelDateValidFrom").val()+"</td><td>"+$("#TravelDateValidTo").val()+"</td><td>Modified</td>")
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
