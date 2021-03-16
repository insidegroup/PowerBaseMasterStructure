<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileGroupsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profiles</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Profiles</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Unique Name", "ListMain", new { page = 1, sortField = "UniqueName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "Hierarchy", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Hierarchy Item", "ListMain", new { page = 1, sortField = "HierarchyItem", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("GDS", "ListMain", new { page = 1, sortField = "GDSName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("PCC/Office ID", "ListMain", new { page = 1, sortField = "PseudoCityOrOfficeId", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Profile Name", "ListMain", new { page = 1, sortField = "ClientProfileGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Publish Date", "ListMain", new { page = 1, sortField = "CreationPublishDate", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<td class="row_header_right">    <div class="home_button">
							<a href="<%=Url.Content("~/")%>" class="red">Home</a>
						</div></td> 
		        </tr> 
                <%
                foreach (var item in Model.ClientProfileGroups) { 
                %>
                <tr>
                    <td width="20%"><abbr title="<%=Html.Encode(item.UniqueName) %>" style="border:none;"><%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Html.Encode(item.UniqueName), 25) %></abbr></td>
					<td width="10%"><%= Html.Encode(item.HierarchyType) %></td>
					<td width="15%"><%= Html.Encode(item.HierarchyItem) %></td>
					<td width="8%"><%= Html.Encode(item.GDSName) %></td>
					<td width="12%"><%= Html.Encode(item.PseudoCityOrOfficeId) %></td>
					<td width="10%"><%= Html.Encode(item.ClientProfileGroupName) %></td>
					<td width="10%"><%= Html.Encode(item.CreationPublishDate) %></td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Items", "Default", new { controller = "ClientProfileItem", action = "List", id = item.ClientProfileGroupId }, new { title = "Items" })%>
                        <%} %>
					</td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Publish", "Default", new { action = "Publish", id = item.ClientProfileGroupId }, new { title = "Publish" })%>
                        <%} %>
					</td>
					<td width="3%">
						<%=  Html.RouteLink("Export", "Default", new { action = "Export", id = item.ClientProfileGroupId }, new { title = "Export" })%>
					</td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ClientProfileGroupId }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ClientProfileGroupId }, new { title = "Delete" })%>
                        <%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="12" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.ClientProfileGroups.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListUnDeleted", page = (Model.ClientProfileGroups.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.ClientProfileGroups.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "ListUnDeleted", page = (Model.ClientProfileGroups.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.ClientProfileGroups.TotalPages > 0) { %>Page <%=Model.ClientProfileGroups.PageIndex%> of <%=Model.ClientProfileGroups.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
					<td class="row_footer_blank_right" colspan="11">
						<%= Html.ActionLink("Deleted Client Profiles", "ListDeleted", null, new { @class = "red", title = "Deleted Client Profiles" })%>&nbsp;
						<%if (Model.HasDomainWriteAccess){ %>
							<%= Html.RouteLink("Create Client Profile", "Main", new { action = "Create" }, new { @class = "red", title = "Create Client Profile" })%>
						<% } %>
					</td>  
				</tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
	$(document).ready(function () {
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search').show();
	})
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/ClientProfileGroup.mvc/ListUnDeleted");
		$("#frmSearch").submit();
	});
	function confirm_alert(node) {
		return confirm("This will generate a text file to display to the user.");
	}
</script>
</asp:Content>