using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyMessageGroupItemHotelValidation
    {
        [Required(ErrorMessage = "Group Name Required")]
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string PolicyMessageGroupItemName { get; set; }

        [Required(ErrorMessage = "Policy Location Required")]
        public int? PolicyLocationId { get; set; }

        [Required(ErrorMessage = "Supplier Name Required")]
        public string SupplierName { get; set; }
    }
}