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
    public class AirlineClassCabinRepository{
    private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

    //Get a Page of ContactTypes - for Page Listings
    public CWTPaginatedList<spDesktopDataAdmin_SelectAirlineClassCabins_v1Result> PageAirlineClassCabins(int page, string filter, string sortField, int sortOrder)
    {
        //get a page of records
        var result = db.spDesktopDataAdmin_SelectAirlineClassCabins_v1(filter, sortField, sortOrder, page).ToList();

        //total records for paging
        int totalRecords = 0;
        if (result.Count() > 0)
        {
            totalRecords = (int)result.First().RecordCount;
        }

        //put into page object
        var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectAirlineClassCabins_v1Result>(result, page, totalRecords);

        //return to user
        return paginatedView;
    }
        
    //Get One Item
    public AirlineClassCabin GetAirlineClassCabin(string airlineClassCode, string supplierCode, int policyRoutingId )
    {
        return db.AirlineClassCabins.SingleOrDefault(c => (c.AirlineClassCode == airlineClassCode &&
            c.SupplierCode == supplierCode && c.PolicyRoutingId == policyRoutingId));
    }

    //Add Data From Linked Tables for Display
    public void EditForDisplay(AirlineClassCabin airlineClassCabin)
    {

        ProductRepository productRepository = new ProductRepository();
        Product product = new Product();
        product = productRepository.GetProduct(airlineClassCabin.ProductId);
        if (product != null)
        {
            airlineClassCabin.ProductName = product.ProductName;
        }

        SupplierRepository supplierRepository = new SupplierRepository();
        Supplier supplier = new Supplier();
        supplier = supplierRepository.GetSupplier(airlineClassCabin.SupplierCode, airlineClassCabin.ProductId);
        if (supplier != null)
        {
            airlineClassCabin.SupplierName = supplier.SupplierName;
        }

    }
        
    //Add to DB
    public void Add(AirlineClassCabin airlineClassCabin, PolicyRouting policyRouting)
    {
        string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

        db.spDesktopDataAdmin_InsertAirlineClassCabin_v1(
            airlineClassCabin.AirlineClassCode,
            airlineClassCabin.SupplierCode,
            airlineClassCabin.ProductId,
            airlineClassCabin.AirlineCabinCode,
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
    }

    //Update in DB
    public void Update(AirlineClassCabin airlineClassCabin, PolicyRouting policyRouting)
    {
        string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

        db.spDesktopDataAdmin_UpdateAirlineClassCabin_v1(
            airlineClassCabin.AirlineClassCode,
            airlineClassCabin.SupplierCode,
            airlineClassCabin.PolicyRoutingId,
            airlineClassCabin.ProductId,
            airlineClassCabin.AirlineCabinCode,
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
            airlineClassCabin.VersionNumber
        );

    }
        
    //Delete From DB
    public void Delete(AirlineClassCabin airlineClassCabin)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteAirlineClassCabin_v1(
                airlineClassCabin.AirlineClassCode,
                airlineClassCabin.SupplierCode,
                airlineClassCabin.PolicyRoutingId,
                adminUserGuid,
                airlineClassCabin.VersionNumber
            );
        }
        
    }
}

