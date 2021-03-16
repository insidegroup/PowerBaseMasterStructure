<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServiceFundVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Service Fund</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Service Funds</div></div>
        <div id="content">
            <% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<table cellpadding="0" border="0" width="100%" cellspacing="0"> 
					<tr> 
						<th class="row_header" colspan="3">Edit Service Fund</th> 
					</tr> 
					<tr>
						<td><label for="ServiceFund_ClientTopUnitName">Client TopUnit Name</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ClientTopUnitName, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td>
							<label id="lblServiceFundMsg"></label>
							<%= Html.HiddenFor(model => model.ServiceFund.ClientTopUnitGuid)%>
							<%= Html.ValidationMessageFor(model => model.ServiceFund.ClientTopUnitName)%>
						</td>
					</tr>
					<tr>
						<td><label for="ServiceFund_GDSCode">GDS</label></td>
						<td><%= Html.DropDownListFor(model => model.ServiceFund.GDSCode, Model.GDSs, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.GDSCode)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundPseudoCityOrOfficeId">Service Fund PCC/OID</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ServiceFundPseudoCityOrOfficeId, new { maxlength = "9" })%><span class="error"> *</span></td>
						<td>
							<label id="lblValidServiceFundPseudoCityOrOfficeIdMessage"></label>
							<%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundPseudoCityOrOfficeId)%>
						</td>
					</tr>
					<tr>
						<td><label for="ServiceFund_PCCCountryCode">PCC/OID Country Code</label></td>
						<td><%= Html.DropDownListFor(model => model.ServiceFund.PCCCountryCode, Model.Countries, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.PCCCountryCode)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_FundUseStatus">Fund Use Status</label></td>
						<td><%= Html.DropDownListFor(model => model.ServiceFund.FundUseStatus, Model.FundUseStatuses, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.FundUseStatus)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundQueue">Service Fund Queue</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ServiceFundQueue, new { maxlength = "9" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundQueue)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundStartTime">Service Fund Start Time</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ServiceFundStartTimeString)%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundStartTimeString)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundEndTime">Service Fund End Time</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ServiceFundEndTimeString)%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundEndTimeString)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_TimeZoneRule">Time Zone</label></td>
						<td><%= Html.DropDownListFor(model => model.ServiceFund.TimeZoneRuleCode, Model.TimeZoneRules, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.TimeZoneRuleCode)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ClientAccountNumber">Client Account Number</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ClientAccountName, new { maxlength = "150" })%><span class="error"> *</span></td>
						<td>
							<%= Html.HiddenFor(model => model.ServiceFund.ClientAccountNumber)%>
							<%= Html.HiddenFor(model => model.ServiceFund.SourceSystemCode)%>
							<%= Html.ValidationMessageFor(model => model.ServiceFund.ClientAccountNumber)%>
						</td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ProductId">Product</label></td>
						<td><%= Html.DropDownListFor(model => model.ServiceFund.ProductId, Model.Products, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ProductId)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_SupplierCode">Supplier</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.SupplierName, new { maxlength = "100" })%><span class="error"> *</span></td>
						<td>
							<label id="lblSupplierNameMsg"></label>
							<%= Html.HiddenFor(model => model.ServiceFund.SupplierCode)%>
							<%= Html.ValidationMessageFor(model => model.ServiceFund.SupplierCode)%>
						</td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundSavingsType">Service Fund Savings Type</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ServiceFundSavingsType, new { maxlength = "10" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundSavingsType)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundMinimumValue">Service Fund Minimum Value</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ServiceFundMinimumValue, new { maxlength = "13" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundMinimumValue)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundCurrencyCode">Service Fund Currency</label></td>
						<td><%= Html.DropDownListFor(model => model.ServiceFund.ServiceFundCurrencyCode, Model.Currencies, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundCurrencyCode)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundRouting">Service Fund Routing</label></td>
						<td><%= Html.DropDownListFor(model => model.ServiceFund.ServiceFundRouting, Model.ServiceFundRoutings, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundRouting)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundClass">Service Fund Class</label></td>
						<td><%= Html.TextBoxFor(model => model.ServiceFund.ServiceFundClass, new { maxlength = "10" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundClass)%></td>
					</tr>
					<tr>
						<td><label for="ServiceFund_ServiceFundRouting">Channel Type</label></td>
						<td><%= Html.DropDownListFor(model => model.ServiceFund.ServiceFundChannelTypeId, Model.ServiceFundChannelTypes, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ServiceFund.ServiceFundChannelTypeId)%></td>
					</tr>
					<tr>
						<td width="25%" class="row_footer_left"></td>
						<td width="45%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
				   <tr>
						<td class="row_footer_blank_left" colspan="2">
							<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
							<a href="javascript:window.print();" class="red" title="Print">Print</a>
						</td>                    
						<td class="row_footer_blank_right">
							<input type="submit" value="Edit" title="Edit" class="red"/>
							<%= Html.HiddenFor(model => model.ServiceFund.VersionNumber)%>
							<%= Html.HiddenFor(model => model.ServiceFund.ServiceFundId)%>
						</td>
					</tr>
			   </table>
			<%}%>
        </div>
    </div>
    
	<script src="<%=Url.Content("~/Scripts/ERD/ServiceFund.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Service Funds", "Main", new { controller = "ServiceFund", action = "List" }, new { title = "Service Funds" })%> &gt;
Edit
</asp:Content>