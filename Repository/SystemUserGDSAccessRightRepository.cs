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
	public class SystemUserGDSAccessRightRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List SystemUserGDSAccessRights
		public CWTPaginatedList<spDesktopDataAdmin_SelectSystemUserGDSAccessRights_v1Result> PageSystemUserGDSAccessRights(string id, string filter, string sortField, int sortOrder, int page, bool deleted)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			var result = db.spDesktopDataAdmin_SelectSystemUserGDSAccessRights_v1(id, filter, sortField, sortOrder, page, deleted, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectSystemUserGDSAccessRights_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;
		}

		//Get SystemUserGDSAccessRights
		public List<SystemUserGDSAccessRight> GetSystemUserGDSAccessRights(int gdsAccessTypeId)
		{
			return db.SystemUserGDSAccessRights.ToList();
		}

		//Get one SystemUserGDSAccessRight
		public SystemUserGDSAccessRight GetSystemUserGDSAccessRight(int gdsAccessTypeId)
		{
			return db.SystemUserGDSAccessRights.SingleOrDefault(c => c.SystemUserGDSAccessRightId == gdsAccessTypeId);
		}

		//Add SystemUserGDSAccessRight
		public void Add(SystemUserGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertSystemUserGDSAccessRight_v1(
				gdsAccessTypeVM.SystemUserGDSAccessRight.GDSSignOnID,
				gdsAccessTypeVM.SystemUserGDSAccessRight.GDSCode,
				gdsAccessTypeVM.SystemUserGDSAccessRight.PseudoCityOrOfficeId,
				gdsAccessTypeVM.SystemUserGDSAccessRight.TAGTIDCertificate,
				gdsAccessTypeVM.SystemUserGDSAccessRight.GDSAccessTypeId,
				gdsAccessTypeVM.SystemUserGDSAccessRight.RequestId,
				gdsAccessTypeVM.SystemUserGDSAccessRight.EnabledDate,
				gdsAccessTypeVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemark,
				gdsAccessTypeVM.SystemUserGDSAccessRight.SystemUserGuid,
				adminUserGuid
			);
		}

		//Edit SystemUserGDSAccessRight
		public void Update(SystemUserGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateSystemUserGDSAccessRight_v1(
				gdsAccessTypeVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightId,
				gdsAccessTypeVM.SystemUserGDSAccessRight.GDSSignOnID,
				gdsAccessTypeVM.SystemUserGDSAccessRight.GDSCode,
				gdsAccessTypeVM.SystemUserGDSAccessRight.PseudoCityOrOfficeId,
				gdsAccessTypeVM.SystemUserGDSAccessRight.TAGTIDCertificate,
				gdsAccessTypeVM.SystemUserGDSAccessRight.GDSAccessTypeId,
				gdsAccessTypeVM.SystemUserGDSAccessRight.RequestId,
				gdsAccessTypeVM.SystemUserGDSAccessRight.EnabledDate,
				gdsAccessTypeVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemark,
				gdsAccessTypeVM.SystemUserGDSAccessRight.SystemUserGuid,
				adminUserGuid
			);
		}

		//UpdateDeletedStatus SystemUserGDSAccessRight
		public void UpdateDeletedStatus(SystemUserGDSAccessRightVM gdsAccessTypeVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateSystemUserGDSAccessRightDeletedStatus_v1(
				gdsAccessTypeVM.SystemUserGDSAccessRight.SystemUserGDSAccessRightId,
				gdsAccessTypeVM.SystemUserGDSAccessRight.DeletedFlag,
				adminUserGuid,
				gdsAccessTypeVM.SystemUserGDSAccessRight.VersionNumber
			);
		}

		public void EditForDisplay(SystemUserGDSAccessRight gdsAccessRight)
		{
			//System User
			SystemUser systemUser = new SystemUser();
			SystemUserRepository systemUserRepository = new SystemUserRepository();
			systemUser = systemUserRepository.GetUserBySystemUserGuid(gdsAccessRight.SystemUserGuid);
			if (systemUser != null)
			{
				gdsAccessRight.SystemUser = systemUser;
			}
		}
		
		//Export Items to CSV
		public byte[] Export(string id)
		{
			StringBuilder sb = new StringBuilder();

			//Add Headers
			List<string> headers = new List<string>();

			headers.Add("SystemUserGuid");
			headers.Add("SystemUserLoginIdentifier");
			headers.Add("UserProfileIdentifier");
			headers.Add("FirstName");
			headers.Add("LastName");
			headers.Add("Location");
			headers.Add("IsActiveFlag ");
			headers.Add("CubaBookingAllowanceIndicator");
			headers.Add("Military and Government User");
			headers.Add("GDSName");
			headers.Add("Home PCC/OfficeID");
			headers.Add("GDSSignOnID");
			headers.Add("GDSAccessTypeName");
			headers.Add("TA/GTID/Certificate");
			headers.Add("EnabledDate");
			headers.Add("InternalRemarks");
			headers.Add("DeletedFlag");
			headers.Add("DeletedDateTime");
			headers.Add("CreationTimeStamp");
			headers.Add("CreationUserIdentifier");
			headers.Add("LastUpdateTimeStamp");
			headers.Add("LastUpdateUserIdenfier");

			sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

			//Add Items
			List<SystemUserGDSAccessRight> gdsAccessRights = db.SystemUserGDSAccessRights.Where(x => x.SystemUserGuid == id).ToList();
			foreach (SystemUserGDSAccessRight item in gdsAccessRights)
			{

				string date_format = "MM/dd/yy HH:mm";

				EditForDisplay(item);

				//SystemUserGDSAccessRightInternalRemarks
				StringBuilder remarksList = new StringBuilder();
				foreach (SystemUserGDSAccessRightInternalRemark systemUserGDSAccessRightInternalRemark in item.SystemUserGDSAccessRightInternalRemarks)
				{
					remarksList.AppendFormat("{0} - {1}; ", 
						systemUserGDSAccessRightInternalRemark.CreationTimestamp.Value.ToString("yyyy-MM-dd"), 
						systemUserGDSAccessRightInternalRemark.InternalRemark);
				}

				sb.AppendFormat(
					"{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}",
					item.SystemUserGuid != null && !string.IsNullOrEmpty(item.SystemUserGuid) ? item.SystemUserGuid : " ",
					item.SystemUserGuid != null && !string.IsNullOrEmpty(item.SystemUser.SystemUserLoginIdentifier) ? item.SystemUser.SystemUserLoginIdentifier : " ",
					item.SystemUserGuid != null && !string.IsNullOrEmpty(item.SystemUser.UserProfileIdentifier) ? item.SystemUser.UserProfileIdentifier : " ",
					item.SystemUserGuid != null && !string.IsNullOrEmpty(item.SystemUser.FirstName) ? item.SystemUser.FirstName : " ",
					item.SystemUserGuid != null && !string.IsNullOrEmpty(item.SystemUser.LastName) ? item.SystemUser.LastName : " ",
					item.SystemUserGuid != null && !string.IsNullOrEmpty(item.SystemUser.LocationName) ? item.SystemUser.LocationName : " ",
					item.SystemUser.IsActiveFlag == true ? "True" : "False",
					item.SystemUser.CubaBookingAllowanceIndicator == true ? "True" : "False",
					item.SystemUser.MilitaryAndGovernmentUserFlag == true ? "True" : "False",
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