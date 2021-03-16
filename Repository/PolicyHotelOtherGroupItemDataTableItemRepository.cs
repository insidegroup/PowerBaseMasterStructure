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
	public class PolicyHotelOtherGroupItemDataTableItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
		public DataTable GetPolicyHotelOtherGroupItemDataTableItems(int id, int policyGroupId, string filter, string sortField, int sortOrder, int page, ref PolicyHotelOtherGroupItemDataTableItemsVM policyHotelOtherGroupItemDataTableItemsVM)
		{
			//query db
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			DataSet policyHotelOtherGroupItemDataTableItems = new DataSet();

			string connectionStringName = Settings.getConnectionStringName();       

			using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				 using(SqlCommand cmd = new SqlCommand("spDesktopDataAdmin_SelectPolicyHotelOtherGroupItemDataTableRows_v1", conn))
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
					adapter.Fill(policyHotelOtherGroupItemDataTableItems);
				 }		
			}

			if (policyHotelOtherGroupItemDataTableItems.Tables[1] != null)
			{
				DataTable dt = policyHotelOtherGroupItemDataTableItems.Tables[1];

				int totalRecords = (from DataRow dr in dt.Rows select (int)dr["Record Count"]).FirstOrDefault();
				int pageSize = 16;

				policyHotelOtherGroupItemDataTableItemsVM.PageIndex = page;
				policyHotelOtherGroupItemDataTableItemsVM.PageSize = pageSize;
				policyHotelOtherGroupItemDataTableItemsVM.TotalCount = totalRecords;
				policyHotelOtherGroupItemDataTableItemsVM.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            }

			return policyHotelOtherGroupItemDataTableItems.Tables[0];
		}

		//Get items
		public List<PolicyHotelOtherGroupItemDataTableItem> GetPolicyHotelOtherGroupItemDataTableItems(int policyGroupId, int policyOtherGroupHeaderId)
		{
			
			List<PolicyHotelOtherGroupItemDataTableItem> policyHotelOtherGroupItemDataTableItems = new List<PolicyHotelOtherGroupItemDataTableItem>();
			
			PolicyHotelOtherGroupItem policyHotelOtherGroupItem = new PolicyHotelOtherGroupItem();
			PolicyHotelOtherGroupItemRepository policyHotelOtherGroupItemRepository = new PolicyHotelOtherGroupItemRepository();
			policyHotelOtherGroupItem = policyHotelOtherGroupItemRepository.GetPolicyHotelOtherGroupItem(policyGroupId,policyOtherGroupHeaderId);

			//Get Columns
			List<PolicyOtherGroupHeaderColumnName> policyOtherGroupHeaderColumnNames = new List<PolicyOtherGroupHeaderColumnName>();
			PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
			policyOtherGroupHeaderColumnNames = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnNames(policyOtherGroupHeaderId);

			if (policyOtherGroupHeaderColumnNames != null)
			{
				foreach(PolicyOtherGroupHeaderColumnName item in policyOtherGroupHeaderColumnNames) {

					PolicyHotelOtherGroupItemDataTableItem policyHotelOtherGroupItemDataTableItem = new PolicyHotelOtherGroupItemDataTableItem()
					{
						PolicyOtherGroupHeaderColumnNameId = item.PolicyOtherGroupHeaderColumnNameId,
						PolicyOtherGroupHeaderColumnName = item
					};
					policyHotelOtherGroupItemDataTableItems.Add(policyHotelOtherGroupItemDataTableItem);
				}
			}

			return policyHotelOtherGroupItemDataTableItems;
		}

		public List<PolicyHotelOtherGroupItemDataTableItem> GetPolicyHotelOtherGroupItemDataTableItems(int policyOtherGroupHeaderId)
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
				.Select(x => new PolicyHotelOtherGroupItemDataTableItem
				{
					PolicyOtherGroupHeaderColumnNameId = x.policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId,
					PolicyOtherGroupHeaderColumnName = x.policyOtherGroupHeaderColumnName
				})
				.ToList();

			 return result;
		}

		//Get one Item
		public PolicyHotelOtherGroupItemDataTableItem GetPolicyHotelOtherGroupItemDataTableItem(int policyHotelOtherGroupItemDataTableItemId)
		{
			return db.PolicyHotelOtherGroupItemDataTableItems.SingleOrDefault(c => c.PolicyHotelOtherGroupItemDataTableItemId == policyHotelOtherGroupItemDataTableItemId);
		}

		//Add
		public void Add(PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlDocument doc = ConvertPolicyHotelOtherGroupItemDataTableItemsToXML(policyHotelOtherGroupItemDataTableItemVM);

			db.spDesktopDataAdmin_InsertPolicyHotelOtherGroupItemDataTableItem_v1(
				policyHotelOtherGroupItemDataTableItemVM.PolicyGroup.PolicyGroupId,
				policyHotelOtherGroupItemDataTableItemVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit
		public void Edit(PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			
			XmlDocument doc = ConvertPolicyHotelOtherGroupItemDataTableItemsToXML(policyHotelOtherGroupItemDataTableItemVM);
			
			db.spDesktopDataAdmin_UpdatePolicyHotelOtherGroupItemDataTableItem_v1(
				policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableRow.PolicyHotelOtherGroupItemDataTableRowId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid,
				policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableRow.VersionNumber
			);

		}

		//Delete
		public void Delete(PolicyHotelOtherGroupItemDataTableRow policyHotelOtherGroupItemDataTableRow)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeletePolicyHotelOtherGroupItemDataTableItem_v1(
				policyHotelOtherGroupItemDataTableRow.PolicyHotelOtherGroupItemDataTableRowId,
				adminUserGuid,
				policyHotelOtherGroupItemDataTableRow.VersionNumber
				);
		}
		
		//Loop through PolicyHotelOtherGroupItemDataTableItems into XML
		public XmlDocument ConvertPolicyHotelOtherGroupItemDataTableItemsToXML(PolicyHotelOtherGroupItemDataTableItemVM policyHotelOtherGroupItemDataTableItemVM)
		{
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("PolicyHotelOtherGroupItemDataTableItems");
			doc.AppendChild(root);

			if (policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableItems != null)
			{
				if (policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableItems.Count > 0)
				{
					XmlElement xmlPolicyHotelOtherGroupItemDataTableItems = doc.CreateElement("PolicyHotelOtherGroupItemDataTableItems");
					foreach (PolicyHotelOtherGroupItemDataTableItem item in policyHotelOtherGroupItemDataTableItemVM.PolicyHotelOtherGroupItemDataTableItems)
					{
						if (item != null && !string.IsNullOrEmpty(item.TableDataItem))
						{
							XmlElement xmlPolicyHotelOtherGroupItemDataTableItem = doc.CreateElement("PolicyHotelOtherGroupItemDataTableItem");

							XmlElement xmlPolicyHotelOtherGroupItemDataTableItem_TableDataItem = doc.CreateElement("TableDataItem");
							xmlPolicyHotelOtherGroupItemDataTableItem_TableDataItem.InnerText = item.TableDataItem;
							xmlPolicyHotelOtherGroupItemDataTableItem.AppendChild(xmlPolicyHotelOtherGroupItemDataTableItem_TableDataItem);

							XmlElement xmlPolicyHotelOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId = doc.CreateElement("PolicyOtherGroupHeaderColumnNameId");
							xmlPolicyHotelOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId.InnerText = item.PolicyOtherGroupHeaderColumnNameId.ToString();
							xmlPolicyHotelOtherGroupItemDataTableItem.AppendChild(xmlPolicyHotelOtherGroupItemDataTableItem_PolicyOtherGroupHeaderColumnNameId);

							xmlPolicyHotelOtherGroupItemDataTableItems.AppendChild(xmlPolicyHotelOtherGroupItemDataTableItem);
						}
					}
					root.AppendChild(xmlPolicyHotelOtherGroupItemDataTableItems);
				}
			}
			return doc;
		}
    }
}
