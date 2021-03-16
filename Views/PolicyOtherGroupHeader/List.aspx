<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeadersVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Other Group Headers</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Other Group Headers</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Order", "ListMain", new { page = 1, sortField = "DisplayOrder", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Service Type", "ListMain", new { page = 1, sortField = "PolicyOtherGroupHeaderServiceTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Product", "ListMain", new { page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("SubProduct", "ListMain", new { page = 1, sortField = "SubProductName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Label", "ListMain", new { page = 1, sortField = "Label", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Table Name", "ListMain", new { page = 1, sortField = "TableName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.PolicyOtherGroupHeaders) { 
                %>
                <tr>
                    <td width="4%"><%= Html.Encode(item.DisplayOrder) %></td>
					<td width="10%"><%= Html.Encode(item.PolicyOtherGroupHeaderServiceTypeDescription) %></td>
					<td width="8%"><%= Html.Encode(item.ProductName) %></td>
					<td width="8%"><%= Html.Encode(item.SubProductName) %></td>
					<td width="20%" class="wrap-text"><%= Html.Encode(item.Label) %></td>
					<td width="15%" class="wrap-text"><%= Html.Encode(item.TableName) %></td>
					<td width="8%">
				        <%=  Html.RouteLink("Label Trans", "Default", new { controller = "PolicyOtherGroupHeaderLabelLanguage", action = "List", id = item.PolicyOtherGroupHeaderId }, new { title = "Label Trans" })%>
                	</td>
					<td width="10%">
				        <% if (item.TableDefinitionsAttachedFlag == true){ %>
							<%= Html.RouteLink("Column Names", "Default", new { controller = "PolicyOtherGroupHeaderColumnName", action = "List", id = item.PolicyOtherGroupHeaderId }, new { title = "Column Names" })%>
						 <%}%>
                	</td>
					<td width="12%">
						<% if (item.TableDefinitionsAttachedFlag == true){ %>
							<%=  Html.RouteLink("Table Name Trans", "Default", new { controller = "PolicyOtherGroupHeaderTableNameLanguage", action = "List", id = item.PolicyOtherGroupHeaderId }, new { title = "Table Name Trans" })%>
						<%}%>
					</td>
					<td width="4%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PolicyOtherGroupHeaderId }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="5%">
						<%if(item.HasWriteAccess.Value){%>
							<%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PolicyOtherGroupHeaderId }, new { title = "Delete" })%>
                        <%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="11" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.PolicyOtherGroupHeaders.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PolicyOtherGroupHeaders.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.PolicyOtherGroupHeaders.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PolicyOtherGroupHeaders.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.PolicyOtherGroupHeaders.TotalPages > 0) { %>Page <%=Model.PolicyOtherGroupHeaders.PageIndex%> of <%=Model.PolicyOtherGroupHeaders.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left" colspan="3">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="8">
						<%if (Model.HasWriteAccess){ %>
							<%if (Model.CanEditOrder){ %>
								<%= Html.ActionLink("Edit Order", "SelectPolicyOtherGroupHeaderToOrder",  null, new { @class = "red", title = "Edit Order" })%>&nbsp;
							<% } %>
							<%= Html.RouteLink("Create Policy Other Group Header", "Main", new { action = "Create" }, new { @class = "red", title = "Create Policy Other Group Header" })%>
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
		$('#search_wrapper').css('height', '24px');
	})
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/PolicyOtherGroupHeader.mvc/List");
		$("#frmSearch").submit();
	});
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
Policy Other Group Headers
</asp:Content>