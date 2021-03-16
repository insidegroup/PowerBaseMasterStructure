using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSOrderDetailRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List GDSOrderDetails
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrderDetail_v1Result> PageGDSOrderDetails(string filter, string sortField, int sortOrder, int page)
		{
			var result = db.spDesktopDataAdmin_SelectGDSOrderDetail_v1(filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrderDetail_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get GDSOrderDetails
		public List<GDSOrderDetail> GetAllGDSOrderDetails()
		{
			return db.GDSOrderDetails.ToList();
		}

		public void EditForDisplay(GDSOrderDetail item)
		{
			item.AbacusFlagNullable = (item.AbacusFlag.HasValue) ? item.AbacusFlag.Value : false;
			item.AllGDSSystemsFlagNullable = (item.AllGDSSystemsFlag.HasValue) ? item.AllGDSSystemsFlag.Value : false;
			item.AmadeusFlagNullable = (item.AmadeusFlag.HasValue) ? item.AmadeusFlag.Value : false;
			item.ApolloFlagNullable = (item.ApolloFlag.HasValue) ? item.ApolloFlag.Value : false;
			item.EDSFlagNullable = (item.EDSFlag.HasValue) ? item.EDSFlag.Value : false;
			item.GalileoFlagNullable = (item.GalileoFlag.HasValue) ? item.GalileoFlag.Value : false;
			item.RadixxFlagNullable = (item.RadixxFlag.HasValue) ? item.RadixxFlag.Value : false;
			item.SabreFlagNullable = (item.SabreFlag.HasValue) ? item.SabreFlag.Value : false;
			item.TravelskyFlagNullable = (item.TravelskyFlag.HasValue) ? item.TravelskyFlag.Value : false;
			item.WorldspanFlagNullable = (item.WorldspanFlag.HasValue) ? item.WorldspanFlag.Value : false;
		}

		//Get one GDSOrderDetail
		public GDSOrderDetail GetGDSOrderDetail(int gdsOrderDetailId)
		{
			return db.GDSOrderDetails.SingleOrDefault(c => c.GDSOrderDetailId == gdsOrderDetailId);
		}

		//Add GDSOrderDetail
		public void Add(GDSOrderDetailVM gdsOrderDetailVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertGDSOrderDetail_v1(
				gdsOrderDetailVM.GDSOrderDetail.GDSOrderDetailName,
 				gdsOrderDetailVM.GDSOrderDetail.AbacusFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.AllGDSSystemsFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.AmadeusFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.ApolloFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.EDSFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.GalileoFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.RadixxFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.SabreFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.TravelskyFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.WorldspanFlagNullable,
				adminUserGuid
			);
		}

		//Edit GDSOrderDetail
		public void Update(GDSOrderDetailVM gdsOrderDetailVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateGDSOrderDetail_v1(
				gdsOrderDetailVM.GDSOrderDetail.GDSOrderDetailId,
				gdsOrderDetailVM.GDSOrderDetail.GDSOrderDetailName,
				gdsOrderDetailVM.GDSOrderDetail.AbacusFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.AllGDSSystemsFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.AmadeusFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.ApolloFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.EDSFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.GalileoFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.RadixxFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.SabreFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.TravelskyFlagNullable,
				gdsOrderDetailVM.GDSOrderDetail.WorldspanFlagNullable,
				adminUserGuid
			);
		}

		//Delete GDSOrderDetail
		public void Delete(GDSOrderDetailVM gdsOrderDetailVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGDSOrderDetail_v1(
				gdsOrderDetailVM.GDSOrderDetail.GDSOrderDetailId,
				adminUserGuid,
				gdsOrderDetailVM.GDSOrderDetail.VersionNumber
			);
		}

		public List<GDSOrderDetailReference> GetGDSOrderDetailReferences(int gdsOrderDetailId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectGDSOrderDetailReferences_v1(gdsOrderDetailId, adminUserGuid)
						 select
							 new GDSOrderDetailReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}

		//Get GDSOrderDetails by GDSCode
		public List<ValidGDSOrderDetailJSON> GetGDSOrderDetailsByGDSCode(string gdsCode)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			return (from n in db.fnDesktopDataAdmin_SelectGDSOrderDetailsByGDSCode_v1(gdsCode, adminUserGuid).OrderBy(x => x.GDSOrderDetailName)
					select
						new ValidGDSOrderDetailJSON
						{
							GDSOrderDetailId = (n.GDSOrderDetailId.HasValue) ? n.GDSOrderDetailId.Value : 0,
							GDSOrderDetailName = n.GDSOrderDetailName.ToString().Trim()
						}
				).ToList();
		}
	}
}