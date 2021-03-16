using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
    public class PassiveSegmentBuilderGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get one ProductGroup
        public ProductGroup GetGroup(int productGroupId)
        {
            return db.ProductGroups.SingleOrDefault(c => c.ProductGroupId == productGroupId && c.ProductGroupDomainTypeId == 1);
        }

            //Get one ProductGroup
        public List<spDesktopDataAdmin_SelectProductGroupProducts_v1Result> GetProducts(int productGroupId)
        {
            return db.spDesktopDataAdmin_SelectProductGroupProducts_v1(productGroupId).ToList();
        }

        //Edit Group
        public void Edit(PassiveSegmentBuilderGroupVM passiveSegmentBuilderGroup)
        {
             string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

             //products to XML
             XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
             XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
             doc.AppendChild(dec);
             XmlElement root = doc.CreateElement("ProductGroup");
             doc.AppendChild(root);

             if (passiveSegmentBuilderGroup.Products != null)
             {
                 if (passiveSegmentBuilderGroup.Products.Count > 0)
                 {
                     XmlElement xmlProducts = doc.CreateElement("Products");
                     foreach (SelectListItem p in passiveSegmentBuilderGroup.Products)
                     {
                         if (p.Selected)
                         {
                             XmlElement xmlProduct = doc.CreateElement("Product");
                             xmlProduct.InnerText = p.Value;
                             xmlProducts.AppendChild(xmlProduct);
                         }
                     }
                     root.AppendChild(xmlProducts);
                 }
             }
             if (passiveSegmentBuilderGroup.SubProducts != null)
             {
                 if (passiveSegmentBuilderGroup.SubProducts.Count > 0)
                 {
                     XmlElement xmlSubProducts = doc.CreateElement("SubProducts");
                     foreach (SelectListItem p in passiveSegmentBuilderGroup.SubProducts)
                     {
                         XmlElement xmlSubProduct = doc.CreateElement("SubProduct");
                         xmlSubProduct.InnerText = p.Value;
                         xmlSubProducts.AppendChild(xmlSubProduct);
                     }
                     root.AppendChild(xmlSubProducts);
                 }
             }

            db.spDesktopDataAdmin_UpdatePassiveSegmentBuilderGroup_v1(
                passiveSegmentBuilderGroup.ProductGroup.ProductGroupId,
                passiveSegmentBuilderGroup.ProductGroup.ProductGroupName,
                passiveSegmentBuilderGroup.ProductGroup.EnabledFlagNonNullable,
                passiveSegmentBuilderGroup.ProductGroup.EnabledDate,
                passiveSegmentBuilderGroup.ProductGroup.ExpiryDate,
                passiveSegmentBuilderGroup.ProductGroup.InheritFromParentFlag,
                passiveSegmentBuilderGroup.ProductGroup.HierarchyType,
                passiveSegmentBuilderGroup.ProductGroup.HierarchyCode,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid,
                passiveSegmentBuilderGroup.ProductGroup.VersionNumber
            );
        }
        
        //Add Group
        public void Add(PassiveSegmentBuilderGroupVM passiveSegmentBuilderGroup)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            //products to XML
            XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("ProductGroup");
            doc.AppendChild(root);

            if (passiveSegmentBuilderGroup.Products != null)
            {
                if (passiveSegmentBuilderGroup.Products.Count > 0)
                {
                    XmlElement xmlProducts = doc.CreateElement("Products");
                    foreach (SelectListItem p in passiveSegmentBuilderGroup.Products)
                    {
                        if (p.Selected)
                        {
                            XmlElement xmlProduct = doc.CreateElement("Product");
                            xmlProduct.InnerText = p.Value;
                            xmlProducts.AppendChild(xmlProduct);
                        }
                    }
                    root.AppendChild(xmlProducts);
                }
            }
            if (passiveSegmentBuilderGroup.SubProducts != null)
            {
                if (passiveSegmentBuilderGroup.SubProducts.Count > 0)
                {
                    XmlElement xmlSubProducts = doc.CreateElement("SubProducts");
                    foreach (SelectListItem p in passiveSegmentBuilderGroup.SubProducts)
                    {
                        XmlElement xmlSubProduct = doc.CreateElement("SubProduct");
                        xmlSubProduct.InnerText = p.Value;
                        xmlSubProducts.AppendChild(xmlSubProduct);
                    }
                    root.AppendChild(xmlSubProducts);
                }
            }


            db.spDesktopDataAdmin_InsertPassiveSegmentBuilderGroup_v1(
                passiveSegmentBuilderGroup.ProductGroup.ProductGroupName,
                passiveSegmentBuilderGroup.ProductGroup.EnabledFlagNonNullable,
                passiveSegmentBuilderGroup.ProductGroup.EnabledDate,
                passiveSegmentBuilderGroup.ProductGroup.ExpiryDate,
                passiveSegmentBuilderGroup.ProductGroup.InheritFromParentFlag,
                passiveSegmentBuilderGroup.ProductGroup.HierarchyType,
                passiveSegmentBuilderGroup.ProductGroup.HierarchyCode,
                System.Xml.Linq.XElement.Parse(doc.OuterXml),
                adminUserGuid
            );
        }
        
   
    }
}

