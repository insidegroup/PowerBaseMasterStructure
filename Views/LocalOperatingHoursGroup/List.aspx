<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectLocalOperatingHoursGroups_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - LocalOperatingHoursGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Local Operating Hours Groups</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="37%" class="row_header_left"><%= Html.RouteLink("Local Operating Hours Group Name", "ListMain", new { page = 1, sortField = "LocalOperatingHoursGroupName", sortOrder = ViewData["NewSortOrder"], filter = Request.QueryString["filter"] })%></th> 
			        <th width="20%">&nbsp;</th>
                    <th width="20%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="7%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
              
                foreach (var item in Model) { 
                   
                %>
                <tr>
                    <td colspan="2"><%= Html.Encode(item.LocalOperatingHoursGroupName) %></td>
                    <td><%= Html.Encode(item.HierarchyType)%></td>
                    <td><%= Html.RouteLink("Items", "Default", new { controller="LocalOperatingHoursItem" , action="List", id = item.LocalOperatingHoursGroupId }, new { title = "Items" })%></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.LocalOperatingHoursGroupId }, new { title = "View" })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.LocalOperatingHoursGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Delete", "Default", new { action="Delete", id = item.LocalOperatingHoursGroupId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="6"><%= Html.ActionLink("Orphaned Local Operating Hours Groups", "ListOrphaned", null, new { @class = "red", title="Orphaned Local Operating Hours Groups" })%>&nbsp;<%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.ActionLink("Create Local Operating Hours Group", "Create", null, new { @class = "red" })%><%} %></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_localoperatinghours').click();
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
        $("#frmSearch").attr("action", "/LocalOperatingHoursGroup.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>
