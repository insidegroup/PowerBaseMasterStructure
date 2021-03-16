<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirVendorGroupItemsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Vendor Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="3%" class="row_header_left"><%=  Html.RouteLink("Order", "List", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "SequenceNumber", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Order" })%></th>
                    <th width="20%"><%=  Html.RouteLink("Routing", "List", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Name", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Routing Name" })%></th>
                    <th width="12%"><%=  Html.RouteLink("Ranking", "List", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "AirVendorRanking", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Ranking" })%></th>
                    <th width="23%"><%=  Html.RouteLink("Supplier", "List", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Supplier" })%></th>
			        <th width="12%">Status</th> 
                    <th width="10%"><%=  Html.RouteLink("Advice", "List", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Advice" })%></th>
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model.PolicyAirVendorGroupItems) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.SequenceNumber) %></td>
                    <td><%= Html.Encode(item.Name)%></td>
                    <td><%= Html.Encode(item.AirVendorRanking)%></td>                    
                    <td><%= Html.Encode(item.SupplierName) %></td>
                    <td><%= Html.Encode(item.PolicyAirStatusDescription) %></td>
                    <td><%= Html.Encode(item.Advice > 0 ? "Yes" : "No")%></td>
                    <td>
                        <%= Html.RouteLink("Advice", "List", new { controller = "AirlineAdvice", action = "List", id = item.PolicyAirVendorGroupItemId }, new { title = "Airline Advice" })%>
                    </td>
                    <td><%=  Html.RouteLink("View", "List", new { action="View", id = item.PolicyAirVendorGroupItemId }, new  { title="View"})%></td>
                    <td>
                        <%if(Model.HasWriteAccess){%>
                        <%=  Html.RouteLink("Edit", "List", new { action = "Edit", id = item.PolicyAirVendorGroupItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (Model.HasWriteAccess)
                          {%>
                        <%=  Html.RouteLink("Delete", "List", new { action = "Delete", id = item.PolicyAirVendorGroupItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>             
                </tr>
                <% 
                } 
                %>
                <tr>
                <td colspan="10" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.PolicyAirVendorGroupItems.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyAirVendorGroupItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.PolicyAirVendorGroupItems.HasNextPage)
                           {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "PolicyAirVendorGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyAirVendorGroupItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.PolicyAirVendorGroupItems.TotalPages > 0)
                                                     { %>Page <%=Model.PolicyAirVendorGroupItems.PageIndex%> of <%=Model.PolicyAirVendorGroupItems.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
            
		        <tr> 
                    <td class="row_footer_blank_left " colspan="3">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>&nbsp;
						<%= Html.RouteLink("Export", "Default", new { action = "Export", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Export" })%>
                    </td>
			        <td class="row_footer_blank_right" colspan="7">
			        <%if (Model.HasWriteAccess) { %>
			            <%= Html.RouteLink("Import", "Default", new { action = "ImportStep1", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Import" })%>
                        <%= Html.RouteLink("Edit Order", "Default", new { action = "EditSequence", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Edit Order" })%>
			            <%= Html.RouteLink("Create", "Default", new { action = "Create", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Create" })%>
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
        $('#search').show();
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val(<%=Model.PolicyGroup.PolicyGroupId%>);
    });

	//Search
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/PolicyAirVendorGroupItem.mvc/List");
		$("#frmSearch").submit();
	});
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
Policy Air Vendor Group Items
</asp:Content>