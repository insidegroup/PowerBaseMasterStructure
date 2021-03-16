using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class ReasonCodeTravelerDescriptionValidation
    {
		[Required(ErrorMessage = "Traveler Description Required")]
		[RegularExpression(@"^([\w\s-.()*À-ÿ']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ReasonCodeTravelerDescription1 { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
