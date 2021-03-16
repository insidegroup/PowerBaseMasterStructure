using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(ChatMessageFAQValidation))]
	public partial class ChatMessageFAQ : CWTBaseModel
    {
    }

    public partial class ChatMessageFAQReference
    {
        public string TableName { get; set; }
    }
}
