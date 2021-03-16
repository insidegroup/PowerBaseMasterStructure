<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPNROutputGroups_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - PNR Output Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">PNR Output Groups</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="45%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "PNROutputGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                    <th width="20%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="12%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "Multiple", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="8%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th>
			        <th width="10%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                 <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.PNROutputGroupName) %></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%= Html.Encode(item.HierarchyType == "Multiple" ? "Yes" : "No")%></td>                    
                    <td><%= Html.RouteLink("Remarks", "Default", new { id = item.PNROutputGroupId, controller = "PNROutputGroupXML", action = "ListLanguages" }, new { title = "View Remarks" })%></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.PNROutputGroupId }, new { title = "View" })%> </td>
                    <td><%= Html.RouteLink("Hierarchy", "Default", new { action = "HierarchySearch", id = item.PNROutputGroupId, h = item.HierarchyType }, new { title = "Hierarchy" })%> </td>
                </tr>              
                <% 
                } 
                %>
               <tr>
                    <td colspan="6" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%= Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"><% if (Model.HasNextPage){  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="5"><%= Html.ActionLink("Orphaned PNR Output Groups", "ListOrphaned", null, new { @class = "red", title="Orphaned PNR Output Groups" })%>&nbsp;<%= Html.ActionLink("Deleted PNR Output Groups", "ListDeleted", null, new { @class = "red", title = "Deleted PNR Output Groups" })%></td> 
		        </tr> 
            </table>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_pnroutputs').click();
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
        $("#frmSearch").attr("action", "/PNROutputGroup.mvc/ListUnDeleted");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>