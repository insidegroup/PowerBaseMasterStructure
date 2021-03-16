using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
	public class OptionalFieldGroupValidation
	{
		[Required(ErrorMessage = "Group Name Required")]
        [RegularExpression(@"^([À-ÿ\s\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
		public string OptionalFieldGroupName { get; set; }

		[Required(ErrorMessage = "Hierarchy Required")]
		public string HierarchyType { get; set; }

		[RequiredIfNot("HierarchyType", "", ErrorMessage = "Hierarchy Required.")]
		public string HierarchyItem { get; set; }

	}
}