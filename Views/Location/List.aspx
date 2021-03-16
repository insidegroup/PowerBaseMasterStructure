<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectLocations_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Locations</div></div>
    <div id="content">
         <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
                <th width="33%" class="row_header_left"><%=  Html.RouteLink("Location", "ListMain", new { controller = "Location", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "LocationName" }, new { title = "Sort By Location" })%></th>
                <th width="25%"><%=  Html.RouteLink("CountryRegion", "ListMain", new { controller = "Location", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryRegionName" }, new { title = "Sort By CountryRegion" })%></th>
                <th width="25%"><%=  Html.RouteLink("Country", "ListMain", new { controller = "Location", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryName" }, new { title = "Sort By Country" })%></th>
		        <th width="5%">&nbsp;</th> 
		        <th width="5%">&nbsp;</th> 
		        <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.LocationName) %></td>
                <td><%= Html.Encode(item.CountryRegionName) %></td>
                <td><%= Html.Encode(item.CountryName) %></td>
                <td><%= Html.RouteLink("View", "Default", new {  controller = "Location", action = "View", id = item.LocationId }, new { title = "View" })%> </td>
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
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %>
                            <%=  Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %>
                            <%=  Html.RouteLink("Next Page>>", "ListMain", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
             <tr> 
                 <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		        <td class="row_footer_blank_right" colspan="5">
		        <%if (ViewData["Access"] == "WriteAccess"){%>
		        <%= Html.RouteLink("Create Location", "Main", new { action = "Create" }, new { @class = "red", title = "Create Location" })%>
		        <% } %>
		        </td> 
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
    $("#frmSearch").attr("action", "/Location.mvc/List");
    $("#frmSearch").submit();

});
 </script>
</asp:Content>