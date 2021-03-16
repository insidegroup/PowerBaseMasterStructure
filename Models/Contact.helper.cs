using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ContactValidation))]
	public partial class Contact : CWTBaseModel
    {
        public string ContactTypeName { get; set; }
    }
}