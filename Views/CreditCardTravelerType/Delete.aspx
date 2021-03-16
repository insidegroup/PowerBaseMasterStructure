<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TravelerTypeCreditCardVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">TravelerType - Credit Cards</div>
		</div>
		<div id="content">
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Delete Credit Card</th>
				</tr>
				<tr>
					<td>Client TopUnit</td>
					<td><%= Html.Encode(Model.CreditCard.ClientTopUnitName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Client SubUnit</td>
					<td><%= Html.Encode(Model.ClientSubUnit.ClientSubUnitName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Client Traveler Type</td>
					<td><%= Html.Encode(Model.TravelerType.TravelerTypeName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Credit Card Type</td>
					<td><%= Html.Encode(Model.CreditCard.CreditCardTypeDescription)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Product</td>
					<td><%= Html.Encode(Model.CreditCard.ProductName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Supplier</td>
					<td><%= Html.Encode(Model.CreditCard.SupplierName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Credit Card Holder Name</td>
					<td><%= Html.Encode(Model.CreditCard.CreditCardHolderName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Credit Card Number</td>
					<td><%= Html.Encode(Model.CreditCard.MaskedCreditCardNumber)%></td>
					<td></td>
				</tr>
				<tr>
					<td>CVV</td>
					<td><%= Html.Encode(!string.IsNullOrEmpty(Model.CreditCard.MaskedCVVNumber) ? Model.CreditCard.MaskedCVVNumber : String.Empty)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Credit Card Vendor</td>
					<td><%= Html.Encode(Model.CreditCard.CreditCardVendorName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Credit Card Type</td>
					<td><%= Html.Encode(Model.CreditCard.CreditCardTypeDescription)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Credit Card Valid From</td>
					<td><%= Html.Encode(Model.CreditCard.CreditCardValidFrom.HasValue ? Model.CreditCard.CreditCardValidFrom.Value.ToString("MMM dd, yyyy") : "No ValidFrom Date")%></td>
					<td></td>
				</tr>
				<tr>
					<td>Credit Card Valid To</td>
					<td><%= Html.Encode(Model.CreditCard.CreditCardValidTo.ToString("MMM dd, yyyy"))%></td>
					<td></td>
				</tr>
				<tr>
					<td>Issue Number</td>
					<td><%= Html.Encode(Model.CreditCard.CreditCardIssueNumber)%></td>
					<td></td>
				</tr>
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="30%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
					<td class="row_footer_blank_right" colspan="2">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red" />
							<%= Html.HiddenFor(model => model.CreditCard.VersionNumber) %>
							<%= Html.HiddenFor(model => model.CreditCard.CreditCardId) %>								
							<%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid)%>
							<%= Html.HiddenFor(model => model.TravelerType.TravelerTypeGuid)%>
							<%--
								<%= Html.HiddenFor(model => model.CreditCard.CreditCardNumber)%>
							--%>
						<%}%>
                    </td>
				</tr>
			</table>
		</div>
	</div>

	<script type="text/javascript">
		$(document).ready(function () {
			$('#menu_clients').click();
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
		})
 </script>
</asp:Content>
