using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{

    public class CustomValidationException : Exception
    {
        public CustomValidationException() : base() { }

        public CustomValidationException(string message) : base(message) { }

    }

    public class ValidationRepository
    {

        //Where Used: ClientSubUnitCDR
        //19 Aug 2013 - altered for QC2013
        //may need to look at this again, should not need 6 if/else statements. Null values causing problems
        public bool IsValidClientSubUnitCDR(int? clientSubUnitClientDefinedReferenceId, string clientSubUnitGuid, string cdrValue, string canssc, int? creditCardId)
        {
            string clientAccountNumber = "";
            string sourceSystemCode = "";

            if(canssc != ""){
                clientAccountNumber = canssc.Split(new[] { '|' })[0];
                sourceSystemCode = canssc.Split(new[] { '|' })[1];
            }

            HierarchyDC   db = new HierarchyDC(Settings.getConnectionString());

            IQueryable<ClientSubUnitClientDefinedReference> result;

            if (clientSubUnitClientDefinedReferenceId == null){ //add
                    if(creditCardId == null){
                        //Account Only
                         result = from n in db.ClientSubUnitClientDefinedReferences
                             where ((n.ClientSubUnitGuid == clientSubUnitGuid) 
                             && (n.ClientAccountNumber == clientAccountNumber) 
                             && (n.SourceSystemCode == sourceSystemCode) 
                             && (n.CreditCardId.Equals(null)) 
                             && (n.Value == cdrValue))
                             select n;
                    }else{
                        //Credit Card Only
                        if(string.IsNullOrEmpty(clientAccountNumber)){
                            result = from n in db.ClientSubUnitClientDefinedReferences
                                where ((n.ClientSubUnitGuid == clientSubUnitGuid) 
                                && (n.Value == cdrValue) 
                                 && (n.ClientAccountNumber.Equals(null)) 
                                && (n.SourceSystemCode.Equals(null)) 
                                && (n.CreditCardId.Equals(creditCardId)))
                                select n;
                        }else{
                        //Card and Account
                            result = from n in db.ClientSubUnitClientDefinedReferences
                                where ((n.ClientSubUnitGuid == clientSubUnitGuid) 
                                && (n.Value == cdrValue) 
                                && (n.SourceSystemCode == sourceSystemCode) 
                                && (n.ClientAccountNumber == clientAccountNumber) 
                                && (n.CreditCardId.Equals(creditCardId)))
                                select n;
                        }
                    }
              

            }else{
                if(creditCardId == null){
                        //Account Only
                         result = from n in db.ClientSubUnitClientDefinedReferences
                             where ((n.ClientSubUnitGuid == clientSubUnitGuid) 
                             && (n.ClientSubUnitClientDefinedReferenceId != clientSubUnitClientDefinedReferenceId) 
                             && (n.ClientAccountNumber == clientAccountNumber) 
                             && (n.SourceSystemCode == sourceSystemCode) 
                             && (n.CreditCardId.Equals(null)) 
                             && (n.Value == cdrValue))
                             select n;
                    }else{
                        //Credit Card Only
                        if(string.IsNullOrEmpty(clientAccountNumber)){
                            result = from n in db.ClientSubUnitClientDefinedReferences
                                where ((n.ClientSubUnitGuid == clientSubUnitGuid) 
                                && (n.ClientSubUnitClientDefinedReferenceId != clientSubUnitClientDefinedReferenceId) 
                                && (n.Value == cdrValue) 
                                && (n.ClientAccountNumber.Equals(null)) 
                                && (n.SourceSystemCode.Equals(null)) 
                                && (n.CreditCardId.Equals(creditCardId)))
                                select n;
                        }else{
                        //Card and Account
                            result = from n in db.ClientSubUnitClientDefinedReferences
                                where ((n.ClientSubUnitGuid == clientSubUnitGuid) 
                                && (n.ClientSubUnitClientDefinedReferenceId != clientSubUnitClientDefinedReferenceId) 
                                && (n.Value == cdrValue) 
                                && (n.SourceSystemCode == sourceSystemCode) 
                                && (n.ClientAccountNumber == clientAccountNumber) 
                                && (n.CreditCardId.Equals(creditCardId)))
                                select n;
                        }
                    }

            }
            int x = result.Count();

            return (result.Count() == 0);
        }


        //Where Used: Team
        public City IsValidCityCode(string cityCode)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

            return (from n in db.Cities
                          where n.CityCode.Equals(cityCode)
                          select
                             n).FirstOrDefault();
        }

        //Where Used: SupplierProduct
        public List<Supplier> IsValidClientDetailSupplierProduct(int clientDetailId , int productId, string supplierCode)
        {
            SupplierDC db = new SupplierDC(Settings.getConnectionString());

            var result = from n in db.Suppliers
                         where ((n.ProductId == productId) && (n.SupplierCode.Equals(supplierCode)))
                         select
                            n;
            return result.ToList();
        }

		//Where Used: Client-Side Validation of Supplier/Product combos
		public List<Supplier> IsValidSupplierProduct(int productId, string supplierCode)
		{
			SupplierDC db = new SupplierDC(Settings.getConnectionString());
			SupplierRepository supplierRepository = new SupplierRepository();

			var result = from n in db.Suppliers
						 where ((n.ProductId == productId) && (n.SupplierCode.Equals(supplierCode)))
						 select
							n;
			return result.ToList();
		}

		//Where Used: Client-Side Validation of SupplierName/Product combos
		public List<Supplier> IsValidSupplierProductName(int productId, string supplierName)
		{
			SupplierDC db = new SupplierDC(Settings.getConnectionString());
			SupplierRepository supplierRepository = new SupplierRepository();

			var result = from n in db.Suppliers
						 where ((n.ProductId == productId) && (n.SupplierName.Equals(supplierName)))
						 select
							n;
			return result.ToList();
		}

		//Where Used: Client-Side Validation of Supplier Code
		public List<Supplier> IsValidSupplierCode(string supplierCode)
		{
			SupplierDC db = new SupplierDC(Settings.getConnectionString());
			SupplierRepository supplierRepository = new SupplierRepository();

			var result = from n in db.Suppliers
						 where (n.SupplierCode.Equals(supplierCode))
						 select
							n;
			return result.ToList();
		}
		
		//Where Used: Client-Side Validation of Supplier/Product combos
        public List<Supplier> IsValidSupplierName(string supplierName)
        {
            SupplierDC db = new SupplierDC(Settings.getConnectionString());
            SupplierRepository supplierRepository = new SupplierRepository();

            var result = from n in db.Suppliers
                         where (n.SupplierName.Equals(supplierName))
                         select
                            n;
            return result.ToList();
        }

        //Where Used: ClientWizard.Policies
        public Country IsValidCountry(string countryName)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
            CountryRepository supplierRepository = new CountryRepository();

            var result = (from n in db.Countries
                          where n.CountryName.Equals(countryName)
                          select
                             n).FirstOrDefault();
            return result;
        }

        //Where Used: ClientWizard.Policies
        public Country IsValidAdminUserCountry(string countryName)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
            CountryRepository supplierRepository = new CountryRepository();

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
             var result = (from n in db.spDesktopDataAdmin_SelectSystemUserCountry_v1(countryName, adminUserGuid)
                         select
                         new Country
                         {
                             CountryCode = n.CountryCode,
                             CountryName = n.CountryName
                         }).FirstOrDefault();

            //var result = (from n in db.spDesktopDataAdmin_SelectSystemUserCountry_v1(countryName, adminUserGuid)
                          
             //                n).FirstOrDefault();
            return result;
        }

        //Where Used: Location
        //checks if country is valid based on users LocationRole
        public Country IsValidCountryWithLocations(string countryName)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
            HierarchyRepository hierarchyRepository = new HierarchyRepository();
            RolesRepository rolesRepository = new RolesRepository();

            //gives a list of locations for the country
            var locations = from c in db.Countries
                         join cr in db.CountryRegions on c.CountryCode equals cr.CountryCode
                         join loc in db.Locations on cr.CountryRegionId equals loc.CountryRegionId
                         into outer from loc in outer.DefaultIfEmpty()
                            where ((c.CountryName == countryName))
                         select
                            loc;

            List<Country> countries = new List<Country>();
            
            int? locationId;
            foreach (Location loc in locations.ToList()) // Loop through List with foreach
            {
                if (loc == null)
                {
                    locationId = null;
                }
                else
                {
                    locationId = loc.LocationId;
                }
                if (rolesRepository.HasWriteAccessToLocation(locationId))

                {
                    Country country = new Country();
                    country = (from c in db.Countries
                                where c.CountryName == countryName
                                select c).FirstOrDefault();
                    //countries.Add(loc.CountryRegion.Country);

                    return country;   //return after one item added
                }
            }
            return null;   //return empty list
        }

        //Where Used: Location
        //checks if country is valid based on users LocationRole
        public Location IsValidLocation(string locationName)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
            var l = (from n in db.Locations
                             where (n.LocationName == locationName) select n).FirstOrDefault();

            if (l != null)
            {
                Location location = new Location();
                location.LocationId = l.LocationId;

                RolesRepository rolesRepository = new RolesRepository();
                if (rolesRepository.HasWriteAccessToLocation(location.LocationId))
                {
                    return location;   //return after one item added
                }
            }
            return null;   //return empty list
        }

        //Where Used: APISCountry
        //trying to insert this item, must check if already exists
        //if available return true, if already exists return false
        public bool IsValidAPISCountries(string originCountryCode, string destinationCountryCode)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
            List<APISCountry> apisCountries = new List<APISCountry>();

            var result = from n in db.APISCountries
                         where n.OriginCountryCode.Trim().Equals(originCountryCode)
                         && n.DestinationCountryCode.Equals(destinationCountryCode)
                         select n;
            apisCountries = result.ToList();



            if (apisCountries.Count > 0)
            {
                return false;   //already in use
            }
            else
            {
                return true;
            }

        }


        //Where Used: PolicyAirVendorGroupItem
        public List<PolicyRoutingJSON> IsValidPolicyRoutingFromTo(string fromTo, string codeType)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
            var result = from n in db.fnDesktopDataAdmin_SelectPolicyRoutingFromTo_v1()
                         where n.Code.Equals(fromTo) && n.CodeType.Equals(codeType)
                         select new PolicyRoutingJSON
                         {
                             Code = n.Code,
                             Name = n.Name,
                             Parent = n.Parent,
                             CodeType = n.CodeType
                         };
            return result.ToList();
        }

        //Where Used: MerchantFeeClientFeeGroup
        public MerchantFee IsMerchantFee(int merchantFeeId)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
            var result = (from n in db.spDesktopDataAdmin_SelectMerchantFee_v1(merchantFeeId)
                          select
                          new MerchantFee
                          {
                              CountryName = n.CountryName,
                              CreditCardVendorName = n.CreditCardVendorName,
                              MerchantFeePercent = n.MerchantFeePercent,
                              ProductName = n.ProductName,
                              SupplierName = n.SupplierName
                          }).FirstOrDefault();
            return result;
        }

        //Where Used: TransactionFeeClientFeeGroup
        public TransactionFeeCarHotel IsTransactionFeeCarHotel(int transactionFeeId)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
            var result = (from n in db.spDesktopDataAdmin_SelectTransactionFeeCarHotel_v1(transactionFeeId)
                          select
                          new TransactionFeeCarHotel
                          {
                              TransactionFeeId = n.TransactionFeeId,
                              ProductName = n.ProductName,
                              TransactionFeeDescription = n.TransactionFeeDescription,
                              PolicyLocationName = n.PolicyLocationName,
                              TravelIndicatorDescription = n.TravelIndicatorDescription,
                              BookingSourceDescription = n.BookingSourceDescription,
                              BookingOriginationDescription = n.BookingOriginationDescription,
                              ChargeTypeDescription = n.ChargeTypeDescription,
                              TransactionTypeCode = n.TransactionTypeCode,
                              FeeCategory = n.FeeCategory,
                              TravelerClassCode = n.TravelerClassCode,
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
                              FeeCurrencyCode = n.FeeCurrencyCode
                          }).FirstOrDefault();
            return result;
        }

        //Where Used: TransactionFeeClientFeeGroup
        public TransactionFeeVM IsTransactionFeeAir(int transactionFeeId)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

            TransactionFeeAir transactionFeeAir = new TransactionFeeAir();
            PolicyRouting policyRouting = new PolicyRouting();

            TransactionFeeAirRepository transactionFeeAirRepository = new TransactionFeeAirRepository();
            transactionFeeAir = transactionFeeAirRepository.GetItem(transactionFeeId);

            PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
            policyRouting = policyRoutingRepository.GetPolicyRouting((int)transactionFeeAir.PolicyRoutingId);
            policyRoutingRepository.EditPolicyRouting(policyRouting);

            TransactionFeeVM transactionFeeVM = new TransactionFeeVM();
            transactionFeeVM.TransactionFee = transactionFeeAir;
            transactionFeeVM.Name = policyRouting.Name;
            transactionFeeVM.FromGlobalFlag = policyRouting.FromGlobalFlag;
            transactionFeeVM.FromCode = policyRouting.FromCode;
            transactionFeeVM.ToGlobalFlag = policyRouting.ToGlobalFlag;
            transactionFeeVM.ToCode  = policyRouting.ToCode;
            transactionFeeVM.RoutingViceVersaFlag = policyRouting.RoutingViceVersaFlag;
            return transactionFeeVM;

           /* var result = (from n in db.spDesktopDataAdmin_SelectTransactionFeeAir_v1(transactionFeeId)
                          select
                         new TransactionFeeAir
                              {
                                  TransactionFeeId = n.TransactionFeeId,
                                  ProductName = n.ProductName,
                                  TransactionFeeDescription = n.TransactionFeeDescription,
                                  TravelIndicatorDescription = n.TravelIndicatorDescription,
                                  BookingSourceDescription = n.BookingSourceDescription,
                                  BookingOriginationDescription = n.BookingOriginationDescription,
                                  ChargeTypeDescription = n.ChargeTypeDescription,
                                  TransactionTypeCode = n.TransactionTypeCode,
                                  FeeCategory = n.FeeCategory,
                                  TravelerClassCode = n.TravelerClassCode,
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
                                  PolicyRoutingId = n.PolicyRoutingId
                              }).FirstOrDefault();
            return result;*/
        }

		//Where Used: SystemUserDefineRoles
		public SystemUser IsValidSystemUser(string id)
		{
			SystemUserDC db = new SystemUserDC(Settings.getConnectionString());

			return (from n in db.SystemUsers
					where n.UserProfileIdentifier.Equals(id)
					select
					   n).FirstOrDefault();
		}

		/// <summary>
		/// Validate GDS Command Format against the database
		/// </summary>
		/// <param name="GDSCommandFormat"></param>
		/// <param name="GDSCode"></param>
		/// <returns></returns>
		public string IsValidGDSCommandFormat(string GDSCommandFormat, string GDSCode)
		{
			HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
			
			string TID = null;
			
			switch (GDSCode)
			{
				case "1G":
				case "1V":
					TID = (from n in db.ClientProfileTravelportGDSFormats
						   where n.DefaultGDSCommandFormat == GDSCommandFormat && n.GDSCode == GDSCode
						   select n.TravelIndustryDesignatorValue).FirstOrDefault();
					break;
				default:
					return "true"; //Don't validate yet
			}
			
			return (TID != null) ? "true" : "false";
		}

        /*
         *https://support.travelport.com/NR/rdonlyres/CC80873C-B071-48CA-8CD9-6834281DB367/0/GalileoDesktopSpecial_Characters.pdf 
         * @.>|\/:L¤#[]
         * 
         * http://sds.sabre.com/XTRANET_Access/sabre.htm
         * /-,^¤<‡$.>?†:*
         * 
         * 
         * 
         */
        public string IsValidClientProfileRowItem(ClientProfileItemRow clientProfileItemRow)
        {
            if(string.IsNullOrEmpty(clientProfileItemRow.Remark))
                clientProfileItemRow.Remark = "";

            if (clientProfileItemRow.GDSCode == "1A")   //Amadeus
            {
                if (clientProfileItemRow.GDSCommandFormat == "RM*ACC")
                {
                    if (clientProfileItemRow.Remark.Length < 1 || clientProfileItemRow.Remark.Length > 10)
                    {
                        return "Remark must be between 1 and 10 characters";
                    }
                }
                if (clientProfileItemRow.GDSCommandFormat == "PCN/")
                {
                    if (clientProfileItemRow.Remark.Length < 3 || clientProfileItemRow.Remark.Length > 64)
                    {
                        return "Remark must be between 3 and 64 characters";
                    }
                }
                if (clientProfileItemRow.GDSCommandFormat == "TKOK" || clientProfileItemRow.GDSCommandFormat == "TKTL")
                {
                    if (clientProfileItemRow.Remark.Length < 2 || clientProfileItemRow.Remark.Length > 19)
                    {
                        return "Remark must be between 2 and 19 characters";
                    }
                }
                if (clientProfileItemRow.GDSCommandFormat == "AP" || clientProfileItemRow.GDSCommandFormat == "APF" || clientProfileItemRow.GDSCommandFormat == "APE")
                {
                    if (clientProfileItemRow.Remark.Length < 1 || clientProfileItemRow.Remark.Length > 90)
                    {
                        return "Remark must be between 1 and 90 characters";
                    }
                }
                if (clientProfileItemRow.GDSCommandFormat == "RC" || clientProfileItemRow.GDSCommandFormat == "RM" || clientProfileItemRow.GDSCommandFormat == "RQ")
                {
                    if (clientProfileItemRow.Remark.Length < 1 || clientProfileItemRow.Remark.Length > 124)
                    {
                        return "Remark must be between 1 and 124 characters";
                    }
                }
            }
            if (clientProfileItemRow.GDSCode == "1G")   //Galileo
            {

                if (clientProfileItemRow.GDSCommandFormat == "D.")
                {
                    if (clientProfileItemRow.Remark.Length > 119)
                    {
                        return "Remark cannot be longer than 119 characters";
                    }

                    string pattern2 = @"(\*([\w\s\-()\u2628\u00A4]){1,37}){1,6}";
                    Match match2 = Regex.Match(clientProfileItemRow.Remark, pattern2, RegexOptions.IgnoreCase);
                    if (!match2.Success)
                    {
                        return "Remark is not a valid format";
                    }
                }


                else if (clientProfileItemRow.GDSCommandFormat == "W.")
                {
                    if (clientProfileItemRow.Remark.Length > 119)
                    {
                        return "Remark cannot be longer than 119 characters";
                    }

                    string pattern1 = @"\*P/";
                    Match match1 = Regex.Match(clientProfileItemRow.Remark, pattern1, RegexOptions.IgnoreCase);
                    if (!match1.Success)
                    {
                        return "Remark must contain a PostCode";
                    }
                    string pattern2 = @"(\*([\w\s\-()\u2628\u00A4]){1,37}){1,5}";
                    Match match2 = Regex.Match(clientProfileItemRow.Remark, pattern2, RegexOptions.IgnoreCase);
                    if (!match2.Success)
                    {
                        return "Remark is not a valid format";
                    }
                }
                else if (clientProfileItemRow.GDSCommandFormat == "RA.")
                {

                    string pattern1 = @"\*P/";
                    Match match1 = Regex.Match(clientProfileItemRow.Remark, pattern1, RegexOptions.IgnoreCase);
                    if (!match1.Success)
                    {
                        return "Remark must contain a PostCode";
                    }
                    string pattern2 = @"(\*([\w\s\-()\u2628\u00A4]){1,37}){1,5}";
                    Match match2 = Regex.Match(clientProfileItemRow.Remark, pattern2, RegexOptions.IgnoreCase);
                    if (!match2.Success)
                    {
                        return "Remark is not a valid format";
                    }
                }

                else if (clientProfileItemRow.Remark.Length > 58)
                {
                    return "Remark cannot be longer than 58 characters";
                }
            }
            if (clientProfileItemRow.GDSCode == "1S")   //Sabre
            {
                if (clientProfileItemRow.Remark.Length > 64)
                {
                    return "Remark cannot be longer than 64 characters";
                }
            }
            if (clientProfileItemRow.GDSCode == "1V")   //Apollo
            {
                if (clientProfileItemRow.GDSCommandFormat == "W-" || clientProfileItemRow.GDSCommandFormat == "D-")
                {
                    if (clientProfileItemRow.ClientProfileMoveStatusId == 2) //Never Move
                    {
                        if (clientProfileItemRow.Remark.Length > 244)
                        {
                            return "Remark cannot be longer than 244 characters";
                        }
                    }
                    else
                    {
                        if (clientProfileItemRow.Remark.Length > 120)
                        {
                            return "Remark cannot be longer than 120 characters";
                        }
                    }
                }
                else
                {
                    if (clientProfileItemRow.Remark.Length > 58)
                    {
                        return "Remark cannot be longer than 58 characters";
                    }
                }
            }
            return "OK";
        }

        //Where Used: Team
        public TravelPort IsValidTravelPortCode(string travelPortCode)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

            return (from n in db.TravelPorts
                    where n.TravelPortCode.Equals(travelPortCode)
                    select
                       n).FirstOrDefault();
        }

        //Where Used: Third Party User
        public bool IsValidPartner(int? partnerId, string partnerName)
        {
            HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

            var result = (from n in db.Partners
                          where n.PartnerName.Equals(partnerName) && n.PartnerId == partnerId
                          select
                             n).FirstOrDefault();

            if (result != null)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}