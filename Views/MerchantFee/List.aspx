<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MerchantFeesVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Merchant Fees</div></div>
    <div id="content">
         <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
			        <th width="23%" class="row_header_left"><%= Html.RouteLink("Description", "ListMain", new { page = 1, sortField = "MerchantFeeDescription", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="21%"><%= Html.RouteLink("Country", "ListMain", new { page = 1, sortField = "CountryName", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="16%"><%= Html.RouteLink("CC Vendor", "ListMain", new { page = 1, sortField = "CreditCardVendorName", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="8%"><%= Html.RouteLink("Product", "ListMain", new { page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="9%"><%= Html.RouteLink("Supplier", "ListMain", new { page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="7%"><%= Html.RouteLink("Merch Fee %", "ListMain", new { page = 1, sortField = "MerchantFeePercent", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="6%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model.MerchantFees) { 
            %>
            <tr>
                <td><%= Html.Encode(CWTStringHelpers.TrimString(item.MerchantFeeDescription,27)) %></td>
                <td><%= Html.Encode(item.CountryName) %></td>
                <td><%= Html.Encode(item.CreditCardVendorName) %></td>
                <td><%= Html.Encode(item.ProductName) %></td>
                <td><%= Html.Encode(item.SupplierName)%></td>
                <td align="right"><%= Html.Encode(item.MerchantFeePercent)%></td>
                <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.MerchantFeeId }, new { title = "View" })%> </td>
                <td>
                    <%if (Model.HasWriteAccess)
                      {%>
                    <%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.MerchantFeeId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if (Model.HasWriteAccess)
                      {%>
                    <%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.MerchantFeeId }, new { title = "Delete" })%>
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
                                <% if (Model.MerchantFees.HasPreviousPage)
                                   { %>
                            <%=  Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.MerchantFees.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.MerchantFees.HasNextPage)
                                   { %>
                            <%=  Html.RouteLink("Next Page>>", "ListMain", new { page = (Model.MerchantFees.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.MerchantFees.TotalPages > 0)
                                                         { %>Page <%=Model.MerchantFees.PageIndex%> of <%=Model.MerchantFees.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
             <tr> 
                 <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		        <td class="row_footer_blank_right" colspan="7">
		        <%if (Model.HasWriteAccess)
            {%>
		        <%= Html.RouteLink("Create Merchant Fee", "Main", new { action = "Create" }, new { @class = "red", title = "Create Merchant Fee" })%>
		        <% } %>
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
    })

    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/MerchantFee.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>
