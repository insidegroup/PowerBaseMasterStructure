<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTeamsBySystemUser_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - SystemUser
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">SystemUser Teams</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="25%" class="row_header_left"><%= Html.RouteLink("Name", "ListMain", new { controller = "SystemUser", action = "ListTeams", page = 1, sortField = "TeamName", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["SystemUserGuid"] })%></th> 
			        <th width="20%">Queue</th> 
			        <th width="47%"><%= Html.RouteLink("Description", "ListMain", new { controller = "SystemUser", action = "ListTeams", page = 1, sortField = "TeamTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["SystemUserGuid"] })%></th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.TeamName) %></td>
                    <td><%= Html.Encode(item.TeamQueue)%></td>
                    <td><%= Html.Encode(item.TeamTypeDescription)%></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Remove", "Default", new { controller = "SystemUserTeam", action = "Delete", id = item.TeamId, systemUserGuid = ViewData["SystemUserGuid"] , from="SystemUser"}, new { title = "Remove SystemUser from Team" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="2" class="row_footer_left">
                    <% if (Model.HasPreviousPage) { %>

                        <%= Html.RouteLink("<<Previous Page", "ListMain", new { controller = "SystemUser", action = "ListTeams", page = (Model.PageIndex - 1), id = ViewData["SystemUserGuid"], sortField = "TeamName", sortOrder = ViewData["CurrentSortOrder"].ToString() })%>
                    <% } %>
                    </td>
                    <td colspan="2" class="row_footer_right">
                    <% if (Model.HasNextPage) {  %>
                        <%= Html.RouteLink("Next Page>>>", "ListMain", new { controller = "SystemUser", action = "ListTeams", page = (Model.PageIndex + 1), id = ViewData["SystemUserGuid"], sortField = "TeamName", sortOrder = ViewData["CurrentSortOrder"].ToString() })%>
                    <% } %> 
                    </td>
                </tr>
                
		        <tr> 
		        	<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
			        <td class="row_footer_blank_right" colspan="3">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.RouteLink("Add SystemUser to Team", "Main", new { controller = "SystemUserTeam", action = "CreateTeamForUser", id = ViewData["SystemUserGuid"] }, new { @class = "red", title = "Add SystemUser to Team" })%></td>
			        <% } %> 
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
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("SystemUsers", "Main", new { controller = "SystemUser", action = "List", }, new { title = "SystemUsers" })%> &gt;
<%=Html.RouteLink(ViewData["SystemUserName"].ToString(), "Main", new { controller = "SystemUser", action = "ViewItem", id = ViewData["SystemUserGuid"] }, new { title = ViewData["SystemUserName"].ToString() })%> &gt;
Teams
</asp:Content>