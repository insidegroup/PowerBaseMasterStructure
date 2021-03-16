using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ChatFAQResponseItemLanguageValidation
    {
        [Required(ErrorMessage = "Translation Required")]
        [RegularExpression(@"^([À-ÿ\w\s\/\*\-_.(),\u0022\“\'\%\$\=\+\?\!\:\;\@\<\>]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ChatFAQResponseItemLanguageDescription { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}