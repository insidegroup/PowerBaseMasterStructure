<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.Policy24HSCOtherGroupItemsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy 24HSC Other Group Items</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy 24HSC Other Group Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Label", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Label", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>Description</th>
					<th><%= Html.RouteLink("Language", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "LanguageName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.Policy24HSCVendorGroupItems) {  %>
                <tr>
                    <td width="20%"><%= Html.Encode(item.Label) %></td>
					<td width="40%" class="wrap-text linkify"><%= System.Web.HttpUtility.HtmlDecode(item.Description) %></td>
					<td width="18%"><%= Html.Encode(item.LanguageName) %></td>
					<td width="10%">
				        <% if (item.TableDefinitionsAttachedFlag == true && item.HasColumns == "1"){ %>
							<%= Html.RouteLink("Table Data", "Default", new { controller = "Policy24HSCOtherGroupItemDataTableItem", action = "List", id = item.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId }, new { title = "Table Data" })%>
						 <%}%>
                	</td>
					<td width="12%">
						<%=  Html.RouteLink("Translations", "Default", new { controller = "Policy24HSCOtherGroupItemLanguage", action = "List", id = item.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId }, new { title = "Translations" })%>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.Policy24HSCVendorGroupItems.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", id = Model.PolicyGroup.PolicyGroupId, page = (Model.Policy24HSCVendorGroupItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.Policy24HSCVendorGroupItems.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", id = Model.PolicyGroup.PolicyGroupId, page = (Model.Policy24HSCVendorGroupItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.Policy24HSCVendorGroupItems.TotalPages > 0) { %>Page <%=Model.Policy24HSCVendorGroupItems.PageIndex%> of <%=Model.Policy24HSCVendorGroupItems.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="3">&nbsp;</td>  
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
		$('#breadcrumb').css('width', '775px');
		$("#frmSearch input[name='ft']").attr('name', 'id').val(<%=Model.PolicyGroup.PolicyGroupId%>);
	})
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/Policy24HSCOtherGroupItem.mvc/List");
		$("#frmSearch").submit();
	});
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
Policy 24HSC Other Group Items
</asp:Content>