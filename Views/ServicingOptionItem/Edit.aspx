<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ServicingOptionItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Servicing Option Groups
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Servicing Option Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Servicing Option Item</th> 
		        </tr>  
                <tr>
                    <td><label for="ServicingOptionId">Servicing Option Item</label></td>
                    <td><%= Html.DropDownList("ServicingOptionId", ViewData["ServicingOptions"] as SelectList, "Please Select...", new { onchange = "LoadServicingOptionValues()" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServicingOptionId)%></td>
                </tr> 
                <tr style="display:none" id="trSelectListServicingOptionItemValue">
                    <td><label for="selServicingOptionItemValue">Value</label></td>
                    <td><select id="selServicingOptionItemValue"></select><span class="error"> *</span></td>
                    <td><label id="selServicingOptionItemValidation"></label></td>
                </tr>
                <tr id="trTextboxServicingOptionItemValue">
                    <td><label for="txtServicingOptionItemValue">Value</label></td>
                    <td><%= Html.TextBox("txtServicingOptionItemValue", "", new { disabled = "disabled", style = "width:175px;", maxlength="50"})%><span class="error"> *</span></td>
                    <td><label id="txtServicingOptionItemValidation"></label></td>
                </tr>
				<tr style="display:none" class="parameter-fields">
                    <td><label for="DepartureTimeWindowMinutes">Departure Time Window</label></td>
                    <td><%= Html.DropDownList("DepartureTimeWindowMinutes", ViewData["DepartureTimeWindowMinutesList"] as SelectList, "Please Select...")%></td>
                    <td><label id="DepartureTimeWindowMinutesValidation"></label></td>
                </tr> 
				<tr style="display:none" class="parameter-fields">
                    <td><label for="ArrivalTimeWindowMinutes">Arrival Time Window</label></td>
                    <td><%= Html.DropDownList("ArrivalTimeWindowMinutes", ViewData["ArrivalTimeWindowMinutesList"] as SelectList, "Please Select...")%></td>
                    <td><label id="ArrivalTimeWindowMinutesValidation"></label></td>
                </tr> 
				<tr style="display:none" class="parameter-fields">
                    <td><label for="MaximumStops">Max Stops</label></td>
                    <td><%= Html.DropDownList("MaximumStops", ViewData["MaximumStopsList"] as SelectList, "Please Select...")%></td>
                    <td><label id="MaximumStopsValidation"></label></td>
                </tr> 
				<tr style="display:none" class="parameter-fields">
                    <td><label for="MaximumConnectionTimeMinutes">Max Connection Time</label></td>
                    <td><%= Html.TextBoxFor(model => model.MaximumConnectionTimeMinutes)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.MaximumConnectionTimeMinutes)%></td>
                </tr>
                <tr style="display:none" class="parameter-fields">
                    <td><label for="UseAlternateAirportFlag">Use Alternate Airport?</label></td>
                    <td><%= Html.CheckBox("UseAlternateAirportFlag", Model.UseAlternateAirportFlag ?? false)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.UseAlternateAirportFlag)%></td>
                </tr>
				<tr style="display:none" class="parameter-fields">
                    <td><label for="NoPenaltyFlag">No Penalty?</label></td>
                    <td><%= Html.CheckBox("NoPenaltyFlag", Model.NoPenaltyFlag ?? false)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.NoPenaltyFlag)%></td>
                </tr>
				<tr style="display:none" class="parameter-fields">
                    <td><label for="NoRestrictionsFlag">No Restrictions?</label></td>
                    <td><%= Html.CheckBox("NoRestrictionsFlag", Model.NoRestrictionsFlag ?? false)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.NoRestrictionsFlag)%></td>
                </tr>
                <tr style="display:none" id="trGDSs">
                    <td><label for="GDSCode">GDS</label></td>
                    <td><%= Html.DropDownList("GDSCode", ViewData["GDSs"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><label id="GDSCodeValidation"></label></td>
                </tr> 
                <tr>
                    <td><label for="ServicingOptionItemInstruction">Instruction</label></td>
                    <td><%= Html.TextBoxFor(model => model.ServicingOptionItemInstruction, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServicingOptionItemInstruction)%></td>
                </tr>
                 <tr>
                    <td><label for="DisplayInApplicationFlag">Display In Application</label></td>
                    <td><%= Html.CheckBox("DisplayInApplicationFlag", Model.DisplayInApplicationFlag ?? false)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.DisplayInApplicationFlag)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="50%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td> 
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Servicing Option Item" title="Edit Servicing Option Item" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.ServicingOptionItemValue) %>
			<%= Html.HiddenFor(model => model.ServicingOptionGroupId) %>
			<%= Html.HiddenFor(model => model.VersionNumber) %>
			<input type="hidden" value="false" id="ServicingOptionGDSRequired" />
    <% } %>

    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ServicingOptionItem.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Servicing Option Groups", "Main", new { controller = "ServicingOptionGroup", action = "ListUnDeleted", }, new { title = "Servicing Option Groups" })%> &gt;
<%=Html.RouteLink(Model.ServicingOptionGroupName, "Default", new { controller = "ServicingOptionGroup", action = "View", id = Model.ServicingOptionGroupId }, new { title = Model.ServicingOptionGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ServicingOptionItem", action = "List", id = Model.ServicingOptionGroupId }, new { title = "Servicing Option Items" })%> &gt;
<%= Html.Encode(Model.ServicingOptionName)%>
</asp:Content>