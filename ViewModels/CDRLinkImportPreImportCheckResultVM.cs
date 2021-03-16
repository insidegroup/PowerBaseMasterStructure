using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CWTDesktopDatabase.ViewModels
{
	public class CDRImportStep1VM : CWTBaseViewModel
   {
        public ClientSubUnit ClientSubUnit { get; set; }
        public string ClientDefinedReferenceItemId { get; set; }
        public string DisplayName { get; set; }
		public string RelatedToDisplayName { get; set; }
        public string ClientSubUnitGuid { get; set; }
		public CDRImportStep2VM CDRImportStep2VM { get; set; }
   }

    public class CDRImportStep1WithFileVM : CDRImportStep1VM
    {
        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase File { get; set; }
    }

    public class CDRImportStep2VM
    {
        public List<string> ReturnMessages { get; set; }
        public bool IsValidData { get; set; }
        public byte[] FileBytes { get; set; } //only included if IsValidData=true;
    }

    public class CDRImportStep3VM
    {
        public List<string> ReturnMessages { get; set; }
        public string ClientDefinedReferenceItemId { get; set; }
        public string DisplayName { get; set; }
		public string RelatedToDisplayName { get; set; }
		public ClientSubUnit ClientSubUnit { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public int DeletedItemCount { get; set; }
        public int AddedItemCount { get; set; }
    }
}