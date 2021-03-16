<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSEndWarningConfigurationLanguagesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - GDS Response Message Advice</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Response Message Advice</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { id = Model.GDSEndWarningConfiguration.GDSEndWarningConfigurationId, page = 1, sortField = "LanguageCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("GDS Response Message Advice", "ListMain", new { id = Model.GDSEndWarningConfiguration.GDSEndWarningConfigurationId, page = 1, sortField = "AdviceMessage", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.GDSEndWarningConfigurationLanguages) { 
                %>
                <tr>
                    <td width="25%"><%= Html.Encode(item.LanguageName) %></td>
					<td width="60%"><%= Html.Encode(item.AdviceMessage) %></td>
					<td width="5%">
						<%=  Html.RouteLink("View", "Default", new { action = "View", id = item.GDSEndWarningConfigurationId, languageCode = item.LanguageCode }, new { title = "View" })%>
					</td>
					<td width="5%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.GDSEndWarningConfigurationId, languageCode = item.LanguageCode }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="5%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.GDSEndWarningConfigurationId, languageCode = item.LanguageCode }, new { title = "Delete" })%>
                        <%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="12" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.GDSEndWarningConfigurationLanguages.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.GDSEndWarningConfigurationLanguages.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.GDSEndWarningConfigurationLanguages.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.GDSEndWarningConfigurationLanguages.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.GDSEndWarningConfigurationLanguages.TotalPages > 0) { %>Page <%=Model.GDSEndWarningConfigurationLanguages.PageIndex%> of <%=Model.GDSEndWarningConfigurationLanguages.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
					<td class="row_footer_blank_right" colspan="11">
						<%if (Model.HasWriteAccess){ %>
							<%= Html.RouteLink("Create GDS Response Message Advice", "Main", new { action = "Create", id = Model.GDSEndWarningConfiguration.GDSEndWarningConfigurationId }, new { @class = "red", title = "Create GDS Response Message Advice" })%>
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
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("GDS Response Messages", "Main", new { controller = "GDSEndWarningConfiguration", action = "List" }, new { title = "GDS Response Messages" })%> &gt;
<%=CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Model.GDSEndWarningConfiguration.IdentifyingWarningMessage, 30) %> &gt;
GDS Response Message Advice
</asp:Content>