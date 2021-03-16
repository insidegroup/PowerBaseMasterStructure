<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTravelerTypeCreditCardVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit TravelerType - Credit Cards</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Credit Card</th> 
		        </tr>  
                
                <tr>
                    <td>Client TopUnit</td>
                    <td><%= Html.Encode(Model.CreditCard.ClientTopUnitName)%></td>
                    <td></td>
                </tr>
               <tr>
                    <td><label for="CreditCard_CreditCardTypeId">Credit Card Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.CreditCard.CreditCardTypeId, Model.CreditCardTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CreditCard.CreditCardTypeId)%></td>
                </tr> 
				<tr>
					<td><label for="CreditCard_ProductId">Product</label></td>
					<td><%= Html.DropDownListFor(model => model.CreditCard.ProductId, ViewData["ProductList"] as SelectList, "Please Select...")%></td>
					<td><%= Html.ValidationMessageFor(model => model.CreditCard.ProductId)%></td>
				</tr>
				<tr>
					<td><label for="CreditCard_SupplierName">Supplier</label></td>
					<td><%= Html.TextBoxFor(model => model.CreditCard.SupplierName)%></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.CreditCard.SupplierName)%>
						<%= Html.HiddenFor(model => model.CreditCard.SupplierCode)%>
						<label id="lblSupplierNameMsg" />
					</td>
				</tr>
                <tr>
                    <td><label for="CreditCard_CreditCardHolderName">Credit Card Holder Name</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CreditCard.CreditCardHolderName, new { maxlength = "100", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CreditCard.CreditCardHolderName)%></td>
                </tr> 
                <tr>
                    <td>Credit Card Number</td>
                    <td><%= Html.Encode(Model.CreditCard.MaskedCreditCardNumber)%></td>
                    <td></td>
                </tr> 
				<tr>
					<td>CVV</td>
					<% if (!string.IsNullOrEmpty(Model.CreditCard.MaskedCVVNumber)){ %>
					<td>
						<%= Html.Encode(Model.CreditCard.MaskedCVVNumber)%>
						<%= Html.HiddenFor(model => model.CreditCard.MaskedCVVNumber)%>
					</td>
					<td></td>
					<% } else { %>
					<td><%= Html.TextBoxFor(model => model.CreditCard.CVV, new { maxlength = "4", autocomplete = "off" })%></td>
					<td><%= Html.ValidationMessageFor(model => model.CreditCard.CVV)%> </td>
					<% } %>
					<td></td>
				</tr>
                <tr>
                    <td>Credit Card Vendor</td>
                    <td><%= Html.Encode(Model.CreditCard.CreditCardVendorName)%></td>
                    <td></td>
                </tr>    
                <tr>
                    <td><label for="CreditCard_CreditCardValidFrom">Credit Card Valid From</label></td>
                    <td> <%= Html.EditorFor(model => model.CreditCard.CreditCardValidFrom, new { maxlength = "11" })%></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCard.CreditCardValidFrom)%> </td>
                </tr>
                  <tr>
                    <td><label for="CreditCard_CreditCardValidTo">Credit Card Valid To </label></td>
                    <td><%= Html.EditorFor(model => model.CreditCard.CreditCardValidTo, new { maxlength = "11" })%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCard.CreditCardValidTo)%> </td>
                </tr>
                <tr>
                    <td><label for="CreditCard_CreditCardIssueNumber">Issue Number</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CreditCard.CreditCardIssueNumber, new { maxlength = "3" })%></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreditCard.CreditCardIssueNumber)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Credit Card" title="Edit Credit Card" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.CreditCard.ClientTopUnitGuid)%>
            <%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid)%>
            <%= Html.HiddenFor(model => model.TravelerType.TravelerTypeGuid)%>
            <%= Html.HiddenFor(model => model.CreditCard.CreditCardId)%>
            <%= Html.HiddenFor(model => model.CreditCard.VersionNumber)%>
            <%= Html.HiddenFor(model => model.CreditCard.CreditCardNumber)%>
            <%= Html.HiddenFor(model => model.CreditCard.CreditCardVendorCode)%>
            <%= Html.HiddenFor(model => model.CreditCard.HierarchyCode)%>
            <%= Html.HiddenFor(model => model.CreditCard.HierarchyItem)%>
            <%= Html.HiddenFor(model => model.CreditCard.HierarchyType)%>
            <%= Html.HiddenFor(model => model.CreditCard.TravelerTypeGuid)%>


    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/CreditCardEdit.js")%>" type="text/javascript"></script>
</asp:Content>

