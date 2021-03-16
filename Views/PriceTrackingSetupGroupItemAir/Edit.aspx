<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PriceTrackingSetupGroupItemAir>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Setup Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Air Setup</div></div>
        <div id="content">       
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "PriceTrackingSetupGroupItemAir", action = "Edit",id = Model.PriceTrackingSetupGroupItemAirId }, FormMethod.Post, new { id = "form0", autocomplete="off" })) {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr> 
			        <th class="row_header" colspan="3">Edit Price Tracking Air Setup</th> 
		        </tr> 
                <tr>
                    <td><label for="ClientHasProvidedWrittenApprovalFlag">Client has provided written approval</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientHasProvidedWrittenApprovalFlag, ViewData["ClientHasProvidedWrittenApprovalFlagList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientHasProvidedWrittenApprovalFlag)%></td>
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
                    <td colspan="3"><strong>Annual Air Volume</strong></td> 
                </tr>
                <tr>
                    <td><label for="AnnualTransactionCount">Number of annual transactions</label></td>
                    <td><%= Html.TextBoxFor(model => model.AnnualTransactionCount, new { maxlength = "9", size = "16"})%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.AnnualTransactionCount)%></td>
                </tr>
                <tr>
                    <td><label for="AnnualSpendAmount">Annual Spend</label></td>
                    <td><%= Html.TextBoxFor(model => model.AnnualSpendAmount, new { maxlength = "16", size = "16"})%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.AnnualSpendAmount)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr> 
                    <td colspan="3"><strong>Thresholds</strong></td> 
                </tr>
                <tr>
                    <td><label for="CurrencyCode">Currency</label></td>
                    <td><%= Html.DropDownListFor(model => model.CurrencyCode, ViewData["Currencies"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CurrencyCode)%></td>
                </tr>
                <tr>
                    <td><label for="EstimatedCWTRebookingFeeAmount">Estimated CWT Rebooking Fees</label></td>
                    <td><%= Html.TextBoxFor(model => model.EstimatedCWTRebookingFeeAmount, new { maxlength = "13", size = "13"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EstimatedCWTRebookingFeeAmount)%></td>
                </tr>
                <tr>
                    <td><label for="CWTVoidRefundFeeAmount">CWT Void/Refund Fees (if applicable)</label></td>
                    <td><%= Html.TextBoxFor(model => model.CWTVoidRefundFeeAmount, new { maxlength = "13", size = "13"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.CWTVoidRefundFeeAmount)%></td>
                </tr>
                <tr>
                    <td><label for="NoPenaltyVoidWindowThresholdAmount">No-Penalty/Void Window Threshold</label></td>
                    <td><%= Html.TextBoxFor(model => model.NoPenaltyVoidWindowThresholdAmount, new { maxlength = "13", size = "13"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.NoPenaltyVoidWindowThresholdAmount)%></td>
                </tr>
                <tr>
                    <td><label for="OutsideVoidPenaltyThresholdAmount">Outside Void (Penalty) Threshold</label></td>
                    <td><%= Html.TextBoxFor(model => model.OutsideVoidPenaltyThresholdAmount, new { maxlength = "13", size = "13"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OutsideVoidPenaltyThresholdAmount)%></td>
                </tr>
                <tr>
                    <td><label for="TimeZoneRuleCode">Ticketing PCC/OID Time Zone</label></td>
                    <td><%= Html.DropDownListFor(model => model.TimeZoneRuleCode, ViewData["TimeZoneRules"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TimeZoneRuleCode)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr> 
                    <td><strong>Switch Window</strong></td>
                    <td colspan="2">Fare Tracking calculates saving opportunities comparing fully refundable fares to non-refundable fares.  Alerts start processing closer to day of departure with increased likelihood of travel.</td>
                </tr>
                <tr>
                    <td><label for="RefundableToRefundableWithPenaltyForRefundAllowedFlag">Refundable to Refundable with penalty for refund</label></td>
                    <td><%= Html.DropDownListFor(model => model.RefundableToRefundableWithPenaltyForRefundAllowedFlag, ViewData["RefundableToRefundableWithPenaltyForRefundAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.RefundableToRefundableWithPenaltyForRefundAllowedFlag)%></td>
                </tr>
                <tr>
                    <td class="indented-value"><label for="RefundableToRefundablePreDepartureDayAmount">If allowed, number of day predeparture to track Refundable to Refundable</label></td>
                    <td><%= Html.TextBoxFor(model => model.RefundableToRefundablePreDepartureDayAmount, new { maxlength = "3", size = "13"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.RefundableToRefundablePreDepartureDayAmount)%></td>
                </tr>
                <tr>
                    <td><label for="NoPenaltyRefundableToPenaltyRefundableThresholdAmount">No-Penalty Refundable to penalty Refundable threshold amount</label></td>
                    <td><%= Html.TextBoxFor(model => model.NoPenaltyRefundableToPenaltyRefundableThresholdAmount, new { maxlength = "13", size = "13"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.NoPenaltyRefundableToPenaltyRefundableThresholdAmount)%></td>
                </tr>
                <tr>
                    <td><label for="RefundableToNonRefundableAllowedFlag">Refundable to Non-Refundable</label></td>
                    <td><%= Html.DropDownListFor(model => model.RefundableToNonRefundableAllowedFlag, ViewData["RefundableToNonRefundableAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.RefundableToNonRefundableAllowedFlag)%></td>
                </tr>
                <tr>
                    <td class="indented-value"><label for="RefundableToNonRefundablePreDepartureDayAmount">If allowed, number of day predeparture to track Refundable to Non-Refundable</label></td>
                    <td><%= Html.TextBoxFor(model => model.RefundableToNonRefundablePreDepartureDayAmount, new { maxlength = "3", size = "13"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.RefundableToNonRefundablePreDepartureDayAmount)%></td>
                </tr>
                <tr>
                    <td><label for="NoPenaltyRefundableToPenaltyNonRefundableThresholdAmount">No-Penalty Refundable to penalty Non-Refundable threshold amount</label></td>
                    <td><%= Html.TextBoxFor(model => model.NoPenaltyRefundableToPenaltyNonRefundableThresholdAmount, new { maxlength = "13", size = "13"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.NoPenaltyRefundableToPenaltyNonRefundableThresholdAmount)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr> 
                    <td colspan="3"><strong>Refunds/Exchange</strong></td>
                </tr>
                <tr>
                    <td><label for="VoidWindowAllowedFlag">Void window</label></td>
                    <td><%= Html.DropDownListFor(model => model.VoidWindowAllowedFlag, ViewData["VoidWindowAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.VoidWindowAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="RefundableToRefundableOutsideVoidWindowAllowedFlag">Refundable to Refundable outside void window (Exchange with partial refund)</label></td>
                    <td><%= Html.DropDownListFor(model => model.RefundableToRefundableOutsideVoidWindowAllowedFlag, ViewData["RefundableToRefundableOutsideVoidWindowAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.RefundableToRefundableOutsideVoidWindowAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag">Non-Refundable to Non-Refundable outside void window (MCO)</label></td>
                    <td><%= Html.DropDownListFor(model => model.NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag, ViewData["NonRefundableToNonRefundableOutsideVoidWindowAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="ExchangesAllowedFlag">Exchanges</label></td>
                    <td><%= Html.DropDownListFor(model => model.ExchangesAllowedFlag, ViewData["ExchangesAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExchangesAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="VoidExchangeAllowedFlag">Void an exchange</label></td>
                    <td><%= Html.DropDownListFor(model => model.VoidExchangeAllowedFlag, ViewData["VoidExchangeAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.VoidExchangeAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="ExchangePreviousExchangeAllowedFlag">Exchange a previous Exchange</label></td>
                    <td><%= Html.DropDownListFor(model => model.ExchangePreviousExchangeAllowedFlag, ViewData["ExchangePreviousExchangeAllowedFlagList"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExchangePreviousExchangeAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag">Non-Refundable to lower Non-Refundable with different change fee</label></td>
                    <td><%= Html.DropDownListFor(model => model.NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag, ViewData["NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="RefundableToLowerNonRefundableAllowedFlag">Refundable to lower Non-Refundable</label></td>
                    <td><%= Html.DropDownListFor(model => model.RefundableToLowerNonRefundableAllowedFlag, ViewData["RefundableToLowerNonRefundableAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.RefundableToLowerNonRefundableAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="RefundableToLowerRefundableAllowedFlag">Refundable to lower Refundable</label></td>
                    <td><%= Html.DropDownListFor(model => model.RefundableToLowerRefundableAllowedFlag, ViewData["RefundableToLowerRefundableAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.RefundableToLowerRefundableAllowedFlag)%></td>
                </tr>
                <tr>
                    <td><label for="NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag">Non-penalty Refundable to lower penalty Refundable</label></td>
                    <td><%= Html.DropDownListFor(model => model.NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag, ViewData["NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr> 
                    <td colspan="3"><strong>Airline Change Fees (NORAM POS only)</strong></td>
                </tr>
                <tr>
                    <td><label for="ChargeChangeFeeUpFrontForSpecificCarriersFlag">Charge change fee up front for specific carriers (eg. United)</label></td>
                    <td><%= Html.DropDownListFor(model => model.ChargeChangeFeeUpFrontForSpecificCarriersFlag, ViewData["ChargeChangeFeeUpFrontForSpecificCarriersFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ChargeChangeFeeUpFrontForSpecificCarriersFlag)%></td>
                </tr>
                <tr>
                    <td><label for="ChangeFeeMustBeUsedFromResidualValueFlag">Change fee must be used from residual value</label></td>
                    <td><%= Html.DropDownListFor(model => model.ChangeFeeMustBeUsedFromResidualValueFlag, ViewData["ChangeFeeMustBeUsedFromResidualValueFlagList"] as SelectList)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ChangeFeeMustBeUsedFromResidualValueFlag)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr> 
                    <td><strong>Private/Negotiated Fares</strong></td>
                    <td colspan="2">Note: Fare Tracking can track to private, negotiated fares.</td>
                </tr>
                <tr>
                    <td><label for="TrackPrivateNegotiatedFareFlag">Track Private/Negotiated fares</label></td>
                    <td><%= Html.DropDownListFor(model => model.TrackPrivateNegotiatedFareFlag, ViewData["TrackPrivateNegotiatedFareFlagList"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TrackPrivateNegotiatedFareFlag)%></td>
                </tr>
                <tr>
                    <td><label for="NegotiatedPricingTrackingCode1">Negotiated pricing codes to track (maximum of four)</label></td>
                    <td colspan="2">
                        <%= Html.TextBoxFor(model => model.NegotiatedPricingTrackingCode1, new { maxlength = "15", size = "15"})%> 
                        <%= Html.TextBoxFor(model => model.NegotiatedPricingTrackingCode2, new { maxlength = "15", size = "15"})%> 
                        <%= Html.TextBoxFor(model => model.NegotiatedPricingTrackingCode3, new { maxlength = "15", size = "15"})%> 
                        <%= Html.TextBoxFor(model => model.NegotiatedPricingTrackingCode4, new { maxlength = "15", size = "15"})%>
                        <%= Html.ValidationMessageFor(model => model.NegotiatedPricingTrackingCode1)%>
                        <%= Html.ValidationMessageFor(model => model.NegotiatedPricingTrackingCode2)%>
                        <%= Html.ValidationMessageFor(model => model.NegotiatedPricingTrackingCode3)%>
                        <%= Html.ValidationMessageFor(model => model.NegotiatedPricingTrackingCode4)%>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr> 
                    <td>
                        <label for="CompanyProfilesRequiringFIQID"><strong>Shared PCC/OID</strong></label><br />
                        Provide name of all company profiles/STARS to add FIQID
                    </td> 
                    <td><%= Html.TextAreaFor(model => model.CompanyProfilesRequiringFIQID, new { maxlength = "240" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.CompanyProfilesRequiringFIQID)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr> 
                    <td><strong>Reason Codes</strong></td>
                    <td colspan="2">Not required for NORAM</td>
                </tr>
                <tr>
                    <td><label for="RealisedSavingsCode">Realised Savings Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.RealisedSavingsCode, new { maxlength = "3", size = "3"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.RealisedSavingsCode)%></td>
                </tr>
                <tr>
                    <td><label for="MissedSavingsCode">Missed Savings Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.MissedSavingsCode, new { maxlength = "3", size = "3"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.MissedSavingsCode)%></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td><label for="CreationTimestamp">Creation Date</label></td>
                    <td><%= Html.Encode(Model.CreationTimestamp.Value.ToString("dd/MMM/yyyy"))%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="MissedSavingsCode">Last Updated</label></td>
                    <td><%= Html.Encode(Model.LastUpdateTimestamp.HasValue ? Model.LastUpdateTimestamp.Value.ToString("dd/MMM/yyyy") : Model.CreationTimestamp.Value.ToString("dd/MMM/yyyy"))%></td>
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
                        <td><label for="ImportPseudoCityOrOfficeId">Import PCC/OID</label></td>
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
                        <td><label for="AutomaticReticketingFlag">Automatic Reticketing</label></td>
                        <td><%= Html.DropDownListFor(model => model.AutomaticReticketingFlag, ViewData["AutomaticReticketingFlagList"] as SelectList)%></td>
                        <td><%= Html.ValidationMessageFor(model => model.AutomaticReticketingFlag)%></td>
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
                        <td><label for="AutomaticReticketingFlag">Automatic Reticketing</label></td>
                        <td><%= Html.Encode(Model.AutomaticReticketingFlag)%></td>
                        <td><%= Html.HiddenFor(model => model.AutomaticReticketingFlag)%></td>
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
            <%= Html.HiddenFor(model => model.VersionNumber)%>
            <%= Html.HiddenFor(model => model.PriceTrackingSetupGroupItemAirId) %>
        <% } %>
        </div>
    </div>

<script src="<%=Url.Content("~/Scripts/ERD/PriceTrackingSetupGroupItemAir.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Setups", "Main", new { controller = "PriceTrackingSetupGroup", action = "ListUnDeleted", }, new { title = "Price Tracking Setups" })%> &gt;
<%=Html.RouteLink(ViewData["PriceTrackingSetupGroupName"].ToString(), "Main", new { controller = "PriceTrackingSetupGroup", action = "View", id = ViewData["PriceTrackingSetupGroupId"].ToString()}, new { title = ViewData["PriceTrackingSetupGroupName"].ToString() })%> &gt;
Edit Price Tracking Air Setup
</asp:Content>