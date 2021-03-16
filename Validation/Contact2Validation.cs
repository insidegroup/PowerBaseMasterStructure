using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Validation
{
    public class Contact2Validation
    {
		[Required(ErrorMessage = "Contact Type Required")]
		public string ContactTypeId { get; set; }
		
		[Required(ErrorMessage = "First Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string LastName { get; set; }

        [Required(ErrorMessage = "Phone Required")]
		public string PrimaryTelephoneNumber { get; set; }

        [Required(ErrorMessage = "Email Required")]
		[RegularExpression(@"^([À-ÿ\w\s-()*@\.]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string EmailAddress { get; set; }

		[RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string FirstAddressLine { get; set; }

		[RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string SecondAddressLine { get; set; }

		[RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string CityName { get; set; }

		[RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string StateProvinceName { get; set; }

		[RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PostalCode { get; set; }

		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }

		[Required(ErrorMessage = "Responsibility Required")]
		[RegularExpression(@"^([À-ÿ\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string ResponsibilityDescription { get; set; }
    }
}