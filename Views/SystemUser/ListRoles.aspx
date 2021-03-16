<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.PaginatedList<CWTDesktopDatabase.Models.fnDesktopDataAdmin_SelectSystemUserRoles_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - SystemUserRoles
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">SystemUser Roles</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="50%" class="row_header_left"><%= Html.RouteLink("Role", "ListMain", new { controller = "SystemUser", action = "ListRoles", page = 0, sortField = "Role", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By Role" })%></th> 
			        <th width="30%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="15%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td colspan="3"><%= Html.Encode(item.AdministratorRoleHierarchyLevelTypeName) %></td>
                    <td align="center">
                    <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.ActionLink("Remove Role", "DeleteRole", new { hierarchyLevelTypeId = item.HierarchyLevelTypeId, administratorRoleId = item.AdministratorRoleId, id = ViewData["SystemUserGuid"] })%>
                    <% } %>
                    </td>
               </tr>        
                <% 
                } 
                %>
                 <tr>
                    <td colspan="4" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                             <% if (Model.HasPreviousPage){ %>
                        <%= Html.RouteLink("<<Previous Page", "ListMain", new { controller = "SystemUser", action = "ListRoles", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), id = ViewData["SystemUserGuid"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.HasNextPage){  %>
                        <%= Html.RouteLink("Next Page>>>", "ListMain", new { controller = "SystemUser", action = "ListRoles", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), id = ViewData["SystemUserGuid"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex+1%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
               
		        <tr> 
		            <td class="row_footer_blank_left">
                        <a href="javascript:history.back();" class="red" title="Back">Back</a>
                        &nbsp;
                        <%if (ViewData["ExportAccess"] == "WriteAccess"){ %>
			                <%= Html.RouteLink("Export", "Main", new { controller="SystemUser", action="ExportSystemUserRoles", id = ViewData["SystemUserGuid"] }, new { @class = "red", title = "Export" })%>
			            <% } %>
		            </td>
			        <td class="row_footer_blank_right" colspan="3">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			            <%= Html.RouteLink("Add Role", "Main", new { controller="SystemUser", action="CreateRole", id = ViewData["SystemUserGuid"] }, new { @class = "red", title = "Add Role" })%>
			        <% } %> 
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>

 <script type="text/javascript">
    $(document).ready(function() {
        $('#menu_teams').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
