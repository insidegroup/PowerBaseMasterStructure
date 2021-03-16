using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;


namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(TripTypeItemValidation))]
	public partial class TripTypeItem : CWTBaseModel
    {
        public string TripTypeGroupName{ get; set; }
        public string TripTypeDescription { get; set; }
        
    }
}
