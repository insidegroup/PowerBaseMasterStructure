<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirCabinGroupItemViewModel>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyAirCabinGroupItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Cabin Group Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr> 
			        <th class="row_header" colspan="3">Edit Policy Air Cabin Group Item</th> 
		        </tr> 
                <tr>
                    <td>Policy Group Name</td>
                    <td colspan="2"><strong><%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyAirCabinGroupItem.PolicyGroupName, 80))%></strong></td>
                </tr>
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_AirlineCabinCode">Airline Cabin Code</label></td>
                    <td><%= Html.DropDownList("PolicyAirCabinGroupItem.AirlineCabinCode", ViewData["AirlineCabinCodeList"] as SelectList, "None")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.AirlineCabinCode)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyAirCabinGroupItem.EnabledFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.EnabledFlag)%></td>
                </tr>  
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirCabinGroupItem.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_ExpiryDate">Expiry Date</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyAirCabinGroupItem.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirCabinGroupItem.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.TravelDateValidFrom)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_TravelDateValidTo">Travel Date Valid To</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyAirCabinGroupItem.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.TravelDateValidTo)%></td>
                </tr> 
                 <tr>
                    <td><label for="PolicyAirCabinGroupItem_FlightDurationAllowedMin">Min Allowed Flight Duration</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMin, new { maxlength = "9" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMin)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_FlightDurationAllowedMax">Max Allowed Flight Duration</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMax, new { maxlength = "9" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMax)%></td>
                </tr>  
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_FlightMileageAllowedMin">Mileage Minimum</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMin, new { maxlength = "5" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMin)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_FlightMileageAllowedMax">Mileage Maximum</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMax, new { maxlength = "5" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMax)%></td>
                </tr>  
                <tr>
                    <td><label for="PolicyAirCabinGroupItem_PolicyProhibitedFlag">Policy Prohibited?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyAirCabinGroupItem.PolicyProhibitedFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.PolicyProhibitedFlag)%></td>
                </tr> 
                 <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="30%" class="row_footer_centre"></td>
                    <td width="40%" class="row_footer_right"></td>
                </tr>
                 <tr>
                    <td class="row_footer_blank" colspan="3"></td>
                </tr>
                <tr> 
			        <th class="row_header" colspan="3">Policy Routing</th> 
		        </tr> 
		        <tr>
                    <td>Policy Routing Name</td>
                    <td><label id="lblAuto"><%=Html.Encode(Model.PolicyRouting.Name) %></label></td>
                    <td><%= Html.HiddenFor(model => model.PolicyRouting.Name)%><label id="lblPolicyRoutingNameMsg"/></td>
                </tr>   
               <tr>
                    <td><label for="PolicyRouting_Name">From Global?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_FromCode">From</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyRouting.FromCode)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromCode)%><label id="lblFrom"/></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_ToGlobalFlag">To Global?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_ToCode">To</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyRouting.ToCode)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToCode)%><label id="lblTo"/></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_RoutingViceVersaFlag">Routing ViceVersa?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                </tr> 
             <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="30%" class="row_footer_centre"></td>
                    <td width="40%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Policy AirCabin Group Item" title="Edit PolicyAirCabinGroupItem" class="red"/></td>
                </tr>
            </table>
           <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.PolicyGroupId)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.PolicyGroupName)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.VersionNumber)%>
    <% } %>
    </div>
</div>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Cabin Group Items", "Default", new { controller = "PolicyAirCabinGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Cabin Group Items" })%> &gt;
Item
</asp:Content>