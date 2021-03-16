using System.ComponentModel.DataAnnotations;
using Foolproof;
using CWTDesktopDatabase.Helpers; 

namespace CWTDesktopDatabase.Validation
{
	public class PseudoCityOrOfficeMaintenanceValidation
    {
		[Required(ErrorMessage = "Pseudo City/Office ID Required")]
		[RegularExpression(@"^([a-zA-Z0-9\-]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string PseudoCityOrOfficeId { get; set; }

		[RegularExpression(@"^([0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string AmadeusId { get; set; }

		[Required(ErrorMessage = "Location Contact Name Required")]
		[RegularExpression(@"^([\w\s*-_.()']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string LocationContactName { get; set; }

		[Required(ErrorMessage = "Location Phone Required")]
		[RegularExpression(@"^([0-9]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string LocationPhone { get; set; }

		[RegularExpression(@"^([\w\s*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string InternalSiteName { get; set; }

		[RegularExpression(@"^([\w\s*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string GovernmentContract { get; set; }

		[RegularExpression(@"^([\w\s*-_.()]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string CIDBPIN { get; set; }

		[RegularExpression(@"^([\w\s*-_.()']+)$", ErrorMessage = "Special character entered is not allowed")]
		public string InternalRemarks { get; set; }
	}
}
