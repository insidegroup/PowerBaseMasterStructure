<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderColumnNameLanguagesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Column Name Translations</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Column Name Translations</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th class="row_header" style="background-color: #fff;" colspan="4">Column Name - <%=Html.Encode(Model.PolicyOtherGroupHeaderColumnName.ColumnName) %></th> 
		        </tr> 
				<tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { id = Model.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId, page = 1, sortField = "LanguageName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Translation", "ListMain", new { id = Model.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId, page = 1, sortField = "ColumnNameTranslation", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.PolicyOtherGroupHeaderColumnNameLanguages) { 
                %>
                <tr>
                    <td width="40%"><%= Html.Encode(item.LanguageName) %></td>
					<td width="40%"><%= Html.Encode(item.ColumnNameTranslation) %></td>
					<td width="10%"><%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PolicyOtherGroupHeaderColumnNameLanguageId }, new { title = "Edit" })%></td>
					<td width="10%"><%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PolicyOtherGroupHeaderColumnNameLanguageId }, new { title = "Delete" })%></td>
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="4" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.PolicyOtherGroupHeaderColumnNameLanguages.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PolicyOtherGroupHeaderColumnNameLanguages.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.PolicyOtherGroupHeaderColumnNameLanguages.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PolicyOtherGroupHeaderColumnNameLanguages.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.PolicyOtherGroupHeaderColumnNameLanguages.TotalPages > 0) { %>Page <%=Model.PolicyOtherGroupHeaderColumnNameLanguages.PageIndex%> of <%=Model.PolicyOtherGroupHeaderColumnNameLanguages.TotalPages%><%} %></div>
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
							<%= Html.RouteLink("Create Column Name Translation", "Main", new { action = "Create", id = Model.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId }, new { @class = "red", title = "Create Column Name Translation" })%>
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
		$('#menu_admin').click();
		$('#search').hide();
		$('#breadcrumb').css('width', 'auto');
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Policy Other Group Header", "Main", new { controller = "PolicyOtherGroupHeader", action = "List" }, new { title = "Policy Other Group Header" })%> &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeader.Label) %> &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeaderColumnName.ColumnName) %>
</asp:Content>