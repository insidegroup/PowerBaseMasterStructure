<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPriceTrackingHandlingFeeItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="30%" class="row_header_left"><%=  Html.RouteLink("Price Tracking System", "ListMain", new { controller = "PriceTrackingHandlingFeeItem", action = "List", id = ViewData["PriceTrackingHandlingFeeGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PriceTrackingSystemName" }, new { title = "Price Tracking System" })%></th>
                    <th width="15%"><%= Html.RouteLink("Product", "ListMain", new { controller = "PriceTrackingHandlingFeeItem", action = "List", id = ViewData["PriceTrackingHandlingFeeGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ProductName" }, new { title = "Product" })%></th>
                    <th width="25%"><%= Html.RouteLink("Saving Amount Percentage", "ListMain", new { controller = "PriceTrackingHandlingFeeItem", action = "List", id = ViewData["PriceTrackingHandlingFeeGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "SavingAmountPercentage" }, new { title = "Saving Amount Percentage" })%></th>
                    <th width="20%"><%= Html.RouteLink("Handling Fee", "ListMain", new { controller = "PriceTrackingHandlingFeeItem", action = "List", id = ViewData["PriceTrackingHandlingFeeGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "HandlingFee" }, new { title = "Handling Fee" })%></th>
			        <th width="3%">&nbsp;</th> 
			        <th width="3%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) {  
                %>
                <tr>
                    <td><%= Html.Encode(item.PriceTrackingSystemName)%></td>
                    <td><%= Html.Encode(item.ProductName) %></td>
                    <td><%= Html.Encode(item.SavingAmountPercentage) %></td>
                    <td><%= Html.Encode(item.HandlingFee) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { id = item.PriceTrackingHandlingFeeItemId, action = "View" }, new { title = "View" })%></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Edit", "Default", new { id = item.PriceTrackingHandlingFeeItemId, action = "Edit" }, new { title = "Edit" })%><%} %></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Delete", "Default", new { id = item.PriceTrackingHandlingFeeItemId, action = "Delete" }, new { title = "Delete" })%><%} %></td>                
                </tr>
                <% 
                } 
                %>
                 <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { id = ViewData["PriceTrackingHandlingFeeGroupId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { id = ViewData["PriceTrackingHandlingFeeGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> 
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
					<td class="row_footer_blank_right" colspan="6">
						<%if (ViewData["Access"] == "WriteAccess"){%>
							<%= Html.RouteLink("Create Price Tracking Item", "Main", new { id = ViewData["PriceTrackingHandlingFeeGroupId"], action = "Create" }, new { @class = "red", title = "Create Price Tracking Item" })%>
						<%} %>
			        </td> 
		        </tr> 
			</table>    
        </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#search').show();
        $('#menu_pricetracking').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val(<%=ViewData["PriceTrackingHandlingFeeGroupId"]%>);
    })

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/PriceTrackingHandlingFeeItem.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Groups", "Main", new { controller = "PriceTrackingHandlingFeeGroup", action = "ListUnDeleted", }, new { title = "Price Tracking Groups" })%> &gt;
<%=ViewData["PriceTrackingHandlingFeeGroupName"]%> &gt;
Price Tracking Items
</asp:Content>