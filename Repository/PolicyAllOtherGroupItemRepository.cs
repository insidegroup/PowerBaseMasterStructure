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
    public class PolicyAllOtherGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAllOtherGroupItems_v1Result> GetPolicyAllOtherGroupItems(int id, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyAllOtherGroupItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAllOtherGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyAllOtherGroupItem GetPolicyAllOtherGroupItem(int policyId, int policyOtherGroupHeaderId)
		{
			return db.PolicyAllOtherGroupItems.SingleOrDefault(c => c.PolicyGroupId == policyId && c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
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
		//public void Add(PolicyAllOtherGroupItem policyAllOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
		//	int? policyAllOtherGroupItemId = new Int32();

		//	db.spDesktopDataAdmin_InsertPolicyAllOtherGroupItem_v1(
		//		ref policyAllOtherGroupItemId,
		//		policyAllOtherGroupItem.PolicyAllStatusId,
		//		policyAllOtherGroupItem.EnabledFlag,
		//		policyAllOtherGroupItem.EnabledDate,
		//		policyAllOtherGroupItem.ExpiryDate,
		//		policyAllOtherGroupItem.TravelDateValidFrom,
		//		policyAllOtherGroupItem.TravelDateValidTo,
		//		policyAllOtherGroupItem.PolicyGroupId,
		//		policyAllOtherGroupItem.SupplierCode,
		//		policyAllOtherGroupItem.ProductId,
		//		policyAllOtherGroupItem.AirVendorRanking,
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

		//	policyAllOtherGroupItem.PolicyAllOtherGroupItemId = (int)policyAllOtherGroupItemId;
		//}

		////Edit
		//public void Update(PolicyAllOtherGroupItem policyAllOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_UpdatePolicyAllOtherGroupItem_v1(
		//		policyAllOtherGroupItem.PolicyAllOtherGroupItemId,
		//		policyAllOtherGroupItem.PolicyAllStatusId,
		//		policyAllOtherGroupItem.EnabledFlag,
		//		policyAllOtherGroupItem.EnabledDate,
		//		policyAllOtherGroupItem.ExpiryDate,
		//		policyAllOtherGroupItem.TravelDateValidFrom,
		//		policyAllOtherGroupItem.TravelDateValidTo,
		//		policyAllOtherGroupItem.PolicyGroupId,
		//		policyAllOtherGroupItem.SupplierCode,
		//		policyAllOtherGroupItem.ProductId,
		//		policyAllOtherGroupItem.AirVendorRanking,
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
		//		policyAllOtherGroupItem.VersionNumber
		//	);

		//}

		////Delete
		//public void Delete(PolicyAllOtherGroupItem policyAllOtherGroupItem)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_DeletePolicyAllOtherGroupItem_v1(
		//		policyAllOtherGroupItem.PolicyAllOtherGroupItemId,
		//		adminUserGuid,
		//		policyAllOtherGroupItem.VersionNumber
		//		);
		//}

		////Get one Item
		//public PolicyAllOtherGroupItem GetPolicyAllOtherGroupItem(int policyAllOtherGroupItemId)
		//{
		//	PolicyAllOtherGroupItemDC db = new PolicyAllOtherGroupItemDC(Settings.getConnectionString());
		//	return db.PolicyAllOtherGroupItems.SingleOrDefault(c => c.PolicyAllOtherGroupItemId == policyAllOtherGroupItemId);
		//}

		////Add Data From Linked Tables for Display
		//public void EditItemForDisplay(PolicyAllOtherGroupItem policyAllOtherGroupItem)
		//{
		//	if (policyAllOtherGroupItem.PolicyAllStatusId != null)
		//	{
		//		int policyAllStatusId = (int)policyAllOtherGroupItem.PolicyAllStatusId;
		//		PolicyAllStatusRepository policyAllStatusRepository = new PolicyAllStatusRepository();
		//		PolicyAllStatus policyAllStatus = new PolicyAllStatus();
		//		policyAllStatus = policyAllStatusRepository.GetPolicyAllStatus(policyAllStatusId);
		//		policyAllOtherGroupItem.PolicyAllStatus = policyAllStatus.PolicyAllStatusDescription;
		//	}
		//	else
		//	{
		//		policyAllOtherGroupItem.PolicyAllStatus = "None";
		//	}

		//	//populate new PolicyAllOtherGroupItem with PolicyGroupName    
		//	if (policyAllOtherGroupItem.PolicyGroupId != 0)
		//	{
		//		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		//		PolicyGroup policyGroup = new PolicyGroup();
		//		policyGroup = policyGroupRepository.GetGroup(policyAllOtherGroupItem.PolicyGroupId);
		//		policyAllOtherGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
		//	}
		//	//Supplier
		//	SupplierRepository supplierRepository = new SupplierRepository();
		//	Supplier supplier = new Supplier();
		//	supplier = supplierRepository.GetSupplier(policyAllOtherGroupItem.SupplierCode, policyAllOtherGroupItem.ProductId);
		//	if (supplier != null)
		//	{
		//		policyAllOtherGroupItem.SupplierName = supplier.SupplierName;
		//	}

		//	//Product
		//	ProductRepository productRepository = new ProductRepository();
		//	Product product = new Product();
		//	product = productRepository.GetProduct(policyAllOtherGroupItem.ProductId);
		//	if (product != null)
		//	{
		//		policyAllOtherGroupItem.ProductName = product.ProductName;
		//	}

		//}
    
    }
}
