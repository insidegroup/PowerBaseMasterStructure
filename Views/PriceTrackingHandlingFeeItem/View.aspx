<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PriceTrackingHandlingFeeItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
     <div id="banner"><div id="banner_text">Price Tracking Items</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Price Tracking Items</th> 
		        </tr> 
                <tr>
                    <td>Price Tracking System</td>
                    <td><%= Html.Encode(Model.PriceTrackingHandlingFeeItem.PriceTrackingSystem1.PriceTrackingSystemName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.PriceTrackingHandlingFeeItem.Product.ProductName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Saving Amount Percentage</td>
                    <td><%= Html.Encode(Model.PriceTrackingHandlingFeeItem.SavingAmountPercentage)%></td>
                    <td></td>
				</tr> 
                <tr>
                    <td>Handling Fee</td>
                    <td><%= Html.Encode(Model.PriceTrackingHandlingFeeItem.HandlingFee)%></td>
                    <td></td>
				</tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
                </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    	$('#menu_pricetracking').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Groups", "Main", new { controller = "PriceTrackingHandlingFeeGroup", action = "ListUnDeleted", id = ViewData["PriceTrackingHandlingFeeGroupId"] }, new { title = "Price Tracking Groups" })%> &gt;
<%=ViewData["PriceTrackingHandlingFeeGroupName"]%> &gt;
View
</asp:Content>
