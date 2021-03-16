using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class FormOfPaymentAdviceMessageGroupItemTranslationValidation
    {
        [Required(ErrorMessage = "Translation Required")]
		[RegularExpression(@"^([À-ÿ\w\s\/\*\-_.(),\u0022\u2018\u2019\“\'\%\$\=\+\?\!\:\;\@\<\>\[\]]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FormOfPaymentAdviceMessageTranslation { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}