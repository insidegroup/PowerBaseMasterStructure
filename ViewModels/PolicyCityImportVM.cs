using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyCityImportStep1VM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public int PolicyGroupId { get; set; }
        public PolicyCityImportStep2VM ImportStep2VM { get; set; }
    }

    public class PolicyCityImportStep1WithFileVM : PolicyCityImportStep1VM
    {
        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase File { get; set; }
    }

    public class PolicyCityImportStep2VM
    {
        public List<string> ReturnMessages { get; set; }
        public bool IsValidData { get; set; }
        public byte[] FileBytes { get; set; } //only included if IsValidData=true;
    }

    public class PolicyCityImportStep3VM
    {
        public List<string> ReturnMessages { get; set; }
        public PolicyGroup PolicyGroup { get; set; }
        public int PolicyGroupId { get; set; }
        public int DeletedItemCount { get; set; }
        public int AddedItemCount { get; set; }
    }
}