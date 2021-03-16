<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PriceTrackingSetupGroupItemHotel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Setup Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
	<div id="banner"><div id="banner_text">Price Tracking Hotel Setup</div></div>
		<div id="content">
		<% Html.EnableClientValidation(); %>
		<% Html.EnableUnobtrusiveJavaScript(); %>

		<% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
			<%= Html.AntiForgeryToken() %>
			<%= Html.ValidationSummary(true) %>
			<table cellpadding="0" cellspacing="0" width="100%"> 
				<tr> 
					<th class="row_header" colspan="3">Edit Price Tracking Hotel Setup</th> 
				</tr>
                <tr>
                    <td><label for="ClientHasProvidedWrittenApprovalFlag">Client has provided written approval</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientHasProvidedWrittenApprovalFlag, ViewData["ClientHasProvidedWrittenApprovalFlagList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientHasProvidedWrittenApprovalFlag)%></td>
                </tr>
                <tr>
                    <td><label for="ClientHasHotelFeesInMSAFlag">Client has hotel fees in MSA</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientHasHotelFeesInMSAFlag, ViewData["ClientHasHotelFeesInMSAFlagList"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientHasHotelFeesInMSAFlag)%></td>
                </tr>
				<tr>
					<td colspan="3">&nbsp;</td>
				</tr>
				<tr> 
					<td colspan="3"><strong>Pricing Model (select either option and enter value)</strong></td> 
				</tr>
				<tr>
					<td><label for="SharedSavingsFlag">Shared Savings (% Savings)</label></td>
					<td>
						<%= Html.DropDownListFor(model => model.SharedSavingsFlag, ViewData["SharedSavingsList"] as SelectList)%>
						<%= Html.TextBoxFor(model => model.SharedSavingsAmount, new { maxlength = "6", size = "13"})%> 
						Recommended
					</td>
					<td>
						<%= Html.ValidationMessageFor(model => model.SharedSavingsFlag)%>
						<%= Html.ValidationMessageFor(model => model.SharedSavingsAmount)%>
						<label id="lblSharedSavingsFlag"></label>
					</td>
				</tr>
				<tr>
					<td><label for="TransactionFeeFlag">Transaction Fee (Fee per Transaction)</label></td>
					<td>
						<%= Html.DropDownListFor(model => model.TransactionFeeFlag, ViewData["TransactionFeeList"] as SelectList)%>
						<%= Html.TextBoxFor(model => model.TransactionFeeAmount, new { maxlength = "13", size = "13"})%>
					</td>
					<td>
						<%= Html.ValidationMessageFor(model => model.TransactionFeeFlag)%>
						<%= Html.ValidationMessageFor(model => model.TransactionFeeAmount)%>
						<label id="lblTransactionFeeFlag"></label>
					</td>
				</tr>
				<tr>
					<td colspan="3">&nbsp;</td>
				</tr>
				<tr> 
					<td colspan="3"><strong>Annual Hotel Volume</strong></td> 
				</tr>
				<tr>
					<td><label for="AnnualTransactionCount">Number of annual transactions</label></td>
					<td><%= Html.TextBoxFor(model => model.AnnualTransactionCount, new { @Value = "", maxlength = "9", size = "16"})%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.AnnualTransactionCount)%></td>
				</tr>
				<tr>
					<td><label for="AnnualSpendAmount">Annual Spend</label></td>
					<td><%= Html.TextBoxFor(model => model.AnnualSpendAmount, new { @Value = "", maxlength = "16", size = "16"})%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.AnnualSpendAmount)%></td>
				</tr>
				<tr>
					<td colspan="3">&nbsp;</td>
				</tr>
                <tr>
                    <td><label for="ClientUsesConfermaVirtualCardsFlag">Client uses Conferma virtual cards</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientUsesConfermaVirtualCardsFlag, ViewData["ClientUsesConfermaVirtualCardsFlagList"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientUsesConfermaVirtualCardsFlag)%></td>
                </tr>
                <tr>
                    <td><label for="CentralFulfillmentTimeZoneRuleCode">Central Fulfillment Time Zone</label></td>
                    <td><%= Html.DropDownListFor(model => model.CentralFulfillmentTimeZoneRuleCode, ViewData["CentralFulfillmentTimeZoneRuleCodes"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.CentralFulfillmentTimeZoneRuleCode)%></td>
                </tr>
                <tr>
                    <td><label for="CentralFulfillmentBusinessHours">Central Fulfillment Business Hours</label></td>
                    <td><%= Html.TextBoxFor(model => model.CentralFulfillmentBusinessHours, new { maxlength = "50" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.CentralFulfillmentBusinessHours)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td><label for="Comment"><strong>Comments</strong></label></td>
                    <td><%= Html.TextAreaFor(model => model.Comment, new { maxlength = "240" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Comment)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
				<tr> 
					<td colspan="3"><strong>Thresholds and Settings</strong></td> 
				</tr>
				<tr>
					<td><label for="CurrencyCode">Currency</label></td>
					<td><%= Html.DropDownListFor(model => model.CurrencyCode, ViewData["Currencies"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.CurrencyCode)%></td>
				</tr>
				<tr>
					<td><label for="EstimatedCWTRebookingFeeAmount">Estimated CWT Rebooking Fees</label></td>
					<td><%= Html.TextBoxFor(model => model.EstimatedCWTRebookingFeeAmount, new { @Value = "", maxlength = "13", size = "13"})%></td>
					<td><%= Html.ValidationMessageFor(model => model.EstimatedCWTRebookingFeeAmount)%></td>
				</tr>
				<tr>
					<td><label for="CWTVoidRefundFeeAmount">CWT Refund Fees (if applicable)</label></td>
					<td><%= Html.TextBoxFor(model => model.CWTVoidRefundFeeAmount, new { @Value = "", maxlength = "13", size = "13"})%></td>
					<td><%= Html.ValidationMessageFor(model => model.CWTVoidRefundFeeAmount)%></td>
				</tr>
				<tr>
					<td><label for="ThresholdAmount">Threshold</label></td>
					<td><%= Html.TextBoxFor(model => model.ThresholdAmount, new { @Value = "", maxlength = "13", size = "13"})%></td>
					<td><%= Html.ValidationMessageFor(model => model.ThresholdAmount)%></td>
				</tr>
				<tr>
					<td align="right"><label for="CalculatedTotalThresholdAmount">Calculated Total Threshold</label></td>
					<td colspan="2"><label id="lblCalculatedTotalThresholdAmount"><%= Html.Encode(Model.CalculatedTotalThresholdAmount)%></label></td>
				</tr>
				<tr>
					<td colspan="3">&nbsp;</td>
				</tr>
				<tr> 
					<td colspan="3"><strong>Rate Codes</strong></td>
				</tr>
				<tr>
					<td><label for="CorporateRateTrackingCode1">Corporate Rate Codes to Track (maximum of four)</label></td>
					<td colspan="2">
						<%= Html.TextBoxFor(model => model.CorporateRateTrackingCode1, new { maxlength = "15", size = "15"})%> 
						<%= Html.TextBoxFor(model => model.CorporateRateTrackingCode2, new { maxlength = "15", size = "15"})%> 
						<%= Html.TextBoxFor(model => model.CorporateRateTrackingCode3, new { maxlength = "15", size = "15"})%> 
						<%= Html.TextBoxFor(model => model.CorporateRateTrackingCode4, new { maxlength = "15", size = "15"})%>
						<%= Html.ValidationMessageFor(model => model.CorporateRateTrackingCode1)%>
						<%= Html.ValidationMessageFor(model => model.CorporateRateTrackingCode2)%>
						<%= Html.ValidationMessageFor(model => model.CorporateRateTrackingCode3)%>
						<%= Html.ValidationMessageFor(model => model.CorporateRateTrackingCode4)%>
					</td>
				</tr>
				<tr>
					<td colspan="3">&nbsp;</td>
				</tr>
				<tr>
					<td><label for="CreationTimestamp">Creation Date</label></td>
					<td><%= Html.Encode(DateTime.Now.ToString("dd/MMM/yyyy"))%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="MissedSavingsCode">Last Updated</label></td>
					<td><%= Html.Encode(DateTime.Now.ToString("dd/MMM/yyyy"))%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="MissedSavingsCode">Last Updated By</label></td>
					<td><%= Html.Encode(ViewData["SystemUser"].ToString())%></td>
					<td></td>
				</tr>
				<tr>
					<td colspan="3">&nbsp;</td>
				</tr>
				<tr> 
					<td colspan="3"><strong>Admin only</strong></td>
				</tr>
				<%if (ViewData["AdminAccess"].ToString() == "WriteAccess"){%>
					<tr class="enabledDate">
						<td><label for="EnabledDate">Enabled Date</label></td>
						<td><%= Html.EditorFor(model => model.EnabledDate)%></td>
						<td><%= Html.ValidationMessageFor(model => model.EnabledDate) %></td>
					</tr>
					<tr class="deactivationDate">
						<td><label for="DeactivationDate">Deactivation  Date</label></td>
						<td><%= Html.EditorFor(model => model.DeactivationDate)%></td>
						<td><%= Html.ValidationMessageFor(model => model.DeactivationDate) %></td>
					</tr>
					<tr>
						<td><label for="EnableValueTrackingFlag">Enable Hotel Value Tracking</label></td>
						<td>
							<%= Html.DropDownListFor(model => model.EnableValueTrackingFlag, ViewData["EnableValueTrackingFlagList"] as SelectList, "Please Select...", new { @class = "price-tracking-admin-dropdown" })%> Yes for all US, No for all other regions
						</td>
						<td> <%= Html.ValidationMessageFor(model => model.EnableValueTrackingFlag)%></td>
					</tr>
					<tr>
						<td><label for="TMCFeeThreshold">Value Threshold Rebooking Fee</label></td>
						<td><%= Html.TextBoxFor(model => model.TMCFeeThreshold, new { maxlength = "13", size = "22"})%></td>
						<td><%= Html.ValidationMessageFor(model => model.TMCFeeThreshold)%></td>
					</tr>
                    <tr>
					    <td><label for="BreakfastChangesAllowedFlag">Breakfast Changes</label></td>
					    <td><%= Html.DropDownListFor(model => model.BreakfastChangesAllowedFlag, ViewData["BreakfastChangesAllowedFlagList"] as SelectList)%></td>
					    <td><%= Html.ValidationMessageFor(model => model.BreakfastChangesAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="BreakfastValue">Breakfast Value</label></td>
					    <td><%= Html.TextBoxFor(model => model.BreakfastValue, new { maxlength = "13", size = "22"})%></td>
					    <td><%= Html.ValidationMessageFor(model => model.BreakfastValue)%></td>
				    </tr>
					<tr>
						<td><label for="ParkingValue">Parking Value</label></td>
						<td><%= Html.TextBoxFor(model => model.ParkingValue, new { maxlength = "13", size = "22"})%></td>
						<td><%= Html.ValidationMessageFor(model => model.ParkingValue)%></td>
					</tr>
					<tr>
						<td><label for="InternetAccessValue">Internet Access Value</label></td>
						<td><%= Html.TextBoxFor(model => model.InternetAccessValue, new { maxlength = "13", size = "22"})%></td>
						<td><%= Html.ValidationMessageFor(model => model.InternetAccessValue)%></td>
					</tr>
				    <tr>
					    <td><label for="RoomTypeUpgradeAllowedFlag">Room Type Changes</label></td>
					    <td><%= Html.DropDownListFor(model => model.RoomTypeUpgradeAllowedFlag, ViewData["RoomTypeUpgradeAllowedFlagList"] as SelectList)%></td>
					    <td><%= Html.ValidationMessageFor(model => model.RoomTypeUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="BeddingTypeUpgradeAllowedFlag">Bedding Type Changes</label></td>
					    <td><%= Html.DropDownListFor(model => model.BeddingTypeUpgradeAllowedFlag, ViewData["BeddingTypeUpgradeAllowedFlagList"] as SelectList)%></td>
					    <td><%= Html.ValidationMessageFor(model => model.BeddingTypeUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="KingQueenUpgradeAllowedFlag">King/Queen to Non King/Queen</label></td>
					    <td><%= Html.DropDownListFor(model => model.KingQueenUpgradeAllowedFlag, ViewData["KingQueenUpgradeAllowedFlagList"] as SelectList)%></td>
					    <td><%= Html.ValidationMessageFor(model => model.KingQueenUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="HotelRateCodeUpgradeAllowedFlag">Hotel Rate Code Changes</label></td>
					    <td><%= Html.DropDownListFor(model => model.HotelRateCodeUpgradeAllowedFlag, ViewData["HotelRateCodeUpgradeAllowedFlagList"] as SelectList)%></td>
					    <td><%= Html.ValidationMessageFor(model => model.HotelRateCodeUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="CancellationPolicyUpgradeAllowedFlag">Cancelation Policy Changes</label></td>
					    <td><%= Html.DropDownListFor(model => model.CancellationPolicyUpgradeAllowedFlag, ViewData["CancellationPolicyUpgradeAllowedFlagList"] as SelectList)%></td>
					    <td><%= Html.ValidationMessageFor(model => model.CancellationPolicyUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="CWTRateTrackingCode1">CWT Rate Codes to Track</label></td>
					    <td colspan="2">
						    <%= Html.TextBoxFor(model => model.CWTRateTrackingCode1, new { maxlength = "15", size = "15"})%> 
						    <%= Html.TextBoxFor(model => model.CWTRateTrackingCode2, new { maxlength = "15", size = "15"})%> 
						    <%= Html.ValidationMessageFor(model => model.CWTRateTrackingCode1)%>
						    <%= Html.ValidationMessageFor(model => model.CWTRateTrackingCode2)%>
					    </td>
				    </tr>
					<tr>
						<td><label for="ImportPseudoCityOrOfficeId">Import PCC/OID </label></td>
						<td><%= Html.TextBoxFor(model => model.ImportPseudoCityOrOfficeId, new { maxlength = "9", size = "22"})%></td>
						<td><%= Html.ValidationMessageFor(model => model.ImportPseudoCityOrOfficeId)%></td>
					</tr>
					<tr>
						<td><label for="ImportQueueInClientPseudoCityOrOfficeId">Import Queue in Client PCC/OID</label></td>
						<td><%= Html.TextBoxFor(model => model.ImportQueueInClientPseudoCityOrOfficeId, new { maxlength = "20", size = "22"})%></td>
						<td><%= Html.ValidationMessageFor(model => model.ImportQueueInClientPseudoCityOrOfficeId)%></td>
					</tr>
					<tr>
						<td><label for="PricingPseudoCityOrOfficeId">Pricing PCC/OID</label></td>
						<td><%= Html.TextBoxFor(model => model.PricingPseudoCityOrOfficeId, new { maxlength = "9", size = "22"})%></td>
						<td><%= Html.ValidationMessageFor(model => model.PricingPseudoCityOrOfficeId)%></td>
					</tr>
					<tr>
						<td><label for="SavingsQueueId">Savings Queue ID</label></td>
						<td><%= Html.TextBoxFor(model => model.SavingsQueueId, new { maxlength = "20", size = "22"})%></td>
						<td><%= Html.ValidationMessageFor(model => model.SavingsQueueId)%></td>
					</tr>
					<tr>
						<td><label for="AlphaCodeRemarkField">Alpha Code Remarks Field</label></td>
						<td><%= Html.TextBoxFor(model => model.AlphaCodeRemarkField, new { maxlength = "20", size = "22"})%></td>
						<td><%= Html.ValidationMessageFor(model => model.AlphaCodeRemarkField)%></td>
					</tr>
					<tr>
					<td><label>Hotel Tracking Alerts Email</label></td>
						<td colspan="2" style="padding: 0;">
							<table cellpadding="0" cellspacing="0" border="0" width="100%">
								<tbody>
									<tr class="HotelTrackingAlertsEmailAddresses_Line_Item">
										<td width="30%">
											<%= Html.TextBox("PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress[0].EmailAddress", "", new { maxlength="100", size = "22", @Class = "HotelTrackingAlertsEmailAddress" })%>
										</td>
										<td width="80%">
											<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
											<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
										</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
				<% } else { %>
					<tr>
						<td><label for="EnabledDate">Enabled Date</label></td>
						<td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToShortDateString() : "No Enabled Date")%></td>
						<td><%= Html.HiddenFor(model => model.EnabledDate)%></td>
					</tr>
					<tr>
						<td><label for="DeactivationDate">Deactivation  Date</label></td>
						<td><%= Html.Encode(Model.DeactivationDate.HasValue ? Model.DeactivationDate.Value.ToShortDateString() : "No Deactivation Date")%></td>
						<td><%= Html.HiddenFor(model => model.DeactivationDate)%></td>
					</tr>
					<tr>
						<td><label for="EnableValueTrackingFlag">Enable Hotel Value Tracking</label></td>
						<td><%= Html.Encode(Model.EnableValueTrackingFlag == true ? "True" : "False")%></td>
						<td><%= Html.HiddenFor(model => model.EnableValueTrackingFlag)%></td>
					</tr>
					<tr>
						<td><label for="TMCFeeThreshold">Value Threshold Rebooking Fee</label></td>
						<td><%= Html.Encode(Model.TMCFeeThreshold)%></td>
						<td><%= Html.HiddenFor(model => model.TMCFeeThreshold)%></td>
					</tr>
                    <tr>
						<td><label for="BreakfastChangesAllowedFlag">Breakfast Changes</label></td>
						<td><%= Html.Encode(Model.BreakfastChangesAllowedFlag == true ? "Allowed" : "Not Allowed")%></td>
						<td><%= Html.HiddenFor(model => model.BreakfastChangesAllowedFlag)%></td>
					</tr>
					<tr>
						<td><label for="BreakfastValue">Breakfast Value</label></td>
						<td><%= Html.Encode(Model.BreakfastValue)%></td>
						<td><%= Html.HiddenFor(model => model.BreakfastValue)%></td>
					</tr>
					<tr>
						<td><label for="ParkingValue">Parking Value</label></td>
						<td><%= Html.Encode(Model.ParkingValue)%></td>
						<td><%= Html.HiddenFor(model => model.ParkingValue)%></td>
					</tr>
					<tr>
						<td><label for="InternetAccessValue">Internet Access Value</label></td>
						<td><%= Html.Encode(Model.InternetAccessValue)%></td>
						<td><%= Html.HiddenFor(model => model.InternetAccessValue)%></td>
					</tr>
                    <tr>
					    <td><label for="RoomTypeUpgradeAllowedFlag">Room Type Changes</label></td>
					    <td><%= Html.Encode(Model.RoomTypeUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed")%></td>
					    <td><%= Html.HiddenFor(model => model.RoomTypeUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="BeddingTypeUpgradeAllowedFlag">Bedding Type Changes</label></td>
					    <td><%= Html.Encode(Model.BeddingTypeUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed")%></td>
					    <td><%= Html.HiddenFor(model => model.BeddingTypeUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="KingQueenUpgradeAllowedFlag">King/Queen to Non King/Queen</label></td>
					    <td><%= Html.Encode(Model.KingQueenUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed")%></td>
					    <td><%= Html.HiddenFor(model => model.KingQueenUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="HotelRateCodeUpgradeAllowedFlag">Hotel Rate Code Changes</label></td>
					    <td><%= Html.Encode(Model.HotelRateCodeUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed")%></td>
					    <td><%= Html.HiddenFor(model => model.HotelRateCodeUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="CancellationPolicyUpgradeAllowedFlag">Cancelation Policy Changes</label></td>
					    <td><%= Html.Encode(Model.CancellationPolicyUpgradeAllowedFlag == true ? "Allowed" : "Not Allowed")%></td>
					    <td><%= Html.HiddenFor(model => model.CancellationPolicyUpgradeAllowedFlag)%></td>
				    </tr>
				    <tr>
					    <td><label for="CWTRateTrackingCode1">CWT Rate Codes to Track</label></td>
					    <td colspan="2">
						    <%= Html.Encode(Model.CWTRateTrackingCode1)%> 
						    <%= Html.Encode(Model.CWTRateTrackingCode2)%> 
						    <%= Html.HiddenFor(model => model.CWTRateTrackingCode1)%>
						    <%= Html.HiddenFor(model => model.CWTRateTrackingCode2)%>
					    </td>
				    </tr>
					<tr>
						<td><label for="ImportPseudoCityOrOfficeId">Import PCC/OID</label></td>
						<td><%= Html.Encode(Model.ImportPseudoCityOrOfficeId)%></td>
						<td><%= Html.HiddenFor(model => model.ImportPseudoCityOrOfficeId)%></td>
					</tr>
					<tr>
						<td><label for="ImportQueueInClientPseudoCityOrOfficeId">Import Queue in Client PCC/OID</label></td>
						<td><%= Html.Encode(Model.ImportQueueInClientPseudoCityOrOfficeId)%></td>
						<td><%= Html.HiddenFor(model => model.ImportQueueInClientPseudoCityOrOfficeId)%></td>
					</tr>
					<tr>
						<td><label for="PricingPseudoCityOrOfficeId">Pricing PCC/OID</label></td>
						<td><%= Html.Encode(Model.PricingPseudoCityOrOfficeId)%></td>
						<td><%= Html.HiddenFor(model => model.PricingPseudoCityOrOfficeId)%></td>
					</tr>
					<tr>
						<td><label for="SavingsQueueId">Savings Queue ID</label></td>
						<td><%= Html.Encode(Model.SavingsQueueId)%></td>
						<td><%= Html.HiddenFor(model => model.SavingsQueueId)%></td>
					</tr>
					<tr>
						<td><label for="AlphaCodeRemarkField">Alpha Code Remarks Field</label></td>
						<td><%= Html.Encode(Model.AlphaCodeRemarkField)%></td>
						<td><%= Html.HiddenFor(model => model.AlphaCodeRemarkField)%></td>
					</tr>
                    <tr>
						<td><label for="AlphaCodeRemarkField">Hotel Tracking Alerts Email</label></td>
						<td></td>
						<td></td>
					</tr>
				<% } %>
				<tr>
					<td width="40%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="20%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
					<td class="row_footer_blank_right"><input type="submit" value="Confirm Edit" title="Confirm Edit" class="red"/></td>
				</tr>
			</table>
			<%= Html.HiddenFor(model => model.PriceTrackingSetupGroupId)%>
			<% } %>
		</div>
	</div>
	<script src="<%=Url.Content("~/Scripts/ERD/PriceTrackingSetupGroupItemHotel.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Setups", "Main", new { controller = "PriceTrackingSetupGroup", action = "ListUnDeleted", }, new { title = "Price Tracking Setups" })%> &gt;
<%=Html.RouteLink(ViewData["PriceTrackingSetupGroupName"].ToString(), "Main", new { controller = "PriceTrackingSetupGroup", action = "View", id = ViewData["PriceTrackingSetupGroupId"].ToString()}, new { title = ViewData["PriceTrackingSetupGroupName"].ToString() })%> &gt;
Edit Price Tracking Hotel Setup
</asp:Content>