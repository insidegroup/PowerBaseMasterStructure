<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldItemsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="25%" class="row_header_left"><%= Html.RouteLink("Optional Field Description", "ListMain", new { page = 1, sortField = "OptionalFieldName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.OptionalFieldGroup.OptionalFieldGroupId })%></th> 
			        <th width="20%"><%= Html.RouteLink("Product Name", "ListMain", new { page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.OptionalFieldGroup.OptionalFieldGroupId })%></th> 
			        <th width="20%"><%= Html.RouteLink("Supplier Name", "ListMain", new { page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.OptionalFieldGroup.OptionalFieldGroupId })%></th> 
			        <th width="20%"><%= Html.RouteLink("Order Number", "ListMain", new { page = 1, sortField = "OptionalFieldDisplayOrder", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.OptionalFieldGroup.OptionalFieldGroupId })%></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.OptionalFieldItems) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.OptionalFieldName)%></td>                    
                    <td><%= Html.Encode(item.ProductName) %></td>
					<td><%= Html.Encode(item.SupplierName)%></td>
                    <td><%= Html.Encode(item.OptionalFieldDisplayOrder) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.OptionalFieldItemId }, new { title = "View" })%> </td>
                    <td><%if (Model.HasWriteAccess){ %><%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.OptionalFieldItemId }, new { title = "Edit" })%><%}%></td>
                    <td><%if (Model.HasWriteAccess){ %><%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.OptionalFieldItemId }, new { title = "Delete" })%><%}%></td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.OptionalFieldItems.HasPreviousPage)
                               { %>
                            <%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.OptionalFieldItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.OptionalFieldGroup.OptionalFieldGroupId }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.OptionalFieldItems.HasNextPage)
                               { %>
                            <%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.OptionalFieldItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.OptionalFieldGroup.OptionalFieldGroupId}, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.OptionalFieldItems.TotalPages > 0)
                                                         { %>Page <%=Model.OptionalFieldItems.PageIndex%> of <%=Model.OptionalFieldItems.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="6">
					<% if(Model.CanEditOrder) { %>
						<%= Html.RouteLink("Edit Order", "Default", new { controller = "OptionalFieldItem", action = "SelectOptionalFieldToOrder", id = Model.OptionalFieldGroup.OptionalFieldGroupId }, new { @class = "red", title = "Edit Order" })%>
					<% } %>
					<% if(Model.CanCreate) { %>
						<%= Html.RouteLink("Create Optional Field Item", "Main", new {action = "Create", id = Model.OptionalFieldGroup.OptionalFieldGroupId}, new {@class = "red", title = "Create Optional Field Item"}) %>
					<% } %>
			    </td>  
		    </tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
    $(document).ready(function () {
    	$('#menu_passivesegmentbuilder').click();
    	$('#menu_passivesegmentbuilder_optionalfields').click();
    	$("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })

 </script>
 </asp:Content>

  <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Optional Field Groups", "Main", new { controller = "OptionalFieldGroup", action = "ListUnDeleted" }, new { title = "Optional Field Groups" })%> &gt;
<%=Html.RouteLink(Model.OptionalFieldGroup.OptionalFieldGroupName, "Main", new { controller = "OptionalFieldGroup", action = "View", id = Model.OptionalFieldGroup.OptionalFieldGroupId }, new { title = Model.OptionalFieldGroup.OptionalFieldGroupName })%> &gt;
Optional Field Group Items
</asp:Content>

