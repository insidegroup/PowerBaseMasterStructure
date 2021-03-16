using System.ComponentModel.DataAnnotations;
using Foolproof;
using System;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PublicHolidayGroupValidation
    {
        [Required(ErrorMessage = "Group Name Required")]
        [RegularExpression(@"^([\w-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string PublicHolidayGroupName { get; set; }

        [Required(ErrorMessage = "EnabledDate Required")]
        public DateTime? EnabledDate { get; set; }

        [Required(ErrorMessage = "Hierarchy Required")]
        public string HierarchyType { get; set; }

        [Required(ErrorMessage = "Hierarchy Item Required.")]
        public string HierarchyItem { get; set; }

        [RequiredIfRegExMatch("HierarchyType", "ClientSubUnitTravelerType", ErrorMessage = "TravelerType Required")]
        public string TravelerTypeGuid { get; set; }

    }
}
