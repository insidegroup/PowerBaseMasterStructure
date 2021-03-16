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
	public class PolicyAirOtherGroupItemDataTableItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public DataTable GetPolicyAirOtherGroupItemDataTableItems(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page, ref PolicyAirOtherGroupItemDataTableItemsVM policyAirOtherGroupItemDataTableItemsVM)
		{
			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			DataSet policyAirOtherGroupItemDataTableItems = new DataSet();

			string connectionStringName = Settings.getConnectionStringName();       

			using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				 using(SqlCommand cmd = new SqlCommand("spDesktopDataAdmin_SelectPolicyAirOtherGroupItemDataTableRows_v1", conn))
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
					adapter.Fill(policyAirOtherGroupItemDataTableItems);
				 }		
			}

			if (policyAirOtherGroupItemDataTableItems.Tables[1] != null)
			{
				DataTable dt = policyAirOtherGroupItemDataTableItems.Tables[1];

				int totalRecords = (from DataRow dr in dt.Rows select (int)dr["Record Count"]).FirstOrDefault();
				int pageSize = 16;

				policyAirOtherGroupItemDataTableItemsVM.PageIndex = page;
				policyAirOtherGroupItemDataTableItemsVM.PageSize = pageSize;
				policyAirOtherGroupItemDataTableItemsVM.TotalCount = totalRecords;
				policyAirOtherGroupItemDataTableItemsVM.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            }

			return policyAirOtherGroupItemDataTableItems.Tables[0];
		}

		//Get items
		public List<PolicyAirOtherGroupItemDataTableItem> GetPolicyAirOtherGroupItemDataTableItems(int policyGroupId, int policyOtherGroupHeaderId)
		{
			
			List<PolicyAirOtherGroupItemDataTableItem> policyAirOtherGroupItemDataTableItems = new List<PolicyAirOtherGroupItemDataTableItem>();
			
			PolicyAirOtherGroupItem policyAirOtherGroupItem = new PolicyAirOtherGroupItem();
			PolicyAirOtherGroupItemRepository policyAirOtherGroupItemRepository = new PolicyAirOtherGroupItemRepository();
			policyAirOtherGroupItem = policyAirOtherGroupItemRepository.GetPolicyAirOtherGroupItem(policyGroupId,policyOtherGroupHeaderId);

			//Get Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnNames(policyOtherGroupHeaderId);

			if (policyOtherGroupHeaderColumnNames != null)
			{
				foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {

					PolicyAirOtherGroupItemDataTableItem policyAirOtherGroupItemDataTableItem = new PolicyAirOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyAirOtherGroupItemDataTableItems.Add(policyAirOtherGroupItemDataTableItem);
				}
			}

			return policyAirOtherGroupItemDataTableItems;
		}

		public List<PolicyAirOtherGroupItemDataTableItem> GetPolicyAirOtherGroupItemDataTableItems(int policyOtherGroupHeaderId)
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
				.Select(x => new PolicyAirOtherGroupItemDataTableItem {
					PolicyOtherGroupHeaderColumnNameId = x.policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
					PolicyOtherGroupHeaderColumnName = x.policyOtherGroupHeaderColumnName
				})
				.ToList();

			 return result;
		}

		//Get one Item
		public PolicyAirOtherGroupItemDataTableItem GetPolicyAirOtherGroupItemDataTableItem(int policyAirOtherGroupItemDataTableItemId)
		{
			return db.PolicyAirOtherGroupItemDataTableItems.SingleOrDefault(c => c.PolicyAirOtherGroupItemDataTableItemId == policyAirOtherGroupItemDataTableItemId);
		}

		//Add
		public void Add(PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument doc = ConvertPolicyAirOtherGroupItemDataTableItemsToXML(policyAirOtherGroupItemDataTableItemVM);

			db.spDesktopDataAdmin_InsertPolicyAirOtherGroupItemDataTableItem_v1(
				policyAirOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId,
				policyAirOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit
		public void Edit(PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			XmlDocument doc = ConvertPolicyAirOtherGroupItemDataTableItemsToXML(policyAirOtherGroupItemDataTableItemVM);
			
			db.spDesktopDataAdmin_UpdatePolicyAirOtherGroupItemDataTableItem_v1(
				policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableRow.PolicyAirOtherGroupItemDataTableRowId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid,
				policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableRow.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyAirOtherGroupItemDataTableRow policyAirOtherGroupItemDataTableRow)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyAirOtherGroupItemDataTableItem_v1(
				policyAirOtherGroupItemDataTableRow.PolicyAirOtherGroupItemDataTableRowId,
				adminUserGuid,
				policyAirOtherGroupItemDataTableRow.VersionNumber
				);
		}
		
		//Loop through PolicyAirOtherGroupItemDataTableItems into XML
		public XmlDocument ConvertPolicyAirOtherGroupItemDataTableItemsToXML(PolicyAirOtherGroupItemDataTableItemVM policyAirOtherGroupItemDataTableItemVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicyAirOtherGroupItemDataTableItems");
			doc.AppendChild(root);

			if (policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableItems != null)
			{
				if (policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableItems.Count > 0)
				{
					XmlElement xmlPolicyAirOtherGroupItemDataTableItems = doc.CreateElement("PolicyAirOtherGroupItemDataTableItems");
					foreach (PolicyAirOtherGroupItemDataTableItem item in policyAirOtherGroupItemDataTableItemVM.PolicyAirOtherGroupItemDataTableItems)
					{
						if (item != null && !string.IsNullOrEmpty(item.TableDataItem))
						{
							XmlElement xmlPolicyAirOtherGroupItemDataTableItem = doc.CreateElement("PolicyAirOtherGroupItemDataTableItem");

							XmlElement xmlPolicyAirOtherGroupItemDataTableItem_TableDataItem = doc.CreateElement("TableDataItem");
							xmlPolicyAirOtherGroupItemDataTableItem_TableDataItem.InnerText = item.TableDataItem;
							xmlPolicyAirOtherGroupItemDataTableItem.AppendChild(xmlPolicyAirOtherGroupItemDataTableItem_TableDataItem);

							XmlElement xmlPolicyAirOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
							xmlPolicyAirOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId.InnerText = item.PolicyOtherGroupHeaderColumnNameId.ToString();
							xmlPolicyAirOtherGroupItemDataTableItem.AppendChild(xmlPolicyAirOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId);

							xmlPolicyAirOtherGroupItemDataTableItems.AppendChild(xmlPolicyAirOtherGroupItemDataTableItem);
						}
					}
					root.AppendChild(xmlPolicyAirOtherGroupItemDataTableItems);
				}
			}
			return doc;
		}
    }
}
