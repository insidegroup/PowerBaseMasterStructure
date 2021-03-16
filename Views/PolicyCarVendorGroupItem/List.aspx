<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyCarVendorGroupItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Car Vendor Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="3%" class="row_header_left"><%=  Html.RouteLink("Seq.", "List", new { controller = "PolicyCarVendorGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "Sequence", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Sequence" })%></th>
                    <th width="19%"><%= Html.RouteLink("Location", "List", new { controller = "PolicyCarVendorGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "PolicyLocationName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Location" })%></th>
                    <th width="19%"><%= Html.RouteLink("Supplier", "List", new { controller = "PolicyCarVendorGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Supplier" })%></th>
                    <th width="15%"><%= Html.RouteLink("Status", "List", new { controller = "PolicyCarVendorGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "PolicyCarStatusDescription", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Status" })%></th>
                    <th width="19%"><%= Html.RouteLink("Enabled Date", "List", new { controller = "PolicyCarVendorGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "EnabledDate", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Enabled Date" })%></th>
                    <th width="10%"><%= Html.RouteLink("Advice", "List", new { controller = "PolicyCarVendorGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Advice" })%></th>
                    <th width="12%">&nbsp;</th> 
                    <th width="4%">&nbsp;</th> 
                    <th width="4%">&nbsp;</th> 
                    <th width="7%" class="row_header_right">&nbsp;</th> 
                </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.SequenceNumber) %></td>
                    <td><%= Html.Encode(item.PolicyLocationName) %></td>
                    <td><%= Html.Encode(item.SupplierName) %></td>
                    <td><%= Html.Encode(item.PolicyCarStatusDescription) %></td>
                    <td><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>                    
                    <td><%= Html.Encode(item.Advice > 0 ? "Yes" : "No")%></td>
                    <td>
                        <%= Html.ActionLink("Car Advice", "List", "CarAdvice", new { id = item.PolicyCarVendorGroupItemId }, null)%>
                    </td>
                    <td><%=  Html.ActionLink("View", "View", new { id = item.PolicyCarVendorGroupItemId })%></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.PolicyCarVendorGroupItemId })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.PolicyCarVendorGroupItemId })%>
                        <%} %>
                    </td>             
                </tr>
                <% 
                } 
                %>
                 <td colspan="10" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "PolicyCarVendorGroupItem", action = "List", id = ViewData["PolicyGroupID"].ToString(), page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "PolicyCarVendorGroupItem", action = "List", id = ViewData["PolicyGroupID"].ToString(), page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="10">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.RouteLink("Edit Sequencing", "Default", new { action = "EditSequence", id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Edit Sequencing" })%>
			        <%= Html.RouteLink("Create Policy Car Vendor Group Item", "Default",  new { action="Create", id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Create Policy Car Vendor Group Item" })%>
			        <% } %> 
			        </td> 
		        </tr> 
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(ViewData["PolicyGroupName"].ToString(), "Default", new { controller = "PolicyGroup", action = "View", id = ViewData["PolicyGroupId"] }, new { title = ViewData["PolicyGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
Policy Car Vendor Group Items
</asp:Content>
