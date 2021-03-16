<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectCountryRegionLocations_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Country Region Locations</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="70%" class="row_header_left"><%=  Html.RouteLink("Location", "List", new { controller = "CountryRegion", action = "ListLocations", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "LocationName", id = ViewData["CountryRegionId"] }, new { title = "Sort By Location" })%></th>
			        <th width="15%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                
                foreach (var item in Model) { 
                 
                %>
                <tr>
                    <td><%= Html.Encode(item.LocationName) %></td>
                    <td></td>
                    <td><%= Html.RouteLink("View", "Default", new { controller="Location", action="View", id = item.LocationId }, new { title = "View" })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%= Html.RouteLink("Edit", "Default", new { controller = "Location", action = "Edit", id = item.LocationId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%= Html.RouteLink("Delete", "Default", new { controller = "Location", action = "Delete", id = item.LocationId}, new { title = "Delete" })%>
                        <%} %>
                    </td>                
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %>
                            <%=  Html.RouteLink("<<Previous Page", "List", new { controller = "CountryRegion", action = "ListLocations", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = ViewData["CountryRegionId"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %>
                            <%=  Html.RouteLink("Next Page>>", "List", new { controller = "CountryRegion", action = "ListLocations", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = ViewData["CountryRegionId"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
                 <tr> 
                     <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			         <td class="row_footer_blank_right" colspan="4">
			            <%if (ViewData["Access"] == "WriteAccess"){%>
			            <%= Html.RouteLink("Create Location", "Main", new { controller="Location", action = "Create" }, new { @class = "red", title = "Create Location" })%>
			            <% } %>
			        </td> 
		        </tr> 
        </table>    
        </div>
</div>
<script type="text/javascript">
 $(document).ready(function() {
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Country Regions", new { controller = "CountryRegion", action = "List" }, new { title="Country Regions"})%> &gt;
<%=Html.RouteLink( ViewData["CountryRegionName"].ToString() ,new{controller="CountryRegion", action="View", id = ViewData["CountryRegionId"]})%> &gt;
Locations
</asp:Content>