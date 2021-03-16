using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace CWTDesktopDatabase.ViewModels
{
    public class ChatFAQResponseItemImportStep1VM : CWTBaseViewModel
    {
        public ChatFAQResponseItemImportStep2VM ImportStep2VM { get; set; }
		public ChatFAQResponseGroup ChatFAQResponseGroup { get; set; }
		public int ChatFAQResponseGroupId { get; set; }
    }

    public class ChatFAQResponseItemImportStep1WithFileVM : ChatFAQResponseItemImportStep1VM
    {
        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase File { get; set; }
    }

    public class ChatFAQResponseItemImportStep2VM
    {
        public List<string> ReturnMessages { get; set; }
        public bool IsValidData { get; set; }
        public byte[] FileBytes { get; set; } //only included if IsValidData=true;
		public int ChatFAQResponseGroupId { get; set; }
    }

    public class ChatFAQResponseItemImportStep3VM
    {
        public List<string> ReturnMessages { get; set; }
		public int AddedItemCount { get; set; }
		public int DeletedItemCount { get; set; }
		public ChatFAQResponseGroup ChatFAQResponseGroup { get; set; }
		public int ChatFAQResponseGroupId { get; set; }
    }
}