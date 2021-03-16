using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(CityLanguageValidation))]
	public partial class CityLanguage : CWTBaseModel
    {
        public string LanguageName { get; set; }
    }
}
