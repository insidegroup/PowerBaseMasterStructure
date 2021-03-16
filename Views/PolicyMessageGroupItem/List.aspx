<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Message Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="22%" class="row_header_left"><%=  Html.RouteLink("Routing or Policy Location", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "RoutingOrLocationName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Order" })%></th>
                    <th width="10%"><%=  Html.RouteLink("Product", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Name" })%></th>
                    <th width="26%"><%=  Html.RouteLink("Supplier", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Status" })%></th>
                    <th width="21%"><%=  Html.RouteLink("Policy Message Name", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "PolicyMessageGroupItemName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Status" })%></th>
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.PolicyMessageGroupItems) {
                    string controllerName = "PolicyMessageGroupItemAir";
                    switch (item.ProductId)
                    {
                        case 3:
                            controllerName = "PolicyMessageGroupItemCar";
                            break;
                        case 2:
                            controllerName = "PolicyMessageGroupItemHotel";
                            break;
                    }
                %>
                <tr>
                    <td><%= Html.Encode(item.RoutingOrLocationName )%></td>
                    <td><%= Html.Encode(item.ProductName)%></td>
                    <td><%= Html.Encode(item.SupplierName) %></td>
                    <td><%= Html.Encode(item.PolicyMessageGroupItemName) %></td>
                    <td>
                        <%= Html.RouteLink("Translations", "List", new { controller = "PolicyMessageGroupItemlanguage", action = "List", id = item.PolicyMessageGroupItemId }, new { title = "Translations" })%>
                    </td>
                    <td><%=  Html.RouteLink("View", "List", new { action = "View", controller = controllerName, id = item.PolicyMessageGroupItemId }, new { title = "View" })%></td>
                    <td>
                        <%if(Model.HasWriteAccess){%>
                        <%=  Html.RouteLink("Edit", "List", new { action = "Edit", controller = controllerName, id = item.PolicyMessageGroupItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (Model.HasWriteAccess)
                          {%>
                        <%=  Html.RouteLink("Delete", "List", new { action = "Delete", controller = controllerName, id = item.PolicyMessageGroupItemId }, new { title = "Delete" })%>
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
                         <% if (Model.PolicyMessageGroupItems.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyMessageGroupItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.PolicyMessageGroupItems.HasNextPage)
                           {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyMessageGroupItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.PolicyMessageGroupItems.TotalPages > 0)
                                                     { %>Page <%=Model.PolicyMessageGroupItems.PageIndex%> of <%=Model.PolicyMessageGroupItems.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
            
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="7">
			        <%if (Model.HasWriteAccess)
             { %>
			        <%= Html.RouteLink("Create Policy Message Group Item", "Default", new { action = "SelectType", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Create Policy Message Group Item" })%>
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
		$("#frmSearch").attr("action", "/PolicyMessageGroupItem.mvc/List");
		$("#frmSearch").submit();
	});
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
Policy Message Group Items
</asp:Content>