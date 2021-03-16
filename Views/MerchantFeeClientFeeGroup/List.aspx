<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupMerchantFeesVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Merchant Fee Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="23%" class="row_header_left"><%= Html.RouteLink("Description", "ListMain", new { page = 1, sortField = "ClientFeeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="21%"><%= Html.RouteLink("Country", "ListMain", new { page = 1, sortField = "CountryName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="16%"><%= Html.RouteLink("CC Vendor", "ListMain", new { page = 1, sortField = "CreditCardVendorName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="8%"><%= Html.RouteLink("Product", "ListMain", new { page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="9%"><%= Html.RouteLink("Supplier", "ListMain", new { page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="7%"><%= Html.RouteLink("Merchant Fee %", "ListMain", new { page = 1, sortField = "MerchantFeePercent", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="6%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.ClientFeeGroupMerchantFees) { 
                %>
                <tr>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.MerchantFeeDescription,27)) %></td>                   
                    <td><%= Html.Encode(item.CountryName)%></td>
                    <td><%= Html.Encode(item.CreditCardVendorName) %></td>
                    <td><%= Html.Encode(item.ProductName)%></td>
                    <td><%= Html.Encode(item.SupplierName) %></td>
                    <td><%= Html.Encode(item.MerchantFeePercent) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", mid = item.MerchantFeeId, id = item.ClientFeeGroupId }, new { title = "View" })%> </td>
                    <td><%if (Model.HasDomainWriteAccess){ %><%= Html.RouteLink("Edit", "Default", new { action = "Edit", mid = item.MerchantFeeId, id = item.ClientFeeGroupId }, new { title = "Edit" })%><%}%></td>
                    <td><%if (Model.HasDomainWriteAccess)
                          { %><%= Html.RouteLink("Delete", "Default", new { action = "Delete", mid = item.MerchantFeeId, id = item.ClientFeeGroupId }, new { title = "Delete" })%><%}%></td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="9" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.ClientFeeGroupMerchantFees.HasPreviousPage)
                               { %>
                            <%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.ClientFeeGroupMerchantFees.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.ClientFeeGroupMerchantFees.HasNextPage)
                               { %>
                            <%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.ClientFeeGroupMerchantFees.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId}, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.ClientFeeGroupMerchantFees.TotalPages > 0)
                                                         { %>Page <%=Model.ClientFeeGroupMerchantFees.PageIndex%> of <%=Model.ClientFeeGroupMerchantFees.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="8">
			    <%= Html.RouteLink("Create Mid Office Merchant Fee Item", "Default", new { action = "Create", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { @class = "red", title = "Create Mid Office Merchant Fee Item" })%>
			    </td>  
		    </tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_clientfeegroups').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search').show();
		$('#ft').attr('name', 'id');
		$("#frmSearch input[name='id']").val(<%=Model.ClientFeeGroup.ClientFeeGroupId%>);
	});

	//Search
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/MerchantFeeClientFeeGroup.mvc/List");
		$("#frmSearch").submit();
	});
 </script>
 </asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink(Model.FeeTypeDisplayName + "s", "Main", new { controller = "ClientFeeGroup", action = "ListUnDeleted", ft = Model.FeeTypeId }, new { title = "Client Fee Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.ClientFeeGroup.ClientFeeGroupName, 45), "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeGroup.ClientFeeGroupName })%> &gt;
Mid Office Merchant Fee Items
</asp:Content>

