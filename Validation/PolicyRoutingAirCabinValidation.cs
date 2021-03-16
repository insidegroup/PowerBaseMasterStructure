using System.ComponentModel.DataAnnotations;
using Foolproof; 

namespace CWTDesktopDatabase.Validation
{
    public class PolicyRoutingAirCabinValidation
    {
        [RequiredIfNotEmpty("Mandatory", ErrorMessage = "PolicyRouting Name Required")]
        public string Name { get; set; }

        [RequiredIfFalse("FromGlobalFlag", ErrorMessage = "Required, or choose Global")]
        public string FromCode { get; set; }

        [RequiredIfFalse("ToGlobalFlag", ErrorMessage = "Required,or choose Global")]
        public string ToCode { get; set; }

        public bool FromGlobalFlag { get; set; }

        public bool ToGlobalFlag { get; set; }

        public bool RoutingViceVersaFlag { get; set; }
    }
}

       
  