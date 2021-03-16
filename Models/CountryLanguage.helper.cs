using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(CountryLanguageValidation))]
	public partial class CountryLanguage : CWTBaseModel
    {
        public string LanguageName { get; set; }
    }
}
