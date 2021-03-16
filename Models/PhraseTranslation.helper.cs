using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(PhraseTranslationValidation))]
	public partial class PhraseTranslation : CWTBaseModel
    {
        public string LanguageName { get; set; }
    }
}
