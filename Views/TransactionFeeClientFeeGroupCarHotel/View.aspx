<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupTransactionFeeCarHotelVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Mid Office Transaction Fees</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Mid Office Transaction Fee</th> 
		    </tr> 
            <tr>
                <td>Product</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.ProductName)%></td>
                <td></td>
            </tr>
             <tr>
                <td>Fee Description</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.TransactionFeeDescription)%></td>
                <td></td>
            </tr>
             <tr>
                <td>Location</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.PolicyLocationName)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>Travel Indicator</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.TravelIndicatorDescription)%></td>
                <td></td>
            </tr>       
            <tr>
                <td>Booking Source</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.BookingSourceDescription)%></td>
                <td></td>
            </tr>   
            <tr>
                <td>Booking Origination</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.BookingOriginationDescription)%></td>
                <td></td>
            </tr>   
            <tr>
                <td>Charge Type</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.ChargeTypeDescription)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Transaction Type</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.TransactionTypeCode)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Fee Category</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.FeeCategory)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Traveler Class</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.TravelerBackOfficeTypeDescription)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>Supplier</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.SupplierName)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Minimum Fee Quantity</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.MinimumFeeCategoryQuantity)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Maximum Fee Quantity</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.MaximumFeeCategoryQuantity)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Minimum Ticket Price</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.MinimumTicketPrice)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Maximum Ticket Price</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.MaximumTicketPrice)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Ticket Price Currency</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.TicketPriceCurrencyName)%></td>
                <td></td>
            </tr>  
              <tr>
                <td>Apply GST?</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.IncursGSTFlag)%></td>
                <td></td>
            </tr>  
              <tr>
                <td>Trip Type Classification</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.TripTypeClassificationDescription)%></td>
                <td></td>
            </tr>           
            <tr>
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.EnabledDate.HasValue ? Model.TransactionFeeCarHotel.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.ExpiryDate.HasValue ? Model.TransactionFeeCarHotel.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                <td></td>
            </tr>     
             <tr>
                <td>Fee Amount</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.FeeAmount)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Fee Currency</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.FeeCurrencyName)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Fee Percent</td>
                <td><%= Html.Encode(Model.TransactionFeeCarHotel.FeePercent)%></td>
                <td></td>
            </tr>      
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right"></td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Mid Office Transaction Fee Groups", "Main", new { controller = "ClientFeeGroup", action = "List" }, new { title = "Mid Office Transaction Fee Groups" })%> &gt;
<%=Html.RouteLink(Model.ClientFeeGroup.ClientFeeGroupName, "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeGroup.ClientFeeGroupName })%> &gt;
<%=Html.RouteLink("Mid Office Transaction Fee Items", "Main", new { controller = "TransactionFeeClientFeeGroup", action = "List", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = "Mid Office Transaction Fee Items" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.TransactionFeeCarHotel.TransactionFeeDescription, 37))%>
</asp:Content>

