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
	public class PolicyCarOtherGroupItemDataTableItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public DataTable GetPolicyCarOtherGroupItemDataTableItems(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page, ref PolicyCarOtherGroupItemDataTableItemsVM policyCarOtherGroupItemDataTableItemsVM)
		{
			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			DataSet policyCarOtherGroupItemDataTableItems = new DataSet();

			string connectionStringName = Settings.getConnectionStringName();       

			using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				 using(SqlCommand cmd = new SqlCommand("spDesktopDataAdmin_SelectPolicyCarOtherGroupItemDataTableRows_v1", conn))
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
					adapter.Fill(policyCarOtherGroupItemDataTableItems);
				 }		
			}

			if (policyCarOtherGroupItemDataTableItems.Tables[1] != null)
			{
				DataTable dt = policyCarOtherGroupItemDataTableItems.Tables[1];

				int totalRecords = (from DataRow dr in dt.Rows select (int)dr["Record Count"]).FirstOrDefault();
				int pageSize = 16;

				policyCarOtherGroupItemDataTableItemsVM.PageIndex = page;
				policyCarOtherGroupItemDataTableItemsVM.PageSize = pageSize;
				policyCarOtherGroupItemDataTableItemsVM.TotalCount = totalRecords;
				policyCarOtherGroupItemDataTableItemsVM.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            }

			return policyCarOtherGroupItemDataTableItems.Tables[0];
		}

		//Get items
		public List<PolicyCarOtherGroupItemDataTableItem> GetPolicyCarOtherGroupItemDataTableItems(int policyGroupId, int policyOtherGroupHeaderId)
		{
			
			List<PolicyCarOtherGroupItemDataTableItem> policyCarOtherGroupItemDataTableItems = new List<PolicyCarOtherGroupItemDataTableItem>();
			
			PolicyCarOtherGroupItem policyCarOtherGroupItem = new PolicyCarOtherGroupItem();
			PolicyCarOtherGroupItemRepository policyCarOtherGroupItemRepository = new PolicyCarOtherGroupItemRepository();
			policyCarOtherGroupItem = policyCarOtherGroupItemRepository.GetPolicyCarOtherGroupItem(policyGroupId,policyOtherGroupHeaderId);

			//Get Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnNames(policyOtherGroupHeaderId);

			if (policyOtherGroupHeaderColumnNames != null)
			{
				foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {

					PolicyCarOtherGroupItemDataTableItem policyCarOtherGroupItemDataTableItem = new PolicyCarOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyCarOtherGroupItemDataTableItems.Add(policyCarOtherGroupItemDataTableItem);
				}
			}

			return policyCarOtherGroupItemDataTableItems;
		}

		public List<PolicyCarOtherGroupItemDataTableItem> GetPolicyCarOtherGroupItemDataTableItems(int policyOtherGroupHeaderId)
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
				.Select(x => new PolicyCarOtherGroupItemDataTableItem {
					PolicyOtherGroupHeaderColumnNameId = x.policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
					PolicyOtherGroupHeaderColumnName = x.policyOtherGroupHeaderColumnName
				})
				.ToList();

			 return result;
		}

		//Get one Item
		public PolicyCarOtherGroupItemDataTableItem GetPolicyCarOtherGroupItemDataTableItem(int policyCarOtherGroupItemDataTableItemId)
		{
			return db.PolicyCarOtherGroupItemDataTableItems.SingleOrDefault(c => c.PolicyCarOtherGroupItemDataTableItemId == policyCarOtherGroupItemDataTableItemId);
		}

		//Add
		public void Add(PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument doc = ConvertPolicyCarOtherGroupItemDataTableItemsToXML(policyCarOtherGroupItemDataTableItemVM);

			db.spDesktopDataAdmin_InsertPolicyCarOtherGroupItemDataTableItem_v1(
				policyCarOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId,
				policyCarOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit
		public void Edit(PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			XmlDocument doc = ConvertPolicyCarOtherGroupItemDataTableItemsToXML(policyCarOtherGroupItemDataTableItemVM);
			
			db.spDesktopDataAdmin_UpdatePolicyCarOtherGroupItemDataTableItem_v1(
				policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableRow.PolicyCarOtherGroupItemDataTableRowId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid,
				policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableRow.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyCarOtherGroupItemDataTableRow policyCarOtherGroupItemDataTableRow)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyCarOtherGroupItemDataTableItem_v1(
				policyCarOtherGroupItemDataTableRow.PolicyCarOtherGroupItemDataTableRowId,
				adminUserGuid,
				policyCarOtherGroupItemDataTableRow.VersionNumber
				);
		}
		
		//Loop through PolicyCarOtherGroupItemDataTableItems into XML
		public XmlDocument ConvertPolicyCarOtherGroupItemDataTableItemsToXML(PolicyCarOtherGroupItemDataTableItemVM policyCarOtherGroupItemDataTableItemVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicyCarOtherGroupItemDataTableItems");
			doc.AppendChild(root);

			if (policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableItems != null)
			{
				if (policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableItems.Count > 0)
				{
					XmlElement xmlPolicyCarOtherGroupItemDataTableItems = doc.CreateElement("PolicyCarOtherGroupItemDataTableItems");
					foreach (PolicyCarOtherGroupItemDataTableItem item in policyCarOtherGroupItemDataTableItemVM.PolicyCarOtherGroupItemDataTableItems)
					{
						if (item != null && !string.IsNullOrEmpty(item.TableDataItem))
						{
							XmlElement xmlPolicyCarOtherGroupItemDataTableItem = doc.CreateElement("PolicyCarOtherGroupItemDataTableItem");

							XmlElement xmlPolicyCarOtherGroupItemDataTableItem_TableDataItem = doc.CreateElement("TableDataItem");
							xmlPolicyCarOtherGroupItemDataTableItem_TableDataItem.InnerText = item.TableDataItem;
							xmlPolicyCarOtherGroupItemDataTableItem.AppendChild(xmlPolicyCarOtherGroupItemDataTableItem_TableDataItem);

							XmlElement xmlPolicyCarOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
							xmlPolicyCarOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId.InnerText = item.PolicyOtherGroupHeaderColumnNameId.ToString();
							xmlPolicyCarOtherGroupItemDataTableItem.AppendChild(xmlPolicyCarOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId);

							xmlPolicyCarOtherGroupItemDataTableItems.AppendChild(xmlPolicyCarOtherGroupItemDataTableItem);
						}
					}
					root.AppendChild(xmlPolicyCarOtherGroupItemDataTableItems);
				}
			}
			return doc;
		}
    }
}
