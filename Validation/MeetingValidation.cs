using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class MeetingValidation
    {
		[Required(ErrorMessage = "Meeting Name Required")]
		[RegularExpression(@"^([À-ÿ\s\w-()_.*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string MeetingName { get; set; }

		[RegularExpression(@"^([À-ÿ\s\w-()_.*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string MeetingReferenceNumber { get; set; }
		
		[Required(ErrorMessage = "Meeting Site Required")]
		[RegularExpression(@"^([À-ÿ\s\w-()_.*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string MeetingLocation { get; set; }

		[Required(ErrorMessage = "Meeting City Required")]
		public string CityCode { get; set; }
		
		[Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required.")]
        public string HierarchyItem { get; set; }
    }
}
