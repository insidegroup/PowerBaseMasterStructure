using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CWTDesktopDatabase.Repository
{
    public class PNROutputGroupLanguageRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of PNROutputGroupLanguages - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPNROutputGroupLanguages_v1Result> PagePNROutputGroupLanguages(int pnrOutputGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPNROutputGroupLanguages_v1(pnrOutputGroupId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPNROutputGroupLanguages_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get Remarks for a PNROutputGroup Language
        public PNROutputGroupLanguage GetItem(int pnrOutputGroupId, string languageCode)
        {
            PNROutputGroupLanguage pnrOutputGroupLanguage = new PNROutputGroupLanguage();
            pnrOutputGroupLanguage =  db.PNROutputGroupLanguages.SingleOrDefault(c => (c.PNROutputGroupId == pnrOutputGroupId)
                    && (c.LanguageCode == languageCode));

            if (pnrOutputGroupLanguage == null)
            {
                PNROutputGroupLanguage pnrOutputGroupLanguage2 = new PNROutputGroupLanguage();
                pnrOutputGroupLanguage2.LanguageCode = languageCode;
                pnrOutputGroupLanguage2.PNROutputGroupId = pnrOutputGroupId;
                return pnrOutputGroupLanguage2;
            }
            return pnrOutputGroupLanguage;
        }

        //get a single Node from the XML field
        public PNROutputGroupXMLItem GetPNROutputGroupXMLItem(int node, PNROutputGroupLanguage pnrOutputGroupLanguage){

            XDocument xDoc = new XDocument(pnrOutputGroupLanguage.PNROutputGroupXML);
            string baseRemarkType = xDoc.Root.Name.ToString();
            var items = xDoc.Element(baseRemarkType).Elements("item");
            var item = xDoc.Element(baseRemarkType).Elements("item").ElementAt(node);
            PNROutputGroupXMLItem pnrOutputGroupXMLItem = new PNROutputGroupXMLItem();

            //Add LanguageName
            if (pnrOutputGroupLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(pnrOutputGroupLanguage.LanguageCode);
                if (language != null)
                {
                    pnrOutputGroupXMLItem.Language = language;
                }
            }

            //Add PolicyGroup Information
            PNROutputGroupRepository pnrOutputGroupRepository = new PNROutputGroupRepository();
            PNROutputGroup pnrOutputGroup = new PNROutputGroup();
            pnrOutputGroup = pnrOutputGroupRepository.GetGroup(pnrOutputGroupLanguage.PNROutputGroupId);
            if (pnrOutputGroup != null)
            {
                pnrOutputGroupXMLItem.PNROutputGroup = pnrOutputGroup;
            }
            //we need to keep version number, no need for other itmes
            PNROutputGroupLanguage pnrOutputGroupLanguage2 = new PNROutputGroupLanguage();
            pnrOutputGroupLanguage2.VersionNumber = pnrOutputGroupLanguage.VersionNumber;
            pnrOutputGroupXMLItem.PNROutputGroupLanguage = pnrOutputGroupLanguage2;

            pnrOutputGroupXMLItem.Language.LanguageCode = pnrOutputGroupLanguage.LanguageCode;
            pnrOutputGroupXMLItem.PNROutputGroup.PNROutputGroupId = pnrOutputGroupLanguage.PNROutputGroupId;
            

            string valueText = (string)item.Value;
            if (valueText != null)
            {
                pnrOutputGroupXMLItem.Value = item.Value;
            }
            string remarkTypeAttribute = (string)item.Attribute("remarktype");
            if (remarkTypeAttribute != null)
            {
                pnrOutputGroupXMLItem.RemarkType = item.Attribute("remarktype").Value;
            }
            string bindAttribute = (string)item.Attribute("bind");
            if (bindAttribute != null)
            {
                pnrOutputGroupXMLItem.Bind = item.Attribute("bind").Value;
            }
            string qualifierAttribute = (string)item.Attribute("qualifier");
            if (qualifierAttribute != null)
            {
                pnrOutputGroupXMLItem.Qualifier = item.Attribute("qualifier").Value;
            }
            string sequenceAttribute = (string)item.Attribute("sequence");
            if (sequenceAttribute != null)
            {
                pnrOutputGroupXMLItem.Sequence = item.Attribute("sequence").Value;
            }
            string updateTypeAttribute = (string)item.Attribute("updatetype");
            if (updateTypeAttribute != null)
            {
                pnrOutputGroupXMLItem.UpdateType = item.Attribute("updatetype").Value;
            }
            string groupIdAttribute = (string)item.Attribute("groupid");
            if (groupIdAttribute != null)
            {
                pnrOutputGroupXMLItem.GroupId = item.Attribute("groupid").Value;
            }
            return pnrOutputGroupXMLItem;
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PNROutputGroupLanguage pnrOutputGroupLanguage)
        {
            //Add LanguageName
            if (pnrOutputGroupLanguage.LanguageCode != null)
            {
                LanguageRepository languageRepository = new LanguageRepository();
                Language language = new Language();
                language = languageRepository.GetLanguage(pnrOutputGroupLanguage.LanguageCode);
                if (language != null)
                {
                    pnrOutputGroupLanguage.LanguageName = language.LanguageName;
                }
            }

            //Add PolicyGroup Information
            PNROutputGroupRepository pnrOutputGroupRepository = new PNROutputGroupRepository();
            PNROutputGroup pnrOutputGroup = new PNROutputGroup();
            pnrOutputGroup = pnrOutputGroupRepository.GetGroup(pnrOutputGroupLanguage.PNROutputGroupId);
            if (pnrOutputGroup != null)
            {
                pnrOutputGroupLanguage.PNROutputGroupName = pnrOutputGroup.PNROutputGroupName;
            }

            //Format XML
            if (pnrOutputGroupLanguage.PNROutputGroupXML != null)
            {
                XDocument xDoc = new XDocument(pnrOutputGroupLanguage.PNROutputGroupXML);
                string baseRemarkType = xDoc.Root.Name.ToString();
                var items = xDoc.Element(baseRemarkType).Elements("item");
                int counter = 0;

                PNROutputGroupXMLDoc pnrOutputGroupXMLDoc = new PNROutputGroupXMLDoc();
                pnrOutputGroupXMLDoc.DocumentRoot = baseRemarkType;

                foreach (var item in items)
                {
                    PNROutputGroupXMLItem pnrOutputGroupXMLItem = new PNROutputGroupXMLItem();
                    pnrOutputGroupXMLItem.ItemNumber = counter;
                    string valueText = (string)item.Value;
                    if (valueText != null)
                    {
                        pnrOutputGroupXMLItem.Value = item.Value;
                    }
                    string remarkTypeAttribute = (string)item.Attribute("remarktype");
                    if (remarkTypeAttribute != null)
                    {
                        pnrOutputGroupXMLItem.RemarkType = item.Attribute("remarktype").Value;
                    }
                    string bindAttribute = (string)item.Attribute("bind");
                    if (bindAttribute != null)
                    {
                        pnrOutputGroupXMLItem.Bind = item.Attribute("bind").Value;
                    }
                    string qualifierAttribute = (string)item.Attribute("qualifier");
                    if (qualifierAttribute != null)
                    {
                        pnrOutputGroupXMLItem.Qualifier = item.Attribute("qualifier").Value;
                    }
                    string sequenceAttribute = (string)item.Attribute("sequence");
                    if (sequenceAttribute != null)
                    {
                        pnrOutputGroupXMLItem.Sequence = item.Attribute("sequence").Value;
                    }
                    string updateTypeAttribute = (string)item.Attribute("updatetype");
                    if (updateTypeAttribute != null)
                    {
                        pnrOutputGroupXMLItem.UpdateType = item.Attribute("updatetype").Value;
                    }
                    string groupIdAttribute = (string)item.Attribute("groupid");
                    if (groupIdAttribute != null)
                    {
                        pnrOutputGroupXMLItem.GroupId = item.Attribute("groupid").Value;
                    }

                    pnrOutputGroupXMLDoc.AddPNROutputGroupXMLItem(pnrOutputGroupXMLItem);
                    counter++;
                }
                pnrOutputGroupLanguage.PNROutputGroupXMLDOM = pnrOutputGroupXMLDoc;
            }
        }

       
        //puts the information from XML(pnrOutputGroupLanguage.PNROutputGroupXML) into Object(PNROutputGroupXMLDoc)
        public void BuildObjectfromXML(PNROutputGroupLanguage pnrOutputGroupLanguage)
        {
            
            XDocument xDoc = new XDocument(pnrOutputGroupLanguage.PNROutputGroupXML);
            string baseRemarkType = xDoc.Root.Name.ToString();
            var items = xDoc.Element(baseRemarkType).Elements("item");

            PNROutputGroupXMLDoc pnrOutputGroupXMLDoc = new PNROutputGroupXMLDoc();
            pnrOutputGroupXMLDoc.DocumentRoot = baseRemarkType;

            foreach (var item in items)
            {
                PNROutputGroupXMLItem pnrOutputGroupXMLItem = new PNROutputGroupXMLItem();
                string valueText = (string)item.Value;
                if (valueText != null)
                {
                    pnrOutputGroupXMLItem.Value = item.Value;
                }
                string remarkTypeAttribute = (string)item.Attribute("remarktype");
                if (remarkTypeAttribute != null)
                {
                    pnrOutputGroupXMLItem.RemarkType = item.Attribute("remarktype").Value;
                }
                string bindAttribute = (string)item.Attribute("bind");
                if (bindAttribute != null)
                {
                    pnrOutputGroupXMLItem.Bind = item.Attribute("bind").Value;
                }
                string qualifierAttribute = (string)item.Attribute("qualifier");
                if (qualifierAttribute != null)
                {
                    pnrOutputGroupXMLItem.Qualifier = item.Attribute("qualifier").Value;
                }
                string sequenceAttribute = (string)item.Attribute("sequence");
                if (sequenceAttribute != null)
                {
                    pnrOutputGroupXMLItem.Sequence = item.Attribute("sequence").Value;
                }
                string updateTypeAttribute = (string)item.Attribute("updatetype");
                if (updateTypeAttribute != null)
                {
                    pnrOutputGroupXMLItem.UpdateType = item.Attribute("updatetype").Value;
                }
                string groupIdAttribute = (string)item.Attribute("groupid");
                if (groupIdAttribute != null)
                {
                    pnrOutputGroupXMLItem.GroupId = item.Attribute("groupid").Value;
                }

                pnrOutputGroupXMLDoc.AddPNROutputGroupXMLItem(pnrOutputGroupXMLItem);
            }
            pnrOutputGroupLanguage.PNROutputGroupXMLDOM = pnrOutputGroupXMLDoc;
        }       
    }
}
