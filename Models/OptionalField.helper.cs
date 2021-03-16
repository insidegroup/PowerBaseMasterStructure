using CWTDesktopDatabase.Validation;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Models
{

    [MetadataType(typeof(OptionalFieldValidation))]
	public partial class OptionalField : CWTBaseModel
    {
        
    }
}
