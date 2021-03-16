<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTeamClientSubUnits_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Team Client SubUnits</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="35%" class="row_header_left"><%= Html.RouteLink("ClientSubUnitName", "List", new { controller = "Team", action = "ListClientSubUnits", page = 1, id = ViewData["TeamId"], sortField = "ClientSubUnitName", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="52%">Include In Client DropList</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ClientSubUnitName) %></td>
                    <td><%= Html.Encode(item.IncludeInClientDroplistFlag) %></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { controller = "ClientSubUnitTeam", action = "Edit", id = item.TeamId, clientSubUnitGuid = item.ClientSubUnitGuid, from = "Team" }, new { title = "Edit Client SubUnit Team" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Remove", "Default", new { controller = "ClientSubUnitTeam", action = "Delete", id = item.TeamId, clientSubUnitGuid = item.ClientSubUnitGuid, from = "Team" }, new { title = "Remove Client SubUnit from Team" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { page = (Model.PageIndex - 1), id = ViewData["TeamId"], sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString()}, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"><% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { page = (Model.PageIndex + 1), id = ViewData["TeamId"], sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%></div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
                
              
		        <tr> 
			        <td class="row_footer_blank_right" colspan="4">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.RouteLink("Add ClientSubUnit to Team", "Default", new { controller = "ClientSubUnitTeam", action = "CreateClientSubUnitForTeam", id = ViewData["TeamId"] }, new { @class = "red", title = "Add ClientSubUnit to Team" })%></td>
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
<%=Html.RouteLink("Teams", "Main", new { controller = "Team", action = "List", }, new { title = "Teams" })%> &gt;
<%=Html.RouteLink(ViewData["TeamName"].ToString(), "Default", new { controller = "Team", action = "View", id = ViewData["TeamId"] }, new { title = ViewData["TeamName"].ToString() })%> &gt;
Client SubUnits
</asp:Content>
