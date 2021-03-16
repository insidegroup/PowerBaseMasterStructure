using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class PriceTrackingHandlingFeeGroupValidation
    {
        [Required(ErrorMessage = "Group Name Required")]
        [RegularExpression(@"^([\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string PriceTrackingHandlingFeeGroupName { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required")]
        public string HierarchyItem { get; set; }

        [RequiredIfRegExMatch("HierarchyType", "ClientSubUnitTravelerType", ErrorMessage = "TravelerType Required")]
        public string TravelerTypeGuid { get; set; }

    }
}
