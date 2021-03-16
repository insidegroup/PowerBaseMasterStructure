using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{

	public partial class PNROutputGroupLanguage : CWTBaseModel
    {
        public string PNROutputGroupName { get; set; }
        public string LanguageName { get; set; }
        public PNROutputGroupXMLDoc PNROutputGroupXMLDOM { get; set; }

    }

    public class PNROutputGroupXMLDoc
    {
        public List<PNROutputGroupXMLItem> PNROutputGroupXMLItems { get; set; }
        public string DocumentRoot { get; set; }

        public PNROutputGroupXMLDoc()
        {
            this.PNROutputGroupXMLItems = new List<PNROutputGroupXMLItem>();
        }

        public void AddPNROutputGroupXMLItem(PNROutputGroupXMLItem pnrOutputGroupXMLItem)
        {
            PNROutputGroupXMLItems.Add(pnrOutputGroupXMLItem);
        }
    }
     [MetadataType(typeof(PNROutputGroupXMLItemValidation))]
    public class PNROutputGroupXMLItem
    {
        public int ItemNumber { get; set; }  //Zero-based Index of Node to be used like Primary Key
        public string RemarkType { get; set; }
        public string Bind { get; set; }
        public string Qualifier { get; set; }
        public string Sequence { get; set; }
        public string UpdateType { get; set; }
        public string GroupId { get; set; }
        public string Value { get; set; }

        public Language Language { get; set; }
        public PNROutputGroup PNROutputGroup { get; set; }
        public PNROutputGroupLanguage PNROutputGroupLanguage { get; set; } //we only use VersionNumber
 
        //public string LanguageCode { get; set; }
        //public string LanguageName { get; set; }
        //public int PNROutputGroupId { get; set; }
        //public int PNROutputGroupName { get; set; }
        public int VersionNumber { get; set; }
        
    }
}
