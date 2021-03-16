using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PublicHolidayDateValidation
    {
        [Required(ErrorMessage = "Description Required")]
        public string PublicHolidayDescription { get; set; }

        [Required(ErrorMessage = "Date Required")]
        public DateTime? PublicHolidayDate1 { get; set; }
    }
}
