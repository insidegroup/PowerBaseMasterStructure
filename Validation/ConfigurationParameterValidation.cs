using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ConfigurationParameterValidation
    {
        [Required(ErrorMessage = "Value Required")]
        public string ConfigurationParameterValue { get; set; }
    }
}