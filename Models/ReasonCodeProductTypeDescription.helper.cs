using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{    
    [MetadataType(typeof(ReasonCodeProductTypeDescriptionValidation))]
	public partial class ReasonCodeProductTypeDescription : CWTBaseModel
    {
        public string ProductName { get; set; }
        public string LanguageName { get; set; }
        public string ReasonCodeTypeDescription { get; set; }
    }
}