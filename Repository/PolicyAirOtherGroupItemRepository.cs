using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyAirOtherGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirOtherGroupItems_v1Result> GetPolicyAirOtherGroupItems(int id, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyAirOtherGroupItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirOtherGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyAirOtherGroupItem GetPolicyAirOtherGroupItem(int policyId, int policyOtherGroupHeaderId)
		{
			return db.PolicyAirOtherGroupItems.SingleOrDefault(c => c.PolicyGroupId == policyId && c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
		}

		//public List<SelectListItem> AirVendorRankings()
		//{
		//	var numbers = (from p in Enumerable.Range(1, 9)
		//				   select new SelectListItem
		//				   {
		//					   Text = p.ToString(),
		//					   Value = p.ToString()
		//				   });
		//	return numbers.ToList();
		//}

		////Add
		//public void Add(PolicyAirOtherGroupItem policyAirOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
		//	int? policyAirOtherGroupItemId = new Int32();

		//	db.spDesktopDataAdmin_InsertPolicyAirOtherGroupItem_v1(
		//		ref policyAirOtherGroupItemId,
		//		policyAirOtherGroupItem.PolicyAirStatusId,
		//		policyAirOtherGroupItem.EnabledFlag,
		//		policyAirOtherGroupItem.EnabledDate,
		//		policyAirOtherGroupItem.ExpiryDate,
		//		policyAirOtherGroupItem.TravelDateValidFrom,
		//		policyAirOtherGroupItem.TravelDateValidTo,
		//		policyAirOtherGroupItem.PolicyGroupId,
		//		policyAirOtherGroupItem.SupplierCode,
		//		policyAirOtherGroupItem.ProductId,
		//		policyAirOtherGroupItem.AirVendorRanking,
		//		policyRouting.Name,
		//		policyRouting.FromGlobalFlag,
		//		policyRouting.FromGlobalRegionCode,
		//		policyRouting.FromGlobalSubRegionCode,
		//		policyRouting.FromCountryCode,
		//		policyRouting.FromCityCode,
		//		policyRouting.FromTravelPortCode,
		//		policyRouting.FromTraverlPortTypeId,
		//		policyRouting.ToGlobalFlag,
		//		policyRouting.ToGlobalRegionCode,
		//		policyRouting.ToGlobalSubRegionCode,
		//		policyRouting.ToCountryCode,
		//		policyRouting.ToCityCode,
		//		policyRouting.ToTravelPortCode,
		//		policyRouting.ToTravelPortTypeId,
		//		policyRouting.RoutingViceVersaFlag,
		//		adminUserGuid
		//	);

		//	policyAirOtherGroupItem.PolicyAirOtherGroupItemId = (int)policyAirOtherGroupItemId;
		//}

		////Edit
		//public void Update(PolicyAirOtherGroupItem policyAirOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_UpdatePolicyAirOtherGroupItem_v1(
		//		policyAirOtherGroupItem.PolicyAirOtherGroupItemId,
		//		policyAirOtherGroupItem.PolicyAirStatusId,
		//		policyAirOtherGroupItem.EnabledFlag,
		//		policyAirOtherGroupItem.EnabledDate,
		//		policyAirOtherGroupItem.ExpiryDate,
		//		policyAirOtherGroupItem.TravelDateValidFrom,
		//		policyAirOtherGroupItem.TravelDateValidTo,
		//		policyAirOtherGroupItem.PolicyGroupId,
		//		policyAirOtherGroupItem.SupplierCode,
		//		policyAirOtherGroupItem.ProductId,
		//		policyAirOtherGroupItem.AirVendorRanking,
		//		policyRouting.Name,
		//		policyRouting.FromGlobalFlag,
		//		policyRouting.FromGlobalRegionCode,
		//		policyRouting.FromGlobalSubRegionCode,
		//		policyRouting.FromCountryCode,
		//		policyRouting.FromCityCode,
		//		policyRouting.FromTravelPortCode,
		//		policyRouting.FromTraverlPortTypeId,
		//		policyRouting.ToGlobalFlag,
		//		policyRouting.ToGlobalRegionCode,
		//		policyRouting.ToGlobalSubRegionCode,
		//		policyRouting.ToCountryCode,
		//		policyRouting.ToCityCode,
		//		policyRouting.ToTravelPortCode,
		//		policyRouting.ToTravelPortTypeId,
		//		policyRouting.RoutingViceVersaFlag,
		//		adminUserGuid,
		//		policyAirOtherGroupItem.VersionNumber
		//	);

		//}

		////Delete
		//public void Delete(PolicyAirOtherGroupItem policyAirOtherGroupItem)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_DeletePolicyAirOtherGroupItem_v1(
		//		policyAirOtherGroupItem.PolicyAirOtherGroupItemId,
		//		adminUserGuid,
		//		policyAirOtherGroupItem.VersionNumber
		//		);
		//}

		////Get one Item
		//public PolicyAirOtherGroupItem GetPolicyAirOtherGroupItem(int policyAirOtherGroupItemId)
		//{
		//	PolicyAirOtherGroupItemDC db = new PolicyAirOtherGroupItemDC(Settings.getConnectionString());
		//	return db.PolicyAirOtherGroupItems.SingleOrDefault(c => c.PolicyAirOtherGroupItemId == policyAirOtherGroupItemId);
		//}

		////Add Data From Linked Tables for Display
		//public void EditItemForDisplay(PolicyAirOtherGroupItem policyAirOtherGroupItem)
		//{
		//	if (policyAirOtherGroupItem.PolicyAirStatusId != null)
		//	{
		//		int policyAirStatusId = (int)policyAirOtherGroupItem.PolicyAirStatusId;
		//		PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
		//		PolicyAirStatus policyAirStatus = new PolicyAirStatus();
		//		policyAirStatus = policyAirStatusRepository.GetPolicyAirStatus(policyAirStatusId);
		//		policyAirOtherGroupItem.PolicyAirStatus = policyAirStatus.PolicyAirStatusDescription;
		//	}
		//	else
		//	{
		//		policyAirOtherGroupItem.PolicyAirStatus = "None";
		//	}

		//	//populate new PolicyAirOtherGroupItem with PolicyGroupName    
		//	if (policyAirOtherGroupItem.PolicyGroupId != 0)
		//	{
		//		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		//		PolicyGroup policyGroup = new PolicyGroup();
		//		policyGroup = policyGroupRepository.GetGroup(policyAirOtherGroupItem.PolicyGroupId);
		//		policyAirOtherGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
		//	}
		//	//Supplier
		//	SupplierRepository supplierRepository = new SupplierRepository();
		//	Supplier supplier = new Supplier();
		//	supplier = supplierRepository.GetSupplier(policyAirOtherGroupItem.SupplierCode, policyAirOtherGroupItem.ProductId);
		//	if (supplier != null)
		//	{
		//		policyAirOtherGroupItem.SupplierName = supplier.SupplierName;
		//	}

		//	//Product
		//	ProductRepository productRepository = new ProductRepository();
		//	Product product = new Product();
		//	product = productRepository.GetProduct(policyAirOtherGroupItem.ProductId);
		//	if (product != null)
		//	{
		//		policyAirOtherGroupItem.ProductName = product.ProductName;
		//	}

		//}
    
    }
}
