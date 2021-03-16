using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.ViewModels;
using System.Text;

namespace CWTDesktopDatabase.Repository
{
    public class ThirdPartyUserRepository
    {
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		LogRepository logRepository = new LogRepository();

		//List of ThirdPartyUsers
		public CWTPaginatedList<spDesktopDataAdmin_SelectThirdPartyUsers_v1Result> PageThirdPartyUsers(
			int page,
			string sortField,
			int sortOrder,
			bool deletedFlag,
			string filterField_1 = "",
			string filterValue_1 = "",
			string filterField_2 = "",
			string filterValue_2 = "",
			string filterField_3 = "",
			string filterValue_3 = ""
		)
		{

			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectThirdPartyUsers_v1(
				adminUserGuid,
				page,
				sortField,
				sortOrder,
				deletedFlag,
				filterField_1,
				filterValue_1,
				filterField_2,
				filterValue_2,
				filterField_3,
				filterValue_3
			).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//add paging information and return1
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectThirdPartyUsers_v1Result>(result, page, totalRecords);
			return paginatedView;
		}

		public IQueryable<ThirdPartyUser> GetAllThirdPartyUsers()
		{
			return db.ThirdPartyUsers.OrderBy(t => t.ThirdPartyName);
		}

		public ThirdPartyUser GetThirdPartyUser(int thirdPartyUserId)
		{
			return db.ThirdPartyUsers.SingleOrDefault(c => c.ThirdPartyUserId == thirdPartyUserId);
		}

		public Dictionary<string, string> GetThirdPartyUserFilters()
		{
			Dictionary<string, string> filters = new Dictionary<string, string>();
			filters.Add("CountryCode", "Country");
			filters.Add("Email", "Email");
			filters.Add("FirstName", "First Name");
			filters.Add("GDSSignOnID", "GDS Sign On ID");
			filters.Add("HomePCCOfficeID", "Home PCC/Office ID");
			filters.Add("ID", "ID");
			filters.Add("LastName", "Last Name");
			filters.Add("ThirdPartyName", "Third Party Name");

			return filters;
		}

		//Add Data From Linked Tables for Display
		public void EditForDisplay(ThirdPartyUser thirdPartyUser)
		{
			//Flags
			thirdPartyUser.IsActiveFlagNonNullable = (thirdPartyUser.IsActiveFlag.HasValue) ? thirdPartyUser.IsActiveFlag.Value : false;
			thirdPartyUser.RoboticUserFlagNonNullable = (thirdPartyUser.RoboticUserFlag.HasValue) ? thirdPartyUser.RoboticUserFlag.Value : false;
			thirdPartyUser.CubaBookingAllowanceIndicatorNonNullable = (thirdPartyUser.CubaBookingAllowanceIndicator.HasValue) ? thirdPartyUser.CubaBookingAllowanceIndicator.Value : false;
			thirdPartyUser.MilitaryAndGovernmentUserFlagNonNullable = (thirdPartyUser.MilitaryAndGovernmentUserFlag.HasValue) ? thirdPartyUser.MilitaryAndGovernmentUserFlag.Value : false;

			if (thirdPartyUser.Partner != null)
			{
				thirdPartyUser.PartnerName = thirdPartyUser.Partner.PartnerName;
			}

			if (thirdPartyUser.Country != null)
			{
				thirdPartyUser.CountryName = thirdPartyUser.Country.CountryName;
			}

			if (thirdPartyUser.ClientTopUnit != null)
			{
				thirdPartyUser.ClientTopUnitName = thirdPartyUser.ClientTopUnit.ClientTopUnitName;
			}

			if (thirdPartyUser.ClientSubUnit != null)
			{
				thirdPartyUser.ClientSubUnitName = thirdPartyUser.ClientSubUnit.ClientSubUnitName;
			}

			if (thirdPartyUser.StateProvinceCode != null)
			{
				StateProvinceRepository stateProvinceRepository = new StateProvinceRepository();
				thirdPartyUser.StateProvince = stateProvinceRepository.GetStateProvinceByCountry(thirdPartyUser.CountryCode, thirdPartyUser.StateProvinceCode);
			}

			//ThirdPartyUserGDSAccessRights
			if (thirdPartyUser.ThirdPartyUserGDSAccessRights != null && thirdPartyUser.ThirdPartyUserGDSAccessRights.Count() > 0)
			{
				List<Entitlement> entitlements = new List<Entitlement>();
				foreach (ThirdPartyUserGDSAccessRight thirdPartyUserGDSAccessRight in thirdPartyUser.ThirdPartyUserGDSAccessRights)
				{
					Entitlement entitlement = new Entitlement()
					{
						tpAgentID = thirdPartyUserGDSAccessRight.GDSSignOnID,
						tpPCC = thirdPartyUserGDSAccessRight.PseudoCityOrOfficeId,
						tpServiceID = thirdPartyUserGDSAccessRight.GDS.GDSName,
						DeletedFlag = thirdPartyUserGDSAccessRight.DeletedFlag == true ? true : false,
						DeletedTimestamp = thirdPartyUserGDSAccessRight.DeletedDateTime
					};
					entitlements.Add(entitlement);
				}
				thirdPartyUser.Entitlements = entitlements;
			}
		}

