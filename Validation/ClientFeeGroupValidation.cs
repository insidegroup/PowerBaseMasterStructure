using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class ClientFeeGroupValidation
    {
        [Required(ErrorMessage = "Group Name Required")]
        [RegularExpression(@"^([\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string ClientFeeGroupName { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [RequiredIfNot("HierarchyType", "", ErrorMessage = "Hierarchy Required.")]
        public string HierarchyItem { get; set; }

        [RequiredIfRegExMatch("HierarchyType", "ClientSubUnitTravelerType", ErrorMessage = "TravelerType Required")]
        public string TravelerTypeGuid { get; set; }

    }
}
