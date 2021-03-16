using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CWTDesktopDatabase.Validation;

namespace CWTDesktopDatabase.Validation
{
    public class MeetingContactValidation
    {
        [Required(ErrorMessage = "First Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s-*\.'()\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string MeetingContactFirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
		[RegularExpression(@"^([À-ÿ\w\s-*\.'()\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string MeetingContactLastName { get; set; }

        [Required(ErrorMessage = "Contact Type Required")]
        public string ContactTypeId { get; set; }

		[Required(ErrorMessage = "Phone Required")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Special character entered is not allowed")]
		public string MeetingContactPhoneNumber { get; set; }

		[Required(ErrorMessage = "Email Required")]
		[RegularExpression(@"^([À-ÿ\w\s-()*@\._'\\\/]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string MeetingContactEmailAddress { get; set; }
		
		[Required(ErrorMessage = "Country Required")]
		public string CountryCode { get; set; }
    }
}