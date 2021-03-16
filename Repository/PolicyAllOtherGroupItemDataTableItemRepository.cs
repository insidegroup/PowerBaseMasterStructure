using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.ViewModels;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CWTDesktopDatabase.Repository
{
	public class PolicyAllOtherGroupItemDataTableItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public DataTable GetPolicyAllOtherGroupItemDataTableItems(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page, ref PolicyAllOtherGroupItemDataTableItemsVM policyAllOtherGroupItemDataTableItemsVM)
		{
			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			DataSet policyAllOtherGroupItemDataTableItems = new DataSet();

			string connectionStringName = Settings.getConnectionStringName();       

			using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				 using(SqlCommand cmd = new SqlCommand("spDesktopDataAdmin_SelectPolicyAllOtherGroupItemDataTableRows_v1", conn))
				 {
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@PolicyOtherGroupHeaderId", id);
					cmd.Parameters.AddWithValue("@PolicyGroupId", policyGroupId);
					cmd.Parameters.AddWithValue("@Filter", filter);
					cmd.Parameters.AddWithValue("@SortField", sortField);
					cmd.Parameters.AddWithValue("@SortOrder", sortOrder);
					cmd.Parameters.AddWithValue("@PageNumber", page);
					cmd.Parameters.AddWithValue("@AdminUserGuid", adminUserGuid);
					conn.Open();

					SqlDataAdapter adapter = new SqlDataAdapter(cmd);
					adapter.Fill(policyAllOtherGroupItemDataTableItems);
				 }		
			}

			if (policyAllOtherGroupItemDataTableItems.Tables[1] != null)
			{
				DataTable dt = policyAllOtherGroupItemDataTableItems.Tables[1];

				int totalRecords = (from DataRow dr in dt.Rows select (int)dr["Record Count"]).FirstOrDefault();
				int pageSize = 16;

				policyAllOtherGroupItemDataTableItemsVM.PageIndex = page;
				policyAllOtherGroupItemDataTableItemsVM.PageSize = pageSize;
				policyAllOtherGroupItemDataTableItemsVM.TotalCount = totalRecords;
				policyAllOtherGroupItemDataTableItemsVM.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            }

			return policyAllOtherGroupItemDataTableItems.Tables[0];
		}

		//Get items
		public List<PolicyAllOtherGroupItemDataTableItem> GetPolicyAllOtherGroupItemDataTableItems(int policyGroupId, int policyOtherGroupHeaderId)
		{
			
			List<PolicyAllOtherGroupItemDataTableItem> policyAllOtherGroupItemDataTableItems = new List<PolicyAllOtherGroupItemDataTableItem>();
			
			PolicyAllOtherGroupItem policyAllOtherGroupItem = new PolicyAllOtherGroupItem();
			PolicyAllOtherGroupItemRepository policyAllOtherGroupItemRepository = new PolicyAllOtherGroupItemRepository();
			policyAllOtherGroupItem = policyAllOtherGroupItemRepository.GetPolicyAllOtherGroupItem(policyGroupId,policyOtherGroupHeaderId);

			//Get Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnNames(policyOtherGroupHeaderId);

			if (policyOtherGroupHeaderColumnNames != null)
			{
				foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {

					PolicyAllOtherGroupItemDataTableItem policyAllOtherGroupItemDataTableItem = new PolicyAllOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyAllOtherGroupItemDataTableItems.Add(policyAllOtherGroupItemDataTableItem);
				}
			}

			return policyAllOtherGroupItemDataTableItems;
		}

		public List<PolicyAllOtherGroupItemDataTableItem> GetPolicyAllOtherGroupItemDataTableItems(int policyOtherGroupHeaderId)
		{
			var result =
				(
					from policyOtherGroupHeaderColumnName in db.PolicyOtherGroupHeaderColumnNames
					join policyOtherGroupHeaderTableNames in db.PolicyOtherGroupHeaderTableNames
						on policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderTableNameId equals
						policyOtherGroupHeaderTableNames.PolicyOtherGroupHeaderTableNameId
					where policyOtherGroupHeaderTableNames.PolicyOtherGroupHeaderId == policyOtherGroupHeaderId
					select new
					{
						policyOtherGroupHeaderColumnName
					}
				)
				.AsEnumerable()
				.Select(x => new PolicyAllOtherGroupItemDataTableItem {
					PolicyOtherGroupHeaderColumnNameId = x.policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
					PolicyOtherGroupHeaderColumnName = x.policyOtherGroupHeaderColumnName
				})
				.ToList();

			 return result;
		}

		//Get one Item
		public PolicyAllOtherGroupItemDataTableItem GetPolicyAllOtherGroupItemDataTableItem(int policyAllOtherGroupItemDataTableItemId)
		{
			return db.PolicyAllOtherGroupItemDataTableItems.SingleOrDefault(c => c.PolicyAllOtherGroupItemDataTableItemId == policyAllOtherGroupItemDataTableItemId);
		}

		//Add
		public void Add(PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument doc = ConvertPolicyAllOtherGroupItemDataTableItemsToXML(policyAllOtherGroupItemDataTableItemVM);

			db.spDesktopDataAdmin_InsertPolicyAllOtherGroupItemDataTableItem_v1(
				policyAllOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId,
				policyAllOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit
		public void Edit(PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			XmlDocument doc = ConvertPolicyAllOtherGroupItemDataTableItemsToXML(policyAllOtherGroupItemDataTableItemVM);
			
			db.spDesktopDataAdmin_UpdatePolicyAllOtherGroupItemDataTableItem_v1(
				policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableRow.PolicyAllOtherGroupItemDataTableRowId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid,
				policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableRow.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyAllOtherGroupItemDataTableRow policyAllOtherGroupItemDataTableRow)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyAllOtherGroupItemDataTableItem_v1(
				policyAllOtherGroupItemDataTableRow.PolicyAllOtherGroupItemDataTableRowId,
				adminUserGuid,
				policyAllOtherGroupItemDataTableRow.VersionNumber
				);
		}
		
		//Loop through PolicyAllOtherGroupItemDataTableItems into XML
		public XmlDocument ConvertPolicyAllOtherGroupItemDataTableItemsToXML(PolicyAllOtherGroupItemDataTableItemVM policyAllOtherGroupItemDataTableItemVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicyAllOtherGroupItemDataTableItems");
			doc.AppendChild(root);

			if (policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableItems != null)
			{
				if (policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableItems.Count > 0)
				{
					XmlElement xmlPolicyAllOtherGroupItemDataTableItems = doc.CreateElement("PolicyAllOtherGroupItemDataTableItems");
					foreach (PolicyAllOtherGroupItemDataTableItem item in policyAllOtherGroupItemDataTableItemVM.PolicyAllOtherGroupItemDataTableItems)
					{
						if (item != null && !string.IsNullOrEmpty(item.TableDataItem))
						{
							XmlElement xmlPolicyAllOtherGroupItemDataTableItem = doc.CreateElement("PolicyAllOtherGroupItemDataTableItem");

							XmlElement xmlPolicyAllOtherGroupItemDataTableItem_TableDataItem = doc.CreateElement("TableDataItem");
							xmlPolicyAllOtherGroupItemDataTableItem_TableDataItem.InnerText = item.TableDataItem;
							xmlPolicyAllOtherGroupItemDataTableItem.AppendChild(xmlPolicyAllOtherGroupItemDataTableItem_TableDataItem);

							XmlElement xmlPolicyAllOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
							xmlPolicyAllOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId.InnerText = item.PolicyOtherGroupHeaderColumnNameId.ToString();
							xmlPolicyAllOtherGroupItemDataTableItem.AppendChild(xmlPolicyAllOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId);

							xmlPolicyAllOtherGroupItemDataTableItems.AppendChild(xmlPolicyAllOtherGroupItemDataTableItem);
						}
					}
					root.AppendChild(xmlPolicyAllOtherGroupItemDataTableItems);
				}
			}
			return doc;
		}
    }
}
