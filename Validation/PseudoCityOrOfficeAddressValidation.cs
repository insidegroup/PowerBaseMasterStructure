using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class PseudoCityOrOfficeAddressValidation
    {
		[Required(ErrorMessage = "First Address Line Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s-()_.',*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FirstAddressLine { get; set; }

		[RegularExpression(@"^([A-zÀ-ÿ\w\s-()_.',*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string SecondAddressLine { get; set; }

		[Required(ErrorMessage = "City Required")]
		[RegularExpression(@"^([A-zÀ-ÿ\w\s-()_.',*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string CityName { get; set; }

		[Required(ErrorMessage = "Postal Code Required")]
		public string PostalCode { get; set; }
		
		[Required(ErrorMessage = "Global Region Required")]
		public string CountryCode { get; set; }
    }
}
