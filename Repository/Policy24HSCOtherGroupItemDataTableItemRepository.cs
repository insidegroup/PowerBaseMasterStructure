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
	public class Policy24HSCOtherGroupItemDataTableItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public DataTable GetPolicy24HSCOtherGroupItemDataTableItems(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page, ref Policy24HSCOtherGroupItemDataTableItemsVM policy24HSCOtherGroupItemDataTableItemsVM)
		{
			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			DataSet policy24HSCOtherGroupItemDataTableItems = new DataSet();

			string connectionStringName = Settings.getConnectionStringName();       

			using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				 using(SqlCommand cmd = new SqlCommand("spDesktopDataAdmin_SelectPolicy24HSCOtherGroupItemDataTableRows_v1", conn))
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
					adapter.Fill(policy24HSCOtherGroupItemDataTableItems);
				 }		
			}

			if (policy24HSCOtherGroupItemDataTableItems.Tables[1] != null)
			{
				DataTable dt = policy24HSCOtherGroupItemDataTableItems.Tables[1];

				int totalRecords = (from DataRow dr in dt.Rows select (int)dr["Record Count"]).FirstOrDefault();
				int pageSize = 16;

				policy24HSCOtherGroupItemDataTableItemsVM.PageIndex = page;
				policy24HSCOtherGroupItemDataTableItemsVM.PageSize = pageSize;
				policy24HSCOtherGroupItemDataTableItemsVM.TotalCount = totalRecords;
				policy24HSCOtherGroupItemDataTableItemsVM.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            }

			return policy24HSCOtherGroupItemDataTableItems.Tables[0];
		}

		//Get items
		public List<Policy24HSCOtherGroupItemDataTableItem> GetPolicy24HSCOtherGroupItemDataTableItems(int policyGroupId, int policyOtherGroupHeaderId)
		{
			
			List<Policy24HSCOtherGroupItemDataTableItem> policy24HSCOtherGroupItemDataTableItems = new List<Policy24HSCOtherGroupItemDataTableItem>();
			
			Policy24HSCOtherGroupItem policy24HSCOtherGroupItem = new Policy24HSCOtherGroupItem();
			Policy24HSCOtherGroupItemRepository policy24HSCOtherGroupItemRepository = new Policy24HSCOtherGroupItemRepository();
			policy24HSCOtherGroupItem = policy24HSCOtherGroupItemRepository.GetPolicy24HSCOtherGroupItem(policyGroupId,policyOtherGroupHeaderId);

			//Get Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnNames(policyOtherGroupHeaderId);

			if (policyOtherGroupHeaderColumnNames != null)
			{
				foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {

					Policy24HSCOtherGroupItemDataTableItem policy24HSCOtherGroupItemDataTableItem = new Policy24HSCOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policy24HSCOtherGroupItemDataTableItems.Add(policy24HSCOtherGroupItemDataTableItem);
				}
			}

			return policy24HSCOtherGroupItemDataTableItems;
		}

		public List<Policy24HSCOtherGroupItemDataTableItem> GetPolicy24HSCOtherGroupItemDataTableItems(int policyOtherGroupHeaderId)
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
				.Select(x => new Policy24HSCOtherGroupItemDataTableItem {
					PolicyOtherGroupHeaderColumnNameId = x.policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
					PolicyOtherGroupHeaderColumnName = x.policyOtherGroupHeaderColumnName
				})
				.ToList();

			 return result;
		}

		//Get one Item
		public Policy24HSCOtherGroupItemDataTableItem GetPolicy24HSCOtherGroupItemDataTableItem(int policy24HSCOtherGroupItemDataTableItemId)
		{
			return db.Policy24HSCOtherGroupItemDataTableItems.SingleOrDefault(c => c.Policy24HSCOtherGroupItemDataTableItemId == policy24HSCOtherGroupItemDataTableItemId);
		}

		//Add
		public void Add(Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument doc = ConvertPolicy24HSCOtherGroupItemDataTableItemsToXML(policy24HSCOtherGroupItemDataTableItemVM);

			db.spDesktopDataAdmin_InsertPolicy24HSCOtherGroupItemDataTableItem_v1(
				policy24HSCOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId,
				policy24HSCOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit
		public void Edit(Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			XmlDocument doc = ConvertPolicy24HSCOtherGroupItemDataTableItemsToXML(policy24HSCOtherGroupItemDataTableItemVM);
			
			db.spDesktopDataAdmin_UpdatePolicy24HSCOtherGroupItemDataTableItem_v1(
				policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableRow.Policy24HSCOtherGroupItemDataTableRowId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid,
				policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableRow.VersionNumber
			);

		}

		//Delete
		public void Delete(Policy24HSCOtherGroupItemDataTableRow policy24HSCOtherGroupItemDataTableRow)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicy24HSCOtherGroupItemDataTableItem_v1(
				policy24HSCOtherGroupItemDataTableRow.Policy24HSCOtherGroupItemDataTableRowId,
				adminUserGuid,
				policy24HSCOtherGroupItemDataTableRow.VersionNumber
				);
		}
		
		//Loop through Policy24HSCOtherGroupItemDataTableItems into XML
		public XmlDocument ConvertPolicy24HSCOtherGroupItemDataTableItemsToXML(Policy24HSCOtherGroupItemDataTableItemVM policy24HSCOtherGroupItemDataTableItemVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("Policy24HSCOtherGroupItemDataTableItems");
			doc.AppendChild(root);

			if (policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableItems != null)
			{
				if (policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableItems.Count > 0)
				{
					XmlElement xmlPolicy24HSCOtherGroupItemDataTableItems = doc.CreateElement("Policy24HSCOtherGroupItemDataTableItems");
					foreach (Policy24HSCOtherGroupItemDataTableItem item in policy24HSCOtherGroupItemDataTableItemVM.Policy24HSCOtherGroupItemDataTableItems)
					{
						if (item != null && !string.IsNullOrEmpty(item.TableDataItem))
						{
							XmlElement xmlPolicy24HSCOtherGroupItemDataTableItem = doc.CreateElement("Policy24HSCOtherGroupItemDataTableItem");

							XmlElement xmlPolicy24HSCOtherGroupItemDataTableItem_TableDataItem = doc.CreateElement("TableDataItem");
							xmlPolicy24HSCOtherGroupItemDataTableItem_TableDataItem.InnerText = item.TableDataItem;
							xmlPolicy24HSCOtherGroupItemDataTableItem.AppendChild(xmlPolicy24HSCOtherGroupItemDataTableItem_TableDataItem);

							XmlElement xmlPolicy24HSCOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
							xmlPolicy24HSCOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId.InnerText = item.PolicyOtherGroupHeaderColumnNameId.ToString();
							xmlPolicy24HSCOtherGroupItemDataTableItem.AppendChild(xmlPolicy24HSCOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId);

							xmlPolicy24HSCOtherGroupItemDataTableItems.AppendChild(xmlPolicy24HSCOtherGroupItemDataTableItem);
						}
					}
					root.AppendChild(xmlPolicy24HSCOtherGroupItemDataTableItems);
				}
			}
			return doc;
		}
    }
}
