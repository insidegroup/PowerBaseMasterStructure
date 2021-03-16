using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Helpers;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyAirCabinGroupItemValidation
    {
        [Required(ErrorMessage = "Airline Cabin Required")]
        public string AirlineCabinCode { get; set; }

        [RegularExpression(@"((^\d*$)|(^$))", ErrorMessage = "Must Be Whole Number or Blank.")]
        [NumericLessThan("FlightDurationAllowedMax", AllowEquality = true, ErrorMessage = "Minimum Duration must be less than Maximum Duration")]
        public int FlightDurationAllowedMin { get; set; }

        [RegularExpression(@"((^\d*$)|(^$))", ErrorMessage = "Must Be Whole Number or Blank.")]
        public int FlightDurationAllowedMax { get; set; }

        [RegularExpression(@"((^\d*$)|(^$))", ErrorMessage = "Must Be Whole Number or Blank.")]
        [NumericLessThan("FlightMileageAllowedMax", AllowEquality = true, ErrorMessage = "Minimum Mileage must be less than Maximum Mileage")]
        public int FlightMileageAllowedMin { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter proper contact details.")]
        public int FlightMileageAllowedMax { get; set; }
    
    }
}
