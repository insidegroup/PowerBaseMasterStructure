using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class GDSContactValidation
    {
		[Required(ErrorMessage = "GDS Required")]
		public string GDSCode { get; set; }

		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }
		
		[Required(ErrorMessage = "Global Region Required")]
		public string GlobalRegionCode { get; set; }

		[Required(ErrorMessage = "Pseudo City/Office ID Business Required")]
		public string PseudoCityOrOfficeBusinessId { get; set; }

		[Required(ErrorMessage = "First Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s/*-_.()']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s/*-_.()']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email Required")]
		[RegularExpression(@"^([À-ÿ\w\s-_()*@\.]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string EmailAddress { get; set; }

		[Required(ErrorMessage = "Phone Required")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Special character entered is not allowed")]
		public string Phone { get; set; }
	}
}