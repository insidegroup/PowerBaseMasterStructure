using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using CWTDesktopDatabase.ViewModels;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingSetupGroupItemAirRepository
    {
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

        //Get one PriceTrackingSetupGroupItemAir
        public PriceTrackingSetupGroupItemAir GetPriceTrackingSetupGroupItemAir(int id)
        {
            return db.PriceTrackingSetupGroupItemAirs.SingleOrDefault(c => c.PriceTrackingSetupGroupItemAirId == id);
        }

        //Get one PriceTrackingSetupGroupItemAir
        public PriceTrackingSetupGroupItemAir GetPriceTrackingSetupGroupItemAirByGroupId(int id)
        {
            return db.PriceTrackingSetupGroupItemAirs.SingleOrDefault(c => c.PriceTrackingSetupGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(PriceTrackingSetupGroupItemAir group)
        {

        }

        //Add to DB
        public void Add(PriceTrackingSetupGroupItemAir priceTrackingSetupGroupItemAir)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPriceTrackingSetupGroupItemAir_v1(
                priceTrackingSetupGroupItemAir.PriceTrackingSetupGroupId,
                priceTrackingSetupGroupItemAir.ClientHasProvidedWrittenApprovalFlag,
                priceTrackingSetupGroupItemAir.SharedSavingsFlag,
                priceTrackingSetupGroupItemAir.SharedSavingsAmount,
                priceTrackingSetupGroupItemAir.TransactionFeeFlag,
                priceTrackingSetupGroupItemAir.TransactionFeeAmount,
                priceTrackingSetupGroupItemAir.CentralFulfillmentTimeZoneRuleCode,
                priceTrackingSetupGroupItemAir.CentralFulfillmentBusinessHours,
                priceTrackingSetupGroupItemAir.Comment,
                priceTrackingSetupGroupItemAir.AnnualTransactionCount,
                priceTrackingSetupGroupItemAir.AnnualSpendAmount,
                priceTrackingSetupGroupItemAir.CurrencyCode,
                priceTrackingSetupGroupItemAir.EstimatedCWTRebookingFeeAmount,
                priceTrackingSetupGroupItemAir.CWTVoidRefundFeeAmount,
                priceTrackingSetupGroupItemAir.NoPenaltyVoidWindowThresholdAmount,
                priceTrackingSetupGroupItemAir.OutsideVoidPenaltyThresholdAmount,
                priceTrackingSetupGroupItemAir.TimeZoneRuleCode,
                priceTrackingSetupGroupItemAir.RefundableToRefundableWithPenaltyForRefundAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToRefundablePreDepartureDayAmount,
                priceTrackingSetupGroupItemAir.NoPenaltyRefundableToPenaltyRefundableThresholdAmount,
                priceTrackingSetupGroupItemAir.RefundableToNonRefundableAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToNonRefundablePreDepartureDayAmount,
                priceTrackingSetupGroupItemAir.NoPenaltyRefundableToPenaltyNonRefundableThresholdAmount,
                priceTrackingSetupGroupItemAir.VoidWindowAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToRefundableOutsideVoidWindowAllowedFlag,
                priceTrackingSetupGroupItemAir.NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag,
                priceTrackingSetupGroupItemAir.ExchangesAllowedFlag,
                priceTrackingSetupGroupItemAir.VoidExchangeAllowedFlag,
                priceTrackingSetupGroupItemAir.ExchangePreviousExchangeAllowedFlag,
                priceTrackingSetupGroupItemAir.NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToLowerNonRefundableAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToLowerRefundableAllowedFlag,
                priceTrackingSetupGroupItemAir.NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag,
                priceTrackingSetupGroupItemAir.ChargeChangeFeeUpFrontForSpecificCarriersFlag,
                priceTrackingSetupGroupItemAir.ChangeFeeMustBeUsedFromResidualValueFlag,
                priceTrackingSetupGroupItemAir.TrackPrivateNegotiatedFareFlag,
                priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode1,
                priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode2,
                priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode3,
                priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode4,
                priceTrackingSetupGroupItemAir.CompanyProfilesRequiringFIQID,
                priceTrackingSetupGroupItemAir.RealisedSavingsCode,
                priceTrackingSetupGroupItemAir.MissedSavingsCode,
                priceTrackingSetupGroupItemAir.EnabledDate,
                priceTrackingSetupGroupItemAir.DeactivationDate,
                priceTrackingSetupGroupItemAir.ImportPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemAir.ImportQueueInClientPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemAir.PricingPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemAir.SavingsQueueId,
                priceTrackingSetupGroupItemAir.AlphaCodeRemarkField,
                priceTrackingSetupGroupItemAir.AutomaticReticketingFlag,
                adminUserGuid
            );
        }

		//Update in DB
		public void Update(PriceTrackingSetupGroupItemAir priceTrackingSetupGroupItemAir)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePriceTrackingSetupGroupItemAir_v1(
                priceTrackingSetupGroupItemAir.PriceTrackingSetupGroupItemAirId,
                priceTrackingSetupGroupItemAir.PriceTrackingSetupGroupId,
                priceTrackingSetupGroupItemAir.ClientHasProvidedWrittenApprovalFlag,
                priceTrackingSetupGroupItemAir.SharedSavingsFlag,
                priceTrackingSetupGroupItemAir.SharedSavingsAmount,
                priceTrackingSetupGroupItemAir.TransactionFeeFlag,
                priceTrackingSetupGroupItemAir.TransactionFeeAmount,
                priceTrackingSetupGroupItemAir.CentralFulfillmentTimeZoneRuleCode,
                priceTrackingSetupGroupItemAir.CentralFulfillmentBusinessHours,
                priceTrackingSetupGroupItemAir.Comment,
                priceTrackingSetupGroupItemAir.AnnualTransactionCount,
                priceTrackingSetupGroupItemAir.AnnualSpendAmount,
                priceTrackingSetupGroupItemAir.CurrencyCode,
                priceTrackingSetupGroupItemAir.EstimatedCWTRebookingFeeAmount,
                priceTrackingSetupGroupItemAir.CWTVoidRefundFeeAmount,
                priceTrackingSetupGroupItemAir.NoPenaltyVoidWindowThresholdAmount,
                priceTrackingSetupGroupItemAir.OutsideVoidPenaltyThresholdAmount,
                priceTrackingSetupGroupItemAir.TimeZoneRuleCode,
                priceTrackingSetupGroupItemAir.RefundableToRefundableWithPenaltyForRefundAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToRefundablePreDepartureDayAmount,
                priceTrackingSetupGroupItemAir.NoPenaltyRefundableToPenaltyRefundableThresholdAmount,
                priceTrackingSetupGroupItemAir.RefundableToNonRefundableAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToNonRefundablePreDepartureDayAmount,
                priceTrackingSetupGroupItemAir.NoPenaltyRefundableToPenaltyNonRefundableThresholdAmount,
                priceTrackingSetupGroupItemAir.VoidWindowAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToRefundableOutsideVoidWindowAllowedFlag,
                priceTrackingSetupGroupItemAir.NonRefundableToNonRefundableOutsideVoidWindowAllowedFlag,
                priceTrackingSetupGroupItemAir.ExchangesAllowedFlag,
                priceTrackingSetupGroupItemAir.VoidExchangeAllowedFlag,
                priceTrackingSetupGroupItemAir.ExchangePreviousExchangeAllowedFlag,
                priceTrackingSetupGroupItemAir.NonRefundableToLowerNonRefundableWithDifferentChangeFeeAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToLowerNonRefundableAllowedFlag,
                priceTrackingSetupGroupItemAir.RefundableToLowerRefundableAllowedFlag,
                priceTrackingSetupGroupItemAir.NonPenaltyRefundableToLowerPenaltyRefundableAllowedFlag,
                priceTrackingSetupGroupItemAir.ChargeChangeFeeUpFrontForSpecificCarriersFlag,
                priceTrackingSetupGroupItemAir.ChangeFeeMustBeUsedFromResidualValueFlag,
                priceTrackingSetupGroupItemAir.TrackPrivateNegotiatedFareFlag,
                priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode1,
                priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode2,
                priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode3,
                priceTrackingSetupGroupItemAir.NegotiatedPricingTrackingCode4,
                priceTrackingSetupGroupItemAir.CompanyProfilesRequiringFIQID,
                priceTrackingSetupGroupItemAir.RealisedSavingsCode,
                priceTrackingSetupGroupItemAir.MissedSavingsCode,
                priceTrackingSetupGroupItemAir.EnabledDate,
                priceTrackingSetupGroupItemAir.DeactivationDate,
                priceTrackingSetupGroupItemAir.ImportPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemAir.ImportQueueInClientPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemAir.PricingPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemAir.SavingsQueueId,
                priceTrackingSetupGroupItemAir.AlphaCodeRemarkField,
                priceTrackingSetupGroupItemAir.AutomaticReticketingFlag,
                adminUserGuid,
                priceTrackingSetupGroupItemAir.VersionNumber
            );
        }
    }
}
