<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectCountryRegions_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Country Regions</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="37%" class="row_header_left"><%=  Html.RouteLink("Country", "ListMain", new { action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryName" }, new { title = "Sort By Country" })%></th>
                    <th width="40%"><%=  Html.RouteLink("Country Region", "ListMain", new { action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryRegionName" }, new { title = "Sort By Country Region" })%></th>
			        <th width="8%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.CountryName) %></td>
                    <td><%= Html.Encode(item.CountryRegionName) %></td>
                    <td><%= Html.RouteLink("Locations", "List", new { controller = "CountryRegion", action = "ListLocations", id = item.CountryRegionId }, new { title = "View Locations" })%></td>  
                    <td><%= Html.RouteLink("View", "Default", new { id = item.CountryRegionId, action = "View" }, new { title = "View" })%> </td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Edit", "Default", new { id = item.CountryRegionId, action = "Edit" }, new { title="Edit"})%><%} %></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Delete", "Default", new { id = item.CountryRegionId, action = "Delete" }, new { title = "Delete" })%><%} %></td>                
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="6" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %>
                            <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %>
                            <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
                 <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="5">
			        <%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Create Country Region", "Main", new { action = "Create" }, new { @class = "red", title = "Create Country Region" })%>
			        <%} %></td> 
		        </tr> 
        </table>    
     </div>
</div>
<script type="text/javascript">
$(document).ready(function() {
    $('#search').show();
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})



//Search
$('#btnSearch').click(function() {
    if ($('#filter').val() == "") {
        alert("No Search Text Entered");
        return false;
    }
    $("#frmSearch").attr("action", "/CountryRegion.mvc/List");
    $("#frmSearch").submit();

});
 </script>
</asp:Content>
