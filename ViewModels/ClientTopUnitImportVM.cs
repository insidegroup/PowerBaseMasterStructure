using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CWTDesktopDatabase.ViewModels
{
    public class ClientTopUnitImportStep1VM : CWTBaseViewModel
    {
        public ClientTopUnit ClientTopUnit { get; set; }
        public string ClientTopUnitGuid { get; set; }
        public ClientTopUnitImportStep2VM ImportStep2VM { get; set; }
    }

    public class ClientTopUnitImportStep1WithFileVM : ClientTopUnitImportStep1VM
    {
        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase File { get; set; }
    }

    public class ClientTopUnitImportStep2VM
    {
        public List<string> ReturnMessages { get; set; }
        public bool IsValidData { get; set; }
        public byte[] FileBytes { get; set; } //only included if IsValidData=true;
    }

    public class ClientTopUnitImportStep3VM
    {
        public List<string> ReturnMessages { get; set; }
        public ClientTopUnit ClientTopUnit { get; set; }
        public string ClientTopUnitGuid { get; set; }
        public int DeletedItemCount { get; set; }
        public int AddedItemCount { get; set; }
    }
}