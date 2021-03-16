<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderLabelLanguagesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Translations</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Label Translations</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, page = 1, sortField = "LanguageName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Translation", "ListMain", new { id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, page = 1, sortField = "LabelTranslation", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.PolicyOtherGroupHeaderLabelLanguages) { 
                %>
                <tr>
                    <td width="40%"><%= Html.Encode(item.LanguageName) %></td>
					<td width="40%"><%= Html.Encode(item.LabelTranslation) %></td>
					<td width="10%">
						<%if(item.HasWriteAccess.Value && item.LanguageCode.ToLower() != "en-gb"){%>
							<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PolicyOtherGroupHeaderLabelLanguageId }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="10%">
						<%if(item.HasWriteAccess.Value && item.LanguageCode.ToLower() != "en-gb"){%>
							<%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PolicyOtherGroupHeaderLabelLanguageId }, new { title = "Delete" })%>
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
                            <% if (Model.PolicyOtherGroupHeaderLabelLanguages.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PolicyOtherGroupHeaderLabelLanguages.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.PolicyOtherGroupHeaderLabelLanguages.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PolicyOtherGroupHeaderLabelLanguages.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.PolicyOtherGroupHeaderLabelLanguages.TotalPages > 0) { %>Page <%=Model.PolicyOtherGroupHeaderLabelLanguages.PageIndex%> of <%=Model.PolicyOtherGroupHeaderLabelLanguages.TotalPages%><%} %></div>
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
							<%= Html.RouteLink("Create Label Translation", "Main", new { action = "Create", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId }, new { @class = "red", title = "Create Label Translation" })%>
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
Label Trans
</asp:Content>