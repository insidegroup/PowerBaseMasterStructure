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
	public class PriceTrackingSetupGroupItemHotelRepository
    {
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

        //Get one PriceTrackingSetupGroupItemHotel
        public PriceTrackingSetupGroupItemHotel GetPriceTrackingSetupGroupItemHotel(int id)
        {
            return db.PriceTrackingSetupGroupItemHotels.SingleOrDefault(c => c.PriceTrackingSetupGroupItemHotelId == id);
        }

        //Get one PriceTrackingSetupGroupItemHotel
        public PriceTrackingSetupGroupItemHotel GetPriceTrackingSetupGroupItemHotelByGroupId(int id)
        {
            return db.PriceTrackingSetupGroupItemHotels.SingleOrDefault(c => c.PriceTrackingSetupGroupId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(PriceTrackingSetupGroupItemHotel group)
        {
            //CalculatedTotalThresholdAmount
			group.CalculatedTotalThresholdAmount = 0;

			//EstimatedCWTRebookingFeeAmount
			if (group.EstimatedCWTRebookingFeeAmount.HasValue)
			{
				group.CalculatedTotalThresholdAmount = group.CalculatedTotalThresholdAmount + group.EstimatedCWTRebookingFeeAmount.Value;
			}

			//CWTVoidRefundFeeAmount
			if (group.CWTVoidRefundFeeAmount.HasValue)
			{
				group.CalculatedTotalThresholdAmount = group.CalculatedTotalThresholdAmount + group.CWTVoidRefundFeeAmount.Value;
			}

			//ThresholdAmount
			if (group.EstimatedCWTRebookingFeeAmount.HasValue)
			{
				group.CalculatedTotalThresholdAmount = group.CalculatedTotalThresholdAmount + ((group.ThresholdAmount.HasValue) ? group.ThresholdAmount.Value : 0);
			}
        }

        //Add to DB
        public void Add(PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            XElement priceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML = GetPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML(priceTrackingSetupGroupItemHotel);

            db.spDesktopDataAdmin_InsertPriceTrackingSetupGroupItemHotel_v1(
                priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupId,
                priceTrackingSetupGroupItemHotel.SharedSavingsFlag,
                priceTrackingSetupGroupItemHotel.SharedSavingsAmount,
                priceTrackingSetupGroupItemHotel.TransactionFeeFlag,
                priceTrackingSetupGroupItemHotel.TransactionFeeAmount,
                priceTrackingSetupGroupItemHotel.ClientHasProvidedWrittenApprovalFlag,
                priceTrackingSetupGroupItemHotel.ClientHasHotelFeesInMSAFlag,
                priceTrackingSetupGroupItemHotel.ClientUsesConfermaVirtualCardsFlag,
                priceTrackingSetupGroupItemHotel.CentralFulfillmentTimeZoneRuleCode,
                priceTrackingSetupGroupItemHotel.CentralFulfillmentBusinessHours,
                priceTrackingSetupGroupItemHotel.Comment,
                priceTrackingSetupGroupItemHotel.AnnualTransactionCount,
                priceTrackingSetupGroupItemHotel.AnnualSpendAmount,
                priceTrackingSetupGroupItemHotel.CurrencyCode,
                priceTrackingSetupGroupItemHotel.EstimatedCWTRebookingFeeAmount,
                priceTrackingSetupGroupItemHotel.CWTVoidRefundFeeAmount,
                priceTrackingSetupGroupItemHotel.ThresholdAmount,
                priceTrackingSetupGroupItemHotel.BreakfastValue,
                priceTrackingSetupGroupItemHotel.RoomTypeUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.BeddingTypeUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.HotelRateCodeUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.CancellationPolicyUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.KingQueenUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode1,
                priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode2,
                priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode3,
                priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode4,
                priceTrackingSetupGroupItemHotel.CWTRateTrackingCode1,
                priceTrackingSetupGroupItemHotel.CWTRateTrackingCode2,
                priceTrackingSetupGroupItemHotel.EnabledDate,
                priceTrackingSetupGroupItemHotel.DeactivationDate,
                priceTrackingSetupGroupItemHotel.BreakfastChangesAllowedFlag,
                priceTrackingSetupGroupItemHotel.ParkingValue,
                priceTrackingSetupGroupItemHotel.EnableValueTrackingFlag,
                priceTrackingSetupGroupItemHotel.InternetAccessValue,
                priceTrackingSetupGroupItemHotel.TMCFeeThreshold,
                priceTrackingSetupGroupItemHotel.ImportPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemHotel.ImportQueueInClientPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemHotel.PricingPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemHotel.SavingsQueueId,
                priceTrackingSetupGroupItemHotel.AlphaCodeRemarkField,
                priceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML,
                adminUserGuid
            );
        }

		//Update in DB
		public void Update(PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            XElement priceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML = GetPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML(priceTrackingSetupGroupItemHotel);

            db.spDesktopDataAdmin_UpdatePriceTrackingSetupGroupItemHotel_v1(
                priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupItemHotelId,
                priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupId,
                priceTrackingSetupGroupItemHotel.SharedSavingsFlag,
                priceTrackingSetupGroupItemHotel.SharedSavingsAmount,
                priceTrackingSetupGroupItemHotel.TransactionFeeFlag,
                priceTrackingSetupGroupItemHotel.TransactionFeeAmount,
                priceTrackingSetupGroupItemHotel.ClientHasProvidedWrittenApprovalFlag,
                priceTrackingSetupGroupItemHotel.ClientHasHotelFeesInMSAFlag,
                priceTrackingSetupGroupItemHotel.ClientUsesConfermaVirtualCardsFlag,
                priceTrackingSetupGroupItemHotel.CentralFulfillmentTimeZoneRuleCode,
                priceTrackingSetupGroupItemHotel.CentralFulfillmentBusinessHours,
                priceTrackingSetupGroupItemHotel.Comment,
                priceTrackingSetupGroupItemHotel.AnnualTransactionCount,
                priceTrackingSetupGroupItemHotel.AnnualSpendAmount,
                priceTrackingSetupGroupItemHotel.CurrencyCode,
                priceTrackingSetupGroupItemHotel.EstimatedCWTRebookingFeeAmount,
                priceTrackingSetupGroupItemHotel.CWTVoidRefundFeeAmount,
                priceTrackingSetupGroupItemHotel.ThresholdAmount,
                priceTrackingSetupGroupItemHotel.BreakfastValue,
                priceTrackingSetupGroupItemHotel.RoomTypeUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.BeddingTypeUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.HotelRateCodeUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.CancellationPolicyUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.KingQueenUpgradeAllowedFlag,
                priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode1,
                priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode2,
                priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode3,
                priceTrackingSetupGroupItemHotel.CorporateRateTrackingCode4,
                priceTrackingSetupGroupItemHotel.CWTRateTrackingCode1,
                priceTrackingSetupGroupItemHotel.CWTRateTrackingCode2,
                priceTrackingSetupGroupItemHotel.EnabledDate,
                priceTrackingSetupGroupItemHotel.DeactivationDate,
                priceTrackingSetupGroupItemHotel.BreakfastChangesAllowedFlag,
                priceTrackingSetupGroupItemHotel.ParkingValue,
                priceTrackingSetupGroupItemHotel.EnableValueTrackingFlag,
                priceTrackingSetupGroupItemHotel.InternetAccessValue,
                priceTrackingSetupGroupItemHotel.TMCFeeThreshold,
                priceTrackingSetupGroupItemHotel.ImportPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemHotel.ImportQueueInClientPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemHotel.PricingPseudoCityOrOfficeId,
                priceTrackingSetupGroupItemHotel.SavingsQueueId,
                priceTrackingSetupGroupItemHotel.AlphaCodeRemarkField,
                priceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML,
                adminUserGuid,
                priceTrackingSetupGroupItemHotel.VersionNumber
            );
        }

        //PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress
        public XElement GetPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML(PriceTrackingSetupGroupItemHotel priceTrackingSetupGroupItemHotel)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddresses");
            doc.AppendChild(root);

            if (priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML != null)
            {
                foreach (PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress priceTrackingSetupGroupAdditionalEmailAddress in priceTrackingSetupGroupItemHotel.PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddressesXML)
                {
                    XmlElement xmlPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress = doc.CreateElement("PriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress");

                    if (!string.IsNullOrEmpty(priceTrackingSetupGroupAdditionalEmailAddress.EmailAddress))
                    {
                        //EmailAddress
                        XmlElement xmlEmailAddress = doc.CreateElement("EmailAddress");
                        xmlEmailAddress.InnerText = priceTrackingSetupGroupAdditionalEmailAddress.EmailAddress.ToString();
                        xmlPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress.AppendChild(xmlEmailAddress);

                        root.AppendChild(xmlPriceTrackingSetupGroupItemHotelTrackingAlertEmailAddress);
                    }
                }
            }

            return System.Xml.Linq.XElement.Parse(doc.OuterXml);
        }


    }
}
