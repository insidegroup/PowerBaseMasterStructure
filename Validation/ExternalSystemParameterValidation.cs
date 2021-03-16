using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class ExternalSystemParameterValidation
    {
        [Required(ErrorMessage = "Parameter Value Required")]
        public string ExternalSystemParameterValue { get; set; }

        [Required(ErrorMessage = "Parameter Type Required")]
        public string ExternalSystemParameterTypeId { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required.")]
        public string HierarchyItem { get; set; }

        [RequiredIfRegExMatch("HierarchyType", "ClientSubUnitTravelerType", ErrorMessage = "TravelerType Required")]
        public string TravelerTypeGuid { get; set; }

    }
}
