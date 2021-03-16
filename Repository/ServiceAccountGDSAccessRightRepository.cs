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
	public class ServiceAccountGDSAccessRightRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List ServiceAccountGDSAccessRights
		public CWTPaginatedList<spDesktopDataAdmin_SelectServiceAccountGDSAccessRights_v1Result> PageServiceAccountGDSAccessRights(string id, string filter, string sortField, int sortOrder, int page, bool deleted)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = db.spDesktopDataAdmin_SelectServiceAccountGDSAccessRights_v1(id, filter, sortField, sortOrder, page, deleted, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectServiceAccountGDSAccessRights_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get ServiceAccountGDSAccessRights
		public List<ServiceAccountGDSAccessRight> GetServiceAccountGDSAccessRights(int gdsAccessTypeId)
		{
			return db.ServiceAccountGDSAccessRights.ToList();
		}

		//Get one ServiceAccountGDSAccessRight
		public ServiceAccountGDSAccessRight GetServiceAccountGDSAccessRight(int serviceAccountGDSAccessRightId)
		{
			return db.ServiceAccountGDSAccessRights.SingleOrDefault(c => c.ServiceAccountGDSAccessRightId == serviceAccountGDSAccessRightId);
		}

		//Add ServiceAccountGDSAccessRight
		public void Add(ServiceAccountGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertServiceAccountGDSAccessRight_v1(
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.GDSSignOnID,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.GDSCode,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.PseudoCityOrOfficeId,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.TAGTIDCertificate,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.GDSAccessTypeId,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.RequestId,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.EnabledDate,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightInternalRemark,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.ServiceAccountId,
				adminUserGuid
			);
		}

		//Edit ServiceAccountGDSAccessRight
		public void Update(ServiceAccountGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateServiceAccountGDSAccessRight_v1(
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightId,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.GDSSignOnID,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.GDSCode,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.PseudoCityOrOfficeId,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.TAGTIDCertificate,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.GDSAccessTypeId,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.RequestId,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.EnabledDate,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightInternalRemark,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.ServiceAccountId,
				adminUserGuid
			);
		}

		//UpdateDeletedStatus ServiceAccountGDSAccessRight
		public void UpdateDeletedStatus(ServiceAccountGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateServiceAccountGDSAccessRightDeletedStatus_v1(
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightId,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.DeletedFlag,
				adminUserGuid,
				gdsAccessTypeVM.ServiceAccountGDSAccessRight.VersionNumber
			);
		}

		public void EditForDisplay(ServiceAccountGDSAccessRight gdsAccessRight)
		{
			//System User
			ServiceAccount systemUser = new ServiceAccount();
			ServiceAccountRepository systemUserRepository = new ServiceAccountRepository();
			systemUser = systemUserRepository.GetServiceAccount(gdsAccessRight.ServiceAccountId);
			if (systemUser != null)
			{
				gdsAccessRight.ServiceAccount = systemUser;
			}
		}
		
		//Export Items to CSV
		public byte[] Export(string id)
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
			List<ServiceAccountGDSAccessRight> gdsAccessRights = db.ServiceAccountGDSAccessRights.Where(x => x.ServiceAccountId == id).ToList();
			foreach (ServiceAccountGDSAccessRight item in gdsAccessRights)
			{

				string date_format = "MM/dd/yy HH:mm";

				EditForDisplay(item);

				//ServiceAccountGDSAccessRightInternalRemarks
				StringBuilder remarksList = new StringBuilder();
				foreach (ServiceAccountGDSAccessRightInternalRemark systemUserGDSAccessRightInternalRemark in item.ServiceAccountGDSAccessRightInternalRemarks)
				{
					remarksList.AppendFormat("{0} - {1}; ", 
						systemUserGDSAccessRightInternalRemark.CreationTimestamp.Value.ToString("yyyy-MM-dd"), 
						systemUserGDSAccessRightInternalRemark.InternalRemark);
				}

				sb.AppendFormat(
					"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35}",

					//ServiceAccount
					item.ServiceAccount != null && !string.IsNullOrEmpty(item.ServiceAccount.ServiceAccountId) ? item.ServiceAccount.ServiceAccountId : " ",
					item.ServiceAccount != null && !string.IsNullOrEmpty(item.ServiceAccount.ServiceAccountName) ? item.ServiceAccount.ServiceAccountName : " ",
					item.ServiceAccount != null && !string.IsNullOrEmpty(item.ServiceAccount.LastName) ? item.ServiceAccount.LastName : " ",
					item.ServiceAccount != null && !string.IsNullOrEmpty(item.ServiceAccount.FirstName) ? item.ServiceAccount.FirstName : " ",
					item.ServiceAccount != null && !string.IsNullOrEmpty(item.ServiceAccount.Email) ? item.ServiceAccount.Email : " ",
					item.ServiceAccount != null && item.ServiceAccount.IsActiveFlag == true ? "True" : "False",
					item.ServiceAccount != null && item.ServiceAccount.CubaBookingAllowanceIndicator == true ? "True" : "False",
					item.ServiceAccount != null && item.ServiceAccount.MilitaryAndGovernmentUserFlag == true ? "True" : "False",
					item.ServiceAccount != null && item.ServiceAccount.RoboticUserFlag == true ? "True" : "False",
					item.ServiceAccount != null && !string.IsNullOrEmpty(item.ServiceAccount.CWTManager) ? item.ServiceAccount.CWTManager : " ",
					item.ServiceAccount != null && item.ServiceAccount.ThirdPartyUserType != null  ? item.ServiceAccount.ThirdPartyUserType : " ",
					item.ServiceAccount != null && item.ServiceAccount.DeletedFlag == true ? "True" : "False",
					item.ServiceAccount != null && item.ServiceAccount.DeletedDateTime.HasValue ? item.ServiceAccount.DeletedDateTime.Value.ToString(date_format) : " ",

					//GDSAccessRight
					item.GDSCode != null && !string.IsNullOrEmpty(item.GDS.GDSName) ? item.GDS.GDSName : " ",
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