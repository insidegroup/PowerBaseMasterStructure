<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTravelPorts_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Travel Ports</div></div>
    <div id="content">
         <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
                <th width="24%" class="row_header_left"><%=  Html.RouteLink("Name", "ListMain", new { controller = "TravelPort", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TravelPortName" }, new { title = "Sort by Name" })%></th>
                <th width="6%"><%=  Html.RouteLink("Code", "ListMain", new { controller = "TravelPort", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TravelPortCode" }, new { title = "Sort by Code" })%></th>
                <th width="25%"><%=  Html.RouteLink("Type", "ListMain", new { controller = "TravelPort", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TravelPortTypeDescription" }, new { title = "Sort by Description" })%></th>
                <th width="20%"><%=  Html.RouteLink("Country", "ListMain", new { controller = "TravelPort", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryName" }, new { title = "Sort by Country" })%></th>
		        <th width="8%">&nbsp;</th> 
		        <th width="4%">&nbsp;</th> 
				<th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.TravelportName)%></td>
                <td><%= Html.Encode(item.TravelPortCode)%></td>
                <td><%= Html.Encode(item.TravelPortTypeDescription)%></td>
                <td><%= Html.Encode(item.CountryName)%></td>
                <td><%= Html.RouteLink("Translations", "Main", new { controller = "TravelPortLanguage", action = "List", id = item.TravelPortCode }, new { title = "Translations" })%></td>
                <td><%= Html.RouteLink("View", "Main", new { id = item.TravelPortCode, action = "ViewItem" }, new { title = "View" })%> </td>        
				<td>
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.TravelPortCode }, new { title = "Edit" })%>
					<%} %>
				</td>
				<td>
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.TravelPortCode }, new { title = "Delete" })%>
					<%} %>
				</td>       
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="8" class="row_footer">
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
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		        <td class="row_footer_blank_right" colspan="6">
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.ActionLink("Create Travel Port", "Create", new { }, new { @class = "red", title = "Create Travel Port" })%>
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
$('#btnSearch').click(function () {
    if ($('#filter').val() == "") {
        alert("No Search Text Entered");
        return false;
    }
    $("#frmSearch").attr("action", "/TravelPort.mvc/List");
    $("#frmSearch").submit();

});
 </script>
</asp:Content>
