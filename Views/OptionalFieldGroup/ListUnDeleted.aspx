<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldGroupsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Groups</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="40%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "OptionalFieldGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                    <th width="15%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="20%"><%= Html.RouteLink("Hierarchy Item", "ListMain", new { page = 1, sortField = "HierarchyItem", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.OptionalFieldGroups) { 
                %>
                <tr>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.OptionalFieldGroupName, 45))%></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%= Html.Encode(item.HierarchyItem) %></td>       
                    <td><%= Html.RouteLink("Items", "Default", new { controller="OptionalFieldItem", action="List",id = item.OptionalFieldGroupId}, new { title = "View" })%> </td>
                    <td><%= Html.RouteLink("View", "Default", new { action="View", id = item.OptionalFieldGroupId }, new { title = "View" })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.OptionalFieldGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.OptionalFieldGroupId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
               <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.OptionalFieldGroups.HasPreviousPage)
                                                        { %><%= Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.OptionalFieldGroups.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"><% if (Model.OptionalFieldGroups.HasNextPage)
                                                         {  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { page = (Model.OptionalFieldGroups.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.OptionalFieldGroups.TotalPages > 0)
                                                         { %>Page <%=Model.OptionalFieldGroups.PageIndex%> of <%=Model.OptionalFieldGroups.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
            </table>
	        <table cellpadding="0" cellspacing="0" border="0" width="100%">
				<tr>
					<td width="20%" class="row_footer_blank_left">
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td width="80%" class="row_footer_blank_right" colspan="6">
					<%= Html.ActionLink("Orphaned Optional Field Groups", "ListOrphaned", null, new { @class = "red", title = "Orphaned  Optional Field Groups" })%>&nbsp;
					<%= Html.ActionLink("Deleted Optional Field Groups", "ListDeleted", null, new { @class = "red", title = "Deleted Optional Field Groups" })%>&nbsp;
					<%if (Model.HasDomainWriteAccess)
						{ %><%= Html.ActionLink("Create  Optional Field Group", "Create", null, new { @class = "red", title = "Create  Optional Field Group" })%>
					<%}%>
					</td> 
				</tr>    
			</table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function() {
		$('#menu_passivesegmentbuilder').click();
		$('#menu_passivesegmentbuilder_optionalfields').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search').show();
	});

    //Search
    $('#btnSearch').click(function() {
      if ($('#filter').val() == "") {
          alert("No Search Text Entered");
          return false;
      }
      $("#frmSearch").attr("action", "/OptionalFieldGroup.mvc/ListUnDeleted");
      $("#frmSearch").submit();

  });
 </script>
</asp:Content>
