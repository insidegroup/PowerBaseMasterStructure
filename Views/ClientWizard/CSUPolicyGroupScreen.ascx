<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.PolicyGroup>" %>
<% Html.EnableClientValidation(); %>
<% using (Html.BeginForm()) {%>
    <%= Html.ValidationSummary(true)%>
    <div id="divSearch">
        <table class="" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th><div style="float:right;"><span id="clientPolicyGroupBackButton"><small><< Back</small></span>&nbsp;<span id="clientPolicyGroupNextButton"><small>Next >></small> </span></div> </th>
            </tr>
        </thead>
        </table>
    </div>
    <table cellpadding="0" cellspacing="0" width="100%"> 
     <tr>
            <td colspan="3">This Client has no Policy Group. If you wish to add Policies you must create the group first, otherwise click next to skip</td>
        </tr> 
        <tr>
            <td width="30%">Policy Group Name</td>
            <td width="40%"><%=Html.Encode(Model.PolicyGroupName)%></td>
            <td width="30%"><%= Html.HiddenFor(model => model.PolicyGroupName) %><label id="lblPolicyGroupNameMsg"/></td>
        </tr>                
        <tr>
            <td><label for="EnabledFlag">Enabled?</label></td>
            <td><%= Html.CheckBox("EnabledFlag", true)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledFlag) %></td>
        </tr>  
        <tr>
            <td><label for="EnabledDate">Enabled Date</label></td>
            <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
            <td><%= Html.ValidationMessageFor(model => model.EnabledDate) %></td>
        </tr> 
        <tr>
            <td><label for="ExpiryDate">Expiry Date</label> </td>
            <td> <%= Html.EditorFor(model => model.ExpiryDate) %></td>
            <td><%= Html.ValidationMessageFor(model => model.ExpiryDate) %></td>
        </tr> 
        <tr>
            <td><label for="InheritFromParentFlag">Inherit From Parent?</label></td>
            <td> <%= Html.EditorFor(model => model.InheritFromParentFlag)%></td>
            <td><%= Html.ValidationMessageFor(model => model.InheritFromParentFlag)%></td>
        </tr>
        <tr>
            <td> <%= Html.LabelFor(model => model.TripTypeId) %></td>
            <td><%= Html.DropDownList("TripTypeId", ViewData["TripTypes"] as SelectList, "None")%></td>
            <td><%= Html.ValidationMessageFor(model => model.TripTypeId) %></td>
        </tr> 
         <tr>
            <td></td>
            <td></td>
            <td><span id="clientPolicyGroupCreateButton"><small>Create Policy Group</small></span></td>
        </tr> 
    </table>
    <%= Html.HiddenFor(model => model.HierarchyType)%>
    <%= Html.HiddenFor(model => model.HierarchyItem)%>
    <%= Html.HiddenFor(model => model.HierarchyCode)%>
        <% } %>
        <script type="text/javascript">
$(document).ready(function() {

    //Navigation
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "EnabledDate",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });


    new JsDatePick({
        useMode: 2,
        isStripped: false,
        target: "ExpiryDate",
        dateFormat: "%M %d %Y",
        cellColorScheme: "beige"
    });


    if ($('#ExpiryDate').val() == "") {
        $('#ExpiryDate').val("No Expiry Date")
    }
    if ($('#EnabledDate').val() == "") {
        $('#EnabledDate').val("No Enabled Date")
    }

});







 </script>