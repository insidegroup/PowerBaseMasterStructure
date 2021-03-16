<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CreditCard>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>
<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
	<script src="<%=Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client Account - Credit Cards</div>
		</div>
		<div id="content">
			<% Html.EnableClientValidation(); %>

			<% using (Html.BeginForm())
	  {%>
			<%= Html.AntiForgeryToken() %>
			<%= Html.ValidationSummary(true) %>

			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Edit Credit Card</th>
				</tr>
				<tr>
					<td>Client Top Unit</td>
					<td><%= Html.Encode(Model.ClientTopUnitName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Client SubUnit</td>
					<td><%= Html.Encode(Model.ClientSubUnitName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Client Account</td>
					<td><%= Html.Encode(Model.ClientAccountName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="CreditCardTypeId">Credit Card Type</label></td>
					<td><%= Html.DropDownList("CreditCardTypeId", ViewData["CreditCardTypeList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.CreditCardTypeId)%></td>
				</tr>
				<tr>
					<td><label for="ProductId">Product</label></td>
					<td><%= Html.DropDownListFor(model => model.ProductId, ViewData["ProductList"] as SelectList, "Please Select...")%></td>
					<td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
				</tr>
				<tr>
					<td>
						<label for="SupplierName">Supplier</label></td>
					<td><%= Html.TextBoxFor(model => model.SupplierName)%></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.SupplierName)%>
						<%= Html.HiddenFor(model => model.SupplierCode)%>
						<label id="lblSupplierNameMsg" />
					</td>
				</tr>
				<tr>
					<td><label for="CreditCardHolderName">Credit Card Holder Name</label></td>
					<td><%= Html.TextBoxFor(model => model.CreditCardHolderName, new { maxlength = "100", autocomplete="off" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.CreditCardHolderName)%></td>
				</tr>
				<tr>
					<td>Credit Card Number</td>
					<td><%= Html.Encode(Model.MaskedCreditCardNumber)%></td>
					<td></td>
				</tr>
				<tr>
					<td>CVV</td>
					<% if (!string.IsNullOrEmpty(Model.MaskedCVVNumber)){ %>
					<td>
						<%= Html.Encode(Model.MaskedCVVNumber)%>
						<%= Html.HiddenFor(model => model.MaskedCVVNumber)%>
					</td>
					<td></td>
					<% } else { %>
					<td><%= Html.TextBoxFor(model => model.CVV, new { maxlength = "4", autocomplete = "off" })%></td>
					<td><%= Html.ValidationMessageFor(model => model.CVV)%> </td>
					<% } %>
					<td></td>
				</tr>
				<tr>
					<td>Credit Card Vendor</td>
					<td><%= Html.Encode(Model.CreditCardVendorName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>
						<label for="CreditCardValidFrom">Credit Card Valid From</label></td>
					<td><%= Html.EditorFor(model => model.CreditCardValidFrom, new { maxlength = "11" })%></td>
					<td><%= Html.ValidationMessageFor(model => model.CreditCardValidFrom)%> </td>
				</tr>
				<tr>
					<td>
						<label for="CreditCardValidTo">Credit Card Valid To </label>
					</td>
					<td><%= Html.EditorFor(model => model.CreditCardValidTo, new { maxlength = "11" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.CreditCardValidTo)%> </td>
				</tr>
				<tr>
					<td>
						<label for="CreditCardIssueNumber">Issue Number</label></td>
					<td><%= Html.TextBoxFor(model => model.CreditCardIssueNumber, new {maxlength="3"})%></td>
					<td><%= Html.ValidationMessageFor(model => model.CreditCardIssueNumber)%> </td>
				</tr>
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="30%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
					<td class="row_footer_blank_right">
						<input type="submit" value="Edit Credit Card" title="Edit Credit Card" class="red" /></td>
				</tr>
			</table>
			<%= Html.HiddenFor(model => model.HierarchyCode)%>
			<%= Html.HiddenFor(model => model.HierarchyItem)%>
			<%= Html.HiddenFor(model => model.HierarchyType)%>
			<%= Html.HiddenFor(model => model.ClientAccountNumber)%>
			<%= Html.HiddenFor(model => model.SourceSystemCode)%>
			<%= Html.HiddenFor(model => model.VersionNumber)%>
			<%= Html.HiddenFor(model => model.CreditCardNumber)%>
			<%= Html.HiddenFor(model => model.ClientSubUnitGuid)%>
			<% } %>
		</div>
	</div>
	<script src="<%=Url.Content("~/Scripts/ERD/CreditCard2.js")%>" type="text/javascript"></script>
</asp:Content>

