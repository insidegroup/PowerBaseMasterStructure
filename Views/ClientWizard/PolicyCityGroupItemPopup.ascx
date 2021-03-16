<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.PolicyCityGroupItemVM>" %>
<% Html.EnableClientValidation(); %>
<% using (Html.BeginForm()) {%>
    <%= Html.AntiForgeryToken() %>
    <%= Html.ValidationSummary(true) %>
    <table cellpadding="0" cellspacing="0" width="100%"> 
        <tr>
            <td><label for="PolicyCityGroupItem_City_Name">City</label></td>
            <td><%= Html.TextBoxFor(model => model.PolicyCityGroupItem.City.Name, new { style = "width:200px;" })%><span class="error"> *</span></td>
            <td>
                <%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.CityCode)%>
            </td>
        </tr> 
         <tr>
                <td><label for="PolicyCityGroupItem_PolicyCityStatusId">City Status</label></td>
                <td><%= Html.DropDownListFor(model => model.PolicyCityGroupItem.PolicyCityStatusId, Model.PolicyCityStatuses, "Please Select...")%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.PolicyCityStatusId)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyCityGroupItem_EnabledFlag">Enabled?</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyCityGroupItem.EnabledFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.EnabledFlag)%></td>
                    
            </tr>  
            <tr>
                <td><label for="PolicyCityGroupItem_EnabledDate">Enabled Date</label></td>
                <td><%= Html.EditorFor(model => model.PolicyCityGroupItem.EnabledDate)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.EnabledDate)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyCityGroupItem_ExpiryDate">Expiry Date</label> </td>
                <td> <%= Html.TextBoxFor(model => model.PolicyCityGroupItem.ExpiryDate)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.ExpiryDate)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyCityGroupItem_TravelDateValidFrom">Travel Date Valid From</label></td>
                <td><%= Html.EditorFor(model => model.PolicyCityGroupItem.TravelDateValidFrom)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.TravelDateValidFrom)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyCityGroupItem_TravelDateValidTo">Travel Date Valid To</label></td>
                <td> <%= Html.EditorFor(model => model.PolicyCityGroupItem.TravelDateValidTo)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.TravelDateValidTo)%></td>
            </tr>  
            <tr>
                <td><label for="PolicyCityGroupItem_InheritFromParentFlag">Inherit From Parent?</label></td>
                <td><%= Html.EditorFor(model => model.PolicyCityGroupItem.InheritFromParentFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.InheritFromParentFlag)%></td>
            </tr>  
        <tr>
            <td width="30%" class="row_footer_left"></td>
            <td width="40%" class="row_footer_centre"></td>
            <td width="30%" class="row_footer_right"></td>
        </tr>
    </table>
    <%=Html.HiddenFor(model => model.PolicyCityGroupItem.PolicyCityGroupItemId)%>
    <%=Html.HiddenFor(model => model.PolicyCityGroupItem.CityCode)%>
    <%=Html.HiddenFor(model => model.PolicyCityGroupItem.PolicyGroupId)%>
    <%=Html.HiddenFor(model => model.PolicyCityGroupItem.VersionNumber)%>
<% } %>
<script type="text/javascript">

    /////////////////////////////////////////////////////////
    // BEGIN CITY AUTOCOMPLETE
    /////////////////////////////////////////////////////////
    $(function () {
        $("#PolicyCityGroupItem_City_Name").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/Cities", type: "POST", dataType: "json",
                    data: { searchText: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Name,
                                id: item.CityCode
                            }
                        }))
                    }
                })
            },
            mustMatch: true,
            select: function (event, ui) {
                $("#PolicyCityGroupItem_CityCode").val(ui.item.id);
            },
            search: function (event, ui) {
                $("#PolicyCityGroupItem_CityCode").val("");
            }
        });
    });
    /////////////////////////////////////////////////////////
    // END CITY AUTOCOMPLETE
    /////////////////////////////////////////////////////////






