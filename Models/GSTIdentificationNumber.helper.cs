using CWTDesktopDatabase.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWTDesktopDatabase.Models
{
	[MetadataType(typeof(GSTIdentificationNumberValidation))]
	public partial class GSTIdentificationNumber : CWTBaseModel
    {
        public string ClientTopUnitName { get; set; }
        public string CountryName { get; set; }
        public string StateProvinceName { get; set; }
	}
    
	public class GSTIdentificationJSON
    {
        public string GSTIdentificationName { get; set; }
        public string GSTIdentificationId { get; set; }
    }
}
