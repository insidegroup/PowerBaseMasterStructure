<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTeamSystemUsers_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Team SystemUsers</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="25%" class="row_header_left"><%= Html.RouteLink("Last Name", "List", new { controller = "Team", action = "ListSystemUsers", page = 0, id = ViewData["TeamId"], sortField = "LastName", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="27%"><%= Html.RouteLink("First Name", "List", new { controller = "Team", action = "ListSystemUsers", page = 0, id = ViewData["TeamId"], sortField = "FirstName", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="20%"><%= Html.RouteLink("Login ID", "List", new { controller = "Team", action = "ListSystemUsers", page = 0, id = ViewData["TeamId"], sortField = "SystemUserLoginIdentifier", sortOrder = ViewData["NewSortOrder"].ToString() })%></th>
					<th width="20%"><%= Html.RouteLink("Profile ID", "List", new { controller = "Team", action = "ListSystemUsers", page = 0, id = ViewData["TeamId"], sortField = "UserProfileIdentifier", sortOrder = ViewData["NewSortOrder"].ToString() })%></th>
					<th width="8%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.LastName) %>, </td>
                    <td><%= Html.Encode(item.FirstName) %> <%= Html.Encode(item.MiddleName) %></td>
					<td><%=Server.HtmlEncode(item.SystemUserLoginIdentifier)%></td>
					<td><%=Server.HtmlEncode(item.UserProfileIdentifier)%></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Remove", "Default", new { controller = "SystemUserTeam", action = "Delete", id = item.TeamId, systemUserGuid = item.SystemUserGuid, from = "Team" }, new { title = "Remove SystemUser from Team" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td class="row_footer_left">
                    <% if (Model.HasPreviousPage) { %>
                        <%= Html.RouteLink("<<Previous Page", "List", new { controller = "Team", action = "ListSystemUsers", page = (Model.PageIndex - 1), id = ViewData["TeamId"], sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() })%>
                    <% } %>
                    </td>
                    <td colspan="4" class="row_footer_right">
                    <% if (Model.HasNextPage) {  %>
                        <%= Html.RouteLink("Next Page>>>", "List", new { controller = "Team", action = "ListSystemUsers", page = (Model.PageIndex + 1), id = ViewData["TeamId"], sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() })%>
                    <% } %> 
                    </td>
                </tr>
		        <tr> 
			        <td class="row_footer_blank_right" colspan="5">
			        <%if(ViewData["Access"] == "WriteAccess"){%>
			        <%= Html.RouteLink("Add SystemUser to Team", "Default", new { controller = "SystemUserTeam", action = "CreateUserForTeam", id = ViewData["TeamId"] }, new { @class = "red", title = "Add SystemUser to Team" })%></td>
			        <%} %>
		        </tr> 
            </table>
        </div>
    </div>

 <script type="text/javascript">
     $(document).ready(function() {
         $('#search').show();
         $('#menu_teams').click();
         $("tr:odd").addClass("row_odd");
         $("tr:even").addClass("row_even");

     	//Search
         $('#ft').attr('name', 'id');
         $("#frmSearch input[name='id']").val('<%=Html.Encode(ViewData["TeamId"])%>');
         $('#btnSearch').click(function () {
         	$("#frmSearch").attr("action", "/Team.mvc/ListSystemUsers");
         	$("#frmSearch").submit();
         });
     })

 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Teams", "Main", new { controller = "Team", action = "List", }, new { title = "Teams" })%> &gt;
<%=Html.RouteLink(ViewData["TeamName"].ToString(), "Default", new { controller = "Team", action = "View", id = ViewData["TeamId"] }, new { title = ViewData["TeamName"].ToString() })%> &gt;
SystemUsers
</asp:Content>
