using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Repository
{
    public class CommissionableRouteItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of CommissionableRouteItems - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectCommissionableRouteItems_v1Result> PageCommissionableRouteItems(int id, int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectCommissionableRouteItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectCommissionableRouteItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get One CommissionableRouteItem
		public CommissionableRouteItem CommissionableRouteItem(int id)
		{
			return db.CommissionableRouteItems.SingleOrDefault(c => c.CommissionableRouteItemId == id);
		}

		//Add Data From Linked Tables for Display
		public void EditForDisplay(CommissionableRouteItem CommissionableRouteItem)
		{

			ProductRepository productRepository = new ProductRepository();
			Product product = new Product();
			product = productRepository.GetProduct(CommissionableRouteItem.ProductId);
			if (product != null)
			{
				CommissionableRouteItem.ProductName = product.ProductName;
			}

			SupplierRepository supplierRepository = new SupplierRepository();
			Supplier supplier = new Supplier();
			supplier = supplierRepository.GetSupplier(CommissionableRouteItem.SupplierCode, CommissionableRouteItem.ProductId);
			if (supplier != null)
			{
				CommissionableRouteItem.SupplierName = supplier.SupplierName;
			}
		}

		//Add to DB
		public void Add(CommissionableRouteItem commissionableRouteItem, PolicyRouting policyRouting)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			int? commissionableRouteItemId = new Int32();

			db.spDesktopDataAdmin_InsertCommissionableRouteItem_v1(
				ref commissionableRouteItemId,
				commissionableRouteItem.TravelIndicator,
				commissionableRouteItem.ClassOfTravel,
				commissionableRouteItem.CommissionAmount,
				commissionableRouteItem.CommissionAmountCurrencyCode,
				commissionableRouteItem.NegotiatedFareFlag, 
				commissionableRouteItem.BSPCommission,
				commissionableRouteItem.CommissionableTaxCodes,
				commissionableRouteItem.CommissionOnTaxes,
				commissionableRouteItem.RemarksOrRoute,
				commissionableRouteItem.SupplierCode,
				commissionableRouteItem.ProductId,
				commissionableRouteItem.CommissionableRouteGroupId,
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

			commissionableRouteItem.CommissionableRouteItemId = (int)commissionableRouteItemId;
		}

		//Update in DB
		public void Update(CommissionableRouteItem commissionableRouteItem, PolicyRouting policyRouting)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateCommissionableRouteItem_v1(
				commissionableRouteItem.CommissionableRouteItemId,
				commissionableRouteItem.TravelIndicator,
				commissionableRouteItem.ClassOfTravel,
				commissionableRouteItem.CommissionAmount,
				commissionableRouteItem.CommissionAmountCurrencyCode,
				commissionableRouteItem.NegotiatedFareFlag, 
				commissionableRouteItem.BSPCommission,
				commissionableRouteItem.CommissionableTaxCodes,
				commissionableRouteItem.CommissionOnTaxes,
				commissionableRouteItem.RemarksOrRoute,
				commissionableRouteItem.SupplierCode,
				commissionableRouteItem.ProductId,
				commissionableRouteItem.CommissionableRouteGroupId,
				commissionableRouteItem.PolicyRoutingId,
				commissionableRouteItem.VersionNumber,
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

		//Delete From DB
		public void Delete(CommissionableRouteItem CommissionableRouteItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteCommissionableRouteItem_v1(
				CommissionableRouteItem.CommissionableRouteItemId,
				adminUserGuid,
				CommissionableRouteItem.VersionNumber
			);
		}

	}
}