		//Create ThirdPartyUser
		public void Add(ThirdPartyUserVM thirdPartyUserVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			string computerName = logRepository.GetComputerName();

			db.spDesktopDataAdmin_InsertThirdPartyUser_v1(
				thirdPartyUserVM.ThirdPartyUser.TISUserId,
				thirdPartyUserVM.ThirdPartyUser.ThirdPartyName,
				thirdPartyUserVM.ThirdPartyUser.LastName,
				thirdPartyUserVM.ThirdPartyUser.FirstName,
				thirdPartyUserVM.ThirdPartyUser.Email,
				thirdPartyUserVM.ThirdPartyUser.IsActiveFlagNonNullable,
				thirdPartyUserVM.ThirdPartyUser.CubaBookingAllowanceIndicatorNonNullable,
				thirdPartyUserVM.ThirdPartyUser.MilitaryAndGovernmentUserFlagNonNullable,
				thirdPartyUserVM.ThirdPartyUser.RoboticUserFlagNonNullable,
				thirdPartyUserVM.ThirdPartyUser.CWTManager,
				thirdPartyUserVM.ThirdPartyUser.ThirdPartyUserTypeId,
				thirdPartyUserVM.ThirdPartyUser.ClientTopUnitGuid,
				thirdPartyUserVM.ThirdPartyUser.ClientSubUnitGuid,
				thirdPartyUserVM.ThirdPartyUser.PartnerId,
				thirdPartyUserVM.ThirdPartyUser.GDSThirdPartyVendorId,
				thirdPartyUserVM.ThirdPartyUser.FirstAddressLine,
				thirdPartyUserVM.ThirdPartyUser.SecondAddressLine,
				thirdPartyUserVM.ThirdPartyUser.CityName,
				thirdPartyUserVM.ThirdPartyUser.PostalCode,
				thirdPartyUserVM.ThirdPartyUser.CountryCode,
				thirdPartyUserVM.ThirdPartyUser.StateProvinceCode,
				thirdPartyUserVM.ThirdPartyUser.Phone,
				thirdPartyUserVM.ThirdPartyUser.InternalRemark,
				adminUserGuid,
				Settings.ApplicationName(),
				Settings.ApplicationVersion(),
				computerName
			);
		}

		//Edit ThirdPartyUser
		public void Update(ThirdPartyUserVM thirdPartyUserVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			string computerName = logRepository.GetComputerName();

			db.spDesktopDataAdmin_UpdateThirdPartyUser_v1(
				thirdPartyUserVM.ThirdPartyUser.ThirdPartyUserId,
				thirdPartyUserVM.ThirdPartyUser.TISUserId,
				thirdPartyUserVM.ThirdPartyUser.ThirdPartyName,
				thirdPartyUserVM.ThirdPartyUser.LastName,
				thirdPartyUserVM.ThirdPartyUser.FirstName,
				thirdPartyUserVM.ThirdPartyUser.Email,
				thirdPartyUserVM.ThirdPartyUser.IsActiveFlagNonNullable,
				thirdPartyUserVM.ThirdPartyUser.CubaBookingAllowanceIndicatorNonNullable,
				thirdPartyUserVM.ThirdPartyUser.MilitaryAndGovernmentUserFlagNonNullable,
				thirdPartyUserVM.ThirdPartyUser.RoboticUserFlagNonNullable,
				thirdPartyUserVM.ThirdPartyUser.CWTManager,
				thirdPartyUserVM.ThirdPartyUser.ThirdPartyUserTypeId,
				thirdPartyUserVM.ThirdPartyUser.ClientTopUnitGuid,
				thirdPartyUserVM.ThirdPartyUser.ClientSubUnitGuid,
				thirdPartyUserVM.ThirdPartyUser.PartnerId,
				thirdPartyUserVM.ThirdPartyUser.GDSThirdPartyVendorId,
				thirdPartyUserVM.ThirdPartyUser.FirstAddressLine,
				thirdPartyUserVM.ThirdPartyUser.SecondAddressLine,
				thirdPartyUserVM.ThirdPartyUser.CityName,
				thirdPartyUserVM.ThirdPartyUser.PostalCode,
				thirdPartyUserVM.ThirdPartyUser.CountryCode,
				thirdPartyUserVM.ThirdPartyUser.StateProvinceCode,
				thirdPartyUserVM.ThirdPartyUser.Phone,
				thirdPartyUserVM.ThirdPartyUser.InternalRemark,
				adminUserGuid,
				thirdPartyUserVM.ThirdPartyUser.VersionNumber,
				Settings.ApplicationName(),
				Settings.ApplicationVersion(),
				computerName			
			);
		}

