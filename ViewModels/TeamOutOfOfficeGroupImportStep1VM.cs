using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CWTDesktopDatabase.ViewModels
{
    public class TeamOutOfOfficeGroupImportStep1VM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public TeamOutOfOfficeGroupImportStep2VM ImportStep2VM { get; set; }
    }

    public class TeamOutOfOfficeGroupImportStep1WithFileVM : TeamOutOfOfficeGroupImportStep1VM
    {
        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase File { get; set; }
    }

    public class TeamOutOfOfficeGroupImportStep2VM
    {
        public List<string> ReturnMessages { get; set; }
        public bool IsValidData { get; set; }
        public byte[] FileBytes { get; set; } //only included if IsValidData=true;
    }

    public class TeamOutOfOfficeGroupImportStep3VM
    {
        public List<string> ReturnMessages { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public int AddedItemCount { get; set; }
    }
}