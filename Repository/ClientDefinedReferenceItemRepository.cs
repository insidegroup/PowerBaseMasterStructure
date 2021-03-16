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
    public class ClientDefinedReferenceItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//List of ClientDefinedReferenceItems for a ClientSubUnit - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItems_v1Result> PageClientDefinedReferenceItems(string filter, string id, string ssc, string can, int page, string sortField, int sortOrder)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientDefinedReferenceItems_v1(filter, id, ssc, can, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItems_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Update Sequences of ClientDefinedReferenceItems
		public void UpdateOptionalFieldItemSequences(System.Xml.Linq.XElement xmlElement)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			db.spDesktopDataAdmin_UpdateClientDefinedReferenceItemSequences_v1(xmlElement, adminUserGuid);

		}

		//List of ClientDefinedReferenceItems Sequence for a ClientSubUnit - Paged
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItemSequences_v1Result> PageClientDefinedReferenceItemSequences(int page, string id, string ssc, string can)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			int pageSize = 50; 
			var result = db.spDesktopDataAdmin_SelectClientDefinedReferenceItemSequences_v1(page, pageSize, id, ssc, can).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientDefinedReferenceItemSequences_v1Result>(result, page, Convert.ToInt32(totalRecords));
			return paginatedView;

		}

		//Add Data From Linked Tables for Display
		public void EditForDisplay(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			//MandatoryFlag
			clientDefinedReferenceItem.MandatoryFlagNullable = (clientDefinedReferenceItem.MandatoryFlag == true) ? true : false;

			//TableDrivenFlag
			clientDefinedReferenceItem.TableDrivenFlagNullable = (clientDefinedReferenceItem.TableDrivenFlag == true) ? true : false;

			//ClientDefinedReferenceItemProducts
			if (clientDefinedReferenceItem.ClientDefinedReferenceItemProducts != null)
			{
				clientDefinedReferenceItem.ClientDefinedReferenceItemProductIds = string.Join(",", clientDefinedReferenceItem.ClientDefinedReferenceItemProducts.OrderBy(x => x.ProductId).Select(p => p.ProductId.ToString()));
			}

			//ClientDefinedReferenceItemContexts
			if (clientDefinedReferenceItem.ClientDefinedReferenceItemContexts != null)
			{
				clientDefinedReferenceItem.ClientDefinedReferenceItemContextIds = string.Join(",", clientDefinedReferenceItem.ClientDefinedReferenceItemContexts.OrderBy(x => x.ContextId).Select(p => p.ContextId.ToString()));
			}
		}

		//Add Item
		public void Add(ClientDefinedReferenceItem clientDefinedReferenceItem)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			XmlElement clientDefinedReferenceItemProducts = GetClientDefinedReferenceItemProducts(clientDefinedReferenceItem);
			XmlElement clientDefinedReferenceItemContexts = GetClientDefinedReferenceItemContexts(clientDefinedReferenceItem);

			db.spDesktopDataAdmin_InsertClientDefinedReferenceItem_v1(
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

			db.spDesktopDataAdmin_DeleteClientDefinedReferenceItem_v1(
				clientDefinedReferenceItem.ClientDefinedReferenceItemId,
				clientDefinedReferenceItem.ClientSubUnitGuid,
				clientDefinedReferenceItem.SequenceNumber, 
				clientDefinedReferenceItem.SourceSystemCode,
				clientDefinedReferenceItem.ClientAccountNumber,
				clientDefinedReferenceItem.VersionNumber,
				adminUserGuid
			);
		}

		public IQueryable<ClientDefinedReferenceItem> GetClientSubUnitClientDefinedReferenceItems(string clientSubUnitGuid)
        {
            return db.ClientDefinedReferenceItems.OrderBy(c => c.DisplayName).Where(c => c.ClientSubUnitGuid == clientSubUnitGuid);
        }
      
        public ClientDefinedReferenceItem GetClientDefinedReferenceItem(string ClientDefinedReferenceItemId)
        {
            return db.ClientDefinedReferenceItems.Where(c => c.ClientDefinedReferenceItemId == ClientDefinedReferenceItemId).FirstOrDefault();
        }
        
		public ClientDefinedReferenceItem GetClientDefinedReferenceItemByDisplayName(string displayName)
        {
            return db.ClientDefinedReferenceItems.Where(c => c.DisplayName == displayName).FirstOrDefault();
        }

        public ClientDefinedReferenceItem GetCSUClientDefinedReferenceItemByCDRId(string clientSubUnitGuid, string clientDefinedReferenceItemId)
        {
            return db.ClientDefinedReferenceItems.Where(c => c.ClientSubUnitGuid == clientSubUnitGuid && c.ClientDefinedReferenceItemId == clientDefinedReferenceItemId).FirstOrDefault();
        }
        
		public ClientDefinedReferenceItem GetCSUClientDefinedReferenceItem(string clientSubUnitGuid, string displayName)
        {
            return db.ClientDefinedReferenceItems.Where(c => c.ClientSubUnitGuid == clientSubUnitGuid && c.DisplayName == displayName).FirstOrDefault();
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