		//UpdateDeletedStatus ThirdPartyUser
		public void UpdateDeletedStatus(ThirdPartyUserVM thirdPartyUserVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateThirdPartyUserDeletedStatus_v1(
				thirdPartyUserVM.ThirdPartyUser.ThirdPartyUserId,
				thirdPartyUserVM.ThirdPartyUser.DeletedFlag,
				adminUserGuid,
				thirdPartyUserVM.ThirdPartyUser.VersionNumber
			);
		}

		//Export Items to CSV
		public byte[] Export(
			string filterField_1 = "",
			string filterValue_1 = "",
			string filterField_2 = "",
			string filterValue_2 = "",
			string filterField_3 = "",
			string filterValue_3 = ""
		)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>();

			headers.Add("Third Party User ID");
			headers.Add("Third Party Name");
			headers.Add("Last Name");
			headers.Add("First Name");
			headers.Add("Email");
			headers.Add("Is Active Flag");
			headers.Add("Cuba Booking Allowance Indicator");
			headers.Add("Military and Government User");
			headers.Add("Robotic User Flag");
			headers.Add("CWT Manager");
			headers.Add("User Type");
			headers.Add("Client SubUnit Name");
			headers.Add("Client SubUnit Guid");
			headers.Add("Partner Name");
			headers.Add("Vendor Name");
			headers.Add("First Address line");
			headers.Add("Second Address line");
			headers.Add("Postal Code");
			headers.Add("State/Province");
			headers.Add("Country");
			headers.Add("Phone Number");
			headers.Add("Internal Remarks");
			headers.Add("Deleted Flag");
			headers.Add("Deleted Date Time");
			headers.Add("Creation Time Stamp");
			headers.Add("Creation User Identifier");
			headers.Add("LastUpdate TimeStamp");
			headers.Add("LastUpdate UserIdentifier");

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<ThirdPartyUserExport> thirdPartyUsers = GetThirdPartyUserExports(
				filterField_1,
				filterValue_1,
				filterField_2,
				filterValue_2,
				filterField_3,
				filterValue_3
			);

			foreach (ThirdPartyUserExport item in thirdPartyUsers)
			{

				string date_format = "MM/dd/yy HH:mm";

				EditForDisplay(item);

				//ThirdPartyUserInternalRemarks
				StringBuilder remarksList = new StringBuilder();
				ThirdPartyUser thirdPartyUser = GetThirdPartyUser(item.ThirdPartyUserId);
				if (thirdPartyUser.ThirdPartyUserInternalRemarks != null && thirdPartyUser.ThirdPartyUserInternalRemarks.Count > 0)
				{
					foreach (ThirdPartyUserInternalRemark thirdPartyUserInternalRemark in thirdPartyUser.ThirdPartyUserInternalRemarks)
					{
						remarksList.AppendFormat("{0} - {1}; ", thirdPartyUserInternalRemark.CreationTimestamp.Value.ToString(date_format), thirdPartyUserInternalRemark.InternalRemark);
					}
				}

				sb.AppendFormat(
					"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27}",
					!string.IsNullOrEmpty(item.TISUserId) ? item.TISUserId : " ",
					!string.IsNullOrEmpty(item.ThirdPartyName) ? item.ThirdPartyName : " ",
					!string.IsNullOrEmpty(item.LastName) ? item.LastName : " ",
					!string.IsNullOrEmpty(item.FirstName) ? item.FirstName : " ",
					!string.IsNullOrEmpty(item.Email) ? item.Email : " ",
					item.IsActiveFlag == true ? "True" : "False",
					item.CubaBookingAllowanceIndicator == true ? "True" : "False",
					item.MilitaryAndGovernmentUserFlag == true ? "True" : "False",
					item.RoboticUserFlag == true ? "True" : "False",
					!string.IsNullOrEmpty(item.CWTManager) ? item.CWTManager : " ",
					!string.IsNullOrEmpty(item.ThirdPartyUserTypeName) ? item.ThirdPartyUserTypeName : " ",
					!string.IsNullOrEmpty(item.ClientSubUnitName) ? item.ClientSubUnitName : " ",
					!string.IsNullOrEmpty(item.ClientSubUnitGuid) ? item.ClientSubUnitGuid : " ",
					!string.IsNullOrEmpty(item.PartnerName) ? item.PartnerName : " ",
					!string.IsNullOrEmpty(item.GDSThirdPartyVendorName) ? item.GDSThirdPartyVendorName : " ",
					!string.IsNullOrEmpty(item.FirstAddressLine) ? item.FirstAddressLine : " ",
					!string.IsNullOrEmpty(item.SecondAddressLine) ? item.SecondAddressLine : " ",
					!string.IsNullOrEmpty(item.PostalCode) ? item.PostalCode : " ",
					!string.IsNullOrEmpty(item.StateProvinceCode) ? item.StateProvinceCode : " ",
					!string.IsNullOrEmpty(item.CountryName) ? item.CountryName : " ",
					!string.IsNullOrEmpty(item.Phone) ? item.Phone : " ",
					remarksList != null && !string.IsNullOrEmpty(remarksList.ToString()) ? string.Format("\"{0}\"", remarksList) : " ",
					item.DeletedFlag == true ? "True" : "False",
					item.DeletedDateTime.HasValue ? item.DeletedDateTime.Value.ToString(date_format) : " ",
					item.CreationTimeStamp.HasValue ? item.CreationTimeStamp.Value.ToString(date_format) : " ",
					!string.IsNullOrEmpty(item.CreationUserIdentifier) ? item.CreationUserIdentifier : " ",
					item.LastUpdateTimeStamp.HasValue ? item.LastUpdateTimeStamp.Value.ToString(date_format) : " ",
					!string.IsNullOrEmpty(item.LastUpdateUserIdentifier) ? item.LastUpdateUserIdentifier : " "
				);

				sb.Append(Environment.NewLine);
			}

