using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyCityGroupItemValidation
    {
        [Required(ErrorMessage = "City Required")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "Status Required")]
        public int PolicyCityStatusId { get; set; }

    }
}