using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class GDSOrderTypeRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List GDSOrderTypes
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrderType_v1Result> PageGDSOrderTypes(string filter, string sortField, int sortOrder, int page)
		{
			var result = db.spDesktopDataAdmin_SelectGDSOrderType_v1(filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSOrderType_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get GDSOrderTypes
		public List<GDSOrderType> GetAllGDSOrderTypes()
		{
			return db.GDSOrderTypes.ToList();
		}

		public void EditForDisplay(GDSOrderType item)
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
			item.IsThirdPartyFlagNullable = (item.IsThirdPartyFlag.HasValue) ? item.IsThirdPartyFlag.Value : false;
		}

		//Get one GDSOrderType
		public GDSOrderType GetGDSOrderType(int gdsOrderTypeId)
		{
			return db.GDSOrderTypes.SingleOrDefault(c => c.GDSOrderTypeId == gdsOrderTypeId);
		}
		
		//Add GDSOrderType
		public void Add(GDSOrderTypeVM gdsOrderTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertGDSOrderType_v1(
				gdsOrderTypeVM.GDSOrderType.GDSOrderTypeName,
 				gdsOrderTypeVM.GDSOrderType.AbacusFlagNullable,
				gdsOrderTypeVM.GDSOrderType.AllGDSSystemsFlagNullable,
				gdsOrderTypeVM.GDSOrderType.AmadeusFlagNullable,
				gdsOrderTypeVM.GDSOrderType.ApolloFlagNullable,
				gdsOrderTypeVM.GDSOrderType.EDSFlagNullable,
				gdsOrderTypeVM.GDSOrderType.GalileoFlagNullable,
				gdsOrderTypeVM.GDSOrderType.RadixxFlagNullable,
				gdsOrderTypeVM.GDSOrderType.SabreFlagNullable,
				gdsOrderTypeVM.GDSOrderType.TravelskyFlagNullable,
				gdsOrderTypeVM.GDSOrderType.WorldspanFlagNullable,
				gdsOrderTypeVM.GDSOrderType.IsThirdPartyFlagNullable,
				adminUserGuid
			);
		}

		//Edit GDSOrderType
		public void Update(GDSOrderTypeVM gdsOrderTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateGDSOrderType_v1(
				gdsOrderTypeVM.GDSOrderType.GDSOrderTypeId,
				gdsOrderTypeVM.GDSOrderType.GDSOrderTypeName,
				gdsOrderTypeVM.GDSOrderType.AbacusFlagNullable,
				gdsOrderTypeVM.GDSOrderType.AllGDSSystemsFlagNullable,
				gdsOrderTypeVM.GDSOrderType.AmadeusFlagNullable,
				gdsOrderTypeVM.GDSOrderType.ApolloFlagNullable,
				gdsOrderTypeVM.GDSOrderType.EDSFlagNullable,
				gdsOrderTypeVM.GDSOrderType.GalileoFlagNullable,
				gdsOrderTypeVM.GDSOrderType.RadixxFlagNullable,
				gdsOrderTypeVM.GDSOrderType.SabreFlagNullable,
				gdsOrderTypeVM.GDSOrderType.TravelskyFlagNullable,
				gdsOrderTypeVM.GDSOrderType.WorldspanFlagNullable,
				gdsOrderTypeVM.GDSOrderType.IsThirdPartyFlagNullable,
				adminUserGuid
			);
		}

		//Delete GDSOrderType
		public void Delete(GDSOrderTypeVM gdsOrderTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGDSOrderType_v1(
				gdsOrderTypeVM.GDSOrderType.GDSOrderTypeId,
				adminUserGuid,
				gdsOrderTypeVM.GDSOrderType.VersionNumber
			);
		}

		public List<GDSOrderTypeReference> GetGDSOrderTypeReferences(int gdsOrderTypeId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = from n in db.spDesktopDataAdmin_SelectGDSOrderTypeReferences_v1(gdsOrderTypeId, adminUserGuid)
						 select
							 new GDSOrderTypeReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}

		//Is GDSOrderType ThirdParty Mandatory
		public bool IsGDSOrderTypeThirdPartyMandatory(int gdsOrderTypeId)
		{
			bool isGDSOrderTypeThirdPartyMandatory = false;

			GDSOrderType gdsOrderType = db.GDSOrderTypes.SingleOrDefault(c => c.GDSOrderTypeId == gdsOrderTypeId);

			if (gdsOrderType != null)
			{
				if(gdsOrderType.IsThirdPartyFlag == true) {
					isGDSOrderTypeThirdPartyMandatory = true;
				}
			}

			return isGDSOrderTypeThirdPartyMandatory;
		}

		//GDSOrderTypeShowDataFlag True/False
		public bool GDSOrderTypeShowDataFlag(int gdsOrderTypeId)
		{
			bool showDataFlag = false;

			GDSOrderType gdsOrderType = db.GDSOrderTypes.SingleOrDefault(c => c.GDSOrderTypeId == gdsOrderTypeId);

			if (gdsOrderType != null)
			{
				if(gdsOrderType.ShowDataFlag == true) {
					showDataFlag = true;
				}
			}

			return showDataFlag;
		}
	}
}