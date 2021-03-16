using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class GDSAdditionalEntryValidation
    {
        [Required(ErrorMessage = "Value Required")]
        [RegularExpression(@"^([\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string GDSAdditionalEntryValue { get; set; }

        [Required(ErrorMessage = "GDS Required")]
        public string GDSCode { get; set; }

        [Required(ErrorMessage = "Event Required")]
        public string GDSAdditionalEntryEventId { get; set; }

        [Required(ErrorMessage = "Product Required")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "Sub Product Required")]
        public string SubProductName { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required.")]
        public string HierarchyItem { get; set; }

        [RequiredIfRegExMatch("HierarchyType", "ClientSubUnitTravelerType", ErrorMessage = "TravelerType Required")]
        public string TravelerTypeGuid { get; set; }

    }
}

