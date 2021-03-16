using System.ComponentModel.DataAnnotations;
using Foolproof; 

namespace CWTDesktopDatabase.Validation
{
    public class PolicyGroupValidation
    {
        [Required(ErrorMessage = "Group Name Required")]
        [RegularExpression(@"^([\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string PolicyGroupName { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required.")]
        public string HierarchyItem { get; set; }

        [RequiredIfRegExMatch("HierarchyType", "ClientSubUnitTravelerType", ErrorMessage = "TravelerType Required")]
        public string TravelerTypeGuid { get; set; }
    }
}
