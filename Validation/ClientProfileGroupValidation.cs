using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class ClientProfileGroupValidation
    {
        [Required(ErrorMessage = "GDS Required")]
        public string GDSCode { get; set; }
		
		[Required(ErrorMessage = "PCC/Office ID Required")]
		[RegularExpression(@"^[0-9a-zA-Z ]+$", ErrorMessage = "Special character entered is not allowed")]
		public string PseudoCityOrOfficeId { get; set; }

		[Required(ErrorMessage = "Back Office Required")]
		public int BackOfficeSytemId { get; set; }

		[Required(ErrorMessage = "Profile Name Required")]
        [RegularExpression(@"^[0-9a-zA-Z ]+$", ErrorMessage = "Special character entered is not allowed")]
		public string ClientProfileGroupName { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [RequiredIfNot("HierarchyType", "", ErrorMessage = "Hierarchy Required.")]
        public string HierarchyItem { get; set; }
    }
}