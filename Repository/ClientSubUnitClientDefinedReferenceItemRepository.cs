using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.ViewModels;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class ClientSubUnitClientDefinedReferenceItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List of ClientDefinedReferenceItems for a ClientSubUnit - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitClientDefinedReferenceItems_v1Result> PageClientSubUnitClientDefinedReferenceItems(string filter, string id, int page, string sortField, int sortOrder)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientSubUnitClientDefinedReferenceItems_v1(filter, id, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitClientDefinedReferenceItems_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Get a Single Item
		public ClientSubUnitClientDefinedReferenceItem GetClientSubUnitClientDefinedReferenceItem(int clientSubUnitClientDefinedReferenceItemId)
		{
			return db.ClientSubUnitClientDefinedReferenceItems.SingleOrDefault(c => c.ClientSubUnitClientDefinedReferenceItemId == clientSubUnitClientDefinedReferenceItemId);
		}

		//Add Item
		public void Add(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlElement clientDefinedReferenceItemProducts = GetClientDefinedReferenceItemProducts(clientDefinedReferenceItem);
			XmlElement clientDefinedReferenceItemContexts = GetClientDefinedReferenceItemContexts(clientDefinedReferenceItem);
			
			db.spDesktopDataAdmin_InsertClientSubUnitClientDefinedReferenceItem_v1(
				clientDefinedReferenceItem.ClientSubUnitGuid,
				clientDefinedReferenceItem.DisplayName,
				clientDefinedReferenceItem.DisplayNameAlias,
				clientDefinedReferenceItem.EntryFormat,
				clientDefinedReferenceItem.MandatoryFlagNullable,
				clientDefinedReferenceItem.MinLength,
				clientDefinedReferenceItem.MaxLength,
				clientDefinedReferenceItem.TableDrivenFlagNullable,
				clientDefinedReferenceItem.OnlineDefaultValue,
				System.Xml.Linq.XElement.Parse(clientDefinedReferenceItemProducts.OuterXml), //Products
				System.Xml.Linq.XElement.Parse(clientDefinedReferenceItemContexts.OuterXml), //Contexts
				adminUserGuid
			);
		}

		//Update Item
		public void Update(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlElement clientDefinedReferenceItemProducts = GetClientDefinedReferenceItemProducts(clientDefinedReferenceItem);
			XmlElement clientDefinedReferenceItemContexts = GetClientDefinedReferenceItemContexts(clientDefinedReferenceItem);

			db.spDesktopDataAdmin_UpdateClientDefinedReferenceItem_v1(
				clientDefinedReferenceItem.ClientDefinedReferenceItemId,
				clientDefinedReferenceItem.ClientSubUnitGuid,
				clientDefinedReferenceItem.DisplayName,
				clientDefinedReferenceItem.DisplayNameAlias,
				clientDefinedReferenceItem.EntryFormat,
				clientDefinedReferenceItem.MandatoryFlagNullable,
				clientDefinedReferenceItem.MinLength,
				clientDefinedReferenceItem.MaxLength,
				clientDefinedReferenceItem.TableDrivenFlagNullable,
				clientDefinedReferenceItem.SourceSystemCode,
				clientDefinedReferenceItem.ClientAccountNumber,
				clientDefinedReferenceItem.OnlineDefaultValue,
				System.Xml.Linq.XElement.Parse(clientDefinedReferenceItemProducts.OuterXml), //Products
				System.Xml.Linq.XElement.Parse(clientDefinedReferenceItemContexts.OuterXml), //Contexts
				clientDefinedReferenceItem.VersionNumber,
				adminUserGuid
			);
		}

		//Delete Item
		public void Delete(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientSubUnitClientDefinedReferenceItem_v1(
				clientDefinedReferenceItem.ClientDefinedReferenceItemId,
				adminUserGuid,
				clientDefinedReferenceItem.VersionNumber
			);
		}

		//Products to XML
		private XmlElement GetClientDefinedReferenceItemProducts(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			XmlDocument docProducts = new XmlDocument();
			XmlDeclaration decProducts = docProducts.CreateXmlDeclaration("1.0", null, null);
			docProducts.AppendChild(decProducts);
			XmlElement rootProducts = docProducts.CreateElement("ClientDefinedReferenceItemProducts");
			docProducts.AppendChild(rootProducts);

			if (!string.IsNullOrEmpty(clientDefinedReferenceItem.ClientDefinedReferenceItemProductIds))
			{
				string[] clientDefinedReferenceItemProductIds = clientDefinedReferenceItem.ClientDefinedReferenceItemProductIds.Split(',');

				XmlElement xmlClientDefinedReferenceItemProducts = docProducts.CreateElement("ClientDefinedReferenceItemProducts");

				foreach (string clientDefinedReferenceItemProductIdValue in clientDefinedReferenceItemProductIds)
				{
					if (!string.IsNullOrEmpty(clientDefinedReferenceItemProductIdValue))
					{
						int clientDefinedReferenceItemContextId;
						if (Int32.TryParse(clientDefinedReferenceItemProductIdValue, out clientDefinedReferenceItemContextId))
						{
							XmlElement xmlClientDefinedReferenceItemProduct = docProducts.CreateElement("ClientDefinedReferenceItemProduct");
							xmlClientDefinedReferenceItemProduct.InnerText = clientDefinedReferenceItemContextId.ToString();
							xmlClientDefinedReferenceItemProducts.AppendChild(xmlClientDefinedReferenceItemProduct);
						}
					}
				}
				rootProducts.AppendChild(xmlClientDefinedReferenceItemProducts);
			}

			return rootProducts;
		}

		//Contexts to XML
		private XmlElement GetClientDefinedReferenceItemContexts(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			XmlDocument docContexts = new XmlDocument();
			XmlDeclaration decContexts = docContexts.CreateXmlDeclaration("1.0", null, null);
			docContexts.AppendChild(decContexts);
			XmlElement rootContexts = docContexts.CreateElement("ClientDefinedReferenceItemContexts");
			docContexts.AppendChild(rootContexts);

			if (!string.IsNullOrEmpty(clientDefinedReferenceItem.ClientDefinedReferenceItemContextIds))
			{
				string[] clientDefinedReferenceItemContextIds = clientDefinedReferenceItem.ClientDefinedReferenceItemContextIds.Split(',');

				XmlElement xmlClientDefinedReferenceItemContexts = docContexts.CreateElement("ClientDefinedReferenceItemContexts");

				foreach (string clientDefinedReferenceItemContextIdValue in clientDefinedReferenceItemContextIds)
				{
					if (!string.IsNullOrEmpty(clientDefinedReferenceItemContextIdValue))
					{
						int clientDefinedReferenceItemContextId;
						if (Int32.TryParse(clientDefinedReferenceItemContextIdValue, out clientDefinedReferenceItemContextId))
						{
							XmlElement xmlClientDefinedReferenceItemContext = docContexts.CreateElement("ClientDefinedReferenceItemContext");
							xmlClientDefinedReferenceItemContext.InnerText = clientDefinedReferenceItemContextId.ToString();
							xmlClientDefinedReferenceItemContexts.AppendChild(xmlClientDefinedReferenceItemContext);
						}
					}
				}
				rootContexts.AppendChild(xmlClientDefinedReferenceItemContexts);
			}

			return rootContexts;
		}
    }
}