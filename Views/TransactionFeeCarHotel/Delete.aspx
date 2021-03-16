<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TransactionFeeCarHotelVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Mid Office Transaction Fees</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete Mid Office Transaction Fee</th> 
		    </tr> 
            <tr>
                <td>Fee Description</td>
                <td><%=CWTStringHelpers.WrapString(Model.TransactionFee.ProductName)%></td>
                <td></td>
            </tr>
             <tr>
                <td>Fee Description</td>
                <td><%= Html.Encode(Model.TransactionFee.TransactionFeeDescription)%></td>
                <td></td>
            </tr>
            <tr>
                <td>Location</td>
                <td><%= Html.Encode(Model.TransactionFee.PolicyLocationName)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Travel Indicator</td>
                <td><%= Html.Encode(Model.TransactionFee.TravelIndicatorDescription)%></td>
                <td></td>
            </tr>       
            <tr>
                <td>Booking Source</td>
                <td><%= Html.Encode(Model.TransactionFee.BookingSourceDescription)%></td>
                <td></td>
            </tr>   
            <tr>
                <td>Booking Origination</td>
                <td><%= Html.Encode(Model.TransactionFee.BookingOriginationDescription)%></td>
                <td></td>
            </tr>   
            <tr>
                <td>Charge Type</td>
                <td><%= Html.Encode(Model.TransactionFee.ChargeTypeDescription)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Transaction Type</td>
                <td><%= Html.Encode(Model.TransactionFee.TransactionTypeCode)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Fee Category</td>
                <td><%= Html.Encode(Model.TransactionFee.FeeCategory)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Traveler Class</td>
                <td><%= Html.Encode(Model.TransactionFee.TravelerBackOfficeTypeDescription)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>Supplier</td>
                <td><%= Html.Encode(Model.TransactionFee.SupplierName)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Minimum Fee Quantity</td>
                <td><%= Html.Encode(Model.TransactionFee.MinimumFeeCategoryQuantity)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Maximum Fee Quantity</td>
                <td><%= Html.Encode(Model.TransactionFee.MaximumFeeCategoryQuantity)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Minimum Ticket Price</td>
                <td><%= Html.Encode(Model.TransactionFee.MinimumTicketPrice)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Maximum Ticket Price</td>
                <td><%= Html.Encode(Model.TransactionFee.MaximumTicketPrice)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>Ticket Price Currency</td>
                <td><%= Html.Encode(Model.TransactionFee.TicketPriceCurrencyName)%></td>
                <td></td>
            </tr>  
              <tr>
                <td>Apply GST?</td>
                <td><%= Html.Encode(Model.TransactionFee.IncursGSTFlag)%></td>
                <td></td>
            </tr>  
              <tr>
                <td>Trip Type Classification</td>
                <td><%= Html.Encode(Model.TransactionFee.TripTypeClassificationDescription)%></td>
                <td></td>
            </tr>           
            <tr>
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.TransactionFee.EnabledDate.HasValue ? Model.TransactionFee.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.TransactionFee.ExpiryDate.HasValue ? Model.TransactionFee.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                <td></td>
            </tr>     
             <tr>
                <td>Fee Amount</td>
                <td><%= Html.Encode(Model.TransactionFee.FeeAmount)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Fee Currency</td>
                <td><%= Html.Encode(Model.TransactionFee.FeeCurrencyName)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Fee Percent</td>
                <td><%= Html.Encode(Model.TransactionFee.FeePercent)%></td>
                <td></td>
            </tr>      
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                <td class="row_footer_blank_right">
                <% using (Html.BeginForm()) { %>
                    <%= Html.AntiForgeryToken() %>
                    <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                    <%= Html.HiddenFor(model => model.TransactionFee.VersionNumber)%>
                    <%= Html.HiddenFor(model => model.TransactionFee.TransactionFeeId)%>
                <%}%>
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
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Mid Office Transaction Fees Definitions", "Main", new { controller = "TransactionFee", action = "List", }, new { title = "Mid Office Merchant Fees" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.TransactionFee.TransactionFeeDescription,37)) %>
</asp:Content>