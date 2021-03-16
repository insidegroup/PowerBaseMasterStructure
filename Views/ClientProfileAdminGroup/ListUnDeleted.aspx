<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileAdminGroupsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profile Administration Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Profile Administration Groups</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "ClientProfileGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Hierarchy Item", "ListMain", new { page = 1, sortField = "HierarchyItem", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("GDS", "ListMain", new { page = 1, sortField = "GDSName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Back Office", "ListMain", new { page = 1, sortField = "BackOfficeSystemDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.ClientProfileAdminGroups) { 
                %>
                <tr>
                    <td width="50%"><%=Html.Encode(item.ClientProfileGroupName)%></td>
					<td width="11%"><%= Html.Encode(item.HierarchyType) %></td>
					<td width="15%"><%= Html.Encode(item.HierarchyItem) %></td>
					<td width="8%"><%= Html.Encode(item.GDSName) %></td>
					<td width="12%"><%= Html.Encode(item.BackOfficeSystemDescription) %></td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Items", "Default", new { controller = "ClientProfileAdminItem", action = "List", id = item.ClientProfileAdminGroupId }, new { title = "Items" })%>
                        <%} %></td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ClientProfileAdminGroupId }, new { title = "Delete" })%>
                        <%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.ClientProfileAdminGroups.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListUnDeleted", page = (Model.ClientProfileAdminGroups.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.ClientProfileAdminGroups.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "ListUnDeleted", page = (Model.ClientProfileAdminGroups.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.ClientProfileAdminGroups.TotalPages > 0) { %>Page <%=Model.ClientProfileAdminGroups.PageIndex%> of <%=Model.ClientProfileAdminGroups.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td colspan="7" style="padding-left:0;">
						<table cellpadding="0" cellspacing="0" border="0" width="100%">
							<tr>
								<td class="row_footer_blank_left">
									<a href="javascript:window.print();" class="red" title="Print">Print</a>
								</td>
								<td class="row_footer_blank_right">
									<%if (Model.HasDomainWriteAccess){ %>
										<%= Html.ActionLink("Deleted Client Profile Administration Groups", "ListDeleted", null, new { @class = "red", title = "Deleted Client Profile Administration Groups" })%>&nbsp;
									<% } %>
									<%if (Model.HasDomainWriteAccess){ %>
										<%= Html.RouteLink("Create Client Profile Administration Group", "Main", new { action = "Create" }, new { @class = "red", title = "Create Client Profile Administration Group" })%>
									<% } %>
								</td>
							</tr>
						</table>
					</td>  
				</tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_clientprofiles').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search').show();
	})
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/ClientProfileAdminGroup.mvc/ListUnDeleted");
		$("#frmSearch").submit();
	});
</script>
</asp:Content>