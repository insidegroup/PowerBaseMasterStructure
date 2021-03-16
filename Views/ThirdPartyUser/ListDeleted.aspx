<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectThirdPartyUsers_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Third Party Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Third Party Users (Deleted)</div></div>
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
			        <th width="10%" class="row_header_left"><%= Html.RouteLink("Third Party User ID", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = 0, sortField = "ThirdPartyUserId", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Third Party User ID" })%></th> 
			        <th width="10%"><%= Html.RouteLink("Third Party Name", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = 0, sortField = "ThirdPartyName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Last Name" })%></th> 
			        <th width="10%"><%= Html.RouteLink("Last Name", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = 0, sortField = "LastName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Last Name" })%></th> 
			        <th width="10%"><%= Html.RouteLink("First Name", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = 0, sortField = "FirstName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By First Name" })%></th> 
			        <th width="8%"><%= Html.RouteLink("Country Code", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = 0, sortField = "CountryCode", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Country Code" })%></th>  
			        <th width="8%"><%= Html.RouteLink("IsActive?", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = 0, sortField = "IsActiveFlag", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By IsActive?" })%></th>  
					<th width="10%"><%= Html.RouteLink("Robotic User", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = 0, sortField = "RoboticUserFlag", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Robotic User" })%></th>  
			        <th width="10%"><%= Html.RouteLink("Internal Remarks", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = 0, sortField = "InternalRemarks", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Internal Remarks" })%></th>  
			        <th width="12%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
					<th width="4%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td class="wrap-text"><%= Html.Encode(item.TISUserId) %></td>
                    <td><%= Html.Encode(item.ThirdPartyName) %></td>
					<td><%= Html.Encode(item.LastName) %></td>
                    <td><%= Html.Encode(item.FirstName) %></td>
                    <td><%= Html.Encode(item.CountryCode) %></td>
					<td><%= Html.Encode(item.IsActiveFlag == true ? "True" : "False") %></td>
                    <td><%= Html.Encode(item.RoboticUserFlag == true ? "True" : "False")%></td>
					<td><%= Html.Encode(item.InternalRemarks)%></td>
					<td><%= Html.RouteLink("GDS Access Rights", "Main", new { controller = "ThirdPartyUserGDSAccessRight", action = "ListUnDeleted", id = item.ThirdPartyUserId }, new { title = "View Third Party User's GDS Access Rights" })%></td>
                    <td><%= Html.RouteLink("View", "Main", new { controller = "ThirdPartyUser", action = "View", id = item.ThirdPartyUserId }, new { title = "View Third Party User" })%></td>
                    <td>
						<%if (item.HasWriteAccess == true){ %>
							<%= Html.RouteLink("Edit", "Main", new { controller = "ThirdPartyUser", action = "Edit", id = item.ThirdPartyUserId }, new { title = "Edit Third Party User" })%>
						<% } %>
                    </td>
                    <td>
						<%if (item.HasWriteAccess == true){ %>
							<%= Html.RouteLink("UnDelete", "Main", new { controller = "ThirdPartyUser", action = "UnDelete", id = item.ThirdPartyUserId }, new { title = "UnDelete Third Party User" })%>
						<% } %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                    <td colspan="12" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
							<%= Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new {title="Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
							<%= Html.RouteLink("Next Page>>>", "ListMain", new { controller = "ThirdPartyUser", action = "ListDeleted", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
               
		        <tr> 
					<td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="11">
						<%= Html.ActionLink("UnDeleted Third Party Users", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted Third Party Users" })%>
			        </td>
		        </tr> 
            </table>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/ThirdPartyUserList.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
Third Party Users (Deleted)
</asp:Content>