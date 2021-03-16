<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MerchantFeeVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Merchant Fees</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm(null, null, FormMethod.Post, new { autocomplete="off" })) {%>
            <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Mid Office Merchant Fee</th> 
		        </tr>  
               <tr>
                    <td><label for="MerchantFee_MerchantFeeDescription">Fee Description</label></td>
                    <td> <%= Html.TextBoxFor(model => model.MerchantFee.MerchantFeeDescription, new { maxlength = "100", autocomplete = "off", style="width:250px;" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MerchantFee.MerchantFeeDescription)%></td>
                </tr> 
                 <tr>
                    <td><label for="MerchantFee_CountryCode">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.MerchantFee.CountryCode, Model.Countries, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MerchantFee.CountryCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="MerchantFee_CreditCardVendorCode">Credit Card Vendor</label></td>
                    <td><%= Html.DropDownListFor(model => model.MerchantFee.CreditCardVendorCode, Model.CreditCardVendors, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MerchantFee.CreditCardVendorCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="MerchantFee_MerchantFeePercent">Merchant Fee Percent</label></td>
                    <td> <%= Html.TextBoxFor(model => model.MerchantFee.MerchantFeePercent, new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MerchantFee.MerchantFeePercent)%></td>
                </tr> 
                 <tr>
                    <td><label for="MerchantFee_ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.MerchantFee.ProductId, Model.Products, "None", new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.MerchantFee.ProductId)%></td>
                </tr>  
                 <tr>
                    <td><label for="MerchantFee_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.MerchantFee.SupplierName, new { maxlength = "100", autocomplete="off" })%></td>
                     <td>
                        <%= Html.ValidationMessageFor(model => model.MerchantFee.SupplierName)%>
                        <%= Html.HiddenFor(model => model.MerchantFee.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr> 
                  <tr>
                    <td><label for="MerchantFee_EnabledDate">Enabled Date</label></td>
                    <td> <%= Html.EditorFor(model => model.MerchantFee.EnabledDate, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.MerchantFee.EnabledDate)%></td>
                </tr> 
                  <tr>
                    <td><label for="MerchantFee_ExpiryDate">Expiry Date</label></td>
                    <td> <%= Html.EditorFor(model => model.MerchantFee.ExpiryDate, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.MerchantFee.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Base Client Fee" title="Edit Base Client Fee" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.MerchantFee.MerchantFeeId)%>
            <%= Html.HiddenFor(model => model.MerchantFee.VersionNumber)%>
    <% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/MerchantFee.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/DateValidation.js")%>" type="text/javascript"></script>
 </asp:Content>
 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Mid Office Merchant Fees", "Main", new { controller = "MerchantFee", action = "List", }, new { title = "Mid Office Merchant Fees" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.MerchantFee.MerchantFeeDescription,37)) %>
</asp:Content>


