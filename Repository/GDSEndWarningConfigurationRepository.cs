using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
	public class GDSEndWarningConfigurationRepository
	{
		private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of GDS Responses
		public CWTPaginatedList<spDesktopDataAdmin_SelectGDSEndWarningConfigurations_v1Result> PageGDSEndWarningConfigurations(int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectGDSEndWarningConfigurations_v1(filter, sortField, sortOrder, page, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectGDSEndWarningConfigurations_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

		//Get one GDSEndWarningConfiguration
		public GDSEndWarningConfiguration GetGroup(int id)
		{
			return db.GDSEndWarningConfigurations.SingleOrDefault(c => c.GDSEndWarningConfigurationId == id);
		}

		//Add GDSEndWarningConfiguration
		public void Add(GDSEndWarningConfiguration gdsEndWarningConfiguration)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//AutomatedCommands to XML
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("AutomatedCommands");
			doc.AppendChild(root);

			if (gdsEndWarningConfiguration.AutomatedCommands != null)
			{
				foreach (AutomatedCommand automatedCommand in gdsEndWarningConfiguration.AutomatedCommands)
				{
					if (automatedCommand != null)
					{
						if (automatedCommand.CommandText != null && automatedCommand.CommandText != null)
						{
							XmlElement xmlAutomatedCommand = doc.CreateElement("AutomatedCommand");

							XmlElement xmlCommandText = doc.CreateElement("CommandText");
							xmlCommandText.InnerText = automatedCommand.CommandText;
							xmlAutomatedCommand.AppendChild(xmlCommandText);

							XmlElement xmlCommandExecutionSequenceNumber = doc.CreateElement("CommandExecutionSequenceNumber");
							xmlCommandExecutionSequenceNumber.InnerText = automatedCommand.CommandExecutionSequenceNumber.ToString();
							xmlAutomatedCommand.AppendChild(xmlCommandExecutionSequenceNumber);

							root.AppendChild(xmlAutomatedCommand);
						}
					}
				}
			}

			db.spDesktopDataAdmin_InsertGDSEndWarningConfiguration_v1(
				gdsEndWarningConfiguration.GDSCode,
				gdsEndWarningConfiguration.IdentifyingWarningMessage,
				gdsEndWarningConfiguration.GDSEndWarningBehaviorTypeId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		//Edit GDSEndWarningConfiguration
		public void Edit(GDSEndWarningConfiguration gdsEndWarningConfiguration)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//AutomatedCommands to XML
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("AutomatedCommands");
			doc.AppendChild(root);

			if (gdsEndWarningConfiguration.AutomatedCommands != null)
			{
				foreach (AutomatedCommand automatedCommand in gdsEndWarningConfiguration.AutomatedCommands)
				{
					if (automatedCommand != null)
					{
						if (automatedCommand.CommandText != null && automatedCommand.CommandText != null)
						{
							XmlElement xmlAutomatedCommand = doc.CreateElement("AutomatedCommand");

							XmlElement xmlCommandText = doc.CreateElement("CommandText");
							xmlCommandText.InnerText = automatedCommand.CommandText;
							xmlAutomatedCommand.AppendChild(xmlCommandText);

							XmlElement xmlCommandExecutionSequenceNumber = doc.CreateElement("CommandExecutionSequenceNumber");
							xmlCommandExecutionSequenceNumber.InnerText = automatedCommand.CommandExecutionSequenceNumber.ToString();
							xmlAutomatedCommand.AppendChild(xmlCommandExecutionSequenceNumber);

							root.AppendChild(xmlAutomatedCommand);
						}
					}
				}
			}

			db.spDesktopDataAdmin_UpdateGDSEndWarningConfiguration_v1(
				gdsEndWarningConfiguration.GDSEndWarningConfigurationId,
				gdsEndWarningConfiguration.GDSCode,
				gdsEndWarningConfiguration.IdentifyingWarningMessage,
				gdsEndWarningConfiguration.GDSEndWarningBehaviorTypeId,
				System.Xml.Linq.XElement.Parse(doc.OuterXml),
				adminUserGuid
			);
		}

		public void Delete(GDSEndWarningConfiguration gdsEndWarningConfiguration)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteGDSEndWarningConfiguration_v1(
				gdsEndWarningConfiguration.GDSEndWarningConfigurationId,
				adminUserGuid,
				gdsEndWarningConfiguration.VersionNumber
			);
		}
	}
}