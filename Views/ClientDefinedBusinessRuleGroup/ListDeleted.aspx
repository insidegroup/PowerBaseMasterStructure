<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedBusinessRuleGroupsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Business Rules Group</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Business Rules Group (Deleted)</div></div>
	<div id="content">
        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
            <tr> 
			    <th width="45%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "ClientDefinedRuleGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Model.Filter, category = Model.Category, clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName })%></th> 
                <th width="20%"><%= Html.RouteLink("Category", "ListMain", new { page = 1, sortField = "Category", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Model.Filter, category = Model.Category, clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName})%></th> 
			    <th width="10%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "LinkedItemCount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Model.Filter, category = Model.Category, clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName })%></th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="8%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
		    </tr> 
            <%
            foreach (var item in Model.ClientDefinedBusinessRuleGroups) { 
            %>
            <tr>
                <td><%= Html.Encode(CWTStringHelpers.TrimString(item.ClientDefinedRuleGroupName, 45))%></td>
				<td><%= Html.Encode(item.Category) %></td>
                <td><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%></td>       
	            <td><%= Html.RouteLink("View", "Default", new { action="View", id = item.ClientDefinedRuleGroupId }, new { title = "View" })%></td>
				<td><%= Html.RouteLink("Hierarchy", "Default", new { id = item.ClientDefinedRuleGroupId, action = "HierarchySearch", h = item.HierarchyType }, new { title = "Hierarchy" })%></td>   
                <td>
                    <%if (Model.HasDomainWriteAccess) { %>
						<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ClientDefinedRuleGroupId, filter = Model.Filter, category = Model.Category, clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
					<%if (Model.HasDomainWriteAccess) { %>
						<%=  Html.RouteLink("UnDelete", "Default", new { action = "UnDelete", id = item.ClientDefinedRuleGroupId }, new { title = "UnDelete" })%>
                    <%} %>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="7" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.ClientDefinedBusinessRuleGroups.HasPreviousPage) { %><%= Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.ClientDefinedBusinessRuleGroups.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Model.Filter, category = Model.Category, clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"><% if (Model.ClientDefinedBusinessRuleGroups.HasNextPage) {  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { page = (Model.ClientDefinedBusinessRuleGroups.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Model.Filter, category = Model.Category, clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.ClientDefinedBusinessRuleGroups.TotalPages > 0) { %>Page <%=Model.ClientDefinedBusinessRuleGroups.PageIndex%> of <%=Model.ClientDefinedBusinessRuleGroups.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
                <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="6">
					<%= Html.ActionLink("UnDeleted Rules Groups", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted Rules Groups" })%>&nbsp;
                </td> 
            </tr> 
		</table>
	</div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ClientDefinedBUsinessRuleGroup.js")%>" type="text/javascript"></script>
</asp:Content>