			return Encoding.ASCII.GetBytes(sb.ToString());
		}

		public List<ThirdPartyUserExport> GetThirdPartyUserExports(string filterField_1 = "", string filterValue_1 = "", string filterField_2 = "",	string filterValue_2 = "", string filterField_3 = "", string filterValue_3 = "")
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = from n in db.spDesktopDataAdmin_SelectThirdPartyUserExport_v1(filterField_1, filterValue_1, filterField_2, filterValue_2, filterField_3, filterValue_3, adminUserGuid)
			 select
				 new ThirdPartyUserExport
				 {
					 ThirdPartyUserId = n.ThirdPartyUserId,
					 TISUserId = n.TISUserId ?? "" ?? "",
					 ThirdPartyName = n.ThirdPartyName ?? "",
					 LastName = n.LastName ?? "",
					 FirstName = n.FirstName ?? "",
					 Email = n.Email ?? "",
					 IsActiveFlag = n.IsActiveFlag ?? false,
					 CubaBookingAllowanceIndicator = n.CubaBookingAllowanceIndicator ?? false,
					 MilitaryAndGovernmentUserFlag = n.MilitaryAndGovernmentUserFlag ?? false,
					 RoboticUserFlag = n.RoboticUserFlag ?? false,
					 CWTManager = n.CWTManager ?? "",
					 ThirdPartyUserTypeName = n.ThirdPartyUserTypeName ?? "",
					 ClientSubUnitGuid = n.ClientSubUnitGuid ?? "",
					 ClientSubUnitName = n.ClientSubUnitName ?? "",
					 PartnerName = n.PartnerName ?? "",
					 GDSThirdPartyVendorName = n.GDSThirdPartyVendorName ?? "",
					 FirstAddressLine = n.FirstAddressLine ?? "",
					 SecondAddressLine = n.SecondAddressLine ?? "",
					 PostalCode = n.PostalCode ?? "",
					 StateProvinceCode = n.StateProvinceCode ?? "",
					 CountryName = n.CountryName ?? "",
					 Phone = n.Phone ?? "",
					 DeletedFlag = n.DeletedFlag ?? false,
					 DeletedDateTime = n.DeletedDateTime,
					 CreationTimeStamp = n.CreationTimestamp,
					 CreationUserIdentifier = n.CreationUserIdentifier,
					 LastUpdateTimeStamp = n.LastUpdateTimestamp,
					 LastUpdateUserIdentifier = n.LastUpdateUserIdentifier
				 };

			return result.ToList();
		}

		//Get PseudoCityOrOfficeIds by GDSCode based on SystemUser and ThirdPartyUser
		public List<ValidPseudoCityOrOfficeIdJSON> GetThirdPartyUserPseudoCityOrOfficeIdsByGDSCode(string gdsCode, int thirdPartyUserId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			return (from n in db.fnDesktopDataAdmin_SelectThirdPartyUserValidPseudoCityOrOfficeIds_v1(gdsCode, thirdPartyUserId, adminUserGuid).OrderBy(x => x.PseudoCityOrOfficeId)
						select
							new ValidPseudoCityOrOfficeIdJSON
							{
								PseudoCityOrOfficeId = n.PseudoCityOrOfficeId.ToString().Trim(),
								GDSCode = n.GDSCode
							}
				).ToList();
		}
    }
}
