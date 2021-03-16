using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ThirdPartyUserValidation
    {
		[Required(ErrorMessage = "Third Party Name Required. Enter a descriptive name.")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ThirdPartyName { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string LastName { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Email Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()/\'\\@]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string Email { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string CWTManager { get; set; }

		[Required(ErrorMessage = "User Type Required")]
		public int ThirdPartyUserTypeId { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FirstAddressLine { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string SecondAddressLine { get; set; }

		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }

		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Special character entered is not allowed")]
		public string Phone { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string InternalRemark { get; set; }
    }
}
