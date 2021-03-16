using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class TransactionFeeAirRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get one Item
        public TransactionFeeAir GetItem(int transactionFeeId)
        {
            var result = (from n in db.spDesktopDataAdmin_SelectTransactionFeeAir_v1(transactionFeeId)
                    select new
                        TransactionFeeAir
                    {
                        TransactionFeeId = n.TransactionFeeId,
                        TransactionFeeDescription = n.TransactionFeeDescription,
                        TravelIndicator = n.TravelIndicator,
                        TravelIndicatorDescription = n.TravelIndicatorDescription,
                        BookingSourceCode = n.BookingSourceCode,
                        BookingSourceDescription = n.BookingSourceDescription,
                        BookingOriginationCode = n.BookingOriginationCode,
                        BookingOriginationDescription = n.BookingOriginationDescription,
                        ChargeTypeCode = n.ChargeTypeCode,
                        ChargeTypeDescription = n.ChargeTypeDescription,
                        TransactionTypeCode = n.TransactionTypeCode,
                        FeeCategory = n.FeeCategory,
                        TravelerClassCode = n.TravelerClassCode,
                        ProductId = n.ProductId,
                        ProductName = n.ProductName,
                        SupplierCode = n.SupplierCode,
                        SupplierName = n.SupplierName,
                        MinimumFeeCategoryQuantity = n.MinimumFeeCategoryQuantity,
                        MaximumFeeCategoryQuantity = n.MaximumFeeCategoryQuantity,
                        MinimumTicketPrice = n.MinimumTicketPrice,
                        MaximumTicketPrice = n.MaximumTicketPrice,
                        TicketPriceCurrencyCode = n.TicketPriceCurrencyCode,
                        IncursGSTFlag = n.IncursGSTFlag,
                        TripTypeClassificationDescription = n.TripTypeClassificationDescription,
                        EnabledDate = n.EnabledDate,
                        ExpiryDate = n.ExpiryDate,
                        FeeAmount = n.FeeAmount,
                        FeePercent = n.FeePercent,
                        FeeCurrencyCode = n.FeeCurrencyCode,
                        VersionNumber = n.VersionNumber,
                        PolicyRoutingId = n.PolicyRoutingId
                    }).FirstOrDefault();

            return result;
        }


        public List<TransactionFeeAir> GetAllTransactionFeeAirs()
        {
            return  (from n in db.TransactionFees
                    where n.ProductId == 1
                     select new TransactionFeeAir
                    {
                        TransactionFeeId = n.TransactionFeeId,
                        TransactionFeeDescription = n.TransactionFeeDescription
                    }).OrderBy(c => c.TransactionFeeDescription).ToList();

            
           // var result = (from n in db.TransactionFees
            //return db.TransactionFees.OrderBy(c => c.TransactionFeeDescription).Where(c => c.ProductId == 1);
        }


        public void EditForDisplay(TransactionFeeAir transactionFee)
        {
            TravelIndicatorRepository travelIndicatorRepository = new TravelIndicatorRepository();
            TravelIndicator travelIndicator = new TravelIndicator();
            travelIndicator = travelIndicatorRepository.GetTravelIndicator(transactionFee.TravelIndicator);
            if (travelIndicator != null)
            {
                transactionFee.TravelIndicatorDescription = travelIndicator.TravelIndicatorDescription;
            }

            CurrencyRepository currencyRepository = new CurrencyRepository();
            Currency currency = new Currency();
            currency = currencyRepository.GetCurrency(transactionFee.FeeCurrencyCode);
            if (currency != null)
            {
                transactionFee.FeeCurrencyName = currency.Name;
            }
            currency = currencyRepository.GetCurrency(transactionFee.TicketPriceCurrencyCode);
            if (currency != null)
            {
                transactionFee.TicketPriceCurrencyName = currency.Name;
            }


            BookingSourceRepository bookingSourceRepository = new BookingSourceRepository();
            BookingSource bookingSource = new BookingSource();
            bookingSource = bookingSourceRepository.GetBookingSource(transactionFee.BookingSourceCode);
            if (bookingSource != null)
            {
                transactionFee.BookingSourceDescription = bookingSource.BookingSourceDescription;
            }

            BookingOriginationRepository bookingOriginationRepository = new BookingOriginationRepository();
            BookingOrigination bookingOrigination = new BookingOrigination();
            bookingOrigination = bookingOriginationRepository.GetBookingOrigination(transactionFee.BookingOriginationCode);
            if (bookingOrigination != null)
            {
                transactionFee.BookingOriginationCode = bookingOrigination.BookingOriginationCode;
            }

            ChargeTypeRepository chargeTypeRepository = new ChargeTypeRepository();
            ChargeType chargeType = new ChargeType();
            chargeType = chargeTypeRepository.GetChargeType(transactionFee.ChargeTypeCode);
            if (bookingOrigination != null)
            {
                transactionFee.ChargeTypeDescription = chargeType.ChargeTypeDescription;
            }

            TravelerBackOfficeTypeRepository travelerBackOfficeTypeRepository = new TravelerBackOfficeTypeRepository();
            TravelerBackOfficeType travelerBackOfficeType = new TravelerBackOfficeType();
            travelerBackOfficeType = travelerBackOfficeTypeRepository.GetTravelerBackOfficeType(transactionFee.TravelerClassCode);
            if (travelerBackOfficeType != null)
            {
                transactionFee.TravelerBackOfficeTypeDescription = travelerBackOfficeType.TravelerBackOfficeTypeDescription;
            }

            if (transactionFee.ProductId != null)
            {
                ProductRepository productRepository = new ProductRepository();
                Product product = new Product();
                product = productRepository.GetProduct((int)transactionFee.ProductId);
                if (product != null)
                {
                    //Supplier
                    if (!String.IsNullOrEmpty(transactionFee.SupplierCode))
                    {
                        SupplierRepository supplierRepository = new SupplierRepository();
                        Supplier supplier = new Supplier();
                        supplier = supplierRepository.GetSupplier(transactionFee.SupplierCode, (int)transactionFee.ProductId);
                        if (supplier != null)
                        {
                            transactionFee.SupplierName = supplier.SupplierName;
                        }
                    }
                }
            }

            if (transactionFee.PolicyLocationId != null)
            {
                PolicyLocationRepository policyLocationRepository = new PolicyLocationRepository();
                PolicyLocation policyLocation = new PolicyLocation();
                policyLocation = policyLocationRepository.GetPolicyLocation((int)transactionFee.PolicyLocationId);
                if (policyLocation != null)
                {
                    transactionFee.PolicyLocationName = policyLocation.PolicyLocationName;
                }
            }

            //IncursGSTFlag is nullable
            if (transactionFee.IncursGSTFlag != true)
            {
                transactionFee.IncursGSTFlag = false;
            }
            transactionFee.IncursGSTFlagNonNullable = (bool)transactionFee.IncursGSTFlag;

        }
        
        //Add to DB
        public void Add(TransactionFeeAir transactionFee, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTransactionFeeAir_v1(
                transactionFee.TransactionFeeDescription,
                transactionFee.TravelIndicator,
                transactionFee.BookingSourceCode,
                transactionFee.BookingOriginationCode,
                transactionFee.ChargeTypeCode,
                transactionFee.TransactionTypeCode,
                transactionFee.FeeCategory,
                transactionFee.TravelerClassCode,
                transactionFee.ProductId,
                transactionFee.SupplierCode,
                transactionFee.MinimumFeeCategoryQuantity,
                transactionFee.MaximumFeeCategoryQuantity,
                transactionFee.MinimumTicketPrice,
                transactionFee.MaximumTicketPrice,
                transactionFee.TicketPriceCurrencyCode,
                transactionFee.TripTypeClassificationId,
                transactionFee.TemplateFeeFlag,
                transactionFee.EnabledDate,
                transactionFee.ExpiryDate,
                transactionFee.IncursGSTFlagNonNullable,
                transactionFee.FeeAmount,
                transactionFee.FeeCurrencyCode,
                transactionFee.FeePercent,
                policyRouting.Name,
                policyRouting.FromGlobalFlag,
                policyRouting.FromGlobalRegionCode,
                policyRouting.FromGlobalSubRegionCode,
                policyRouting.FromCountryCode,
                policyRouting.FromCityCode,
                policyRouting.FromTravelPortCode,
                policyRouting.FromTraverlPortTypeId,
                policyRouting.ToGlobalFlag,
                policyRouting.ToGlobalRegionCode,
                policyRouting.ToGlobalSubRegionCode,
                policyRouting.ToCountryCode,
                policyRouting.ToCityCode,
                policyRouting.ToTravelPortCode,
                policyRouting.ToTravelPortTypeId,
                policyRouting.RoutingViceVersaFlag,
                adminUserGuid
            );
        }

        //Add to DB
        public void Update(TransactionFeeAir transactionFee, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTransactionFeeAir_v1(
                transactionFee.TransactionFeeId,
                transactionFee.TransactionFeeDescription,
                transactionFee.TravelIndicator,
                transactionFee.BookingSourceCode,
                transactionFee.BookingOriginationCode,
                transactionFee.ChargeTypeCode,
                transactionFee.TransactionTypeCode,
                transactionFee.FeeCategory,
                transactionFee.TravelerClassCode,
                transactionFee.SupplierCode,
                transactionFee.MinimumFeeCategoryQuantity,
                transactionFee.MaximumFeeCategoryQuantity,
                transactionFee.MinimumTicketPrice,
                transactionFee.MaximumTicketPrice,
                transactionFee.TicketPriceCurrencyCode,
                transactionFee.TripTypeClassificationId,
                transactionFee.TemplateFeeFlag,
                transactionFee.EnabledDate,
                transactionFee.ExpiryDate,
                transactionFee.IncursGSTFlagNonNullable,
                transactionFee.FeeAmount,
                transactionFee.FeeCurrencyCode,
                transactionFee.FeePercent,
                policyRouting.Name,
                policyRouting.FromGlobalFlag,
                policyRouting.FromGlobalRegionCode,
                policyRouting.FromGlobalSubRegionCode,
                policyRouting.FromCountryCode,
                policyRouting.FromCityCode,
                policyRouting.FromTravelPortCode,
                policyRouting.FromTraverlPortTypeId,
                policyRouting.ToGlobalFlag,
                policyRouting.ToGlobalRegionCode,
                policyRouting.ToGlobalSubRegionCode,
                policyRouting.ToCountryCode,
                policyRouting.ToCityCode,
                policyRouting.ToTravelPortCode,
                policyRouting.ToTravelPortTypeId,
                policyRouting.RoutingViceVersaFlag,
                adminUserGuid,
                transactionFee.VersionNumber
            );
        }

        //Delete From DB
        public void Delete(TransactionFeeAir transactionFee)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteTransactionFeeAir_v1(
               transactionFee.TransactionFeeId,
               adminUserGuid,
               transactionFee.VersionNumber
           );
        }
    }
}