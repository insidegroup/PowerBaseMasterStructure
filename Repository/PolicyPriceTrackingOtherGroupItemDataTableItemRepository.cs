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
	public class PolicyPriceTrackingOtherGroupItemDataTableItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public DataTable GetPolicyPriceTrackingOtherGroupItemDataTableItems(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page, ref PolicyPriceTrackingOtherGroupItemDataTableItemsVM PolicyPriceTrackingOtherGroupItemDataTableItemsVM)
		{
			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			DataSet PolicyPriceTrackingOtherGroupItemDataTableItems = new DataSet();

			string connectionStringName = Settings.getConnectionStringName();       

			using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				 using(SqlCommand cmd = new SqlCommand("spDesktopDataAdmin_SelectPolicyPriceTrackingOtherGroupItemDataTableRows_v1", conn))
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
					adapter.Fill(PolicyPriceTrackingOtherGroupItemDataTableItems);
				 }		
			}

			if (PolicyPriceTrackingOtherGroupItemDataTableItems.Tables[1] != null)
			{
				DataTable dt = PolicyPriceTrackingOtherGroupItemDataTableItems.Tables[1];

				int totalRecords = (from DataRow dr in dt.Rows select (int)dr["Record Count"]).FirstOrDefault();
				int pageSize = 16;

				PolicyPriceTrackingOtherGroupItemDataTableItemsVM.PageIndex = page;
				PolicyPriceTrackingOtherGroupItemDataTableItemsVM.PageSize = pageSize;
				PolicyPriceTrackingOtherGroupItemDataTableItemsVM.TotalCount = totalRecords;
				PolicyPriceTrackingOtherGroupItemDataTableItemsVM.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            }

			return PolicyPriceTrackingOtherGroupItemDataTableItems.Tables[0];
		}

		//Get items
		public List<PolicyPriceTrackingOtherGroupItemDataTableItem> GetPolicyPriceTrackingOtherGroupItemDataTableItems(int policyGroupId, int policyOtherGroupHeaderId)
		{
			
			List<PolicyPriceTrackingOtherGroupItemDataTableItem> PolicyPriceTrackingOtherGroupItemDataTableItems = new List<PolicyPriceTrackingOtherGroupItemDataTableItem>();
			
			PolicyPriceTrackingOtherGroupItem PolicyPriceTrackingOtherGroupItem = new PolicyPriceTrackingOtherGroupItem();
			PolicyPriceTrackingOtherGroupItemRepository PolicyPriceTrackingOtherGroupItemRepository = new PolicyPriceTrackingOtherGroupItemRepository();
			PolicyPriceTrackingOtherGroupItem = PolicyPriceTrackingOtherGroupItemRepository.GetPolicyPriceTrackingOtherGroupItem(policyGroupId,policyOtherGroupHeaderId);

			//Get Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnNames(policyOtherGroupHeaderId);

			if (policyOtherGroupHeaderColumnNames != null)
			{
				foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {

					PolicyPriceTrackingOtherGroupItemDataTableItem PolicyPriceTrackingOtherGroupItemDataTableItem = new PolicyPriceTrackingOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					PolicyPriceTrackingOtherGroupItemDataTableItems.Add(PolicyPriceTrackingOtherGroupItemDataTableItem);
				}
			}

			return PolicyPriceTrackingOtherGroupItemDataTableItems;
		}

		public List<PolicyPriceTrackingOtherGroupItemDataTableItem> GetPolicyPriceTrackingOtherGroupItemDataTableItems(int policyOtherGroupHeaderId)
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
				.Select(x => new PolicyPriceTrackingOtherGroupItemDataTableItem {
					PolicyOtherGroupHeaderColumnNameId = x.policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
					PolicyOtherGroupHeaderColumnName = x.policyOtherGroupHeaderColumnName
				})
				.ToList();

			 return result;
		}

		//Get one Item
		public PolicyPriceTrackingOtherGroupItemDataTableItem GetPolicyPriceTrackingOtherGroupItemDataTableItem(int PolicyPriceTrackingOtherGroupItemDataTableItemId)
		{
			return db.PolicyPriceTrackingOtherGroupItemDataTableItems.SingleOrDefault(c => c.PolicyPriceTrackingOtherGroupItemDataTableItemId == PolicyPriceTrackingOtherGroupItemDataTableItemId);
		}

		//Add
		public void Add(PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument doc = ConvertPolicyPriceTrackingOtherGroupItemDataTableItemsToXML(PolicyPriceTrackingOtherGroupItemDataTableItemVM);

			db.spDesktopDataAdmin_InsertPolicyPriceTrackingOtherGroupItemDataTableItem_v1(
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId,
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit
		public void Edit(PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			XmlDocument doc = ConvertPolicyPriceTrackingOtherGroupItemDataTableItemsToXML(PolicyPriceTrackingOtherGroupItemDataTableItemVM);
			
			db.spDesktopDataAdmin_UpdatePolicyPriceTrackingOtherGroupItemDataTableItem_v1(
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableRow.PolicyPriceTrackingOtherGroupItemDataTableRowId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid,
				PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableRow.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyPriceTrackingOtherGroupItemDataTableRow PolicyPriceTrackingOtherGroupItemDataTableRow)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyPriceTrackingOtherGroupItemDataTableItem_v1(
				PolicyPriceTrackingOtherGroupItemDataTableRow.PolicyPriceTrackingOtherGroupItemDataTableRowId,
				adminUserGuid,
				PolicyPriceTrackingOtherGroupItemDataTableRow.VersionNumber
				);
		}
		
		//Loop through PolicyPriceTrackingOtherGroupItemDataTableItems into XML
		public XmlDocument ConvertPolicyPriceTrackingOtherGroupItemDataTableItemsToXML(PolicyPriceTrackingOtherGroupItemDataTableItemVM PolicyPriceTrackingOtherGroupItemDataTableItemVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicyPriceTrackingOtherGroupItemDataTableItems");
			doc.AppendChild(root);

			if (PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableItems != null)
			{
				if (PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableItems.Count > 0)
				{
					XmlElement xmlPolicyPriceTrackingOtherGroupItemDataTableItems = doc.CreateElement("PolicyPriceTrackingOtherGroupItemDataTableItems");
					foreach (PolicyPriceTrackingOtherGroupItemDataTableItem item in PolicyPriceTrackingOtherGroupItemDataTableItemVM.PolicyPriceTrackingOtherGroupItemDataTableItems)
					{
						if (item != null && !string.IsNullOrEmpty(item.TableDataItem))
						{
							XmlElement xmlPolicyPriceTrackingOtherGroupItemDataTableItem = doc.CreateElement("PolicyPriceTrackingOtherGroupItemDataTableItem");

							XmlElement xmlPolicyPriceTrackingOtherGroupItemDataTableItem_TableDataItem = doc.CreateElement("TableDataItem");
							xmlPolicyPriceTrackingOtherGroupItemDataTableItem_TableDataItem.InnerText = item.TableDataItem;
							xmlPolicyPriceTrackingOtherGroupItemDataTableItem.AppendChild(xmlPolicyPriceTrackingOtherGroupItemDataTableItem_TableDataItem);

							XmlElement xmlPolicyPriceTrackingOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
							xmlPolicyPriceTrackingOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId.InnerText = item.PolicyOtherGroupHeaderColumnNameId.ToString();
							xmlPolicyPriceTrackingOtherGroupItemDataTableItem.AppendChild(xmlPolicyPriceTrackingOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId);

							xmlPolicyPriceTrackingOtherGroupItemDataTableItems.AppendChild(xmlPolicyPriceTrackingOtherGroupItemDataTableItem);
						}
					}
					root.AppendChild(xmlPolicyPriceTrackingOtherGroupItemDataTableItems);
				}
			}
			return doc;
		}
    }
}
