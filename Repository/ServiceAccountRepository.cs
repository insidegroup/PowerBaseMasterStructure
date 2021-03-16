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
    public class ServiceAccountRepository
    {
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        LogRepository logRepository = new LogRepository();

        //List of ServiceAccounts
		public CWTPaginatedList<spDesktopDataAdmin_SelectServiceAccounts_v1Result> PageServiceAccounts(
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
			var result = db.spDesktopDataAdmin_SelectServiceAccounts_v1(
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
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectServiceAccounts_v1Result>(result, page, totalRecords);
			return paginatedView;
		}

		public ServiceAccount GetServiceAccount(string serviceAccountId)
		{
			return db.ServiceAccounts.SingleOrDefault(c => c.ServiceAccountId == serviceAccountId);
		}

		public Dictionary<string, string> GetThirdPartyUserFilters()
		{
			Dictionary<string, string> filters = new Dictionary<string, string>();
			filters.Add("FirstName", "First Name");
			filters.Add("GDSSignOnID", "GDS Sign On ID");
			filters.Add("HomePCCOfficeID", "Home PCC/Office ID");
			filters.Add("ID", "ID");
			filters.Add("LastName", "Last Name");
			filters.Add("ServiceAccountID", "Service Account ID");
			filters.Add("ServiceAccountName", "Service Account Name");

			return filters;
		}

        //Add Data From Linked Tables for Display
        public void EditForDisplay(ServiceAccount serviceAccount)
        {
            //Flags
            serviceAccount.CubaBookingAllowanceIndicatorNonNullable = (serviceAccount.CubaBookingAllowanceIndicator.HasValue) ? serviceAccount.CubaBookingAllowanceIndicator.Value : false;
            serviceAccount.MilitaryAndGovernmentUserFlagNonNullable = (serviceAccount.MilitaryAndGovernmentUserFlag.HasValue) ? serviceAccount.MilitaryAndGovernmentUserFlag.Value : false;
        }
        
        //Create ServiceAccount
        public void Add(ServiceAccountVM serviceAccountVM)
		{
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            string computerName = logRepository.GetComputerName();

            //Generate a unique account Id
			serviceAccountVM.ServiceAccount.ServiceAccountId = CWTStringHelpers.GenerateDateBasedId();

            db.spDesktopDataAdmin_InsertServiceAccount_v1(
                serviceAccountVM.ServiceAccount.ServiceAccountId,
                serviceAccountVM.ServiceAccount.ServiceAccountName,
                serviceAccountVM.ServiceAccount.LastName,
                serviceAccountVM.ServiceAccount.FirstName,
                serviceAccountVM.ServiceAccount.Email,
                serviceAccountVM.ServiceAccount.IsActiveFlag,
                serviceAccountVM.ServiceAccount.CubaBookingAllowanceIndicator,
                serviceAccountVM.ServiceAccount.MilitaryAndGovernmentUserFlag,
                serviceAccountVM.ServiceAccount.RoboticUserFlag,
                serviceAccountVM.ServiceAccount.CWTManager,
                serviceAccountVM.ServiceAccount.ThirdPartyUserType,
                serviceAccountVM.ServiceAccount.InternalRemark,
                adminUserGuid,
                Settings.ApplicationName(),
                Settings.ApplicationVersion(),
                computerName
            );
        }

        //Edit ServiceAccount
        public void Update(ServiceAccountVM serviceAccountVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            string computerName = logRepository.GetComputerName();

            db.spDesktopDataAdmin_UpdateServiceAccount_v1(
                serviceAccountVM.ServiceAccount.ServiceAccountId,
                serviceAccountVM.ServiceAccount.ServiceAccountName,
                serviceAccountVM.ServiceAccount.LastName,
                serviceAccountVM.ServiceAccount.FirstName,
                serviceAccountVM.ServiceAccount.Email,
                serviceAccountVM.ServiceAccount.IsActiveFlag,
                serviceAccountVM.ServiceAccount.CubaBookingAllowanceIndicatorNonNullable,
                serviceAccountVM.ServiceAccount.MilitaryAndGovernmentUserFlagNonNullable,
                serviceAccountVM.ServiceAccount.RoboticUserFlag,
                serviceAccountVM.ServiceAccount.CWTManager,
                serviceAccountVM.ServiceAccount.InternalRemark,
                adminUserGuid,
                serviceAccountVM.ServiceAccount.VersionNumber,
                Settings.ApplicationName(),
                Settings.ApplicationVersion(),
                computerName
            );
        }

        //UpdateDeletedStatus ServiceAccount
        public void UpdateDeletedStatus(ServiceAccountVM serviceAccountVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateServiceAccountDeletedStatus_v1(
                serviceAccountVM.ServiceAccount.ServiceAccountId,
                serviceAccountVM.ServiceAccount.DeletedFlag,
                adminUserGuid,
                serviceAccountVM.ServiceAccount.VersionNumber
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
	
            headers.Add("Service Account ID");
            headers.Add("Service Account Name");
            headers.Add("Last Name");
            headers.Add("First Name");
            headers.Add("Email");
            headers.Add("Is Active Flag");
            headers.Add("Cuba Booking Allowance Indicator");
            headers.Add("Military and Government User");
            headers.Add("Robotic User Flag");
            headers.Add("CWT Manager");
            headers.Add("User Type");
            headers.Add("Internal Remarks");
            headers.Add("DeletedFlag");
            headers.Add("Deleted Date Time");
            headers.Add("CreationTimeStamp");
            headers.Add("CreationUserIdentifier");
            headers.Add("LastUpdateTimeStamp");
            headers.Add("LastUpdateUserIdentifier");

            sb.AppendLine(String.Join(",", headers.Select(x => x.ToString()).ToArray()));

            //Add Items
            List<ServiceAccount> serviceAccounts = GetServiceAccountsExports(
                                                       filterField_1,
                                                       filterValue_1,
                                                       filterField_2,
                                                       filterValue_2,
                                                       filterField_3,
                filterValue_3
            );

            foreach (ServiceAccount item in serviceAccounts)
            {

                string date_format = "MM/dd/yy HH:mm";

                //EditForDisplay(item);

                //ThirdPartyUserInternalRemarks
                StringBuilder remarksList = new StringBuilder();
                ServiceAccount serviceAccount = GetServiceAccount(item.ServiceAccountId);
                foreach (ServiceAccountInternalRemark serviceAccountInternalRemark in serviceAccount.ServiceAccountInternalRemarks)
                {
					remarksList.AppendFormat("{0} - {1}; ", serviceAccountInternalRemark.CreationTimestamp.Value.ToString(date_format), serviceAccountInternalRemark.InternalRemark);
                }

                sb.AppendFormat(
                    "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}",
                    !string.IsNullOrEmpty(item.ServiceAccountId) ? item.ServiceAccountId : " ",
                    !string.IsNullOrEmpty(item.ServiceAccountName) ? item.ServiceAccountName : " ",
                    !string.IsNullOrEmpty(item.LastName) ? item.LastName : " ",
                    !string.IsNullOrEmpty(item.FirstName) ? item.FirstName : " ",
                    !string.IsNullOrEmpty(item.Email) ? item.Email : " ",
                    item.IsActiveFlag == true ? "True" : "False",
                    item.CubaBookingAllowanceIndicator == true ? "True" : "False",
                    item.MilitaryAndGovernmentUserFlag == true ? "True" : "False",
                    item.RoboticUserFlag == true ? "True" : "False",
                    !string.IsNullOrEmpty(item.CWTManager) ? item.CWTManager : " ",
                    !string.IsNullOrEmpty(item.ThirdPartyUserType) ? item.ThirdPartyUserType : " ",
                    remarksList != null && !string.IsNullOrEmpty(remarksList.ToString()) ? string.Format("\"{0}\"", remarksList) : " ",
                    item.DeletedFlag == true ? "True" : "False",
                    item.DeletedDateTime.HasValue ? item.DeletedDateTime.Value.ToString(date_format) : " ",
                    item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString(date_format) : " ",
                    !string.IsNullOrEmpty(item.CreationUserIdentifier) ? item.CreationUserIdentifier : " ",
                    item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString(date_format) : " ",
                    !string.IsNullOrEmpty(item.LastUpdateUserIdentifier) ? item.LastUpdateUserIdentifier : " "
                );

                sb.Append(Environment.NewLine);
            }

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public List<ServiceAccount> GetServiceAccountsExports(string filterField_1 = "", string filterValue_1 = "", string filterField_2 = "", string filterValue_2 = "", string filterField_3 = "", string filterValue_3 = "")
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            var result = from n in db.spDesktopDataAdmin_SelectServiceAccountExport_v1(filterField_1, filterValue_1, filterField_2, filterValue_2, filterField_3, filterValue_3, adminUserGuid)
                         select
                             new ServiceAccount
                             {
                                 ServiceAccountId = n.ServiceAccountId ?? "" ?? "",
                                 ServiceAccountName = n.ServiceAccountName ?? "",
                                 LastName = n.LastName ?? "",
                                 FirstName = n.FirstName ?? "",
                                 Email = n.Email ?? "",
                                 IsActiveFlag = n.IsActiveFlag ?? false,
                                 CubaBookingAllowanceIndicator = n.CubaBookingAllowanceIndicator ?? false,
                                 MilitaryAndGovernmentUserFlag = n.MilitaryAndGovernmentUserFlag ?? false,
                                 RoboticUserFlag = n.RoboticUserFlag ?? false,
                                 CWTManager = n.CWTManager ?? "",
                                 ThirdPartyUserType = n.ThirdPartyUserType,
                                 DeletedFlag = n.DeletedFlag ?? false,
                                 DeletedDateTime = n.DeletedDateTime,
                                 CreationTimestamp = n.CreationTimestamp,
                                 CreationUserIdentifier = n.CreationUserIdentifier,
                                 LastUpdateTimestamp = n.LastUpdateTimestamp,
                                 LastUpdateUserIdentifier = n.LastUpdateUserIdentifier
                             };

            return result.ToList();
        }

		//Get PseudoCityOrOfficeIds by GDSCode based on ServiceAccount and logged in user
		public List<ValidPseudoCityOrOfficeIdJSON> GetServiceAccountPseudoCityOrOfficeIdsByGDSCode(string gdsCode, string serviceAccountId)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			return (from n in db.fnDesktopDataAdmin_SelectServiceAccountValidPseudoCityOrOfficeIds_v1(gdsCode, serviceAccountId, adminUserGuid).OrderBy(x => x.PseudoCityOrOfficeId)
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