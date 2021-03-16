using System.ComponentModel.DataAnnotations;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyAirMissedSavingsThresholdGroupItemLanguageValidation
    {
        [Required(ErrorMessage = "Missed Savings Advice Required")]
        public string MissedSavingsAdvice { get; set; }

        [Required(ErrorMessage = "Language Required")]
        public string LanguageCode { get; set; }
    }
}
