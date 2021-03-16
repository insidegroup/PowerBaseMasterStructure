using CWTDesktopDatabase.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
	public class PriceTrackingContactValidation
    {
		[Required(ErrorMessage = "Contact Type Required")]
		public int ContactTypeId { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [RegularExpression(@"^([À-ÿ\w\s/\-\.'\\]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [RegularExpression(@"^([À-ÿ\w\s/\-\.'\\]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [RegularExpression(@"^([À-ÿ\w\s\-\._'@/\\]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string EmailAddress { get; set; }
	}
}