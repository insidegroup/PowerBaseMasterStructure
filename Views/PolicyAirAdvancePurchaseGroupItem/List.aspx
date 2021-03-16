<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirParameterGroupItemsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Advance Purchase Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="20%" class="row_header_left"><%=  Html.RouteLink("Advance Purchase Days", "List", new { controller = "PolicyAirAdvancePurchaseGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "PolicyAirParameterValue", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Advance Purchase Days" })%></th>
                    <th width="24%"><%=  Html.RouteLink("Routing", "List", new { controller = "PolicyAirAdvancePurchaseGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Name", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Routing Name" })%></th>
                    <th width="27%" colspan="2">Travel date valid from/to</th>
                    <th width="10%"><%=  Html.RouteLink("Advice", "List", new { controller = "PolicyAirAdvancePurchaseGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Advice" })%></th>
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model.PolicyAirParameterGroupItems) { %>
                <tr>
                    <td><%= Html.Encode(item.PolicyAirParameterValue) %></td>
                    <td><% if(item.PolicyRoutingId != null) { %><%:Html.Encode(item.FromCode)%>  to  <%:Html.Encode(item.ToCode)%><%} %></td>
                    <td><%= Html.Encode(item.TravelDateValidFrom.HasValue ? item.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "")%></td>  
                    <td><%= Html.Encode(item.TravelDateValidTo.HasValue ? item.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "")%></td>  
                    <td><%= Html.Encode(item.Advice > 0 ? "Yes" : "No")%><//td>
                    <td><%= Html.RouteLink("Advice", "List", new { controller = "PolicyAirAdvancePurchaseGroupItemLanguage", action = "List", id = item.PolicyAirParameterGroupItemId, Model.PolicyGroup.PolicyGroupId }, new { title = "Advice" })%></td>
                    <td><%= Html.RouteLink("View", "List", new { action="View", id = item.PolicyAirParameterGroupItemId }, new  { title="View"})%></td>
                    <td>
                        <%if(Model.HasWriteAccess){%>
							<%=  Html.RouteLink("Edit", "List", new { action = "Edit", id = item.PolicyAirParameterGroupItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (Model.HasWriteAccess) {%>
							 <%=  Html.RouteLink("Delete", "List", new { action = "Delete", id = item.PolicyAirParameterGroupItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>             
                </tr>
                <% 
                } 
                %>
                <tr>
                <td colspan="9" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
							<% if (Model.PolicyAirParameterGroupItems.HasPreviousPage){ %>
								<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "PolicyAirAdvancePurchaseGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyAirParameterGroupItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
							<%}%>
                        </div>
                        <div class="paging_right">
							<% if (Model.PolicyAirParameterGroupItems.HasNextPage){ %>
								<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "PolicyAirAdvancePurchaseGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyAirParameterGroupItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
							<%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.PolicyAirParameterGroupItems.TotalPages > 0) { %>Page <%=Model.PolicyAirParameterGroupItems.PageIndex%> of <%=Model.PolicyAirParameterGroupItems.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
            
		    <tr> 
                <td class="row_footer_blank_left">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
					<a href="javascript:window.print();" class="red" title="Print">Print</a>
                </td>
				<td class="row_footer_blank_right" colspan="8">
					<%if (Model.HasWriteAccess) { %>
						<%= Html.RouteLink("Create Policy Air Advance Purchase Group Item", "Default", new { action = "Create", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Create Policy Air Advance Purchase Group Item" })%>
					<% } %> 
			    </td> 
		    </tr> 
        </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
        $('#search').hide();
    });
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
Policy Air Advance Purchase Group Items
</asp:Content>