using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class ClientDefinedRuleGroupValidation
    {
        [Required(ErrorMessage = "Group Name Required")]
        public string ClientDefinedRuleGroupName { get; set; }

		[Required(ErrorMessage = "Category Required")]
		public string Category { get; set; }
		
		[Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [RequiredIfNot("HierarchyType", "", ErrorMessage = "Hierarchy Required")]
        public string HierarchyItem { get; set; }
    }
}
