using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CWTDesktopDatabase.ViewModels
{
    //http://stackoverflow.com/questions/10722428/dataannotations-notrequired-attribute
   public class CDRLinkImportPreImportCheckVM : CWTBaseViewModel
   {
        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase File { get; set; }

        public ClientSubUnit ClientSubUnit { get; set; }
        public string ClientDefinedReferenceItemId { get; set; }
        public string DisplayName { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public PreImportCheckResult PreImportCheckResult { get; set; }

        public CDRLinkImportPreImportCheckVM()
        {
        }

        public CDRLinkImportPreImportCheckVM(HttpPostedFileBase file, ClientSubUnit clientSubUnit, string clientDefinedReferenceItemId, string displayName, string clientSubUnitGuid, PreImportCheckResult preImportCheckResult)
        {
            ClientSubUnit = clientSubUnit;
            ClientDefinedReferenceItemId = clientDefinedReferenceItemId;
            DisplayName = displayName;
            File = file;
            PreImportCheckResult = preImportCheckResult;
        }

   }

   public class PreImportCheckResult
   {
       public List<string> ReturnMessages { get; set; }
       public bool IsValidData { get; set; }
       public XmlDocument XmlDocument { get; set; } //only included if IsValidData=true;
       public string FileXML { get; set; } //only included if IsValidData=true;
   }
}