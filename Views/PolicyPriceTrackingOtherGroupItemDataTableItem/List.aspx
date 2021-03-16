<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyPriceTrackingOtherGroupItemDataTableItemsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Table Data Items</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Table Data Items</div></div>
        <div id="content">
			<table width="100%" cellpadding="0" cellspacing="0" border="0">
               <tr> 
			        <th class="row_header row_header_white" colspan="<%= Html.Encode(Model.PolicyPriceTrackingOtherGroupItemDataTableItems.Columns.Count + 1) %>">
						Table Name - <%= Html.Encode(Model.PolicyOtherGroupHeader.TableName) %>
			        </th> 
		        </tr>
				<tr>
					<% foreach (System.Data.DataColumn col in Model.PolicyPriceTrackingOtherGroupItemDataTableItems.Columns) { 
						if(col.ColumnName != "PolicyPriceTrackingOtherGroupItemDataTableRowId") { %>
							<th><%= Html.RouteLink(col.ColumnName, "List", new { id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = col.ColumnName, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
						<% }  
					}%>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
				</tr>
				<% foreach (System.Data.DataRow row in Model.PolicyPriceTrackingOtherGroupItemDataTableItems.Rows) { %>
					<tr>
						<% foreach (System.Data.DataColumn col in Model.PolicyPriceTrackingOtherGroupItemDataTableItems.Columns) { 
								if(col.ColumnName != "PolicyPriceTrackingOtherGroupItemDataTableRowId") { %>           
									<td class="preserve-whitespace linkify"><%= System.Web.HttpUtility.HtmlDecode(row[col.ColumnName].ToString()) %></td>
							<% } 
						}%>
						<td width="8%">
							<%if(Model.HasWriteAccess){%>
								<%=Html.RouteLink("Edit", "Default", new { action = "Edit", id = Html.Encode(row["PolicyPriceTrackingOtherGroupItemDataTableRowId"]), policyGroupId = Model.PolicyGroup.PolicyGroupId, PolicyOtherGroupHeaderId = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId }, new { title = "Edit" })%>
							<%} %>
						</td>
						<td width="10%">
							<%if(Model.HasWriteAccess){%>
								<%=Html.RouteLink("Delete", "Default", new { action = "Delete", id = Html.Encode(row["PolicyPriceTrackingOtherGroupItemDataTableRowId"]), policyGroupId = Model.PolicyGroup.PolicyGroupId, PolicyOtherGroupHeaderId = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId }, new { title = "Delete" })%>
							<%} %>
						</td>
					</tr>
				<% } %>
                <tr>
                    <td colspan="<%= Html.Encode(Model.PolicyPriceTrackingOtherGroupItemDataTableItems.Columns.Count + 1) %>" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.PageIndex != 1){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PageIndex - 1), id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId, sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.PageIndex != Model.TotalPages && Model.TotalPages > 0){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PageIndex + 1),  id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId, sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0) { %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left" colspan="<%= Model.PolicyPriceTrackingOtherGroupItemDataTableItems.Columns.Count - 1 %>">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="2">
						<%if (Model.HasWriteAccess){ %>
							<%= Html.RouteLink("Create Table Data Item", "Main", new { action = "Create", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Create Table Data Item" })%>
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
		$('#search').show();
		$('#breadcrumb').css('width', '775px');
		$("#frmSearch input[name='ft']").attr('name', 'id').val(<%=Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId%>);
		$("#frmSearch input[name='id']").after('<input type="hidden" name="policyGroupId" value="<%=Model.PolicyGroup.PolicyGroupId%>" />');
	})
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/PolicyPriceTrackingOtherGroupItemDataTableItem.mvc/List");
		$("#frmSearch").submit();
	});
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Price Tracking Other Group Items", "Default", new { controller = "PolicyPriceTrackingOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Price Tracking Other Group Items" })%> &gt;
<%=Html.RouteLink(Model.PolicyOtherGroupHeader.Label, "Default", new { controller = "PolicyPriceTrackingOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyOtherGroupHeader.Label })%> &gt;
<%=Html.RouteLink(Model.PolicyOtherGroupHeader.TableName, "Default", new { controller = "PolicyPriceTrackingOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyOtherGroupHeader.TableName })%> &gt;
Table Data
</asp:Content>