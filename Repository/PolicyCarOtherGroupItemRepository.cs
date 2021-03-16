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
    public class PolicyCarOtherGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarOtherGroupItems_v1Result> GetPolicyCarOtherGroupItems(int id, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyCarOtherGroupItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarOtherGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyCarOtherGroupItem GetPolicyCarOtherGroupItem(int policyId, int policyOtherGroupHeaderId)
		{
			return db.PolicyCarOtherGroupItems.SingleOrDefault(c => c.PolicyGroupId == policyId && c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
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
		//public void Add(PolicyCarOtherGroupItem policyCarOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
		//	int? policyCarOtherGroupItemId = new Int32();

		//	db.spDesktopDataAdmin_InsertPolicyCarOtherGroupItem_v1(
		//		ref policyCarOtherGroupItemId,
		//		policyCarOtherGroupItem.PolicyCarStatusId,
		//		policyCarOtherGroupItem.EnabledFlag,
		//		policyCarOtherGroupItem.EnabledDate,
		//		policyCarOtherGroupItem.ExpiryDate,
		//		policyCarOtherGroupItem.TravelDateValidFrom,
		//		policyCarOtherGroupItem.TravelDateValidTo,
		//		policyCarOtherGroupItem.PolicyGroupId,
		//		policyCarOtherGroupItem.SupplierCode,
		//		policyCarOtherGroupItem.ProductId,
		//		policyCarOtherGroupItem.AirVendorRanking,
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

		//	policyCarOtherGroupItem.PolicyCarOtherGroupItemId = (int)policyCarOtherGroupItemId;
		//}

		////Edit
		//public void Update(PolicyCarOtherGroupItem policyCarOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_UpdatePolicyCarOtherGroupItem_v1(
		//		policyCarOtherGroupItem.PolicyCarOtherGroupItemId,
		//		policyCarOtherGroupItem.PolicyCarStatusId,
		//		policyCarOtherGroupItem.EnabledFlag,
		//		policyCarOtherGroupItem.EnabledDate,
		//		policyCarOtherGroupItem.ExpiryDate,
		//		policyCarOtherGroupItem.TravelDateValidFrom,
		//		policyCarOtherGroupItem.TravelDateValidTo,
		//		policyCarOtherGroupItem.PolicyGroupId,
		//		policyCarOtherGroupItem.SupplierCode,
		//		policyCarOtherGroupItem.ProductId,
		//		policyCarOtherGroupItem.AirVendorRanking,
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
		//		policyCarOtherGroupItem.VersionNumber
		//	);

		//}

		////Delete
		//public void Delete(PolicyCarOtherGroupItem policyCarOtherGroupItem)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_DeletePolicyCarOtherGroupItem_v1(
		//		policyCarOtherGroupItem.PolicyCarOtherGroupItemId,
		//		adminUserGuid,
		//		policyCarOtherGroupItem.VersionNumber
		//		);
		//}

		////Get one Item
		//public PolicyCarOtherGroupItem GetPolicyCarOtherGroupItem(int policyCarOtherGroupItemId)
		//{
		//	PolicyCarOtherGroupItemDC db = new PolicyCarOtherGroupItemDC(Settings.getConnectionString());
		//	return db.PolicyCarOtherGroupItems.SingleOrDefault(c => c.PolicyCarOtherGroupItemId == policyCarOtherGroupItemId);
		//}

		////Add Data From Linked Tables for Display
		//public void EditItemForDisplay(PolicyCarOtherGroupItem policyCarOtherGroupItem)
		//{
		//	if (policyCarOtherGroupItem.PolicyCarStatusId != null)
		//	{
		//		int policyCarStatusId = (int)policyCarOtherGroupItem.PolicyCarStatusId;
		//		PolicyCarStatusRepository policyCarStatusRepository = new PolicyCarStatusRepository();
		//		PolicyCarStatus policyCarStatus = new PolicyCarStatus();
		//		policyCarStatus = policyCarStatusRepository.GetPolicyCarStatus(policyCarStatusId);
		//		policyCarOtherGroupItem.PolicyCarStatus = policyCarStatus.PolicyCarStatusDescription;
		//	}
		//	else
		//	{
		//		policyCarOtherGroupItem.PolicyCarStatus = "None";
		//	}

		//	//populate new PolicyCarOtherGroupItem with PolicyGroupName    
		//	if (policyCarOtherGroupItem.PolicyGroupId != 0)
		//	{
		//		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		//		PolicyGroup policyGroup = new PolicyGroup();
		//		policyGroup = policyGroupRepository.GetGroup(policyCarOtherGroupItem.PolicyGroupId);
		//		policyCarOtherGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
		//	}
		//	//Supplier
		//	SupplierRepository supplierRepository = new SupplierRepository();
		//	Supplier supplier = new Supplier();
		//	supplier = supplierRepository.GetSupplier(policyCarOtherGroupItem.SupplierCode, policyCarOtherGroupItem.ProductId);
		//	if (supplier != null)
		//	{
		//		policyCarOtherGroupItem.SupplierName = supplier.SupplierName;
		//	}

		//	//Product
		//	ProductRepository productRepository = new ProductRepository();
		//	Product product = new Product();
		//	product = productRepository.GetProduct(policyCarOtherGroupItem.ProductId);
		//	if (product != null)
		//	{
		//		policyCarOtherGroupItem.ProductName = product.ProductName;
		//	}

		//}
    
    }
}
