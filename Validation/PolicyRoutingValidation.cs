using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyRoutingValidation
    {

        // [Required(ErrorMessage = "Name Required")]
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Required, or choose Global")]
        public string FromCode { get; set; }

        //[RequiredIfFalse("ToGlobalFlag", ErrorMessage = "Required,or choose Global")]
        //[Required(ErrorMessage = "Required, or choose Global")]
        public string ToCode { get; set; }
       

    }
}
