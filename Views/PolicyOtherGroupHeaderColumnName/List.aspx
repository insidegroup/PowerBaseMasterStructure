<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderColumnNamesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Column Names</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Column Names</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th class="row_header" style="background-color: #fff;" colspan="4">Table Name - <%=Html.Encode(Model.PolicyOtherGroupHeader.TableName) %></th> 
		        </tr> 
				<tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Order", "ListMain", new { id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, page = 1, sortField = "DisplayOrder", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Column Name", "ListMain", new { id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, page = 1, sortField = "ColumnName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
					foreach (var item in Model.PolicyOtherGroupHeaderColumnNames)
					{ 
                %>
                <tr>
                    <td width="10%"><%= Html.Encode(item.DisplayOrder) %></td>
					<td width="60%"><%= Html.Encode(item.ColumnName) %></td>
					<td width="14%">
						<%=  Html.RouteLink("Translations", "Default", new { controller = "PolicyOtherGroupHeaderColumnNameLanguage", action = "List", id = item.PolicyOtherGroupHeaderColumnNameId }, new { title = "Translations" })%>
					</td>
					<td width="8%">
						<%if(item.HasWriteAccess.Value ){%>
							<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PolicyOtherGroupHeaderColumnNameId }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="8%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PolicyOtherGroupHeaderColumnNameId }, new { title = "Delete" })%>
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
                            <% if (Model.PolicyOtherGroupHeaderColumnNames.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, page = (Model.PolicyOtherGroupHeaderColumnNames.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.PolicyOtherGroupHeaderColumnNames.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, page = (Model.PolicyOtherGroupHeaderColumnNames.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.PolicyOtherGroupHeaderColumnNames.TotalPages > 0) { %>Page <%=Model.PolicyOtherGroupHeaderColumnNames.PageIndex%> of <%=Model.PolicyOtherGroupHeaderColumnNames.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="3">
						<%if (Model.HasWriteAccess){ %>
							<% if(Model.PolicyOtherGroupHeaderColumnNames.Count > 1) { %>
								<%= Html.RouteLink("Edit Order", "Default", new { action = "EditSequence", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId }, new { @class = "red", title = "Edit Order" })%>
							<% } %>
							<%= Html.RouteLink("Create Column Name", "Main", new { action = "Create", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId }, new { @class = "red", title = "Create Column Name" })%>
						<% } %>
					</td>  
				</tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
	$(document).ready(function () {
		$("tr:even").addClass("row_odd");
		$("tr:odd").addClass("row_even");
		$('#menu_admin').click();
		$('#search').hide();
		$('#breadcrumb').css('width', 'auto');
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Policy Other Group Header", "Main", new { controller = "PolicyOtherGroupHeader", action = "List", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId }, new { title = "Policy Other Group Header" })%> &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeader.Label) %> &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeader.TableName) %>
</asp:Content>