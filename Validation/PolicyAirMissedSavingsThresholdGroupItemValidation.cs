using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Validation
{
	[Bind(Exclude = "CreationTimestamp")]
	public class PolicyAirMissedSavingsThresholdGroupItemValidation
    {
        [Required(ErrorMessage = "Amount Required")]
        public decimal? MissedThresholdAmount { get; set; }

        [Required(ErrorMessage = "Currency Required")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "Routing Required")]
        public string RoutingCode { get; set; }
    }
}