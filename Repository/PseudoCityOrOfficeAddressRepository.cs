using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class PseudoCityOrOfficeAddressRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		private HierarchyRepository hierarchyRepository = new HierarchyRepository();

		//List PseudoCityOrOfficeAddresses
        public CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeAddress_v1Result> GetPseudoCityOrOfficeAddresses(string sortField, int sortOrder, string filter, int page)
        {
			var result = db.spDesktopDataAdmin_SelectPseudoCityOrOfficeAddress_v1(sortField, sortOrder, filter, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPseudoCityOrOfficeAddress_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

		//Get PseudoCityOrOfficeAddresses
		public List<PseudoCityOrOfficeAddress> GetPseudoCityOrOfficeAddresses(int pseudoCityOrOfficeAddressId)
		{
			return db.PseudoCityOrOfficeAddresses.ToList();
		}

		//Get one PseudoCityOrOfficeAddress
		public PseudoCityOrOfficeAddress GetPseudoCityOrOfficeAddress(int pseudoCityOrOfficeAddressId)
		{
			return db.PseudoCityOrOfficeAddresses.SingleOrDefault(c => c.PseudoCityOrOfficeAddressId == pseudoCityOrOfficeAddressId);
		}

		//Get PseudoCityOrOfficeAddressCountryGlobalRegion for JSON
		public List<PseudoCityOrOfficeAddressCountryGlobalRegionJSON> GetPseudoCityOrOfficeAddressCountryGlobalRegion(int pseudoCityOrOfficeAddressId)
		{

			List<PseudoCityOrOfficeAddressCountryGlobalRegionJSON> pseudoCityOrOfficeAddressCountryGlobalRegionJSONs = new List<PseudoCityOrOfficeAddressCountryGlobalRegionJSON>();

			PseudoCityOrOfficeAddress pseudoCityOrOfficeAddress = new PseudoCityOrOfficeAddress();
			pseudoCityOrOfficeAddress = GetPseudoCityOrOfficeAddress(pseudoCityOrOfficeAddressId);
			if (pseudoCityOrOfficeAddress != null)
			{
				string globalSubRegionCode = pseudoCityOrOfficeAddress.Country.GlobalSubRegionCode;
				GlobalSubRegion globalSubRegion = hierarchyRepository.GetGlobalSubRegion(globalSubRegionCode);
				if (globalSubRegion != null)
				{
					GlobalRegion globalRegion = hierarchyRepository.GetGlobalRegion(globalSubRegion.GlobalRegionCode);
					if (globalRegion != null)
					{
						PseudoCityOrOfficeAddressCountryGlobalRegionJSON pseudoCityOrOfficeAddressCountryGlobalRegionJSON = new PseudoCityOrOfficeAddressCountryGlobalRegionJSON()
						{
							CountryCode = pseudoCityOrOfficeAddress.Country.CountryCode,
							CountryName = pseudoCityOrOfficeAddress.Country.CountryName,
							GlobalRegionCode = globalRegion.GlobalRegionCode,
							GlobalRegionName = globalRegion.GlobalRegionName
						};
						pseudoCityOrOfficeAddressCountryGlobalRegionJSONs.Add(pseudoCityOrOfficeAddressCountryGlobalRegionJSON);
					}
				}
			}
			return pseudoCityOrOfficeAddressCountryGlobalRegionJSONs;
		}

		//Get All PseudoCityOrOfficeAddress
		public IQueryable<PseudoCityOrOfficeAddress> GetAllPseudoCityOrOfficeAddresses()
		{
			return db.PseudoCityOrOfficeAddresses.OrderBy(c => c.FirstAddressLine);
		}

		//Get All PseudoCityOrOfficeAddress
		public IQueryable<PseudoCityOrOfficeAddress> GetSelectedPseudoCityOrOfficeAddress(int pseudoCityOrOfficeAddressId)
		{
			return db.PseudoCityOrOfficeAddresses.Where(c => c.PseudoCityOrOfficeAddressId == pseudoCityOrOfficeAddressId).OrderBy(c => c.FirstAddressLine);
		}

        //Edit PseudoCityOrOfficeAddress for Display
        public void EditForDisplay(PseudoCityOrOfficeAddress pseudoCityOrOfficeAddress)
        {
            if(pseudoCityOrOfficeAddress.CountryCode != null && pseudoCityOrOfficeAddress.StateProvinceCode != null)
            {
                StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
                StateProvince stateProvince = stateProvinceRepository.GetStateProvinceByCountry(pseudoCityOrOfficeAddress.CountryCode, pseudoCityOrOfficeAddress.StateProvinceCode);
                if(stateProvince != null)
                {
                    pseudoCityOrOfficeAddress.StateProvinceName = stateProvince.Name;
                }
            }
        }

        //Get All PseudoCityOrOfficeAddress based upon the role of the system user and their Location. 
        public List<PseudoCityOrOfficeAddress> GetUserPseudoCityOrOfficeAddresses()
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectPseudoCityOrOfficeAddressByLocation_v1(adminUserGuid)
						 select
							 new PseudoCityOrOfficeAddress
							 {
								PseudoCityOrOfficeAddressId = n.PseudoCityOrOfficeAddressId,
								FirstAddressLine = n.FirstAddressLine.ToString(),
								SecondAddressLine = n.SecondAddressLine != null ? n.SecondAddressLine.ToString() : "",
								CityName = n.CityName.ToString(),
								CountryCode = n.CountryCode.ToString(),
								StateProvinceCode = n.StateProvinceCode != null ? n.StateProvinceCode.ToString() : "",
								PostalCode = n.PostalCode.ToString(),
							 };

			return result.ToList();			
		}

		//Add PseudoCityOrOfficeAddress
		public void Add(PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPseudoCityOrOfficeAddress_v1(
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.FirstAddressLine,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.SecondAddressLine,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.CityName,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.CountryCode,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.StateProvinceCode,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.PostalCode,
				adminUserGuid
			);
        }

		//Edit PseudoCityOrOfficeAddress
		public void Update(PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePseudoCityOrOfficeAddress_v1(
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.FirstAddressLine,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.SecondAddressLine,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.CityName,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.CountryCode,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.StateProvinceCode,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.PostalCode,
				adminUserGuid
			);
		}

		//Delete PseudoCityOrOfficeAddress
		public void Delete(PseudoCityOrOfficeAddressVM pseudoCityOrOfficeAddressVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePseudoCityOrOfficeAddress_v1(
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId,
				adminUserGuid,
				pseudoCityOrOfficeAddressVM.PseudoCityOrOfficeAddress.VersionNumber
			);
		}

		public List<PseudoCityOrOfficeAddressReference> GetPseudoCityOrOfficeAddressReferences(int pseudoCityOrOfficeAddressId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectPseudoCityOrOfficeAddressReferences_v1(pseudoCityOrOfficeAddressId, adminUserGuid)
						 select
							 new PseudoCityOrOfficeAddressReference
							 {
								 TableName = n.TableName.ToString()
							 };

			return result.ToList();
		}
	}
}