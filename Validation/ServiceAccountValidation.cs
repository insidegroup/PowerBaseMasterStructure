using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ServiceAccountValidation
    {
		[Required(ErrorMessage = "Service Account Name Required. Enter a descriptive name.")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ServiceAccountName { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string LastName { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FirstName { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()/\'\\@]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string Email { get; set; }

        [RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string CWTManager { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s/*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string InternalRemark { get; set; }
    }
}
