using System.ComponentModel.DataAnnotations;


namespace CWTDesktopDatabase.Validation
{
    [MetadataType(typeof(ControlValueTranslationValidation))]
    public class ControlValueTranslationValidation
    {
        [Required(ErrorMessage = "Translation Required")]
        public string ControlValueTranslation { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}