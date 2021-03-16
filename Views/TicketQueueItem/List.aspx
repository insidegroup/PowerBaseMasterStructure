<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTicketQueueGroupTicketQueueItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Ticket Queue Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="20%" class="row_header_left"><%= Html.RouteLink("Description", "List", new { page = 1, sortField = "TicketQueueItemDescription", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%></th> 
			        <th width="10%"><%= Html.RouteLink("PCC/OfficeID", "List", new { page = 1, sortField = "PseudoCityOrOfficeId", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%></th> 
			        <th width="5%"><%= Html.RouteLink("GDS", "List", new { page = 1, sortField = "GDSCode", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%></th>
					<th width="10%"><%= Html.RouteLink("QNumber", "List", new { page = 1, sortField = "QueueNumber", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%></th>
					<th width="10%"><%= Html.RouteLink("QCategory", "List", new { page = 1, sortField = "QueueCategory", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%></th>
					<th width="10%"><%= Html.RouteLink("Ticket Type", "List", new { page = 1, sortField = "TicketTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%></th>
					<th width="10%"><%= Html.RouteLink("Tkt Remark", "List", new { page = 1, sortField = "TicketingFieldRemark", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%></th>
					<th width="10%"><%= Html.RouteLink("Tkt Command", "List", new { page = 1, sortField = "TicketingCommand", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%></th>
					<th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.TicketQueueItemDescription) %></td>
                    <td><%= Html.Encode(item.PseudoCityOrOfficeId) %></td>
					<td><%= Html.Encode(item.GDSCode) %></td>
					<td><%= Html.Encode(item.QueueNumber) %></td>
					<td><%= Html.Encode(item.QueueCategory) %></td>
					<td><%= Html.Encode(item.TicketTypeDescription) %></td>
					<td><%= Html.Encode((item.TicketingFieldRemark != null && item.TicketingFieldRemark.Length > 20) ? string.Format("{0}...", item.TicketingFieldRemark.Substring(0, 17)) : item.TicketingFieldRemark) %></td>
					<td class="wrap-text"><%= Html.Encode(item.TicketingCommand) %></td>
					<td><%= Html.ActionLink("View", "View", new { id = item.TicketQueueItemId })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.TicketQueueItemId })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.TicketQueueItemId })%>
                        <%} %>
                    </td>
                </tr>              
                <% 
                } 
                %>
                <tr>
                    <td class="row_footer_left">
                    <% if (Model.HasPreviousPage) { %>
                        <%= Html.RouteLink("<<Previous Page", "List", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%>
                    <% } %>
                    </td>
                    <td colspan="10" class="row_footer_right">
                    <% if (Model.HasNextPage) {  %>
                        <%= Html.RouteLink("Next Page>>>", "List", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), id = ViewData["ticketQueueGroupId"] })%>
                    <% } %> 
                    </td>
                </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="10"><%if (ViewData["Access"] == "WriteAccess")
                    { %><%= Html.ActionLink("Create Ticket Queue Item", "Create", new { Id = ViewData["ticketQueueGroupId"] }, new { @class = "red", title = "Create Ticket Queue Item" })%><%}%></td> 
		        </tr> 
            </table>

    </div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_ticketqueuegroups').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Ticket Queue Groups", "Main", new { controller = "TicketQueueGroup", action = "ListUnDeleted", }, new { title = "Ticket Queue Groups" })%> &gt;
<%=Html.RouteLink(ViewData["TicketQueueGroupName"].ToString(), "Default", new { controller = "TicketQueueGroup", action = "View", id = ViewData["TicketQueueGroupId"].ToString() }, new { title = ViewData["TicketQueueGroupName"] })%> &gt;
Items
</asp:Content>