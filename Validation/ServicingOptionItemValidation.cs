using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class ServicingOptionItemValidation
    {
        [Required(ErrorMessage = "Item Required")]
        public string ServicingOptionId { get; set; }

        [Required(ErrorMessage = "Value Required")]
        public string ServicingOptionItemValue { get; set; }

        [Required(ErrorMessage = "Instruction Required")]
        public string ServicingOptionItemInstruction { get; set; }

		[RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numbers are allowed")]
		public string MaximumConnectionTimeMinutes { get; set; }
    }
}
