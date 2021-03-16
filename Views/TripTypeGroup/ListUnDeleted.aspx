<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTripTypeGroups_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Trip Type Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Trip Type Groups</div></div>
    <div id="content">
         <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
            <tr> 
			        <th width="22%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "TripTypeGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
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
                <td colspan="2" class="text-wrap"><%= Html.Encode(item.TripTypeGroupName)%></td>
                <td><%= Html.Encode(item.HierarchyType) %></td>
                <td><%if (item.HierarchyType == "ClientSubUnit") { %><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%><% } %></td>                    
                <td><%= Html.RouteLink("Items", "List", new { controller="TripTypeItem", id = item.TripTypeGroupId }, new { title="Items"})%></td>
                <td><%= Html.RouteLink("View", "Default", new { action="View", id = item.TripTypeGroupId }, new { title = "View" })%> </td>
                <td>
                    <%if (item.HierarchyType == "ClientSubUnit") { %>
                        <%= Html.RouteLink("Hierarchy", "Default", new { id = item.TripTypeGroupId, action = "LinkedClientSubUnits" }, new { title = "Hierarchy" })%>
                    <%} %>
                </td>
                <td>
                    <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.TripTypeGroupId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.TripTypeGroupId }, new { title = "Delete" })%>
                    <%} %>
                </td>
            </tr>              
            <% 
            } 
            %>
            <tr>
            <td colspan="9" class="row_footer">
                <div class="paging_container">
                    <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListUnDeleted", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                    <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListUnDeleted", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                    <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                </div>
            </td>
            </tr>
		    <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="8"><%= Html.ActionLink("Orphaned Trip Type Groups", "ListOrphaned", null, new { @class = "red", title="Orphaned Trip Type Groups" })%>&nbsp;<%= Html.RouteLink("Deleted Trip Type Groups", "Main", new { action = "ListDeleted" }, new { @class = "red", title = "Deleted Trip Type Groups" })%>
			    <%if (ViewData["Access"] == "WriteAccess")
            { %><%= Html.RouteLink("Create Trip Type Group", "Main", new { action = "Create" }, new { @class = "red", title = "Create Trip Type Group" })%><%} %></td> 
		    </tr> 
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_triptypegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
    })

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/TripTypeGroup.mvc/ListUnDeleted");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>