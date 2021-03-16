<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTeams_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Teams</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="25%" class="row_header_left"><%= Html.RouteLink("Name", "ListMain", new { controller = "Team", action = "List", page = 1, sortField = "Name", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="20%">                        <%= Html.RouteLink("Hierarchy", "ListMain", new { controller = "Team", action = "List", page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="12%">&nbsp;</th> 
			        <th width="11%">&nbsp;</th> 
			        <th width="14%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.TeamName) %></td>
                    <td><%= Html.Encode(item.HierarchyType)%></td>
                    <td><%=  Html.RouteLink("ClientSubUnits", "Default", new { controller = "Team", action = "ListClientSubUnits", id = item.TeamId }, new { title = "View Team's SystemUsers" })%></td>
                    <td><%=  Html.RouteLink("SystemUsers", "Default", new { controller = "Team", action = "ListSystemUsers", id = item.TeamId }, new { title = "View Team's SystemUsers" })%></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Move/Copy Users", "Default", new { controller = "Team", action = "MoveCopyUsersStep1", id = item.TeamId }, new { title = "Copy Users" })%>
                        <%} %>
                    </td>
                    <td><%=  Html.RouteLink("View", "Default", new { controller = "Team", action = "View", id = item.TeamId }, new { title = "View Team" })%></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { controller = "Team", action = "Edit", id = item.TeamId }, new { title = "Edit Team" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Delete", "Default", new { controller = "Team", action = "Delete", id = item.TeamId }, new { title = "Delete Team" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="8" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%= Html.RouteLink("<<Previous Page", "ListMain", new { controller = "Team", action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] })%><%}%></div>
                            <div class="paging_right"><% if (Model.HasNextPage){  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { controller = "Team", action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] })%><%}%></div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                  <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="7">
			        <%if (ViewData["Access"] == "WriteAccess"){%>
			         <%= Html.RouteLink("Create Team", "Main", new { action = "Create" }, new { @class = "red", title = "Create Team" })%>
			        <%} %></td> 
		        </tr> 
            </table>
        </div>
    </div>

 <script type="text/javascript">
     $(document).ready(function () {
         $('#search').show();
         $('#menu_teams').click();
         $("tr:odd").addClass("row_odd");
         $("tr:even").addClass("row_even");
     })


     //Search
     $('#btnSearch').click(function () {
         if ($('#filter').val() == "") {
             alert("No Search Text Entered");
             return false;
         }
         $("#frmSearch").attr("action", "/Team.mvc/List");
         $("#frmSearch").submit();

     });
 </script>
</asp:Content>

