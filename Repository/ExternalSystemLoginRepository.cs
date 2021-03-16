using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Xml;

namespace CWTDesktopDatabase.Repository
{
    public class ExternalSystemLoginRepository
    {
        HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get one GetBackOfficeIdentifier
        //This is ExternalSystemLoginName for ExternalSystem with Id=2
        public ExternalSystemLogin GetBackOfficeIdentifier(string systemUserGuid)
        {
            ExternalSystemLoginRepository externalSystemLoginRepository = new ExternalSystemLoginRepository();
            ExternalSystemLogin externalSystemLogin = new ExternalSystemLogin();

            var result = from n in db.spDesktopDataAdmin_SelectExternalSystemLoginName_v1(systemUserGuid)
                         select
                             new ExternalSystemLogin
                             {
                                 ExternalSystemLoginId = n.ExternalSystemLoginId,
                                 ExternalSystemLoginName = n.BackOfficeAgentIdentifier.Trim(),
                                 VersionNumber = n.VersionNumber
                             };

            return result.ToList().FirstOrDefault();
        }

        //Edit Item
        public void EditBackOfficeIdentifier(string systemUserGuid, int externalSystemLoginId, string externalSystemLoginName, int versionNumber)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateExternalSystemLoginName_v1(
                systemUserGuid,
                externalSystemLoginId,
                externalSystemLoginName,
                adminUserGuid,
                versionNumber
            );
        }

        //Delete Item
        public void DeleteBackOfficeIdentifier(int externalSystemLoginId, int versionNumber)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteExternalSystemLoginName_v1(
                externalSystemLoginId,
                adminUserGuid,
                versionNumber
            );
        }

		//Change Item
		public void AddBackOfficeIdentifier(string systemUserGuid, string externalSystemLoginName)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertExternalSystemLoginName_v1(
					systemUserGuid,
					externalSystemLoginName,
					2,
					adminUserGuid
				);

		}

		/*
		 * GetBackOfficeIdentifiers (ExternalSystemLoginSystemUserCountry)
		 */

		//Get a list of GetBackOfficeIdentifiers (ExternalSystemLoginSystemUserCountry)
		//Back Office Counselor Identifier = ExternalSystemLoginName
		public List<ExternalSystemLoginSystemUserCountry> GetBackOfficeIdentifiers(string systemUserGuid)
		{
			var result = from n in db.spDesktopDataAdmin_SelectExternalSystemLoginSystemUserCountry_v1(systemUserGuid)
						 select
							new ExternalSystemLoginSystemUserCountry
							{
								ExternalSystemLoginName = n.BackOfficeAgentIdentifier.Trim(),
								CountryCode = n.CountryCode,
								IsDefaultFlag = n.IsDefaultFlag,
								VersionNumber = n.VersionNumber
							};

			return result.ToList();
		}

		//Change Item
		public void AddBackOfficeIdentifiers(string systemUserGuid, List<ExternalSystemLoginSystemUserCountry> externalSystemLoginSystemUserCountries)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			//Items to XML
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("ExternalSystemLoginSystemUserCountries");
			doc.AppendChild(root);

			if (externalSystemLoginSystemUserCountries != null)
			{
				foreach (ExternalSystemLoginSystemUserCountry externalSystemLoginSystemUserCountry in externalSystemLoginSystemUserCountries)
				{
					if (externalSystemLoginSystemUserCountry != null && !string.IsNullOrEmpty(externalSystemLoginSystemUserCountry.ExternalSystemLoginName))
					{
						XmlElement xmExternalSystemLoginSystemUserCountry = doc.CreateElement("ExternalSystemLoginSystemUserCountry");

						//ExternalSystemLoginName
						XmlElement xmlExternalSystemLoginName = doc.CreateElement("ExternalSystemLoginName");
						xmlExternalSystemLoginName.InnerText = externalSystemLoginSystemUserCountry.ExternalSystemLoginName.ToString();
						xmExternalSystemLoginSystemUserCountry.AppendChild(xmlExternalSystemLoginName);

						//CountryCode
						XmlElement xmlCountryCode = doc.CreateElement("CountryCode");
						xmlCountryCode.InnerText = externalSystemLoginSystemUserCountry.CountryCode.ToString();
						xmExternalSystemLoginSystemUserCountry.AppendChild(xmlCountryCode);

						//IsDefaultFlag
						XmlElement xmlIsDefaultFlag = doc.CreateElement("IsDefaultFlag");
						xmlIsDefaultFlag.InnerText = externalSystemLoginSystemUserCountry.IsDefaultFlag.ToString();
						xmExternalSystemLoginSystemUserCountry.AppendChild(xmlIsDefaultFlag);

						//VersionNumber
						XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
						xmlVersionNumber.InnerText = externalSystemLoginSystemUserCountry.VersionNumber.ToString();
						xmExternalSystemLoginSystemUserCountry.AppendChild(xmlVersionNumber);

						root.AppendChild(xmExternalSystemLoginSystemUserCountry);
					}
				}
			}

			db.spDesktopDataAdmin_UpdateExternalSystemLoginSystemUserCountry_v1(
					systemUserGuid,
					System.Xml.Linq.XElement.Parse(doc.OuterXml),
					2,
					adminUserGuid
					);

		}
    }
}