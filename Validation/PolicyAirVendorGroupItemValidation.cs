using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace CWTDesktopDatabase.Validation
{
    public class PolicyAirVendorGroupItemValidation
    {
		[Required(ErrorMessage = "Status Required.")]
		public int PolicyAirStatusId { get; set; }

		[Required(ErrorMessage = "Supplier Name Required")]
        public string SupplierName { get; set; }

        [RequiredIf("PolicyAirStatusId", "1", ErrorMessage = "Ranking Required")]
        public int AirVendorRanking { get; set; }
    }
}
