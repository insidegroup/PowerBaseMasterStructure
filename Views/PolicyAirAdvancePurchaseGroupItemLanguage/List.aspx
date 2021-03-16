<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirParameterGroupItemLanguagesVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Air Advance Purchase Advice</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Air Advance Purchase Advice</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { id = Model.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId, policyGroupId = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "LanguageName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Air Advance Purchase Advice", "ListMain", new { id = Model.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId, policyGroupId = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Translation", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.PolicyAirParameterGroupItemLanguages) { 
                %>
                <tr>
                    <td width="30%"><%= Html.Encode(item.LanguageName) %></td>
					<td width="55%" class="wrap-text"><%= Html.Encode(item.Translation) %></td>
					<td width="5%">
						<%= Html.RouteLink("View", "Main", new { action = "View", policyGroupId = Model.PolicyGroup.PolicyGroupId, policyAirParameterGroupItemId = item.PolicyAirParameterGroupItemId, languageCode = item.LanguageCode }, new { title = "View" })%>
					</td>
					<td width="5%">
						<%if(item.HasWriteAccess.Value){%>
							<%= Html.RouteLink("Edit", "Main", new { action = "Edit", policyGroupId = Model.PolicyGroup.PolicyGroupId, policyAirParameterGroupItemId = item.PolicyAirParameterGroupItemId, languageCode = item.LanguageCode }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="5%">
						<%if(item.HasWriteAccess.Value){%>
							<%= Html.RouteLink("Delete", "Main", new { action = "Delete", policyGroupId = Model.PolicyGroup.PolicyGroupId, policyAirParameterGroupItemId = item.PolicyAirParameterGroupItemId, languageCode = item.LanguageCode }, new { title = "Delete" })%>
                        <%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.PolicyAirParameterGroupItemLanguages.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PolicyAirParameterGroupItemLanguages.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.PolicyAirParameterGroupItemLanguages.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PolicyAirParameterGroupItemLanguages.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.PolicyAirParameterGroupItemLanguages.TotalPages > 0) { %>Page <%=Model.PolicyAirParameterGroupItemLanguages.PageIndex%> of <%=Model.PolicyAirParameterGroupItemLanguages.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="4">
						<%if (Model.HasWriteAccess){ %>
							<%= Html.RouteLink("Create Air Advance Purchase Advice", "Main", new { action = "Create", id = Model.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId, policyGroupId = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Create Air Advance Purchase Advice" })%>
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
			$('#search').hide();
			$('#search_wrapper').css('height', '32px');
			$('#breadcrumb').css('width', '775px');
			$('#breadcrumb').css('width', 'auto');
		})
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Time Window Group Items", "Default", new { controller = "PolicyAirTimeWindowGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Time Window Group Items" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)) %> &gt;
Air Advance Purchase Advice
</asp:Content>