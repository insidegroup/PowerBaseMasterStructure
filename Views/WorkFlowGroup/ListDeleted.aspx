<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectWorkFlowGroups_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - WorkFlow Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">WorkFlow Groups (Deleted)</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
		        <th width="58%" class="row_header_left"><%= Html.RouteLink("WorkFlow Group Name", "ListMain", new { page = 1, sortField = "WorkFlowGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Sort By Group Name" })%></th>
		        <th width="20%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Sort By Hierarchy" })%></th>
		        <th width="12%"><%= Html.RouteLink("Enabled Date", "ListMain", new { page = 1, sortField = "EnabledDate", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Sort By Enabled Date" })%></th>
		        <th width="5%">&nbsp;</th> 
		        <th width="5%">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.WorkFlowGroupName)%></td>
                <td><%= Html.Encode(item.HierarchyType) %></td>
                <td><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>                    
                <td><%= Html.RouteLink("Items", "List", new { controller="WorkflowItem" , action="List", id = item.WorkFlowGroupId }, new { title = "Items" })%></td>
                <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.WorkFlowGroupId }, new { title = "View" })%> </td>
            </tr>              
            <% 
            } 
            %>
            <tr>
                <td colspan="5" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
            	<td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		        <td class="row_footer_blank_right" colspan="4"><%= Html.RouteLink("List UnDeleted WorkFlow Groups", "Main", new { action = "ListUnDeleted" }, new { @class = "red", title = "UnDeleted WorkFlowGroups" })%></td> 
	        </tr> 
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_workflowgroups').click();
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
        $("#frmSearch").attr("action", "/WorkFlowGroup.mvc/ListDeleted");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>