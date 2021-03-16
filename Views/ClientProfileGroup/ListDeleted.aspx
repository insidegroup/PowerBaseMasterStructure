<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileGroupsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profile List</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Profiles (Deleted)</div></div>
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
					<th>&nbsp;</th>
					<th class="row_header_right">&nbsp;</th> 
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
					<td width="3%">Items</td>
					<td width="3%">Publish</td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("View", "Default", new { action = "View", id = item.ClientProfileGroupId }, new { title = "View" })%>
                        <%} %>
					</td>
					<td width="3%"><a href="#" onclick="return confirm_alert(this);">Export</a></td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ClientProfileGroupId }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="3%">
						<%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("UnDelete", "Default", new { action = "UnDelete", id = item.ClientProfileGroupId }, new { title = "UnDelete" })%>
                        <%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="13" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.ClientProfileGroups.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListDeleted", page = (Model.ClientProfileGroups.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.ClientProfileGroups.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "ListDeleted", page = (Model.ClientProfileGroups.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.ClientProfileGroups.TotalPages > 0) { %>Page <%=Model.ClientProfileGroups.PageIndex%> of <%=Model.ClientProfileGroups.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
					<td class="row_footer_blank_right" colspan="12">
						<%if (Model.HasDomainWriteAccess){ %>
							<%= Html.RouteLink("UnDeleted Client Profiles", "Main", new { action = "ListUnDeleted" }, new { @class = "red", title = "UnDeleted Client Profiles" })%>
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
		$("#frmSearch").attr("action", "/CientProfileGroup.mvc/ListDeleted");
		$("#frmSearch").submit();
	});
	function confirm_alert(node) {
		return confirm("This will generate a text file to display to the user.");
	}
</script>
</asp:Content>