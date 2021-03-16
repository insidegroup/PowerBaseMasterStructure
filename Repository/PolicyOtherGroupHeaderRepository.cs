using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyOtherGroupHeaderRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of GDS Responses
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaders_v1Result> PagePolicyOtherGroupHeaders(int page, string filter, string sortField, int sortOrder, ref bool canEditOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaders_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
				canEditOrder = (bool)result.First().CanEditOrder;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaders_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one PolicyOtherGroupHeader
		public PolicyOtherGroupHeader GetPolicyOtherGroupHeader(int policyOtherGroupHeaderId)
		{
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			
			policyOtherGroupHeader = db.PolicyOtherGroupHeaders.Where(c => c.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId).FirstOrDefault();

			if (policyOtherGroupHeader != null)
			{
				//Labels
				PolicyOtherGroupHeaderLabel policyOtherGroupHeaderLabel = new PolicyOtherGroupHeaderLabel();
				policyOtherGroupHeaderLabel = db.PolicyOtherGroupHeaderLabels.Where(x => x.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId).FirstOrDefault();
				if (policyOtherGroupHeaderLabel != null)
				{
					policyOtherGroupHeader.Label = policyOtherGroupHeaderLabel.Label;
					policyOtherGroupHeader.LabelLanguageCode = "en-gb";
				}

				//Table Name
				PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
				policyOtherGroupHeaderTableName = db.PolicyOtherGroupHeaderTableNames.Where(x => x.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId).SingleOrDefault();
				if (policyOtherGroupHeaderTableName != null)
				{
					policyOtherGroupHeader.TableName = policyOtherGroupHeaderTableName.TableName;
					policyOtherGroupHeader.TableNameLanguageCode = "en-gb";
				}
			}

			return policyOtherGroupHeader; 
		}

		//Add PolicyOtherGroupHeader
		public void Add(PolicyOtherGroupHeader policyOtherGroupHeader)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPolicyOtherGroupHeader_v1(
				policyOtherGroupHeader.PolicyOtherGroupHeaderServiceTypeId,
				policyOtherGroupHeader.ProductId,
				policyOtherGroupHeader.SubProductId,
				policyOtherGroupHeader.Label,
				policyOtherGroupHeader.LabelLanguageCode,
				policyOtherGroupHeader.TableDefinitionsAttachedFlag,
				policyOtherGroupHeader.TableName,
				policyOtherGroupHeader.TableNameLanguageCode,
                policyOtherGroupHeader.RestrictAccessToAdminFlag,
                policyOtherGroupHeader.DisplayTopFlag,
                policyOtherGroupHeader.DisplayBottomFlag,
                policyOtherGroupHeader.DisplayRestrictedTranslationInPowerLibraryFlag,
                policyOtherGroupHeader.OnlineSensitiveDataFlag,
                adminUserGuid
			);
		}

		//Edit PolicyOtherGroupHeader
		public void Edit(PolicyOtherGroupHeader policyOtherGroupHeader)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePolicyOtherGroupHeader_v1(
				policyOtherGroupHeader.PolicyOtherGroupHeaderId,
				policyOtherGroupHeader.PolicyOtherGroupHeaderServiceTypeId,
				policyOtherGroupHeader.ProductId,
				policyOtherGroupHeader.SubProductId,
				policyOtherGroupHeader.Label,
				policyOtherGroupHeader.LabelLanguageCode,
				policyOtherGroupHeader.TableDefinitionsAttachedFlag,
				policyOtherGroupHeader.TableName,
				policyOtherGroupHeader.TableNameLanguageCode,
                policyOtherGroupHeader.RestrictAccessToAdminFlag,
                policyOtherGroupHeader.DisplayTopFlag,
                policyOtherGroupHeader.DisplayBottomFlag,
                policyOtherGroupHeader.DisplayRestrictedTranslationInPowerLibraryFlag,
                policyOtherGroupHeader.OnlineSensitiveDataFlag,
                adminUserGuid,
				policyOtherGroupHeader.VersionNumber
			);
		}

		public void Delete(PolicyOtherGroupHeader policyOtherGroupHeader)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOtherGroupHeader_v1(
				policyOtherGroupHeader.PolicyOtherGroupHeaderId,
				adminUserGuid,
				policyOtherGroupHeader.VersionNumber
			);
		}

		//List of PolicyGroupCountries For Sequencing
		public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderSequences_v1Result> GetPolicyOtherGroupHeaderSequences(int policyOtherGroupHeaderServiceTypeId, int productId, int subProductId, int page)
		{
			//query db
			int pageSize = 50;
			var result = db.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderSequences_v1(page, pageSize, policyOtherGroupHeaderServiceTypeId, productId, subProductId).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//add paging information and return1
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyOtherGroupHeaderSequences_v1Result>(result, page, totalRecords, pageSize);
			return paginatedView;
		}

		//Update Sequences of PolicyOtherGroupHeader
		public void UpdatePolicyOtherGroupHeaderSequences(System.Xml.Linq.XElement xmlElement)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			db.spDesktopDataAdmin_UpdatePolicyOtherGroupHeaderSequences_v1(xmlElement, adminUserGuid);

		}

	}
}