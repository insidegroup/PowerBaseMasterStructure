<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectSystemUsers_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - SystemUsers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">SystemUsers</div></div>
        <div id="content">
        <%using (Html.BeginForm(null, null, FormMethod.Get, new { id = "form0" })){ %>
				<div id="divSearch">
					<table class="" cellspacing="0" width="100%">
						<thead>
							<tr>
								<th>Search field:</th>
								<th>
									<%= Html.DropDownList("filterField_1", ViewData["Filters_1"] as SelectList, "Please Select...") %>
									<br />
									<%= Html.TextBox("filterValue_1") %>
								</th>
								<th>
									<%= Html.DropDownList("filterField_2", ViewData["Filters_2"] as SelectList, "Please Select...") %>
									<br />
									<%= Html.TextBox("filterValue_2") %>
								</th>
								<th>
									<%= Html.DropDownList("filterField_3", ViewData["Filters_3"] as SelectList, "Please Select...") %>
									<br />
									<%= Html.TextBox("filterValue_3") %>
								</th>
								<th><span id="SearchButton"><small>Search >> </small></span></th>
								<th>&nbsp;</th>
							</tr>
							<tr id="lastSearch">
								<th colspan="6">
									<span class="error"></span>
								</th>
							</tr>
						</thead>
					</table>
				</div>
			<% } %>
			<div class="home_button">
				<a href="/" class="red">Home</a>
			</div>
			<table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="10%" class="row_header_left"><%= Html.RouteLink("Last Name", "ListMain", new { controller = "SystemUser", action = "List", page = 0, sortField = "LastName", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["FilterField_1"].ToString(), filterValue_1 = ViewData["FilterValue_1"].ToString(), filterField_2  = ViewData["FilterField_2"].ToString(), filterValue_2 = ViewData["FilterValue_2"].ToString(), filterField_3 = ViewData["FilterField_3"].ToString(), filterValue_3 = ViewData["FilterValue_3"].ToString() }, new { title = "Sort By Last Name" })%></th> 
			        <th width="10%"><%= Html.RouteLink("First Name", "ListMain", new { controller = "SystemUser", action = "List", page = 0, sortField = "FirstName", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["FilterField_1"].ToString(), filterValue_1 = ViewData["FilterValue_1"].ToString(), filterField_2  = ViewData["FilterField_2"].ToString(), filterValue_2 = ViewData["FilterValue_2"].ToString(), filterField_3 = ViewData["FilterField_3"].ToString(), filterValue_3 = ViewData["FilterValue_3"].ToString() }, new { title = "Sort By First Name" })%></th> 
			        <th width="12%"><%= Html.RouteLink("Location", "ListMain", new { controller = "SystemUser", action = "List", page = 0, sortField = "LocationName", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["FilterField_1"].ToString(), filterValue_1 = ViewData["FilterValue_1"].ToString(), filterField_2  = ViewData["FilterField_2"].ToString(), filterValue_2 = ViewData["FilterValue_2"].ToString(), filterField_3 = ViewData["FilterField_3"].ToString(), filterValue_3 = ViewData["FilterValue_3"].ToString() }, new { title = "Sort By Location Name" })%></th>  
			        <th width="10%"><%= Html.RouteLink("Login", "ListMain", new { controller = "SystemUser", action = "List", page = 0, sortField = "SystemUserLoginIdentifier", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["FilterField_1"].ToString(), filterValue_1 = ViewData["FilterValue_1"].ToString(), filterField_2  = ViewData["FilterField_2"].ToString(), filterValue_2 = ViewData["FilterValue_2"].ToString(), filterField_3 = ViewData["FilterField_3"].ToString(), filterValue_3 = ViewData["FilterValue_3"].ToString() }, new { title = "Sort By Login" })%></th>  
					<th width="8%"><%= Html.RouteLink("Profile ID", "ListMain", new { controller = "SystemUser", action = "List", page = 0, sortField = "UserProfileIdentifier", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["FilterField_1"].ToString(), filterValue_1 = ViewData["FilterValue_1"].ToString(), filterField_2  = ViewData["FilterField_2"].ToString(), filterValue_2 = ViewData["FilterValue_2"].ToString(), filterField_3 = ViewData["FilterField_3"].ToString(), filterValue_3 = ViewData["FilterValue_3"].ToString() }, new { title = "Sort By Profile ID" })%></th>  
			        <th width="8%"><%= Html.RouteLink("IsActive?", "ListMain", new { controller = "SystemUser", action = "List", page = 0, sortField = "IsActiveFlag", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["FilterField_1"].ToString(), filterValue_1 = ViewData["FilterValue_1"].ToString(), filterField_2  = ViewData["FilterField_2"].ToString(), filterValue_2 = ViewData["FilterValue_2"].ToString(), filterField_3 = ViewData["FilterField_3"].ToString(), filterValue_3 = ViewData["FilterValue_3"].ToString() }, new { title = "Sort By IsActive" })%></th>  
					<th width="12%">&nbsp;</th> 
					<th width="4%">&nbsp;</th> 
					<th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="9%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.LastName) %>, </td>
                    <td><%= Html.Encode(item.FirstName) %> <%= Html.Encode(item.MiddleName) %></td>
                    <td><%= Html.Encode(item.LocationName) %></td>
					<td><%= Html.Encode(item.SystemUserLoginIdentifier) %></td>
                    <td><%= Html.Encode(item.UserProfileIdentifier)%></td>
					<td><%= Html.Encode(item.IsActiveFlag == true ? "True" : "False" )%></td>
					<td><%= Html.RouteLink("GDS Access Rights", "Main", new { controller = "SystemUserGDSAccessRight", action = "ListUnDeleted", id = item.SystemUserGuid }, new { title = "View System User's GDS Access Rights" })%></td>
                    <td><%= Html.RouteLink("GDSs", "Main", new { controller = "SystemUserGDS", action = "List", id = item.SystemUserGuid }, new { title = "View SystemUser's GDSs" })%></td>
                    <td><%= Html.RouteLink("Teams", "ListMain", new { controller = "SystemUser", action = "ListTeams", id = item.SystemUserGuid }, new { title = "View SystemUser's Teams" })%></td>
                    <td><%= Html.RouteLink("Roles", "ListMain", new { controller = "SystemUser", action = "ListRoles", id = item.SystemUserGuid }, new { title = "View SystemUser's Roles" })%></td>
                    <td>	
						<%if (item.HasDefineRolesWriteAccess.Value){%>
						<%= Html.RouteLink("Define Roles", "ListMain", new { controller = "SystemUser", action = "DefineRolesStep1", id = item.SystemUserGuid }, new { title = "Define SystemUser's Roles" })%>
						<% } %>
                    </td>
                    <td><%= Html.RouteLink("View", "Main", new { controller = "SystemUser", action = "ViewItem", id = item.SystemUserGuid }, new { title = "View SystemUser" })%></td>
                    <td><%if (item.HasLocationWriteAccess == true || item.LocationId == null) { %><%=  Html.RouteLink("Edit", "Main", new { controller = "SystemUser", action = "Edit", id = item.SystemUserGuid }, new { title = "Edit SystemUser" })%><%} %></td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                    <td colspan="13" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                                        <%= Html.RouteLink("<<Previous Page", "ListMain", new { controller = "SystemUser", action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filterField_1 = ViewData["FilterField_1"].ToString(), filterValue_1 = ViewData["FilterValue_1"].ToString(), filterField_2  = ViewData["FilterField_2"].ToString(), filterValue_2 = ViewData["FilterValue_2"].ToString(), filterField_3 = ViewData["FilterField_3"].ToString(), filterValue_3 = ViewData["FilterValue_3"].ToString() }, new {title="Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                                        <%= Html.RouteLink("Next Page>>>", "ListMain", new { controller = "SystemUser", action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filterField_1 = ViewData["FilterField_1"].ToString(), filterValue_1 = ViewData["FilterValue_1"].ToString(), filterField_2  = ViewData["FilterField_2"].ToString(), filterValue_2 = ViewData["FilterValue_2"].ToString(), filterField_3 = ViewData["FilterField_3"].ToString(), filterValue_3 = ViewData["FilterValue_3"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
               
		        <tr> 
					<td class="row_footer_blank_left" colspan="4">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
					<td class="row_footer_blank_right" colspan="9"></td>
		        </tr> 
            </table>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/SystemUserList.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Teams", "Main", new { controller = "Team", action = "List", }, new { title = "Teams" })%> &gt;
SystemUsers
</asp:Content>