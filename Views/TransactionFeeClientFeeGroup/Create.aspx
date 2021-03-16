<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupTransactionFeeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Transaction Fee Items</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Mid Office Transaction Fee Item</th> 
		        </tr>  
                <tr>
                    <td>Group Name</td>
                    <td> <%= Model.ClientFeeGroup.ClientFeeGroupName%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="TransactionFee_ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFee.ProductId, Model.Products, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFee.ProductId)%></td>
                </tr> 
                 <tr>
                    <td><label for="TransactionFee_TransactionFeeId">Fee Description</label></td>
                    <td><%= Html.DropDownListFor(model => model.TransactionFeeClientFeeGroup.TransactionFeeId, Model.TransactionFees, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TransactionFeeClientFeeGroup.TransactionFeeId)%></td>
                </tr>  
                <tr style="display:none" id="PolicyLocationNameDiv">
                    <td>Location</td>
                    <td><span id="PolicyLocationName"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Travel Indicator</td>
                    <td><span id="TravelIndicatorDescription"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Booking Source</td>
                    <td><span id="BookingSourceDescription"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Booking Origination</td>
                    <td><span id="BookingOriginationDescription"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Charge Type</td>
                    <td><span id="ChargeTypeDescription"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Transaction Type</td>
                    <td><span id="TransactionTypeCode"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Fee Category</td>
                    <td><span id="FeeCategory"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Traveler Class</td>
                    <td><span id="TravelerClassCode"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Supplier</td>
                    <td><span id="SupplierName"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Minimum Fee Quantity</td>
                    <td><span id="MinimumFeeCategoryQuantity"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Maximum Fee Quantity</td>
                    <td><span id="MaximumFeeCategoryQuantity"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Minimum Ticket Price</td>
                    <td><span id="MinimumTicketPrice"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Maximum Ticket Price</td>
                    <td><span id="MaximumTicketPrice"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Ticket Price Currency</td>
                    <td><span id="TicketPriceCurrencyCode"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Apply GST?</td>
                    <td><span id="IncursGSTFlag"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Trip Type Classification</td>
                    <td><span id="TripTypeClassificationDescription"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Enabled Date</td>
                    <td><span id="EnabledDate"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Expiry Date</td>
                    <td><span id="ExpiryDate"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Fee Amount</td>
                    <td><span id="FeeAmount"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Fee Currency</td>
                    <td><span id="FeeCurrencyCode"></span></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Fee Percent</td>
                    <td><span id="FeePercent"></span></td>
                    <td></td>
                </tr>  
                  <tr style="display:none" id="PolicyRouting_Header_Div"> 
			        <th class="row_header" colspan="3">Policy Routing</th> 
		        </tr> 
                <tr style="display:none" id="PolicyRouting_Name_Div"> 
                    <td>Name</td>
                    <td><span id="Name"></span></td>
                    <td></td>
                </tr> 
                <tr style="display:none" id="PolicyRouting_FromGlobalFlag_Div"> 
                    <td>From Global?</td>
                    <td><span id="FromGlobalFlag"></span></td>
                    <td></td>
                </tr> 
                <tr style="display:none" id="PolicyRouting_FromCode_Div"> 
                    <td>From</td>
                    <td><span id="FromCode"></span></td>
                    <td></td>
                </tr>  
                 <tr style="display:none" id="PolicyRouting_ToGlobalFlag_Div"> 
                    <td>To Global?</td>
                    <td><span id="ToGlobalFlag"></span></td>
                    <td></td>
                </tr> 
                 <tr style="display:none" id="PolicyRouting_ToCode_Div"> 
                    <td>To</td>
                    <td><span id="ToCode"></span></td>
                    <td></td>
                </tr>  
                <tr style="display:none" id="PolicyRouting_RoutingViceVersaFlag_Div"> 
                    <td>Routing ViceVersa?</td>
                    <td><span id="RoutingViceVersaFlag"></span></td>
                    <td></td>
                </tr>   
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Mid Office Transaction Fee Item" title="Create Mid Office Transaction Fee Item" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.TransactionFeeClientFeeGroup.ClientFeeGroupId)%>
            <%= Html.HiddenFor(model => model.TransactionFeeClientFeeGroup.TransactionFeeId)%>
            <%= Html.HiddenFor(model => model.TransactionFeeClientFeeGroup.ProductId)%>
    <% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/TransactionFeeClientFeeGroup.js")%>" type="text/javascript"></script>
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Mid Office Transaction Fee Groups", "Main", new { controller = "ClientFeeGroup", action = "List" }, new { title = "Mid Office Transaction Fee Groups" })%> &gt;
<%=Html.RouteLink(Model.ClientFeeGroup.ClientFeeGroupName, "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeGroup.ClientFeeGroupName })%> &gt;
Mid Office Transaction Fee Items
</asp:Content>



