<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTicketQueueGroups_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Ticket Queue Groups (Deleted)</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="22%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "TicketQueueGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="20%">&nbsp;</th>
                    <th width="20%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="8%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "LinkedItemCount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="6%">&nbsp;</th>
                    <th width="5%">&nbsp;</th>
			        <th width="8%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td colspan="2" class="text-wrap"><%= Html.Encode(item.TicketQueueGroupName) %></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%if (item.HierarchyType == "ClientSubUnit") { %><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%><% } %></td>                    
                    <td><%= Html.RouteLink("Items", "List", new { controller = "TicketQueueItem", id = item.TicketQueueGroupId }, new { title = "Items" })%></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.TicketQueueGroupId }, new { title = "View" })%> </td>
                    <td>
                        <%if (item.HierarchyType == "ClientSubUnit") { %>
                          <%= Html.RouteLink("Hierarchy", "Default", new { id = item.TicketQueueGroupId, action = "LinkedClientSubUnits" }, new { title = "Hierarchy" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                            <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.TicketQueueGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("UnDelete", "UnDelete", new { id = item.TicketQueueGroupId }, new { title = "UnDelete" })%>
                        <%} %>
                    </td>
                </tr>              
                <% 
                } 
                %>
                <tr>
                <td colspan="9" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListDeleted", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListDeleted", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                 <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="8"><%= Html.ActionLink("UnDeleted Ticket Queue Groups", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted Ticket Queue Groups" })%></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#menu_ticketqueuegroups').click();
        $('#search').show();
    })

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/TicketQueueGroup.mvc/ListDeleted");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>