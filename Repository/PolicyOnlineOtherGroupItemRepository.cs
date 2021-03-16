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
    public class PolicyOnlineOtherGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItems_v1Result> GetPolicyOnlineOtherGroupItems(int id, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyOnlineOtherGroupItem GetPolicyOnlineOtherGroupItem(int policyId, int policyOtherGroupHeaderId)
		{
			return db.PolicyOnlineOtherGroupItems.SingleOrDefault(c => c.PolicyGroupId == policyId && c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
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
		//public void Add(PolicyOnlineOtherGroupItem PolicyOnlineOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
		//	int? PolicyOnlineOtherGroupItemId = new Int32();

		//	db.spDesktopDataAdmin_InsertPolicyOnlineOtherGroupItem_v1(
		//		ref PolicyOnlineOtherGroupItemId,
		//		PolicyOnlineOtherGroupItem.PolicyAirStatusId,
		//		PolicyOnlineOtherGroupItem.EnabledFlag,
		//		PolicyOnlineOtherGroupItem.EnabledDate,
		//		PolicyOnlineOtherGroupItem.ExpiryDate,
		//		PolicyOnlineOtherGroupItem.TravelDateValidFrom,
		//		PolicyOnlineOtherGroupItem.TravelDateValidTo,
		//		PolicyOnlineOtherGroupItem.PolicyGroupId,
		//		PolicyOnlineOtherGroupItem.SupplierCode,
		//		PolicyOnlineOtherGroupItem.ProductId,
		//		PolicyOnlineOtherGroupItem.AirVendorRanking,
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

		//	PolicyOnlineOtherGroupItem.PolicyOnlineOtherGroupItemId = (int)PolicyOnlineOtherGroupItemId;
		//}

		////Edit
		//public void Update(PolicyOnlineOtherGroupItem PolicyOnlineOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_UpdatePolicyOnlineOtherGroupItem_v1(
		//		PolicyOnlineOtherGroupItem.PolicyOnlineOtherGroupItemId,
		//		PolicyOnlineOtherGroupItem.PolicyAirStatusId,
		//		PolicyOnlineOtherGroupItem.EnabledFlag,
		//		PolicyOnlineOtherGroupItem.EnabledDate,
		//		PolicyOnlineOtherGroupItem.ExpiryDate,
		//		PolicyOnlineOtherGroupItem.TravelDateValidFrom,
		//		PolicyOnlineOtherGroupItem.TravelDateValidTo,
		//		PolicyOnlineOtherGroupItem.PolicyGroupId,
		//		PolicyOnlineOtherGroupItem.SupplierCode,
		//		PolicyOnlineOtherGroupItem.ProductId,
		//		PolicyOnlineOtherGroupItem.AirVendorRanking,
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
		//		PolicyOnlineOtherGroupItem.VersionNumber
		//	);

		//}

		////Delete
		//public void Delete(PolicyOnlineOtherGroupItem PolicyOnlineOtherGroupItem)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_DeletePolicyOnlineOtherGroupItem_v1(
		//		PolicyOnlineOtherGroupItem.PolicyOnlineOtherGroupItemId,
		//		adminUserGuid,
		//		PolicyOnlineOtherGroupItem.VersionNumber
		//		);
		//}

		////Get one Item
		//public PolicyOnlineOtherGroupItem GetPolicyOnlineOtherGroupItem(int PolicyOnlineOtherGroupItemId)
		//{
		//	PolicyOnlineOtherGroupItemDC db = new PolicyOnlineOtherGroupItemDC(Settings.getConnectionString());
		//	return db.PolicyOnlineOtherGroupItems.SingleOrDefault(c => c.PolicyOnlineOtherGroupItemId == PolicyOnlineOtherGroupItemId);
		//}

		////Add Data From Linked Tables for Display
		//public void EditItemForDisplay(PolicyOnlineOtherGroupItem PolicyOnlineOtherGroupItem)
		//{
		//	if (PolicyOnlineOtherGroupItem.PolicyAirStatusId != null)
		//	{
		//		int policyAirStatusId = (int)PolicyOnlineOtherGroupItem.PolicyAirStatusId;
		//		PolicyAirStatusRepository policyAirStatusRepository = new PolicyAirStatusRepository();
		//		PolicyAirStatus policyAirStatus = new PolicyAirStatus();
		//		policyAirStatus = policyAirStatusRepository.GetPolicyAirStatus(policyAirStatusId);
		//		PolicyOnlineOtherGroupItem.PolicyAirStatus = policyAirStatus.PolicyAirStatusDescription;
		//	}
		//	else
		//	{
		//		PolicyOnlineOtherGroupItem.PolicyAirStatus = "None";
		//	}

		//	//populate new PolicyOnlineOtherGroupItem with PolicyGroupName    
		//	if (PolicyOnlineOtherGroupItem.PolicyGroupId != 0)
		//	{
		//		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		//		PolicyGroup policyGroup = new PolicyGroup();
		//		policyGroup = policyGroupRepository.GetGroup(PolicyOnlineOtherGroupItem.PolicyGroupId);
		//		PolicyOnlineOtherGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
		//	}
		//	//Supplier
		//	SupplierRepository supplierRepository = new SupplierRepository();
		//	Supplier supplier = new Supplier();
		//	supplier = supplierRepository.GetSupplier(PolicyOnlineOtherGroupItem.SupplierCode, PolicyOnlineOtherGroupItem.ProductId);
		//	if (supplier != null)
		//	{
		//		PolicyOnlineOtherGroupItem.SupplierName = supplier.SupplierName;
		//	}

		//	//Product
		//	ProductRepository productRepository = new ProductRepository();
		//	Product product = new Product();
		//	product = productRepository.GetProduct(PolicyOnlineOtherGroupItem.ProductId);
		//	if (product != null)
		//	{
		//		PolicyOnlineOtherGroupItem.ProductName = product.ProductName;
		//	}

		//}
    
    }
}
