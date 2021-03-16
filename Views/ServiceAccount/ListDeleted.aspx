<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectServiceAccounts_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Service Accounts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Service Accounts (Deleted)</div></div>
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
			        <th width="13%" class="row_header_left"><%= Html.RouteLink("Service Account ID", "ListMain", new { controller = "ServiceAccount", action = "ListDeleted", page = 0, sortField = "ServiceAccountID", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["filterField_1"].ToString(), filterValue_1 = ViewData["filterValue_1"].ToString(), filterField_2 = ViewData["filterField_2"].ToString(), filterValue_2 = ViewData["filterValue_2"].ToString(), filterField_3 = ViewData["filterField_3"].ToString(), filterValue_3 = ViewData["filterValue_3"].ToString()  }, new { title = "Sort By Service Account ID" })%></th> 
			        <th width="14%"><%= Html.RouteLink("Service Account Name", "ListMain", new { controller = "ServiceAccount", action = "ListDeleted", page = 0, sortField = "ServiceAccountName", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["filterField_1"].ToString(), filterValue_1 = ViewData["filterValue_1"].ToString(), filterField_2 = ViewData["filterField_2"].ToString(), filterValue_2 = ViewData["filterValue_2"].ToString(), filterField_3 = ViewData["filterField_3"].ToString(), filterValue_3 = ViewData["filterValue_3"].ToString()  }, new { title = "Sort By Service Account Name" })%></th>
                    <th width="12%"><%= Html.RouteLink("Last Name", "ListMain", new { controller = "ServiceAccount", action = "ListDeleted", page = 0, sortField = "LastName", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["filterField_1"].ToString(), filterValue_1 = ViewData["filterValue_1"].ToString(), filterField_2 = ViewData["filterField_2"].ToString(), filterValue_2 = ViewData["filterValue_2"].ToString(), filterField_3 = ViewData["filterField_3"].ToString(), filterValue_3 = ViewData["filterValue_3"].ToString()  }, new { title = "Sort By Last Name" })%></th> 
			        <th width="15%"><%= Html.RouteLink("First Name", "ListMain", new { controller = "ServiceAccount", action = "ListDeleted", page = 0, sortField = "FirstName", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["filterField_1"].ToString(), filterValue_1 = ViewData["filterValue_1"].ToString(), filterField_2 = ViewData["filterField_2"].ToString(), filterValue_2 = ViewData["filterValue_2"].ToString(), filterField_3 = ViewData["filterField_3"].ToString(), filterValue_3 = ViewData["filterValue_3"].ToString()  }, new { title = "Sort By First Name" })%></th> 
			        <th width="8%"><%= Html.RouteLink("IsActive?", "ListMain", new { controller = "ServiceAccount", action = "ListDeleted", page = 0, sortField = "IsActiveFlag", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["filterField_1"].ToString(), filterValue_1 = ViewData["filterValue_1"].ToString(), filterField_2 = ViewData["filterField_2"].ToString(), filterValue_2 = ViewData["filterValue_2"].ToString(), filterField_3 = ViewData["filterField_3"].ToString(), filterValue_3 = ViewData["filterValue_3"].ToString()  }, new { title = "Sort By IsActive?" })%></th>  					  
			        <th width="14%"><%= Html.RouteLink("Internal Remarks", "ListMain", new { controller = "ServiceAccount", action = "ListDeleted", page = 0, sortField = "InternalRemarks", sortOrder = ViewData["NewSortOrder"].ToString(), filterField_1 = ViewData["filterField_1"].ToString(), filterValue_1 = ViewData["filterValue_1"].ToString(), filterField_2 = ViewData["filterField_2"].ToString(), filterValue_2 = ViewData["filterValue_2"].ToString(), filterField_3 = ViewData["filterField_3"].ToString(), filterValue_3 = ViewData["filterValue_3"].ToString()  }, new { title = "Sort By Internal Remarks" })%></th>  
			        <th width="12%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th>  
		        </tr>
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ServiceAccountId) %></td>
                    <td><%= Html.Encode(item.ServiceAccountName) %></td>
					<td><%= Html.Encode(item.LastName) %></td>
                    <td><%= Html.Encode(item.FirstName) %></td>
					<td><%= Html.Encode(item.IsActiveFlag == true ? "True" : "False") %></td>
					<td><%= Html.Encode(item.InternalRemarks)%></td>
					<td><%= Html.RouteLink("GDS Access Rights", "Main", new { controller = "ServiceAccountGDSAccessRight", action = "ListUnDeleted", id = item.ServiceAccountId }, new { title = "View Service Account's GDS Access Rights" })%></td>
                    <td><%= Html.RouteLink("View", "Main", new { controller = "ServiceAccount", action = "View", id = item.ServiceAccountId }, new { title = "View Service Account" })%></td>
                    <td>
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.RouteLink("Edit", "Main", new { controller = "ServiceAccount", action = "Edit", id = item.ServiceAccountId }, new { title = "Edit Service Account" })%>
						<% } %>
                    </td>
                    <td>
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.RouteLink("UnDelete", "Main", new { controller = "ServiceAccount", action = "UnDelete", id = item.ServiceAccountId }, new { title = "UnDelete Service Account" })%>
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
							<%= Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ServiceAccount", action = "ListDeleted", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filterField_1 = ViewData["filterField_1"].ToString(), filterValue_1 = ViewData["filterValue_1"].ToString(), filterField_2 = ViewData["filterField_2"].ToString(), filterValue_2 = ViewData["filterValue_2"].ToString(), filterField_3 = ViewData["filterField_3"].ToString(), filterValue_3 = ViewData["filterValue_3"].ToString()  }, new {title="Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
							<%= Html.RouteLink("Next Page>>>", "ListMain", new { controller = "ServiceAccount", action = "ListDeleted", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filterField_1 = ViewData["filterField_1"].ToString(), filterValue_1 = ViewData["filterValue_1"].ToString(), filterField_2 = ViewData["filterField_2"].ToString(), filterValue_2 = ViewData["filterValue_2"].ToString(), filterField_3 = ViewData["filterField_3"].ToString(), filterValue_3 = ViewData["filterValue_3"].ToString()  }, new { title = "Next Page" })%>
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
						<%= Html.ActionLink("UnDeleted Service Accounts", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted Service Accounts" })%>
			        </td>
		        </tr> 
            </table>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/ServiceAccountList.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
Service Accounts (Deleted)
</asp:Content>