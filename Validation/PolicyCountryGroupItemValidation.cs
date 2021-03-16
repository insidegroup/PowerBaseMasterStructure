using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyCountryGroupItemValidation
    {
        [Required(ErrorMessage = "Country Required")]
        public string CountryCode { get; set; }

		[Required(ErrorMessage = "Status Required")]
		public int PolicyCountryStatusId { get; set; }

    }
}
