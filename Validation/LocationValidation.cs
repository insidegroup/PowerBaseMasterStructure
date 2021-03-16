using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class LocationValidation
    {
        [Required(ErrorMessage = "Country Required")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Country Region Required")]
        public int CountryRegionId { get; set; }

        [Required(ErrorMessage = "Location Required")]
        [RegularExpression(@"^([\w\s-()*]+)$", ErrorMessage = "Special character entered is not allowed")]
        public string LocationName { get; set; }

    }
}
