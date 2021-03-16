<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TransactionFeeCarHotelVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/TransactionFeeCarHotel.js")%>" type="text/javascript"></script>  
<script src="<%=Url.Content("~/Scripts/DateValidation.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Transaction Fees</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm("Create", null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Mid Office Transaction Fee</th> 
		        </tr>  
                <tr>
                    <td>Product</td>
                    <td><%=Model.TransactionFee.ProductName %></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="TransactionFee_TransactionFeeDescription">Fee Description</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TransactionFee.TransactionFeeDescription, new { maxlength = "100", autocomplete = "off", style="width:250px;" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.TransactionFeeDescription)%></td>
                </tr> 
                 <tr>
                    <td><label for="TransactionFee_PolicyLocationId">Location</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.PolicyLocationId, Model.PolicyLocations, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.PolicyLocationId)%></td>
                </tr> 
                 <tr>
                    <td><label for="TransactionFee_TravelIndicator">Travel Indicator</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.TravelIndicator, Model.TravelIndicators, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.TravelIndicator)%></td>
                </tr>   
                 <tr>
                    <td><label for="TransactionFee_BookingSourceCode">Booking Source</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.BookingSourceCode, Model.BookingSources, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.BookingSourceCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="TransactionFee_BookingOriginationCode">Booking Origination</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.BookingOriginationCode, Model.BookingOriginations, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.BookingOriginationCode)%></td>
                </tr>
                 <tr>
                    <td><label for="TransactionFee_ChargeTypeCode">Charge Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.ChargeTypeCode, Model.ChargeTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.ChargeTypeCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="TransactionFee_TransactionTypeCode">Transaction Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.TransactionTypeCode, Model.TransactionTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.TransactionTypeCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="TransactionFee_FeeCategory">Fee Category</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.FeeCategory, Model.FeeCategories, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.FeeCategory)%></td>
                </tr>  
                 <tr>
                    <td><label for="TransactionFee_TravelerClassCode">Traveler Class</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.TravelerClassCode, Model.TravelerBackOfficeTypes, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.TravelerClassCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="TransactionFee_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TransactionFee.SupplierName, new { maxlength = "100" })%><span class="error"> *</span></td>
                     <td>
                    <%= Html.ValidationMessageFor(model => model.TransactionFee.SupplierName)%>
                    <%= Html.HiddenFor(model => model.TransactionFee.SupplierCode)%>
                    <label id="lblSupplierNameMsg"/>
                </td>
                </tr> 
                <tr>
                    <td><label for="TransactionFee_MinimumFeeCategoryQuantity">Minimum Fee Quantity</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TransactionFee.MinimumFeeCategoryQuantity, new { maxlength = "2" })%> </td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.MinimumFeeCategoryQuantity)%></td>
                </tr> 
                <tr>
                    <td><label for="TransactionFee_MaximumFeeCategoryQuantity">Maximum Fee Quantity</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TransactionFee.MaximumFeeCategoryQuantity, new { maxlength = "2" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.MaximumFeeCategoryQuantity)%></td>
                </tr> 
                <tr>
                    <td><label for="TransactionFee_MinimumTicketPrice">Minimum Ticket Price</label></td>
                    <td> <%= Html.EditorFor(model => model.TransactionFee.MinimumTicketPrice, "numeric8.2")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.MinimumTicketPrice)%></td>
                </tr> 
                <tr>
                    <td><label for="TransactionFee_MaximumTicketPrice">Maximum Ticket Price</label></td>
                    <td> <%= Html.EditorFor(model => model.TransactionFee.MaximumTicketPrice, "numeric8.2")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.MaximumTicketPrice)%></td>
                </tr>  
                 <tr>
                    <td><label for="TransactionFee_TicketPriceCurrencyCode">Ticket Price Currency</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.TicketPriceCurrencyCode, Model.TicketPriceCurrencies, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.TicketPriceCurrencyCode)%></td>
                </tr> 
                <tr>
                    <td><label for="TransactionFee_IncursGSTFlag">Apply GST?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.TransactionFee.IncursGSTFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.IncursGSTFlag)%></td>
                </tr>  
                 <tr>
                    <td><label for="TransactionFee_TripTypeClassificationId">Trip Type Classification</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.TripTypeClassificationId, Model.TripTypeClassifications, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.TripTypeClassificationId)%></td>
                </tr> 
                  <tr>
                    <td><label for="TransactionFee_EnabledDate">Enabled Date</label></td>
                    <td> <%= Html.EditorFor(model => model.TransactionFee.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.EnabledDate)%></td>
                </tr> 
                  <tr>
                    <td><label for="TransactionFee_ExpiryDate">Expiry Date</label></td>
                    <td> <%= Html.EditorFor(model => model.TransactionFee.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.ExpiryDate)%></td>
                </tr> 
                 <tr>
                    <td><label for="TransactionFee_FeeAmount">Fee Amount</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TransactionFee.FeeAmount, new { maxlength = "9" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.FeeAmount)%></td>
                </tr> 
                <tr>
                    <td><label for="TransactionFee_FeeCurrencyCode">Fee Currency</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.FeeCurrencyCode, Model.FeeCurrencies, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.FeeCurrencyCode)%></td>
                </tr> 
                 <tr>
                    <td><label for="TransactionFee_FeePercent">Fee Percent</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TransactionFee.FeePercent, new { maxlength = "6" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.FeePercent)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Mid Office Transaction Fee" title="Create Mid Office Transaction Fee" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.TransactionFee.ProductId)%>
    <% } %>
    </div>
</div>

 </asp:Content>
 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Mid Office Transaction Fees Definitions", "Main", new { controller = "TransactionFee", action = "List", }, new { title = "Mid Office Transaction Fees" })%> &gt;
</asp:Content>


