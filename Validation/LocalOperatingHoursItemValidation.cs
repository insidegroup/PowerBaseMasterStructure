using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Exclude = "CreationTimestamp")]
	public class LocalOperatingHoursItemValidation
    {
        [Required(ErrorMessage = "Week Day Required")]
        public int WeekDayId { get; set; }

        [RegularExpression(@"^(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):?([0-5]?[0-9]?)$", ErrorMessage = "Format entered is not allowed")]
        [Required(ErrorMessage = "Opening Time Required")]
        public DateTime? OpeningTime { get; set; }

        [RegularExpression(@"^(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):?([0-5]?[0-9]?)$", ErrorMessage = "Format entered is not allowed")]
        [Required(ErrorMessage = "Closing Time Required")]
        public DateTime? ClosingTime { get; set; }
    }
}
