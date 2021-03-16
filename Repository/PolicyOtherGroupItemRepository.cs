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
    public class PolicyOtherGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupItems_v1Result> GetPolicyOtherGroupItems(int id, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyOtherGroupItem GetPolicyOtherGroupItem(int policyId, int policyOtherGroupHeaderId)
		{
			return db.PolicyOtherGroupItems.SingleOrDefault(c => c.PolicyGroupId == policyId && c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
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
		//public void Add(PolicyOtherGroupItem policyOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
		//	int? policyOtherGroupItemId = new Int32();

		//	db.spDesktopDataAdmin_InsertPolicyOtherGroupItem_v1(
		//		ref policyOtherGroupItemId,
		//		policyOtherGroupItem.PolicyStatusId,
		//		policyOtherGroupItem.EnabledFlag,
		//		policyOtherGroupItem.EnabledDate,
		//		policyOtherGroupItem.ExpiryDate,
		//		policyOtherGroupItem.TravelDateValidFrom,
		//		policyOtherGroupItem.TravelDateValidTo,
		//		policyOtherGroupItem.PolicyGroupId,
		//		policyOtherGroupItem.SupplierCode,
		//		policyOtherGroupItem.ProductId,
		//		policyOtherGroupItem.AirVendorRanking,
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

		//	policyOtherGroupItem.PolicyOtherGroupItemId = (int)policyOtherGroupItemId;
		//}

		////Edit
		//public void Update(PolicyOtherGroupItem policyOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_UpdatePolicyOtherGroupItem_v1(
		//		policyOtherGroupItem.PolicyOtherGroupItemId,
		//		policyOtherGroupItem.PolicyStatusId,
		//		policyOtherGroupItem.EnabledFlag,
		//		policyOtherGroupItem.EnabledDate,
		//		policyOtherGroupItem.ExpiryDate,
		//		policyOtherGroupItem.TravelDateValidFrom,
		//		policyOtherGroupItem.TravelDateValidTo,
		//		policyOtherGroupItem.PolicyGroupId,
		//		policyOtherGroupItem.SupplierCode,
		//		policyOtherGroupItem.ProductId,
		//		policyOtherGroupItem.AirVendorRanking,
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
		//		policyOtherGroupItem.VersionNumber
		//	);

		//}

		////Delete
		//public void Delete(PolicyOtherGroupItem policyOtherGroupItem)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_DeletePolicyOtherGroupItem_v1(
		//		policyOtherGroupItem.PolicyOtherGroupItemId,
		//		adminUserGuid,
		//		policyOtherGroupItem.VersionNumber
		//		);
		//}

		////Get one Item
		//public PolicyOtherGroupItem GetPolicyOtherGroupItem(int policyOtherGroupItemId)
		//{
		//	PolicyOtherGroupItemDC db = new PolicyOtherGroupItemDC(Settings.getConnectionString());
		//	return db.PolicyOtherGroupItems.SingleOrDefault(c => c.PolicyOtherGroupItemId == policyOtherGroupItemId);
		//}

		////Add Data From Linked Tables for Display
		//public void EditItemForDisplay(PolicyOtherGroupItem policyOtherGroupItem)
		//{
		//	if (policyOtherGroupItem.PolicyStatusId != null)
		//	{
		//		int policyStatusId = (int)policyOtherGroupItem.PolicyStatusId;
		//		PolicyStatusRepository policyStatusRepository = new PolicyStatusRepository();
		//		PolicyStatus policyStatus = new PolicyStatus();
		//		policyStatus = policyStatusRepository.GetPolicyStatus(policyStatusId);
		//		policyOtherGroupItem.PolicyStatus = policyStatus.PolicyStatusDescription;
		//	}
		//	else
		//	{
		//		policyOtherGroupItem.PolicyStatus = "None";
		//	}

		//	//populate new PolicyOtherGroupItem with PolicyGroupName    
		//	if (policyOtherGroupItem.PolicyGroupId != 0)
		//	{
		//		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		//		PolicyGroup policyGroup = new PolicyGroup();
		//		policyGroup = policyGroupRepository.GetGroup(policyOtherGroupItem.PolicyGroupId);
		//		policyOtherGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
		//	}
		//	//Supplier
		//	SupplierRepository supplierRepository = new SupplierRepository();
		//	Supplier supplier = new Supplier();
		//	supplier = supplierRepository.GetSupplier(policyOtherGroupItem.SupplierCode, policyOtherGroupItem.ProductId);
		//	if (supplier != null)
		//	{
		//		policyOtherGroupItem.SupplierName = supplier.SupplierName;
		//	}

		//	//Product
		//	ProductRepository productRepository = new ProductRepository();
		//	Product product = new Product();
		//	product = productRepository.GetProduct(policyOtherGroupItem.ProductId);
		//	if (product != null)
		//	{
		//		policyOtherGroupItem.ProductName = product.ProductName;
		//	}

		//}
    
    }
}
