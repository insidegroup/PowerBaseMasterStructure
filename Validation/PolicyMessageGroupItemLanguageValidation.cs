using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyMessageGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Message Required")]
        [StringLength(4000, ErrorMessage = "Name cannot be longer than 4000 characters.")]
        public string PolicyMessageGroupItemTranslationMarkdown { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}