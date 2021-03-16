<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServiceFundVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Service Fund</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Service Funds</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Service Fund</th> 
		        </tr> 
				<tr>
					<td><label for="ServiceFund_ClientTopUnitName">Client TopUnit Name</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ClientTopUnitName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ClientTopUnitGuid">Client TopUnit Guid</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ClientTopUnitGuid)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_GDSCode">GDS</label></td>
					<td><%= Html.Encode(Model.ServiceFund.GDS.GDSName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundPseudoCityOrOfficeId">Service Fund PCC/OID</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundPseudoCityOrOfficeId)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_PCCCountryCode">PCC/OID Country Code</label></td>
					<td><%= Html.Encode(Model.ServiceFund.Country.CountryName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_FundUseStatus">Fund Use Status</label></td>
					<td><%= Html.Encode(Model.ServiceFund.FundUseStatus)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundQueue">Service Fund Queue</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundQueue)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundStartTime">Service Fund Start Time</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundStartTimeString)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundEndTime">Service Fund End Time</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundEndTimeString)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_TimeZoneRule">Time Zone</label></td>
					<td><%= Html.Encode(Model.ServiceFund.TimeZoneRule.TimeZoneRuleCodeDesc)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ClientAccountNumber">Client Account Number</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ClientAccountName)%>, <%= Html.Encode(Model.ServiceFund.ClientAccountNumber)%>, <%= Html.Encode(Model.ServiceFund.SourceSystemCode)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ProductId">Product</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ProductName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_SupplierCode">Supplier</label></td>
					<td><%= Html.Encode(Model.ServiceFund.SupplierName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundSavingsType">Service Fund Savings Type</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundSavingsType)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundMinimumValue">Service Fund Minimum Value</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundMinimumValue.ToString("0.#####"))%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundCurrencyCode">Service Fund Currency</label></td>
					<td><%= Html.Encode(Model.ServiceFund.Currency.Name)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundRouting">Service Fund Routing</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundRouting)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundClass">Service Fund Class</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundClass)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ServiceFund_ServiceFundRouting">Channel Type</label></td>
					<td><%= Html.Encode(Model.ServiceFund.ServiceFundChannelType.ServiceFundChannelTypeName)%></td>
					<td></td>
				</tr>
                <tr>
                    <td width="40%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
							<%= Html.HiddenFor(model => model.ServiceFund.VersionNumber)%>
							<%= Html.HiddenFor(model => model.ServiceFund.ServiceFundId)%>
						<%}%>
                    </td>
                </tr>
           </table>
        </div>
    </div>
	<script type="text/javascript">
		$(document).ready(function () {
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
			$('#menu_admin').click();
		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Service Funds", "Main", new { controller = "ServiceFund", action = "List" }, new { title = "Service Funds" })%> &gt;
Delete
</asp:Content>