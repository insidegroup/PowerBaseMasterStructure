<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ServicingOptionVM>" %>

<script type="text/javascript">
$(function(){
	$('#ServicingOptionItemInstruction').NobleCount('#countBO', {on_negative: function(){$('#countBO').css('color','red')}, on_positive: function(){$('#countBO').css('color','black')}, max_chars:100});
	$('#ServicingOptionItemValue').NobleCount('#countBO2', {on_negative: function(){$('#countBO2').css('color','red')},on_positive: function(){$('#countBO2').css('color','black')}, max_chars:50});
});

$(document).ready(function () {

    $('#hiddenAddedServicingOptionsTable tr').each(function () {
        if ($(this).attr("serviceItemID") == 180) {
        	$("#ServicingOptionId option[value='180']").remove();
        }
        if ($(this).attr("serviceItemID") == 181) {
        	$("#ServicingOptionId option[value='181']").remove();
        }
    })

    LoadServicingOptionValues();
})
</script>
<table>
    <tr>
        <td><label for="ServicingOptionId">Service Option</label></td>
        <td><%= Html.DropDownList("ServicingOptionId", ViewData["ServicingOptions"] as SelectList, "Please Select...", new { onchange = "LoadServicingOptionValues()"})%><span class="error"> *</span></td>
        <td></td>
    </tr>
    <tr style="display:none" id="trSelectListServicingOptionItemValue">
        <td><label for="selServicingOptionItemValue">Value</label></td>
        <td><select id="selServicingOptionItemValue"></select><span class="error"> *</span></td>
        <td></td>
    </tr>
    <tr id="trTextboxServicingOptionItemValue">
        <td><label for="txtServicingOptionItemValue">Value</label></td>
        <td><%= Html.TextBoxFor(model => model.ServicingOptionItem.ServicingOptionItemValue, new { style = "width:175px;", maxlength="50"})%><span class="error"> *</span></td>
        <td style="font-size:9px" id="countBO2"></td> 
    </tr>
	<tr style="display:none" class="parameter-fields">
        <td><label for="DepartureTimeWindowMinutes">Departure Time Window</label></td>
        <td><%= Html.DropDownListFor(model => model.ServicingOptionItem.DepartureTimeWindowMinutes, ViewData["DepartureTimeWindowMinutesList"] as SelectList, "Please Select...")%></td>
        <td></td>
    </tr> 
	<tr style="display:none" class="parameter-fields">
        <td><label for="ArrivalTimeWindowMinutes">Arrival Time Window</label></td>
        <td><%= Html.DropDownListFor(model => model.ServicingOptionItem.ArrivalTimeWindowMinutes, ViewData["ArrivalTimeWindowMinutesList"] as SelectList, "Please Select...")%></td>
        <td></td>
    </tr> 
	<tr style="display:none" class="parameter-fields">
        <td><label for="MaximumStops">Max Stops</label></td>
        <td><%= Html.DropDownListFor(model => model.ServicingOptionItem.MaximumStops, ViewData["MaximumStopsList"] as SelectList, "Please Select...")%></td>
        <td></td>
    </tr> 
	<tr style="display:none" class="parameter-fields">
        <td><label for="MaximumConnectionTimeMinutes">Max Connection Time</label></td>
        <td><%= Html.TextBoxFor(model => model.ServicingOptionItem.MaximumConnectionTimeMinutes)%></td>
        <td></td>
    </tr>
    <tr style="display:none" class="parameter-fields">
        <td><label for="UseAlternateAirportFlag">Use Alternate Airport?</label></td>
        <td><%= Html.CheckBox("UseAlternateAirportFlag", Model.ServicingOptionItem.UseAlternateAirportFlag ?? false)%></td>
        <td></td>
    </tr>
	<tr style="display:none" class="parameter-fields">
        <td><label for="NoPenaltyFlag">No Penalty?</label></td>
        <td><%= Html.CheckBox("NoPenaltyFlag", Model.ServicingOptionItem.NoPenaltyFlag ?? false)%></td>
        <td></td>
    </tr>
	<tr style="display:none" class="parameter-fields">
        <td><label for="NoRestrictionsFlag">No Restrictions?</label></td>
        <td><%= Html.CheckBox("NoRestrictionsFlag", Model.ServicingOptionItem.NoRestrictionsFlag ?? false)%></td>
        <td></td>
    </tr>
    <tr style="display:none" id="trGDSs">
        <td><label for="GDSCode">GDS</label></td>
        <td><%= Html.DropDownListFor(model => model.ServicingOptionItem.GDSCode, ViewData["GDSs"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
        <td></td>
    </tr>
    <tr>
        <td><label for="ServicingOptionItemInstruction">Instruction to counsellor</label></td>
        <td><%= Html.TextBoxFor(model => model.ServicingOptionItem.ServicingOptionItemInstruction, new { style = "width:350px;", maxlength = "100" })%><span class="error"> *</span></td>
        <td></td>
    </tr>
    <tr>
	    <td></td><td style="font-size:9px"><span id="countBO"></span>&nbsp; characters remaining</td>
        <td></td>
    </tr>
</table>
<%= Html.HiddenFor(model => model.ServicingOptionItem.VersionNumber)%>
<input type="hidden" id="ServicingOptionItemValue" />
