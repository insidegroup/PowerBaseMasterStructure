using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(SystemUserGDSValidation))]
	public partial class SystemUserGDS : CWTBaseModel
    {
        public string SystemUserName { get; set; }
        public string GDSName { get; set; }
        public string PseudoCityOrOfficeId { get; set; }
        public string GDSSignOn { get; set; }
    }
}

