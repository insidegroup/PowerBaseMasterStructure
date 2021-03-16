using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;


namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ControlValueValidation))]
	public partial class ControlValue : CWTBaseModel
    {
        public string ControlPropertyDescription { get; set; }
        //public string ControlName { get; set; }

    }
}
