<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectAPISCountries_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">APIS Countries</div></div>
    <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
                <th width="26%" class="row_header_left"><%=  Html.RouteLink("Origin", "ListMain", new { controller = "APISCountry", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "OriginCountryName" }, new { title = "Sort By Origin Country" })%></th>
                <th width="26%"><%=  Html.RouteLink("Destination", "ListMain", new { controller = "APISCountry", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "DestinationCountryName" }, new { title = "Sort By Destination Country" })%></th>
                <th width="37%"><%=  Html.RouteLink("Start Date", "ListMain", new { controller = "APISCountry", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "StartDate" }, new { title = "Sort By Start Date" })%></th>
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
		    </tr> 
            <%
                
            foreach (var item in Model) { 
                  
            %>
            <tr>
                <td><%= Html.Encode(item.OriginCountryName) %></td>
                <td><%= Html.Encode(item.DestinationCountryName) %></td>
                <td><%= Html.Encode(item.StartDate.ToString("MMM dd, yyyy"))%></td>     
                <td<%if (ViewData["Access"] == "WriteAccess"){%>><%= Html.ActionLink("Edit", "Edit", new {occ = item.OriginCountryCode, dcc = item.DestinationCountryCode}, new { title = "Edit APIS Country" })%><%}%></td>
                <td><%if (ViewData["Access"] == "WriteAccess"){%><%= Html.ActionLink("Delete", "Delete", new { occ = item.OriginCountryCode, dcc = item.DestinationCountryCode }, new { title = "Delete APIS Country" })%><%}%></td>                
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="5" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                            <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "APISCountry", action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                            <% if (Model.HasNextPage){ %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "APISCountry", action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
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
                <%= Html.RouteLink("Create APIS Country", "Main", new { action = "Create" }, new { @class = "red", title = "Create APIS Country" })%>
			    <%}%></td> 
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
    $("#frmSearch").attr("action", "/APISCountry.mvc/List");
    $("#frmSearch").submit();

});
 </script>
</asp:Content>
