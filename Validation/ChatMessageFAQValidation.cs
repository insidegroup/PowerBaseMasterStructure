using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class ChatMessageFAQValidation
    {
        [Required(ErrorMessage = "Chat Message FAQ Required")]
		[RegularExpression(@"^([À-ÿ\w\s\/\*\-_.(),\u0022\“\'\%\$\=\+\?\!\:\;\@\<\>]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ChatMessageFAQName { get; set; }
	}
}