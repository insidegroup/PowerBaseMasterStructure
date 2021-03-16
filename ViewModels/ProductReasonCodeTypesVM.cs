using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Xml.Serialization;
using System.Data.Linq;

namespace CWTDesktopDatabase.ViewModels
{
    public class ProductReasonCodeTypesVM : CWTBaseViewModel
    {
        public ProductReasonCodeTypes ProductReasonCodeTypes { get; set; }
        public List<Product> Products { get; set; }
        public List<ReasonCodeType> ReasonCodeTypes { get; set; }

        public ProductReasonCodeTypesVM()
        {
        }
        public ProductReasonCodeTypesVM(ProductReasonCodeTypes productReasonCodeTypes, List<Product> products, List<ReasonCodeType> reasonCodeTypes)
        {
            ProductReasonCodeTypes = productReasonCodeTypes;
            Products = products;
            ReasonCodeTypes = reasonCodeTypes;
        }

    }
    [Serializable()]
    [XmlRoot("ProductReasonCodeTypes")]
    public class ProductReasonCodeTypes
    {
       // [XmlArray("ProductReasonCodeTypes")]
       // [XmlArrayItem("ProductReasonCodeType", typeof(ProductReasonCodeType))]
       // public ProductReasonCodeType[] ProductReasonCodeType { get; set; }
        [XmlElementAttribute("ProductReasonCodeType")]
        public List<ProductReasonCodeType> ProductReasonCodeType { get; set; }
    }

    [Serializable()]
    [XmlRoot("ProductReasonCodeType")]
    public class ProductReasonCodeType
    {
        public Product Product { get; set; }
        public ReasonCodeType ReasonCodeType { get; set; }

        [XmlElementAttribute("ReasonCodeGroup")]
        public List<ReasonCodeGroup> ReasonCodeGroup { get; set; }

    }

    //public class ReasonCodeGroups
    //{
        //[XmlArrayItem("ReasonCodeGroups", typeof(ReasonCodeGroup))]
        //public ReasonCodeGroup[] ReasonCodeGroups { get; set; }
        
    //        [XmlElementAttribute("ReasonCodeGroup")]
    //        public List<ReasonCodeGroup> ReasonCodeGroup { get; set; }
    //}
    [XmlRoot("ReasonCodeGroup")]
    public partial class ReasonCodeGroup
    {
        [XmlElement("ReasonCodeGroupId")]
        public int ReasonCodeGroupId { get; set; }

        public string Source { get; set; }
        public string SourceCode { get; set; }
    }

    
}
    /*
    public class ProductReasonCodeTypesVM : CWTBaseViewModel
    {

        [Serializable()]
        [XmlRoot("ProductReasonCodeType")]
        public class ProductReasonCodeType
        {
            [XmlElement("Product")]
            public Product Product { get; set; }

            [XmlElement("ReasonCodeType")]
            public ReasonCodeType ReasonCodeType { get; set; }

            [XmlElement("ReasonCodeGroups")]
            public ReasonCodeGroups ReasonCodeGroups { get; set; }

            
            public ProductReasonCodeType()
            {
            }
            public ProductReasonCodeType(Product product, ReasonCodeType reasonCodeType, List<ReasonCodeGroup> reasonCodeGroups)
            {
                Product = product;
                ReasonCodeType = reasonCodeType;
                ReasonCodeGroups = reasonCodeGroups;
            }
             

        }

        public List<ProductReasonCodeType> ProductReasonCodeTypes;

        public ProductReasonCodeTypesVM()
        {
        }

        public ProductReasonCodeTypesVM(List<ProductReasonCodeType> productReasonCodeTypes)
        {
            ProductReasonCodeTypes = productReasonCodeTypes;
        }
     * */
