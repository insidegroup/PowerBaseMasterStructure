using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class TeamValidation
    {
        
        [Required(ErrorMessage = "Team Name Required")]
        [RegularExpression(@"^([\w\s-()*]+)?$", ErrorMessage = "Special character entered is not allowed")]
        public string TeamName { get; set; }

		[Required(ErrorMessage = "Team Email Required")]
		public string TeamEmail { get; set; }

		[Required(ErrorMessage = "Team Phone Number Required")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Special character entered is not allowed")]
		public string TeamPhoneNumber { get; set; }

        [Required(ErrorMessage = "Team Type Required")]
        public string TeamTypeCode { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required.")]
        public string HierarchyItem { get; set; }

        [RequiredIfRegExMatch("HierarchyType", "ClientSubUnitTravelerType", ErrorMessage = "TravelerType Required")]
        public string TravelerTypeGuid { get; set; }
    }
}
