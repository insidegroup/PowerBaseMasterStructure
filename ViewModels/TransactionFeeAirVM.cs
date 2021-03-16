using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TransactionFeeAirVM : CWTBaseViewModel
    {
        public TransactionFeeAir TransactionFee { get; set; }
        public PolicyRouting PolicyRouting { get; set; }
        public IEnumerable<SelectListItem> TravelIndicators { get; set; }
        public IEnumerable<SelectListItem> BookingSources { get; set; }
        public IEnumerable<SelectListItem> BookingOriginations { get; set; }
        public IEnumerable<SelectListItem> ChargeTypes{ get; set; }
        public IEnumerable<SelectListItem> TransactionTypes { get; set; }
        public IEnumerable<SelectListItem> FeeCategories { get; set; }
        public IEnumerable<SelectListItem> TravelerBackOfficeTypes { get; set; }
        public IEnumerable<SelectListItem> TripTypeClassifications { get; set; }
        public IEnumerable<SelectListItem> TicketPriceCurrencies { get; set; }
        public IEnumerable<SelectListItem> FeeCurrencies { get; set; }

        public TransactionFeeAirVM()
        {
        }
        public TransactionFeeAirVM(TransactionFeeAir transactionFee, PolicyRouting policyRouting, 
            IEnumerable<SelectListItem> travelIndicators, 
            IEnumerable<SelectListItem> bookingSources, 
            IEnumerable<SelectListItem> bookingOriginations,
            IEnumerable<SelectListItem> chargeTypes,
            IEnumerable<SelectListItem> transactionTypes,
            IEnumerable<SelectListItem> feeCategories,
            IEnumerable<SelectListItem> travelerBackOfficeTypes,
            IEnumerable<SelectListItem> tripTypeClassifications,
            IEnumerable<SelectListItem> ticketPriceCurrencies,
            IEnumerable<SelectListItem> feeCurrencies
            )
        {
            TransactionFee = transactionFee;
            PolicyRouting = policyRouting;
            TravelIndicators = travelIndicators;
            BookingSources = bookingSources;
            ChargeTypes = chargeTypes;
            BookingOriginations = bookingOriginations;
            TransactionTypes = transactionTypes;
            FeeCategories = feeCategories;
            TravelerBackOfficeTypes = travelerBackOfficeTypes;
            TripTypeClassifications = tripTypeClassifications;
            TicketPriceCurrencies = ticketPriceCurrencies;
            FeeCurrencies = feeCurrencies;
        }
    }
}