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
	public class PolicyOnlineOtherGroupItemDataTableItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public DataTable GetPolicyOnlineOtherGroupItemDataTableItems(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page, ref PolicyOnlineOtherGroupItemDataTableItemsVM PolicyOnlineOtherGroupItemDataTableItemsVM)
		{
			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			DataSet PolicyOnlineOtherGroupItemDataTableItems = new DataSet();

			string connectionStringName = Settings.getConnectionStringName();       

			using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				 using(SqlCommand cmd = new SqlCommand("spDesktopDataAdmin_SelectPolicyOnlineOtherGroupItemDataTableRows_v1", conn))
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
					adapter.Fill(PolicyOnlineOtherGroupItemDataTableItems);
				 }		
			}

			if (PolicyOnlineOtherGroupItemDataTableItems.Tables[1] != null)
			{
				DataTable dt = PolicyOnlineOtherGroupItemDataTableItems.Tables[1];

				int totalRecords = (from DataRow dr in dt.Rows select (int)dr["Record Count"]).FirstOrDefault();
				int pageSize = 16;

				PolicyOnlineOtherGroupItemDataTableItemsVM.PageIndex = page;
				PolicyOnlineOtherGroupItemDataTableItemsVM.PageSize = pageSize;
				PolicyOnlineOtherGroupItemDataTableItemsVM.TotalCount = totalRecords;
				PolicyOnlineOtherGroupItemDataTableItemsVM.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            }

			return PolicyOnlineOtherGroupItemDataTableItems.Tables[0];
		}

		//Get items
		public List<PolicyOnlineOtherGroupItemDataTableItem> GetPolicyOnlineOtherGroupItemDataTableItems(int policyGroupId, int policyOtherGroupHeaderId)
		{
			
			List<PolicyOnlineOtherGroupItemDataTableItem> PolicyOnlineOtherGroupItemDataTableItems = new List<PolicyOnlineOtherGroupItemDataTableItem>();
			
			PolicyOnlineOtherGroupItem PolicyOnlineOtherGroupItem = new PolicyOnlineOtherGroupItem();
			PolicyOnlineOtherGroupItemRepository PolicyOnlineOtherGroupItemRepository = new PolicyOnlineOtherGroupItemRepository();
			PolicyOnlineOtherGroupItem = PolicyOnlineOtherGroupItemRepository.GetPolicyOnlineOtherGroupItem(policyGroupId,policyOtherGroupHeaderId);

			//Get Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnNames(policyOtherGroupHeaderId);

			if (policyOtherGroupHeaderColumnNames != null)
			{
				foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {

					PolicyOnlineOtherGroupItemDataTableItem PolicyOnlineOtherGroupItemDataTableItem = new PolicyOnlineOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					PolicyOnlineOtherGroupItemDataTableItems.Add(PolicyOnlineOtherGroupItemDataTableItem);
				}
			}

			return PolicyOnlineOtherGroupItemDataTableItems;
		}

		public List<PolicyOnlineOtherGroupItemDataTableItem> GetPolicyOnlineOtherGroupItemDataTableItems(int policyOtherGroupHeaderId)
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
				.Select(x => new PolicyOnlineOtherGroupItemDataTableItem {
					PolicyOtherGroupHeaderColumnNameId = x.policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
					PolicyOtherGroupHeaderColumnName = x.policyOtherGroupHeaderColumnName
				})
				.ToList();

			 return result;
		}

		//Get one Item
		public PolicyOnlineOtherGroupItemDataTableItem GetPolicyOnlineOtherGroupItemDataTableItem(int PolicyOnlineOtherGroupItemDataTableItemId)
		{
			return db.PolicyOnlineOtherGroupItemDataTableItems.SingleOrDefault(c => c.PolicyOnlineOtherGroupItemDataTableItemId == PolicyOnlineOtherGroupItemDataTableItemId);
		}

		//Add
		public void Add(PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument doc = ConvertPolicyOnlineOtherGroupItemDataTableItemsToXML(PolicyOnlineOtherGroupItemDataTableItemVM);

			db.spDesktopDataAdmin_InsertPolicyOnlineOtherGroupItemDataTableItem_v1(
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId,
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit
		public void Edit(PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			XmlDocument doc = ConvertPolicyOnlineOtherGroupItemDataTableItemsToXML(PolicyOnlineOtherGroupItemDataTableItemVM);
			
			db.spDesktopDataAdmin_UpdatePolicyOnlineOtherGroupItemDataTableItem_v1(
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableRow.PolicyOnlineOtherGroupItemDataTableRowId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid,
				PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableRow.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyOnlineOtherGroupItemDataTableRow PolicyOnlineOtherGroupItemDataTableRow)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyOnlineOtherGroupItemDataTableItem_v1(
				PolicyOnlineOtherGroupItemDataTableRow.PolicyOnlineOtherGroupItemDataTableRowId,
				adminUserGuid,
				PolicyOnlineOtherGroupItemDataTableRow.VersionNumber
				);
		}
		
		//Loop through PolicyOnlineOtherGroupItemDataTableItems into XML
		public XmlDocument ConvertPolicyOnlineOtherGroupItemDataTableItemsToXML(PolicyOnlineOtherGroupItemDataTableItemVM PolicyOnlineOtherGroupItemDataTableItemVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicyOnlineOtherGroupItemDataTableItems");
			doc.AppendChild(root);

			if (PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableItems != null)
			{
				if (PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableItems.Count > 0)
				{
					XmlElement xmlPolicyOnlineOtherGroupItemDataTableItems = doc.CreateElement("PolicyOnlineOtherGroupItemDataTableItems");
					foreach (PolicyOnlineOtherGroupItemDataTableItem item in PolicyOnlineOtherGroupItemDataTableItemVM.PolicyOnlineOtherGroupItemDataTableItems)
					{
						if (item != null && !string.IsNullOrEmpty(item.TableDataItem))
						{
							XmlElement xmlPolicyOnlineOtherGroupItemDataTableItem = doc.CreateElement("PolicyOnlineOtherGroupItemDataTableItem");

							XmlElement xmlPolicyOnlineOtherGroupItemDataTableItem_TableDataItem = doc.CreateElement("TableDataItem");
							xmlPolicyOnlineOtherGroupItemDataTableItem_TableDataItem.InnerText = item.TableDataItem;
							xmlPolicyOnlineOtherGroupItemDataTableItem.AppendChild(xmlPolicyOnlineOtherGroupItemDataTableItem_TableDataItem);

							XmlElement xmlPolicyOnlineOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
							xmlPolicyOnlineOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId.InnerText = item.PolicyOtherGroupHeaderColumnNameId.ToString();
							xmlPolicyOnlineOtherGroupItemDataTableItem.AppendChild(xmlPolicyOnlineOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId);

							xmlPolicyOnlineOtherGroupItemDataTableItems.AppendChild(xmlPolicyOnlineOtherGroupItemDataTableItem);
						}
					}
					root.AppendChild(xmlPolicyOnlineOtherGroupItemDataTableItems);
				}
			}
			return doc;
		}
    }
}
