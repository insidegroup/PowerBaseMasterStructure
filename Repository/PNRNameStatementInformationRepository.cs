using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Xml;
using CWTDesktopDatabase.ViewModels;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
	public class PNRNameStatementInformationRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of PagePNRNameStatementInformation Items - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectPNRNameStatementInformationItems_v1Result> PagePNRNameStatementInformationItems(int page, string id, string sortField, int? sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectPNRNameStatementInformationItems_v1(id, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPNRNameStatementInformationItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get a Single Item
		public PNRNameStatementInformation GetPNRNameStatementInformation(string PNRNameStatementInformationId)
		{
			return db.PNRNameStatementInformations.SingleOrDefault(
				c => c.PNRNameStatementInformationId == PNRNameStatementInformationId
			);
		}

		//Get Delimeters
		public List<SelectListItem> GetPNRNameStatementInformationDelimiters()
		{
			var delimeters = new List<SelectListItem>();
			
			delimeters.Add(new SelectListItem { Value = " ", Text = "None (blank)" });
			delimeters.Add(new SelectListItem { Value = "*", Text = "Asterisk (*)" });
            delimeters.Add(new SelectListItem { Value = "/", Text = "Forward Slash (/)" });
            delimeters.Add(new SelectListItem { Value = "-", Text = "Dash (-)" });
            delimeters.Add(new SelectListItem { Value = ")", Text = "Close Parenthesis ())" });
            delimeters.Add(new SelectListItem { Value = "(ID", Text = "Open Parenthesis ((ID)" });

            return delimeters;
		}

		//Get ClientDefinedReferenceItems for a PNRNameStatementInformation item
		public List<SelectListItem> GetPNRNameStatementInformationStatementInformation(string clientSubUnitGuid)
		{
			var result = from n in db.ClientDefinedReferenceItems.Where(c => c.ClientSubUnitGuid == clientSubUnitGuid)
						 select new SelectListItem
						 {
							 Value = n.ClientDefinedReferenceItemId,
							 Text = n.DisplayName
						 };

			List<SelectListItem> list = result.ToList();
				
			list.Add(new SelectListItem() { Value = "External System ID", Text = "External System ID" });

			return list;

		}

		//Add to DB
		public void Add(PNRNameStatementInformationVM PNRNameStatementInformationVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertPNRNameStatementInformation_v1(
				PNRNameStatementInformationVM.PNRNameStatementInformation.ClientSubUnitGuid,
				PNRNameStatementInformationVM.PNRNameStatementInformation.GDSCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter1,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field1_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field1_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field1_DisplayName,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter2,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field2_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field2_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field2_DisplayName, 
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter3,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field3_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field3_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field3_DisplayName, 
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter4,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field4_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field4_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field4_DisplayName, 
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter5,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field5_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field5_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field5_DisplayName, 
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter6,
				adminUserGuid
			);
		}

		//Update in DB
		public void Update(PNRNameStatementInformationVM PNRNameStatementInformationVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdatePNRNameStatementInformation_v1(
				PNRNameStatementInformationVM.PNRNameStatementInformation.PNRNameStatementInformationId,
				PNRNameStatementInformationVM.PNRNameStatementInformation.ClientSubUnitGuid,
				PNRNameStatementInformationVM.PNRNameStatementInformation.GDSCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter1,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field1_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field1_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field1_DisplayName,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter2,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field2_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field2_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field2_DisplayName,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter3,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field3_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field3_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field3_DisplayName,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter4,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field4_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field4_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field4_DisplayName,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter5,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field5_PNRMappingTypeCode,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field5_ReferToRecordIdentifier,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Field5_DisplayName,
				PNRNameStatementInformationVM.PNRNameStatementInformation.Delimiter6,
				adminUserGuid,
				PNRNameStatementInformationVM.PNRNameStatementInformation.VersionNumber
			);
		}

		//Delete From DB
		public void Delete(PNRNameStatementInformation PNRNameStatementInformation)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePNRNameStatementInformation_v1(
				PNRNameStatementInformation.PNRNameStatementInformationId,
				adminUserGuid,
				PNRNameStatementInformation.VersionNumber
			);
		}
	}
}