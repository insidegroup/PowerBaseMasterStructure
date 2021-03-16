<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectGDSContacts_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Contact</div></div>
    <div id="content">
		<div class="home_button">
			<a href="/" class="red">Home</a>
		</div>
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
                <th width="10%" class="row_header_left"><%=  Html.RouteLink("GDS", "ListMain", new { controller = "GDSContact", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSName" }, new { title = "GDS" })%></th>
                <th width="10%"><%= Html.RouteLink("Country", "ListMain", new { controller = "GDSContact", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryName" }, new { title = "Country" })%></th>
                <th width="15%"><%= Html.RouteLink("PCC/OID Business", "ListMain", new { controller = "GDSContact", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PseudoCityOrOfficeBusinessName" }, new { title = "PCC/OID Business" })%></th>
                <th width="15%">GDS Request Type(s)</th>
                <th width="10%"><%= Html.RouteLink("Last Name", "ListMain", new { controller = "GDSContact", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "LastName" }, new { title = "Last Name" })%></th>
                <th width="10%"><%= Html.RouteLink("First Name", "ListMain", new { controller = "GDSContact", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "FirstName" }, new { title = "First Name" })%></th>
                <th width="15%"><%= Html.RouteLink("Email", "ListMain", new { controller = "GDSContact", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "EmailAddress" }, new { title = "Email" })%></th>
		        <th width="4%">&nbsp;</th> 
				<th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.GDSName)%></td>
                <td><%= Html.Encode(item.CountryName)%></td>
                <td><%= Html.Encode(item.PseudoCityOrOfficeBusinessName)%></td>
                <td><%= Html.Encode(item.GDSRequestTypes)%></td>
				<td class="wrap-text"><%= Html.Encode(item.LastName)%></td>
                <td class="wrap-text"><%= Html.Encode(item.FirstName)%></td>
				<td class="wrap-text"><%= Html.Encode(item.EmailAddress)%></td>
                <td><%= Html.RouteLink("View", "Main", new { id = item.GDSContactId, action = "ViewItem" }, new { title = "View" })%> </td>        
				<td>
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.GDSContactId }, new { title = "Edit" })%>
					<%} %>
				</td>
				<td>
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.GDSContactId }, new { title = "Delete" })%>
					<%} %>
				</td>       
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="10" class="row_footer">
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
		        <td class="row_footer_blank_right" colspan="8">
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.ActionLink("Create GDS Contact", "Create", new { }, new { @class = "red", title = "Create GDS Contact" })%>
					<% } %>
		        </td> 
	        </tr> 
        </table>    
    </div>
</div>
<script type="text/javascript">
$(document).ready(function() {
    $('#search').show();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})

//Search
$('#btnSearch').click(function () {
	if ($('#filter').val() == "") {
		alert("No Search Text Entered");
		return false;
	}
	$("#frmSearch").attr("action", "/GDSContact.mvc/List");
	$("#frmSearch").submit();

});
 </script>
</asp:Content>
