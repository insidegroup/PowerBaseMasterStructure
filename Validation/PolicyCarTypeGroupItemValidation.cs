using System.ComponentModel.DataAnnotations;


namespace CWTDesktopDatabase.Validation
{
    public class PolicyCarTypeGroupItemValidation
    {
        [Required(ErrorMessage = "Policy Location Required")]
        public int PolicyLocationId { get; set; }

        [Required(ErrorMessage = "Car Status Required")]
        public int PolicyCarStatusId { get; set; }

        [Required(ErrorMessage = "CarType Category Required")]
        public int CarTypeCategoryId { get; set; }

    }
}
