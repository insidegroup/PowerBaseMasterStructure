using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CWTDesktopDatabase.Repository
{
	public class ThirdPartyUserGDSAccessRightRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
		LogRepository logRepository = new LogRepository();

		//List ThirdPartyUserGDSAccessRights
		public CWTPaginatedList<spDesktopDataAdmin_SelectThirdPartyUserGDSAccessRights_v1Result> PageThirdPartyUserGDSAccessRights(int id, string filter, string sortField, int sortOrder, int page, bool deleted)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = db.spDesktopDataAdmin_SelectThirdPartyUserGDSAccessRights_v1(id, filter, sortField, sortOrder, page, deleted, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectThirdPartyUserGDSAccessRights_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get ThirdPartyUserGDSAccessRights
		public List<ThirdPartyUserGDSAccessRight> GetThirdPartyUserGDSAccessRights(int gdsAccessTypeId)
		{
			return db.ThirdPartyUserGDSAccessRights.ToList();
		}

		//Get one ThirdPartyUserGDSAccessRight
		public ThirdPartyUserGDSAccessRight GetThirdPartyUserGDSAccessRight(int thirdPartyUserGDSAccessRightId)
		{
			return db.ThirdPartyUserGDSAccessRights.SingleOrDefault(c => c.ThirdPartyUserGDSAccessRightId == thirdPartyUserGDSAccessRightId);
		}

		//Add ThirdPartyUserGDSAccessRight
		public void Add(ThirdPartyUserGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			string computerName = logRepository.GetComputerName();

			db.spDesktopDataAdmin_InsertThirdPartyUserGDSAccessRight_v1(
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.GDSSignOnID,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.GDSCode,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.PseudoCityOrOfficeId,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.TAGTIDCertificate,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.GDSAccessTypeId,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.RequestId,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.EnabledDate,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemark,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId,
				gdsAccessTypeVM.ThirdPartyUser.TISUserId, 
				gdsAccessTypeVM.ThirdPartyUser.ThirdPartyName,
				gdsAccessTypeVM.ThirdPartyUser.LastName,
				gdsAccessTypeVM.ThirdPartyUser.FirstName,
				gdsAccessTypeVM.ThirdPartyUser.Email,
				adminUserGuid,
				Settings.ApplicationName(),
				Settings.ApplicationVersion(),
				computerName,
				gdsAccessTypeVM.ThirdPartyUser.ClientTopUnitGuid
			);
		}

		//Edit ThirdPartyUserGDSAccessRight
		public void Update(ThirdPartyUserGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			string computerName = logRepository.GetComputerName();

			db.spDesktopDataAdmin_UpdateThirdPartyUserGDSAccessRight_v1(
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.GDSSignOnID,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.GDSCode,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.PseudoCityOrOfficeId,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.TAGTIDCertificate,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.GDSAccessTypeId,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.RequestId,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.EnabledDate,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemark,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserId,
				gdsAccessTypeVM.ThirdPartyUser.TISUserId,
				gdsAccessTypeVM.ThirdPartyUser.ThirdPartyName,
				gdsAccessTypeVM.ThirdPartyUser.LastName,
				gdsAccessTypeVM.ThirdPartyUser.FirstName,
				gdsAccessTypeVM.ThirdPartyUser.Email,
				adminUserGuid,
				Settings.ApplicationName(),
				Settings.ApplicationVersion(),
				computerName,
				gdsAccessTypeVM.ThirdPartyUser.ClientTopUnitGuid
			);
		}

		//UpdateDeletedStatus ThirdPartyUserGDSAccessRight
		public void UpdateDeletedStatus(ThirdPartyUserGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateThirdPartyUserGDSAccessRightDeletedStatus_v1(
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.DeletedFlag,
				adminUserGuid,
				gdsAccessTypeVM.ThirdPartyUserGDSAccessRight.VersionNumber
			);
		}

		public void EditForDisplay(ThirdPartyUserGDSAccessRight gdsAccessRight)
		{
			//System User
			ThirdPartyUser systemUser = new ThirdPartyUser();
			ThirdPartyUserRepository systemUserRepository = new ThirdPartyUserRepository();
			systemUser = systemUserRepository.GetThirdPartyUser(gdsAccessRight.ThirdPartyUserId);
			if (systemUser != null)
			{
				gdsAccessRight.ThirdPartyUser = systemUser;
			}
		}
		
		//Export Items to CSV
		public byte[] Export(int id)
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
			headers.Add("Deleted Flag");
			headers.Add("Deleted Date Time");
			headers.Add("GDSName");
			headers.Add("Home PCC/OfficeID");
			headers.Add("GDS Sign On ID");
			headers.Add("GDS Access Type Name");
			headers.Add("TA/GTID/Certificate");
			headers.Add("Enabled Date");
			headers.Add("Internal Remarks");
			headers.Add("Deleted Flag");
			headers.Add("Deteted Date Time");
			headers.Add("Creation TimeStamp");
			headers.Add("Creation User Identifier");
			headers.Add("Last Update TimeStamp");
			headers.Add("Last Update User Identifier");

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<ThirdPartyUserGDSAccessRight> gdsAccessRights = db.ThirdPartyUserGDSAccessRights.Where(x => x.ThirdPartyUserId == id).ToList();
			foreach (ThirdPartyUserGDSAccessRight item in gdsAccessRights)
			{

				string date_format = "MM/dd/yy HH:mm";

				EditForDisplay(item);

				//ThirdPartyUserGDSAccessRightInternalRemarks
				StringBuilder remarksList = new StringBuilder();
				foreach (ThirdPartyUserGDSAccessRightInternalRemark systemUserGDSAccessRightInternalRemark in item.ThirdPartyUserGDSAccessRightInternalRemarks)
				{
					remarksList.AppendFormat("{0} - {1}; ", 
						systemUserGDSAccessRightInternalRemark.CreationTimestamp.Value.ToString("yyyy-MM-dd"), 
						systemUserGDSAccessRightInternalRemark.InternalRemark);
				}

				sb.AppendFormat(
					"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35}",

					//ThirdPartyUser
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.TISUserId) ? item.ThirdPartyUser.TISUserId : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.ThirdPartyName) ? item.ThirdPartyUser.ThirdPartyName : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.LastName) ? item.ThirdPartyUser.LastName : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.FirstName) ? item.ThirdPartyUser.FirstName : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.Email) ? item.ThirdPartyUser.Email : " ",
					item.ThirdPartyUser != null && item.ThirdPartyUser.IsActiveFlag == true ? "True" : "False",
					item.ThirdPartyUser != null && item.ThirdPartyUser.CubaBookingAllowanceIndicator == true ? "True" : "False",
					item.ThirdPartyUser != null && item.ThirdPartyUser.MilitaryAndGovernmentUserFlag == true ? "True" : "False",
					item.ThirdPartyUser != null && item.ThirdPartyUser.RoboticUserFlag == true ? "True" : "False",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.CWTManager) ? item.ThirdPartyUser.CWTManager : " ",
					item.ThirdPartyUser != null && item.ThirdPartyUser.ThirdPartyUserType != null && !string.IsNullOrEmpty(item.ThirdPartyUser.ThirdPartyUserType.ThirdPartyUserTypeName) ? item.ThirdPartyUser.ThirdPartyUserType.ThirdPartyUserTypeName : " ",
					item.ThirdPartyUser != null && item.ThirdPartyUser.ClientSubUnit != null && !string.IsNullOrEmpty(item.ThirdPartyUser.ClientSubUnit.ClientSubUnitName) ? item.ThirdPartyUser.ClientSubUnit.ClientSubUnitName : " ",
					item.ThirdPartyUser != null && item.ThirdPartyUser.ClientSubUnit != null && !string.IsNullOrEmpty(item.ThirdPartyUser.ClientSubUnit.ClientSubUnitGuid) ? item.ThirdPartyUser.ClientSubUnit.ClientSubUnitGuid : " ",
					item.ThirdPartyUser != null && item.ThirdPartyUser.Partner != null && !string.IsNullOrEmpty(item.ThirdPartyUser.Partner.PartnerName) ? item.ThirdPartyUser.Partner.PartnerName : " ",
					item.ThirdPartyUser != null && item.ThirdPartyUser.GDSThirdPartyVendor != null && !string.IsNullOrEmpty(item.ThirdPartyUser.GDSThirdPartyVendor.GDSThirdPartyVendorName) ? item.ThirdPartyUser.GDSThirdPartyVendor.GDSThirdPartyVendorName : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.FirstAddressLine) ? item.ThirdPartyUser.FirstAddressLine : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.SecondAddressLine) ? item.ThirdPartyUser.SecondAddressLine : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.PostalCode) ? item.ThirdPartyUser.PostalCode : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.StateProvinceCode) ? item.ThirdPartyUser.StateProvinceCode : " ",
					item.ThirdPartyUser != null && item.ThirdPartyUser.Country != null && !string.IsNullOrEmpty(item.ThirdPartyUser.Country.CountryName) ? item.ThirdPartyUser.Country.CountryName : " ",
					item.ThirdPartyUser != null && !string.IsNullOrEmpty(item.ThirdPartyUser.Phone) ? item.ThirdPartyUser.Phone : " ",
					item.ThirdPartyUser != null && item.ThirdPartyUser.DeletedFlag == true ? "True" : "False",
					item.ThirdPartyUser != null && item.ThirdPartyUser.DeletedDateTime.HasValue ? item.ThirdPartyUser.DeletedDateTime.Value.ToString(date_format) : " ",

					//GDSAccessRight
					item.GDS != null && !string.IsNullOrEmpty(item.GDS.GDSName) ? item.GDS.GDSName : " ",
					!string.IsNullOrEmpty(item.PseudoCityOrOfficeId) ? item.PseudoCityOrOfficeId : " ",
					!string.IsNullOrEmpty(item.GDSSignOnID) ? item.GDSSignOnID : " ",
					item.GDSAccessType != null && !string.IsNullOrEmpty(item.GDSAccessType.GDSAccessTypeName) ? item.GDSAccessType.GDSAccessTypeName : " ",
					!string.IsNullOrEmpty(item.TAGTIDCertificate) ? item.TAGTIDCertificate : " ",
					item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString(date_format) : " ",
					remarksList != null && !string.IsNullOrEmpty(remarksList.ToString()) ? string.Format("\"{0}\"", remarksList) : " ",
					item.DeletedFlag == true ? "True" : "False",
					item.DeletedDateTime.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : " ",
					item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : " ",
					!string.IsNullOrEmpty(item.CreationUserIdentifier) ? item.CreationUserIdentifier : " ",
					item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : " ",
					string.IsNullOrEmpty(item.LastUpdateUserIdentifier) ? item.LastUpdateUserIdentifier : " "
				);

				sb.Append(Environment.NewLine);
			}

			return Encoding.ASCII.GetBytes(sb.ToString());
		}
	}
}