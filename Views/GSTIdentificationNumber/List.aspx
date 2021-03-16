<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectGSTIdentificationNumbers_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GST Identification Numbers</div></div>
    <div id="content">
		<div class="home_button">
			<a href="/" class="red">Home</a>
		</div>
         <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
                <th width="20%" class="row_header_left"><%=  Html.RouteLink("Client TopUnit Name", "ListMain", new { controller = "GSTIdentificationNumber", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ClientTopUnitName" }, new { title = "Sort by Client TopUnit Name" })%></th>
                <th width="14%"><%= Html.RouteLink("Client Entity Name", "ListMain", new { controller = "GSTIdentificationNumber", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GSTIdentificationNumber" }, new { title = "Sort by Client Entity Name" })%></th>
                <th width="18%"><%= Html.RouteLink("GST Identification Number", "ListMain", new { controller = "GSTIdentificationNumber", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GSTIdentificationNumber" }, new { title = "Sort by GST Identification Number" })%></th>
                <th width="18%"><%= Html.RouteLink("First Address Line", "ListMain", new { controller = "GSTIdentificationNumber", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "FirstAddressLine" }, new { title = "Sort by First Address Line" })%></th>
                <th width="10%"><%= Html.RouteLink("City", "ListMain", new { controller = "GSTIdentificationNumber", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CityName" }, new { title = "Sort by City" })%></th>
                <th width="10%"><%= Html.RouteLink("Country", "ListMain", new { controller = "GSTIdentificationNumber", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryName" }, new { title = "Sort by Country" })%></th>
		        <th width="4%">&nbsp;</th> 
				<th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td class="wrap-text"><%= Html.Encode(item.ClientTopUnitName)%></td>
                <td class="wrap-text"><%= Html.Encode(item.ClientEntityName)%></td>
                <td class="wrap-text"><%= Html.Encode(item.GSTIdentificationNumber)%></td>
                <td class="wrap-text"><%= Html.Encode(item.FirstAddressLine)%></td>
                <td class="wrap-text"><%= Html.Encode(item.CityName)%></td>
                <td class="wrap-text"><%= Html.Encode(item.CountryName)%></td>
                <td><%= Html.RouteLink("View", "Main", new { id = item.GSTIdentificationNumberId, action = "ViewItem" }, new { title = "View" })%> </td>
				<td>
					<%if (item.HasWriteAccess.Value){ %>
						<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.GSTIdentificationNumberId }, new { title = "Edit" })%>
					<%} %>
				</td>
				<td>
					<%if (item.HasWriteAccess.Value){ %>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.GSTIdentificationNumberId }, new { title = "Delete" })%>
					<%} %>
				</td>       
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="9" class="row_footer">
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
                <td class="row_footer_blank_left" colspan="2">
                    <a href="javascript:history.back();" class="red" title="Back">Back</a> 
                    <a href="javascript:window.print();" class="red" title="Print">Print</a>
                </td>
		        <td class="row_footer_blank_right" colspan="7">
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.ActionLink("Create GST Identification Number", "Create", new { }, new { @class = "red", title = "Create GST Identification Number" })%>
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
    $('.full-width #search_wrapper').css('height', 'auto');
})
//Search
$('#btnSearch').click(function () {
    if ($('#filter').val() == "") {
        alert("No Search Text Entered");
        return false;
    }
    $("#frmSearch").attr("action", "/GSTIdentificationNumber.mvc/List");
    $("#frmSearch").submit();

});
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
GST Identification Numbers
</asp:Content>