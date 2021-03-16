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
    public class PolicyHotelOtherGroupItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelOtherGroupItems_v1Result> GetPolicyHotelOtherGroupItems(int id, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyHotelOtherGroupItems_v1(id, filter, sortField, sortOrder, page, adminUserGuid).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyHotelOtherGroupItems_v1Result>(result,page,totalRecords);
            return paginatedView;

        }

		//Get one Item
		public PolicyHotelOtherGroupItem GetPolicyHotelOtherGroupItem(int policyId, int policyOtherGroupHeaderId)
		{
			return db.PolicyHotelOtherGroupItems.SingleOrDefault(c => c.PolicyGroupId == policyId && c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId);
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
		//public void Add(PolicyHotelOtherGroupItem policyHotelOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
		//	int? policyHotelOtherGroupItemId = new Int32();

		//	db.spDesktopDataAdmin_InsertPolicyHotelOtherGroupItem_v1(
		//		ref policyHotelOtherGroupItemId,
		//		policyHotelOtherGroupItem.PolicyHotelStatusId,
		//		policyHotelOtherGroupItem.EnabledFlag,
		//		policyHotelOtherGroupItem.EnabledDate,
		//		policyHotelOtherGroupItem.ExpiryDate,
		//		policyHotelOtherGroupItem.TravelDateValidFrom,
		//		policyHotelOtherGroupItem.TravelDateValidTo,
		//		policyHotelOtherGroupItem.PolicyGroupId,
		//		policyHotelOtherGroupItem.SupplierCode,
		//		policyHotelOtherGroupItem.ProductId,
		//		policyHotelOtherGroupItem.AirVendorRanking,
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

		//	policyHotelOtherGroupItem.PolicyHotelOtherGroupItemId = (int)policyHotelOtherGroupItemId;
		//}

		////Edit
		//public void Update(PolicyHotelOtherGroupItem policyHotelOtherGroupItem, PolicyRouting policyRouting)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_UpdatePolicyHotelOtherGroupItem_v1(
		//		policyHotelOtherGroupItem.PolicyHotelOtherGroupItemId,
		//		policyHotelOtherGroupItem.PolicyHotelStatusId,
		//		policyHotelOtherGroupItem.EnabledFlag,
		//		policyHotelOtherGroupItem.EnabledDate,
		//		policyHotelOtherGroupItem.ExpiryDate,
		//		policyHotelOtherGroupItem.TravelDateValidFrom,
		//		policyHotelOtherGroupItem.TravelDateValidTo,
		//		policyHotelOtherGroupItem.PolicyGroupId,
		//		policyHotelOtherGroupItem.SupplierCode,
		//		policyHotelOtherGroupItem.ProductId,
		//		policyHotelOtherGroupItem.AirVendorRanking,
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
		//		policyHotelOtherGroupItem.VersionNumber
		//	);

		//}

		////Delete
		//public void Delete(PolicyHotelOtherGroupItem policyHotelOtherGroupItem)
		//{
		//	string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

		//	db.spDesktopDataAdmin_DeletePolicyHotelOtherGroupItem_v1(
		//		policyHotelOtherGroupItem.PolicyHotelOtherGroupItemId,
		//		adminUserGuid,
		//		policyHotelOtherGroupItem.VersionNumber
		//		);
		//}

		////Get one Item
		//public PolicyHotelOtherGroupItem GetPolicyHotelOtherGroupItem(int policyHotelOtherGroupItemId)
		//{
		//	PolicyHotelOtherGroupItemDC db = new PolicyHotelOtherGroupItemDC(Settings.getConnectionString());
		//	return db.PolicyHotelOtherGroupItems.SingleOrDefault(c => c.PolicyHotelOtherGroupItemId == policyHotelOtherGroupItemId);
		//}

		////Add Data From Linked Tables for Display
		//public void EditItemForDisplay(PolicyHotelOtherGroupItem policyHotelOtherGroupItem)
		//{
		//	if (policyHotelOtherGroupItem.PolicyHotelStatusId != null)
		//	{
		//		int policyHotelStatusId = (int)policyHotelOtherGroupItem.PolicyHotelStatusId;
		//		PolicyHotelStatusRepository policyHotelStatusRepository = new PolicyHotelStatusRepository();
		//		PolicyHotelStatus policyHotelStatus = new PolicyHotelStatus();
		//		policyHotelStatus = policyHotelStatusRepository.GetPolicyHotelStatus(policyHotelStatusId);
		//		policyHotelOtherGroupItem.PolicyHotelStatus = policyHotelStatus.PolicyHotelStatusDescription;
		//	}
		//	else
		//	{
		//		policyHotelOtherGroupItem.PolicyHotelStatus = "None";
		//	}

		//	//populate new PolicyHotelOtherGroupItem with PolicyGroupName    
		//	if (policyHotelOtherGroupItem.PolicyGroupId != 0)
		//	{
		//		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		//		PolicyGroup policyGroup = new PolicyGroup();
		//		policyGroup = policyGroupRepository.GetGroup(policyHotelOtherGroupItem.PolicyGroupId);
		//		policyHotelOtherGroupItem.PolicyGroupName = policyGroup.PolicyGroupName;
		//	}
		//	//Supplier
		//	SupplierRepository supplierRepository = new SupplierRepository();
		//	Supplier supplier = new Supplier();
		//	supplier = supplierRepository.GetSupplier(policyHotelOtherGroupItem.SupplierCode, policyHotelOtherGroupItem.ProductId);
		//	if (supplier != null)
		//	{
		//		policyHotelOtherGroupItem.SupplierName = supplier.SupplierName;
		//	}

		//	//Product
		//	ProductRepository productRepository = new ProductRepository();
		//	Product product = new Product();
		//	product = productRepository.GetProduct(policyHotelOtherGroupItem.ProductId);
		//	if (product != null)
		//	{
		//		policyHotelOtherGroupItem.ProductName = product.ProductName;
		//	}

		//}
    
    }
}
