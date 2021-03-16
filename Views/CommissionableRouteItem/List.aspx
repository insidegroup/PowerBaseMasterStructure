<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectCommissionableRouteItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Commissionable Route Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Commissionable Route Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="15%" class="row_header_left"><%=  Html.RouteLink("Product", "ListMain", new { controller = "CommissionableRouteItem", action = "List", id = ViewData["CommissionableRouteGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ProductName" }, new { title = "Product" })%></th>
                    <th width="15%"><%= Html.RouteLink("Supplier", "ListMain", new { controller = "CommissionableRouteItem", action = "List", id = ViewData["CommissionableRouteGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "SupplierName" }, new { title = "Supplier" })%></th>
			        <th width="10%"><%= Html.RouteLink("Indicator", "ListMain", new { controller = "CommissionableRouteItem", action = "List", id = ViewData["CommissionableRouteGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TravelIndicator" }, new { title = "Travel Indicator" })%></th>
			        <th width="10%"><%= Html.RouteLink("Class", "ListMain", new { controller = "CommissionableRouteItem", action = "List", id = ViewData["CommissionableRouteGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ClassOfTravel" }, new { title = "Class of Travel" })%></th>
			        <th width="10%"><%= Html.RouteLink("Amt/Perc", "ListMain", new { controller = "CommissionableRouteItem", action = "List", id = ViewData["CommissionableRouteGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "AmtPer" }, new { title = "Amt/Perc" })%></th>
			        <th width="10%"><%= Html.RouteLink("Currency", "ListMain", new { controller = "CommissionableRouteItem", action = "List", id = ViewData["CommissionableRouteGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CommissionAmountCurrencyCode" }, new { title = "Currency" })%></th>
			        <th width="10%"><%= Html.RouteLink("Neg Fare?", "ListMain", new { controller = "CommissionableRouteItem", action = "List", id = ViewData["CommissionableRouteGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "NegotiatedFareFlag" }, new { title = "Neg Fare?" })%></th>
			        <th width="10%"><%= Html.RouteLink("Remarks", "ListMain", new { controller = "CommissionableRouteItem", action = "List", id = ViewData["CommissionableRouteGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "RemarksOrRoute" }, new { title = "Remarks" })%></th>
			        <th width="3%">&nbsp;</th> 
			        <th width="3%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) {  
                %>
                <tr>
                    <td><%= Html.Encode(item.ProductName)%></td>
                    <td><%= Html.Encode(item.SupplierName) %></td>
					<td><%= Html.Encode(item.TravelIndicator) %></td>
					<td><%= Html.Encode(item.ClassOfTravel) %></td>
					<td><%= Html.Encode(item.AmtPer) %></td>
					<td><%= Html.Encode(item.CommissionAmountCurrencyCode) %></td>
					<td><%= Html.Encode(item.NegotiatedFareFlag == true ? "True" : "False") %></td>
					<td><%= Html.Encode(CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(item.RemarksOrRoute, 20))%></td>
                    <td><%= Html.RouteLink("View", "Default", new { id = item.CommissionableRouteItemId, action = "View" }, new { title = "View" })%></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Edit", "Default", new { id = item.CommissionableRouteItemId, action = "Edit" }, new { title = "Edit" })%><%} %></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Delete", "Default", new { id = item.CommissionableRouteItemId, action = "Delete" }, new { title = "Delete" })%><%} %></td>                
                </tr>
                <% 
                } 
                %>
                 <tr>
                    <td colspan="11" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { id = ViewData["CommissionableRouteGroupId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { id = ViewData["CommissionableRouteGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
                <tr> 
					<td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="10">
						<%if (ViewData["Access"] == "WriteAccess"){%>
							<%= Html.RouteLink("Create Commissionable Route Item", "Main", new { id = ViewData["CommissionableRouteGroupId"], action = "Create" }, new { @class = "red", title = "Create Commissionable Route Item" })%>
						<%} %>
			        </td> 
		        </tr> 
			</table>    
        </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#search').show();
        $('#menu_commissionableroutes').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val(<%=ViewData["CommissionableRouteGroupId"]%>);
    })

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/CommissionableRouteItem.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Commissionable Route Groups", "Main", new { controller = "CommissionableRouteGroup", action = "ListUnDeleted", }, new { title = "Commissionable Route Groups" })%> &gt;
<%=ViewData["CommissionableRouteGroupName"]%> &gt;
Commissionable Route Items
</asp:Content>