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
	public class PolicyOtherGroupItemDataTableItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public DataTable GetPolicyOtherGroupItemDataTableItems(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page, ref PolicyOtherGroupItemDataTableItemsVM policyOtherGroupItemDataTableItemsVM)
		{
			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			DataSet policyOtherGroupItemDataTableItems = new DataSet();

			string connectionStringName = Settings.getConnectionStringName();       

			using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				 using(SqlCommand cmd = new SqlCommand("spDesktopDataAdmin_SelectPolicyOtherGroupItemDataTableRows_v1", conn))
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
					adapter.Fill(policyOtherGroupItemDataTableItems);
				 }		
			}

			if (policyOtherGroupItemDataTableItems.Tables[1] != null)
			{
				DataTable dt = policyOtherGroupItemDataTableItems.Tables[1];

				int totalRecords = (from DataRow dr in dt.Rows select (int)dr["Record Count"]).FirstOrDefault();
				int pageSize = 16;

				policyOtherGroupItemDataTableItemsVM.PageIndex = page;
				policyOtherGroupItemDataTableItemsVM.PageSize = pageSize;
				policyOtherGroupItemDataTableItemsVM.TotalCount = totalRecords;
				policyOtherGroupItemDataTableItemsVM.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            }

			return policyOtherGroupItemDataTableItems.Tables[0];
		}

		//Get items
		public List<PolicyOtherGroupItemDataTableItem> GetPolicyOtherGroupItemDataTableItems(int policyGroupId, int policyOtherGroupHeaderId)
		{
			
			List<PolicyOtherGroupItemDataTableItem> policyOtherGroupItemDataTableItems = new List<PolicyOtherGroupItemDataTableItem>();
			
			PolicyOtherGroupItem policyOtherGroupItem = new PolicyOtherGroupItem();
			PolicyOtherGroupItemRepository policyOtherGroupItemRepository = new PolicyOtherGroupItemRepository();
			policyOtherGroupItem = policyOtherGroupItemRepository.GetPolicyOtherGroupItem(policyGroupId,policyOtherGroupHeaderId);

			//Get Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnNames(policyOtherGroupHeaderId);

			if (policyOtherGroupHeaderColumnNames != null)
			{
				foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {

					PolicyOtherGroupItemDataTableItem policyOtherGroupItemDataTableItem = new PolicyOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyOtherGroupItemDataTableItems.Add(policyOtherGroupItemDataTableItem);
				}
			}

			return policyOtherGroupItemDataTableItems;
		}

		public List<PolicyOtherGroupItemDataTableItem> GetPolicyOtherGroupItemDataTableItems(int policyOtherGroupHeaderId)
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
				.Select(x => new PolicyOtherGroupItemDataTableItem {
					PolicyOtherGroupHeaderColumnNameId = x.policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
					PolicyOtherGroupHeaderColumnName = x.policyOtherGroupHeaderColumnName
				})
				.ToList();

			 return result;
		}

		//Get one Item
		public PolicyOtherGroupItemDataTableItem GetPolicyOtherGroupItemDataTableItem(int policyOtherGroupItemDataTableItemId)
		{
			return db.PolicyOtherGroupItemDataTableItems.SingleOrDefault(c => c.PolicyOtherGroupItemDataTableItemId == policyOtherGroupItemDataTableItemId);
		}

		//Add
		public void Add(PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument doc = ConvertPolicyOtherGroupItemDataTableItemsToXML(policyOtherGroupItemDataTableItemVM);

			db.spDesktopDataAdmin_InsertPolicyOtherGroupItemDataTableItem_v1(
				policyOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId,
				policyOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit
		public void Edit(PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			XmlDocument doc = ConvertPolicyOtherGroupItemDataTableItemsToXML(policyOtherGroupItemDataTableItemVM);
			
			db.spDesktopDataAdmin_UpdatePolicyOtherGroupItemDataTableItem_v1(
				policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableRow.PolicyOtherGroupItemDataTableRowId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid,
				policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableRow.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyOtherGroupItemDataTableRow policyOtherGroupItemDataTableRow)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOtherGroupItemDataTableItem_v1(
				policyOtherGroupItemDataTableRow.PolicyOtherGroupItemDataTableRowId,
				adminUserGuid,
				policyOtherGroupItemDataTableRow.VersionNumber
				);
		}
		
		//Loop through PolicyOtherGroupItemDataTableItems into XML
		public XmlDocument ConvertPolicyOtherGroupItemDataTableItemsToXML(PolicyOtherGroupItemDataTableItemVM policyOtherGroupItemDataTableItemVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicyOtherGroupItemDataTableItems");
			doc.AppendChild(root);

			if (policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableItems != null)
			{
				if (policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableItems.Count > 0)
				{
					XmlElement xmlPolicyOtherGroupItemDataTableItems = doc.CreateElement("PolicyOtherGroupItemDataTableItems");
					foreach (PolicyOtherGroupItemDataTableItem item in policyOtherGroupItemDataTableItemVM.PolicyOtherGroupItemDataTableItems)
					{
						if (item != null && !string.IsNullOrEmpty(item.TableDataItem))
						{
							XmlElement xmlPolicyOtherGroupItemDataTableItem = doc.CreateElement("PolicyOtherGroupItemDataTableItem");

							XmlElement xmlPolicyOtherGroupItemDataTableItem_TableDataItem = doc.CreateElement("TableDataItem");
							xmlPolicyOtherGroupItemDataTableItem_TableDataItem.InnerText = item.TableDataItem;
							xmlPolicyOtherGroupItemDataTableItem.AppendChild(xmlPolicyOtherGroupItemDataTableItem_TableDataItem);

							XmlElement xmlPolicyOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
							xmlPolicyOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId.InnerText = item.PolicyOtherGroupHeaderColumnNameId.ToString();
							xmlPolicyOtherGroupItemDataTableItem.AppendChild(xmlPolicyOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId);

							xmlPolicyOtherGroupItemDataTableItems.AppendChild(xmlPolicyOtherGroupItemDataTableItem);
						}
					}
					root.AppendChild(xmlPolicyOtherGroupItemDataTableItems);
				}
			}
			return doc;
		}
    }
}
