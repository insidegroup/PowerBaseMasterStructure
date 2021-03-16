<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirCabinGroupItemViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Cabin Group Item</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
	        <tr> 
		        <th class="row_header" colspan="3">View Policy Air Cabin Group Item</th> 
	        </tr> 
            <tr>
                <td>Airline Cabin Code</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.AirlineCabinCode)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Enabled?</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.EnabledFlag)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.EnabledDate.HasValue ? Model.PolicyAirCabinGroupItem.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.ExpiryDate.HasValue ? Model.PolicyAirCabinGroupItem.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Travel Date Valid From</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.TravelDateValidFrom.HasValue ? Model.PolicyAirCabinGroupItem.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "No Travel From Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Travel Date Valid To</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.TravelDateValidFrom.HasValue ? Model.PolicyAirCabinGroupItem.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "No Travel From Date")%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Min Allowed Flight Duration</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightDurationAllowedMin)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Max Allowed Flight Duration</td>
                <td> <%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightDurationAllowedMax)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Mileage Minimum</td>
                <td> <%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightMileageAllowedMin)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Mileage Maximum</td>
                <td> <%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightMileageAllowedMax)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Policy Prohibited?</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.PolicyProhibitedFlag)%></td>
                <td></td>
            </tr> 
             <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
             <tr>
                <td class="row_footer_blank" colspan="3"></td>
            </tr>
            <tr> 
		        <th class="row_header" colspan="3">PolicyRouting</th> 
	        </tr> 
            <tr>
                <td><label for="PolicyRouting_Name">Name</label></td>
                <td><%= Html.Encode(Model.PolicyRouting.Name)%></td>
                <td></td>
            </tr> 
           <tr>
                <td><label for="PolicyRouting_Name">From Global?</label></td>
                <td><%= Html.Encode(Model.PolicyRouting.FromGlobalFlag)%></td>
                <td></td>
            </tr> 
           <tr>
                <td><label for="PolicyRouting_FromCode">From</label></td>
                <td> <%= Html.Encode(Model.PolicyRouting.FromCode)%></td>
                <td>(<%= Html.Encode(Model.PolicyRouting.FromName)%>)</td>
            </tr> 
           <tr>
                <td><label for="PolicyRouting_ToGlobalFlag">To Global?</label></td>
                <td><%= Html.Encode(Model.PolicyRouting.ToGlobalFlag)%></td>
                <td></td>
            </tr> 
           <tr>
                <td><label for="PolicyRouting_ToCode">To</label></td>
                <td> <%= Html.Encode(Model.PolicyRouting.ToCode)%></td>
                <td>(<%= Html.Encode(Model.PolicyRouting.ToName)%>)</td>
            </tr> 
           <tr>
                <td><label for="PolicyRouting_RoutingViceVersaFlag">Routing ViceVersa?</label></td>
                <td><%= Html.Encode(Model.PolicyRouting.RoutingViceVersaFlag)%></td>
                <td></td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
                </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');
})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Cabin Group Items", "Default", new { controller = "PolicyAirCabinGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Cabin Group Items" })%> &gt;
Item
</asp:Content>