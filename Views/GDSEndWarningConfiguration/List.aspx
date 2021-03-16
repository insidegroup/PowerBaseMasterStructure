<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSEndWarningConfigurationsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - GDS Response Messages</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Response Messages</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("GDS", "ListMain", new { page = 1, sortField = "GDSName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("GDS Response Message", "ListMain", new { page = 1, sortField = "IdentifyingWarningMessage", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Behaviour", "ListMain", new { page = 1, sortField = "GDSEndWarningBehaviorTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Advice", "ListMain", new { page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.GDSEndWarningConfigurations) { 
                %>
                <tr>
                    <td width="20%"><%= Html.Encode(item.GDSName) %></td>
					<td width="30%" class="wrap-text"><%= Html.Encode(item.IdentifyingWarningMessage) %></td>
					<td width="15%"><%= Html.Encode(item.GDSEndWarningBehaviorTypeDescription) %></td>
					<td width="15%">
						<% if(Html.Encode(item.Advice) == "0") { %>
							No
						<% } else {  %>
							Yes
						<% } %>
					</td>
					<td width="5%">
				        <%=  Html.RouteLink("Advice", "Default", new { controller = "GDSEndWarningConfigurationLanguage", action = "List", id = item.GDSEndWarningConfigurationId }, new { title = "Items" })%>
                	</td>
					<td width="5%">
						<%=  Html.RouteLink("View", "Default", new { action = "View", id = item.GDSEndWarningConfigurationId }, new { title = "View" })%>
					</td>
					<td width="5%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.GDSEndWarningConfigurationId }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="5%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.GDSEndWarningConfigurationId }, new { title = "Delete" })%>
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
                            <% if (Model.GDSEndWarningConfigurations.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.GDSEndWarningConfigurations.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.GDSEndWarningConfigurations.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.GDSEndWarningConfigurations.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.GDSEndWarningConfigurations.TotalPages > 0) { %>Page <%=Model.GDSEndWarningConfigurations.PageIndex%> of <%=Model.GDSEndWarningConfigurations.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
					<td class="row_footer_blank_right" colspan="11">
						<%if (Model.HasWriteAccess){ %>
							<%= Html.RouteLink("Create GDS Response Message", "Main", new { action = "Create" }, new { @class = "red", title = "Create GDS Response Message" })%>
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
		$('#search').show();
	})
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/GDSEndWarningConfiguration.mvc/List");
		$("#frmSearch").submit();
	});
</script>
</asp:Content>