/*
Validate PolicyCityGroupItem and write to hidden tables or return validation errors
*/
function PolicyCityGroupItemValidation() {

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
    var validCity = true;

        jQuery.ajax({
            type: "POST",
            url: "/Validation.mvc/IsValidCityCode",
            data: { cityCode: $("#PolicyCityGroupItem_CityCode").val() },
            success: function (data) {

                if (jQuery.isEmptyObject(data)) {
                    validCity = false;
                }
            },
            dataType: "json",
            async: false
        });
        if (!validCity) {
            $("#PolicyCityGroupItem_CityCode_validationMessage").removeClass('field-validation-valid');
            $("#PolicyCityGroupItem_CityCode_validationMessage").addClass('field-validation-error');
            $("#PolicyCityGroupItem_CityCode_validationMessage").text("This is not a valid City.");
            return;
        }

    var url = '/ClientWizard.mvc/PolicyCityGroupItemValidation';

    //handle date messages
    var dExpiryDate = $('#PolicyCityGroupItem_ExpiryDate').val();
    var dEnabledDate = $('#PolicyCityGroupItem_EnabledDate').val()
    var dTravelDateValidFrom = $('#PolicyCityGroupItem_TravelDateValidFrom').val();
    var dTravelDateValidTo = $('#PolicyCityGroupItem_TravelDateValidTo').val()

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
    //Build Object to Store PolicyCityGroupItem
    var policyCityGroupItem = {
        PolicyCityGroupItemId: $("#PolicyCityGroupItem_PolicyCityGroupItemId").val(),
        PolicyGroupId: $("#PolicyCityGroupItem_PolicyGroupId").val(),
        PolicyCityStatusId: $("#PolicyCityGroupItem_PolicyCityStatusId").val(),
        EnabledFlag: $("#PolicyCityGroupItem_EnabledFlag:checked").val(),
        EnabledDate: dEnabledDate,
        ExpiryDate: dExpiryDate,
        TravelDateValidFrom: dTravelDateValidFrom,
        TravelDateValidTo: dTravelDateValidTo,
        CityName: $("#PolicyCityGroupItem_City_CityName").val(),
        CityCode: $("#PolicyCityGroupItem_CityCode").val(),
        InheritFromParentFlag: $("#PolicyCityGroupItem_InheritFromParentFlag").is(":checked"),
        VersionNumber: $("#PolicyCityGroupItem_VersionNumber").val()
    };


    var policyCityGroupItemVM = {
        PolicyCityGroupItem: policyCityGroupItem
    }


    //AJAX (JSON) POST of PolicyCityGroupItem Object
    $.ajax({
        type: "POST",
        data: JSON.stringify(policyCityGroupItemVM),
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.success) {
				
				//add to tables
				var InheritFromParentFlag = "";
				if ($('#PolicyCityGroupItem_InheritFromParentFlag').is(":checked")) {
					InheritFromParentFlag = "True"
				}else {
					InheritFromParentFlag = "False"
				}
				
				var EnabledFlag = "";
				if ($("#PolicyCityGroupItem_EnabledFlag").is(':checked')) {
				    EnabledFlag = "True"	
				}else {
				    EnabledFlag = "False"
				}

                if ($("#PolicyCityGroupItem_PolicyCityGroupItemId").val() == "0") {
				
				    //new item
				    //hidden
                    $('#hiddenAddedPolicyCityGroupItemsTable').append("<tr PolicyCityGroupItemId='" + $("#PolicyCityGroupItem_PolicyCityGroupItemId").val() + "' PolicyGroupId = '" + $("#PolicyCityGroupItem_PolicyGroupId").val() + "' PolicyCityStatusId='" + $("#PolicyCityGroupItem_PolicyCityStatusId").val() + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + dEnabledDate + "' ExpiryDate='" + dExpiryDate + "' TravelDateValidFrom='" + dTravelDateValidFrom + "' TravelDateValidTo='" + dTravelDateValidTo + "' CityName='" + $("#PolicyCityGroupItem_City_Name").val() + "' CityCode='" + $("#PolicyCityGroupItem_CityCode").val() + "' InheritFromParentFlag='" + InheritFromParentFlag + "' VersionNumber='" + $("#PolicyCityGroupItem_VersionNumber").val() + "'></tr>")	
				
				    //visible
                    $('#currentPolicyCityGroupItemsTable tbody').append("<tr PolicyCityGroupItemId='" + $("#PolicyCityGroupItem_PolicyCityGroupItemId").val() + "' Source='Custom'><td>" + $('#PolicyCityGroupItem_City_Name').val() + "</td><td>Custom</td><td>" + $('#PolicyCityGroupItem_PolicyCityStatusId :selected').text() + "</td><td>" + dEnabledDate + "</td><td>" + dExpiryDate + "</td><td>" + dTravelDateValidFrom + "</td><td>" + dTravelDateValidTo + "</td><td>Added</td></tr>")
				}
				
				else {
					
					//current with ammend
					
					//hidden

				    $('#hiddenChangedPolicyCityGroupItemsTable').append("<tr PolicyCityGroupItemId='" + $("#PolicyCityGroupItem_PolicyCityGroupItemId").val() + "' PolicyGroupId = '" + $("#PolicyCityGroupItem_PolicyGroupId").val() + "' PolicyCityStatusId='" + $("#PolicyCityGroupItem_PolicyCityStatusId").val() + "' EnabledFlag='" + EnabledFlag + "' EnabledDate='" + dEnabledDate + "' ExpiryDate='" + dExpiryDate + "' TravelDateValidFrom='" + dTravelDateValidFrom + "' TravelDateValidTo='" + dTravelDateValidTo + "' CityName='" + $("#PolicyCityGroupItem_City_Name").val() + "' CityCode='" + $("#PolicyCityGroupItem_CityCode").val() + "' InheritFromParentFlag='" + InheritFromParentFlag + "' VersionNumber='" + $("#PolicyCityGroupItem_VersionNumber").val() + "'></tr>")
					//visible
					
					$('#currentPolicyCityGroupItemsTable tbody tr').each(function(){

					    if ($(this).attr("PolicyCityGroupItemId") == $('#PolicyCityGroupItem_PolicyCityGroupItemId').val()) {


					        $(this).html("<td>" + $('#PolicyCityGroupItem_City_Name').val() + "</td><td>Custom</td><td>" + $('#PolicyCityGroupItem_PolicyCityStatusId :selected').text() + "</td><td>" + dEnabledDate + "</td><td>" + dExpiryDate + "</td><td>" + dTravelDateValidFrom + "</td><td>" + dTravelDateValidTo + "</td><td>Modified</td>")
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
        target: "PolicyCityGroupItem_ExpiryDate",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });

    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "PolicyCityGroupItem_EnabledDate",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });

    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "PolicyCityGroupItem_TravelDateValidFrom",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });

    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "PolicyCityGroupItem_TravelDateValidTo",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });


    if ($('#PolicyCityGroupItem_EnabledDate').val() == "") {
        $('#PolicyCityGroupItem_EnabledDate').val("No Enabled Date");
    }
    if ($('#PolicyCityGroupItem_ExpiryDate').val() == "") {
        $('#PolicyCityGroupItem_ExpiryDate').val("No Expiry Date");
    }
    if ($('#PolicyCityGroupItem_TravelDateValidFrom').val() == "") {
        $('#PolicyCityGroupItem_TravelDateValidFrom').val("No Travel Date Valid From");
    }
    if ($('#PolicyCityGroupItem_TravelDateValidTo').val() == "") {
        $('#PolicyCityGroupItem_TravelDateValidTo').val("No Travel Date Valid To");
    }
    /////////////////////////////////////////////////////////
    // END DATES SETUP
    /////////////////////////////////////////////////////////


   
});
</script>
