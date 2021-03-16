<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedRuleGroupsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Rules Groups</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Rules Groups (Deleted)</div></div>
	<div id="content">
        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
            <tr> 
			    <th width="35%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "ClientDefinedRuleGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Model.Filter, clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, clientAccountNumber = Model.ClientAccountNumber, sourceSystemCode = Model.SourceSystemCode, travelerTypeName = Model.TravelerTypeName,  clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName })%></th> 
                <th width="20%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Model.Filter, clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, clientAccountNumber = Model.ClientAccountNumber, sourceSystemCode = Model.SourceSystemCode, travelerTypeName = Model.TravelerTypeName,  clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName })%></th> 
                <th width="10%"><%= Html.RouteLink("Category", "ListMain", new { page = 1, sortField = "Category", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Model.Filter, clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, clientAccountNumber = Model.ClientAccountNumber, sourceSystemCode = Model.SourceSystemCode, travelerTypeName = Model.TravelerTypeName,  clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName})%></th> 
			    <th width="10%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "LinkedItemCount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Model.Filter, clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, clientAccountNumber = Model.ClientAccountNumber, sourceSystemCode = Model.SourceSystemCode, travelerTypeName = Model.TravelerTypeName,  clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName })%></th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="8%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
		    </tr> 
            <%
            foreach (var item in Model.ClientDefinedRuleGroups) { 
            %>
            <tr>
                <td><%= Html.Encode(CWTStringHelpers.TrimString(item.ClientDefinedRuleGroupName, 45))%></td>
                <td><%= Html.Encode(item.HierarchyType) %></td>
				<td><%= Html.Encode(item.Category) %></td>
                <td><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%></td>       
	            <td><%= Html.RouteLink("View", "Default", new { action="View", id = item.ClientDefinedRuleGroupId }, new { title = "View" })%></td>
				<td><%= Html.RouteLink("Hierarchy", "Default", new { id = item.ClientDefinedRuleGroupId, action = "HierarchySearch", h = item.HierarchyType }, new { title = "Hierarchy" })%></td>   
                <td>
					<%if (Model.HasDomainWriteAccess && item.HasWriteAccess.Value) { %>
						<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ClientDefinedRuleGroupId, filter = Model.Filter, clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, clientAccountNumber = Model.ClientAccountNumber, sourceSystemCode = Model.SourceSystemCode, travelerTypeName = Model.TravelerTypeName,  clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if(item.HasWriteAccess.Value && item.HasWriteAccess.Value){%>
						<%=  Html.RouteLink("UnDelete", "Default", new { action = "UnDelete", id = item.ClientDefinedRuleGroupId }, new { title = "UnDelete" })%>
                    <%} %>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="8" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.ClientDefinedRuleGroups.HasPreviousPage) { %><%= Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.ClientDefinedRuleGroups.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Model.Filter, clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, clientAccountNumber = Model.ClientAccountNumber, sourceSystemCode = Model.SourceSystemCode, travelerTypeName = Model.TravelerTypeName,  clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"><% if (Model.ClientDefinedRuleGroups.HasNextPage) {  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { page = (Model.ClientDefinedRuleGroups.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Model.Filter, clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, clientAccountNumber = Model.ClientAccountNumber, sourceSystemCode = Model.SourceSystemCode, travelerTypeName = Model.TravelerTypeName,  clientDefinedRuleGroupName = Model.ClientDefinedRuleGroupName }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.ClientDefinedRuleGroups.TotalPages > 0) { %>Page <%=Model.ClientDefinedRuleGroups.PageIndex%> of <%=Model.ClientDefinedRuleGroups.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
                <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="7">
					<%= Html.ActionLink("UnDeleted Rules Groups", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted Rules Groups" })%>&nbsp;
                </td> 
            </tr> 
		</table>
	</div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ClientDefinedRuleGroup.js")%>" type="text/javascript"></script>
</asp:Content>