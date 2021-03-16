<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientTelephonies_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Telephony</div></div>
    <div id="content">
		<div class="home_button">
			<a href="/" class="red">Home</a>
		</div>
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
                <th width="10%" class="row_header_left"><%=  Html.RouteLink("Phone Number", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PhoneNumber" }, new { title = "Phone Number" })%></th>
                <th width="12%"><%=  Html.RouteLink("Country", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryName" }, new { title = "Country" })%></th>
                <th width="5%"><%=  Html.RouteLink("Prefix", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "InternationalPrefixCode" }, new { title = "Prefix" })%></th>
                <th width="10%"><%=  Html.RouteLink("Hierarchy", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "HierarchyType" }, new { title = "Hierarchy" })%></th>
				<th width="13%"><%=  Html.RouteLink("Hierarchy Item", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "HierarchyName" }, new { title = "Hierarchy Item" })%></th>
                <th width="7%"><%=  Html.RouteLink("GUID", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "HierarchyItem" }, new { title = "GUID" })%></th>
                <th width="7%"><%=  Html.RouteLink("Main/Alt", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "MainNumberFlag" }, new { title = "Main/Alt" })%></th>
                <th width="12%"><%=  Html.RouteLink("Type", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TelephonyTypeDescription" }, new { title = "Type" })%></th>
                <th width="10%"><%=  Html.RouteLink("Expiry Date", "ListMain", new { controller = "ClientTelephony", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ExpiryDate" }, new { title = "Expiry Date" })%></th>
		        <th width="4%">&nbsp;</th> 
				<th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.PhoneNumber)%></td>
                <td><%= Html.Encode(item.CountryName)%></td>
                <td><%= Html.Encode(item.InternationalPrefixCode != null ? item.InternationalPrefixCode.Replace("+", "") : "")%></td>
                <td><%= Html.Encode(item.HierarchyType)%></td>
                <td><%= Html.Encode(item.HierarchyName)%></td>
				<td><%= Html.Encode(item.HierarchyItem)%></td>
                <td><%= Html.Encode(item.MainNumberFlag)%></td>
				<td><%= Html.Encode(item.TelephonyTypeDescription)%></td>
                <td><%= Html.Encode(item.ExpiryDate != null ? item.ExpiryDate.Value.ToString("MMM dd, yyyy") : "")%></td>
                <td><%= Html.RouteLink("View", "Main", new { id = item.ClientTelephonyId, action = "ViewItem" }, new { title = "View" })%> </td>        
				<td>
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.ClientTelephonyId }, new { title = "Edit" })%>
					<%} %>
				</td>
				<td>
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.ClientTelephonyId }, new { title = "Delete" })%>
					<%} %>
				</td>       
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="12" class="row_footer">
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
                <td class="row_footer_blank_left" colspan="3"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		        <td class="row_footer_blank_right" colspan="9">
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.ActionLink("Create Client Telephony", "Create", new { }, new { @class = "red", title = "Create Client Telephony" })%>
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
    $("#frmSearch").attr("action", "/ClientTelephony.mvc/List");
    $("#frmSearch").submit();

});
 </script>
</asp:Content>
