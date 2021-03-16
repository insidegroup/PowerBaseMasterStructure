using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class TransactionFeeCarHotelVM : CWTBaseViewModel
    {
        public TransactionFeeCarHotel TransactionFee { get; set; }
        public IEnumerable<SelectListItem> TravelIndicators { get; set; }
        public IEnumerable<SelectListItem> PolicyLocations { get; set; }
        public IEnumerable<SelectListItem> BookingSources { get; set; }
        public IEnumerable<SelectListItem> BookingOriginations { get; set; }
        public IEnumerable<SelectListItem> ChargeTypes{ get; set; }
        public IEnumerable<SelectListItem> TransactionTypes { get; set; }
        public IEnumerable<SelectListItem> FeeCategories { get; set; }
        public IEnumerable<SelectListItem> TravelerBackOfficeTypes { get; set; }
        public IEnumerable<SelectListItem> TripTypeClassifications { get; set; }
        public IEnumerable<SelectListItem> TicketPriceCurrencies { get; set; }
        public IEnumerable<SelectListItem> FeeCurrencies { get; set; }

        public TransactionFeeCarHotelVM()
        {
        }
        public TransactionFeeCarHotelVM(TransactionFeeCarHotel transactionFee,
            IEnumerable<SelectListItem> travelIndicators,
            IEnumerable<SelectListItem> policyLocations,
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
            TravelIndicators = travelIndicators;
            PolicyLocations = policyLocations;
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