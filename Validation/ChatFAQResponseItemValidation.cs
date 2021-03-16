using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ChatFAQResponseItemValidation
    {
        [Required(ErrorMessage = "Chat Message FAQ Description Required")]
        public int ChatMessageFAQId { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }

        [Required(ErrorMessage = "Response Required")]
        [RegularExpression(@"^([À-ÿ\w\s\/\*\-_.(),\u0022\“\'\%\$\=\+\?\!\:\;\@\<\>]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ChatFAQResponseItemDescription { get; set; }
    }
}
