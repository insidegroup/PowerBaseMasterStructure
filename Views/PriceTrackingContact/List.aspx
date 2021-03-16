<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPriceTrackingContacts_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Contacts</div></div>
    <div id="content">
		<div class="home_button">
			<a href="/" class="red">Home</a>
		</div>
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
                <th width="15%" class="row_header_left"><%=  Html.RouteLink("Contact Type", "ListMain", new { controller = "PriceTrackingContact", id = ViewData["PriceTrackingSetupGroupId"].ToString(), action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ContactTypeName" }, new { title = "Contact Type" })%></th>
                <th width="12%"><%= Html.RouteLink("Last Name", "ListMain", new { controller = "PriceTrackingContact", id = ViewData["PriceTrackingSetupGroupId"].ToString(), action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "LastName" }, new { title = "Last Name" })%></th>
                <th width="12%"><%= Html.RouteLink("First Name", "ListMain", new { controller = "PriceTrackingContact", id = ViewData["PriceTrackingSetupGroupId"].ToString(), action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "FirstName" }, new { title = "First Name" })%></th>
                <th width="20%">Email</th>
                <th width="10%"><%= Html.RouteLink("User Type", "ListMain", new { controller = "PriceTrackingContact", id = ViewData["PriceTrackingSetupGroupId"].ToString(), action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PriceTrackingContactUserTypeName" }, new { title = "User Type" })%></th>
                <th width="10%"><%= Html.RouteLink("Dashboard?", "ListMain", new { controller = "PriceTrackingContact", id = ViewData["PriceTrackingSetupGroupId"].ToString(), action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PriceTrackingDashboardAccessTypeName" }, new { title = "Dashboard?" })%></th>
                <th width="10%"><%= Html.RouteLink("Email Alerts?", "ListMain", new { controller = "PriceTrackingContact", id = ViewData["PriceTrackingSetupGroupId"].ToString(), action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PriceTrackingEmailAlertTypeName" }, new { title = "Email Alerts?" })%></th>
				<th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.ContactTypeName)%></td>
                <td class="wrap-text"><%= Html.Encode(item.LastName)%></td>
                <td class="wrap-text"><%= Html.Encode(item.FirstName)%></td>
				<td class="wrap-text"><%= Html.Encode(item.EmailAddress)%></td>
                <td><%= Html.Encode(item.PriceTrackingContactUserTypeName)%></td>
                <td><%= Html.Encode(item.PriceTrackingDashboardAccessFlag.HasValue? item.PriceTrackingDashboardAccessFlag.Value.ToString() : "")%></td>
                <td><%= Html.Encode(item.PriceTrackingEmailAlertTypeName)%></td>
				<td>
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.PriceTrackingContactId }, new { title = "Edit" })%>
					<%} %>
				</td>
				<td>
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.PriceTrackingContactId }, new { title = "Delete" })%>
					<%} %>
				</td>       
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="9" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id =  ViewData["PriceTrackingSetupGroupId"].ToString() }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id =  ViewData["PriceTrackingSetupGroupId"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>   
            <tr> 
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		        <td class="row_footer_blank_right" colspan="7">
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.ActionLink("Create", "Create", new { id = ViewData["PriceTrackingSetupGroupId"].ToString() }, new { @class = "red", title = "Create" })%>
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
	$('#breadcrumb').css('width', 'auto');
	$('.full-width #search_wrapper').css('height', '22px');
    $("#frmSearch input[name='ft']").attr('name', 'id').val(<%=ViewData["PriceTrackingSetupGroupId"]%>);
})

//Search
$('#btnSearch').click(function () {
	if ($('#filter').val() == "") {
		alert("No Search Text Entered");
		return false;
	}
	$("#frmSearch").attr("action", "/PriceTrackingContact.mvc/List");
	$("#frmSearch").submit();

});
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Setups", "Main", new { controller = "PriceTrackingSetupGroup", action = "ListUnDeleted" }, new { title = "Price Tracking Setups" })%> &gt;
<%=Html.RouteLink(ViewData["PriceTrackingSetupGroupName"].ToString(), "Main", new { controller = "PriceTrackingSetupGroup", action = "View", id = ViewData["PriceTrackingSetupGroupId"].ToString()}, new { title = ViewData["PriceTrackingSetupGroupName"].ToString() })%> &gt;
Price Tracking Contacts
</asp:Content>