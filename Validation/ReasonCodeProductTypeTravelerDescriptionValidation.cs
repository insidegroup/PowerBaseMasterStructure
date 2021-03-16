using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class ReasonCodeProductTypeTravelerDescriptionValidation
    {
		[Required(ErrorMessage = "Traveler Description Required")]
		[RegularExpression(@"^([\w\s-.()*À-ÿ']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ReasonCodeProductTypeTravelerDescription1 { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
