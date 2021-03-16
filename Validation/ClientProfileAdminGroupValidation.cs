using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class ClientProfileAdminGroupValidation
    {
        [Required(ErrorMessage = "GDS Required")]
        public string GDSCode { get; set; }
		
		[Required(ErrorMessage = "Back Office Required")]
		public int BackOfficeSytemId { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [RequiredIfNot("HierarchyType", "", ErrorMessage = "Hierarchy Required.")]
        public string HierarchyItem { get; set; }
    }
}