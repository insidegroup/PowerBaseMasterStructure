using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PhraseTranslationValidation
    {
        [Required(ErrorMessage = "Phrase Translation Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Phrase Translation must have a minimum of 2 characters")]
        public string PhraseTranslation1 { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}