using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(ConfigurationParameterValidation))]
	public partial class ConfigurationParameter : CWTBaseModel
    {
    }
}
