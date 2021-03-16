using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class FareRestrictionRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of FareRestrictions - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectFareRestrictions_v1Result> PageFareRestrictions(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectFareRestrictions_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectFareRestrictions_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get One FareRestriction
        public FareRestriction GetFareRestriction(int id){
        
            return db.FareRestrictions.SingleOrDefault(c => c.FareRestrictionId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(FareRestriction fareRestriction)
        {
            //Add LanguageName
            if (fareRestriction.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(fareRestriction.LanguageCode);
                if (language != null)
                {
                    fareRestriction.LanguageName = language.LanguageName;
                }
            }

            ProductRepository productRepository = new ProductRepository();
            Product product = new Product();
            product = productRepository.GetProduct(fareRestriction.ProductId);
            if (product != null)
            {
                fareRestriction.ProductName = product.ProductName;
            }

            SupplierRepository supplierRepository = new SupplierRepository();
            Supplier supplier = new Supplier();
            supplier = supplierRepository.GetSupplier(fareRestriction.SupplierCode,fareRestriction.ProductId);
            if (supplier != null)
            {
                fareRestriction.SupplierName = supplier.SupplierName;
            }

        }
        
        //Add to DB
        public void Add(FareRestriction fareRestriction, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int? fareRestrictionId = new Int32();

            db.spDesktopDataAdmin_InsertFareRestriction_v1(
                ref fareRestrictionId,
                fareRestriction.Changes,
                fareRestriction.Cancellations,
                fareRestriction.ReRoute,
                fareRestriction.ValidOn,
                fareRestriction.MinimumStay,
                fareRestriction.MaximumStay,
                fareRestriction.FareBasis,
                fareRestriction.BookingClass, 
                fareRestriction.SupplierCode,
                fareRestriction.ProductId,
                fareRestriction.LanguageCode,
                fareRestriction.DefaultChecked,
                policyRouting.Name,
                policyRouting.FromGlobalFlag,
                policyRouting.FromGlobalRegionCode,
                policyRouting.FromGlobalSubRegionCode,
                policyRouting.FromCountryCode,
                policyRouting.FromCityCode,
                policyRouting.FromTravelPortCode,
                policyRouting.FromTraverlPortTypeId,
                policyRouting.ToGlobalFlag  ,
                policyRouting.ToGlobalRegionCode,
                policyRouting.ToGlobalSubRegionCode,
                policyRouting.ToCountryCode,
                policyRouting.ToCityCode,
                policyRouting.ToTravelPortCode,
                policyRouting.ToTravelPortTypeId,
                policyRouting.RoutingViceVersaFlag,
                adminUserGuid
            );

            fareRestriction.FareRestrictionId = (int)fareRestrictionId;

        }
        
        //Update in DB
        public void Update(FareRestriction fareRestriction, PolicyRouting policyRouting)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

          


            db.spDesktopDataAdmin_UpdateFareRestriction_v1(
                fareRestriction.FareRestrictionId,
                fareRestriction.Changes,
                fareRestriction.Cancellations,
                fareRestriction.ReRoute,
                fareRestriction.ValidOn,
                fareRestriction.MinimumStay,
                fareRestriction.MaximumStay,
                fareRestriction.FareBasis,
                fareRestriction.BookingClass,
                fareRestriction.SupplierCode,
                fareRestriction.ProductId,
                fareRestriction.LanguageCode,
                fareRestriction.DefaultChecked,
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
                fareRestriction.VersionNumber
            );

        }
        
        //Delete From DB
        public void Delete(FareRestriction fareRestriction)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteFareRestriction_v1(
                fareRestriction.FareRestrictionId,
                adminUserGuid,
                fareRestriction.VersionNumber
            );
        }

    }
}

