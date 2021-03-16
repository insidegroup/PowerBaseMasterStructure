<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CreditCard>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Account - Credit Cards</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(true); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Add Credit Card</th> 
		        </tr>
                <tr>
                    <td>Client TopUnit</td>
                    <td colspan="2"><%= Html.Encode(Model.ClientTopUnitName)%></td>
                </tr> 
                 <tr>
                    <td>Client SubUnit</td>
                    <td colspan="2"><%= Html.Encode(Model.ClientSubUnitName)%></td>
                </tr> 
                 <tr>
                    <td>Client Account</td>
                    <td colspan="2"><%= Html.Encode(Model.ClientAccountName)%></td>
                </tr> 
                <tr>
                    <td><label for="CreditCardTypeId">Credit Card Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.CreditCardTypeId, ViewData["CreditCardTypeList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CreditCardTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.ProductId, ViewData["ProductList"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
                </tr> 
                <tr>
                    <td><label for="CreditCard_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.SupplierName)%></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.SupplierName)%>
                        <%= Html.HiddenFor(model => model.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
				</tr>
				<tr>
                    <td><label for="CreditCardHolderName">Credit Card Holder Name</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CreditCardHolderName, new { maxlength = "100", autocomplete="off"  })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CreditCardHolderName)%></td>
                </tr> 
                 <tr>
                    <td><label for="CreditCardNumber">Credit Card Number</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CreditCardNumber, new {maxlength="24", autocomplete="off" })%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCardNumber)%> </td>
                </tr>
				<tr>
                    <td><label for="CVV">CVV</label></td>
                    <td><%= Html.TextBoxFor(model => model.CVV, new { maxlength = "4", autocomplete = "off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.CVV)%> </td>
                </tr> 
                <tr>
                    <td><label for="CreditCardVendorCode">Credit Card Vendor</label></td>
                    <td><%= Html.DropDownList("CreditCardVendorCode", ViewData["CreditCardVendorsList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CreditCardVendorCode)%></td>
                </tr>  
                <tr>
                    <td><label for="CreditCardValidFrom">Credit Card Valid From</label></td>
                    <td> <%= Html.EditorFor(model => model.CreditCardValidFrom, new { maxlength = "11" })%></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCardValidFrom)%> </td>
                </tr>
                  <tr>
                    <td><label for="CreditCardValidTo">Credit Card Valid To </label></td>
                    <td><%= Html.TextBox("CreditCardValidTo", "", new { maxlength = "11" })%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCardValidTo)%> </td>
                </tr>
                                <tr>
                    <td><label for="CreditCardIssueNumber">Issue Number</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CreditCardIssueNumber, new {maxlength="3"})%></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCardIssueNumber)%> </td>
                </tr> 
                
                
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Add Credit Card" title="Add Credit Card" class="red"/></td>
                </tr>
            </table>
            <%= Html.Hidden("HierarchyType", "ClientAccount")%>
            <%= Html.Hidden("CanHaveRealCreditCardsFlag", Model.CanHaveRealCreditCardsFlag)%>
            <%= Html.Hidden("WarningShownFlag", Model.WarningShownFlag)%>
            <%= Html.Hidden("HierarchyItem", Model.ClientAccountName)%>
            <%= Html.HiddenFor(model => model.ClientTopUnitGuid)%>
            <%= Html.HiddenFor(model => model.ClientSubUnitGuid)%>
            <%= Html.HiddenFor(model => model.ClientAccountNumber)%>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
            <%= Html.Hidden("OriginalCreditCardNumber", "")%>

    <% } %>
        </div>
    </div>
<%if (Model.CanHaveRealCreditCardsFlag)
  {%>
    <script src="<%=Url.Content("~/Scripts/ERD/CreditCard2.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/ERD/ValidCreditCardNumbersAllowed2.js")%>" type="text/javascript"></script>
<%}else{ %>
    <script src="<%=Url.Content("~/Scripts/ERD/CreditCard2.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/ERD/ValidCreditCardNumbersNotAllowed2.js")%>" type="text/javascript"></script>
<%} %>
</asp:Content>


