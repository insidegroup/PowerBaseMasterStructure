<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.SystemUserGDSAccessRightsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Rights
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Rights</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
				<tr> 
			        <th width="7%" class="row_header_left"><%= Html.RouteLink("GDS", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSName" }, new { title = "GDS" })%></th>
                    <th width="14%"><%= Html.RouteLink("Home PCC/Office ID", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PseudoCityOrOfficeId" }, new { title = "Home PCC/Office ID" })%></th>
                    <th width="10%"><%= Html.RouteLink("GDS Sign On ID", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSSignOnID" }, new { title = "GDS Sign On ID" })%></th>
                    <th width="14%"><%= Html.RouteLink("TA/GTID/Certificate", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TAGTIDCertificate" }, new { title = "TA/GTID/Certificate" })%></th>
                    <th width="10%"><%= Html.RouteLink("GDS Access Type", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSAccessTypeName" }, new { title = "GDS Access Type" })%></th>
                    <th width="10%"><%= Html.RouteLink("Request ID", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "RequestId" }, new { title = "Request ID" })%></th>
                    <th width="10%"><%= Html.RouteLink("Enabled Date", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "EnabledDate" }, new { title = "Enabled Date" })%></th>
                    <th width="10%"><%= Html.RouteLink("Internal Remarks", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "InternalRemarks" }, new { title = "Internal Remarks" })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.SystemUserGDSAccessRights)
                { 
                %>
                <tr>
                    <td><%= Html.Encode(item.GDSName) %></td>
                    <td><%= Html.Encode(item.PseudoCityOrOfficeId) %></td>
                    <td><%= Html.Encode(item.GDSSignOnID) %></td>
                    <td><%= Html.Encode(item.TAGTIDCertificate) %></td>
                    <td><%= Html.Encode(item.GDSAccessTypeName) %></td>
                    <td><%= Html.Encode(item.RequestId) %></td>
                    <td><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td> 
                    <td><%= Html.Encode(item.InternalRemarks) %></td>
                    <td align="center">
                        <%= Html.ActionLink("View", "View", new { id = item.SystemUserGDSAccessRightId })%>
                    </td>
                    <td align="center">
                        <%if (item.HasWriteAccess == true){ %>
							<%= Html.ActionLink("Edit", "Edit", new { id = item.SystemUserGDSAccessRightId })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (item.HasWriteAccess == true){ %>
							<%= Html.ActionLink("Delete", "Delete", new { id = item.SystemUserGDSAccessRightId })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
					<td colspan="11" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
								<% if (Model.SystemUserGDSAccessRights.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = (Model.SystemUserGDSAccessRights.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.SystemUserGDSAccessRights.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid, page = (Model.SystemUserGDSAccessRights.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.SystemUserGDSAccessRights.TotalPages > 0){ %>Page <%=Model.SystemUserGDSAccessRights.PageIndex%> of <%=Model.SystemUserGDSAccessRights.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a> 
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Export", "Export", new { id = Model.SystemUser.SystemUserGuid }, new { @class = "red", title = "Export" })%>
						<%}%>
                    </td>
			        <td class="row_footer_blank_right" colspan="9">
						<%= Html.ActionLink("Deleted GDS Access Rights", "ListDeleted", new { id = Model.SystemUser.SystemUserGuid }, new { @class = "red", title = "Deleted GDS Access Rights" })%>
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create GDS Access Rights", "Create", new { id = Model.SystemUser.SystemUserGuid }, new { @class = "red", title = "Create GDS Access Rights" })%>
						<%}%>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('.full-width #search_wrapper').css('height', '26px');

		//Search
		$('#search').show();
		$('#ft').attr('name', 'id');
		$("#frmSearch input[name='id']").val('<%=Html.Encode(Model.SystemUser.SystemUserGuid)%>');
		$('#btnSearch').click(function () {
			if ($('#filter').val() == "") {
				alert("No Search Text Entered");
				return false;
			}
			$("#frmSearch").attr("action", "/SystemUserGDSAccessRight.mvc/ListUnDeleted");
			$("#frmSearch").submit();

		});
	});
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Teams", "Main", new { controller = "Team", action = "List", }, new { title = "Teams" })%> &gt;
<%=Html.RouteLink("System Users", "Main", new { controller = "SystemUser", action = "List", }, new { title = "System Users" })%> &gt;
<%= Html.Encode(Model.SystemUser.FirstName) %> <%= Html.Encode(Model.SystemUser.LastName) %>, <%= Html.Encode(Model.SystemUser.UserProfileIdentifier) %> &gt;
GDS Access Rights
</asp:Content>