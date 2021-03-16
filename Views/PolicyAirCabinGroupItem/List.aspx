<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyAirCabinGroupItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Cabin Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                 <tr> 
                    <th class="row_header_left"></th> 
                    <th></th> 
                    <th colspan="2">Flight Duration</th>
                    <th colspan="2">Flight Mileage</th>
                    <th></th>
                    <th></th>
                    <th></th> 
                    <th></th> 
                    <th class="row_header_right"></th> 
                  </tr> 
                  <tr> 
                    <th width="26%"><%=  Html.RouteLink("Routing", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "Name", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Routing" })%></th>
                    <th width="15%"><%=  Html.RouteLink("Airline Cabin", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "AirlineCabinDefaultDescription", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Routing" })%></th>
                    <th width="7%"><%=  Html.RouteLink("Min", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "FlightDurationAllowedMin", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Routing" })%></th>
                    <th width="7%"><%=  Html.RouteLink("Max", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "FlightDurationAllowedMax", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Routing" })%></th>
                    <th width="7%"><%=  Html.RouteLink("Min", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "FlightMileageAllowedMin", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Routing" })%></th>
                    <th width="7%"><%=  Html.RouteLink("Max", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "FlightMileageAllowedMax", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Routing" })%></th>
                    <th width="10%"><%=  Html.RouteLink("Advice", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Advice" })%></th>
                    <th width="6%">&nbsp;</th> 
                    <th width="4%">&nbsp;</th> 
                    <th width="4%">&nbsp;</th> 
                    <th width="7%">&nbsp;</th> 
                  </tr> 
               <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.Name)%></td>
                    <td><%= Html.Encode(item.AirlineCabinDefaultDescription)%></td>
                    <td><%= Html.Encode(item.FlightDurationAllowedMin)%></td>
                    <td><%= Html.Encode(item.FlightDurationAllowedMax)%></td>
                    <td><%= Html.Encode(item.FlightMileageAllowedMin)%></td>
                    <td><%= Html.Encode(item.FlightMileageAllowedMax)%></td>                   
                    <td><%= Html.Encode(item.Advice > 0 ? "Yes" : "No")%></td>
                    <td><%= Html.RouteLink("Advice", "Default", new { controller="AirCabinAdvice", action="List", id = item.PolicyAirCabinGroupItemId }, new { title = "Air Cabin Advice" })%></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.PolicyAirCabinGroupItemId })%></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit","Default", new { action="Edit", id = item.PolicyAirCabinGroupItemId })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PolicyAirCabinGroupItemId })%>
                        <%} %>
                    </td>             
                </tr>
                <% 
                } 
                %>
                <tr>
                <td colspan="11" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new {  id = ViewData["PolicyGroupID"].ToString(), page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new {  id = ViewData["PolicyGroupID"].ToString(), page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
                </tr> 
		        <tr> 
                 <td colspan="3" class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="8">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.ActionLink("Create Policy Air Cabin Group Item", "Create", new { id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Create Policy Air Cabin Group Item" })%>
			        <% } %> 
			        </td> 
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
<%=Html.RouteLink(ViewData["PolicyGroupName"].ToString(), "Default", new { controller = "PolicyGroup", action = "View", id = ViewData["PolicyGroupId"] }, new { title = ViewData["PolicyGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
Policy Air Cabin Group Items
</asp:Content>