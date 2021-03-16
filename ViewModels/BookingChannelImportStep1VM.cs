using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CWTDesktopDatabase.ViewModels
{
    public class BookingChannelImportStep1VM : CWTBaseViewModel
    {
        public ClientSubUnit ClientSubUnit { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public BookingChannelImportStep2VM ImportStep2VM { get; set; }
    }

    public class BookingChannelImportStep1WithFileVM : BookingChannelImportStep1VM
    {
        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase File { get; set; }
    }

    public class BookingChannelImportStep2VM
    {
        public List<string> ReturnMessages { get; set; }
        public bool IsValidData { get; set; }
        public byte[] FileBytes { get; set; } //only included if IsValidData=true;
    }

    public class BookingChannelImportStep3VM
    {
        public List<string> ReturnMessages { get; set; }
        public ClientSubUnit ClientSubUnit { get; set; }
        public string ClientSubUnitGuid { get; set; }
        public int DeletedItemCount { get; set; }
        public int AddedItemCount { get; set; }
    }
}