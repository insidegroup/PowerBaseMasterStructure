
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyCarOtherGroupItemLanguagesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Translations</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Translations</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "LanguageName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Translation", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Translation", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.PolicyCarVendorGroupItemLanguages) { 
                %>
                <tr>
                    <td width="40%"><%= Html.Encode(item.LanguageName) %></td>
					<td width="40%" class="wrap-text linkify"><%= System.Web.HttpUtility.HtmlDecode(item.Translation) %></td>
					<td width="10%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PolicyCarOtherGroupItemLanguageId, policyGroupId = item.PolicyGroupId, policyOtherGroupHeaderId = item.PolicyOtherGroupHeaderId }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="10%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PolicyCarOtherGroupItemLanguageId, policyGroupId = item.PolicyGroupId, policyOtherGroupHeaderId = item.PolicyOtherGroupHeaderId }, new { title = "Delete" })%>
                        <%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="4" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.PolicyCarVendorGroupItemLanguages.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PolicyCarVendorGroupItemLanguages.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.PolicyCarVendorGroupItemLanguages.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PolicyCarVendorGroupItemLanguages.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.PolicyCarVendorGroupItemLanguages.TotalPages > 0) { %>Page <%=Model.PolicyCarVendorGroupItemLanguages.PageIndex%> of <%=Model.PolicyCarVendorGroupItemLanguages.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="3">
						<%if (Model.HasWriteAccess){ %>
							<%= Html.RouteLink("Create Translation", "Main", new { action = "Create", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Create Translation" })%>
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
		$('#search_wrapper').css('height', '24px');
		$('#breadcrumb').css('width', '775px');
		$('#breadcrumb').css('width', 'auto');
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Car Other Group Items", "Default", new { controller = "PolicyCarOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Car Other Group Items" })%> &gt;
<%= Html.Encode(Model.PolicyOtherGroupHeader.Label) %> &gt;
Translations
</asp:Content>